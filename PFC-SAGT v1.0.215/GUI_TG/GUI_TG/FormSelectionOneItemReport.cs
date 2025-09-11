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
 * Fecha de revisión: 27/Mar/2012
 * 
 * Descripción: 
 *      Formulario para la selección de la media que vamos a mostrar.
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
    public partial class FormSelectionOneItemReport : Form
    {
        public enum TypeSelectReport
        {
            Analysis, Means
        }

        /*========================================================================================================
         * Constantes
         *========================================================================================================*/
        // nombre del archivo que contiene las traducciones
        const string FILE_TRANS = "formSelectionOneMeanReport.txt";
        const string LANG_PATH = "\\lang\\";

        /*========================================================================================================
         * Variables 
         *========================================================================================================*/
        private string titleAnalysis = "Seleccione informe de análisis";


        /*========================================================================================================
         * Constructores
         *========================================================================================================*/
        public FormSelectionOneItemReport(List<string> lst, TransLibrary.Language lang)
        {
            InitializeComponent();
            LoadListSelection(lst);
            traslationElements(lang, Application.StartupPath + LANG_PATH + FILE_TRANS);
        }


        public FormSelectionOneItemReport(List<string> lst, TransLibrary.Language lang, TypeSelectReport typeSelect)
            : this(lst, lang)
        {
            if(typeSelect.Equals(TypeSelectReport.Analysis))
            {
                this.Text = this.titleAnalysis;
            }
        }


        /*========================================================================================================
         * Métodos
         *========================================================================================================*/ 

        /* Descripción:
         *  Rellena el listBox
         */
        private void LoadListSelection(List<string> lst)
        {
            int n = lst.Count;
            for (int i = 0; i < n; i++)
            {
                cListBoxListsFacets.Items.Add(lst[i]);
            }
        }


        /* Descripción:
         *  Devuelve el valor seleccionado
         */
        public int SelectionIndex()
        {
            int r = -1;
            if (this.cListBoxListsFacets.CheckedItems.Count > 0)
            {
                r = this.cListBoxListsFacets.SelectedIndex;
            }
            return r;
        }


        /* Descripción:
         *  Evento, cuando se selecciona un indice se desmarcan todo los demás de esta manera
         *  solo habra uno marcado.
         */
        private void cListBoxListsFacets_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pos = this.SelectionIndex();
            if (pos > -1)
            {
                int n = this.cListBoxListsFacets.Items.Count;
                for (int i = 0; i < n; i++)
                {
                    this.cListBoxListsFacets.SetItemChecked(i, false);
                }
                this.cListBoxListsFacets.SetItemChecked(pos, true);
            }
            else
            {
                // Mostramos un mensaje indicando
            }
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

                name = this.Name.ToString();
                this.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                name = "titleAnalysis";
                titleAnalysis = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos el boton aceptar
                name = this.btOk.Name.ToString();
                this.btOk.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el boton cancelar
                name = this.btCancel.Name.ToString();
                this.btCancel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " Se produjo un error al traducir " + name);
            }
        }// end traslationElements

        #endregion Traducción de la ventana

    }// end partial class FormSelectionOneMeanReport
}// namespace GUI_TG
