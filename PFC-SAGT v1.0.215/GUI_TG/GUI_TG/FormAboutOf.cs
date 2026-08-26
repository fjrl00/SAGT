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
 * Fecha de revisión: 27/Jun/2012      
 * 
 */
using System;
using System.Windows.Forms;

namespace GUI_GT
{
    public partial class FormAboutOf : Form
    {

        /*=================================================================================================
         * Constructores
         *=================================================================================================*/
        // Constructor

        public FormAboutOf()
        {
            InitializeComponent();
        }


        /*
         * Descripción:
         *  Construye la ventana, da valor a la variable version.
         */
        public FormAboutOf(TransLibrary.Language lang, string nameFileTrans, string version)
            : this()
        {
            this.traslationElementsAboutOf(lang, nameFileTrans); // traducimos los textos
            lbVersion.Text += version;
        }


        /*=================================================================================================
         * Métodos
         *=================================================================================================*/

        /*
         * Descripción:
         *  Cambia de idioma los elementos de la ventana.
         * Parámetros:
         *      TransLibrary.Language lang: Idioma al que vamos a traducir los elementos de la ventana.
         */
        private void traslationElementsAboutOf(TransLibrary.Language lang, string nameFileTrans)
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
                // Traducimos la etiqueta del nombre del programa
                name = lbSagtName.Name.ToString();
                lbSagtName.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta versión
                name = lbVersion.Name.ToString();
                lbVersion.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta de alumno
                name = lbAlumName.Name.ToString();
                lbAlumName.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta de director de proyecto
                name = lbProjectDirector.Name.ToString();
                lbProjectDirector.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // traducimos la etiqueta de Asesor Metodológico
                name = lbMethodologicalAdviser.Name.ToString();
                lbMethodologicalAdviser.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el texto
                name = tbComment.Name.ToString();
                tbComment.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el boton aceptar
                name = btAccept.Name.ToString();
                btAccept.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " Se produjo un error al traducir " + name);
            }
        }// end traslationElementsAboutOf


        /*
         * Descripción:
         *  Cierra la ventana FormAboutOf al pulsar el boton Aceptar.
         */
        private void btAccept_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    } // end public partial class FormAboutOf : Form
} // end namespace GUI_TG
