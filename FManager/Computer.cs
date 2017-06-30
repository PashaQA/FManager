using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FManager
{
    public class ParseFile
    {
        private Assistant assis = new Assistant();
        private DB db = new DB();

        public ParseFile()
        {

        }
        
        /// <summary>
        /// Возвращает массив записей с файла по указанному пути
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Выполняет запись в выбранную таблицу с файла, соответствующего выбранной таблице
        /// </summary>
        /// <param name="table"></param>
        public void SetDataFromFileToDB(DB.Tables table, bool update=false)
        {
            string path = string.Empty;
            if (table == DB.Tables.He)
            {
                path = assis.locationHeFile;
                if (update)
                    path = assis.locationNewFileHe;
            }
            else if (table == DB.Tables.She)
            {
                path = assis.locationSheFile;
                if (update)
                    path = assis.locationNewFile;
            }
            else
            {
                MessageBox.Show("Запись в данную таблицу пока что не возможна.", "Сыровато",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string[] fullList = GetParsedList(path);
            Dictionary<string, string[]> allMonths = setAllMonths(fullList);
            for (int i = 0; i < allMonths.Count; i++)
            {
                string[] names = getAllNamesMonthYears(fullList);
                for (int j = 0; j < allMonths[names[i]].Length; j++)
                {
                    string line = allMonths[names[i]][j];
                    string _date = getYearFromLine(names[i]) + "-" + getMonthFromLine(names[i]);
                    string _day = getDay(line);
                    string _event = "";
                    string _count = "";
                    string _count_expense = "";
                    string _desc = "";
                    string _type = "";
                    if (_day.StartsWith("-"))
                    {
                        int counter = 0;
                        while (true)
                        {
                            counter++;
                            if (!getDay(allMonths[names[i]][j - counter]).StartsWith("-"))
                            {
                                _day = getDay(allMonths[names[i]][j - counter]);
                                break;
                            }
                        }
                    }
                    _date += "-" + _day;
                    string[] list = line.Split(' ');
                    for (int k = 0; k < list.Length; k++)
                    {
                        if (list[k].StartsWith("+"))
                            _event = "+";
                        else if (list[k] == "-")
                            _event = "-";
                        else if (list[k] == "--")
                            _event = "--";

                        if (list[list.Length - 1].StartsWith("(")) _count = list[list.Length - 2];
                        else _count = list[list.Length - 1];

                        if (isMultiple(list[k]))
                            _count_expense = list[k].Remove(0, 1);

                        if (_desc == "" && list[k].StartsWith("-"))
                        {
                            int idxLocalStart = -1;
                            int idxLocalEnd = -1;
                            for (int x = 0; x < list.Length; x++)
                            {
                                if (idxLocalStart != -1 && list[x].StartsWith("-"))
                                    idxLocalEnd = x;
                                if (idxLocalStart == -1 && list[x].StartsWith("-"))
                                    idxLocalStart = x;
                            }
                            for (int x = idxLocalStart; x < idxLocalEnd; x++)
                            {
                                if (list[x].StartsWith("x") || list[x].StartsWith("х"))
                                    continue;
                                _desc += list[x] + " ";
                            }
                            _desc = _desc.Replace("-", "").Replace("--", "");
                            if (_desc[0] == ' ')
                                _desc = _desc.Remove(0, 1);
                        }

                        if (list[k].StartsWith("("))
                        {
                            _type = list[k].Remove(0, 1);
                            _type = _type.Remove(_type.Length - 1);
                        }
                    }
                    db.InsertIntoCasual(table, _date, _event, _count, _count_expense, _desc, _type, line);
                }
            }

        }

        private String getDay(string line)
        {
            int idxData = line.IndexOf(' ');
            return line = line.Remove(idxData);
        }

        private String[] getFirstMonth(string[] fullList)
        {
            int idxStart = -1;
            int idxStop = -1;

            for (int i = 0; i < fullList.Length; i++)
            {
                if (idxStart != -1 && fullList[i].StartsWith("----"))
                {
                    idxStop = i;
                    break;
                }
                else if (i == fullList.Length - 1 && idxStop == -1)
                    idxStop = i;

                if (fullList[i].StartsWith("----"))
                    idxStart = i;
            }

            List<string> newList = new List<string>();
            for (int i = idxStart + 1; i < idxStop; i++)
                newList.Add(fullList[i]);

            string[] returnedList = new string[newList.Count];
            for (int i = 0; i < newList.Count; i++)
                returnedList[i] = newList[i];

            return returnedList;
        }

        private Int32 getCountMonth(string[] fullList)
        {
            int counter = 0;
            for (int i = 0; i < fullList.Length; i++)
            {
                if (fullList[i].StartsWith("----"))
                    counter++;
            }
            return counter;
        }

        private String getMonthFromLine(string line)
        {
            int idx = line.LastIndexOf('-');
            line = line.Remove(0, idx + 1);
            idx = line.IndexOf(' ');
            string month = line.Remove(idx);
            if (month == "Январь")
                month = "01";
            else if (month == "Февраль")
                month = "02";
            else if (month == "Март")
                month = "03";
            else if (month == "Апрель")
                month = "04";
            else if (month == "Май")
                month = "05";
            else if (month == "Июнь")
                month = "06";
            else if (month == "Июль")
                month = "07";
            else if (month == "Август")
                month = "08";
            else if (month == "Сентябрь")
                month = "09";
            else if (month == "Октябрь")
                month = "10";
            else if (month == "Ноябрь")
                month = "11";
            else if (month == "Декабрь")
                month = "12";
            else
                month = "Не верно указано название месяца: " + line;
            return month;
        }

        private String getYearFromLine(string line)
        {
            int idx = line.LastIndexOf('-');
            line = line.Remove(0, idx + 1);
            idx = line.IndexOf(' ');
            return line.Remove(0, idx + 1);
        }

        private String[] getAllNamesMonthYears(string[] fullList)
        {
            int counter = 0;
            string[] months = new string[getCountMonth(fullList)];
            for (int i = 0; i < fullList.Length; i++)
            {
                if (fullList[i].StartsWith("----"))
                {
                    months[counter] = fullList[i];
                    counter++;
                }
            }
            return months;
        }

        private Boolean isMultiple(string line)
        {
            if (line[0] == 'x' || line[0] == 'х')
            {
                if (line[1] == '0' || line[1] == '1' || line[1] == '2' ||
                    line[1] == '3' || line[1] == '4' || line[1] == '5' ||
                    line[1] == '6' || line[1] == '7' || line[1] == '8' ||
                    line[1] == '9')
                    return true;
            }
            return false;
        }

        private String[] fromFullListRemoveFirstMonth(string[] fullList, string[] month)
        {
            List<string> newList = new List<string>();
            for (int i = month.Length + 1; i < fullList.Length; i++)
                newList.Add(fullList[i]);

            string[] returnedNewList = new string[newList.Count];
            for (int i = 0; i < newList.Count; i++)
                returnedNewList[i] = newList[i];

            return returnedNewList;
        }

        private Dictionary<string, string[]> setAllMonths(string[] fullList)
        {
            Dictionary<string, string[]> allMonths = new Dictionary<string, string[]>();
            int count = getCountMonth(fullList);
            for (int i = 0; i < count; i++)
            {
                string[] month = getFirstMonth(fullList);
                allMonths[getAllNamesMonthYears(fullList)[0]] = month;
                fullList = fromFullListRemoveFirstMonth(fullList, month);
            }
            return allMonths;
        }
    }

    public class ParseDB
    {
        private const string _id = "id";
        private const string _date_expense = "date_expense";
        private const string _event_type = "event_type";
        private const string _count = "count";
        private const string _count_expenses = "count_expenses";
        private const string _description = "description";
        private const string _type = "type";
        private const string _full_line = "full_line";

        private Dictionary<int, Dictionary<string, string>> data;
        private Dictionary<string, string[]> resultDic = new Dictionary<string, string[]>();
        private List<string> listDescExpenses = new List<string>();
        private List<string> listDescPersonExpenses = new List<string>();
        private List<string> listCount = new List<string>();

        public ParseDB(Dictionary<int, Dictionary<string, string>> data)
        {
            this.data = data;
        }

        /// <summary>
        /// Возвращает все года, которые есть в статистике
        /// </summary>
        /// <returns></returns>
        public List<int> getYears()
        {
            List<int> years = new List<int>();
            for (int i = 0; i < data.Count; i++)
            {
                bool exists = false;
                int idx = data[i][_date_expense].LastIndexOf('/');
                string year = data[i][_date_expense].Remove(0, idx + 1);
                idx = year.IndexOf(' ');
                year = year.Remove(idx);
                for (int j = 0; j < years.Count; j++)
                {
                    if (Convert.ToInt32(year) == years[j])
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists) years.Add(Convert.ToInt32(year));
            }
            return years;
        }

        /// <summary>
        /// Возвращает список месяцев определенного года
        ///     Если не указать год, вернет месяц и год всей статистики в формате мм/гггг
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<string> getMonths(int year = 0)
        {
            List<int> years = getYears();
            List<string> months = new List<string>();
            for (int i = 0; i < data.Count; i++)
            {
                if (year != 0)
                {
                    if (data[i][_date_expense].IndexOf(year.ToString()) != -1)
                    {
                        bool exists = false;
                        int idx = data[i][_date_expense].IndexOf('/');
                        string month = data[i][_date_expense].Remove(0, idx + 1);
                        idx = month.IndexOf('/');
                        month = month.Remove(idx);
                        for (int j = 0; j < months.Count; j++)
                        {
                            if (month == months[j])
                            {
                                exists = true;
                                break;
                            }
                        }
                        if (!exists) months.Add(month);
                    }
                }
                else
                {
                    for (int j = 0; j < years.Count; j++)
                    {
                        if (data[i][_date_expense].IndexOf(years[j].ToString()) != -1)
                        {
                            bool exists = false;
                            int idx = data[i][_date_expense].IndexOf('/');
                            string month = data[i][_date_expense].Remove(0, idx + 1);
                            idx = month.IndexOf('/');
                            month = month.Remove(idx);
                            for (int k = 0; k < months.Count; k++)
                            {
                                if (month + " " + years[j].ToString() == months[k])
                                {
                                    exists = true;
                                    break;
                                }
                            }
                            if (!exists) months.Add(month + " " + years[j].ToString());
                        }
                    }
                }
            }
            for (int i = 0; i < months.Count; i++)
            {
                if (months[i] == "01")
                {
                    months.Remove(months[i]);
                    months.Add("Январь");
                    i--;
                }
                else if (months[i] == "02")
                {
                    months.Remove(months[i]);
                    months.Add("Февраль");
                    i--;
                }
                else if (months[i] == "03")
                {
                    months.Remove(months[i]);
                    months.Add("Март");
                    i--;
                }
                else if (months[i] == "04")
                {
                    months.Remove(months[i]);
                    months.Add("Апрель");
                    i--;
                }
                else if (months[i] == "05")
                {
                    months.Remove(months[i]);
                    months.Add("Май");
                    i--;
                }
                else if (months[i] == "06")
                {
                    months.Remove(months[i]);
                    months.Add("Июнь");
                    i--;
                }
                else if (months[i] == "07")
                {
                    months.Remove(months[i]);
                    months.Add("Июль");
                    i--;
                }
                else if (months[i] == "08")
                {
                    months.Remove(months[i]);
                    months.Add("Август");
                    i--;
                }
                else if (months[i] == "09")
                {
                    months.Remove(months[i]);
                    months.Add("Сентябрь");
                    i--;
                }
                else if (months[i] == "10")
                {
                    months.Remove(months[i]);
                    months.Add("Октябрь");
                    i--;
                }
                else if (months[i] == "11")
                {
                    months.Remove(months[i]);
                    months.Add("Ноябрь");
                    i--;
                }
                else if (months[i] == "12")
                {
                    months.Remove(months[i]);
                    months.Add("Декабрь");
                    i--;
                }
                else break;
            }
            return months;
        }

        /// <summary>
        /// Возвращает словарь затрат за все время
        ///  Ключи -, --, count
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string[]> getExpensesAllTime()
        {
            int expenses = 0;
            int expensesPerson = 0;
            ClearLists();
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i][_event_type] == "-")
                    expenses += getExpensesFromDay(data[i]);
                else if (data[i][_event_type] == "--")
                    expensesPerson += getExpensesFromDay(data[i]);
            }
            resultDic["-"] = new string[1] { expenses.ToString() };
            resultDic["--"] = new string[1] { expensesPerson.ToString() };
            resultDic["count"] = new string[1] { getMonths().Count.ToString() };
            return resultDic;
        }

        /// <summary>
        /// Возвращает словарь затрат за весь текущий год
        ///     Ключи словаря -,--, descExpenses, descPersonExpenses
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string[]> getExpensesYear(string year)
        {
            int expenses = 0;
            int personExpenses = 0;
            ClearLists();
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i][_date_expense].IndexOf(year) != -1)
                {
                    if (data[i][_event_type] == "-")
                    {
                        expenses += getExpensesFromDay(data[i]);
                        listDescExpenses.Add(data[i][_full_line]);
                    }
                    else if (data[i][_event_type] == "--")
                    {
                        personExpenses += getExpensesFromDay(data[i]);
                        listDescPersonExpenses.Add(data[i][_full_line]);
                    }
                }
            }
            resultDic["-"] = new string[1] { expenses.ToString() };
            resultDic["--"] = new string[1] { personExpenses.ToString() };
            resultDic["descExpenses"] = getResDesc(listDescExpenses);
            resultDic["descPersonExpenses"] = getResDesc(listDescPersonExpenses);
            return resultDic;
        }

        /// <summary>
        /// Возвращает словарь затрат за месяц
        ///     Ключи словаря: -, --, descExpenses, descPersonExpenses
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string[]> getExpensesMonth(string year, string month)
        {
            int expenses = 0;
            int personExpenses = 0;
            ClearLists();
            string date = getIntMonthFromString(month) + "/" + year;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i][_date_expense].IndexOf(date) != -1)
                {
                    if (data[i][_event_type] == "-")
                    {
                        expenses += getExpensesFromDay(data[i]);
                        listDescExpenses.Add(data[i][_full_line]);
                    }
                    if (data[i][_event_type] == "--")
                    {
                        personExpenses += getExpensesFromDay(data[i]);
                        listDescPersonExpenses.Add(data[i][_full_line]);
                    }
                }
            }
            resultDic["-"] = new string[1] { expenses.ToString() };
            resultDic["--"] = new string[1] { personExpenses.ToString() };
            resultDic["descExpenses"] = getResDesc(listDescExpenses);
            resultDic["descPersonExpenses"] = getResDesc(listDescPersonExpenses);
            return resultDic;
        }

        /// <summary>
        /// Возвращает словарь среднемесячной затраты
        ///     Ключи словаря -, --, descExpenses, descPersonExpenses
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string[]> getAVGExpenses()
        {
            int expenses = 0;
            int personExpenses = 0;
            ClearLists();
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i][_event_type] == "-")
                {
                    expenses += getExpensesFromDay(data[i]);
                    listDescExpenses.Add(data[i][_full_line]);
                }
                else if (data[i][_event_type] == "--")
                {
                    personExpenses += getExpensesFromDay(data[i]);
                    listDescPersonExpenses.Add(data[i][_full_line]);
                }
            }
            expenses = expenses / getMonths().Count;
            personExpenses = personExpenses / getMonths().Count;
            resultDic["-"] = new string[1] { expenses.ToString() };
            resultDic["--"] = new string[1] { personExpenses.ToString() };
            resultDic["descExpenses"] = getResDesc(listDescExpenses);
            resultDic["descPersonExpenses"] = getResDesc(listDescPersonExpenses);
            return resultDic;
        }

        /// <summary>
        ///     /// Возвращает информацию по сигаретам за все время
        ///     Ключи -, --, descExpenses, descPersonExpenses, count
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public Dictionary<string, string[]> getCigarettesAllTime()
        {
            int count = 0;
            int expenses = 0;
            int personExpenses = 0;
            ClearLists();
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i][_description].IndexOf("сиги") != -1)
                {
                    if (data[i][_event_type] == "-")
                    {
                        expenses += getExpensesFromDay(data[i]);
                        listDescExpenses.Add(data[i][_full_line]);
                        count += getCount(data[i][_count_expenses]);
                    }
                    else if (data[i][_event_type] == "--")
                    {
                        personExpenses += getExpensesFromDay(data[i]);
                        listDescPersonExpenses.Add(data[i][_full_line]);
                        count += getCount(data[i][_count_expenses]);
                    }
                }
            }
            resultDic["-"] = new string[1] { expenses.ToString() };
            resultDic["--"] = new string[1] { personExpenses.ToString() };
            resultDic["descExpenses"] = getResDesc(listDescExpenses);
            resultDic["descPersonExpenses"] = getResDesc(listDescPersonExpenses);
            resultDic["count"] = new string[1] { count.ToString() };
            return resultDic;
        }

        /// <summary>
        /// Возвращает словарь среднемесячной затраты
        ///     Ключи словаря -, --, descExpenses, descPersonExpenses
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string[]> getCigarettesAVG()
        {
            int expenses = 0;
            int personExpenses = 0;
            int count = 0;
            ClearLists();
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i][_description].IndexOf("сиги") != -1)
                {
                    if (data[i][_event_type] == "-")
                    {
                        expenses += getExpensesFromDay(data[i]);
                        listDescExpenses.Add(data[i][_full_line]);
                        count += getCount(data[i][_count_expenses]);
                    }
                    else if (data[i][_event_type] == "--")
                    {
                        personExpenses += getExpensesFromDay(data[i]);
                        listDescPersonExpenses.Add(data[i][_full_line]);
                        count += getCount(data[i][_count_expenses]);
                    }
                }
            }
            expenses = expenses / getMonths().Count;
            personExpenses = personExpenses / getMonths().Count;
            resultDic["-"] = new string[1] { expenses.ToString() };
            resultDic["--"] = new string[1] { personExpenses.ToString() };
            resultDic["count"] = new string[1] { (count / getMonths().Count).ToString() };
            resultDic["descExpenses"] = getResDesc(listDescExpenses);
            resultDic["descPersonExpenses"] = getResDesc(listDescPersonExpenses);
            return resultDic;
        }

        /// <summary>
        ///     /// Возвращает информацию по сигаретам за выбранный год
        ///     Ключи -, --, descExpenses, descPersonExpenses, count
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public Dictionary<string, string[]> getCigarettesYear(string year)
        {
            int count = 0;
            int expenses = 0;
            int personExpenses = 0;
            ClearLists();
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i][_date_expense].IndexOf(year) != -1)
                {
                    if (data[i][_description].IndexOf("сиги") != -1)
                    {
                        if (data[i][_event_type] == "-")
                        {
                            expenses += getExpensesFromDay(data[i]);
                            listDescExpenses.Add(data[i][_full_line]);
                            count += getCount(data[i][_count_expenses]);
                        }
                        else if (data[i][_event_type] == "--")
                        {
                            personExpenses += getExpensesFromDay(data[i]);
                            count += getCount(data[i][_count_expenses]);
                            listDescPersonExpenses.Add(data[i][_full_line]);
                        }
                    }
                }
            }
            resultDic["-"] = new string[1] { expenses.ToString() };
            resultDic["--"] = new string[1] { personExpenses.ToString() };
            resultDic["descExpenses"] = getResDesc(listDescExpenses);
            resultDic["descPersonExpenses"] = getResDesc(listDescPersonExpenses);
            resultDic["count"] = new string[1] { count.ToString() };
            return resultDic;
        }

        /// <summary>
        ///     /// Возвращает информацию по сигаретам за выбранный месяц
        ///     Ключи -, --, descExpenses, descPersonExpenses, count
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public Dictionary<string, string[]> getCigarettesMonth(string year, string month)
        {
            int count = 0;
            int expenses = 0;
            int personExpenses = 0;
            ClearLists();
            string date = getIntMonthFromString(month) + "/" + year;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i][_date_expense].IndexOf(date) != -1)
                {
                    if (data[i][_description].IndexOf("сиги") != -1)
                    {
                        if (data[i][_event_type] == "-")
                        {
                            expenses += getExpensesFromDay(data[i]);
                            count += getCount(data[i][_count_expenses]);
                            listDescExpenses.Add(data[i][_full_line]);
                        }
                        else if (data[i][_event_type] == "--")
                        {
                            personExpenses += getExpensesFromDay(data[i]);
                            count += getCount(data[i][_count_expenses]);
                            listDescPersonExpenses.Add(data[i][_full_line]);
                        }
                    }
                }
            }
            resultDic["-"] = new string[1] { expenses.ToString() };
            resultDic["--"] = new string[1] { personExpenses.ToString() };
            resultDic["descExpenses"] = getResDesc(listDescExpenses);
            resultDic["descPersonExpenses"] = getResDesc(listDescPersonExpenses);
            resultDic["count"] = new string[1] { count.ToString() };
            return resultDic;
        }


        /// <summary>
        /// Возвращает словарь по затратам на свечи
        ///     Ключи expenses, count, descExpenses
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string[]> getCandles()
        {
            int expenses = 0;
            int count = 0;
            ClearLists();
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i][_description].IndexOf("свечи") != -1)
                {
                    expenses += getExpensesFromDay(data[i]);
                    listDescExpenses.Add(data[i][_full_line]);
                    count += getCount(data[i][_count_expenses]);
                }
            }
            resultDic["expenses"] = new string[1] { expenses.ToString() };
            resultDic["count"] = new string[1] { count.ToString() };
            resultDic["descExpenses"] = getResDesc(listDescExpenses);
            return resultDic;
        }

        /// <summary>
        /// Возвращает словарь по затратам на спокойствие
        ///     Ключи expenses, count, descExpenses
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string[]> getRelax()
        {
            int expenses = 0;
            int count = 0;
            ClearLists();
            for(int i=0;i<data.Count;i++)
            {
                if(data[i][_description].IndexOf("спокойствие") != -1)
                {
                    expenses += getExpensesFromDay(data[i]);

                    int idx = data[i][_date_expense].IndexOf(' ');
                    string date = data[i][_date_expense].Remove(idx);

                    listDescExpenses.Add(date + " - " + data[i][_count]);
                    count += getCount(data[i][_count_expenses]);
                }
            }
            resultDic["expenses"] = new string[1] { expenses.ToString() };
            resultDic["count"] = new string[1] { count.ToString() };
            resultDic["descExpenses"] = getResDesc(listDescExpenses);
            return resultDic;
        }

        public String getStringMonthFromInt(object month)
        {
            if ((string)month == "01")
                return "Январь";
            else if ((string)month == "02")
                return "Февраль";
            else if ((string)month == "03")
                return "Март";
            else if ((string)month == "04")
                return "Апрель";
            else if ((string)month == "05")
                return "Май";
            else if ((string)month == "06")
                return "Июнь";
            else if ((string)month == "07")
                return "Июль";
            else if ((string)month == "08")
                return "Август";
            else if ((string)month == "09")
                return "Сентябрь";
            else if ((string)month == "10")
                return "Октябрь";
            else if ((string)month == "11")
                return "Ноябрь";
            else if ((string)month == "12")
                return "Декабрь";
            else return "";
        }

        public String getIntMonthFromString(object month)
        {
            if ((string)month == "Январь")
                return "01";
            else if ((string)month == "Февраль")
                return "02";
            else if ((string)month == "Март")
                return "03";
            else if ((string)month == "Апрель")
                return "04";
            else if ((string)month == "Май")
                return "05";
            else if ((string)month == "Июнь")
                return "06";
            else if ((string)month == "Июль")
                return "07";
            else if ((string)month == "Август")
                return "08";
            else if ((string)month == "Сентябрь")
                return "09";
            else if ((string)month == "Октябрь")
                return "10";
            else if ((string)month == "Ноябрь")
                return "11";
            else if ((string)month == "Декабрь")
                return "12";
            else return "88";
        }

        private Int32 getExpensesFromDay(Dictionary<string, string> day)
        {
            if (day[_count_expenses] == "0")
                return Convert.ToInt32(day[_count]);
            else
                return Convert.ToInt32(day[_count]) * Convert.ToInt32(day[_count_expenses]);
        }

        private String[] getResDesc(List<string> listDesc)
        {
            string[] res = new string[listDesc.Count];
            for (int i = 0; i < listDesc.Count; i++)
                res[i] = listDesc[i];
            return res;
        }

        private void ClearLists()
        {
            listDescExpenses.Clear();
            listDescPersonExpenses.Clear();
        }

        private Int32 getCount(string count_expenses)
        {
            if (count_expenses == "0")
                return 1;
            else
                return Convert.ToInt32(count_expenses);
        }

    }
}
