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
 * Fecha de revisión: 11/Ago/2011                     
 * 
 * Descripción:
 *      Ventana para añadir los nuevos niveles de significación.
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
    public partial class FormAddSignificanceLevels : Form
    {
        // Fichero con las traduciones
        const string ADD_SIGNIFICANCE_LEVELS_STRINGS = "formAddSignificanceLevels.txt"; //etiquetas del menú Análisis
        const string LANG_PATH = "\\lang\\";
        /*
         * Varibles que contiene textos y etiquetas para la traducción
         */
        string stringName = "Nombre";
        string stringLevel = "Nivel";
        string stringSize = "Tamaño del universo";
        string stringNewLevel = "Nuevo nivel";
        string stringNewSizeOfUniverse = "Nuevo tamaño del universo";


        /* Variables */
        TransLibrary.Language lang;
        TransLibrary.ReadFileTrans dicMeans;
        ListFacets lfInstrumentation;
        ListFacets lfDifferentiation;
         
        public FormAddSignificanceLevels()
        {
            InitializeComponent();
        }

        public FormAddSignificanceLevels(TransLibrary.Language lang, TransLibrary.ReadFileTrans dicMeans, ListFacets lfInstrumentation, ListFacets lfDifferentiation)
        {
            InitializeComponent();
            this.lang = lang;
            this.dicMeans = dicMeans;
            this.lfInstrumentation = lfInstrumentation;
            this.lfDifferentiation = lfDifferentiation;
            traslationElements(this.lang, Application.StartupPath + LANG_PATH + ADD_SIGNIFICANCE_LEVELS_STRINGS);
            // Cargamos los datos de las facetas de diferenciación
            LoadDiffFacetsGridAddSignLevel();
            // Cargamos los datos de las facetas de intrumentación
            LoadInstFacetsGridAddSignLevel();
        }

        /*
         * Descripción:
         *  Cierra la ventana.
         */
        private void btCancel_Click(object sender, EventArgs e)
        {
            // this.Close();
        }

        /* Descripción:
         *  Cargamos los datos de las factas de difereciación en el dataGridViewEx correspondiente.
         */
        private void LoadDiffFacetsGridAddSignLevel()
        {
            DataGridViewEx.DataGridViewEx dgv = this.dgvExDiffFacts;

            dgv.Rows.Clear();
            int numCol = 4;
            // dgv.ColumnCount = numCol;
            dgv.NumeroColumnas = numCol;

            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.AllowUserToResizeColumns = true;

            // Set the column header style.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Aqua;
            columnHeaderStyle.Font = new Font("Verdana", 9, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            dgv.DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Regular);

            // ponemos el texto a todas las cabeceras de las columnas
            //dgv.Columns[0].Name = this.stringName;
            dgv.Columns[0].HeaderText = this.stringName;
            dgv.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns[0].ReadOnly = true;
            dgv.Columns[0].Width = 100;
            // dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //dgv.Columns[1].Name = this.stringLevel;
            dgv.Columns[1].HeaderText = this.stringLevel;
            dgv.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns[1].ReadOnly = true;
            dgv.Columns[1].Width = 100;
            dgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            //dgv.Columns[2].Name = this.stringSize;
            dgv.Columns[2].HeaderText = this.stringSize;
            dgv.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns[2].ReadOnly = true;
            // dgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            dgv.Columns[2].Width = 250;
            dgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
  
            dgv.Columns[3].HeaderText = this.stringNewSizeOfUniverse;
            dgv.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns[3].DefaultCellStyle.BackColor = Color.Gold;
            dgv.Columns[3].ReadOnly = false;
            dgv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;//.ColumnHeader;
            dgv.ColumnaTipoEntrada(3, DataGridViewEx.TipoEntrada.Todos); // Numeros i letras ya que podemos poner inf

            dgv.Columns[0].Width = 100;
            /*
            for (int i = 0; i < 4; i++)
            {
                dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
             */

            int numFacets = this.lfDifferentiation.Count();
            ListFacets lf = this.lfDifferentiation;

            for (int i = 0; i < numFacets; i++)
            {
                object[] my_Row1 = new object[numCol];
                Facet f = lf.FacetInPos(i);
                my_Row1[0] = f.Name();
                my_Row1[1] = f.Level();
                int sizeOfUniverse = f.SizeOfUniverse();
                if (sizeOfUniverse.Equals(int.MaxValue))
                {
                    my_Row1[2] = Facet.INFINITE;
                }
                else
                {
                    my_Row1[2] = f.SizeOfUniverse();
                }
                my_Row1[3] = my_Row1[2].ToString();
                dgv.Rows.Add(my_Row1);
            }
        }

        /*
         * Descripción:
         *  Carga en el dataGrid los datos de las facetas de instrumentación.
         */
        public void LoadInstFacetsGridAddSignLevel()
        {
            DataGridViewEx.DataGridViewEx dgv = this.dgvExInstFacts;

            dgv.Rows.Clear();
            int numCol = 5;
            // dgv.ColumnCount = numCol;
            dgv.NumeroColumnas = numCol;

            // dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Set the column header style.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Aqua;
            columnHeaderStyle.Font = new Font("Verdana", 9, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            dgv.DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Regular);

            // ponemos el texto a todas las cabeceras de las columnas
            //dgv.Columns[0].Name = this.stringName;
            dgv.Columns[0].HeaderText = this.stringName;
            dgv.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns[0].ReadOnly = true;
            dgv.Columns[0].Width = 100;
            //dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //dgv.Columns[1].Name = this.stringLevel;
            dgv.Columns[1].HeaderText = this.stringLevel;
            dgv.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns[1].ReadOnly = true;
            dgv.Columns[1].Width = 100;
            // dgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            //dgv.Columns[2].Name = this.stringSize;
            dgv.Columns[2].HeaderText = this.stringSize;
            dgv.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns[2].ReadOnly = true;
            // dgv.Columns[2].Width = 200;
            dgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //dgv.Columns[3].Name = this.stringNewLevel;
            dgv.Columns[3].HeaderText = this.stringNewLevel;
            dgv.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns[3].DefaultCellStyle.BackColor = Color.Gold;
            dgv.Columns[3].ReadOnly = false;
            //dgv.Columns[3].Width = 200;
            dgv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgv.ColumnaTipoEntrada(3, DataGridViewEx.TipoEntrada.SoloNumeros);
            dgv.Columns[4].HeaderText = this.stringNewSizeOfUniverse;
            dgv.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns[4].DefaultCellStyle.BackColor = Color.Gold;
            dgv.Columns[4].ReadOnly = false;
            dgv.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.ColumnaTipoEntrada(4, DataGridViewEx.TipoEntrada.Todos);

            /*
            for (int i = 0; i < 5; i++)
            {
                dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
             */

            int numFacets = this.lfInstrumentation.Count();
            ListFacets lf = this.lfInstrumentation;

            for (int i = 0; i < numFacets; i++)
            {
                object[] my_Row1 = new object[numCol];
                Facet f = lf.FacetInPos(i);
                my_Row1[0] = f.Name();
                my_Row1[1] = f.Level();
                int sizeOfUniverse = f.SizeOfUniverse();
                if (sizeOfUniverse.Equals(int.MaxValue))
                {
                    my_Row1[2] = Facet.INFINITE;
                }
                else
                {
                    my_Row1[2] = f.SizeOfUniverse();
                }
                my_Row1[3] = my_Row1[1].ToString();
                my_Row1[4] = my_Row1[2].ToString();
                dgv.Rows.Add(my_Row1);
            }
        } // public void LoadDataGridAddSignLevel()

        /*
         * Descripción:
         *  Evalua que los datos este correctos, añade la lista de facetas con los nuevos niveles,
         *  llama al método del formPrincipal: AddG_Parameters y cierra la ventana
         */
        private void btOk_Click(object sender, EventArgs e)
        {
            /*
            int numFacets = this.lfIndepend.Count();
            ListFacets lf = this.lfIndepend;

            bool correct = true;
            try
            {
                for (int i = 0; i < numFacets && correct; i++)
                {
                    DataGridViewRow my_row = this.dgvExAddLevelSign.Rows[i];
                    int n = int.Parse(my_row.Cells[3].Value.ToString());
                    Facet f = lf.FacetInPos(i);
                    if(n>f.SizeOfUniverse())
                    {
                        correct =false;
                        MessageBox.Show("El tamaño de el nivel es mayor que el universo");
                    }else if(n<0){
                        correct = false;
                        MessageBox.Show("El tamaño de el nivel no puede ser negativo");
                    }else{
                        f.Level(n);
                    }
                }
            }
            catch (FormatException formEx)
            {
                // Se produjo la excepción al obtener el nivel de la faceta
                MessageBox.Show(formEx.Message);
                correct = false;
            }

            if (correct)
            {
                // Creamos nuevas facetas que son copias de las anterioeres pero con los nuevos
                // niveles.
                 
                
                ListFacets newLevelListFacets = new ListFacets();
                for (int i = 0; i < numFacets; i++)
                {
                    Facet f = lf.FacetInPos(i);
                    string name = f.Name();
                    // el nuevo nivel se obtiene de la tabla
                    // DataGridViewRow my_row =
                    int numCol = this.dgvExAddLevelSign.Columns.Count - 1;
                    int level = int.Parse(this.dgvExAddLevelSign.Rows[i].Cells[numCol].Value.ToString());
                    int sizeUni = f.SizeOfUniverse();
                    string comment = f.Comment();
                    Facet auxF = new Facet(name,level,comment,sizeUni);
                    newLevelListFacets.Add(auxF);
                }

                this.formPrincipal.AddG_Parameters(newLevelListFacets);
                this.Close();
                
            }// end if
            */
        }// private void btOk_Click(object sender, EventArgs e)

        /* Descripción:
         *  Devuelve el dataGridViewEx con las Facetas de instrumentación.
         */
        public DataGridViewEx.DataGridViewEx DataGridViewExAddInstrumentationLevels()
        {
            return this.dgvExInstFacts;
        }

        /* Descripción:
         *  Devuelve el dataGridViewEx con las Facetas de Diferenciación.
         */
        public DataGridViewEx.DataGridViewEx DataGridViewExAddDifferentiationLevels()
        {
            return this.dgvExDiffFacts;
        }


        #region Traducción de la ventana
        /*======================================================================================
         * Traducción de la ventana:
         *  - Traducción de los textos de la ventana
         *  - Traducción de el menú contextual del dataGrid
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
                // Traducimos las etiquetas
                name = this.lbDiff_Facets.Name.ToString();
                this.lbDiff_Facets.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbInstr_Facets.Name.ToString();
                this.lbInstr_Facets.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el boton aceptar
                name = this.btOk.Name.ToString();
                this.btOk.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el boton cancelar
                name = this.btCancel.Name.ToString();
                this.btCancel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la cabeceras de las columnas
                name = "stringName";
                stringName = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "stringLevel";
                stringLevel = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "stringSize";
                stringSize = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "stringNewLevel";
                stringNewLevel = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = "stringNewSizeOfUniverse";
                stringNewSizeOfUniverse = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducción del menu contextual del datagridViewEx
                this.TranslationTContextualMenu(this.dgvExInstFacts);
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " Se produjo un error al traducir " + name);
            }
        }

        /*
         * Descripción:
         *  Cambia de idioma el menú contextual de los dataGridVieWEx
         */
        public void TranslationTContextualMenu(DataGridViewEx.DataGridViewEx dgvEx)
        {
            TransLibrary.Language lang = this.lang;
            try
            {
                dgvEx.ContextMenuStrip.Items[0].Text = dicMeans.labelTraslation("Cut").GetTranslation(lang).ToString();
                dgvEx.ContextMenuStrip.Items[1].Text = dicMeans.labelTraslation("Copy").GetTranslation(lang).ToString();
                dgvEx.ContextMenuStrip.Items[2].Text = dicMeans.labelTraslation("Paste").GetTranslation(lang).ToString();
                dgvEx.ContextMenuStrip.Items[3].Text = dicMeans.labelTraslation("Remove").GetTranslation(lang).ToString();
                // el siguente item es el separador y por eso nos lo saltamos
                dgvEx.ContextMenuStrip.Items[5].Text = dicMeans.labelTraslation("SelectAll").GetTranslation(lang).ToString();
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " Se produjo un error al traducir el menú contextual");
            }
        }

        #endregion Traducción de la ventana


        #region Eventos sobre el dataGridViewEx

        /*============================================================================================
         * Eventos:
         *  - dgvExInstFacts_KeyPressEditorCelda
         *  - dgvExDiffFacts_KeyPressEditorCelda
         *============================================================================================*/

        /* Descripción:
         *  Evento que se lanza sobre el dataGridViewEx de dgvExInstFacts. Este evento no es compartido
         *  de dataGridView original de Visual Studio.
         *  Si se esta editando el tamaño del universo solo se podrá escribir un número entero o si pusalmos
         *  la tecla 'i' escribimos "INF".
         */
        private void dgvExInstFacts_KeyPressEditorCelda(object sender, KeyPressEventArgs e)
        {
            DataGridViewCell celda = this.dgvExInstFacts.CurrentCell;
            int columnIndex = this.dgvExInstFacts.CurrentCell.ColumnIndex;

            if ((this.dgvExInstFacts.Columns.Count - 1) == columnIndex)
            {
                if (e.KeyChar == 'i' || e.KeyChar == 'I')
                {
                    celda.Value = Facet.INFINITE;// Facet.INFINITE;
                    this.dgvExInstFacts.CurrentCell = this.dgvExInstFacts.Rows[this.dgvExInstFacts.CurrentRow.Index].Cells[0];
                    this.dgvExInstFacts.CurrentCell = this.dgvExInstFacts.Rows[this.dgvExInstFacts.CurrentRow.Index].Cells[columnIndex];
                    e.Handled = false;
                    this.dgvExInstFacts.CurrentCell.Value = Facet.INFINITE;
                }
                else
                {
                    if (e.KeyChar == 8)
                    {
                        e.Handled = false;
                        return;
                    }
                    // Código 48 se coresponde con el cero y el 57 con el nueve
                    if (e.KeyChar >= 48 && e.KeyChar <= 57)
                        e.Handled = false;
                    else
                        e.Handled = true;
                }
            }
        }

        /* Descripción:
         *  Evento que se lanza sobre el dataGridViewEx de dgvExDiffFacts. Este evento no es compartido
         *  de dataGridView original de Visual Studio.
         *  Si se esta editando el tamaño del universo solo se podrá escribir un número entero o si pusalmos
         *  la tecla 'i' escribimos "INF".
         */
        private void dgvExDiffFacts_KeyPressEditorCelda(object sender, KeyPressEventArgs e)
        {
            DataGridViewCell celda = this.dgvExDiffFacts.CurrentCell;
            int columnIndex = this.dgvExDiffFacts.CurrentCell.ColumnIndex;

            if ((this.dgvExDiffFacts.Columns.Count - 1) == columnIndex)
            {
                if (e.KeyChar == 'i' || e.KeyChar == 'I')
                {
                    celda.Value = Facet.INFINITE;// Facet.INFINITE;
                    this.dgvExDiffFacts.CurrentCell = this.dgvExDiffFacts.Rows[this.dgvExDiffFacts.CurrentRow.Index].Cells[0];
                    this.dgvExDiffFacts.CurrentCell = this.dgvExDiffFacts.Rows[this.dgvExDiffFacts.CurrentRow.Index].Cells[columnIndex];
                    e.Handled = false;
                    this.dgvExDiffFacts.CurrentCell.Value = Facet.INFINITE;
                }
                else
                {
                    if (e.KeyChar == 8)
                    {
                        e.Handled = false;
                        return;
                    }
                    // Código 48 se coresponde con el cero y el 57 con el nueve
                    if (e.KeyChar >= 48 && e.KeyChar <= 57)
                        e.Handled = false;
                    else
                        e.Handled = true;
                }
            }
        }

        #endregion Eventos sobre el dataGridViewEx

    } // public partial class FormAddSignificanceLevels : Form
} // namespace GUI_TG
