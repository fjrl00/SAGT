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
 * Fecha de revisión: 4/Jul/2011           
 * 
 */
using System;
using System.Windows.Forms;
using MultiFacetData;

namespace GUI_GT
{
    public partial class FormMeasurDesign : Form
    {

        /*=================================================================================
         * Constantes
         *=================================================================================*/
        // nombre del archivo que contiene las traducciones
        const string STRING_TEXT = "formMeasurDesign.txt";
        const string LANG_PATH = "\\lang\\";


        /*=================================================================================
         * Variables
         *=================================================================================*/
        // Variables
        ListFacets lfDiff; // lista de facetas de diferenciación, en principio la lista vacia
        ListFacets lfInst; // lista de facetas de instrumentación, en principio la original
        ListFacets lfParent; // lista de facetas original
        // Mensages (String)
        // Este mensage se muestra si no se han seleccionado las fuentes de variación de manera correcta
        private string txtMessageNoSourceOfDiff = "No hay una fuente dependiente seleccionada";
        private string txtMessageNoSourceOfInst = "No hay una fuente de instrumentación seleccionada";


        /*=================================================================================
         * Constructores
         *=================================================================================*/

        public FormMeasurDesign()
        {
            InitializeComponent();
        }


        /*
         * Descripción:
         *  Constructor. Inicializa las variables que contiene la lista de facetas (fuentes de
         *  variciación) facetas de diferenciación e instrumentación.
         */
        public FormMeasurDesign(ListFacets lfDiff, ListFacets lfInst, ListFacets lfParent, TransLibrary.Language lang)
        {
            InitializeComponent();
            // traducimos
            this.traslationElements(lang, Application.StartupPath + LANG_PATH + STRING_TEXT);

            // asignamos las fuentes de variación
            this.lfDiff = lfDiff;
            this.lfInst = lfInst;
            // Por defecto las facetas irán todas a instrumentación
            if (lfDiff.IsEmpty() && lfInst.IsEmpty())
            {
                foreach (Facet f in lfParent)
                {
                    this.lfInst.Add(f);
                }
            }
            this.lfParent = lfParent;
                
            LoadSourceOfInstListBox();
            this.listBoxSourceInst.SetSelected(0,true);
            WriteSourceOfVarInTextBox();
        }



        /*=================================================================================
         * Métodos
         *=================================================================================*/

        /*
         * Descripción:
         *  Carga las fuentes de variación en cada una de los listBox. El listBox de la izquierda
         *  contiene las fuentes de instrumentación, el listBox de la derecha contiene
         *  las fuentes de diferenciación.
         */
        private void LoadSourceOfInstListBox()
        {
            // limpiamos los listBox
            this.listBoxSourceDiff.Items.Clear();
            this.listBoxSourceInst.Items.Clear();
            // cargamos los datos en el listBox de fuentes instrumentación
            foreach (Facet f in this.lfInst)
            {
                this.listBoxSourceInst.Items.Add(f.Name());
            }
            // cargamos los datos en el listBox de fuentes diferenciación
            foreach (Facet f in this.lfDiff)
            {
                this.listBoxSourceDiff.Items.Add(f.Name());
            }
        }


        /* Descripicón:
         *  Escribe el diseño de medida en el textBox.
         */
        private void WriteSourceOfVarInTextBox()
        {
            this.tbMeasurementDesign.Text = "";
            if ((this.lfDiff.Count() > 0) && (this.lfInst.Count() > 0))
            {
                this.tbMeasurementDesign.Text = this.lfDiff.StringOfListFactes() + "/" + this.lfInst.StringOfListFactes(); 
            }
        }


        /*
         * Descripción:
         *  Mueve la faceta seleccionada de el listBox de fuentes independientes al listBox de 
         *  fuentes dependientes.
         */
        private void btMoveLeft_Click(object sender, EventArgs e)
        {
            if (this.listBoxSourceInst.SelectedIndex >= 0)
            {
                int pos = this.listBoxSourceInst.SelectedIndex;
                this.lfDiff.ParentOrderAdd(this.lfInst.FacetInPos(pos), lfParent);
                this.lfInst.Remove(this.lfInst.FacetInPos(pos));
                WriteSourceOfVarInTextBox();
                LoadSourceOfInstListBox();
                if (listBoxSourceInst.Items.Count > 0)
                {
                    // mientras haya elementos en el listBox selecionaremos el primero
                    this.listBoxSourceInst.SetSelected(0, true);
                }
                else
                {
                    // si no hay elementos en el listBox seleccionaremos los del otro listBox
                    this.listBoxSourceDiff.SetSelected(0,true);
                }
            }
            else
            {
                MessageBox.Show(txtMessageNoSourceOfInst, "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            
        }


        /* Descripción:
         *  Mueve la faceta seleccionada del listBos de fuentes de diferenciación al listBox de 
         *  fuentes de instrumentación.
         */
        private void btMoveRight_Click(object sender, EventArgs e)
        {
            if (this.listBoxSourceDiff.SelectedIndex >= 0)
            {
                int pos = this.listBoxSourceDiff.SelectedIndex;
                this.lfInst.ParentOrderAdd(this.lfDiff.FacetInPos(pos), lfParent);
                this.lfDiff.Remove(this.lfDiff.FacetInPos(pos));
                WriteSourceOfVarInTextBox();
                LoadSourceOfInstListBox();
                if (listBoxSourceDiff.Items.Count > 0)
                {
                    // mientras haya elementos en el listBox selecionaremos el primero
                    this.listBoxSourceDiff.SetSelected(0, true);
                }
                else
                {
                    // si no hay elementos en el listBox seleccionaremos los del otro listBox
                    this.listBoxSourceInst.SetSelected(0, true);
                }
            }
            else
            {
                MessageBox.Show(txtMessageNoSourceOfDiff, "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


        /*
         * Descripción:
         *  Cierra la ventana y cancela la operación.
         */
        private void btCancel_Click(object sender, EventArgs e)
        {
            /*
            foreach(Facet f in this.lfLeft)
            {
                this.lfLeft.Remove(f);
            }
             */
            // this.Close();
        }


        /*
         * Descripcíón:
         *  Comprueba que se ha definido el diseño de medida correctamente y
         *  a continuación muestra el analisis de varianza.
         */
        private void btOK_Click(object sender, EventArgs e)
        {
            /*
            if (this.lfLeft.Count() == 0 || this.lfRight.Count() == 0)
            {
                MessageBox.Show(txtMessageNoValidate, "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
               this.Close();
            }
            */
        }


        /* Descripción:
         *  Devuelve el número de facetas definidas como facetas dependientes (Facetas a la izquierda de 
         *  la expresión)
         */
        public int ListFacetDiff()
        {
            return this.lfDiff.Count();
        }


        /* Descripción:
         *  Devuelve el número de facetas definidas como facetas independientes (Facetas a la derecha de 
         *  la expresión)
         */
        public int ListFacetInst()
        {
            return this.lfInst.Count();
        }


        #region Tradución de la ventana FormMeasurDesign
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
                name = this.btOK.Name.ToString();
                this.btOK.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el boton cancelar
                name = this.btCancel.Name.ToString();
                this.btCancel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el boton mover a la izquierda
                name = this.btMoveLeft.Name.ToString();
                this.btMoveLeft.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el boton mover a la derecha
                name = this.btMoveRight.Name.ToString();
                this.btMoveRight.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // traducimos las etiquetas
                name = this.lbMeasurementDesign.Name.ToString();
                this.lbMeasurementDesign.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbInstr_Facets.Name.ToString();
                this.lbInstr_Facets.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbDiff_Facets.Name.ToString();
                this.lbDiff_Facets.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // traducimos los tres mensages
                name = "txtMessageNoSourceOfDiff";
                this.txtMessageNoSourceOfDiff = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "txtMessageNoSourceOfInst";
                this.txtMessageNoSourceOfInst = dic.labelTraslation(name).GetTranslation(lang).ToString();
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " Se produjo un error al traducir " + name);
            }
        }

        #endregion Tradución de la ventana FormMeasurDesign

    }// end public partial class FormMeasurDesign : Form
} // end namespace GUI_TG
