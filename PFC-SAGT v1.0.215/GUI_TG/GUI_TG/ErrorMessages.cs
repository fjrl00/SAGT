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
 *  Contiene los string con los mensages de error
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TransLibrary;
using MultiFacetData;
using MultiFacetPY;
//
using System.Diagnostics;
using System.IO; // para poder usar File.Exist
using ConfigCFG;

namespace GUI_GT
{
    public partial class FormPrincipal : Form
    {

        private string btOkWindowsMessage = "Aceptar"; // Texto botón "Aceptar" de la ventana de dialogo
        private string btCancelWindowsMessage = "Cancelar"; // Texto botón  "Cancelar" de la ventana de dialogo
        private string titleMessageError1 = "Error"; // Titulo de la ventana de error
        private string titleAdvice = "Aviso"; // título de la ventana de aviso
        private string titleSaved = "Guardado";
        private string titleSave = "Guardar";

        /***************************************************************************************************
         * MENSAGE DE ERROR
         **************************************************************************************************/
        private string errorMinNumFacet = "El número de facetas ha de ser como mínimo 2.";
        private string errorMessageTraslation = "Se produjo un error al traducir:";
        private string errorNoTableObs = "No se ha creado la tabla de variables observadas.";
        private string errorVarTableObs = "Error: uno o más varibles observadas no contiene datos numéricos.";
        private string errorNoNumFacet = "No ha especificado el número de facetas.";
        private string errorOverUniverse = "El tamaño del nivel es mayor que el universo.";
        private string errorLevelFormat = "Los niveles no han sido especificados correctamente.";
        private string errorOverMaxRows = "Se ha sobrepasado el tamaño máximo de la tabla";
        private string errorInvalidRange = "Intervalo no valido";
        private string errorNoFileSelected = "No se ha seleccionado un fichero.";
        private string errorFormatFile = "El fichero no esta en el formato correcto.";
        private string errorM_DesignNoValidate = "Debe haber al menos una fuente de variación a cada lado.";
        private string errorNoValidateSSqEdit = "Existen valores para suma de cuadrados no validas.";
        private string errorNoFacetSelected = "No ha seleccionado ninguna faceta.";
        private string errorNoListMeans = "No hay tablas de medias";
        private string errorNoTableMeansDispose = "No se ha determinado el tipo de tabla de media";
        private string errorNoSSQ = "No hay tabla de suma de cuadrados";
        private string errorNoNesting = "No hay facetas anidadas";
        private string errorNoOperation = "Operación no válida";
        private string errorNoSelectFacetNesting = "No se ha seleccionado la faceta anidante";
        private string errorNoSelectFacetNested = "No se ha seleccionado la faceta anidada";
        private string errorNoOmitTwoFacet = "En número mínimo de facetas sin omitir debe ser dos";
        private string errorZeroLevelToReduce = "Imposible reducir: 0 niveles";
        private string errorReadingFileScore = "Error al leer el fichero de puntuaciones";
        private string errorReadingFile = "Error al leer el archivo";
        private string errorReadingIt = "Se produjo un error al leer";
        private string errorSourceSsqEduG = "Error al leer la fuente";
        private string errorFileInUse = "El archivo esta siendo usado por otro programa";
        private string errorNoUserNameOrPassword = "Nombre de usuario o contraseña no validos";
        private string errorNoFileName = "Debe indicar el nombre del archivo";
        private string errorNameFileExist = "Ya existe un archivo con el mismo nombre";
        private string errorNoExistWorkspace = "No se ha encontrado el espacio de trabajo. Se usará el espacio de trabajo por defecto.";
        private string errorValueNullOrEmpty = "Hay campos vacios";
        private string errorDuplicateNameFacet = "Nombre de faceta duplicado"; 

        // Errores de Proyecto
        private string errorNoNameProject = "Nombre de proyecto no válido";
        private string errorProyectExist = "Ya existe un proyecto con el mismo nombre";
        private string errorNoProyectActivate = "No hay un proyecto activo";
        private string errorNoInsertProyect = "Error al insertar proyecto";
        private string txtNoProyectFound = "0 Proyectos encontrados. ¿Desea ver todos los proyectos?";
        // Errores de Conexión
        private string errorNoCommunication = "No se ha establecido comunicación";
        private string errorNoValidateRol = "No se corresponde con un perfil válido";


        /****************************************************************************************************
         * MENSAGES DE VENTANA DE DIALOGO
         ****************************************************************************************************/
        private string txtConfirmBuildNesting = "No podrá seguir editando las facetas \nhasta que el proceso finalice ¿Desea continuar?";
        private string txtAdvise = "No está conectado al servidor";
        private string txtSaveDataObserved = "La tabla de frecuencias se ha guardado";
        private string txtNoSaveScores = "Las puntuaciones no se han guardado";
        private string txtSaveScores = "Las puntuaciones se han guardado";
        private string txtNoSaveSumOfSquares = "Las sumas de cuadrados no se han guardado";
        private string txtSaveSumOfSquares = "Las sumas de cuadrados se han guardado";
        private string txtInfoImportScores = "Se han introducido [n] valores";
        private string txtTheDataIsSaved = "Los datos se han guardado";
        private string txtConnect = "Esta conectado";
        private string txtConfirmCloseConnect = "Cerrará la conexión. ¿Desea continuar?";
        private string txtProyectCreated = "Proyecto creado";
        private string txtConfirmSaveWedService = "Esta operación puede tardar varios minutos. ¿Desea continuar?";
        private string txtMessageNoSelected = "No hay seleccionado ningún elemento"; // Mensage de que no ha seleccionado ningún elemento del checkedListBox.
        private string txtConfirmClearMeans = "Se perderán las medias si no las ha guardado. ¿Desea continuar?";
        private string txtTheDatasIsLoaded = "Los datos han sido cargados";


        /****************************************************************************************************
         * FILTROS
         ****************************************************************************************************/
        private string filterDatas = "Fichero de puntuaciones";
        private string filterSsqExportEduG = "Fichero de sumas de cuadrados EduG";

         /*
         * Descripción:
         *  Traduce todos los textos de la ventana al idioma que se encuentre activo
         */
        private void TraslationErrorMessages(TransLibrary.Language lang)
        {
            // el diccionario que contiene los mensages de error es dicError
            string name = "";
            try
            {
                name = "btOk"; // Usamos esta clave ya que coincide con los valores
                btOkWindowsMessage = this.dicMessage.labelTraslation("btOk").GetTranslation(lang).ToString();
                name = "btCancel";
                btCancelWindowsMessage = this.dicMessage.labelTraslation("btCancel").GetTranslation(lang).ToString();
                name = "titleMessageError1";
                titleMessageError1 = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "titleAdvice";
                titleAdvice = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "titleSave";
                titleSave = dicError.labelTraslation(name).GetTranslation(lang).ToString();

                name = "errorMinNumFacet";
                errorMinNumFacet = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorMessageTraslation";
                errorMessageTraslation = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNoTableObs";
                errorNoTableObs = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNoNumFacet";
                errorNoNumFacet = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorOverUniverse";
                errorOverUniverse = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorLevelFormat";
                errorLevelFormat = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorOverMaxRows";
                errorOverMaxRows = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorInvalidRange";
                errorInvalidRange = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNoFileSelected";
                errorNoFileSelected = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorFormatFile";
                errorFormatFile = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorM_DesignNoValidate";
                errorM_DesignNoValidate = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNoValidateSSqEdit";
                errorNoValidateSSqEdit = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNoFacetSelected";
                errorNoFacetSelected = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNoListMeans";
                errorNoListMeans = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNoTableMeansDispose";
                errorNoTableMeansDispose = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNoSSQ";
                errorNoSSQ = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNoNesting";
                errorNoNesting = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNoOperation";
                errorNoOperation = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNoSelectFacetNesting";
                errorNoSelectFacetNesting = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNoSelectFacetNested";
                errorNoSelectFacetNested = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNoOmitTwoFacet";
                errorNoOmitTwoFacet = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorZeroLevelToReduce";
                errorZeroLevelToReduce = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorReadingFileScore";
                errorReadingFileScore = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorReadingFile";
                errorReadingFile = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorReadingIt";
                errorReadingIt = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorSourceSsqEduG";
                errorSourceSsqEduG = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorFileInUse";
                errorFileInUse = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNoUserNameOrPassword";
                errorNoUserNameOrPassword = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNoFileName";
                errorNoFileName = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNameFileExist";
                errorNameFileExist = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNoExistWorkspace";
                errorNoExistWorkspace = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorValueNullOrEmpty";
                errorValueNullOrEmpty = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorDuplicateNameFacet";
                errorDuplicateNameFacet = dicError.labelTraslation(name).GetTranslation(lang).ToString();

                // Errores de Proyecto
                name = "errorNoNameProject";
                errorNoNameProject = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorProyectExist";
                errorProyectExist = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNoProyectActivate";
                errorNoProyectActivate = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "errorNoInsertProyect";
                errorNoInsertProyect = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "txtNoProyectFound";
                txtNoProyectFound = dicError.labelTraslation(name).GetTranslation(lang).ToString();

                // Errores con el servicio web
                name = "errorNoCommunication";
                errorNoCommunication = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                // Error con el rol
                name = "errorNoValidateRol";
                errorNoValidateRol = dicError.labelTraslation(name).GetTranslation(lang).ToString();


                /* Traducimos el texto de la ventana de dialogo cuando no hay nigún elemento de 
                 * checkListBox seleccionado. */
                name = "txtMessageNoSelected";
                this.txtMessageNoSelected = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                // traducimos los textos de la ventana de confirmación para el caso de que exista medias creadas previamente
                name = "txtConfirmClearMeans";
                this.txtConfirmClearMeans = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "txtConfirmBuildNesting";
                txtConfirmBuildNesting = dicError.labelTraslation(name).GetTranslation(lang).ToString();

                name = "txtAdvise";
                txtAdvise = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "txtSaveDataObserved";
                txtSaveDataObserved = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "txtNoSaveScores";
                txtNoSaveScores = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "txtSaveScores";
                txtSaveScores = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "txtNoSaveSumOfSquares";
                txtNoSaveSumOfSquares = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "txtSaveSumOfSquares";
                txtSaveSumOfSquares = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "txtInfoImportScores";
                txtInfoImportScores = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "txtTheDataIsSaved";
                txtTheDataIsSaved = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "txtConnect";
                txtConnect = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "txtConfirmCloseConnect";
                txtConfirmCloseConnect = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "txtProyectCreated";
                txtProyectCreated = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "txtConfirmSaveWedService";
                txtConfirmSaveWedService = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "txtTheDatasIsLoaded";
                txtTheDatasIsLoaded = dicError.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos filtros
                name = "filterDatas";
                filterDatas = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                name = "filterSsqExportEduG";
                filterSsqExportEduG = dicError.labelTraslation(name).GetTranslation(lang).ToString();
                
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                // MessageBox.Show(lEx.Message + " " + errorMessageTraslation + " " + name);
                ShowMessageErrorOK(lEx.Message + " " + errorMessageTraslation + " " + name);
            }
        }


        /* Descripción:
         *  Muestra un mensage de información sin titulo y sin icono
         */
        public void ShowMessageInfo(string msg)
        {
            MsgBoxUtil.HackMessageBox(btOkWindowsMessage);
            MessageBox.Show(msg);
        }

        /* Descripción:
         *  Muestra un mensage de información con titulo pero sin icono
         */
        public void ShowMessageInfo(string msg, string title)
        {
            MsgBoxUtil.HackMessageBox(btOkWindowsMessage);
            MessageBox.Show(msg, title);
        }


        /* Descripción:
         *  Muestra una ventana de message de error con los textos traducidos.
         * Parámetros:
         *      string msg: Texto que mostraremos en la ventana de error.
         */
        public void ShowMessageErrorOK(string msg)
        {
            MsgBoxUtil.HackMessageBox(btOkWindowsMessage);
            MessageBox.Show(msg, this.titleMessageError1, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        /* Descripción:
         *  Muestra una ventana de message de error con los textos traducidos.
         * Parámetros:
         *      string msg: Texto que mostraremos en la ventana de error.
         *      MessageBoxIcon : Icono que se mostrará
         */
        public void ShowMessageErrorOK(string msg,string titleMsg, MessageBoxIcon iconMessage)
        {
            MsgBoxUtil.HackMessageBox(btOkWindowsMessage);
            MessageBox.Show(msg, titleMsg, MessageBoxButtons.OK, iconMessage);
        }


        /* Descripción:
         *  Muestra la ventana de dialogo con los botones traducidos. Devuelve el resultado de dicho
         *  dialogo.
         * Parámetros:
         *      string titleConfirm: Título de la ventana de dialogo.
         *      string txDialog: Texto que se muestra en la ventana de dialogo.
         *      MessageBoxIcon icon: icono que se mostrará en la ventana.
         */
        public DialogResult ShowMessageDialog(string titleConfirm, string txtDialog, MessageBoxIcon icon)
        {
            MsgBoxUtil.HackMessageBox(btOkWindowsMessage, btCancelWindowsMessage);
            DialogResult res = MessageBox.Show(txtDialog, titleConfirm, MessageBoxButtons.OKCancel,
                icon);
            return res;
        }


        /* Descripción:
         *  Muestra la ventana de dialogo con los botones traducidos. Devuelve el resultado de dicho
         *  dialogo. Mostrará el icono de una excalmación.
         * Parámetros:
         *      string titleConfirm: Título de la ventana de dialogo.
         *      string txDialog: Texto que se muestra en la ventana de dialogo.
         */
        public DialogResult ShowMessageDialog(string titleConfirm, string txtDialog)
        {
            return ShowMessageDialog(titleConfirm, txtDialog, MessageBoxIcon.Exclamation);
        }

    } // end public partial class FormPrincipal : Form
} // end namespace GUI_TG