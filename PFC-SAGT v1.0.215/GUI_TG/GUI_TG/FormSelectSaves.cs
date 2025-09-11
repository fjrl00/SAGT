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
 * Fecha de revisión: 15/Dic/2011                           
 * 
 * Descripción:
 *      Permite la selección de los  elementos que se van a guardar.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUI_GT
{
    public partial class FormSelectSaves : Form
    {
        /*=========================================================================================
         *  Constantes
         *=========================================================================================*/
        const string STRING_TEXT = "formSelectSaves.txt";
        const string LANG_PATH = "\\lang\\";

        /*=========================================================================================
         *  Constructores
         *=========================================================================================*/ 

        /* Descripción:
         *  Constructor por defecto.
         */ 
        public FormSelectSaves()
        {
            InitializeComponent();
            // Ocultamos la etiqueta y el textbox de nombre del archivo
            this.tbNameFile.Visible = false;
            this.lbNameFile.Visible = false;
            // Así ponemos el texto con fondo transparente
            this.lbSavesText.Parent = this.pictureBoxHeader;
        }


        /* Descripción:
         *  Constructor. Inicializa las casillas habilitandolas y marcandolas según el caso.
         * Parámetros:
         *      TransLibrary.Language lang: Idioma que mostrará los textos de la ventana.
         *      bool bData: Indica si se habilitará o no la pestaña de tabla de frecuencias.
         *      bool bMean: Indica si se habilitará o no la pestaña de tablas de medias. 
         *      bool bSsq: Indica si se habilitará o no la tablas de análisis.
         */
        public FormSelectSaves(TransLibrary.Language lang, bool bData, bool bMean, bool bSsq)
            :this()
        {
            traslationElements(lang, Application.StartupPath + LANG_PATH + STRING_TEXT);
            if (bData)
            {
                this.checkBoxSelectDatas.Checked = true;
            }
            else
            {
                this.checkBoxSelectDatas.Enabled = false;
            }
            if (bMean)
            {
                this.checkBoxSelectTableMeans.Checked = true;
            }
            else
            {
                this.checkBoxSelectTableMeans.Enabled = false;
            }
            if (bSsq)
            {
                this.checkBoxSelectSSQ.Checked = true;
            }
            else
            {
                this.checkBoxSelectSSQ.Enabled = false;
            }
        }// end FormSelectSaves


        /* Descripción:
         *  Constructor. Inicializa las casillas habilitandolas y marcandolas según el caso.
         * Parámetros:
         *      TransLibrary.Language lang: Idioma que mostrará los textos de la ventana.
         *      bool bData: Indica si se habilitará o no la pestaña de tabla de frecuencias.
         *      bool bMean: Indica si se habilitará o no la pestaña de tablas de medias. 
         *      bool bSsq: Indica si se habilitará o no la tablas de análisis.
         *      bool visible: Indica si se va a mostrár o no la etiqueta y el textBox de nombre del archivo
         */
        public FormSelectSaves(TransLibrary.Language lang, bool bData, bool bMean, bool bSsq, bool visible)
            : this(lang, bData, bMean, bSsq)
        {
            // Ocultamos o mostramos la etiqueta y el textbox de nombre del archivo
            this.tbNameFile.Visible = visible;
            this.lbNameFile.Visible = visible;
        }

        #region Métodos de consulta
        /*=========================================================================================
         *  Métodos de counsulta
         *  - IsDataSelected
         *  - IsMeansSelected
         *  - IsSsqSelected
         *=========================================================================================*/

        /* Descripción:
         *  Devuelve true si esta marcado el checkBox de datos, false en caso contrario.
         */
        public bool IsDataSelected()
        {
            return this.checkBoxSelectDatas.Checked;
        }


        /* Descripción:
         *  Devuelve true si esta marcado el checkBox de medias, false en caso contrario.
         */
        public bool IsMeansSelected()
        {
            return this.checkBoxSelectTableMeans.Checked;
        }


        /* Descripción:
         *  Devuelve true si esta marcado el checkBox de suma de cuadrados, false en caso contrario.
         */
        public bool IsSsqSelected()
        {
            return this.checkBoxSelectSSQ.Checked;
        }


        /* Descripción:
         *  Devuelve el nombre del archivo
         */
        public string NameFile()
        {
            return this.tbNameFile.Text;
        }

        #endregion Métodos de consulta


        #region Traducción de la ventana
        /*======================================================================================
         * Traducción de la ventana
         *======================================================================================*/

        /*
         * Descripción:
         *  Traduce todos los textos de la ventana al idioma que se encuentre activo
         */
        private void traslationElements(TransLibrary.Language lang, string pathFileTrans)
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

                // Traducimos etiquetas
                name = this.lbSavesText.Name.ToString();
                this.lbSavesText.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos los checkBox
                name = this.checkBoxSelectDatas.Name.ToString();
                this.checkBoxSelectDatas.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.checkBoxSelectTableMeans.Name.ToString();
                this.checkBoxSelectTableMeans.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.checkBoxSelectSSQ.Name.ToString();
                this.checkBoxSelectSSQ.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos la etiqueta de nombre del archivo
                name = this.lbNameFile.Name.ToString();
                this.lbNameFile.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " " + "Error al traducir:" + " " + name);
            }
        }// end traslationElements
        #endregion Traducción de la ventana


    }// end public partial class FormSelectSaves
}// end namespace GUI_TG
