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
 * Fecha de revisión: 26/Abr/2012               
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
using MultiFacetData;

namespace GUI_GT
{
    public partial class FormOptionsForChart_Two : Form
    {
        /****************************************************************************************************
         *  Variables
         ****************************************************************************************************/
        // nombre del archivo que contiene las traducciones
        const string STRING_TEXT = "formOptionsForChart_Two.txt";
        const string LANG_PATH = "\\lang\\";

        /****************************************************************************************************
         * Constructores
         ****************************************************************************************************/
        public FormOptionsForChart_Two()
        {
            InitializeComponent();
        }


        public FormOptionsForChart_Two(TransLibrary.Language lang, ListFacets lf)
        {
            InitializeComponent();
            traslationElements(lang, Application.StartupPath + LANG_PATH + STRING_TEXT);
            LoadListFacetsInCheckListBox(lf);
        }


        /* Descripción:
         *  Método auxiliar del constructor. Rellena el checkedListBox con los nombres de las facetas
         */
        public void LoadListFacetsInCheckListBox(ListFacets lf)
        {
            // ahora agregamos una lista de facetas
            int n = lf.Count();
            for (int i = 0; i < n; i++)
            {
                Facet f = lf.FacetInPos(i);

                string stringNameFacets = "["+f.Name()+"]";
                
                this.cListBoxListsFacets.Items.Add(stringNameFacets);
            }
        }


        /****************************************************************************************************
         * Métodos de consulta
         ****************************************************************************************************/

        /* Descripción:
         *  Devuelve el CheckedListBox para poder consultar las posiciones marcadas.
         */
        public CheckedListBox CheckedListBoxListFacets()
        {
            return this.cListBoxListsFacets;
        }


        /* Descripción:
         *  Devuelve un int a partir del cual comienza representarse los valores de la gráfica. Devuelve
         *  cero si el valor es nulo.
         */
        public int Beginning()
        {
            int retVal = 0;
            if (!string.IsNullOrEmpty(this.textBoxBeginning.Text))
            {
                retVal = int.Parse(this.textBoxBeginning.Text);
            }
            return retVal;
        }


        /* Descripción:
         *  Devuelve un int a partir del cual terminará de representarse los valores de la gráfica. Devuelve
         *  cero si el valor es nulo.
         */
        public int Ending()
        {
            int retVal = 0;
            if (!string.IsNullOrEmpty(this.textBoxEnding.Text))
            {
                retVal = int.Parse(this.textBoxEnding.Text);
            }
            return retVal;
        }

        /* Descripción:
         *  Devuelve el valor numerico del incremeto
         */
        public int Increment()
        {
            return Decimal.ToInt32(this.numericUpDownInterval.Value);
        }


        /****************************************************************************************************
         * Eventos
         ****************************************************************************************************/

        /* Descripción:
         *  Evento que fuerza que solo se puedan escribir enteros positivos en el textBox de 
         *  valor inicial del intervalo.
         */
        private void textBoxEnding_KeyPress(object sender, KeyPressEventArgs e)
        {
            NumberValueEvent(sender, e);
        }


        /* Descripción:
         *  Evento que fuerza que solo se puedan escribir enteros positivos en el textBox de 
         *  valor final del intervalo.
         */
        private void textBoxBeginning_KeyPress(object sender, KeyPressEventArgs e)
        {
            NumberValueEvent(sender, e);
        }


        /* Descripción:
         *  Fuerza a que se escriban números enteros positivos
         */
        private void NumberValueEvent(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }

            if (e.KeyChar >= 48 && e.KeyChar <= 57)
                e.Handled = false;
            else
                e.Handled = true;
        }


        #region Traducción de la ventana
        /****************************************************************************************************
         * Traducciones
         ****************************************************************************************************/
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
                // Taducimos la etiqueta "Facetas de instrumentación:"
                name = this.lbInst_Facets.Name.ToString();
                this.lbInst_Facets.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el boton aceptar
                name = this.btOK.Name.ToString();
                this.btOK.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el boton cancelar
                name = this.btCancel.Name.ToString();
                this.btCancel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Tradución del groupBox de Intervalo de valores
                name = this.groupBoxRangeValues.Name.ToString();
                this.groupBoxRangeValues.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbBeginning.Name.ToString();
                this.lbBeginning.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbEnding.Name.ToString();
                this.lbEnding.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbIncrement.Name.ToString();
                this.lbIncrement.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " Se produjo un error al traducir " + name);
            }
        }
        #endregion Traducción de la ventana


    }// public partial class FormOptionsForChart_Two : Form
}// namespace GUI_TG
