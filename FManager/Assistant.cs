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

        public string locationSystemFolder
        {
            get
            {
                return locationWorkFolder + "\\System";
            }
        }

        public string locationFilesFolder
        {
            get
            {
                return locationSystemFolder + "\\Files";
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
                return locationUpdateFolder + @"\Updater_FManager\she.txt";
            }
        }

        public string locationNewFileHe
        {
            get
            {
                return locationUpdateFolder + @"\Updater_FManager\he.txt";
            }
        }

        public string locationUpdateDate
        {
            get
            {
                return locationSystemFolder + @"\updateDate.sys";
            }
        }

        public string locationRepository { get; set; }
       
        public string locationUpdateFolder { get; set; }

        public string locationGitFile
        {
            get
            {
                return locationSystemFolder + @"\git.exe";
            }
        }

        public string locationGitExistsFile
        {
            get
            {
                return locationSystemFolder + @"\gitExists.sys";
            }
        }


        public string locationHeGifts
        {
            get
            {
                return locationWorkFolder + @"\HeGifts.sys";
            }
        }

        public string locationHeBig
        {
            get
            {
                return locationWorkFolder + @"\HeBig.sys";
            }
        }

        public string locationSheBig
        {
            get
            {
                return locationWorkFolder + @"\SheBig.sys";
            }
        }

        public string locationNewFileHeGifts
        {
            get
            {
                return locationUpdateFolder+ @"\Updater_FManager\hegifts.txt";
            }
        }

        public string locationNewFileHeBig
        {
            get
            {
                return locationUpdateFolder + @"\Updater_FManager\hebig.txt";
            }
        }
        
        public string locationNewFileSheBig
        {
            get
            {
                return locationUpdateFolder + @"\Updater_FManager\shebig.txt";
            }
        }      
    
        public string locationPashaFile
        {
            get
            {
                return locationSystemFolder + @"\admin.sys";
            }
        }

        public string locationFileLastCheckSizeSystemFolder
        {
            get
            {
                return locationSystemFolder + "\\dateLastCheck.sys";
            }
        }

        public string locationFirstVersion
        {
            get
            {
                return locationWorkFolder + "\\FManager_version_1.exe";
            }
        }

        public string locationPermissionStartFM
        {
            get
            {
                return locationSystemFolder + "\\permission.sys";
            }
        }

        public string locationNeedExport
        {
            get
            {
                return locationSystemFolder + "\\need_export.sys";
            }
        }

        public string locationNeedBackup
        {
            get
            {
                return locationSystemFolder + "\\need_backup.sys";
            }
        }

        public string locationPermissionAutoExport
        {
            get
            {
                return locationSystemFolder + "\\permission_auto_export.sys";
            }
        }

        public List<string> ready_tables { get; set; }

        /// <summary>
        /// Оба файла обновления конвертирует в список для записи в БД
        /// </summary>
        public void ConvertNewFilesToList()
        {
            ready_tables = new List<string>();

            string newFile = null;
            string[] splited = null;
            string[] new_files = Directory.GetFiles(locationUpdateFolder + "\\Updater_FManager");
            foreach(var file in new_files)
            {
                if (file.EndsWith("he.txt"))
                    ready_tables.Add("he.txt");
                else if (file.EndsWith("hebig.txt"))
                    ready_tables.Add("hebig.txt");
                else if (file.EndsWith("hegifts.txt"))
                    ready_tables.Add("hegifts.txt");
                else if (file.EndsWith("she.txt"))
                    ready_tables.Add("she.txt");
                else if (file.EndsWith("shebig.txt"))
                    ready_tables.Add("shebig.txt");
            }

            for (int i = 0; i < new_files.Length; i++)
            {
                using (StreamReader sr = new StreamReader(new_files[i], Encoding.Default))
                {
                    newFile = sr.ReadToEnd();
                    splited = newFile.Split('\n');
                }

                //сохранить файл в виде списка
                using (StreamWriter sw = new StreamWriter(new_files[i], false, Encoding.Default))
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
                + @"cd \ & cd " + locationUpdateFolder
                + " & "
                + @"git clone --no-checkout https://github.com/PashaQA/Updater_FManager.git & cd Updater_FManager"
                + " & "
                + "git reset --hard HEAD"
                + " & "
                + "git log");
            sw.Close();
        }

        /// <summary>
        /// Создает папку в по заданному пути.
        /// </summary>
        /// <param name="fullpath"></param>
        public void CreateDirectory(string fullpath)
        {
            Directory.CreateDirectory(fullpath);
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
                Directory.Delete(locationRepository, true);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Создает очередную директорию и возвращает к ней путь.
        /// </summary>
        /// <param name="directoryname"></param>
        /// <returns></returns>
        public String CreateNumericDirectory(string directoryname)
        {
            if (!Directory.Exists(locationFilesFolder))
                Directory.CreateDirectory(locationFilesFolder);
            string numericFolder = locationFilesFolder + @"\" + directoryname + @"\";
            if (!Directory.Exists(numericFolder))
                CreateDirectory(numericFolder);
            string[] folders = Directory.GetDirectories(numericFolder);
            if (folders.Length == 0)
            {
                CreateDirectory(numericFolder + "0");
                return locationRepository = numericFolder + "0";
            }
            else
            {
                CreateDirectory(numericFolder + folders.Length);
                return locationRepository = numericFolder + folders.Length;
            }
        }

        /// <summary>
        /// Возвращает полный путь до определенной нумерованной директории
        /// </summary>
        /// <param name="dirBeforeNum"></param>
        /// <param name="dirAfterNum"></param>
        /// <returns></returns>
        public String getNumericFolder(string dirBeforeNum, string dirAfterNum)
        {
            //Не раочая функция, проверить пути до папок
            string[] folders = Directory.GetDirectories(locationWorkFolder + "\\" + dirBeforeNum);
            string[] nums = folders[folders.Length - 1].Split('\\');
            return locationWorkFolder + "\\" + dirBeforeNum + "\\" + nums[nums.Length] + "\\" + dirAfterNum;

        }
    }
}
