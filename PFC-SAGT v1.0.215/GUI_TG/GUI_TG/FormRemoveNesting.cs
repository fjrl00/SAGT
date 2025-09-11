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
 * Fecha de revisión: 25/Oct/2011                           
 * 
 * Descripción:
 *  Eliminación de anidamientos. Podemos seleccionar varios anidamientos a la vez.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MultiFacetData;

namespace GUI_GT
{
    public partial class FormRemoveNesting : Form
    {
        /*=========================================================================================
         *  Constantes 
         *=========================================================================================*/
        // Constantes 
        const string STRING_TEXT = "formRemoveNesting.txt";
        const string LANG_PATH = "\\lang\\";

        /*=========================================================================================
         *  Constructores
         *=========================================================================================*/ 
        public FormRemoveNesting()
        {
            InitializeComponent();
        }

        public FormRemoveNesting(TransLibrary.Language lang,  List<String> lf_desing):
            this()
        {
            traslationElements(lang, Application.StartupPath + LANG_PATH + STRING_TEXT);
            InitCheckedListBoxSelectNestingRemove(lf_desing);
        }

        /* Descripción:
         *  Introduce en el checkedListBox una lista compuesta por pares de facetas que representan
         *  los diseños.
         * Parámetros:
         *  ListFacets lf_nesting: lista de facetas en la cual se anida.
         */
        private void InitCheckedListBoxSelectNestingRemove(List<String> lf_desing)
        {
            int n = lf_desing.Count;
            for (int i = 0; i < n; i++)
            {
                string line = lf_desing[i];
                cListBoxSelectNestingRemove.Items.Add(line);
            }
        }

        /* Descripción:
         *  Devuelve el CheckedListBox para poder consultar los anidamientos que han sido seleccionados
         *  para ser eliminados.
         */
        public CheckedListBox CheckedListBoxSelectNestingRemove()
        {
            return cListBoxSelectNestingRemove;
        }

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
                name = this.btRemove.Name.ToString();
                this.btRemove.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el boton cancelar
                name = this.btCancel.Name.ToString();
                this.btCancel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos etiquetas
                name = this.lbSelectNesting.Name.ToString();
                this.lbSelectNesting.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " " + "Error al traducir:" + " " + name);
            }
        }
        #endregion Traducción de la ventana


    }// end public partial class FormRemoveNesting : Form
}// end namespace GUI_TG
