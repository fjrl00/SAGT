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
 * Fecha de revisión: 27/Oct/2011                           
 * 
 * Descripción:
 *  Asignación de anidamientos. Los anidamientos se realizan uno a uno.
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
    public partial class FormAddNesting : Form
    {

        // Constantes 
        const string STRING_TEXT = "formAddNesting.txt";
        const string LANG_PATH = "\\lang\\";

        /*=========================================================================================
         *  Variables
         *=========================================================================================*/

        private string string_select_facet = "Seleccione la faceta";
        private string string_select_nesting_facet = "Seleccione la faceta de anidamiento";
        private string errorNoUse2TimesFacets = "Error: No podemos usar dos veces la faceta";
        private ListFacets lf;

        /*=========================================================================================
         *  Constructores
         *=========================================================================================*/
        public FormAddNesting()
        {
            InitializeComponent();
        }

        public FormAddNesting(TransLibrary.Language lang, ListFacets lf)
            :this()
        {
            this.lf = lf;
            traslationElements(lang, Application.StartupPath + LANG_PATH + STRING_TEXT);
            InitNestingFacetComboBox();
            this.rbNest.Checked = true;// Marcamos la operación anidar por defecto
            this.gBoxOperation.Enabled = false; // por defecto el groupBox esta desactivado
        }
        
        /* Descripción:
         *  Inicializa los comboBox
         */
        private void InitNestingFacetComboBox()
        {
            this.cBoxSelectFacet.Items.Add(string_select_facet);
            this.cBoxNestingFacet.Items.Add(string_select_nesting_facet);

            int l = this.lf.Count();
            for (int i = 0; i < l; i++)
            {
                Facet f = this.lf.FacetInPos(i);
                this.cBoxSelectFacet.Items.Add(f.ListFacetDesing());
                this.cBoxNestingFacet.Items.Add("[" + f.Name() + "]");
            }

            this.cBoxSelectFacet.SelectedIndex = 0;
            this.cBoxNestingFacet.SelectedIndex = 0;
        }

        /* Descripción:
         *  Devuelve la posición del comBoBox de la faceta seleccionada
         */
        public int ComboBoxSelectFacet()
        {
            int pos = cBoxSelectFacet.SelectedIndex - 1;
            return pos;
            //return lf.FacetInPos(pos);
        }

        /* Descripción:
         *  Devuelve la posición del comBoBox de la faceta anidante
         */
        public int ComboBoxNestingFacet()
        {
            int pos = cBoxNestingFacet.SelectedIndex - 1;
            return pos;
            // return lf.FacetInPos(pos);
        }


        /* Descripción:
         *  Evento que se lanza al modificar el contenido de la pestaña selección
         *  de facetas.
         */
        private void cBoxSelectFacet_SelectedIndexChanged(object sender, EventArgs e)
        {
            change_C_Box();
            //int pos = cBoxSelectFacet.SelectedIndex ;
            //if (pos != 0)
            //{
            //    Facet f = this.lf.FacetInPos(pos-1);
            //    string desing = f.ListFacetDesing();
            //    if (desing.Contains(Facet.NETES_CHAR))
            //    {
            //        this.gBoxOperation.Enabled = true;
            //        int pos2 = cBoxNestingFacet.SelectedIndex;
            //        if (pos2 != 0)
            //        {
            //            Facet f2 = this.lf.FacetInPos(pos2-1);
            //            string f2_name = "[" + f2.Name() + "]"; 
            //            string oper = "";
            //            if (!desing.Contains(f2_name))
            //            {
            //                if (this.rbNest.Checked == true)
            //                {
            //                    oper = Facet.NETES_CHAR;
            //                }
            //                tbResult.Text = desing + oper + f2_name;
            //            }
            //            else
            //            {
            //                // error no podemos usar dos veces la faceta de anidamiento
            //                cBoxSelectFacet.SelectedIndex = 0;
            //                this.rbNest.Checked = true;// Marcamos la operación anidar por defecto
            //                this.gBoxOperation.Enabled = false; // por defecto el groupBox esta desactivado
            //                MessageBox.Show("Error: No podemos usar dos veces la misma faceta");
            //            }
            //        }
            //    }
            //    else
            //    {
            //        this.rbNest.Checked = true;// Marcamos la operación anidar por defecto
            //        this.gBoxOperation.Enabled = false; // por defecto el groupBox esta desactivado
            //    }
            //}
            //else
            //{
            //    this.rbNest.Checked = true;// Marcamos la operación anidar por defecto
            //    this.gBoxOperation.Enabled = false; // por defecto el groupBox esta desactivado
            //    this.tbResult.Text = "";
            //}
        }

        /* Descripción:
         *  Evento que se lanza al modificar el contenido de la pestaña selección
         *  de facetas anidante.
         */
        private void cBoxNestingFacet_SelectedIndexChanged(object sender, EventArgs e)
        {
            change_C_Box();
            //int pos_nesting = this.cBoxNestingFacet.SelectedIndex;
            //if (pos_nesting != 0)
            //{
            //    Facet f2 = this.lf.FacetInPos(pos_nesting-1);
            //    int pos_select_facet = this.cBoxSelectFacet.SelectedIndex;
            //    if(pos_select_facet != 0)
            //    {
            //        Facet f = this.lf.FacetInPos(pos_select_facet-1);
            //        string desing = f.ListFacetDesing();
            //        string oper = Facet.NETES_CHAR;
            //        if (desing.Contains(Facet.NETES_CHAR))
            //        {
            //            this.gBoxOperation.Enabled = true;
            //            if (this.rbNest.Checked == true)
            //            {
            //                oper = "";
            //            }
            //        }
            //        else
            //        {
            //            this.rbNest.Checked = true;// Marcamos la operación anidar por defecto
            //            this.gBoxOperation.Enabled = false; // por defecto el groupBox esta desactivado

            //        }
            //        tbResult.Text = desing + oper + "[" + f2.Name() + "]";
            //    }
            //    else
            //    {
            //        this.rbNest.Checked = true;// Marcamos la operación anidar por defecto
            //        this.gBoxOperation.Enabled = false; // por defecto el groupBox esta desactivado
            //    }
            //}
            //else
            //{
            //    this.rbNest.Checked = true;// Marcamos la operación anidar por defecto
            //    this.gBoxOperation.Enabled = false; // por defecto el groupBox esta desactivado
            //    this.tbResult.Text = "";
            //}
        }

        private void change_C_Box()
        {
            int pos_select_facet = cBoxSelectFacet.SelectedIndex;
            int pos_nesting_facet = cBoxNestingFacet.SelectedIndex;

            if ((pos_select_facet > 0) && (pos_nesting_facet > 0))
            {
                Facet f = this.lf.FacetInPos(pos_select_facet - 1);
                string desing = f.ListFacetDesing(); // obtenemos el diseño
                Facet f2 = this.lf.FacetInPos(pos_nesting_facet - 1);
                string f2_name = "[" + f2.Name() + "]"; // obtenemos la operación
                string oper = "";
                // Comprobamos que no hallamos usado anteriormente la faceta en el diseño
                if (!desing.Contains(f2_name))
                {
                    if (desing.Contains(Facet.NEST_CHAR))
                    {
                        // esta permitido cambiar de operacion
                        this.gBoxOperation.Enabled = true;
                    }
                    else
                    {
                        // Solo podemos usar anicamiento
                        this.rbNest.Checked = true;// Marcamos la operación anidar por defecto
                        this.gBoxOperation.Enabled = false; // por defecto el groupBox esta desactivado
                    }
                    if (this.rbNest.Checked == true)
                    {
                        oper = Facet.NEST_CHAR;
                    }
                    tbResult.Text = desing + oper + f2_name;
                }
                else
                {
                    // error no podemos usar dos veces la faceta de anidamiento
                    this.cBoxNestingFacet.SelectedIndex = 0;
                    this.rbNest.Checked = true;// Marcamos la operación anidar por defecto
                    this.gBoxOperation.Enabled = false; // por defecto el groupBox esta desactivado
                    MessageBox.Show(errorNoUse2TimesFacets +" "+f2_name);
                }

                
            }
            else
            {
                this.rbNest.Checked = true;// Marcamos la operación anidar por defecto
                this.gBoxOperation.Enabled = false; // por defecto el groupBox esta desactivado
                this.tbResult.Text = "";
            }
        }

        /* Desccrición:
         *  Evento que selanza cuando cambia el valor del radioButton anidar.
         */
        private void rbNest_CheckedChanged(object sender, EventArgs e)
        {
            change_C_Box();
        }

        /* Desccrición:
         *  Evento que selanza cuando cambia el valor del radioButton cruzar.
         */
        private void rbCross_CheckedChanged(object sender, EventArgs e)
        {
            change_C_Box();
        }

        /* Descripción:
         *  Devuelve el valor del radioButton Anidar.
         */
        public bool RadioButtonNest()
        {
            return rbNest.Checked;
        }

        /* Descripción:
         *  Devuelve el valor del radioButton Cruzar.
         */
        public bool RadioButtonCross()
        {
            return rbCross.Checked;
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

                // Traducimos etiquetas
                name = this.lbSelectFacet.Name.ToString();
                this.lbSelectFacet.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbSelectNestingFacet.Name.ToString();
                this.lbSelectNestingFacet.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbResult.Name.ToString();
                this.lbResult.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                //Traducimos el groupBox
                name = this.gBoxOperation.Name.ToString();
                this.gBoxOperation.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos los radio button
                name = this.rbNest.Name.ToString();
                this.rbNest.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.rbCross.Name.ToString();
                this.rbCross.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos los textos de los comboBox
                name = "STRING_SELECT_FACET";
                string_select_facet = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "STRING_SELECT_NESTING_FACET";
                string_select_nesting_facet = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos mensage de error
                name = "errorNoUse2TimesFacets";
                errorNoUse2TimesFacets = dic.labelTraslation(name).GetTranslation(lang).ToString();
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " " + "Error al traducir:" + " " + name);
            }
        }
        #endregion Traducción de la ventana


    }// end public partial class FormAddNesting : Form
}// end namespace