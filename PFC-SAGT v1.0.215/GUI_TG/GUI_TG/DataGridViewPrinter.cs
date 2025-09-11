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
 * Fecha de revisión: 29/Dic/2011
 * 
 * Descripción:
 *  Permite la impresión de un datagridView
 *  Codigo perteneciente a :
 *      Salan Al-Ani
 *      http://www.codeproject.com/KB/printing/datagridviewprinter.aspx
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUI_GT
{
    class DataGridViewPrinter
    {
        /*================================================================================================================
         * Variables
         *================================================================================================================*/

        private DataGridView TheDataGridView;   // El DataGridView Control que va a ser imprimido
        private PrintDocument ThePrintDocument; // El PrintDocument que va a usarse para imprimir
        private bool IsCenterOnPage; // Determina si el informe será imprimido centrado arriba en la página
        private bool IsWithTitle;    // Determina si la página contiene texto de título
        private string TheTitleText; // El título de texto será impreso en cada página  (si IsWithTitle esta puesto a true)
        private Font TheTitleFont;   // La fuente será usada con el texto de título (si IsWithTitle esta puesto a true)
        private Color TheTitleColor; // El color que será usado con el texto de título (si IsWithTitle esta puesto a true)
        private bool IsWithPaging;   // Determina se se emplea páginación

        static int CurrentRow; // Un parámetro estático que mantienen un seguimiento sobre qué fila (en el control DataGridView) que deben imprimirse

        static int PageNumber;

        private int PageWidth;
        private int PageHeight;
        private int LeftMargin;
        private int TopMargin;
        private int RightMargin;
        private int BottomMargin;

        private float CurrentY; // Un parámetro que guardar la pista en la coordenada Y de la página, por lo que el siguiente objeto para ser impreso puede comenzar a partir de la coordenada Y

        private float RowHeaderHeight;
        private List<float> RowsHeight;
        private List<float> ColumnsWidth;
        private float TheDataGridViewWidth;

        // Mantener una lista genérica para celebrar start/stop puntos para la columna impresión
        // Este será utilizados para el envasado en situaciones en las que el DataGridView no caben en una sola página
        private List<int[]> mColumnPoints;
        private List<float> mColumnPointsWidth;
        private int mColumnPoint;

        /*================================================================================================================
         * Constructores
         *================================================================================================================*/

        // El constructor de clase
        public DataGridViewPrinter(DataGridView aDataGridView, PrintDocument aPrintDocument, bool CenterOnPage, bool WithTitle, string aTitleText, Font aTitleFont, Color aTitleColor, bool WithPaging)
        {
            TheDataGridView = aDataGridView;
            ThePrintDocument = aPrintDocument;
            IsCenterOnPage = CenterOnPage;
            IsWithTitle = WithTitle;
            TheTitleText = aTitleText;
            TheTitleFont = aTitleFont;
            TheTitleColor = aTitleColor;
            IsWithPaging = WithPaging;

            PageNumber = 0;

            RowsHeight = new List<float>();
            ColumnsWidth = new List<float>();

            mColumnPoints = new List<int[]>();
            mColumnPointsWidth = new List<float>();

            // El cálculo del PageHeight y PageWidth
            if (!ThePrintDocument.DefaultPageSettings.Landscape)
            {
                PageWidth = ThePrintDocument.DefaultPageSettings.PaperSize.Width;
                PageHeight = ThePrintDocument.DefaultPageSettings.PaperSize.Height;
            }
            else
            {
                PageHeight = ThePrintDocument.DefaultPageSettings.PaperSize.Width;
                PageWidth = ThePrintDocument.DefaultPageSettings.PaperSize.Height;
            }

            // El calculo de los margenes de la página
            LeftMargin = ThePrintDocument.DefaultPageSettings.Margins.Left;
            TopMargin = ThePrintDocument.DefaultPageSettings.Margins.Top;
            RightMargin = ThePrintDocument.DefaultPageSettings.Margins.Right;
            BottomMargin = ThePrintDocument.DefaultPageSettings.Margins.Bottom;

            // En primer lugar, la fila actual que debe imprimirse la primera fila del control DataGridView
            CurrentRow = 0;
        }// end DataGridViewPrinter


        /*================================================================================================================
         * Métodos
         *================================================================================================================*/

        // La función que calcular la altura de cada fila (incluida la fila de encabezado), la anchura de cada columna (según el texto más largo en todas sus células incluyendo la celda de cabecera), y toda la anchura Datagridview
        private void Calculate(Graphics g)
        {
            if (PageNumber == 0) // Solo se calcula una vez
            {
                SizeF tmpSize = new SizeF();
                Font tmpFont;
                float tmpWidth;

                TheDataGridViewWidth = 0;
                for (int i = 0; i < TheDataGridView.Columns.Count; i++)
                {
                    tmpFont = TheDataGridView.ColumnHeadersDefaultCellStyle.Font;
                    if (tmpFont == null) // Si no hay HeaderFont especial estilo, a continuación, utilizar el estilo de fuente predeterminado de Datagridview
                        tmpFont = TheDataGridView.DefaultCellStyle.Font;

                    tmpSize = g.MeasureString(TheDataGridView.Columns[i].HeaderText, tmpFont);
                    tmpWidth = tmpSize.Width;
                    RowHeaderHeight = tmpSize.Height;

                    for (int j = 0; j < TheDataGridView.Rows.Count; j++)
                    {
                        tmpFont = TheDataGridView.Rows[j].DefaultCellStyle.Font;
                        if (tmpFont == null) // Si no hay ninguna especial estilo de fuente del CurrentRow, a continuación, utilice el valor por defecto uno asociado con el control DataGridView
                            tmpFont = TheDataGridView.DefaultCellStyle.Font;

                        tmpSize = g.MeasureString("Anything", tmpFont);
                        RowsHeight.Add(tmpSize.Height);

                        tmpSize = g.MeasureString(TheDataGridView.Rows[j].Cells[i].EditedFormattedValue.ToString(), tmpFont);
                        if (tmpSize.Width > tmpWidth)
                            tmpWidth = tmpSize.Width;
                    }
                    if (TheDataGridView.Columns[i].Visible)
                        TheDataGridViewWidth += tmpWidth;
                    ColumnsWidth.Add(tmpWidth);
                }

                // Definir el start/stop columna puntos basado en el ancho de página y el ancho Datagridview
                // Usaremos este a fin de determinar las columnas que se dibujan en cada una de las páginas y cómo el ajuste será manejada
                // De forma predeterminada, el envoltorio se extravio tales que el número máximo de columnas para una página será determinar
                int k;

                int mStartPoint = 0;
                for (k = 0; k < TheDataGridView.Columns.Count; k++)
                    if (TheDataGridView.Columns[k].Visible)
                    {
                        mStartPoint = k;
                        break;
                    }

                int mEndPoint = TheDataGridView.Columns.Count;
                for (k = TheDataGridView.Columns.Count - 1; k >= 0; k--)
                    if (TheDataGridView.Columns[k].Visible)
                    {
                        mEndPoint = k + 1;
                        break;
                    }

                float mTempWidth = TheDataGridViewWidth;
                float mTempPrintArea = (float)PageWidth - (float)LeftMargin - (float)RightMargin;

                // Sólo nos preocupamos por manipulación en el datagridview anchura total es más grande que la área de impresión
                if (TheDataGridViewWidth > mTempPrintArea)
                {
                    mTempWidth = 0.0F;
                    for (k = 0; k < TheDataGridView.Columns.Count; k++)
                    {
                        if (TheDataGridView.Columns[k].Visible)
                        {
                            mTempWidth += ColumnsWidth[k];
                            // Si el ancho es más grande que el área de la página, a continuación, definir una nueva columna intervalo de impresión
                            if (mTempWidth > mTempPrintArea)
                            {
                                mTempWidth -= ColumnsWidth[k];
                                mColumnPoints.Add(new int[] { mStartPoint, mEndPoint });
                                mColumnPointsWidth.Add(mTempWidth);
                                mStartPoint = k;
                                mTempWidth = ColumnsWidth[k];
                            }
                        }
                        // Nuestro punto final es en realidad un índice por encima del índice actual
                        mEndPoint = k + 1;
                    }
                }
                // Agregar el último conjunto de columnas
                mColumnPoints.Add(new int[] { mStartPoint, mEndPoint });
                mColumnPointsWidth.Add(mTempWidth);
                mColumnPoint = 0;
            }
        }// end Calculate


        // La función de imprimir el título, número de página, y encabezado de fila
        private void DrawHeader(Graphics g)
        {
            CurrentY = (float)TopMargin;

            // Impresión del número de página (si isWithPaging esta puesto a true)
            if (IsWithPaging)
            {
                PageNumber++;
                string PageString = "Page " + PageNumber.ToString();

                StringFormat PageStringFormat = new StringFormat();
                PageStringFormat.Trimming = StringTrimming.Word;
                PageStringFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                PageStringFormat.Alignment = StringAlignment.Far;

                Font PageStringFont = new Font("Tahoma", 8, FontStyle.Regular, GraphicsUnit.Point);

                RectangleF PageStringRectangle = new RectangleF((float)LeftMargin, CurrentY, (float)PageWidth - (float)RightMargin - (float)LeftMargin, g.MeasureString(PageString, PageStringFont).Height);

                g.DrawString(PageString, PageStringFont, new SolidBrush(Color.Black), PageStringRectangle, PageStringFormat);

                CurrentY += g.MeasureString(PageString, PageStringFont).Height;
            }

            // Impresión de título (si IsWithTitle esta puesto a true)
            if (IsWithTitle)
            {
                StringFormat TitleFormat = new StringFormat();
                TitleFormat.Trimming = StringTrimming.Word;
                TitleFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                if (IsCenterOnPage)
                    TitleFormat.Alignment = StringAlignment.Center;
                else
                    TitleFormat.Alignment = StringAlignment.Near;

                RectangleF TitleRectangle = new RectangleF((float)LeftMargin, CurrentY, (float)PageWidth - (float)RightMargin - (float)LeftMargin, g.MeasureString(TheTitleText, TheTitleFont).Height);

                g.DrawString(TheTitleText, TheTitleFont, new SolidBrush(TheTitleColor), TitleRectangle, TitleFormat);

                CurrentY += g.MeasureString(TheTitleText, TheTitleFont).Height;
            }

            // Cálculo de la coordenada x que inicien el proceso de impresión se iniciará desde
            float CurrentX = (float)LeftMargin;
            if (IsCenterOnPage)
                CurrentX += (((float)PageWidth - (float)RightMargin - (float)LeftMargin) - mColumnPointsWidth[mColumnPoint]) / 2.0F;

            // Configurando HeaderFore style
            Color HeaderForeColor = TheDataGridView.ColumnHeadersDefaultCellStyle.ForeColor;
            if (HeaderForeColor.IsEmpty) // Si no hay un especial HeaderFore style, entonces usamos el stilo por defecto del DataGridView
                HeaderForeColor = TheDataGridView.DefaultCellStyle.ForeColor;
            SolidBrush HeaderForeBrush = new SolidBrush(HeaderForeColor);

            // Configurando HeaderBack style
            Color HeaderBackColor = TheDataGridView.ColumnHeadersDefaultCellStyle.BackColor;
            if (HeaderBackColor.IsEmpty) // Si no hay un especial HeaderBack style, entonces usamos el stilo por defecto del DataGridView
                HeaderBackColor = TheDataGridView.DefaultCellStyle.BackColor;
            SolidBrush HeaderBackBrush = new SolidBrush(HeaderBackColor);

            // Ajuste del LinePen que será utilizada para dibujar líneas, rectángulos (derivada de la propiedad GridColor del control DataGridView)
            Pen TheLinePen = new Pen(TheDataGridView.GridColor, 1);

            // Configurando HeaderFont style
            Font HeaderFont = TheDataGridView.ColumnHeadersDefaultCellStyle.Font;
            if (HeaderFont == null) // If there is no special HeaderFont style, then use the default DataGridView font style
                HeaderFont = TheDataGridView.DefaultCellStyle.Font;

            // Calculando y dibujando HeaderBounds        
            RectangleF HeaderBounds = new RectangleF(CurrentX, CurrentY, mColumnPointsWidth[mColumnPoint], RowHeaderHeight);
            g.FillRectangle(HeaderBackBrush, HeaderBounds);

            // Ajuste del formato que se usa para imprimir cada celda del encabezado de fila
            StringFormat CellFormat = new StringFormat();
            CellFormat.Trimming = StringTrimming.Word;
            CellFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;

            // Impresión de cada visible celda de la fila de encabezado
            RectangleF CellBounds;
            float ColumnWidth;
            for (int i = (int)mColumnPoints[mColumnPoint].GetValue(0); i < (int)mColumnPoints[mColumnPoint].GetValue(1); i++)
            {
                if (!TheDataGridView.Columns[i].Visible) continue; // Si la columna no es visible luego ignorar esta iteración

                ColumnWidth = ColumnsWidth[i];

                // Compruebe la alineación Currentcell y aplicarlo al CellFormat
                if (TheDataGridView.ColumnHeadersDefaultCellStyle.Alignment.ToString().Contains("Right"))
                    CellFormat.Alignment = StringAlignment.Far;
                else if (TheDataGridView.ColumnHeadersDefaultCellStyle.Alignment.ToString().Contains("Center"))
                    CellFormat.Alignment = StringAlignment.Center;
                else
                    CellFormat.Alignment = StringAlignment.Near;

                CellBounds = new RectangleF(CurrentX, CurrentY, ColumnWidth, RowHeaderHeight);

                // Imprimimos el texto de celda
                g.DrawString(TheDataGridView.Columns[i].HeaderText, HeaderFont, HeaderForeBrush, CellBounds, CellFormat);

                // La celda límites
                if (TheDataGridView.RowHeadersBorderStyle != DataGridViewHeaderBorderStyle.None) // Dibujar la celda sólo si la frontera HeaderBorderStyle no es ninguno
                    g.DrawRectangle(TheLinePen, CurrentX, CurrentY, ColumnWidth, RowHeaderHeight);

                CurrentX += ColumnWidth;
            }

            CurrentY += RowHeaderHeight;
        }// end DrawHeader


        // La función que se pueden imprimir un montón de filas que caben en una sola página
        // Cuando se devuelve true, significa que hay más filas aún no impresas, así que otra acción PagePrint es necesaria 
        // Cuando se devuelve false, significa que todas las filas están impresas (el parámetro CureentRow alcanza la última fila del control DataGridView) y no se requiere la intervención PagePrint
        private bool DrawRows(Graphics g)
        {
            // La configuración del LinePen será utilizada para dibujar líneas y rectángulos (derivada de la propiedad GridColor del control DataGridView)
            Pen TheLinePen = new Pen(TheDataGridView.GridColor, 1);

            // El estilo parámetros que se utilizan para imprimir cada celda
            Font RowFont;
            Color RowForeColor;
            Color RowBackColor;
            SolidBrush RowForeBrush;
            SolidBrush RowBackBrush;
            SolidBrush RowAlternatingBackBrush;

            // Ajuste del formato que se usa para imprimir cada celda
            StringFormat CellFormat = new StringFormat();
            CellFormat.Trimming = StringTrimming.Word;
            CellFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit;

            // Impresión de cada célula visible
            RectangleF RowBounds;
            float CurrentX;
            float ColumnWidth;
            while (CurrentRow < TheDataGridView.Rows.Count)
            {
                if (TheDataGridView.Rows[CurrentRow].Visible) // Imprimir las celdas del CurrentRow sólo si esa fila es visible
                {
                    // Ajuste de la fila estilo de fuente
                    RowFont = TheDataGridView.Rows[CurrentRow].DefaultCellStyle.Font;
                    if (RowFont == null) // Si no hay ningún  estilo especial de fuente del CurrentRow, a continuación, utilice el valor por defecto uno asociado con el control DataGridView
                        RowFont = TheDataGridView.DefaultCellStyle.Font;

                    // Configuracion del RowFore style
                    RowForeColor = TheDataGridView.Rows[CurrentRow].DefaultCellStyle.ForeColor;
                    if (RowForeColor.IsEmpty) // Si no hay estilo especial RowFore del CurrentRow, a continuación, utilice el valor por defecto uno asociado con el control DataGridView
                        RowForeColor = TheDataGridView.DefaultCellStyle.ForeColor;
                    RowForeBrush = new SolidBrush(RowForeColor);

                    // Setting the RowBack (for even rows) and the RowAlternatingBack (for odd rows) styles
                    // Ajuste del RowBack (incluso para filas) y el RowAlternatingBack (para filas raras) estilos ??
                    RowBackColor = TheDataGridView.Rows[CurrentRow].DefaultCellStyle.BackColor;
                    if (RowBackColor.IsEmpty) // If the there is no special RowBack style of the CurrentRow, then use the default one associated with the DataGridView control
                    {
                        RowBackBrush = new SolidBrush(TheDataGridView.DefaultCellStyle.BackColor);
                        RowAlternatingBackBrush = new SolidBrush(TheDataGridView.AlternatingRowsDefaultCellStyle.BackColor);
                    }
                    else // Si no hay un estilo especial RowBack del CurrentRow, luego utilizarlo tanto para el RowBack y los estilos RowAlternatingBack
                    {
                        RowBackBrush = new SolidBrush(RowBackColor);
                        RowAlternatingBackBrush = new SolidBrush(RowBackColor);
                    }

                    // Calculando el comienzo de la coordenada X donde será el comienzo del proceso de impresión  
                    CurrentX = (float)LeftMargin;
                    if (IsCenterOnPage)
                        CurrentX += (((float)PageWidth - (float)RightMargin - (float)LeftMargin) - mColumnPointsWidth[mColumnPoint]) / 2.0F;

                    // Calcular límites de todo el CurrentRow 
                    RowBounds = new RectangleF(CurrentX, CurrentY, mColumnPointsWidth[mColumnPoint], RowsHeight[CurrentRow]);

                    // Filling the back of the CurrentRow
                    if (CurrentRow % 2 == 0)
                        g.FillRectangle(RowBackBrush, RowBounds);
                    else
                        g.FillRectangle(RowAlternatingBackBrush, RowBounds);

                    // Imprimiendo cada celda visible del CurrentRow                
                    for (int CurrentCell = (int)mColumnPoints[mColumnPoint].GetValue(0); CurrentCell < (int)mColumnPoints[mColumnPoint].GetValue(1); CurrentCell++)
                    {
                        if (!TheDataGridView.Columns[CurrentCell].Visible) continue; // Si la celda pertenece a una columna invisible, entonces ignorar esta iteración

                        // Compruebe la alineación Currentcell y aplicarlo al CellFormat
                        if (TheDataGridView.Columns[CurrentCell].DefaultCellStyle.Alignment.ToString().Contains("Right"))
                            CellFormat.Alignment = StringAlignment.Far;
                        else if (TheDataGridView.Columns[CurrentCell].DefaultCellStyle.Alignment.ToString().Contains("Center"))
                            CellFormat.Alignment = StringAlignment.Center;
                        else
                            CellFormat.Alignment = StringAlignment.Near;

                        ColumnWidth = ColumnsWidth[CurrentCell];
                        RectangleF CellBounds = new RectangleF(CurrentX, CurrentY, ColumnWidth, RowsHeight[CurrentRow]);

                        // Imprime el texto de la celda
                        g.DrawString(TheDataGridView.Rows[CurrentRow].Cells[CurrentCell].EditedFormattedValue.ToString(), RowFont, RowForeBrush, CellBounds, CellFormat);

                        // Dibuja los límites de la celda
                        if (TheDataGridView.CellBorderStyle != DataGridViewCellBorderStyle.None) // Draw the cell border only if the CellBorderStyle is not None
                            g.DrawRectangle(TheLinePen, CurrentX, CurrentY, ColumnWidth, RowsHeight[CurrentRow]);

                        CurrentX += ColumnWidth;
                    }
                    CurrentY += RowsHeight[CurrentRow];

                    // Verificar si el CurrentY se sobrepasa una página boundries
                    // If so then exit the function and returning true meaning another PagePrint action is required
                    // Si es así, entonces salir de la función y devolvemos true en otro llamada a la accion PagePrint ??
                    if ((int)CurrentY > (PageHeight - TopMargin - BottomMargin))
                    {
                        CurrentRow++;
                        return true;
                    }
                }
                CurrentRow++;
            }

            CurrentRow = 0;
            mColumnPoint++; // Continuar imprimiendo el siguiente grupo de columnas

            if (mColumnPoint == mColumnPoints.Count) // Que significa que todas las columnas están impresos
            {
                mColumnPoint = 0;
                return false;
            }
            else
                return true;
        }// end DrawRows


        // El método que llama todas las demás funciones
        public bool DrawDataGridView(Graphics g)
        {
            try
            {
                Calculate(g);
                DrawHeader(g);
                bool bContinue = DrawRows(g);
                return bContinue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Operation failed: " + ex.Message.ToString(), Application.ProductName + " - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


    }// end class DataGridViewPrinter
}// namespace GUI_TG
