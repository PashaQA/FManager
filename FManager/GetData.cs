using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FManager
{
    /// <summary>
    /// Возвращает разлиные запросы по затратам
    /// </summary>
    public class GetData
    {
        private string month;
        private string year;
        private string[] fullList;
        private int expenses;
        private int expensesAllTimes;
        private int profitSum;
        private int expensesParty;
        private int expensesPartyAllTimes;
        private int expensesMust;
        private int expensesMustAllTimes;
        private int expensesPersonSum;
        private int expensesCigarettesSum;
        private int expensesCigarettesSumAllTimes;
        private int cigarettesCount;
        private int cigarettesCountAllTimes;
        private int expensesShe;
        private int expensesSheAllTimes;
        private int expensesCandle;
        private int expensesRelax;
        private bool hierogliph;
        private List<string> daysList = new List<string>();
        private List<string> errorSumList = new List<string>();
        private List<string> profitList = new List<string>();
        private List<string> expensesPartyList = new List<string>();
        private List<string> expensesPartyListAllTimes = new List<string>();
        private List<string> expensesMustList = new List<string>();
        private List<string> expensesMustListAllTimes = new List<string>();
        private List<string> months = new List<string>();
        private List<string> expensesPersonList = new List<string>();
        private List<string> expensesCigarettesList = new List<string>();
        private List<string> expensesCigarettesListAllTimes = new List<string>();
        private List<string> expensesSheList = new List<string>();
        private List<string> expensesSheListAllTimes = new List<string>();
        private List<string> expensesCandleList = new List<string>();
        private List<string> expensesRelaxList = new List<string>();
        private SetData set = new SetData();

        public GetData(string[] fullList, string month, string year, RadioButton he)
        {
            expenses = 0;
            expensesAllTimes = 0;
            profitSum = 0;
            expensesParty = 0;
            expensesPartyAllTimes = 0;
            expensesMust = 0;
            expensesMustAllTimes = 0;
            expensesPersonSum = 0;
            expensesCigarettesSum = 0;
            expensesCigarettesSumAllTimes = 0;
            cigarettesCount = 0;
            cigarettesCountAllTimes = 0;
            expensesShe = 0;
            this.month = month;
            this.year = year;
            this.fullList = fullList;
            hierogliph = IsHierogliph(set.locationCurrentFile);
            getDaysFromMonth();
            getExpensesFromMonth();
            getExpensesFromMonth(true);
            if (!he.Checked)
            {
                getProfitDaysFromMonth();
                getProfitSumFromMonth();
            }
            getExpensesDaysFromMonthParty();
            getExpensesDaysFromMonthParty(true);
            getExpensesSumFromMonthParty();
            getExpensesSumFromMonthParty(true);
            getExpensesDaysFromMonthMust();
            getExpensesDaysFromMonthMust(true);
            getExpensesSumFromMonthMust();
            getExpensesSumFromMonthMust(true);
            getMonths();
            if (!he.Checked)
            {
                getExpensesPersonList();
                getExpensesPersonSum();
                getExpensesCigarettesList();
                getExpensesCigarettesList(true);
                getExpensesCigarettesSum(ref cigarettesCount, ref expensesCigarettesSum);
                getExpensesCigarettesSum(ref cigarettesCountAllTimes, ref expensesCigarettesSumAllTimes, true);
            }
            if (he.Checked)
            {
                getExpensesDaysFromMonthShe();
                getExpensesDaysFromMonthShe(true);
                getExpensesSumFromMonthShe();
                getExpensesSumFromMonthShe(true);
                getExpensesRelaxList();
                getExpensesRelaxSum();
                getExpensesCandleList();
                getExpensesCandleSum();
            }
        }

        private Boolean IsHierogliph(string filename)
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

        private List<string> getDaysFromMonth()
        {
            int indexStart = 0;
            for (int i = 0; i < fullList.Length; i++)
            {
                if (fullList[i].EndsWith(month + " " + year))
                {
                    indexStart = i;
                    break;
                }
            }
            for (int i = indexStart + 1; i < fullList.Length; i++)
            {
                if (fullList[i].StartsWith("----"))
                    break;
                daysList.Add(fullList[i]);
            }
            return daysList;
        }

        private Int32 getExpensesFromMonth(bool year = false)
        {
            List<string> daysListLocal = new List<string>();
            if (year)
            {
                for (int i = 0; i < fullList.Length; i++)
                    daysListLocal.Add(fullList[i]);
            }
            else
            {
                for (int i = 0; i < daysList.Count; i++)
                    daysListLocal.Add(daysList[i]);
            }
            foreach (var day in daysListLocal)
            {
                if (day.StartsWith("------"))
                    continue;
                string parsed = day;
                int counter = 0;
                int multiple = 0;
                string[] importMulti = null;

                foreach (var symbols in parsed)
                {
                    if (symbols == '-')
                        counter++;
                }
                if (counter <= 1)
                {
                    errorSumList.Add(parsed);
                    continue;
                }

                if (parsed.EndsWith("(т)") || parsed.EndsWith("(н)") || parsed.EndsWith("(она)"))
                {
                    int indexParam = parsed.LastIndexOf(" ");
                    parsed = parsed.Remove(indexParam);
                }
                importMulti = parsed.Split(' ');
                foreach (var r in importMulti)
                {
                    if (r.StartsWith("x") || r.StartsWith("х"))
                    {
                        if (r.Length <= 3)
                        {
                            multiple = Convert.ToInt32(r.Remove(0, 1));
                            break;
                        }
                    }
                }

                int indexMoney = parsed.LastIndexOf(' ');
                try
                {

                    int sum = Convert.ToInt32(parsed.Remove(0, indexMoney + 1));
                    if (multiple != 0)
                        sum = sum * multiple;
                    if (year)
                        expensesAllTimes += sum;
                    else
                    {
                        expenses += sum;
                    }
                }
                catch (Exception)
                {
                    errorSumList.Add(parsed);
                }
            }
            if (year)
                return expenses;
            else
                return expensesAllTimes;
        }

        private List<string> getProfitDaysFromMonth()
        {
            for (int i = 0; i < daysList.Count; i++)
            {
                for (int j = 0; j < daysList[i].Length; j++)
                {
                    if (daysList[i][j] == '+')
                        profitList.Add(daysList[i]);
                }
            }
            return profitList;
        }

        private Int32 getProfitSumFromMonth()
        {
            foreach (var r in profitList)
            {
                int index = r.IndexOf('+');
                profitSum += Convert.ToInt32(r.Remove(0, index + 1));
            }
            return profitSum;
        }

        private List<string> getExpensesDaysFromMonthParty(bool year = false)
        {
            if (year)
            {
                for (int i = 0; i < fullList.Length; i++)
                {
                    if (fullList[i].EndsWith("(т)"))
                        expensesPartyListAllTimes.Add(fullList[i]);
                }
                return expensesPartyListAllTimes;
            }
            else
            {
                for (int i = 0; i < daysList.Count; i++)
                {
                    if (daysList[i].EndsWith("(т)"))
                        expensesPartyList.Add(daysList[i]);
                }
                return expensesPartyList;
            }
        }

        private Int32 getExpensesSumFromMonthParty(bool year = false)
        {
            if (year)
            {
                for (int i = 0; i < expensesPartyListAllTimes.Count; i++)
                {
                    int idx = expensesPartyListAllTimes[i].LastIndexOf(' ');
                    string line = expensesPartyListAllTimes[i].Remove(idx);
                    idx = line.LastIndexOf(' ');
                    expensesPartyAllTimes += Convert.ToInt32(line.Remove(0, idx + 1));
                }
                return expensesPartyAllTimes;
            }
            else
            {
                for (int i = 0; i < expensesPartyList.Count; i++)
                {
                    int idx = expensesPartyList[i].LastIndexOf(' ');
                    string line = expensesPartyList[i].Remove(idx);
                    idx = line.LastIndexOf(' ');
                    expensesParty += Convert.ToInt32(line.Remove(0, idx + 1));
                }
                return expensesParty;
            }
        }

        private List<string> getExpensesDaysFromMonthMust(bool year = false)
        {
            if (year)
            {
                for (int i = 0; i < fullList.Length; i++)
                {
                    if (fullList[i].EndsWith("(н)"))
                        expensesMustListAllTimes.Add(fullList[i]);
                }
                return expensesMustListAllTimes;
            }
            else
            {
                for (int i = 0; i < daysList.Count; i++)
                {
                    if (daysList[i].EndsWith("(н)"))
                        expensesMustList.Add(daysList[i]);
                }
                return expensesMustList;
            }
        }

        private Int32 getExpensesSumFromMonthMust(bool year = false)
        {
            if (year)
            {
                for (int i = 0; i < expensesMustListAllTimes.Count; i++)
                {
                    int idx = expensesMustListAllTimes[i].LastIndexOf(' ');
                    string line = expensesMustListAllTimes[i].Remove(idx);
                    idx = line.LastIndexOf(' ');
                    expensesMustAllTimes += Convert.ToInt32(line.Remove(0, idx + 1));
                }
                return expensesMustAllTimes;
            }
            else
            {
                for (int i = 0; i < expensesMustList.Count; i++)
                {
                    int idx = expensesMustList[i].LastIndexOf(' ');
                    string line = expensesMustList[i].Remove(idx);
                    idx = line.LastIndexOf(' ');
                    expensesMust += Convert.ToInt32(line.Remove(0, idx + 1));
                }
                return expensesMust;
            }
        }

        private List<string> getMonths()
        {
            for (int i = 0; i < fullList.Length; i++)
            {
                if (fullList[i].StartsWith("-------") && !fullList[i].EndsWith("----"))
                {
                    int idx = fullList[i].LastIndexOf('-');
                    months.Add(fullList[i].Remove(0, idx + 1));
                }
            }
            return months;
        }

        private List<string> getExpensesPersonList()
        {
            for (int i = 0; i < daysList.Count; i++)
            {
                int idx = daysList[i].LastIndexOf("-- ");
                if (idx != -1)
                    expensesPersonList.Add(daysList[i]);
            }
            return expensesPersonList;
        }

        private Int32 getExpensesPersonSum()
        {
            foreach (var day in expensesPersonList)
            {
                string parsed = day;
                int multiple = 0;
                string[] importMulti = null;

                if (parsed.EndsWith("(т)") || parsed.EndsWith("(н)"))
                {
                    int indexParam = parsed.LastIndexOf(" ");
                    parsed = parsed.Remove(indexParam);
                }

                importMulti = parsed.Split(' ');
                foreach (var multi in importMulti)
                {
                    if (multi.StartsWith("x") || multi.StartsWith("х"))
                    {
                        if (multi.Length <= 3)
                        {
                            multiple = Convert.ToInt32(multi.Remove(0, 1));
                            break;
                        }
                    }
                }

                int indexMoney = parsed.LastIndexOf(' ');
                int sum = Convert.ToInt32(parsed.Remove(0, indexMoney + 1));
                if (multiple != 0)
                    sum = sum * multiple;
                expensesPersonSum += sum;
            }
            return expensesPersonSum;
        }

        private List<string> getExpensesCigarettesList(bool year = false)
        {
            if (year)
            {
                for (int i = 0; i < fullList.Length; i++)
                {
                    int idx = 0;
                    idx = fullList[i].IndexOf("сиги");
                    if (idx != -1)
                    {
                        expensesCigarettesListAllTimes.Add(fullList[i]);
                    }
                }
                return expensesCigarettesListAllTimes;
            }
            for (int i = 0; i < daysList.Count; i++)
            {
                int idx = daysList[i].IndexOf("сиги");
                if (idx != -1)
                {
                    expensesCigarettesList.Add(daysList[i]);
                }
            }
            return expensesCigarettesList;
        }

        private Int32 getExpensesCigarettesSum(ref int cigarettesCount, ref int expensesCigarettesSum, bool year = false)
        {
            List<string> days = new List<string>();
            if (year)
            {
                days = expensesCigarettesListAllTimes;
            }
            else
            {
                days = expensesCigarettesList;
            }
            for (int i = 0; i < days.Count; i++)
            {
                int multiple = 0;
                int sum = 0;
                string[] importMulti = days[i].Split(' ');
                foreach (var r in importMulti)
                {
                    if (r.StartsWith("x") || r.StartsWith("х"))
                    {
                        if (r.Length <= 3)
                        {
                            multiple = Convert.ToInt32(r.Remove(0, 1));
                            break;
                        }
                    }
                }
                int idx = days[i].LastIndexOf(' ');
                sum = Convert.ToInt32(days[i].Remove(0, idx + 1));
                if (multiple != 0)
                {
                    sum += sum * multiple;
                    cigarettesCount += multiple;
                }
                else
                    cigarettesCount++;
                expensesCigarettesSum += sum;
            }
            return expensesCigarettesSum;
        }

        private List<string> getExpensesDaysFromMonthShe(bool year = false)
        {
            if (year)
            {
                for (int i = 0; i < fullList.Length; i++)
                {
                    if (fullList[i].EndsWith("(она)"))
                        expensesSheListAllTimes.Add(fullList[i]);
                }
                return expensesSheListAllTimes;
            }
            else
            {
                for (int i = 0; i < daysList.Count; i++)
                {
                    if (daysList[i].EndsWith("(она)"))
                        expensesSheList.Add(daysList[i]);
                }
                return expensesSheList;
            }
        }

        private Int32 getExpensesSumFromMonthShe(bool year = false)
        {
            if (year)
            {
                for (int i = 0; i < fullList.Length; i++)
                {
                    if (fullList[i].EndsWith("(она)"))
                    {
                        int idxParam = fullList[i].LastIndexOf(' ');
                        string day = fullList[i].Remove(idxParam);
                        idxParam = day.LastIndexOf(' ');
                        expensesSheAllTimes += Convert.ToInt32(day.Remove(0, idxParam + 1));
                    }

                }
                return expensesSheAllTimes;
            }
            else
            {
                for (int i = 0; i < daysList.Count; i++)
                {
                    if (daysList[i].EndsWith("(она)"))
                    {
                        int idxParam = daysList[i].LastIndexOf(' ');
                        string day = daysList[i].Remove(idxParam);
                        idxParam = day.LastIndexOf(' ');
                        expensesShe += Convert.ToInt32(day.Remove(0, idxParam + 1));
                    }

                }
                return expensesShe;
            }
        }

        private List<string> getExpensesRelaxList()
        {
            for (int i = 0; i < fullList.Length; i++)
            {
                char[] symbols = fullList[i].ToCharArray();
                for (int j = 0; j < symbols.Length; j++)
                {
                    if (symbols[j] == 'с' && symbols[j + 1] == 'п' && symbols[j + 2] == 'о'
                        && symbols[j + 3] == 'к' && symbols[j + 4] == 'о' && symbols[j + 5] == 'й'
                         && symbols[j + 6] == 'с' && symbols[j + 7] == 'т' && symbols[j + 8] == 'в'
                         && symbols[j + 9] == 'и' && symbols[j + 10] == 'е')
                    {
                        expensesRelaxList.Add(fullList[i]);
                        break;
                    }
                }
            }
            return expensesRelaxList;
        }

        private Int32 getExpensesRelaxSum()
        {
            for (int i = 0; i < expensesRelaxList.Count; i++)
            {
                char[] symbols = expensesRelaxList[i].ToCharArray();
                for (int j = 0; j < symbols.Length; j++)
                {
                    if (symbols[j] == 'с' && symbols[j + 1] == 'п' && symbols[j + 2] == 'о'
                        && symbols[j + 3] == 'к' && symbols[j + 4] == 'о' && symbols[j + 5] == 'й'
                         && symbols[j + 6] == 'с' && symbols[j + 7] == 'т' && symbols[j + 8] == 'в'
                         && symbols[j + 9] == 'и' && symbols[j + 10] == 'е')
                    {
                        int idxParam = 0;
                        if (expensesRelaxList[i].IndexOf("(н)") != -1)
                        {
                            idxParam = expensesRelaxList[i].LastIndexOf('-');
                            string value = expensesRelaxList[i].Remove(0, idxParam + 2);
                            idxParam = value.LastIndexOf(' ');
                            expensesRelax += Convert.ToInt32(value.Remove(idxParam));
                        }
                        else
                        {
                            idxParam = expensesRelaxList[i].LastIndexOf('-');
                            expensesRelax += Convert.ToInt32(expensesRelaxList[i].Remove(0, idxParam + 2));
                        }
                    }
                }
            }
            return expensesRelax;
        }

        private List<string> getExpensesCandleList()
        {
            for (int i = 0; i < fullList.Length; i++)
            {
                char[] symbols = fullList[i].ToCharArray();
                for (int j = 0; j < symbols.Length; j++)
                {
                    if (symbols[j] == 'с' && symbols[j + 1] == 'в' && symbols[j + 2] == 'е'
                        && symbols[j + 3] == 'ч' && symbols[j + 4] == 'и')
                    {
                        expensesCandleList.Add(fullList[i]);
                        break;
                    }
                }
            }
            return expensesCandleList;
        }

        private Int32 getExpensesCandleSum()
        {
            for (int i = 0; i < expensesCandleList.Count; i++)
            {
                int multiple = 0;
                int sum = 0;
                string[] importMulti = expensesCandleList[i].Split(' ');
                foreach (var r in importMulti)
                {
                    if (r.StartsWith("x") || r.StartsWith("х"))
                    {
                        if (r.Length <= 3)
                        {
                            multiple = Convert.ToInt32(r.Remove(0, 1));
                            break;
                        }
                    }
                }
                int idxParam = expensesCandleList[i].LastIndexOf(' ');
                sum = Convert.ToInt32(expensesCandleList[i].Remove(0, idxParam + 1));
                if (multiple != 0)
                    sum = sum * multiple;
                expensesCandle += sum;
            }
            return expensesCandle;
        }

        /// <summary>
        /// Воздвращает true, если файл содержит иероглифы
        /// </summary>
        public bool IsHierogliphCurrentFile
        {
            get
            {
                return hierogliph;
            }
        }

        /// <summary>
        /// Возвращает true, если файл содержит иероглифы
        /// </summary>
        public bool IsHierogliphNewFile
        {
            get
            {
                return IsHierogliph(set.locationNewFile);
            }
        }

        public class PrintData : GetData
        {
            public readonly List<string> current_months = new List<string>();

            public PrintData(string[] fullList, string month, string year, RadioButton he)
                : base(fullList, month, year, he)
            {
                current_months = months;
            }

            private void IsNull(TextBox textbox, List<string> list)
            {
                if (list.Count == 0)
                    textbox.Text = "В выбранном месяце отсутствует информация по данному параметру";
            }

            private void IsNull(TextBox textbox, int sum)
            {
                if (sum == 0)
                    textbox.Text = "В выбранном месяце отсутствует информация по данному параметру";
            }

            /// <summary>
            /// Выводит на экран данные, которые не были посчитаны в момент подстеча затрат
            /// </summary>
            public void ErrorSum(TextBox textbox)
            {
                textbox.Clear();
                textbox.Text += "Не посчитанные данные растрат:" + Environment.NewLine;
                foreach (var errors in errorSumList)
                    textbox.Text += errors;
            }

            /// <summary>
            /// Выводит на экран только перечень дней выбранного месяца.
            /// </summary>
            public void DaysFromMonth(TextBox textbox)
            {
                textbox.Clear();
                for (int i = 0; i < daysList.Count; i++)
                    textbox.Text += daysList[i] + Environment.NewLine;
            }

            /// <summary>
            /// Выводит на экран сумму потраченых денег в определенный месяц
            /// </summary>
            public void Expenses(TextBox textbox)
            {
                textbox.Clear();
                textbox.Text = "В месяце " + month.ToLower() + " " + year +
                    " года было потрачено: " + expenses + " рублей";
                IsNull(textbox, expenses);
            }

            /// <summary>
            /// Выводит на экран список дней с прибылью
            /// </summary>
            /// <param name="textbox"></param>
            public void ProfitList(TextBox textbox)
            {
                textbox.Clear();
                for (int i = 0; i < profitList.Count; i++)
                    textbox.Text += profitList[i] + Environment.NewLine;
                IsNull(textbox, profitList);
            }

            /// <summary>
            /// Выводит на экран суммарное количество прибыли
            /// </summary>
            public void ProfitSum(TextBox textbox)
            {
                textbox.Clear();
                textbox.Text = "В месяце " + month.ToLower() + " " + year +
                    " года было получено: " + profitSum + " рублей";
                IsNull(textbox, profitSum);
            }

            /// <summary>
            /// Выводит на экран список всех дней с затратами по параметру (т) - тусы
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesPartyList(TextBox textbox)
            {
                textbox.Clear();
                for (int i = 0; i < expensesPartyList.Count; i++)
                    textbox.Text += expensesPartyList[i] + Environment.NewLine;
                IsNull(textbox, expensesPartyList);
            }

            /// <summary>
            /// Выводит на экран суммарное количество затрат по параметру (т) - Тусы.
            /// </summary>
            public void ExpensesPartySum(TextBox textbox)
            {
                textbox.Clear();
                textbox.Text = "В месяце " + month.ToLower() + " " + year +
                    " года было потрачено: " + expensesParty + " рублей";
                IsNull(textbox, expensesParty);
            }

            /// <summary>
            /// Выводит на экран список всех дней с затратами по параметру (н) - необходимые затраты
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesMustList(TextBox textbox)
            {
                textbox.Clear();
                for (int i = 0; i < expensesMustList.Count; i++)
                    textbox.Text += expensesMustList[i] + Environment.NewLine;
                IsNull(textbox, expensesMustList);
            }

            /// <summary>
            /// Выводит на экран суммарное количество затрат по параметру (т) - тусы.
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesMustSum(TextBox textbox)
            {
                textbox.Clear();
                textbox.Text += "В месяце " + month.ToLower() + " " + year +
                    " года было потрачено: " + expensesMust + " рублей";
                IsNull(textbox, expensesMust);
            }

            /// <summary>
            /// Выводит на экран список всех затрат за свои счет
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesPersonList(TextBox textbox)
            {
                textbox.Clear();
                for (int i = 0; i < expensesPersonList.Count; i++)
                    textbox.Text += expensesPersonList[i] + Environment.NewLine;
                IsNull(textbox, expensesPersonList);
            }

            /// <summary>
            /// Выводить на экран суммарное количество затрат за свой счет
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesPersonSum(TextBox textbox)
            {
                textbox.Clear();
                textbox.Text = "В месяце " + month.ToLower() + " " + year +
                    " года своих денег было потрачено: " + expensesPersonSum + " рублей";
                IsNull(textbox, expensesPersonSum);
            }

            /// <summary>
            /// Выводит на экран результат накопления или затраты в зависимости от (накопления - затраты)
            /// </summary>
            /// <param name="textbox"></param>
            public void ProfitAfterExpenses(TextBox textbox)
            {
                int result = profitSum - expensesPersonSum;
                textbox.Clear();
                if (result > 0)
                    textbox.Text = "В месяце " + month.ToLower() + " " + year +
                        " года, исходя из накопленных денег и потраченых денег, " +
                        "в результате за месяц у тебя накопилось: " + result + " рублей.";
                else
                    textbox.Text = "В месяце " + month.ToLower() + " " + year +
                        " года, исходя из накопленных денег и потраченых денег, " +
                        "в результате за месяц ты ушла в минус на: " + result + " рублей.";
                textbox.Text += Environment.NewLine;
                textbox.Text += "Подсчеты:" + Environment.NewLine;
                textbox.Text += "Накоплено: " + profitSum + Environment.NewLine;
                textbox.Text += "Потрачено: " + expenses + Environment.NewLine;
                textbox.Text += "Из всех потраченных, свои денех было потрачено: " + expensesPersonSum +
                    Environment.NewLine;
            }

            /// <summary>
            /// Выводит на экран список всех дней с затратами на сиги выбранного месяца
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesCigarettesList(TextBox textbox)
            {
                textbox.Clear();
                for (int i = 0; i < expensesCigarettesList.Count; i++)
                    textbox.Text += expensesCigarettesList[i] + Environment.NewLine;
                IsNull(textbox, expensesCigarettesList);
            }

            /// <summary>
            /// Выводит на экран суммарное количество затрат только на сигареты
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesCigarettesSum(TextBox textbox)
            {
                textbox.Clear();
                textbox.Text = "В месяце " + month.ToLower() + " " + year +
                    " года только на сигареты было потрачено: " + expensesCigarettesSum + " рублей";
                IsNull(textbox, expensesCigarettesSum);
            }

            /// <summary>
            /// Выводит на экран количество купленных пакет в выбранном месяце
            /// </summary>
            /// <param name="textbox"></param>
            public void CigarettesCount(TextBox textbox)
            {
                textbox.Clear();
                textbox.Text = "Было куплено пачек в количестве: " + cigarettesCount;
                IsNull(textbox, cigarettesCount);
            }

            /// <summary>
            /// Выводит на экран описание суммы растрат
            /// </summary>
            /// <param name="textbox"></param>
            public void AboutExpenses(TextBox textbox)
            {
                textbox.Clear();
                textbox.Text += "Баловство: " + (expenses - expensesParty - expensesMust - expensesPersonSum) + " pублей" +
                    Environment.NewLine;
                textbox.Text += "Затраты (т): " + expensesParty + " pублей" + Environment.NewLine;
                textbox.Text += "Затраты (н): " + expensesMust + " pублей" + Environment.NewLine;
                textbox.Text += "Затраты (--): " + expensesPersonSum + " pублец" + Environment.NewLine;
                textbox.Text += "Сиги: " + expensesCigarettesSum + " pублей" + Environment.NewLine;
                textbox.Text += "Количество пачек: " + cigarettesCount + " штук.";
            }

            /// <summary>
            /// Выводит на экран список дней с затратой на сигареты за год.
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesCigarettesListAllTimes(TextBox textbox)
            {
                textbox.Clear();
                for (int i = 0; i < expensesCigarettesListAllTimes.Count; i++)
                    textbox.Text += expensesCigarettesListAllTimes[i] + Environment.NewLine;
                IsNull(textbox, expensesCigarettesListAllTimes);
            }

            /// <summary>
            /// Выводит на экран суммарное количество затрат только на сигераты за год.
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesCigarettesSumAllTimes(TextBox textbox)
            {
                textbox.Clear();
                textbox.Text = "За " + year + " год была потрачена сумма на сигареты в размере: " + expensesCigarettesSumAllTimes +
                    " рублей.";
                IsNull(textbox, expensesCigarettesSumAllTimes);
            }

            /// <summary>
            /// Выводит на экран суммарное количество купленных пакет за год
            /// </summary>
            /// <param name="textbox"></param>
            public void CigarettesCountAllTimes(TextBox textbox)
            {
                textbox.Clear();
                textbox.Text = "За " + year + " год было куплено пачек сигарет в размере: " + cigarettesCountAllTimes +
                    " шт.";
                IsNull(textbox, cigarettesCountAllTimes);
            }

            /// <summary>
            /// Выводит на экран список затрат за месяц по параметру '(она)'
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesSheList(TextBox textbox)
            {
                textbox.Clear();
                for (int i = 0; i < expensesSheList.Count; i++)
                    textbox.Text += expensesSheList[i] + Environment.NewLine;
                IsNull(textbox, expensesSheList);
            }

            /// <summary>
            /// Выводит на экран сумму потраченый денег за текущий месяц по параметру '(она)'
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesSheSum(TextBox textbox)
            {
                textbox.Clear();
                textbox.Text += "В месяце " + month.ToLower() + " " + year +
                    " года только на НЕЁ было потрачено: " + expensesShe + " рублей";
                IsNull(textbox, expensesShe);
            }

            /// <summary>
            /// Выводит на экран список затрат за все время по параметру '(она)'
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesSheListAllTimes(TextBox textbox)
            {
                textbox.Clear();
                for (int i = 0; i < expensesSheListAllTimes.Count; i++)
                    textbox.Text += expensesSheListAllTimes[i] + Environment.NewLine;
                IsNull(textbox, expensesSheListAllTimes);
            }

            /// <summary>
            /// Выводит на экран сумму потраченый денег за все время по параметру '(она)'
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesSheSumAllTimes(TextBox textbox)
            {
                textbox.Clear();
                textbox.Text += "За все время только на НЕЁ было потрачено: " + expensesSheAllTimes + " рублей";
                IsNull(textbox, expensesShe);
            }

            /// <summary>
            /// Выводит на экран список затрат за все время
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesListAllTimes(TextBox textbox)
            {
                textbox.Clear();
                for (int i = 0; i < fullList.Length; i++)
                    textbox.Text += fullList[i] + Environment.NewLine;
            }

            /// <summary>
            /// Выводит на экран сумму затрат за все время
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesSumAllTimes(TextBox textbox)
            {
                textbox.Clear();
                textbox.Text += "За все время было потрачено: " + expensesAllTimes + " рублей";
                IsNull(textbox, expensesAllTimes);
            }

            /// <summary>
            /// Выводит на экран список затрат за все время по параметру '(т)'
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesPartyListAllTimes(TextBox textbox)
            {
                textbox.Clear();
                for (int i = 0; i < expensesPartyListAllTimes.Count; i++)
                    textbox.Text += expensesPartyListAllTimes[i] + Environment.NewLine;
                IsNull(textbox, expensesPartyListAllTimes);
            }

            /// <summary>
            /// Выводит на экран сумму затрат за все время по параметру '(т)'
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesPartySumAllTimes(TextBox textbox)
            {
                textbox.Clear();
                textbox.Text += "За все время только на тусовки было потрачено: " + expensesPartyAllTimes + " рублей";
                IsNull(textbox, expensesPartyAllTimes);
            }

            /// <summary>
            /// Выводит на экран список затрат за все время по параметру '(н)'
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesMustListAllTimes(TextBox textbox)
            {
                textbox.Clear();
                for (int i = 0; i < expensesMustListAllTimes.Count; i++)
                    textbox.Text += expensesMustListAllTimes[i] + Environment.NewLine;
                IsNull(textbox, expensesMustListAllTimes);
            }

            /// <summary>
            /// Выводит на экран сумму затрат за все время по параметру '(н)'
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesMustSumAllTimes(TextBox textbox)
            {
                textbox.Clear();
                textbox.Text += "За все время только на необходимые затраты  было потрачено: " + expensesMustAllTimes + " рублей";
                IsNull(textbox, expensesMust);
            }

            /// <summary>
            /// Выводит на экран список затрат за все время по ключевому слову 'спокойствие'
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesRelaxList(TextBox textbox)
            {
                textbox.Clear();
                for (int i = 0; i < expensesRelaxList.Count; i++)
                    textbox.Text += expensesRelaxList[i] + Environment.NewLine;
                IsNull(textbox, expensesRelaxList);
            }

            /// <summary>
            /// Выводит на экран сумму затрат за все время по ключевому слову 'спокойствие'
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesRelaxSum(TextBox textbox)
            {
                textbox.Clear();
                textbox.Text += "За все время только 'спокойствие'  было потрачено: " + expensesRelax + " рублей";
                IsNull(textbox, expensesRelax);
            }

            /// <summary>
            /// Выводит на экран список затрат за все время по ключевому слову 'свечи'
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesCandleList(TextBox textbox)
            {
                textbox.Clear();
                for (int i = 0; i < expensesCandleList.Count; i++)
                    textbox.Text += expensesCandleList[i] + Environment.NewLine;
                IsNull(textbox, expensesCandleList);
            }

            /// <summary>
            /// Выводит на экран сумму затрат за все время по ключевому слову 'свечи'
            /// </summary>
            /// <param name="textbox"></param>
            public void ExpensesCandleSum(TextBox textbox)
            {
                textbox.Clear();
                textbox.Text += "За все время только 'свечи'  было потрачено: " + expensesCandle + " рублей";
                IsNull(textbox, expensesCandle);
            }
        }

    }

}
