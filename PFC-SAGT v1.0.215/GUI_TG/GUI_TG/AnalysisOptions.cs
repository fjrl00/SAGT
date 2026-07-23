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
 *      Clase parcial ("partial") del FormPrincipal. Contiene los métodos referentes a la parte de
 *      Análisis de varianza del plan y estimación de los componentes de varianza.
 */
using AuxMathCalcGT;
using ImportEduGSsq;
using MultiFacetData;
using ProjectSSQ;
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
        /**********************************************************************************************
         * VARIABLES
         **********************************************************************************************/
        private ListFacets analysisSourceOfVarDiff; //fuentes de variación diferenciación
        private ListFacets analysisSourceOfVarInst; // fuentes de Varicion instrumentación

        private ListFacets listFacetsAnalysis = null; // lista de facetas que se emplea para el analisis
        private List<string> llFacetsAnalysis; // Lista


        /*
         * Descripción:
         *  Pone los campos de texto vacios para aquellos label que deben mostrarse inicialmente vacios.
         */
        private void ClearListBoxAnalysis()
        {
            // Label Suma de cuadrados 
            lbAnalysisTotalSsq.Text = "";
            lbAnalysisTotalDF.Text = "";
            // Label de G-Parámetros
            lbAnalysisTotal_Target.Text = "";
            lbAnalysisTotal_Error_Rel.Text = "";
            lbAnalysisTotal_Error_Abs.Text = "";
            lbAnalysisStandDev.Text = "";
            lbAnalysisRelativeSE.Text = "";
            lbAnalysisAbsoluteSE.Text = "";
            lbAnalysisCoef_G_Rel.Text = "";
            lbAnalysisCoef_G_Abs.Text = "";

            // Ocultamos el tabPage de edición de Facetas y suma de cuadrados
            this.tabPageAnalysisFacetas.Parent = null;
            this.tabPageAnalysisEditingSSq.Parent = null;
        }


        /* Descripción:
         *  Pregunta el número de facetas y luego activa la edición de la tabla de facetas
         */
        private void tsmiActionNewFileAnalysisSSQ_Click(object sender, EventArgs e)
        {
            // First: confirmation screen triggers in case there's analysis data loaded. If the user cancels, we stop the method execution and do not lose the data.
            if (sagtElements.GetAnalysis_and_G_Study() != null)
            {
                if (sagtElements.GetAnalysis_and_G_Study().TableAnalysisVariance() != null)
                {
                    DialogResult res = ShowMessageDialog(titleConfirm, txtConfirmClose);

                    if (res != DialogResult.OK)
                    {
                        return; // user cancelled → stop method execution completely
                    }
                }
            }

            // We start out by closing everything and removing all data in Analysis
            cleanerAllTabPageAnalysis();

            // Deshabilitamos el menú principal poniendo la variable booleana disableTopLeftButtons a true.
            this.disableTopLeftButtons = true;
            // desactivamos el menú de acciones de análisis
            this.mStripAnalysis.Enabled = false;
            // número de facetas 
            int t = 0;
            FormAssignNumOfFacets fAssignNumFacets = new FormAssignNumOfFacets(this.dicMessage, this.LanguageActually());

            bool salir = false;
            do
            {
                DialogResult res = fAssignNumFacets.ShowDialog();
                switch (res)
                {
                    case DialogResult.Cancel:
                        salir = true;
                        this.CancelAcciónAnalisysEditionOfFacet();
                        break;
                    case DialogResult.OK:
                        if (String.IsNullOrEmpty(fAssignNumFacets.TextBoxNumOfFacets()))
                        {
                            // Si el textBox esta vació avisamos del error
                            ShowMessageErrorOK(errorNoNumFacet, this.titleMessageError1, MessageBoxIcon.Stop);
                        }
                        else
                        {
                            int numFacet = int.Parse(fAssignNumFacets.TextBoxNumOfFacets());
                            if (numFacet < 2)
                            {
                                ShowMessageErrorOK(errorMinNumFacet, this.titleMessageError1, MessageBoxIcon.Stop);
                            }
                            else
                            {
                                t = numFacet;
                                salir = true;
                                // Asignamos la disposición de las facetas
                                provision = fAssignNumFacets.CheckGroupBoxProvisionOfFacets();
                            }
                        }
                        break;
                }// end switch
            } while (!salir);

            // si tenemos más de una faceta entonces pasamos a editarlas
            if (t >= 2)
            {
                CleanerDataGridViewExFacets(this.dGridViewExAnalysis_TableFacet);
                this.dGridViewExAnalysis_TableFacet.NumeroFilas = t;
                enableEditingFacetAnalysis();
                // Mostramos los botones
                enableAnalysisButtonsFacets(provision);
            }
        }// tsmiActionNewFileAnalysisSSQ_Click


        /* Descripción:
         *  Se ejecuta cuando se Cancela la operación de edición de facetas, que puede ser la
         *  etapa previa a la edición de suma de cuadrados de la tabla de análisis.
         */
        private void CancelAcciónAnalisysEditionOfFacet()
        {
            this.disableTopLeftButtons = false; // hemos finalizado la edición de facetas
            this.mStripAnalysis.Enabled = true; // habilitamos el uso del menu
        }


        /*
         * Descripción:
         *  Habilita los botones "Anidar faceta" y "Quitar anidamiento" del tabPageAnalysisFacetas.
         *  en el caso de que la variable que se pasa como parámetro coincida con Mixed. Si no
         *  coincide los inhabilita.
         * Parámetros:
         *  ProvisionOfFacets provision: Indica el tipo de diposición de las facetas. 
         */
        private void enableAnalysisButtonsFacets(ProvisionOfFacets provision)
        {
            // El botón generar tabla de análisis estará visible
            btEditSumOfSquaresOnAnalisys.Enabled = true;
            btEditSumOfSquaresOnAnalisys.Visible = true;

            // si la diposición de facetas no es mixta no mostramos el botón de generar anidamientos
            if (provision.Equals(ProvisionOfFacets.Mixed))
            {
                // habilitamos los botones de anidar facetas
                btAnalysis_NestingFacet.Enabled = true;
                btAnalysis_NestingFacet.Visible = true;
                btAnalysis_RemoveNesting.Enabled = true;
                btAnalysis_RemoveNesting.Visible = true;
            }
            else
            {
                btAnalysis_NestingFacet.Enabled = false;
                btAnalysis_NestingFacet.Visible = false;
                btAnalysis_RemoveNesting.Enabled = false;
                btAnalysis_RemoveNesting.Visible = false;
            }
        }


        /* Descripción:
         * Este método oculta el botón generar tabla de observaciones muestra el botón de aceptar
         */
        private void enableAnalysisButtonsEditFacets()
        {
            // Ocultamos los botones para anidar facetas y quitar anidamientos
            btAnalysis_NestingFacet.Enabled = false;
            btAnalysis_NestingFacet.Visible = false;
            btAnalysis_RemoveNesting.Enabled = false;
            btAnalysis_RemoveNesting.Visible = false;
            // Botón generar tabla de observaciones oculto
            btEditSumOfSquaresOnAnalisys.Enabled = false;
            btEditSumOfSquaresOnAnalisys.Visible = false;
        }


        /* Descripción:
         *  Habilita, muestra y oculta los elementos necesarios para la edición de las facetas de la Opción Análisis.
         *  No actua sobre los botones solo sobre los tabPages y los menús.
         */
        private void enableEditingFacetAnalysis()
        {
            // Deshabilitamos el menú principal poniendo la variable booleana disableTopLeftButtons a true.
            this.disableTopLeftButtons = true;
            this.mStripAnalysis.Enabled = false;
            // Mostramos la pestaña de edición  de facetas
            this.tabPageAnalysisFacetas.Parent = this.tabControlAnalysisSSQ;
            // Ocultamos las otras cuatro pestañas
            this.tabPageAnalysisEditingSSq.Parent = null;
            this.tabPageAnalysisSSQ.Parent = null;
            this.tabPageAnalysisG_P.Parent = null;
            this.tabPageAnalysisOpt.Parent = null;
            this.tabPageAnalysisInf.Parent = null;

            // We ensure all cells are editable
            int nCol = dGridViewExAnalysis_TableFacet.ColumnCount;
            for (int i = 0; i < nCol; i++)
            {
                dGridViewExAnalysis_TableFacet.Columns[i].ReadOnly = false;
            }
        }


        /* Descripción:
         *  Muestra el tapPage de edicion de suma de cuadrados en la opción de Análisis
         */
        private void enableEditingSSqAnalysis()
        {
            // Deshabilitamos el menú principal poniendo la variable booleana disableTopLeftButtons a true.
            this.disableTopLeftButtons = true;
            this.mStripAnalysis.Enabled = false;
            this.tabPageAnalysisFacetas.Parent = null;
            this.tabPageAnalysisEditingSSq.Parent = this.tabControlAnalysisSSQ;
        }


        /* Descripción:
         *  Se ejecuta tras pulsar el botón de anidar faceta en la opción de análisis. Muestra
         *  la ventana donde seleccionaremos los anidamientos.
         */
        private void btActionAnalysis_NestingFacet_Click()
        {
            HandleAddNesting(dGridViewExAnalysis_TableFacet);
        }// end btActionAnalysis_NestingFacet_Click


        /* Descripción:
         *  Muestra la ventana para seleccionar aquellos diseños de facetas que queramos eliminar de 
         *  nuestro diseño.
         */
        private void btActionAnalysis_RemoveNesting_Click()
        {
            HandleRemoveNesting(dGridViewExAnalysis_TableFacet);
        }// end btActionAnalysis_RemoveNesting_Click


        /* Descripción:
         *  Lee los datatos de las facetas de la tabla de análisis y muestra la tabla de suma de 
         *  cuadrados para que estos puedan ser introducidos por el usuario.
         */
        private void btActionEditSumOfSquaresOnAnalisys_Click()
        {
            if (this.lf_nestings == null)     //Si no tenemos facetas anidadas mediante método mixto
                this.listFacetsAnalysis = dgvExToListFacets(this.dGridViewExAnalysis_TableFacet);   // Leemos de la tabla
            else                            //De lo contrario
                this.listFacetsAnalysis = this.lf_nestings;                                           // Cogemos lo que hemos ido rellenando ya

            // Si los datos son correctos continuamos las comprobaciones.
            if (this.listFacetsAnalysis != null)
            {
                // Si es necesario realizamos el anidamiento total de las facetas 
                if (this.provision.Equals(ProvisionOfFacets.Nested))
                {
                    // Hacemos un anidamiento de las facetas
                    this.listFacetsAnalysis.NestingAllFacet();
                }

                // Pedimos al usuario que inserte el diseño de medida
                this.analysisSourceOfVarDiff = new ListFacets();
                this.analysisSourceOfVarInst = new ListFacets();
                // Creamos la ventana para introducir el diseño de medida
                FormMeasurDesign formMeasurDesign = new FormMeasurDesign(analysisSourceOfVarDiff, analysisSourceOfVarInst, listFacetsAnalysis, cfgApli.GetConfigLanguage());
                bool salir = false; // variable de control del bucle

                do
                {
                    DialogResult res = formMeasurDesign.ShowDialog();
                    switch (res)
                    {
                        case (DialogResult.Cancel): salir = true; break;
                        case (DialogResult.OK):
                            if (formMeasurDesign.ListFacetDiff() == 0 || formMeasurDesign.ListFacetInst() == 0)
                            {
                                ShowMessageErrorOK(errorM_DesignNoValidate, "", MessageBoxIcon.Stop);
                            }
                            else
                            {
                                salir = true;
                                Aux_IntroduceSsqValues();

                            }
                            break;
                    }
                } while (!salir);
            }
            this.lf_nestings = null;
        }// end btActionEditSumOfSquaresOnAnalisys_Click


        /* Descripción:
         *  Prepara el dataGrid para que insertamos los valores de la suma de cuadrados tras introducir 
         *  las facetas y el diseño de medida.
         */
        private void Aux_IntroduceSsqValues()
        {
            // Mostramos el diseño de medida en los textBox
            ShowMeDesignInAnalysisTextBoxs(analysisSourceOfVarDiff, analysisSourceOfVarInst);
            /* Insertamos las facetas en la tabla de facetas de la pestaña de edición se 
             * suma de cuadrados */
            LoadListFacetInDataGridView(listFacetsAnalysis, dgvExAnalysis_FacetEditSsq);
            // Abilitamos el modo edición
            enableEditingSSqAnalysis();
            tabPageAnalysisEditingSSq.Text = tabPageAnalysisSSQ.Text; // Cambiar texto a "Suma de cuadrados"

            // Optenemos la lista de combinaciones sin repetición de la lista de facetas
            this.llFacetsAnalysis = listFacetsAnalysis.CombinationStringWithoutRepetition();

            // Set the column header style.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Aqua;
            columnHeaderStyle.Font = new Font("Verdana", 9, FontStyle.Bold);
            this.dgvExAnalysisEditSSq.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            this.dgvExAnalysisEditSSq.DefaultCellStyle.Font = fontCellTable;
            this.dgvExAnalysisEditSSq.ColumnHeadersVisible = true;

            // insertamos en la tabla de edición de suma de cuadrados
            this.dgvExAnalysisEditSSq.NumeroColumnas = 2;
            // Primera columna [0] (Fuentes de variación)
            this.PropertyColumnDGV(this.dgvExAnalysisEditSSq, IND_SOURCE_OF_VAR, 150, this.sourceOfVarString);
            this.dgvExAnalysisEditSSq.Columns[0].ReadOnly = true;
            this.dgvExAnalysisEditSSq.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;
            // Tercera columna [1] (suma de cuadrados)
            this.PropertyColumnDGV(this.dgvExAnalysisEditSSq, IND_SSQ, 200, this.ssqString);
            this.dgvExAnalysisEditSSq.Columns[1].ReadOnly = false;
            this.dgvExAnalysisEditSSq.ReadOnly = false;

            // Insertamos las filas de datos (fuente de variación suma de cuadrados)
            foreach (string sub_lf in llFacetsAnalysis)
            {
                object[] my_Row = new object[2];

                my_Row[0] = sub_lf;
                my_Row[1] = "";

                // insertamos la tupla
                this.dgvExAnalysisEditSSq.Rows.Add(my_Row);
            }
        }// end Aux_IntroduceSsqValues


        /* Descripción:
         *  Carga el dataGrid de edición de suma de cuadrados con los valores actuales.
         * Parámetros:
         *      ListFacets listFacetsAnalysis: La lista facetas que se mostrará en el dataGrid
         *      que se encuentra sobre la tabla de suma de cuadrados.
         */
        private void Aux_EditSsqValues(ListFacets listFacetsAnalysis)
        {
            // Introducimos los valores antiguos de la suma de cuadrados
            TableAnalysisOfVariance tb = sagtElements.GetAnalysis_and_G_Study().TableAnalysisVariance();
            List<string> lKeys = tb.SourcesOfVar();
            TableG_Study_Percent tableG = sagtElements.GetAnalysis_and_G_Study().TableG_Study_Percent();
            // Mostramos el diseño de medida en los textBox
            ShowMeDesignInAnalysisTextBoxs(tableG.LfDifferentiation(), tableG.LfInstrumentation());
            // Abilitamos el modo edición
            enableEditingSSqAnalysis();

            // limpiamos el dataGridViewEx de las facetas
            this.dgvExAnalysisEditSSq.NumeroFilas = 0;
            this.dgvExAnalysisEditSSq.Rows.Clear();

            // Optenemos la lista de combinaciones sin repetición de la lista de facetas
            // this.llFacetsAnalysis = listFacetsAnalysis.CombinationStringWithoutRepetition();

            // Set the column header style.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Aqua;
            columnHeaderStyle.Font = new Font("Verdana", 9, FontStyle.Bold);
            this.dgvExAnalysisEditSSq.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            this.dgvExAnalysisEditSSq.DefaultCellStyle.Font = fontCellTable;
            this.dgvExAnalysisEditSSq.ColumnHeadersVisible = true;

            // insertamos en la tabla de edición de suma de cuadrados
            this.dgvExAnalysisEditSSq.NumeroColumnas = 2;
            // Primera columna [0] (Fuentes de variación)
            this.PropertyColumnDGV(this.dgvExAnalysisEditSSq, IND_SOURCE_OF_VAR, 150, this.sourceOfVarString);
            this.dgvExAnalysisEditSSq.Columns[0].ReadOnly = true;
            this.dgvExAnalysisEditSSq.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;
            // Tercera columna [1] (suma de cuadrados)
            this.PropertyColumnDGV(this.dgvExAnalysisEditSSq, IND_SSQ, 200, this.ssqString);
            this.dgvExAnalysisEditSSq.Columns[1].ReadOnly = false;
            this.dgvExAnalysisEditSSq.ReadOnly = false;


            int numKeys = lKeys.Count;
            // Insertamos las filas de datos (fuente de variación suma de cuadrados)
            for (int i = 0; i < numKeys; i++)
            {
                object[] my_Row = new object[2];
                string key = lKeys[i];
                my_Row[0] = key;
                my_Row[1] = tb.SSQ(key); ;

                // insertamos la tupla
                this.dgvExAnalysisEditSSq.Rows.Add(my_Row);
            }
        }// end Aux_EditSsqValues


        /* Descripción:
         *  Comprueba que la suma de cuadrados se ha introducido correctamente. Si no esta correcto
         *  dara un aviso de error. Si se ha introducido correctamente preguntará donde guardarlo
         */
        private void btActionSaveAnalysisSsq_Click(object sender, EventArgs e)
        {
            /* Si estamos en el modo edición entonces la lista de fuentes serán la perteneciente a la tabla de análisis
             */
            if (this.disableTopLeftButtons && sagtElements.GetAnalysis_and_G_Study() != null)
            {
                listFacetsAnalysis = sagtElements.GetAnalysis_and_G_Study().TableAnalysisVariance().ListFacets();
                llFacetsAnalysis = sagtElements.GetAnalysis_and_G_Study().TableAnalysisVariance().SourcesOfVar();
            }

            Dictionary<string, double?> ssq = new Dictionary<string, double?>();
            bool correct = true;
            try
            {
                int numRow = this.dgvExAnalysisEditSSq.RowCount;
                for (int i = 0; i < numRow && correct; i++)
                {
                    DataGridViewRow my_Row = this.dgvExAnalysisEditSSq.Rows[i];
                    // el valor uno se corresponde con la columna que contiene la suma de los cuadrados
                    // si la celda se dejó vacía, la convertimos en 0
                    if(my_Row.Cells[1].Value == null || my_Row.Cells[1].Value.ToString() == "")
                    {
                        my_Row.Cells[1].Value = "0";
                    }
                    string s = my_Row.Cells[1].Value.ToString();
                    double d = (double)ConvertNum.String2Double(s);
                    ssq.Add(llFacetsAnalysis[i], d);
                }
            }
            catch (FormatException)
            {
                // Se ha produccido un error al leer la suma de cuadrados
                correct = false;
                ShowMessageErrorOK(errorNoValidateSSqEdit);
            }

            if (correct)
            {
                // Step 1: Assemble new analysis
                if (this.disableTopLeftButtons && sagtElements.GetAnalysis_and_G_Study() != null)
                {
                    // NOTE: Messy and unoptimized code
                    // Si estamos en el modo edición entonces primero actualizamos ssq
                    sagtElements.GetAnalysis_and_G_Study().UpdateSsq(ssq);
                    // Y después, guardamos los datos de la tabla de facetas
                    AnalysisUpdateFacets();
                }
                else
                {
                    // Creamos el objeto tabla de análisis varianza
                    TableAnalysisOfVariance tbAnalysisVar = new TableAnalysisOfVariance(listFacetsAnalysis, ssq);

                    // Creamos el objeto de tabla de G-Parámetros
                    TableG_Study_Percent gp = new TableG_Study_Percent(analysisSourceOfVarDiff, analysisSourceOfVarInst, tbAnalysisVar);

                    // Cargamos los datos en los textBox
                    ShowMeDesignInAnalysisTextBoxs(gp.LfDifferentiation(), gp.LfInstrumentation());

                    // Actualizamos la variable global de análisis con los nuevos valores
                    sagtElements.SetAnalysis_and_G_Study(new Analysis_and_G_Study(tbAnalysisVar, gp));
                }
                DateTime date = DateTime.Now;
                sagtElements.GetAnalysis_and_G_Study().SetDateTime(date);

                // Step 2: Save to file
                SaveFileButton(this.sagtElements);

                // Step 3: Update UI
                // Mostramos los datos
                LoadAllDataGridWithDataAnalysis(sagtElements.GetAnalysis_and_G_Study(), sagtElements.GetAnalysis_and_G_Study().GetNameFileDataCreation());
                
                // Ocultamos el tabPage de editar suma de cuadrados y mostramos los restantes
                disableEditingFacetAnalysis();
            }// end if
        }// end btActionSaveAnalysisSsq_Click



        /* Descripción:
         *  Muestra en los dataGrid y textBox de Análisis los datos del objeto que se pasa como parámetro.
         */
        private void LoadAllDataGridWithDataAnalysis(Analysis_and_G_Study anl_tAnalysis_G_study_opt, string nameFile)
        {
            TableAnalysisOfVariance tbAnalysisVar = anl_tAnalysis_G_study_opt.TableAnalysisVariance();
            ListFacets listFacetsAnalysis = tbAnalysisVar.ListFacets();
            LoadListFacetInDataGridView(listFacetsAnalysis, dgvExAnalysisSourceOfVarSsq);
            LoadListFacetInDataGridView(listFacetsAnalysis, dgvExAnalysisFacetsOpt);
            LoadSSQ_InDataGridView(tbAnalysisVar, this.dgvExAnalysisSSq);
            LoadAnalysisTotalSSQ_TableComp(tbAnalysisVar);

            LoadG_ParametersInDataGridView(anl_tAnalysis_G_study_opt, this.dgvExAnalysis_GP);
            TableG_Study_Percent gp = anl_tAnalysis_G_study_opt.TableG_Study_Percent();
            ListFacets list_diff = gp.LfDifferentiation();
            ListFacets list_inst = gp.LfInstrumentation();
            ShowMeDesignInAnalysisTextBoxs(list_diff, list_inst);
            LoadAnalysisTotalG_Parameters(gp);
            LoadDataGridViewExOptimizationResum(anl_tAnalysis_G_study_opt,
                this.dgvAnalysisResumOpt);
            // Mostramos los datos de el tabPage de información
            this.tbFileAnalysisProvede.Text = nameFile;
            this.tbDateAnalysisCreated.Text = anl_tAnalysis_G_study_opt.GetDateTime().ToString();
            this.rTextBoxAnalysisInfo.Text = anl_tAnalysis_G_study_opt.GetTextComment();
        }// LoadAllDataGridWithDataAnalysis


        /* Descipción:
         *  Se ejecuta cuando se pulsa sobre abrir en el menú vertical de Análisis. Muestra el cuadro
         *  de dialogo para seleccionar el archivo que se va a abrir.
         */
        private void tsmiActionOpenAnalysis_Click()
        {
            DialogResult res = DialogResult.OK;
            if (sagtElements.GetAnalysis_and_G_Study() != null)
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

                string fileFilter = (
                    this.sagtFiles + FILTER_SAGT_FILE +
                    "Legacy Analysis File" + FILTER_ANALYSIS_FILTER + "|" + 
                    this.allFiles + FILTER_ALL_FILE);
                openDialog.Filter = fileFilter;

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    switch (System.IO.Path.GetExtension(openDialog.FileName).ToLowerInvariant())
                    {
                        case ".anls":
                            LoadAnalysisFile(openDialog.FileName);
                            break;
                        default:
                            loadFileSagt(openDialog.FileName);
                            break;
                    }
                }
            }
        }// end tsmiActionOpenAnalysis_Click


        /* Descripción:
         *  Carga los datos de un fichero de analisis en  mostrando los datos al usuario
         * Parámetros:
         *  String path: El nombre del archivo.
         */
        private void LoadAnalysisFile(string path)
        {
            try
            {
                Analysis_and_G_Study tb_aux = ProjectSSQ.Analysis_and_G_Study.ReadingFileAnalysisSSQ(path);
                if (sagtElements.GetAnalysis_and_G_Study() != null)
                {
                    // Limpiamos todas las tablas
                    cleanerAllTabPageAnalysis();
                }
                sagtElements.SetAnalysis_and_G_Study(tb_aux);
                // Cargamos los datos en los datagridView
                // LoadAllDataInDataGridViewEx_SSQOptions();
                LoadAllDataGridWithDataAnalysis(sagtElements.GetAnalysis_and_G_Study(), path);
            }
            catch (Analysis_and_G_Study_Exception e)
            {
                // Mostramos información del error
                ShowMessageErrorOK(errorReadingFile + ". " + e.Message, titleMessageError1, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                /* NOTA: Excepción no controlada, revisar.
                 */
                // Mostramos un mensaje indicando que se produjo un error al leer el archivo.
                // MessageBox.Show(errorReadingFile, titleMessageError1);
                ShowMessageErrorOK("Error LoadAnalysisFile(string path): " + ex.Message);
            }
        }// end LoadAnalysisFile


        /* Descripción:
         *  Calcula los coeficientes de generabilidad para un nuevo nivel de optimización tras introducirlos
         *  por el usuario. Los datos resultantes se añaden como una nueva columna a la tabla resumen de 
         *  optimización.
         */
        private void tsmiActionAnalysis_AddLevelSign_Click(Analysis_and_G_Study anl_tAnalysis_G_study_opt)
        {
            // Combrobamos que haya un objeto de tipo Tabla de análisis
            if (anl_tAnalysis_G_study_opt == null)
            {
                ShowMessageErrorOK(errorNoSSQ);
            }
            else
            {
                G_ParametersOptimization newG_ParametersOpt = AddSignificanceLevel(anl_tAnalysis_G_study_opt);
                /* Si se ha devuelto null es que se ha cancelado la operación
                 * si no es null lo incorporaremos a la tabla de parámetros de optimización
                 */
                if (newG_ParametersOpt != null)
                {
                    anl_tAnalysis_G_study_opt.AddG_Parameter(newG_ParametersOpt);

                    // Posicionamos el tabPage de optimización
                    this.tabControlAnalysisSSQ.SelectedIndex = 2; // El dos se corresponde con el tabPabge optimización
                    ListFacets listFacets = anl_tAnalysis_G_study_opt.TableAnalysisVariance().ListFacets();
                    // Añadimos una nueva columna
                    AddColunmToDGVOptimization(listFacets, newG_ParametersOpt, this.dgvAnalysisResumOpt);
                }
            }
        }


        /* Descripción:
         *  Carga los valores de la suma total de la suma de cuadrados y los grados de libertad en
         *  sus respetivas etiquetas en el tabPageAnalysisSSQ.
         */
        private void LoadAnalysisTotalSSQ_TableComp(TableAnalysisOfVariance tbAnalysisVar)
        {
            int numOfDecimal = cfgApli.GetNumberOfDecimals();
            string puntoDecimal = this.cfgApli.GetDecimalSeparator();
            this.lbAnalysisTotalSsq.Text = ConvertNum.DecimalToString(tbAnalysisVar.CalcTotalSSQ(), numOfDecimal, puntoDecimal);
            this.lbAnalysisTotalDF.Text = tbAnalysisVar.CalcTotalDF().ToString();
        }


        /* Descripción:
         *  Muestra el diseño de medida en los textBox  de editar suma de cuadrados en los tabPage da Análisis.
         * Parámetros:
         *  ListFacets analysisSourceOfVarDiff: Lista de facetas de diferenciación.
         *  ListFacets analysisSourceOfVarInst: Lista de facetas de instrumentación.
         */
        private void ShowMeDesignInAnalysisTextBoxs(ListFacets analysisSourceOfVarDiff, ListFacets analysisSourceOfVarInst)
        {
            string mDesing = analysisSourceOfVarDiff.StringOfListFactes() +
                        "/ " + analysisSourceOfVarInst.StringOfListFactes();
            tbAnalysisMeasDesignG_P.Text = mDesing;
            tbAnalysisMesurDesignOpt.Text = mDesing;
        }


        /*
         * Descripción:
         *  Carga los valores totales de los G-Parmeters en las etiquetas de total suma de fuentes 
         *  objetivo, total varianza de error relativo y total varianza del error absoluto. Los datos
         *  se muestran el tabPageAnalysisG_Parameters
         *  
         */
        private void LoadAnalysisTotalG_Parameters(TableG_Study gParameter)
        {
            int numOfDecimal = cfgApli.GetNumberOfDecimals();
            string puntoDecimal = this.cfgApli.GetDecimalSeparator();
            this.lbAnalysisTotal_Target.Text = ConvertNum.DecimalToString(gParameter.TotalDifferentiationVariance(), numOfDecimal, puntoDecimal);

            this.lbAnalysisTotal_Error_Rel.Text = ConvertNum.DecimalToString(gParameter.TotalRelErrorVar(), numOfDecimal, puntoDecimal);
            this.lbAnalysisTotal_Error_Abs.Text = ConvertNum.DecimalToString(gParameter.TotalAbsErrorVar(), numOfDecimal, puntoDecimal);

            // Calculamos las desviaciones tipicas
            this.lbAnalysisStandDev.Text = ConvertNum.DecimalToString(gParameter.TargetStandDev(), numOfDecimal, puntoDecimal);
            this.lbAnalysisRelativeSE.Text = ConvertNum.DecimalToString(gParameter.ErrorRelStandDev(), numOfDecimal, puntoDecimal);
            this.lbAnalysisAbsoluteSE.Text = ConvertNum.DecimalToString(gParameter.ErrorAbsStandDev(), numOfDecimal, puntoDecimal);

            // calculamos los coeficientes de generalizabilidad
            this.lbAnalysisCoef_G_Rel.Text = ConvertNum.DecimalToString(gParameter.CoefG_Rel(), numOfDecimal, puntoDecimal);
            this.lbAnalysisCoef_G_Abs.Text = ConvertNum.DecimalToString(gParameter.CoefG_Abs(), numOfDecimal, puntoDecimal);

        }


        /* Descripción:
         *  Cancela la operación de editar las facetas y restaura el programa al estado original.
         */
        private void btActionCancelEditFacetOnAnalysis_Click(object sender, EventArgs e)
        {
            this.lf_nestings = null;
            disableEditingFacetAnalysis();
        }


        /* Descripción:
         *  Restaura el estado de los elementos necesarios tras la edición de las facetas de la Oción Ananlisis.
         */
        private void disableEditingFacetAnalysis()
        {
            // Deshabilitamos el menú principal poniendo la variable booleana disableTopLeftButtons a true.
            this.disableTopLeftButtons = false;
            this.mStripAnalysis.Enabled = true;
            // Ocultamos la pestaña de edición  de facetas
            this.tabPageAnalysisFacetas.Parent = null;
            this.tabPageAnalysisEditingSSq.Parent = null;
            // Mostramoslas otras tres pestañas
            this.tabPageAnalysisSSQ.Parent = this.tabControlAnalysisSSQ;
            this.tabPageAnalysisG_P.Parent = this.tabControlAnalysisSSQ;
            this.tabPageAnalysisOpt.Parent = this.tabControlAnalysisSSQ;
            this.tabPageAnalysisInf.Parent = this.tabControlAnalysisSSQ;
        }


        /* Descripción: 
         *  Cancela la operación de editar la suma de cuadrados e inicializa los tabPage de análisis
         *  a sus valores por defecto.
         * Parámetros:
         *  Analysis_and_G_Study analysisNew: valor actual modificado.
         *  Analysis_and_G_Study analysisOld: valor antiguo a restaurar.
         */
        private void btActionCancelEditSsq_Click()
        {
            this.lf_nestings = null;
            disableEditingFacetAnalysis();
            // Ponemos el modo edición a false para poder usar el menú principal
            this.disableTopLeftButtons = false;
            // Activamos el menú de acciones de Análisis
            this.mStripAnalysis.Enabled = true;
        }


        /* Descripción:
         *  Limpia todos los tabPage de la opción de Análisis
         */
        private void cleanerAllTabPageAnalysis()
        {
            string mDesign = "";
            // Limpiamos los TextBox de diseño de medida
            tbAnalysisMeasDesignG_P.Text = mDesign;
            tbAnalysisMesurDesignOpt.Text = mDesign;

            // Limpiamos los label de los tabPage G-Parámetros y suma de cudrados
            ClearListBoxAnalysis();

            // Limpiamos los textBox del tabPage de información
            this.tbFileAnalysisProvede.Text = mDesign;
            this.tbDateAnalysisCreated.Text = mDesign;
            this.rTextBoxAnalysisInfo.Text = mDesign;
            // Limpiamos el dgvEx del tabPage edición de Facetas
            ClearDataGridViewEx(dGridViewExAnalysis_TableFacet);
            // Limpiamos el dgvEx que muesta las facetas del tabPage edición de suma de cuadrados
            ClearDataGridViewEx(dgvExAnalysis_FacetEditSsq);
            // Limpiamos el dgvEx del tabPage edición de suma de cuadrados
            ClearDataGridViewEx(dgvExAnalysisEditSSq);
            // Limiamos los dgvEx del tabPage de suma de cuadrados
            ClearDataGridViewEx(dgvExAnalysisSourceOfVarSsq);
            ClearDataGridViewEx(dgvExAnalysisSSq);
            // Limpiamos el dgvEx del tabPage G-Parámetros
            ClearDataGridViewEx(dgvExAnalysis_GP);
            // Limpiamos el dgvEx de optimización
            ClearDataGridViewEx(dgvExAnalysisFacetsOpt);
            ClearDataGridViewEx(dgvAnalysisResumOpt);
            // ponemos las variables globales a null
            this.listFacetsAnalysis = null;
            sagtElements.SetAnalysis_and_G_Study(null);
        }// end cleanerAllTabPageAnalysis()


        /* Descripción:
         *  Cierrar los elementos abiertos e inicializa los registros textBox y DataGridViewEx.
         */
        private void tsmiActionCloseAnalysis_Click(object sender, EventArgs e)
        {
            if (sagtElements.GetAnalysis_and_G_Study() != null)
            {
                TableAnalysisOfVariance tableAnalysis = sagtElements.GetAnalysis_and_G_Study().TableAnalysisVariance();
                if (tableAnalysis != null)
                {
                    DialogResult res = ShowMessageDialog(titleConfirm, txtConfirmClose);
                    switch (res)
                    {
                        case (DialogResult.OK):
                            // Llamamos al método que lo limpia los elementos
                            cleanerAllTabPageAnalysis();
                            break;
                    }
                }
            }
        }


        /* Descripción:
         *  Se encarga de importar un nuevo fichero de suma de cuadrados perteneciente a alguna
         *  de las otras aplicaciones de teoria de la generalizabilidad (EduG 6.0, GT 2.0).
         */
        private void tsmiActionImportAnalysis_Click()
        {
            //FormWaiting fw = null;
            try
            {
                TransLibrary.Language lang = this.LanguageActually();
                FormSSQImport formSSQ_Import = new FormSSQImport(lang, this.cfgApli.Get_Path_Workspace());
                bool salir = false;
                do
                {
                    DialogResult res = formSSQ_Import.ShowDialog();

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
                                //fw = ShowLoadingScreen(msgLoading);
                                this.importAnalysis_SSqFile(formSSQ_Import.pathFile());
                                //CloseLoadingScreen(fw);
                                salir = true;
                            }
                            break;
                    }
                } while (!salir);
            }
            catch (IOException)
            {
                // Mostramos un mensage indicando que el fichero esta siendo usado
                ShowMessageErrorOK(errorFileInUse);
            }
            catch (Exception ex)
            {
                // Mostramos un mensage indicando que el fichero no esta en formato correcto.
                // ShowMessageErrorOK(errorFormatFile);
                ShowMessageErrorOK("Error tsmiActionImportAnalysis_Click():" + ex.Message);
            }
        }// end tsmiActionImportAnalysis_Click


        /* Descripción:
         *  Importa un fichero de suma de cuadrados.
         */
        public void importAnalysis_SSqFile(string path)
        {
            FormWaiting fw = null;

            try
            {
                // Extraemos el nombre del fichero del path
                string fileExt = fileExtension(path).ToLower(); // Pasamos a minúsculas la extensión
                TypeOfFile typeOfFile = (TypeOfFile)Enum.Parse(typeof(TypeOfFile), fileExt, true);

                Analysis_and_G_Study tAnalysis_tG_Study_Opt = null;
                // para poder compararla. 
                switch (typeOfFile)
                {
                    case (TypeOfFile.ssq):
                        fw = ShowLoadingScreen(msgLoading);
                        tAnalysis_tG_Study_Opt = Aux_loadListTableSSqOfFileSsq(path);
                        CloseLoadingScreen(fw);
                        break;
                    case (TypeOfFile.rsa):
                        fw = ShowLoadingScreen(msgLoading);
                        tAnalysis_tG_Study_Opt = Aux_loadListTableSSqOfFileRsa(path);
                        CloseLoadingScreen(fw);
                        break;
                    case (TypeOfFile.txt):
                        fw = ShowLoadingScreen(msgLoading);
                        List<AnalysisSsqEduG> listAnalysisEduG = AnalysisSsqEduG.ReadFileReportTxtEduG(path);
                        CloseLoadingScreen(fw);
                        tAnalysis_tG_Study_Opt = Aux_SelectAnalysisOfListAnalyisReports(listAnalysisEduG);
                        break;
                    case (TypeOfFile.rtf):
                        fw = ShowLoadingScreen(msgLoading);
                        List<AnalysisSsqEduG> listAnalysisEduG2 = AnalysisSsqEduG.ReadFileReportRtfEduG(path);
                        CloseLoadingScreen(fw);
                        tAnalysis_tG_Study_Opt = Aux_SelectAnalysisOfListAnalyisReports(listAnalysisEduG2);
                        break;
                    case (TypeOfFile.xls):
                        fw = ShowLoadingScreen(msgLoading);
                        tAnalysis_tG_Study_Opt = Aux_loadListTableSSqOfFileXls(path);
                        CloseLoadingScreen(fw);
                        break;
                    default:
                        ShowMessageErrorOK(errorInvalidExtension);
                        break;
                }
                if (tAnalysis_tG_Study_Opt != null)
                {
                    DateTime date = DateTime.Now;
                    sagtElements.SetAnalysis_and_G_Study(tAnalysis_tG_Study_Opt);
                    sagtElements.GetAnalysis_and_G_Study().SetDateTime(date);
                    sagtElements.GetAnalysis_and_G_Study().SetNameFileDataCreation(path);
                    LoadAllDataGridWithDataAnalysis(sagtElements.GetAnalysis_and_G_Study(), path);
                }
            }
            catch (SSqPY_Exception)
            {
                CloseLoadingScreen(fw);
                ShowMessageErrorOK(errorFormatFile);
            }
        }// end importAnalysis_SSqFile


        /* Descripción:
         *  Generar un archivo Excel a partir de los datos contenidos en las tablas de Análisis.
         */
        private void tsmiActionAnalysisExportExcel_Click(object sender, EventArgs e)
        {
            if (sagtElements.GetAnalysis_and_G_Study() == null)
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
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    // FormWaiting fw = ShowLoadingScreen(msgLoading);  // seemingly loads too fast

                    var sheetList = new List<(string SheetName, DataGridView Grid)>
                    {
                        (tabPagMultiFacet.Text, dgvExAnalysisSourceOfVarSsq),
                        (tabPageAnalysisSSQ.Text, dgvExAnalysisSSq),
                        (tabPageAnalysisG_P.Text, dgvExAnalysis_GP),
                        (tabPageAnalysisOpt.Text, dgvAnalysisResumOpt),
                    };
                    ExportExcel.ExportMultipleSheets(sheetList, saveDialog.FileName);

                    // MessageBox.Show("Fin");
                    Process.Start(saveDialog.FileName); //opens the file
                    saveDialog.Dispose();

                    // CloseLoadingScreen(fw);
                }
            }
        }// end tsmiActionAnalysisExportExcel_Click


        /* Descripción:
         *  Permite añadir y editar comentarios en el tabPageInfo de la opción Análisis
         */
        private void btActionAnalysisEditComment_Click(Analysis_and_G_Study anl_tAnalysis_G_study_opt)
        {
            if (anl_tAnalysis_G_study_opt == null)
            {
                ShowMessageErrorOK(errorNoFileSelected);
            }
            else
            {
                TransLibrary.Language lang = this.LanguageActually();
                string text = this.rTextBoxAnalysisInfo.Text;
                FormEditFileComment formEditComment = new FormEditFileComment(text, lang);
                bool salir = false;
                do
                {
                    DialogResult res = formEditComment.ShowDialog();
                    switch (res)
                    {
                        case DialogResult.Cancel: salir = true; break;
                        case DialogResult.OK:
                            anl_tAnalysis_G_study_opt.SetTextComment(formEditComment.GetRichTextBoxText());
                            this.rTextBoxAnalysisInfo.Text = anl_tAnalysis_G_study_opt.GetTextComment();
                            // guardamos en el fichero
                            // tsmiActionAnalysis_Save_Click();
                            salir = true;
                            break;
                    }
                } while (!salir);
            }
        }// end btActionAnalysisEditComment_Click


        /* Descripción:
         *  Introduce los datos leidos de un fichero de datos en la tabla de edición de suma de cuadrados.
         */
        private void btActionImportAnalysisEditSsq_Click(DataGridViewEx.DataGridViewEx tableAnalysisEditSSq)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            if (Directory.Exists(this.cfgApli.Get_Path_Workspace()))
            {
                openDialog.InitialDirectory = this.cfgApli.Get_Path_Workspace();
            }

            /* fileFilter = "Fichero de puntuaciones" + " (*.dat)|*.dat|" + "Fichero de sumas de cuadrados EduG"+ " (*.edug)|*.edug|"
             * "Todos los archivos" + " (*.*)|*.*"
             */
            string fileFilter = (filterDatas + FILTER_DATA + filterSsqExportEduG + FILTER_SSQ_EDUG
                + this.allFiles + FILTER_ALL_FILE);
            openDialog.Filter = fileFilter;

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Contenedor de la suma de cudrados
                    List<double> listScores = null;

                    string pathFile = openDialog.FileName;
                    string extFile = pathFile.Substring(pathFile.LastIndexOf('.') + 1);
                    switch (extFile)
                    {
                        case (DEFAULT_EXT_SCORE):
                            // Leemos las sumas de cuadrados
                            // como es un método estático lo podemos usar para leer los cuadrados
                            listScores = MultiFacetsObs.ReadingFileDataScore(openDialog.FileName);
                            break;
                        case (DEFAULT_EXT_SSQ_EDUG):
                            string key = "";
                            try
                            {
                                Dictionary<string, double?> dicSSq = AnalysisSsqEduG.ImportSsq_File(openDialog.FileName);
                                // Tomamos los datos en el mismo orden 
                                List<string> ldesign = this.llFacetsAnalysis;
                                if (sagtElements.GetAnalysis_and_G_Study() != null)
                                {
                                    TableAnalysisOfVariance tableAnalysis = sagtElements.GetAnalysis_and_G_Study().TableAnalysisVariance();
                                    ListFacets lf = tableAnalysis.ListFacets();
                                    ldesign = lf.CombinationStringWithoutRepetition();
                                }

                                listScores = new List<double>();

                                int numDesign = ldesign.Count;
                                for (int i = 0; i < numDesign; i++)
                                {
                                    key = ldesign[i];
                                    listScores.Add((double)dicSSq[key]);
                                }
                            }
                            catch (KeyNotFoundException)
                            {
                                // si hay alguna clave que no esta contenida devolvemos el error
                                ShowMessageErrorOK(errorSourceSsqEduG);
                                listScores = null;
                            }
                            break;
                        default:
                            // Por defecto leemos los ficheros .dat
                            listScores = MultiFacetsObs.ReadingFileDataScore(openDialog.FileName);
                            break;
                    }

                    if (listScores != null)
                    {
                        // Introducimos los datos e informamos del resultado.
                        int numOfRows = tableAnalysisEditSSq.RowCount;
                        int numOfList = listScores.Count;
                        int n = numOfList;
                        if (numOfRows < numOfList)
                        {
                            n = numOfRows;
                        }

                        int nCols = tableAnalysisEditSSq.ColumnCount - 1;

                        // Número de decimales para la representación
                        int numOfDecimal = cfgApli.GetNumberOfDecimals();
                        // Punto de separación decimal
                        string puntoDecimal = this.cfgApli.GetDecimalSeparator();

                        // Introducimos los datos en el datagridView;
                        for (int i = 0; i < n; i++)
                        {
                            //tableScores.Rows[i].Cells[nCols].Value = DecimalSetting.DecimalToString(listScores[i], numOfDecimal, puntoDecimal);
                            tableAnalysisEditSSq.Rows[i].Cells[nCols].Value = listScores[i].ToString();
                        }

                        // Mostramos mensaje
                        string message = txtInfoImportScores;
                        message = message.Replace("[n]", n.ToString());
                        ShowMessageInfo(message);

                    }
                }
                catch (MultiFacetObsException ex)
                {
                    // Mostramos un mensaje de error informando de que no se han podido extraer los datos
                    ShowMessageErrorOK($"{errorReadingFileScore}\n\n{ex.Message}", titleMessageError1, MessageBoxIcon.Error);
                }
            }
        }// end btActionsImportScores_Click


        /* Descripción:
         *  - Lee análisis de componentes de varianza generado por la librería VCA de R
         *  - Crea y guarda el análisis resultante
         */
        private void btActionImportAnalysisVCA_Click()
        {
            // Si estamos en el modo edición entonces actualizamos estos valores temporales
            if (this.disableTopLeftButtons && sagtElements.GetAnalysis_and_G_Study() != null)
            {
                listFacetsAnalysis = sagtElements.GetAnalysis_and_G_Study().TableAnalysisVariance().ListFacets();
                analysisSourceOfVarDiff = sagtElements.GetAnalysis_and_G_Study().TableG_Study_Percent().LfDifferentiation();
                analysisSourceOfVarInst = sagtElements.GetAnalysis_and_G_Study().TableG_Study_Percent().LfInstrumentation();
            }

            // Select VCA CSV
            OpenFileDialog openDialog = new OpenFileDialog();

            if (Directory.Exists(cfgApli.Get_Path_Workspace()))
                openDialog.InitialDirectory = cfgApli.Get_Path_Workspace();

            openDialog.Filter = ("Comma-separated values" + FILTER_CSV + this.allFiles + FILTER_ALL_FILE);

            if (openDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                // Read VCA analysis
                TableAnalysisOfVariance tbAnalysisVar =
                    new TableAnalysisOfVariance(listFacetsAnalysis, openDialog.FileName, true);

                // Build G-study
                TableG_Study_Percent gp =
                    new TableG_Study_Percent(
                        analysisSourceOfVarDiff,
                        analysisSourceOfVarInst,
                        tbAnalysisVar);

                Analysis_and_G_Study analysis =
                    new Analysis_and_G_Study(tbAnalysisVar, gp);

                // Ask where to save
                SaveFileDialog saveDialog = new SaveFileDialog();

                if (Directory.Exists(cfgApli.Get_Path_Workspace()))
                    saveDialog.InitialDirectory = cfgApli.Get_Path_Workspace();

                saveDialog.DefaultExt = "anls";
                saveDialog.Filter = "Analysis file" + FILTER_ANALYSIS_FILTER;
                saveDialog.AddExtension = true;
                saveDialog.OverwritePrompt = true;

                if (saveDialog.ShowDialog() != DialogResult.OK)
                    return;

                analysis.SetDateTime(DateTime.Now);
                analysis.SetNameFileDataCreation(saveDialog.FileName);

                analysis.WritingFileAnalysisSSQ(saveDialog.FileName);

                sagtElements.SetAnalysis_and_G_Study(analysis);

                ShowMeDesignInAnalysisTextBoxs(gp.LfDifferentiation(), gp.LfInstrumentation());

                LoadAllDataGridWithDataAnalysis(
                    sagtElements.GetAnalysis_and_G_Study(),
                    saveDialog.FileName);

                disableEditingFacetAnalysis();
            }
            catch (Exception ex)
            {
                ShowMessageErrorOK(txtCvaError + "\n\n" + ex.Message);
            }
        }


        /* Descripción:
         *  - Lee análisis de componentes de varianza generado por la librería VCA de R
         *  - Crea y guarda el análisis resultante
         */
        private void btActionImportAnalysisSAS_Click()
        {
            // Si estamos en el modo edición entonces actualizamos estos valores temporales
            if (this.disableTopLeftButtons && sagtElements.GetAnalysis_and_G_Study() != null)
            {
                listFacetsAnalysis = sagtElements.GetAnalysis_and_G_Study().TableAnalysisVariance().ListFacets();
                analysisSourceOfVarDiff = sagtElements.GetAnalysis_and_G_Study().TableG_Study_Percent().LfDifferentiation();
                analysisSourceOfVarInst = sagtElements.GetAnalysis_and_G_Study().TableG_Study_Percent().LfInstrumentation();
            }

            // Select SAS .LST (also give option for .txt)
            OpenFileDialog openDialog = new OpenFileDialog();

            if (Directory.Exists(cfgApli.Get_Path_Workspace()))
                openDialog.InitialDirectory = cfgApli.Get_Path_Workspace();

            openDialog.Filter = (
                    "SAS Listing Files" + " (*.lst)|*.lst|" +
                    "Text Files" + " (*.txt)|*.txt|" +
                    this.allFiles + FILTER_ALL_FILE);

            if (openDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                // Read SAS analysis
                TableAnalysisOfVariance tbAnalysisVar =
                    new TableAnalysisOfVariance(listFacetsAnalysis, openDialog.FileName, false);

                // Build G-study
                TableG_Study_Percent gp =
                    new TableG_Study_Percent(
                        analysisSourceOfVarDiff,
                        analysisSourceOfVarInst,
                        tbAnalysisVar);

                Analysis_and_G_Study analysis =
                    new Analysis_and_G_Study(tbAnalysisVar, gp);

                // Ask where to save
                SaveFileDialog saveDialog = new SaveFileDialog();

                if (Directory.Exists(cfgApli.Get_Path_Workspace()))
                    saveDialog.InitialDirectory = cfgApli.Get_Path_Workspace();

                saveDialog.DefaultExt = "anls";
                saveDialog.Filter = "Analysis file" + FILTER_ANALYSIS_FILTER;
                saveDialog.AddExtension = true;
                saveDialog.OverwritePrompt = true;

                if (saveDialog.ShowDialog() != DialogResult.OK)
                    return;

                analysis.SetDateTime(DateTime.Now);
                analysis.SetNameFileDataCreation(saveDialog.FileName);

                analysis.WritingFileAnalysisSSQ(saveDialog.FileName);

                sagtElements.SetAnalysis_and_G_Study(analysis);

                ShowMeDesignInAnalysisTextBoxs(gp.LfDifferentiation(), gp.LfInstrumentation());

                LoadAllDataGridWithDataAnalysis(
                    sagtElements.GetAnalysis_and_G_Study(),
                    saveDialog.FileName);

                disableEditingFacetAnalysis();
            }
            catch (Exception ex)
            {
                ShowMessageErrorOK(txtSasError + "\n\n" + ex.Message);
            }
        }


        /* Descripción:
         *  Soporte para la edición de facetas en Análisis
         */
        private void AnalysisUpdateFacets()
        {
            try
            {
                // Leer las facetas;
                ListFacets newLf = dgvExToListFacets(this.dgvExAnalysis_FacetEditSsq);

                // Actualizar la lista actual con los valores de la nueva
                ListFacets oldLf = sagtElements.GetAnalysis_and_G_Study().TableAnalysisVariance().ListFacets();
                newLf = oldLf.RemplaceListFacets(newLf);

                // generar la tabla de análisis partiendo de la suma de cuadrados anterior
                // actualizar los valores de optimización
                sagtElements.GetAnalysis_and_G_Study().ReplaceListOfFacet(newLf);
                //********************************************************************************
                sagtElements.GetAnalysis_and_G_Study().SetNameFileDataCreation(sagtElements.GetAnalysis_and_G_Study().GetNameFileDataCreation());
                sagtElements.GetAnalysis_and_G_Study().SetDateTime(sagtElements.GetAnalysis_and_G_Study().GetDateTime());

                // cargar los valores nuevos.
                LoadAllDataGridWithDataAnalysis(sagtElements.GetAnalysis_and_G_Study(), sagtElements.GetAnalysis_and_G_Study().GetNameFileDataCreation());

                // Restauramos ReadOnly
                for (int i = 0; i < dgvExAnalysis_FacetEditSsq.ColumnCount; i++)
                {
                    dgvExAnalysis_FacetEditSsq.Columns[i].ReadOnly = true;
                }
            }
            catch (ListFacetsException)
            {
                // Mostramos un mensaje de error indicando que no puede haber facetas repetidas
                ShowMessageErrorOK(errorDuplicateNameFacet);
            }
            catch (Exception ex)
            {
                // Capturamos la excepción y mostramos el problema
                ShowMessageErrorOK("Error btActionAcept_Click(): " + ex.Message);
            }

        }// end btActionAcept_Click



        /* Descripción:
         *  Edita las sumas de cuadrados de la opción análisis.
         */
        private void tsmiActionAnalysisEditSsq_Click()
        {
            if (sagtElements.GetAnalysis_and_G_Study() == null)
            {//begin if (*1*)
                ShowMessageErrorOK(errorNoSSQ);
            }
            else
            {
                /* EDITING FACETS */
                CleanerDataGridViewExFacets(dgvExAnalysis_FacetEditSsq); // limpiamos dgv
                LoadListFacetInDataGridView(
                    sagtElements.GetAnalysis_and_G_Study().GetListFacets(),
                    dgvExAnalysis_FacetEditSsq, false, true); // cargamos la lista de facetas en el datagrid (versión nombres)
                // Permitimos la edición de las columnas
                for (int i = 0; i < dgvExAnalysis_FacetEditSsq.ColumnCount; i++)
                {
                    dgvExAnalysis_FacetEditSsq.Columns[i].ReadOnly = false;
                }

                /* EDITING SSQ */
                /* Mostramos el tabPage de edición con las suma de cuadrados editables.
                 * Soló la suma de cuadrados no las fuentes de variación.
                 */
                // Ocultamos los tabPage
                // Ocultamos las pestañas
                foreach (TabPage tabPage in this.tabControlAnalysisSSQ.TabPages)
                {
                    tabPage.Parent = null;
                }
                tabPageAnalysisEditingSSq.Text = this.btAnalysisEditComment.Text;   // Change tab text to "Editar"
                Aux_EditSsqValues(sagtElements.GetAnalysis_and_G_Study().TableAnalysisVariance().ListFacets());
            }
        }// end tsmiActionAnalysisEditSsq_Click


        /* Descripción:
         *  Permite cambiar el diseño de medida de un modelo.
         */
        private void tsmiActionChangeModel_Click()
        {
            if (sagtElements.GetAnalysis_and_G_Study() == null)
            {//begin if (*1*)
                ShowMessageErrorOK(errorNoSSQ);
            }
            else
            {
                // Avisamos de que se perdera información si no se ha guardado
                // Lista de facetas de Instrumentación y diferenciación
                analysisSourceOfVarDiff = sagtElements.GetAnalysis_and_G_Study().TableG_Study_Percent().LfDifferentiation();
                analysisSourceOfVarInst = sagtElements.GetAnalysis_and_G_Study().TableG_Study_Percent().LfInstrumentation();
                // mostramos la ventana de selección de diseño
                // Creamos la ventana para introducir el diseño de medida
                FormMeasurDesign formMeasurDesign = new FormMeasurDesign(analysisSourceOfVarDiff, analysisSourceOfVarInst, sagtElements.GetAnalysis_and_G_Study().GetListFacets(), cfgApli.GetConfigLanguage());
                bool salir = false; // variable de control del bucle

                do
                {
                    DialogResult res = formMeasurDesign.ShowDialog();
                    switch (res)
                    {
                        case (DialogResult.Cancel): salir = true; break;
                        case (DialogResult.OK):
                            if (formMeasurDesign.ListFacetDiff() == 0 || formMeasurDesign.ListFacetInst() == 0)
                            {
                                ShowMessageErrorOK(errorM_DesignNoValidate, "", MessageBoxIcon.Stop);
                            }
                            else
                            {
                                // Obtenemos la lista de facetas de instrumentación y de diferenciación.
                                // analysisSourceOfVarDiff;
                                // analysisSourceOfVarInst;
                                // si se ha seleccionado aceptar aplicamos los cambios
                                string nameFile = sagtElements.GetAnalysis_and_G_Study().GetNameFileDataCreation();
                                TableAnalysisOfVariance tbAnalysisOfVar = sagtElements.GetAnalysis_and_G_Study().TableAnalysisVariance();
                                TableG_Study_Percent tb_G_study_percent = new TableG_Study_Percent(analysisSourceOfVarDiff, analysisSourceOfVarInst, tbAnalysisOfVar);
                                sagtElements.SetAnalysis_and_G_Study(new Analysis_and_G_Study(tbAnalysisOfVar, tb_G_study_percent));
                                sagtElements.GetAnalysis_and_G_Study().SetNameFileDataCreation(nameFile);
                                LoadAllDataGridWithDataAnalysis(sagtElements.GetAnalysis_and_G_Study(), nameFile);
                                salir = true;

                            }
                            break;
                    }
                } while (!salir);
            }
        }// end tsmiActionChangeModel_Click


        #region Cambio de idioma de los elementos del tabPageAnalysis
        /*
         * Descripción:
         *  Traduce los elementos del TabPageAnalysis.
         * Parámetros:
         *  TransLibrary.Language lang: idioma al que vamos a traducir los elementos.
         *  string nameFileTrans: Nombre del fichero que contiene las traducciones.
         */
        private void TranslationAnalysisElements(TransLibrary.Language lang, string nameFileTrans)
        {
            TransLibrary.ReadFileTrans dic = new TransLibrary.ReadFileTrans(nameFileTrans);
            string name = "";
            try
            {
                // traducimos las etiquetas de las pestañas
                // Traducimos el tapPage: Facetas
                name = this.tabPageAnalysisFacetas.Name.ToString();
                this.tabPageAnalysisFacetas.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el tabPage de edición de suma de cuadrados
                name = this.tabPageAnalysisEditingSSq.Name.ToString();
                this.tabPageAnalysisEditingSSq.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el tapPage: Suma de cuadrados
                name = this.tabPageAnalysisSSQ.Name.ToString();
                this.tabPageAnalysisSSQ.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el tapPage: G-Parámetros
                name = this.tabPageAnalysisG_P.Name.ToString();
                this.tabPageAnalysisG_P.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el tapPage: Optimización
                name = this.tabPageAnalysisOpt.Name.ToString();
                this.tabPageAnalysisOpt.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el tapPage: Información
                name = this.tabPageAnalysisInf.Name.ToString();
                this.tabPageAnalysisInf.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Botones del tabPage Facetas
                name = this.btAnalysis_NestingFacet.Name.ToString();
                this.btAnalysis_NestingFacet.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.btAnalysis_RemoveNesting.Name.ToString();
                this.btAnalysis_RemoveNesting.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.btEditSumOfSquaresOnAnalisys.Name.ToString();
                this.btEditSumOfSquaresOnAnalisys.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Botón cancelar de tabPage de edición de suma de cuadrados
                name = this.btCancelEditSsq.Name.ToString();
                this.btCancelEditSsq.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Botón Guardar de tabPage de edición de suma de cuadrados
                name = this.btSaveAnalysisSsq.Name.ToString();
                this.btSaveAnalysisSsq.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Botón Importar suma de cuadrados del tabPage edición de suma de cuadrados
                name = this.btImportAnalysisEditSsq.Name.ToString();
                this.btImportAnalysisEditSsq.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.btImportAnalysisVCA.Name.ToString();
                this.btImportAnalysisVCA.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.btImportAnalysisSAS.Name.ToString();
                this.btImportAnalysisSAS.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Botón Editar del tabPage Información
                name = this.btAnalysisEditComment.Name.ToString();
                this.btAnalysisEditComment.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Etiquetas del tabPageInformación
                name = this.lbFileAnalysisProvede.Name.ToString();
                this.lbFileAnalysisProvede.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbDateAnalysisCreated.Name.ToString();
                this.lbDateAnalysisCreated.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Etiqueta de diseño de medida
                name = lbAnalysisMeasDesignG_P.Name.ToString();
                lbAnalysisMeasDesignG_P.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                lbAnalysisMesurDesignOpt.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos los menú contextuales de los dataGridViewEx
                TranslationTContextualMenu(this.dGridViewExAnalysis_TableFacet, dicMeans, lang);
                TranslationTContextualMenu(this.dgvExAnalysisSourceOfVarSsq, dicMeans, lang);
                TranslationTContextualMenu(this.dgvExAnalysisSSq, dicMeans, lang);
                TranslationTContextualMenu(this.dgvExAnalysis_GP, dicMeans, lang);
                TranslationTContextualMenu(this.dgvExAnalysisFacetsOpt, dicMeans, lang);
                TranslationTContextualMenu(this.dgvAnalysisResumOpt, dicMeans, lang);

                /*
                // Traducimos la ventan de gráficos   
                if (this.formShowCharts != null)
                {
                    this.formShowCharts.traslationElements(lang);
                }
                */
                if (dgvExAnalysisSourceOfVarSsq.ColumnCount != 0)
                {
                    // Cambiamos el nombre de las columnas
                    dgvExAnalysisSourceOfVarSsq.Columns[0].HeaderText = nameColFacet; // Nombre de la columna Etiquetas (dependerá del idioma).
                    dgvExAnalysisSourceOfVarSsq.Columns[1].HeaderText = nameColLevel; // Nombre de la columna Niveles (dependerá del idioma).
                    dgvExAnalysisSourceOfVarSsq.Columns[2].HeaderText = nameColSizeOfUniverse; // Nombre de la columna Descripción (dependerá del idioma).
                    dgvExAnalysisSourceOfVarSsq.Columns[3].HeaderText = nameColComment; // Nombre de la columna Descripción
                }


                // actuamos sobre el dataGridViewEx
                if (dgvExAnalysisSSq.ColumnCount != 0)
                {
                    // Cambiamos el nombre de las columnas
                    dgvExAnalysisSSq.Columns[IND_SOURCE_OF_VAR].HeaderText = sourceOfVarString; // Nombre de la columna "Fuentes de variación".
                    dgvExAnalysisSSq.Columns[IND_SSQ].HeaderText = ssqString; // Nombre de la columna Niveles (dependerá del idioma).
                    dgvExAnalysisSSq.Columns[IND_DEGREE_OF_FREEDOM].HeaderText = degreeOfFreedomString; // Nombre de la columna Descripción (dependerá del idioma).
                    dgvExAnalysisSSq.Columns[IND_MSQ].HeaderText = msqString;
                    dgvExAnalysisSSq.Columns[IND_RANDOM_COMP].HeaderText = randomCompString;
                    dgvExAnalysisSSq.Columns[IND_MIX_COMP].HeaderText = mixCompString;
                    dgvExAnalysisSSq.Columns[IND_CORRECTED_COMP].HeaderText = correctedComp;
                }


                // actuamos sobre el dGridViewExG_Parameters
                if (dgvExAnalysis_GP.ColumnCount != 0)
                {
                    dgvExAnalysis_GP.Columns[0].HeaderText = source;
                    dgvExAnalysis_GP.Columns[1].HeaderText = diff_var;
                    dgvExAnalysis_GP.Columns[2].HeaderText = source;
                    dgvExAnalysis_GP.Columns[3].HeaderText = rel_err_var;
                    dgvExAnalysis_GP.Columns[4].HeaderText = percent_rel_err;
                    dgvExAnalysis_GP.Columns[5].HeaderText = abs_err_var;
                    dgvExAnalysis_GP.Columns[6].HeaderText = percent_abs_err;
                }


                // Actuamos sobre dGridViewExFacetsOptimization
                if (dgvExAnalysisFacetsOpt.ColumnCount != 0)
                {
                    dgvExAnalysisFacetsOpt.Columns[IND_NAME].HeaderText = nameColFacet; // Nombre de la columna Etiquetas (dependerá del idioma).
                    dgvExAnalysisFacetsOpt.Columns[IND_LEVEL].HeaderText = nameColLevel; // Nombre de la columna Niveles (dependerá del idioma).
                    dgvExAnalysisFacetsOpt.Columns[IND_SIZE_OF_UNIVERSE].HeaderText = this.nameColSizeOfUniverse;
                    dgvExAnalysisFacetsOpt.Columns[IND_SSQQDESC].HeaderText = this.nameColComment;
                }


                // Actuamos sobre el dGridViewExOptimizationResum
                if (this.dgvAnalysisResumOpt.ColumnCount != 0)
                {
                    // dGridViewExOptimizationResum.Columns[0].HeaderText = name_resum;
                    // Entonces pintamos la tabla de resumen de nuevo
                    LoadDataGridViewExOptimizationResum(sagtElements.GetAnalysis_and_G_Study(),
                        this.dgvAnalysisResumOpt);
                }

                /*
                // Traducimos las etiquetas de texto de los tabPage
                name = lbAnalysisTotalSsq.Name.ToString();
                lbAnalysisTotalSsq.Text = dic.labelTraslation(name).LangTraslation(lang).ToString();
                name = lbTotalTargetVar.Name.ToString();
                lbTotalTargetVar.Text = dic.labelTraslation(name).LangTraslation(lang).ToString();
                name = lbTotalRelErr.Name.ToString();
                lbTotalRelErr.Text = dic.labelTraslation(name).LangTraslation(lang).ToString();
                */

                name = lbAnalysisTotal.Name.ToString();
                lbAnalysisTotal.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbAnalysisTotalTargetVar.Name.ToString();
                lbAnalysisTotalTargetVar.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbAnalysisTotalRelErr.Name.ToString();
                lbAnalysisTotalRelErr.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbAnalysisTotalAbsErr.Name.ToString();
                lbAnalysisTotalAbsErr.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbAnalysisTextStandDev.Name.ToString();
                lbAnalysisTextStandDev.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbAnalysisTextRelSE.Name.ToString();
                lbAnalysisTextRelSE.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbAnalysisTextAbsoluteSE.Name.ToString();
                lbAnalysisTextAbsoluteSE.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbAnalysisTextGeneralizabilityCoef.Name.ToString();
                lbAnalysisTextGeneralizabilityCoef.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbAnalysisTextCoef_G_Rel.Name.ToString();
                lbAnalysisTextCoef_G_Rel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = lbAnalysisTextCoef_G_Abs.Name.ToString();
                lbAnalysisTextCoef_G_Abs.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                ShowMessageErrorOK(lEx.Message + " " + errorMessageTraslation + " " + name);
            }
        } // private void TraslationAnalysisElements

        #endregion Cambio de idioma de los elementos del tabPageAnalysis


    }// public partial class FormPrincipal : Form
}// namespace GUI_TG