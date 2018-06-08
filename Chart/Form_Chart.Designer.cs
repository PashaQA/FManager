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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.chartColumns = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.buttonExecute = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.panelSpecial = new System.Windows.Forms.Panel();
            this.checkBoxK = new System.Windows.Forms.CheckBox();
            this.checkBoxB = new System.Windows.Forms.CheckBox();
            this.buttonInfoInSpesialParams = new System.Windows.Forms.Button();
            this.checkBoxShe = new System.Windows.Forms.CheckBox();
            this.checkBoxWithoutParams = new System.Windows.Forms.CheckBox();
            this.checkBoxN = new System.Windows.Forms.CheckBox();
            this.checkBoxT = new System.Windows.Forms.CheckBox();
            this.labelHeaderMode = new System.Windows.Forms.Label();
            this.panelMode = new System.Windows.Forms.Panel();
            this.radioButtonModeSheBig = new System.Windows.Forms.RadioButton();
            this.radioButtonModeHeBig = new System.Windows.Forms.RadioButton();
            this.radioButtonModeShe = new System.Windows.Forms.RadioButton();
            this.radioButtonModeHeGifts = new System.Windows.Forms.RadioButton();
            this.radioButtonModeHe = new System.Windows.Forms.RadioButton();
            this.panelStart = new System.Windows.Forms.Panel();
            this.cbMonthsStart = new System.Windows.Forms.ComboBox();
            this.numericYearsStart = new System.Windows.Forms.NumericUpDown();
            this.panelStop = new System.Windows.Forms.Panel();
            this.cbMonthsStop = new System.Windows.Forms.ComboBox();
            this.numericYearsStop = new System.Windows.Forms.NumericUpDown();
            this.labelHeaderStart = new System.Windows.Forms.Label();
            this.labelHeaderStop = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartColumns)).BeginInit();
            this.panelSpecial.SuspendLayout();
            this.panelMode.SuspendLayout();
            this.panelStart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericYearsStart)).BeginInit();
            this.panelStop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericYearsStop)).BeginInit();
            this.SuspendLayout();
            // 
            // chartColumns
            // 
            chartArea2.Name = "ChartArea1";
            this.chartColumns.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartColumns.Legends.Add(legend2);
            this.chartColumns.Location = new System.Drawing.Point(12, 130);
            this.chartColumns.Name = "chartColumns";
            this.chartColumns.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Fire;
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.LegendText = "Сумма затрат";
            series2.Name = "Series1";
            this.chartColumns.Series.Add(series2);
            this.chartColumns.Size = new System.Drawing.Size(797, 300);
            this.chartColumns.TabIndex = 0;
            this.chartColumns.Text = "chart1";
            title2.Name = "Title1";
            title2.Text = "Затраты";
            this.chartColumns.Titles.Add(title2);
            // 
            // buttonExecute
            // 
            this.buttonExecute.Location = new System.Drawing.Point(12, 100);
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.Size = new System.Drawing.Size(124, 24);
            this.buttonExecute.TabIndex = 1;
            this.buttonExecute.Text = "Выполнить расчет";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Ivory;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Font = new System.Drawing.Font("MV Boli", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(233, 5);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(124, 13);
            this.label10.TabIndex = 55;
            this.label10.Text = "Специальные параметры";
            // 
            // panelSpecial
            // 
            this.panelSpecial.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.panelSpecial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSpecial.Controls.Add(this.checkBoxK);
            this.panelSpecial.Controls.Add(this.checkBoxB);
            this.panelSpecial.Controls.Add(this.buttonInfoInSpesialParams);
            this.panelSpecial.Controls.Add(this.checkBoxShe);
            this.panelSpecial.Controls.Add(this.checkBoxWithoutParams);
            this.panelSpecial.Controls.Add(this.checkBoxN);
            this.panelSpecial.Controls.Add(this.checkBoxT);
            this.panelSpecial.Location = new System.Drawing.Point(220, 12);
            this.panelSpecial.Name = "panelSpecial";
            this.panelSpecial.Size = new System.Drawing.Size(435, 96);
            this.panelSpecial.TabIndex = 52;
            // 
            // checkBoxK
            // 
            this.checkBoxK.AutoSize = true;
            this.checkBoxK.Font = new System.Drawing.Font("MV Boli", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxK.Location = new System.Drawing.Point(150, 8);
            this.checkBoxK.Name = "checkBoxK";
            this.checkBoxK.Size = new System.Drawing.Size(34, 19);
            this.checkBoxK.TabIndex = 7;
            this.checkBoxK.Text = "K";
            this.checkBoxK.UseVisualStyleBackColor = true;
            // 
            // checkBoxB
            // 
            this.checkBoxB.AutoSize = true;
            this.checkBoxB.Font = new System.Drawing.Font("MV Boli", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxB.Location = new System.Drawing.Point(40, 57);
            this.checkBoxB.Name = "checkBoxB";
            this.checkBoxB.Size = new System.Drawing.Size(34, 19);
            this.checkBoxB.TabIndex = 6;
            this.checkBoxB.Text = "Б";
            this.checkBoxB.UseVisualStyleBackColor = true;
            // 
            // buttonInfoInSpesialParams
            // 
            this.buttonInfoInSpesialParams.BackColor = System.Drawing.Color.Yellow;
            this.buttonInfoInSpesialParams.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonInfoInSpesialParams.Location = new System.Drawing.Point(12, 10);
            this.buttonInfoInSpesialParams.Name = "buttonInfoInSpesialParams";
            this.buttonInfoInSpesialParams.Size = new System.Drawing.Size(20, 75);
            this.buttonInfoInSpesialParams.TabIndex = 5;
            this.buttonInfoInSpesialParams.Text = "?";
            this.buttonInfoInSpesialParams.UseVisualStyleBackColor = false;
            // 
            // checkBoxShe
            // 
            this.checkBoxShe.AutoSize = true;
            this.checkBoxShe.Font = new System.Drawing.Font("MV Boli", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxShe.Location = new System.Drawing.Point(40, 74);
            this.checkBoxShe.Name = "checkBoxShe";
            this.checkBoxShe.Size = new System.Drawing.Size(51, 19);
            this.checkBoxShe.TabIndex = 3;
            this.checkBoxShe.Text = "ОНА";
            this.checkBoxShe.UseVisualStyleBackColor = true;
            // 
            // checkBoxWithoutParams
            // 
            this.checkBoxWithoutParams.AutoSize = true;
            this.checkBoxWithoutParams.Font = new System.Drawing.Font("MV Boli", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxWithoutParams.Location = new System.Drawing.Point(40, 40);
            this.checkBoxWithoutParams.Name = "checkBoxWithoutParams";
            this.checkBoxWithoutParams.Size = new System.Drawing.Size(119, 19);
            this.checkBoxWithoutParams.TabIndex = 2;
            this.checkBoxWithoutParams.Text = "Без параметров";
            this.checkBoxWithoutParams.UseVisualStyleBackColor = true;
            // 
            // checkBoxN
            // 
            this.checkBoxN.AutoSize = true;
            this.checkBoxN.Font = new System.Drawing.Font("MV Boli", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxN.Location = new System.Drawing.Point(40, 24);
            this.checkBoxN.Name = "checkBoxN";
            this.checkBoxN.Size = new System.Drawing.Size(35, 19);
            this.checkBoxN.TabIndex = 1;
            this.checkBoxN.Text = "Н";
            this.checkBoxN.UseVisualStyleBackColor = true;
            // 
            // checkBoxT
            // 
            this.checkBoxT.AutoSize = true;
            this.checkBoxT.Font = new System.Drawing.Font("MV Boli", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxT.Location = new System.Drawing.Point(40, 8);
            this.checkBoxT.Name = "checkBoxT";
            this.checkBoxT.Size = new System.Drawing.Size(33, 19);
            this.checkBoxT.TabIndex = 0;
            this.checkBoxT.Text = "Т";
            this.checkBoxT.UseVisualStyleBackColor = true;
            // 
            // labelHeaderMode
            // 
            this.labelHeaderMode.AutoSize = true;
            this.labelHeaderMode.BackColor = System.Drawing.Color.Ivory;
            this.labelHeaderMode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelHeaderMode.Font = new System.Drawing.Font("MV Boli", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(204)));
            this.labelHeaderMode.Location = new System.Drawing.Point(673, 5);
            this.labelHeaderMode.Name = "labelHeaderMode";
            this.labelHeaderMode.Size = new System.Drawing.Size(44, 13);
            this.labelHeaderMode.TabIndex = 54;
            this.labelHeaderMode.Text = "Режимы";
            // 
            // panelMode
            // 
            this.panelMode.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.panelMode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMode.Controls.Add(this.radioButtonModeSheBig);
            this.panelMode.Controls.Add(this.radioButtonModeHeBig);
            this.panelMode.Controls.Add(this.radioButtonModeShe);
            this.panelMode.Controls.Add(this.radioButtonModeHeGifts);
            this.panelMode.Controls.Add(this.radioButtonModeHe);
            this.panelMode.Location = new System.Drawing.Point(661, 12);
            this.panelMode.Name = "panelMode";
            this.panelMode.Size = new System.Drawing.Size(148, 96);
            this.panelMode.TabIndex = 53;
            // 
            // radioButtonModeSheBig
            // 
            this.radioButtonModeSheBig.AutoSize = true;
            this.radioButtonModeSheBig.Font = new System.Drawing.Font("MV Boli", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonModeSheBig.Location = new System.Drawing.Point(12, 26);
            this.radioButtonModeSheBig.Name = "radioButtonModeSheBig";
            this.radioButtonModeSheBig.Size = new System.Drawing.Size(71, 20);
            this.radioButtonModeSheBig.TabIndex = 4;
            this.radioButtonModeSheBig.TabStop = true;
            this.radioButtonModeSheBig.Text = "SheBig";
            this.radioButtonModeSheBig.UseVisualStyleBackColor = true;
            // 
            // radioButtonModeHeBig
            // 
            this.radioButtonModeHeBig.AutoSize = true;
            this.radioButtonModeHeBig.Font = new System.Drawing.Font("MV Boli", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonModeHeBig.Location = new System.Drawing.Point(11, 75);
            this.radioButtonModeHeBig.Name = "radioButtonModeHeBig";
            this.radioButtonModeHeBig.Size = new System.Drawing.Size(63, 20);
            this.radioButtonModeHeBig.TabIndex = 3;
            this.radioButtonModeHeBig.TabStop = true;
            this.radioButtonModeHeBig.Text = "HeBig";
            this.radioButtonModeHeBig.UseVisualStyleBackColor = true;
            // 
            // radioButtonModeShe
            // 
            this.radioButtonModeShe.AutoSize = true;
            this.radioButtonModeShe.Font = new System.Drawing.Font("MV Boli", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonModeShe.Location = new System.Drawing.Point(12, 10);
            this.radioButtonModeShe.Name = "radioButtonModeShe";
            this.radioButtonModeShe.Size = new System.Drawing.Size(51, 20);
            this.radioButtonModeShe.TabIndex = 0;
            this.radioButtonModeShe.TabStop = true;
            this.radioButtonModeShe.Text = "She";
            this.radioButtonModeShe.UseVisualStyleBackColor = true;
            // 
            // radioButtonModeHeGifts
            // 
            this.radioButtonModeHeGifts.AutoSize = true;
            this.radioButtonModeHeGifts.Font = new System.Drawing.Font("MV Boli", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonModeHeGifts.Location = new System.Drawing.Point(11, 59);
            this.radioButtonModeHeGifts.Name = "radioButtonModeHeGifts";
            this.radioButtonModeHeGifts.Size = new System.Drawing.Size(72, 20);
            this.radioButtonModeHeGifts.TabIndex = 2;
            this.radioButtonModeHeGifts.TabStop = true;
            this.radioButtonModeHeGifts.Text = "HeGifts";
            this.radioButtonModeHeGifts.UseVisualStyleBackColor = true;
            // 
            // radioButtonModeHe
            // 
            this.radioButtonModeHe.AutoSize = true;
            this.radioButtonModeHe.Font = new System.Drawing.Font("MV Boli", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonModeHe.Location = new System.Drawing.Point(11, 42);
            this.radioButtonModeHe.Name = "radioButtonModeHe";
            this.radioButtonModeHe.Size = new System.Drawing.Size(43, 20);
            this.radioButtonModeHe.TabIndex = 1;
            this.radioButtonModeHe.TabStop = true;
            this.radioButtonModeHe.Text = "He";
            this.radioButtonModeHe.UseVisualStyleBackColor = true;
            // 
            // panelStart
            // 
            this.panelStart.BackColor = System.Drawing.Color.Ivory;
            this.panelStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panelStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStart.Controls.Add(this.cbMonthsStart);
            this.panelStart.Controls.Add(this.numericYearsStart);
            this.panelStart.Location = new System.Drawing.Point(12, 12);
            this.panelStart.Name = "panelStart";
            this.panelStart.Size = new System.Drawing.Size(202, 38);
            this.panelStart.TabIndex = 56;
            // 
            // cbMonthsStart
            // 
            this.cbMonthsStart.FormattingEnabled = true;
            this.cbMonthsStart.Location = new System.Drawing.Point(3, 9);
            this.cbMonthsStart.Name = "cbMonthsStart";
            this.cbMonthsStart.Size = new System.Drawing.Size(108, 21);
            this.cbMonthsStart.TabIndex = 3;
            // 
            // numericYearsStart
            // 
            this.numericYearsStart.Location = new System.Drawing.Point(121, 9);
            this.numericYearsStart.Name = "numericYearsStart";
            this.numericYearsStart.Size = new System.Drawing.Size(73, 20);
            this.numericYearsStart.TabIndex = 4;
            // 
            // panelStop
            // 
            this.panelStop.BackColor = System.Drawing.Color.Ivory;
            this.panelStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panelStop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStop.Controls.Add(this.cbMonthsStop);
            this.panelStop.Controls.Add(this.numericYearsStop);
            this.panelStop.Location = new System.Drawing.Point(12, 59);
            this.panelStop.Name = "panelStop";
            this.panelStop.Size = new System.Drawing.Size(202, 38);
            this.panelStop.TabIndex = 30;
            // 
            // cbMonthsStop
            // 
            this.cbMonthsStop.FormattingEnabled = true;
            this.cbMonthsStop.Location = new System.Drawing.Point(3, 11);
            this.cbMonthsStop.Name = "cbMonthsStop";
            this.cbMonthsStop.Size = new System.Drawing.Size(108, 21);
            this.cbMonthsStop.TabIndex = 3;
            // 
            // numericYearsStop
            // 
            this.numericYearsStop.Location = new System.Drawing.Point(121, 11);
            this.numericYearsStop.Name = "numericYearsStop";
            this.numericYearsStop.Size = new System.Drawing.Size(73, 20);
            this.numericYearsStop.TabIndex = 4;
            // 
            // labelHeaderStart
            // 
            this.labelHeaderStart.AutoSize = true;
            this.labelHeaderStart.BackColor = System.Drawing.Color.Ivory;
            this.labelHeaderStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelHeaderStart.Font = new System.Drawing.Font("MV Boli", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(204)));
            this.labelHeaderStart.Location = new System.Drawing.Point(16, 5);
            this.labelHeaderStart.Name = "labelHeaderStart";
            this.labelHeaderStart.Size = new System.Drawing.Size(83, 13);
            this.labelHeaderStart.TabIndex = 57;
            this.labelHeaderStart.Text = "Начало периода";
            // 
            // labelHeaderStop
            // 
            this.labelHeaderStop.AutoSize = true;
            this.labelHeaderStop.BackColor = System.Drawing.Color.Ivory;
            this.labelHeaderStop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelHeaderStop.Font = new System.Drawing.Font("MV Boli", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(204)));
            this.labelHeaderStop.Location = new System.Drawing.Point(16, 52);
            this.labelHeaderStop.Name = "labelHeaderStop";
            this.labelHeaderStop.Size = new System.Drawing.Size(77, 13);
            this.labelHeaderStop.TabIndex = 58;
            this.labelHeaderStop.Text = "Конец периода";
            // 
            // Form_Chart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 638);
            this.Controls.Add(this.labelHeaderStop);
            this.Controls.Add(this.labelHeaderStart);
            this.Controls.Add(this.panelStop);
            this.Controls.Add(this.panelStart);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.panelSpecial);
            this.Controls.Add(this.labelHeaderMode);
            this.Controls.Add(this.panelMode);
            this.Controls.Add(this.buttonExecute);
            this.Controls.Add(this.chartColumns);
            this.Name = "Form_Chart";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form_Chart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartColumns)).EndInit();
            this.panelSpecial.ResumeLayout(false);
            this.panelSpecial.PerformLayout();
            this.panelMode.ResumeLayout(false);
            this.panelMode.PerformLayout();
            this.panelStart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericYearsStart)).EndInit();
            this.panelStop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericYearsStop)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartColumns;
        private System.Windows.Forms.Button buttonExecute;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panelSpecial;
        private System.Windows.Forms.CheckBox checkBoxK;
        private System.Windows.Forms.CheckBox checkBoxB;
        private System.Windows.Forms.Button buttonInfoInSpesialParams;
        private System.Windows.Forms.CheckBox checkBoxShe;
        private System.Windows.Forms.CheckBox checkBoxWithoutParams;
        private System.Windows.Forms.CheckBox checkBoxN;
        private System.Windows.Forms.CheckBox checkBoxT;
        private System.Windows.Forms.Label labelHeaderMode;
        private System.Windows.Forms.Panel panelMode;
        private System.Windows.Forms.RadioButton radioButtonModeSheBig;
        private System.Windows.Forms.RadioButton radioButtonModeHeBig;
        private System.Windows.Forms.RadioButton radioButtonModeShe;
        private System.Windows.Forms.RadioButton radioButtonModeHeGifts;
        private System.Windows.Forms.RadioButton radioButtonModeHe;
        private System.Windows.Forms.Panel panelStart;
        private System.Windows.Forms.ComboBox cbMonthsStart;
        private System.Windows.Forms.NumericUpDown numericYearsStart;
        private System.Windows.Forms.Panel panelStop;
        private System.Windows.Forms.ComboBox cbMonthsStop;
        private System.Windows.Forms.NumericUpDown numericYearsStop;
        private System.Windows.Forms.Label labelHeaderStart;
        private System.Windows.Forms.Label labelHeaderStop;
    }
}

