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
    public partial class Form_Chart : Form
    {
        private Mode mode;
        private ParseDBCasualTables pdbCasual;
        private ParseDBBigTables pdbBig;
        private List<string> spezialParams;
        private int year;

        private enum Mode
        {
            Casual,
            Big
        }


        public Form_Chart(ParseDBCasualTables _pdbCasual, ParseDBBigTables _pdbBig, int _mode, List<string> _spezialParams, decimal _year)
        {
            //Настройка для вычесления затрат
            pdbCasual = _pdbCasual;
            pdbBig = _pdbBig;
            if (_mode == 0)
                mode = Mode.Casual;
            else
                mode = Mode.Big;
            spezialParams = _spezialParams;
            year = Convert.ToInt32(_year);

            InitializeComponent();

            //Настройка для панели диаграммы
            chartCurrentYear.ChartAreas[0].AxisX.ScaleView.Zoom(0, 12);
            chartCurrentYear.ChartAreas[0].CursorX.IsUserEnabled = true;
            chartCurrentYear.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chartCurrentYear.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chartCurrentYear.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chartCurrentYear.ChartAreas[0].CursorY.IsUserEnabled = true;
            chartCurrentYear.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chartCurrentYear.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chartCurrentYear.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;

            chartAllYears.ChartAreas[0].AxisX.ScaleView.Zoom(0, 12);
            chartAllYears.ChartAreas[0].CursorX.IsUserEnabled = true;
            chartAllYears.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chartAllYears.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chartAllYears.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

            chartAllYears.ChartAreas[0].CursorY.IsUserEnabled = true;
            chartAllYears.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chartAllYears.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chartAllYears.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;

            textBoxListMonths.ReadOnly = true;
        }

        private void Form_Chart_Load(object sender, EventArgs e)
        {
            if (mode == Mode.Casual)
            {
                List<int> years = pdbCasual.getYears();
                for(int i =0;i<years.Count;i++)
                {
                    List<string> months = pdbCasual.getMonths(years[i]);
                    for(int j=0;j<months.Count;j++)
                    {
                        if(years[i] == year)
                        {
                            textBoxListMonths.Text += months[j] + Environment.NewLine;
                            chartCurrentYear.Series[0].Points.AddXY(months[j], pdbCasual.getExpensesMonth(spezialParams, year.ToString(), months[j])["-"][0]);
                        }
                        textBoxListMonthsAll.Text += months[j] + years[i].ToString() + Environment.NewLine;
                        chartAllYears.Series[0].Points.AddXY(months[j] + years[i].ToString(), pdbCasual.getExpensesMonth(spezialParams, years[i].ToString(), months[j])["-"][0]);
                    }
                }
            }
            else
            {
                int first_year = pdbBig.getFirstYear;
                int last_year = pdbBig.getLastYear;
                List<int> years = new List<int>();
                while(first_year <= last_year)
                {
                    years.Add(first_year);
                    first_year++;
                }
                for (int i = 0; i < years.Count; i++)
                {
                    List<string> months = pdbBig.getMonths(years[i].ToString());
                    for (int j = 0; j < months.Count; j++)
                    {
                        if (years[i] == year)
                        {
                            textBoxListMonths.Text += months[j] + Environment.NewLine;
                            chartCurrentYear.Series[0].Points.AddXY(months[j], pdbBig.Month(year.ToString(), months[j])["-"][0]);
                        }
                        textBoxListMonthsAll.Text += months[j] + years[i].ToString() + Environment.NewLine;
                        chartAllYears.Series[0].Points.AddXY(months[j], pdbBig.Month(years[j].ToString(), months[j])["-"][0]);
                    }
                }
            }
        }
    }
}
