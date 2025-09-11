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
 * Fecha de revisión: 25/Ene/2012                           
 * 
 * Descripción:
 *      Ventana en la que se selecciona los niveles que se va a omitir para el estudio.
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
    public partial class FormOmitLevelFacet : Form
    {
        /*-------------------------------------------------------------------------------------
         * Constantes
         *-------------------------------------------------------------------------------------*/

        // nombre del archivo que contiene las traducciones
        const string STRING_TEXT = "formOmitLevelFacet.txt";
        const string LANG_PATH = "\\lang\\";


        /*-------------------------------------------------------------------------------------
         * Variables
         *-------------------------------------------------------------------------------------*/

        // variables 
        private ListFacets listFacets;
        

        /*-------------------------------------------------------------------------------------
         * Constructores
         *-------------------------------------------------------------------------------------*/
        public FormOmitLevelFacet()
        {
            InitializeComponent();
        }


        public FormOmitLevelFacet(TransLibrary.Language lang, ListFacets listFacets)
            : this()
        {
            traslationElements(lang, Application.StartupPath + LANG_PATH + STRING_TEXT);
            this.listFacets = listFacets;
            InitComboBoxSelectFacet(this.listFacets);
        }


        #region Eventos de ComboBox Seleccionar faceta


        /* Descripción:
         *  Evento que se lanza cuando cambia el indice de comboBox seleccionar faceta.
         */
        private void cBoxSelectFacet_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = cBoxSelectFacet.SelectedIndex;
            Facet f = this.listFacets.FacetInPos(ind);
            int level = f.Level();
            checkedListBoxSelectShipLevels.Items.Clear();
            for (int i = 0; i < level; i++)
            {
                checkedListBoxSelectShipLevels.Items.Add(i+1);
                bool b = f.GetSkipLevels(i + 1);
                checkedListBoxSelectShipLevels.SetItemChecked(i,b);
            }
        }


        /* Descripción:
         *  Aplica los valores marcados en la variable Multifaceta.
         */
        private void checkedListBoxSelectShipLevels_SelectedValueChanged(object sender, EventArgs e)
        {
            int indFacet = this.cBoxSelectFacet.SelectedIndex;
            int indLevel = this.checkedListBoxSelectShipLevels.SelectedIndex;
            Facet f = this.listFacets.FacetInPos(indFacet);

            f.SetSkipLevels(indLevel+1, this.checkedListBoxSelectShipLevels.GetItemChecked(indLevel));
        }

        #endregion Eventos de ComboBox Seleccionar faceta


        /*-------------------------------------------------------------------------------------
         * Métodos
         *-------------------------------------------------------------------------------------*/

        /* Descripción:
         *  Inicializa el GroupBox con los nombres de las facetas
         */
        private void InitComboBoxSelectFacet(ListFacets listFacets)
        {
            int n = listFacets.Count();
            for (int i = 0; i < n; i++)
            {
                Facet f = listFacets.FacetInPos(i);
                cBoxSelectFacet.Items.Add(f.Name());
            }
            cBoxSelectFacet.SelectedIndex = 0;
        }


        /* Descripción:
         *  Devuelve el checkBox para que pueda ser consultado
         */
        public ComboBox GetComBoxSelectFacet()
        {
            return this.cBoxSelectFacet;
        }


        /* Descripción:
         *  Deuelve el CheckedListBox con los niveles de las facetas
         */
        public CheckedListBox GetCheckedListBoxSelectShipLevels()
        {
            return this.checkedListBoxSelectShipLevels;
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
                name = this.btOk.Name.ToString();
                this.btOk.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el boton cancelar
                name = this.btCancel.Name.ToString();
                this.btCancel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos etiquetas
                name = this.lbSelectFacet.Name.ToString();
                this.lbSelectFacet.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbSelectSkipLevels.Name.ToString();
                this.lbSelectSkipLevels.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " Se produjo un error al traducir " + name);
            }
        }// end traslationElements


        #endregion Traducción de la ventana
        

    }// end public partial class FormOmitLevelFacet
}// end namespace GUI_TG
