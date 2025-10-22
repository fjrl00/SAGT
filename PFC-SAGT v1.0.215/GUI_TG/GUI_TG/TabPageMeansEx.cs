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
 * Fecha de revisión: 3/Nov/2011  
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using DataGridViewEx;
using AuxMathCalcGT;
using ProjectMeans;
using ConfigCFG;

namespace GUI_GT
{
    public class TabPageMeansEx: TabPage
    {
        /*====================================================================================================
         * Variables de instancia
         *====================================================================================================*/
        DataGridViewEx.DataGridViewEx dgvExTableMean; // DataGridViewEx tabla con las medias
        Label lb_txt_GrandMeans; // Eiqueta con el texto "Gran media en el idioma correspondiente

        // Nombres de la columnas de la tabla de facetas
        private string grandMean = "Gran Media";
        private string nameColMeans = "Medias"; // Nombre de la columna Medias (dependerá del idioma).
        private string nameColVariance = "Varianza"; // Nombre de la columna Varianza (dependerá del idioma).
        private string nameColStd_Dev = "Desviación Típica"; // Nombre de la columna Desviación tipica (dependerá del idioma).

        // Nombre de las columnas adicionales de Tabla de medias de diferencias
        private string nameColDiffMean = "Dif. de medias";
        private string nameColDiffVar = "Dif. de varianzas";
        private string nameColDiffStd_dev = "Dif. des. típica";

        // Nombre de las columnas adicionales de Tabla de medias de puntuación típica.
        private string nameColTypScore = "Puntuación típica";

        // valores
        private double? gMean;
        private double? variance;
        private double? std_dev;

        /*====================================================================================================
         * Constructores
         *====================================================================================================*/

        /* Descripción:
         *  Constructor para la tabla de medias por defecto.
         */
        public TabPageMeansEx(Image blackground, string name, string txtGrandMeans, Point point_label1, string gm,
            string m, string variance, string stdv, string cut, string copy,
            string paste, string remove, string selectAll) :
            base()
        {
            this.BackgroundImage = blackground;
            // Textos
            this.grandMean = gm;
            this.nameColMeans = m;
            this.nameColVariance = variance;
            this.nameColStd_Dev = stdv;
            
            // creamos el tabPage
            this.Text = name;
            this.MinimumSize = new System.Drawing.Size(568, 444);
            this.MaximumSize = new Size(1200, 700);

            // creamos el dataGridViewEx
            dgvExTableMean = new DataGridViewEx.DataGridViewEx();

            dgvExTableMean.Location = new System.Drawing.Point(13, 7);

            // dgvExTableMean.Size = new System.Drawing.Size(500, 400);
            dgvExTableMean.Size = new System.Drawing.Size(this.Width-50, this.Height-100);
            dgvExTableMean.MaximumSize = new Size(1100, 400);

            // newDataGridViewEx.Anchor = this.tabControlMeans.Anchor;// (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right );
            dgvExTableMean.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            dgvExTableMean.Dock = DockStyle.Fill; // esta propiedad permite mostrar el scrollbar

            // Añadimos el dataGridViewEx creado en la lista de datagridViewEx
            // this.listOfDataGridViewEx.Add(newDataGridViewEx);

            dgvExTableMean.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dgvExTableMean.Margin = new Padding(3, 3, 3, 3);

            dgvExTableMean.AllowUserToAddRows = false;

            // dgvExTableMean.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvExTableMean.RowHeadersVisible = false;

            dgvExTableMean.ScrollBars = ScrollBars.Both;
            dgvExTableMean.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // hay que traducir cada uno de los menu contextuales de los dataGridViewEx
            TranslationTContextualMenu(cut, copy, paste, remove, selectAll);

            // Añadimos la etiqueta el texto "Gran Media"
            this.lb_txt_GrandMeans = new Label();
            this.lb_txt_GrandMeans.AutoSize = true;
            this.lb_txt_GrandMeans.Font = new Font("Verdana", 10, FontStyle.Bold);
            this.lb_txt_GrandMeans.MinimumSize = new Size(0, 0);
            this.lb_txt_GrandMeans.MaximumSize = new Size(0, 0);
            // this.lb_txt_GrandMeans.
            this.lb_txt_GrandMeans.Text = txtGrandMeans;
            // this.lb_txt_GrandMeans.Location = new System.Drawing.Point(point_label1.X, point_label1.Y);
            this.lb_txt_GrandMeans.Location = new System.Drawing.Point(point_label1.X, this.Height - 25);
            this.lb_txt_GrandMeans.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);

            // añadimos el dataGridView
            this.Controls.Add(dgvExTableMean);
            this.Controls.Add(lb_txt_GrandMeans);
            // this.Controls.Add(lb_GrandMeans);

            // añadimos el nuevo tabPage
            // loadMeansInDataGridView(tmeans, dgvExTableMean);

            // tabControlMeans.TabPages.Add(newTabPage);
        }// end (constructor 1) public TabPageMeansEx

        /* Descripción:
         *  Constructor para la tabla diferencias de medias.
         *  
         *  DOES THE EXACT SAME THING AS THE OTHER CONSTRUCTOR EXCEPT IT ALSO RECEIVES 
         *  TRANSLATION DATA FOR THE LAST COLUMNS
         */
        public TabPageMeansEx(Image blackground, string name, string txtGrandMeans, Point point_label1, string gm,
            string m, string variance, string stdv, string nameColDiffMean, string nameColDiffVar, 
            string nameColDiffStd_dev, string cut, string copy, string paste, string remove, string selectAll) :
            base()
        {
            this.BackgroundImage = blackground;
            // Textos media por defecto
            this.grandMean = gm;
            this.nameColMeans = m;
            this.nameColVariance = variance;
            this.nameColStd_Dev = stdv;
            // Textos de diferencia de medias
            this.nameColDiffMean = nameColDiffMean;
            this.nameColDiffVar = nameColDiffVar;
            this.nameColDiffStd_dev = nameColDiffStd_dev;

            // creamos el tabPage
            this.Text = name;
            this.MinimumSize = new System.Drawing.Size(568, 444);
            this.MaximumSize = new Size(1200, 700);

            // creamos el dataGridViewEx
            dgvExTableMean = new DataGridViewEx.DataGridViewEx();

            dgvExTableMean.Location = new System.Drawing.Point(13, 7);

            // dgvExTableMean.Size = new System.Drawing.Size(500, 400);
            dgvExTableMean.Size = new System.Drawing.Size(this.Width - 50, this.Height - 100);
            dgvExTableMean.MaximumSize = new Size(1100, 400);

            // newDataGridViewEx.Anchor = this.tabControlMeans.Anchor;// (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right );
            dgvExTableMean.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            dgvExTableMean.Dock = DockStyle.Fill; // esta propiedad permite mostrar el scrollbar

            // Añadimos el dataGridViewEx creado en la lista de datagridViewEx
            // this.listOfDataGridViewEx.Add(newDataGridViewEx);

            dgvExTableMean.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dgvExTableMean.Margin = new Padding(3, 3, 3, 3);

            dgvExTableMean.AllowUserToAddRows = false;

            // dgvExTableMean.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvExTableMean.RowHeadersVisible = false;

            dgvExTableMean.ScrollBars = ScrollBars.Both;
            dgvExTableMean.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // hay que traducir cada uno de los menu contextuales de los dataGridViewEx
            TranslationTContextualMenu(cut, copy, paste, remove, selectAll);

            // Añadimos la etiqueta el texto "Gran Media"
            this.lb_txt_GrandMeans = new Label();
            this.lb_txt_GrandMeans.AutoSize = true;
            this.lb_txt_GrandMeans.Font = new Font("Verdana", 10, FontStyle.Bold);
            this.lb_txt_GrandMeans.MinimumSize = new Size(0, 0);
            this.lb_txt_GrandMeans.MaximumSize = new Size(0, 0);
            // this.lb_txt_GrandMeans.
            this.lb_txt_GrandMeans.Text = txtGrandMeans;
            // this.lb_txt_GrandMeans.Location = new System.Drawing.Point(point_label1.X, point_label1.Y);
            this.lb_txt_GrandMeans.Location = new System.Drawing.Point(point_label1.X, this.Height - 25);
            this.lb_txt_GrandMeans.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);

            // añadimos el dataGridView
            this.Controls.Add(dgvExTableMean);
            this.Controls.Add(lb_txt_GrandMeans);
            // this.Controls.Add(lb_GrandMeans);

            // añadimos el nuevo tabPage
            // loadMeansInDataGridView(tmeans, dgvExTableMean);

            // tabControlMeans.TabPages.Add(newTabPage);
        }// end (constructor 2) public TabPageMeansEx

        /*=============================================================================================
         * Métodos de consulta
         *=============================================================================================*/
        public DataGridViewEx.DataGridViewEx GetDataGridViewEx()
        {
            return this.dgvExTableMean;
        }


        public Label GetLabelGrandMean()
        {
            return this.lb_txt_GrandMeans;
        }


        /*==============================================================================================
         * Métodos de instancia
         *==============================================================================================*/

        /* Descripción:
         *  Introduce los valores en la tabla los datos de la tabla de medias que se pasa como parámetro.
         *  El segundo parámetro sirbe para establecer los valores de configuración.
         * Parámetros:
         *      TableMeans tMeans: Tabla de medias
         *      ConfigCFG.ConfigCFG cfgApli: Parámetros de configuración.
         */
        public void SetTableMeans(InterfaceTableMeans tMeans, ConfigCFG.ConfigCFG cfgApli)
        {
            dgvExTableMean.Rows.Clear();
            int n = tMeans.TableColumns();
            dgvExTableMean.NumeroColumnas = n;
            dgvExTableMean.AllowUserToResizeRows = false;

            // --- Common Header Style ---
            var columnHeaderStyle = new DataGridViewCellStyle
            {
                BackColor = Color.Aqua,
                Font = new Font("Verdana", 9, FontStyle.Bold)
            };
            dgvExTableMean.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            dgvExTableMean.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            int nonFacetCols = 0;
            if (tMeans is TableMeans)
            {
                nonFacetCols = 3;
            }
            else if (tMeans is TableMeansDif)
            {
                nonFacetCols = 6;
            }
            else if (tMeans is TableMeansTypScore)
            {
                nonFacetCols = 5;
            }

            int facetCount = n - nonFacetCols;

            // --- Facet Columns ---
            for (int colu = 0; colu < facetCount; colu++)
            {
                var name = tMeans.ListFacets().FacetInPos(colu).Name();
                var col = dgvExTableMean.Columns[colu];
                col.Name = name;
                col.HeaderText = name;
                col.ReadOnly = true;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // --- Dynamically Add the Final Columns ---
            AddFinalColumns(dgvExTableMean, tMeans, facetCount);

            dgvExTableMean.DefaultCellStyle.Font = new Font("Verdana", 8);

            // --- Fill Rows ---
            int fila = tMeans.TableRows();
            int numOfDecimal = cfgApli.GetNumberOfDecimals();
            string puntoDecimal = cfgApli.GetDecimalSeparator();

            for (int f = 0; f < fila; ++f)
            {
                object[] mi_fila = new object[n + 1];
                for (int c = 0; c < n; c++)
                {
                    if (c < facetCount)
                        mi_fila[c] = tMeans.Data(f, c);
                    else
                        mi_fila[c] = ConvertNum.DecimalToString(tMeans.Data(f, c), numOfDecimal, puntoDecimal);
                }
                dgvExTableMean.Rows.Add(mi_fila);
            }

            // --- Final Stats ---
            this.gMean = tMeans.GrandMean();
            this.variance = tMeans.Variance();
            this.std_dev = tMeans.StdDev();
            SetTextGrandMean(numOfDecimal, puntoDecimal);
        }

        private void AddFinalColumns(DataGridViewEx.DataGridViewEx dgv, InterfaceTableMeans tMeans, int startIndex)
        {
            if (tMeans is TableMeans)
            {
                PropertyColumnTableMeansDGV(dgv, startIndex + 0, nameColMeans, 100);
                PropertyColumnTableMeansDGV(dgv, startIndex + 1, nameColVariance, 100);
                PropertyColumnTableMeansDGV(dgv, startIndex + 2, nameColStd_Dev, 150);
            }
            else if (tMeans is TableMeansDif)
            {
                PropertyColumnTableMeansDGV(dgv, startIndex + 0, nameColMeans, 100);
                PropertyColumnTableMeansDGV(dgv, startIndex + 1, nameColVariance, 100);
                PropertyColumnTableMeansDGV(dgv, startIndex + 2, nameColStd_Dev, 150);
                PropertyColumnTableMeansDGV(dgv, startIndex + 3, nameColDiffMean, 100);
                PropertyColumnTableMeansDGV(dgv, startIndex + 4, nameColDiffVar, 100);
                PropertyColumnTableMeansDGV(dgv, startIndex + 5, nameColDiffStd_dev, 100);
            }
            else if (tMeans is TableMeansTypScore)
            {
                PropertyColumnTableMeansDGV(dgv, startIndex + 0, nameColMeans, 100);
                PropertyColumnTableMeansDGV(dgv, startIndex + 1, nameColVariance, 100);
                PropertyColumnTableMeansDGV(dgv, startIndex + 2, nameColStd_Dev, 150);
                PropertyColumnTableMeansDGV(dgv, startIndex + 3, nameColDiffMean, 100);
                PropertyColumnTableMeansDGV(dgv, startIndex + 4, nameColTypScore, 100);
            }
        }


        /* Descripción:
         *  Construye la linea de la gran media.
         * Parámetros:
         *      int numOfDecimal: número de decimales que se representará el dato. 
         *      string puntoDecimal: caracter que se empleara como punto decimal.
         */
        public void SetTextGrandMean(int numOfDecimal, string puntoDecimal)
        {
            string sGM = ConvertNum.DecimalToString(this.gMean, numOfDecimal, puntoDecimal);
            string sVar = ConvertNum.DecimalToString(this.variance, numOfDecimal, puntoDecimal);
            string sStD = ConvertNum.DecimalToString(this.std_dev, numOfDecimal, puntoDecimal);
            this.lb_txt_GrandMeans.Text = (this.grandMean + ": " + sGM + "       " + this.nameColVariance + ": " + sVar
                + "       " + this.nameColStd_Dev + ": " + sStD);
        }


        /* Descripción:
         *  Asigna las caracteristicas de la columna de un dataGridView
         */
        private static void PropertyColumnTableMeansDGV(DataGridViewEx.DataGridViewEx dgvTableMeans, int pos, string nameCol, int width)
        {
            // int n = dgvTableMeans.Columns.Count;
            // dgvTableMeans.Columns[n - pos].Name = nameCol;
            dgvTableMeans.Columns[pos].HeaderText = nameCol;
            // dgvTableMeans.Columns[pos].Width = width; // asignamos longitud
            dgvTableMeans.Columns[pos].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            // e impedimos que la ultima fila sea ordenable al pulsar la cabecera.
            dgvTableMeans.Columns[pos].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvTableMeans.Columns[pos].ReadOnly = true;
            dgvTableMeans.Columns[pos].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight;
        }


        /*
         * Descripción:
         *  Cambia de idioma el menú contextual de los dataGridVieWEx
         */
        public void TranslationTContextualMenu(string cut, string copy, string paste, string remove, 
            string selectAll)
        {
            this.dgvExTableMean.ContextMenuStrip.Items[0].Text = cut;
            this.dgvExTableMean.ContextMenuStrip.Items[1].Text = copy;
            this.dgvExTableMean.ContextMenuStrip.Items[2].Text = paste;
            this.dgvExTableMean.ContextMenuStrip.Items[3].Text = remove;
            // el siguente item es el separador y por eso nos lo saltamos
            this.dgvExTableMean.ContextMenuStrip.Items[5].Text = selectAll;
        }


        /* Descripción:
         *  Reescribe en función de los nuevos parámetros la línea que indica la gran media, la varianza y
         *  la desviación típica.
         *  
         * Parámetros:
         *      int numOfDecimal: número de decimales se emplea para construir la línea donde aparece la gran 
         *              media, la varianza y la desviación típica de la tabla.
         *      string puntoDecimal: separador decimal (punto o coma) se emplea para construir la línea donde 
         *      aparece la gran media, la varianza y la desviación típica de la tabla.
         *      string gm: texto "Gran media" en el idioma correspondiente
         *      string m: texto "Media" en el idioma correspondiente
         *      string variance: texto "Varianza" en el idioma correspondiente
         *      string stdv: texto "Desv. típica" en el idioma correspondiente
         *      string mDif: texto "Diferencia de media" en el idioma correspondiente
         *      string vDif: texto "Diferencia de varianza" en el idioma correspondiente
         *      string std_devDif: texto "Diferencia de desv. típica" en el idioma correspondiente
         *      string pointT: texto "Puntuación típica" en el idioma correspondiente
         */
        public void TranslateLabel(int numOfDecimal, string puntoDecimal, string gm, string m,
            string variance, string stdv, string diffMeans, string diffVariance, string diff_std_dev, string typScore)
        {
            // Asignamos las traducciónes
            this.grandMean = gm; // gran media
            this.nameColMeans = m; // columna media
            this.nameColVariance = variance; // columna varianza
            this.nameColStd_Dev = stdv; // dolumna desviación típica
            this.nameColDiffMean = diffMeans; // columna diferencia de medias 
            this.nameColDiffVar = diffVariance; // columna diferencia de varianzas
            this.nameColDiffStd_dev = diff_std_dev; // columna de diferencia de desviación típica
            this.nameColTypScore = typScore; // columna de puntuación típica.

            SetTextGrandMean(numOfDecimal, puntoDecimal);
        }

    }// end class TabPageMeansEx
}// end namespace GUI_TG
