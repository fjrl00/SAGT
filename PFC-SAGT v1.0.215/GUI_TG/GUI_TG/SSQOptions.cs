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
 * Fecha de revisión: 10/Jul/2012                           
 * 
 * Descripción:
 *      Clase parcial ("partial") del FormPrincipal. Contiene los métodos referentes a la parte de
 *      Suma de cuadrados: Analisis de varianza del plan y estimacion de los componentes de varianza.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MultiFacetData;
using ProjectSSQ;
using AuxMathCalcGT;
using SsqPY;
using System.Drawing; // se usa para las propiedades de la cabecera de columna (color,fuente,etc)
using System.Threading; // permite usar hilos
using Sagt;
using ImportEduGSsq;


namespace GUI_GT
{
    public partial class FormPrincipal : Form
    {
        #region Variables relaccionadas con la opción SSQ

        // Cabeceras de las columnas pertenecientes al tabPage de SSQ
        //===========================================================
        private string sourceOfVarString = "Fuentes de variación";
        private string ssqString = "Suma de cuadrados";
        private string degreeOfFreedomString = "Grado de libertad";
        private string msqString = "Cuadrado medio";
        private string randomCompString = "Aleatorios";
        private string mixCompString = "Mixtos";
        private string correctedComp = "Corregidos";
        private string porcentageString = "%";
        private string standardErrorString = "Error estándar";

        // CONSTANTES: indices de la tabla Analisis de varianza
        // ====================================================
        const int IND_SOURCE_OF_VAR = 0;     // indice de la columna 'Fuente de variación' de la tabla 'Suma de cuadrados'
        const int IND_SSQ = 1;  // indice de la columna 'suma de cuadrados'.
        const int IND_DEGREE_OF_FREEDOM = 2;    // indice de la columna 'grado de libertad'
        const int IND_MSQ = 3; // indice de la columna 'cuadrado medio'.
        const int IND_RANDOM_COMP = 4; // indice de la columna de Componente de varianza aleatorio
        const int IND_MIX_COMP = 5; // indice de la columna de componente de varianza mixto
        const int IND_CORRECTED_COMP = 6; // indice de la componente de varianza corregida
        const int IND_PORCENTAGE = 7; // indice de la columna del porcentaje
        const int IND_STANDARD_ERROR = 8; // indice de la columna del error standar.

        // Cabeceras de las columnas pertenecientes al tabPage de G_Parameters
        // ===================================================================
        private string source = "Fuente";
        private string diff_var = "Varianza de diferenciación";
        private string rel_err_var = "Varianza de error relativa";
        private string percent_rel_err = "% relativo";
        private string abs_err_var = "Varianza de error absoluta";
        private string percent_abs_err = "% absoluto";

        // Cabeceras de las columnas pertenecientes al tabPage de Optimization
        // ===================================================================
        // private string sizeOfUniverse = "Tamaño del universo";
        private string levelsProcess = "Niveles procesados";
        // indice de la posición en la tabla de optimización de la columna Niveles de procesamiento
        private int IND_LEVELS_PROCESS = 2;
        // indice de la posición en la tabla de optimización de la columna Tamaño del universo
        private int IND_SIZE_OF_UNIVERSE = 3;

        // String para la columna de resumen de datos
        //=============================================
        private string total_nb_obs = "Total de observaciones";
        private string relat_measmt = "Coeficiente G relativo";
        private string absol_measmt = "Coeficiente G absoluto";
        private string relat_err_var = "Error relativo";
        private string absol_err_var = "Error absoluto";
        private string stand_relat_err = "Desv. típica del error relativo";
        private string stand_absol_err = "Desv. típica del error absoluto";

        // Cabecera de las columnas resumen de datos
        //==========================================
        private string name_resum = "Nombre de los valores";
        private string resum = "Resumen";

        private string noData = "N/A";

        // Variables de tipo ProjectSSQ
        // Analysis_and_G_Study tAnalysis_tG_Study_Opt;

        // Ventana de gráficos
        private FormShowCharts formShowCharts;

        #endregion Variables relaccionadas con la opción SSQ


        /* Descripción:
         *  Abre un archivo de suma de cuadrados de SAGT.
         */
        private void tsmiActionOpenSSQ_Click(object sender, EventArgs e)
        {
            DialogResult res = DialogResult.OK;
            Analysis_and_G_Study tAnalysis_tG_Study_Opt = this.sagtElements.GetAnalysis_and_G_Study();
            if (tAnalysis_tG_Study_Opt != null)
            {
                res = ShowMessageDialog(titleConfirm, txtConfirmClose);
            }
            if (res == DialogResult.OK)
            {
                OpenFileDialog openDialog = new OpenFileDialog();

                if (Directory.Exists(this.cfgApli.Get_Path_Workspace()))
                {
                    openDialog.InitialDirectory = this.cfgApli.Get_Path_Workspace();
                }

                string fileFilter = (FILTER_SAGT_FILE + this.allFiles + FILTER_ALL_FILE);
                openDialog.Filter = fileFilter;

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Analysis_and_G_Study tb_aux = ProjectSSQ.Analysis_and_G_Study.ReadingFileAnalysisSSQ(openDialog.FileName);
                        this.sagtElements = SagtFile.ReadingSagtFile(openDialog.FileName);
                        //if (tAnalysis_tG_Study_Opt != null)
                        //{
                        //    // Limpiamos todas las tablas
                        //    ClearTabPageSSQ();
                        //}
                        //// tAnalysis_tG_Study_Opt = tb_aux;
                        //LoadAllDataInDataGridViewEx_SSQOptions();
                        loadSagtElements(openDialog.FileName, sagtElements);
                    }
                    catch (Analysis_and_G_Study_Exception)
                    {
                        // Mostramos un mensaje de error
                        MessageBox.Show(errorReadingFile, titleMessageError1);
                    }
                }
            }
        }// end private void tsmiActionOpenSSQ_Click


        /* Descripción:
         *  Salva los datos los datos de las tablas de sumas de cuadrados en un archivo de SAGT.
         */
        private void tsmiActionSSQSave_Click(object sender, EventArgs e)
        {
            // DialogResult res = DialogResult.OK;
            // TableAnalysisOfVariance tb = this.tAnalysis_tG_Study_Opt.TableAnalysisVariance();
            Analysis_and_G_Study tAnalysis_tG_Study_Opt = this.sagtElements.GetAnalysis_and_G_Study();
            if (tAnalysis_tG_Study_Opt == null)
            {
                // si no tenemos lista de medias lanzamos un mensaje de error
                ShowMessageErrorOK(errorNoSSQ);
            }
            else
            {
                //// Preguntamos al usuario por el archivo
                //SaveFileDialog saveDialog = new SaveFileDialog();
                //saveDialog.DefaultExt = "tssq";
                //saveDialog.Filter = "SSQ SAGT file(*.tssq)|*.tssq";
                //saveDialog.OverwritePrompt = true; // muestra advertencia si el fichero ya existe
                //saveDialog.AddExtension = true;
                //if (saveDialog.ShowDialog() == DialogResult.OK)
                //{
                //    // this.tAnalysis_tG_Study_Opt.WritingFileAnalysisSSQ(saveDialog.FileName);
                //    this.sagtElements.WritingSagtFile(saveDialog.FileName);
                //    MessageBox.Show("Las datos se han guardado", "Guardado");
                //}
                SaveFileSagt(this.sagtElements);
            }
        }// end private void tsmiActionSSQSave_Click


        /*
         * Descripción:
         *  Calcula los datos de la estimación de la suma de cuadrados del elemento mutifaceta 
         *  actual y los muestra en el dataGridView.
         */
        private void EstimationPlan()
        {
            MultiFacetsObs multiFacets = this.sagtElements.GetMultiFacetsObs();
            if (multiFacets == null)
            {
                ShowMessageErrorOK(errorNoTableObs);
                //MessageBox.Show(errorNoTableObs);
            }
            else
            {// (* 1 *)
                // Recuperamos las casillas de omision de facetas
                ReadColumnOmit(this.sagtElements, this.dataGridViewExFacets);
                ListFacets actual_lf = this.sagtElements.GetMultiFacetsObs().ListFacets();
                ListFacets withoutOmit_lf = actual_lf.ListFacetWithoutOmit();


                // si withoutOmit_lf tiene menos de 2 elementos lanzamos un mensage de error
                if (withoutOmit_lf.Count() < 2)
                {// (* 2 *)
                    ShowMessageErrorOK(errorNoOmitTwoFacet);
                }
                else
                {
                    TransLibrary.Language lang = this.LanguageActually();
                    ListFacets lf = actual_lf;

                    // crearemos la ventana y le pasamos como argumento el objeto multifaceta
                    // ListFacets lf = multiFacets.ListFacets();
                    if (actual_lf.Count() != withoutOmit_lf.Count())
                    {
                        lf = withoutOmit_lf; // asignación de las facetas no omitidas
                        // multiFacets = this.sagtElements.GetMultiFacetsObs();
                        multiFacets = multiFacets.OmitFacetInDataTable();
                    }

                    // aplicamos la eliminación de niveles
                    multiFacets = multiFacets.SkipAndRestoreIndexLevelFacetInDataTable();

                    // Primero debemos preguntarle al usuario por el diseño de medida.
                    ListFacets sourceOfDifferentiation = new ListFacets();
                    ListFacets sourceOfInstrumentation = new ListFacets();
                    foreach (Facet f in multiFacets.ListFacets())
                    {
                        sourceOfInstrumentation.Add(f);
                    }

                    FormMeasurDesign formMeasurDesign = new FormMeasurDesign(sourceOfDifferentiation, sourceOfInstrumentation, cfgApli.GetConfigLanguage());

                    bool salir = false;
                    do
                    {
                        DialogResult res = formMeasurDesign.ShowDialog();
                        switch (res)
                        {
                            case (DialogResult.Cancel): salir = true; break;
                            case (DialogResult.OK):
                                if (formMeasurDesign.ListFacetDiff() == 0 || formMeasurDesign.ListFacetInst() == 0)
                                {
                                    MessageBox.Show(errorM_DesignNoValidate, "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                }
                                else
                                {
                                    salir = true;
                                    // Mostramos el diseño de medida en el textBox de los tabPage de suma de cuadrados
                                    // ShowMeDessingInTextBoxs(sourceOfDifferentiation, sourceOfInstrumentation);

                                    bool zero = this.cfgApli.GetNull_to_Zero();

                                    TableAnalysisOfVariance tableAnalysis = new TableAnalysisOfVariance(multiFacets, zero);
                                    TableG_Study_Percent tableG_Study = new TableG_Study_Percent(sourceOfDifferentiation, sourceOfInstrumentation, tableAnalysis);

                                    // Inicializamos la lista de G_Parámetros de Optimización
                                    List<G_ParametersOptimization> listG_ParametersOpt = new List<G_ParametersOptimization>();

                                    Analysis_and_G_Study tAnalysis_tG_Study_Opt = new Analysis_and_G_Study(tableAnalysis, tableG_Study, listG_ParametersOpt);

                                    // Guardamos los datos referentes a la creación de la suma de cuadrados
                                    string nameFile = multiFacets.NameFileObs();
                                    tAnalysis_tG_Study_Opt.SetNameFileDataCreation(nameFile);

                                    DateTime date = DateTime.Now;
                                    tAnalysis_tG_Study_Opt.SetDateTime(date);
                                    // Actualizamos los datos
                                    this.sagtElements.SetAnalysis_and_G_Study(tAnalysis_tG_Study_Opt);

                                    // Mostramos todos los datos en los dataGridView
                                    LoadAllDataInDataGridViewEx_SSQOptions(tAnalysis_tG_Study_Opt);

                                    // mostramos el tabPage de suma de cuadrados
                                    ExcludeTabPages();
                                    this.tabPageSSQ.Parent = this.tabControlOptions;
                                    // Restauramos los colores
                                    this.RestoreColorMenu(this.mStripMain);
                                    // Asignamos el nuevo color
                                    this.tsmiSSQ.BackColor = System.Drawing.SystemColors.Highlight;
                                }
                                break;
                        }// end switch
                    } while (!salir);
                }
            }// end if  (* 1 *)
        }//private void EstimationPlan()


        /*
         * Descripción:
         *  Muestra los datos en todos los dataGridView de la opcionSSQ
         */
        public void LoadAllDataInDataGridViewEx_SSQOptions(Analysis_and_G_Study tAnalysis_tG_Study_Opt)
        {
            // Analysis_and_G_Study tAnalysis_tG_Study_Opt = this.sagtElements.GetAnalysis_and_G_Study();

            TableAnalysisOfVariance tableAnalysis = tAnalysis_tG_Study_Opt.TableAnalysisVariance();
            TableG_Study_Percent tableG_Study = tAnalysis_tG_Study_Opt.TableG_Study_Percent();
            // Mostramos el diseño en el taexBox
            ListFacets sourceOfVarInstrumentation = tableG_Study.LfInstrumentation();
            ListFacets sourceOfDifferentiation = tableG_Study.LfDifferentiation();
            ShowMeDessingInTextBoxs(sourceOfDifferentiation, sourceOfVarInstrumentation);
            //mostramos los datos de las facetas
            this.LoadListFacetInDataGridView(tableAnalysis.ListFacets(), this.dGridViewExSourceOfVar);
            // ahora debemos rellenar la tabla.
            LoadSSQ_InDataGridView(tableAnalysis, this.dataGridViewExSSQ);
            // los valores totales de la tabla
            LoadTotalSSQ_TableComp(tableAnalysis);
            // ahora los GParameters
            LoadG_ParametersInDataGridView(tAnalysis_tG_Study_Opt, this.dGridViewExG_Parameters);
            LoadTotalG_Parameters(tableG_Study);
            LoadDGridViewExFacetsOptimization(tAnalysis_tG_Study_Opt);
            LoadDataGridViewExOptimizationResum(tAnalysis_tG_Study_Opt, 
                this.dGridViewExOptimizationResum);
            // Los datos de la pestaña de información
            this.tbNameFileSsqInfo.Text = tAnalysis_tG_Study_Opt.GetNameFileDataCreation();
            this.tbDateFileSsqInfo.Text = tAnalysis_tG_Study_Opt.GetDateTime().ToString();
            this.richTextBoxSsqComment.Text = tAnalysis_tG_Study_Opt.GetTextComment();

        }// end LoadAllDataInDataGridViewEx_SSQOptions


        /*
         * Descripción:
         *  Limpia los campos de texto de los label que deben mostrarse inicialmente vacios.
         */
        private void ClearListBoxSSQ()
        {
            lbToltalSSQ.Text = "";
            lbTotalDF.Text = "";
            lbTotal_Target.Text = "";
            lbTotal_Error_Rel.Text = "";
            lbTotal_Error_Abs.Text = "";
            lbStandDev.Text = "";
            lbRelativeSE.Text = "";
            lbAbsoluteSE.Text = "";
            lbCoef_G_Rel.Text = "";
            lbCoef_G_Abs.Text = "";
        }


        /*
         * Descripción:
         *  Carga los valores de la suma total de la suma de cuadrados y los grados de libertad en
         *  sus respetivas etiquetas en el tabPageSSQ_TableComp.
         * Parámetros:
         *      TableAnalysisOfVariance tableAnalysis: Tabla de Análisis de varianza.
         */
        private void LoadTotalSSQ_TableComp(TableAnalysisOfVariance tableAnalysis)
        {
            int numOfDecimal = cfgApli.GetNumberOfDecimals();
            string puntoDecimal = this.cfgApli.GetDecimalSeparator();
            this.lbToltalSSQ.Text = ConvertNum.DecimalToString(tableAnalysis.CalcTotalSSQ(), numOfDecimal, puntoDecimal);
            this.lbTotalDF.Text = tableAnalysis.CalcTotalDF().ToString();
        }


        /*
         * Descripción:
         *  Carga los valores totales de los G-Parmeters en las etiquetas de total suma de fuentes 
         *  objetivo, total varianza de error relativo y total varianza de error absoluto.
         * Parámetros:
         *  TableG_Study_Percent tableG_Study: Tabla de G parámetros.
         */
        private void LoadTotalG_Parameters(TableG_Study_Percent tableG_Study)
        {
            int numOfDecimal = cfgApli.GetNumberOfDecimals();
            string puntoDecimal = this.cfgApli.GetDecimalSeparator();
            this.lbTotal_Target.Text = ConvertNum.DecimalToString(tableG_Study.TotalDifferentiationVariance(), numOfDecimal, puntoDecimal);
            this.lbTotal_Error_Rel.Text = ConvertNum.DecimalToString(tableG_Study.TotalRelErrorVar(), numOfDecimal, puntoDecimal);
            this.lbTotal_Error_Abs.Text = ConvertNum.DecimalToString(tableG_Study.TotalAbsErrorVar(), numOfDecimal, puntoDecimal);

            // Calculamos las desviaciones tipicas
            this.lbStandDev.Text = ConvertNum.DecimalToString(tableG_Study.TargetStandDev(), numOfDecimal, puntoDecimal);
            this.lbRelativeSE.Text = ConvertNum.DecimalToString(tableG_Study.ErrorRelStandDev(), numOfDecimal, puntoDecimal);
            this.lbAbsoluteSE.Text = ConvertNum.DecimalToString(tableG_Study.ErrorAbsStandDev(), numOfDecimal, puntoDecimal);

            // calculamos los coeficientes de generalizabilidad
            this.lbCoef_G_Rel.Text = ConvertNum.DecimalToString(tableG_Study.CoefG_Rel(), numOfDecimal, puntoDecimal);
            this.lbCoef_G_Abs.Text = ConvertNum.DecimalToString(tableG_Study.CoefG_Abs(), numOfDecimal, puntoDecimal);
        }


        /*
         * Descripción:
         *  Muestra los datos de un ListTableSSQ en el dataGridView del tabPageSSQ
         * Parámetros:
         *      TableAnalysisOfVariance tbAnalysisVar: Es el objeto tabla de análisis de varianza de donse se
         *              extraen los datos.
         *      DataGridViewEx.DataGridViewEx dgvExSSq: Es el dataGridViewEx donde se van a mostrar los datos 
         *              de la suma de cuadrados.
         */
        private void LoadSSQ_InDataGridView(TableAnalysisOfVariance tbAnalysisVar, DataGridViewEx.DataGridViewEx dgvExSSq)
        {
            // primero limpiamos el dataGridView por si hubiera algún dato anterior.
            dgvExSSq.Rows.Clear();
            dgvExSSq.ColumnHeadersVisible = true;
            /* Asignamos el número de columnas. 
             */
            int num_col = 9;
            // dgvExSSq.ColumnCount = num_col;
            dgvExSSq.NumeroColumnas = num_col;

            dgvExSSq.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Set the column header style.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Aqua;
            columnHeaderStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
            dgvExSSq.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            dgvExSSq.DefaultCellStyle.Font = this.fontCellTable;

            // Primera columna [0] (Fuentes de variación)
            this.PropertyColumnDGV(dgvExSSq, IND_SOURCE_OF_VAR, this.sourceOfVarString);
            // tercera columna [1] (suma de cuadrados)
            this.PropertyColumnDGV(dgvExSSq, IND_SSQ, this.ssqString);
            // segunda columna [2] (grado de libertad)
            this.PropertyColumnDGV(dgvExSSq, IND_DEGREE_OF_FREEDOM, this.degreeOfFreedomString);
            // cuarta columna [3] (cuadrados medios)
            this.PropertyColumnDGV(dgvExSSq, IND_MSQ, this.msqString);
            // quinta columna [4] (componentes de varianza aleatorios)
            this.PropertyColumnDGV(dgvExSSq, IND_RANDOM_COMP, this.randomCompString);
            // sexta columna [5] (componentes de varianza mixtos)
            this.PropertyColumnDGV(dgvExSSq, IND_MIX_COMP, mixCompString);
            // septima columna [6] (componentes de varianza corregidos)
            this.PropertyColumnDGV(dgvExSSq, IND_CORRECTED_COMP, correctedComp);
            // octova columna [7] (Porcentaje)
            this.PropertyColumnDGV(dgvExSSq, IND_PORCENTAGE, 100, this.porcentageString);
            // novena columna [8] (Porcentaje)
            this.PropertyColumnDGV(dgvExSSq, IND_STANDARD_ERROR, this.standardErrorString);


            dgvExSSq.Columns[IND_SOURCE_OF_VAR].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;

            List<string> ldesign = tbAnalysisVar.SourcesOfVar();

            int numOfDecimal = cfgApli.GetNumberOfDecimals();
            string puntoDecimal = this.cfgApli.GetDecimalSeparator();

            ListFacets lf = tbAnalysisVar.ListFacets();

            foreach (string key in ldesign)
            {
                object[] my_Row = new object[num_col];

                ListFacets lf_key = lf.ListDesignFacets(key);
                my_Row[IND_SOURCE_OF_VAR] = key;
                my_Row[IND_DEGREE_OF_FREEDOM] = tbAnalysisVar.DegreesOfFreedom(key);
                my_Row[IND_SSQ] = ConvertNum.DecimalToString(tbAnalysisVar.SSQ(key), numOfDecimal, puntoDecimal);
                my_Row[IND_MSQ] = ConvertNum.DecimalToString(tbAnalysisVar.MSQ(key), numOfDecimal, puntoDecimal);
                my_Row[IND_RANDOM_COMP] = ConvertNum.DecimalToString(tbAnalysisVar.RandomComp(key), numOfDecimal, puntoDecimal);
                my_Row[IND_MIX_COMP] = ConvertNum.DecimalToString(tbAnalysisVar.MixModComp(key), numOfDecimal, puntoDecimal);
                my_Row[IND_CORRECTED_COMP] = ConvertNum.DecimalToString(tbAnalysisVar.CorretedComp(key), numOfDecimal, puntoDecimal);
                my_Row[IND_PORCENTAGE] = ConvertNum.DecimalToString(tbAnalysisVar.Porcentage(key), numOfDecimal, puntoDecimal);
                my_Row[IND_STANDARD_ERROR] = ConvertNum.DecimalToString(tbAnalysisVar.StandardError(key), numOfDecimal, puntoDecimal);

                dgvExSSq.Rows.Add(my_Row);
            }
            
        }// end LoadSSQ_InDataGridView


        /*
         * Descripción:
         *  Muestra los datos en un dataGridView G_parameters
         * Parámetros:
         *  Analysis_and_G_Study analysis_G_Study_opt: Objeto que contiene los datos que se van a mostrar.
         *          En concreto se mostrará la tabla de G-Parametros.
         *  DataGridViewEx.DataGridViewEx dgvExG_P: El dataGrid donde se mostrarán los datos.
         */
        private void LoadG_ParametersInDataGridView(Analysis_and_G_Study analysis_G_Study_opt, DataGridViewEx.DataGridViewEx dgvExG_P)
        {
            // primero limpiamos el dataGridView por si hubiera algún dato anterior.
            dgvExG_P.Rows.Clear();
            dgvExG_P.ColumnHeadersVisible = true;
            /* Asignamos el número de columnas. 
             */
            int num_col = 7;
            // dgvExG_P.ColumnCount = num_col;
            dgvExG_P.NumeroColumnas = num_col;

            dgvExG_P.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Set the column header style.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Aqua;
            columnHeaderStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
            dgvExG_P.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            dgvExG_P.DefaultCellStyle.Font = this.fontCellTable;

            this.PropertyColumnDGV(dgvExG_P, 0, this.source);
            this.PropertyColumnDGV(dgvExG_P, 1, this.diff_var);
            this.PropertyColumnDGV(dgvExG_P, 2, this.source);
            this.PropertyColumnDGV(dgvExG_P, 3, this.rel_err_var);
            this.PropertyColumnDGV(dgvExG_P, 4, this.percent_rel_err);
            this.PropertyColumnDGV(dgvExG_P, 5, this.abs_err_var);
            this.PropertyColumnDGV(dgvExG_P, 6, this.percent_abs_err);

            // dgvExG_P.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            // dgvExG_P.AutoResizeColumnHeadersHeight();
            // dgvExG_P.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            // dgvExG_P.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);

            // Alineamos a la izquierda las dos columnas que contiene texto (Fuentes de variación)
            dgvExG_P.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;
            dgvExG_P.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;

            for (int i = 0; i < num_col; i++)
            {
                dgvExG_P.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            int numOfDecimal = cfgApli.GetNumberOfDecimals();
            string puntoDecimal = this.cfgApli.GetDecimalSeparator();

            TableAnalysisOfVariance tbAnalysisVar = analysis_G_Study_opt.TableAnalysisVariance();

            TableG_Study_Percent gp = analysis_G_Study_opt.TableG_Study_Percent();

            List<string> ldesign = tbAnalysisVar.SourcesOfVar();
            foreach (string key in ldesign)
            {
                object[] my_Row = new object[num_col];
                if (gp.TargetContainsKey(key))
                {
                    // es un objetivo
                    my_Row[0] = key;
                    my_Row[1] = ConvertNum.DecimalToString(gp.Target(key), numOfDecimal, puntoDecimal);
                    my_Row[2] = null;
                    my_Row[3] = null;
                    my_Row[4] = null;
                    my_Row[5] = null;
                    my_Row[6] = null;
                }
                else if (gp.ErrorContainsKey(key))
                {
                    // es un fuente de variación
                    my_Row[0] = null;
                    my_Row[1] = null;
                    my_Row[2] = key;
                    /*
                     * Las siguientes lineas de codigo son provisionales sirben para que no
                     * se imprima en la celda el mensage N/A cuando se trata de un valor null
                     */
                    string st = ConvertNum.DecimalToString(gp.Error(key).RelErrorVar(), numOfDecimal, puntoDecimal);
                    string st_perc_err_rel = ConvertNum.DecimalToString(gp.Percent(key).RelErrorVar(), numOfDecimal, puntoDecimal);
                    if (st.Equals(noData))
                    {
                        st = "";
                        st_perc_err_rel = "";
                    }
                    my_Row[3] = st;
                    my_Row[4] = st_perc_err_rel;
                    string st2 = ConvertNum.DecimalToString(gp.Error(key).AbsErrorVar(), numOfDecimal, puntoDecimal);
                    string st_perc_err_abs = ConvertNum.DecimalToString(gp.Percent(key).AbsErrorVar(), numOfDecimal, puntoDecimal);
                    if (st2.Equals(noData))
                    {
                        st2 = "";
                        st_perc_err_abs = "";
                    }
                    my_Row[5] = st2;
                    my_Row[6] = st_perc_err_abs;
                }

                dgvExG_P.Rows.Add(my_Row);
            }

            dgvExG_P.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        }// end LoadG_ParametersInDataGridView


        /* Descripción:
         *  Carga las facetas en el dGridViewExFacetsOptimization para que esten presentes durante la optimización
         */
        private void LoadDGridViewExFacetsOptimization(Analysis_and_G_Study analysis_GStudy_Opt)
        {
            this.dGridViewExFacetsOptimization.Rows.Clear();
            this.dGridViewExFacetsOptimization.ColumnHeadersVisible = true;

            int num_col = 4;
            // this.dGridViewExFacetsOptimization.ColumnCount = num_col;
            this.dGridViewExFacetsOptimization.NumeroColumnas = num_col;

            this.dGridViewExFacetsOptimization.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Set the column header style.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Aqua;
            columnHeaderStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
            this.dGridViewExFacetsOptimization.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            this.dGridViewExFacetsOptimization.DefaultCellStyle.Font = this.fontCellTable;

            DataGridViewEx.DataGridViewEx dgv = this.dGridViewExFacetsOptimization;

            // Primera columna faceta
            //dgv.Columns[IND_NAME].Name = nameColFacet; // Nombre de la columna Etiquetas (dependerá del idioma).
            dgv.Columns[IND_NAME].HeaderText = nameColFacet; 
            dgv.Columns[IND_NAME].Width = 100;
            dgv.Columns[IND_NAME].SortMode = DataGridViewColumnSortMode.NotSortable;

            // segunda columna nivel
            // dgv.Columns[IND_LEVEL].Name = nameColLevel; // Nombre de la columna Niveles (dependerá del idioma).
            dgv.Columns[IND_LEVEL].HeaderText = nameColLevel;
            dgv.Columns[IND_LEVEL].Width = 100;
            dgv.Columns[IND_LEVEL].SortMode = DataGridViewColumnSortMode.NotSortable;

            // tercera columna descripción/ comentario
            // dgv.Columns[IND_LEVELS_PROCESS].Name = this.levelsProcess; // Nombre de la columna Descripción (dependerá del idioma).
            dgv.Columns[IND_LEVELS_PROCESS].HeaderText = this.levelsProcess; 
            dgv.Columns[IND_LEVELS_PROCESS].Width = 100;
            dgv.Columns[IND_LEVELS_PROCESS].SortMode = DataGridViewColumnSortMode.NotSortable;

            // tercera columna descripción/ comentario
            // dgv.Columns[IND_SIZE_OF_UNIVERSE].Name = this.sizeOfUniverse; // Nombre de la columna Descripción (dependerá del idioma).
            dgv.Columns[IND_SIZE_OF_UNIVERSE].HeaderText = this.nameColSizeOfUniverse;
            dgv.Columns[IND_SIZE_OF_UNIVERSE].Width = 100;
            dgv.Columns[IND_SIZE_OF_UNIVERSE].SortMode = DataGridViewColumnSortMode.NotSortable;

            for (int i = 0; i < 4; i++)
            {
                dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            TableAnalysisOfVariance tableAnalysis = analysis_GStudy_Opt.TableAnalysisVariance();

            foreach (Facet f in tableAnalysis.ListFacets())
            {
                object[] my_Row = new object[num_col];

                my_Row[IND_NAME] = f.Name();
                my_Row[IND_LEVEL] = f.Level();
                my_Row[IND_LEVELS_PROCESS] = f.Level();

                if (f.SizeOfUniverse().Equals(int.MaxValue))
                {
                    my_Row[IND_SIZE_OF_UNIVERSE] = Facet.INFINITE;
                }
                else
                {
                    my_Row[IND_SIZE_OF_UNIVERSE] = f.SizeOfUniverse();
                }

                dgv.Rows.Add(my_Row);
            }
        }// end LoadDGridViewExFacetsOptimization


        /*
         * Descripción:
         *  Carga los datos en el dGridViewOptimiztionResumen.
         * Parámetros: 
         *      TableAnalysisOfVariance tbAnalisisVar: Tabla de análisis de varianza
         *      List<TableAnalysisOfVariance> listTableOfVar, Lista de objetos tabla de analisis de varianza
         *      List<G_Parameters> listG_Parameters: Lista de objetos de G-Parámetros
         *      DataGridViewEx.DataGridViewEx dgvEx: El dataGridViewEx donde se mostrarán los datos.
         */
        private void LoadDataGridViewExOptimizationResum(Analysis_and_G_Study analysis_G_Study_Opt,
            DataGridViewEx.DataGridViewEx dgvEx)
        {
            dgvEx.Rows.Clear();
            dgvEx.ColumnHeadersVisible = true;
            int numCol = 2;

            dgvEx.NumeroColumnas = numCol;

            dgvEx.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Set the column header style.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Aqua;
            columnHeaderStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
            dgvEx.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            dgvEx.DefaultCellStyle.Font = this.fontCellTable;

            this.PropertyColumnDGV(dgvEx, 0, 170, this.name_resum);
            this.PropertyColumnDGV(dgvEx, 1, 150, this.resum);

            TableAnalysisOfVariance tbAnalisisVar = analysis_G_Study_Opt.TableAnalysisVariance();
            ListFacets lf = tbAnalisisVar.ListFacets();

            foreach (Facet f in lf)
            {
                string name = f.Name();
                // Incluimos además de nivel el tamaño del universo
                string sizeOfUnv = Facet.INFINITE;
                int s = f.SizeOfUniverse();
                if (!int.MaxValue.Equals(s))
                {
                    sizeOfUnv = s.ToString();
                }
                string level = "(" + f.Level() + "; " + sizeOfUnv + ")";

                AuxLoadDataGridViewOptimizationResum(dgvEx, name, level);
            }

            // Obtenmos el primer elmento para rellenar las dos primeras columnas de datos
            TableG_Study_Percent gp = analysis_G_Study_Opt.TableG_Study_Percent();

            // Numero de decimales para la representación
            int numOfDecimal = cfgApli.GetNumberOfDecimals();
            // Punto de separación decimal
            string puntoDecimal = this.cfgApli.GetDecimalSeparator();

            AuxLoadDataGridViewOptimizationResum(dgvEx, this.total_nb_obs, tbAnalisisVar.ListFacets().MultiOfLevel());
            AuxLoadDataGridViewOptimizationResum(dgvEx, this.relat_measmt, ConvertNum.DecimalToString(gp.CoefG_Rel(), numOfDecimal, puntoDecimal));
            AuxLoadDataGridViewOptimizationResum(dgvEx, this.absol_measmt, ConvertNum.DecimalToString(gp.CoefG_Abs(), numOfDecimal, puntoDecimal));
            AuxLoadDataGridViewOptimizationResum(dgvEx, this.relat_err_var, ConvertNum.DecimalToString(gp.TotalRelErrorVar(), numOfDecimal, puntoDecimal));
            AuxLoadDataGridViewOptimizationResum(dgvEx, this.absol_err_var, ConvertNum.DecimalToString(gp.TotalAbsErrorVar(), numOfDecimal, puntoDecimal));
            AuxLoadDataGridViewOptimizationResum(dgvEx, this.stand_relat_err, ConvertNum.DecimalToString(gp.ErrorRelStandDev(), numOfDecimal, puntoDecimal));
            AuxLoadDataGridViewOptimizationResum(dgvEx, this.stand_absol_err, ConvertNum.DecimalToString(gp.ErrorAbsStandDev(), numOfDecimal, puntoDecimal));

            dgvEx.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;

            List<G_ParametersOptimization> listG_ParametersOpt = analysis_G_Study_Opt.ListG_P_Optimization();

            int l = listG_ParametersOpt.Count; 
                // this.listG_Parameters.Count;
            for (int i = 0; i < l; i++)
            {
                G_ParametersOptimization gp_aux = listG_ParametersOpt[i];

                ListFacets newListFacets = gp_aux.G_ListFacets();
                // AddColunmToDGVOptimization(gp_aux, this.dGridViewExOptimizationResum);
                AddColunmToDGVOptimization(newListFacets, gp_aux, dgvEx);

            }

            // Ajustamos la altura de las filas
            // dgvEx.AutoResizeRows(DataGridViewAutoSizeRowsMode.DisplayedCells);
            dgvEx.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

        }// end private void LoadDataGridViewExOptimizationResum


        /* Descripción:
         *  Operción auxiliar inserta la primeras dos columnas de datos en la tabla de optimización. 
         *  Esta son el nombre de la variable resumen y su valor inicial.
         */
        private void AuxLoadDataGridViewOptimizationResum(DataGridView dgv, object a, object b)
        {
            object[] my_Row1 = new object[2];
            my_Row1[0] = a;
            my_Row1[1] = b;
            dgv.Rows.Add(my_Row1);
        }


        /*
         * Descripción:
         *  Añade una nueva columna a la tabla de resumen de datos, pestaña de optimización.
         *  
         * Parámetros:
         *  ListFacets newListFacets: Lista de facetas con los nuevos niveles de optimización.
         *  G_ParametersOptimization newG_Parameters: Tabla de G-Parámetros.
         *  TableAnalysisOfVariance tbAnalysisVar: Tabla análisis de varianza
         *  DataGridViewEx.DataGridViewEx dgvExOptimization: Donde se mostrarán los datos.
         */
        private void AddColunmToDGVOptimization(ListFacets newListFacets,G_ParametersOptimization newG_Parameters
            ,DataGridViewEx.DataGridViewEx dgvExOptimization)
        {
            // Creamos la nueva columna y la añadimos
            DataGridViewColumn dgc = new DataGridViewColumn();
            dgc.CellTemplate = dgvExOptimization.Columns[0].CellTemplate;
            dgc.AutoSizeMode = dgvExOptimization.Columns[0].AutoSizeMode;
            dgc.ReadOnly = true;
            dgc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight;
            //int num_col_pos = this.listG_Parameters.Count;
            int num_col_pos = dgvExOptimization.ColumnCount;
            dgc.Name = resum + " " + num_col_pos.ToString();
            dgvExOptimization.Columns.Add(dgc);
            
            // Usamos la nueva lista de facetas y la mostramos
            ListFacets lf = newG_Parameters.G_ListFacets();

            int numFacets = lf.Count();
            for (int i = 0; i < numFacets; i++)
            {
                string name = newListFacets.FacetInPos(i).Name();
                Facet f = lf.LookingFacet(name);
                string sizeOfUnv = Facet.INFINITE;
                int s = f.SizeOfUniverse();
                if (!int.MaxValue.Equals(s))
                {
                    sizeOfUnv = s.ToString();
                }
                dgvExOptimization.Rows[i].Cells[num_col_pos].Value = "(" + f.Level() + "; " + sizeOfUnv + ")";
            }

            // Número de decimales para la representación
            int numOfDecimal = cfgApli.GetNumberOfDecimals();
            // Punto de separación decimal
            string puntoDecimal = this.cfgApli.GetDecimalSeparator();

            // Ahora rellenamos los datos
            // int n = dgvExOptimization.ColumnCount;
            dgvExOptimization.Rows[numFacets].Cells[num_col_pos].Value = lf.MultiOfLevel(); // newListTableSSQ.CalcTotalDF();
            dgvExOptimization.Rows[numFacets + 1].Cells[num_col_pos].Value =
                ConvertNum.DecimalToString(newG_Parameters.CoefG_Rel(), numOfDecimal, puntoDecimal);
            dgvExOptimization.Rows[numFacets + 2].Cells[num_col_pos].Value =
                ConvertNum.DecimalToString(newG_Parameters.CoefG_Abs(), numOfDecimal, puntoDecimal);
            dgvExOptimization.Rows[numFacets + 3].Cells[num_col_pos].Value =
                ConvertNum.DecimalToString(newG_Parameters.TotalRelErrorVar(), numOfDecimal, puntoDecimal);
            dgvExOptimization.Rows[numFacets + 4].Cells[num_col_pos].Value =
                ConvertNum.DecimalToString(newG_Parameters.TotalAbsErrorVar(), numOfDecimal, puntoDecimal);
            dgvExOptimization.Rows[numFacets + 5].Cells[num_col_pos].Value =
                ConvertNum.DecimalToString(newG_Parameters.ErrorRelStandDev(), numOfDecimal, puntoDecimal);
            dgvExOptimization.Rows[numFacets + 6].Cells[num_col_pos].Value =
                ConvertNum.DecimalToString(newG_Parameters.ErrorAbsStandDev(), numOfDecimal, puntoDecimal);
        }// end AddColunmToDGVOptimization


        /*
         * Descripción:
         *  Actua sobre las propiedades de una columna de un dataGridView indicandole su etiqueta y logitud
         *  que se pasan como parámetros.
         * Parámetros:
         *      DataGridView dgv: el dataGridView sobre el que queremos actuar.
         *      int pos: columna sobre la que queremos actuar.
         *      int lg: Longitud de la columna.
         *      string label: Etiqueta de la columna.
         */
        private void PropertyColumnDGV(DataGridViewEx.DataGridViewEx dgv, int pos, int lg, string label)
        {
            // dgv.Columns[pos].Name = label; // Nombre de la columna Descripción (dependerá del idioma).
            dgv.Columns[pos].HeaderText = label;
            dgv.Columns[pos].Width = lg;
            dgv.Columns[pos].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns[pos].ReadOnly = true; 
            dgv.Columns[pos].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight;
            dgv.Columns[pos].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }


        /* Descripción:
         *  Actua sobre las propiedades de una columna de un dataGridView indicandole su etiqueta y logitud
         *  que se pasan como parámetros.
         * Parámetros:
         *      DataGridView dgv: el dataGridView sobre el que queremos actuar.
         *      int pos: columna sobre la que queremos actuar.
         *      string label: Etiqueta de la columna.
         */
        private void PropertyColumnDGV(DataGridViewEx.DataGridViewEx dgv, int pos, string label)
        {
            // dgv.Columns[pos].Name = label; // Nombre de la columna Descripción (dependerá del idioma).
            dgv.Columns[pos].HeaderText = label;
            // dgv.Columns[pos].Width = lg;
            // dgv.Columns[pos].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns[pos].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns[pos].ReadOnly = true;
            dgv.Columns[pos].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight;
            dgv.Columns[pos].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }


        /* Descripción:
         *  Añadimos un nuevo nivel de optimización al estudió.
         */
        private void tsmiActionAddLevelSign_Click(Analysis_and_G_Study tablesOfAnalysisG)
        {
            // Combrobamos que haya un objeto de tipo Tabla de análisis
            if(tablesOfAnalysisG == null)
            {
                ShowMessageErrorOK(errorNoSSQ);
            }
            else
            {
                G_ParametersOptimization newG_ParametersOpt = AddSignificanceLevel(tablesOfAnalysisG);

                if (newG_ParametersOpt != null)
                {
                    tablesOfAnalysisG.AddG_Parameter(newG_ParametersOpt);

                    // Posicionamos el tabPage de optimización
                    this.tabControlSSQ.SelectedIndex = 2; // El dos se corresponde con el tabPabge optimización
                    ListFacets listFactes = tablesOfAnalysisG.TableAnalysisVariance().ListFacets();
                    // Añadimos una nueva columna
                    AddColunmToDGVOptimization(listFactes, newG_ParametersOpt, this.dGridViewExOptimizationResum);
                }
            }
        }// end tsmiActionAddLevelSign_Click


        /*
         * Descripción:
         *  Pregunta los nuevos datos de significación al usuario (nuevos niveles y tamaño del universo 
         *  para las facetas de intrumentación, y tamaño del universo para las facetas de 
         *  diferenciación), calcula a partir de las nuevas facetas los coeficientes de 
         *  generalizabilidad y los añade al dataGridView de resumen de datos.
         *  
         * Parámetros:
         *  Analysis_and_G_Study tablesOfAnalysisG: El objeto que contiene las tablas de análisis, G-Parámetros
         *  y optimización.
         */
        private G_ParametersOptimization AddSignificanceLevel(Analysis_and_G_Study tablesOfAnalysisG)
        {
            G_ParametersOptimization retVal = null; // valor de retorno

            TransLibrary.Language lang = this.LanguageActually();
            // Lista de facetas de instrumentación original
            ListFacets lfInst = tablesOfAnalysisG.TableG_Study_Percent().LfInstrumentation();
            // Lista de facetas de diferenciación original
            ListFacets lfDiff = tablesOfAnalysisG.TableG_Study_Percent().LfDifferentiation();
            // Ventana donde introducimos los nuevos niveles
            FormAddSignificanceLevels formAddSign = new FormAddSignificanceLevels(lang, this.dicMeans, lfInst, lfDiff);

            bool salir = false; // variable de salida de bucle
            do
            {// (*1*)
                DialogResult res = formAddSign.ShowDialog();
                switch (res)
                {//switch (*1*)
                    case (DialogResult.Cancel): salir = true; break;
                        // Hemos cancelado la operación y salimos del bucle
                    case (DialogResult.OK):
                        // Para facetas de instrumentación
                        int numInstFacets = lfInst.Count(); 
                        // Para facetas de diferenciación
                        int numDiffFacets = lfDiff.Count(); 

                        // DataGridViewEx con las facetas de instrumentación
                        DataGridViewEx.DataGridViewEx dgvExAddInstLevelSign = formAddSign.DataGridViewExAddInstrumentationLevels();
                        // DataGridViewEx con las facetas de diferenciación
                        DataGridViewEx.DataGridViewEx dgvExAddDiffLevelSign = formAddSign.DataGridViewExAddDifferentiationLevels();

                        bool correct = true;
                        try
                        {
                            // Verificamos que esten correctamente las facetas de instrumentación
                            for (int i = 0; i < numInstFacets && correct; i++)
                            {
                                DataGridViewRow my_row = dgvExAddInstLevelSign.Rows[i];
                                if ((my_row.Cells[3].Value == null)
                                    || (my_row.Cells[4].Value == null))
                                {
                                    correct = false;
                                    ShowMessageErrorOK(errorValueNullOrEmpty);
                                }
                                else
                                {
                                    int level = int.Parse(my_row.Cells[3].Value.ToString());
                                    int size = readSizeOfUniverse(my_row.Cells[4].Value.ToString());

                                    if (level > size)
                                    {// (*2*)
                                        correct = false;
                                        ShowMessageErrorOK(errorOverUniverse);
                                        // MessageBox.Show("El tamaño de el nivel es mayor que el universo");
                                    }//end if (*2*)
                                }
                            }// end for

                            // Verificamos que esten correctamente las facetas de diferenciación
                            for (int i = 0; i < numDiffFacets && correct; i++)
                            {
                                DataGridViewRow my_row = dgvExAddDiffLevelSign.Rows[i];
                                if ((my_row.Cells[1].Value == null)
                                    || (my_row.Cells[3].Value == null))
                                {
                                    correct = false;
                                    ShowMessageErrorOK(errorValueNullOrEmpty);
                                }
                                else
                                {
                                    int level = int.Parse(my_row.Cells[1].Value.ToString());
                                    int newSize = readSizeOfUniverse(my_row.Cells[3].Value.ToString());

                                    if (level > newSize)
                                    {// (*3*)
                                        correct = false;
                                        ShowMessageErrorOK(errorOverUniverse);
                                        // MessageBox.Show("El tamaño de el nivel es mayor que el universo");
                                    }//end if (*3*)
                                }
                            }// end for

                        }
                        catch (FormatException)
                        {
                            // Se produjo la excepción al obtener el nivel de la faceta
                            ShowMessageErrorOK(errorLevelFormat);
                            //MessageBox.Show(formEx.Message);
                            correct = false;
                        }

                        if (correct)
                        {// (*4*)
                            try
                            {
                                // lista de facetas que contedra tanto las facetas de instrumentación como de diferenciación.
                                
                                ListFacets newlf = (ListFacets)tablesOfAnalysisG.TableAnalysisVariance().ListFacets().Clone();

                                /* Modificamos los valores de las facetas de intrumentación en la lista clonada.
                                 */
                                ListFacets newLevelListInstFacets = new ListFacets();
                                int numCol = dgvExAddInstLevelSign.Columns.Count - 1;
                                for (int i = 0; i < numInstFacets; i++)
                                {
                                    string name = dgvExAddInstLevelSign.Rows[i].Cells[0].Value.ToString();
                                    // el nuevo nivel se obtiene de la tabla
                                    int newlevel = int.Parse(dgvExAddInstLevelSign.Rows[i].Cells[numCol - 1].Value.ToString());
                                    int newSizeUni = readSizeOfUniverse(dgvExAddInstLevelSign.Rows[i].Cells[numCol].Value.ToString());
                                    
                                    Facet auxF = newlf.LookingFacet(name);
                                    auxF.Level(newlevel);
                                    auxF.SizeOfUniverse(newSizeUni);
                                    newLevelListInstFacets.Add(auxF);
                                }

                                /* Modificamos el tamaño de los niveles y del universo de la facetas de diferenciación.
                                 */
                                ListFacets newLevelListDiffFacets = new ListFacets();
                                numCol = dgvExAddDiffLevelSign.Columns.Count - 1;
                                for (int i = 0; i < numDiffFacets; i++)
                                {
                                    string name = dgvExAddDiffLevelSign.Rows[i].Cells[0].Value.ToString();
                                    // el nuevo nivel se obtiene de la tabla
                                    
                                    int newSizeUni = readSizeOfUniverse(dgvExAddDiffLevelSign.Rows[i].Cells[numCol].Value.ToString());
                                    
                                    Facet auxF = newlf.LookingFacet(name);
                                    
                                    auxF.SizeOfUniverse(newSizeUni);
                                    newLevelListDiffFacets.Add(auxF);
                                }

                                retVal = tablesOfAnalysisG.Calc_G_ParametersOptimización(newlf, newLevelListDiffFacets, newLevelListInstFacets);


                                /*===============================================================================================*/

                                salir = true;
                            }
                            catch (FormatException)
                            {
                                // Se produjo la excepción al obtener el nivel de la faceta
                                ShowMessageErrorOK(errorLevelFormat);
                                //MessageBox.Show(formEx.Message);
                                correct = false;
                            }
                            catch (FacetException)
                            {
                                // Se produjo la excepción al obtener el nivel de la faceta
                                ShowMessageErrorOK(errorLevelFormat);
                                //MessageBox.Show(formEx.Message);
                                correct = false;
                            }

                        }// end if (*4*)
                        break;
                }//end switch (*1*)
            } while (!salir);// (*1*)

            return retVal;
            
        }// end AddSignificanceLevel
      

        /* Descripción:
         *  Método auxiliar. Obtiene la nueva lista de facetas con los niveles introducidos 
         *  por el usuario para su optimización.
         */
        private ListFacets Aux_ReadNewListFacets(Analysis_and_G_Study tablesOfAnalysisG)
        {
            TransLibrary.Language lang = this.LanguageActually();
            ListFacets sourceOfVarInstrumentation = tablesOfAnalysisG.TableG_Study_Percent().LfInstrumentation();
            ListFacets sourceOfDifferentiation = tablesOfAnalysisG.TableG_Study_Percent().LfDifferentiation();
            FormAddSignificanceLevels formAddSign = new FormAddSignificanceLevels(lang, this.dicMeans, sourceOfVarInstrumentation, sourceOfDifferentiation);

            // lista de facetas que contedra tanto las facetas de instrumentación como de diferenciación.
            ListFacets newlf = new ListFacets();// valor de retorno

            bool salir = false;
            do
            {// (*1*)
                DialogResult res = formAddSign.ShowDialog();
                switch (res)
                {//switch (*1*)
                    case (DialogResult.Cancel): salir = true; break;
                    case (DialogResult.OK):
                        // Para factas de intrumentación
                        int numInstFacets = sourceOfVarInstrumentation.Count();
                        ListFacets lfInst = sourceOfVarInstrumentation;
                        // Para facetas de diferenciación
                        int numDiffFacets = sourceOfDifferentiation.Count();
                        ListFacets lfDiff = sourceOfDifferentiation;

                        // data gridviewEx con las facetas de instrumentación
                        DataGridViewEx.DataGridViewEx dgvExAddInstLevelSign = formAddSign.DataGridViewExAddInstrumentationLevels();
                        // data gridViewEx con las facetas de diferenciación
                        DataGridViewEx.DataGridViewEx dgvExAddDiffLevelSign = formAddSign.DataGridViewExAddDifferentiationLevels();

                        bool correct = true;
                        try
                        {
                            // Verificamos que esten correctamente las facetas de instrumentación
                            for (int i = 0; i < numInstFacets && correct; i++)
                            {
                                DataGridViewRow my_row = dgvExAddInstLevelSign.Rows[i];
                                int level = int.Parse(my_row.Cells[3].Value.ToString());
                                int size = readSizeOfUniverse(my_row.Cells[4].Value.ToString());
                                // Facet f = lfInst.FacetInPos(i);
                                if (level > size)
                                {// (*2*)
                                    correct = false;
                                    ShowMessageErrorOK(errorOverUniverse);
                                    // MessageBox.Show("El tamaño de el nivel es mayor que el universo");
                                }//end if (*2*)

                            }

                            // Verificamos que esten correctamente las facetas de diferenciación
                            for (int i = 0; i < numDiffFacets && correct; i++)
                            {
                                DataGridViewRow my_row = dgvExAddDiffLevelSign.Rows[i];
                                int level = int.Parse(my_row.Cells[1].Value.ToString());
                                int newSize = readSizeOfUniverse(my_row.Cells[3].Value.ToString());
                                // Facet f = lfDiff.FacetInPos(i);
                                if (level > newSize)
                                {// (*3*)
                                    correct = false;
                                    ShowMessageErrorOK(errorOverUniverse);
                                    // MessageBox.Show("El tamaño de el nivel es mayor que el universo");
                                }

                            }//end if (*3*)

                        }
                        catch (FormatException)
                        {
                            // Se produjo la excepción al obtener el nivel de la faceta
                            ShowMessageErrorOK(errorLevelFormat);
                            //MessageBox.Show(formEx.Message);
                            correct = false;
                        }

                        if (correct)
                        {// (*4*)
                            
                            /* Creamos una lista de facetas de instrumentación con los datos introducidos 
                             * por el usuario.
                             */
                            ListFacets newLevelListInstFacets = new ListFacets();
                            int numCol = dgvExAddInstLevelSign.Columns.Count - 1;
                            for (int i = 0; i < numInstFacets; i++)
                            {
                                Facet f = lfInst.FacetInPos(i);
                                string name = f.Name();
                                // el nuevo nivel se obtiene de la tabla
                                int level = int.Parse(dgvExAddInstLevelSign.Rows[i].Cells[numCol - 1].Value.ToString());
                                int sizeUni = readSizeOfUniverse(dgvExAddInstLevelSign.Rows[i].Cells[numCol].Value.ToString());
                                string comment = f.Comment();
                                string design = f.ListFacetDesing();
                                Facet auxF = new Facet(name, level, comment, sizeUni, design);
                                newLevelListInstFacets.Add(auxF);
                                newlf.Add(auxF);
                            }

                            /* Creamos una lista de facetas de diferenciación con los datos introducidos 
                             * por el usuario.
                             */
                            ListFacets newLevelListDiffFacets = new ListFacets();
                            numCol = dgvExAddDiffLevelSign.Columns.Count - 1;
                            for (int i = 0; i < numDiffFacets; i++)
                            {
                                Facet f = lfDiff.FacetInPos(i);
                                string name = f.Name();
                                // el nuevo nivel se obtiene de la tabla
                                int level = f.Level();
                                // int sizeUni = int.Parse(dgvExAddDiffLevelSign.Rows[i].Cells[numCol].Value.ToString());
                                int sizeUni = readSizeOfUniverse(dgvExAddDiffLevelSign.Rows[i].Cells[numCol].Value.ToString());
                                string comment = f.Comment();
                                string design = f.ListFacetDesing();
                                Facet auxF = new Facet(name, level, comment, sizeUni, design);
                                newLevelListDiffFacets.Add(auxF);
                                newlf.Add(auxF);
                            }

                            /* Tenemos que ordenar la lista de facetas newlf en el mismo orden que la original
                             * para luego no tener problemas para encontrar los datos */
                            TableAnalysisOfVariance tableAnalysis = tablesOfAnalysisG.TableAnalysisVariance();

                            /* Reordeno la nueva lista de facetas para que tenga en mismo orden que la original ya que de otro
                             * modo obtendréamos errores al mostrar los datos en la Tabla de resumen (opción de optimización).
                             */
                            ListFacets originalLF = tableAnalysis.ListFacets(); // lista de facetas original
                            newlf = originalLF.SortByListFacets(newlf); // nueva lista de facetas ordenada

                            salir = true;

                        }// end if (*4*)
                    break;
                }//end switch (*1*)
            } while (!salir);// (*1*)
            return newlf;
        }// end Aux_ReadNewListFacets


        /*
         * Descripción:
         *  Procedimiento para mostrar la ventana con los gráficos.
         * Parámetros:
         *      Analysis_and_G_Study tAnalysis_tG_Study_Opt: El objeto que contiene las tablas de análisis
         *          de sumas de cuadrados con los parámetros que vamos a mostrar.
         */
        private void ShowMeTheGraphics(Analysis_and_G_Study tAnalysis_tG_Study_Opt)
        {
            // Combrobamos que haya un objeto de tipo Tabla de análisis
            if (tAnalysis_tG_Study_Opt == null)
            {
                ShowMessageErrorOK(errorNoSSQ);
            }
            else if (this.formShowCharts == null && tAnalysis_tG_Study_Opt.TableG_Study_Percent() != null)
            {
                /* Codigo provisional */
                List<G_ParametersOptimization> list_G_Opt = new List<G_ParametersOptimization>();
                TableG_Study_Percent tableG_Study = tAnalysis_tG_Study_Opt.TableG_Study_Percent();
                list_G_Opt.Add(tableG_Study.G_ParametersOptimization());
                List<G_ParametersOptimization> listG_ParametersOpt = tAnalysis_tG_Study_Opt.ListG_P_Optimization();

                foreach (G_ParametersOptimization g in listG_ParametersOpt)
                {
                    list_G_Opt.Add(g);
                }

                this.formShowCharts = new FormShowCharts(this, cfgApli, list_G_Opt);
                formShowCharts.Show();
            }
        }// ShowMeTheGraphics


        /* Descripción:
         *  Se ejecuta al seleccióna la opción "Gráfico Coef. G Abs" del menú de acciónes de suma de cuadrados.
         *  Muestra una gráfica de representación lineal.
         */
        private void tsmiActionChartCoefGAbs_Click(Analysis_and_G_Study tAnalysis_tG_Study_Opt)
        {
            // Combrobamos que haya un objeto de tipo Tabla de análisis
            if (tAnalysis_tG_Study_Opt == null)
            {
                ShowMessageErrorOK(errorNoSSQ);
            }
            else if (this.formShowCharts == null)
            {
                TableG_Study_Percent tableG_Study = tAnalysis_tG_Study_Opt.TableG_Study_Percent();
                // primero preguntamos al ususario
                TransLibrary.Language lang = this.LanguageActually();
                ListFacets lf = tableG_Study.LfInstrumentation();
                FormOptionsForChart_Two formOptioms = new FormOptionsForChart_Two(lang, lf);
                bool salir = false;
                do
                {
                    DialogResult res = formOptioms.ShowDialog();

                    switch (res)
                    {
                        case DialogResult.Cancel: salir = true; break;
                        case DialogResult.OK:
                            CheckedListBox checkedLtBox = formOptioms.CheckedListBoxListFacets();
                            int beginning = formOptioms.Beginning(); // Comienzo de la representación
                            int ending = formOptioms.Ending(); // final de la representación
                            int increment = formOptioms.Increment(); // Valor del incremento

                            // Debe haber al menos una faceta seleccionada
                            if (checkedLtBox.CheckedItems.Count > 0)
                            {
                                if (beginning > 0 && ending > 0 && beginning < ending)
                                {
                                    salir = true;
                                    ListFacets lfSeleted = FacetsSelectedIn_cListBox(lf, checkedLtBox);

                                    try
                                    {
                                        FormShowCharts2 formShowCharts2 =
                                            new FormShowCharts2(cfgApli, tAnalysis_tG_Study_Opt, lfSeleted,
                                                true, this.tsmiChartCoefGAbs.Text, beginning, ending, increment);
                                        formShowCharts2.Show();
                                    }
                                    catch (InvalidOperationException inv_ex)
                                    {
                                        ShowMessageErrorOK(inv_ex.Message);
                                    }
                                }
                                else
                                {
                                    // El intervalo no es valido
                                    ShowMessageErrorOK(errorInvalidRange);
                                }
                            }
                            else
                            {
                                // Lanzamos un mensaje indicando que no hay ningún elemento seleccionado
                                ShowMessageErrorOK(errorNoFacetSelected);
                            }
                            break;
                    }

                } while (!salir);
            }
        }// end tsmiActionCharts2_Click


        /* Descripción:
         *  Devuelve la lista con las facetas seleccionadas.
         */
        private ListFacets FacetsSelectedIn_cListBox(ListFacets lf, CheckedListBox checkedLtBox)
        {
            ListFacets retListF = new ListFacets();
            int n = checkedLtBox.Items.Count;
            
            for (int i = 0; i < n; i++)
            {
                if (checkedLtBox.GetItemChecked(i))
                {
                    retListF.Add(lf.FacetInPos(i));
                }
            }
            return retListF;
        }


        /* Descripción:
         *  Se ejecuta al seleccióna la opción "Gráfico Coef. G Rel" del menú de acciónes de suma de cuadrados.
         *  Muestra una gráfica de representación lineal.
         */
        private void tsmiActionChartCoefGRel_Click(Analysis_and_G_Study tAnalysis_tG_Study_Opt)
        {
            // Combrobamos que haya un objeto de tipo Tabla de análisis
            if (tAnalysis_tG_Study_Opt == null)
            {
                ShowMessageErrorOK(errorNoSSQ);
            }
            else if (this.formShowCharts == null)
            {
                TableG_Study_Percent tableG_Study = tAnalysis_tG_Study_Opt.TableG_Study_Percent();
                TransLibrary.Language lang = this.LanguageActually();
                ListFacets lf = tableG_Study.LfInstrumentation();
                FormOptionsForChart_Two formOptioms = new FormOptionsForChart_Two(lang, lf);
                bool salir = false;
                do
                {
                    DialogResult res = formOptioms.ShowDialog();

                    switch (res)
                    {
                        case DialogResult.Cancel: salir = true; break;
                        case DialogResult.OK:
                            CheckedListBox checkedLtBox = formOptioms.CheckedListBoxListFacets();
                            int beginning = formOptioms.Beginning(); // Comienzo de la representación
                            int ending = formOptioms.Ending(); // final de la representación
                            int increment = formOptioms.Increment(); // Valor del incremento

                            // Debe haber al menos una faceta seleccionada
                            if (checkedLtBox.CheckedItems.Count > 0)
                            {
                                if (beginning > 0 && ending > 0 && beginning < ending)
                                {

                                    salir = true;
                                    ListFacets lfSeleted = FacetsSelectedIn_cListBox(lf, checkedLtBox);
                                    FormShowCharts2 formShowCharts2 = 
                                        new FormShowCharts2(cfgApli, tAnalysis_tG_Study_Opt, lfSeleted,
                                            false, this.tsmiChartCoefGRel.Text, beginning, ending, increment);
                                    formShowCharts2.Show();
                                }
                                else
                                {
                                    // El intervalo no es valido
                                    ShowMessageErrorOK(errorInvalidRange);
                                }
                            }
                            else
                            {
                                // Lanzamos un mensaje indicando que no hay ningún elemento seleccionado
                                ShowMessageErrorOK(errorNoFacetSelected);
                            }
                            break;
                    }

                } while (!salir);
            }
        }// end gráfico3ActionToolStripMenuItem_Click


        /* Descripción:
         *  Muestra el diseño de medida en los textBox
         * Parámetros:
         *  ListFacets sourceOfDifferentiation: Lista de facetas de diferenciación.
         *  ListFacets sourceOfInstrumentation: Lista de facetas de intrumentación.
         */
        private void ShowMeDessingInTextBoxs(ListFacets sourceOfDifferentiation, ListFacets sourceOfInstrumentation)
        {
            this.tbMeasurementDesign.Text = sourceOfDifferentiation.StringOfListFactes() +
                        "/" + sourceOfInstrumentation.StringOfListFactes();
            this.tbMesurDesign2.Text = this.tbMeasurementDesign.Text;
            this.tbMeasurementDesign2.Text = this.tbMeasurementDesign.Text;
        }


        /* Descripción:
         *  Cuando la ventana se cierra se encarga de volver a porner la variable formShowCharts a null.
         */
        public void FormShowChartsClosed()
        {
            this.formShowCharts = null;
        }


        /* Descripción:
         *  Muestra la ventana para la importación de los datos de suma de cuadrados de archivos .rsa (resultado de analisis
         *  GT E 2.0)
         */
        private void tsmiActionSSQImport_Click(object sender, EventArgs e)
        {
            // Esta ventana se mostrará mientras se carga el fichero
            // CWait fw = new CWait(msgLoading);
            // Thread th = new Thread(new ThreadStart(fw.CWaitShowDialog));

            try
            {
                TransLibrary.Language lang = this.LanguageActually();
                FormSSQImport formSSQ_Import = new FormSSQImport(lang);
                bool salir = false;
                do
                {
                    DialogResult res = formSSQ_Import.ShowDialog();
                    // quitamos y ponemos el foco en la ventana para que esta se actualize
                    this.Enabled = false;
                    this.Enabled = true;

                    switch (res)
                    {
                        case DialogResult.Cancel: salir = true; break;
                        case DialogResult.OK:
                            if (String.IsNullOrEmpty(formSSQ_Import.fileName()))
                            {
                                // lanzamos un mensaje de error: no hay fichero seleccionado
                                this.ShowMessageErrorOK(errorNoFileSelected);
                            }
                            else
                            {
                                // th.Start();
                                this.importSSqFile(formSSQ_Import.pathFile());
                                // th.Abort();
                                salir = true;
                            }
                            break;
                    }
                } while (!salir);
            }
            catch (SSqPY_Exception)
            {
                // th.Abort();
                // Se producjo un error al leer el archivo
                ShowMessageErrorOK(errorFormatFile);
            }
            catch (IOException)
            {
                // th.Abort();
                // MessageBox.Show("Otro porgrama esta usando el archivo");
                ShowMessageErrorOK(errorFileInUse);
            }
            catch (Exception ex)
            {
                // th.Abort();
                // Mostramos un mensaje indicando que el fichero no tiene el formato correcto
                // MessageBox.Show(ex.Message);
                ShowMessageErrorOK(ex.Message);
            }
        }// end tsmiActionSSQImport_Click


        /* Descripción:
         *  Importa un fichero de suma de cuadrados para construir el objeto listTableSSQ .
         */
        public void importSSqFile(string path)
        {
            // Esta ventana se mostrará mientras se carga el fichero
            //CWait fw = new CWait(msgLoading);
            //Thread th = new Thread(new ThreadStart(fw.CWaitShowDialog));
            //CWait fw2 = new CWait(msgLoading);
            //Thread th2 = new Thread(new ThreadStart(fw2.CWaitShowDialog));

            try
            {
                ListFacets sourceOfDifferentiation; // Lista de facetas de diferenciación
                ListFacets sourceOfInstrumentation; // Lista de facetas de intrumentación

                // Extraemos el nombre del fichero del path
                string fileExt = fileExtension(path).ToLower(); // Pasamos a minúsculas la extensión
                TypeOfFile typeOfFile = (TypeOfFile)Enum.Parse(typeof(TypeOfFile), fileExt, true);
                DateTime date = DateTime.Now;

                Analysis_and_G_Study tAnalysis_tG_Study_Opt = this.sagtElements.GetAnalysis_and_G_Study();
                // para poder compararla. 
                switch (typeOfFile)
                {
                    case (TypeOfFile.ssq):
                        // Mostramos todos los datos en los dataGridView
                        tAnalysis_tG_Study_Opt = Aux_loadListTableSSqOfFileSsq(path);
                        break;
                    case (TypeOfFile.rsa):
                        tAnalysis_tG_Study_Opt = Aux_loadListTableSSqOfFileRsa(path);
                        break;
                    case (TypeOfFile.txt):
                        //th.Start();
                        List<AnalysisSsqEduG> listAnalysisEduG = AnalysisSsqEduG.ReadFileReportTxtEduG(path);
                        //th.Abort();
                        tAnalysis_tG_Study_Opt = Aux_SelectAnalysisOfListAnalyisReports(listAnalysisEduG);
                        break;
                    case (TypeOfFile.rtf):
                        //th.Start();
                        List<AnalysisSsqEduG> listAnalysisEduG2 = AnalysisSsqEduG.ReadFileReportRtfEduG(path);
                        //th.Abort();
                        tAnalysis_tG_Study_Opt = Aux_SelectAnalysisOfListAnalyisReports(listAnalysisEduG2);
                        break;
                    case (TypeOfFile.xls):
                        // Ficheros xls de excel
                        tAnalysis_tG_Study_Opt = Aux_loadListTableSSqOfFileXls(path);
                        break;
                    default:
                        ShowMessageErrorOK("No se muestra ninguno");
                        break;
                }// end switch

                if (tAnalysis_tG_Study_Opt != null)
                {
                    //th2.Start();
                    date = DateTime.Now;
                    tAnalysis_tG_Study_Opt.SetDateTime(date);
                    tAnalysis_tG_Study_Opt.SetNameFileDataCreation(path);
                    // Mostramos el diseño de medida en los textBox
                    sourceOfDifferentiation = tAnalysis_tG_Study_Opt.List_Facets_Differentiation();
                    sourceOfInstrumentation = tAnalysis_tG_Study_Opt.List_Facets_Intrumentation();
                    ShowMeDessingInTextBoxs(sourceOfDifferentiation, sourceOfInstrumentation);
                    // Mostramos todos los datos en los dataGridView
                    LoadAllDataInDataGridViewEx_SSQOptions(tAnalysis_tG_Study_Opt);
                    // Actualizamos
                    this.sagtElements.SetAnalysis_and_G_Study(tAnalysis_tG_Study_Opt);
                    // mostramos el tabPage de suma de cuadrados
                    this.tabPageSSQ.Parent = this.tabControlOptions;
                    //th2.Abort();
                }
            }
            catch (ImportEduGSsq.AnalysisSsqEduG_Exception)
            {
                //th.Abort();
                //th2.Abort();
                // Se producjo un error al leer el archivo
                ShowMessageErrorOK(errorFormatFile);
            }
        }// end importSSqFile


        /* Descripción:
         *  Importa un fichero de suma de cuadrados .ssq del programa GT E 2.0 para construir el objeto 
         *  listTableSSQ .
         */
        public Analysis_and_G_Study Aux_loadListTableSSqOfFileSsq(string path)
        {
            SSqPY ssqPY = SSqPY.ReadFileSsqPY(path);
            ListFacets sourceOfDifferentiation = ssqPY.SourceOfVarDepend();
            ListFacets sourceOfInstrumentation = ssqPY.SourceOfVarInDepend();

            TableG_Study_Percent tableG_Study = new TableG_Study_Percent(sourceOfDifferentiation, sourceOfInstrumentation, ssqPY);

            return new Analysis_and_G_Study(ssqPY, tableG_Study);
        }


        /* Descripción:
         *  Importa un fichero de resultado de suma de cuadrados .rsa del programa GT E 2.0 para construir el objeto 
         *  listTableSSQ.
         */
        private Analysis_and_G_Study Aux_loadListTableSSqOfFileRsa(string path)
        {
            RsaSsqPY rsaFile = RsaSsqPY.ReadFileRsaPY(path);
            List<SSqPY> ssqPY = rsaFile.List_SsqOfFile();
            SSqPY ssqPYaux = ssqPY[0];

            ListFacets sourceOfDifferentiation = ssqPYaux.SourceOfVarDepend();
            ListFacets sourceOfInstrumentation = ssqPYaux.SourceOfVarInDepend();

            // Inicializamos la lista de G_Parameters
            List<TableG_Study_Percent> listG_Parameters = rsaFile.SssqListOfG_Parameters();

            List<G_ParametersOptimization> listG_ParametersOpt = new List<G_ParametersOptimization>();
            int n = listG_Parameters.Count;
            for (int i = 1; i < n; i++)
            {
                listG_ParametersOpt.Add(listG_Parameters[i].G_ParametersOptimization());
            }

            return new Analysis_and_G_Study(ssqPY[0], listG_Parameters[0], listG_ParametersOpt);
        }// loadListTableSSqOfFileRsa


        /* Descripción:
         *  Devuelve un objeto con la lista tablas de análisis importado de un informe de suma de cuadrados de
         *  EduG 6.0
         */
        private Analysis_and_G_Study Aux_SelectAnalysisOfListAnalyisReports(List<AnalysisSsqEduG> listAnalysisEduG)
        {

            try
            {
                
                Analysis_and_G_Study retVal = null; // Valor de retorno

                List<string> listString = new List<string>();

                for (int i = 0; i < listAnalysisEduG.Count; i++)
                {
                    listString.Add(nameAnalysisDocument + " " + (i + 1) + ";   " + listAnalysisEduG[i].GetDateTime().ToString());
                }


                TransLibrary.Language lang = this.cfgApli.GetConfigLanguage();
                FormSelectionOneItemReport formSelectionOne = new FormSelectionOneItemReport(listString, lang,
                    FormSelectionOneItemReport.TypeSelectReport.Analysis);

                bool salir = false;
                do
                {

                    // Refrescamos la ventana
                    DialogResult res = formSelectionOne.ShowDialog();
                    switch (res)
                    {
                        case DialogResult.Cancel:
                            salir = true;
                            break;
                        case DialogResult.OK:
                            int pos = formSelectionOne.SelectionIndex();
                            if (pos >= 0 && pos <= listAnalysisEduG.Count)
                            {
                                retVal = listAnalysisEduG[pos];
                                salir = true;
                            }
                            else
                            {
                                // Mostramos un mensaje de error mostrando que no se ha seleccionado ninguno
                                ShowMessageErrorOK(txtMessageNoSelected);
                            }

                            break;
                    }
                } while (!salir);
                return retVal;
            }catch(ImportEduGSsq.AnalysisSsqEduG_Exception)
            {
                // Se producjo un error al leer el archivo
                ShowMessageErrorOK(errorFormatFile);
                return null;
            }
        }// end Aux_SelectAnalysisOfListAnalyisReports


        /* Descripción:
         *  Importa un fichero con los resultado de suma de cuadrados .xls de excel generados a partir
         *  de este mismo programa.
         */
        private Analysis_and_G_Study Aux_loadListTableSSqOfFileXls(string path)
        {
            List<string> namesTables = ImportExcel.GetTableExcel(path);
            Analysis_and_G_Study tAnalysisSsq = null;
            if (namesTables.Count != 4)
            {
                // No esta en el formato correcto
                MessageBox.Show(errorFormatFile);
            }
            else
            {
                ListFacets lf = null;
                TableAnalysisOfVariance tableAnalysis = null;
                TableG_Study_Percent tableG = null;
                List<G_ParametersOptimization> tableResum = null;

                // Necesiatamos averiagura el tipo de tabla de medias
                TransLibrary.ReadFileTrans dic = new TransLibrary.ReadFileTrans(Application.StartupPath + LANG_PATH + DATA_STRINGS);
                TransLibrary.WordTranslation transFacets = dic.labelTraslation(tabPagMultiFacet.Name.ToString());
                dic = new TransLibrary.ReadFileTrans(Application.StartupPath + LANG_PATH + SSQ_STRINGS);
                TransLibrary.WordTranslation transSSq = dic.labelTraslation(tabPageSSQ_TableComp.Name.ToString());
                TransLibrary.WordTranslation transG_p = dic.labelTraslation(tabPageG_Parameters.Name.ToString());
                TransLibrary.WordTranslation transResum = dic.labelTraslation(tabPageOptimization.Name.ToString());


                string nameTableFacets = ""; // Contendrá el nombre de la tabla de facetas
                string nameTableAnalysisOfVariance = ""; // Contendrá el nombre de la tabla de análisis de varianza
                string nameTableG_p = ""; // Contendrá el nombre de la tabla G Study
                string nameTableResum = ""; // Contendrá el nombre de la tabla de optimización

                int n = namesTables.Count;
                
                for (int i = 0; i < n; i++)
                {
                    string nameTable = namesTables[i];
                    if(nameTable[0].Equals('\''))
                    {
                        nameTable = nameTable.Substring(1);
                        if (nameTable.LastIndexOf('\'') == (nameTable.Length - 1))
                        {
                            nameTable = nameTable.Remove((nameTable.Length - 1));
                        }
                    }
                    int num = nameTable.LastIndexOf("$");
                    string auxNameTable = nameTable.Remove(num);// eliminamos la última posición

                    if (transFacets.TranslationIncluded(auxNameTable))
                    {
                        nameTableFacets = nameTable;
                    }

                    if (transSSq.TranslationIncluded(auxNameTable))
                    {
                        nameTableAnalysisOfVariance = nameTable;
                    }

                    if (transG_p.TranslationIncluded(auxNameTable))
                    {
                        nameTableG_p = nameTable;
                    }

                    if (transResum.TranslationIncluded(auxNameTable))
                    {
                        nameTableResum = nameTable;
                    }
                }// end for


                // Tabla con la lista de facetas
                DataTable dtFacets = ImportExcel.GetDataTableExcel(path, nameTableFacets);
                lf = DataTable2ListFacets(dtFacets);

                // Tabla de análisis de suma de cuadrados
                DataTable dtSsqTable = ImportExcel.GetDataTableExcel(path, nameTableAnalysisOfVariance);
                tableAnalysis = DataTable2TableAnalysisOfVariance(dtSsqTable, lf);

                // Tabla resumen
                DataTable dtResumTable = ImportExcel.GetDataTableExcel(path, nameTableResum);
                tableResum = DataTable2TableResum(dtResumTable, lf);

                G_ParametersOptimization g_p_op = tableResum[0];
                tableResum.Remove(g_p_op);

                // Tabla de G-Parámetros
                DataTable dtG_pTable = ImportExcel.GetDataTableExcel(path, nameTableG_p);
                tableG = DataTable2TableG_Study(dtG_pTable, lf, g_p_op);

                tAnalysisSsq = new Analysis_and_G_Study(tableAnalysis, tableG, tableResum);
            }// end if

            return tAnalysisSsq;
        }// Aux_loadListTableSSqOfFileRsa


        /* Descripción:
         *  Toma un dataTable y una lista de facetas y devuelve una tabla de análisis de varianza.
         */
        private TableAnalysisOfVariance DataTable2TableAnalysisOfVariance(DataTable dt, ListFacets lf)
        {
            List<string> ldesign = new List<string>(); // Lista de diseños contendrá las claves

            Dictionary<string, double> df = new Dictionary<string,double>(); // grado de libertad 
            Dictionary<string, double?> ssq = new Dictionary<string,double?>(); // suma de cuadrados
            Dictionary<string, double?> msq = new Dictionary<string,double?>(); // Suma de cuadrados medios (M.S.C.)

            // Componente de Varianza aleatorio
            Dictionary<string, double?> randomComp = new Dictionary<string,double?>();
            // Componente de Varianza Mixto
            Dictionary<string, double?> mixComp = new Dictionary<string,double?>(); 
            // Componente de Varianza Corregido
            Dictionary<string, double?> correcComp = new Dictionary<string,double?>();

            // Porcentaje
            Dictionary<string, double?> porcentage = new Dictionary<string,double?>();
            // Error estandar
            Dictionary<string, double?> standardError = new Dictionary<string,double?>();

            // Bucle en el que cargamos los datos
            int r = dt.Rows.Count;
            for (int i = 0; i < r; i++)
            {
                DataRow row = dt.Rows[i];
                string design = (string)row[0].ToString();
                ldesign.Add(design);
                double? c_ssq = ConvertNum.String2Double((string)row[1].ToString());
                ssq.Add(design, c_ssq);
                double? c_df = ConvertNum.String2Double((string)row[2].ToString());
                df.Add(design, (double)c_df);
                double? c_msq = ConvertNum.String2Double((string)row[3].ToString());
                msq.Add(design, c_msq);
                double? c_randon = ConvertNum.String2Double((string)row[4].ToString());
                randomComp.Add(design, c_randon);
                double? c_mix = ConvertNum.String2Double((string)row[5].ToString());
                mixComp.Add(design, c_mix);
                double? c_correc = ConvertNum.String2Double((string)row[6].ToString());
                correcComp.Add(design, c_correc);
                double? c_porcen = ConvertNum.String2Double((string)row[7].ToString());
                porcentage.Add(design, c_porcen);
                double? c_standard = ConvertNum.String2Double((string)row[8].ToString());
                standardError.Add(design, c_standard);
            }
            
            // Valor de retorno 
            return new TableAnalysisOfVariance(lf, ldesign, ssq, df, msq, randomComp, mixComp, correcComp,
                porcentage, standardError);
        }// end DataTable2TableAnalysisOfVariance


        /* Descripción:
         *  Genera una tabla de análisis de varianza a partir de un dataTable.
         */
        private TableG_Study_Percent DataTable2TableG_Study(DataTable dt, ListFacets lf, 
            G_ParametersOptimization g_p)
        {
            ListFacets lfDifferentiation = new ListFacets();
            ListFacets lfInstrumentation = new ListFacets();
            Dictionary<string, double?> differentiationVar = new Dictionary<string,double?>();
            Dictionary<string, ErrorVar> errorVar = new Dictionary<string,ErrorVar>();
            Dictionary<string, ErrorVar> percentError = new Dictionary<string,ErrorVar>();

            int r = dt.Rows.Count;
            for (int i = 0; i < r; i++)
            {
                DataRow row = dt.Rows[i];
                if (!string.IsNullOrEmpty((string)row[0].ToString()))
                {
                    string design = (string)row[0].ToString();
                    ListFacets newLf = lf.ListDesignFacets(design);
                    lfDifferentiation = lfDifferentiation.ConcatenateWithoutRepetitions(newLf);
                    double? d = ConvertNum.String2Double((string)row[1].ToString());
                    differentiationVar.Add(design, d);
                }
                if (!string.IsNullOrEmpty((string)row[2].ToString()))
                {
                    string design = (string)row[2].ToString();
                    ListFacets newLf = lf.ListDesignFacets(design);
                    lfInstrumentation = lfInstrumentation.ConcatenateWithoutRepetitions(newLf);
                    double? e1 = ConvertNum.String2Double((string)row[3].ToString());
                    double? p1 = ConvertNum.String2Double((string)row[4].ToString());
                    double? e2 = ConvertNum.String2Double((string)row[5].ToString());
                    double? p2 = ConvertNum.String2Double((string)row[6].ToString());
                    ErrorVar error = new ErrorVar(e1, e2);
                    ErrorVar percen = new ErrorVar(p1, p2);
                    errorVar.Add(design, error);
                    percentError.Add(design, percen);
                }
            }
            lfInstrumentation = lfInstrumentation.Difference(lfDifferentiation);

            return new TableG_Study_Percent(lfDifferentiation, lfInstrumentation, differentiationVar,
                errorVar, percentError, g_p);
        }// end DataTable2TableG_Study


        /* Descripción:
         *  Toma del dataTable con la tabla resumen que se pasa como parámetro, y construye la lista 
         *  de G_Parámetros.
         */
        private List<G_ParametersOptimization> DataTable2TableResum(DataTable dt, ListFacets lf)
        {
            List<G_ParametersOptimization> listG_p = new List<G_ParametersOptimization>();
            int r = dt.Rows.Count;
            int c = dt.Columns.Count;

            for (int j = 1; j < c; j++)
            {
                ListFacets opListFacets = new ListFacets();

                for (int i = 0; i < (r - 7); i++)
                {
                    Facet f = lf.LookingFacet((string)dt.Rows[i][0].ToString());
                    string levelAndUniverse = (string)dt.Rows[i][j].ToString();

                    char[] delimeterChars2 = { ' ', '(', ';',')' }; // nuestro delimitador será el caracter '/'
                    string[] arrayOfSplit = levelAndUniverse.Split(delimeterChars2, StringSplitOptions.RemoveEmptyEntries);
                    int level = int.Parse(arrayOfSplit[0]);
                    f.Level(level);
                    if (arrayOfSplit[1].Equals(Facet.INFINITE))
                    {
                        f.SizeOfUniverse(int.MaxValue);
                    }
                    else
                    {
                        int size = int.Parse(arrayOfSplit[1]);
                        f.SizeOfUniverse(size);
                    }
                    opListFacets.Add(f);
                }

                // Suma total de las varianzas de las fuentes objetivo
                double total_differentiation_var = 0;
                // coeficente G relativo
                double coefG_Rel = (double)ConvertNum.String2Double((string)dt.Rows[r - 6][j].ToString()); ;
                // Coeficiente G absoluto
                double coefG_Abs = (double)ConvertNum.String2Double((string)dt.Rows[r - 5][j].ToString()); ;
                // Varianza del error relativa
                double totalRelErrorVar = (double)ConvertNum.String2Double((string)dt.Rows[r - 4][j].ToString());
                // Varianza del error absoluta
                double totalAbsErrorVar = (double)ConvertNum.String2Double((string)dt.Rows[r - 3][j].ToString());
                // Desviación típica relativa
                double errorRelStandDev = (double)ConvertNum.String2Double((string)dt.Rows[r - 2][j].ToString()); ;
                // Desviación típica absoluta
                double errorAbsStandDev = (double)ConvertNum.String2Double((string)dt.Rows[r - 1][j].ToString());
                
                // desviación típica de las fuentes objetivo
                double targetStandDev = 0;

                G_ParametersOptimization g_p = new G_ParametersOptimization(opListFacets, total_differentiation_var,
                    coefG_Rel, coefG_Abs, totalRelErrorVar, totalAbsErrorVar, errorRelStandDev, errorAbsStandDev,
                    targetStandDev);
                listG_p.Add(g_p);
            }
            
            return listG_p;
        }// end DataTable2TableResum


        /* Descripción:
         *  Cierra todos los elementos abiertos en suma de cuadrados.
         */
        private void tsmiActionSSQClose_Click(object sender, EventArgs e)
        {
            Analysis_and_G_Study tAnalysis_tG_Study_Opt = this.sagtElements.GetAnalysis_and_G_Study();
            if (tAnalysis_tG_Study_Opt != null)
            {
                TableAnalysisOfVariance tableAnalysis = tAnalysis_tG_Study_Opt.TableAnalysisVariance();
                if (tableAnalysis != null)
                {
                    DialogResult res = ShowMessageDialog(titleConfirm, txtConfirmClose);
                    switch (res)
                    {
                        case (DialogResult.OK):
                            // Llamamos al método que lo limpia los elementos
                            ClearTabPageSSQ();
                            break;
                    }
                }
            }
        }// end tsmiActionSSQClose_Click


        /*
         * Descripción:
         *  Limpia los dataGridView de los tabPageSSQ y libera recursos de memoria liberados para tal fin.
         */
        private void ClearTabPageSSQ()
        {
            // limpiamos los label
            ClearListBoxSSQ();
            // limpiamos los label de diseño de medida
            string mDesign = "";
            this.tbMeasurementDesign.Text = mDesign;
            this.tbMesurDesign2.Text = mDesign;
            this.tbMeasurementDesign2.Text = mDesign;
            // Limpiamos los campos del tabPage de información
            this.tbNameFileSsqInfo.Text = mDesign;
            this.tbDateFileSsqInfo.Text = mDesign;
            this.richTextBoxSsqComment.Text = mDesign;
            // Limpiamos los dataGridViewEx;
            ClearDataGridViewEx(dGridViewExSourceOfVar);
            ClearDataGridViewEx(dataGridViewExSSQ);
            ClearDataGridViewEx(dGridViewExG_Parameters);
            ClearDataGridViewEx(dGridViewExFacetsOptimization);
            ClearDataGridViewEx(dGridViewExOptimizationResum);
            // Ponemos las variables a null
            this.sagtElements.SetAnalysis_and_G_Study(null);

        }// end ClearTabPageSSQ()


        /* Descripción:
         *  Operación auxiliar. Limpia los dataGridView, ocultando los campos de de las etiquetas de las 
         *  columnas.
         * Parámetros:
         *      DataGridViewEx dgvEx: El dataGridViewEx que quereos limpiar.
         */
        private void ClearDataGridViewEx(DataGridViewEx.DataGridViewEx dgvEx)
        {
            dgvEx.NumeroFilas = 0;
            dgvEx.Rows.Clear();
            dgvEx.ColumnHeadersVisible = false; // Ocultamos el encabezado de una tabla
        }


        /* Descrpción:
         *  Generar un archivo Excel a partir de los datos contenidos en las tablas de suma de cuadrados.
         */
        private void tsmiActionSSq_ExportExcel_Click(object sender, EventArgs e)
        {
            Analysis_and_G_Study tAnalysis_tG_Study_Opt = this.sagtElements.GetAnalysis_and_G_Study();
            if (tAnalysis_tG_Study_Opt == null)
            {
                ShowMessageErrorOK(errorNoSSQ);
            }
            else
            {
                // ---------- cuadro de dialogo para Guardar
                SaveFileDialog saveDialog = new SaveFileDialog();

                if (Directory.Exists(this.cfgApli.Get_Path_Workspace()))
                {
                    saveDialog.InitialDirectory = this.cfgApli.Get_Path_Workspace();
                }

                saveDialog.DefaultExt = DEFAULT_EXT_EXCEL; // extensión por defecto del fichero
                string fileFilter = "xls file" + FILTER_EXCEL;
                saveDialog.Filter = fileFilter;
                saveDialog.AddExtension = true;
                saveDialog.RestoreDirectory = true;
                saveDialog.Title = titleSave; // Título de la ventana de salvado
                // CuadroDialogo.InitialDirectory = @"c:\";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportExcel expExcel = new ExportExcel();
                    expExcel.addXlsWorksheet(dGridViewExOptimizationResum, tabPageOptimization.Text);
                    expExcel.addNewXlsWorksheet(dGridViewExG_Parameters, tabPageG_Parameters.Text);
                    expExcel.addNewXlsWorksheet(dataGridViewExSSQ, tabPageSSQ_TableComp.Text);
                    expExcel.addNewXlsWorksheet(dGridViewExSourceOfVar, tabPagMultiFacet.Text);

                    expExcel.saveFileExcel(saveDialog.FileName);


                    // MessageBox.Show("Fin");
                    saveDialog.Dispose();
                    saveDialog = null;
                    expExcel.aplicationExcelQuit();
                }
            }
        }// end tsmiActionSSq_ExportExcel_Click


        /* Descripción:
         *  Muestra la ventana para editar los comentarios en la pestaña información de la opción
         *  suma de cuadrados.
         */
        private void btActionSSQEditComment_Click(object sender, EventArgs e)
        {
            if ((sagtElements.GetAnalysis_and_G_Study() == null))
            {
                ShowMessageErrorOK(errorNoFileSelected);
            }
            else
            {
                TransLibrary.Language lang = this.LanguageActually();
                string text = this.richTextBoxSsqComment.Text;
                FormEditFileComment formEditComment = new FormEditFileComment(text, lang);
                bool salir = false;
                do
                {
                    DialogResult res = formEditComment.ShowDialog();
                    switch (res)
                    {
                        case DialogResult.Cancel: salir = true; break;
                        case DialogResult.OK:
                            Analysis_and_G_Study tAnalysis_tG_Study_Opt = this.sagtElements.GetAnalysis_and_G_Study();
                            tAnalysis_tG_Study_Opt.SetTextComment(formEditComment.GetRichTextBoxText());
                            this.richTextBoxSsqComment.Text = tAnalysis_tG_Study_Opt.GetTextComment();

                            salir = true;
                            break;
                    }
                } while (!salir);
            }
        }// end btActionSSQEditComment_Click


        /* Descripción:
         *  Devuelve un entero con el tamaño del universo o lanza una excepción de tipo FormatException
         *  que debe tratar el método que lo llame.
         */
        public static int readSizeOfUniverse(string s)
        {
            int retVal = 0;
            s = s.ToUpper();
            if (s.Equals(Facet.INFINITE))
            {
                retVal = int.MaxValue;
            }
            else
            {
                retVal = int.Parse(s);
            }
            return retVal;
        }


        /* Descipción:
         *  Exporta la lista de suma de cuadrados en un fichero de texto.
         */
        private void tsmiAction_SSq_ExportSquares_Click(Analysis_and_G_Study tAnalysis_tG_Study_Opt)
        {
            if (tAnalysis_tG_Study_Opt == null)
            {// (* 1 *)
                ShowMessageErrorOK(errorNoSSQ);
            }
            else
            {
                TableAnalysisOfVariance tableAnalysis = tAnalysis_tG_Study_Opt.TableAnalysisVariance();
                if (tableAnalysis == null)
                {// (* 2 *)
                    ShowMessageErrorOK(errorNoSSQ);
                    //MessageBox.Show(errorNoSSQ);
                }
                else
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();

                    if (Directory.Exists(this.cfgApli.Get_Path_Workspace()))
                    {
                        saveDialog.InitialDirectory = this.cfgApli.Get_Path_Workspace();
                    }

                    saveDialog.DefaultExt = DEFAULT_EXT_SCORE;
                    // filter = "Fichero de puntuaciones" + " (*.dat)|*.dat|" + "Fichero de sumas de cuadrados EduG" + " (*.edug)|*.edug|" + "Todos los archivos" + " (*.*)|*.*"
                    string filter = (filterDatas + FILTER_DATA + filterSsqExportEduG + FILTER_SSQ_EDUG + this.allFiles + FILTER_ALL_FILE);
                    saveDialog.Filter = filter;
                    saveDialog.OverwritePrompt = true; // muestra advertencia si el fichero ya existe
                    saveDialog.AddExtension = true;
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        bool res = false;
                        string pathFile = saveDialog.FileName;
                        string extFile = pathFile.Substring(pathFile.LastIndexOf('.')+1);
                        switch (extFile)
                        {
                            case(DEFAULT_EXT_SCORE):
                                res = tableAnalysis.WritingFileDataSumOfSquares(pathFile);
                                break;
                            case (DEFAULT_EXT_SSQ_EDUG):
                                res = AnalysisSsqEduG.WritingFileExportEduG_Ssq(tableAnalysis, pathFile);
                                break;
                            default:
                                res = tableAnalysis.WritingFileDataSumOfSquares(pathFile);
                                break;
                        }


                        if (res)
                        {
                            // Mostramos un mensaje de que las sumas de cuadrados se han guardado
                            ShowMessageInfo(txtSaveSumOfSquares, titleSaved);
                            // MessageBox.Show(txtSaveSumOfSquares, titleSaved);
                        }
                        else
                        {
                            // Mostramos un mensaje ERROR, en el que las puntuaciones NO se han guardado
                            ShowMessageInfo(txtNoSaveSumOfSquares, titleMessageError1);
                            // MessageBox.Show(txtNoSaveSumOfSquares, titleMessageError1);
                        }
                    }
                }// end if (* 2 *)
            }// enf if (* 1 *)
            
        }// end tsmiActions_SSq_ExportSquares_Click


        #region Métodos referentes a la edición de descripción de facetas desde la opción de suma de cuadrados
        /* Descripción:
         *  Se ejecuta al pulsar sobre el botón "Editar descripción". Permite modificar la descripción de
         *  las facetas desde la opción de suma de cuadrados.
         */
        private void tsmiActionEditFacetDescription_Click()
        {
            this.editionModeOn = true; // Ponemos el modo edición a true
            this.mStripSSQ.Enabled = false; // Inhabilitamos el menú vertical de suma de cuadrados

            Analysis_and_G_Study ssqEditDescriptionFacet = this.sagtElements.GetAnalysis_and_G_Study();

            // Ocultamos las pestañas
            foreach (TabPage tabPage in this.tabControlSSQ.TabPages)
            {
                tabPage.Parent = null;
            }

            // this.anl_tAnalysis_G_study_opt_Old = this.anl_tAnalysis_G_study_opt; // Guardamos la tabla de análisis actual por si deshacemos los cambios

            //Cargamos los datos de las facetas en el dataGrid
            CleanerDataGridViewExFacets(dgvExSSQ_EditDescriptionFacets); // limpiamos el datagrid de facetas
            ListFacets lf = ssqEditDescriptionFacet.GetListFacets(); // retomamos la lista de facetas
            LoadListFacetInDataGridView(lf, dgvExSSQ_EditDescriptionFacets, false, true); // cargamos la lista de facetas en el datagrid
            // Permitimos la edición de las columnas
            int nCol = dgvExSSQ_EditDescriptionFacets.ColumnCount;
            dgvExSSQ_EditDescriptionFacets.Columns["col_Description"].ReadOnly = false; // columna de descripción

            // Mostramos solo la pestaña de facetas
            this.tabPageEditDescriptionFacets.Parent = this.tabControlSSQ;
        }// end tsmiActionEditFacetDescription_Click



        /* Descripción:
         *  Se activa al pulsar sobre el botón Aceptar de la pestaña editar descripción de facetas.
         */
        private void btActionEditDescriptionFacetsAcept_Click()
        {
            Analysis_and_G_Study ssqEditDescriptionFacet = this.sagtElements.GetAnalysis_and_G_Study();
            string nameFileDataCreation = ssqEditDescriptionFacet.GetNameFileDataCreation();
            DateTime dateCreation = ssqEditDescriptionFacet.GetDateTime();

            try
            {
                // Leer las facetas;
                ListFacets newLf = dgvExToListFacets(dgvExSSQ_EditDescriptionFacets);

                // Actualizar la lista actual con los valores de la nueva
                ListFacets oldLf = ssqEditDescriptionFacet.TableAnalysisVariance().ListFacets();
                newLf = oldLf.RemplaceListFacets(newLf);

                // generar la tabla de análisis partiendo de la suma de cuadrados anterior
                // actualizar los valores de optimización
                ssqEditDescriptionFacet = ssqEditDescriptionFacet.ReplaceListOfFacet(newLf);
                ssqEditDescriptionFacet.SetNameFileDataCreation(nameFileDataCreation);
                // this.anl_tAnalysis_G_study_opt.SetNameFileDataCreation(nameFileDataCreation);
                ssqEditDescriptionFacet.SetDateTime(dateCreation);
                //this.anl_tAnalysis_G_study_opt.SetDateTime(dateCreation);
                this.anl_tAnalysis_G_study_opt_Old = null;

                // Actualizamos el valor
                this.sagtElements.SetAnalysis_and_G_Study(ssqEditDescriptionFacet);
                // cargar los valores nuevos.
                LoadAllDataInDataGridViewEx_SSQOptions(ssqEditDescriptionFacet);

                // Restauramos las pestañas y salimos del modo edición
                disableEditDescriptionFacet();
            }
            catch (ListFacetsException)
            {
                // Mostramos un mensaje de error indicando que no puede haber facetas repetidas
                ShowMessageErrorOK(errorDuplicateNameFacet);
            }
            catch (Exception ex)
            {
                // Capturamos la excepción y mostramos el problema
                ShowMessageErrorOK("Error btActionEditDescriptionFacetsAcept_Click(): " + ex.Message);
            }
        }


        /* Descripción:
         *  Se activa al pulsar sobre el botón Cancelar de la pestaña editar descripción de facetas.
         */
        private void btActionEditDescriptionFacetsCancel_Click()
        {
            disableEditDescriptionFacet();
        }


        /* Descripción:
         *  Restaura el estado de los elementos necesarios tras la edición de las facetas de la Oción Ananlisis.
         */
        private void disableEditDescriptionFacet()
        {
            // Ocultamos la pestaña facetas y mostramos todas las demas
            this.tabPageEditDescriptionFacets.Parent = null;
            this.tabPageSSQ_TableComp.Parent = this.tabControlSSQ;
            this.tabPageG_Parameters.Parent = this.tabControlSSQ;
            this.tabPageOptimization.Parent = this.tabControlSSQ;
            this.tbPageSsqInfo.Parent = this.tabControlSSQ;
            this.editionModeOn = false; // hemos finalizado la edición de facetas
            this.mStripSSQ.Enabled = true; // habilitamos el uso del menu
        }
        #endregion Métodos referentes a la edición de descripción de facetas desde la opción de suma de cuadrados


        #region Cambio de idioma de los elementos del tabPageSSQ
        /*
         * Descripción:
         *  Traduce los elementos del TabPageSSQ.
         * Parámetros:
         *  TransLibrary.Language lang: idioma al que vamos a traducir los elementos.
         *  string nameFileTrans: Nombre del fichero que contiene las traducciones.
         */
        private void TranslationSSQElements(TransLibrary.Language lang, string nameFileTrans)
        {
            TransLibrary.ReadFileTrans dic = new TransLibrary.ReadFileTrans(nameFileTrans);
            string name = "";
            try
            {
                // traducimos las etiquetas de las pestañas
                // Traducimos el tabPage: Suma de cuadrados
                name = this.tabPageSSQ_TableComp.Name.ToString();
                this.tabPageSSQ_TableComp.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el tabPage: G-Parámetros
                name = this.tabPageG_Parameters.Name.ToString();
                this.tabPageG_Parameters.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el tabPage: Optimización
                name = this.tabPageOptimization.Name.ToString();
                this.tabPageOptimization.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el tabPage: Intormación
                name = this.tbPageSsqInfo.Name.ToString();
                this.tbPageSsqInfo.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la pestaña de edición de facetas
                name = this.tabPageEditDescriptionFacets.Name.ToString();
                this.tabPageEditDescriptionFacets.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Botones aceptar y cancelar de la pestaña edición de descripción de facetas
                name = this.btEditDescriptionFacetsAcept.Name.ToString();
                this.btEditDescriptionFacetsAcept.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.btEditDescriptionFacetsCancel.Name.ToString();
                this.btEditDescriptionFacetsCancel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos la ventan de gráficos   
                if (this.formShowCharts != null)
                {
                    this.formShowCharts.traslationElements(lang, Application.StartupPath + LANG_PATH + FormShowCharts.STRING_TEXT);
                }

                // Traducimos los menú contextuales de los dataGridViewEx
                TranslationTContextualMenu(this.dGridViewExSourceOfVar, dicMeans, lang);
                TranslationTContextualMenu(this.dataGridViewExSSQ, dicMeans, lang);
                TranslationTContextualMenu(this.dGridViewExG_Parameters, dicMeans, lang);
                TranslationTContextualMenu(this.dGridViewExFacetsOptimization, dicMeans, lang);
                TranslationTContextualMenu(this.dGridViewExOptimizationResum, dicMeans, lang);
                TranslationTContextualMenu(this.dgvExSSQ_EditDescriptionFacets, dicMeans, lang);

                // Etiqueta de diseño de medida
                name = lbMeasurementDesign.Name.ToString();
                lbMeasurementDesign.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                lbMeasuDesignGP.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                lbMesurDesign2.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                if (dGridViewExSourceOfVar.ColumnCount != 0)
                {
                    // Cambiamos el nombre de las columnas
                    dGridViewExSourceOfVar.Columns[0].HeaderText = nameColFacet; // Nombre de la columna Etiquetas (dependerá del idioma).
                    dGridViewExSourceOfVar.Columns[1].HeaderText = nameColLevel; // Nombre de la columna Niveles (dependerá del idioma).
                    dGridViewExSourceOfVar.Columns[2].HeaderText = nameColSizeOfUniverse; // Nombre de la columna Descripción (dependerá del idioma).
                    dGridViewExSourceOfVar.Columns[3].HeaderText = nameColComment; // Nombre de la columna Descripción
                }
                // Columnas del dataGridViewEx Suma de cuadrados
                name = "sourceOfVarString";
                sourceOfVarString = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "ssqString";
                ssqString = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "degreeOfFreedomString";
                degreeOfFreedomString = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "msqString";
                msqString = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "randomCompString";
                randomCompString = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "mixCompString";
                mixCompString = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "correctedComp";
                correctedComp = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "standardErrorString";
                standardErrorString = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // actuamos sobre el dataGridViewEx
                if (dataGridViewExSSQ.ColumnCount != 0)
                {
                    // Cambiamos el nombre de las columnas
                    dataGridViewExSSQ.Columns[IND_SOURCE_OF_VAR].HeaderText = sourceOfVarString; // Nombre de la columna "Fuentes de variación".
                    dataGridViewExSSQ.Columns[IND_SSQ].HeaderText = ssqString; // Nombre de la columna Niveles (dependerá del idioma).
                    dataGridViewExSSQ.Columns[IND_DEGREE_OF_FREEDOM].HeaderText = degreeOfFreedomString; // Nombre de la columna Descripción (dependerá del idioma).
                    dataGridViewExSSQ.Columns[IND_MSQ].HeaderText = msqString;
                    dataGridViewExSSQ.Columns[IND_RANDOM_COMP].HeaderText = randomCompString;
                    dataGridViewExSSQ.Columns[IND_MIX_COMP].HeaderText = mixCompString;
                    dataGridViewExSSQ.Columns[IND_CORRECTED_COMP].HeaderText = correctedComp;
                    dataGridViewExSSQ.Columns[IND_STANDARD_ERROR].HeaderText = standardErrorString;
                }

                // Cabecera de las columnas del dataGridViewEx G-Parametros
                name = "source";
                source = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "diff_var";
                diff_var = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "rel_err_var";
                rel_err_var = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "abs_err_var";
                abs_err_var = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // actuamos sobre el dGridViewExG_Parameters
                if (dGridViewExG_Parameters.ColumnCount != 0)
                {
                    dGridViewExG_Parameters.Columns[0].HeaderText = source;
                    dGridViewExG_Parameters.Columns[1].HeaderText = diff_var;
                    dGridViewExG_Parameters.Columns[2].HeaderText = source;
                    dGridViewExG_Parameters.Columns[3].HeaderText = rel_err_var;
                    dGridViewExG_Parameters.Columns[4].HeaderText = abs_err_var;
                }

                // Cabecera de las columnas de dGridViewExFacetsOptimization
                name = "levelsProcess";
                levelsProcess = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Actuamos sobre dGridViewExFacetsOptimization
                if (dGridViewExFacetsOptimization.ColumnCount != 0)
                {
                    dGridViewExFacetsOptimization.Columns[IND_NAME].HeaderText = nameColFacet; // Nombre de la columna Etiquetas (dependerá del idioma).
                    dGridViewExFacetsOptimization.Columns[IND_LEVEL].HeaderText = nameColLevel; // Nombre de la columna Niveles (dependerá del idioma).
                    dGridViewExFacetsOptimization.Columns[IND_LEVELS_PROCESS].HeaderText = levelsProcess;
                    dGridViewExFacetsOptimization.Columns[IND_SIZE_OF_UNIVERSE].HeaderText = this.nameColSizeOfUniverse;
                }

                // Cabecera de las columnas y etiquetas de dGridViewExOptimizationResum
                name = "name_resum";
                name_resum = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "resum";
                resum = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "total_nb_obs";
                total_nb_obs = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "relat_measmt";
                relat_measmt = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "absol_measmt";
                absol_measmt = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "relat_err_var";
                relat_err_var = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "absol_err_var";
                absol_err_var = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "stand_relat_err";
                stand_relat_err = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "stand_absol_err";
                stand_absol_err = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Actuamos sobre el dGridViewExOptimizationResum
                if (dGridViewExOptimizationResum.ColumnCount != 0)
                {
                    // dGridViewExOptimizationResum.Columns[0].HeaderText = name_resum;
                    // Entonces pintamos la tabla de resumen de nuevo
                    Analysis_and_G_Study tAnalysis_tG_Study_Opt = this.sagtElements.GetAnalysis_and_G_Study();
                    LoadDataGridViewExOptimizationResum(tAnalysis_tG_Study_Opt, this.dGridViewExOptimizationResum);
                }

                // Traducimos las etiquetas de texto de los tabPage
                name = lbTextTotalSSQ.Name.ToString();
                lbTextTotalSSQ.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbTotalTargetVar.Name.ToString();
                lbTotalTargetVar.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbTotalRelErr.Name.ToString();
                lbTotalRelErr.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbTotalAbsErr.Name.ToString();
                lbTotalAbsErr.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbTextStandDev.Name.ToString();
                lbTextStandDev.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbTextRelSE.Name.ToString();
                lbTextRelSE.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbTextAbsoluteSE.Name.ToString();
                lbTextAbsoluteSE.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbTextGeneralizabilityCoef.Name.ToString();
                lbTextGeneralizabilityCoef.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbTextCoef_G_Rel.Name.ToString();
                lbTextCoef_G_Rel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbTextGeneralizabilityCoef.Name.ToString();
                lbTextGeneralizabilityCoef.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbTextCoef_G_Rel.Name.ToString();
                lbTextCoef_G_Rel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                
                // Etiquetas del tabPage: Información
                name = lbNameFileSsqInfo.Name.ToString();
                lbNameFileSsqInfo.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbDateFileSsqInfo.Name.ToString();
                lbDateFileSsqInfo.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Boton Editar del tabPage: Información
                name = btSsqEditComment.Name.ToString();
                btSsqEditComment.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " " + errorMessageTraslation + " " + name);
            }
        } // private void TraslationSSQElements
        #endregion Cambio de idioma de los elementos del tabPageDat

    } // end public partial class FormPrincipal : Form
} // end namespace GUI_TG