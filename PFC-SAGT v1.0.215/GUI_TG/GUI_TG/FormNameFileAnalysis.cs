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
 * Fecha de revisión: 11/Jun/2012                           
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

namespace GUI_GT
{
    public partial class FormNameFileAnalysis : Form
    {
        /***********************************************************************************************
         * Constantes 
         ***********************************************************************************************/
        // Constantes 
        const string STRING_TEXT = "formNameFileAnalysis.txt";
        const string LANG_PATH = "\\lang\\";


        /***********************************************************************************************
         * Constructores
         ***********************************************************************************************/
        public FormNameFileAnalysis()
        {
            InitializeComponent();
        }


        public FormNameFileAnalysis(TransLibrary.Language lang)
            : this()
        {
            traslationElements(lang, Application.StartupPath + LANG_PATH + STRING_TEXT);
        }


        /***********************************************************************************************
         * Métodos
         ***********************************************************************************************/
        public string TextNameFile()
        {
            return this.tbNameFile.Text;
        }


        #region Traducción de la ventana
        /*======================================================================================
         * Traducción de la ventana
         *======================================================================================*/

        /*
         * Descripción:
         *  Traduce todos los textos de la ventana al idioma que se encuentre activo
         */
        private void traslationElements(TransLibrary.Language lang, string nameFileTrans)
        {
            TransLibrary.ReadFileTrans dic = new TransLibrary.ReadFileTrans(nameFileTrans);
            string name = "";
            try
            {
                // Traducimos los Textos de la ventana
                //====================================

                // Traducimos el título de la ventana
                name = this.Name.ToString();
                this.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el botón aceptar
                name = this.btOk.Name.ToString();
                this.btOk.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el botón cancelar
                name = this.btCancel.Name.ToString();
                this.btCancel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " " + "Error al traducir:" + " " + name);
            }
        }
        #endregion Traducción de la ventana
    }
}
