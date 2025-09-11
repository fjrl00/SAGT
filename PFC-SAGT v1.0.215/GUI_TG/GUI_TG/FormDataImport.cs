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
 * Fecha de revisión: 23/Mar/2012
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GUI_GT
{
    public partial class FormDataImport : Form
    {
        // Variables
        // nombre del archivo que contiene las traducciones
        const string STRING_DATA_IMPORT = "formDataImport.txt"; // Dialogos y mensages
        const string LANG_PATH = "\\lang\\";
        
        string sagt_initial_directory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\SAGT\\Workspace";

        string[] typesFiles = {"Seleccione un fichero" ,
                                  "Datos observados, GT E 2.0 (Pierre Ysewijn, 1996)",
                                  "Resultado de medias, GT E 2.0 (Pierre Ysewijn, 1996)",
                                  "Tabla de frecuencias exportada por SAGT a Excel",
                                  "Ficheros .csv"};

        string[] fileDescriptions = {"",
                            "Tabla de frecuencia extraída de un fichero de observaciones (.obs) generado con el software GT 2.0 E. Este archivo contiene los datos de las facetas además del nombre del fichero de datos (.dat) que contiene las frecuencias. El fichero de observaciones y el de datos debe encontrarse en el mismo directorio para que pueda ser leído.",
                            "Obtiene la tabla de frecuencias a partir de un fichero de medias (.rsm) generado con el software GT 2.0 E. Se aprovecha de que la última tabla de medias coincide con la tabla de frecuencias para obtenerla.",
                            "Importa los datos de un fichero Excel siempre que este tenga el mismo formato que los ficheros exportados por esta misma aplicación.",
                            "Ficheros .csv exportados por la aplicación de José Antonio López López."};

        string[] fileFilter = {"Todos los archivos|*.*",
                              "GT E 2.0 (*.obs)|*.obs|Todos los archivos|*.*",
                              "GT E 2.0 (*.rsm)|*.rsm|Todos los archivos|*.*",
                              "Excel (*.xls)|*.xls|Todos los archivos|*.*",
                              "(*.csv)|*.csv|Todos los archivos|*.*"};
                                
        string pathImportFile;

        TransLibrary.Language lang;


        // Construcctores

        public FormDataImport()
        {
            InitializeComponent();
        }

        public FormDataImport(TransLibrary.Language lang)
        {
            InitializeComponent();
            this.lang = lang;
            // this.formPrincipal = formP;
            traslationElements(Application.StartupPath + LANG_PATH + STRING_DATA_IMPORT);
            initComboBoxTypesFiles();
            this.cBoxTypesFiles.SelectedIndex = 0;
            btBrowse.Enabled = false;
            LoadImagenHeader();
        }


        /* Descripción:
         *  Carga la imagen de cabecera
         */
        private void LoadImagenHeader()
        {
            HideImagenHeader();
            this.pictBoxNormal.Visible = true;
        }

        /* Descripción:
         *  Oculta las imagenes de cabecera.
         */
        private void HideImagenHeader()
        {
            this.pictBoxNormal.Visible = false;
            this.pictBoxOBS.Visible = false;
            this.pictBoxRSM.Visible = false;
            this.pictBoxExcel.Visible = false;
            this.pictBoxCSv.Visible = false;
        }

        /* Descrpición:
         *  Se ejecuta al seleccionar en el checkBox
         */
        private void SelectHeaderImagen()
        {
            int n = this.cBoxTypesFiles.SelectedIndex;
            
            HideImagenHeader();
            switch (n)
            {
                case(0):
                    this.pictBoxNormal.Visible = true;
                    tbComment.Text = fileDescriptions[n];
                    break;
                case(1):
                    this.pictBoxOBS.Visible = true;
                    tbComment.Text = fileDescriptions[n];
                    break;
                case(2):
                    this.pictBoxRSM.Visible = true;
                    tbComment.Text = fileDescriptions[n];
                    break;
                case (3):
                    this.pictBoxExcel.Visible = true;
                    tbComment.Text = fileDescriptions[n];
                    break;
                case (4):
                    this.pictBoxCSv.Visible = true;
                    tbComment.Text = fileDescriptions[n];
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
                // Extraemos el nombre del fichero del path
                string fileExt = this.fileExtension(openDialog.FileName).ToLower(); // Pasamos a minúsculas la extensión

                this.tbNameFile.Text = this.extractFileNamePath(openDialog.FileName);
                // almacenamos el path del fichero                     
                pathImportFile = openDialog.FileName; 
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
                this.formPrincipal.importDataFile(pathImportFile);
                
            }
             * */
        }
      


        /* Descripción:
         *  Cierra la ventana y cancela la operación.
         */
        private void btCancel_Click(object sender, EventArgs e)
        {
            //this.Close();
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


        #region Traducción de la venatana FormDataImport
        /*
         * Descripción:
         *  Cambia de idioma los elementos de la ventana.
         * Parámetros:
         *      TransLibrary.Language lang: Idioma al que vamos a traducir los elementos de la ventana.
         */
        private void traslationElements(string pathFileTrans)
        {
            TransLibrary.ReadFileTrans dic = new TransLibrary.ReadFileTrans(pathFileTrans);
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

            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                // Se produjo un error al traducir
                MsgBoxUtil.HackMessageBox(this.btOk.Text);
                MessageBox.Show(lEx.Message + " Se produjo un error al traducir " + name);
            }
        }

        #endregion Traducción de la venatana FormDataImport

        private void cBoxTypesFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectHeaderImagen();
        }

    }// end public partial class FormDataImport : Form
}// end namespace GUI_TG
