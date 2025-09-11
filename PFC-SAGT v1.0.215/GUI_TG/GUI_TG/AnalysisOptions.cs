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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AuxMathCalcGT;
using MultiFacetData;
using ProjectSSQ;
using SsqPY;
using System.Threading; // permite usar hilos
using ImportEduGSsq;

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
        // private List<TableAnalysisOfVariance> listOfListAnalysisTableSSQ;
        // private List<TableG_Study_Percent> listAnalysisG_Parameters;
        private Analysis_and_G_Study anl_tAnalysis_G_study_opt; // variable con las tablas de análisis con la que se trabaja actualmente
        private Analysis_and_G_Study anl_tAnalysis_G_study_opt_Old; // variable antigua, para permitir deshacer cambios.


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
            // Deshabilitamos el menú principal poniendo la variable booleana editionModeOn a true.
            this.editionModeOn = true;
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
                this.anl_tAnalysis_G_study_opt_Old = this.anl_tAnalysis_G_study_opt; // guardamos los datos por si deseamos cancelar
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
            this.editionModeOn = false; // hemos finalizado la edición de facetas
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
            // El botón Aceptar estará oculto ya que este solo se mostrará al editar las facetas
            btAcept.Enabled = false;
            btAcept.Visible = false;
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
            // Botón aceptar visible
            btAcept.Enabled = true;
            btAcept.Visible = true;
        }


        /* Descripción:
         *  Habilita, muestra y oculta los elementos necesarios para la edición de las facetas de la Opción Análisis.
         *  No actua sobre los botones solo sobre los tabPages y los menús.
         */
        private void enableEditingFacetAnalysis()
        {
            // Deshabilitamos el menú principal poniendo la variable booleana editionModeOn a true.
            this.editionModeOn = true;
            this.mStripAnalysis.Enabled = false;
            // Mostramos la pestaña de edición  de facetas
            this.tabPageAnalysisFacetas.Parent = this.tabControlAnalysisSSQ;
            // Ocultamos las otras cuatro pestañas
            this.tabPageAnalysisEditingSSq.Parent = null;
            this.tabPageAnalysisSSQ.Parent = null;
            this.tabPageAnalysisG_P.Parent = null;
            this.tabPageAnalysisOpt.Parent = null;
            this.tabPageAnalysisInf.Parent = null;
        }


        /* Descripción:
         *  Muestra el tapPage de edicion de suma de cuadrados en la opción de Análisis
         */
        private void enableEditingSSqAnalysis()
        {
            // Deshabilitamos el menú principal poniendo la variable booleana editionModeOn a true.
            this.editionModeOn = true;
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
            if (this.listFacetsAnalysis == null)
            {
                ListFacets lf = validateFacetTable(dGridViewExAnalysis_TableFacet);
                if (lf != null)
                {
                    // Lanzamos una advertencia: Ya no podrá editar las facetas
                    DialogResult res = ShowMessageDialog(titleConfirm, txtConfirmBuildNesting);
                    if (res.Equals(DialogResult.OK))
                    {
                        this.listFacetsAnalysis = lf;
                    }
                }
            }


            if (this.listFacetsAnalysis != null)
            {
                FormAddNesting fAddNesting = new FormAddNesting(this.LanguageActually(), this.listFacetsAnalysis);

                bool salir = false;
                do
                {
                    DialogResult res = fAddNesting.ShowDialog();
                    switch (res)
                    {
                        case DialogResult.Cancel:
                            salir = true;
                            break;
                        case DialogResult.OK:
                            // Asignamos el anidamiento
                            int posSelctFacet = fAddNesting.ComboBoxSelectFacet();
                            if (posSelctFacet >= 0)
                            {
                                int posNestedFacet = fAddNesting.ComboBoxNestingFacet();
                                if (posNestedFacet >= 0)
                                {
                                    // Tenemos tanto la faceta anidada como la anidante
                                    Facet f = this.listFacetsAnalysis.FacetInPos(posSelctFacet);
                                    Facet f_Nested = this.listFacetsAnalysis.FacetInPos(posNestedFacet);
                                    try
                                    {
                                        if (fAddNesting.RadioButtonNest())
                                        {
                                            f.ListFacetsDesignNesting(f_Nested);
                                        }
                                        else if (fAddNesting.RadioButtonCross())
                                        {
                                            f.ListFacetsDesignCrossed(f_Nested);
                                        }
                                        else
                                        {
                                            // operación no valida
                                            ShowMessageErrorOK(errorNoOperation);
                                        }
                                        // pintamos de nuevo las facetas en la tabla
                                        LoadListFacetInDataGridView(this.listFacetsAnalysis, dGridViewExAnalysis_TableFacet);
                                        salir = true;
                                    }
                                    catch (FacetException)
                                    {
                                        // error no se ha podido realizar el anidamiento correctamente
                                        ShowMessageErrorOK(errorNoOperation);
                                    }
                                }
                                else
                                {
                                    // error no ha seleccionado la faceta anidante
                                    ShowMessageErrorOK(errorNoSelectFacetNesting);
                                }
                            }
                            else
                            {
                                // error no ha seleccionado la faceta
                                ShowMessageErrorOK(errorNoSelectFacetNested);
                            }

                            break;
                    }
                } while (!salir);
            }// end if
        }// end btActionAnalysis_NestingFacet_Click


        /* Descripción:
         *  Muestra la ventana para seleccionar aquellos diseños de facetas que queramos eliminar de 
         *  nuestro diseño. Este llama a su vez al método btActionEditSumOfSquaresOnAnalisys_Click de la 
         *  clase parcial AnalysisOptions.cs
         */
        private void btActionAnalysis_RemoveNesting_Click()
        {
            if (this.listFacetsAnalysis != null)
            {
                bool nesting = false;
                int n = this.listFacetsAnalysis.Count();
                for (int i = 0; i < n && !nesting; i++)
                {
                    Facet f = this.listFacetsAnalysis.FacetInPos(i);
                    nesting = f.IsNesting();
                }

                if (nesting)
                {
                    List<string> lfSelectFacet = new List<string>();
                    n = this.listFacetsAnalysis.Count();
                    for (int i = 0; i < n; i++)
                    {
                        Facet f = this.listFacetsAnalysis.FacetInPos(i);
                        if (f.IsNesting())
                        {
                            string aux = f.ListFacetDesing();
                            int m = aux.Count();
                            lfSelectFacet.Add(aux);
                        }
                    }

                    FormRemoveNesting fRemoveNesting = new FormRemoveNesting(this.LanguageActually(), lfSelectFacet);

                    bool salir = false;
                    do
                    {
                        DialogResult res = fRemoveNesting.ShowDialog();
                        switch (res)
                        {
                            case DialogResult.Cancel:
                                salir = true;

                                break;
                            case DialogResult.OK:
                                CheckedListBox ckListBox = fRemoveNesting.CheckedListBoxSelectNestingRemove();
                                foreach (int indexChecked in ckListBox.CheckedIndices)
                                {
                                    string desing = lfSelectFacet[indexChecked];
                                    int pos = desing.IndexOf("]") - 1;
                                    string name = desing.Substring(1, pos);
                                    Facet f_nesting = this.listFacetsAnalysis.LookingFacet(name);
                                    f_nesting.ListFacetsDesignRemove();
                                    // Pintamos de nuevo la tabla
                                    LoadListFacetInDataGridView(this.listFacetsAnalysis, dGridViewExAnalysis_TableFacet);
                                    salir = true;
                                }

                                break;
                        }
                    } while (!salir);
                }
                else
                {
                    // avisamos al usuario de que no puede eliminar ningún anidamiento porque no existen.
                    ShowMessageErrorOK(errorNoNesting);
                }
            }
            else
            {
                // avisamos al usuario de que no puede eliminar ningún anidamiento porque no existen.
                ShowMessageErrorOK(errorNoNesting);
            }
        }// end btActionAnalysis_RemoveNesting_Click


        /* Descripción:
         *  Lee los datatos de las facetas de la tabla de análisis y muestra la tabla de suma de 
         *  cuadrados para que estos puedan ser introducidos por el usuario.
         */
        private void btActionEditSumOfSquaresOnAnalisys_Click()
        {
            // Leemos la lista de facetas del dataGridView
            if (this.listFacetsAnalysis == null)
            {
                this.listFacetsAnalysis = dgvExToListFacets(this.dGridViewExAnalysis_TableFacet);
            }
            if (listFacetsAnalysis != null && listFacetsAnalysis.Count() > 1)
            {/* 1 */
                /* Si los datos son correctos continuamos las comprobaciones.
                 * Si es necesario realizamos el anidamiento total de las facetas */
                if (this.provision.Equals(ProvisionOfFacets.Nested))
                {
                    // Hacemos un anidamiento de las facetas
                    this.listFacetsAnalysis.NestingAllFacet();
                }


                /* Si estamos en el modo edición entonces introducimos la suma de cuadrados
                 * permitir modificarla.
                 */
                if (this.editionModeOn && false)
                {
                    Aux_EditSsqValues(this.anl_tAnalysis_G_study_opt.TableAnalysisVariance().ListFacets());
                }
                else
                {// (* 2 *)
                    // Si no estamos en modo edición significa que estamos introduciendo los valores por primera vez
                    // Pedimos al usuario que inserte el diseño de medida
                    this.analysisSourceOfVarDiff = new ListFacets();
                    this.analysisSourceOfVarInst = new ListFacets();

                    foreach (Facet f in listFacetsAnalysis)
                    {
                        analysisSourceOfVarInst.Add(f);
                    }

                    // Creamos la ventana para introducir el diseño de medida
                    FormMeasurDesign formMeasurDesign = new FormMeasurDesign(analysisSourceOfVarDiff, analysisSourceOfVarInst, cfgApli.GetConfigLanguage());
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

                                    // MessageBox.Show(errorM_DesignNoValidate, "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                }// end if (* 2 *)
                

            }// end if (* 1 *)
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

            // Optenemos la lista de combinaciones sin repetición de la lista de facetas
            this.llFacetsAnalysis = listFacetsAnalysis.CombinationStringWithoutRepetition();
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
            TableAnalysisOfVariance tb = this.anl_tAnalysis_G_study_opt.TableAnalysisVariance();
            List<string> lKeys = tb.SourcesOfVar();
            TableG_Study_Percent tableG = this.anl_tAnalysis_G_study_opt.TableG_Study_Percent();
            // Mostramos el diseño de medida en los textBox
            ShowMeDesignInAnalysisTextBoxs(tableG.LfDifferentiation(), tableG.LfInstrumentation());
            /* Insertamos las facetas en la tabla de facetas de la pestaña de edición se 
             * suma de cuadrados */
            LoadListFacetInDataGridView(listFacetsAnalysis, dgvExAnalysis_FacetEditSsq);
            // Abilitamos el modo edición
            enableEditingSSqAnalysis();

            // limpiamos el dataGridViewEx de las facetas
            this.dgvExAnalysisEditSSq.NumeroFilas = 0;
            this.dgvExAnalysisEditSSq.Rows.Clear();
            
            // Optenemos la lista de combinaciones sin repetición de la lista de facetas
            // this.llFacetsAnalysis = listFacetsAnalysis.CombinationStringWithoutRepetition();
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
                my_Row[1] = tb.SSQ(key);;

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
            /* Si estamos en el modo edición entonces la lista de fuentes de variación será la 
             * perteneciente a la tabla de análisis faceta de análisis será
             */
            if (this.editionModeOn && this.anl_tAnalysis_G_study_opt!=null)
            {
                listFacetsAnalysis = this.anl_tAnalysis_G_study_opt.TableAnalysisVariance().ListFacets();
                llFacetsAnalysis = this.anl_tAnalysis_G_study_opt.TableAnalysisVariance().SourcesOfVar();
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
                    if (my_Row.Cells[1].Value != null)
                    {
                        string s = my_Row.Cells[1].Value.ToString();
                        // double d = double.Parse(s);
                        double d = (double)ConvertNum.String2Double(s);
                        ssq.Add(llFacetsAnalysis[i], d);
                    }
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

                /* La suma de cuadrados se ha leido correctamente. abrimos una ventana de dialogo para
                 * preguntar al usuario donde desea almacenar los datos.
                 */
                SaveFileDialog saveDialog = new SaveFileDialog();

                if (Directory.Exists(this.cfgApli.Get_Path_Workspace()))
                {
                    saveDialog.InitialDirectory = this.cfgApli.Get_Path_Workspace();
                }

                saveDialog.DefaultExt = "anls";
                string fileAnalysisFilter = "Analysis file" + FILTER_ANALYSIS_FILTER;
                saveDialog.Filter = fileAnalysisFilter;
                saveDialog.OverwritePrompt = true; // muestra advertencia si el fichero ya existe
                saveDialog.AddExtension = true;
                DialogResult res = saveDialog.ShowDialog();
                switch (res)
                {
                    case(DialogResult.Cancel):
                        break;
                    case(DialogResult.OK):
                        // almacenamos el fichero

                        if (this.editionModeOn && this.anl_tAnalysis_G_study_opt!=null)
                        {
                            // Si estamos en el modo edición entonces calculamos
                            this.anl_tAnalysis_G_study_opt = this.anl_tAnalysis_G_study_opt.UpdateSsq(ssq);
                        }
                        else
                        {
                            // Creamos el objeto tabla de análisis varianza
                            TableAnalysisOfVariance tbAnalysisVar = new TableAnalysisOfVariance(listFacetsAnalysis, ssq);

                            // Creamos el objeto de tabla de G-Parámetros
                            TableG_Study_Percent gp = new TableG_Study_Percent(analysisSourceOfVarDiff, analysisSourceOfVarInst, tbAnalysisVar);

                            // Cargamos los datos en los textBox
                            string mDesing = tbAnalysisMdesign.Text;
                            showMeDesignInAllAnalysisTextBoxs(mDesing);

                            // Actualizamos la variable global de análisis con los nuevos valores
                            this.anl_tAnalysis_G_study_opt = new Analysis_and_G_Study(tbAnalysisVar, gp);
                        }

                        DateTime date = DateTime.Now;
                        this.anl_tAnalysis_G_study_opt.SetDateTime(date);
                        this.anl_tAnalysis_G_study_opt.SetNameFileDataCreation(saveDialog.FileName);
                        // guardamos el fichero
                        this.anl_tAnalysis_G_study_opt.WritingFileAnalysisSSQ(saveDialog.FileName);
                        // Mostramos los datos
                        LoadAllDataGridWithDataAnalysis(this.anl_tAnalysis_G_study_opt,saveDialog.FileName.ToString());

                        // Ocultamos el tabPage de editar suma de cuadrados y mostramos los restantes
                        disableEditingFacetAnalysis();

                        break;
                }// end switch  
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
            ShowMeDesignInAnalysisTextBoxs(list_diff,list_inst);
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
            if (this.anl_tAnalysis_G_study_opt != null)
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

                string fileFilter = ("Analysis" + FILTER_ANALYSIS_FILTER + "|" + this.allFiles + FILTER_ALL_FILE);
                openDialog.Filter = fileFilter;

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadAnalysisFile(openDialog.FileName);
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
                if (this.anl_tAnalysis_G_study_opt != null)
                {
                    // Limpiamos todas las tablas
                    cleanerAllTabPageAnalysis();
                }
                this.anl_tAnalysis_G_study_opt = tb_aux;
                // Cargamos los datos en los datagridView
                // LoadAllDataInDataGridViewEx_SSQOptions();
                LoadAllDataGridWithDataAnalysis(this.anl_tAnalysis_G_study_opt, path);
            }
            catch (Analysis_and_G_Study_Exception e)
            {
                // Mostramos información del error
                // MessageBox.Show(errorReadingFile + ". " + e.Message, titleMessageError1);
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
         *  Se ejecuta cuando se pulsa sobre Guardar en el menú vertical de Análisis. Muestra el cuadro
         *  de dialogo para seleccionar el archivo en el que se va a guardar las tablas de análisis.
         */
        private void tsmiActionAnalysis_Save_Click()
        {
            // DialogResult res = DialogResult.OK;
            // TableAnalysisOfVariance tb = this.tAnalysis_tG_Study_Opt.TableAnalysisVariance();
            if (this.anl_tAnalysis_G_study_opt == null)
            {
                // si no tenemos lista de medias lanzamos un mensaje de error
                ShowMessageErrorOK(errorNoSSQ);
            }
            else
            {
                // Preguntamos al usuario por el archivo
                SaveFileDialog saveDialog = new SaveFileDialog();

                if (Directory.Exists(this.cfgApli.Get_Path_Workspace()))
                {
                    saveDialog.InitialDirectory = this.cfgApli.Get_Path_Workspace();
                }

                saveDialog.DefaultExt = "anls";
                string fileAnalysisFilter = "Analysis file" + FILTER_ANALYSIS_FILTER;
                saveDialog.Filter = fileAnalysisFilter;
                saveDialog.OverwritePrompt = true; // muestra advertencia si el fichero ya existe
                saveDialog.AddExtension = true;
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    this.anl_tAnalysis_G_study_opt.WritingFileAnalysisSSQ(saveDialog.FileName);
                    // Mostramos un mensaje indicando que se ha salvado
                    // MessageBox.Show(txtTheDataIsSaved, titleSaved);
                    ShowMessageInfo(txtTheDataIsSaved, titleSaved);
                }
            }
        }


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
                    AddColunmToDGVOptimization(listFacets,newG_ParametersOpt, this.dgvAnalysisResumOpt);
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
                        "/" + analysisSourceOfVarInst.StringOfListFactes();
            tbAnalysisMdesign.Text = mDesing;
            // Resto de textBox
            tbAnalysisMeasurDesignSsq.Text = mDesing;
            tbAnalysisMeasDesignG_P.Text = mDesing;
            tbAnalysisMesurDesignOpt.Text = mDesing;
        }


        /* Descripción:
         *  Copia el diseño de medida previamente calculado y almacenado en el textBox de edición de 
         *  suma de cuadrados y lo copia al resto de textBox de los otros tabPage de Análisis. Esto
         *  debe hacerse cuando se ha concluido afirmativamente la edición de suma de cuadrados.
         */
        private void showMeDesignInAllAnalysisTextBoxs(string mDesign)
        {
            //string mDesing = tbAnalysisMdesign.Text;
            tbAnalysisMeasurDesignSsq.Text = mDesign;
            tbAnalysisMeasDesignG_P.Text = mDesign;
            tbAnalysisMesurDesignOpt.Text = mDesign;
        }


        /*
         * Descripción:
         *  Carga los valores totales de los G-Parmeters en las etiquetas de total suma de fuentes 
         *  objetivo, total varianza de error relativo y total varianza de error absoluto. Los datos
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
            this.anl_tAnalysis_G_study_opt_Old = null;
            disableEditingFacetAnalysis();
        }


        /* Descripción:
         *  Restaura el estado de los elementos necesarios tras la edición de las facetas de la Oción Ananlisis.
         */
        private void disableEditingFacetAnalysis()
        {
            // Deshabilitamos el menú principal poniendo la variable booleana editionModeOn a true.
            this.editionModeOn = false;
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
            disableEditingFacetAnalysis();
            // Ponemos el modo edición a false para poder usar el menú principal
            this.editionModeOn = false;
            // Activamos el menú de acciones de Análisis
            this.mStripAnalysis.Enabled = true;
            // limpiamos todos los campos de Análisis
            cleanerAllTabPageAnalysis();
            // Restauramos el valor anterior si lo había
            if (this.anl_tAnalysis_G_study_opt_Old != null)
            {
                this.anl_tAnalysis_G_study_opt = this.anl_tAnalysis_G_study_opt_Old;
                // Cargamos los valores antiguos
                string nameFile = this.anl_tAnalysis_G_study_opt.GetNameFileDataCreation();
                LoadAllDataGridWithDataAnalysis(this.anl_tAnalysis_G_study_opt, nameFile);
                // Ponemos el valor antiguo a null para ahorra memoria
                this.anl_tAnalysis_G_study_opt_Old = null;
            }
        }


        /* Descripción:
         *  Limpia todos los tabPage de la opción de Análisis
         */
        private void cleanerAllTabPageAnalysis()
        {
            string mDesign = "";
            // Limpiamos los TextBox de diseño de medida
            showMeDesignInAllAnalysisTextBoxs(mDesign);

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
            this.anl_tAnalysis_G_study_opt = null;
        }// end cleanerAllTabPageAnalysis()


        /* Descripción:
         *  Cierrar los elementos abiertos e inicializa los registros textBox y DataGridViewEx.
         */
        private void tsmiActionCloseAnalysis_Click(object sender, EventArgs e)
        {
            if (this.anl_tAnalysis_G_study_opt!=null)
            {
                TableAnalysisOfVariance tableAnalysis = this.anl_tAnalysis_G_study_opt.TableAnalysisVariance();
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
            //// Ventana de carga
            //CWait fw = new CWait(msgLoading);
            //Thread th = new Thread(new ThreadStart(fw.CWaitShowDialog));
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
                                this.importAnalysis_SSqFile(formSSQ_Import.pathFile());
                                // th.Abort();
                                salir = true;
                            }
                            break;
                    }
                } while (!salir);
            }
            catch (IOException)
            {
                // th.Abort();
                // Mostramos un mensage indicando que el fichero esta siendo usado
                // MessageBox.Show(errorFileInUse);
                ShowMessageErrorOK(errorFileInUse);
            }
            catch (Exception ex)
            {
                // th.Abort();
                // Mostramos un mensage indicando que el fichero no esta en formato correcto.
                // MessageBox.Show(errorFormatFile);
                ShowMessageErrorOK("Error tsmiActionImportAnalysis_Click():" + ex.Message);
            }
        }// end tsmiActionImportAnalysis_Click


        /* Descripción:
         *  Importa un fichero de suma de cuadrados para construir el objeto listTableSSQ.
         */
        public void importAnalysis_SSqFile(string path)
        {
            // Ventana de carga
            CWait fw = new CWait(msgLoading);
            Thread th = new Thread(new ThreadStart(fw.CWaitShowDialog));
            CWait fw2 = new CWait(msgLoading);
            Thread th2 = new Thread(new ThreadStart(fw2.CWaitShowDialog));

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
                        th.Start();
                        tAnalysis_tG_Study_Opt = Aux_loadListTableSSqOfFileSsq(path);
                        th.Abort();
                        break;
                    case (TypeOfFile.rsa):
                        th.Start();
                        tAnalysis_tG_Study_Opt = Aux_loadListTableSSqOfFileRsa(path);
                        th.Abort();
                        break;
                    case (TypeOfFile.txt):
                        th.Start();
                        List<AnalysisSsqEduG> listAnalysisEduG = AnalysisSsqEduG.ReadFileReportTxtEduG(path);
                        th.Abort();
                        tAnalysis_tG_Study_Opt = Aux_SelectAnalysisOfListAnalyisReports(listAnalysisEduG);
                        break;
                    case (TypeOfFile.rtf):
                        th.Start();
                        List<AnalysisSsqEduG> listAnalysisEduG2 = AnalysisSsqEduG.ReadFileReportRtfEduG(path);
                        th.Abort();
                        tAnalysis_tG_Study_Opt = Aux_SelectAnalysisOfListAnalyisReports(listAnalysisEduG2);
                        break;
                    case (TypeOfFile.xls):
                        // Ficheros xls de excel
                        th.Start();
                        tAnalysis_tG_Study_Opt = Aux_loadListTableSSqOfFileXls(path);
                        th.Abort();
                        break;
                    default:
                        ShowMessageErrorOK("No se muestra ninguno");
                        // MessageBox.Show("No se muestra ninguno");
                        break;
                }
                if (tAnalysis_tG_Study_Opt != null)
                {
                    th2.Start();
                    DateTime date = DateTime.Now;
                    this.anl_tAnalysis_G_study_opt = tAnalysis_tG_Study_Opt;
                    this.anl_tAnalysis_G_study_opt.SetDateTime(date);
                    this.anl_tAnalysis_G_study_opt.SetNameFileDataCreation(path);
                    LoadAllDataGridWithDataAnalysis(this.anl_tAnalysis_G_study_opt, path);
                    th2.Abort();
                }
            }
            catch (SSqPY_Exception)
            {
                th.Abort();
                th2.Abort();
                // Se producjo un error al leer el archivo
                ShowMessageErrorOK(errorFormatFile);
            }
        }// end importAnalysis_SSqFile


        /* Descripción:
         *  Generar un archivo Excel a partir de los datos contenidos en las tablas de Análisis.
         */
        private void tsmiActionAnalysisExportExcel_Click(object sender, EventArgs e)
        {
            if (this.anl_tAnalysis_G_study_opt == null)
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
                    expExcel.addXlsWorksheet(dgvAnalysisResumOpt, tabPageOptimization.Text);
                    expExcel.addNewXlsWorksheet(dgvExAnalysis_GP, tabPageG_Parameters.Text);
                    expExcel.addNewXlsWorksheet(dgvExAnalysisSSq, tabPageSSQ_TableComp.Text);
                    expExcel.addNewXlsWorksheet(dgvExAnalysisSourceOfVarSsq, tabPagMultiFacet.Text);

                    expExcel.saveFileExcel(saveDialog.FileName);


                    // MessageBox.Show("Fin");
                    saveDialog.Dispose();
                    saveDialog = null;
                    expExcel.aplicationExcelQuit();
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
                                if (this.anl_tAnalysis_G_study_opt != null)
                                {
                                    TableAnalysisOfVariance tableAnalysis = this.anl_tAnalysis_G_study_opt.TableAnalysisVariance();
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
                        // MessageBox.Show(message);
                        ShowMessageInfo(message);

                    }
                }
                catch (MultiFacetObsException)
                {
                    // Mostramos un mensaje de error informando de que no se han podido extraer los datos
                    // MessageBox.Show(errorReadingFileScore, titleMessageError1);
                    ShowMessageErrorOK(errorReadingFileScore, titleMessageError1, MessageBoxIcon.Error);
                }
            }
        }// end btActionsImportScores_Click


        /* Descripción:
         *  Toma el objeto de Tabla de análisis de la opción suma de cuadrados y realiza una copia en 
         *  profundidad que luego se mostrará en la opción de análisis.
         */
        private void tsmiActionSSqToAnalysis_Click()
        {
            Analysis_and_G_Study tAnalysis_tG_Study_Opt = this.sagtElements.GetAnalysis_and_G_Study();
            if (tAnalysis_tG_Study_Opt == null)
            {
                // si no tenemos lista de medias lanzamos un mensaje de error
                ShowMessageErrorOK(errorNoSSQ);
            }
            else
            {
                bool bCreateAnalysis = true;
                // Comprobamos si hay un objeto abierto en la opción de analisis
                if (this.anl_tAnalysis_G_study_opt != null)
                {
                    // informamos y preguntamos confirmación
                    DialogResult res2 = ShowMessageDialog(titleConfirm, txtConfirmClearMeans);

                    if (res2 == DialogResult.Cancel)
                    {
                        bCreateAnalysis = false;
                    }
                }
                // Si es true copiamos y mostramos
                if (bCreateAnalysis)
                {
                    this.anl_tAnalysis_G_study_opt = this.sagtElements.CopyTablesOfAnalysis();
                    string nameFile = this.anl_tAnalysis_G_study_opt.GetNameFileDataCreation();
                    this.anl_tAnalysis_G_study_opt.SetNameFileDataCreation(nameFile);
                    LoadAllDataGridWithDataAnalysis(anl_tAnalysis_G_study_opt, nameFile);
                    // Asignamos la pestaña
                    ExcludeTabPages();
                    this.tabPageAnalysis.Parent = this.tabControlOptions;                    
                    // Restauramos los colores
                    this.RestoreColorMenu(this.mStripMain);
                    // Asignamos el nuevo color
                    this.tsmiAnalysis.BackColor = System.Drawing.SystemColors.Highlight;
                }
            }
        }// end tsmiActionSSqToAnalysis_Click



        /* Descripción:
         *  Edita las facetas de las tablas de sumas de cuadrados para la opción análisis.
         */
        private void tsmiActionAnalysisEditFacets_Click()
        {
            if (this.anl_tAnalysis_G_study_opt == null)
            {//begin if (*1*)
                ShowMessageErrorOK(errorNoSSQ);
                // MessageBox.Show(errorNoSSQ);
            }
            else
            {
                this.editionModeOn = true; // Ponemos el modo edición a true
                this.mStripAnalysis.Enabled = false; // Inhabilitamos el menú vertical de Análisis
                
                // Ocultamos las pestañas
                foreach (TabPage tabPage in this.tabControlAnalysisSSQ.TabPages)
                {
                    tabPage.Parent = null;
                }

                this.anl_tAnalysis_G_study_opt_Old = this.anl_tAnalysis_G_study_opt; // Guardamos la tabla de análisis actual por si deshacemos los cambios
                
                //Cargamos los datos de las facetas en el dataGrid
                CleanerDataGridViewExFacets(dGridViewExAnalysis_TableFacet); // limpiamos el datagrid de facetas
                ListFacets lf = this.anl_tAnalysis_G_study_opt.GetListFacets(); // retomamos la lista de facetas
                LoadListFacetInDataGridView(lf, dGridViewExAnalysis_TableFacet, false, true); // cargamos la lista de facetas en el datagrid
                // Permitimos la edición de las columnas
                int nCol = dGridViewExAnalysis_TableFacet.ColumnCount;
                for (int i = 0; i < nCol; i++)
                {
                    dGridViewExAnalysis_TableFacet.Columns[i].ReadOnly = false;
                }

                dGridViewExAnalysis_TableFacet.Columns[1].ReadOnly = true; // columna de niveles
                dGridViewExAnalysis_TableFacet.Columns[2].ReadOnly = true; // columna de tamaño del universo

                // Mostramos los botones de aceptar y cancelar y ocultamos el resto.
                enableAnalysisButtonsEditFacets();
                // Mostramos solo la pestaña de facetas
                this.tabPageAnalysisFacetas.Parent = this.tabControlAnalysisSSQ;
            }// end if
        }// end tsmiActionAnalysisEditFacets_Click


        /* Descripción:
         *  Lanza la operación cuando se aceptan los cambios introducidos en la edición de facetas.
         *  Este llama a su vez al método btActionAcept_Click de la 
         *  clase parcial AnalysisOptions.cs
         */
        private void btActionAcept_Click()
        {
            string nameFileDataCreation = this.anl_tAnalysis_G_study_opt.GetNameFileDataCreation();
            DateTime dateCreation = this.anl_tAnalysis_G_study_opt.GetDateTime();

            try
            {
                // Leer las facetas;
                ListFacets newLf = dgvExToListFacets(this.dGridViewExAnalysis_TableFacet);

                // Actualizar la lista actual con los valores de la nueva
                ListFacets oldLf = this.anl_tAnalysis_G_study_opt.TableAnalysisVariance().ListFacets();
                newLf = oldLf.RemplaceListFacets(newLf);

                // generar la tabla de análisis partiendo de la suma de cuadrados anterior
                // actualizar los valores de optimización
                this.anl_tAnalysis_G_study_opt = this.anl_tAnalysis_G_study_opt.ReplaceListOfFacet(newLf);
                //********************************************************************************
                string namePrueba = this.anl_tAnalysis_G_study_opt_Old.GetNameFileDataCreation();
                    //********************************************************************************
                this.anl_tAnalysis_G_study_opt.SetNameFileDataCreation(this.anl_tAnalysis_G_study_opt_Old.GetNameFileDataCreation());
                // this.anl_tAnalysis_G_study_opt.SetNameFileDataCreation(nameFileDataCreation);
                this.anl_tAnalysis_G_study_opt.SetDateTime(this.anl_tAnalysis_G_study_opt_Old.GetDateTime());
                //this.anl_tAnalysis_G_study_opt.SetDateTime(dateCreation);
                this.anl_tAnalysis_G_study_opt_Old = null;

                // cargar los valores nuevos.
                LoadAllDataGridWithDataAnalysis(this.anl_tAnalysis_G_study_opt, this.anl_tAnalysis_G_study_opt.GetNameFileDataCreation());

                // Restauramos las pestañas y salimos del modo edición
                disableEditingFacetAnalysis();
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
            if (this.anl_tAnalysis_G_study_opt == null)
            {//begin if (*1*)
                ShowMessageErrorOK(errorNoSSQ);
                // MessageBox.Show(errorNoSSQ);
            }
            else
            {
                // this.editionModeOn = true;
                // mStripData.Enabled = false;// desactivamos el menu
                enableEditingSSqAnalysis();
                /* Mostramos el tabPage de edición con las suma de cuadrados editables.
                 * Soló la suma de cuadrados no las fuentes de variación.
                 */
                // asignamos para tener la posibilidad de deshacer los cambios.
                this.anl_tAnalysis_G_study_opt_Old = this.anl_tAnalysis_G_study_opt;
                // Ocultamos los tabPage
                // Ocultamos las pestañas
                foreach (TabPage tabPage in this.tabControlAnalysisSSQ.TabPages)
                {
                    tabPage.Parent = null;
                }
                Aux_EditSsqValues(this.anl_tAnalysis_G_study_opt.TableAnalysisVariance().ListFacets());
            }
        }// end tsmiActionAnalysisEditSsq_Click


        /* Descripción:
         *  Permite cambiar el diseño de medida de un modelo.
         */
        private void tsmiActionChangeModel_Click()
        {
            if (this.anl_tAnalysis_G_study_opt == null)
            {//begin if (*1*)
                ShowMessageErrorOK(errorNoSSQ);
                // MessageBox.Show(errorNoSSQ);
            }
            else
            {
                // Avisamos de que se perdera información si no se ha guardado
                // Lista de facetas de Instrumentación y diferenciación
                analysisSourceOfVarDiff = this.anl_tAnalysis_G_study_opt.TableG_Study_Percent().LfDifferentiation();
                analysisSourceOfVarInst = this.anl_tAnalysis_G_study_opt.TableG_Study_Percent().LfInstrumentation();
                // mostramos la ventana de selección de diseño
                // Creamos la ventana para introducir el diseño de medida
                FormMeasurDesign formMeasurDesign = new FormMeasurDesign(analysisSourceOfVarDiff, analysisSourceOfVarInst, cfgApli.GetConfigLanguage());
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
                                // MessageBox.Show(errorM_DesignNoValidate, "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                ShowMessageErrorOK(errorM_DesignNoValidate, "", MessageBoxIcon.Stop);
                            }
                            else
                            {
                                // Obtenemos la lista de facetas de instrumentación y de diferenciación.
                                // analysisSourceOfVarDiff;
                                // analysisSourceOfVarInst;
                                // si se ha seleccionado aceptar aplicamos los cambios
                                string nameFile = this.anl_tAnalysis_G_study_opt.GetNameFileDataCreation();
                                TableAnalysisOfVariance tbAnalysisOfVar = this.anl_tAnalysis_G_study_opt.TableAnalysisVariance();
                                TableG_Study_Percent tb_G_study_percent = new TableG_Study_Percent(analysisSourceOfVarDiff, analysisSourceOfVarInst, tbAnalysisOfVar);
                                this.anl_tAnalysis_G_study_opt = new Analysis_and_G_Study(tbAnalysisOfVar, tb_G_study_percent);
                                this.anl_tAnalysis_G_study_opt.SetNameFileDataCreation(nameFile);
                                LoadAllDataGridWithDataAnalysis(this.anl_tAnalysis_G_study_opt, nameFile);
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
                // Botón aceptar edición de facetas
                name = this.btAcept.Name.ToString();
                this.btAcept.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Botón cancelar edición de facetas
                name = this.btCancelEditFacetOnAnalysis.Name.ToString();
                this.btCancelEditFacetOnAnalysis.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Botón cancelar de tabPage de edición de suma de cuadrados
                this.btCancelEditSsq.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Botón Guardar de tabPage de edición de suma de cuadrados
                name = this.btSaveAnalysisSsq.Name.ToString();
                this.btSaveAnalysisSsq.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Botón Importar suma de cuadrados del tabPage edición de suma de cuadrados
                name = this.btImportAnalysisEditSsq.Name.ToString();
                this.btImportAnalysisEditSsq.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Botón Editar del tabPage Información
                name = this.btAnalysisEditComment.Name.ToString();
                this.btAnalysisEditComment.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Etiquetas del tabPageInformación
                name = this.lbFileAnalysisProvede.Name.ToString();
                this.lbFileAnalysisProvede.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbDateAnalysisCreated.Name.ToString();
                this.lbDateAnalysisCreated.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Etiqueta de diseño de medida
                name = lbAnalysisMeasurDesignSsq.Name.ToString();
                lbAnalysisMeasurDesignSsq.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                lbAnalysisMeasDesignG_P.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                lbAnalysisMesurDesignOpt.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                lbAnalysis_M_Desing_EditSsq.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

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
                    dgvExAnalysis_GP.Columns[4].HeaderText = abs_err_var;
                }


                // Actuamos sobre dGridViewExFacetsOptimization
                if (dgvExAnalysisFacetsOpt.ColumnCount != 0)
                {
                    dgvExAnalysisFacetsOpt.Columns[IND_NAME].HeaderText = nameColFacet; // Nombre de la columna Etiquetas (dependerá del idioma).
                    dgvExAnalysisFacetsOpt.Columns[IND_LEVEL].HeaderText = nameColLevel; // Nombre de la columna Niveles (dependerá del idioma).
                    dgvExAnalysisFacetsOpt.Columns[IND_LEVELS_PROCESS].HeaderText = levelsProcess;
                    dgvExAnalysisFacetsOpt.Columns[IND_SIZE_OF_UNIVERSE].HeaderText = this.nameColSizeOfUniverse;
                }


                // Actuamos sobre el dGridViewExOptimizationResum
                if (this.dgvAnalysisResumOpt.ColumnCount != 0)
                {
                    // dGridViewExOptimizationResum.Columns[0].HeaderText = name_resum;
                    // Entonces pintamos la tabla de resumen de nuevo
                    LoadDataGridViewExOptimizationResum(this.anl_tAnalysis_G_study_opt, 
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
                // MessageBox.Show(lEx.Message + " " + errorMessageTraslation + " " + name);
                ShowMessageErrorOK(lEx.Message + " " + errorMessageTraslation + " " + name);
            }
        } // private void TraslationAnalysisElements

        #endregion Cambio de idioma de los elementos del tabPageAnalysis


    }// public partial class FormPrincipal : Form
}// namespace GUI_TG