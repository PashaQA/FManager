using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FManager
{
    public class Assistant
    {
        public string locationWorkFolder
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\FManager";
            }
        }

        public string locationSheFile
        {
            get
            {
                return locationWorkFolder + @"\Latest.sys";
            }
        }

        public string locationHeFile
        {
            get
            {
                return locationWorkFolder + @"\LatestHe.sys";
            }
        }
       
        public string locationNewFile
        {
            get
            { 
                return locationRepository + @"\she.txt";
            }
        }

        public string locationNewFileHe
        {
            get
            {
                return locationRepository + @"\he.txt";
            }
        }

        public string locationUpdateDate
        {
            get
            {
                return locationWorkFolder + @"\updateDate.sys";
            }
        }

        public string locationRepository
        {
            get
            {

                return locationWorkFolder + @"\Updater_FManager";
            }
        }
       
        public string locationGitFile
        {
            get
            {
                return locationWorkFolder + @"\git.exe";
            }
        }

        /// <summary>
        /// Оба файла обновления конвертирует в список для записи в БД
        /// </summary>
        public void ConvertNewFilesToList()
        {
            string newFile = null;
            string[] splited = null;
            string[] locationNewFiles = new string[2] { locationNewFile, locationNewFileHe };

            for (int i = 0; i < locationNewFiles.Length; i++)
            {
                //убирает пробелы
                using (StreamReader sr = new StreamReader(locationNewFiles[i], Encoding.Default))
                {
                    newFile = sr.ReadToEnd();
                    splited = newFile.Split('\n');
                }

                //сохранить файл в виде списка
                using (StreamWriter sw = new StreamWriter(locationNewFiles[i], false, Encoding.Default))
                {
                    for (int j = 0; j < splited.Length; j++)
                        sw.WriteLine(splited[j]);
                }
            }
        }

        /// <summary>
        /// Создает пустой файл по выбранному пути
        /// </summary>
        /// <param name="filename"></param>
        public void CreateFile(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {

            }
        }

        /// <summary>
        /// Создает батник загрузки обновления
        /// </summary>
        /// <param name="name"></param>
        public void CreateBat(string name)
        {
            StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                + "\\FManager\\" + name + ".bat");
            sw.WriteLine("cmd.exe", "/c "
                + @"cd \ & cd " + locationWorkFolder
                + " & "
                + @"git clone --no-checkout https://github.com/PashaQA/Updater_FManager.git & cd Updater_FManager"
                + " & "
                + "git reset --hard HEAD"
                + " & "
                + "git log");
            sw.Close();
        }

        /// <summary>
        /// Првоеряет на наличае иероглифов в файле 
        /// </summary>
        /// <param name="fullList"></param>
        /// <returns></returns>
        public Boolean IsHierogliph(string[] fullList)
        {
            char[] array = fullList[2].ToCharArray();
            foreach (var symbol in array)
            {
                if (symbol == 1035 ||
                    symbol == 1034 ||
                    symbol == 1105 ||
                    symbol == 1038 ||
                    symbol == 1027)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Удалает файлы с обновлением
        /// </summary>
        /// <param name="textBoxUpdate"></param>
        public void DeleteUpdateFiles(TextBox textBoxUpdate)
        {
            try
            {
                File.Delete(locationNewFile);
                File.Delete(locationNewFileHe);

            }
            catch (Exception)
            {
                textBoxUpdate.Text += "Не удалось удалить старые файлы обновления, советую почистиь из в ручную из рабочей папки." + Environment.NewLine;
            }
        }

    }
}
