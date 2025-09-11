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
 * Fecha de revisión: 05/Ene/2012                           
 * 
 * Descripción:
 *      Permite la selección de los  elementos que se van a formar parte del informe.
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
    public partial class FormSelectReports : Form
    {
        /*=========================================================================================
         *  Constantes
         *=========================================================================================*/
        const string STRING_TEXT = "formSelectReports.txt";
        const string LANG_PATH = "\\lang\\";

        /*=========================================================================================
         *  Constructores
         *=========================================================================================*/ 

        public FormSelectReports()
        {
            InitializeComponent();
            // Así ponemos el texto con fondo transparente
            this.lbSelectReportText.Parent = this.pictureBoxHeader;
        }

        public FormSelectReports(TransLibrary.Language lang, bool bData, bool bMean, bool bSsq)
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
        }

        #region Métodos de counsulta
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


        #endregion Métodos de counsulta


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
                name = this.lbSelectReportText.Name.ToString();
                this.lbSelectReportText.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos los checkBox
                name = this.checkBoxSelectDatas.Name.ToString();
                this.checkBoxSelectDatas.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.checkBoxSelectTableMeans.Name.ToString();
                this.checkBoxSelectTableMeans.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.checkBoxSelectSSQ.Name.ToString();
                this.checkBoxSelectSSQ.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " " + "Error al traducir:" + " " + name);
            }
        }

        #endregion Traducción de la ventana

    }// end public partial class FormSelectReports
}// end namespace GUI_TG
