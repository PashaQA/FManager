namespace Starter
{
    partial class Form_Start_Apps
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Start_Apps));
            this.labelHeaderStartAnalitics = new System.Windows.Forms.Label();
            this.panelStartAnalitics = new System.Windows.Forms.Panel();
            this.buttonStartCManager = new System.Windows.Forms.Button();
            this.buttonStartFManager = new System.Windows.Forms.Button();
            this.labelHeaderHelper = new System.Windows.Forms.Label();
            this.panelHelper = new System.Windows.Forms.Panel();
            this.buttonHelpStarter = new System.Windows.Forms.Button();
            this.buttonHelpCManager = new System.Windows.Forms.Button();
            this.buttonHelpFManager = new System.Windows.Forms.Button();
            this.labelHeaderUpdater = new System.Windows.Forms.Label();
            this.panelUpdater = new System.Windows.Forms.Panel();
            this.textBoxBackup = new System.Windows.Forms.TextBox();
            this.buttonBackup = new System.Windows.Forms.Button();
            this.labelHeaderCommitComment = new System.Windows.Forms.Label();
            this.textBoxCommitComment = new System.Windows.Forms.TextBox();
            this.labelHeaderFinancePanel = new System.Windows.Forms.Label();
            this.panelUpdaterFinance = new System.Windows.Forms.Panel();
            this.checkBoxHeCar = new System.Windows.Forms.CheckBox();
            this.checkBoxHeGifts = new System.Windows.Forms.CheckBox();
            this.checkBoxHeBig = new System.Windows.Forms.CheckBox();
            this.checkBoxHe = new System.Windows.Forms.CheckBox();
            this.checkBoxSheBig = new System.Windows.Forms.CheckBox();
            this.checkBoxShe = new System.Windows.Forms.CheckBox();
            this.checkBoxCheckroom = new System.Windows.Forms.CheckBox();
            this.checkBoxFinanceAll = new System.Windows.Forms.CheckBox();
            this.buttonUploadUpdate = new System.Windows.Forms.Button();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.SystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пересчитатьРазмерРабочихФайловToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.запуститьДокументациюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.панельЗапускаАналитиковToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.финансовыйМенеджерToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.перейтиКСистемнымПапкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.запускПервойВерсииПриложенияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripSystem = new System.Windows.Forms.MenuStrip();
            this.panelStartAnalitics.SuspendLayout();
            this.panelHelper.SuspendLayout();
            this.panelUpdater.SuspendLayout();
            this.panelUpdaterFinance.SuspendLayout();
            this.menuStripSystem.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelHeaderStartAnalitics
            // 
            this.labelHeaderStartAnalitics.AutoSize = true;
            this.labelHeaderStartAnalitics.BackColor = System.Drawing.Color.Ivory;
            this.labelHeaderStartAnalitics.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelHeaderStartAnalitics.Font = new System.Drawing.Font("MV Boli", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(204)));
            this.labelHeaderStartAnalitics.Location = new System.Drawing.Point(24, 181);
            this.labelHeaderStartAnalitics.Name = "labelHeaderStartAnalitics";
            this.labelHeaderStartAnalitics.Size = new System.Drawing.Size(95, 13);
            this.labelHeaderStartAnalitics.TabIndex = 52;
            this.labelHeaderStartAnalitics.Text = "Запуск аналитиков";
            // 
            // panelStartAnalitics
            // 
            this.panelStartAnalitics.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.panelStartAnalitics.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStartAnalitics.Controls.Add(this.buttonStartCManager);
            this.panelStartAnalitics.Controls.Add(this.buttonStartFManager);
            this.panelStartAnalitics.Location = new System.Drawing.Point(12, 188);
            this.panelStartAnalitics.Name = "panelStartAnalitics";
            this.panelStartAnalitics.Size = new System.Drawing.Size(122, 118);
            this.panelStartAnalitics.TabIndex = 51;
            // 
            // buttonStartCManager
            // 
            this.buttonStartCManager.Location = new System.Drawing.Point(11, 63);
            this.buttonStartCManager.Name = "buttonStartCManager";
            this.buttonStartCManager.Size = new System.Drawing.Size(95, 38);
            this.buttonStartCManager.TabIndex = 1;
            this.buttonStartCManager.Text = "Гардеробный менеджер";
            this.buttonStartCManager.UseVisualStyleBackColor = true;
            // 
            // buttonStartFManager
            // 
            this.buttonStartFManager.Location = new System.Drawing.Point(11, 19);
            this.buttonStartFManager.Name = "buttonStartFManager";
            this.buttonStartFManager.Size = new System.Drawing.Size(95, 38);
            this.buttonStartFManager.TabIndex = 0;
            this.buttonStartFManager.Text = "Финансовый менеджер";
            this.buttonStartFManager.UseVisualStyleBackColor = true;
            this.buttonStartFManager.Click += new System.EventHandler(this.buttonStartFManager_Click);
            // 
            // labelHeaderHelper
            // 
            this.labelHeaderHelper.AutoSize = true;
            this.labelHeaderHelper.BackColor = System.Drawing.Color.Ivory;
            this.labelHeaderHelper.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelHeaderHelper.Font = new System.Drawing.Font("MV Boli", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(204)));
            this.labelHeaderHelper.Location = new System.Drawing.Point(181, 181);
            this.labelHeaderHelper.Name = "labelHeaderHelper";
            this.labelHeaderHelper.Size = new System.Drawing.Size(56, 13);
            this.labelHeaderHelper.TabIndex = 54;
            this.labelHeaderHelper.Text = "Помощник";
            // 
            // panelHelper
            // 
            this.panelHelper.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.panelHelper.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelHelper.Controls.Add(this.buttonHelpStarter);
            this.panelHelper.Controls.Add(this.buttonHelpCManager);
            this.panelHelper.Controls.Add(this.buttonHelpFManager);
            this.panelHelper.Location = new System.Drawing.Point(169, 188);
            this.panelHelper.Name = "panelHelper";
            this.panelHelper.Size = new System.Drawing.Size(170, 118);
            this.panelHelper.TabIndex = 53;
            // 
            // buttonHelpStarter
            // 
            this.buttonHelpStarter.Location = new System.Drawing.Point(11, 8);
            this.buttonHelpStarter.Name = "buttonHelpStarter";
            this.buttonHelpStarter.Size = new System.Drawing.Size(151, 24);
            this.buttonHelpStarter.TabIndex = 2;
            this.buttonHelpStarter.Text = "Документация стартера";
            this.buttonHelpStarter.UseVisualStyleBackColor = true;
            // 
            // buttonHelpCManager
            // 
            this.buttonHelpCManager.Location = new System.Drawing.Point(11, 78);
            this.buttonHelpCManager.Name = "buttonHelpCManager";
            this.buttonHelpCManager.Size = new System.Drawing.Size(151, 34);
            this.buttonHelpCManager.TabIndex = 1;
            this.buttonHelpCManager.Text = "Документация гардеробного менеджера";
            this.buttonHelpCManager.UseVisualStyleBackColor = true;
            // 
            // buttonHelpFManager
            // 
            this.buttonHelpFManager.Location = new System.Drawing.Point(11, 38);
            this.buttonHelpFManager.Name = "buttonHelpFManager";
            this.buttonHelpFManager.Size = new System.Drawing.Size(151, 34);
            this.buttonHelpFManager.TabIndex = 0;
            this.buttonHelpFManager.Text = "Документация финансового менеджера";
            this.buttonHelpFManager.UseVisualStyleBackColor = true;
            // 
            // labelHeaderUpdater
            // 
            this.labelHeaderUpdater.AutoSize = true;
            this.labelHeaderUpdater.BackColor = System.Drawing.Color.Ivory;
            this.labelHeaderUpdater.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelHeaderUpdater.Font = new System.Drawing.Font("MV Boli", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(204)));
            this.labelHeaderUpdater.Location = new System.Drawing.Point(24, 312);
            this.labelHeaderUpdater.Name = "labelHeaderUpdater";
            this.labelHeaderUpdater.Size = new System.Drawing.Size(64, 13);
            this.labelHeaderUpdater.TabIndex = 56;
            this.labelHeaderUpdater.Text = "Обновление";
            // 
            // panelUpdater
            // 
            this.panelUpdater.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.panelUpdater.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelUpdater.Controls.Add(this.textBoxBackup);
            this.panelUpdater.Controls.Add(this.buttonBackup);
            this.panelUpdater.Controls.Add(this.labelHeaderCommitComment);
            this.panelUpdater.Controls.Add(this.textBoxCommitComment);
            this.panelUpdater.Controls.Add(this.labelHeaderFinancePanel);
            this.panelUpdater.Controls.Add(this.panelUpdaterFinance);
            this.panelUpdater.Controls.Add(this.checkBoxCheckroom);
            this.panelUpdater.Controls.Add(this.checkBoxFinanceAll);
            this.panelUpdater.Controls.Add(this.buttonUploadUpdate);
            this.panelUpdater.Location = new System.Drawing.Point(12, 320);
            this.panelUpdater.Name = "panelUpdater";
            this.panelUpdater.Size = new System.Drawing.Size(327, 233);
            this.panelUpdater.TabIndex = 55;
            // 
            // textBoxBackup
            // 
            this.textBoxBackup.BackColor = System.Drawing.Color.Ivory;
            this.textBoxBackup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBackup.Location = new System.Drawing.Point(11, 7);
            this.textBoxBackup.Multiline = true;
            this.textBoxBackup.Name = "textBoxBackup";
            this.textBoxBackup.ReadOnly = true;
            this.textBoxBackup.Size = new System.Drawing.Size(308, 51);
            this.textBoxBackup.TabIndex = 60;
            this.textBoxBackup.MouseMove += new System.Windows.Forms.MouseEventHandler(this.textBoxBackup_MouseMove);
            // 
            // buttonBackup
            // 
            this.buttonBackup.Location = new System.Drawing.Point(245, 70);
            this.buttonBackup.Name = "buttonBackup";
            this.buttonBackup.Size = new System.Drawing.Size(74, 133);
            this.buttonBackup.TabIndex = 59;
            this.buttonBackup.Text = "Зугрузить последнюю версию файлов аналитики (backup)";
            this.buttonBackup.UseVisualStyleBackColor = true;
            this.buttonBackup.Click += new System.EventHandler(this.buttonBackup_Click);
            // 
            // labelHeaderCommitComment
            // 
            this.labelHeaderCommitComment.AutoSize = true;
            this.labelHeaderCommitComment.BackColor = System.Drawing.Color.Ivory;
            this.labelHeaderCommitComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelHeaderCommitComment.Font = new System.Drawing.Font("MV Boli", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(204)));
            this.labelHeaderCommitComment.Location = new System.Drawing.Point(20, 118);
            this.labelHeaderCommitComment.Name = "labelHeaderCommitComment";
            this.labelHeaderCommitComment.Size = new System.Drawing.Size(109, 13);
            this.labelHeaderCommitComment.TabIndex = 58;
            this.labelHeaderCommitComment.Text = "Комментарий коммита";
            // 
            // textBoxCommitComment
            // 
            this.textBoxCommitComment.BackColor = System.Drawing.Color.Ivory;
            this.textBoxCommitComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCommitComment.Location = new System.Drawing.Point(16, 129);
            this.textBoxCommitComment.Multiline = true;
            this.textBoxCommitComment.Name = "textBoxCommitComment";
            this.textBoxCommitComment.Size = new System.Drawing.Size(140, 38);
            this.textBoxCommitComment.TabIndex = 58;
            // 
            // labelHeaderFinancePanel
            // 
            this.labelHeaderFinancePanel.AutoSize = true;
            this.labelHeaderFinancePanel.BackColor = System.Drawing.Color.Ivory;
            this.labelHeaderFinancePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelHeaderFinancePanel.Font = new System.Drawing.Font("MV Boli", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(204)));
            this.labelHeaderFinancePanel.Location = new System.Drawing.Point(175, 61);
            this.labelHeaderFinancePanel.Name = "labelHeaderFinancePanel";
            this.labelHeaderFinancePanel.Size = new System.Drawing.Size(48, 13);
            this.labelHeaderFinancePanel.TabIndex = 57;
            this.labelHeaderFinancePanel.Text = "Финансы";
            // 
            // panelUpdaterFinance
            // 
            this.panelUpdaterFinance.BackColor = System.Drawing.Color.Beige;
            this.panelUpdaterFinance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelUpdaterFinance.Controls.Add(this.checkBoxHeCar);
            this.panelUpdaterFinance.Controls.Add(this.checkBoxHeGifts);
            this.panelUpdaterFinance.Controls.Add(this.checkBoxHeBig);
            this.panelUpdaterFinance.Controls.Add(this.checkBoxHe);
            this.panelUpdaterFinance.Controls.Add(this.checkBoxSheBig);
            this.panelUpdaterFinance.Controls.Add(this.checkBoxShe);
            this.panelUpdaterFinance.Location = new System.Drawing.Point(169, 70);
            this.panelUpdaterFinance.Name = "panelUpdaterFinance";
            this.panelUpdaterFinance.Size = new System.Drawing.Size(72, 155);
            this.panelUpdaterFinance.TabIndex = 3;
            // 
            // checkBoxHeCar
            // 
            this.checkBoxHeCar.AutoSize = true;
            this.checkBoxHeCar.Location = new System.Drawing.Point(8, 128);
            this.checkBoxHeCar.Name = "checkBoxHeCar";
            this.checkBoxHeCar.Size = new System.Drawing.Size(56, 17);
            this.checkBoxHeCar.TabIndex = 6;
            this.checkBoxHeCar.Text = "HeCar";
            this.checkBoxHeCar.UseVisualStyleBackColor = true;
            this.checkBoxHeCar.CheckedChanged += new System.EventHandler(this.checkBoxHeCar_CheckedChanged);
            // 
            // checkBoxHeGifts
            // 
            this.checkBoxHeGifts.AutoSize = true;
            this.checkBoxHeGifts.Location = new System.Drawing.Point(9, 105);
            this.checkBoxHeGifts.Name = "checkBoxHeGifts";
            this.checkBoxHeGifts.Size = new System.Drawing.Size(61, 17);
            this.checkBoxHeGifts.TabIndex = 4;
            this.checkBoxHeGifts.Text = "HeGifts";
            this.checkBoxHeGifts.UseVisualStyleBackColor = true;
            this.checkBoxHeGifts.CheckedChanged += new System.EventHandler(this.checkBoxHeGifts_CheckedChanged);
            // 
            // checkBoxHeBig
            // 
            this.checkBoxHeBig.AutoSize = true;
            this.checkBoxHeBig.Location = new System.Drawing.Point(9, 82);
            this.checkBoxHeBig.Name = "checkBoxHeBig";
            this.checkBoxHeBig.Size = new System.Drawing.Size(55, 17);
            this.checkBoxHeBig.TabIndex = 3;
            this.checkBoxHeBig.Text = "HeBig";
            this.checkBoxHeBig.UseVisualStyleBackColor = true;
            this.checkBoxHeBig.CheckedChanged += new System.EventHandler(this.checkBoxHeBig_CheckedChanged);
            // 
            // checkBoxHe
            // 
            this.checkBoxHe.AutoSize = true;
            this.checkBoxHe.Location = new System.Drawing.Point(9, 59);
            this.checkBoxHe.Name = "checkBoxHe";
            this.checkBoxHe.Size = new System.Drawing.Size(40, 17);
            this.checkBoxHe.TabIndex = 2;
            this.checkBoxHe.Text = "He";
            this.checkBoxHe.UseVisualStyleBackColor = true;
            this.checkBoxHe.CheckedChanged += new System.EventHandler(this.checkBoxHe_CheckedChanged);
            // 
            // checkBoxSheBig
            // 
            this.checkBoxSheBig.AutoSize = true;
            this.checkBoxSheBig.Location = new System.Drawing.Point(9, 36);
            this.checkBoxSheBig.Name = "checkBoxSheBig";
            this.checkBoxSheBig.Size = new System.Drawing.Size(60, 17);
            this.checkBoxSheBig.TabIndex = 1;
            this.checkBoxSheBig.Text = "SheBig";
            this.checkBoxSheBig.UseVisualStyleBackColor = true;
            this.checkBoxSheBig.CheckedChanged += new System.EventHandler(this.checkBoxSheBig_CheckedChanged);
            // 
            // checkBoxShe
            // 
            this.checkBoxShe.AutoSize = true;
            this.checkBoxShe.Location = new System.Drawing.Point(9, 13);
            this.checkBoxShe.Name = "checkBoxShe";
            this.checkBoxShe.Size = new System.Drawing.Size(45, 17);
            this.checkBoxShe.TabIndex = 0;
            this.checkBoxShe.Text = "She";
            this.checkBoxShe.UseVisualStyleBackColor = true;
            this.checkBoxShe.CheckedChanged += new System.EventHandler(this.checkBoxShe_CheckedChanged);
            // 
            // checkBoxCheckroom
            // 
            this.checkBoxCheckroom.AutoSize = true;
            this.checkBoxCheckroom.Location = new System.Drawing.Point(89, 176);
            this.checkBoxCheckroom.Name = "checkBoxCheckroom";
            this.checkBoxCheckroom.Size = new System.Drawing.Size(74, 17);
            this.checkBoxCheckroom.TabIndex = 2;
            this.checkBoxCheckroom.Text = "Гардероб";
            this.checkBoxCheckroom.UseVisualStyleBackColor = true;
            this.checkBoxCheckroom.CheckedChanged += new System.EventHandler(this.checkBoxCheckroom_CheckedChanged);
            // 
            // checkBoxFinanceAll
            // 
            this.checkBoxFinanceAll.AutoSize = true;
            this.checkBoxFinanceAll.Location = new System.Drawing.Point(16, 176);
            this.checkBoxFinanceAll.Name = "checkBoxFinanceAll";
            this.checkBoxFinanceAll.Size = new System.Drawing.Size(75, 17);
            this.checkBoxFinanceAll.TabIndex = 1;
            this.checkBoxFinanceAll.Text = "Финансы";
            this.checkBoxFinanceAll.UseVisualStyleBackColor = true;
            this.checkBoxFinanceAll.CheckedChanged += new System.EventHandler(this.checkBoxFinanceAll_CheckedChanged);
            // 
            // buttonUploadUpdate
            // 
            this.buttonUploadUpdate.Location = new System.Drawing.Point(11, 66);
            this.buttonUploadUpdate.Name = "buttonUploadUpdate";
            this.buttonUploadUpdate.Size = new System.Drawing.Size(151, 38);
            this.buttonUploadUpdate.TabIndex = 0;
            this.buttonUploadUpdate.Text = "Залить обновление для аналитиков";
            this.buttonUploadUpdate.UseVisualStyleBackColor = true;
            this.buttonUploadUpdate.Click += new System.EventHandler(this.buttonUploadUpdate_Click);
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.BackColor = System.Drawing.Color.Ivory;
            this.textBoxStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxStatus.Location = new System.Drawing.Point(17, 37);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ReadOnly = true;
            this.textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxStatus.Size = new System.Drawing.Size(320, 141);
            this.textBoxStatus.TabIndex = 57;
            // 
            // SystemToolStripMenuItem
            // 
            this.SystemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.пересчитатьРазмерРабочихФайловToolStripMenuItem,
            this.запуститьДокументациюToolStripMenuItem,
            this.перейтиКСистемнымПапкаToolStripMenuItem,
            this.запускПервойВерсииПриложенияToolStripMenuItem});
            this.SystemToolStripMenuItem.Name = "SystemToolStripMenuItem";
            this.SystemToolStripMenuItem.Size = new System.Drawing.Size(167, 20);
            this.SystemToolStripMenuItem.Text = "Системные возможности";
            // 
            // пересчитатьРазмерРабочихФайловToolStripMenuItem
            // 
            this.пересчитатьРазмерРабочихФайловToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlText;
            this.пересчитатьРазмерРабочихФайловToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("пересчитатьРазмерРабочихФайловToolStripMenuItem.Image")));
            this.пересчитатьРазмерРабочихФайловToolStripMenuItem.Name = "пересчитатьРазмерРабочихФайловToolStripMenuItem";
            this.пересчитатьРазмерРабочихФайловToolStripMenuItem.Size = new System.Drawing.Size(292, 22);
            this.пересчитатьРазмерРабочихФайловToolStripMenuItem.Text = "Пересчитать размер рабочих файлов";
            this.пересчитатьРазмерРабочихФайловToolStripMenuItem.Click += new System.EventHandler(this.пересчитатьРазмерРабочихФайловToolStripMenuItem_Click);
            // 
            // запуститьДокументациюToolStripMenuItem
            // 
            this.запуститьДокументациюToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.панельЗапускаАналитиковToolStripMenuItem,
            this.финансовыйМенеджерToolStripMenuItem});
            this.запуститьДокументациюToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("запуститьДокументациюToolStripMenuItem.Image")));
            this.запуститьДокументациюToolStripMenuItem.Name = "запуститьДокументациюToolStripMenuItem";
            this.запуститьДокументациюToolStripMenuItem.Size = new System.Drawing.Size(292, 22);
            this.запуститьДокументациюToolStripMenuItem.Text = "Запустить документацию";
            // 
            // панельЗапускаАналитиковToolStripMenuItem
            // 
            this.панельЗапускаАналитиковToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("панельЗапускаАналитиковToolStripMenuItem.Image")));
            this.панельЗапускаАналитиковToolStripMenuItem.Name = "панельЗапускаАналитиковToolStripMenuItem";
            this.панельЗапускаАналитиковToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.панельЗапускаАналитиковToolStripMenuItem.Text = "Панель запуска аналитиков";
            // 
            // финансовыйМенеджерToolStripMenuItem
            // 
            this.финансовыйМенеджерToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("финансовыйМенеджерToolStripMenuItem.Image")));
            this.финансовыйМенеджерToolStripMenuItem.Name = "финансовыйМенеджерToolStripMenuItem";
            this.финансовыйМенеджерToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.финансовыйМенеджерToolStripMenuItem.Text = "Финансовый менеджер";
            // 
            // перейтиКСистемнымПапкаToolStripMenuItem
            // 
            this.перейтиКСистемнымПапкаToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("перейтиКСистемнымПапкаToolStripMenuItem.Image")));
            this.перейтиКСистемнымПапкаToolStripMenuItem.Name = "перейтиКСистемнымПапкаToolStripMenuItem";
            this.перейтиКСистемнымПапкаToolStripMenuItem.Size = new System.Drawing.Size(292, 22);
            this.перейтиКСистемнымПапкаToolStripMenuItem.Text = "Перейти к системным папкам";
            this.перейтиКСистемнымПапкаToolStripMenuItem.Click += new System.EventHandler(this.перейтиКСистемнымПапкаToolStripMenuItem_Click);
            // 
            // запускПервойВерсииПриложенияToolStripMenuItem
            // 
            this.запускПервойВерсииПриложенияToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("запускПервойВерсииПриложенияToolStripMenuItem.Image")));
            this.запускПервойВерсииПриложенияToolStripMenuItem.Name = "запускПервойВерсииПриложенияToolStripMenuItem";
            this.запускПервойВерсииПриложенияToolStripMenuItem.Size = new System.Drawing.Size(292, 22);
            this.запускПервойВерсииПриложенияToolStripMenuItem.Text = "Запуск первой версии приложения";
            this.запускПервойВерсииПриложенияToolStripMenuItem.Click += new System.EventHandler(this.запускПервойВерсииПриложенияToolStripMenuItem_Click);
            // 
            // menuStripSystem
            // 
            this.menuStripSystem.BackColor = System.Drawing.Color.Ivory;
            this.menuStripSystem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.menuStripSystem.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStripSystem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SystemToolStripMenuItem});
            this.menuStripSystem.Location = new System.Drawing.Point(0, 0);
            this.menuStripSystem.Name = "menuStripSystem";
            this.menuStripSystem.Size = new System.Drawing.Size(351, 24);
            this.menuStripSystem.TabIndex = 58;
            this.menuStripSystem.Text = "menuStrip1";
            // 
            // Form_Start_Apps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Cornsilk;
            this.ClientSize = new System.Drawing.Size(351, 555);
            this.Controls.Add(this.textBoxStatus);
            this.Controls.Add(this.labelHeaderUpdater);
            this.Controls.Add(this.panelUpdater);
            this.Controls.Add(this.labelHeaderHelper);
            this.Controls.Add(this.panelHelper);
            this.Controls.Add(this.labelHeaderStartAnalitics);
            this.Controls.Add(this.panelStartAnalitics);
            this.Controls.Add(this.menuStripSystem);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripSystem;
            this.Name = "Form_Start_Apps";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Панель запуска аналитиков";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_Start_Apps_FormClosed);
            this.Shown += new System.EventHandler(this.Form_Start_Apps_Shown);
            this.panelStartAnalitics.ResumeLayout(false);
            this.panelHelper.ResumeLayout(false);
            this.panelUpdater.ResumeLayout(false);
            this.panelUpdater.PerformLayout();
            this.panelUpdaterFinance.ResumeLayout(false);
            this.panelUpdaterFinance.PerformLayout();
            this.menuStripSystem.ResumeLayout(false);
            this.menuStripSystem.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelHeaderStartAnalitics;
        private System.Windows.Forms.Panel panelStartAnalitics;
        private System.Windows.Forms.Button buttonStartCManager;
        private System.Windows.Forms.Button buttonStartFManager;
        private System.Windows.Forms.Label labelHeaderHelper;
        private System.Windows.Forms.Panel panelHelper;
        private System.Windows.Forms.Button buttonHelpCManager;
        private System.Windows.Forms.Button buttonHelpFManager;
        private System.Windows.Forms.Label labelHeaderUpdater;
        private System.Windows.Forms.Panel panelUpdater;
        private System.Windows.Forms.Label labelHeaderFinancePanel;
        private System.Windows.Forms.Panel panelUpdaterFinance;
        private System.Windows.Forms.CheckBox checkBoxCheckroom;
        private System.Windows.Forms.CheckBox checkBoxFinanceAll;
        private System.Windows.Forms.Button buttonUploadUpdate;
        private System.Windows.Forms.CheckBox checkBoxHeBig;
        private System.Windows.Forms.CheckBox checkBoxHe;
        private System.Windows.Forms.CheckBox checkBoxSheBig;
        private System.Windows.Forms.CheckBox checkBoxShe;
        private System.Windows.Forms.CheckBox checkBoxHeGifts;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.Label labelHeaderCommitComment;
        private System.Windows.Forms.TextBox textBoxCommitComment;
        private System.Windows.Forms.Button buttonBackup;
        private System.Windows.Forms.TextBox textBoxBackup;
        private System.Windows.Forms.Button buttonHelpStarter;
        private System.Windows.Forms.ToolStripMenuItem SystemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пересчитатьРазмерРабочихФайловToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStripSystem;
        private System.Windows.Forms.ToolStripMenuItem запуститьДокументациюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem перейтиКСистемнымПапкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem запускПервойВерсииПриложенияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem панельЗапускаАналитиковToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem финансовыйМенеджерToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxHeCar;
    }
}