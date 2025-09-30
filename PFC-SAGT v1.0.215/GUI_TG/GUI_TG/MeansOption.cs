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
 *      Clase parcial ("partial") del FormPrincipal. Contiene los métodos referentes a la parte de
 *      Medias: generación de valores estadisticos a partir de una lista de Facetas y la tabla de 
 *      observaciones.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MultiFacetData;
using System.Drawing;
using ProjectMeans;
using ImportEduGMeans;
using ProjectSSQ;
using AuxMathCalcGT;
using System.Threading;
using Sagt;


namespace GUI_GT
{
    public partial class FormPrincipal : Form
    {
        #region Variables relaccionadas con la opción de Medias
        /*=============================================================================================
         * Variables relaccionadas con la opción de Medias
         *=============================================================================================*/

        // Cabeceras de las columnas pertenecientes al tabPage de datos
        //=============================================================

        // Nombres de la columnas de la tabla de facetas
        private string nameColMeans = "Medias"; // Nombre de la columna Medias (dependerá del idioma).
        private string nameColVariance = "Varianza"; // Nombre de la columna Varianza (dependerá del idioma).
        private string nameColStd_Dev = "Desviación Típica"; // Nombre de la columna Desviación tipica (dependerá del idioma).

        // Etiquetas que se muestran bajo ta tabla
        private string txtGrandMean = "Gran Media";

        // Nombre de las columnas adicionales de Tabla de medias de diferencias
        private string nameColDiffMean = "Diferencia de medias";
        private string nameColDiffVar = "Diferencia de varianzas";
        private string nameColDiffStd_dev = "Dif. de la desv. típica";

        // Nombre de las columnas adicionales de Tabla de medias de puntuación típica.
        private string nameColTypScore = "Puntuación típica";


        #endregion Variables relaccionadas con la opción de Medias




        /* Descripción:
         *  Devuelve la lista de medias.
         */
        public ListMeans ListMeans()
        {
            return this.sagtElements.GetListMeans();
        }


        /*
         * Descripción:
         *  Inicializa los valores y las variables relacionadas con el TabPageMeans
         */
        private void InitializeMeansOptions()
        {
            // ocultamos el tabPage de información
            this.tbPageMeanInfo.Parent = null;
        }


        /*
         * Descipción:
         *  Añade los tabPage de medias a partir de la medias selecciónadas que se pasa como parámetro.
         * Parámetros:
         *      List<ListFacets> selectListFacets: Lista de facetas en función de las cuales se crearan las
         *                  medias.
         *      List<string> ldesign: Lista que contiene los strings con los diseños de las facetas en el mismo orden que
         *                  selectListFacets.
         *      MultiFacetsObs multiFacetObsData: Objeto multifaceta a partir del cual se calculan las medias.
         *      
         *      string nameFileTrans: Fichero con las traducciónes de las etiqueta y cabeceras de columnas.
         */
        public void createMeansOfListOfListFacets(List<ListFacets> selectListFacets, List<string> ldesign, MultiFacetsObs multiFacetObsData, 
            ConfigCFG.ConfigCFG cfgApli, string nameFileTrans)
        {
            ListMeans listMeans = new ListMeans();

            bool zero = cfgApli.GetNull_to_Zero();

            switch (cfgApli.GetTypeOfTableMeans())
            {// (*1*)
                case(ConfigCFG.TypeOfTableMeans.Default):
                    int n = selectListFacets.Count();
                    for (int i = 0; i < n; i++)
                    {
                        string design = ldesign[i];
                        ListFacets lf = selectListFacets[i];
                        listMeans.Add(new TableMeans(lf, design, multiFacetObsData, zero));
                    }
                    break;
                case (ConfigCFG.TypeOfTableMeans.TableMeansDif):
                    n = selectListFacets.Count();
                    for (int i = 0; i < n; i++)
                    {
                        string design = ldesign[i];
                        ListFacets lf = selectListFacets[i];
                        listMeans.Add(new TableMeansDif(lf, design, multiFacetObsData, zero));
                    }
                    break;
                case (ConfigCFG.TypeOfTableMeans.TableMeansTipPoint):
                    n = selectListFacets.Count();
                    for (int i = 0; i < n ; i++)
                    {
                        string design = ldesign[i];
                        ListFacets lf = selectListFacets[i];
                        listMeans.Add(new TableMeansTypScore(lf, design, multiFacetObsData, zero));
                    }
                    break;
                default:
                    ShowMessageErrorOK(errorNoTableMeansDispose);
                    break;
            }// end switch (*1*)

            // Guardamos los datos de la creación de las tablas de medias
            string nameFile = multiFacetObsData.NameFileObs(); // obtenemos el nombre del fichero de datos.
            listMeans.SetNameFileDataCreation(nameFile); // asignamos el nombre del fichero a partir del cual hemos generado los datos.
            
            DateTime date = DateTime.Now;
            listMeans.SetDateTime(date);
            this.sagtElements.SetListMeans(listMeans); // actualizamos la variable;
            // Mostramos los datos en las tablas
            listOfTableMeansToTabPageMeans(listMeans);
        }// end createMeansOfListOfListFacets


        /*
         * Descipción:
         *  Añade los tabPage de medias a partir de el objeto listMeans.
         * Parámetros:
         *      ListMeans listM: Lista de medias.
         */
        private void listOfTableMeansToTabPageMeans(ListMeans listM)
        {
            // Pasamos las etiquetas en el idioma correcto
            TransLibrary.Language lang = this.LanguageActually();
            TransLibrary.ReadFileTrans dic = new TransLibrary.ReadFileTrans(Application.StartupPath + LANG_PATH + MEAN_STRINGS);

            // Asignamos los textos traducidos
            string gm = dic.labelTraslation(txtGrandMean).GetTranslation(lang).ToString();
            string means = dic.labelTraslation(nameColMeans).GetTranslation(lang).ToString();
            string var = dic.labelTraslation(nameColVariance).GetTranslation(lang).ToString();
            string stdv = dic.labelTraslation(nameColStd_Dev).GetTranslation(lang).ToString();

            string cut = dicMeans.labelTraslation("Cut").GetTranslation(lang).ToString();
            string copy = dicMeans.labelTraslation("Copy").GetTranslation(lang).ToString();
            string paste = dicMeans.labelTraslation("Paste").GetTranslation(lang).ToString();
            string remove = dicMeans.labelTraslation("Remove").GetTranslation(lang).ToString();
            // el siguente item es el separador y por eso nos lo saltamos
            string selectAll = dicMeans.labelTraslation("SelectAll").GetTranslation(lang).ToString();

            Image blackground = this.tbPageMeanInfo.BackgroundImage;
            ListMeans listMeans = listM;
            // foreach (TableMeans tmeans in listMeans)

            foreach (InterfaceTableMeans tmeans in listMeans)
            {
                AddTabPageTableMeans(tmeans, blackground, gm, means, var, stdv, cut, copy, paste, remove, selectAll);
            }

            // this.tableMeans = new TableMeans(lf,this.multiFacets);
            // this.loadMeansInDataGridView(this.listMeans.TableMeansInPos(0),this.dataGridViewMeans);
            this.tabPageMeansMain.Parent = null;
            // this.tabPageScondMeans.Parent = null;

            this.tbFileMeanProvede.Text = listMeans.GetNameFileDataCreation();
            this.tbDateMeanCreated.Text = listMeans.GetDateTime().ToString();
            this.rTxtBoxMeanInfo.Text = listMeans.GetTextComment();
            this.tbPageMeanInfo.Parent = this.tabControlMeans;
        }


        /*
         * Descripción:
         *  Añade un tabPage nuevo al tabControl de medias que contiene un dataGridView
         * Parámetros:
         *      TableMeans tmeans: Tabla con los datos que vamos a escribir en el dataGridViewEx
         */
        private void AddTabPageTableMeans(InterfaceTableMeans tmeans, Image blackground, string grandMean, string mean, string var,
            string stdv, string cut, string copy, string paste, string remove, string selectAll)
        {
            TabPageMeansEx newTabPage = new TabPageMeansEx(blackground, tmeans.FacetDesign(),
                this.txtGrandMean, this.lbGrandMean.Location, grandMean, mean, var, stdv, cut, copy,
                paste, remove, selectAll);

            // Determinamos el tipo al que pertenece
            // tomamos el tipo original;
            ListMeans listMeans = this.sagtElements.GetListMeans();
            InterfaceTableMeans tb = listMeans.TableMeansInPos(0);
            // par poder comparar el tipo
            TableMeans tbM = new TableMeans();
            TableMeansDif tbDiff = new TableMeansDif();
            TableMeansTypScore tbPointScore = new TableMeansTypScore();

            if (tb.GetType() == tbM.GetType())
            {
                newTabPage.SetTableMeans((TableMeans)tmeans, this.cfgApli);
            }
            else if (tb.GetType() == tbDiff.GetType())
            {
                newTabPage.SetTableMeans((TableMeansDif)tmeans, this.cfgApli);
            }
            else if (tb.GetType() == tbPointScore.GetType())
            {
                newTabPage.SetTableMeans((TableMeansTypScore)tmeans, this.cfgApli);
            }
            else
            {
                // Muestra un mensaje indicando que se produjo un error al traducir.
                ShowMessageErrorOK(errorMessageTraslation);
            }
            
            tabControlMeans.TabPages.Add(newTabPage);

        }// end AddTabPageTableMeans

          

        /* Descripción:
         *  Abre un archivo de medias.
         */
        private void tsmiActionOpenMeans_Click(object sender, EventArgs e)
        {
            DialogResult res = DialogResult.OK;
            ListMeans listMeans = this.sagtElements.GetListMeans();
            if (listMeans != null)
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

                string fileFilter = ("SAGT file" + FILTER_SAGT_FILE + this.allFiles + FILTER_ALL_FILE);
                openDialog.Filter = fileFilter;

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        //listMeans = ProjectMeans.ListMeans.ReadingFileListMeans(openDialog.FileName);
                        this.sagtElements = SagtFile.ReadingSagtFile(openDialog.FileName);
                        listMeans = this.sagtElements.GetListMeans();
                        //if (listMeans != null)
                        //{
                        //    // cerramos todos los tabPage
                        //    ClearTabPageMeans(listMeans);
                        //    listOfTableMeansToTabPageMeans(listMeans, MEAN_STRINGS);
                        //}
                        loadSagtElements(openDialog.FileName, sagtElements);
                    }catch(ListMeansException)
                    {
                        // Mostramos un mensaje de error al leer el acrchivo
                        ShowMessageInfo(errorReadingFile, titleMessageError1);
                    } 
                }
            }
        }// end private void tsmiActionOpenMeans_Click


        /* Descripción:
         *  Salva los datos los datos de las tablas de medias en un archivo.
         */
        private void tsmiActionMeansSave_Click(object sender, EventArgs e)
        {
            ListMeans listMeans = this.sagtElements.GetListMeans();
            if (listMeans == null)
            {
                // si no tenemos lista de medias lanzamos un mensaje de error
                ShowMessageErrorOK(errorNoListMeans);
            }
            else
            {
                //// Preguntamos al usuario por el archivo
                //SaveFileDialog saveDialog = new SaveFileDialog();
                //saveDialog.DefaultExt = "mean";
                //saveDialog.Filter = "mean file(*.mean)|*.mean";
                //saveDialog.OverwritePrompt = true; // muestra advertencia si el fichero ya existe
                //saveDialog.AddExtension = true;
                //if (saveDialog.ShowDialog() == DialogResult.OK)
                //{
                //    // listMeans.WritingFileListMeans(saveDialog.FileName);
                //    this.sagtElements.WritingSagtFile(saveDialog.FileName);
                //    MessageBox.Show("Las datos medias se han guardado", "Guardado");
                //}
               SaveFileSagt(this.sagtElements);
            }
        }// end private void tsmiActionMeansSave_Click


        /*
         * Descripción:
         *  Elimina los tabPage y vacia los dataGridViewEx de medias.
         */
        public void ClearTabPageMeans()
        {
            // elimina todos los tabPage
            tabControlMeans.TabPages.Clear();
            this.tabControlMeans.TabPages.Add(this.tabPageMeansMain);
            // this.tabPageMeansMain.Parent = null;
            // sagtElements.SetListMeans(null);
        }


        /* Descripción:
         *  Tras pedir confirmación cierra todas las medias.
         */
        private void tsmiActionMeansClose_Click(object sender, EventArgs e)
        {
            ListMeans listMeans = this.sagtElements.GetListMeans();
            if (listMeans != null)
            {
                /*
                TransLibrary.Language lang = this.cfgApli.GetConfigLanguage();

                string ok = this.dicMessage.labelTraslation("btOk").LangTraslation(lang).ToString();
                string cancel = this.dicMessage.labelTraslation("btCancel").LangTraslation(lang).ToString();
                MsgBoxUtil.HackMessageBox(ok, cancel);
                DialogResult res = MessageBox.Show(txtConfirmClose,
                    titleConfirm, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                 */
                DialogResult res = ShowMessageDialog(titleConfirm, txtConfirmClose);

                if (res == DialogResult.OK)
                {
                    // cerramos todos los tabPage
                    ClearTabPageMeans();
                    this.sagtElements.SetListMeans(null);
                }
            }
        }


        /* Descripción:
         *  Este método es llamado al pulsar en la opción "Importar" del menú vertical de "Medias".
         *  Muestra la ventana de importación de tipos de archivos.
         */
        private void tsmiActionImportMeans_Click(object sender, EventArgs e)
        {
            // CWait fw = new CWait(msgLoading);
            // Thread th = new Thread(new ThreadStart(fw.CWaitShowDialog));

            try
            {
                TransLibrary.Language lang = this.LanguageActually();
                FormMeansImport formImport = new FormMeansImport(lang);
                //this.Visible = false;
                bool salir = false;

                do
                {
                    DialogResult res = formImport.ShowDialog();
                    // quitamos y ponemos el foco en la ventana para que esta se actualize
                    this.Enabled = false;
                    this.Enabled = true;

                    switch (res)
                    {
                        case DialogResult.Cancel: salir = true; break;
                        case DialogResult.OK:
                            if (String.IsNullOrEmpty(formImport.FileName()))
                            {
                                // lanzamos un mensaje de error: no hay fichero seleccionado
                                this.ShowMessageErrorOK(errorNoFileSelected);
                            }
                            else
                            {
                                /* Primero compruebo si hay tablas de medias activas
                                 */
                                ListMeans lm = this.sagtElements.GetListMeans();
                                bool bReadfile = true;

                                if (lm != null)
                                {
                                    /* Hay tablas de medias activas. Preguntamos si desea continuar */
                                    DialogResult res2 = ShowMessageDialog(titleConfirm, txtConfirmClose);

                                    if (res2 == DialogResult.OK)
                                    {
                                        // cerramos todos los tabPage
                                        ClearTabPageMeans();
                                        this.sagtElements.SetListMeans(null);
                                        // Refrescamos la ventana
                                        this.Refresh();

                                        Application.DoEvents();
                                    }
                                    else
                                    {
                                        bReadfile = false;
                                    }
                                }

                                if (bReadfile)
                                {
                                    //CWait fw = new CWait(msgLoading);
                                    //Thread th = new Thread(new ThreadStart(fw.CWaitShowDialog));

                                    // Start the thread
                                    // th.Start();
                                    this.importMeansFile(formImport.pathFile(), MEAN_STRINGS);
                                    // th.Abort();
                                }
                                salir = true;
                            }
                            break;
                    }
                } while (!salir);
            }
            catch (TransLibrary.LabelTranslationException l)
            {
                //th.Abort();
                /* Mostramos un mensaje de error al traducir*/
                MessageBox.Show(l.Message);
            }
            catch (Exception ex)
            {
                // th.Abort();
                /* Mostramos un mensaje de error en el que se indicará que el 
                 * fichero no esta en el fomato correcto. */
                MessageBox.Show(ex.Message);
            }
        }// end tsmiActionImportMeans_Click


        /* Descripción:
         *  Importa un fichero de medias para construir el objeto lista de medias.
         */
        public void importMeansFile(string path, string nameFileTras)
        {
            //this.formImport.Close();

            //FormWaiting fw = new FormWaiting();
            //fw.Show();

            // Extraemos el nombre del fichero del path
            string fileExt = fileExtension(path).ToLower(); // Pasamos a minúsculas la extensión
            // para poder compararla. 
            switch (fileExt)
            {
                case (DEFAULT_EXT_RSM): loadListOfMeansOfFileRms(path); break;
                case ("txt"): loadListOfMeansOfFileText_EduG(path, nameFileTras); break;
                case ("rtf"): loadListOfMeansOfFileRtf_EduG(path); break;
                case (DEFAULT_EXT_EXCEL): loadListTableMeansFileXls(path); break; 
                default:
                    MessageBox.Show("No se muestra ninguno");
                    break;
            }
            //fw.Close();
        }


        /* Descripción:
         *  Crear la lista de tablas de medias a partir de un objeto mutifaceta exportado de un fichero
         *  .rsm (resultado de medias GT E 2.0)
         * Parámetros:
         *      string path: path donde se encuentra el fichero rms que vamos a leer.
         */
        private void loadListOfMeansOfFileRms(string path)
        {
            MultiFacetsObs auxMultiFacetObs = ListMeansPY.ListMeansPY.ReadFileRsmPY(path);
            ListMeans listMeans = this.sagtElements.GetListMeans();
            ClearTabPageMeans();
            ListFacets lf = auxMultiFacetObs.ListFacets();
            // List<ListFacets> auxListFacets = lf.CombinationWithoutRepetition();
            List<ListFacets> auxListFacets = new List<ListFacets>();
            List<string> cswr = lf.CombinationStringWithoutRepetition();
            foreach(string design in cswr)
            {
                ListFacets newlf = lf.ListDesignFacets(design);
                auxListFacets.Add(newlf);
            }
            createMeansOfListOfListFacets(auxListFacets, cswr, auxMultiFacetObs, this.cfgApli, MEAN_STRINGS);
        }


        /* Descripción:
         *  Lee los datos del fichero que se pasa como parámetro y crea una lista de tablas de
         *  medias.
         * Parámetros:
         *      string path: Path donde se encuentra el fichero
         *      string nameFileTras: nombre del fichero donde estan las traducciónes para mostrar las
         *          cabeceras de las columnas y las etiquetas en el idioma correspondiente.
         */
        private void loadListOfMeansOfFileText_EduG(string path, string nameFileTras)
        {
            //CWait fw = new CWait(msgLoading);
            //Thread th = new Thread(new ThreadStart(fw.CWaitShowDialog));

            try
            {
                // Refrescamos la ventana
                this.Refresh();
                Application.DoEvents();
                // Start the thread
                //th.Start();

                List<ListMeansEduG> listOfListMeans = ListMeansEduG.ReadFileReportTxtEduG(path, cfgApli.GetTypeOfTableMeans());
                List<string> listString = new List<string>();

                for (int i = 0; i < listOfListMeans.Count; i++)
                {
                    listString.Add(nameColMeans + " " + (i + 1) + ";   " + listOfListMeans[i].GetDateTime().ToString());
                }

                //th.Abort();

                TransLibrary.Language lang = this.cfgApli.GetConfigLanguage();
                FormSelectionOneItemReport formSelectionOne = new FormSelectionOneItemReport(listString, lang);
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
                            if (pos >= 0 && pos <= listOfListMeans.Count)
                            {
                                ListMeans listMeans = listOfListMeans[pos];
                                // ListMeans listMeans = ListMeansEduG.ReadFileReportRtfEduG(path);
                                listMeans.SetNameFileDataCreation(path);
                                DateTime date = DateTime.Now;
                                listMeans.SetDateTime(date);
                                this.sagtElements.SetListMeans(listMeans);
                                listOfTableMeansToTabPageMeans(listMeans);
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
            }
            catch (ListMeansEduG_Exception)
            {
                //th.Abort();
                ShowMessageErrorOK(errorFormatFile);
            }
        }// end loadListOfMeansOfFileText_EduG


        /* Descripción:
         *  Lee los datos del fichero que se pasa como parámetro y crea una lista de tablas de
         *  medias.
         */
        private void loadListOfMeansOfFileRtf_EduG(string path)
        {
            //CWait fw = new CWait(msgLoading);
            //Thread th = new Thread(new ThreadStart(fw.CWaitShowDialog));
            //CWait fw2 = new CWait(msgLoading);
            //Thread th2 = new Thread(new ThreadStart(fw2.CWaitShowDialog));

            try
            {
                // Refrescamos la ventana
                this.Refresh();
                Application.DoEvents();
                // Start the thread
                //th.Start();

                List<ListMeansEduG> listOfListMeans = ListMeansEduG.ReadFileReportRtfEduG(path, cfgApli.GetTypeOfTableMeans());
                List<string> listString = new List<string>();
                
                for (int i = 0; i < listOfListMeans.Count; i++)
                {
                    listString.Add(nameColMeans + " " + (i + 1) + ";   " + listOfListMeans[i].GetDateTime().ToString()); 
                }

                //th.Abort();
                TransLibrary.Language lang = this.cfgApli.GetConfigLanguage();
                FormSelectionOneItemReport formSelectionOne = new FormSelectionOneItemReport(listString, lang);
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
                            if (pos >= 0 && pos <= listOfListMeans.Count)
                            {
                                //th2.Start();
                                ListMeans listMeans = listOfListMeans[pos];
                                // ListMeans listMeans = ListMeansEduG.ReadFileReportRtfEduG(path);
                                listMeans.SetNameFileDataCreation(path);
                                DateTime date = DateTime.Now;
                                listMeans.SetDateTime(date);
                                this.sagtElements.SetListMeans(listMeans);
                                listOfTableMeansToTabPageMeans(listMeans);
                                //th2.Abort();
                                salir = true;
                            }
                            else
                            {
                                // Mostramos un mensaje de error mostrando que no se ha seleccionado ninguno
                                ShowMessageErrorOK(txtMessageNoSelected);
                            }
                            break;
                    }
                }while(!salir);
            }
            catch (ListMeansEduG_Exception)
            {
                //th.Abort();
                //th2.Abort();
                ShowMessageErrorOK(errorFormatFile);
            }
        }


        /* Descripción:
         *  Importa los datos de la tablas de medias de un fichero xls
         */
        private void loadListTableMeansFileXls(string path)
        {
            List<string> namesTables = ImportExcel.GetTableExcel(path);
            if (namesTables.Count < 2)
            {
                // No esta en el formato correcto
                ShowMessageErrorOK(errorFormatFile);
            }
            else
            {
                ListMeans lm = new ListMeans();
                int n = namesTables.Count;
                List<string> namesTablesaux = ImportExcel.GetTableExcel(path);
                DataTable dtGrandMeans = ImportExcel.GetDataTableExcel(path, "Grand Mean$");

                for (int i = 0; i < n; i++)
                {
                    string nameTable = namesTables[i];
                    if (!nameTable.Contains("Grand Mean"))
                    {
                        DataTable dtMeansTable = ImportExcel.GetDataTableExcel(path, nameTable);

                        // Necesiatamos averiagura el tipo de tabla de medias
                        TransLibrary.ReadFileTrans dic = new TransLibrary.ReadFileTrans(Application.StartupPath + LANG_PATH + MEAN_STRINGS);
                        TransLibrary.WordTranslation tras = dic.labelTraslation("Medias");

                        string tbDesign = dtGrandMeans.Rows[i-1][1].ToString();
                        double? gm = ConvertNum.String2Double((string)dtGrandMeans.Rows[i - 1][2].ToString());
                        double? variance = ConvertNum.String2Double((string)dtGrandMeans.Rows[i - 1][3].ToString());
                        double? stdDev = ConvertNum.String2Double((string)dtGrandMeans.Rows[i - 1][4].ToString());
                        InterfaceTableMeans tbMeans = DataTable2TableMeans(dtMeansTable, gm, variance, stdDev, tbDesign, tras);
                        lm.Add(tbMeans);
                    }
                }
                lm.SetNameFileDataCreation(path);
                DateTime date = DateTime.Now;
                lm.SetDateTime(date);
                this.sagtElements.SetListMeans(lm);
                // Mostramos los datos en las tablas
                listOfTableMeansToTabPageMeans(lm);
            }
        }// loadListTableMeansFileXls


        /* Descripción:
         *  Muestra la ventana para editar los comentarios en la opción de medias.
         */
        private void btActionMeanEditComment_Click(object sender, EventArgs e)
        {
            if ((sagtElements.GetListMeans() == null))
            {
                ShowMessageErrorOK(errorNoFileSelected);
            }
            else
            {
                TransLibrary.Language lang = this.LanguageActually();
                string text = this.rTxtBoxMeanInfo.Text;
                FormEditFileComment formEditComment = new FormEditFileComment(text, lang);
                bool salir = false;
                do
                {
                    DialogResult res = formEditComment.ShowDialog();
                    switch (res)
                    {
                        case DialogResult.Cancel: salir = true; break;
                        case DialogResult.OK:
                            ListMeans listMeans = this.sagtElements.GetListMeans();
                            listMeans.SetTextComment(formEditComment.GetRichTextBoxText());
                            this.rTxtBoxMeanInfo.Text = listMeans.GetTextComment();

                            salir = true;
                            break;
                    }
                } while (!salir);
            }
            
        }// end btActionMeanEditComment_Click


        /* Descripción:
         *  Generar un archivo Excel a partir de los datos contenidos en las tablas medias.
         */
        private void tsmiActionMeansExportExcel_Click(object sender, EventArgs e)
        {
            ListMeans listMeans = this.sagtElements.GetListMeans();
            if (listMeans != null)
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
                    int nlistMeans = listMeans.Count();
                    int pos = nlistMeans-1;

                    // Exportamos la tablas con los diseños (nombre de las pestañas y los datos de la grand media)
                    DataGridView dgvGrandMean = AuxDataGridViewGrandMeans(listMeans);
                    expExcel.addXlsWorksheet(dgvGrandMean, "Grand Mean");

                    // Exportamos el resto de las tablas
                    TabPageMeansEx tp = (TabPageMeansEx)tabControlMeans.TabPages[pos];
                    DataGridViewEx.DataGridViewEx dgvExMean = tp.GetDataGridViewEx();
                    expExcel.addNewXlsWorksheet(dgvExMean, "Media " + (nlistMeans));
                    // expExcel.addXlsWorksheet(dgvExMean, tp.Text);

                    pos--;
                    nlistMeans--;
                    // this.tabControlMeans.
                    for (int i = pos; i >=0 ; i--)
                    {
                        tp = (TabPageMeansEx)tabControlMeans.TabPages[i];
                        dgvExMean = tp.GetDataGridViewEx();
                        expExcel.addNewXlsWorksheet(dgvExMean, "Media " + nlistMeans);
                        nlistMeans--;
                    }

                    expExcel.saveFileExcel(saveDialog.FileName);


                    // MessageBox.Show("Fin");
                    saveDialog.Dispose();
                    saveDialog = null;
                    expExcel.aplicationExcelQuit();
                }
            }
        }// end tsmiActionMeansExportExcel_Click


        /* Descripción:
         *  Operación Auxiliar que crea dataGrid con diversos datos para facilitar la importación de las tablas
         *  de medias
         */
        private DataGridView AuxDataGridViewGrandMeans(ListMeans lm)
        {
            DataGridView dgvGrandMeans = new DataGridView();
            dgvGrandMeans.Columns.Add("means", "Name");
            dgvGrandMeans.Columns.Add("tabName", "Design");
            dgvGrandMeans.Columns.Add("grandMean", txtGrandMean);
            dgvGrandMeans.Columns.Add("variance", nameColVariance);
            dgvGrandMeans.Columns.Add("standarDev", nameColStd_Dev);
            // rellenamos la tabla
            int n = tabControlMeans.TabPages.Count-1;

            for (int i = 0; i < n; i++)
            {
                TabPageMeansEx tp = (TabPageMeansEx)tabControlMeans.TabPages[i];
                InterfaceTableMeans tm = lm.TableMeansInPos(i);

                object[] my_Row = new object[5];
                my_Row[0] = "Media " + (i+1);
                my_Row[1] = tp.Text;
                my_Row[2] = tm.GrandMean();
                my_Row[3] = tm.Variance();
                my_Row[4] = tm.StdDev();

                dgvGrandMeans.Rows.Add(my_Row);
            }
            return dgvGrandMeans;
        }


        /* Descripción:
         *  Devuelve una tabla de medias
         */
        private InterfaceTableMeans DataTable2TableMeans(DataTable dt, double? grandMean,
            double? variance, double? stdDev, string tbDesign, TransLibrary.WordTranslation trans)
        {
            InterfaceTableMeans tm = null;

            /* Necesito averiguar que tipo de tabla de medias es. Lo averiguare mediante la posición
             * de la columna media.
             */
            int r = dt.Rows.Count;
            int c = dt.Columns.Count;
            int pos = c-1;

            bool found = false;
            
            while (pos >= 0 && !found)
            {
                string sMeans = dt.Columns[pos].ColumnName;
                found = trans.TranslationIncluded(sMeans);
                if (!found)
                {
                    pos--;
                }
            }

            // Si es pos == c-4 entonces medias por defecto
            if (pos == (c - 3))
            {
                tm = new TableMeans(dt, grandMean, variance, stdDev, tbDesign);
            }
            // Si es pos == c-5 entonces puntuación típica
            if (pos == (c - 5))
            {
                tm = new TableMeansTypScore(dt, grandMean, variance, stdDev, tbDesign);
            }
            // Si es pos == c-6 entonces medias de las desviaciones
            if (pos == (c - 6))
            {
                tm = new TableMeansDif(dt, grandMean, variance, stdDev, tbDesign);
            }

            return tm;
        }// end DataTable2TableMeans


        #region Cambio de idioma de los elementos del tabPageMeans
        /*
         * Descripción:
         *  Traduce los elementos del TabPageMeans.
         * Parámetros:
         *  TransLibrary.Language lang: idioma al que vamos a traducir los elementos.
         *  string nameFileTrans: Nombre del fichero que contiene las traducciones.
         */
        private void TranslationMeansElements(TransLibrary.Language lang, string nameFileTrans)
        {
            TransLibrary.ReadFileTrans dic = new TransLibrary.ReadFileTrans(nameFileTrans);
            string name = "";
            try
            {
                // etiqueta del tabPage Medias
                string means = dic.labelTraslation(nameColMeans).GetTranslation(lang).ToString();
                tabPageMeansMain.Text = means;
                // textos inferiores:
                string grandMean = dic.labelTraslation(txtGrandMean).GetTranslation(lang).ToString();
                lbGrandMean.Text = grandMean;
                string variance = dic.labelTraslation(nameColVariance).GetTranslation(lang).ToString();
                lbMeanVariance.Text = variance;
                string stdv = dic.labelTraslation(nameColStd_Dev).GetTranslation(lang).ToString();
                // Etiqueta de desviación standar
                // lbStandDev.Text = stdv;
                lbStdDv.Text = stdv;
                string diffMean = dic.labelTraslation(nameColDiffMean).GetTranslation(lang).ToString();
                string diffVar = dic.labelTraslation(nameColDiffVar).GetTranslation(lang).ToString();
                string diffStd_Dev = dic.labelTraslation(nameColDiffStd_dev).GetTranslation(lang).ToString();
                string TypScore = dic.labelTraslation(nameColTypScore).GetTranslation(lang).ToString();

                // etiqueta del tabPage Info
                name = this.tbPageMeanInfo.Name.ToString();
                this.tbPageMeanInfo.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // labels contenidos en el tabPage Info 
                name = this.lbFileMeanProvede.Name.ToString();
                this.lbFileMeanProvede.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbDateMeanCreated.Name.ToString();
                this.lbDateMeanCreated.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Boton editar
                name = this.btMeanEditComment.Name.ToString();
                this.btMeanEditComment.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                int numOfDecimal = this.cfgApli.GetNumberOfDecimals();
                string puntoDecimal = this.cfgApli.GetDecimalSeparator();

                ListMeans listMeans = this.sagtElements.GetListMeans();

                if (listMeans != null)
                {
                    this.tbPageMeanInfo.Parent = null;// no contiene una tabla de medias así que lo quitamos
                    //foreach (DataGridViewEx.DataGridViewEx dgvEx in this.listOfDataGridViewEx)
                    foreach (TabPage tp in this.tabControlMeans.TabPages)
                    {
                        TabPageMeansEx tpEx = (TabPageMeansEx)tp;
                        DataGridViewEx.DataGridViewEx dgvEx = tpEx.GetDataGridViewEx();

                        int n = dgvEx.NumeroColumnas;
                        // tomamos el tipo original;
                        InterfaceTableMeans tb = listMeans.TableMeansInPos(0);
                        // par poder comparar el tipo
                        TableMeans tbM = new TableMeans();
                        TableMeansDif tbDiff = new TableMeansDif();
                        TableMeansTypScore tbPointScore = new TableMeansTypScore();

                        if (tb.GetType() == tbM.GetType())
                        {
                            dgvEx.Columns[n - 3].HeaderText = means;
                            dgvEx.Columns[n - 2].HeaderText = variance;
                            dgvEx.Columns[n - 1].HeaderText = stdv;
                        }
                        else if (tb.GetType() == tbDiff.GetType())
                        {
                            dgvEx.Columns[n - 6].HeaderText = means;
                            dgvEx.Columns[n - 5].HeaderText = variance;
                            dgvEx.Columns[n - 4].HeaderText = stdv;
                            dgvEx.Columns[n - 3].HeaderText = diffMean;
                            dgvEx.Columns[n - 2].HeaderText = diffVar;
                            dgvEx.Columns[n - 1].HeaderText = diffStd_Dev;
                        }
                        else if (tb.GetType() == tbPointScore.GetType())
                        {
                            dgvEx.Columns[n - 6].HeaderText = means;
                            dgvEx.Columns[n - 5].HeaderText = variance;
                            dgvEx.Columns[n - 4].HeaderText = stdv;
                            dgvEx.Columns[n - 3].HeaderText = diffMean;
                            dgvEx.Columns[n - 2].HeaderText = diffVar;
                            dgvEx.Columns[n - 1].HeaderText = diffStd_Dev;
                        }
                        else
                        {
                            MessageBox.Show("Se produjo un error al traducir ");
                        }

                        TranslationTContextualMenu(dgvEx, dicMeans, lang);
                        tpEx.TraslateLabel(numOfDecimal, puntoDecimal, grandMean, means, variance, stdv,
                            diffMean, diffVar, diffStd_Dev, TypScore);
                    }
                    this.tbPageMeanInfo.Parent = this.tabControlMeans;
                }
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " Se produjo un error al traducir " + name);
            }
        } // private void TraslationDataElements
        #endregion Cambio de idioma de los elementos del tabPageMeans

    } // end public partial class FormPrincipal : Form
} // end namespace GUI_TG
