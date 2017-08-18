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
    ///     Реализация интерфейса: 5.5h
    ///     Реализация первичной логики и доработка по интерфейсу: 8h
    ///     Реализация модуля аналитики: 10h
    ///     Реализация модуля администратора: 7h
    ///     Реализация модуля обновления: 1h
    ///     Реализация аналитики по таблицам He: 12h
    ///     Реализация модуля дополнительных возможностей: 2h
    /// 
    ///     
    ///   Необходимо реализовать следущие модули:
    ///   -Реализовать дополнительные возможности:
    ///     -Решить проблему с удалением папки репозитория перед запуском программы (Это нужно для того, чтобы обновление полностью приходило)
    ///     -Добавить приложения для заливки обновления и бэкапа
    ///     -Запуск первой версиии приложения
    ///   
    public partial class FManager : Form
    {
        private string lastUpdate = string.Empty;
        private decimal year;
        private string month;
        private string previusMonth;
        private List<string> spezailParams = new List<string>();
        private bool isFirstStart = true;

        private Dictionary<int, Dictionary<string, string>> response;
        private Dictionary<int, Dictionary<string, string>> responseHePerson;
        private Assistant assistant = new Assistant();
        private DB db = new DB();
        private ParseDBCasualTables pdbCasual;
        private ParseDBHeTables pdbHe;
        private ParseFile pfile = new ParseFile();
        private EventAssistant eventAssis = new EventAssistant();

        private enum Mode
        {
            Casual,
            He
        }

        private enum periodExpenses
        {
            Month,
            Year,
            AVG,
            ALL
        }

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
            //Задает дефолтные параметры статистики
            spezailParams.Add("");
            spezailParams.Add("т");
            spezailParams.Add("н");
            spezailParams.Add("она");
            //По дефолту активирован режим She
            radioButtonModeShe.Checked = true;
            //По дефолту выбрана вспомогательная таблица для вывода информации из прямого запроса
            radioButtonOutputInDop.Checked = true;
            //По дефолту активированы все специальные параметры
            checkBoxT.Checked = true;
            checkBoxN.Checked = true;
            checkBoxWithoutParams.Checked = true;
            checkBoxShe.Checked = true;
            //По дефолту выбрано оба типа затрат
            //По дефолту недоступны простому пользователю функции администратора
            radioButtonModeHe.Enabled = false;
            radioButtonModeHeGifts.Enabled = false;
            radioButtonModeHeBig.Enabled = false;
            panelModuleADM.Enabled = false;
            buttonAccessUpdate.Enabled = false;
            //По дефолту кнопка "Обновить недоступна"
            btnUpdate.Enabled = false;
            isFirstStart = false;

            //Проверка на запуск с правами администратора
            if (eventAssis.IsExists(assistant.locationPashaFile))
            {
                radioButtonModeHe.Enabled = true;
                radioButtonModeHeGifts.Enabled = true;
                radioButtonModeHeBig.Enabled = true;
                panelModuleADM.Enabled = true;
                buttonAccessUpdate.Enabled = true;
                checkBoxPasha.Checked = true;
            }
           
        }

        private void btnChooseData_Click(object sender, EventArgs e)
        {
            SetStats(true);
        }

        private void numericYears_Click(object sender, EventArgs e)
        {
            cbMonths.Focus();
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            assistant.ConvertNewFilesToList();
            pfile.SetDataFromFileToDBCasual(DB.Tables.He, true);
            pfile.SetDataFromFileToDBCasual(DB.Tables.She, true);
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
                textBoxIsUpdate.Text = Environment.NewLine + "На данный момент установлена последняя версия данных.";
            }
            else
            {
                textBoxIsUpdate.ForeColor = Color.Red;
                textBoxIsUpdate.Text = Environment.NewLine + "Необходимо обновить данные.";
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
                checkBoxShe.Enabled = true;
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

            List<string> months = new List<string>();
            if (getMode == Mode.Casual)
                months = pdbCasual.getMonths(Convert.ToInt32(numericYears.Value));
            else if (getMode == Mode.He)
                months = pdbHe.getMonths(numericYears.Value.ToString());

            for (int i = 0; i < months.Count; i++)
                cbMonths.Items.Add(months[i]);
            cbMonths.SelectedItem = months[months.Count - 1];
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
            checkBoxShe.Enabled = false;
            checkBoxPasha.Checked = false;
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
                            result = MessageBox.Show("Таблица " + activeTables[i].ToString() + " уже существует, её пересоздать?",
                                "Несостыковочка", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            exists = true;
                            break;
                        }
                    }
                    if (exists && result == DialogResult.Yes)
                    {
                        db.DeleteTables(activeTables[i]);
                        db.CreateTable(activeTables[i]);
                        textBoxInfoMessage.Text += "Таблица '" + activeTables[i].ToString() + "' создана" + Environment.NewLine;
                    }
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
                    if (activeTables[i] == DB.Tables.He || activeTables[i] == DB.Tables.She)
                        pfile.SetDataFromFileToDBCasual(activeTables[i]);
                    if (activeTables[i] == DB.Tables.HeBig || activeTables[i] == DB.Tables.HeGifts)
                        pfile.SetDataFromFileToDBHeTables(activeTables[i]);
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

            if (response.Count > 0)
            {
                //Подготовка визуализации таблицы для заполнения данными
                eventAssis.SetSettingsDataGrid(response, view);

                //Заполение данными
                db.PrintDataFromCasual(response, view);
            }
            else
                view.Columns.Clear();
        }

        private void panelDopInstrumental_DoubleClick(object sender, EventArgs e)
        {
            textBoxPassword.Focus();
        }

        /// <summary>
        /// Задает статистику на панели аналитики
        /// </summary>
        private void SetStats(bool fromButton = false)
        {
            if (!eventAssis.IsExists("stats.mdf"))
            {
                MessageBox.Show("Отсутствует файл статистики.", "Глобальная ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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

            textBoxList.Clear();

            if (radioButtonModeHe.Checked)
            {
                response = db.GetData(DB.Tables.He);
                responseHePerson = db.GetData(DB.Tables.HeGifts);
            }
            else if (radioButtonModeShe.Checked)
                response = db.GetData(DB.Tables.She);
            else if (radioButtonModeHeBig.Checked)
                response = db.GetData(DB.Tables.HeBig);
            else if (radioButtonModeHeGifts.Checked)
                response = db.GetData(DB.Tables.HeGifts);

            if (getMode == Mode.Casual)
            {
                pdbCasual = new ParseDBCasualTables(response);
                if (radioButtonModeHe.Checked)
                    pdbHe = new ParseDBHeTables(responseHePerson);
            }
            else if (getMode == Mode.He)
                pdbHe = new ParseDBHeTables(response);

            if (!fromButton)
            {
                if (getMode == Mode.Casual)
                {
                    if (pdbCasual.getYears().Count > 0)
                    {
                        numericYears.Minimum = pdbCasual.getYears()[0];
                        numericYears.Maximum = pdbCasual.getYears()[pdbCasual.getYears().Count - 1];
                        numericYears.Value = pdbCasual.getYears()[pdbCasual.getYears().Count - 1];
                    }
                    else
                    {
                        TableClear();
                    }
                }
                else if (getMode == Mode.He)
                {
                    if (pdbHe.getFirstYear != 0 && pdbHe.getLastYear != 0)
                    {
                        numericYears.Minimum = pdbHe.getFirstYear;
                        numericYears.Maximum = pdbHe.getLastYear;
                        numericYears.Value = pdbHe.getLastYear;
                    }
                    else
                    {
                        TableClear();
                    }
                }
            }

            year = numericYears.Value;
            month = cbMonths.SelectedItem.ToString();
            previusMonth = string.Empty;

            Dictionary<int, Dictionary<string, string>> parseResponse = new Dictionary<int, Dictionary<string, string>>();

            if (cbMonths.SelectedIndex >= 1)
                previusMonth = cbMonths.Items[cbMonths.SelectedIndex - 1].ToString();
            else
            {
                List<string> months = new List<string>();
                if (getMode == Mode.Casual)
                    months = pdbCasual.getMonths(Convert.ToInt32(year - 1));
                else if (getMode == Mode.He)
                    months = pdbHe.getMonths(year.ToString());
                if (months.Count != 0)
                    previusMonth = months[months.Count - 1];
            }

            //Объявления словарей для вывода аналитики
            Dictionary<string, string[]> expensesAllTime = new Dictionary<string, string[]>();
            Dictionary<string, string[]> expensesAVG = new Dictionary<string, string[]>();
            Dictionary<string, string[]> expensesYear = new Dictionary<string, string[]>();
            Dictionary<string, string[]> expensesMonth = new Dictionary<string, string[]>();
            Dictionary<string, string[]> expensesPreviusYear = new Dictionary<string, string[]>();
            Dictionary<string, string[]> expensesPriviusMonth = new Dictionary<string, string[]>();

            //Объявление словарей для вывода аналитики Casuals
            //She
            Dictionary<string, string[]> cigarettesAllTime = new Dictionary<string, string[]>();
            Dictionary<string, string[]> cigarettesAVG = new Dictionary<string, string[]>();
            Dictionary<string, string[]> cigarettesYear = new Dictionary<string, string[]>();
            Dictionary<string, string[]> cigarettesMonth = new Dictionary<string, string[]>();
            Dictionary<string, string[]> cigarettesPreviusYear = new Dictionary<string, string[]>();
            Dictionary<string, string[]> cigarettesPreviusMonth = new Dictionary<string, string[]>();

            //He
            Dictionary<string, string[]> candels = new Dictionary<string, string[]>();
            Dictionary<string, string[]> relax = new Dictionary<string, string[]>();

            //Формирование списка текстбоксов для определения необходимости добавления скролла
            TextBox[] expenses = new TextBox[6] { textBoxWindowExpensesAll, textBoxExpensesAVG, textBoxWindowExpensesCurrentYear
                ,textBoxWindowExpensesCurrentMonth,textBoxWindowExpensesPreviusYear,textBoxWindowExpensesPreviusMonth};

            if (getMode == Mode.Casual)
            {
                if (radioButtonModeHe.Checked)
                    eventAssis.EnabledScrolls(expenses);
                else
                    eventAssis.DisableScrolls(expenses);

                expensesAllTime = pdbCasual.getExpensesAllTime(spezailParams);
                expensesAVG = pdbCasual.getAVGExpenses(spezailParams);
                expensesYear = pdbCasual.getExpensesYear(spezailParams, year.ToString());
                expensesMonth = pdbCasual.getExpensesMonth(spezailParams, year.ToString(), month);
                expensesPreviusYear = pdbCasual.getExpensesYear(spezailParams, (year - 1).ToString());
                expensesPriviusMonth = pdbCasual.getExpensesMonth(spezailParams, year.ToString(), previusMonth);

                //She
                if (radioButtonModeShe.Checked)
                {
                    cigarettesAllTime = pdbCasual.getCigarettesAllTime();
                    cigarettesAVG = pdbCasual.getCigarettesAVG();
                    cigarettesYear = pdbCasual.getCigarettesYear(year.ToString());
                    cigarettesMonth = pdbCasual.getCigarettesMonth(year.ToString(), month);
                    cigarettesPreviusYear = pdbCasual.getCigarettesYear((year - 1).ToString());
                    cigarettesPreviusMonth = pdbCasual.getCigarettesMonth(year.ToString(), previusMonth);
                }
                //He
                else if (radioButtonModeHe.Checked)
                {
                    candels = pdbCasual.getCandles();
                    relax = pdbCasual.getRelax();
                }
            }
            else if (getMode == Mode.He)
            {
                eventAssis.EnabledScrolls(expenses);

                expensesAllTime = pdbHe.All();
                expensesAVG = pdbHe.AVG();
                expensesYear = pdbHe.Year(year.ToString());
                expensesMonth = pdbHe.Month(year.ToString(), month);
                expensesPreviusYear = pdbHe.Year((year - 1).ToString());
                expensesPriviusMonth = pdbHe.Month(year.ToString(), previusMonth);
            }

            //Затраты
            //Без панели
            if (radioButtonModeHe.Checked) textBoxWindowExpensesAll.Text = eventAssis.TemplateOutputDataString(expensesAllTime, false, pdbHe.All());
            else textBoxWindowExpensesAll.Text = eventAssis.TemplateOutputDataString(expensesAllTime);
            if (radioButtonModeHe.Checked) textBoxExpensesAVG.Text = eventAssis.TemplateOutputDataString(expensesAVG, false, pdbHe.AVG());
            else textBoxExpensesAVG.Text = eventAssis.TemplateOutputDataString(expensesAVG);

            //За текущий год
            if (radioButtonModeHe.Checked) textBoxWindowExpensesCurrentYear.Text = eventAssis.TemplateOutputDataString(expensesYear, false, pdbHe.Year(year.ToString()));
            else textBoxWindowExpensesCurrentYear.Text = eventAssis.TemplateOutputDataString(expensesYear);
            if (radioButtonModeHe.Checked) textBoxWindowExpensesCurrentMonth.Text = eventAssis.TemplateOutputDataString(expensesMonth, false, pdbHe.Month(year.ToString(), month));
            else textBoxWindowExpensesCurrentMonth.Text = eventAssis.TemplateOutputDataString(expensesMonth);

            //За прошедший год
            if (radioButtonModeHe.Checked) textBoxWindowExpensesPreviusYear.Text = eventAssis.TemplateOutputDataString(expensesPreviusYear, false, pdbHe.Year((year - 1).ToString()));
            else textBoxWindowExpensesPreviusYear.Text = eventAssis.TemplateOutputDataString(expensesPreviusYear);
            if (radioButtonModeHe.Checked) textBoxWindowExpensesPreviusMonth.Text = eventAssis.TemplateOutputDataString(expensesPriviusMonth, false, pdbHe.Month(year.ToString(), previusMonth));
            else textBoxWindowExpensesPreviusMonth.Text = eventAssis.TemplateOutputDataString(expensesPriviusMonth);

            //Сигареты
            //Без панели
            if (radioButtonModeShe.Checked)
            {
                textBoxCigarettesAll.Text = eventAssis.TemplateOutputDataString(cigarettesAllTime);
                textBoxCigarettesAVG.Text = eventAssis.TemplateOutputDataString(cigarettesAVG);
                //За текущий год
                textBoxCigarettesCurrentYearYear.Text = eventAssis.TemplateOutputDataString(cigarettesYear);
                textBoxCigarettesCurrentYearMonth.Text = eventAssis.TemplateOutputDataString(cigarettesMonth);
                //За предыдущий год
                textBoxCigarettesPreviusYearYear.Text = eventAssis.TemplateOutputDataString(cigarettesPreviusYear);
                textBoxCigarettesPreviusYearMonth.Text = eventAssis.TemplateOutputDataString(cigarettesPreviusMonth);
                //За текущий год
                textBoxResultYear.Text = eventAssis.TemplateOutputDataString(expensesYear, true);
                textBoxResultMonth.Text = eventAssis.TemplateOutputDataString(expensesMonth, true);
                //За предыдущий год
                textBoxResultPreviusYear.Text = eventAssis.TemplateOutputDataString(expensesPreviusYear, true);
                textBoxResultPreviusMonth.Text = eventAssis.TemplateOutputDataString(expensesPriviusMonth, true);
            }
            if (radioButtonModeHe.Checked)
            {
                //Свечи
                textBoxCandles.Text = eventAssis.TemplateOutputDataString(candels);

                //Спокойствие
                textBoxRelax.Text = eventAssis.TemplateOutputDataString(relax);
            }

            //Подготовливает список затрат только для выбранного месяца для вывода в DataGrid
            for (int i = 0, j = 0; i < response.Count; i++)
                if (response[i]["date_expense"].IndexOf(ParseDBAssistant.getIntMonthFromString(month) + "/" + year.ToString()) != -1)
                {
                    parseResponse[j] = response[i];
                    j++;
                }

            if (parseResponse.Count > 0)
            {
                //Подготовка визуализации таблицы для заполнения данными
                eventAssis.SetSettingsDataGrid(parseResponse, dataOsn);
                //Заполение данными
                db.PrintDataFromCasual(parseResponse, dataOsn);
            }
            else
                dataOsn.Columns.Clear();
        }

        private void radioButtonModeHe_CheckedChanged(object sender, EventArgs e)
        {
            if (!isFirstStart)
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

        }

        private void textBoxWindowExpensesAll_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxList.Clear();
            if (getMode == Mode.He)
                eventAssis.SetDescExpensesInTextBox(textBoxList, getDictFromMode(periodExpenses.ALL));
            else
                textBoxList.Text = "Не вижу смысла детализировать информацию за все время в столь маленьком окошке." +
                    " Для таких целей был создан отдельный контрол.";
        }

        private void textBoxExpensesAVG_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxList.Clear();
            textBoxList.Text = "Не вижу смысла детализировать среднестатистическую информацию";
        }

        private void textBoxWindowExpensesCurrentYear_MouseClick(object sender, MouseEventArgs e)
        {
            eventAssis.SetDescExpensesInTextBox(textBoxList, getDictFromMode(periodExpenses.Year, spezailParams, year.ToString()), false,
                getDictFromMode(periodExpenses.Year, spezailParams, year.ToString(),"", true));
        }

        private void textBoxWindowExpensesCurrentMonth_MouseClick(object sender, MouseEventArgs e)
        {
            eventAssis.SetDescExpensesInTextBox(textBoxList, getDictFromMode(periodExpenses.Month, spezailParams, year.ToString(), month), false,
                getDictFromMode(periodExpenses.Month, spezailParams, year.ToString(), month, true));
        }

        private void textBoxWindowExpensesPreviusYear_MouseClick(object sender, MouseEventArgs e)
        {
            eventAssis.SetDescExpensesInTextBox(textBoxList, getDictFromMode(periodExpenses.Year, spezailParams, (year - 1).ToString()));
        }

        private void textBoxWindowExpensesPreviusMonth_MouseClick(object sender, MouseEventArgs e)
        {
            eventAssis.SetDescExpensesInTextBox(textBoxList, getDictFromMode(periodExpenses.Month, spezailParams, year.ToString(), previusMonth));
        }

        private void textBoxCigarettesAll_MouseClick(object sender, MouseEventArgs e)
        {
            eventAssis.SetDescExpensesInTextBox(textBoxList, pdbCasual.getCigarettesAllTime());
        }

        private void textBoxCigarettesAVG_MouseClick(object sender, MouseEventArgs e)
        {
            eventAssis.SetDescExpensesInTextBox(textBoxList, pdbCasual.getCigarettesAVG());
        }

        private void textBoxCigarettesCurrentYearYear_MouseClick(object sender, MouseEventArgs e)
        {
            eventAssis.SetDescExpensesInTextBox(textBoxList, pdbCasual.getCigarettesYear(year.ToString()));
        }

        private void textBoxCigarettesCurrentYearMonth_MouseClick(object sender, MouseEventArgs e)
        {
            eventAssis.SetDescExpensesInTextBox(textBoxList, pdbCasual.getCigarettesMonth(year.ToString(), month));
        }

        private void textBoxCigarettesPreviusYearYear_MouseClick(object sender, MouseEventArgs e)
        {
            eventAssis.SetDescExpensesInTextBox(textBoxList, pdbCasual.getCigarettesYear((year - 1).ToString()));
        }

        private void textBoxCigarettesPreviusYearMonth_MouseClick(object sender, MouseEventArgs e)
        {
            eventAssis.SetDescExpensesInTextBox(textBoxList, pdbCasual.getCigarettesMonth(year.ToString(), previusMonth));
        }

        private void textBoxCandles_MouseClick(object sender, MouseEventArgs e)
        {
            eventAssis.SetDescExpensesInTextBox(textBoxList, pdbCasual.getCandles());
        }

        private void textBoxRelax_MouseClick(object sender, MouseEventArgs e)
        {
            eventAssis.SetDescExpensesInTextBox(textBoxList, pdbCasual.getRelax());
        }

        private void textBoxResultYear_MouseClick(object sender, MouseEventArgs e)
        {
            eventAssis.SetDescExpensesInTextBox(textBoxList, pdbCasual.getExpensesYear(spezailParams, year.ToString()), true);
        }

        private void textBoxResultMonth_MouseClick(object sender, MouseEventArgs e)
        {
            eventAssis.SetDescExpensesInTextBox(textBoxList, pdbCasual.getExpensesMonth(spezailParams, year.ToString(), month), true);
        }

        private void textBoxResultPreviusYear_MouseClick(object sender, MouseEventArgs e)
        {
            eventAssis.SetDescExpensesInTextBox(textBoxList, pdbCasual.getExpensesYear(spezailParams, (year - 1).ToString()), true);
        }

        private void textBoxResultPreviusMonth_MouseClick(object sender, MouseEventArgs e)
        {
            eventAssis.SetDescExpensesInTextBox(textBoxList, pdbCasual.getExpensesMonth(spezailParams, year.ToString(), previusMonth), true);
        }

        private void FManager_Shown(object sender, EventArgs e)
        {
            if (CheckUpdate())
                if (eventAssis.IsExists("gitExists.sys"))
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

        private void checkBoxT_CheckedChanged(object sender, EventArgs e)
        {
            if (!isFirstStart)
            {
                if (checkBoxT.Checked)
                {
                    eventAssis.setSpezialParam(spezailParams, "т", EventAssistant.CmdForSpezialParam.Add);
                }
                else
                {
                    eventAssis.setSpezialParam(spezailParams, "т", EventAssistant.CmdForSpezialParam.Delete);
                }
                SetStats();
            }
        }

        private void checkBoxN_CheckedChanged(object sender, EventArgs e)
        {
            if (!isFirstStart)
            {
                if (checkBoxN.Checked)
                {
                    eventAssis.setSpezialParam(spezailParams, "н", EventAssistant.CmdForSpezialParam.Add);
                }
                else
                {
                    eventAssis.setSpezialParam(spezailParams, "н", EventAssistant.CmdForSpezialParam.Delete);
                }
                SetStats();
            }
        }

        private void checkBoxWithoutParams_CheckedChanged(object sender, EventArgs e)
        {
            if (!isFirstStart)
            {
                if (checkBoxWithoutParams.Checked)
                {
                    eventAssis.setSpezialParam(spezailParams, "", EventAssistant.CmdForSpezialParam.Add);
                }
                else
                {
                    eventAssis.setSpezialParam(spezailParams, "", EventAssistant.CmdForSpezialParam.Delete);
                }
                SetStats();
            }
        }

        private void checkBoxShe_CheckedChanged(object sender, EventArgs e)
        {
            if (!isFirstStart)
            {
                if (checkBoxShe.Checked)
                {
                    eventAssis.setSpezialParam(spezailParams, "она", EventAssistant.CmdForSpezialParam.Add);
                }
                else
                {
                    eventAssis.setSpezialParam(spezailParams, "она", EventAssistant.CmdForSpezialParam.Delete);
                }
                SetStats();
            }
        }

        private void checkBoxPasha_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPasha.Checked)
            {
                if (!eventAssis.IsExists("admin.sys"))
                    assistant.CreateFile(assistant.locationPashaFile);
                textBoxInfoMessage.Text += "Разрешен дефолтный запуск приложения с правами администратора." + Environment.NewLine;
            }
            else
            {
                if (eventAssis.IsExists("admin.sys"))
                    File.Delete(assistant.locationPashaFile);
                radioButtonModeHe.Enabled = false;
                radioButtonModeHeGifts.Enabled = false;
                radioButtonModeHeBig.Enabled = false;
                panelModuleADM.Enabled = false;
                checkBoxShe.Enabled = false;
                textBoxInfoMessage.Text += "Запрещен дефолтный запуск приложения с правами администратора." + Environment.NewLine;
            }

        }

        private void radioButtonModeHeGifts_CheckedChanged(object sender, EventArgs e)
        {
            SetStats();
        }

        private void radioButtonModeHeBig_CheckedChanged(object sender, EventArgs e)
        {
            SetStats();
        }


        private void buttonExecuteDopInstrumental_Click(object sender, EventArgs e)
        {
            if (comboBoxDopInstrumental.SelectedItem.ToString().StartsWith("Экспортировать"))
            {
                textBoxInfoMessage.Text += "Началась экспортировка таблицы" + Environment.NewLine;

                if (comboBoxDopInstrumental.SelectedItem.ToString() == "Экспортировать She")
                    pfile.SetDataFromDBToFile(db.GetData(DB.Tables.She), DB.Tables.She);

                if (comboBoxDopInstrumental.SelectedItem.ToString() == "Экспортировать He")
                    pfile.SetDataFromDBToFile(db.GetData(DB.Tables.He), DB.Tables.He);

                if (comboBoxDopInstrumental.SelectedItem.ToString() == "Экспортировать HeGifts")
                    pfile.SetDataFromDBToFile(db.GetData(DB.Tables.HeGifts), DB.Tables.HeGifts);

                if (comboBoxDopInstrumental.SelectedItem.ToString() == "Экспортировать HeBig")
                    pfile.SetDataFromDBToFile(db.GetData(DB.Tables.HeBig), DB.Tables.HeBig);


                textBoxInfoMessage.Text += "Экспортировка таблицы завершена." + Environment.NewLine;
            }
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

        /// <summary>
        /// Возвращает выбранный мод
        /// </summary>
        private Mode getMode
        {
            get
            {
                if (radioButtonModeHe.Checked || radioButtonModeShe.Checked)
                    return Mode.Casual;
                else if (radioButtonModeHeBig.Checked || radioButtonModeHeGifts.Checked)
                    return Mode.He;
                return new Mode();
            }
        }

        /// <summary>
        /// Выполняет очистку таблицы
        /// </summary>
        private void TableClear()
        {
            panelChooseData.Enabled = false;
            textBoxWindowExpensesAll.Text += Environment.NewLine + EventAssistant.Errors.emptyTable + Environment.NewLine;
        }

        private Dictionary<string, string[]> getDictFromMode(periodExpenses period, List<string> spezialParams = null, string year = "", string month = "", bool hePerson = false)
        {
            if (getMode == Mode.Casual && !hePerson)
            {
                if (period == periodExpenses.Month)
                    return pdbCasual.getExpensesMonth(spezialParams, year, month);
                else if (period == periodExpenses.Year)
                    return pdbCasual.getExpensesYear(spezialParams, year);
                else if (period == periodExpenses.ALL)
                    return pdbCasual.getExpensesAllTime(spezialParams);
                else if (period == periodExpenses.AVG)
                    return pdbCasual.getCigarettesAVG();
            }
            else if (getMode == Mode.He || hePerson)
            {
                if (period == periodExpenses.Month)
                    return pdbHe.Month(year, month);
                else if (period == periodExpenses.Year)
                    return pdbHe.Year(year);
                else if (period == periodExpenses.ALL)
                    return pdbHe.All();
                else if (period == periodExpenses.AVG)
                    return pdbHe.AVG();
            }
            else throw new Exception("Не подходящий мод для даннойго функционала.");
            return null;
        }
    }

    public class EventAssistant
    {
        private Assistant assistant = new Assistant();

        public EventAssistant()
        {

        }

        public enum CmdForSpezialParam
        {
            Add,
            Delete
        }

        /// <summary>
        /// Проверяет валидность пароля
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Проверяет наличие выбранного файла в рабочей директории
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Проверяет наличие файлов для выключения панелей исходя из переданного списка контролов
        /// </summary>
        /// <param name="controls"></param>
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

        /// <summary>
        /// Делает проверку гита
        /// </summary>
        /// <returns></returns>
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
                if (keys[i] == "id" || keys[i] == "count" || keys[i] == "param")
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

        /// <summary>
        /// Не возволяет нижним контролам выйти за край формы
        /// </summary>
        /// <param name="form"></param>
        /// <param name="control"></param>
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

        /// <summary>
        /// Шаблон для вывода количества затрат в панель аналитики
        /// </summary>
        /// <param name="expenses"></param>
        /// <param name="personExpenses"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public String TemplateOutputDataString(Dictionary<string, string[]> dict = null, bool shePerson = false, Dictionary<string, string[]> hePerson = null)
        {
            string result = "";
            bool countExists = false;
            bool hePersonKeys = false;
            DB.Tables table = DB.Tables.AbstractCASUAL;

            //Определение анализируемой таблицы
            foreach (KeyValuePair<string, string[]> keyValue in dict)
            {
                if (keyValue.Key == "countHolidays")
                {
                    table = DB.Tables.AbstractHE;
                    break;
                }
                if (keyValue.Key == "expenses")
                {
                    hePersonKeys = true;
                    break;
                }
                if (keyValue.Key == "count")
                {
                    countExists = true;
                    break;
                }
            }

            //Обработка данных повседневной таблицы
            if (table == DB.Tables.AbstractCASUAL)
            {
                if (shePerson)
                {
                    result += "--: " + dict["--"][0] + " руб." + Environment.NewLine;
                    result += "+ : " + dict["+"][0] + " руб." + Environment.NewLine;
                    int summ = Convert.ToInt32(dict["+"][0]) - Convert.ToInt32(dict["--"][0]);
                    if (summ >= 0)
                        result += "Итого: за выбранную дату ты в плюсе на " + summ + " руб.";
                    else
                        result += "Итого: за выбранную дату ты в минусе на " + summ + " руб.";
                }
                else if (hePersonKeys)
                {
                    result += "Затраты : " + dict["expenses"][0] + " руб." + Environment.NewLine;
                    result += "Количество : " + dict["count"][0] + " шт." + Environment.NewLine;
                }
                else
                {
                    result += "- : " + dict["-"][0] + " руб." + Environment.NewLine;
                    if (dict["--"][0] != "0")
                        result += "-- : " + dict["--"][0] + " руб." + Environment.NewLine;
                    if (countExists)
                        result += "Количество : " + dict["count"][0] + Environment.NewLine;
                    if (!(hePerson is null))
                    {
                        result += "Gifts: " + hePerson["--"][0] + " руб." + Environment.NewLine;
                        result += "Res: " + (Convert.ToInt32(dict["-"][0]) + Convert.ToInt32(hePerson["--"][0])).ToString() + " руб." + Environment.NewLine;
                    }
                }
            }
            //Обработка данных He таблицы
            else if (table == DB.Tables.AbstractHE)
            {
                result += "- : " + dict["-"][0] + " руб." + Environment.NewLine;
                if (dict["--"][0] == "0")
                {
                    result += "events: " + dict["countHolidays"][0] + Environment.NewLine;
                }
                else
                {
                    result += "-- : " + dict["--"][0] + "руб." + Environment.NewLine;
                    result += "holidays: " + dict["countHolidays"][0] + Environment.NewLine;
                    result += "gifts: " + dict["countGifts"][0] + Environment.NewLine;
                }
            }
            string parsedResult = result;
            if(result.EndsWith("\n") || result.EndsWith("\r"))
            {
                for(int i=result.Length-1;i>0;i--)
                {
                    if (result[i] == '\n' || result[i] == '\r')
                       parsedResult = result.Remove(i);
                    else break;
                }
            }
            return parsedResult;
        }

        public void PrintDateLastUpdate(TextBox textBoxUpdate, string lastUpdate)
        {
            textBoxUpdate.Text += "Дата последнего обновления: " + lastUpdate;
        }

        /// <summary>
        /// Добавляет, либо удаляет из списка выбранный специальный параметр
        /// </summary>
        /// <param name="speziaParam"></param>
        /// <param name="param"></param>
        /// <param name="cmd"></param>
        public void setSpezialParam(List<string> speziaParam, string param, CmdForSpezialParam cmd)
        {
            bool exists = false;
            for (int i = 0; i < speziaParam.Count; i++)
            {
                if (cmd == CmdForSpezialParam.Add)
                {
                    if (speziaParam[i] == param)
                    {
                        exists = true;
                        break;
                    }
                }
                if (cmd == CmdForSpezialParam.Delete)
                {
                    if (speziaParam[i] == param)
                    {
                        speziaParam.Remove(param);
                        return;
                    }
                }

            }
            if (!exists)
                speziaParam.Add(param);
        }
        /// <summary>
        /// Вывод список затрат в выбранный текстбокс
        /// </summary>
        /// <param name="textbox"></param>
        /// <param name="desc"></param>
        public void SetDescExpensesInTextBox(TextBox textbox, Dictionary<string, string[]> desc, bool shePerson = false, Dictionary<string, string[]> hePerson = null)
        {
            textbox.Clear();
            DB.Tables table = DB.Tables.AbstractHE;

            foreach (var keyValues in desc)
            {
                if (keyValues.Key == "descExpenses")
                {
                    table = DB.Tables.AbstractCASUAL;
                    break;
                }
            }

            if (table == DB.Tables.AbstractCASUAL)
            {
                if (shePerson)
                {
                    textbox.Text = "--------------------Прибыль" + Environment.NewLine;
                    for (int i = 0; i < desc["descProfit"].Length; i++)
                        textbox.Text += desc["descProfit"][i] + Environment.NewLine;

                    textbox.Text += "---------------------Убыль" + Environment.NewLine;
                    for (int i = 0; i < desc["descPersonExpenses"].Length; i++)
                        textbox.Text += desc["descPersonExpenses"][i] + Environment.NewLine;
                }
                else
                {
                    textbox.Text = "----------------------------Обычные затраты" + Environment.NewLine;

                    for (int i = 0; i < desc["descExpenses"].Length; i++)
                        textbox.Text += desc["descExpenses"][i] + Environment.NewLine;
                    if(!(hePerson is null))
                    {
                        textbox.Text += "------------------------Gifts" + Environment.NewLine;
                        for (int i = 0; i < hePerson["aboutSkyline"].Length; i++)
                            textbox.Text += hePerson["aboutSkyline"][i] + Environment.NewLine;
                    }
                    else if (desc["descPersonExpenses"].Length > 0)
                    {
                        textbox.Text += "--------------------Затраты из своих денег" + Environment.NewLine;
                        for (int i = 0; i < desc["descPersonExpenses"].Length; i++)
                            textbox.Text += desc["descPersonExpenses"][i] + Environment.NewLine;
                    }
                }
            }
            else if (table == DB.Tables.AbstractHE)
            {
                if (desc["--"][0] == "0")
                {
                    for (int i = 0; i < desc["aboutGifts"].Length; i++)
                        textbox.Text += desc["aboutGifts"][i] + Environment.NewLine + Environment.NewLine;
                }
                else
                {
                    textbox.Text = "------------------Праздники" + Environment.NewLine;
                    for (int i = 0; i < desc["aboutHolidays"].Length; i++)
                        textbox.Text += desc["aboutHolidays"][i] + Environment.NewLine + Environment.NewLine;

                    textbox.Text += "------------------Подарки" + Environment.NewLine;
                    for (int i = 0; i < desc["aboutGifts"].Length; i++)
                        textbox.Text += desc["aboutGifts"][i] + Environment.NewLine + Environment.NewLine;
                }
            }
        }

        /// <summary>
        /// Добавляет скроллы у выбранных текстбоксов
        /// </summary>
        /// <param name="controls"></param>
        public void EnabledScrolls(TextBox[] controls)
        {
            for (int i = 0; i < controls.Length; i++)
                controls[i].ScrollBars = ScrollBars.Both;
        }

        /// <summary>
        /// Убирает скроллы у выбранных текст боксов
        /// </summary>
        /// <param name="controls"></param>
        public void DisableScrolls(TextBox[] controls)
        {
            for (int i = 0; i < controls.Length; i++)
                controls[i].ScrollBars = ScrollBars.None;
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