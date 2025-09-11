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
 * Fecha de revisión: 04/Jun/2012
 * 
 * Descripción:
 *      Muestra los datos resumenes en una gráfica creada con un chartControl
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MultiFacetData;
using ProjectSSQ;
using System.Windows.Forms.DataVisualization.Charting;
using ConfigCFG;

namespace GUI_GT
{
    public partial class FormShowCharts : Form
    {
        /*=================================================================================
         * Constantes
         *=================================================================================*/
        // nombre del archivo que contiene las traducciones
        public const string STRING_TEXT = "formShowCharts.txt"; // archivo con las traducciones de la ventana
        const string LANG_PATH = "\\lang\\";

        // Nombres de las series gráficas
        const string SERIE_COEFG_REL = "Series_coefG_Rel";
        const string SERIE_COEFG_ABS = "Series_coefG_Abs";
        const string SERIE_TOTAL_REL_ERROR_VAR = "SeriesTotalRelErrorVar";
        const string SERIE_TOTAL_ABS_ERROR_VAR = "SeriesTotalAbsErrorVar";
        const string SERIE_ERROR_REL_STAND = "SeriesErrorRelStandDev";
        const string SERIE_ERROR_ABS_STAND = "SeriesErrorAbsStandDev";

        /*=================================================================================
         * variables 
         *=================================================================================*/
        // Directorio de trabajo por defecto
        string sagt_initial_directory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\SAGT\\Workspace";

        private ConfigCFG.ConfigCFG cfg;
        private string resum = "Resumen";
        private FormPrincipal formP;


        /* Descripción:
         *  Inicializa las componentes.
         */
        public FormShowCharts()
        {
            InitializeComponent();
        }


        /* Descripción:
         *  Inicializa la ventana tomando todos los parámetros y creando una serie
         * Parámetros:
         *      TransLibrary.Language lang: idioma para la traducción de los etiquetas de la ventana.
         *      List<G_ParametersOptimization> lG_Parameters: Lista con los datos para dibujar la tabla.
         */
        public FormShowCharts(FormPrincipal formP, ConfigCFG.ConfigCFG cfg, List<G_ParametersOptimization> lG_Parameters)
        {
            InitializeComponent();
            this.cfg = cfg;
            this.formP = formP;
            traslationElements(this.cfg.GetConfigLanguage(), Application.StartupPath + LANG_PATH + STRING_TEXT);
            LoadPointsInSerie(lG_Parameters);

            this.chartG_Parameters.ChartAreas[0].AxisX.ScrollBar.Size = 10;
            this.chartG_Parameters.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = 10;

            this.chartG_Parameters.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
        }


        /*
         * Descripción:
         *  Introduce los G_Parameters en una serie
         */
        private void LoadPointsInSerie(List<G_ParametersOptimization> lG_Parameters)
        {
            foreach (G_ParametersOptimization g in lG_Parameters)
            {
                int pos = lG_Parameters.IndexOf(g);
                AddG_Parameter(pos, g);
            }
        }


        /* Descripción:
         *  Añade un elemento G_Parameters en la gráfica en la posición que se pasa como parámetro.
         */
        public void AddG_Parameter(int pos, G_ParametersOptimization g)
        {
            // Color de borde de las barras
            Color borderColor = Color.Gray;
            // grosor de la linea de borde
            int borderWith = 1;
            // Estilo de gradiente
            GradientStyle gStyle = GradientStyle.DiagonalRight;

            /*
             * Coeficiente G Relativo
             */
            if (this.cfg.GetCheck_coefG_Rel())
            {
                double v = (double)g.CoefG_Rel();
                introducirSerie(pos, v, SERIE_COEFG_REL);
                this.chartG_Parameters.Series[SERIE_COEFG_REL].Color = this.cfg.GetColor_coefG_Rel();
                this.chartG_Parameters.Series[SERIE_COEFG_REL].BackSecondaryColor = ColorMoreBrightness(this.cfg.GetColor_coefG_Rel());
                this.chartG_Parameters.Series[SERIE_COEFG_REL].BackGradientStyle = gStyle;
                this.chartG_Parameters.Series[SERIE_COEFG_REL].BorderColor = borderColor;
                this.chartG_Parameters.Series[SERIE_COEFG_REL].BorderWidth = borderWith;
            }
            else
            {
                this.chartG_Parameters.Series[SERIE_COEFG_REL].IsVisibleInLegend = false;
            }

            /*
             * Coeficiente G Absoluto
             */
            if (this.cfg.GetCheck_coefG_Abs())
            {
                double v = (double)g.CoefG_Abs();
                introducirSerie(pos, v, SERIE_COEFG_ABS);
                this.chartG_Parameters.Series[SERIE_COEFG_ABS].Color = this.cfg.GetColor_coefG_Abs();
                this.chartG_Parameters.Series[SERIE_COEFG_ABS].BackSecondaryColor = ColorMoreBrightness(this.cfg.GetColor_coefG_Abs());
                this.chartG_Parameters.Series[SERIE_COEFG_ABS].BackGradientStyle = gStyle;
                this.chartG_Parameters.Series[SERIE_COEFG_ABS].BorderColor = borderColor;
                this.chartG_Parameters.Series[SERIE_COEFG_ABS].BorderWidth = borderWith;
            }
            else
            {
                this.chartG_Parameters.Series[SERIE_COEFG_ABS].IsVisibleInLegend = false;
            }

            /*
             * Varianza del Error Relativo
             */
            if (cfg.GetCheckTotalRelErrorVar())
            {
                double v = (double)g.TotalRelErrorVar();
                introducirSerie(pos, v, SERIE_TOTAL_REL_ERROR_VAR);
                this.chartG_Parameters.Series[SERIE_TOTAL_REL_ERROR_VAR].Color = this.cfg.GetColorTotalRelErrorVar();
                this.chartG_Parameters.Series[SERIE_TOTAL_REL_ERROR_VAR].BackSecondaryColor = ColorMoreBrightness(this.cfg.GetColorTotalRelErrorVar());
                this.chartG_Parameters.Series[SERIE_TOTAL_REL_ERROR_VAR].BackGradientStyle = gStyle;
                this.chartG_Parameters.Series[SERIE_TOTAL_REL_ERROR_VAR].BorderColor = borderColor;
                this.chartG_Parameters.Series[SERIE_TOTAL_REL_ERROR_VAR].BorderWidth = borderWith;
            }
            else
            {
                this.chartG_Parameters.Series[SERIE_TOTAL_REL_ERROR_VAR].IsVisibleInLegend = false;
            }

            /*
             * Varianza del Error Absoluto
             */
            if (this.cfg.GetCheckTotalAbsErrorVar())
            {
                double v = (double)g.TotalAbsErrorVar();
                introducirSerie(pos, v, SERIE_TOTAL_ABS_ERROR_VAR);
                this.chartG_Parameters.Series[SERIE_TOTAL_ABS_ERROR_VAR].Color = this.cfg.GetColorTotalAbsErrorVar();
                this.chartG_Parameters.Series[SERIE_TOTAL_ABS_ERROR_VAR].BackSecondaryColor = ColorMoreBrightness(this.cfg.GetColorTotalAbsErrorVar());
                this.chartG_Parameters.Series[SERIE_TOTAL_ABS_ERROR_VAR].BackGradientStyle = gStyle;
                this.chartG_Parameters.Series[SERIE_TOTAL_ABS_ERROR_VAR].BorderColor = borderColor;
                this.chartG_Parameters.Series[SERIE_TOTAL_ABS_ERROR_VAR].BorderWidth = borderWith;
            }
            else
            {
                this.chartG_Parameters.Series[SERIE_TOTAL_ABS_ERROR_VAR].IsVisibleInLegend = false;
            }

            /*
             * Error Relativo Estandar 
             */
            if (this.cfg.GetCheckErrorRelStandDev())
            {
                double v = (double)g.ErrorRelStandDev();
                introducirSerie(pos, v, SERIE_ERROR_REL_STAND);
                this.chartG_Parameters.Series[SERIE_ERROR_REL_STAND].Color = this.cfg.GetColorErrorRelStandDev();
                this.chartG_Parameters.Series[SERIE_ERROR_REL_STAND].BackSecondaryColor = ColorMoreBrightness(this.cfg.GetColorErrorRelStandDev());
                this.chartG_Parameters.Series[SERIE_ERROR_REL_STAND].BackGradientStyle = gStyle;
                this.chartG_Parameters.Series[SERIE_ERROR_REL_STAND].BorderColor = borderColor;
                this.chartG_Parameters.Series[SERIE_ERROR_REL_STAND].BorderWidth = borderWith;
            }
            else
            {
                this.chartG_Parameters.Series[SERIE_ERROR_REL_STAND].IsVisibleInLegend = false;
            }

            /*
             * Error Absoluto Estandar 
             */
            if (this.cfg.GetCheckErrorAbsStandDev())
            {
                double v = (double)g.ErrorAbsStandDev();
                introducirSerie(pos, v, SERIE_ERROR_ABS_STAND);
                this.chartG_Parameters.Series[SERIE_ERROR_ABS_STAND].Color = this.cfg.GetColorErrorAbsStandDev();
                this.chartG_Parameters.Series[SERIE_ERROR_ABS_STAND].BackSecondaryColor = ColorMoreBrightness(this.cfg.GetColorErrorAbsStandDev());
                this.chartG_Parameters.Series[SERIE_ERROR_ABS_STAND].BackGradientStyle = gStyle;
                this.chartG_Parameters.Series[SERIE_ERROR_ABS_STAND].BorderColor = borderColor; // color de borde de la barra
                this.chartG_Parameters.Series[SERIE_ERROR_ABS_STAND].BorderWidth = borderWith; // grosor de la linea de borde

            }
            else
            {
                this.chartG_Parameters.Series[SERIE_ERROR_ABS_STAND].IsVisibleInLegend = false;
            }

            
        }// end AddG_Parameter(int pos,G_Parameters g)



        /* Descripción:
         *  Operación auxiliar que introduce el valor en la serie. Y pone como etiqueta dicho valor.
         * Parámetros:
         *      int pos: posición de la serie.
         *      double valor valor de la serie y etiqueta.
         *      string serie: nombre de la serie.
         */
        private void introducirSerie(int pos, double valor,string serie)
        {
            double v = valor;
            DataPoint dataP = new DataPoint(pos, v);
            this.chartG_Parameters.Series[serie].Points.Add(dataP);
            string labelResum = resum;
            if (pos != 0)
            {
                labelResum = resum + " " + pos;
            }
            this.chartG_Parameters.Series[serie].Points[pos].AxisLabel = labelResum;
            // Muestra el resuldo encima de la barra
            //this.chartG_Parameters.Series[serie].Label = Math.Round(v,3).ToString();
        }



        /* Descripción:
         *  Devuelve un color un poco más claro que el que se pasa como parámetro
         */
        private static Color ColorMoreBrightness(Color colorF)
        {
            RGBHSL.HSL colorHSL = RGBHSL.RGB_to_HSL(colorF);

            double b = colorHSL.L; // brillo

            double newB = b - (b / 8);
            if  (newB>=0)// (newB < 360d)
            {
                b = newB;
            }


            // Color colorHSB = ColorFromAhsb(a, h, s, b);

            return RGBHSL.ModifyBrightness(colorF, b);
        }

        /* Descripción:
         *  Salva la gráfica como una imagen em el archivo especificado en el cuadro de diálogo
         */
        private void btSaveChartImage_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            
            if (Directory.Exists(sagt_initial_directory))
            {
                saveDialog.InitialDirectory = sagt_initial_directory;
            }

            saveDialog.DefaultExt = "jpg";
            saveDialog.Filter = "JPEG (*.jpg)| *.jpg | PNG (*.png)| *.png | GIF (*.gif) | *.gif";
            saveDialog.OverwritePrompt = true; // muestra advertencia si el fichero ya existe
            saveDialog.AddExtension = true;
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                switch (saveDialog.FilterIndex)
                {
                    case(0):
                        this.chartG_Parameters.SaveImage(saveDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case(1):
                        this.chartG_Parameters.SaveImage(saveDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case(2):
                        this.chartG_Parameters.SaveImage(saveDialog.FileName, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    default:
                        this.chartG_Parameters.SaveImage(saveDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                }
            }
        }

        

        #region Traducción de la ventana
        /*======================================================================================
         * Traducción de la ventana
         *======================================================================================*/

        /*
         * Descripción:
         *  Traduce todos los textos de la ventana al idioma que se encuentre activo
         */
        public void traslationElements(TransLibrary.Language lang, string pathFileTrans)
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

                // Traducimos el boton de guardar
                name = this.btSaveChartImage.Name.ToString();
                this.btSaveChartImage.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos las etiquestas de la leyenda
                int n = this.chartG_Parameters.Series.Count;
                for (int i = 0; i < n; i++)
                {
                    name = this.chartG_Parameters.Series[i].Name.ToString();
                    this.chartG_Parameters.Series[i].LegendText = dic.labelTraslation(name).GetTranslation(lang).ToString();
                }

                name = "resum";
                resum = dic.labelTraslation(name).GetTranslation(lang).ToString();
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " Se produjo un error al traducir " + name);
            }
        }
        #endregion Traducción de la ventana


        /* Descripción:
         *  Evento lanzado cuando se cierra la ventana. Llama al metod de SSQOption.cs FormShowChartsClosed.
         */
        private void FormShowCharstClosed(object sender, FormClosedEventArgs e)
        {
            this.formP.FormShowChartsClosed();
        }



    } // end public partial class FormShowCharts : Form
}// end namespace GUI_TG