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
using AuxMathCalcGT;
using ImportEduGSsq;
using MultiFacetData;
using ProjectSSQ;
using Sagt;
using SsqPY;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing; // se usa para las propiedades de la cabecera de columna (color,fuente,etc)
using System.IO;
using System.Windows.Forms;


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
        private string rel_err_var = "Varianza del error relativo";
        private string percent_rel_err = "% relativo";
        private string abs_err_var = "Varianza del error absoluto";
        private string percent_abs_err = "% absoluto";

        // Cabeceras de las columnas pertenecientes al tabPage de Optimization
        // ===================================================================
        // private string sizeOfUniverse = "Tamaño del universo";
        // indice de la posición en la tabla de optimización de la columna Tamaño del universo
        private int IND_SIZE_OF_UNIVERSE = 2;
        // indice de la posición en la tabla de optimización de la columna Descripción
        private int IND_SSQQDESC = 3;

        // String para la columna de resumen de datos
        //=============================================
        private string total_nb_obs = "Total de observaciones";
        private string relat_measmt = "Coeficiente G relativo";
        private string absol_measmt = "Coeficiente G absoluto";
        private string relat_err_var = "Error relativo";
        private string absol_err_var = "Error absoluto";
        private string stand_relat_err = "Error estándar de medición relativo";
        private string stand_absol_err = "Error estándar de medición absoluto";

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
            }
            else
            {// (* 1 *)
                // Recuperamos las casillas de omision de facetas
                ReadColumnOmit(this.sagtElements, this.dataGridViewExFacets);
                ListFacets actual_lf = this.sagtElements.GetMultiFacetsObs().ListFacets();
                ListFacets withoutOmit_lf = actual_lf.ListFacetsWithoutOmit();


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
                    if (actual_lf.Count() != withoutOmit_lf.Count())    // Si hay facetas (y posiblemente niveles) omitidos, nos ocupamos de ambas cosas
                    {
                        lf = withoutOmit_lf; // asignación de las facetas no omitidas
                        multiFacets = multiFacets.OmitFacetInDataTable();
                    }
                    else if (lf.HasSkipLevels())    // Si hay solo niveles omitidos, nos ocupamos de solo eso
                    {
                        multiFacets = multiFacets.SkipIndexLevelFacetInDataTable();
                    }

                    // Arreglamos índices después de la omisión de niveles
                    multiFacets = multiFacets.RestoreIndexLevelFacetInDataTable();

                    // Primero debemos preguntarle al usuario por el diseño de medida.
                    ListFacets sourceOfDifferentiation = new ListFacets();
                    ListFacets sourceOfInstrumentation = new ListFacets();
                    FormMeasurDesign formMeasurDesign = new FormMeasurDesign(sourceOfDifferentiation, sourceOfInstrumentation, multiFacets.ListFacets(), cfgApli.GetConfigLanguage(), showVCA: true);

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
                                    ShowMessageErrorOK(errorM_DesignNoValidate);
                                }
                                else
                                {
                                    FormWaiting fw = ShowLoadingScreen(msgLoading);

                                    // Mostramos el diseño de medida en el textBox de los tabPage de suma de cuadrados
                                    // ShowMeDessingInTextBoxs(sourceOfDifferentiation, sourceOfInstrumentation);

                                    TableAnalysisOfVariance tableAnalysis = null;

                                    if (formMeasurDesign.UseVCA)
                                    {
                                        // Análisis de varianza mediante la librería VCA de R
                                        try
                                        {
                                            tableAnalysis = RRunner.RunVcaAnova(multiFacets);
                                        }
                                        catch (Exception ex)
                                        {
                                            CloseLoadingScreen(fw);
                                            ShowMessageErrorOK(txtCvaError + "\n\n" + ex.Message);
                                        }
                                    }
                                    else
                                    {
                                        bool zero = this.cfgApli.GetNull_to_Zero();
                                        tableAnalysis = new TableAnalysisOfVariance(multiFacets, zero);
                                    }

                                    if (tableAnalysis != null)
                                    {
                                        salir = true;

                                        TableG_Study_Percent tableG_Study = new TableG_Study_Percent(sourceOfDifferentiation, sourceOfInstrumentation, tableAnalysis);

                                        // Inicializamos la lista de G_Parámetros de Optimización
                                        List<G_ParametersOptimization> listG_ParametersOpt = new List<G_ParametersOptimization>();

                                        sagtElements.SetAnalysis_and_G_Study(new Analysis_and_G_Study(tableAnalysis, tableG_Study, listG_ParametersOpt));

                                        // Guardamos los datos referentes a la creación de la suma de cuadrados
                                        string nameFile = multiFacets.NameFileObs();
                                        sagtElements.GetAnalysis_and_G_Study().SetNameFileDataCreation(nameFile);

                                        DateTime date = DateTime.Now;
                                        sagtElements.GetAnalysis_and_G_Study().SetDateTime(date);

                                        // Mostramos todos los datos en los dataGridView
                                        LoadAllDataGridWithDataAnalysis(sagtElements.GetAnalysis_and_G_Study(), nameFile);

                                        // mostramos el tabPage de suma de cuadrados
                                        ExcludeTabPages();
                                        this.tabPageAnalysis.Parent = this.tabControlOptions;
                                        // Restauramos los colores
                                        this.RestoreColorMenu(this.mStripMain);
                                        // Asignamos el nuevo color
                                        this.tsmiAnalysis.BackColor = System.Drawing.SystemColors.Highlight;

                                        CloseLoadingScreen(fw);
                                    }
                                }
                                break;
                        }// end switch
                    } while (!salir);
                }
            }// end if  (* 1 *)
        }//private void EstimationPlan()


        /*
         * Descripción:
         *  Muestra los datos de un TableAnalysisOfVariance en el dataGridView del tabPageSSQ
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
                my_Row[IND_MIX_COMP] = ConvertNum.DecimalToString(tbAnalysisVar.MixedComp(key), numOfDecimal, puntoDecimal);
                my_Row[IND_CORRECTED_COMP] = ConvertNum.DecimalToString(tbAnalysisVar.CorrectedComp(key), numOfDecimal, puntoDecimal);
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

            AuxLoadDataGridViewOptimizationResum(dgvEx, this.total_nb_obs, tbAnalisisVar.ListFacets().MultOfLevels());
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
        private void AddColunmToDGVOptimization(ListFacets newListFacets, G_ParametersOptimization newG_Parameters
            , DataGridViewEx.DataGridViewEx dgvExOptimization)
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
            dgvExOptimization.Rows[numFacets].Cells[num_col_pos].Value = lf.MultOfLevels();
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
         *  out bool cleanRequested: se pone a true si el usuario ha pulsado "Limpiar" en lugar de "Aceptar",
         *      indicando que hay que vaciar la lista de niveles de optimización añadidos hasta ahora, en
         *      vez de añadir uno nuevo.
         */
        private G_ParametersOptimization AddSignificanceLevel(Analysis_and_G_Study tablesOfAnalysisG, out bool cleanRequested)
        {
            G_ParametersOptimization retVal = null; // valor de retorno
            cleanRequested = false;

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
                    case (DialogResult.Abort):
                        // Se ha pulsado "Limpiar": salimos indicando que hay que vaciar la lista de
                        // niveles de optimización, sin añadir ningún nuevo G_ParametersOptimization.
                        cleanRequested = true;
                        salir = true;
                        break;
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
                                    int size = Facet.readSizeOfUniverse(my_row.Cells[4].Value.ToString());
                                    int old_size = Facet.readSizeOfUniverse(my_row.Cells[2].Value.ToString());

                                    if (size > old_size)
                                    {
                                        correct = false;
                                        ShowMessageErrorOK(errorDUniverse);
                                    } 
                                    else if (level > size)
                                    {// (*2*)
                                        correct = false;
                                        ShowMessageErrorOK(errorOverUniverse);
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
                                    int newSize = Facet.readSizeOfUniverse(my_row.Cells[3].Value.ToString());
                                    int old_size = Facet.readSizeOfUniverse(my_row.Cells[2].Value.ToString());

                                    if (newSize > old_size)
                                    {
                                        correct = false;
                                        ShowMessageErrorOK(errorDUniverse);
                                    } 
                                    else if (level > newSize)
                                    {// (*3*)
                                        correct = false;
                                        ShowMessageErrorOK(errorOverUniverse);
                                    }//end if (*3*)
                                }
                            }// end for

                        }
                        catch (FormatException)
                        {
                            // Se produjo la excepción al obtener el nivel de la faceta
                            ShowMessageErrorOK(errorLevelFormat);
                            correct = false;
                        }

                        if (correct)
                        {// (*4*)
                            try
                            {
                                // lista de facetas que contedra tanto las facetas de instrumentación como de diferenciación.

                                ListFacets newlf = tablesOfAnalysisG.TableAnalysisVariance().ListFacets().DeepClone();

                                /* Modificamos los valores de las facetas de intrumentación en la lista clonada.
                                 */
                                ListFacets newLevelListInstFacets = new ListFacets();
                                int numCol = dgvExAddInstLevelSign.Columns.Count - 1;
                                for (int i = 0; i < numInstFacets; i++)
                                {
                                    string name = dgvExAddInstLevelSign.Rows[i].Cells[0].Value.ToString();
                                    // el nuevo nivel se obtiene de la tabla
                                    int newlevel = int.Parse(dgvExAddInstLevelSign.Rows[i].Cells[numCol - 1].Value.ToString());
                                    int newSizeUni = Facet.readSizeOfUniverse(dgvExAddInstLevelSign.Rows[i].Cells[numCol].Value.ToString());

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

                                    int newSizeUni = Facet.readSizeOfUniverse(dgvExAddDiffLevelSign.Rows[i].Cells[numCol].Value.ToString());

                                    Facet auxF = newlf.LookingFacet(name);

                                    auxF.SizeOfUniverse(newSizeUni);
                                    newLevelListDiffFacets.Add(auxF);
                                }

                                retVal = tablesOfAnalysisG.D_Study(newlf, newLevelListDiffFacets, newLevelListInstFacets);


                                /*===============================================================================================*/

                                salir = true;
                            }
                            catch (FormatException)
                            {
                                // Se produjo la excepción al obtener el nivel de la faceta
                                ShowMessageErrorOK(errorLevelFormat);
                                correct = false;
                            }
                            catch (FacetException)
                            {
                                // Se produjo la excepción al obtener el nivel de la faceta
                                ShowMessageErrorOK(errorLevelFormat);
                                correct = false;
                            }

                        }// end if (*4*)
                        break;
                }//end switch (*1*)
            } while (!salir);// (*1*)

            return retVal;

        }// end AddSignificanceLevel


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
         *  Se ejecuta al seleccióna la opción "Gráfico Coef. G Abs" del menú de acciónes de suma de cuadrados.
         *  Muestra una gráfica de representación lineal.
         */
        private void tsmiActionChartCoefGAbs_Click(Analysis_and_G_Study tAnalysis_tG_Study_Opt)
        {
            ShowChartCoefG(tAnalysis_tG_Study_Opt, true, this.tsmiAnalysisChartCoefGAbs.Text);
        }

        /* Descripción:
         *  Se ejecuta al seleccióna la opción "Gráfico Coef. G Rel" del menú de acciónes de suma de cuadrados.
         *  Muestra una gráfica de representación lineal.
         */
        private void tsmiActionChartCoefGRel_Click(Analysis_and_G_Study tAnalysis_tG_Study_Opt)
        {
            ShowChartCoefG(tAnalysis_tG_Study_Opt, false, this.tsmiAnalysisChartCoefGRel.Text);
        }

        private void ShowChartCoefG(Analysis_and_G_Study tAnalysis_tG_Study_Opt, bool isAbs, string text)
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
                                if (beginning > 0 && beginning < ending && beginning + increment <= ending)
                                {
                                    ListFacets lfSeleted = FacetsSelectedIn_cListBox(lf, checkedLtBox);

                                    bool allUniversesBelow = true;
                                    foreach (Facet f in lfSeleted)
                                    {
                                        if (ending <= f.SizeOfUniverse())
                                        {
                                            allUniversesBelow = false;
                                        }
                                    }
                                    if(allUniversesBelow)
                                    {
                                        ShowMessageErrorOK(errorDUniverse);
                                        break;
                                    }
                                    
                                    salir = true;
                                    try
                                    {
                                        FormShowCharts2 formShowCharts2 =
                                            new FormShowCharts2(cfgApli, tAnalysis_tG_Study_Opt, lfSeleted,
                                                isAbs, text, beginning, ending, increment);
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
        }


        /* Descripción:
         *  Cuando la ventana se cierra se encarga de volver a porner la variable formShowCharts a null.
         */
        public void FormShowChartsClosed()
        {
            this.formShowCharts = null;
        }


        /* Descripción:
         *  Importa un fichero de suma de cuadrados .ssq del programa GT E 2.0 para construir el objeto.
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
         *  Importa un fichero de resultado de suma de cuadrados .rsa del programa GT E 2.0 para construir el objeto.
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
        }


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
            }
            catch (ImportEduGSsq.AnalysisSsqEduG_Exception)
            {
                // Se producjo un error al leer el archivo
                ShowMessageErrorOK(errorFormatFile);
                return null;
            }
        }// end Aux_SelectAnalysisOfListAnalyisReports

        private Analysis_and_G_Study Aux_loadListTableSSqOfFileXls(string path)
        {
            try
            {
                TransLibrary.ReadFileTrans dic = new TransLibrary.ReadFileTrans(Application.StartupPath + LANG_PATH + DATA_STRINGS);
                TransLibrary.WordTranslation transFacets = dic.labelTraslation(tabPagMultiFacet.Name.ToString());
                dic = new TransLibrary.ReadFileTrans(Application.StartupPath + LANG_PATH + ANALYSIS_STRINGS);
                TransLibrary.WordTranslation transSSq = dic.labelTraslation(tabPageAnalysisSSQ.Name.ToString());
                TransLibrary.WordTranslation transG_p = dic.labelTraslation(tabPageAnalysisG_P.Name.ToString());
                TransLibrary.WordTranslation transResum = dic.labelTraslation(tabPageAnalysisOpt.Name.ToString());

                return ImportExcel.ImportFileXLS_to_AAGS(path, transFacets, transSSq, transG_p, transResum);
            }
            catch (Analysis_and_G_Study_Exception)   //todo
            {
                ShowMessageErrorOK(errorFormatFile);
                return null;    //Note: functions that call this function should take this return null into account to avoid deleting data
            }
        }


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
                        string extFile = pathFile.Substring(pathFile.LastIndexOf('.') + 1);
                        switch (extFile)
                        {
                            case (DEFAULT_EXT_SCORE):
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
                        }
                        else
                        {
                            // Mostramos un mensaje ERROR, en el que las puntuaciones NO se han guardado
                            ShowMessageInfo(txtNoSaveSumOfSquares, titleMessageError1);
                        }
                    }
                }// end if (* 2 *)
            }// enf if (* 1 *)

        }// end tsmiActions_SSq_ExportSquares_Click


        #region Cambio de idioma de los elementos del tabPageSSQ
        /*
         * Descripción:
         *  Traduce los elementos del TabPageSSQ.
         * Parámetros:
         *  TransLibrary.Language lang: idioma al que vamos a traducir los elementos.
         *  string nameFileTrans: Nombre del fichero que contiene las traducciones.
         * NOTE: Some of the variables here may be currently unused
         */
        private void TranslationSSQElements(TransLibrary.Language lang, string nameFileTrans)
        {
            TransLibrary.ReadFileTrans dic = new TransLibrary.ReadFileTrans(nameFileTrans);
            string name = "";
            try
            {
                // Traducimos la ventan de gráficos   
                if (this.formShowCharts != null)
                {
                    this.formShowCharts.traslationElements(lang, Application.StartupPath + LANG_PATH + FormShowCharts.STRING_TEXT);
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

                // Cabecera de las columnas del dataGridViewEx G-Parametros
                name = "source";
                source = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "diff_var";
                diff_var = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "rel_err_var";
                rel_err_var = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "percent_rel_err";
                percent_rel_err = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "abs_err_var";
                abs_err_var = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "percent_abs_err";
                percent_abs_err = dic.labelTraslation(name).GetTranslation(lang).ToString();

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
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " " + errorMessageTraslation + " " + name);
            }
        } // private void TraslationSSQElements
        #endregion Cambio de idioma de los elementos del tabPageDat

    } // end public partial class FormPrincipal : Form
} // end namespace GUI_TG