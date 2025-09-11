namespace GUI_GT
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.tabControlSettings = new System.Windows.Forms.TabControl();
            this.tabPageReports = new System.Windows.Forms.TabPage();
            this.comboBoxTextFontFamily = new System.Windows.Forms.ComboBox();
            this.lbNameTextFont = new System.Windows.Forms.Label();
            this.numericUpDownTextFontSize = new System.Windows.Forms.NumericUpDown();
            this.lbSizeTextFont = new System.Windows.Forms.Label();
            this.groupBoxSettingFontComment = new System.Windows.Forms.GroupBox();
            this.comboBoxTableFontFamily = new System.Windows.Forms.ComboBox();
            this.lbNameTableFont = new System.Windows.Forms.Label();
            this.numericUpDownTableFontSize = new System.Windows.Forms.NumericUpDown();
            this.lbSizeTableFont = new System.Windows.Forms.Label();
            this.groupBoxSettingFontTable = new System.Windows.Forms.GroupBox();
            this.checkBoxNull_to_zero = new System.Windows.Forms.CheckBox();
            this.checkBoxShadingRows = new System.Windows.Forms.CheckBox();
            this.groupBoxTypeOfTableMeans = new System.Windows.Forms.GroupBox();
            this.radioBtTypePoint = new System.Windows.Forms.RadioButton();
            this.radioBtDifference = new System.Windows.Forms.RadioButton();
            this.radioBtDefault = new System.Windows.Forms.RadioButton();
            this.cBoxDecimalSeparator = new System.Windows.Forms.ComboBox();
            this.lbDecimalSeparator = new System.Windows.Forms.Label();
            this.cBoxNumberOfDecimals = new System.Windows.Forms.ComboBox();
            this.lbNumberOfDecimals = new System.Windows.Forms.Label();
            this.tabPageGraphics = new System.Windows.Forms.TabPage();
            this.groupBoxChartSetting = new System.Windows.Forms.GroupBox();
            this.lbMarkerStyle = new System.Windows.Forms.Label();
            this.comboBoxMarkerStyle = new System.Windows.Forms.ComboBox();
            this.comboBoxLabelPoint = new System.Windows.Forms.ComboBox();
            this.lbLabelPoint = new System.Windows.Forms.Label();
            this.comboBoxChartType = new System.Windows.Forms.ComboBox();
            this.lbChartType = new System.Windows.Forms.Label();
            this.checkBoxErrorAbsStandDev = new System.Windows.Forms.CheckBox();
            this.checkBoxErrorRelStandDev = new System.Windows.Forms.CheckBox();
            this.checkBoxTotalAbsErrorVar = new System.Windows.Forms.CheckBox();
            this.checkBoxTotalRelErrorVar = new System.Windows.Forms.CheckBox();
            this.checkBox_coefG_Abs = new System.Windows.Forms.CheckBox();
            this.checkBox_coefG_Rel = new System.Windows.Forms.CheckBox();
            this.tabPageConect = new System.Windows.Forms.TabPage();
            this.tbPath_Workspace = new System.Windows.Forms.TextBox();
            this.lbWorkspace = new System.Windows.Forms.Label();
            this.colorChartDialog = new System.Windows.Forms.ColorDialog();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.pictBoxColorErrorAbsStandDev = new System.Windows.Forms.PictureBox();
            this.pictBoxColorErrorRelStandDev = new System.Windows.Forms.PictureBox();
            this.pictBoxColorTotalAbsErrorVar = new System.Windows.Forms.PictureBox();
            this.pictBoxColorTotalRelErrorVar = new System.Windows.Forms.PictureBox();
            this.pictBoxColor_Coef_G_Abs = new System.Windows.Forms.PictureBox();
            this.pictBoxColor_Coef_G_Rel = new System.Windows.Forms.PictureBox();
            this.btSelectWorkspace = new System.Windows.Forms.Button();
            this.btRemoveUserPass = new System.Windows.Forms.Button();
            this.btLoadDefaultSetting = new System.Windows.Forms.Button();
            this.btHelp = new System.Windows.Forms.Button();
            this.tabControlSettings.SuspendLayout();
            this.tabPageReports.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTextFontSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTableFontSize)).BeginInit();
            this.groupBoxTypeOfTableMeans.SuspendLayout();
            this.tabPageGraphics.SuspendLayout();
            this.groupBoxChartSetting.SuspendLayout();
            this.tabPageConect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxColorErrorAbsStandDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxColorErrorRelStandDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxColorTotalAbsErrorVar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxColorTotalRelErrorVar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxColor_Coef_G_Abs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxColor_Coef_G_Rel)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlSettings
            // 
            this.tabControlSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlSettings.Controls.Add(this.tabPageReports);
            this.tabControlSettings.Controls.Add(this.tabPageGraphics);
            this.tabControlSettings.Controls.Add(this.tabPageConect);
            this.tabControlSettings.Location = new System.Drawing.Point(3, 13);
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.SelectedIndex = 0;
            this.tabControlSettings.Size = new System.Drawing.Size(578, 315);
            this.tabControlSettings.TabIndex = 3;
            // 
            // tabPageReports
            // 
            this.tabPageReports.Controls.Add(this.comboBoxTextFontFamily);
            this.tabPageReports.Controls.Add(this.lbNameTextFont);
            this.tabPageReports.Controls.Add(this.numericUpDownTextFontSize);
            this.tabPageReports.Controls.Add(this.lbSizeTextFont);
            this.tabPageReports.Controls.Add(this.groupBoxSettingFontComment);
            this.tabPageReports.Controls.Add(this.comboBoxTableFontFamily);
            this.tabPageReports.Controls.Add(this.lbNameTableFont);
            this.tabPageReports.Controls.Add(this.numericUpDownTableFontSize);
            this.tabPageReports.Controls.Add(this.lbSizeTableFont);
            this.tabPageReports.Controls.Add(this.groupBoxSettingFontTable);
            this.tabPageReports.Controls.Add(this.checkBoxNull_to_zero);
            this.tabPageReports.Controls.Add(this.checkBoxShadingRows);
            this.tabPageReports.Controls.Add(this.groupBoxTypeOfTableMeans);
            this.tabPageReports.Controls.Add(this.cBoxDecimalSeparator);
            this.tabPageReports.Controls.Add(this.lbDecimalSeparator);
            this.tabPageReports.Controls.Add(this.cBoxNumberOfDecimals);
            this.tabPageReports.Controls.Add(this.lbNumberOfDecimals);
            this.tabPageReports.Location = new System.Drawing.Point(4, 22);
            this.tabPageReports.Name = "tabPageReports";
            this.tabPageReports.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageReports.Size = new System.Drawing.Size(570, 289);
            this.tabPageReports.TabIndex = 0;
            this.tabPageReports.Text = "Informes";
            this.tabPageReports.UseVisualStyleBackColor = true;
            // 
            // comboBoxTextFontFamily
            // 
            this.comboBoxTextFontFamily.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTextFontFamily.FormattingEnabled = true;
            this.comboBoxTextFontFamily.Location = new System.Drawing.Point(123, 125);
            this.comboBoxTextFontFamily.Name = "comboBoxTextFontFamily";
            this.comboBoxTextFontFamily.Size = new System.Drawing.Size(249, 21);
            this.comboBoxTextFontFamily.TabIndex = 17;
            this.comboBoxTextFontFamily.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxTextFontFamily_DrawItem);
            this.comboBoxTextFontFamily.SelectedIndexChanged += new System.EventHandler(this.comboBoxTextFontFamily_SelectedIndexChanged);
            // 
            // lbNameTextFont
            // 
            this.lbNameTextFont.AutoSize = true;
            this.lbNameTextFont.Location = new System.Drawing.Point(23, 129);
            this.lbNameTextFont.Name = "lbNameTextFont";
            this.lbNameTextFont.Size = new System.Drawing.Size(40, 13);
            this.lbNameTextFont.TabIndex = 16;
            this.lbNameTextFont.Text = "Fuente";
            // 
            // numericUpDownTextFontSize
            // 
            this.numericUpDownTextFontSize.Location = new System.Drawing.Point(491, 125);
            this.numericUpDownTextFontSize.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.numericUpDownTextFontSize.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownTextFontSize.Name = "numericUpDownTextFontSize";
            this.numericUpDownTextFontSize.Size = new System.Drawing.Size(48, 20);
            this.numericUpDownTextFontSize.TabIndex = 15;
            this.numericUpDownTextFontSize.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // lbSizeTextFont
            // 
            this.lbSizeTextFont.AutoSize = true;
            this.lbSizeTextFont.Location = new System.Drawing.Point(430, 129);
            this.lbSizeTextFont.Name = "lbSizeTextFont";
            this.lbSizeTextFont.Size = new System.Drawing.Size(46, 13);
            this.lbSizeTextFont.TabIndex = 14;
            this.lbSizeTextFont.Text = "Tamaño";
            // 
            // groupBoxSettingFontComment
            // 
            this.groupBoxSettingFontComment.Location = new System.Drawing.Point(11, 106);
            this.groupBoxSettingFontComment.Name = "groupBoxSettingFontComment";
            this.groupBoxSettingFontComment.Size = new System.Drawing.Size(548, 54);
            this.groupBoxSettingFontComment.TabIndex = 18;
            this.groupBoxSettingFontComment.TabStop = false;
            this.groupBoxSettingFontComment.Text = "Fuente de Texto";
            // 
            // comboBoxTableFontFamily
            // 
            this.comboBoxTableFontFamily.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTableFontFamily.FormattingEnabled = true;
            this.comboBoxTableFontFamily.Location = new System.Drawing.Point(123, 61);
            this.comboBoxTableFontFamily.Name = "comboBoxTableFontFamily";
            this.comboBoxTableFontFamily.Size = new System.Drawing.Size(249, 21);
            this.comboBoxTableFontFamily.TabIndex = 11;
            this.comboBoxTableFontFamily.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxFontFamily_DrawItem);
            this.comboBoxTableFontFamily.SelectedIndexChanged += new System.EventHandler(this.comboBoxFontFamily_SelectedIndexChanged);
            // 
            // lbNameTableFont
            // 
            this.lbNameTableFont.AutoSize = true;
            this.lbNameTableFont.Location = new System.Drawing.Point(23, 65);
            this.lbNameTableFont.Name = "lbNameTableFont";
            this.lbNameTableFont.Size = new System.Drawing.Size(40, 13);
            this.lbNameTableFont.TabIndex = 10;
            this.lbNameTableFont.Text = "Fuente";
            // 
            // numericUpDownTableFontSize
            // 
            this.numericUpDownTableFontSize.Location = new System.Drawing.Point(491, 61);
            this.numericUpDownTableFontSize.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.numericUpDownTableFontSize.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownTableFontSize.Name = "numericUpDownTableFontSize";
            this.numericUpDownTableFontSize.Size = new System.Drawing.Size(48, 20);
            this.numericUpDownTableFontSize.TabIndex = 9;
            this.numericUpDownTableFontSize.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // lbSizeTableFont
            // 
            this.lbSizeTableFont.AutoSize = true;
            this.lbSizeTableFont.Location = new System.Drawing.Point(430, 65);
            this.lbSizeTableFont.Name = "lbSizeTableFont";
            this.lbSizeTableFont.Size = new System.Drawing.Size(46, 13);
            this.lbSizeTableFont.TabIndex = 8;
            this.lbSizeTableFont.Text = "Tamaño";
            // 
            // groupBoxSettingFontTable
            // 
            this.groupBoxSettingFontTable.Location = new System.Drawing.Point(11, 42);
            this.groupBoxSettingFontTable.Name = "groupBoxSettingFontTable";
            this.groupBoxSettingFontTable.Size = new System.Drawing.Size(548, 54);
            this.groupBoxSettingFontTable.TabIndex = 13;
            this.groupBoxSettingFontTable.TabStop = false;
            this.groupBoxSettingFontTable.Text = "Fuente de Tabla";
            // 
            // checkBoxNull_to_zero
            // 
            this.checkBoxNull_to_zero.AutoSize = true;
            this.checkBoxNull_to_zero.Location = new System.Drawing.Point(258, 215);
            this.checkBoxNull_to_zero.Name = "checkBoxNull_to_zero";
            this.checkBoxNull_to_zero.Size = new System.Drawing.Size(192, 17);
            this.checkBoxNull_to_zero.TabIndex = 12;
            this.checkBoxNull_to_zero.Text = "Interpretar valores nulos como cero";
            this.checkBoxNull_to_zero.UseVisualStyleBackColor = true;
            // 
            // checkBoxShadingRows
            // 
            this.checkBoxShadingRows.AutoSize = true;
            this.checkBoxShadingRows.Location = new System.Drawing.Point(258, 182);
            this.checkBoxShadingRows.Name = "checkBoxShadingRows";
            this.checkBoxShadingRows.Size = new System.Drawing.Size(84, 17);
            this.checkBoxShadingRows.TabIndex = 7;
            this.checkBoxShadingRows.Text = "Sobrear filas";
            this.checkBoxShadingRows.UseVisualStyleBackColor = true;
            // 
            // groupBoxTypeOfTableMeans
            // 
            this.groupBoxTypeOfTableMeans.Controls.Add(this.radioBtTypePoint);
            this.groupBoxTypeOfTableMeans.Controls.Add(this.radioBtDifference);
            this.groupBoxTypeOfTableMeans.Controls.Add(this.radioBtDefault);
            this.groupBoxTypeOfTableMeans.Location = new System.Drawing.Point(26, 173);
            this.groupBoxTypeOfTableMeans.Name = "groupBoxTypeOfTableMeans";
            this.groupBoxTypeOfTableMeans.Size = new System.Drawing.Size(195, 105);
            this.groupBoxTypeOfTableMeans.TabIndex = 6;
            this.groupBoxTypeOfTableMeans.TabStop = false;
            this.groupBoxTypeOfTableMeans.Text = "Tipo de tabla de medias";
            // 
            // radioBtTypePoint
            // 
            this.radioBtTypePoint.AutoSize = true;
            this.radioBtTypePoint.Location = new System.Drawing.Point(18, 76);
            this.radioBtTypePoint.Name = "radioBtTypePoint";
            this.radioBtTypePoint.Size = new System.Drawing.Size(109, 17);
            this.radioBtTypePoint.TabIndex = 2;
            this.radioBtTypePoint.TabStop = true;
            this.radioBtTypePoint.Text = "Puntuación típica";
            this.radioBtTypePoint.UseVisualStyleBackColor = true;
            // 
            // radioBtDifference
            // 
            this.radioBtDifference.AutoSize = true;
            this.radioBtDifference.Location = new System.Drawing.Point(18, 48);
            this.radioBtDifference.Name = "radioBtDifference";
            this.radioBtDifference.Size = new System.Drawing.Size(73, 17);
            this.radioBtDifference.TabIndex = 1;
            this.radioBtDifference.TabStop = true;
            this.radioBtDifference.Text = "Diferencia";
            this.radioBtDifference.UseVisualStyleBackColor = true;
            // 
            // radioBtDefault
            // 
            this.radioBtDefault.AutoSize = true;
            this.radioBtDefault.Location = new System.Drawing.Point(18, 21);
            this.radioBtDefault.Name = "radioBtDefault";
            this.radioBtDefault.Size = new System.Drawing.Size(80, 17);
            this.radioBtDefault.TabIndex = 0;
            this.radioBtDefault.TabStop = true;
            this.radioBtDefault.Text = "Por defecto";
            this.radioBtDefault.UseVisualStyleBackColor = true;
            // 
            // cBoxDecimalSeparator
            // 
            this.cBoxDecimalSeparator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxDecimalSeparator.FormattingEnabled = true;
            this.cBoxDecimalSeparator.Location = new System.Drawing.Point(415, 13);
            this.cBoxDecimalSeparator.Name = "cBoxDecimalSeparator";
            this.cBoxDecimalSeparator.Size = new System.Drawing.Size(127, 21);
            this.cBoxDecimalSeparator.TabIndex = 5;
            // 
            // lbDecimalSeparator
            // 
            this.lbDecimalSeparator.AutoSize = true;
            this.lbDecimalSeparator.Location = new System.Drawing.Point(277, 16);
            this.lbDecimalSeparator.Name = "lbDecimalSeparator";
            this.lbDecimalSeparator.Size = new System.Drawing.Size(95, 13);
            this.lbDecimalSeparator.TabIndex = 4;
            this.lbDecimalSeparator.Text = "Separador decimal";
            // 
            // cBoxNumberOfDecimals
            // 
            this.cBoxNumberOfDecimals.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxNumberOfDecimals.FormatString = "N0";
            this.cBoxNumberOfDecimals.FormattingEnabled = true;
            this.cBoxNumberOfDecimals.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cBoxNumberOfDecimals.Location = new System.Drawing.Point(175, 13);
            this.cBoxNumberOfDecimals.Name = "cBoxNumberOfDecimals";
            this.cBoxNumberOfDecimals.Size = new System.Drawing.Size(59, 21);
            this.cBoxNumberOfDecimals.TabIndex = 3;
            // 
            // lbNumberOfDecimals
            // 
            this.lbNumberOfDecimals.AutoSize = true;
            this.lbNumberOfDecimals.Location = new System.Drawing.Point(23, 16);
            this.lbNumberOfDecimals.Name = "lbNumberOfDecimals";
            this.lbNumberOfDecimals.Size = new System.Drawing.Size(109, 13);
            this.lbNumberOfDecimals.TabIndex = 2;
            this.lbNumberOfDecimals.Text = "Número de decimales";
            // 
            // tabPageGraphics
            // 
            this.tabPageGraphics.Controls.Add(this.groupBoxChartSetting);
            this.tabPageGraphics.Controls.Add(this.checkBoxErrorAbsStandDev);
            this.tabPageGraphics.Controls.Add(this.checkBoxErrorRelStandDev);
            this.tabPageGraphics.Controls.Add(this.checkBoxTotalAbsErrorVar);
            this.tabPageGraphics.Controls.Add(this.checkBoxTotalRelErrorVar);
            this.tabPageGraphics.Controls.Add(this.checkBox_coefG_Abs);
            this.tabPageGraphics.Controls.Add(this.checkBox_coefG_Rel);
            this.tabPageGraphics.Controls.Add(this.pictBoxColorErrorAbsStandDev);
            this.tabPageGraphics.Controls.Add(this.pictBoxColorErrorRelStandDev);
            this.tabPageGraphics.Controls.Add(this.pictBoxColorTotalAbsErrorVar);
            this.tabPageGraphics.Controls.Add(this.pictBoxColorTotalRelErrorVar);
            this.tabPageGraphics.Controls.Add(this.pictBoxColor_Coef_G_Abs);
            this.tabPageGraphics.Controls.Add(this.pictBoxColor_Coef_G_Rel);
            this.tabPageGraphics.Location = new System.Drawing.Point(4, 22);
            this.tabPageGraphics.Name = "tabPageGraphics";
            this.tabPageGraphics.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGraphics.Size = new System.Drawing.Size(570, 289);
            this.tabPageGraphics.TabIndex = 1;
            this.tabPageGraphics.Text = "Gráficos";
            this.tabPageGraphics.UseVisualStyleBackColor = true;
            // 
            // groupBoxChartSetting
            // 
            this.groupBoxChartSetting.Controls.Add(this.lbMarkerStyle);
            this.groupBoxChartSetting.Controls.Add(this.comboBoxMarkerStyle);
            this.groupBoxChartSetting.Controls.Add(this.comboBoxLabelPoint);
            this.groupBoxChartSetting.Controls.Add(this.lbLabelPoint);
            this.groupBoxChartSetting.Controls.Add(this.comboBoxChartType);
            this.groupBoxChartSetting.Controls.Add(this.lbChartType);
            this.groupBoxChartSetting.Location = new System.Drawing.Point(342, 12);
            this.groupBoxChartSetting.Name = "groupBoxChartSetting";
            this.groupBoxChartSetting.Size = new System.Drawing.Size(199, 225);
            this.groupBoxChartSetting.TabIndex = 12;
            this.groupBoxChartSetting.TabStop = false;
            this.groupBoxChartSetting.Text = "Gráfico Coeficiente G";
            // 
            // lbMarkerStyle
            // 
            this.lbMarkerStyle.AutoSize = true;
            this.lbMarkerStyle.Location = new System.Drawing.Point(9, 128);
            this.lbMarkerStyle.Name = "lbMarkerStyle";
            this.lbMarkerStyle.Size = new System.Drawing.Size(52, 13);
            this.lbMarkerStyle.TabIndex = 5;
            this.lbMarkerStyle.Text = "Marcador";
            // 
            // comboBoxMarkerStyle
            // 
            this.comboBoxMarkerStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMarkerStyle.FormattingEnabled = true;
            this.comboBoxMarkerStyle.Location = new System.Drawing.Point(10, 145);
            this.comboBoxMarkerStyle.Name = "comboBoxMarkerStyle";
            this.comboBoxMarkerStyle.Size = new System.Drawing.Size(168, 21);
            this.comboBoxMarkerStyle.TabIndex = 4;
            // 
            // comboBoxLabelPoint
            // 
            this.comboBoxLabelPoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLabelPoint.FormattingEnabled = true;
            this.comboBoxLabelPoint.Location = new System.Drawing.Point(10, 93);
            this.comboBoxLabelPoint.Name = "comboBoxLabelPoint";
            this.comboBoxLabelPoint.Size = new System.Drawing.Size(168, 21);
            this.comboBoxLabelPoint.TabIndex = 3;
            // 
            // lbLabelPoint
            // 
            this.lbLabelPoint.AutoSize = true;
            this.lbLabelPoint.Location = new System.Drawing.Point(7, 77);
            this.lbLabelPoint.Name = "lbLabelPoint";
            this.lbLabelPoint.Size = new System.Drawing.Size(133, 13);
            this.lbLabelPoint.TabIndex = 2;
            this.lbLabelPoint.Text = "Etiquetas de puntuaciones";
            // 
            // comboBoxChartType
            // 
            this.comboBoxChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxChartType.FormattingEnabled = true;
            this.comboBoxChartType.Location = new System.Drawing.Point(10, 43);
            this.comboBoxChartType.Name = "comboBoxChartType";
            this.comboBoxChartType.Size = new System.Drawing.Size(168, 21);
            this.comboBoxChartType.TabIndex = 1;
            // 
            // lbChartType
            // 
            this.lbChartType.AutoSize = true;
            this.lbChartType.Location = new System.Drawing.Point(7, 23);
            this.lbChartType.Name = "lbChartType";
            this.lbChartType.Size = new System.Drawing.Size(78, 13);
            this.lbChartType.TabIndex = 0;
            this.lbChartType.Text = "Tipo de gráfico";
            // 
            // checkBoxErrorAbsStandDev
            // 
            this.checkBoxErrorAbsStandDev.AutoSize = true;
            this.checkBoxErrorAbsStandDev.Location = new System.Drawing.Point(25, 186);
            this.checkBoxErrorAbsStandDev.Name = "checkBoxErrorAbsStandDev";
            this.checkBoxErrorAbsStandDev.Size = new System.Drawing.Size(132, 17);
            this.checkBoxErrorAbsStandDev.TabIndex = 5;
            this.checkBoxErrorAbsStandDev.Text = "Desv. Típica Absoluta";
            this.checkBoxErrorAbsStandDev.UseVisualStyleBackColor = true;
            // 
            // checkBoxErrorRelStandDev
            // 
            this.checkBoxErrorRelStandDev.AutoSize = true;
            this.checkBoxErrorRelStandDev.Location = new System.Drawing.Point(25, 152);
            this.checkBoxErrorRelStandDev.Name = "checkBoxErrorRelStandDev";
            this.checkBoxErrorRelStandDev.Size = new System.Drawing.Size(130, 17);
            this.checkBoxErrorRelStandDev.TabIndex = 4;
            this.checkBoxErrorRelStandDev.Text = "Desv. Típica Relativa";
            this.checkBoxErrorRelStandDev.UseVisualStyleBackColor = true;
            // 
            // checkBoxTotalAbsErrorVar
            // 
            this.checkBoxTotalAbsErrorVar.AutoSize = true;
            this.checkBoxTotalAbsErrorVar.Location = new System.Drawing.Point(25, 118);
            this.checkBoxTotalAbsErrorVar.Name = "checkBoxTotalAbsErrorVar";
            this.checkBoxTotalAbsErrorVar.Size = new System.Drawing.Size(153, 17);
            this.checkBoxTotalAbsErrorVar.TabIndex = 3;
            this.checkBoxTotalAbsErrorVar.Text = "Varianza del Error Absoluto";
            this.checkBoxTotalAbsErrorVar.UseVisualStyleBackColor = true;
            // 
            // checkBoxTotalRelErrorVar
            // 
            this.checkBoxTotalRelErrorVar.AutoSize = true;
            this.checkBoxTotalRelErrorVar.Location = new System.Drawing.Point(25, 84);
            this.checkBoxTotalRelErrorVar.Name = "checkBoxTotalRelErrorVar";
            this.checkBoxTotalRelErrorVar.Size = new System.Drawing.Size(151, 17);
            this.checkBoxTotalRelErrorVar.TabIndex = 2;
            this.checkBoxTotalRelErrorVar.Text = "Varianza del Error Relativo";
            this.checkBoxTotalRelErrorVar.UseVisualStyleBackColor = true;
            // 
            // checkBox_coefG_Abs
            // 
            this.checkBox_coefG_Abs.AutoSize = true;
            this.checkBox_coefG_Abs.Location = new System.Drawing.Point(25, 50);
            this.checkBox_coefG_Abs.Name = "checkBox_coefG_Abs";
            this.checkBox_coefG_Abs.Size = new System.Drawing.Size(114, 17);
            this.checkBox_coefG_Abs.TabIndex = 1;
            this.checkBox_coefG_Abs.Text = "Coeficiente G Abs.";
            this.checkBox_coefG_Abs.UseVisualStyleBackColor = true;
            // 
            // checkBox_coefG_Rel
            // 
            this.checkBox_coefG_Rel.AutoSize = true;
            this.checkBox_coefG_Rel.Location = new System.Drawing.Point(25, 16);
            this.checkBox_coefG_Rel.Name = "checkBox_coefG_Rel";
            this.checkBox_coefG_Rel.Size = new System.Drawing.Size(112, 17);
            this.checkBox_coefG_Rel.TabIndex = 0;
            this.checkBox_coefG_Rel.Text = "Coeficiente G Rel.";
            this.checkBox_coefG_Rel.UseVisualStyleBackColor = true;
            // 
            // tabPageConect
            // 
            this.tabPageConect.Controls.Add(this.btSelectWorkspace);
            this.tabPageConect.Controls.Add(this.btRemoveUserPass);
            this.tabPageConect.Controls.Add(this.tbPath_Workspace);
            this.tabPageConect.Controls.Add(this.lbWorkspace);
            this.tabPageConect.Controls.Add(this.btLoadDefaultSetting);
            this.tabPageConect.Location = new System.Drawing.Point(4, 22);
            this.tabPageConect.Name = "tabPageConect";
            this.tabPageConect.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConect.Size = new System.Drawing.Size(570, 289);
            this.tabPageConect.TabIndex = 2;
            this.tabPageConect.Text = "Conexión";
            this.tabPageConect.UseVisualStyleBackColor = true;
            // 
            // tbPath_Workspace
            // 
            this.tbPath_Workspace.BackColor = System.Drawing.SystemColors.Window;
            this.tbPath_Workspace.Location = new System.Drawing.Point(19, 51);
            this.tbPath_Workspace.Name = "tbPath_Workspace";
            this.tbPath_Workspace.ReadOnly = true;
            this.tbPath_Workspace.Size = new System.Drawing.Size(533, 20);
            this.tbPath_Workspace.TabIndex = 2;
            // 
            // lbWorkspace
            // 
            this.lbWorkspace.AutoSize = true;
            this.lbWorkspace.Location = new System.Drawing.Point(17, 35);
            this.lbWorkspace.Name = "lbWorkspace";
            this.lbWorkspace.Size = new System.Drawing.Size(95, 13);
            this.lbWorkspace.TabIndex = 1;
            this.lbWorkspace.Text = "Espacio de trabajo";
            // 
            // btOk
            // 
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Image = global::GUI_GT.Properties.Resources.button_ok_h22x22;
            this.btOk.Location = new System.Drawing.Point(285, 340);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(92, 32);
            this.btOk.TabIndex = 0;
            this.btOk.Text = "Aceptar";
            this.btOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Image = global::GUI_GT.Properties.Resources.button_cancel_h22x22;
            this.btCancel.Location = new System.Drawing.Point(384, 340);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(92, 32);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancelar";
            this.btCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // pictBoxColorErrorAbsStandDev
            // 
            this.pictBoxColorErrorAbsStandDev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictBoxColorErrorAbsStandDev.Location = new System.Drawing.Point(196, 186);
            this.pictBoxColorErrorAbsStandDev.Name = "pictBoxColorErrorAbsStandDev";
            this.pictBoxColorErrorAbsStandDev.Size = new System.Drawing.Size(34, 17);
            this.pictBoxColorErrorAbsStandDev.TabIndex = 11;
            this.pictBoxColorErrorAbsStandDev.TabStop = false;
            this.pictBoxColorErrorAbsStandDev.Click += new System.EventHandler(this.pictBoxColorErrorAbsStandDev_Click);
            // 
            // pictBoxColorErrorRelStandDev
            // 
            this.pictBoxColorErrorRelStandDev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictBoxColorErrorRelStandDev.Location = new System.Drawing.Point(196, 152);
            this.pictBoxColorErrorRelStandDev.Name = "pictBoxColorErrorRelStandDev";
            this.pictBoxColorErrorRelStandDev.Size = new System.Drawing.Size(34, 17);
            this.pictBoxColorErrorRelStandDev.TabIndex = 10;
            this.pictBoxColorErrorRelStandDev.TabStop = false;
            this.pictBoxColorErrorRelStandDev.Click += new System.EventHandler(this.pictBoxColorErrorRelStandDev_Click);
            // 
            // pictBoxColorTotalAbsErrorVar
            // 
            this.pictBoxColorTotalAbsErrorVar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictBoxColorTotalAbsErrorVar.Location = new System.Drawing.Point(196, 118);
            this.pictBoxColorTotalAbsErrorVar.Name = "pictBoxColorTotalAbsErrorVar";
            this.pictBoxColorTotalAbsErrorVar.Size = new System.Drawing.Size(34, 17);
            this.pictBoxColorTotalAbsErrorVar.TabIndex = 9;
            this.pictBoxColorTotalAbsErrorVar.TabStop = false;
            this.pictBoxColorTotalAbsErrorVar.Click += new System.EventHandler(this.pictBoxColorTotalAbsErrorVar_Click);
            // 
            // pictBoxColorTotalRelErrorVar
            // 
            this.pictBoxColorTotalRelErrorVar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictBoxColorTotalRelErrorVar.Location = new System.Drawing.Point(196, 84);
            this.pictBoxColorTotalRelErrorVar.Name = "pictBoxColorTotalRelErrorVar";
            this.pictBoxColorTotalRelErrorVar.Size = new System.Drawing.Size(34, 17);
            this.pictBoxColorTotalRelErrorVar.TabIndex = 8;
            this.pictBoxColorTotalRelErrorVar.TabStop = false;
            this.pictBoxColorTotalRelErrorVar.Click += new System.EventHandler(this.pictBoxColorTotalRelErrorVar_Click);
            // 
            // pictBoxColor_Coef_G_Abs
            // 
            this.pictBoxColor_Coef_G_Abs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictBoxColor_Coef_G_Abs.Location = new System.Drawing.Point(196, 50);
            this.pictBoxColor_Coef_G_Abs.Name = "pictBoxColor_Coef_G_Abs";
            this.pictBoxColor_Coef_G_Abs.Size = new System.Drawing.Size(34, 17);
            this.pictBoxColor_Coef_G_Abs.TabIndex = 7;
            this.pictBoxColor_Coef_G_Abs.TabStop = false;
            this.pictBoxColor_Coef_G_Abs.Click += new System.EventHandler(this.pictBoxColor_Coef_G_Abs_Click);
            // 
            // pictBoxColor_Coef_G_Rel
            // 
            this.pictBoxColor_Coef_G_Rel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictBoxColor_Coef_G_Rel.Location = new System.Drawing.Point(196, 16);
            this.pictBoxColor_Coef_G_Rel.Name = "pictBoxColor_Coef_G_Rel";
            this.pictBoxColor_Coef_G_Rel.Size = new System.Drawing.Size(34, 17);
            this.pictBoxColor_Coef_G_Rel.TabIndex = 6;
            this.pictBoxColor_Coef_G_Rel.TabStop = false;
            this.pictBoxColor_Coef_G_Rel.Click += new System.EventHandler(this.pictBoxColor_Coef_G_Rel_Click);
            // 
            // btSelectWorkspace
            // 
            this.btSelectWorkspace.Image = global::GUI_GT.Properties.Resources.open_folder_h22;
            this.btSelectWorkspace.Location = new System.Drawing.Point(377, 86);
            this.btSelectWorkspace.Name = "btSelectWorkspace";
            this.btSelectWorkspace.Size = new System.Drawing.Size(176, 35);
            this.btSelectWorkspace.TabIndex = 4;
            this.btSelectWorkspace.Text = "Seleccionar espacio de trabajo";
            this.btSelectWorkspace.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btSelectWorkspace.UseVisualStyleBackColor = true;
            this.btSelectWorkspace.Click += new System.EventHandler(this.btSelectWorkspace_Click);
            // 
            // btRemoveUserPass
            // 
            this.btRemoveUserPass.Image = global::GUI_GT.Properties.Resources.Recycle_Bin_h22x22;
            this.btRemoveUserPass.Location = new System.Drawing.Point(297, 232);
            this.btRemoveUserPass.Name = "btRemoveUserPass";
            this.btRemoveUserPass.Size = new System.Drawing.Size(123, 35);
            this.btRemoveUserPass.TabIndex = 3;
            this.btRemoveUserPass.Text = "Eliminar contraseñas";
            this.btRemoveUserPass.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btRemoveUserPass.UseVisualStyleBackColor = true;
            this.btRemoveUserPass.Click += new System.EventHandler(this.btRemoveUserPass_Click);
            // 
            // btLoadDefaultSetting
            // 
            this.btLoadDefaultSetting.Image = global::GUI_GT.Properties.Resources.Configuration_Settings_22x22;
            this.btLoadDefaultSetting.Location = new System.Drawing.Point(430, 232);
            this.btLoadDefaultSetting.Name = "btLoadDefaultSetting";
            this.btLoadDefaultSetting.Size = new System.Drawing.Size(123, 35);
            this.btLoadDefaultSetting.TabIndex = 0;
            this.btLoadDefaultSetting.Text = "Cargar los valores por defecto";
            this.btLoadDefaultSetting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btLoadDefaultSetting.UseVisualStyleBackColor = true;
            this.btLoadDefaultSetting.Click += new System.EventHandler(this.btLoadDefaultSetting_Click);
            // 
            // btHelp
            // 
            this.btHelp.Image = global::GUI_GT.Properties.Resources.HelpIcon_22x22;
            this.btHelp.Location = new System.Drawing.Point(483, 340);
            this.btHelp.Name = "btHelp";
            this.btHelp.Size = new System.Drawing.Size(92, 32);
            this.btHelp.TabIndex = 2;
            this.btHelp.Text = "Ayuda";
            this.btHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btHelp.UseVisualStyleBackColor = true;
            this.btHelp.Click += new System.EventHandler(this.btHelp_Click);
            // 
            // FormSettings
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(584, 384);
            this.Controls.Add(this.tabControlSettings);
            this.Controls.Add(this.btHelp);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.Text = "Configuración";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.tabControlSettings.ResumeLayout(false);
            this.tabPageReports.ResumeLayout(false);
            this.tabPageReports.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTextFontSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTableFontSize)).EndInit();
            this.groupBoxTypeOfTableMeans.ResumeLayout(false);
            this.groupBoxTypeOfTableMeans.PerformLayout();
            this.tabPageGraphics.ResumeLayout(false);
            this.tabPageGraphics.PerformLayout();
            this.groupBoxChartSetting.ResumeLayout(false);
            this.groupBoxChartSetting.PerformLayout();
            this.tabPageConect.ResumeLayout(false);
            this.tabPageConect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxColorErrorAbsStandDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxColorErrorRelStandDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxColorTotalAbsErrorVar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxColorTotalRelErrorVar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxColor_Coef_G_Abs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxColor_Coef_G_Rel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btHelp;
        private System.Windows.Forms.TabControl tabControlSettings;
        private System.Windows.Forms.TabPage tabPageReports;
        private System.Windows.Forms.TabPage tabPageGraphics;
        private System.Windows.Forms.ComboBox cBoxNumberOfDecimals;
        private System.Windows.Forms.Label lbNumberOfDecimals;
        private System.Windows.Forms.Label lbDecimalSeparator;
        private System.Windows.Forms.ComboBox cBoxDecimalSeparator;
        private System.Windows.Forms.CheckBox checkBoxErrorAbsStandDev;
        private System.Windows.Forms.CheckBox checkBoxErrorRelStandDev;
        private System.Windows.Forms.CheckBox checkBoxTotalAbsErrorVar;
        private System.Windows.Forms.CheckBox checkBoxTotalRelErrorVar;
        private System.Windows.Forms.CheckBox checkBox_coefG_Abs;
        private System.Windows.Forms.CheckBox checkBox_coefG_Rel;
        private System.Windows.Forms.GroupBox groupBoxTypeOfTableMeans;
        private System.Windows.Forms.RadioButton radioBtTypePoint;
        private System.Windows.Forms.RadioButton radioBtDifference;
        private System.Windows.Forms.RadioButton radioBtDefault;
        private System.Windows.Forms.CheckBox checkBoxShadingRows;
        private System.Windows.Forms.Label lbSizeTableFont;
        private System.Windows.Forms.NumericUpDown numericUpDownTableFontSize;
        private System.Windows.Forms.Label lbNameTableFont;
        private System.Windows.Forms.ComboBox comboBoxTableFontFamily;
        private System.Windows.Forms.CheckBox checkBoxNull_to_zero;
        private System.Windows.Forms.GroupBox groupBoxSettingFontTable;
        private System.Windows.Forms.ComboBox comboBoxTextFontFamily;
        private System.Windows.Forms.Label lbNameTextFont;
        private System.Windows.Forms.NumericUpDown numericUpDownTextFontSize;
        private System.Windows.Forms.Label lbSizeTextFont;
        private System.Windows.Forms.GroupBox groupBoxSettingFontComment;
        private System.Windows.Forms.PictureBox pictBoxColor_Coef_G_Rel;
        private System.Windows.Forms.ColorDialog colorChartDialog;
        private System.Windows.Forms.PictureBox pictBoxColor_Coef_G_Abs;
        private System.Windows.Forms.PictureBox pictBoxColorErrorAbsStandDev;
        private System.Windows.Forms.PictureBox pictBoxColorErrorRelStandDev;
        private System.Windows.Forms.PictureBox pictBoxColorTotalAbsErrorVar;
        private System.Windows.Forms.PictureBox pictBoxColorTotalRelErrorVar;
        private System.Windows.Forms.TabPage tabPageConect;
        private System.Windows.Forms.TextBox tbPath_Workspace;
        private System.Windows.Forms.Label lbWorkspace;
        private System.Windows.Forms.Button btLoadDefaultSetting;
        private System.Windows.Forms.Button btRemoveUserPass;
        private System.Windows.Forms.GroupBox groupBoxChartSetting;
        private System.Windows.Forms.ComboBox comboBoxLabelPoint;
        private System.Windows.Forms.Label lbLabelPoint;
        private System.Windows.Forms.ComboBox comboBoxChartType;
        private System.Windows.Forms.Label lbChartType;
        private System.Windows.Forms.Label lbMarkerStyle;
        private System.Windows.Forms.ComboBox comboBoxMarkerStyle;
        private System.Windows.Forms.Button btSelectWorkspace;
    }
}