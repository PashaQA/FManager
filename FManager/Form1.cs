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
    /// <summary>
    /// 7.Общее положение за год:
    /// -Отдельное  поле для вывода. Выводит все месяца, напротив пишет результат -
    /// в плюсе за месяц или нет, выводит сумму, выделяет цветом.
    /// </summary>
    /// Формирование программы 16h
    /// Доработка над модулями:
    ///     Работа над обновление LatestHe - 7h
    ///     Обработка с добавлением возможности сохранения ошибки при запуске драйвера - 1.5h
    ///     Работа над переносом обновления под Git - 7h
    public partial class FManager : Form
    {
        string lastUpdate = string.Empty;
        private GetData.PrintData print = null;
        private SetData set = new SetData();
        private DB db = new DB();

        public FManager()
        {
            InitializeComponent();
            AllInvisible();
            labelLastUpdate.Visible = true;
            radioButtonShe.Checked = true;

            numericYears.Minimum = 2016;
            numericYears.Maximum = 2017;
        }

        private void FManager_Load(object sender, EventArgs e)
        {
            this.Height = Screen.PrimaryScreen.Bounds.Height - 100;
            cbMonths.SelectedIndex = 0;
            numericYears.Value = 2017;
            CheckFileExists();
        }

        private void btnChooseData_Click(object sender, EventArgs e)
        {
            string locationCurrentFile = set.locationCurrentFile;
            if (radioButtonHe.Checked)
            {
                locationCurrentFile = set.locationHeFile;
            }
            bool haventMonth = true;
            if (cbMonths.SelectedIndex == -1)
            {
                MessageBox.Show("Необходимо выбрать месяц для отображение статистики.");
                cbMonths.Focus();
            }
            else
            {
                print = new GetData.PrintData(set.GetParsedList(locationCurrentFile),
                    cbMonths.SelectedItem.ToString(), numericYears.Value.ToString(), radioButtonHe);
                if (print.IsHierogliphCurrentFile)
                {
                    MessageBox.Show("Файл содержит иероглифы, что означает что он не был обработан в ручную." +
                        " в таком случае я его удалю, а вы," +
                        " если не профукали файлы обновления, просто выполнить обновление файла."
                        + " Если таковых файлов не имеется, сообщите разработчику.." +
                        " Если после обновления такая же ошибка, то тут уже накосячил разработчик," +
                        " сообщите ему об этом.");
                    File.Delete(set.locationCurrentFile);
                    panelChooseData.Visible = false;
                    return;
                }
                for (int i = 0; i < print.current_months.Count; i++)
                {
                    if (print.current_months[i].StartsWith(cbMonths.SelectedItem.ToString()) && print.current_months[i].EndsWith(numericYears.Value.ToString()))
                    {
                        infoParamT.Visible = true;
                        infoParamN.Visible = true;
                        if (radioButtonShe.Checked)
                        {
                            infoCategoryPerson1.Visible = true;
                            infoCategoryPerson2.Visible = true;
                            tabControlButtons.Visible = true;
                        }
                        if (radioButtonHe.Checked)
                        {
                            tabControlButtonsHe.Visible = true;
                        }
                        textBoxList.Clear();
                        textBoxSum.Clear();
                        textBoxAboutExpenses.Clear();
                        haventMonth = false;
                        labelUpdateSucceful.Visible = false;
                        break;
                    }
                }
                if (haventMonth)
                {
                    MessageBox.Show("Такого месяца еще нет в статистики, выберите другой.", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cbMonths.SelectedIndex = 0;
                    cbMonths.Focus();
                }
            }
        }

        private void numericYears_Click(object sender, EventArgs e)
        {
            if (numericYears.Value == 2016)
            {
                cbMonths.SelectedIndex = 5;
            }
            else
            {
                cbMonths.SelectedIndex = 0;
            }
            cbMonths.Focus();
        }

        private void btnChooseDaysList_Click(object sender, EventArgs e)
        {
            textBoxList.Visible = true;
            print.DaysFromMonth(textBoxList);
        }

        private void btnExpensesSum_Click(object sender, EventArgs e)
        {
            textBoxSum.Visible = true;
            print.Expenses(textBoxSum);
        }

        private void btnProfitSum_Click(object sender, EventArgs e)
        {
            textBoxSum.Visible = true;
            print.ProfitSum(textBoxSum);
        }

        private void btnExpensesTSum_Click(object sender, EventArgs e)
        {
            textBoxSum.Visible = true;
            print.ExpensesPartySum(textBoxSum);
        }

        private void btnExpensesNSum_Click(object sender, EventArgs e)
        {
            textBoxSum.Visible = true;
            print.ExpensesMustSum(textBoxSum);
        }

        private void btnProfitList_Click(object sender, EventArgs e)
        {
            textBoxList.Visible = true;
            print.ProfitList(textBoxList);
        }

        private void btnExpensesTList_Click(object sender, EventArgs e)
        {
            textBoxList.Visible = true;
            print.ExpensesPartyList(textBoxList);
        }

        private void btnExpensesNList_Click(object sender, EventArgs e)
        {
            textBoxList.Visible = true;
            print.ExpensesMustList(textBoxList);
        }

        private void btnExpensesPersonList_Click(object sender, EventArgs e)
        {
            textBoxList.Visible = true;
            print.ExpensesPersonList(textBoxList);
        }

        private void btnExpensesPersonSum_Click(object sender, EventArgs e)
        {
            textBoxSum.Visible = true;
            print.ExpensesPersonSum(textBoxSum);
        }

        private void btnProfitAfterExpenses_Click(object sender, EventArgs e)
        {
            textBoxSum.Visible = true;
            print.ProfitAfterExpenses(textBoxSum);
        }

        private void btnAboutExpenses_Click(object sender, EventArgs e)
        {
            textBoxAboutExpenses.Visible = true;
            print.AboutExpenses(textBoxAboutExpenses);
        }

        private void btnAboutCigarettes_Click(object sender, EventArgs e)
        {
            textBoxSum.Visible = true;
            print.ExpensesCigarettesSum(textBoxSum);
            print.ExpensesCigarettesList(textBoxList);
            print.CigarettesCount(textBoxAboutExpenses);
        }

        private void btnCigarettesCountAllTimes_Click(object sender, EventArgs e)
        {
            textBoxAboutExpenses.Visible = true;
            print.CigarettesCountAllTimes(textBoxAboutExpenses);
        }

        private void btnExpensesCigarettesAllTimes_Click(object sender, EventArgs e)
        {
            textBoxSum.Visible = true;
            print.ExpensesCigarettesSumAllTimes(textBoxSum);
        }

        private void btnExpensesCigarettesListAllTimes_Click(object sender, EventArgs e)
        {
            textBoxList.Visible = true;
            print.ExpensesCigarettesListAllTimes(textBoxList);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string locationNewFile = set.locationNewFile;
            string locationCurrentFile = set.locationCurrentFile;
            string locationUpdateFile = set.locationUpdateDate;
            if (radioButtonHe.Checked)
            {
                locationNewFile = set.locationNewFileHe;
                locationCurrentFile = set.locationHeFile;
                locationUpdateFile = set.locationUpdateDateHe;
            }
            set.ConvertNewFileToList();
            string[] currentFile = set.GetParsedList(locationCurrentFile);
            string[] newFile = set.GetParsedList(locationNewFile);
            List<string> updateFile = new List<string>();
            for (int i = 0; i < currentFile.Length; i++)
                updateFile.Add(currentFile[i]);
            for (int i = 0; i < newFile.Length; i++)
            {
                updateFile.Add(newFile[i]);
            }
            using (StreamWriter sw = new StreamWriter(locationCurrentFile, false, Encoding.Default))
            {
                for (int i = 0; i < updateFile.Count; i++)
                    sw.WriteLine(updateFile[i]);
                labelUpdateSucceful.Visible = true;
            }
            try
            {
                File.Delete(set.locationNewFile);
                File.Delete(set.locationNewFileHe);
            }
            catch (Exception)
            {

            }
            using (StreamWriter sr = new StreamWriter(locationUpdateFile, false))
                sr.WriteLine(lastUpdate);
            labelAnswerToUpdate.Text = string.Empty;
            btnUpdate.Visible = false;
            CheckFileExists();
            PrintDateLastUpdate();
        }

        private void btnLookUpdate_Click(object sender, EventArgs e)
        {
            string allTextFromCMD = string.Empty;
            string dateCurrentUpdate = string.Empty;
            string locationUpdateFile = string.Empty;
            string locationCurrentFile = string.Empty;
            string locationNewFile = string.Empty;
            int counter = 0;

            ProcessStartInfo infoTest = new ProcessStartInfo("cmd.exe", "/c "
                + @"cd \ & cd " + set.locationWorkFolder
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

            labelAnswerToUpdate.Text = "Началась закачка файла последней информации.";
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
            labelAnswerToUpdate.Text += Environment.NewLine + "Началось выявления даты последней закачки.";

            //Выявление даты последнего обновления
            int index = allTextFromCMD.IndexOf("Date");
            if (index == -1)
            {
                try
                {
                    if (File.Exists(set.locationWorkFolder + "\\gitExists.sys"))
                        File.Delete(set.locationWorkFolder + "\\gitExists.sys");
                }
                catch (Exception)
                {

                }
                DialogResult result = MessageBox.Show("При скачивании обновления возникла не предвиденная ошибка," +
                    " мы предполагаем что это связано с нарушением работы модуля обновления." +
                    " Вам следует выполнить установку модуля обновления",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    panelWorkGit.Visible = true;
                    btnInstallGit.Visible = true;
                    ActiveForm.Width = 1111;
                    labelAnswerToUpdate.Text = "Состояние ошибки.";
                    return;
                }
            }
            lastUpdate = allTextFromCMD.Remove(0, index);
            index = lastUpdate.IndexOf("Date");
            lastUpdate = lastUpdate.Remove(index + 27);


            if (radioButtonShe.Checked)
            {
                locationUpdateFile = set.locationUpdateDate;
                locationCurrentFile = set.locationCurrentFile;
                locationNewFile = set.locationNewFile;
            }
            else
            {
                locationUpdateFile = set.locationUpdateDateHe;
                locationCurrentFile = set.locationHeFile;
                locationNewFile = set.locationNewFileHe;
            }
            set.CreateFile(locationUpdateFile);
            using (StreamReader readerUpdateFile = new StreamReader(locationUpdateFile))
            {
                dateCurrentUpdate = readerUpdateFile.ReadToEnd();
            }
            if (lastUpdate != dateCurrentUpdate)
            {
                string[] currentFile = set.GetParsedList(locationCurrentFile);
                string[] newFile = set.GetParsedList(locationNewFile);
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (currentFile[currentFile.Length - (i + 1)] == newFile[newFile.Length - (j + 1)])
                            counter++;
                    }
                }
                if (counter < 3)
                {
                    btnUpdate.Visible = true;
                    labelAnswerToUpdate.Text = "Необходимо обновить данные.";
                }
                else
                {
                    labelAnswerToUpdate.Text = "На данный момент установлена последняя версия данных.";
                }

            }
            else
            {
                labelAnswerToUpdate.Text = "На данный момент установлена последняя версия данных.";
            }
        }

        private void CheckFileExists(bool he = false)
        {
            List<string> filesBuild = new List<string>();
            List<string> filesNotFound = new List<string>();
            filesBuild.Add("Latest.sys");
            filesBuild.Add("LatestHe.sys");


            string[] files = Directory.GetFiles(set.locationWorkFolder);

            for (int i = 0; i < filesBuild.Count; i++)
            {
                for (int j = 0; j < files.Length; j++)
                {
                    int idx = files[j].LastIndexOf('\\');
                    files[j] = files[j].Remove(0, idx + 1);
                    if (filesBuild[i] == files[j])
                        break;
                    else if (j == files.Length - 1)
                        filesNotFound.Add(filesBuild[i]);
                }
            }
            if (filesNotFound.Count < 2)
            {
                radioButtonHe.Visible = true;
                radioButtonShe.Visible = true;
                panelChooseData.Visible = true;
                panelUpdate.Visible = false;
                panelEnableGit.Visible = true;
            }
            foreach (var file in filesNotFound)
            {
                if (file == filesBuild[0])
                {
                    if (filesNotFound.Count < 2)
                    {
                        radioButtonHe.Checked = true;
                        radioButtonShe.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("В данном случае у вас отсутствуют оба файла статистики," +
                            " а это собственно говорят значит, что теперь вам нечего смотреть," +
                            " программа прекращает свою работу. В данном случае необходимо сообщить пэхумбру.",
                              "Ужасно", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Close();
                    }
                }
                if (file == filesBuild[1])
                {
                    radioButtonHe.Visible = false;
                }
            }
            if (File.Exists(set.locationWorkFolder + "\\gitExists.sys"))
                panelUpdate.Visible = true;
        }

        private void radioButtonCreateBat_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCreateBat.Checked)
                set.CreateBat("Debug");
            labelBatCreated.Visible = true;
        }

        private void FManager_FormClosed(object sender, FormClosedEventArgs e)
        {
            File.Delete(set.locationWorkFolder + "\\Debug.bat");
        }

        private void PrintDateLastUpdate()
        {
            labelLastUpdate.Text = "";
            string location = string.Empty;
            if (radioButtonHe.Checked) location = set.locationUpdateDateHe;
            else location = set.locationUpdateDate;
            if (!File.Exists(location))
            {
                labelLastUpdate.Text = "На данный момент у вас отсутствует файл обновления.";
                return;
            }
            using (StreamReader sr = new StreamReader(location))
            {
                string data = sr.ReadToEnd();
                if (data != "" || data != " ")
                    labelLastUpdate.Text = data;
            }
        }

        private void AllInvisible()
        {
            infoParamT.Visible = false;
            infoParamN.Visible = false;
            infoCategoryPerson1.Visible = false;
            infoCategoryPerson2.Visible = false;
            tabControlButtons.Visible = false;
            tabControlButtonsHe.Visible = false;
            textBoxList.Visible = false;
            textBoxSum.Visible = false;
            textBoxAboutExpenses.Visible = false;
            panelChooseData.Visible = false;
            btnUpdate.Visible = false;
            labelUpdateSucceful.Visible = false;
            labelBatCreated.Visible = false;
            panelChooseData.Visible = false;
            panelUpdate.Visible = false;
            panelEnableGit.Visible = false;
            panelWorkGit.Visible = false;
            labelVisible.Visible = false;
            textBoxPassword.Visible = false;
            btnRelax.Visible = false;
            btnCandle.Visible = false;
            btnInstallGit.Visible = false;
            btnClosePanelGit.Visible = false;
        }

        private void radioButtonShe_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonShe.Checked)
            {
                CheckFileExists();
                PrintDateLastUpdate();
                tabControlButtonsHe.Visible = false;
            }
            else
            {
                AllInvisible();
            }
            textBoxList.Visible = false;
            textBoxSum.Visible = false;
            textBoxAboutExpenses.Visible = false;
        }

        private void radioButtonhe_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonHe.Checked)
            {
                PrintDateLastUpdate();
                labelVisible.Visible = true;
                textBoxPassword.Visible = true;
                textBoxPassword.Focus();
                panelChooseData.Visible = false;
                panelUpdate.Visible = false;
            }
            else
            {
                labelVisible.Visible = false;
                textBoxPassword.Visible = false;
            }
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            if (checkBoxDopQuery.Checked && ValidPassword(textBoxPassword.Text))
            {
                btnRelax.Visible = true;
                btnCandle.Visible = true;
                textBoxPassword.Clear();
                return;
            }
            if (ValidPassword(textBoxPassword.Text))
            {
                CheckFileExists(true);
                PrintDateLastUpdate();
                textBoxPassword.Clear();
                panelChooseData.Visible = true;
                panelUpdate.Visible = true;
            }
        }

        private Boolean ValidPassword(string password)
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

        private void btnMonthListHe_Click(object sender, EventArgs e)
        {
            textBoxList.Visible = true;
            print.DaysFromMonth(textBoxList);
            textBoxSum.Visible = true;
            print.Expenses(textBoxSum);
        }

        private void btnTMonthListHe_Click(object sender, EventArgs e)
        {
            textBoxList.Visible = true;
            print.ExpensesPartyList(textBoxList);
            textBoxSum.Visible = true;
            print.ExpensesPartySum(textBoxSum);
        }

        private void btnNMonthListHe_Click(object sender, EventArgs e)
        {
            textBoxList.Visible = true;
            print.ExpensesMustList(textBoxList);
            textBoxSum.Visible = true;
            print.ExpensesMustSum(textBoxSum);
        }

        private void btnSheMonthListHe_Click(object sender, EventArgs e)
        {
            textBoxList.Visible = true;
            print.ExpensesSheList(textBoxList);
            textBoxSum.Visible = true;
            print.ExpensesSheSum(textBoxSum);
        }

        private void btnSheAlListHe_Click(object sender, EventArgs e)
        {
            textBoxList.Visible = true;
            print.ExpensesSheListAllTimes(textBoxList);
            textBoxSum.Visible = true;
            print.ExpensesSheSumAllTimes(textBoxSum);
        }

        private void btnAllListHe_Click(object sender, EventArgs e)
        {
            textBoxList.Visible = true;
            print.ExpensesListAllTimes(textBoxList);
            textBoxSum.Visible = true;
            print.ExpensesSumAllTimes(textBoxSum);
        }

        private void btnNAllListHe_Click(object sender, EventArgs e)
        {
            textBoxList.Visible = true;
            print.ExpensesMustListAllTimes(textBoxList);
            textBoxSum.Visible = true;
            print.ExpensesMustSumAllTimes(textBoxSum);
        }

        private void btnTAllListHe_Click(object sender, EventArgs e)
        {
            textBoxList.Visible = true;
            print.ExpensesPartyListAllTimes(textBoxList);
            textBoxSum.Visible = true;
            print.ExpensesPartySumAllTimes(textBoxSum);
        }

        private void btnRelax_Click(object sender, EventArgs e)
        {
            textBoxList.Visible = true;
            print.ExpensesRelaxList(textBoxList);
            textBoxSum.Visible = true;
            print.ExpensesRelaxSum(textBoxSum);
        }

        private void btnCandle_Click(object sender, EventArgs e)
        {
            textBoxList.Visible = true;
            print.ExpensesCandleList(textBoxList);
            textBoxSum.Visible = true;
            print.ExpensesCandleSum(textBoxSum);
        }

        private void checkBoxDopQuery_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxDopQuery.Checked)
            {
                textBoxPassword.Focus();
            }
            else
            {
                btnRelax.Visible = false;
                btnCandle.Visible = false;
            }
        }



        private void btnClosePanelGit_Click(object sender, EventArgs e)
        {
            panelWorkGit.Visible = false;
            btnClosePanelGit.Visible = false;
            FManager.ActiveForm.Width = 845;
        }

        private void btnCheckGit_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(@"C:\Program Files (x86)\Git") || Directory.Exists(@"C:\Program Files\Git"))
            {
                panelWorkGit.Visible = false;
                panelUpdate.Visible = true;
                FManager.ActiveForm.Width = 845;
            }
            else
            {
                btnInstallGit.Visible = true;
                panelUpdate.Visible = false;
                if (File.Exists(set.locationWorkFolder + "\\gitExists.sys"))
                    File.Delete(set.locationWorkFolder + "\\gitExists.sys");
                label9.Text = "На данном компьюетере" + Environment.NewLine +
                    "скорее всего отсутствует" + Environment.NewLine +
                     "инструмента обновления данных." + Environment.NewLine +
                    "В данном случае система может ошибиться," + Environment.NewLine +
                    "но если вы сами не уверены, " + Environment.NewLine +
                    "то лучше запустите установку инструмента.";
            }
        }

        private void btnInstallGit_Click(object sender, EventArgs e)
        {
            labelForStatusGitInstall.Text = "Началась распаковка файла..." + Environment.NewLine;
            byte[] git = Properties.Resources.Git;
            File.WriteAllBytes(set.locationGitFile, git);
            labelForStatusGitInstall.Text += "Файл распакован, начало установки.." + Environment.NewLine;
            Process process = Process.Start(set.locationGitFile);
            process.EnableRaisingEvents = true;
            process.WaitForExit();
            if (Directory.Exists(@"C:\Program Files (x86)\Git") || Directory.Exists(@"C:\Program Files\Git"))
            {
                panelWorkGit.Visible = false;
                panelUpdate.Visible = true;
                FManager.ActiveForm.Width = 845;
                labelForStatusGitInstall.Text = "Установка завершена.";
                set.CreateFile(set.locationWorkFolder + "\\gitExists.sys");
            }
            else
            {
                labelForStatusGitInstall.Text = "Либо установка была отменена," +
                    Environment.NewLine +
                    "либо сообщи о случившемся разработчику";
            }
            try
            {
                File.Delete(set.locationGitFile);
            }
            catch (Exception)
            {

            }
        }

        private void btnDontCheck_Click(object sender, EventArgs e)
        {
            panelWorkGit.Visible = false;
            panelUpdate.Visible = true;
            FManager.ActiveForm.Width = 845;
            btnClosePanelGit.Visible = false;
            set.CreateFile(set.locationWorkFolder + "\\gitExists.sys");
        }

        private void btnOpenPanelGit_Click(object sender, EventArgs e)
        {
            labelForStatusGitInstall.Text = "";
            btnClosePanelGit.Visible = true;
            FManager.ActiveForm.Width = 1111;
            panelWorkGit.Visible = true;
        }
    }
}