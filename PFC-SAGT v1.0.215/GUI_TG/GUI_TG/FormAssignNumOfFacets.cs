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
 * Fecha de revisión: 06/Oct/2011                           
 * 
 * Descripción:
 *  Asigna el valor que tendrá el número de Facetas de la tabla de facetas
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
    public partial class FormAssignNumOfFacets : Form
    {
        // Variables y Constantes
        //=======================

        // nombre del archivo que contiene las traducciones
        const string STRING_TEXT = "formAssignNumOfFacets.txt";
        const string LANG_PATH = "\\lang\\";

        // Traducción para los mensages y los botones de las ventanas modales
        private TransLibrary.ReadFileTrans dicMessage;

        /*=========================================================================================
         *  Constructores
         *=========================================================================================*/
        public FormAssignNumOfFacets()
        {
            InitializeComponent();
        }

        public FormAssignNumOfFacets(TransLibrary.ReadFileTrans dicMessage, TransLibrary.Language lang)
        {
            
            this.dicMessage = dicMessage;
            InitializeComponent();
            traslationElements(lang, Application.StartupPath + LANG_PATH + STRING_TEXT);
            this.rbCrossed.Checked = true;
        }
        
        /* Descripción:
         *  Impide que se escriba otracosa que no sean numeros en el textBox tBoxNumberOfFacets.
         */
        private void tBoxNumberOfFacets_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
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


        /* Descripción:
         *  Oculta o muestra según el caso el tipo de diseño activo
         */
        private void pictureBox()
        {
            pictureBoxCrossed.Visible = rbCrossed.Checked;
            pictureBoxCrossed_bn.Visible = !rbCrossed.Checked;
            pictureBoxNested.Visible = rbNested.Checked;
            pictureBoxNested_bn.Visible = !rbNested.Checked;
            pictureBoxMixed.Visible = rbMixed.Checked;
            pictureBoxMixed_bn.Visible = !rbMixed.Checked;
        }


        #region Eventos que se ejecutan al pulsar un radioButton
        /* Descripción:
         *  Estos eventos se lanza al pulsar uno de los radioButton y ejecuta
         *  el método que muestra el dibujo correspondiente al tipo de diseño
         *  seleccionado.
         */
        private void rbCrossed_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox();
        }

        private void rbNested_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox();
        }

        private void rbMixed_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox();
        }

        #endregion Eventos que se ejecutan al pulsar un radioButton


        /*
         * Descripción:
         *  Cierra la ventana sin guardar los cambios
         */
        private void btCancel_Click(object sender, EventArgs e)
        {/*
            this.formPrincipal.SetNumOfFacetForTable(0);
            this.formPrincipal.CancelAcciónEditionOfFacet();
            this.Close();
          */
        }

        /* Descripción:
         *  Establece el número de facetas con el que se va a crear la tabla
         */
        private void btOk_Click(object sender, EventArgs e)
        {
            /*
            if (String.IsNullOrEmpty(TextBoxNumOfFacets()))
            {
                // Si el textBox esta vació avisamos del error
                TransLibrary.Language lang = this.formPrincipal.LanguageActually();
                string ok = this.dicMessage.labelTraslation("btOk").LangTraslation(lang).ToString();

                MsgBoxUtil.HackMessageBox(ok);

                MessageBox.Show(messageError2, this.titleMessageError1, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                int t = int.Parse(this.tBoxNumberOfFacets.Text);
                if (t < 2)
                {
                    TransLibrary.Language lang = this.formPrincipal.LanguageActually();
                    string ok = this.dicMessage.labelTraslation("btOk").LangTraslation(lang).ToString();

                    MsgBoxUtil.HackMessageBox(ok);

                    MessageBox.Show(messageError1, this.titleMessageError1, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    this.formPrincipal.SetNumOfFacetForTable(t);                  
                    this.Close();
                    // lanzamos la carga de la ventana
                }
            }
             */
        }

        /* Descripción:
         *  Devuelve el contenido del textBox que contiene el numero de facetas
         */
        public string TextBoxNumOfFacets()
        {
            return this.tBoxNumberOfFacets.Text;
        }

        /* Descripción:
         *  Devuelve un enumerado que indica la disposición de las facetas que se ha seleccionado
         */
        public MultiFacetData.ProvisionOfFacets CheckGroupBoxProvisionOfFacets()
        {
            MultiFacetData.ProvisionOfFacets retVal = MultiFacetData.ProvisionOfFacets.Crossed;
            if (this.rbCrossed.Checked)
            {
                return retVal;
            }
            else if (this.rbMixed.Checked)
            {
                retVal = MultiFacetData.ProvisionOfFacets.Mixed;
            }
            else
            {
                retVal = MultiFacetData.ProvisionOfFacets.Nested;
            }
            return retVal;
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
                name = this.lbTextAddNumFacets.Name.ToString();
                this.lbTextAddNumFacets.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbTextNumFacets.Name.ToString();
                this.lbTextNumFacets.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos la cabecera del groupBox
                name = this.groupBoxProvisionOfFacets.Name.ToString();
                this.groupBoxProvisionOfFacets.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducción de los radio button
                name = this.rbCrossed.Name.ToString();
                this.rbCrossed.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.rbNested.Name.ToString();
                this.rbNested.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.rbMixed.Name.ToString();
                this.rbMixed.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message +" "+ "Error al traducir:"+ " " + name);   
            }
        }
        #endregion Traducción de la ventana


    } // public partial class FormAssignNumOfFacets : Form
} // namespace GUI_TG