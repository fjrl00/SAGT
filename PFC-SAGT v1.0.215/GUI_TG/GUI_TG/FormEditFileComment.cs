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
 * Fecha de revisión: 25/Ago/2011       
 * 
 * Descripción:
 *  Ventana para editar los comentarios que se añaden a los ficheros.
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
    public partial class FormEditFileComment : Form
    {
        /*====================================================================================================
         *  Costantes y variables
         *====================================================================================================*/
        const string STRING_TEXT = "formEditFileComment.txt";
        const string LANG_PATH = "\\lang\\";

        /*====================================================================================================
         *  Constructores
         *====================================================================================================*/

        public FormEditFileComment()
        {
            InitializeComponent();
        }

        public FormEditFileComment(string text)
            : this()
        {
            this.richTextBoxComment.Text = text;
        }

        public FormEditFileComment(string text, TransLibrary.Language lang)
            : this(text)
        {
            traslationElements(lang, Application.StartupPath + LANG_PATH + STRING_TEXT);
            richTextBoxComment.Focus();
        }

        /*====================================================================================================
         *  Métodos de consulta
         *====================================================================================================*/

        /* Descripción:
         *  Devuelve el texto presente en RichTextBox.
         */
        public string GetRichTextBoxText()
        {
            return richTextBoxComment.Text;
        }


        #region Traducción de la ventana
        /*======================================================================================
         * Traducción de la ventana
         *======================================================================================*/

        /*
         * Descripción:
         *  Traduce todos los textos de la ventana al idioma que se encuentre activo
         */
        public void traslationElements(TransLibrary.Language lang, string pathFileTrans)
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

                // Traducimos los botones
                name = this.btOk.Name.ToString();
                this.btOk.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.btCancel.Name.ToString();
                this.btCancel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " Se produjo un error al traducir " + name);
            }
        }
        #endregion Traducción de la ventana
        
    }// end public partial class FormEditFileComment
}// end namespace GUI_TG
