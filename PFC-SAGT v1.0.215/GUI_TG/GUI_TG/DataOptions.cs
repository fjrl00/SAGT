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
 * Fecha de revisión: 13/Dic/2012                           
 * 
 * Descripción:
 *      Clase parcial ("partial") del FormPrincipal. Contiene los métodos referentes a la parte de
 *      Datos: creación de Facetas y tabla de observaciones.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MultiFacetData;
using AuxMathCalcGT;
using ProjectSSQ;
using ProjectMeans;
using System.Drawing; // se usa para las propiedades de la cabecera de columna (color,fuente,etc)
using System.Threading; // permite usar hilos
using System.Globalization; // permite leer doubles con independencia del punto decimal que usemos
using myExcel = Microsoft.Office.Interop.Excel; // permite interactuar con ficheros Excel
using Sagt;
using System.IO;


namespace GUI_GT
{
    public partial class FormPrincipal : Form
    {
        #region Variables relaccionadas con la opción de datos

        // CONSTANTES
        // ==========
        const string NAME_COL_OMIT = "nameColOmit";
        const string FILTER_DATA = " (*.dat)|*.dat|"; // Filtro del fichero de datos
        const string FILTER_SSQ_EDUG = " (*.edug)|*.edug|"; // Filtro de fichero de suma de cuadrados EduG
        const string FILTER_SAGT_FILE = " (*.sagt)|*.sagt|";
        const string FILTER_GT_OBS_FILE = " (*.obs)|*.obs|";
        const string FILTER_ALL_FILE = " (*.*)|*.*";
        const string FILTER_ANALYSIS_FILTER  = " (*.anls)|*.anls";
        const string FILTER_EXCEL = " (*.xls)|*.xls";
        const string DEFAULT_EXT_SAGT = "sagt";
        const string DEFAULT_EXT_RSM = "rsm";
        const string DEFAULT_EXT_EXCEL = "xls";
        const string DEFAULT_EXT_SCORE = "dat";
        const string DEFAULT_EXT_SSQ_EDUG = "edug";
        const string DEFAULT_EXT_OBS = "obs";

        // Fuente empleada en las tablas
        //==============================
        Font fontCellTable = new Font("Verdana", 8, FontStyle.Regular);

        // Cabeceras de las columnas pertenecientes al tabPage de datos
        //=============================================================

        // Nombres de la columnas de la tabla de facetas
        private string nameColFacet = "Nombres"; // Nombre de la columna Etiquetas (dependerá del idioma).
        private string nameColLevel = "Niveles"; // Nombre de la columna Nivel (dependerá del idioma).
        private string nameColSizeOfUniverse = "Tamaño del universo"; // Nombre de la columna Tamaño del universo.
        private string nameColComment = "Descripción"; // Nombre de la columna Descripción (dependerá del idioma).
        private string nameColOmit = "Omitir"; // Indica si la faceta se omite o no del estudio

        private string measurementVariable = "Variable de medida"; // se corresponde con la última columna de la tabla de observaciones

        // Variable de SAGT que contiene la Tabla de de frecuencias
        private SagtFile sagtElements = new SagtFile();

        // variable pertenecientes al proyecto MultiFacetData
        // MultiFacetsObs multiFacets = null;

        // variable para la inserción/edición de facetas 
        ListFacets lf_global = null;
        // Variable que indica la disposición de la facetas para el objeto multifaceta que se esta creando
        // La disposición de las facetas puede ser: cruzada, anidada o mixta.
        MultiFacetData.ProvisionOfFacets provision;

        // Variable booleana que estará a true cuando se edita un objeto
        bool editMultiFacetObs;

        // CONSTANTES: indices de la tabla Facetas
        const int IND_NAME = 0;     // indice de la columna 'Nombre' de la tabla 'Facetas'
        const int IND_LEVEL = 1;    // indice de la columna 'Nivel' de la tabla 'Facetas'
        const int IND_SIZE_OF_UNIVERSE_FACET = 2; // indice de la columna 'Tamaño del universo' de la tabla 'Facetas'
        const int IND_COMMENT = 3;  // indice de la columna 'Descripción' de la tabla 'Facetas'
        // const int IND_OMIT_FACET = 4; // indice para la omisión de facetas

        // Numero de facetas que va a tener la tabla de facetas debe introducirla el usuario antes de editar la tabla
        // private int numOfFacetForTable;

        #endregion Variables relaccionadas con la opción de datos

        
        /* Descripción:
         *  Inicializa la opción de Datos:
         *  oculta el boton de observaciones y el botón cancelar
         */
        private void InitializeDataOption()
        {
            this.tbDescription.ReadOnly = true;
            this.tbFileName.Enabled = false;
            this.tbFileName.ReadOnly = true;
            this.tabPageDataEditFacets.Parent = null;
            this.checkBoxHideNulls.Enabled = false;
            this.disableButtonsFacets();
            this.disableButtonObsTable();
        }

        
        /* Descripción:
         *  Acción que se ejecuta tra pulsar el bóton Abrir del menú vertical de la opción Datos.
         *  Abre un archivo de datos y lo carga en la tablas del tabPage Data.
         */
        private void tsmiActionOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            if (Directory.Exists(this.cfgApli.Get_Path_Workspace()))
            {
                openDialog.InitialDirectory = this.cfgApli.Get_Path_Workspace();
            }

            string fileFilter = (this.sagtFiles + FILTER_SAGT_FILE + this.gtFiles + FILTER_GT_OBS_FILE + this.allFiles + FILTER_ALL_FILE);
            openDialog.Filter = fileFilter;
            
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                this.checkBoxHideNulls.Enabled = false;
                this.checkBoxHideNulls.Checked = false;
                this.checkBoxHideNulls.Enabled = true;
                // Extraemos el nombre del fichero del path
                string fileExt = fileExtension(openDialog.FileName).ToLower(); // Pasamos a minúsculas la extensión
                // para poder compararla.                     
                switch (fileExt)
                {
                    case (DEFAULT_EXT_SAGT): loadFileSagt(openDialog.FileName); break;
                    case (DEFAULT_EXT_OBS): loadFileObs(openDialog.FileName); break;
                }
                // nos posicionamos en el tabPage de las facetas.
                tabControlData.SelectedIndex = 0;
            }
        }


        /* Descripción:
         *  Muestra la ventana de importación de facetas y tabla de variables observadas.
         */
        private void tsmiActionDataImport_Click(object sender, EventArgs e)
        {
            //CWait fw = new CWait(msgLoading);
            //Thread th = new Thread(new ThreadStart(fw.CWaitShowDialog));
            try
            {
                TransLibrary.Language lang = this.LanguageActually();
                FormDataImport formDataImport = new FormDataImport(lang);

                bool salir = false;

                do
                {
                    DialogResult res = formDataImport.ShowDialog();
                    // quitamos y ponemos el foco en la ventana para que esta se actualize
                    this.Enabled = false;
                    this.Enabled = true;

                    switch (res)
                    {
                        case DialogResult.Cancel: salir = true; break;
                        case DialogResult.OK:
                            if (String.IsNullOrEmpty(formDataImport.pathFile()))
                            {
                                // lanzamos un mensaje de error: no hay fichero seleccionado
                                this.ShowMessageErrorOK(errorNoFileSelected);
                            }
                            else
                            {
                                //CWait fw = new CWait(msgLoading);
                                //Thread th = new Thread(new ThreadStart(fw.CWaitShowDialog));

                                // Start the thread
                                //th.Start();
                                this.importDataFile(formDataImport.pathFile());
                                //th.Abort();
                                salir = true;
                            }
                            break;
                    }
                } while (!salir);
            }
            catch(IOException)
            {
                //th.Abort();
                // Mostramos un mensaje indicando que el fichero esta siendo usado por otro programa
                ShowMessageErrorOK(errorFileInUse);
            }
            catch (Exception ex)
            {
                //th.Abort();
                // Mostramos un mensaje indicando que el fichero no esta en el formato correcto
                // MessageBox.Show(ex.Message);
                ShowMessageErrorOK(ex.Message);
            }
        }// end tsmiActionDataImport_Click


        // Auxiliares:
        // ===========

        /* Descripción:
         *  Carga de un fichero obs (lee el fichero con las facetas con fomato del progama "GT").
         * Parámetros:
         *      string path: ubicación del fichero
         */
        private void loadFileObs(string path)
        {
            // leemos el fichero obs
            MultiFacetPY.MultiFacetPY mfpy = MultiFacetPY.MultiFacetPY.ReadFileObsPY(path);
            
            
            string fileNameData = extractFileNamePath(path);
            // asignamos a la variable global los valores cargados
            // multiFacets = mfpy;
            this.sagtElements.SetMultiFacetsObs(mfpy);
            loadMultiFacets(fileNameData, mfpy);
        }


        /* Descripción:
         *  Carga los datos de las facetas y de las variables observadas a traves de un fichero .rms 
         *  (lee el fichero resultado de medias con fomato del progama "GT").
         * Parámetros:
         *      string path: ubicación del fichero
         * Excepciones:
         *      Cualquier error producido avisará al usuario de que el fichero no esta en el formato
         *      correcto.
         */
        private void loadMultiFacetOfFileRms(string path)
        {
            try
            {
                // this.multiFacets = ProjectMeansPY.ListMeansPY.ReadFileRsmPY(path);
                MultiFacetsObs multiFacets = ListMeansPY.ListMeansPY.ReadFileRsmPY(path);
                this.sagtElements.SetMultiFacetsObs(multiFacets);
                string fileNameData = extractFileNamePath(path);
                // asignamos a la variable global los valores cargados
                loadMultiFacets(fileNameData, multiFacets);
            }
            catch (MultiFacetPY.MultiFacetPYException)
            {
                ShowMessageErrorOK(errorFormatFile);
            }
            catch (MultiFacetObsException)
            {
                ShowMessageErrorOK(errorFormatFile);
            }
        }// end loadMultiFacetOfFileRms


        /* Descripción:
         *  Importa los datos de la tabla de facetas y table de frecuencias de un fichero xls
         */
        private void loadMultiFacetFileXls(string path)
        {
            List<string> namesTables = ImportExcel.GetTableExcel(path);
            if (namesTables.Count != 2)
            {
                // No esta en el formato correcto
                ShowMessageErrorOK(errorFormatFile);
            }
            else
            {
                // la primera tabla debe contener las facetas.
                DataTable dtFacets = ImportExcel.GetDataTableExcel(path, namesTables[0]);
                ListFacets lf = DataTable2ListFacets(dtFacets);
                MultiFacetsObs mfo = new MultiFacetsObs(lf, path, "");
                // La segunda tabla debe contener la tabla de frecuencias
                DataTable dtObsTable = ImportExcel.GetDataTableExcel(path, namesTables[1]);
                InterfaceObsTable obsTb = mfo.ObservationTable();
                obsTb = DataTable2Observation(dtObsTable, obsTb);
                mfo.ObservationTable(obsTb);
                this.sagtElements.SetMultiFacetsObs(mfo);
                loadMultiFacets(path, mfo);
            }
        }//end loadMultiFacetFileXls


        /* Descripción:
         *  Importa los datos de un fichero .csv y carga los datos del objeto multifaceta
         *  obtenido de la impotación.
         */
        private void loadMultiFacetFileCsv(string path)
        {
            try
            {
                MultiFacetsObs multiFacets = ImportCSV.ImportFileCSV_to_MultiFacetsObs(path);
                this.sagtElements.SetMultiFacetsObs(multiFacets);
                string fileNameData = extractFileNamePath(path);
                // asignamos a la variable global los valores cargados
                loadMultiFacets(fileNameData, multiFacets);
            }
            catch (MultiFacetPY.MultiFacetPYException)
            {
                ShowMessageErrorOK(errorFormatFile);
            }
            catch (MultiFacetObsException)
            {
                ShowMessageErrorOK(errorFormatFile);
            }
        }// end loadMultiFacetFileCsv


        /* Descripción:
         *  Importa un fichero de datos/medias para construir el objeto multifaceta.
         */
        private void importDataFile(string path)
        {
            // Extraemos el nombre del fichero del path
            string fileExt = fileExtension(path).ToLower(); // Pasamos a minúsculas la extensión
            // para poder compararla. 
            switch (fileExt)
            {
                case (DEFAULT_EXT_OBS): loadFileObs(path); break;
                case (DEFAULT_EXT_RSM): loadMultiFacetOfFileRms(path); break;
                case (DEFAULT_EXT_EXCEL):loadMultiFacetFileXls(path); break;
                case ("csv"): loadMultiFacetFileCsv(path); break;
                default:
                    // MessageBox.Show("No se muestra ninguno");
                    ShowMessageInfo("No se muestra ninguno");
                    break;
            }
        }


        /* Descripción:
         *  Carga un fichero sagt;
         * Parámetros:
         *      string path: ubicación del fichero.
         * Excepciones:
         *     MultiFacetObsException: si no ha podido crear el objeto correctamente. Avisará
         *          al usuario de que el fichero no esta en el formato correcto.
         */
        private void loadFileSagt(string path)
        {
            string fileNameData = extractFileNamePath(path);
            try
            {
                // MultiFacetsObs multiFacets = MultiFacetsObs.ReadingFileObsData(path);
                sagtElements = SagtFile.ReadingSagtFile(path);
                // Cargamos los elementos
                MultiFacetsObs multiFacets = sagtElements.GetMultiFacetsObs();
                // sagtElements.SetMultiFacetsObs(multiFacets);
                //loadMultiFacets(fileNameData, multiFacets);
                loadSagtElements(fileNameData, sagtElements);
            }
            catch(MultiFacetObsException)
            {
                ShowMessageErrorOK(errorFormatFile);
            }
        }


        /* Descripción:
         *  Carga los datos de sagtElements o deja vacio los campos.
         * Parámetros:
         *      string fileNameData: Nombre del fichero del que se han obtenido los datos.
         *      SagtFile sagtElements: Elementos que se van a cargar.
         */
        private void loadSagtElements(string fileNameData, SagtFile sagtElements)
        {
            // Primero cargamos los elementos del objeto multifaceta
            MultiFacetsObs multiFacets = sagtElements.GetMultiFacetsObs();
            if (multiFacets == null)
            {
                // Limpiamos los campos del tabPage Datos
                clearDataElements();
            }
            else
            {
                // Cargamos los datos de las fuentes de variabilidad y tabla de frecuencias
                loadMultiFacets(fileNameData, multiFacets);
                // nos posicionamos
                ExcludeTabPages();
                // Restauramos los colores
                this.RestoreColorMenu(this.mStripMain);
                // Asignamos el color para la opción del menú datos
                this.tsmiDat.BackColor = System.Drawing.SystemColors.Highlight;
                // seleccionamos el tabPage datos
                tabPageData.Parent = this.tabControlOptions;
            }
            // Cargamos los elementos de la lista de tablas de medias
            ListMeans listMeans = sagtElements.GetListMeans();
            if (listMeans == null)
            {
                // limpiamos el tabPage de medias
                ClearTabPageMeans();
            }
            else
            {
                // Cargamos los datos de las tablas de medias
                // cerramos todos los tabPage
                ClearTabPageMeans();
                listOfTableMeansToTabPageMeans(listMeans);

                ExcludeTabPages();
                // Restauramos los colores
                this.RestoreColorMenu(this.mStripMain);
                // Asignamos el color para la opción del menú datos
                this.tsmiMeans.BackColor = System.Drawing.SystemColors.Highlight;
                // seleccionamos el tabPage datos
                tabPageMeans.Parent = this.tabControlOptions;
            }
            // Cargamos los elementos de tablas de análisis
            Analysis_and_G_Study tAnalysis_tG_Study_Opt = sagtElements.GetAnalysis_and_G_Study();
            if (tAnalysis_tG_Study_Opt == null)
            {
                // Limpiamos los tabPage de sumas de cuadrados
                ClearTabPageSSQ();
            }
            else
            {
                // Cargamos los datos de las tablas de las tablas de análisis de varianza
                LoadAllDataInDataGridViewEx_SSQOptions(tAnalysis_tG_Study_Opt);

                ExcludeTabPages();
                // Restauramos los colores
                this.RestoreColorMenu(this.mStripMain);
                // Asignamos el color para la opción del menú datos
                this.tsmiSSQ.BackColor = System.Drawing.SystemColors.Highlight;
                // seleccionamos el tabPage datos
                tabPageSSQ.Parent = this.tabControlOptions;
            }
        }// end LoadSagtElements



        /* Descripción:
         *  Carga los datos de la variable multiFacets en los componentes de tabPage data.
         * Parámetros:
         *      string fileNameData: nombre del fichero desde el que se cargan los datos.
         */
        private void loadMultiFacets(string fileNameData, MultiFacetsObs multiFacets)
        {
            loadDataInTabPageFacets(fileNameData, multiFacets);
            loadDataInTabPageObsTable(multiFacets);
            this.richTextBoxDataComment.Text = multiFacets.Comment();
            this.checkBoxHideNulls.Enabled = true;
        }


        /* Descripción:
         *  Muestra los datos en las tablas de facetas y en los textbox del TabPage Datos.
         * Parámetros:
         *      string fileName: Nombre del fichero del que se extraen los datos.
         *      MultiFacetsObs multiFacets: Objeto que contiene los datos de las facetas y las variables observadas.
         */
        private void loadDataInTabPageFacets(string fileName, MultiFacetsObs multiFacets)
        {
            tbFileName.Text = fileName;
            tbDescription.Text = multiFacets.DescriptionFile();
            // cargamos los datos en el dataGridView
            // this.LoadListFacetInDataGridView(multiFacets.ListFacets(), this.dataGridViewExFacets);
            this.LoadListFacetInDataGridView(multiFacets.ListFacets(), this.dataGridViewExFacets, true);
        } // end loadDataInTabPageFacets


        /* Decripción:
         *  Carga los datos de una lista de facetas en un dataGridView que se pasa como parámetro.
         *  No se mostrará la columna de facetas omitidas. En lugar de mostrar el nombre de la faceta
         *  se mostrára su diseño.
         *  
         * Parámetros:
         *  ListFacets lf: lista de facetas que se representara en el dataGrid.
         *  DataGridViewEx.DataGridViewEx dgv: DataGrid que mostrará los datos.
         */
        private void LoadListFacetInDataGridView(ListFacets lf, DataGridViewEx.DataGridViewEx dgv)
        {
            LoadListFacetInDataGridView(lf, dgv, false);

        }// end LoadListFacetInDataGridView


        /* Decripción:
         *  Carga los datos de una lista de facetas en un dataGridView que se pasa como parámetro.
         *  En lugar de mostrar el nombre de la faceta se mostrára su diseño.
         *  
         * Parámetros:
         *  ListFacets lf: lista de facetas que se representara en el dataGrid.
         *  DataGridViewEx.DataGridViewEx dgv: DataGrid que mostrará los datos.
         *  bool columnOmit: Si es true se mostrará la columna de facetas omitidas.
         */
        private void LoadListFacetInDataGridView(ListFacets lf, DataGridViewEx.DataGridViewEx dgv,
            bool columnOmit)
        {
            LoadListFacetInDataGridView(lf, dgv, columnOmit, false);
        }


        /* Decripción:
         *  Carga los datos de una lista de facetas mostrando la columna de omisión de facetas en 
         *  un dataGridView que se pasa como parámetro.
         * Parámetros:
         *      ListFacets lf: Lista de facetas
         *      DataGridViewEx.DataGridViewEx dgv: datagridView donde se meten los los
         *      bool columnOmit: variable booleana que indica si se muestran la columna de omitir facetas
         *      bool viewName: variable booleana, si esta a true mostrará el nombre de la faceta, en otro
         *          caso mostrará el diseño
         */
        private void LoadListFacetInDataGridView(ListFacets lf, DataGridViewEx.DataGridViewEx dgv, 
            bool columnOmit, bool viewName)
        {
            dgv.NumeroFilas = 0;
            dgv.Rows.Clear();
            dgv.ColumnHeadersVisible = true;
            dgv.AllowUserToAddRows = false; // impide que el usuario pueda añadir nuevas filas

            int numCol = 4; // número de columnas 3 (Etiquetas,nivel,descipción)
            //dgv.ColumnCount = numCol; 
            dgv.NumeroColumnas = numCol;
            dgv.NumeroFilas = 0;

            // Set the column header style.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Aqua;
            columnHeaderStyle.Font = new Font("Verdana", 9, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            dgv.DefaultCellStyle.Font = fontCellTable;

            // Primera columna faceta
            dgv.Columns[IND_NAME].Name = "nameColFacet"; // Nombre de la columna Etiquetas (dependerá del idioma).
            dgv.Columns[IND_NAME].HeaderText = nameColFacet;
            // dgv.Columns[IND_NAME].Width = 100;
            dgv.Columns[IND_NAME].MinimumWidth = 100;
            dgv.Columns[IND_NAME].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns[IND_NAME].ReadOnly = true;
            dgv.Columns[IND_NAME].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            // segunda columna nivel
            //dgv.Columns[IND_LEVEL].Name = nameColLevel; // Nombre de la columna Niveles (dependerá del idioma).
            dgv.Columns[IND_LEVEL].HeaderText = nameColLevel;
            // dgv.Columns[IND_LEVEL].Width = 100;
            dgv.Columns[IND_LEVEL].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.ColumnaTipoEntrada(IND_LEVEL, DataGridViewEx.TipoEntrada.SoloNumeros);
            dgv.Columns[IND_LEVEL].ReadOnly = true;

            // tercera columna tamaño del universo
            dgv.Columns[IND_SIZE_OF_UNIVERSE_FACET].Name = "nameColSizeOfUniverse"; // Nombre de la columna Niveles (dependerá del idioma).
            dgv.Columns[IND_SIZE_OF_UNIVERSE_FACET].HeaderText = nameColSizeOfUniverse;
            // dgv.Columns[IND_SIZE_OF_UNIVERSE_FACET].Width = 100;
            dgv.Columns[IND_SIZE_OF_UNIVERSE_FACET].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv.Columns[IND_SIZE_OF_UNIVERSE_FACET].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns[IND_SIZE_OF_UNIVERSE_FACET].ReadOnly = true;

            // Cuarta columna descripción/ comentario
            //dgv.Columns[IND_COMMENT].Name = nameColComment; // Nombre de la columna Descripción (dependerá del idioma).
            dgv.Columns[IND_COMMENT].HeaderText = nameColComment;
            // dgv.Columns[IND_COMMENT].MinimumWidth = 200;
            dgv.Columns[IND_COMMENT].Width = 500;
            dgv.Columns[IND_COMMENT].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns[IND_COMMENT].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns[IND_COMMENT].ReadOnly = true;

            /* Sí es true se  mostrará la columna de checkbox para señalar las facetas que se omitirán en el
             * estudio.
             */
            if(columnOmit)
            {
                DataGridViewCheckBoxColumn d1 = new DataGridViewCheckBoxColumn();
                d1.Name = NAME_COL_OMIT;
                d1.HeaderText = nameColOmit;
                dgv.Columns[IND_COMMENT].Width = 50;
                // dgv.Columns[IND_COMMENT].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgv.Columns.Add(d1);
            }


            int n = lf.Count();
            for (int i = 0; i < n; i++)
            {
                Facet f = lf.FacetInPos(i);
                object[] my_Row = new object[numCol];

                if (viewName)
                {
                    my_Row[IND_NAME] = f.Name();
                }
                else
                {
                    my_Row[IND_NAME] = f.ListFacetDesing();
                }
                
                my_Row[IND_LEVEL] = f.Level();
                int s = f.SizeOfUniverse();
                if (int.MaxValue.Equals(s))
                {
                    my_Row[IND_SIZE_OF_UNIVERSE_FACET] = Facet.INFINITE;

                }
                else
                {
                    my_Row[IND_SIZE_OF_UNIVERSE_FACET] = s;
                }

                my_Row[IND_COMMENT] = f.Comment();
                dgv.Rows.Add(my_Row);

                if(columnOmit)
                {
                    dgv.Rows[i].Cells[NAME_COL_OMIT].Value = f.Omit();
                }
            }

            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            // dgv.ReadOnly = true;
        }// end LoadListFacetInDataGridView


        /*
         * Decripción:
         *  Carga los datos de una lista de facetas en un dataGridView que se pasa como parámetro.
         *  En la columna nombre se mostrará solo su nombre.
         */
        private void LoadListFacetInDataGridViewForEdit(ListFacets lf, DataGridViewEx.DataGridViewEx dgv)
        {
            dgv.Rows.Clear();
            dgv.ColumnHeadersVisible = true;
            dgv.AllowUserToAddRows = false; // impide que el usuario pueda añadir nuevas filas

            int numCol = 4; // número de columnas 3 (Etiquetas,nivel,descipción)
            // dgv.ColumnCount = numCol; 
            dgv.NumeroColumnas = numCol;
            dgv.NumeroFilas = 0;

            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Set the column header style.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Aqua;
            // columnHeaderStyle.Font = new Font("Verdana", 10, FontStyle.Bold);
            columnHeaderStyle.Font = new Font("Verdana", 10, FontStyle.Regular);
            dgv.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            dgv.DefaultCellStyle.Font = fontCellTable;

            // Primera columna faceta
            // dgv.Columns[IND_NAME].Name = nameColFacet; // Nombre de la columna Etiquetas (dependerá del idioma).
            dgv.Columns[IND_NAME].HeaderText = nameColFacet;
            dgv.Columns[IND_NAME].Width = 100;
            dgv.Columns[IND_NAME].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns[IND_NAME].ReadOnly = true;

            // segunda columna nivel
            //dgv.Columns[IND_LEVEL].Name = nameColLevel; // Nombre de la columna Niveles (dependerá del idioma).
            dgv.Columns[IND_LEVEL].HeaderText = nameColLevel;
            dgv.Columns[IND_LEVEL].Width = 100;
            dgv.Columns[IND_LEVEL].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.ColumnaTipoEntrada(IND_LEVEL, DataGridViewEx.TipoEntrada.SoloNumeros);
            dgv.Columns[IND_LEVEL].ReadOnly = true;

            // tercera columna tamaño del universo
            // dgv.Columns[IND_SIZE_OF_UNIVERSE_FACET].Name = nameColSizeOfUniverse; // Nombre de la columna Niveles (dependerá del idioma).
            dgv.Columns[IND_SIZE_OF_UNIVERSE_FACET].HeaderText = nameColSizeOfUniverse;
            dgv.Columns[IND_SIZE_OF_UNIVERSE_FACET].Width = 100;
            dgv.Columns[IND_SIZE_OF_UNIVERSE_FACET].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv.Columns[IND_SIZE_OF_UNIVERSE_FACET].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns[IND_SIZE_OF_UNIVERSE_FACET].ReadOnly = true;

            // Cuarta columna descripción/ comentario
            //dgv.Columns[IND_COMMENT].Name = nameColComment; // Nombre de la columna Descripción (dependerá del idioma).
            dgv.Columns[IND_COMMENT].HeaderText = nameColComment;
            dgv.Columns[IND_COMMENT].Width = 500;
            dgv.Columns[IND_COMMENT].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns[IND_COMMENT].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns[IND_COMMENT].ReadOnly = true;

            foreach (Facet f in lf)
            {
                object[] my_Row = new object[numCol];

                my_Row[IND_NAME] = f.Name();
                my_Row[IND_LEVEL] = f.Level();
                int s = f.SizeOfUniverse();
                if (int.MaxValue.Equals(s))
                {
                    my_Row[IND_SIZE_OF_UNIVERSE_FACET] = Facet.INFINITE;
                }
                else
                {
                    my_Row[IND_SIZE_OF_UNIVERSE_FACET] = s;
                }

                my_Row[IND_COMMENT] = f.Comment();
                dgv.Rows.Add(my_Row);
            }

        } // end LoadListFacetInDataGridViewForEdit



        /*
         * Descripción:
         *  Operación auxiliar, inicializa el data grid de facetas para que se puedan introducir 
         *  los datos.
         * Parámetros:
         *  DataGridViewEx.DataGridViewEx dgvExFacets: es el dataGridViewEx que vamos a inicializar para 
         *          la introducción de datos de facetas.
         */
        private void CleanerDataGridViewExFacets(DataGridViewEx.DataGridViewEx dgvExFacets)
        {
         
            dgvExFacets.NumeroFilas = 0;

            dgvExFacets.Rows.Clear();
            
            // dgvExFacets.ColumnCount = 4; // numero de columnas 3 (Etiquetas,nivel,tamaño del universo,descipción)
            dgvExFacets.NumeroColumnas = 4;

            dgvExFacets.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Set the column header style.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Aqua;
            columnHeaderStyle.Font = new Font("Verdana", 10, FontStyle.Bold);
            dgvExFacets.ColumnHeadersDefaultCellStyle = columnHeaderStyle;

            dgvExFacets.Columns[IND_NAME].Name = "nameColFacet"; // Nombre de la columna Etiquetas (dependerá del idioma).
            dgvExFacets.Columns[IND_NAME].HeaderText = nameColFacet;
            // dgvExFacets.Columns[IND_NAME].Width = 100;
            dgvExFacets.Columns[IND_NAME].SortMode = DataGridViewColumnSortMode.NotSortable;
            // dgvExFacets.Columns[IND_LEVEL].Name = nameColLevel; // Nombre de la columna Niveles (dependerá del idioma).
            dgvExFacets.Columns[IND_LEVEL].HeaderText = nameColLevel;
            // dgvExFacets.Columns[IND_LEVEL].Width = 100;
            dgvExFacets.Columns[IND_LEVEL].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvExFacets.ColumnaTipoEntrada(IND_LEVEL, DataGridViewEx.TipoEntrada.SoloNumeros);
            dgvExFacets.Columns[IND_SIZE_OF_UNIVERSE_FACET].Name = "nameColSizeOfUniverse"; // tamaño del universo
            dgvExFacets.Columns[IND_SIZE_OF_UNIVERSE_FACET].HeaderText = nameColSizeOfUniverse;
            // dgvExFacets.Columns[IND_SIZE_OF_UNIVERSE_FACET].Width = 100;
            dgvExFacets.Columns[IND_SIZE_OF_UNIVERSE_FACET].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvExFacets.Columns[IND_COMMENT].Name = "col_Description"; // Nombre de la columna Descripción (dependerá del idioma).
            dgvExFacets.Columns[IND_COMMENT].HeaderText = nameColComment;
            // dgvExFacets.Columns[IND_COMMENT].Width = 500;
            dgvExFacets.Columns[IND_COMMENT].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvExFacets.Columns[IND_COMMENT].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // impedimos que se puedan insertar nuevas filas
            dgvExFacets.AllowUserToAddRows = false;
            // permitimos que se vea el encabezado de las columnas
            dgvExFacets.ColumnHeadersVisible = true;

        }// end CleanerDataGridViewExFacets


        #region Operaciones sobre el dataGridViewEx
        /*
         * Carga las facetas en el dataGriview
         */
        #endregion Operaciones sobre el dataGridViewEx


        /* Descripción:
         *  Carga la cabecera de la tabla de observaciones en el dataGridView.
         */
        private void LoadHeadersInObsTable(MultiFacetsObs multiFacets, DataGridViewEx.DataGridViewEx dgvExObsTable)
        {
            // dgvExObsTable.Rows.Clear();
            dgvExObsTable.ColumnHeadersVisible = true;
            int n = multiFacets.ListFacets().Count();
            // dgvExObsTable.ColumnCount = n + 1;
            dgvExObsTable.NumeroColumnas = n + 1;

            // Set the column header style.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Aqua;
            columnHeaderStyle.Font = new Font("Verdana", 10, FontStyle.Bold);
            dgvExObsTable.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            dgvExObsTable.DefaultCellStyle.Font = fontCellTable;

            for (int colu = 0; colu < n; colu++)
            {
                // dgvExObsTable.Columns[colu].Name = multiFacets.ListFacets()[colu].Name();
                // dgvExObsTable.Columns[colu].HeaderText = multiFacets.ListFacets().FacetInPos(colu).Name();
                dgvExObsTable.Columns[colu].HeaderText = multiFacets.ListFacets().FacetInPos(colu).ListFacetDesing();
                // impedimos que las columnas sean reordenables al pulsar la cabecera
                dgvExObsTable.Columns[colu].SortMode = DataGridViewColumnSortMode.NotSortable;
                // dgvExObsTable.Columns[colu].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells; // .AllCells;
                dgvExObsTable.Columns[colu].ReadOnly = true;
            }

            /* measurementVariable: Contiene el nombre de la columna de datos (esta cambiará 
             * en función del idioma elegido).
             * 
             * 'n' se corresponde ahora con el indice de la última columna, aquella que contiene
             * las "variables observadas".
             */
            // dgvExObsTable.Columns[n].Name = measurementVariable; // asignamos nombre
            dgvExObsTable.Columns[n].HeaderText = measurementVariable;
            dgvExObsTable.Columns[n].Width = 200; // asignamos longitud
            // e impedimos que la ultima fila sea ordenable al pulsar la cabecera.
            dgvExObsTable.Columns[n].SortMode = DataGridViewColumnSortMode.NotSortable;
            // dgvExObsTable.Columns[n].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            // dgvExObsTable.Columns[n].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvExObsTable.Columns[n].ReadOnly = true;
            //dgvExObsTable.Columns[n].DefaultCellStyle.Format = "###,##0.00";
            // dgvExObsTable.AutoSize = true; // DataGridViewAutoSizeColumnMode.AllCells;
        }// end LoadHeadersInObsTable


        /*
         * Descripción:
         *  Carga los datos del objetos que se pasa como parametro en la tabla de datos (la que muestra los datos
         *  de la variable observable).
         * Parámetros:
         *      MultiFacetsObs multiFacets: Objeto que contiene los datos que se almacenarán en las tablas
         */
        private void loadDataInTabPageObsTable(MultiFacetsObs multiFacets)
        {
            dataGridViewExObsTable.Rows.Clear();// limpiamos el dataGridViewEx

            //DataTable dtPrueba = multiFacets.ObservationTable().ObsTable2DataTable(multiFacets.ListFacets());

            //dataGridViewExObsTable.DataSource = dtPrueba;

            // Escribimos la cabecera de la tabla
            LoadHeadersInObsTable(multiFacets, dataGridViewExObsTable);


            int fila = multiFacets.ObservationTable().ObsTableRows();
            int col = multiFacets.ObservationTable().ObsTableColumns();

            for (int f = 0; f < fila; ++f)
            {
                object[] mi_fila = new object[col + 1]; // + 1 para los datos a introducir
                //Meto los datos en la fila
                for (int c = 0; c < col; c++)
                {
                    mi_fila[c] = multiFacets.ObservationTable().Data(f, c);
                }

                dataGridViewExObsTable.Rows.Add(mi_fila);
            }
            
        }// end loadDataInTabPageObsTable


       /* Descripción:
        *  Carga los datos del objetos que se pasa como parámetro en la tabla de datos (la que muestra los datos
        *  de la variable observable). Pero en este caso solo carga los valores distintos de null.
        * Parámetros:
        *      MultiFacetsObs multiFacets: Objeto que contiene los datos que se almacenarán en las tablas
        */
        private void loadData_hideNulls_InTabPageObsTable(MultiFacetsObs multiFacets)
        {
            dataGridViewExObsTable.Rows.Clear();// limpiamos el dataGridViewEx

            //DataTable dtPrueba = multiFacets.ObservationTable().ObsTable2DataTable(multiFacets.ListFacets());

            //dataGridViewExObsTable.DataSource = dtPrueba;

            // Escribimos la cabecera de la tabla
            LoadHeadersInObsTable(multiFacets, dataGridViewExObsTable);


            int fila = multiFacets.ObservationTable().ObsTableRows();
            int col = multiFacets.ObservationTable().ObsTableColumns();

            InterfaceObsTable obsTable = multiFacets.ObservationTable();
            for (int f = 0; f < fila; ++f)
            {
                // Insertamos en la tabla si es distinto de null;
                if (obsTable.Data(f) != null)
                {
                    object[] mi_fila = new object[col + 1]; // + 1 para los datos a introducir
                    //Meto los datos en la fila
                    for (int c = 0; c < col; c++)
                    {
                        mi_fila[c] = obsTable.Data(f, c);
                    }

                    dataGridViewExObsTable.Rows.Add(mi_fila);
                }
            }

        }// end loadData_hideNulls_InTabPageObsTable


        /* Descripción:
         *  Tras pulsar en el borton aceptar del tabPage Tabla de observaciones almacenamos los datos 
         *  en un archivo.
         */
        private void btDataObsOkAndSaveFile()
        {
            // Primero recojo los datos de la tabla de observaciones en el objeto multifaceta
            if (this.readDataGridViewExObsTableAndSaveInMultiObsTable())
            {
                // luego guardo los datos en un archivo
                saveFileData(this.sagtElements);

                // this.dataGridViewExFacets.ReadOnly = true;
                int n = this.dataGridViewExFacets.Columns.Count;
                for (int i = 0; i < n; i++)
                {
                    this.dataGridViewExFacets.Columns[i].ReadOnly = true;
                }

                int numCol = this.dataGridViewExObsTable.ColumnCount;
                this.dataGridViewExObsTable.Columns[numCol-1].ReadOnly = true;
                LoadListFacetInDataGridView(this.sagtElements.GetMultiFacetsObs().ListFacets() ,dataGridViewExFacets, true);

                disableButtonsFacets();
                disableButtonObsTable();
                // Habilitamos el menú de acciones del menú vertical datos.
                mStripData.Enabled = true;
                // Activamos el menú principal poniendo la variable editionModeOn a false.
                this.editionModeOn = false;
                // Mostramos el checkBox "Ocultar nulos"
                this.checkBoxHideNulls.Visible = true;
                this.checkBoxHideNulls.Enabled = true;
                this.checkBoxHideNulls.Checked = false;
            }
        }// end btDataObsOkAndSaveFile
        

        /* Descripción:
         *  Pregunta al usuario donde quiere almacenar la información, si la respuesta es afirmativa, se 
         *  guarda las facetas y la tabla de observaciones en un fichero.
         */
        private void saveFileData(SagtFile sagtElements)
        {
            if (sagtElements.GetMultiFacetsObs() == null)
            {
                ShowMessageErrorOK(errorNoTableObs);
                //MessageBox.Show(errorNoTableObs);
            }
            else
            {
                SaveFileSagt(sagtElements);
            }
        }// end saveFileData


        /* Descripción:
         *  Operación auxiliar. Seleccionamos los datos y los guardamos en un fichero Sagt
         */
        private void SaveFileSagt(SagtFile sagtElements)
        {
            bool bData = !(sagtElements.GetMultiFacetsObs() == null); // pestaña de tabla de frecuencias
            bool bMean = !(sagtElements.GetListMeans() == null); // pestaña de tablas de medias
            bool bSsq = !(sagtElements.GetAnalysis_and_G_Study() == null); // pestaña de análisis de varianza

            FormSelectSaves formSelectSaves = new FormSelectSaves(this.LanguageActually(), bData, bMean, bSsq);

            bool salir = false;
            do
            {
                DialogResult res = formSelectSaves.ShowDialog();
                switch (res)
                {
                    case DialogResult.Cancel: salir = true; break;
                    case DialogResult.OK:
                        // comprobamos las pestañas marcadas
                        MultiFacetsObs tData = null;
                        ListMeans lMeans = null;
                        Analysis_and_G_Study tAnalysis = null;

                        // Comprobamos las pestañas seleccionadas
                        bData = formSelectSaves.IsDataSelected();
                        bMean = formSelectSaves.IsMeansSelected();
                        bSsq = formSelectSaves.IsSsqSelected();
                        // al menos una de las pestañas seleccionadas
                        if (bData || bMean || bSsq)
                        {// (* 1 *)
                            if (bData)
                            {
                                // actualiza los valores omit de la lista de facetas
                                ReadColumnOmit(sagtElements, this.dataGridViewExFacets);
                                tData = sagtElements.GetMultiFacetsObs();
                            }
                            if (bMean)
                            {
                                lMeans = sagtElements.GetListMeans();
                            }
                            if (bSsq)
                            {
                                tAnalysis = sagtElements.GetAnalysis_and_G_Study();
                            }
                            SagtFile sagtElementsSave = new SagtFile(tData, lMeans, tAnalysis);
                            // Habrimos la ventana de dialogo y guardamos
                            System.Windows.Forms.DialogResult resulDialog = DialogAndSaveSagtFile(sagtElementsSave);
                            
                            btGenerateTableObsDisables();
                            salir = true;
                        }
                        else
                        {
                            // No hay ningún elemento seleccionado, mostramos un mensaje
                            // no hay ningún elemento seleccionado.
                            ShowMessageErrorOK(txtMessageNoSelected, "", MessageBoxIcon.Stop);

                        }// end if (* 1 *)
                        break;
                }// end switch
            } while (!salir);
        }// end SaveFileSagt()


        /* Descripción: 
         *  Metodo auxiliar, muestra la ventana de dialogo para guardar un sagtFile en la ubicación especificada
         */
        private System.Windows.Forms.DialogResult DialogAndSaveSagtFile(SagtFile sagtElementsSave)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();

            if (Directory.Exists(this.cfgApli.Get_Path_Workspace()))
            {
                saveDialog.InitialDirectory = this.cfgApli.Get_Path_Workspace();
            }

            saveDialog.DefaultExt = DEFAULT_EXT_SAGT; // Extensión del fichero sagt
            string fileFilert = "sagt file" + FILTER_SAGT_FILE + FILTER_ALL_FILE;
            saveDialog.Filter = fileFilert;
            saveDialog.OverwritePrompt = true; // muestra advertencia si el fichero ya existe
            saveDialog.AddExtension = true;
            System.Windows.Forms.DialogResult resulDialog = saveDialog.ShowDialog();
            if (resulDialog == DialogResult.OK)
            {
                // multiFacet.WritingFileObsData(saveDialog.FileName);
                sagtElementsSave.WritingSagtFile(saveDialog.FileName);
                // Si tenemos la tabla de frecuencias abierta actualizamos el nombre
                MultiFacetsObs multiFacet = sagtElements.GetMultiFacetsObs();
                if (multiFacet != null)
                {
                    // Existe una tabla de frecuencias y por tanto actualizamos el nombre del fichero
                    multiFacet.NameFileObs(saveDialog.FileName);
                    this.tbFileName.Text = saveDialog.FileName;
                }
                // Mostrmos un mensaje indicando que los datos han sido guardados
                // MessageBox.Show(txtSaveDataObserved, titleSaved);
                ShowMessageInfo(txtSaveDataObserved, titleSaved);
            }
            // deshabilitamos los botones de facetas
            return resulDialog;
        }



        /* Descripción:
         *  Lee los datos del dataGridViewEx de la tabla de observaciones y lo almacena en la variable
         *  multifaceta
         */
        private bool readDataGridViewExObsTableAndSaveInMultiObsTable()
        {
            try
            {
                List<double?> readDatas = new List<double?>();
                int nCols = this.dataGridViewExObsTable.NumeroColumnas-1; // columna donde se encuentra los datos
                int nRows = this.dataGridViewExObsTable.RowCount; // número de filas para poder iterar

                for (int i = 0; i < nRows; i++)
                {
                    DataGridViewRow my_Row = this.dataGridViewExObsTable.Rows[i];
                    if (my_Row.Cells[nCols].Value != null)
                    {
                        string s = my_Row.Cells[nCols].Value.ToString();
                        double? d = ConvertNum.String2Double(s);
                        readDatas.Add(d);
                    }
                    else
                    {
                        readDatas.Add(null);
                    }

                }
                MultiFacetsObs multiFacets = this.sagtElements.GetMultiFacetsObs();
                multiFacets.AssignDataToTheTableObs(readDatas);
                return true;
            }
            catch (FormatException)
            {
                // muestra el mensage: "Error: uno o más varibles observadas no contiene datos numéricos"
                ShowMessageErrorOK(errorVarTableObs);
                return false;
            }
        }// end readDataGridViewExObsTableAndSaveInMultiObsTable


        /* Descripción:
         *  Lee los elementos de la columna Omitir del dataGrid de facetas y actuliza la variable
         *  de lista de facetas en el la variable sagtElements
         */
        public void ReadColumnOmit(SagtFile sagtElements, DataGridViewEx.DataGridViewEx dgvFacet)
        {
            // Solo se ejecuta en el caso de que exista una tabla de frecuencias.
            // En ese caso hay que actualizar la facetas omitidas para guardarla correctamente
            if (sagtElements.GetMultiFacetsObs() != null)
            {
                ListFacets lf = sagtElements.GetMultiFacetsObs().ListFacets();

                if (dgvFacet.Columns[NAME_COL_OMIT] != null)
                {
                    int n = lf.Count();
                    for (int i = 0; i < n; i++)
                    {
                        Facet f = lf.FacetInPos(i);
                        // bool omit = dgvFacet.Columns["nameColOmit"].
                        DataGridViewCheckBoxCell checkBoxCell = (DataGridViewCheckBoxCell)dgvFacet.Rows[i].Cells[NAME_COL_OMIT];
                        bool omit = Convert.ToBoolean(checkBoxCell.Value);
                        f.Omit(omit);
                    }
                }
            }
        }


        /* Descripción:
         *  Acción que se ejecuta tra pulsar el bóton Nuevo del menú vertical de la opción Datos.
         *  Genera un nuevo objeto MultiFaceta
         */
        private void tsmiActionNewMultiFacetData_Click(object sender, EventArgs e)
        {
            // Deshabilitamos el menú principal poniendo la variable booleana editionModeOn a true.
            this.editionModeOn = true;
            // desactivamos el menu de acciones
            mStripData.Enabled = false;
            int t = 0; // número de facetas

            /* El siguiente codigo es provisonal
             */
            FormAssignNumOfFacets fAssignNumFacets = new FormAssignNumOfFacets(this.dicMessage, this.LanguageActually());

            bool salir = false;
            do
            {
                DialogResult res = fAssignNumFacets.ShowDialog();
                switch (res)
                {
                    case DialogResult.Cancel:
                        salir = true; // ponemos la variable a true para salir del bucle
                        this.CancelAcciónEditionOfFacet();
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
                                salir = true; // ponemos la variable a true para salir del bucle
                                // Asignamos la disposición de las facetas
                                provision = fAssignNumFacets.CheckGroupBoxProvisionOfFacets();
                            }
                        }
                        break;
                }
            } while (!salir);

            if (t >= 2)
            {
                // limpiamos los campos
                tbFileName.Text = "";
                tbDescription.Text = "";
                
                // dataGridViewExFacets.ReadOnly = false;
                int n = dataGridViewExFacets.Columns.Count;
                for (int i = 0; i < n; i++)
                {
                    dataGridViewExFacets.Columns[i].ReadOnly = false;
                }

                CleanerDataGridViewExFacets(this.dataGridViewExFacets);
                // permitimos que se puedan añadir filas a la tabla
                // dataGridViewExFacets.AllowUserToAddRows = true;
                dataGridViewExFacets.NumeroFilas = 0 ;
                dataGridViewExFacets.NumeroFilas = t;
                // ocultamos el tabPage de la tabla de datos
                tabPageObsTable.Parent = null;
                tabPageDataInfo.Parent = null;
                // Mostramos los botones
                enableButtonsFacets(provision);
                
                // permitimos que pueda introducir la descripción del archivo
                tbDescription.Enabled = true;
                tbDescription.ReadOnly = false;
            }
        }// end tsmiActionNewMultiFacetData_Click


        #region Habilita deshabilita botones ok y cancel de los tabpages
        /*
         * Descripción:
         *  Habilita los botones "Canelar" y "Generar tabla de oservaciones" del tabPageFacetas.
         *  Si la disposición de las facetas no es mixta no se mostrará el botón de generar anidamientos
         * Parámetros:
         *  ProvisionOfFacets provision: Indica el tipo de diposición de las facetas. 
         */
        private void enableButtonsFacets(ProvisionOfFacets provision)
        {
            // si la diposición de facetas no es mixta no mostramos el boton de generar anidamientos
            if (provision.Equals(ProvisionOfFacets.Mixed))
            {
                // habilitamos los botones de anidar facetas
                btNestingFacet.Enabled = true;
                btNestingFacet.Visible = true;
                btRemoveNesting.Enabled = true;
                btRemoveNesting.Visible = true;
            }

            // habilitamos el botón generar
            btGenerateTableObs.Enabled = true;
            btGenerateTableObs.Visible = true;

            // habilitamos el botón Cancelar
            btDataFacetsCancel.Enabled = true;
            btDataFacetsCancel.Visible = true;
        }


        /* Descripción:
         *  Deshabilita los botones "Canelar" y "Generar tabla de oservaciones" del tabPageFacetas 
         */
        private void disableButtonsFacets()
        {
            // deshabilitamos el botón de anidar facetas
            btNestingFacet.Enabled = false;
            btNestingFacet.Visible = false;
            btRemoveNesting.Enabled = false;
            btRemoveNesting.Visible = false;

            // deshabilitamos el boton generar
            btGenerateTableObs.Enabled = false;
            btGenerateTableObs.Visible = false;

            // deshabilitamos el botton Cancelar
            btDataFacetsCancel.Enabled = false;
            btDataFacetsCancel.Visible = false;
        }


        /* Descripción:
         *  Deshabilita los botones ok y cancel del tabPage de tabla de observaciones
         */
        private void disableButtonObsTable()
        {
            btImportScores.Enabled = false;
            btImportScores.Visible = false;

            btDataObsSave.Enabled = false;
            btDataObsSave.Visible = false;

            btDataObsCancel.Enabled = false;
            btDataObsCancel.Visible = false;
        }


        /* Descripción:
         *  Habilita los botones ok y cancel del tabPage de tabla de observaciones
         */
        private void enableButtonObsTable()
        {
            btImportScores.Enabled = true;
            btImportScores.Visible = true;

            btDataObsSave.Enabled = true;
            btDataObsSave.Visible = true;

            btDataObsCancel.Enabled = true;
            btDataObsCancel.Visible = true;
        }
        

        /* Descripción:
         *  Deshabilita el boton Generar Tabla de observaciones
         */
        private void btGenerateTableObsDisables()
        {
            btGenerateTableObs.Enabled = false;
            btGenerateTableObs.Visible = false;
        }


        /* Descripción:
         *  Cancela la operación de edición de facetas previa a la edición de la tabla de observaciones.
         */
        public void CancelAcciónEditionOfFacet()
        {
            // Ocultamos el checkBox de "Ocultar nulos"
            this.checkBoxHideNulls.Visible = true;
            this.checkBoxHideNulls.Enabled = false;
            this.checkBoxHideNulls.Checked = false;

            // limpiamos los campos
            // CleanerDataGridViewExFacets();
            this.dataGridViewExFacets.Rows.Clear();
            //this.dataGridViewExFacets.NumeroFilas = 0;
            this.dataGridViewExFacets.NumeroColumnas = 0;
            this.dataGridViewExObsTable.Rows.Clear();
            this.dataGridViewExObsTable.NumeroColumnas = 0;
            //this.dataGridViewExObsTable.NumeroFilas = 0;

            // this.numOfFacetForTable = 0;

            // Vuelve a mostrar la pestaña de Observaciones
            tabPageObsTable.Parent = tabControlData;
            tabPageDataInfo.Parent = tabControlData;

            //  Habilitamos el menu de acciones de tabPage Datos
            mStripData.Enabled = true;
            // deshabilitamos los botones 'Cancelar' y 'Generar tabla de observaciones'
            disableButtonsFacets();
            // ponemos la lista de facetas a null
            this.lf_global = null;
            // Activamos el menú principal poniendo la variable editionModeOn a false.
            this.editionModeOn = false;
            tbDescription.Text = "";
            tbDescription.ReadOnly = true;
            tbFileName.Enabled = false;
        }

        #endregion Habilita deshabilita botones ok y cancel de los tabpages


        /* Descripción:
         *  Abre la ventana para la inserción de un anidamiento. Este botón solo esta disponible cuando
         *  se selecciona la disposición de facetas mixta.
         */
        private void btActionNestingFacet_Click(object sender, EventArgs e)
        {
            // bool editionFacet = false;
            if (this.lf_global == null)
            {
                ListFacets lf = validateFacetTable(dataGridViewExFacets);
                if (lf != null)
                {
                    // Lanzamos una advertencia: Ya no podrá editar las facetas
                    DialogResult res = ShowMessageDialog(titleConfirm, txtConfirmBuildNesting);
                    if (res.Equals(DialogResult.OK))
                    {
                        this.lf_global = lf;
                    }
                }
            }


            if (this.lf_global != null)
            {
                FormAddNesting fAddNesting = new FormAddNesting(this.LanguageActually(), this.lf_global);

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
                                    Facet f = this.lf_global.FacetInPos(posSelctFacet);
                                    Facet f_Nested = this.lf_global.FacetInPos(posNestedFacet);
                                    // anidamos
                                    /* if (f.IsValidateNesting(f_Nested))
                                    // if(f_Nested.IsValidateNesting(f))
                                    {
                                        f.ListFacetsDesignNesting(f_Nested);
                                        // f_Nested.ListFacetsNesting(f);
                                        // pintamos de nuevo las facetas en la tabla
                                        LoadListFacetInDataGridView(this.lf_global, dataGridViewExFacets);
                                        salir = true;
                                    }*/
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
                                        LoadListFacetInDataGridView(this.lf_global, dataGridViewExFacets);
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
            }
        }// end btActionNestingFacet_Click


        /* Descripción:
         *  Abrer la ventana para la eliminación de anidamientos.
         */
        private void btActionRemoveNesting_Click(object sender, EventArgs e)
        {
            if (lf_global != null)
            {
                bool nesting = false;
                int n = lf_global.Count();
                for (int i = 0; i < n && !nesting; i++)
                {
                    Facet f = lf_global.FacetInPos(i);
                    nesting = f.IsNesting();
                }

                if (nesting)
                {
                    List<string> lfSelectFacet = new List<string>();
                    n = lf_global.Count();
                    for (int i = 0; i < n ; i++)
                    {
                        Facet f = lf_global.FacetInPos(i);
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
                                if (ckListBox.CheckedIndices.Count > 0)
                                {
                                    foreach (int indexChecked in ckListBox.CheckedIndices)
                                    {
                                        string desing = lfSelectFacet[indexChecked];
                                        int pos = desing.IndexOf("]") - 1;
                                        string name = desing.Substring(1, pos);
                                        Facet f_nesting = lf_global.LookingFacet(name);
                                        f_nesting.ListFacetsDesignRemove();
                                        // Pintamos de nuevo la tabla
                                        LoadListFacetInDataGridView(this.lf_global, dataGridViewExFacets);
                                        salir = true;
                                    }
                                }
                                else
                                {
                                    // Error no se ha seleccionado ningún elemento
                                    ShowMessageErrorOK(txtMessageNoSelected);
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
        } // end btActionRemoveNesting_Click


        /* Descripción:
         *  Edición de la tabla de varibles observadas (Tabla de observaciones)
         */
        private void EditDataObsTable()
        {
            if (this.sagtElements.GetMultiFacetsObs() == null)
            {
                ShowMessageErrorOK(errorNoTableObs);
                // MessageBox.Show(errorNoTableObs);
            }
            else
            {

                this.editionModeOn = true;
                this.editMultiFacetObs = true;
                mStripData.Enabled = false;
                tabControlData.SelectedIndex = 1;
                int numCol = this.dataGridViewExObsTable.ColumnCount;
                this.dataGridViewExObsTable.Columns[numCol - 1].ReadOnly = false;
                this.enableButtonObsTable();
                // Ocultamos el checkBox de "Ocultar nullos"
                this.checkBoxHideNulls.Visible = false;
                this.checkBoxHideNulls.Enabled = false;
            }
        }


        /* Descripción:
         *  Permite modificar el contenido de las facetas que no alteran la tabla de observaciones.
         *  Esto es, todos los atributos de una tabla menos los niveles de una faceta (Nombre, Tamaño del
         *  universo y descripción).
         */
        private void tsmiActionData_EditFacets_Click(object sender, EventArgs e)
        {
            if (this.sagtElements.GetMultiFacetsObs() == null)
            {//begin if (*1*)
                ShowMessageErrorOK(errorNoTableObs);
                // MessageBox.Show(errorNoTableObs);
            }
            else
            {
                this.editionModeOn = true;
                this.editMultiFacetObs = true;
                mStripData.Enabled = false;
                tabControlData.SelectedIndex = 0;
                // Ocultamos las pestaña de facetas y tabla de observaciones
                this.tabPagMultiFacet.Parent = null;
                this.tabPageObsTable.Parent = null;
                this.tabPageDataInfo.Parent = null;

                // Mostramos la pestaña de editar facetas
                this.tabPageDataEditFacets.Parent = this.tabControlData;

                // Mostramos los datos en los campos
                MultiFacetsObs multiFacets = this.sagtElements.GetMultiFacetsObs();
                this.tbEditFacetFileName.Text = multiFacets.NameFileObs();
                this.tbEditFacetDescription.Text = multiFacets.DescriptionFile();
                //Cargamos los datos de las facetas en la tabla
                CleanerDataGridViewExFacets(dGridViewExDataEditFacets);
                LoadListFacetInDataGridView(multiFacets.ListFacets(), dGridViewExDataEditFacets, false, true);
                // ocultamos la columna que no podemos modificar, es decir la de los niveles
                dGridViewExDataEditFacets.Columns[IND_LEVEL].Visible = false;
                // Permitimos la modificación del resto de columnas.
                int nCol = dGridViewExDataEditFacets.ColumnCount;
                for (int i = 0; i < nCol; i++)
                {
                    dGridViewExDataEditFacets.Columns[i].ReadOnly = false;
                }
                dGridViewExDataEditFacets.Columns[IND_LEVEL].ReadOnly = true;
                // dGridViewExDataEditFacets.ReadOnly = false;

            }// end if(*1*)
        }// end private void tsmiActionData_EditFacets_Click


        /* Descripción:
         *  Guarda los cambios que se realizan tras la edición de las facetas
         */
        private void btActionDataEditFacetsSave_Click()
        {
            // primero debemos verificar que todas las facetas sigan siendo validas
            MultiFacetsObs multiFacets = this.sagtElements.GetMultiFacetsObs();
            InterfaceObsTable obsTable = multiFacets.ObservationTable();
            ListFacets lf = validateFacetTable(this.dGridViewExDataEditFacets);
            if (lf != null) // si los datos son correctos...
            {
                // Recuperamos las facetas modificadas de la tabla y la descripción
                string newDescription = tbEditFacetDescription.Text;

                string pathfile = multiFacets.NameFileObs();
                MultiFacetsObs newMultiFacet = new MultiFacetsObs(lf, obsTable, pathfile, newDescription,
                    this.richTextBoxDataComment.Text);
                
                // Actualizamos las tablas de facetas
                //=================================== 
                LoadListFacetInDataGridView(lf, this.dataGridViewExFacets, true);
                // Actualizamos el campo descripción.
                tbDescription.Text = newMultiFacet.DescriptionFile();

                // Escribimos la cabecera de la tabla de observaciones ya que los datos no han cambiado
                LoadHeadersInObsTable(newMultiFacet, dataGridViewExObsTable);
                // asignamos el nuevo objeto multifaceta
                this.sagtElements.SetMultiFacetsObs(newMultiFacet);
                this.sagtElements.WritingSagtFile(pathfile);
                // "Cerramos" la edición mediante el siguiente método
                btActionDataEditFacetsCancel_Click();
            }
        }// end btActionDataEditFacetsSave_Click


        /* Descripción:
         *  Cancela la edición de la facetas.
         */
        private void btActionDataEditFacetsCancel_Click()
        {
            // Mostramoslas pestaña de facetas y tabla de observaciones
            this.tabPagMultiFacet.Parent = this.tabControlData;
            this.tabPageObsTable.Parent = this.tabControlData;
            this.tabPageDataInfo.Parent = this.tabControlData;

            // Ocultamos la pestaña de editar facetas
            this.tabPageDataEditFacets.Parent = null;

            // El textBox de descripción lo ponemos en solo lectura
            this.tbDescription.ReadOnly = true;

            this.editionModeOn = false;
            this.editMultiFacetObs = false;
            mStripData.Enabled = true;
            this.lf_global = null;
        }


        /*
         * Descripción:
         *  Acción que se ejecuta tras pulsar el botón Generar tabla de observaciones cuando se
         *  termina de editar las facetas. Inserta de nuevo el tabPage de la tabla de datos y 
         *  deshabilita los botones de facetas: 'Generar tabla de observaciones' y Cancelar.
         */
        private void btAccionGenerateTableObs_Click(object sender, EventArgs e)
        {
            ListFacets lf = null;
            // Comprobamos si hay datos en la varible de edición de facetas
            if (this.lf_global == null)
            {
                // comprobamos que los datos de la tabla son correctos y los obtenemos de la tabla.
                lf = validateFacetTable(this.dataGridViewExFacets);
                if (this.provision.Equals(ProvisionOfFacets.Nested))
                {
                    // Hacemos un anidamiento de las facetas
                    lf.NestingAllFacet();
                    LoadListFacetInDataGridView(lf, dataGridViewExFacets);
                }
            }
            else
            {
                lf = this.lf_global;
            }

            if (lf != null) // si los datos son correctos...
            {
                try
                {
                    // Ocultamos el checkBox de "Ocultar nulos"
                    this.checkBoxHideNulls.Visible = false;
                    this.checkBoxHideNulls.Enabled = false;
                    MultiFacetsObs multiFacets = new MultiFacetsObs(lf, "no name", tbDescription.Text);
                    this.sagtElements.SetMultiFacetsObs(multiFacets);
                    /* Una vez que pasamos a editar la tabla de datos observados ya no podemos modificar
                     * el nivel de las facetas. */
                    this.dataGridViewExFacets.Columns[IND_LEVEL].ReadOnly = true;
                    this.dataGridViewExFacets.Columns[IND_SIZE_OF_UNIVERSE_FACET].ReadOnly = true;

                    tabPageObsTable.Parent = tabControlData;
                    tabPageDataInfo.Parent = tabControlData;

                    tabControlData.SelectedIndex = 1;
                    // creamos los datos multifaceta
                    loadDataInTabPageObsTable(multiFacets);
                    // desabilitamos los botones de facetas.
                    disableButtonsFacets();
                    int numCol = this.dataGridViewExObsTable.NumeroColumnas;
                    this.dataGridViewExObsTable.Columns[numCol - 1].ReadOnly = false;

                    /* habilitamos el menú de acciones.
                    mStripData.Enabled = true;
                     */
                    this.enableButtonObsTable();

                }
                catch (ObsTableException)
                {
                    // Se ha sobrepasado el número de filas de la tabla
                    ShowMessageErrorOK(errorNoTableObs);
                }
                               
            }
            this.lf_global = null;
        }// end btAccionGenerateTableObs_Click


        /* Descripción:
         *  Muestra la ventana de selección de medias en caso de que no haya ningun elemento multifaceta 
         *  en uso lanzara un mensaje de error al usuario avisando de que no hay ninguna faceta en uso.
         */
        private void tsmiActionBuildMeans_Click(object sender, EventArgs e)
        {
            MultiFacetsObs multiFacets  = this.sagtElements.GetMultiFacetsObs();
            if (multiFacets == null)
            {// (* 1 *)
                ShowMessageErrorOK(errorNoTableObs);
                //MessageBox.Show(errorNoTableObs);
            }
            else
            {
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

                    // omitimos los niveles marcados
                    if (lf.HasSkipLevels())
                    {
                        multiFacets = multiFacets.SkipIndexLevelFacetInDataTable();
                    }
                    
                    List<string> cswr = lf.CombinationStringWithoutRepetition();
                    List<string> ldesingSeleted = new List<string>();
                    
                    FormListFacets formListFacet = new FormListFacets(lang, cswr);
                    
                    bool salir = false;
                    do
                    {
                        DialogResult res = formListFacet.ShowDialog();
                        switch (res)
                        {// (* 1 *)
                            case DialogResult.Cancel: salir = true; break;
                            case DialogResult.OK:

                                List<ListFacets> selectedListFacets = new List<ListFacets>();

                                CheckedListBox ckListBox = formListFacet.CheckedListBoxListsFacets();
                                int n = ckListBox.Items.Count;

                                for (int i = 0; i < n; i++)
                                {
                                    if (ckListBox.GetItemChecked(i))
                                    {
                                        string design = ckListBox.Items[i].ToString();
                                        ldesingSeleted.Add(design);
                                        ListFacets newLf = lf.ListDesignFacets(design);
                                        selectedListFacets.Add(newLf);
                                    }
                                }

                                if (selectedListFacets.Count > 0)
                                {// (* 3 *)
                                    if (this.ListMeans() != null)
                                    {
                                        DialogResult res2 = ShowMessageDialog(titleConfirm, txtConfirmClearMeans);

                                        if (res2 == DialogResult.OK)
                                        {
                                            this.ClearTabPageMeans();
                                            // hay al menos algún elemento seleccionado
                                            createMeansOfListOfListFacets(selectedListFacets, ldesingSeleted, multiFacets, this.cfgApli, MEAN_STRINGS);
                                            salir = true;

                                            ExcludeTabPages();
                                            this.tabPageMeans.Parent = this.tabControlOptions;
                                            // Restauramos los colores
                                            this.RestoreColorMenu(this.mStripMain);
                                            // Asignamos el nuevo color
                                            this.tsmiMeans.BackColor = System.Drawing.SystemColors.Highlight;
                                        }
                                    }
                                    else
                                    {
                                        // hay al menos algún elemento seleccionado
                                        createMeansOfListOfListFacets(selectedListFacets, ldesingSeleted, multiFacets, this.cfgApli, MEAN_STRINGS);

                                        ExcludeTabPages();
                                        this.tabPageMeans.Parent = this.tabControlOptions;
                                        // Restauramos los colores
                                        this.RestoreColorMenu(this.mStripMain);
                                        // Asignamos el nuevo color
                                        this.tsmiMeans.BackColor = System.Drawing.SystemColors.Highlight;
                                        salir = true;
                                    }
                                }
                                else
                                {
                                    // no hay ningún elemento seleccionado.
                                    ShowMessageErrorOK(txtMessageNoSelected, "", MessageBoxIcon.Stop);
                                }// end if (* 3 *)

                                break;
                        }// end switch(* 1 *)
                    } while (!salir);
                }// end if (* 2 *)

            }// end if (* 1 *)
        }// end tsmiActionBuildMeans_Click


        /* Descripción:
         *  Cancelación de la edición de una tabla de variables observadas. Si la tabla esta siendo creada
         *  se debe volver a dejar las tablas vacias y los objetos a null. Si la tabla estaba editando 
         *  un objeto creado se debe restaurar sus valores.
         */
        private void btActionCancelGenerateTableObs_Click()
        {
            if (this.editMultiFacetObs)
            {
                /* En este caso estamos editando la tabla de observaciones y por tanto debemos volver a 
                 * cargar los valores iniciales. */
                MultiFacetsObs multiFacets = sagtElements.GetMultiFacetsObs();
                loadDataInTabPageObsTable(multiFacets);
                
                // Ponemos la variable editarMultiFacets
                this.editMultiFacetObs = false;
                // Mostramos el checkBox de "Ocultar nulos"
                this.checkBoxHideNulls.Visible = true;
                this.checkBoxHideNulls.Enabled = true;
                this.checkBoxHideNulls.Checked = false;
            }
            else
            {
                /* En este caso no estamos editando niguna tabla de facetas sino que la estamos creando,
                 * con lo cual si la cancelamos debemos dejar las tablas de facetas y observaciones
                 * vacias y las variables a null. */
                
                // limpiamos los dataGridViewEx
                CancelAcciónEditionOfFacet();    
            }

            // ocultamos los botones de aceptar y cancelar
            disableButtonObsTable();
            // Activamos el menú principal poniendo la variable editionModeOn a false.
            this.editionModeOn = false;
            this.mStripData.Enabled = true;
            this.lf_global = null;
        }// end btActionCancelGenerateTableObs_Click


        /* Descripción:
         *  Lee los datos de un dataGridView que contiene una tabla de facetas. Devuelve una lista 
         *  de facetas con los datos que estan representados en el dataGridViewEx
         * Parámetros:
         *      DataGridViewEx.DataGridViewEx dgvEx: Es la tabla que contiene los datos de las facetas.
         */
        private ListFacets dgvExToListFacets(DataGridViewEx.DataGridViewEx dgvExFacets)
        {
            ListFacets lf = new ListFacets(); // valor de retorno
            try
            {
                //List<Facet> lf = new List<Facet>();

                int n = dgvExFacets.RowCount;
                for (int i = 0; i < n; i++)
                {
                    DataGridViewRow my_row = dgvExFacets.Rows[i];
                    string name = "";
                    if (my_row.Cells[IND_NAME].Value != null)
                    {
                        // Si la celda de nombre esta vacia lanzo una excepción
                        name = my_row.Cells[IND_NAME].Value.ToString();
                    }
                    int level = 0;
                    if (my_row.Cells[IND_LEVEL].Value != null)
                    {
                        level = int.Parse(my_row.Cells[IND_LEVEL].Value.ToString());
                    }
                    int size = 0;
                    if (my_row.Cells[IND_SIZE_OF_UNIVERSE_FACET].Value == null)
                    {
                        size = int.MaxValue;
                    }
                    else
                    {
                        if (my_row.Cells[IND_SIZE_OF_UNIVERSE_FACET].Value.ToString().ToUpper().Equals("INF"))
                        {
                            size = int.MaxValue;
                        }
                        else
                        {
                            size = int.Parse(my_row.Cells[IND_SIZE_OF_UNIVERSE_FACET].Value.ToString());
                        }
                    }

                    string comment = "";
                    if (my_row.Cells[IND_COMMENT].Value != null)
                    {
                        comment = my_row.Cells[IND_COMMENT].Value.ToString();
                    }

                    Facet f = new Facet(name, level, comment, size);
                    lf.Add(f);
                }
            }
            catch (FormatException formEx)
            {
                // Se produjo la excepción al obtener el nivel de la 
                // MessageBox.Show(formEx.Message);
                ShowMessageErrorOK(formEx.Message);
                lf = null;
            }
            catch (ListFacetsException facetEx)
            {
                // Se produjo una excecpción al crear una faceta
                // MessageBox.Show(facetEx.Message);
                ShowMessageErrorOK(facetEx.Message);
                lf = null;
            }
            catch (FacetException facetEx)
            {
                // Se produjo una excecpción al crear una faceta
                // MessageBox.Show(facetEx.Message);
                ShowMessageErrorOK(facetEx.Message);
                lf = null;
            }
            return lf;
        }// end dgvExToListFacets


        /*
         * Descripción:
         *  Si los datos son validos devuelve una lista de facetas. Si no son validos lanzará un mensaje
         *  de error y devolverá null.
         * Parámetros:
         *  DataGridViewEx.DataGridViewEx dgv: El dataGridView de donde obtemos los datos de las
         *  facetas
         */
        private ListFacets validateFacetTable(DataGridViewEx.DataGridViewEx dgv)
        {
            ListFacets valret = null; // variable de retorno
            try
            {
                valret = dgvExToListFacets(dgv);
                // this.multiFacets = new MultiFacetsObs(lf, "no name", tbDescription.Text);
            }
            catch (MultiFacetObsException multFactEx)
            {
                // se produjo un error al crear el objeto multifaceta
                // MessageBox.Show(multFactEx.Message);
                ShowMessageErrorOK(multFactEx.Message);
                valret = null;
            }
            
            return valret;
        } // private bool validateFacetTable()


        /*
         * Descripción:
         *  Cierra los elementos de tabPagedata
         */
        private void closeDataElements()
        {
            if(this.sagtElements.GetMultiFacetsObs() != null){
                // Deshabilitamos el checkBox de ocultar
                this.checkBoxHideNulls.Enabled = false;
                // limpiamos los campos
                clearDataElements();
                this.sagtElements.SetMultiFacetsObs(null);
                tabControlData.SelectedIndex = 0;
            }
        }


        /* Descripción:
         *  Limpia los campos del tabPageDatos.
         */
        private void clearDataElements()
        {
            CleanerDataGridViewExFacets(this.dataGridViewExFacets);
            this.dataGridViewExObsTable.Rows.Clear();
            this.dataGridViewExObsTable.ColumnHeadersVisible = false;
            this.dataGridViewExFacets.ColumnHeadersVisible = false;
            this.tbDescription.Text = "";
            this.tbDescription.ReadOnly = true;
            this.tbFileName.Text = "";
            this.richTextBoxDataComment.Text = "";
            this.checkBoxHideNulls.Checked = false;
        }


        /* Descripción:
         *  Introduce comentarios en la opción de tabla de frecuencias
         */
        private void btActionDataEditComment_Click(object sender, EventArgs e)
        {
            if ((sagtElements.GetMultiFacetsObs() == null))
            {
                ShowMessageErrorOK(errorNoFileSelected);
            }
            else
            {
                TransLibrary.Language lang = this.LanguageActually();
                string text = this.richTextBoxDataComment.Text;
                FormEditFileComment formEditComment = new FormEditFileComment(text, lang);
                bool salir = false;
                do
                {
                    DialogResult res = formEditComment.ShowDialog();
                    switch (res)
                    {
                        case DialogResult.Cancel: salir = true; break;
                        case DialogResult.OK:
                            MultiFacetsObs mfo = this.sagtElements.GetMultiFacetsObs();
                            mfo.Comment(formEditComment.GetRichTextBoxText());
                            this.richTextBoxDataComment.Text = mfo.Comment();
                            salir = true;
                            break;
                    }
                } while (!salir);
            }
        }// end btActionDataEditComment_Click


        #region Exprotar los datos a un archivo Excel

        /* Descripción:
         *  Exporta los datos de las facetas y la tabla de observaciones a un archivo Excel.
         */
        private void tsmiActionDataExportExcel_Click(object sender, EventArgs e)
        {
            if(this.sagtElements.GetMultiFacetsObs() != null)
            {
                SaveFileExcelDialog();
            }
        }


        /* Descripción:
         *  Cuadro de dialogo para guardar el archivo Excel.
         */
        public void SaveFileExcelDialog()
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
            saveDialog.Title = titleSave;
            // CuadroDialogo.InitialDirectory = @"c:\";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {/*
                ExcelApp.ActiveWorkbook.SaveCopyAs(CuadroDialogo.FileName);
                ExcelApp.ActiveWorkbook.Saved = true;
                CuadroDialogo.Dispose();
                CuadroDialogo = null;
                ExcelApp.Quit();
              */

                ExportExcel expExcel = new ExportExcel();
                expExcel.addXlsWorksheet(dataGridViewExObsTable, tabPageObsTable.Text);
                expExcel.addNewXlsWorksheet(dataGridViewExFacets, tabPagMultiFacet.Text);
                
                expExcel.saveFileExcel(saveDialog.FileName);
                

                // MessageBox.Show("Fin");
                saveDialog.Dispose();
                saveDialog = null;
                expExcel.aplicationExcelQuit();
            }
        }// end SaveFileExcelDialog
       

        #endregion Exportar los datos a un archivo Excel


        #region Importar los datos de un archivo Excel
        /*================================================================================================
         * Importa los datos de un archivo Excel
         * 
         * NOTA:
         *  Para ello emplea ADOX y oledb
         *================================================================================================*/

        /* Descripción:
         *  Toma los datos de un DataTable y genera con ellos una lista de facetas
         */
        private ListFacets DataTable2ListFacets(DataTable dt)
        {
            ListFacets lf = null;
            try
            {
                lf = new ListFacets();

                int r = dt.Rows.Count;

                for (int i = 0; i < r; i++)
                {
                    DataRow row = dt.Rows[i];

                    string design = (string)row[0].ToString();
                    string name = ExtractNameOfDesign(design);
                    int level = int.Parse((string)row[1].ToString());
                    int size = int.MaxValue;
                    string stSize = ((string)row[2].ToString()).Trim();
                    if (!stSize.Equals(Facet.INFINITE))
                    {
                        size = int.Parse(stSize);
                    }
                    string description = (string)row[3].ToString();

                    Facet f = new Facet(name, level, description, size, design);
                    lf.Add(f);
                }
            }
            catch (FormatException e)
            {
                // contiene un campo incorrecto
                throw e;
            }
            return lf;
        }// end DataTable2ListFacets


        /* Descripción:
         *  Método auxiliar. De un diseño obtiene el nombre de la faceta de la faceta
         */
        private string ExtractNameOfDesign(string design)
        {
            int posI = design.IndexOf("[");
            int posf = design.IndexOf("]");
            return (design.Substring(posI+1,posf-1));
        }


        /* Descripción:
         *  Devuelve la lista de observaciones leidas de un datatable
         */
        private InterfaceObsTable DataTable2Observation(DataTable dt, InterfaceObsTable obsTb)
        {
            InterfaceObsTable res = null;
            try
            {
                res = obsTb;
                int r = dt.Rows.Count;
                int c = dt.Columns.Count;

                for (int i = 0; i < r; i++)
                {
                    DataRow row = dt.Rows[i];
                    double? d = ConvertNum.String2Double((string)row[c-1].ToString());
                    res.Data(d,i);
                }
            }
            catch (FormatException)
            {
                // Hemos cometido un error al leer
                throw new ObsTableException(errorFormatFile);
            }
            return res;
        }

        #endregion Importar los datos de un archivo Excel


        /* Descripción:
         *  Exporta los datos en un fichero de texto que pueda ser recuperado por EduG
         */
        private void tsmiActionExportScore_Click(MultiFacetsObs multiFacet)
        {
            if (multiFacet == null)
            {
                ShowMessageErrorOK(errorNoTableObs);
                //MessageBox.Show(errorNoTableObs);
            }
            else
            {
                SaveFileDialog saveDialog = new SaveFileDialog();

                if (Directory.Exists(this.cfgApli.Get_Path_Workspace()))
                {
                    saveDialog.InitialDirectory = this.cfgApli.Get_Path_Workspace();
                }

                saveDialog.DefaultExt = DEFAULT_EXT_SCORE; // Extensión por defecto de un fichero de puntuaciones
                string filter = (filterDatas + FILTER_DATA + this.allFiles + FILTER_ALL_FILE);
                saveDialog.Filter = filter;
                saveDialog.OverwritePrompt = true; // muestra advertencia si el fichero ya existe
                saveDialog.AddExtension = true;
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    MultiFacetsObs multiFacetData = this.sagtElements.GetMultiFacetsObs();
                    bool res = multiFacetData.WritingFileDataScore(saveDialog.FileName);
                    if (res)
                    {
                        // Mostramos un mensaje de que las puntuaciones se han guardado
                        // MessageBox.Show(txtSaveScores, titleSaved);
                        ShowMessageInfo(txtSaveScores, titleSaved);
                    }
                    else
                    {
                        // Mostramos un mensaje ERROR, en el que las puntuaciones NO se han guardado
                        // MessageBox.Show(txtNoSaveScores, titleMessageError1);
                        ShowMessageInfo(txtSaveScores, titleSaved);
                    }
                }
                // Deshabilitamos los botones de facetas
                btGenerateTableObsDisables();
            }
        }// end tsmiActionExportScore_Click


        /* Descripción:
         *  Introduce los datos leidos de un fichero de datos en la tabla de observaciones.
         */
        private void btActionImportScores_Click(MultiFacetsObs multiFacet, DataGridViewEx.DataGridViewEx tableScores)
        {
            if (multiFacet == null)
            {
                ShowMessageErrorOK(errorNoTableObs);
                //MessageBox.Show(errorNoTableObs);
            }
            else
            {
                OpenFileDialog openDialog = new OpenFileDialog();

                if (Directory.Exists(this.cfgApli.Get_Path_Workspace()))
                {
                    openDialog.InitialDirectory = this.cfgApli.Get_Path_Workspace();
                }

                string fileFilter = (filterDatas + FILTER_DATA + this.allFiles + FILTER_ALL_FILE);
                openDialog.Filter = fileFilter;

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Leemos las puntuaciones
                        List<double> listScores = MultiFacetsObs.ReadingFileDataScore(openDialog.FileName);
                        // Introducimos los datos e informamos del resultado.
                        int numOfRows = tableScores.RowCount;
                        int numOfList = listScores.Count;
                        int n = numOfList;
                        if (numOfRows < numOfList)
                        {
                            n = numOfRows;
                        }

                        int nCols = tableScores.ColumnCount-1;

                        // Número de decimales para la representación
                        int numOfDecimal = cfgApli.GetNumberOfDecimals();
                        // Punto de separación decimal
                        string puntoDecimal = this.cfgApli.GetDecimalSeparator();

                        // Introducimos los datos en el datagridView;
                        for (int i = 0; i < n; i++)
                        {
                            // tableScores.Rows[i].Cells[nCols].Value = DecimalSetting.DecimalToString(listScores[i], numOfDecimal, puntoDecimal);
                            tableScores.Rows[i].Cells[nCols].Value = listScores[i].ToString();
                        }

                        // Mostramos mensaje
                        string message = txtInfoImportScores;
                        message = message.Replace("[n]", n.ToString());
                        // MessageBox.Show(message);
                        ShowMessageInfo(message);
                    }
                    catch (MultiFacetObsException)
                    {
                        // Mostramos un mensaje de error informando de que no se han podido extraer los datos
                        // MessageBox.Show(errorReadingFileScore, titleMessageError1);
                        ShowMessageErrorOK(errorReadingFileScore, titleMessageError1, MessageBoxIcon.Error);
                    }
                }
            }
        }// end btActionImportScores_Click


        /* Descripción:
         *  Muestra la ventana para la selección de niveles que serán omitidos en el estudio de generalizabilidad.
         */
        private void tsmiActionDataOmitLevels_Click(object sender, EventArgs e)
        {
            MultiFacetsObs multiFacets  = this.sagtElements.GetMultiFacetsObs();
            if (multiFacets == null)
            {// (* 1 *)
                ShowMessageErrorOK(errorNoTableObs);
                //MessageBox.Show(errorNoTableObs);
            }
            else
            {
                ListFacets listFacets = multiFacets.ListFacets();
                ListFacets lf = (ListFacets)listFacets.Clone(); 
                FormOmitLevelFacet formOmitLevelFacet = new FormOmitLevelFacet(cfgApli.GetConfigLanguage(), lf);

                bool salir = false;
                do
                {
                    DialogResult res = formOmitLevelFacet.ShowDialog();
                    switch (res)
                    {
                        case DialogResult.Cancel: 
                            salir = true; 
                            break;
                        case DialogResult.OK:

                            // Comprobamos que exista al menos un nivel de cada faceta no ha sido seleccionado
                            // en caso contrario lanzamos una excepción con el mensaje.
                            bool b = true;
                            
                            int n = lf.Count();
                            int i = 0;
                            while(b && i<n)
                            {
                                Facet f = lf.FacetInPos(i);
                                int levels = f.Level();
                                int j = 0;
                                while(b && j<levels)
                                {
                                    b = f.GetSkipLevels(j + 1);
                                    if (b)
                                    {
                                        j++;
                                    }
                                }
                                
                                if (j == (levels) && b)
                                {
                                    /* Si b es true y el indice j es igual al nivel significa
                                     * que todos los niveles estan omitidos y por tanto es una 
                                     * reducción no permitida.
                                     */
                                    b = false;
                                }
                                else
                                {
                                    /* Si j es menor que el nivel significa que al menos
                                     * se puede reducir a un nivel y debemos seguir
                                     * examinando el resto de facetas.
                                     */
                                    b = true;
                                }
                                i++;
                            }

                            // si se ha seleccionado todos los niveles de alguna faceta entonces
                            if (!b)
                            {
                                ShowMessageErrorOK(errorZeroLevelToReduce);
                            }
                            else
                            {
                                multiFacets.ListFacets(lf);
                                // Condición de salir
                                salir = true;
                            }
                            break;
                    }
                } while (!salir);

            }// end if (* 1 *)

        }// end tsmiActionDataOmitLevels_Click

        #region Cambio de idioma de los elementos del tabPageData
        /*
         * Descripción:
         *  Traduce los elementos del TabPageData.
         * Parámetros:
         *  TransLibrary.Language lang: idioma al que vamos a traducir los elementos.
         *  string nameFileTrans: Nombre del fichero que contiene las traducciones.
         */
        private void TranslationDataElements(TransLibrary.Language lang, string nameFileTrans)
        {
            TransLibrary.ReadFileTrans dic = new TransLibrary.ReadFileTrans(nameFileTrans);
            string name = "";
            try
            {
                // traducimos las etiquetas de las pestañas

                // Traducimos el tabPage: Facetas
                name = tabPagMultiFacet.Name.ToString();
                tabPagMultiFacet.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos el tabPage: Tabla de observaciones
                name = tabPageObsTable.Name.ToString();
                tabPageObsTable.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos el tabPage: Tabla de Editar Facetas
                name = tabPageDataEditFacets.Name.ToString();
                tabPageDataEditFacets.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos el tabPage: Información
                name = tabPageDataInfo.Name.ToString();
                tabPageDataInfo.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // traducimos la etiqueta de archivo                
                name = lbFileData.Name.ToString();
                lbFileData.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta en el tabPage de Editar Facetas
                lbEditFacetsFileData.Text = lbFileData.Text;

                // traducimos la etiqueta de comentario
                name = lbCommentsData.Name.ToString();
                lbCommentsData.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta en el tabPage de Editar Facetas
                lbEditFacetsCommentsData.Text = lbCommentsData.Text;

                // Traducimos el botón de anidar facetas
                name = btNestingFacet.Name.ToString();
                btNestingFacet.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                name = btRemoveNesting.Name.ToString();
                btRemoveNesting.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos el botón de generar tabla de observaciones
                name = btGenerateTableObs.Name.ToString();
                btGenerateTableObs.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // botón cancelar de facetas
                name = btDataFacetsCancel.Name.ToString();
                btDataFacetsCancel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Botón Importar puntuaciones
                name = btImportScores.Name.ToString();
                btImportScores.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // botón aceptar de editar tabla obs
                name = btDataObsSave.Name.ToString();
                btDataObsSave.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // botón Aceptar de Editar Facetas
                btDataEditFacetsSave.Text = btDataObsSave.Text;

                // botón cancelar de editar tabla obs
                name = btDataObsCancel.Name.ToString();
                btDataObsCancel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // botón cancelar de Editar Facetas
                btDataEditFacetsCancel.Text = btDataObsCancel.Text;

                // botón de editar comentarios (tabPageDataInfo) 
                name = btDataEditComment.Name.ToString();
                btDataEditComment.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos los nombres de las columnas de las tablas
                name = "nameColFacet";
                nameColFacet = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "nameColLevel";
                nameColLevel = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "nameColSizeOfUniverse";
                nameColSizeOfUniverse = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "nameColOmit";
                nameColOmit = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "measurementVariable";
                measurementVariable = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "nameColComment";
                nameColComment = dic.labelTraslation(name).GetTranslation(lang).ToString();

                if (dataGridViewExFacets.ColumnCount != 0)
                {
                    // Cambiamos el nombre de las columnas
                    dataGridViewExFacets.Columns[IND_NAME].HeaderText = nameColFacet; // Nombre de la columna Etiquetas (dependerá del idioma).
                    dataGridViewExFacets.Columns[IND_LEVEL].HeaderText = nameColLevel; // Nombre de la columna Niveles (dependerá del idioma).
                    dataGridViewExFacets.Columns[IND_SIZE_OF_UNIVERSE_FACET].HeaderText = nameColSizeOfUniverse;
                    dataGridViewExFacets.Columns[IND_COMMENT].HeaderText = nameColComment; // Nombre de la columna Descripción (dependerá del idioma).
                }

                if (dataGridViewExFacets.Columns["nameColOmit"] != null)
                {
                    dataGridViewExFacets.Columns["nameColOmit"].HeaderText = nameColOmit;
                }

                int n = dataGridViewExObsTable.ColumnCount-1;   // Contamos el número de columnas de la tabla de observaciones
                                                            // esta nos indicara si contiene datos o no.
                if (n > 0)
                {
                    // Cambiamos el nombre de la columna variable observada.
                    dataGridViewExObsTable.Columns[n].HeaderText = measurementVariable;
                }

                // Traducimos los menus contextuales de los dos dataGridViewEx
                TranslationTContextualMenu(dataGridViewExFacets, dicMeans, lang);
                TranslationTContextualMenu(dataGridViewExObsTable, dicMeans, lang);
                // Traducimos el checkBox de "Ocultar nulos"
                name = checkBoxHideNulls.Name.ToString();
                checkBoxHideNulls.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                // MessageBox.Show(lEx.Message + " Se produjo un error al traducir " + name);
                ShowMessageErrorOK(lEx.Message + " " + errorMessageTraslation + " " + name);
            }
        } // private void TraslationDataElements
        #endregion Cambio de idioma de los elementos del tabPageData


    } // end public partial class FormPrincipal : Form
} // end namespace GUI_TG
