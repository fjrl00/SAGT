/* 
 * Proyecto: SOFTWARE PARA LA APLICACIÓN DE LA TEORÍA DE LA GENERALIZABILIDAD
 * Nº de orden: 4778
 * 
 * Alumno:   Francisco Jesús Ramos Pérez
 * 
 * Directores de Proyecto:
 *          Dr. Don José Luis Pastrana Brincones
 *          Dr. Don Antonio Hernández Mendo
 * 
 * Fecha de revisión: 02/Jul/2012                           
 * 
 * Descripción:
 *      Ventana con las opciones de configuración.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text; // para poder usar InstalledFontCollection
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ConfigCFG;

namespace GUI_GT
{
    public partial class FormSettings : Form
    {
        /*-------------------------------------------------------------------------------------
         * Constantes
         *-------------------------------------------------------------------------------------*/

        // nombre del archivo que contiene las traducciones
        const string STRING_TEXT = "formSettings.txt";
        const string LANG_PATH = "\\lang\\";


        /*-------------------------------------------------------------------------------------
         * Variables
         *-------------------------------------------------------------------------------------*/

        // variables 
        private InstalledFontCollection installedFonts = new InstalledFontCollection();
        ConfigCFG.ConfigCFG cfg;
        int posIndexComboBox;
        private string toolTipText = "Haga doble clic para seleccionar el color.";
        private string errorFileInUse = "El fichero está en uso";
        private string messageRemoveFile = "Se han eliminado las contraseñas";
        private string pathManual;
        private string defaultWorkspace;
        string filepass;

        /*-------------------------------------------------------------------------------------
         * Constructores
         *-------------------------------------------------------------------------------------*/

        /* Descripción:
         *  Inicializa las componentes.
         */
        public FormSettings()
        {
            InitializeComponent();
        }


        /* Descripción:
         *  Inicializa las componentes.
         * Parámetros:
         *      TransLibrary.Language lang: idioma para la traducción de los etiquetas de la ventana.
         */
        public FormSettings(ConfigCFG.ConfigCFG config, string pathManual, string filepass, string defaultWorkspace)
        {
            this.cfg = config;
            this.pathManual = pathManual;
            this.filepass = filepass;
            this.defaultWorkspace = defaultWorkspace;
            InitializeComponent();
            traslationElements(cfg.GetConfigLanguage(), Application.StartupPath + LANG_PATH + STRING_TEXT);
            this.cBoxNumberOfDecimals.SelectedIndex = cfg.GetNumberOfDecimals();
            this.checkBoxNull_to_zero.Checked = cfg.GetNull_to_Zero();
            this.tbPath_Workspace.Text = cfg.Get_Path_Workspace();
            this.InitSettingReports();
            this.InitCheckBoxCharts();
            this.InitColorPictureBox();
            InitGroupBoxTypeOfTableMeans();
            InitChartComboBox();
        }


        /*-------------------------------------------------------------------------------------
         * Métodos
         *-------------------------------------------------------------------------------------*/

        #region ComboBox de tipos de fuentes

        private void FormSettings_Load(object sender, EventArgs e)
        {
            // ComboBox de Fuentes de tabla
            comboBoxTableFontFamily.DataSource = installedFonts.Families;
            comboBoxTableFontFamily.DisplayMember = "Name";

            comboBoxTableFontFamily.DrawMode = DrawMode.OwnerDrawFixed;

            int n = comboBoxTableFontFamily.FindString(this.cfg.GetTableFontFamily());
            comboBoxTableFontFamily.SelectedIndex = n;

            // ComboBox de Fuentes de texto
            comboBoxTextFontFamily.DataSource = installedFonts.Families;
            comboBoxTextFontFamily.DisplayMember = "Name";

            comboBoxTextFontFamily.DrawMode = DrawMode.OwnerDrawFixed;

            n = comboBoxTextFontFamily.FindString(this.cfg.GetTextFontFamily());
            comboBoxTextFontFamily.SelectedIndex = n;
        }


        /* Descripción:
         *  Carga el comboBox con todos los nombres de todos tipos de letras instalados, mostrados cada
         *  uno con su respectiva fuente. Los datos se cargan en el comboBox de la fuentes de tabla.
         */
        private void comboBoxFontFamily_DrawItem(object sender, DrawItemEventArgs e)
        {
            FontFamily family = installedFonts.Families[e.Index];
            FontStyle style = FontStyle.Regular;
            if (!family.IsStyleAvailable(style))
                style = FontStyle.Bold;
            if (!family.IsStyleAvailable(style))
                style = FontStyle.Italic;
            Font fnt = new Font(family, 10, style);
            Brush brush;
            if (e.State == DrawItemState.Selected)
            {
                brush = new SolidBrush(Color.White);
            }
            else
            {
                brush = new SolidBrush(comboBoxTableFontFamily.ForeColor);
            }

            e.DrawBackground();
            e.Graphics.DrawString(family.GetName(0),
                                  fnt, brush, e.Bounds.Location);
        }


        /* Descripción:
         *  Evento que se lanza al cambiar el indice en el comboBox de Fuente de tabla.
         */
        private void comboBoxFontFamily_SelectedIndexChanged(object sender, EventArgs e)
        {

            FontFamily family = this.installedFonts.Families[comboBoxTableFontFamily.SelectedIndex];
            
            FontStyle style = FontStyle.Regular;
            if (!family.IsStyleAvailable(style))
                style = FontStyle.Bold;
            if (!family.IsStyleAvailable(style))
                style = FontStyle.Italic;
        }


        /* Descripción:
         *  Carga el comboBox con todos los nombres de todos tipos de letras instalados, mostrados cada
         *  uno con su respectiva fuente. Los datos se cargan en el comboBox de la fuentes de texto.
         */
        private void comboBoxTextFontFamily_DrawItem(object sender, DrawItemEventArgs e)
        {
            FontFamily family = installedFonts.Families[e.Index];
            FontStyle style = FontStyle.Regular;
            if (!family.IsStyleAvailable(style))
                style = FontStyle.Bold;
            if (!family.IsStyleAvailable(style))
                style = FontStyle.Italic;
            Font fnt = new Font(family, 10, style);
            Brush brush;
            if (e.State == DrawItemState.Selected)
            {
                brush = new SolidBrush(Color.White);
            }
            else
            {
                brush = new SolidBrush(comboBoxTextFontFamily.ForeColor);
            }

            e.DrawBackground();
            e.Graphics.DrawString(family.GetName(0),
                                  fnt, brush, e.Bounds.Location);
        }


        /* Descripción:
         *  Evento que se lanza al cambiar el indice en el comboBox de Fuente de texto.
         */
        private void comboBoxTextFontFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            FontFamily family = this.installedFonts.Families[comboBoxTextFontFamily.SelectedIndex];

            FontStyle style = FontStyle.Regular;
            if (!family.IsStyleAvailable(style))
                style = FontStyle.Bold;
            if (!family.IsStyleAvailable(style))
                style = FontStyle.Italic;
        }


        #endregion ComboBox de tipos de fuentes


        /* Descripción:
         *  Inicializa los valores de los checkBox de la pestaña Gráficos a través de los datos
         *  de la configuración.
         */
        private void InitCheckBoxCharts()
        {
            this.checkBox_coefG_Abs.Checked = this.cfg.GetCheck_coefG_Abs();
            this.checkBox_coefG_Rel.Checked = this.cfg.GetCheck_coefG_Rel();
            this.checkBoxErrorAbsStandDev.Checked = this.cfg.GetCheckErrorAbsStandDev();
            this.checkBoxErrorRelStandDev.Checked = this.cfg.GetCheckErrorRelStandDev();
            this.checkBoxTotalAbsErrorVar.Checked = this.cfg.GetCheckTotalAbsErrorVar();
            this.checkBoxTotalRelErrorVar.Checked = this.cfg.GetCheckTotalRelErrorVar();
        }


        /* Descripción:
         *  Inicializa los pictureBox con los colores
         */
        private void InitColorPictureBox()
        {
            this.pictBoxColor_Coef_G_Rel.BackColor = this.cfg.GetColor_coefG_Rel();
            InitTooltip(this.pictBoxColor_Coef_G_Rel, this.toolTipText);
            this.pictBoxColor_Coef_G_Abs.BackColor = this.cfg.GetColor_coefG_Abs();
            InitTooltip(this.pictBoxColor_Coef_G_Abs, this.toolTipText);
            this.pictBoxColorErrorAbsStandDev.BackColor = this.cfg.GetColorErrorAbsStandDev();
            InitTooltip(this.pictBoxColorErrorAbsStandDev, this.toolTipText);
            this.pictBoxColorErrorRelStandDev.BackColor = this.cfg.GetColorErrorRelStandDev();
            InitTooltip(this.pictBoxColorErrorRelStandDev, this.toolTipText);
            this.pictBoxColorTotalAbsErrorVar.BackColor = this.cfg.GetColorTotalAbsErrorVar();
            InitTooltip(this.pictBoxColorTotalAbsErrorVar, this.toolTipText);
            this.pictBoxColorTotalRelErrorVar.BackColor = this.cfg.GetColorTotalRelErrorVar();
            InitTooltip(this.pictBoxColorTotalRelErrorVar, this.toolTipText);
        }


        /* Descripción:
         *  Crea un toolTip al control que se le pasa como primer parámetro con el texto que se le pasa
         *  como segundo parámetros.
         */
        private void InitTooltip(Control c, string text)
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(c, text);
        }


        /* Descripción:
         *  Carga los datos del informe como el sobreado de las filas en los informes, tamaño de la fuente.
         */
        private void InitSettingReports()
        {
            this.checkBoxShadingRows.Checked = this.cfg.GetShadingRows();
            this.numericUpDownTableFontSize.Value = this.cfg.GetTableFontSize();
            this.numericUpDownTextFontSize.Value = this.cfg.GetTextFontSize();
        }


        /* Decripción:
         *  Inicializa los radiobutton de tipo de tabla de medias con la opción marcada en 
         *  la configuración.
         */
        private void InitGroupBoxTypeOfTableMeans()
        {
            TypeOfTableMeans typeOfTable = cfg.GetTypeOfTableMeans();
            switch (typeOfTable)
            {
                case (TypeOfTableMeans.Default):
                    radioBtDefault.Checked = true;
                    break;
                case (TypeOfTableMeans.TableMeansDif):
                    radioBtDifference.Checked = true;
                    break;
                case (TypeOfTableMeans.TableMeansTipPoint):
                    radioBtTypePoint.Checked = true;
                    break;
                default:
                    throw new ConfigCFG.ConfigCFGException("Error en el archivo de configuración");
            }
        }


        /* Descripción:
         *  Inicializa el los comboBox de representación de los gráficos de Coeficiente G.
         */
        private void InitChartComboBox()
        {
            this.comboBoxChartType.Items.AddRange(new object[] {
            "Line",
            "Spline"});
            this.comboBoxChartType.SelectedIndex = this.comboBoxChartType.FindStringExact(cfg.GetSerieChartType().ToString());

            this.comboBoxLabelPoint.Items.AddRange(new object[] {
            "None",
            "Top",
            "TopLeft",
            "TopRight",
            "Right",
            "Bottom",
            "BottomLeft",
            "BottomRight",
            "Left",
            "Center"});

            string lp = cfg.GetLabelAlignmentStyles().ToString();
            this.comboBoxLabelPoint.SelectedIndex = this.comboBoxLabelPoint.FindStringExact(lp);

            this.comboBoxMarkerStyle.Items.AddRange(new object[] {
                "Circle", "Cross", "Diamond", "None", "Square", "Star10", "Star4", "Star5", "Star6", "Triangle"});
            string ms = cfg.GetMarkerStyle().ToString();
            this.comboBoxMarkerStyle.SelectedIndex = this.comboBoxMarkerStyle.FindStringExact(ms);
        }// end InitChartComboBox



        /* Descipción: 
         *  Asigna a la configuración el tipo de tabla seleccionado.
         */
        private void CheckedGroupBoxTypeOfTableMeans()
        {
            if (this.radioBtDifference.Checked)
            {
                this.cfg.SetTypeTableOfMeans(TypeOfTableMeans.TableMeansDif);
            }
            else if (this.radioBtTypePoint.Checked)
            {
                this.cfg.SetTypeTableOfMeans(TypeOfTableMeans.TableMeansTipPoint);
            }
            else
            {
                this.cfg.SetTypeTableOfMeans(TypeOfTableMeans.Default);
            }
        }


        /* Descripción:
         *  Carga los datos de los checkBox de la pestaña gráficos en la variable de configuración.
         */
        private void SaveSettingsChartsCheckBoxs()
        {
            this.cfg.SetCheck_coefG_Abs(this.checkBox_coefG_Abs.Checked);
            this.cfg.SetCheck_coefG_Rel(this.checkBox_coefG_Rel.Checked);
            this.cfg.SetCheckErrorAbsStandDev(this.checkBoxErrorAbsStandDev.Checked);
            this.cfg.SetCheckErrorRelStandDev(this.checkBoxErrorRelStandDev.Checked);
            this.cfg.SetCheckTotalAbsErrorVar(this.checkBoxTotalAbsErrorVar.Checked);
            this.cfg.SetCheckTotalRelErrorVar(this.checkBoxTotalRelErrorVar.Checked);
        }


        /* Descripción:
         *  Asigna el valor del checkBox "Sobrear filas" a la variable correspondiente de configuración.
         */
        private void SaveSettingReports()
        {
            this.cfg.SetShadingRows(this.checkBoxShadingRows.Checked);
            int n = Decimal.ToInt32(this.numericUpDownTableFontSize.Value);
            this.cfg.SetTableFontSize(n);
            n = Decimal.ToInt32(this.numericUpDownTextFontSize.Value);
            this.cfg.SetTextFontSize(n);
            string fontName = this.installedFonts.Families[comboBoxTableFontFamily.SelectedIndex].Name.ToString();
            this.cfg.SetTableFontFamily(fontName);
            fontName = this.installedFonts.Families[comboBoxTextFontFamily.SelectedIndex].Name.ToString();
            this.cfg.SetTextFontFamily(fontName);
        }


        /* Descripción:
         *  Devuelve true si se ha producido cambios en los checkBox de la pestaña gráficos con respecto
         *  de la variable de configuración.
         */
        private bool ChekingChangeCheckBox()
        {
            return !this.cfg.GetCheck_coefG_Abs().Equals(this.checkBox_coefG_Abs.Checked) 
            || !this.cfg.GetCheck_coefG_Rel().Equals(this.checkBox_coefG_Rel.Checked)
            || !this.cfg.GetCheckErrorAbsStandDev().Equals(this.checkBoxErrorAbsStandDev.Checked)
            || !this.cfg.GetCheckErrorRelStandDev().Equals(this.checkBoxErrorRelStandDev.Checked)
            || !this.cfg.GetCheckTotalAbsErrorVar().Equals(this.checkBoxTotalAbsErrorVar.Checked)
            || !this.cfg.GetCheckTotalRelErrorVar().Equals(this.checkBoxTotalRelErrorVar.Checked);
        }


        /* Descripción:
         *  Cierra la ventana de Configuración.
         */
        private void btCancel_Click(object sender, EventArgs e)
        {
            // this.Close();
        }


        /* Descripción:
         *  Devuelve la variable de configuración tras actualizarla con los parámetros indicados 
         *  por el usuario.
         */
        public ConfigCFG.ConfigCFG UpdateConfig()
        {
            // Actualizamos la tabla de medias seleccionada
            CheckedGroupBoxTypeOfTableMeans();
            // Actualizamos el separador decimal y el número de decimales y si tomamos el valor null como zero
            DecimalSetting();
            // Actualizamos la configuración de los informes
            SaveSettingReports();
            // Si se han modificado los checkBox de gráficos los actualizamos.
            if (this.ChekingChangeCheckBox())
            {
                this.SaveSettingsChartsCheckBoxs();
            }
            this.cfg.Set_Path_Workspace(this.tbPath_Workspace.Text);
            // Actualizamos el tipo de gráfico
            SerieChartTypeSetting();
            LabelAlignmentStyles();
            MarkerStyle();
            return this.cfg;
        }


        /* Descripción:
         *  Guarda los cambios y cerramos la ventana.
         */
        private void btOk_Click(object sender, EventArgs e)
        {
            //CheckedGroupBoxTypeOfTableMeans();
            //// Optenemos el path del fichero de configuración.
            //string pathDocuments = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //string pathSagtDir = pathDocuments + "\\" + FormPrincipal.SAGT_DIR;

            //// cfg.WriteFileConfig(pathSagtDir);

            //cfg = this.UpdateConfig();
            //// Guardamos el archivo de configuración en el directorio "\Documents\SAGT"
            //cfg.WriteFileConfig(pathSagtDir);
            // this.Close();
        }// end btOk_Click


        /* Descripción:
         *  Actualiza la variable de configuración (ConfigCFG.ConfigCFG cfg) con el valor del 
         *  separador decimal y el número de decimales indicados por el usuario
         */
        private void DecimalSetting()
        {
            cfg.SetNull_to_Zero(this.checkBoxNull_to_zero.Checked);
            int newNumberOfDecimals = this.cBoxNumberOfDecimals.SelectedIndex;
           
            if (!cfg.GetNumberOfDecimals().Equals(newNumberOfDecimals)
                || !this.posIndexComboBox.Equals(this.cBoxDecimalSeparator.SelectedIndex))
            {
                cfg.SetNumberOfDecimal(newNumberOfDecimals);
                int pos = this.cBoxDecimalSeparator.SelectedIndex;
                switch (pos)
                {
                    case (0):
                        // Si el separador decimal es un punto almaceno dicho valor
                        cfg.SetDecimalSeparator(ConfigCFG.ConfigCFG.DECIMAL_SEPARATOR_PERIOD);
                        break;
                    case (1):
                        // Si el separador decimal es una coma almaceno dicho valor
                         cfg.SetDecimalSeparator(ConfigCFG.ConfigCFG.DECIMAL_SEPARATOR_COMMA);
                        break;
                }
                // formPrincipal.RefreshDataInAllData();
            }
        }


        /* Descripción:
         *  Actualiza la variable de tipo de gráfica
         */
        private void SerieChartTypeSetting()
        {
            int pos = this.comboBoxChartType.SelectedIndex;
            switch (pos)
            {
                case(0):
                    cfg.SetSerieChartType(SeriesChartType.Line);
                    break;
                case(1):
                    cfg.SetSerieChartType(SeriesChartType.Spline);
                    break;
            }
        }


        /* Descripción:
         *  Actuliza la configuración para la posición en la que se muestran las etiquetas.
         */
        private void LabelAlignmentStyles()
        {
            string textSeleted = this.comboBoxLabelPoint.SelectedItem.ToString();
            ConfigCFG.LabelAlignmentStyles labelAlig = 
                (ConfigCFG.LabelAlignmentStyles)Enum.Parse(typeof(ConfigCFG.LabelAlignmentStyles), textSeleted);
            cfg.SetLabelAlignmentStyles(labelAlig);
        }


        /* Descripción:
         *  Marca del estilo de puntos de la gráfica de coeficiente G
         */
        private void MarkerStyle()
        {
            string textSeleted = this.comboBoxMarkerStyle.SelectedItem.ToString();
            MarkerStyle markerStyle =
                (MarkerStyle)Enum.Parse(typeof(MarkerStyle), textSeleted);
            cfg.SetMarkerStyle(markerStyle);
        }


        /*
         * Descripción:
         *  Muestra la ventana de ayuda tras hacer click en el boton ayuda de la ventana de configuración
         */
        private void btHelp_Click(object sender, EventArgs e)
        {
            // this.formPrincipal.ShowWindowsHelp();
            Help.ShowHelp(this, pathManual,HelpNavigator.KeywordIndex, "Configuración");
        }


        /* Descripción:
         *  Carga los valores por defecto a excepción del idioma que se mantiene.
         */
        private void btLoadDefaultSetting_Click(object sender, EventArgs e)
        {
            ConfigCFG.ConfigCFG defaultCFG = new ConfigCFG.ConfigCFG();
            TransLibrary.Language lang = cfg.GetConfigLanguage();
            defaultCFG.SetConfigLanguage(lang);
            this.cfg = defaultCFG;
            traslationElements(cfg.GetConfigLanguage(), Application.StartupPath + LANG_PATH + STRING_TEXT);
            this.cBoxNumberOfDecimals.SelectedIndex = cfg.GetNumberOfDecimals();
            this.checkBoxNull_to_zero.Checked = cfg.GetNull_to_Zero();
            this.cfg.Set_Path_Workspace(this.defaultWorkspace);
            this.tbPath_Workspace.Text = this.cfg.Get_Path_Workspace();
            this.InitSettingReports();
            this.InitCheckBoxCharts();
            this.InitColorPictureBox();
            InitGroupBoxTypeOfTableMeans();
            // Cargamos las fuentes
            int n = comboBoxTableFontFamily.FindString(this.cfg.GetTableFontFamily());
            comboBoxTableFontFamily.SelectedIndex = n;
            n = comboBoxTextFontFamily.FindString(this.cfg.GetTextFontFamily());
            comboBoxTextFontFamily.SelectedIndex = n;
        }




        /* Descripción:
         *  Elimina el fichero con las claves de usuario 
         */
        private void btRemoveUserPass_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(filepass))
                {
                    File.Delete(filepass);
                    MessageBox.Show(messageRemoveFile);
                }
            }
            catch (IOException)
            {
                // No se ha eliminado porque el fichero esta en uso
                MessageBox.Show(errorFileInUse);
            }
        }


        /* Descripción:
         *  Permite seleccionar el directorio de espacio de trabajo
         */
        private void btSelectWorkspace_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb_dialog = new FolderBrowserDialog();
            DialogResult res = fb_dialog.ShowDialog();
            if (res.Equals(DialogResult.OK))
            {
                this.tbPath_Workspace.Text = fb_dialog.SelectedPath;
            }
        }// end btSelectWorkspace_Click



        #region Asignación de colores para los gráficos (Eventos de pulsación sobre los cuadros de color)

        /* Descripción:
         *  Lanza la ventana para la selección de colores
         */
        private Color SelectColor(Color c)
        {
            Color retVal = c;
            ColorDialog colorDlg = new ColorDialog();

            colorDlg.AllowFullOpen = false;
            colorDlg.AnyColor = true;
            colorDlg.SolidColorOnly = false;
            colorDlg.Color = Color.Red;

            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                retVal = colorDlg.Color;
            }
            return retVal;
        }


        /* Descripción:
         *  Selecciona el color para representar el Coeficiente G Relativo haciendo doble click en 
         *  el cuadro de color.
         */
        private void pictBoxColor_Coef_G_Rel_Click(object sender, EventArgs e)
        {
            this.pictBoxColor_Coef_G_Rel.BackColor = SelectColor(this.pictBoxColor_Coef_G_Rel.BackColor);
            this.cfg.SetColor_coefG_Rel(this.pictBoxColor_Coef_G_Rel.BackColor);
        }


        /* Descripción:
         *  Selecciona el color para representar el Coeficiente G Absoluto haciendo doble click en 
         *  el cuadro de color.
         */
        private void pictBoxColor_Coef_G_Abs_Click(object sender, EventArgs e)
        {
            this.pictBoxColor_Coef_G_Abs.BackColor = SelectColor(this.pictBoxColor_Coef_G_Abs.BackColor);
            this.cfg.SetColor_coefG_Abs(this.pictBoxColor_Coef_G_Abs.BackColor);
        }


        /* Descripción:
         *  Selecciona el color para representar la Varianza de Error Relativa haciendo doble click en 
         *  el cuadro de color.
         */
        private void pictBoxColorTotalRelErrorVar_Click(object sender, EventArgs e)
        {
            this.pictBoxColorTotalRelErrorVar.BackColor = SelectColor(this.pictBoxColorTotalRelErrorVar.BackColor);
            this.cfg.SetColorTotalRelErrorVar(this.pictBoxColorTotalRelErrorVar.BackColor);
        }


        /* Descripción:
         *  Selecciona el color para representar la Varianza de Error Absoluta haciendo doble click en 
         *  el cuadro de color.
         */
        private void pictBoxColorTotalAbsErrorVar_Click(object sender, EventArgs e)
        {
            this.pictBoxColorTotalAbsErrorVar.BackColor = SelectColor(this.pictBoxColorTotalAbsErrorVar.BackColor);
            this.cfg.SetColorTotalAbsErrorVar(this.pictBoxColorTotalAbsErrorVar.BackColor);
        }


        /* Descripción:
         *  Selecciona el color para representar la Error Relativo Estandar haciendo doble click en 
         *  el cuadro de color.
         */
        private void pictBoxColorErrorRelStandDev_Click(object sender, EventArgs e)
        {
            this.pictBoxColorErrorRelStandDev.BackColor = SelectColor(this.pictBoxColorErrorRelStandDev.BackColor);
            this.cfg.SetColorErrorRelStandDev(this.pictBoxColorErrorRelStandDev.BackColor);
        }


        /* Descripción:
         *  Selecciona el color para representar la Error Absoluto Estandar haciendo doble click en 
         *  el cuadro de color.
         */
        private void pictBoxColorErrorAbsStandDev_Click(object sender, EventArgs e)
        {
            this.pictBoxColorErrorAbsStandDev.BackColor = SelectColor(this.pictBoxColorErrorAbsStandDev.BackColor);
            this.cfg.SetColorErrorAbsStandDev(this.pictBoxColorErrorAbsStandDev.BackColor);
        }

        #endregion Asignación de colores para los gráficos (Eventos de pulsación sobre los cuadros de color)



        #region Traducción de la ventana
        /*======================================================================================
         * Traducción de la ventana
         *======================================================================================*/

        /*
         * Descripción:
         *  Traduce todos los textos de la ventana al idioma que se encuentre activo
         */
        private void traslationElements(TransLibrary.Language lang, string pathFileTrans)
        {
            TransLibrary.ReadFileTrans dic = new TransLibrary.ReadFileTrans(pathFileTrans);
            string name = "";
            try
            {
                // Traducimos los Textos de la ventana
                //====================================

                // Traducimos el título de la ventana
                name = this.Name.ToString();
                this.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el botón aceptar
                name = this.btOk.Name.ToString();
                this.btOk.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el botón cancelar
                name = this.btCancel.Name.ToString();
                this.btCancel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el botón ayuda
                name = this.btHelp.Name.ToString();
                this.btHelp.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                

                // Traducimos las pestañas del tabControl
                // [TabPage Informes]
                name = this.tabPageReports.Name.ToString();
                this.tabPageReports.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // TabPage Gráficos
                name = this.tabPageGraphics.Name.ToString();
                this.tabPageGraphics.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // [TabPage Conexión]
                name = this.tabPageConect.Name.ToString();
                this.tabPageConect.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el botón de borrar contraseñas
                name = this.btRemoveUserPass.Name.ToString();
                this.btRemoveUserPass.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el botón cargar los valores por defecto
                name = this.btLoadDefaultSetting.Name.ToString();
                this.btLoadDefaultSetting.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta espacio de trabajo
                name = this.lbWorkspace.Name.ToString();
                this.lbWorkspace.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el botón espacio de trabajo 
                name = this.btSelectWorkspace.Name.ToString();
                this.btSelectWorkspace.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos etiquetas
                name = this.lbNumberOfDecimals.Name.ToString();
                this.lbNumberOfDecimals.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbDecimalSeparator.Name.ToString();
                this.lbDecimalSeparator.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbSizeTableFont.Name.ToString();
                this.lbSizeTableFont.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                this.lbSizeTextFont.Text = this.lbSizeTableFont.Text;
                name = this.lbNameTableFont.Name.ToString();
                this.lbNameTableFont.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                this.lbNameTextFont.Text = this.lbNameTableFont.Text;

                // Traducimos los groupBox
                name = this.groupBoxSettingFontTable.Name.ToString();
                this.groupBoxSettingFontTable.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.groupBoxSettingFontComment.Name.ToString();
                this.groupBoxSettingFontComment.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Inicializamos el combox con las traducciones de los separdores
                string period = dic.labelTraslation("stringPeriod").GetTranslation(lang).ToString();
                string comma = dic.labelTraslation("stringComma").GetTranslation(lang).ToString();
                this.cBoxDecimalSeparator.Items.Add(period);
                this.cBoxDecimalSeparator.Items.Add(comma);
                /* Inicializamos el comboBox y la variable entera que usamos para saber si se ha modificado
                 * el separdor decimal empleado para representar los datos.
                 */
                if (cfg.GetDecimalSeparator().Equals(ConfigCFG.ConfigCFG.DECIMAL_SEPARATOR_PERIOD))
                {
                    // Entonces es un punto
                    this.cBoxDecimalSeparator.SelectedIndex = 0;
                    this.posIndexComboBox = 0;
                }
                else
                {
                    // Entonces es una coma
                    this.cBoxDecimalSeparator.SelectedIndex = 1;
                    this.posIndexComboBox = 1;
                }

                // Traducimos los textos de los checkBox
                name = this.checkBoxShadingRows.Name.ToString();
                this.checkBoxShadingRows.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                name = this.checkBoxNull_to_zero.Name.ToString();
                this.checkBoxNull_to_zero.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                name = this.checkBox_coefG_Abs.Name.ToString();
                this.checkBox_coefG_Abs.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                name = this.checkBox_coefG_Rel.Name.ToString();
                this.checkBox_coefG_Rel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                name = this.checkBoxErrorAbsStandDev.Name.ToString();
                this.checkBoxErrorAbsStandDev.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                name = this.checkBoxErrorRelStandDev.Name.ToString();
                this.checkBoxErrorRelStandDev.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                name = this.checkBoxTotalAbsErrorVar.Name.ToString();
                this.checkBoxTotalAbsErrorVar.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                name = this.checkBoxTotalRelErrorVar.Name.ToString();
                this.checkBoxTotalRelErrorVar.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // GroupBox Tipos de tabla de medias
                name = this.groupBoxTypeOfTableMeans.Name.ToString();
                this.groupBoxTypeOfTableMeans.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // RadioButton del GroupBox Tipos de tabla de medias
                name = this.radioBtDefault.Name.ToString();
                this.radioBtDefault.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.radioBtDifference.Name.ToString();
                this.radioBtDifference.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.radioBtTypePoint.Name.ToString();
                this.radioBtTypePoint.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos el texto del toolTip
                name = "toolTipText";
                toolTipText = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos los textos de groupBoxSettingChart
                name = this.groupBoxChartSetting.Name.ToString();
                groupBoxChartSetting.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbChartType.Name.ToString();
                lbChartType.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbLabelPoint.Name.ToString();
                lbLabelPoint.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbMarkerStyle.Name.ToString();
                lbMarkerStyle.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos los mensages
                name = "errorFileInUse";
                errorFileInUse = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "messageRemoveFile";
                messageRemoveFile = dic.labelTraslation(name).GetTranslation(lang).ToString();

            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " Se produjo un error al traducir " + name);
            }
        }
        #endregion Traducción de la ventana

    } // public partial class FormSettings : Form
} // namespace GUI_TG
