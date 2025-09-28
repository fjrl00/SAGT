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
 * Fecha de revisión: 17/May/2012       Versión: 1.0 build 215.0.4
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using TransLibrary;
using MultiFacetData;
using ProjectMeans;
using MultiFacetPY;
using ProjectSSQ;
using System.Threading; // permite usar hilos
using System.Diagnostics;
using System.IO; // para poder usar File.Exist
using ConfigCFG;
using ConnectLibrary;

//============================================
using GUI_GT.MenPasWS;


namespace GUI_GT
{
    public partial class FormPrincipal : Form
    {
        // Versión del programa
        string version = "v1.0 build 215.0.4";
        string version2 = " ";


        #region Constantes y Variables
        /*=================================================================================
         * Constantes
         *=================================================================================*/

        // Directorios donde se guardarán los archivos de configuración y trabajo
        public const string SAGT_DIR = "SAGT"; // Directorio donde se guardarán los archivos de configuración y la carpeta de trabajo
        public const string WORKSPACE_DIR = "Workspace"; // Directorio de trabajo

        // Directorio de los ficheros de traducción
        const string LANG_PATH = "\\lang\\";
        const string MANUAL_PATH = "\\Manual\\";

        //  Directorio por defecto

        // string sagt_initial_directory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\" + SAGT_DIR + "\\" + WORKSPACE_DIR;

        // fichero de ayuda chm
        private string manual_file_chm = "Ayuda SAGT.chm"; 

        // Constantes referentes a los archivos de idiomas
        const string MENU_OPTIONS = "idioma_menu.txt"; // etiquetas del menú principal
        const string USER_STRING = "userStrings.txt"; // etiquetas de la opción usuarios
        const string PROJECT_STRING = "projectStrings.txt"; // etiquetas de la opción proyectos
        const string DATA_STRINGS = "dataStrings.txt"; // etiquetas del menú vertical de la opción Datos
        const string MEAN_STRINGS = "meansStrings.txt"; // etiquetas del menú vertical de medias
        const string SSQ_STRINGS = "ssqStrings.txt"; // etiquetas del menú suma de cuadrados
        const string STRING_MESSAGE = "string_message.txt"; // Dialogos y mensages
        const string ANALYSIS_STRINGS = "analysisStrings.txt"; // etiquetas del menú Análisis
        const string ERROR_MSG = "errorMsg.txt"; // Mensages de error

        /*=================================================================================
         * Variables
         *=================================================================================*/

        // Mensages
        //=========
        private string txtConfirmExit = "¿Está seguro que quiere salir?"; // mensage de confirmación para salir del programa.
        private string titleConfirm = "Confirmar"; // Titulo de la ventana de confirmación
        private string txtConfirmClose = "Se perderán los datos si no las ha guardado. ¿Desea continuar?";
        private string msgLoading = "Cargando...";
        private string msgConnecting = "Conectando...";

        // Filtros
        //========
        private string sagtFiles = "Archivos SAGT";
        private string gtFiles = "Arhivos GT";
        private string allFiles = "Todos los archivos";

        // Variables de control
        //=====================
        private bool editionModeOn = false;
        private bool conected = false; // Indica cuando  el programa se encuentra conectado a la aplicación
                                       // de servidor.

        // Parámetros de configuración
        // ===========================
        private ConfigCFG.ConfigCFG cfgApli = new ConfigCFG.ConfigCFG();

        // Este diccionario contien las traducciónes para las ventanas de dialogo
        TransLibrary.ReadFileTrans dicMessage;
        TransLibrary.ReadFileTrans dicMeans;
        TransLibrary.ReadFileTrans dicError;

        // Conexión con el servidor;
        // ServiceReference1.SagtWSoapClient sagtWS_Client;
        MenPasWS.SagtW sagtWS_Client; // = new GUI_GT.MenPasWS.SagtW();

        #endregion Constantes y Variables


        public enum TypeOfFile
        {
            txt, rtf, ssq, rsa, xls, csv
        }


        /***********************************************************************************
         *==================================================================================
         * Constructores
         *==================================================================================
         ************************************************************************************/

        /*
         * Descripción:
         *  Inicializa las componentes.
         */
        public FormPrincipal()
        {
            InitializeComponent();
            
            this.Text = this.Text + " " + this.version + this.version2;
            this.dicError = new ReadFileTrans(Application.StartupPath + LANG_PATH + ERROR_MSG);
            this.dicMessage = new TransLibrary.ReadFileTrans(Application.StartupPath + LANG_PATH + STRING_MESSAGE);
            this.dicMeans = new ReadFileTrans(Application.StartupPath + LANG_PATH + MEAN_STRINGS);

            LoadConfigCFG();
            
            this.editMultiFacetObs = false;

            // Incializa la opción de suma de cuadrados ocultando el tabPage de edición de la descripción de facetas
            this.tabPageEditDescriptionFacets.Parent = null;
            InitializeProjectsOption(); // Inicializa la opción de proyectos
            InitializeDataOption(); // Inicializa la opción de datos
            InitializeMeansOptions(); // Inicializa la opción de medias
            ClearListBoxSSQ(); // Limpiamos los listBox 
            ClearListBoxAnalysis(); // Limpiamos los listBox de analysis
            ExcludeTabPages();
            this.tabPageData.Parent = this.tabControlOptions;
            // Asignamos el color para la opción del menú datos
            this.tsmiDat.BackColor = System.Drawing.SystemColors.Highlight;
            // Ocultamos y deshabilitamos el botón de desconectar
            tsmiDisconnect.Enabled = conected;
            tsmiDisconnect.Visible = conected;
            EnableSaveWedService(conected, this.activeUser);
        }// end FormPrincipal


        /* Descripción:
         *  Constructor al que se le pasa como argumento el archivo que se va a abrir.
         */
        public FormPrincipal(string path)
            : this()
        {
            // Extraemos el nombre del fichero del path
            string fileExt = fileExtension(path).ToLower(); // Pasamos a minúsculas la extensión
            // para poder compararla. 
            switch (fileExt)
            {
                case ("sagt"): loadFileSagt(path); 
                    // hacemos que se muestre el primer contenido del archivo
                    ExcludeTabPages();
                    // Restauramos los colores
                    this.RestoreColorMenu(this.mStripMain);
                    if (this.sagtElements.GetMultiFacetsObs() != null)
                    {
                        // Asignamos el color para la opción del menú datos
                        this.tsmiDat.BackColor = System.Drawing.SystemColors.Highlight;
                        // seleccionamos el tabPage datos
                        tabPageData.Parent = this.tabControlOptions;
                        this.tabControlOptions.SelectedTab = this.tabPageData;
                    }
                    else if (this.sagtElements.GetListMeans() != null)
                    {
                        // Asignamos el nuevo color
                        this.tsmiMeans.BackColor = System.Drawing.SystemColors.Highlight;
                        // this.tabControlOptions.SelectedTab = this.tabPageMeans;
                        
                        this.tabPageMeans.Parent = this.tabControlOptions;
                    }
                    else if (this.sagtElements.GetAnalysis_and_G_Study() != null)
                    {
                        // Asignamos el nuevo color
                        this.tsmiSSQ.BackColor = System.Drawing.SystemColors.Highlight;
                        // this.tabControlOptions.SelectedTab = this.tabPageSSQ;
                        this.tabPageSSQ.Parent = this.tabControlOptions;
                    }
                    break;
                case ("anls"): LoadAnalysisFile(path); 
                    // hacemos que se muestre el tabPage de análisis
                    ExcludeTabPages();
                    // Restauramos los colores
                    this.RestoreColorMenu(this.mStripMain);
                    // Asignamos el nuevo color
                    this.tsmiAnalysis.BackColor = System.Drawing.SystemColors.Highlight;
                    // this.tabControlOptions.SelectedIndex = TABPAGE_ANALYSIS;
                    this.tabPageAnalysis.Parent = this.tabControlOptions;
                    // this.tabControlOptions.SelectedTab = this.tabPageAnalysis;
                    break;
                default:
                    ShowMessageErrorOK("No se muestra ninguno");
                    // MessageBox.Show("No se muestra ninguno");
                    break;
            }
        }


        /*=================================================================================
         * Operaciones
         *=================================================================================*/

        /* Descripción:
         *  Habilita o deshabilita la opción del menú de guardar en red en función del parámetro enableOption y 
         *  de el perfil de usuario que se le pasan como parámetros. Además de habilitar o deshabilitar las
         *  opciones del menú proyectos
         */
        private void EnableSaveWedService(bool enableOption, SagtUser user)
        {
            // Métodos para abrir
            this.tsmiDataOpenWebService.Enabled = enableOption;            
            this.tsmiMeansOpenWebService.Enabled = enableOption;            
            this.tsmiSSqOpenWebService.Enabled = enableOption;            
            this.tsmiAnalysisOpenWebService.Enabled = enableOption;
            // Métodos para guardar 
            if (!enableOption || user.GetAuthorizationToAccess().Equals(SagtUser.UserAccess.Administrador)
                || user.GetAuthorizationToAccess().Equals(SagtUser.UserAccess.Ad_Restringido))
            {
                this.tsmiDataSavedWebService.Enabled = enableOption;
                this.tsmiMeansSavedWebService.Enabled = enableOption;
                this.tsmiSSqSavedWebService.Enabled = enableOption;
                this.tsmiAnalysisvedWebService.Enabled = enableOption;
            }

            if (user != null)
            {
                // Acceder a la administración de proyectos
                if (!enableOption || user.GetAuthorizationToAccess().Equals(SagtUser.UserAccess.Administrador))
                {
                    EnableProjectsOption(enableOption); // habilita la opción de proyectos
                    foreach (ToolStripMenuItem tsmi in this.mStripProjects.Items)
                    {
                        tsmi.Enabled = enableOption;
                    }
                }

                /* Si es administrador restringido la única opción admisible detro de la opción proyectos
                 * será la de busqueda para que pueda poner un proyecto activo
                 */
                if (user.GetAuthorizationToAccess().Equals(SagtUser.UserAccess.Ad_Restringido)
                    || user.GetAuthorizationToAccess().Equals(SagtUser.UserAccess.Usuario))
                {
                    this.mStripProjects.Enabled = enableOption;
                    foreach (ToolStripMenuItem tsmi in this.mStripProjects.Items)
                    {
                        tsmi.Enabled = false;
                    }
                    tsmiSearchProject.Enabled = enableOption;
                }
            }
            else
            {
                tsmiSearchProject.Enabled = false;
                EnableProjectsOption(enableOption);
            }
        }// end EnableSaveWedService


        /*
         * Descripción:
         *  Carga el archivo de configuración en la aplicación a partir de la información contenida
         *  en el fichero de configuración especificado en la clase ConfigCFG.
         *  La ubicación del archivo deconfiguración será: "c:\USER\[Nombre de usuario]\Documents\SAGT"
         *  y el del espacio de trabajo: "c:\USER\[Nombre de usuario]\Documents\SAGT\Workspace"
         */
        private void LoadConfigCFG()
        {
            string pathDocuments = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string pathSagtDir = pathDocuments + "\\" + SAGT_DIR;

            // Comprobamos si existe el directorio de trabajo y si no existe lo creamos
            // El path final será "c:\USER\[Nombre de usuario]\Documents\SAGT\Workspace"
            if (Directory.Exists(pathSagtDir))
            {
                string path = pathSagtDir + "\\" + WORKSPACE_DIR;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            else
            {
                string path = pathSagtDir + "\\" + WORKSPACE_DIR;
                Directory.CreateDirectory(path);
            }

            // Comprobamos si existe el fichero de configuración en la ubicación especificada y sino lo
            // creamos.
            string pathFileCfg = pathSagtDir + "\\" + cfgApli.NameFileConfig();
            if (File.Exists(pathFileCfg))
            {
                try
                {
                    cfgApli.ReadFileConfig(pathSagtDir);
                    // string oldPath = cfgApli.Get_Path_Workspace();
                    if (string.IsNullOrEmpty(cfgApli.Get_Path_Workspace()))
                    {
                        string path = pathSagtDir + "\\" + WORKSPACE_DIR;
                        cfgApli.Set_Path_Workspace(path);
                    }
                    else if (!Directory.Exists(cfgApli.Get_Path_Workspace()))
                    {
                        // No existe el directorio de trabajo
                        // Informamos y cargamos el directorio de trabajo por defecto
                        ShowMessageErrorOK(errorNoExistWorkspace);
                        string path = pathSagtDir + "\\" + WORKSPACE_DIR;
                        cfgApli.Set_Path_Workspace(path);
                    }
                    changeLanguage(cfgApli.GetConfigLanguage());
                }
                catch (ConfigCFGException e)
                {
                    // Se produjo un error al leer el archivo
                    // MessageBox.Show(e.Message + " " + "Se produjo un error al leer" + " " + cfgApli.NameFileConfig());
                    ShowMessageErrorOK(e.Message + " " + errorReadingFile + " " + cfgApli.NameFileConfig());
                }
            }
            else
            {
                // Si no existe lo creamos con los valores por defecto
                string path = pathSagtDir + "\\" + WORKSPACE_DIR;
                cfgApli.Set_Path_Workspace(path);
                cfgApli.WriteFileConfig(pathSagtDir);
                changeLanguage(cfgApli.GetConfigLanguage());
            }
        }// end LoadConfigCFG


        #region Operaciones para crear los dicionarios y realizar el cambio de idioma
        /*=======================================================================================================
         * Cambio de idioma
         *=======================================================================================================*/
        private void changeLanguage(TransLibrary.Language lang)
        {
            // Traducimos mensages de error
            TraslationErrorMessages(lang);
            // Traducimos mensages y filtros;
            TranslationMessageAndFilter(lang);
            // Cambia de idioma el menú principal
            TranslationMenuItems(lang, Application.StartupPath + LANG_PATH + MENU_OPTIONS, this.mStripMain);
            // Cambia de idioma el menú de proyectos
            TranslationMenuItems(lang, Application.StartupPath + LANG_PATH + PROJECT_STRING, this.mStripProjects);
            // Cambia los objetos del tabPage del menú Proyectos
            TranslationProjectsElements(lang, Application.StartupPath + LANG_PATH + PROJECT_STRING);
            // Cambia de idioma el menú del tabPage de la opcion Datos
            TranslationMenuItems(lang, Application.StartupPath + LANG_PATH + DATA_STRINGS, this.mStripData);
            // Cambia los objetos del tabPage del menú de datos datos
            TranslationDataElements(lang, Application.StartupPath + LANG_PATH + DATA_STRINGS);
            // Cambia de idioma el menú del tabPage de la opcion Means
            TranslationMenuItems(lang, Application.StartupPath + LANG_PATH + MEAN_STRINGS, this.mStripMeans);
            // Traducimos las etiqueta del tabPage y los dataGridView
            TranslationMeansElements(lang, Application.StartupPath + LANG_PATH + MEAN_STRINGS);
            // Cambia de idioma el menú del tabPage de la opcion SSQ
            TranslationMenuItems(lang, Application.StartupPath + LANG_PATH + SSQ_STRINGS, this.mStripSSQ);
            // Cambia los objetos del tabPage de suma de cuadrados
            TranslationSSQElements(lang, Application.StartupPath + LANG_PATH + SSQ_STRINGS);
            // Cambia de idioma el menú del tabPage de la opcion Analysis
            TranslationMenuItems(lang, Application.StartupPath + LANG_PATH + ANALYSIS_STRINGS, this.mStripAnalysis);
            // Cambia los objetos del tabPageAnalysis
            TranslationAnalysisElements(lang, Application.StartupPath + LANG_PATH + ANALYSIS_STRINGS);
     
            // Cambia los dialogos
            TranslationStringDialogMessage(lang, STRING_MESSAGE);
            
            
        }// end changeLanguage

        /* Descripción:
         *  Traducción de mensages y filtros
         */
        private void TranslationMessageAndFilter(TransLibrary.Language lang)
        {
            // traducimos filtros
            string name = "";
            try
            {
                name = "sagtFiles";
                this.sagtFiles = this.dicMessage.labelTraslation(name).GetTranslation(lang).ToString();
                name = "gtFiles";
                this.gtFiles = this.dicMessage.labelTraslation(name).GetTranslation(lang).ToString();
                name = "allFiles";
                this.allFiles = this.dicMessage.labelTraslation(name).GetTranslation(lang).ToString();
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                // Se produjo un error al traducir
                // MessageBox.Show(lEx.Message + " " + errorMessageTraslation + " " + name);
                ShowMessageErrorOK(lEx.Message + " " + errorMessageTraslation + " " + name);
            } 
        }

        /*
         * Descripción:
         *  Cambia de idioma los elementos del menu.
         * Parámetros:
         *      TransLibrary.Language lang: idioma al que se quiere cambiar.
         *      string nameFileTrans: contien el nombre del fichero con lase traducciónes de los distintos
         *          objetos.
         */
        private void TranslationMenuItems(TransLibrary.Language lang, string nameFileTrans, MenuStrip menu)
        {
            TransLibrary.ReadFileTrans dic = new TransLibrary.ReadFileTrans(nameFileTrans);
            string name = "";
            try
            {

                if (this.mStripMain.Items != null)
                {
                    /*
                     * NOTA:
                     *  En lugar de emplear en la cavecera del bucle:
                     *          foreach (ToolStripMenuItem item in menu.Items)
                     *  Uso:
                     *          foreach (ToolStripItem item in menu.Items)
                     *  El motivo es que ToolStripItem es más genérico que ToolStripMenuItem
                     *  y admite los separadores. De otra forma devolveria el error:
                     *          "No se puede convertir un objeto de tipo 
                     *           'System.Windows.Forms.ToolStripSeparator' al tipo 
                     *           'System.Windows.Forms.ToolStripMenuItem'."
                     */
                    foreach (ToolStripItem item in menu.Items)
                    {
                        if(item.Equals(this.tsSeparator1) || item.Equals(this.tsSeparator2))
                        {
                            // no hago nada
                        }else{
                            name = item.Name.ToString();
                            item.Text =
                                dic.labelTraslation(name).GetTranslation(lang).ToString();
                            
                            if (((ToolStripMenuItem)item).DropDownItems != null)
                            {
                                this.translationDropDownItems((ToolStripMenuItem)item, dic, lang);

                            }
                            
                        
                            /*
                            name = mnuItemOpcion.Name.ToString();
                            mnuItemOpcion.Text =
                                dic.labelTraslation(name).LangTraslation(lang).ToString();
                            if (mnuItemOpcion.DropDownItems != null)
                            {
                                this.translationDropDownItems(mnuItemOpcion, dic, lang);

                            }
                             */
                        }
                    }
                }// end if

            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                // Se produjo un error al traducir mostramos el mensaje
                // MessageBox.Show(lEx.Message + " " + errorMessageTraslation + " " + name);
                ShowMessageErrorOK(lEx.Message + " " + errorMessageTraslation + " " + name);
            }       
        }


        /*
         * Descripción:
         *  Cambia de idioma los elementos de los submenu.
         * Parámetros:
         *  ToolStripMenuItem mnuItemOpcion: submenu que queremos cambiar de idioma.
            TransLibrary.ReadFileTrans dic: Estructura de datos con la traducción de los elementos.
         *  TransLibrary.Language lang: idioma al que queremos traducir los elementos.
         */
        private void translationDropDownItems(ToolStripMenuItem mnuItemOpcion,
            TransLibrary.ReadFileTrans dic, TransLibrary.Language lang)
        {
            string name = "";
            try
            {
                foreach (ToolStripMenuItem mnuItem2 in mnuItemOpcion.DropDownItems)
                {
                    name = mnuItem2.Name.ToString();

                    mnuItem2.Text =
                        dic.labelTraslation(mnuItem2.Name.ToString()).GetTranslation(lang).ToString();
                    if (mnuItem2.DropDownItems != null)
                    {
                        this.translationDropDownItems(mnuItem2, dic, lang);
                    }
                }
                
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                // Se produjo un error al traducir mostramos mensaje
                // MessageBox.Show(lEx.Message + " Se produjo un error al traducir " + name);
                ShowMessageErrorOK(lEx.Message + " " + errorMessageTraslation + " " + name);
            }
        }


        /*
         * Descripción: 
         *  Cambia de idioma los diversos mensages que se muestran. El nombre de la variable se pasa 
         *  como parámetro de busqueda en el diccionario.
         * Parámetros:
         *      TransLibrary.Language lang: Idioma al que se desea traducir. 
         *      string nameFileTrans: nombre del fichero que contiene las traducciones.
         * Excepciones:
         *  Lanza una TransLibrary.LabelTranslationException si no encuentra la tradución el diccionario.
         */
        private void TranslationStringDialogMessage(TransLibrary.Language lang, string nameFileTrans)
        {
            TransLibrary.ReadFileTrans dic = new TransLibrary.ReadFileTrans(Application.StartupPath + LANG_PATH + nameFileTrans);
            string name = "";
            try
            {
                // Ventana salir de la aplicación
                name = "titleConfirm";
                titleConfirm = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "txtConfirmExit";
                txtConfirmExit = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "txtConfirmClose";
                txtConfirmClose = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Mensage de la ventana de carga
                name ="msgLoading";
                msgLoading = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Mensage de la ventana de conexión
                name = "msgConnecting";
                msgConnecting = dic.labelTraslation(name).GetTranslation(lang).ToString();

            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                // MessageBox.Show(lEx.Message + " " + errorMessageTraslation + " " + name);
                ShowMessageErrorOK(lEx.Message + " " + errorMessageTraslation + " " + name);
            }
        }


        /*
         * Descripción:
         *  Cambia de idioma el menú contextual de los dataGridVieWEx
         */
        public void TranslationTContextualMenu(DataGridViewEx.DataGridViewEx dgvEx, TransLibrary.ReadFileTrans dicMeans,
            TransLibrary.Language lang)
        {
            try
            {
                dgvEx.ContextMenuStrip.Items[0].Text = dicMeans.labelTraslation("Cut").GetTranslation(lang).ToString();
                dgvEx.ContextMenuStrip.Items[1].Text = dicMeans.labelTraslation("Copy").GetTranslation(lang).ToString();
                dgvEx.ContextMenuStrip.Items[2].Text = dicMeans.labelTraslation("Paste").GetTranslation(lang).ToString();
                dgvEx.ContextMenuStrip.Items[3].Text = dicMeans.labelTraslation("Remove").GetTranslation(lang).ToString();
                // el siguente item es el separador y por eso nos lo saltamos
                dgvEx.ContextMenuStrip.Items[5].Text = dicMeans.labelTraslation("SelectAll").GetTranslation(lang).ToString();
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                // Mostramos el mensaje de error al traducir
                // MessageBox.Show(lEx.Message + " Se produjo un error al traducir el menú contextual");
                ShowMessageErrorOK(lEx.Message + errorMessageTraslation);
            }
        }


        /* Descripción:
         *  Devuelve el diccionario con los mensages de error.
         */
        public TransLibrary.ReadFileTrans GetTransErrorMsgDic()
        {
            return this.dicError;
        }


        /* Descripción:
         *  Devuelve el diccionario con los mensages y las traducciones de las ventanas modales.
         */
        public TransLibrary.ReadFileTrans GetTransDicMessage()
        {
            return this.dicMessage;
        }
        #endregion Operaciones para crear los dicionarios y realizar el cambio de idioma



        #region Menú Cambio de idioma
        /*
         * Descripción:
         *  Método para cambiar el idioma de la aplicación a español. Se ejecuata al seleccionar
         *  en el menú Herramientas-->Idiomas-->Español.
         *  Realiza una llamada a changeLanguage que es quien realmente cambia el texto de los
         *  objetos de la interfaz gráfica.
         */
        private void tsmiSpanish_Click(object sender, EventArgs e)
        {
            this.cfgApli.SetConfigLanguage(Language.spanish);
            this.changeLanguage(Language.spanish);
        }
        /*
         * Descripción:
         *  Método para cambiar el idioma de la aplicación a ingles. Se ejecuata al seleccionar
         *  en el menú Herramientas-->Idiomas-->Inglés.
         *  Realiza una llamada a changeLanguage que es quien realmente cambia el texto de los
         *  objetos de la interfaz gráfica.
         */
        private void tsmiEnglish_Click(object sender, EventArgs e)
        {
            this.cfgApli.SetConfigLanguage(Language.english);
            this.changeLanguage(Language.english);
        }


        /*
         * Descripción:
         *  Método para cambiar el idioma de la aplicación a francés. Se ejecuata al seleccionar
         *  en el menú Herramientas-->Idiomas-->Francés.
         *  Realiza una llamada a changeLanguage que es quien realmente cambia el texto de los
         *  objetos de la interfaz gráfica.
         */
        private void tsmiFrench_Click(object sender, EventArgs e)
        {
            this.cfgApli.SetConfigLanguage(Language.french);
            this.changeLanguage(Language.french);
        }


        /*
         * Descripción:
         *  Método para cambiar el idioma de la aplicación a portugues. Se ejecuata al seleccionar
         *  en el menú Herramientas-->Idiomas-->Portugues.
         *  Realiza una llamada a changeLanguage que es quien realmente cambia el texto de los
         *  objetos de la interfaz gráfica.
         */
        private void tsmiPortuguese_Click(object sender, EventArgs e)
        {
            this.cfgApli.SetConfigLanguage(Language.portuguese);
            this.changeLanguage(Language.portuguese);
        }


        /*
         * Descripción:
         *  Método de condulta. Devuelve el idioma actual de la aplicación.
         */
        public Language LanguageActually()
        {
            return cfgApli.GetConfigLanguage();
        }
        #endregion menú cambio de idioma


        #region Seleción del menú opciones
        /*======================================================================================
         * Seleción del menú opciones:
         *      - Conectar
         *      - Desconectar
         *      - Usuarios
         *      - Proyectos
         *      - Datos
         *      - Medias
         *      - Suma de cuadrados
         *      - Análisis
         *      - Restaurar el color de las opciones del menú principal
         *      - Quitar todos los tabPage del tabControl
         *======================================================================================*/


        /* Descripción:
         *  Oculta y deshabilita el botón de conectar. Muestra y habilita el botón de desconectar.
         */
        private void tsmiConnect_Click(object sender, EventArgs e)
        {
            if (!this.editionModeOn)
            {
                string pathDocuments = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string filepass = pathDocuments + "\\" + SAGT_DIR + "\\" + PassUsers.FILE_USER_PASS;

                FormPass formPass = new FormPass(this.LanguageActually(), filepass);
                PassUsers dtUser = new PassUsers();

                if (File.Exists(filepass))
                {
                    dtUser = PassUsers.LoadDataTable(filepass);
                }

                bool salir = false;
                do
                {
                    DialogResult res = formPass.ShowDialog();
                    switch (res)
                    {
                        case DialogResult.Cancel:
                            salir = true;
                            break;
                        case DialogResult.OK:
                            // Esta ventana se mostrará mientras se carga el fichero
                            CWait fw = new CWait(msgConnecting);
                            Thread th = new Thread(new ThreadStart(fw.CWaitShowDialog));

                            try
                            {
                                // Ocultamos y deshabilitamos el botón de conectar
                                tsmiConnect.Enabled = false;
                                tsmiConnect.Visible = false;
                                // Mostramos y habilitamos el botón de desconectar
                                tsmiDisconnect.Enabled = true;
                                tsmiDisconnect.Visible = true;
                                salir = true;

                                // Mensaje nombre de usuario o contraseña no validos
                                string mensage = errorNoUserNameOrPassword;
                                string user = formPass.UserId(); // nombre de usuario
                                string pass = formPass.UserPass(); // contaseña

                                
                                th.Start();

                                // Inicio de conexión
                                sagtWS_Client = new GUI_GT.MenPasWS.SagtW();
      
                                bool isUserValidate = sagtWS_Client.EstaRegistrado(user.ToLower(), pass.ToLower());

                                if (isUserValidate)
                                {
                                    mensage = txtConnect; // Mensaje esta conectado
                                    // Entonces almacenamos la clave
                                    dtUser.UpdateDataRow(user, pass, formPass.SavePass());
                                    dtUser.EncryptDataTableToFile(filepass);
                                    formPass.AuxAutoComplete();
                                    // Comprobamos si esta en la base de datos y si no lo almacenamos
                                    int idUser = sagtWS_Client.Add_SagtUser(user.ToLower());

                                    // Obtenemos los datos
                                    // perfil
                                    string rol = sagtWS_Client.Obtener_perfil(user.ToLower());
                                    SagtUser.UserAccess userRol = (SagtUser.UserAccess)Enum.Parse(typeof(SagtUser.UserAccess), rol);
                                    string group = sagtWS_Client.Obtener_grupo(user.ToLower());
                                    // Creamos el usuario
                                    this.activeUser = new SagtUser(idUser, user.ToLower(), userRol, group);

                                    // sagtWS_Client.l

                                    this.conected = true; // ponemos la variable local que indica que esta connectado a true
                                    this.EnableSaveWedService(this.conected, this.activeUser);
                                }
                                else
                                {
                                    sagtWS_Client = null;
                                }
                                th.Abort();

                                // MessageBox.Show(mensage);
                                ShowMessageInfo(mensage);
                            }
                            catch (EndpointNotFoundException)
                            {
                                th.Abort();
                                AuxNoConection();
                                salir = true;
                            }
                            catch (InvalidOperationException)
                            {
                                th.Abort();
                                AuxNoConection();
                                salir = true;
                            }
                            catch (ArgumentException)
                            {
                                th.Abort();
                                AuxNoConection();
                                salir = true;
                            }
                            break;
                    }
                } while (!salir);
            }
        }// end tsmiConnect_Click


        /* Descripción:
         *  Operación auxiliar que se ejecuta al producirse una excepción durate la conexión o por la conexion
         */
        private void AuxNoConection()
        {
            ShowMessageErrorOK(errorNoCommunication);
            // Mostramos y habilitamosel botón de conectar
            tsmiConnect.Enabled = true;
            tsmiConnect.Visible = true;
            // Ocultamos y deshabilitamos  el botón de desconectar
            tsmiDisconnect.Enabled = false;
            tsmiDisconnect.Visible = false;
            this.conected = false; // ponemos la variable local que indica que esta connectado a false
            this.EnableSaveWedService(this.conected, this.activeUser);
        }

        /* Descripción:
         *  Oculta y deshabilita el botón de desconectar. Muestra y habilita el botón de conectar.
         */
        private void tsmiDisconnect_Click(object sender, EventArgs e)
        {
            if (!this.editionModeOn)
            {
                // Pedimos confirmación
                DialogResult res = ShowMessageDialog(titleAdvice, txtConfirmCloseConnect);
                switch (res)
                {
                    case (DialogResult.OK):
                        // Ocultamos y deshabilitamos el botón de desconectar
                        tsmiDisconnect.Enabled = false;
                        tsmiDisconnect.Visible = false;
                        // Mostramos y habilitamos el botón de conectar
                        tsmiConnect.Enabled = true;
                        tsmiConnect.Visible = true;
                        this.conected = false;
                        this.EnableSaveWedService(this.conected, this.activeUser);
                        // proyecto activo y usuario activo a null
                        this.activeProject = null;
                        this.activeUser = null;
                        // Limpiamos el proyecto activo
                        // Borro los campos
                        ClearProjectsText();
                        // Ponemos el servico a web a null
                        this.sagtWS_Client = null;
                        break;
                }
            }
        }// end tsmiDisconnect_Click



        /* Descripción:
         *  Selecciona el tabPage correspondiente a proyectos
         */
        private void tsmiProjects_Click(object sender, EventArgs e)
        {
            if (!this.editionModeOn)
            {
                ExcludeTabPages();
                // Restauramos los colores
                this.RestoreColorMenu(this.mStripMain);
                // Asignamos el color para la opción del menú proyectos
                this.tsmiProjects.BackColor = System.Drawing.SystemColors.Highlight;
                // seleccionamos el tabPage proyectos
                this.tabPageProjects.Parent = this.tabControlOptions;
                // Si no esta conectado lanzamos un aviso
                if (!conected)
                {
                    ShowMessageErrorOK(txtAdvise, titleAdvice, MessageBoxIcon.Information);
                }
            }
        }


        /*
         * Descripción:
         *  Selecciona el tabpage correspondiente a los datos (tabPageData).
         */
        private void tsmiDat_Click(object sender, EventArgs e)
        {
            if (!this.editionModeOn)
            {
                ExcludeTabPages();
                // Restauramos los colores
                this.RestoreColorMenu(this.mStripMain);
                // Asignamos el color para la opción del menú datos
                this.tsmiDat.BackColor = System.Drawing.SystemColors.Highlight;
                // seleccionamos el tabPage datos
                tabPageData.Parent = this.tabControlOptions;
            }
        }


        /*
         * Descripción:
         *  Selecciona el tabpage correspondiente a las medias (tabPageMeans).
         */
        private void tsmiMeans_Click(object sender, EventArgs e)
        {
            if (!this.editionModeOn)
            {
                ExcludeTabPages();
                this.tabPageMeans.Parent = this.tabControlOptions;
                // Restauramos los colores
                this.RestoreColorMenu(this.mStripMain);
                // Asignamos el nuevo color
                this.tsmiMeans.BackColor = System.Drawing.SystemColors.Highlight;
            }
        }


        /*
         * Descripción:
         *  Selecciona el tabPaage correspondiente a suma de cuadrados
         */
        private void tsmiSSQ_Click(object sender, EventArgs e)
        {
            if (!this.editionModeOn)
            {
                ExcludeTabPages();
                // Asignamos el tabPage de suma de cuadrados al tabControl de opciones
                this.tabPageSSQ.Parent = this.tabControlOptions;
                // Restauramos los colores
                this.RestoreColorMenu(this.mStripMain);
                // Asignamos el nuevo color
                this.tsmiSSQ.BackColor = System.Drawing.SystemColors.Highlight;
            }
            
        }


        /*
         * Descripción:
         *  Pusamos sobre la opción Analisis del menu de opciones. Se muestra el tabPage de analisis
         *  con su menú vertical.
         */
        private void tsmiAnalyses_Click(object sender, EventArgs e)
        {
            if (!this.editionModeOn)
            {
                ExcludeTabPages();
                // Restauramos los colores
                this.RestoreColorMenu(this.mStripMain);
                // Asignamos el nuevo color
                this.tsmiAnalysis.BackColor = System.Drawing.SystemColors.Highlight;
                // this.tabControlOptions.SelectedIndex = TABPAGE_ANALYSIS;
                this.tabPageAnalysis.Parent = this.tabControlOptions;
            }
        }


        /*
         * Descripción:
         *  Restaura el color original de las opciones del menu
         */
        private void RestoreColorMenu(MenuStrip menu)
        {
            foreach (ToolStripItem item in menu.Items)
            {
                if (!item.Equals(this.tsSeparator1) && !item.Equals(this.tsSeparator2))
                {
                    item.BackColor = System.Drawing.SystemColors.Control;
                }
            }
        }


        /* Descripción:
         *  Quita todos los tabPage del tabControl
         */
        private void ExcludeTabPages()
        {
            foreach (TabPage tp in this.tabControlOptions.TabPages)
            {
                tp.Parent = null;
            }
        }


        #endregion Seleción del menú opciones


        #region Operaciones auxiliares para manipular path y devolver el nombre del fichero o la extensión
        /*
         * Descripción:
         *  Devueve el nombre del fichero.
         * Parámetros:
         *  string path: path del fichero que estamos leyendo.
         *  
         * NOTA:
         *  Si desea incluir una barra diagonal inversa, ésta debe estar precedida de otra 
         *  barra diagonal inversa.
         */
        public string extractFileNamePath(string path)
        {
            int pos = path.LastIndexOf("\\") + 1; // le sumamos uno para obtener la posición 
            // siguiente a la de la barra invertida
            return path.Substring(pos);
        }

        /*
         * Descripción:
         *  Devuelve la extensión de un fichero, en el caso de que el fichero no tenga 
         *  extensión devuelve la cadena vacía.
         * Parámetros:
         *      String fileName: Nombre del fichero.
         */
        public string fileExtension(string path)
        {
            string retVal = "";
            int pos = path.LastIndexOf(".") + 1; // le sumamos uno para obtener la posición 
                                                 // siguiente a la de la barra invertida
            if (pos > (-1))
            {
                // hemos encontrado la posición
                retVal = path.Substring(pos);
            }
            return retVal;
        }

        #endregion Operaciones auxiliares para manipular path y devolver el nombre del fichero o la extensión


        #region Selección de opciones del submenú Herramientas
        /*======================================================================================
         * Seleción del submenú Herramientas:
         *      - Calculadora
         *      - Configuración
         *======================================================================================*/
        /*
         * Descripción:
         *  Al pulsar en el menú Herramientas la opción calculadora ejecuta la calculadora de Windows.
         */
        private void tsmiCalc_Click(object sender, EventArgs e)
        {
            Process p = Process.Start("calc.exe");
        }


        /* Descripción:
        *  Llama al método que se encarga de abrir la ventana de Configuración. Se activa al pulsar la
        *  opción configuración del menú Herramientas.
        */
        private void tsmiSettings_Click(object sender, EventArgs e)
        {
            int oldNumberOfDecimals = cfgApli.GetNumberOfDecimals(); // número antiguo de decimales
            string oldDecimalSeparator = cfgApli.GetDecimalSeparator(); // separador decimal antiguo
            string pathManual = Application.StartupPath + MANUAL_PATH + manual_file_chm;

            string pathDocuments = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filepass = pathDocuments + "\\" + SAGT_DIR + "\\" + PassUsers.FILE_USER_PASS;
            string pathDefaultWorkSpace = pathDocuments + "\\" + SAGT_DIR + "\\" + WORKSPACE_DIR;

            bool salir = false;
            FormSettings formSetting = new FormSettings(cfgApli, pathManual, filepass, pathDefaultWorkSpace);

            do
            {
                DialogResult res = formSetting.ShowDialog();
                switch (res)
                {
                    case DialogResult.Cancel: salir = true; 
                        break;
                    case DialogResult.OK:
                        // Optenemos el path del fichero de configuración.
                        string pathSagtDir = pathDocuments + "\\" + FormPrincipal.SAGT_DIR;
                        // Actualizamos los parametros de configuración con los seleccionados en la ventana
                        cfgApli = formSetting.UpdateConfig();
                        // Guardamos el archivo de configuración en el directorio "\Documents\SAGT"
                        cfgApli.WriteFileConfig(pathSagtDir);

                        // Si cambia el numero de decimales o el caracter separador refrescamos todos los datos.
                        if (!cfgApli.GetNumberOfDecimals().Equals(oldNumberOfDecimals)
                            || !cfgApli.GetDecimalSeparator().Equals(oldDecimalSeparator))
                        {    
                            this.RefreshDataInAllData();
                        }

                        salir = true;
                        break;
                    //case DialogResult.Yes:
                    //    this.ShowWindowsHelp();
                    //    break;
                }
            } while (!salir);

        }// end tsmiSettings_Click

        #endregion Selección de opciones del submenú Herramientas


        /*
         * Descripción:
         *  Captura el evento de cerrar la ventana principal y pide confirmación antes de salir del 
         *  programa principal. 
         */
        private void FormPrincipal_Closing(object sender, FormClosingEventArgs e)
        {
            
            if (e.CloseReason == CloseReason.UserClosing)   //check para ver si el cierre es por el usuario, único caso donde la ventana de confirmación debe aparecer
            {
                DialogResult res = ShowMessageDialog(
                    titleConfirm,
                    txtConfirmExit,
                    MessageBoxIcon.Exclamation);

                if (res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }

            // Guardar configuración en caso de que cerremos
            if (!e.Cancel)
            {
                string pathDocuments = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string pathSagtDir = Path.Combine(pathDocuments, SAGT_DIR);
                cfgApli.WriteFileConfig(pathSagtDir);
            }
        }


        #region Selección de opciones del submenú Ayuda
        /*======================================================================================
         * Seleción del submenú Herramientas:
         *      - Indice (mostrar ayuda .chm)
         *      - Acerca de SAGT
         *======================================================================================*/
        /*
         * Descripción:
         *  Muestra la ventana Acerca de con información sobre la aplicación cuando se hace clip 
         *  en la opción Acerca de SAGT del menú Ayuda.
         */
        private void tsmiAboutOf_Click(object sender, EventArgs e)
        {
            FormAboutOf formAboutof = new FormAboutOf(cfgApli.GetConfigLanguage(), 
                Application.StartupPath + LANG_PATH + STRING_MESSAGE, version);
            formAboutof.ShowDialog();
        }

        /* Descripción:
         *  Muestra la ventana de ayuda tras ser selecciónada en el menú prinicpal
         */
        private void tsmiIndex_Click(object sender, EventArgs e)
        {
            ShowWindowsHelp();
        }

        /* Descripción:
         *  Muestra la ventana de ayuda tras ser selecciónada en el menú prinicpal
         */
        public void ShowWindowsHelp()
        {
            string pathManual = Application.StartupPath + MANUAL_PATH + manual_file_chm;
            Help.ShowHelp(this, pathManual);
        }

        #endregion Selección de opciones del submenú Ayuda


        /* Descripción:
         *  Refresca los datos que se muestran en el programa. La llamada a este método se produce cuando
         *  cambiamos el número de decimales.
         */
        public void RefreshDataInAllData()
        {
            // Número de decimales para la representación
            int numOfDecimal = cfgApli.GetNumberOfDecimals();
            // Punto de separación decimal
            string puntoDecimal = this.cfgApli.GetDecimalSeparator();

            // Refrescamos medias
            ListMeans listMeans = this.sagtElements.GetListMeans();
            if (listMeans != null)
            {
                ListMeans aux = listMeans;
                ClearTabPageMeans();
                listOfTableMeansToTabPageMeans(listMeans);
            }
            // Refrescamos los g_Parámetros del la opción suma de cuadrados
            Analysis_and_G_Study tAnalysis_tG_Study_Opt = this.sagtElements.GetAnalysis_and_G_Study();
            if(tAnalysis_tG_Study_Opt!= null)
            {
                LoadAllDataInDataGridViewEx_SSQOptions(tAnalysis_tG_Study_Opt);
            }

            // Refrescamos los datos de la opción de análisis
            if (anl_tAnalysis_G_study_opt != null)
            {
                LoadAllDataGridWithDataAnalysis(this.anl_tAnalysis_G_study_opt, this.anl_tAnalysis_G_study_opt.GetNameFileDataCreation());
            }
        }// RefreshDataInAllData


        #region Botones del menú de acciones de Datos
        /*************************************************************************************************
         * ===============================================================================================
         * Botones del menú vertical de la opción Datos
         * 
         * - Nuevo (tsmiNewMultiFacetData_Click)
         * - Abrir (tsmiDataOpenLocal_Click)
         * - Abrir desde servicio Web (tsmiDataOpenWebService_Click)
         * - Importar (tsmiDataImport_Click)
         * - Guardar (tsmiSaveMultiFacetData_Click)
         * - Guardar en servicio Web (tsmiDataSavedWebService_Click)
         * - Omitir niveles (tsmiDataOmitLevels_Click)
         * - Editar Facetas (tsmiData_EditFacets_Click)
         * - Editar Datos (tsmiDataEditObs_Click)
         * - Generar medias (tsmiBuildMeans_Click)
         * - Suma de cuadrados (EstimationPlan --> ssqOptions.cs)
         *              (tsmiDataToSSQ_Click)
         * - Exportar a excel (tsmiDataExportExcel_Click)
         * - Exportar datos (tsmiExportScore_Click)
         * - Exportar Word (tsmiDataWordExport_Click)
         * - Exportar PDF (tsmiDataPdfExport_Click)
         * - Cerrar (btDataEditFacetsCancel_Click)
         *===============================================================================================
         ************************************************************************************************/

        /* Descripción:
         *  Se activa cuando se pulsa la acción nuevo del menú de la opción datos
         */
        private void tsmiNewMultiFacetData_Click(object sender, EventArgs e)
        {
            tsmiActionNewMultiFacetData_Click(sender, e);
        }


        /* Descripción:
         *  Se activa al pulsar la acción "Abrir" del menú de datos. Llama a un método que se encuentra 
         *  en la clase parcial dataOptions
         */
        private void tsmiDataOpenLocal_Click(object sender, EventArgs e)
        {
            tsmiActionOpenFile_Click(sender, e);
        }


        /* Descripción:
         *  Se activa al pulsar "Abrir desde servicio Web" del menú Datos.
         */
        private void tsmiDataOpenWebService_Click(object sender, EventArgs e)
        {
            tsmiActionOpenWebService_Click(TypeFile.sagt);
        }
        

        /* Descripción:
         *  Se activa al pulsar la acción "Importar" del menú vertical de la opción "Datos". Llama a un
         *  método que se encuentra en la clase parcial DataOptions.cs.
         */
        private void tsmiDataImport_Click(object sender, EventArgs e)
        {
            this.checkBoxHideNulls.Enabled = false;
            this.checkBoxHideNulls.Checked = false;
            this.checkBoxHideNulls.Enabled = true;
            // este método se encuentra en la clase parcial DataOptions.cs
            tsmiActionDataImport_Click(sender, e);
        }


        /*
         * Descripción:
         *  Almacena los datos de la variable multifacet en un archivo. Llama a un método que se encuentra 
         *  en la clase parcial dataOptions.
         */
        private void tsmiDataSavedLocal_Click(object sender, EventArgs e)
        {
            saveFileData(this.sagtElements);
        }


        /* Descripción:
         *  Se activa al hacer click en la opción "Guardar en servicio Web" de la opción
         *  Datos.
         */
        private void tsmiDataSavedWebService_Click(object sender, EventArgs e)
        {
            if (sagtElements.GetMultiFacetsObs() == null)
            {
                ShowMessageErrorOK(errorNoTableObs);
                //MessageBox.Show(errorNoTableObs);
            }
            else
            {
                tsmiActionDataSavedWebService_Click();
            }
        }


        /* Descripción:
         *  Este método se ejecuta cuando pusamos la opción editar facetas en el menú de acciónes de la
         *  opción datos.
         */
        private void tsmiData_EditFacets_Click(object sender, EventArgs e)
        {
            tsmiActionData_EditFacets_Click(sender, e);
        }


        /* Descripción:
         *  Este método se lanza cuando seleccionamos la acción de editar datos en el menú vertical 
         *  de la opción datos.
         */
        private void tsmiDataEditObs_Click(object sender, EventArgs e)
        {
            // Llama al método (...) de la clase parcial DataOptions
            // Se encaga de editar los datos de la tabla de observaciones.
            CWait fw = new CWait(msgLoading);
            Thread th = new Thread(new ThreadStart(fw.CWaitShowDialog));
            try
            {
                // Start the thread
                th.Start();
                MultiFacetsObs multiFacets = this.sagtElements.GetMultiFacetsObs();
                if (this.checkBoxHideNulls.Checked)
                {
                    // Si la propiedad checked esta a false se muestran todos los valores
                    loadDataInTabPageObsTable(multiFacets);
                }
                th.Abort();
            }
            catch (IOException ex)
            {
                th.Abort();
                ShowMessageErrorOK(ex.Message);
            }
            EditDataObsTable();
        }


        /* Descripción:
         *  Este método se lanza cuando seleccionamos la acción de omitir niveles del menú vertical de
         *  la opción datos.
         */
        private void tsmiDataOmitLevels_Click(object sender, EventArgs e)
        {
            // Llama al método (...) de la clase parcial DataOptions
            tsmiActionDataOmitLevels_Click(sender, e);
        }


        /*
         * Descripción:
         *  Abre la ventana con la lista de combinaciones sin repeticion de las facetas.
         */
        private void tsmiBuildMeans_Click(object sender, EventArgs e)
        {
            // Llama al metodo ShowWindowsBuildMeans de la clase parcial dataOptions.cs
            tsmiActionBuildMeans_Click(sender,e);
        }


        /*
         * Descripción:
         *  Llama al método que realiza la la suma de cuadrados y el diseño de medidas de la clase 
         *  parcial SSQOptions.cs
         */
        private void tsmiDataToSSQ_Click(object sender, EventArgs e)
        {
            // Este método se encuentra en SSQOptions.cs
            EstimationPlan();
        }
        

        /* Descrpción:
         *  Generar un archivo Excel a partir de los datos contenidos en la tabla de facetas y datos.
         */
        private void tsmiDataExportExcel_Click(object sender, EventArgs e)
        {
            tsmiActionDataExportExcel_Click(sender, e);
        }


        /* Descripción:
         *  Exporta los datos en un fichero de texto que pueda ser recuperado por EduG
         */
        private void tsmiExportScore_Click(object sender, EventArgs e)
        {
            tsmiActionExportScore_Click(this.sagtElements.GetMultiFacetsObs());
        }


        /* Descripción:
         *  Llama al método que se encarga de exportar los datos del informe en un documento Word.
         */
        private void tsmiDataWordExport_Click(object sender, EventArgs e)
        {
            SelectElementsAndCreateWordDocument();
        }

        /* Descripción:
         *  Llama al método que se encarga de exportar los datos del informe en un documento PDF.
         */
        private void tsmiDataPdfExport_Click(object sender, EventArgs e)
        {
            SelectElementsAndCreatePDFDocument();
        }


        /* Descripción:
         *  Cancela la edición de la facetas.
         */
        private void btDataEditFacetsCancel_Click(object sender, EventArgs e)
        {
            // este método se encuentra en la clase parcial DataOptions
            btActionDataEditFacetsCancel_Click();
        }


        /*
         * Descripción: 
         *  Cierra los elementos activos en el tabPage Data. Para ello hace uso de la función
         *  closeDataElements() que se encuentra en la clase parcial DataOptions.
         */
        private void tsmiDataClose_Click(object sender, EventArgs e)
        {
            this.checkBoxHideNulls.Checked = false;
            this.checkBoxHideNulls.Enabled = false;
            // este método se encuentra en la clase parcial DataOptions
            closeDataElements();
        }


        #endregion Botones del menú de acciones de Datos


        #region Botones de los tabPage: Tabla de Facetas y Tabla de observaciones
        /*=============================================================================================
         * Botones de los TabPage: Tabla de Facetas y Tabla de observaciones
         * 
         *  - Botón Salvar Facetas al editarlas (btDataEditFacetsSave_Click)
         *  - Botón Anidar facetas (btNestingFacet_Click)
         *  - Botón Quitar anidamiento (btRemoveNesting_Click)
         *  - Botón Generar tabla de observaciones (btGenerateTableObs_Click)
         *  - Botón Cancelar (Edición de facetas) (btDataFacetsCancel_Click)
         *  - Botón Importar datos (btImportScores_Click)
         *  - Botón Salvar (Editar tabla de observaciones) (btDataObsSave_Click()
         *  - Botón Cancelar (Editar tabla de observaciones) (btDataObsCancel_Click)
         *  - Editar cometarios (btDataEditComment_Click)
         *  - Ocultar nulos en la tabla de frecuencias (checkBoxHideNulls_CheckedChanged)
         *============================================================================================*/


        /* Descripción:
         *  Guarda los cambios que se realizan tras la edición de las facetas (Modificación del nombre,
         *  tamaño del universo o descripción de una faceta. La modificación del nivel de una faceta implica
         *  reconstruir la tabla de frecuencias, por eso no se permite.)
         */
        private void btDataEditFacetsSave_Click(object sender, EventArgs e)
        {
            // este método se encuentra en la clase parcial DataOptions
            btActionDataEditFacetsSave_Click();
        }

        /* Descripción:
         *  Abre la ventana para la inserción de un anidamiento. Este botón solo esta disponible cuando
         *  se selecciona disposición de facetas mixta.
         */
        private void btNestingFacet_Click(object sender, EventArgs e)
        {
            btActionNestingFacet_Click(sender, e);
        }


        /* Descripción:
         *  Abre la ventana para la eliminación de anidamientos.
         */
        private void btRemoveNesting_Click(object sender, EventArgs e)
        {
            btActionRemoveNesting_Click(sender, e);
        }


        /* Descripción:
         *  Se activa al pulsar la el botón "Generar tabla de observaciones" del menú de datos. Llama a un método que se encuentra 
         *  en la clase parcial dataOptions
         */
        private void btGenerateTableObs_Click(object sender, EventArgs e)
        {
            btAccionGenerateTableObs_Click(sender, e);
        }


        /* Descripción:
         *  Cancela la introducción de facetas. Llama a un método que se encuentra 
         *  en la clase parcial dataOptions.
         */
        private void btDataFacetsCancel_Click(object sender, EventArgs e)
        {
            CancelAcciónEditionOfFacet();
        }


        /* Descripción:
         *  Introduce los datos leidos de un fichero de datos en la tabla de observaciones.
         */
        private void btImportScores_Click(object sender, EventArgs e)
        {
           btActionImportScores_Click(this.sagtElements.GetMultiFacetsObs(),this.dataGridViewExObsTable);
        }


        /* Descripción:
        *  Tras pulsar en el botón aceptar del tabPage Tabla de observaciones almacenamos los datos 
        *  en un archivo.
        */
        private void btDataObsSave_Click(object sender, EventArgs e)
        {
            // Llama a un método de la clase parcial DataOptions.cs
            btDataObsOkAndSaveFile();
        }


        /* Descripción:
         *  Cancelamos la edición de la tabla de variables observadas. Si el objeto multifaceta esta siendo 
         *  creado de un objeto multifaceta por primera vez dejamos las tablas vacias. Si lo estamos editando 
         *  cancelamos la edidción y restauramos los valores del archivo.
         */
        private void btDataObsCancel_Click(object sender, EventArgs e)
        {
            /* Llamamos a la operación de cancelar la edición de un MultiObsTable.
             * El método se encuentra en la clase parcial DataOptions.cs */
            btActionCancelGenerateTableObs_Click();
        }


        /* Descripción:
         *  Se ejecuta al pulsar sobre el botón de editar en el tabPage Información de la
         *  poción datos.
         */
        private void btDataEditComment_Click(object sender, EventArgs e)
        {
            btActionDataEditComment_Click(sender, e);
        }

        /* Descripción:
         *  Este evento se activa al cambiar el estado del checkBox "Ocular nulos" (CheckBoxHideNull). Al marcarlo deben
         *  ocultarse los valores nulos de la tabla de frecuencias. Al desmarcarlo deben mostrarse todos los valores.
         */
        private void checkBoxHideNulls_CheckedChanged(object sender, EventArgs e)
        {
            CWait fw = new CWait(msgLoading);
            Thread th = new Thread(new ThreadStart(fw.CWaitShowDialog));
            try
            {
                // Start the thread
                th.Start();
                MultiFacetsObs multiFacets = this.sagtElements.GetMultiFacetsObs();
                if (this.checkBoxHideNulls.Checked)
                {
                    // Si la propiedad checked esta a true se ocultan los valores
                    loadData_hideNulls_InTabPageObsTable(multiFacets);
                }
                else
                {
                    // Si la propiedad checked esta a false se muestran todos los valores
                    loadDataInTabPageObsTable(multiFacets);
                }
                th.Abort();
            }
            catch (IOException ex)
            {
                th.Abort();
                ShowMessageErrorOK(ex.Message);
            }
        }// end checkBoxHideNulls_CheckedChanged

        #endregion Botones de los tabPage: Tabla de Facetas y Tabla de observaciones


        #region Botones del menú de acciones de Medias
        /*===============================================================================================
         * Botones del menú vertical de la opción Medias
         *  - Abrir (tsmiMeansOpenLocal_Click)
         *  - Importar
         *  - Guardar
         *  - Información
         *  - Exportar a excel
         *  - Imprimir
         *  - Vista previa
         *  - Exportar a Word(tsmiMeansWordExport_Click)
         *  - Exportar a PDF (tsmiMeansPdfExport_Click)
         *  - Cerrar
         *  - Editar (Boton editar comentario)
         *===============================================================================================*/

        /* Descripción:
         *  Abre un archivo de medias.
         */
        private void tsmiMeansOpenLocal_Click(object sender, EventArgs e)
        {
            tsmiActionOpenMeans_Click(sender, e);
        }


        /* Descripción:
         *  Abre un fichero tomando los datos de una base de datos alojada en un servicio Web.
         */
        private void tsmiMeansOpenWebService_Click(object sender, EventArgs e)
        {
            tsmiActionOpenWebService_Click(TypeFile.sagt);
        }


        /*
         * Descripción:
         *  Al pulsar el la opción del menú importar muestra la ventana para seleccionar el tipo
         *  de archivo del que se quieren leer los datos.
         */
        private void tsmiImportMeans_Click(object sender, EventArgs e)
        {
            // llamamos al metodo tsmiActionImportMeans_Click de la clase parcial MeansOption.cs
            tsmiActionImportMeans_Click(sender, e);
        }


        /* Descripción:
         *  Guarda las tablas de medias en un archivo
         */
        private void tsmiMeansSavedLocal_Click(object sender, EventArgs e)
        {
            tsmiActionMeansSave_Click(sender, e);
        }


        /* Descripción:
         *  Guarda los datos de un archivo en un servicio web.
         */
        private void tsmiMeansSavedWebService_Click(object sender, EventArgs e)
        {
            if ((sagtElements.GetListMeans() == null))
            {
                ShowMessageErrorOK(errorNoListMeans);
            }
            else
            {
                tsmiActionDataSavedWebService_Click();
            }
        }


        /* Descripción:
         *  Accede directamente al tabPage de información.
         */
        private void tsmiMeanInformation_Click(object sender, EventArgs e)
        {
            if ((sagtElements.GetListMeans() == null))
            {
                ShowMessageErrorOK(errorNoFileSelected);
            }
            else
            {
                ListMeans listMeans = this.sagtElements.GetListMeans();
                if (listMeans != null)
                {
                    int numTabPage = this.tabControlMeans.Controls.Count;
                    this.tabControlMeans.SelectedIndex = numTabPage - 1;
                }
            }
        }


        /* Descrpción:
         *  Generar un archivo Excel a partir de los datos contenidos en las tablas medias.
         */
        private void tsmiMeansExportExcel_Click(object sender, EventArgs e)
        {
            tsmiActionMeansExportExcel_Click(sender, e);
        }


        /* Descripción:
         *  Llama al método que se encuentra en la clase parcial PrintSagtDocuments.cs que se encarga de
         *  mostrar una ventana de selección de impresora y luego imprime el documento.
         */
        private void tsmiMeansPrinter_Click(object sender, EventArgs e)
        {
            tsmiDataPrinter_Click(sender, e);
        }


        /* Descripción:
         *  Llama al método que se encuentra en la clase parcial PrintSagtDocuments.cs 
         *  Muestra la ventana de selección de informes y un a vez seleccionados muestra la vista previa.
         */
        private void tsmiMeansPrintPreview_Click(object sender, EventArgs e)
        {
            tsmiDataPrintPreview_Click(sender, e);
        }


        /* Descripción:
         *  Llama al método que se encarga de exportar los datos del informe en un documento Word.
         */
        private void tsmiMeansWordExport_Click(object sender, EventArgs e)
        {
            SelectElementsAndCreateWordDocument();
        }


        /* Descripción:
         *  Llama al método que se encarga de exportar los datos del informe en un documento PDF.
         */
        private void tsmiMeansPdfExport_Click(object sender, EventArgs e)
        {
            SelectElementsAndCreatePDFDocument();
        }


        /* Descripción:
         *  Cierra las medias que se encuentren abiertas, tras pedir confirmación.
         */
        private void tsmiMeansClose_Click(object sender, EventArgs e)
        {
            // Llama al método que cierra las medias de la clase parcial MeansOptions.cs
            tsmiActionMeansClose_Click(sender, e);
        }


        /* Descripción:
         * Boton editar, permite editar en el richTextBox
         */
        private void btMeanEditComment_Click(object sender, EventArgs e)
        {
            // Llama al método que se encuetra en la clase parcial MeansOptions.cs
            btActionMeanEditComment_Click(sender, e);
        }

        #endregion Botones del menú de acciones de Medias


        #region Botones del menú de acciones de Suma de cuadrados
        /*===============================================================================================
         * Botones del menú vertical de la opción Suma de cuadrados
         *  - Abrir (tsmiSSqOpenLocal_Click)
         *  - Importar (tsmiSSQImport_Click)
         *  - Guardar (tsmiSSqSave_Click)
         *  - Editar descripción (tsmiEditFacetDescription_Click)
         *  - Añadir nivel de significación (tsmiAddLevelSign_Click)
         *  - Muestra gráfico de barras (tsmiChartOptimization_Click)
         *  - Muestra gráfico de Coef G Abs (tsmiChartCoefGAbs_Click)
         *  - Muestra gráfico de Coef G Rel (tsmiChartCoefGRel_Click)
         *  - Analizar (tsmiSSqToAnalysis_Click)
         *  - Exportar a excel
         *  - Exportar cuadrados
         *  - Imprimir (tsmiSsqPrinter_Click)
         *  - Vista previa (tsmiSsqPrintPreview_Click)
         *  - Exportar a Word (tsmiSSqWordExport_Click)
         *  - Exportar a PDF (tsmiSSqPdfExport_Click)
         *  - Cerrar suma de cuadrados
         *  - Editar (Botón editar comentario)
         *===============================================================================================*/

        /* Descripción:
         *  Permite abrir un fichero de suma de cuadrados
         */
        private void tsmiSSqOpenLocal_Click(object sender, EventArgs e)
        {
            tsmiActionOpenSSQ_Click(sender, e);
        }


        /* Descripción:
         *  Permite abrir un fichero almacenado en una base de datos.
         */
        private void tsmiSSqOpenWebService_Click(object sender, EventArgs e)
        {
            tsmiActionOpenWebService_Click(TypeFile.sagt);
        }


        /* Descripción:
         *  Al pulsar el la opción del menú importar muestra la ventana para seleccionar el tipo
         *  de archivo del que se quieren leer los datos para importar la suma de cuadrado.
         */
        private void tsmiSSQImport_Click(object sender, EventArgs e)
        {
            // el siguiente metodo se encuentra en la clase parcial SSQOptions.cs
            tsmiActionSSQImport_Click(sender, e);
        }


        /* Descripción:
         *  Guarda en un archivo los datos de la tabla de análisis de varianza, G_studie y lista de
         *  parámetros de optimización (nuevos niveles de optimización.)
         */
        private void tsmiSSqSavedLocal_Click(object sender, EventArgs e)
        {
            tsmiActionSSQSave_Click(sender, e);
        }


        /* Descripción:
         *  Se ejecuta al pulsar sobre el botón "Editar descripción". Permite modificar la descripción de
         *  las facetas desde la opción de suma de cuadrados.
         */
        private void tsmiEditFacetDescription_Click(object sender, EventArgs e)
        {
            if ((sagtElements.GetAnalysis_and_G_Study() == null))
            {
                ShowMessageErrorOK(errorNoSSQ);
            }
            else
            {
                tsmiActionEditFacetDescription_Click();
            }
        }


        /* Descripción: 
         *  Almacena los datos de un archivo en un servicio Web.
         */
        private void tsmiSSqSavedWebService_Click(object sender, EventArgs e)
        {
            if ((sagtElements.GetAnalysis_and_G_Study() == null))
            {
                ShowMessageErrorOK(errorNoSSQ);
            }
            else
            {
                tsmiActionDataSavedWebService_Click();
            }
        }
        

        /*
         * Descripción: 
         *  Llama al método que añade el nuevo nivel de significación en la clase parcial SSQOptions.cs
         */
        private void tsmiAddLevelSign_Click(object sender, EventArgs e)
        {
            Analysis_and_G_Study tAnalysis_tG_Study_Opt = this.sagtElements.GetAnalysis_and_G_Study();
            tsmiActionAddLevelSign_Click(tAnalysis_tG_Study_Opt);
            //AddSignificanceLevel(this.tAnalysis_tG_Study_Opt);
        }


        /* Descripción:
         *  Se ejecuta al seleccióna la opción "Gráfico 1" del menú de acciónes de suma de cuadrados.
         *  Muestra un gráfico de barras.
         */
        private void tsmiChartOptimization_Click(object sender, EventArgs e)
        {
            Analysis_and_G_Study tAnalysis_tG_Study_Opt = this.sagtElements.GetAnalysis_and_G_Study();
            ShowMeTheGraphics(tAnalysis_tG_Study_Opt);
        }


        /* Descripción:
         *  Se ejecuta al seleccióna la opción "Gráfico Coef. G Abs" del menú de acciónes de suma de cuadrados.
         *  Muestra una gráfica de representación lineal.
         */
        private void tsmiChartCoefGAbs_Click(object sender, EventArgs e)
        {
            Analysis_and_G_Study tAnalysis_tG_Study_Opt = this.sagtElements.GetAnalysis_and_G_Study();
            tsmiActionChartCoefGAbs_Click(tAnalysis_tG_Study_Opt);
        }


        /* Descripción:
         *  Se ejecuta al seleccióna la opción "Gráfico Coef. G Rel" del menú de acciónes de suma de cuadrados.
         *  Muestra una gráfica de representación lineal.
         */
        private void tsmiChartCoefGRel_Click(object sender, EventArgs e)
        {
            Analysis_and_G_Study tAnalysis_tG_Study_Opt = this.sagtElements.GetAnalysis_and_G_Study();
            tsmiActionChartCoefGRel_Click(tAnalysis_tG_Study_Opt);
        }


        /* Descripción:
         *  Se ejecuta al pulsar el botón Analizar en el  menú  horizontal de suma de cuadrados. Llama
         *  al método que crea un objeto de analysis de suma de cuadrados.
         */
        private void tsmiSSqToAnalysis_Click(object sender, EventArgs e)
        {
            tsmiActionSSqToAnalysis_Click();
        }


        /* Descrpción:
         *  Generar un archivo Excel a partir de los datos contenidos en las tablas de suma de cuadrados.
         */
        private void tsmiSSq_ExportExcel_Click(object sender, EventArgs e)
        {
            tsmiActionSSq_ExportExcel_Click(sender, e);
        }


        /* Descipción:
         *  Exporta la lista de suma de cuadrados en un fichero de texto.
         */
        private void tsmiSSq_ExportSquares_Click(object sender, EventArgs e)
        {
            Analysis_and_G_Study tAnalysis_tG_Study_Opt = this.sagtElements.GetAnalysis_and_G_Study();
            tsmiAction_SSq_ExportSquares_Click(tAnalysis_tG_Study_Opt);
        }


        /* Descripción:
         *  Llama al método que se encuentra en la clase parcial PrintSagtDocuments.cs que se encarga de
         *  mostrar una ventana de selección de impresora y luego imprime el documento.
         */
        private void tsmiSsqPrinter_Click(object sender, EventArgs e)
        {
            tsmiDataPrinter_Click(sender, e);
        }


        /* Descripción:
         *  Llama al método que se encuentra en la clase parcial PrintSagtDocuments.cs 
         *  Muestra la ventana de selección de informes y un a vez seleccionados muestra la vista previa.
         */
        private void tsmiSsqPrintPreview_Click(object sender, EventArgs e)
        {
            tsmiDataPrintPreview_Click(sender, e);
        }


        /* Descripción:
         *  Llama al método que se encarga de exportar los datos del informe en un documento Word.
         */
        private void tsmiSSqWordExport_Click(object sender, EventArgs e)
        {
            SelectElementsAndCreateWordDocument();
        }


        /* Descripción:
         *  Llama al método que se encarga de exportar los datos del informe en un documento PDF.
         */
        private void tsmiSSqPdfExport_Click(object sender, EventArgs e)
        {
            SelectElementsAndCreatePDFDocument();
        }


        /* Descripción:
         *  Cierra todos los elementos abiertos en suma de cuadrados. Llama al método tsmiActionSSQClose_Click
         *  que se encuentra en la clase parcial SSQOptions.cs
         */
        private void tsmiSSQClose_Click(object sender, EventArgs e)
        {
            tsmiActionSSQClose_Click(sender, e);
        }


        /* Descripción:
         * Botón editar, permite editar comentarios en el richTextBox de suma de cuadrados.
         */
        private void btSsqEditComment_Click(object sender, EventArgs e)
        {
            // Llama al método que se encuetra en la clase parcial SSQOptions.cs
            btActionSSQEditComment_Click(sender, e);
        }


        /* Descripción:
         *  Se activa al pulsar sobre el botón Aceptar de la pestaña editar descripción de facetas.
         */ 
        private void btEditDescriptionFacetsAcept_Click(object sender, EventArgs e)
        {
            btActionEditDescriptionFacetsAcept_Click();
        }


        /* Descripción:
         *  Se activa al pulsar sobre el botón Cancelar de la pestaña editar descripción de facetas.
         */
        private void btEditDescriptionFacetsCancel_Click(object sender, EventArgs e)
        {
            btActionEditDescriptionFacetsCancel_Click();
        }

        #endregion Botones del menu de acciones de Suma de cuadrados


        #region Botones del menú de acciones de Análisis
        /***************************************************************************************************
         * Botones del menú vertical de la opción Análisis:
         * ================================================
         * 
         *  - Nuevo (tsmiNewFileAnalysisSSQ_Click)
         *  - Abrir (tsmiOpenAnalysis_Click)
         *  - Importar (tsmiImportAnalysis_Click)
         *  - Guardar (tsmiAnalysis_Save_Click)
         *  - Editar facetas (tsmiAnalysisEditFacets_Click)
         *  - Cambiar modelo (tsmiChangeModel_Click)
         *  - Editar cuadrados (tsmiAnalysisEditSsq_Click)
         *  - Niveles de optimización (tsmiAnalysis_AddLevelSign_Click)
         *  - Gráfico optimización(tsmiAnalysisChartOptimization_Click)
         *  - Gráfico Coef. G Abs (tsmiAnalysisChartCoefGAbs_Click)
         *  - Gráfico Coef. G Rel (tsmiAnalysisChartCoefGRel_Click)
         *  - Exportar a Excel (tsmiAnalysisExportExcel_Click)
         *  - Exportar sumas de cuadrados (tsmiAnalysis_ExportSquares_Click)
         *  - Exportar a Word (tsmiAnalysisWordExport_Click)
         *  - Exportar a PDF (tsmiAnalysisPdfExport_Click)
         *  - Cerrar (tsmiCloseAnalysis_Click)
         ***************************************************************************************************/

        /* Descripción:
         *  Llama al método tsmiActionNewFileAnalysisSSQ_Click de la clase parcial AnalysisOptions.cs
         *  que se encarga de crear un nuevo fichero de suma de cuadrados a partir de los datos introducidos
         *  por el usuario.
         */
        private void tsmiNewFileAnalysisSSQ_Click(object sender, EventArgs e)
        {
            tsmiActionNewFileAnalysisSSQ_Click(sender, e);
        }


        /* Descipción:
         *  Llama al método tsmiActionOpenAnalysis_Click de la clase parcial AnalysisOptions.cs
         *  que se encarga de abrir un nuevo fichero de suma de cuadrados.
         */
        private void tsmiAnalysisOpenLocal_Click(object sender, EventArgs e)
        {
            tsmiActionOpenAnalysis_Click();
        }


        /* Descripción:
         *  Recupera los datos de un fichero de análisis de la base de datos del servicio Web.
         */ 
        private void tsmiAnalysisOpenWebService_Click(object sender, EventArgs e)
        {
            tsmiActionOpenWebService_Click(TypeFile.anls);
        }


        /* Descipción:
         *  Llama al método tsmiActionImportAnalysis_Click de la clase parcial AnalysisOptions.cs
         *  que se encarga de importar un nuevo fichero de suma de cuadrados perteneciente a alguna
         *  de las otras aplicaciones de teoria de la generalizabilidad (EduG 6.0, GT 2.0).
         */
        private void tsmiImportAnalysis_Click(object sender, EventArgs e)
        {
            tsmiActionImportAnalysis_Click();
        }


        /* Descripción:
         *  Llama al método tsmiActionAnalysis_Save_Click de la clase parcial AnalysisOptions.cs
         *  que se encarga de guardar un nuevo fichero de suma de cuadrados.
         */
        private void tsmiAnalysisSavedLocal_Click(object sender, EventArgs e)
        {
            tsmiActionAnalysis_Save_Click();
        }


        /* Descripción:
         *  Almacena el contenido de un archivo de análisis en la base de datos del servicio Web
         */
        private void tsmiAnalysisvedWebService_Click(object sender, EventArgs e)
        {
            if ((this.anl_tAnalysis_G_study_opt == null))
            {
                ShowMessageErrorOK(errorNoSSQ);
            }
            else
            {
                tsmiActionAnalysisSavedWebService_Click();
            }
        }


        /* Descripción:
         *  Llama al método que se encarga de editar las facetas de la suma de cuadrados de
         *  la opción análisis.
         */
        private void tsmiAnalysisEditFacets_Click(object sender, EventArgs e)
        {
            tsmiActionAnalysisEditFacets_Click();
        }



        /* Descripción:
         *  Permite cambiar el diseño de medida de un modelo.
         */
        private void tsmiChangeModel_Click(object sender, EventArgs e)
        {
            tsmiActionChangeModel_Click();
        }


        /* Descripción:
         *  Llama al método que se encarga de editar  la suma de cuadrados de la opción análisis.
         */
        private void tsmiAnalysisEditSsq_Click(object sender, EventArgs e)
        {
            tsmiActionAnalysisEditSsq_Click();
        }


        /* Descripción:
         *  Llama al método tsmiActionAnalysis_AddLevelSign_Click de la clase parcial AnalysisOptions.cs
         *  que se encarga de guardar un nuevo fichero de suma de cuadrados.
         */
        private void tsmiAnalysis_AddLevelSign_Click(object sender, EventArgs e)
        {
            tsmiActionAnalysis_AddLevelSign_Click(this.anl_tAnalysis_G_study_opt);
        }


        /* Descripción:
         *  Se activa al seleccionar la opción de mostrar gráfico de barras con los parámetros
         *  de resumen de optimización.
         */
        private void tsmiAnalysisChartOptimization_Click(object sender, EventArgs e)
        {
            ShowMeTheGraphics(this.anl_tAnalysis_G_study_opt);
        }


        /* Descripción:
         *  Se activa al seleccionar la opción de mostrar el gráfico con el coeficiente G Absoluto
         */
        private void tsmiAnalysisChartCoefGAbs_Click(object sender, EventArgs e)
        {
            tsmiActionChartCoefGAbs_Click(this.anl_tAnalysis_G_study_opt);
        }


        /* Descripción:
         *  Se activa al seleccionar la opción de mostrar el gráfico con el coeficiente G Relativo
         */
        private void tsmiAnalysisChartCoefGRel_Click(object sender, EventArgs e)
        {
            tsmiActionChartCoefGRel_Click(this.anl_tAnalysis_G_study_opt);
        }


        /* Descripción:
         *  Generar un archivo Excel a partir de los datos contenidos en las tablas de Análisis.
         */
        private void tsmiAnalysisExportExcel_Click(object sender, EventArgs e)
        {
            tsmiActionAnalysisExportExcel_Click(sender, e);
        }


        /* Descipción:
         *  Exporta la lista de suma de cuadrados en un fichero de texto.
         */
        private void tsmiAnalysis_ExportSquares_Click(object sender, EventArgs e)
        {
            // Mandamos las tabla de análisis de varianza como parámetro del método
            tsmiAction_SSq_ExportSquares_Click(this.anl_tAnalysis_G_study_opt);
        }


        /* Descripción:
         *  Llama al método que se encarga de exportar los datos del informe en un documento Word.
         */
        private void tsmiAnalysisWordExport_Click(object sender, EventArgs e)
        {
            WriterAnalysisWordDocument();
        }


        /* Descripción:
         *  Llama al método que se encarga de exportar los datos del informe en un documento PDF.
         */
        private void tsmiAnalysisPdfExport_Click(object sender, EventArgs e)
        {
            CreateAnalysisPdfDocument();
        }


        /* Descripción:
         *  Llama al método tsmiActionNewFileAnalysisSSQ_Click de la clase parcial AnalysisOptions.cs que
         *  se encarga de cerrar  los elementos habiertos e inicializarlos.
         */
        private void tsmiCloseAnalysis_Click(object sender, EventArgs e)
        {
            tsmiActionCloseAnalysis_Click(sender, e);   
        }

        #endregion Botones del menú de acciones de Análisis


        #region Botones de los tabPage de la Opción Análisis
        /***************************************************************************************************
         * Botones de los tabPage de la Opción Análisis
         * 
         *  - Anidar facetas (btAnalysis_NestingFacet_Click)
         *  - Quitar anidamiento (btAnalysis_RemoveNesting_Click)
         *  - Genera y Edita tabla de suma de cuadrados (btEditSumOfSquaresOnAnalisys_Click)
         *  - Cancelar edición de facetas (btCancelEditFacetOnAnalysis_Click)
         *  - Importar suma de cuadrados (btImportAnalysisEditSsq_Click)
         *  - Salva los datos del analysis de suma de cuadrados. (btSaveAnalysisSsq_Click)
         *  - Cancela la operación editar la suma de cuadrados (btCancelEditSsq_Click)
         *  - Editar comentarios (btAnalysisEditComment_Click)
         ***************************************************************************************************/


        /* Descripción:
         *  Muestra la ventana desde donde seleccionaremos tanto la faceta anidante como la anidada.
         *  Este llama a su vez al método btActionEditSumOfSquaresOnAnalisys_Click de la 
         *  clase parcial AnalysisOptions.cs
         */
        private void btAnalysis_NestingFacet_Click(object sender, EventArgs e)
        {
            btActionAnalysis_NestingFacet_Click();   
        }


        /* Descripción:
         *  Muestra la ventana para seleccionar aquellos diseños de facetas que queramos eliminar de 
         *  nuestro diseño. Este llama a su vez al método btActionEditSumOfSquaresOnAnalisys_Click de la 
         *  clase parcial AnalysisOptions.cs
         */
        private void btAnalysis_RemoveNesting_Click(object sender, EventArgs e)
        {
            btActionAnalysis_RemoveNesting_Click();
        }


        /* Descripción:
         *  Lee las facetas de la tabla y si estan corectos muestra la tabla para introducir la suma de
         *  los cuadrados. Este llama a su vez al método btActionEditSumOfSquaresOnAnalisys_Click de la 
         *  clase parcial AnalysisOptions.cs
         */
        private void btEditSumOfSquaresOnAnalisys_Click(object sender, EventArgs e)
        {
            btActionEditSumOfSquaresOnAnalisys_Click();
        }


        /* Descripción:
         *  Lanza la operación cuando se aceptan los cambios de la edición de facetas.
         *  Este llama a su vez al método btActionAcept_Click de la 
         *  clase parcial AnalysisOptions.cs
         */
        private void btAcept_Click(object sender, EventArgs e)
        {
            btActionAcept_Click();
        }


        /* Descripción:
         *  Se activa al pulsar cancelar del tabPageFacets de la opción Análisis. Este llama a su vez al
         *  método btActionCancelEditFacetOnAnalysis_Click de la clase parcial AnalysisOptions.cs
         */
        private void btCancelEditFacetOnAnalysis_Click(object sender, EventArgs e)
        {
            btActionCancelEditFacetOnAnalysis_Click(sender, e);
        }


        /* Descripción:
         *  Se activa al pulsar el botón de importar suma de cuadrados en la edición de sumas 
         *  de cuadrados de la opción análisis.
         */
        private void btImportAnalysisEditSsq_Click(object sender, EventArgs e)
        {
            
            btActionImportAnalysisEditSsq_Click(this.dgvExAnalysisEditSSq);
        }


        /* Descripción:
         *  Guarda los datos tras introducir la suma de cuadrados. Este llama 
         *  a su vez al método btActionSaveAnalysisSsq_Click de la clase parcial AnalysisOptions.cs
         */
        private void btSaveAnalysisSsq_Click(object sender, EventArgs e)
        {
            btActionSaveAnalysisSsq_Click(sender, e);
        }


        /* Descripción:
         *  Cancela la operación de introducir la suma de cuadrados para su posterior análisis. Este llama 
         *  a su vez al método btActionCancelEditSsq_Click de la clase parcial AnalysisOptions.cs
         */
        private void btCancelEditSsq_Click(object sender, EventArgs e)
        {
            btActionCancelEditSsq_Click();
        }

        

        /* Descripción:
         *  Permite añadir y editar comentarios en el tabPageInfo de la opción Análisis
         */
        private void btAnalysisEditComment_Click(object sender, EventArgs e)
        {
            // Llama al método que se encuetra en la clase parcial AnalysisOptions.cs
            btActionAnalysisEditComment_Click(this.anl_tAnalysis_G_study_opt);
        }


        #endregion Botones de los tabPage de la Opción Analisis


        #region Eventos sobre el form principal
        /*=================================================================================================
         * Eventos sobre el form principal
         * ===============================
         *  - FormPrincipal_Load (cargar la ventana)
         *  - FormPrincipal_SizeChanged (cambiar el tamaño)
         *=================================================================================================*/

        /* Descripción:
         *  Muestra la ventana de carga (splashscreen) al inicio de la aplicación
         */
        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            FormSplashScreen splashScreen = new FormSplashScreen(version);
            splashScreen.Show();
        }


        /* Descripción:
         *  Refresca la ventana tras cambiar el tamaño de la ventana. Evita que aparezcan marcas raras.
         */
        private void FormPrincipal_SizeChanged(object sender, EventArgs e)
        {
            // Refrescamos la ventana
            this.Refresh();

            Application.DoEvents();
        }

        #endregion Eventos sobre el form principal


        #region  Eventos sobre el DataGridViewEx de Facetas
        /*===================================================================================================
         * Eventos
         *  - dataGridViewExFacets_KeyPressEditorCelda (Opcion Datos)
         *  - dGridViewExAnalysis_TableFacet_KeyPressEditorCelda (Opción Análisis)
         *  - dGridViewExDataEditFacets_KeyPressEditorCelda (Opción Datos)
         *  - EditionDataGridViewExFacets (Método auxiliar de los dos anteriores)
         *  - dataGridViewExObsTable_KeyPressEditorCelda (Opción Datos)
         *  - dgvExAnalysisEditSSq_KeyPressEditorCelda (Opción Análisis)
         *  - writerDouble_KeyPressEditorCelda (Método auxiliar de los dos anteriores)
         *===================================================================================================*/


        /* Descripción:
         *  Evento que se lanza sobre el dataGridViewEx de facetas. Este evento no es compartido
         *  de dataGridView original de Visual Studio. Realiza una llamada a otro proceso que es el
         *  que se encarga de todo ya que su función es compartida por varios dataGrid.
         *  Si se esta editando la columna de nombre de las facetas, impide que se escriba otro coracter que
         *  no sea una letra un número o el espacio.
         *  Si se esta editando el tamaño del universo solo se podrá escribir un número entero o si pusalmos
         *  la tecla 'i' escribimos "INF".
         */
        private void dataGridViewExFacets_KeyPressEditorCelda(object sender, KeyPressEventArgs e)
        {
            EditionDataGridViewExFacets(this.dataGridViewExFacets, sender, e);
        }


        /* Descripción:
         *  Controla la escritura de la columna Tamaño del universo de el dataGridViewEx dGridViewExAnalysis_TableFacet.
         *  Permite tan solo la escritura de enteros o si se pulsa la i escribe "INF" (Infinito).
         */
        private void dGridViewExAnalysis_TableFacet_KeyPressEditorCelda(object sender, KeyPressEventArgs e)
        {
            EditionDataGridViewExFacets(this.dGridViewExAnalysis_TableFacet, sender, e);
        }


        /* Descripción
         *  Evento que controla la edición del campo Tamaño del universo en el dataGrid de edición de facetas.
         */
        private void dGridViewExDataEditFacets_KeyPressEditorCelda(object sender, KeyPressEventArgs e)
        {
            EditionDataGridViewExFacets(this.dGridViewExDataEditFacets, sender, e);
        }


        /* Descripción:
         *  Realiza el proceso de control sobre la escritura de la columna Tamaño del universo para
         *  el dataGridViewEx que se pasa como parámetro.
         */
        private void EditionDataGridViewExFacets(DataGridViewEx.DataGridViewEx dgvEx,
            object sender, KeyPressEventArgs e)
        {
            DataGridViewCell celda = dgvEx.CurrentCell;
            int columnIndex = dgvEx.CurrentCell.ColumnIndex;

            if (dgvEx.Columns[columnIndex].Name == "nameColFacet")
            {
                if (Char.IsLetter(e.KeyChar))
                {
                    e.Handled = false;
                }
                else if (Char.IsControl(e.KeyChar))
                {
                    e.Handled = false;
                }
                else if (Char.IsSeparator(e.KeyChar))
                {
                    e.Handled = false;
                }
                else if (e.KeyChar >= 48 && e.KeyChar <= 57)
                {
                    // Código 48 se coresponde con el cero y el 57 con el nueve
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else if (dgvEx.Columns[columnIndex].Name == "nameColSizeOfUniverse")
            {
                if (e.KeyChar == 'i' || e.KeyChar == 'I')
                {
                    celda.Value = "Inf";// Facet.INFINITE;
                    int pos = dgvEx.CurrentRow.Index;
                    dgvEx.CurrentCell = dgvEx.Rows[pos].Cells[0];
                    dgvEx.CurrentCell = dgvEx.Rows[pos].Cells[columnIndex];
                    e.Handled = false;
                    dgvEx.CurrentCell.Value = "INF";
                }
                else
                {
                    if (e.KeyChar == 8)
                    {
                        e.Handled = false;
                        return;
                    }
                    // Código 48 se coresponde con el cero y el 57 con el nueve
                    if (e.KeyChar >= 48 && e.KeyChar <= 57)
                        e.Handled = false;
                    else
                        e.Handled = true;
                }
            }
        }// end EditionDataGridViewExFacets


        /* Descripción:
         *  Se encarga de actualizar inmediatamente los valores de las celdas de checkBox al ser clicadas
         */
        private void dataGridViewExFacets_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridViewExFacets.CurrentCell is DataGridViewCheckBoxCell)
            {
                dataGridViewExFacets.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        /* Descripción:
         *  Evento que controla que se pulsen las teclas validas para editar la tabla de frecuencias.
         */
        private void dataGridViewExObsTable_KeyPressEditorCelda(object sender, KeyPressEventArgs e)
        {
            writerDouble_KeyPressEditorCelda(dataGridViewExObsTable, sender, e);
        }


        /* Descripción:
         *  Evento que controla que se pulsen las teclas validas para editar la suma de cuadrados.
         */
        private void dgvExAnalysisEditSSq_KeyPressEditorCelda(object sender, KeyPressEventArgs e)
        {
            writerDouble_KeyPressEditorCelda(dgvExAnalysisEditSSq, sender, e);
        }


        /* Descripción:
         *  Controla la edición de la ultima columna de la tabla de frecuencia permitiendo que
         *  unicamente se escriban los caracteres validos para la escritura de un "double".
         */
        private void writerDouble_KeyPressEditorCelda(DataGridViewEx.DataGridViewEx dgvEx, object sender, KeyPressEventArgs e)
        {
            // Código 48 se coresponde con el cero y el 57 con el nueve
            if (e.KeyChar >= 48 && e.KeyChar <= 57)
            {
                e.Handled = false;
            }
            else if (e.KeyChar == '.' || e.KeyChar == ',' || e.KeyChar == '-')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }


        #endregion  Eventos sobre el DataGridViewEx de Facetas    


        #region Botones del menú de acciones de Proyectos
        /***************************************************************************************************
         * Botones del tabPage de la Opción Proyectos
         * 
         *  - Crear (tsmiAddProject_Click)
         *  - Editar (tsmiEditProject_Click)
         *  - Buscar (tsmiSearchProject_Click)
         *  - Asignar director (tsmiAddDirectorToProject_Click)
         ***************************************************************************************************/
        /* Descripción:
         *  Se activa al hacer click sobre la opción "Crear Proyecto"
         */
        private void tsmiAddProject_Click(object sender, EventArgs e)
        {
            tsmiActionAddProject_Click();
        }


        /* Descripción:
         *  Se activa al pulsar en la opción editar del menú vertical de proyectos.
         */
        private void tsmiEditProject_Click(object sender, EventArgs e)
        {
            tsmiActionEditProject_Click();
        }



        /* Descripción:
         *  Se activa al pulsar en la opción buscar del menú vertical de proyectos
         */
        private void tsmiSearchProject_Click(object sender, EventArgs e)
        {
            tsmiActionSearchProject_Click();
        }


        /* Descripción:
         *  Se activa al pulsar sobre la opción Asignar Director.
         */
        private void tsmiAddDirectorToProject_Click(object sender, EventArgs e)
        {
            tsmiActionAddDirectorToProject_Click();
        }


        #endregion Botones del menú de acciones de Proyectos

        

        #region Botones del tapPage Proyectos
        /***************************************************************************************************
         * Botones del tabPage de la Opción Proyectos
         * 
         *  - Botón Aceptar (btProyectOk_Click)
         *  - Botón Cancelar (btProyectCancel_Click)
         ***************************************************************************************************/

        /* Descripción:
         *  Se activa al hacer clic en le botón Aceptar de la opción proyectos
         */
        private void btProyectOk_Click(object sender, EventArgs e)
        {
            switch(this.stateProyect)
            {
                case(StateOfOptionProyects.AddProyect):
                    AddProyect();
                    break;
                case(StateOfOptionProyects.Search):
                    SearchProject();
                    break;
                case(StateOfOptionProyects.Edition):
                    EditionProject();
                    break;
            }
        }// end btProyectOk_Click


        /* Descripción:
         *  Se activa al hacer clic en le botón Cancelar de la opción proyectos.
         */
        private void btProyectCancel_Click(object sender, EventArgs e)
        {
            //switch (this.stateProyect)
            //{
            //    case (StateOfOptionProyects.AddProyect):
            //        ExitsAddProyect();
            //        break;
            //}

            /* Es posible que renombre este proyecto por btActionProyectCancel_Click
             */
            ExitsAddProject();
        }// end btProyectCancel_Click


        #endregion Botones del tapPage Proyectos


        
    } // end public partial class FormPrincipal : Form
} // end namespace GUI_TG