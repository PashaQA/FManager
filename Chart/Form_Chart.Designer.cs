namespace Chart
{
    partial class Form_Chart
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Chart));
            this.chartCurrentYear = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.textBoxListMonths = new System.Windows.Forms.TextBox();
            this.chartAllYears = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.textBoxListMonthsAll = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chartCurrentYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartAllYears)).BeginInit();
            this.SuspendLayout();
            // 
            // chartCurrentYear
            // 
            this.chartCurrentYear.BorderSkin.BorderWidth = 3;
            chartArea1.BorderWidth = 5;
            chartArea1.Name = "ChartArea1";
            this.chartCurrentYear.ChartAreas.Add(chartArea1);
            legend1.BorderWidth = 5;
            legend1.Font = new System.Drawing.Font("Modern No. 20", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend1";
            this.chartCurrentYear.Legends.Add(legend1);
            this.chartCurrentYear.Location = new System.Drawing.Point(12, 12);
            this.chartCurrentYear.Name = "chartCurrentYear";
            this.chartCurrentYear.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.IsXValueIndexed = true;
            series1.Legend = "Legend1";
            series1.LegendText = "Затраты по месяцам";
            series1.Name = "Series1";
            series1.YValuesPerPoint = 4;
            this.chartCurrentYear.Series.Add(series1);
            this.chartCurrentYear.Size = new System.Drawing.Size(845, 301);
            this.chartCurrentYear.TabIndex = 0;
            this.chartCurrentYear.Text = "chart1";
            title1.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title1.Name = "Title1";
            title1.Text = "Затраты за год";
            this.chartCurrentYear.Titles.Add(title1);
            // 
            // textBoxListMonths
            // 
            this.textBoxListMonths.Font = new System.Drawing.Font("Lucida Fax", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxListMonths.Location = new System.Drawing.Point(875, 13);
            this.textBoxListMonths.Multiline = true;
            this.textBoxListMonths.Name = "textBoxListMonths";
            this.textBoxListMonths.Size = new System.Drawing.Size(169, 300);
            this.textBoxListMonths.TabIndex = 1;
            // 
            // chartAllYears
            // 
            this.chartAllYears.BorderSkin.BorderWidth = 3;
            chartArea2.BorderWidth = 5;
            chartArea2.Name = "ChartArea1";
            this.chartAllYears.ChartAreas.Add(chartArea2);
            legend2.BorderWidth = 5;
            legend2.Font = new System.Drawing.Font("Modern No. 20", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend2.IsTextAutoFit = false;
            legend2.Name = "Legend1";
            this.chartAllYears.Legends.Add(legend2);
            this.chartAllYears.Location = new System.Drawing.Point(12, 319);
            this.chartAllYears.Name = "chartAllYears";
            this.chartAllYears.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series2.IsXValueIndexed = true;
            series2.Legend = "Legend1";
            series2.LegendText = "Затраты по месяцам";
            series2.Name = "Series1";
            series2.YValuesPerPoint = 4;
            this.chartAllYears.Series.Add(series2);
            this.chartAllYears.Size = new System.Drawing.Size(845, 301);
            this.chartAllYears.TabIndex = 2;
            this.chartAllYears.Text = "chart1";
            title2.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title2.Name = "Title1";
            title2.Text = "Затраты за все время";
            this.chartAllYears.Titles.Add(title2);
            // 
            // textBoxListMonthsAll
            // 
            this.textBoxListMonthsAll.Font = new System.Drawing.Font("Lucida Fax", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxListMonthsAll.Location = new System.Drawing.Point(875, 320);
            this.textBoxListMonthsAll.Multiline = true;
            this.textBoxListMonthsAll.Name = "textBoxListMonthsAll";
            this.textBoxListMonthsAll.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxListMonthsAll.Size = new System.Drawing.Size(169, 300);
            this.textBoxListMonthsAll.TabIndex = 3;
            // 
            // Form_Chart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1056, 626);
            this.Controls.Add(this.textBoxListMonthsAll);
            this.Controls.Add(this.chartAllYears);
            this.Controls.Add(this.textBoxListMonths);
            this.Controls.Add(this.chartCurrentYear);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_Chart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Диаграммизатор";
            this.Load += new System.EventHandler(this.Form_Chart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartCurrentYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartAllYears)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartCurrentYear;
        private System.Windows.Forms.TextBox textBoxListMonths;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartAllYears;
        private System.Windows.Forms.TextBox textBoxListMonthsAll;
    }
}

