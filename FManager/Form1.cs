using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace FManager
{
    /// Формирование программы 16h
    /// Доработка над модулями:
    ///     Работа над обновление LatestHe - 7h
    ///     Обработка с добавлением возможности сохранения ошибки при запуске драйвера - 1.5h
    ///     Работа над переносом обновления под Git - 7h
    ///  Разработка version 2.0: 
    ///     Работа над модулем считывания и записи данных из файла в БД: 12h
    ///     Проектирование интерфейса и часть первичной логики: 1h
    ///     Реализация интерфейса: 5h
    ///     Реализация первичной логики и доработка по интерфейсу: 8h
    ///     Реализация модуля аналитики: 6h
    ///     Реализация модуля администратора: 6.5h
    ///     Реализация модуля обновления: 1h
    ///     
    ///   Необходимо реализовать следущие модули:
    ///   -По клику на ячейку с статистикой - в текстовом поле отображать данный список
    ///   -Добавить настройку вывода данных в зависимости от выбранных специальных параметров
    ///   -Реализовать дополнительные возможности:
    ///     -Экспорт из БД данных в файл по ГОСТу первой версии приложения
    ///     -Запуск первой версиии приложения
    ///   
    public partial class FManager : Form
    {
        private string lastUpdate = string.Empty;
        private decimal year;
        private string month;
        private string previusMonth;

        private Dictionary<int, Dictionary<string, string>> response;
        private Assistant assistant = new Assistant();
        private DB db = new DB();
        private ParseDB pdb;
        private ParseFile pfile = new ParseFile();
        private EventAssistant eventAssis = new EventAssistant();
        public FManager()
        {
            InitializeComponent();
        }

        private void FManager_Load(object sender, EventArgs e)
        {
            //Задает доступность панелям в зависимости от существующий файлов
            List<Control> modules = new List<Control>();
            modules.Add(panelAnalitics);
            modules.Add(panelSpecial);
            modules.Add(panelMode);
            modules.Add(panelDopInstrumental);
            modules.Add(panelModuleUpdate);
            modules.Add(panelModuleADM);
            modules.Add(dataDop);
            modules.Add(dataOsn);
            eventAssis.CheckFiles(modules);
            if (!eventAssis.CheckGit())
            {
                textBoxInfoMessage.Text += "На данном компьютере скорее всего отсутствует инструмент обновления данных." +
                    "В данном случае система может ошибиться, но если вы сами не уверены, " +
                    "то лучше запустите установку инструмента." + Environment.NewLine;
            }

            //Задает внешние значения параметров панелей
            this.Height = Screen.PrimaryScreen.Bounds.Height - 100;
            eventAssis.SetPositionBottomControl(this, dataOsn);
            eventAssis.SetPositionBottomControl(this, textBoxList);
            eventAssis.SetPositionBottomControl(this, dataDop);
            //По дефолту активированы все специальные параметры
            checkBoxT.Checked = true;
            checkBoxN.Checked = true;
            checkBoxWithoutParams.Checked = true;
            checkBoxExpenses.Checked = true;
            checkBoxShe.Checked = true;
            //По дефолту активирован режим She
            radioButtonModeShe.Checked = true;
            //По дефолту выбрана вспомогательная таблица для вывода информации из прямого запроса
            radioButtonOutputInDop.Checked = true;
            //По дефолту недоступны простому пользователю функции администратора
            radioButtonModeHe.Enabled = false;
            radioButtonModeHeGifts.Enabled = false;
            radioButtonModeHeBig.Enabled = false;
            panelModuleADM.Enabled = false;
            buttonAccessUpdate.Enabled = false;
            //По дефолту кнопка "Обновить недоступна"
            btnUpdate.Enabled = false;
            //Заполнение первоначальных элементов статистики
            if (eventAssis.IsExists("stats.mdf"))
                SetStats();
        }

        private void btnChooseData_Click(object sender, EventArgs e)
        {
            SetStats();
        }

        private void numericYears_Click(object sender, EventArgs e)
        {
            cbMonths.Focus();
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            assistant.ConvertNewFilesToList();
            pfile.SetDataFromFileToDB(DB.Tables.He, true);
            pfile.SetDataFromFileToDB(DB.Tables.She, true);
            using (StreamWriter sr = new StreamWriter(assistant.locationUpdateDate, false))
                sr.WriteLine(lastUpdate);
            assistant.DeleteUpdateFiles(textBoxUpdate);
            textBoxIsUpdate.Text = string.Empty;
            eventAssis.PrintDateLastUpdate(textBoxUpdate, lastUpdate);
        }

        private void btnLookUpdate_Click(object sender, EventArgs e)
        {
            textBoxIsUpdate.Text = string.Empty;
            textBoxUpdate.Text += Environment.NewLine;

            string allTextFromCMD = string.Empty;
            string dateCurrentUpdate = string.Empty;
            string locationNewFile = string.Empty;

            ProcessStartInfo infoTest = new ProcessStartInfo("cmd.exe", "/c "
                + @"cd \ & cd " + assistant.locationWorkFolder
                + " & "
                + @"git clone --no-checkout https://github.com/PashaQA/Updater_FManager.git & cd Updater_FManager"
                + " & "
                + "git reset --hard HEAD"
                + " & "
                + "git log");
            infoTest.CreateNoWindow = true;
            infoTest.UseShellExecute = false;
            infoTest.WindowStyle = ProcessWindowStyle.Hidden;
            infoTest.RedirectStandardInput = true;
            infoTest.RedirectStandardOutput = true;

            textBoxUpdate.Text += "Началась закачка файла последней информации." + Environment.NewLine;
            try
            {
                Process processTest = Process.Start(infoTest);
                processTest.EnableRaisingEvents = true;
                using (StreamReader readerOutput = processTest.StandardOutput)
                {
                    processTest.WaitForExit();
                    allTextFromCMD = readerOutput.ReadToEnd();
                }
            }
            catch (Exception)
            {

            }
            textBoxUpdate.Text += "Началось выявления даты последней закачки." + Environment.NewLine;

            //Выявление даты последнего обновления
            int index = allTextFromCMD.IndexOf("Date");
            if (index == -1)
            {
                try
                {
                    if (File.Exists(assistant.locationWorkFolder + "\\gitExists.sys"))
                        File.Delete(assistant.locationWorkFolder + "\\gitExists.sys");
                }
                catch (Exception)
                {

                }
                DialogResult result = MessageBox.Show("При скачивании обновления возникла не предвиденная ошибка," +
                    " мы предполагаем что это связано с нарушением работы модуля обновления." +
                    " Вам следует выполнить установку модуля обновления",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            lastUpdate = allTextFromCMD.Remove(0, index);
            index = lastUpdate.IndexOf("Date");
            lastUpdate = lastUpdate.Remove(index + 27);

            using (StreamReader readerUpdateFile = new StreamReader(assistant.locationUpdateDate))
                dateCurrentUpdate = readerUpdateFile.ReadToEnd();

            if (dateCurrentUpdate.StartsWith(lastUpdate))
            {
                textBoxIsUpdate.ForeColor = Color.Black;
                textBoxIsUpdate.Text = Environment.NewLine+ "На данный момент установлена последняя версия данных.";
            }
            else
            {
                textBoxIsUpdate.ForeColor = Color.Red;
                textBoxIsUpdate.Text = Environment.NewLine+ "Необходимо обновить данные.";
            }
        }

        private void radioButtonCreateBat_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCreateBat.Checked)
                assistant.CreateBat("Debug");
            textBoxUpdate.Text += "Batch file created.";
        }

        private void FManager_FormClosed(object sender, FormClosedEventArgs e)
        {
            File.Delete(assistant.locationWorkFolder + "\\Debug.bat");
            assistant.DeleteUpdateFiles(textBoxUpdate);
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            if (eventAssis.ValidPassword(textBoxPassword.Text))
            {
                radioButtonModeHe.Enabled = true;
                radioButtonModeHeGifts.Enabled = true;
                radioButtonModeHeBig.Enabled = true;
                panelModuleADM.Enabled = true;
                buttonAccessUpdate.Enabled = true;
                textBoxPassword.Clear();
            }
        }

        private void btnCheckGit_Click(object sender, EventArgs e)
        {
            radioButtonModeHe.Enabled = false;
            radioButtonModeHeGifts.Enabled = false;
            radioButtonModeHeBig.Enabled = false;
            panelModuleADM.Enabled = false;
        }

        private void btnInstallGit_Click(object sender, EventArgs e)
        {
           textBoxUpdate.Text = "Началась распаковка файла..." + Environment.NewLine;
            byte[] git = Properties.Resources.Git;
            File.WriteAllBytes(assistant.locationGitFile, git);
            textBoxUpdate.Text += "Файл распакован, начало установки.." + Environment.NewLine;
            Process process = Process.Start(assistant.locationGitFile);
            process.EnableRaisingEvents = true;
            process.WaitForExit();
            if (Directory.Exists(@"C:\Program Files (x86)\Git") || Directory.Exists(@"C:\Program Files\Git"))
            {
                textBoxUpdate.Text = "Установка завершена.";
                assistant.CreateFile(assistant.locationWorkFolder + "\\gitExists.sys");
            }
            else
            {
                textBoxUpdate.Text = "Либо установка была отменена," +
                    Environment.NewLine +
                    "либо сообщи о случившемся разработчику";
            }
            try
            {
                File.Delete(assistant.locationGitFile);
            }
            catch (Exception)
            {

            }
        }


        private void numericYears_ValueChanged(object sender, EventArgs e)
        {
            cbMonths.Items.Clear();
            List<string> months = pdb.getMonths(Convert.ToInt32(numericYears.Value));
            for (int i = 0; i < months.Count; i++)
                cbMonths.Items.Add(months[i]);
            cbMonths.SelectedItem = months[0];
        }

        private void buttonInfoInSpesialParams_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Параметер (т) - затраты на тусах, либо для тусы\n" +
                "Параметер (н) - необходимое затраты, которых просто не избежать\n" +
                "Параметр (без параметров) - выводит информацию о затратах, в которых не указан ни один параметр\n" +
                "Категория (--) - сегодня мама дала 100р, завтра это твои деньги. Если ты что-то купила за деньги " +
                "которые тебе дали вчера, которые ты накопила за вчера, будет считаться затратой на твои деньги.",
                "Описание парметров", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonBlock_Click(object sender, EventArgs e)
        {
            radioButtonModeHe.Enabled = false;
            radioButtonModeHeGifts.Enabled = false;
            radioButtonModeHeBig.Enabled = false;
            panelModuleADM.Enabled = false;
        }

        private void radioButtonModeShe_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonModeShe.Checked)
            {
                checkBoxShe.Enabled = false;

                textBoxCandles.Visible = false;
                dataCandles.Visible = false;
                labelHeaderWindowCandles.Visible = false;

                textBoxRelax.Visible = false;
                dataRelax.Visible = false;
                labelHeaderRelax.Visible = false;

                SetStats();
            }

        }

        private void buttonShowFieldPassword_Click(object sender, EventArgs e)
        {
            int y = panelChooseData.Location.Y;
            int x = panelChooseData.Location.X;
            panelChooseData.Location = new Point(x, y + 30);
            System.Threading.Thread.Sleep(1000);
            y = panelChooseData.Location.Y;
            x = panelChooseData.Location.X;
            panelChooseData.Location = new Point(x, y - 30);

        }

        private void buttonExecuteCMDADM_Click(object sender, EventArgs e)
        {
            //Проверка на то, что хотя бы одна таблица выбрана
            if (checkBoxHeADM.Checked == false &&
                    checkBoxHeBigADM.Checked == false &&
                    checkBoxHeGiftsADM.Checked == false &&
                    checkBoxSheADM.Checked == false)
            {
                MessageBox.Show("Необходимо выбрать таблицу.", "Маленькая неясность",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            List<DB.Tables> existsTables = new List<DB.Tables>();
            existsTables = db.GetExistsTables();

            List<DB.Tables> activeTables = new List<DB.Tables>();
            if (checkBoxHeADM.Checked)
                activeTables.Add(DB.Tables.He);
            if (checkBoxHeGiftsADM.Checked)
                activeTables.Add(DB.Tables.HeGifts);
            if (checkBoxHeBigADM.Checked)
                activeTables.Add(DB.Tables.HeBig);
            if (checkBoxSheADM.Checked)
                activeTables.Add(DB.Tables.She);

            for (int i = 0; i < activeTables.Count; i++)
            {
                bool exists = false;
                DialogResult result = DialogResult.Yes;

                if (comboBoxCMDsADM.SelectedItem.ToString() == "Создать таблицу")
                {
                    for (int j = 0; j < existsTables.Count; j++)
                    {
                        if (activeTables[i] == existsTables[j])
                        {
                            result = MessageBox.Show("Таблица " + activeTables[i].ToString() + "уже существует, её пересоздать?",
                                "Несостыковочка", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            exists = true;
                            break;
                        }
                    }
                    if (exists && result == DialogResult.Yes)
                        db.DeleteTables(activeTables[i]);
                    else if (result == DialogResult.Yes)
                    {
                        db.CreateTable(activeTables[i]);
                        textBoxInfoMessage.Text += "Таблица '" + activeTables[i].ToString() + "' создана" + Environment.NewLine;

                    }

                }
                if (comboBoxCMDsADM.SelectedItem.ToString() == "Удалить таблицу")
                {
                    db.DeleteTables(activeTables[i]);
                    textBoxInfoMessage.Text += "Таблица '" + activeTables[i].ToString() + "' удалена" + Environment.NewLine;

                }
                if (comboBoxCMDsADM.SelectedItem.ToString() == "Очистить таблицу")
                {
                    db.ClearTable(activeTables[i]);
                    textBoxInfoMessage.Text += "Таблица '" + activeTables[i].ToString() + "' очищена" + Environment.NewLine;
                }
                if (comboBoxCMDsADM.SelectedItem.ToString() == "Записать в таблицу с файла")
                {
                    textBoxInfoMessage.Text += "Началась запись данных с файла.." + Environment.NewLine;
                    for (int j = 0; j < existsTables.Count; j++)
                    {
                        if (activeTables[i] == existsTables[j])
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                    {
                        result = MessageBox.Show("Таблица '" + activeTables[i].ToString() + "' не существует, её создать?", "Несостыковка",
                             MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                            db.CreateTable(activeTables[i]);
                        else
                            continue;
                    }
                    pfile.SetDataFromFileToDB(activeTables[i]);
                    textBoxInfoMessage.Text += "Данные успешно записаны в таблицу " + activeTables[i].ToString() + Environment.NewLine;
                    panelChooseData.Enabled = true;
                }
            }
        }

        private void buttonExecuteSQLQuery_Click(object sender, EventArgs e)
        {
            response = db.ExecuteQuery(textBoxSQLQuery.Text);

            //Выбор таблицы для вывода информации
            DataGridView view = null;
            if (radioButtonOutputInOsn.Checked)
                view = dataOsn;
            if (radioButtonOutputInDop.Checked)
                view = dataDop;

            //Подготовка визуализации таблицы для заполнения данными
            eventAssis.SetSettingsDataGrid(response, view);

            //Заполение данными
            db.PrintDataFromCasual(response, view);

        }

        private void panelDopInstrumental_DoubleClick(object sender, EventArgs e)
        {
            textBoxPassword.Focus();
        }

        /// <summary>
        /// Задает статистику на панели аналитики
        /// </summary>
        private void SetStats()
        {
            textBoxWindowExpensesAll.Clear();
            textBoxExpensesAVG.Clear();
            textBoxWindowExpensesCurrentYear.Clear();
            textBoxWindowExpensesCurrentMonth.Clear();
            textBoxWindowExpensesPreviusYear.Clear();
            textBoxWindowExpensesPreviusMonth.Clear();
            textBoxCigarettesAll.Clear();
            textBoxCigarettesAVG.Clear();
            textBoxCigarettesCurrentYearYear.Clear();
            textBoxCigarettesCurrentYearMonth.Clear();
            textBoxCigarettesPreviusYearYear.Clear();
            textBoxCigarettesPreviusYearMonth.Clear();
            textBoxCandles.Clear();
            textBoxRelax.Clear();

            if (radioButtonModeHe.Checked)
                response = db.GetData(DB.Tables.He);
            if (radioButtonModeShe.Checked)
                response = db.GetData(DB.Tables.She);

            pdb = new ParseDB(response);

            if (pdb.getYears().Count > 0)
            {
                numericYears.Minimum = pdb.getYears()[0];
                numericYears.Maximum = pdb.getYears()[pdb.getYears().Count - 1];
            }
            else
            {
                panelChooseData.Enabled = false;
                textBoxWindowExpensesAll.Text += Environment.NewLine + EventAssistant.Errors.emptyTable + Environment.NewLine;
            }

            year = numericYears.Value;
            month = cbMonths.SelectedItem.ToString();
            previusMonth = string.Empty;
            Dictionary<int, Dictionary<string, string>> parseResponse = new Dictionary<int, Dictionary<string, string>>();

            if (cbMonths.SelectedIndex >= 1)
                previusMonth = cbMonths.Items[cbMonths.SelectedIndex - 1].ToString();
            else
            {
                List<string> months = pdb.getMonths(Convert.ToInt32(year - 1));
                if (months.Count != 0)
                    previusMonth = months[months.Count - 1];
            }

            //Затраты
            //Без панели
            textBoxWindowExpensesAll.Text = eventAssis.TemplateOutputDataString(pdb.getExpensesAllTime()["-"][0],
                pdb.getExpensesAllTime()["--"][0], pdb.getExpensesAllTime()["count"][0]);
            textBoxExpensesAVG.Text = eventAssis.TemplateOutputDataString(pdb.getAVGExpenses()["-"][0], pdb.getAVGExpenses()["--"][0]);
            //За текущий год
            textBoxWindowExpensesCurrentYear.Text = eventAssis.TemplateOutputDataString(pdb.getExpensesYear(year.ToString())["-"][0],
                pdb.getExpensesYear(year.ToString())["--"][0]);
            textBoxWindowExpensesCurrentMonth.Text = eventAssis.TemplateOutputDataString(pdb.getExpensesMonth(year.ToString(), month)["-"][0],
                pdb.getExpensesMonth(year.ToString(), month)["--"][0]);
            //За прошедший год
            textBoxWindowExpensesPreviusYear.Text = eventAssis.TemplateOutputDataString(pdb.getExpensesYear((year - 1).ToString())["-"][0],
                pdb.getExpensesYear((year - 1).ToString())["--"][0]);
            textBoxWindowExpensesPreviusMonth.Text = eventAssis.TemplateOutputDataString(pdb.getExpensesMonth(year.ToString(), previusMonth)["-"][0],
                pdb.getExpensesMonth(year.ToString(), previusMonth)["--"][0]);

            //Сигареты
            //Без панели
            textBoxCigarettesAll.Text = eventAssis.TemplateOutputDataString(pdb.getCigarettesAllTime()["-"][0], pdb.getCigarettesAllTime()["--"][0],
                pdb.getCigarettesAllTime()["count"][0]);
            textBoxCigarettesAVG.Text = eventAssis.TemplateOutputDataString(pdb.getCigarettesAVG()["-"][0], pdb.getCigarettesAVG()["--"][0],
                pdb.getCigarettesAVG()["count"][0]);
            //За текущий год
            textBoxCigarettesCurrentYearYear.Text = eventAssis.TemplateOutputDataString(pdb.getCigarettesYear(year.ToString())["-"][0],
                pdb.getCigarettesYear(year.ToString())["--"][0], pdb.getCigarettesYear(year.ToString())["count"][0]);
            textBoxCigarettesCurrentYearMonth.Text = eventAssis.TemplateOutputDataString(pdb.getCigarettesMonth(year.ToString(), month)["-"][0],
                pdb.getCigarettesMonth(year.ToString(), month)["--"][0], pdb.getCigarettesMonth(year.ToString(), month)["count"][0]);
            //За предыдущий год
            textBoxCigarettesPreviusYearYear.Text = eventAssis.TemplateOutputDataString(pdb.getCigarettesYear((year - 1).ToString())["-"][0],
                pdb.getCigarettesYear((year - 1).ToString())["--"][0], pdb.getCigarettesYear((year - 1).ToString())["count"][0]);
            textBoxCigarettesPreviusYearMonth.Text = eventAssis.TemplateOutputDataString(pdb.getCigarettesMonth(year.ToString(), previusMonth)["-"][0],
                pdb.getCigarettesMonth(year.ToString(), previusMonth)["--"][0], pdb.getCigarettesMonth(year.ToString(), previusMonth)["count"][0]);

            //Свечи
            textBoxCandles.Text = eventAssis.TemplateOutputDataString(pdb.getCandles()["expenses"][0], "", pdb.getCandles()["count"][0]);

            //Спокойствие
            textBoxRelax.Text = eventAssis.TemplateOutputDataString(pdb.getRelax()["expenses"][0], pdb.getRelax()["count"][0]);

            for (int i = 0, j = 0; i < response.Count; i++)
                if (response[i]["date_expense"].IndexOf(pdb.getIntMonthFromString(month) + "/" + year.ToString()) != -1)
                {
                    parseResponse[j] = response[i];
                    j++;
                }


            //Подготовка визуализации таблицы для заполнения данными
            eventAssis.SetSettingsDataGrid(parseResponse, dataOsn);
            //Заполение данными
            db.PrintDataFromCasual(parseResponse, dataOsn);
        }

        /// <summary>
        ///  Проверяет наличие обновления и если оно есть, подготавливает данные для обновления
        /// </summary>
        private Boolean CheckUpdate()
        {
            textBoxIsUpdate.Text = string.Empty;
            textBoxUpdate.Text += Environment.NewLine;

            string allTextFromCMD = string.Empty;
            string dateCurrentUpdate = string.Empty;
            string locationNewFile = string.Empty;

            ProcessStartInfo infoTest = new ProcessStartInfo("cmd.exe", "/c "
                + @"cd \ & cd " + assistant.locationWorkFolder
                + " & "
                + @"git clone --no-checkout https://github.com/PashaQA/Updater_FManager.git & cd Updater_FManager"
                + " & "
                + "git reset --hard HEAD"
                + " & "
                + "git log");
            infoTest.CreateNoWindow = true;
            infoTest.UseShellExecute = false;
            infoTest.WindowStyle = ProcessWindowStyle.Hidden;
            infoTest.RedirectStandardInput = true;
            infoTest.RedirectStandardOutput = true;

            textBoxUpdate.Text += "Началась закачка файла последней информации." + Environment.NewLine;
            try
            {
                Process processTest = Process.Start(infoTest);
                processTest.EnableRaisingEvents = true;
                using (StreamReader readerOutput = processTest.StandardOutput)
                {
                    processTest.WaitForExit();
                    allTextFromCMD = readerOutput.ReadToEnd();
                }
            }
            catch (Exception)
            {

            }
            textBoxUpdate.Text += "Началось выявления даты последней закачки." + Environment.NewLine;

            //Выявление даты последнего обновления
            int index = allTextFromCMD.IndexOf("Date");
            if (index == -1)
            {
                try
                {
                    if (File.Exists(assistant.locationWorkFolder + "\\gitExists.sys"))
                        File.Delete(assistant.locationWorkFolder + "\\gitExists.sys");
                }
                catch (Exception)
                {

                }
                DialogResult result = MessageBox.Show("При скачивании обновления возникла не предвиденная ошибка," +
                    " мы предполагаем что это связано с нарушением работы модуля обновления." +
                    " Вам следует выполнить установку модуля обновления",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            lastUpdate = allTextFromCMD.Remove(0, index);
            index = lastUpdate.IndexOf("Date");
            lastUpdate = lastUpdate.Remove(index + 27);

            using (StreamReader readerUpdateFile = new StreamReader(assistant.locationUpdateDate))
                dateCurrentUpdate = readerUpdateFile.ReadToEnd();

            if (dateCurrentUpdate.StartsWith(lastUpdate))
            {
                textBoxIsUpdate.ForeColor = Color.Black;
                textBoxIsUpdate.Text = Environment.NewLine + "На данный момент установлена последняя версия данных.";
                return false;
            }
            else
            {
                textBoxIsUpdate.ForeColor = Color.Red;
                textBoxIsUpdate.Text = Environment.NewLine + "Необходимо обновить данные.";
                return true;
            }
        }
        private void radioButtonModeHe_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonModeHe.Checked)
            {
                checkBoxShe.Enabled = true;

                textBoxCandles.Visible = true;
                dataCandles.Visible = true;
                labelHeaderWindowCandles.Visible = true;

                textBoxRelax.Visible = true;
                dataRelax.Visible = true;
                labelHeaderRelax.Visible = true;

                SetStats();
            }

        }

        private void textBoxExpensesAVG_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxList.Text = "----------------------------" + Environment.NewLine;

            for (int i = 0; i < pdb.getAVGExpenses()["descExpenses"].Length; i++)
                textBoxList.Text += pdb.getAVGExpenses()["descExpenses"][i] + Environment.NewLine;

            textBoxList.Text += "----------------------------" + Environment.NewLine;

            for (int i = 0; i < pdb.getAVGExpenses()["descPersonExpenses"].Length; i++)
                textBoxList.Text += pdb.getAVGExpenses()["descPersonExpenses"][i] + Environment.NewLine;
        }

        private void textBoxWindowExpensesCurrentYear_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxList.Text = "----------------------------" + Environment.NewLine;

            for (int i = 0; i < pdb.getExpensesYear(year.ToString())["descExpenses"].Length; i++)
                textBoxList.Text += pdb.getExpensesYear(year.ToString())["descExpenses"][i] + Environment.NewLine;

            textBoxList.Text += "----------------------------" + Environment.NewLine;

            for (int i = 0; i < pdb.getExpensesYear(year.ToString())["descPersonExpenses"].Length; i++)
                textBoxList.Text += pdb.getExpensesYear(year.ToString())["descPersonExpenses"][i] + Environment.NewLine;
        }

        private void textBoxWindowExpensesCurrentMonth_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void textBoxWindowExpensesPreviusYear_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void textBoxWindowExpensesPreviusMonth_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void textBoxCigarettesAll_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void textBoxCigarettesAVG_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void textBoxCigarettesCurrentYearYear_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void textBoxCigarettesCurrentYearMonth_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void textBoxCigarettesPreviusYearYear_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void textBoxCigarettesPreviusYearMonth_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void textBoxCandles_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void textBoxRelax_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void FManager_Shown(object sender, EventArgs e)
        {
            if (CheckUpdate())
                if(eventAssis.IsExists("gitExists.sys"))
                    btnUpdate.Enabled = true;
                else
                    textBoxUpdate.Text += "Система не может обнаружить инструмент обновления, если вы считаете что он установлен, нажниме на 'Разрешить обновление'.";
            eventAssis.PrintDateLastUpdate(textBoxUpdate, lastUpdate);
        }

        private void buttonAccessUpdate_Click(object sender, EventArgs e)
        {
            assistant.CreateFile(assistant.locationWorkFolder + "\\gitExists.sys");
            btnUpdate.Enabled = true;
        }
    }

    public class EventAssistant
    {
        private Assistant assistant = new Assistant();

        public EventAssistant()
        {

        }

        public Boolean ValidPassword(string password)
        {
            int counter = 0;
            List<char> symbols = new List<char>();
            for (int i = 0; i < password.Length; i++)
                symbols.Add(password[i]);
            List<int> indexer = new List<int>();
            for (int i = 0; i < symbols.Count; i++)
            {
                bool exist = false;
                if (symbols[i] == '8')
                {
                    for (int j = 0; j < indexer.Count; j++)
                    {
                        if (i == indexer[j])
                            exist = true;
                    }
                    if (!exist)
                        counter++;
                    indexer.Add(i);
                }
                if (counter == 2)
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean IsExists(string name)
        {
            string[] existsFiles = Directory.GetFiles(assistant.locationWorkFolder);
            for (int i = 0; i < existsFiles.Length; i++)
            {
                if (existsFiles[i].EndsWith(name))
                    return true;
            }
            return false;
        }

        public void CheckFiles(List<Control> controls)
        {
            for (int i = 0; i < controls.Count; i++)
            {
                if (!IsExists("stats.mdf"))
                {
                    if (controls[i].Name == "panelAnalitics" || controls[i].Name == "panelSpecial"
                        || controls[i].Name == "panelModuleUpdate" || controls[i].Name == "panelModuleADM"
                        || controls[i].Name == "dataOsn" || controls[i].Name == "panelDop")
                        controls[i].Enabled = false;
                }
                else if (!CheckGit())
                {
                    if (controls[i].Name == "panelUpdate")
                        controls[i].Enabled = false;
                }
                else if (!IsExists("updateDate.sys"))
                    assistant.CreateFile(assistant.locationUpdateDate);
            }
        }

        public Boolean CheckGit()
        {
            if (Directory.Exists(@"C:\Program Files (x86)\Git") || Directory.Exists(@"C:\Program Files\Git"))
            {
                return true;
            }
            else
            {

                if (File.Exists(assistant.locationWorkFolder + "\\gitExists.sys"))
                    File.Delete(assistant.locationWorkFolder + "\\gitExists.sys");
                return false;
            }
        }

        /// <summary>
        ///  Задает ширину дата грида, добавленное нужное количество строк.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="view"></param>
        public void SetSettingsDataGrid(Dictionary<int, Dictionary<string, string>> response, DataGridView view)
        {
            view.Columns.Clear();
            List<string> keys = new List<string>();
            foreach (var key in response[0].Keys)
                keys.Add(key);
            for (int i = 0; i < response[0].Count; i++)
            {
                view.Columns.Add(keys[i], keys[i]);
                if (keys[i] == "id" || keys[i] == "count")
                    view.Columns[i].Width = 50;
                if (keys[i] == "full_line")
                    view.Columns[i].Width += 100;
                if (keys[i] == "event_type" || keys[i] == "type")
                    view.Columns[i].Width = 35;
                if (keys[i] == "date_expense")
                    view.Columns[i].Width = view.Columns[i].Width - 25;
            }
            int offset = 0;
            if (keys.Count == 8) offset = 75;
            view.Size = new Size(view.Columns.GetColumnsWidth(DataGridViewElementStates.Displayed)
                + (keys.Count * 18) - offset, view.Size.Height);
            for (int i = 0; i < response.Count - 1; i++)
                view.Rows.Add();
        }

        public void SetPositionBottomControl(Form form, Control control)
        {
            Rectangle recControl = control.Bounds;
            Rectangle recForm = form.Bounds;
            if (recControl.Y + recControl.Height + recForm.Y > recForm.Height)
            {
                while (true)
                {
                    int height = control.Height;
                    if (recControl.Y + height + recForm.Y + 5 == recForm.Height)
                        break;
                    control.Size = new Size(control.Width, control.Height - 1);
                }
            }
        }

        public String TemplateOutputDataString(string expenses = "", string personExpenses = "", string count = "")
        {
            string result = "";
            if (expenses != string.Empty)
                result += "- : " + expenses + " руб." + Environment.NewLine;
            if (personExpenses != "0")
                result += "-- : " + personExpenses + " руб." + Environment.NewLine;
            if (count != string.Empty)
                result += "Количество : " + count;
            return result;
        }

        public void PrintDateLastUpdate(TextBox textBoxUpdate, string lastUpdate)
        {
            textBoxUpdate.Text += "Дата последнего обновления: " + lastUpdate;
        }

        /// <summary>
        /// Данный класс возвращает текст ошибок
        /// </summary>
        public static class Errors
        {
            /// <summary>
            /// При чтении данных отсутствует таблица, либо она пуста.
            /// </summary>
            public static String emptyTable
            {
                get
                {
                    return "Либо таблица не создана, либо они пуста.При запуске проверка идет по таблице 'She' ";
                }
            }
        }
    }
}