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
 * Fecha de revisión: 03/Jul/2012
 * 
 * Descripción:
 *      Clase parcial ("partial") del FormPrincipal. Contiene los métodos referentes a la parte de
 *      Proyectos: Creación de proyectos y su gestión.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing; // se usa para las propiedades de la cabecera de columna (color,fuente,etc)
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using ConnectLibrary;
using Sagt;
using MultiFacetData;
using ProjectMeans;
using ProjectSSQ;
using System.Web.Services.Protocols;


namespace GUI_GT
{
    public partial class FormPrincipal : Form
    {
        /***********************************************************************************************
         * Variables
         ***********************************************************************************************/
        // Proyecto activo
        private SagtProject activeProject;
        // Usuario Activo
        private SagtUser activeUser;
        // Por defecto el estado es normal.
        private StateOfOptionProyects stateProyect = StateOfOptionProyects.Normal;

        // Enumerado para determinar el estado
        public enum StateOfOptionProyects
        {
            Normal, AddProyect, Edition, Search
        }


        /* Descripción:
         *  Inicializa la opción de Proyectos:
         *  inactiva el menú y los datos del groupBox proyecto activo hasta que se conecte.
         */
        private void InitializeProjectsOption()
        {
            EnableProjectsOption(false);
            EnabledOrDisabledButtonOkCancel(false);
            ReadOnlyProyectsText(true);
        }


        /* Descripción:
         *  Habilita la opción de proyectos
         */
        private void EnableProjectsOption(bool enableOption)
        {
            this.mStripProjects.Enabled = enableOption;
            this.gBoxActiveProject.Enabled = enableOption;
        }


        /* Descripción:
         *  Habilita o deshabilita los botones de Aceptar y Cancelar en función del parámetro
         *  de entrada.
         */
        private void EnabledOrDisabledButtonOkCancel(bool enable)
        {
            this.btProyectOk.Enabled = enable;
            this.btProyectOk.Visible = enable;
            this.btProyectCancel.Enabled = enable;
            this.btProyectCancel.Visible = enable;
        }


        /* Descripción:
         *  Limpia los campos de texto
         */
        private void ClearProjectsText()
        {
            this.tbNameProject.Text = "";
            this.tbDirectorProject.Text = "";
            this.rTextBoxDescriptionProject.Text = "";
            this.tbDateProject.Text = "";
        }


        /* Descripción:
         *  Habilita los campos de texto
         */
        private void ReadOnlyProyectsText(bool enable)
        {
            this.tbNameProject.ReadOnly = enable;
            this.tbDirectorProject.ReadOnly = enable;
            this.rTextBoxDescriptionProject.ReadOnly = enable;
            this.tbDateProject.ReadOnly = enable;
        }


        /* Descripción:
         *  Toma los datos de un proyecto y genera un objeto de la clase proyecto.
         */
        private void tsmiActionAddProject_Click()
        {
            try
            {
                // entramos en el modo edición
                this.editionModeOn = true;
                // inhabilitamos el menú vertical de proyectos
                this.mStripProjects.Enabled = false;
                // Estado actual de proyecto de normal pasa a AddProject
                this.stateProyect = StateOfOptionProyects.AddProyect;
                // Limpiamos los campos de texto
                ClearProjectsText();
                /* Habilitamos los campos de texto:
                 *  - Nombre de proyecto y descripción.
                 * Los campos de fecha y director de proyecto no se activan
                 */
                // ReadOnlyProyectsText(false);
                this.tbNameProject.ReadOnly = false;
                this.rTextBoxDescriptionProject.ReadOnly = false;

                // Habilitamos los botones
                EnabledOrDisabledButtonOkCancel(true);
            }
            catch (EndpointNotFoundException)
            {
                AuxNoConection();
            }
            catch (InvalidOperationException)
            {
                AuxNoConection();
            }
            catch (SoapException)
            {
                AuxNoConection();
            }
        }// end tsmiActionAddProject_Click


        /* Descripción:
         *  Se activa al hacer clic en le botón Aceptar de la opción proyectos
         */
        private void AddProyect()
        {
            try
            {
                SagtProject project = new SagtProject(this.tbNameProject.Text, this.tbDirectorProject.Text,
                    this.rTextBoxDescriptionProject.Text);
                DataSet dsProject = project.Proyect2DataSet();
                // Comprobar si existe un proyecto con el mismo nombre

                DataSet dsSameProject = sagtWS_Client.SelectSameProyects(dsProject, "");

                // Si no existe
                if (dsSameProject.Tables[0].Rows.Count == 0)
                {
                    // Insertar proyecto en la base de datos
                    // Inserción de datos en el servidor de prueba.
                    int pk = sagtWS_Client.Insert_Project(dsProject);


                    project.SetPK_Project(pk);
                    // actualizar los datos entre ellos el de la hora
                    // El nuevo proyecto será el proyecto activo
                    this.activeProject = project;


                    // Restauramos
                    LoadTextProyect(this.activeProject);
                    // Ponemos los textos a readOnly  
                    ReadOnlyProyectsText(true);
                    // Habilitamos el menú vertical de proyectos
                    this.mStripProjects.Enabled = true;
                    // Salimos del modo edición
                    this.editionModeOn = false;
                    // Inhabilitamos los botones
                    EnabledOrDisabledButtonOkCancel(false);

                    // mostrar mensage de que el proyecto ha sido creado
                    // MessageBox.Show(txtProyectCreated);
                    ShowMessageInfo(txtProyectCreated);
                }
                else
                {
                    // Si el proyecto existe
                    // Mostrar mensage de error
                    ShowMessageErrorOK(errorProyectExist);
                }
            }
            catch (SagtProjectException)
            {
                // Mensage de error nombre de proyecto no válido
                ShowMessageErrorOK(errorNoNameProject);
            }
            catch (ConnectDB_Exception er)
            {
                // Mensage de error al insertar en la base de datos
                ShowMessageErrorOK(errorNoInsertProyect + " " + er.Message);
            }
            catch (SoapException)
            {
                AuxNoConection();
            }
        }// end AddProyect


        /* Descripción:
         *  Carga los datos de un proyecto
         */
        private void LoadTextProyect(SagtProject project)
        {
            if (project != null)
            {
                this.tbNameProject.Text = project.GetNameProject();
                this.tbDirectorProject.Text = project.GetNameDirectorProject();
                this.rTextBoxDescriptionProject.Text = project.GetDescription();
                this.tbDirectorProject.Text = project.GetNameDirectorProject();
                this.tbDateProject.Text = project.GetDateCreation().ToString();
            }
            else
            {
                ClearProjectsText();
            }
        }// end LoadTextProyect


        /* Descripción:
         *  Se activa al hacer clic en le botón Cancelar de la opción proyectos
         */
        private void ExitsAddProject()
        {
            // Restauramos
            LoadTextProyect(this.activeProject);
            // Ponemos los textos a readOnly  
            ReadOnlyProyectsText(true);
            // Habilitamos el menú vertical de proyectos
            this.mStripProjects.Enabled = true;
            // Salimos del modo edición
            this.editionModeOn = false;
            // Inhabilitamos los botones
            EnabledOrDisabledButtonOkCancel(false);
            this.stateProyect = StateOfOptionProyects.Normal;
        }


        /* Descripción:
         *  Habilita los campos para su edición y lo actualiza
         */
        private void tsmiActionEditProject_Click()
        {
            try
            {
                if (this.activeProject != null)
                {
                    this.stateProyect = StateOfOptionProyects.Edition;
                    // Introducimos el modo edición
                    this.editionModeOn = true;
                    // Habilitamos los textBox;
                    this.rTextBoxDescriptionProject.Enabled = true;
                    this.rTextBoxDescriptionProject.ReadOnly = false;
                    this.tbNameProject.Enabled = true;
                    this.tbNameProject.ReadOnly = false;
                    // Habilitamos los botones de aceptar y cancelar
                    EnabledOrDisabledButtonOkCancel(true);
                }
                else
                {
                    // Lanzamos un mensaje de error: No hay proyecto activo
                    ShowMessageErrorOK(errorNoProyectActivate);
                }
            }
            catch (EndpointNotFoundException)
            {
                AuxNoConection();
            }
            catch (InvalidOperationException)
            {
                AuxNoConection();
            }
            catch (SoapException)
            {
                AuxNoConection();
            }
        }// end tsmiActionEditProject_Click


        /* Descripción:
         *  Se activa al pulsar en la opción buscar del menú vertical de proyectos
         */
        private void tsmiActionSearchProject_Click()
        {
            try
            {
                /* Si es administrador restringido solo tendrá acceso a sus proyectos
                 */
                SagtUser.UserAccess userRol = this.activeUser.GetAuthorizationToAccess();
                if (userRol.Equals(SagtUser.UserAccess.Ad_Restringido))
                {
                    int userID = this.activeUser.GetUserID();
                    // Buscamos los proyectos
                    DataTable dtProjects = this.sagtWS_Client.SelectProyectsForUsers(userID).Tables[0];
                    ShowFormSelectDataTable(dtProjects);
                }
                else if(userRol.Equals(SagtUser.UserAccess.Usuario))
                {
                    string nameUserMenpas = sagtWS_Client.ReturnNameUserMenPas(this.activeUser.GetUserID());
                    string group = sagtWS_Client.Obtener_grupo(nameUserMenpas);
                    // Buscamos los proyectos
                    string expression = "Grupo = '" + group + "'";
                    DataTable dtPersons = this.sagtWS_Client.Lista_PersonasAD_SAGT().Tables[0];
                    DataRow[] rows = dtPersons.Select(expression);
                    int n = rows.Length;
                    if (n > 0)
                    {
                        string menPasUser = (string)rows[0]["Usuario"];
                        int fk_administ = sagtWS_Client.Add_SagtUser(menPasUser);

                        // int pk_project = this.sagtWS_Client.ReturnPk_project(group);
                        // this.activeProject = this.sagtWS_Client.
                        DataTable dtProjects = sagtWS_Client.SelectProyectsForUsers(fk_administ).Tables[0];
                        int n_cols = dtProjects.Columns.Count;
                        for (int i = 1; i < n; i++)
                        {
                            menPasUser = (string)rows[i]["Usuario"];
                            fk_administ = sagtWS_Client.Add_SagtUser(menPasUser);
                            DataTable dt = sagtWS_Client.SelectProyectsForUsers(fk_administ).Tables[0];
                            int n_r = dt.Rows.Count;
                            for (int j = 0; j < n_r; j++)
                            {
                                DataRow r = dtProjects.NewRow();
                                for (int k = 0; k < n_cols; k++)
                                {
                                    r[k] = dt.Rows[j][k];
                                }
                                dtProjects.Rows.Add(r);
                            }
                        }
                        ShowFormSelectDataTable(dtProjects);
                    }
                    
                }
                else
                {
                    // Cambio el estado de proyectos de normal a busqueda
                    this.stateProyect = StateOfOptionProyects.Search;
                    // Bloqueo el menú principal
                    this.editionModeOn = true;
                    // Inhabilito el menú vertical
                    this.mStripProjects.Enabled = false;
                    // Borro los campos
                    ClearProjectsText();
                    // Permito que se escriba en ellos
                    ReadOnlyProyectsText(false);
                    // Muestro los botones
                    EnabledOrDisabledButtonOkCancel(true);
                }
            }
            catch (EndpointNotFoundException)
            {
                AuxNoConection();
            }
            catch (InvalidOperationException)
            {
                AuxNoConection();
            }
            catch (SoapException)
            {
                AuxNoConection();
            }
        }// end tsmiActionSearchProject_Click

        
        /* Descripción:
         *  Actualiza los campos de nombre de proyecto y descripción y actualiza la base de datos
         */
        private void EditionProject()
        {
            SagtProject project = new SagtProject(this.tbNameProject.Text, this.tbDirectorProject.Text,
                    this.rTextBoxDescriptionProject.Text);
            DataSet dsProject = project.Proyect2DataSet();
            // Comprobamos que no exista un proyecto con el mismo nombre
            DataSet dsSameProject = sagtWS_Client.SelectSameProyects(dsProject, "");
            // indicamos si se ha cambiado el nombre.
            bool changeNameProject = !this.activeProject.GetNameProject().ToLower().Equals(project.GetNameProject().ToLower());
            // Si existe
            if (changeNameProject && dsSameProject.Tables[0].Rows.Count != 0 )
            {
                // Lanzamos un mensage
                ShowMessageErrorOK(errorProyectExist);
            }
            else
            {
                // si no lo hay actualizamos
                this.activeProject.SetNameProject(this.tbNameProject.Text);
                this.activeProject.SetDescription(this.rTextBoxDescriptionProject.Text);
                // Actualizamos la base de datos
                DataSet ds = this.activeProject.Proyect2DataSet();
                sagtWS_Client.UpdateProject(ds);
                ExitsAddProject();
            }
        }// end EditionProject


        /* Descripción:
         *  Realiza una busqueda con los campos introducidos. Si encuentra uno o más registros lo muestra
         *  en el formulario FormSelectProyect. En otro caso mostrará un mensage de que no se ha encontrado
         *  ningúno y restaurará los valores del proyecto activo. Si selecionamos un proyecto en el 
         *  formulario se actualizará el proyecto activo.
         */
        private void SearchProject()
        {
            if (string.IsNullOrEmpty(this.tbNameProject.Text) && string.IsNullOrEmpty(this.rTextBoxDescriptionProject.Text))
            {
                // Si no existe ningún proyecto que cumpla las condiciones lanzamos un mensage
                QuestionSelectAllProyects();
            }
            else
            {
                // Tomar los datos de los registros y mandar el mensaje de busqueda que devolverá un dataTable
                DataSet dtProyect = this.sagtWS_Client.SelectLikeProyects(this.tbNameProject.Text, this.rTextBoxDescriptionProject.Text);
                // si no encuentra registros
                if (dtProyect.Tables[0].Rows.Count == 0)
                {
                    QuestionSelectAllProyects();
                }
                else
                {
                    // Si encuentra registros
                    ShowFormSelectDataTable(dtProyect.Tables[0]);
                }
            }

        }// end SearchProject


        /* Descripción:
         *  Se pregunta al usuario si quiere mostrar todos los registos. En caso afirmativo se muestran
         *  en un nuevo formulario en otro caso no hacemos nada.
         */
        private void QuestionSelectAllProyects()
        {
            DialogResult res = ShowMessageDialog(titleAdvice, txtNoProyectFound, MessageBoxIcon.Question);
            switch (res)
            {
                // Si en la ventana hemos seleccionadado cancelar entonces no hacemos nada
                case (DialogResult.Cancel):
                    break;
                // Si hemos seleccionado aceptar entonces tendremos nuevo proyecto activo
                case (DialogResult.OK):
                    switch (this.activeUser.GetAuthorizationToAccess())
                    {
                        case (SagtUser.UserAccess.Administrador):
                            // realizamos una nueva busqueda de todos los proyectos accesibles
                            DataTable dtProyect = this.sagtWS_Client.SelectAllProyects().Tables[0];
                            ShowFormSelectDataTable(dtProyect);
                            break;
                        case (SagtUser.UserAccess.Ad_Restringido):
                            // realizamos una nueva busqueda de todos los proyectos accesibles
                            dtProyect = this.sagtWS_Client.SelectProyectsForUsers(this.activeUser.GetUserID()).Tables[0];
                            ShowFormSelectDataTable(dtProyect);
                            break;
                        default:
                            // Mensaje de error no se corresponde con un perfil válido
                            ShowMessageErrorOK(errorNoValidateRol);
                            break;
                    }
                    break;
            }
        }


        /* Descripción:
         *  Muestra la ventana de selección con el dataTable que se pasa como parámetro.
         *  Actualiza el proyecto activo.
         */
        private void ShowFormSelectDataTable(DataTable dtProyect)
        {
            // Crear la ventana con el data table
            FormSelectFromDataTable formSelect = new FormSelectFromDataTable(this.LanguageActually(), dtProyect);
            DialogResult dres = formSelect.ShowDialog();

            switch (dres)
            {
                // Si en la ventana hemos seleccionado cancelar entonces no hacemos nada
                case (DialogResult.Cancel):
                    break;
                // Si hemos seleccionado aceptar entonces tendremos nuevo proyecto activo
                case (DialogResult.OK):
                    this.activeProject = formSelect.SelectProject();
                    int id_director = this.activeProject.GetId_Director();
                    if (id_director != 0)
                    {
                        string name_User_MenPas = this.sagtWS_Client.ReturnNameUserMenPas(this.activeProject.GetId_Director());
                        string name = this.sagtWS_Client.dameDatos(name_User_MenPas);
                        this.activeProject.SetNameDirectorProject(name);
                    }
                    ExitsAddProject();
                    break;
            }
        }


        /* Descripción:
         *  Pregunta al Administrador por el usuario que pueden diriguir un proyecto 
         */
        private void tsmiActionAddDirectorToProject_Click()
        {
            try
            {
                if (this.activeProject != null)
                {
                    DataTable dtAdminds = this.sagtWS_Client.Lista_PersonasAD_SAGT().Tables[0];
                    // Crear la ventana con el data table
                    FormSelectFromDataTable formSelect = new FormSelectFromDataTable(this.LanguageActually(), dtAdminds);
                    DialogResult dres = formSelect.ShowDialog();
                    switch (dres)
                    {
                        // Si en la ventana hemos seleccionadado cancelar entonces no hacemos nada
                        case (DialogResult.Cancel):
                            break;
                        // Si hemos seleccionado aceptar entonces tendremos nuevo proyecto activo
                        case (DialogResult.OK):
                            // actualizamos el identificador de administrador de proyecto
                            int table_index = formSelect.SelectDataTableIndex();
                            string name_User_id = dtAdminds.Rows[table_index]["Usuario"].ToString();
                            int idUser = sagtWS_Client.Add_SagtUser(name_User_id.ToLower());

                            string name_Adminst = dtAdminds.Rows[table_index]["Nombre"].ToString() + " " + dtAdminds.Rows[table_index]["Apellidos"].ToString();
                            this.activeProject.SetId_Director(idUser);
                            this.activeProject.SetNameDirectorProject(name_Adminst);
                            // Actualizamos el proyecto en la base de datos
                            DataSet ds = this.activeProject.Proyect2DataSet();
                            sagtWS_Client.UpdateProject(ds);
                            // Obtenemos el identificador
                            ExitsAddProject();
                            break;
                    }
                }
                else
                {
                    // No hay un proyecto activo
                    ShowMessageErrorOK(errorNoProyectActivate);
                }
            }
            catch (EndpointNotFoundException)
            {
                AuxNoConection();
            }
            catch (InvalidOperationException)
            {
                AuxNoConection();
            }
            catch (SoapException)
            {
                AuxNoConection();
            }
        }// end tsmiActionAddDirectorToProject_Click


        /* Descripción:
         *  De momento para realizar pruebas almacena los datos en la base de datos local.
         */
        private void tsmiActionDataSavedWebService_Click()
        {
            try
            {
                // Comprobamos que haya un proyecto activo
                if (activeProject != null)
                {
                    // Identificador de usuario
                    int id_user = 0;
                    // Obtenemos el identificador de proyecto
                    int id_project = activeProject.GetPK_Project(); // Identificador de proyecto
                    SaveFileSagtWedService(this.sagtElements, id_project, id_user);
                }
                else
                {
                    // No hay un proyecto activo y lanzamos un mensage
                    // Lanzamos un mensaje de error: No hay proyecto activo
                    ShowMessageErrorOK(errorNoProyectActivate);
                }
            }
            catch (EndpointNotFoundException)
            {
                AuxNoConection();
            }
            catch (InvalidOperationException)
            {
                AuxNoConection();
            }
            catch (SoapException)
            {
                AuxNoConection();
            }
        }// end tsmiActionDataSavedWebService_Click


        /* Descripción:
         *  De momento para realizar pruebas almacena los datos en la base de datos local.
         */
        private void tsmiActionAnalysisSavedWebService_Click()
        {
            try
            {
                // Comprobamos que haya un proyecto activo
                if (activeProject != null)
                {
                    // Identificador de usuario
                    int id_user = 0;
                    // Obtenemos el identificador de proyecto
                    int id_project = activeProject.GetPK_Project(); // Identificador de proyecto
                    SaveFileAnalysisWedService(this.anl_tAnalysis_G_study_opt, id_project, id_user);
                }
                else
                {
                    // No hay un proyecto activo y lanzamos un mensage
                    // Lanzamos un mensaje de error: No hay proyecto activo
                    ShowMessageErrorOK(errorNoProyectActivate);
                }
            }
            catch (EndpointNotFoundException)
            {
                AuxNoConection();
            }
            catch (InvalidOperationException)
            {
                AuxNoConection();
            }
        }// end tsmiActionAnalysisSavedWebService_Click


        /* Descripción:
         * Toma los datos de un fichero de análisis y lo almacena en la base de datos
         */
        private void SaveFileAnalysisWedService(Analysis_and_G_Study analysisGT, int id_project, int id_user)
        {
            FormNameFileAnalysis formNameFile = new FormNameFileAnalysis(this.LanguageActually());
            bool salir = false;
            do
            {
                DialogResult res = formNameFile.ShowDialog();
                switch (res)
                {
                    case (DialogResult.Cancel):
                        salir = true;
                        break;
                    case(DialogResult.OK):
                        // Comprobamos que es un nombre válido y guardamos
                        string nameFile = formNameFile.TextNameFile();

                        if (string.IsNullOrEmpty(nameFile))
                        {
                            // Error no se ha espeficado el nombre del archivo
                            ShowMessageErrorOK(errorNoFileName);
                        }
                        else
                        {
                            // Comprobamos que el nombre sea valido
                            if (!IsNameFileValidatedForProjectWedService(nameFile, id_project, TypeFile.anls))
                            {
                                // Error ya hay un fichero con ese nombre.
                                ShowMessageErrorOK(errorNameFileExist);
                            }
                            else
                            {
                                // Guardamos los datatos
                                SaveAnalysisFileInWedService(nameFile, analysisGT, id_project, id_user);
                                salir = true;
                            }
                        }
                        break;
                }
            } while (!salir);

        }// end SaveFileAnalysisWedService


        /* Descripción:
         *  Muestra un mensaje de confirmación y guarda el archivo con el nombre que se pasa como 
         *  parámetro en la base de datos.
         */
        private void SaveAnalysisFileInWedService(string nameFile, Analysis_and_G_Study analysisFile, int id_project, int id_user)
        {
            // Pedimos confirmación
            DialogResult result = ShowMessageDialog(titleAdvice, txtConfirmSaveWedService, MessageBoxIcon.Question);

            switch (result)
            {
                case (DialogResult.OK):
                    DataSet[] array_ds_analysis = analysisFile.Analysis_and_G_Study2ListDataSets();
                    sagtWS_Client.Insert_AnalysisFile(array_ds_analysis, nameFile, id_project, id_user);
                    // Mostramos un mensage indicando que los datos se han guardado
                    ShowMessageInfo(txtTheDataIsSaved);
                    break;
            }
        }


        /* Descripción:
         *  Operación auxiliar. Seleccionamos los datos y los guardamos en un fichero Sagt.
         * Parámetros:
         *      SagtFile sagtElements: elemento que se va a almacenar.
         *      int id_project: Identificador de proyecto.
         *      int id_user: Identificador de usuario.
         */
        private void SaveFileSagtWedService(SagtFile sagtElements, int id_project, int id_user)
        {
            bool bData = !(sagtElements.GetMultiFacetsObs() == null); // pestaña de tabla de frecuencias
            bool bMean = !(sagtElements.GetListMeans() == null); // pestaña de tablas de medias
            bool bSsq = !(sagtElements.GetAnalysis_and_G_Study() == null); // pestaña de análisis de varianza

            FormSelectSaves formSelectSaves = new FormSelectSaves(this.LanguageActually(), bData, bMean, bSsq, true);

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
                            // Comprobamos que hay un nombre de archivo y que este es valido
                            // Comprobamos que hay algún nombre
                            string nameFileSagt = formSelectSaves.NameFile();
                            if (string.IsNullOrEmpty(nameFileSagt))
                            {
                                // Error no se ha espeficado el nombre del archivo
                                ShowMessageErrorOK(errorNoFileName);
                            }
                            else
                            {
                                // Comprobamos que el nombre sea valido
                                if (!IsNameFileValidatedForProjectWedService(nameFileSagt, id_project, TypeFile.sagt))
                                {
                                    // Error ya hay un fichero con ese nombre.
                                    ShowMessageErrorOK(errorNameFileExist);
                                }
                                else
                                {
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

                                    SaveSagtFileInWedService(nameFileSagt, sagtElementsSave, id_project, id_user);
                                    // Mostramos un mensage indicando que los datos se han guardado
                                    ShowMessageInfo(txtTheDataIsSaved);

                                    // btGenerateTableObsDisables(); Esta linea solo es necesaría si editamos la tabla de frecuencias
                                    salir = true;
                                }
                            }
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
        }// end SaveFileSagtWedService


        /* Descripción:
         *  Muestra un mensaje de confirmación y guarda el archivo con el nombre que se pasa como 
         *  parámetro en la base de datos.
         */
        private void SaveSagtFileInWedService(string nameFile, SagtFile sagtFile, int id_project, int id_user)
        {
            // Pedimos confirmación
            DialogResult result = ShowMessageDialog(titleAdvice, txtConfirmSaveWedService, MessageBoxIcon.Question);

            switch (result)
            {
                case (DialogResult.OK):
                    // actualizamos los datos por si se ha modificado la tabla
                    ReadColumnOmit(sagtFile, this.dataGridViewExFacets);

                    DataSet ds_mfo = null;
                    if (sagtFile.GetMultiFacetsObs() != null)
                    {
                        ds_mfo = sagtFile.GetMultiFacetsObs().MultiFacetObs2DataSet();
                    }
                    DataSet[] array_ds_Means = null;
                    if (sagtFile.GetListMeans() != null)
                    {
                        array_ds_Means = sagtFile.GetListMeans().ListMeans2DataSet();
                    }
                    DataSet[] array_analysis = null;
                    if (sagtFile.GetAnalysis_and_G_Study() != null)
                    {
                        array_analysis = sagtFile.GetAnalysis_and_G_Study().Analysis_and_G_Study2ListDataSets();
                    }
                    //
                    // Analysis_and_G_Study aaa = ProjectSSQ.Analysis_and_G_Study.ListDataSet2Analysis_and_G_Study(array_analysis);

                    sagtWS_Client.Insert_SagtFile(ds_mfo, array_ds_Means, array_analysis, nameFile, id_project, id_user);
                    break;
            }
        }


        /* Descripción:
         *  Comprueba que el nombre del archivo para ese proyecto es válido. Devuelve true si no hay
         *  ningún archivo con su nombre, false en otro caso.
         */
        private bool IsNameFileValidatedForProjectWedService(string nameFile, int project, TypeFile type)
        {
            DataTable dtNameFiles = this.sagtWS_Client.SelectNameFilesProject(nameFile, project, type.ToString()).Tables[0];
            return (dtNameFiles.Rows.Count == 0);
        }



        #region Selección de un fichero y carga de este en la aplicación
        /* Descripción:
         *  Se activa al pulsar en la opción "Abrir desde servicio web" del menú vertical de Datos,
         *  Medias, Suma de cuadrados
         */
        private void tsmiActionOpenWebService_Click(TypeFile typeFile)
        {
            try
            {
                // enlazamos con la base de datos
                // InterfaceConnectDB BD = new ConnectDB();
                // Comprobamos que haya un proyecto activo
                if (activeProject != null)
                {
                    int fk_project = activeProject.GetPK_Project();

                    int fk_user = 0;// aun no se como controlar al usuario

                    DataTable dt = sagtWS_Client.SelectFiles(fk_project, fk_user, typeFile.ToString()).Tables[0];

                    // Crear la ventana con el data table
                    FormSelectFromDataTable formSelect = new FormSelectFromDataTable(this.LanguageActually(), dt);
                    DialogResult dres = formSelect.ShowDialog();

                    switch (dres)
                    {
                        // Si en la ventana hemos seleccionadado cancelar entonces no hacemos nada
                        case (DialogResult.Cancel):
                            break;
                        // Si hemos seleccionado aceptar entonces tendremos nuevo proyecto activo
                        case (DialogResult.OK):

                                this.checkBoxHideNulls.Enabled = false;
                                this.checkBoxHideNulls.Checked = false;
                                this.checkBoxHideNulls.Enabled = true;
                            // Cargamos el fichero seleccionado.
                            int indx = formSelect.SelectDataTableIndex();
                            int pk_file = (int)dt.Rows[indx]["pk_file"];
                            string fileNameData = dt.Rows[indx]["name_file"].ToString();
                            if (TypeFile.sagt.Equals(typeFile))
                            {
                                MenPasWS.SagtFileW sagtFileW = sagtWS_Client.ReturnSagtFile(pk_file);

                                MultiFacetsObs mfo = null;
                                if (sagtFileW.dsMultiFacetObs != null)
                                {
                                    mfo = MultiFacetData.MultiFacetsObs.DataSet2MultiFacetObs(sagtFileW.dsMultiFacetObs);
                                }
                                ListMeans listMeans = null;
                                if (sagtFileW.listDataSetMeans != null)
                                {
                                    listMeans = ProjectMeans.ListMeans.ListDataSet2ListMeans(sagtFileW.listDataSetMeans);
                                }

                                Analysis_and_G_Study analysis = null;
                                if (sagtFileW.ldsAnalysis != null)
                                {
                                    analysis = ProjectSSQ.Analysis_and_G_Study.ListDataSet2Analysis_and_G_Study(sagtFileW.ldsAnalysis);
                                }
                                Sagt.SagtFile sagtFile = new SagtFile(mfo, listMeans, analysis);
                                // Realizamos un load
                                this.sagtElements = sagtFile;
                                loadSagtElements(fileNameData, this.sagtElements);
                                ShowMessageInfo(txtTheDatasIsLoaded);
                            }
                            else if (TypeFile.anls.Equals(typeFile))
                            {
                                DataSet[] array_ds_analysis = sagtWS_Client.Return_AnalysisFile(pk_file);
                                this.anl_tAnalysis_G_study_opt = ProjectSSQ.Analysis_and_G_Study.ListDataSet2Analysis_and_G_Study(array_ds_analysis);

                                // Cargamos el fichero de análisis
                                LoadAllDataGridWithDataAnalysis(this.anl_tAnalysis_G_study_opt, fileNameData);
                            }
                            break;
                    }
                }
                else
                {
                    // No hay un proyecto activo y lanzamos un mensage
                    // Lanzamos un mensaje de error: No hay proyecto activo
                    ShowMessageErrorOK(errorNoProyectActivate);
                }
            }
            catch (EndpointNotFoundException)
            {
                AuxNoConection();
            }
            catch (InvalidOperationException)
            {
                AuxNoConection();
            }
        }// end tsmiActionOpenWebService_Click

        #endregion Selección de un fichero y carga de este en la aplicación



        #region Cambio de idioma de los elementos del tabPageProject
        /*
         * Descripción:
         *  Traduce los elementos del TabPageProject.
         * Parámetros:
         *  TransLibrary.Language lang: idioma al que vamos a traducir los elementos.
         *  string nameFileTrans: Nombre del fichero que contiene las traducciones.
         */
        private void TranslationProjectsElements(TransLibrary.Language lang, string nameFileTrans)
        {
            TransLibrary.ReadFileTrans dic = new TransLibrary.ReadFileTrans(nameFileTrans);
            string name = "";

            try
            {
                // Traducimos el groupBox de proyecto activo
                name = this.gBoxActiveProject.Name.ToString();
                this.gBoxActiveProject.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta de nombre de proyecto
                name = this.lbNameProject.Name.ToString();
                this.lbNameProject.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta de fecha de creación del proyecto
                name = this.lbDateProject.Name.ToString();
                this.lbDateProject.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta de director de proyecto
                name = this.lbDirectorProject.Name.ToString();
                this.lbDirectorProject.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta de descripción de proyecto
                name = this.lbDescriptionProject.Name.ToString();
                this.lbDescriptionProject.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos los botones de Aceptar y Cancelar
                name = this.btProyectOk.Name.ToString();
                this.btProyectOk.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.btProyectCancel.Name.ToString();
                this.btProyectCancel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                // Muestra un mensage de error al traducir
                ShowMessageErrorOK(lEx.Message + " " + errorMessageTraslation + " " + name);
            }

        } // private void TraslationProjectsElements

        #endregion Cambio de idioma de los elementos del tabPageProject


    } // end public partial class FormPrincipal : Form
} // end namespace GUI_TG
