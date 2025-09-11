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

namespace GUI_GT
{
    public partial class FormSSQImport : Form
    {
        /*=================================================================================
         * Constantes
         *=================================================================================*/
        // nombre del archivo que contiene las traducciones
        const string STRING_DATA_IMPORT = "formSSQ_Import.txt"; // Dialogos y mensages
        const string LANG_PATH = "\\lang\\";

        /*=================================================================================
         * Variables
         *=================================================================================*/

        // Directorio de trabajo por defecto
        string sagt_initial_directory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\SAGT\\Workspace";

        string[] typesFiles = {"Seleccione un fichero" ,
                                  "Suma de cuadrados, GT E 2.0 (Pierre Ysewijn, 1996)", 
                                  "Resultado de suma de cuadrados, GT E 2.0 (Pierre Ysewijn, 1996)",
                                  "Informe de análisis, EduG 6.0 - e, - f",
                                  "Análisis exportado por SAGT a Excel"};

        string[] fileDescriptions = {"",
                            "Fichero de suma de cuadrados (.ssq) generado por el software GT 2.0 E.",
                            "Fichero de análisis de suma de cuadrados (.rsa) generado por el software GT 2.0 E.",
                            "EduG 6.0 genera informes de análisis de suma de cuadrados que pueden ser exportados en ficheros .txt y .rtf. Para que las tablas puedan ser recuperadas de un txt debe respetarse que el campo \"Facet\" no tenga más de 25 caracteres. Para prevenir errores dichos ficheros no deben ser editados y modificados por los usuarios. Si un fichero contiene más de un informe el usuario podrá elegir el informe de entre los presentes.",
                            "Importa los datos de un fichero Excel siempre que este tenga el mismo formato que los ficheros exportados por esta misma aplicación."};

        string[] fileFilter = {"Todos los archivos|*.*",
                              "GT E 2.0 (*.ssq)|*.ssq|Todos los archivos|*.*",
                              "GT E 2.0 (*.rsa)|*.rsa|Todos los archivos|*.*",
                              "EduG 6.0 (*.txt,*.rtf)|*.txt;*.rtf|Todos los archivos|*.*",
                              "Excel (*.xls)|*.xls|Todos los archivos|*.*"};

        string pathImportFile;

        TransLibrary.Language lang;


        /**************************************************************************************************
         * CONSTRUCTORES
         **************************************************************************************************/

        public FormSSQImport()
        {
            InitializeComponent();
        }

        public FormSSQImport(TransLibrary.Language lang)
        {
            InitializeComponent();
            this.lang = lang;
            traslationElementsSSQ_Import();
            initComboBoxTypesFiles();
            this.cBoxTypesFiles.SelectedIndex = 0;
            btBrowse.Enabled = false;
        }


        /* Descripción:
         *  Carga la imagen de cabecera
         */
        private void LoadImagenHeader()
        {
            this.pictBoxNormal.Visible = true;
            this.pictBoxSSQ.Visible = false;
            this.pictBoxRSA.Visible = false;
            this.pictBoxEduG.Visible = false;
            this.pictBoxExcel.Visible = false;
        }


        /* Descripción:
         *  Oculta las imagenes de cabecera.
         */
        private void HideImagenHeader()
        {
            this.pictBoxNormal.Visible = false;
            this.pictBoxSSQ.Visible = false;
            this.pictBoxRSA.Visible = false;
            this.pictBoxEduG.Visible = false;
            this.pictBoxExcel.Visible = false;
        }


        /* Descrpición:
         *  Se ejecuta al seleccionar en el checkBox
         */
        private void SelectHeaderImagen()
        {
            int n = this.cBoxTypesFiles.SelectedIndex;
            tbComment.Text = this.fileDescriptions[n];
            HideImagenHeader();
            switch (n)
            {
                case (0):
                    this.pictBoxNormal.Visible = true;
                    break;
                case (1):
                    this.pictBoxSSQ.Visible = true;
                    break;
                case (2):
                    this.pictBoxRSA.Visible = true;
                    break;
                case (3):
                    this.pictBoxEduG.Visible = true;
                    break;
                case (4):
                    this.pictBoxExcel.Visible = true;
                    break;
            }
        }


        /* Descripción:
         *  Inicializa el comboBox con los string de typesFiles.
         */
        public void initComboBoxTypesFiles()
        {
            int l = this.typesFiles.Length;
            for (int i = 0; i < l; i++)
            {
                this.cBoxTypesFiles.Items.Add(this.typesFiles[i]);
            }
        }


        /* Descripción:
         *  Habilita o deshabilita el boton examinar en función del indice del comboBox. Si esta seleccionado el
         *  primero el boton explorar estará deshabilitado en otro caso estará habilitado.
         */
        private void Selection_cBoxtypeFile(object sender, EventArgs e)
        {
            if (this.cBoxTypesFiles.SelectedIndex > 0)
            {
                btBrowse.Enabled = true;
            }
            else
            {
                btBrowse.Enabled = false;
            }
            SelectHeaderImagen();
        }


        /* Descripción:
         *  Seleccionamos el fichero que queremos importar.
         */
        private void btBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            if (Directory.Exists(sagt_initial_directory))
            {
                openDialog.InitialDirectory = sagt_initial_directory;
            }

            openDialog.Filter = fileFilter[this.cBoxTypesFiles.SelectedIndex];

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                // almacenamos el path del fichero                     
                pathImportFile = openDialog.FileName;
                // Extraemos el nombre del fichero del path
                string fileExt = this.fileExtension(pathImportFile).ToLower(); // Pasamos a minúsculas la extensión

                this.tbNameFile.Text = this.extractFileNamePath(openDialog.FileName);
            }
        }


        /* Descrición:
         *  Importamos los datos del fichero seleccionado. En caso de que no haya ningún fichero
         *  seleccionado lanzamos una excepción.
         */
        private void btOk_Click(object sender, EventArgs e)
        {
            /*
            if (String.IsNullOrEmpty(this.tbNameFile.Text))
            {
                // lanzamos un mensaje de error: no hay fichero seleccionado
                formPrincipal.ShowMessageErrorOK(this.txtMessageNoFileSelected);
            }
            else
            {
                //cerramos la ventana y abrimos el fichero
                this.Close();
                this.formPrincipal.importSSqFile(this.pathImportFile);
            }
             */
        }


        /* Descripción:
         *  Cierra la ventana y cancela la operación.
         */
        private void btCancel_Click(object sender, EventArgs e)
        {
            // this.Close();
        }


        /* Descripción:
         *  Devuelve le nombre del fichero.
         */
        public string fileName()
        {
            return tbNameFile.Text;
        }


        /* Descripción:
         *  Devuelve el path de el fichero que se quiere importar.
         */
        public string pathFile()
        {
            return this.pathImportFile;
        }


        #region Operaciones auxiliares sobre el path
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
        #endregion Operaciones auxiliares sobre el path


        #region Traducción de la venatana FormSSQImport
        /*
         * Descripción:
         *  Cambia de idioma los elementos de la ventana.
         * Parámetros:
         *      TransLibrary.Language lang: Idioma al que vamos a traducir los elementos de la ventana.
         */
        private void traslationElementsSSQ_Import()
        {
            
            TransLibrary.ReadFileTrans dic = new TransLibrary.ReadFileTrans(Application.StartupPath + LANG_PATH + STRING_DATA_IMPORT);
            string name = "";
            try
            {
                // Traducimos los Textos de la ventana
                //====================================

                // Traducimos el título de la ventana
                name = this.Name.ToString();
                this.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el boton aceptar
                name = this.btOk.Name.ToString();
                this.btOk.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el boton cancelar
                name = this.btCancel.Name.ToString();
                this.btCancel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el boton examinar
                name = this.btBrowse.Name.ToString();
                this.btBrowse.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta sobre el comboBox
                name = this.lbSelectTypeFile.Name.ToString();
                this.lbSelectTypeFile.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos el arrasy para los textos del comboBox
                name = "typesFiles0";
                this.typesFiles[0] = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "typesFiles1";
                this.typesFiles[1] = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "typesFiles2";
                this.typesFiles[2] = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "typesFiles3";
                this.typesFiles[3] = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "typesFiles4";
                this.typesFiles[4] = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos la descripción de los ficheros
                name = "fileDescriptions1";
                this.fileDescriptions[1] = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "fileDescriptions2";
                this.fileDescriptions[2] = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "fileDescriptions3";
                this.fileDescriptions[3] = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "fileDescriptions4";
                this.fileDescriptions[4] = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // traducimos los filtros
                name = "fileFilter0";
                this.fileFilter[0] = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "fileFilter1";
                this.fileFilter[1] = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "fileFilter2";
                this.fileFilter[2] = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "fileFilter3";
                this.fileFilter[3] = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "fileFilter4";
                this.fileFilter[4] = dic.labelTraslation(name).GetTranslation(lang).ToString();

            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " Se produjo un error al traducir " + name);
            }
        }
        #endregion Traducción de la venatana FormSSQImport

    } // end public partial class FormSSQImport : Form
}// end namespace GUI_TG
