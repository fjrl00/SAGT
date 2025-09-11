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
 * Fecha de revisión: 2/Nov/2011
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
using ProjectMeans;

namespace GUI_GT
{
    public partial class FormListFacets : Form
    {
        /*========================================================================================================
         * Constantes
         *========================================================================================================*/ 
        // nombre del archivo que contiene las traducciones
        const string STRING_TEXT = "formListFacets.txt";
        const string LANG_PATH = "\\lang\\";

        /*========================================================================================================
         * Variables 
         *========================================================================================================*/ 
        private string txtSelectAll = ""; // guardara el texto "Seleccionar todo" en el idioma correspondiente.
        private string txtUnCheckAll = "";// guardara el texto "Desmarcar todo" en el idioma correspondiente.
        // private string txtMessageNoSelected = "No hay seleccionado ningún elemento"; // Mensage de que no ha seleccionado ningún elemento del checkedListBox.
        

        private bool selectOrUchecked = true; /* Variable booleana para saber en que estado se encuentra el
                                               * boton seleccionar. Si esta a true entoces indica seleccionar 
                                               * todo, false en el caso de deseleccionar todo*/

        List<ListFacets> listOfListFacets; // Combinaciones de la lista de facetas para contruir las tablas de medias
        // FormPrincipal formP;


        /*========================================================================================================
         * Constructores 
         *========================================================================================================*/ 
        
        /*
         * Descripción:
         *  Constructror de la clase, inicializa las componentes y llama al método de traducción
         *  de los elementos del formulario.
         */
        public FormListFacets(MultiFacetsObs multiFacets, TransLibrary.Language lang)
        {
            InitializeComponent();
            this.traslationElements(lang, Application.StartupPath + LANG_PATH + STRING_TEXT);
            // primero debemos generar la lita de combinaciones sin repetición de las facetas
            ListFacets listFacetsActual = multiFacets.ListFacets();
            this.listOfListFacets = listFacetsActual.CombinationWithoutRepetition();
            this.LoadListCombFacetsInCheckListBox(this.listOfListFacets);
            
        }


        /*
        * Descripción:
        *  Constructror de la clase, inicializa las componentes y llama al método de traducción
        *  de los elementos del formulario.
        */
        public FormListFacets(TransLibrary.Language lang, List<string> listStringFacets)
        {
            InitializeComponent();
            this.traslationElements(lang, Application.StartupPath + LANG_PATH + STRING_TEXT);
            this.LoadListCombFacetsInCheckListBox(listStringFacets);

        }


        /*========================================================================================================
         * Métodos
         *========================================================================================================*/ 
        /*
         * Descripción:
         *  Carga en el CheckListBox la combinaciones sin repeticion de el objeto multifaceta.
         */
        private void LoadListCombFacetsInCheckListBox(List<ListFacets> listOfListFacets)
        {
            
            // ahora agregamos una lista de facetas
            int n = listOfListFacets.Count;
            for (int i = 0; i < n; i++)
            {
                ListFacets lf = listOfListFacets[i];

                string stringListOfFacets = lf.StringOfListFactes();
                // stringListOfFacets = stringListOfFacets + "]";
                // añadimos el string de los niveles de la lista al checkListBox
                this.cListBoxListsFacets.Items.Add(stringListOfFacets);
            }
        }


        /*
         * Descripción:
         *  Carga en el CheckListBox la combinaciones sin repeticion de el objeto multifaceta.
         */
        private void LoadListCombFacetsInCheckListBox(List<string> listOfListFacets)
        {

            // ahora agregamos una lista de facetas
            int n = listOfListFacets.Count;
            for (int i = 0; i < n; i++)
            {
                string lf = listOfListFacets[i];
                this.cListBoxListsFacets.Items.Add(lf);
            }
        }

        /*
         * Descripción
         *  Cierra la ventana
         */
        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /*
         * Descripción:
         *  Cierrea la ventana y muestra las medias de las listas de facetas seleccionadas.
         *  En el caso de que no haya ninguna selecciona. mostrara un mensage de que no ha 
         *  seleccionado ninguna.
         */
        private void btOK_Click(object sender, EventArgs e)
        {
           

        } // end private void btOK_Click(object sender, EventArgs e)

        /* Descripción:
         *  Devuelve una lista con las listas de facetas seleccionadas en el CheckListBox.
         */
        public List<string> SelectedListFacets()
        {
            // bool encontrado = false;
            int n = this.cListBoxListsFacets.Items.Count;
            // int ret = -1;
            List<string> selectedListFacets = new List<string>();

            for (int i = 0; i < n; i++)
            {
                // encontrado = this.cListBoxListsFacets.GetItemChecked(i);
                if (this.cListBoxListsFacets.GetItemChecked(i))
                {
                    selectedListFacets.Add(this.cListBoxListsFacets.Items[i].ToString());
                }
            }
            return selectedListFacets;
        }
        //public List<ListFacets> SelectedListFacets()
        //{
        //    // bool encontrado = false;
        //    int n = this.cListBoxListsFacets.Items.Count;
        //    // int ret = -1;
        //    List<ListFacets> selectedListFacets = new List<ListFacets>();

        //    for (int i = 0; i < n; i++)
        //    {
        //        // encontrado = this.cListBoxListsFacets.GetItemChecked(i);
        //        if (this.cListBoxListsFacets.GetItemChecked(i))
        //        {
        //            selectedListFacets.Add(this.listOfListFacets[i]);
        //        }
        //    }
        //    return selectedListFacets;
        //}

        /* Descripción:
         *  Devuelve el CheckedListBox para que pueda ser consulatado
         */
        public CheckedListBox CheckedListBoxListsFacets()
        {
            return this.cListBoxListsFacets;
        }

        /*
         * Descripción:
         *  Pulsación del boton "Seleccionar todo/Desmarcar todo"
         */
        private void btSelect_Click(object sender, EventArgs e)
        {
            int n = this.cListBoxListsFacets.Items.Count; /* Guardamos el número de elementos para recorrer la 
                                                           * colección del checkedListBox. */
            if (selectOrUchecked)
            {
                // se encuentra en el estado Seleccionar todo
                for (int i = 0; i < n; i++)
                {
                    this.cListBoxListsFacets.SetItemChecked(i, true);
                } 
                this.btSelect.Text = this.txtUnCheckAll;
                this.selectOrUchecked = false;
            }
            else
            {
                // se encuentra en el estado desmarcar todo
                for (int i = 0; i < n; i++)
                {
                    this.cListBoxListsFacets.SetItemChecked(i, false);
                } 
                // this.cListBoxListsFacets.ClearSelected(); // desmarca todos los elementos seleccionados
                this.btSelect.Text = this.txtSelectAll;
                this.selectOrUchecked = true;
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

                // Traducimos el título de la ventana
                name = this.Name.ToString();
                this.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el boton aceptar
                name = this.btOK.Name.ToString();
                this.btOK.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el boton cancelar
                name = this.btCancel.Name.ToString();
                this.btCancel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                /* Traducimos el boton seleccionar/deseleccionar, para ello tenemos que cargar
                 * los textos en las variables txtSelectAll y txtUnchecked all*/
                name = this.btSelect.Name.ToString();
                name = dic.labelTraslation(name).GetTranslation(lang).ToString();

                /* Ahora tenemos que estraer las dos partes del texto que se encuentran separados
                 * por el caracter "/". */
                char[] delimeterChars = { '/' }; // nuestro delimitador será el caracter blanco
                string[] arrayOfTxt = name.Split(delimeterChars);
                if (arrayOfTxt.Length != 2)
                {
                    throw new TransLibrary.LabelTranslationException("Error en el dicionario");
                }

                txtSelectAll = arrayOfTxt[0]; // La primera posición del array contien "Seleccionar todo" 
                                              // en el idioma correspondiente.

                txtUnCheckAll = arrayOfTxt[1]; // La segunda posición del array contiene "Deseleccionar todo"
                                               // en el idioma correspondiente

                // por último ponemos el texto en el btSelect
                this.btSelect.Text = txtSelectAll;

            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " Se produjo un error al traducir " + name);
            } 
        }
        #endregion Traducción de la ventana


    } // public partial class FormListFacets : Form
}// end namespace GUI_TG
