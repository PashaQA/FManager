using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FManager;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Libs;

namespace Starter
{
    /// FManager---------------------------------------------------------------------------------------
    /// Формирование программы 16h
    /// Доработка над модулями:
    ///     Работа над обновление LatestHe - 7h
    ///     Обработка с добавлением возможности сохранения ошибки при запуске драйвера - 1.5h
    ///     Работа над переносом обновления под Git - 7h
    ///     
    ///  Разработка version 2.0: 
    ///     Работа над модулем считывания и записи данных из файла в БД: 12h
    ///     Проектирование интерфейса и часть первичной логики: 1h
    ///     Реализация интерфейса: 6h
    ///     Реализация первичной логики и доработка по интерфейсу: 8h
    ///     Реализация модуля аналитики: 12h
    ///     Реализация модуля администратора: 7h
    ///     Реализация модуля обновления: 12.5h
    ///     Реализация аналитики по таблицам He: 12h
    ///     Реализация модуля дополнительных возможностей: 2h
    ///     Реализоция перехода на SQLite: 3h
    /// 
    ///  Исправление багов:
    ///     Если первый символ в строке название месяца не равен '-', тогда этот месяц игнорировался 1h
    ///     Разделитель даты в случае разных БД 0.5h
    
    /// Starter------------------------------------------------------------------------------------------
    /// Проектировка дизайна 2h
    /// Реализация лаунчеров 9.5h
    ///  
    /// 
    ///--------------------------------------------------------------------------------------------------
    
    /// Chart--------------------------------------------------------------------------------------------
    /// Работа над интерфейсом: 0.5h
    /// Работа над back-end: 1h
    /// ------------------------------------------------------------------------------------------------
    
    ///------
    ///FManager
    /// -Добавить возможность запуска FManager'a без проверки на обновление, чтение данных с файла. Вынести данные настройки в стартер.
    ///     Подумать о том, какие еще настройки можно вынести в стартер.
    ///------

    ///--------------------------------------------------------------------------------------------------
    ///     Версия 3.0
    ///     Аналитика одежды.
    ///         /// CManager-----------------------------------------------------------------------------------------
    /// 
                ///--------------------------------------------------------------------------------------------------
    ///-------------------------------------------------------------------------------------------------

    public partial class Form_Start_Apps : Form
    {
        private const string _he = "Он.txt";
        private const string _he_eng = "he.txt";
        private const string _heBig = "ОнКрупный.txt";
        private const string _heBig_eng = "hebig.txt";
        private const string _heGifts = "ОнШмот.txt";
        private const string _heGifts_eng = "hegifts.txt";
        private const string _she = "Она.txt";
        private const string _she_eng = "she.txt";
        private const string _sheBig = "ОнаКрупный.txt";
        private const string _sheBig_eng = "shebig.txt";

        private bool calibrate;
        private Assistant assis = new Assistant();
        private List<string> uploadFiles = new List<string>();

        public Form_Start_Apps()
        {
            InitializeComponent();
        }

        private void Form_Start_Apps_Shown(object sender, EventArgs e)
        {
            if (!Directory.Exists(assis.locationSystemFolder))
                Directory.CreateDirectory(assis.locationSystemFolder);

            if (File.Exists(assis.locationFileLastCheckSizeSystemFolder))
            {
                DateTime dt = DateTime.Parse(File.ReadAllText(assis.locationFileLastCheckSizeSystemFolder));
                dt = dt.AddMonths(1);
                if (dt < DateTime.Now)
                    AppendTextInStatus("Вы давно не проверяли размер системных папок. Для вашего же благо нужно запустить данную проверку.");
            }
            else
            {
                AppendTextInStatus("Вы никогда не проверяли размер системных папок. Необходимо запустить данную проверку" +
                    " для начала слежки за активностью роста размера системных папок.");
            }

            if(File.Exists(assis.locationNeedBackup))
            {
                SetEnablePanels(false);
                AppendTextInStatus("Необходимо выполнить загрузку экспортированных данных (клик по кнопке 'backup')");
            }
        }

        private void buttonStartFManager_Click(object sender, EventArgs e)
        {
            assis.CreateFile(assis.locationPermissionStartFM);
            Process.Start(assis.locationWorkFolder + "\\FManager.exe");
            AppendTextInStatus("Запуск приложения FManager..");
        }

        private void checkBoxFinanceAll_CheckedChanged(object sender, EventArgs e)
        {
            if (calibrate)
            {
                checkBoxFinanceAll.Checked = false;
            }
            else if (checkBoxFinanceAll.Checked)
            {
                checkBoxShe.Checked = true;
                checkBoxSheBig.Checked = true;
                checkBoxHe.Checked = true;
                checkBoxHeBig.Checked = true;
                checkBoxHeGifts.Checked = true;
            }
            else
            {
                checkBoxShe.Checked = false;
                checkBoxSheBig.Checked = false;
                checkBoxHe.Checked = false;
                checkBoxHeBig.Checked = false;
                checkBoxHeGifts.Checked = false;
            }

        }

        private void checkBoxCheckroom_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCheckroom.Checked)
                if (!checkChoosedFile(""))
                    checkBoxCheckroom.Checked = false;
        }

        private void checkBoxShe_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShe.Checked)
            {
                if (!checkChoosedFile(_she))
                {
                    checkBoxShe.Checked = false;
                }
                else
                {
                    uploadFiles.Add(_she_eng);
                    RenameFileIntoWorkFolder(_she, _she_eng);
                }
                CalibrationFinanaceCBChecked();
            }
            else
            {
                uploadFiles.Remove(_she_eng);
                RenameFileIntoWorkFolder(_she_eng, _she);
                CalibrationFinanceCBUnchecked();
            }
        }

        private void checkBoxSheBig_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSheBig.Checked)
            {
                if (!checkChoosedFile(_sheBig))
                {
                    checkBoxSheBig.Checked = false;
                }
                else
                {
                    uploadFiles.Add(_sheBig_eng);
                    RenameFileIntoWorkFolder(_sheBig, _sheBig_eng);
                }
                CalibrationFinanaceCBChecked();
            }
            else
            {
                uploadFiles.Remove(_sheBig_eng);
                RenameFileIntoWorkFolder(_sheBig_eng, _sheBig);
                CalibrationFinanceCBUnchecked();
            }
        }

        private void checkBoxHe_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHe.Checked)
            {
                if (!checkChoosedFile(_he))
                {
                    checkBoxHe.Checked = false;
                }
                else
                {
                    RenameFileIntoWorkFolder(_he, _he_eng);
                    uploadFiles.Add(_he_eng);
                }
                CalibrationFinanaceCBChecked();
            }
            else
            {
                uploadFiles.Remove(_he_eng);
                RenameFileIntoWorkFolder(_he_eng, _he);
                CalibrationFinanceCBUnchecked();
            }
        }

        private void checkBoxHeBig_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHeBig.Checked)
            {
                if (!checkChoosedFile(_heBig))
                {
                    checkBoxHeBig.Checked = false;
                }
                else
                {
                    RenameFileIntoWorkFolder(_heBig, _heBig_eng);
                    uploadFiles.Add(_heBig_eng);
                }
                CalibrationFinanaceCBChecked();
            }
            else
            {
                uploadFiles.Remove(_heBig_eng);
                RenameFileIntoWorkFolder(_heBig_eng, _heBig);
                CalibrationFinanceCBUnchecked();
            }
        }

        private void checkBoxHeGifts_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHeGifts.Checked)
            {
                if (!checkChoosedFile(_heGifts))
                {
                    checkBoxHeGifts.Checked = false;
                }
                else
                {
                    RenameFileIntoWorkFolder(_heGifts, _heGifts_eng);
                    uploadFiles.Add(_heGifts_eng);
                }
                CalibrationFinanaceCBChecked();
            }
            else
            {
                uploadFiles.Remove(_heGifts_eng);
                RenameFileIntoWorkFolder(_heGifts_eng, _heGifts);
                CalibrationFinanceCBUnchecked();
            }
        }


        private void buttonUploadUpdate_Click(object sender, EventArgs e)
        {
            if (!checkBoxHe.Checked && !checkBoxHeBig.Checked && !checkBoxHeGifts.Checked &&
                !checkBoxShe.Checked && !checkBoxSheBig.Checked && !checkBoxFinanceAll.Checked && !checkBoxCheckroom.Checked)
            {
                MessageBox.Show("Необходимо выбрать таблицу для обновления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidateComment()) return;

            string uploader_data = assis.CreateNumericDirectory("uploader_data");
            string updater_fmanager = "\\Updater_FManager";

            AppendTextInStatus("Началась закачка репозитория для очистки.");

            ProcessStartInfo info = new ProcessStartInfo("cmd.exe", @"/c cd \ & cd " + uploader_data +
                @" & git clone https://github.com/PashaQA/Updater_FManager.git");
            Process gitExec = Process.Start(info);
            gitExec.EnableRaisingEvents = true;
            gitExec.WaitForExit();
            gitExec.Close();

            string[] files = Directory.GetFiles(uploader_data + updater_fmanager);
            foreach (var file in files)
                if (file.EndsWith(".txt"))
                    File.Delete(file);

            AppendTextInStatus("Закачка завершена, очищаем репозиторий.");
            CommitUpdate(uploader_data + updater_fmanager, textBoxCommitComment.Text);
            AppendTextInStatus("Репозиторий очищен и готов к загрузки файлов обновления.");

            for (int i = 0; i < uploadFiles.Count; i++)
                File.Move(assis.locationWorkFolder + @"\" + uploadFiles[i],
                   uploader_data + @"\Updater_FManager\" + uploadFiles[i]);

            AppendTextInStatus("Началась заканча обновления.");
            CommitUpdate(uploader_data + updater_fmanager, textBoxCommitComment.Text.Replace(' ', '_'));
            AppendTextInStatus("Закачка обновлений выполнена успешно.");

            textBoxCommitComment.Text = string.Empty;
        }

        private void buttonBackup_Click(object sender, EventArgs e)
        {
            List<string> backupFiles = new List<string>();
            string[] files = Directory.GetFiles(assis.locationWorkFolder);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].EndsWith(".sys"))
                    if (!files[i].EndsWith("admin.sys") && !files[i].EndsWith("updateDate.sys"))
                        backupFiles.Add(files[i]);
                if (files[i].EndsWith("stats.mdf"))
                        backupFiles.Add(files[i]);
            }

            int count_exists = 0;
            for (int i = 0; i < backupFiles.Count; i++)
            {
                if (backupFiles[i].EndsWith("Latest.sys"))
                {
                    count_exists++;
                    AppendTextInStatus("Для загрузки на сервер обнаружен файл: Latest.sys");
                }
                else if (backupFiles[i].EndsWith("SheBig.sys"))
                {
                    count_exists++;
                    AppendTextInStatus("Для загрузки на сервер обнаружен файл: SheBig.sys");
                }
                else if (backupFiles[i].EndsWith("LatestHe.sys"))
                {
                    count_exists++;
                    AppendTextInStatus("Для загрузки на сервер обнаружен файл: LatestHe.sys");
                }
                else if (backupFiles[i].EndsWith("HeBig.sys"))
                {
                    count_exists++;
                    AppendTextInStatus("Для загрузки на сервер обнаружен файл: HeBig.sys");
                }
                else if (backupFiles[i].EndsWith("HeGifts.sys"))
                {
                    count_exists++;
                    AppendTextInStatus("Для загрузки на сервер обнаружен файл: HeGifts.sys");
                }
                else if (backupFiles[i].EndsWith("stats.mdf"))
                {
                    count_exists++;
                    AppendTextInStatus("Для загрузки на сервер обнаружен файл: stats.mdf");
                }
            }
            if (count_exists == 1)
            {
                if (backupFiles[0].EndsWith("stats.mdf"))
                {
                    MessageBox.Show("Для обновления готова только БД, но этого не достаточно и это не надежно. Выполните экспорт данных.",
                        "Обнаружена уязвимость", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    AppendTextInStatus("Отменена загрузка последних файлов статистики из-за их отсутствия.");
                    return;
                }
            }

            if (count_exists < 6)
            {
                DialogResult result = MessageBox.Show("Не все файлы файлы статистики были обнаружены. Загрузить существующие?", "Уязвимость целостности",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    AppendTextInStatus("Отменена загрузка файлов статистики.");
                    return;
                }
            }
            
            if (!ValidateComment()) return;

            string backup = assis.CreateNumericDirectory("backup");
            string backup_stats = @"\Backup_Stats\";

            ProcessStartInfo info = new ProcessStartInfo("cmd.exe", @"/c cd \ & cd " + backup +
                @" & git clone https://github.com/PashaQA/Backup_Stats.git");
            Process gitExec = Process.Start(info);
            gitExec.EnableRaisingEvents = true;
            gitExec.WaitForExit();
            gitExec.Close();

            for (int i = 0; i < backupFiles.Count; i++)
            {
                int idx = backupFiles[i].LastIndexOf("\\");
                string file_name = backupFiles[i].Remove(0, idx + 1);
                string ready_file = backup + backup_stats + file_name;

                if (File.Exists(ready_file))
                    File.Delete(ready_file);

                if (backupFiles[i].EndsWith(".sys"))
                    File.Move(backupFiles[i], ready_file);
                else
                    File.Copy(backupFiles[i], backup + backup_stats + file_name);
            }

            CommitUpdate(backup + backup_stats, textBoxCommitComment.Text.Replace(' ', '_'));

            AppendTextInStatus("Выбранные файлы статистики успешно загружены.");

            if (File.Exists(assis.locationNeedBackup))
            {
                SetEnablePanels(true);
                File.Delete(assis.locationNeedBackup);
            }

            textBoxCommitComment.Text = string.Empty;
        }

        private void Form_Start_Apps_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hide();
            string[] folders = Directory.GetDirectories(assis.locationFilesFolder);
            foreach(string folder in folders)
            {
                try
                {
                    Directory.Delete(folder, true);
                }
                catch(Exception)
                {

                }
            }

        }

        private void пересчитатьРазмерРабочихФайловToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppendTextInStatus("Запущен пересчет размера системной папки..");
            long size = 0;
            string[] folders = Directory.GetDirectories(assis.locationSystemFolder);
            foreach(string folder in folders)
            {
                string[] files = Directory.GetFiles(folder, "*", SearchOption.AllDirectories);
                foreach(string file in files)
                {
                    FileInfo info = new FileInfo(file);
                    size += info.Length;
                }
            }
            size = size / 1024 / 1024;

            if (!File.Exists(assis.locationFileLastCheckSizeSystemFolder))
                assis.CreateFile(assis.locationFileLastCheckSizeSystemFolder);
            using (StreamWriter sw = new StreamWriter(assis.locationFileLastCheckSizeSystemFolder))
                sw.Write(DateTime.Now.ToShortDateString());
                
            AppendTextInStatus("Размер системной папки составляет: " + size.ToString() + "мб.");
            if (size >= 100)
                AppendTextInStatus("Необходимо подчистить системные папки. В меню системных "+
                    "возможностей выберите 'Перейти к системным папкам' и удалите папку 'Files'.");
        }

        private void запуститьДокументациюToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void перейтиКСистемнымПапкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("cmd.exe", "/c start " + assis.locationSystemFolder);
        }

        private void запускПервойВерсииПриложенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!File.Exists(assis.locationHeFile) || !File.Exists(assis.locationSheFile))
            {
                AppendTextInStatus("Один из файлов аналитики по формату старой версии отсутствует. "+
                    "Необходимо запустить вторую версиюю и в дополнительных возможностях экспортировать таблицу He и She.");
                return;
            }
            if (!File.Exists(assis.locationFirstVersion))
            {
                AppendTextInStatus("Отсутствует файл первом версии, поэтому мы запустии его распаковку.");
                byte[] first_version = Properties.Resources.FManager_version_1;
                File.WriteAllBytes(assis.locationFirstVersion, first_version);
            }
            AppendTextInStatus("Запуск первой версии приложения..");
            Process.Start(assis.locationFirstVersion);
        }

        private void textBoxStatus_MouseMove(object sender, MouseEventArgs e)
        {
            textBoxStatus.Enabled = false;
        }

        private void textBoxBackup_MouseMove(object sender, MouseEventArgs e)
        {
            textBoxBackup.Enabled = false;
        }

        /// <summary>
        /// Если все чек боксы включены, значить основной чек бокс тоже включаем
        /// </summary>
        private void CalibrationFinanaceCBChecked()
        {
            if (checkBoxShe.Checked && checkBoxSheBig.Checked && checkBoxHe.Checked &&
                checkBoxHeBig.Checked && checkBoxHeGifts.Checked)
                checkBoxFinanceAll.Checked = true;
        }

        /// <summary>
        /// Если все чек боксы выключены, значит основной чек бокс выключаем
        /// </summary>
        private void CalibrationFinanceCBUnchecked()
        {
            calibrate = true;
            if (!checkBoxShe.Checked || !checkBoxSheBig.Checked || !checkBoxHe.Checked ||
                !checkBoxHeBig.Checked || !checkBoxHeGifts.Checked)
                checkBoxFinanceAll.Checked = false;
            calibrate = false;
        }

        /// <summary>
        /// После активации чекбокса проверяет, действительно ли есть данный файл в рабочей директории.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private Boolean checkChoosedFile(string filename)
        {
            string[] files = Directory.GetFiles(assis.locationWorkFolder);
            for (int i = 0; i < files.Length; i++)
                if (files[i].EndsWith(filename))
                    return true;
            MessageBox.Show("Выбранный файл(" + filename + ") отсутствует в рабочей директорий.", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return false;
        }

        /// <summary>
        /// Переименновывает файлы внутри рабочей директории.
        /// </summary>
        /// <param name="old_name"></param>
        /// <param name="new_name"></param>
        private void RenameFileIntoWorkFolder(string old_name, string new_name)
        {
            try
            {
                File.Move(assis.locationWorkFolder + @"\" + old_name,
                    assis.locationWorkFolder + @"\" + new_name);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Выполняет git push директории, указаной в качестве аргумента
        /// </summary>
        /// <param name="fullpathdirectory"></param>
        /// <param name="message"></param>
        private void CommitUpdate(string fullpathdirectory, string message)
        {
            Process.Start("cmd.exe", @"/c cd \ & cd " + fullpathdirectory +
                @"& git add ." +
                @"& git commit -m '" + message + "'" +
                @"& git push origin -u");
        }

        /// <summary>
        /// Возвращает true, если в окне комментариев написан хотя бы один символ.
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateComment()
        {
            if (textBoxCommitComment.Text == "")
            {
                DialogResult result = MessageBox.Show("Вы уверены что хотите залить обновление без комментария?",
                    "Неясности", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    MessageBox.Show("Введите пожалуйста комментарий.", "Ясности", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxCommitComment.Focus();
                    AppendTextInStatus("Отменена загрузки из-за отсутствия комментрия.");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Добавляет текст в конец окна отображения шага системы.
        /// Один шаг на одной линии.
        /// </summary>
        /// <param name="text"></param>
        private void AppendTextInStatus(string text)
        {
            textBoxStatus.AppendText(text);
            textBoxStatus.Text += Environment.NewLine;
        }

        /// <summary>
        /// Делают доступыми, либо не доступными все панели
        /// </summary>
        /// <param name="value"></param>
        private void SetEnablePanels(bool value)
        {
            SystemToolStripMenuItem.Enabled = value;
            panelStartAnalitics.Enabled = value;
            panelHelper.Enabled = value;
            panelUpdaterFinance.Enabled = value;
            buttonUploadUpdate.Enabled = value;
            checkBoxFinanceAll.Enabled = value;
        }
    }
}
