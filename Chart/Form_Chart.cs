using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Libs;

namespace Chart
{
    /// <summary>
    /// 1. По дефолту пишем мин дату старта и макс дату завершения
    /// 2. расчет идет по месяцам
    /// 3. если запросы тяжелые, делаем кеширование
    /// </summary>
    public partial class Form_Chart : Form
    {
        private DB_sqlite db = new DB_sqlite();
        private ParseDBCasualTables pdbCasual;
        private ParseDBBigTables pdbBig;

        private DB.Tables activeTable;
        private List<string> spezialParams;
        
        public Form_Chart(List<string> _spezialParams, DB.Tables _activeTable)
        {
            spezialParams = _spezialParams;
            activeTable = _activeTable;
            InitializeComponent();
        }

        private void Form_Chart_Load(object sender, EventArgs e)
        {
            //Инициализируем пришедшие с другой формы параметры
            foreach (string param in spezialParams)
            {
                if (param == "") checkBoxWithoutParams.Checked = true;
                if (param == "т") checkBoxT.Checked = true;
                if (param == "н") checkBoxN.Checked = true;
                if (param == "б") checkBoxB.Checked = true;
                if (param == "она") checkBoxShe.Checked = true;
                if (param == "к") checkBoxK.Checked = true;
            }
            if (activeTable == DB.Tables.He) radioButtonModeHe.Checked = true;
            else if (activeTable == DB.Tables.HeBig) radioButtonModeHeBig.Checked = true;
            else if (activeTable == DB.Tables.HeGifts) radioButtonModeHeGifts.Checked = true;
            else if (activeTable == DB.Tables.She) radioButtonModeShe.Checked = true;
            else if (activeTable == DB.Tables.SheBig) radioButtonModeSheBig.Checked = true;

            //Задаем рамки временного запроса 
            cbMonthsStart.Items.Clear();
            List<string> months = new List<string>();
            if (radioButtonModeHe.Checked || radioButtonModeShe.Checked)
            {
                pdbCasual = new ParseDBCasualTables(db.GetData(activeTable));
                List<int> years = pdbCasual.getYears();
                numericYearsStart.Minimum = years[0];
                numericYearsStart.Maximum = years[years.Count - 1];
                numericYearsStop.Minimum = years[0];
                numericYearsStop.Maximum = years[years.Count - 1];
                months = pdbCasual.getMonths(Convert.ToInt32(numericYearsStart.Value));   
            }
            else
            {
                pdbBig = new ParseDBBigTables(db.GetData(activeTable));
                numericYearsStart.Minimum = pdbBig.getFirstYear;
                numericYearsStart.Maximum = pdbBig.getLastYear;
                numericYearsStop.Minimum = pdbBig.getFirstYear;
                numericYearsStop.Maximum = pdbBig.getLastYear;
                months = pdbBig.getMonths(numericYearsStart.Value.ToString());
            }
            for (int i = 0; i < months.Count; i++)
            {
                cbMonthsStart.Items.Add(months[i]);
            }
            cbMonthsStart.SelectedItem = months[0];
            if (months.Count > 0) cbMonthsStop.SelectedItem = months[1];
            else cbMonthsStop.SelectedItem = months[1];
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            //1. Убедиться, что при загрузке отобразились правильные периоды запроса 
            //2. Подтягиваем временной период и расчитываем по месяцам
            //3. Добавить возможность расчета по годам
            //4. Продумать на количеством диаграм, попробовать програмно задавать тип диаграммы.
            //
        }
    }
}
