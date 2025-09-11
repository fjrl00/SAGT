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
 * Fecha de revisión: 28/May/2012
 * 
 * Descripción:
 *      Muestra los proyectos seleccionables
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConnectLibrary;

namespace GUI_GT
{
    public partial class FormSelectFromDataTable : Form
    {
        /***********************************************************************************************
         * Constantes 
         ***********************************************************************************************/
        // Constantes 
        const string STRING_TEXT = "formSelectFromDataTable.txt";
        const string LANG_PATH = "\\lang\\";

        /***********************************************************************************************
         * Variables
         ***********************************************************************************************/


        /***********************************************************************************************
         * Constructores
         ***********************************************************************************************/
        public FormSelectFromDataTable()
        {
            InitializeComponent();
        }

        public FormSelectFromDataTable(TransLibrary.Language lang, DataTable dt)
            : this()
        {
            dataGridViewSelectData.DataSource = dt;// Cargamos los datos
            if (dt.Rows.Count == 0)
            {
                this.btOk.Enabled = false;
            }
            traslationElements(lang, Application.StartupPath + LANG_PATH + STRING_TEXT); // Traducimos
            
        }

        /***********************************************************************************************
         * Operaciones
         ***********************************************************************************************/

        /* Descripción:
         *  Devuelve la row seleccionada
         */
        public SagtProject SelectProject()
        {
            DataGridViewRow my_Row = this.dataGridViewSelectData.SelectedRows[0];

            int pk = (int)my_Row.Cells["pk_projects"].Value;
            string name = my_Row.Cells["name_projects"].Value.ToString();
            string descriptions = my_Row.Cells["description"].Value.ToString();
            // string sDate = my_Row.Cells["date_project"].Value.ToString();
            int fk_administ = (int)my_Row.Cells["fk_administ"].Value;

            // DateTime date = DateTime.ParseExact(sDate, "dd/MM/yyyy HH:mm:ss", new CultureInfo("es-ES", false));
            DateTime date = (DateTime)my_Row.Cells["date_project"].Value;
            return new SagtProject(pk, name, date, fk_administ, "", descriptions);
        }


        /* Descripicón:
         *  Devuelve el indice de la fila seleccionada
         */
        public int SelectDataTableIndex()
        {
            return this.dataGridViewSelectData.SelectedRows[0].Index;
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

                // Traducimos las cabeceras de las columans
                int numColum = this.dataGridViewSelectData.ColumnCount;
                for (int i = 0; i < numColum; i++)
                {
                    name = this.dataGridViewSelectData.Columns[i].Name;
                    this.dataGridViewSelectData.Columns[name].HeaderText = dic.labelTraslation(name).GetTranslation(lang).ToString();
                }
               
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " " + "Error al traducir:" + " " + name);
            }
        }
        #endregion Traducción de la ventana

    }// end class FormSelectFromDataTable
}// namespace GUI_GT
