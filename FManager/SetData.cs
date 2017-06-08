using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FManager
{
    public class SetData
    {
        public string locationWorkFolder
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\FManager";
            }
        }

        public string locationCurrentFile
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

        public string locationUpdateDateHe
        {
            get
            {
                return locationWorkFolder + @"\updateDateHe.sys";
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

        public void ConvertNewFileToList()
        {
            string newFile = null;
            string[] splited = null;
            string locationNewFile = this.locationNewFile;
            if (File.Exists(locationNewFileHe))
                locationNewFile = locationNewFileHe;
            using (StreamReader sr = new StreamReader(locationNewFile, Encoding.Default))
            {
                newFile = sr.ReadToEnd();
                splited = newFile.Split('\n');
            }

            //сохранить файл в виде списка
            using (StreamWriter sw = new StreamWriter(locationNewFile, false, Encoding.Default))
            {
                for (int i = 0; i < splited.Length; i++)
                    sw.WriteLine(splited[i]);
            }
        }

        public string[] GetParsedList(string path)
        {
            List<string> deletedSpace = new List<string>();
            StreamReader sr = null;
            if (path.IndexOf("Latest") != -1 || path.IndexOf("he") != -1)
                sr = new StreamReader(path, Encoding.Default);
            else if (path.IndexOf("she") != -1)
                sr = new StreamReader(path, Encoding.UTF8); 
            string allText = sr.ReadToEnd();
            sr.Close();
            string[] splited = allText.Split('\n');

            //удаление последнего симлова '\r'
            while (splited[0].LastIndexOf('\r') != -1)
            {
                for (int i = 0; i < splited.Length; i++)
                {
                    if (splited[i].EndsWith("\r"))
                        splited[i] = splited[i].Remove(splited[i].Length - 1);
                    if (splited[i].EndsWith(" "))
                        splited[i] = splited[i].Remove(splited[i].Length - 1);
                }
            }

            for (int i = 0; i < splited.Length; i++)
            {
                if (splited[i] != "")
                    deletedSpace.Add(splited[i]);
            }

            splited = new string[deletedSpace.Count];

            for (int i = 0; i < deletedSpace.Count; i++)
                splited[i] = deletedSpace[i];

            return splited;
        }

        public void CreateFile(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {

            }
        }

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
    }
}
