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
 * Fecha de revisión: 06/Feb/2012                           
 * 
 * Descripción:
 *      Clase parcial ("partial") del FormPrincipal. Contiene los métodos referentes a la parte de
 *      generación de informes.
 */

using MultiFacetData;
using ProjectMeans;
using ImportEduGMeans;
using Sagt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GUI_GT
{
    public partial class FormPrincipal : Form
    {
        /*=================================================================================
         * Constantes
         *=================================================================================*/

        // Constantes
        const string FILE_STRING_REPORT = "reports.txt"; // fichero que contiene las traducciónes del informe
        const string SAGT = "SAGT ";
        const string NAME_STUDENT = "Francisco Jesús Ramos Pérez";
        const string NAME_PROJECT_DIRECTOR = "Dr. Don Antonio Hernández Mendo";
        const string NAME_ACADEMIC_DIRECTOR = "Dr. Don José Luis Pastrana Brincones";
        const string UNIV_UMA = "Universidad de Málaga";

        /*=================================================================================
         * Variables
         *=================================================================================*/

        // Nombre de los informes
        string nameSagtReport = "Informe SAGT";
        
        // Variable (variará en función del idioma)
        string pag = "Página";
        string developer = "Desarrollado por";
        string projectDirector = "Director de proyecto";
        string academicDirector = "Director académico";

        // Titulos de los informes
        string issuanceOfReport = "Emisión de informe";
        string titleDataReport = "Informe de tabla de frecuencias";
        string titleMeansReport = "Informe de tablas de medias";
        string titleSsqReport = "Informe de análisis de varianzas";
        string titleG_ParametersReport = "G Parámetros";

        // Tabla
        string stringTable = "Tabla";

        // Variables booleanas
        bool bData = false; // indica si se imprimirá el informe de la tabla de frecuencias
        bool bMeans = false; // indica si se imprimirá el informe de la lista de tabla de medias
        bool bSsq = false; // indica si se imprmimirá el informe de las tablas de análisis de varianza

        bool isFinishTableFacet = false;// indica si se ha terminado con la tabla de facetas
        bool isFinishTableObs = false; // indica si se ha terminado con la tabla de observaciones
        bool isFinishTableMean = false; // indica si se ha terminado de imprimir la presente tabla de medias
        bool isFinishTableSsq = false; // indica si se ha terminado de imprimir la tabla de suma de cuadrados
        bool isFinishTableG_Parameters = false; // indica si se ha terminado de imprimir la tabla de G-Parámetros
        bool isFirstPageObsTable = false; // indica si se esta imprimiendo la primera pagina de la tabla 
                                          // la tabla de observaciones. Esto es para calcular el ancho de la tabla
        bool bFirstPageReportMeans = false; // Indica si es la primera página del informe de medias.
        bool bFirstPageReportSsq = false; // Indica si es la primera página del informe de suma de cuadrados
        bool bFirstPageG_Parameters = false; // Indica si es la primera página de la tabla G_Parámetros
        bool bFirsPageOfCommentMeans = false;
        bool bFirsPageOfCommentData = false;
        bool bFirsPageOfCommentSSq = false;
        bool isPrintNameTable = false; // indica si se ha imprimido el nombre de la tabla
        bool isPrintLineDesign = false; // indica si se ha imprimido la linea de diseño de medida

        int iPosListMeans = 0; // Posición de la lista de medias
        int iTopMargin = 0; // Margen superior de escritura

        // array de lineas de texto
        ArrayList arrStringLine = new ArrayList();
        int iLine = 0;


        #region Member Variables (para la construcción de la página)

        StringFormat strFormat; //Used to format the grid rows.

        ArrayList arrColumnLefts = new ArrayList();//Used to save left coordinates of columns
        ArrayList arrColumnWidths = new ArrayList();//Used to save column widths
        int iCellHeight = 0; //Used to get/set the datagridview cell height
        int iTotalWidth = 0; //
        int iRow = 0; //Used as counter
        bool bFirstPage = false; //Used to check whether we are printing first page
        bool bNewPage = false;// Used to check whether we are printing a new page
        int iHeaderHeight = 0; //Used for the header height

        int numPag = 0;// Número de página

        #endregion



        #region Print Button Click Event
        /// <summary>
        /// Handles the print button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiDataPrinter_Click(object sender, EventArgs e)
        {
            bData = !(sagtElements.GetMultiFacetsObs() == null);
            bMeans = !(sagtElements.GetListMeans() == null);
            bSsq = !(sagtElements.GetAnalysis_and_G_Study() == null);

            FormSelectReports formSelectReports = new FormSelectReports(this.LanguageActually(), bData, bMeans, bSsq);

            bool salir = false;
            do
            {
                DialogResult res = formSelectReports.ShowDialog();
                switch (res)
                {
                    case DialogResult.Cancel: salir = true; break;
                    case DialogResult.OK:
                        // comprobamos las pestañas marcadas
                        bData = formSelectReports.IsDataSelected();
                        bMeans = formSelectReports.IsMeansSelected();
                        bSsq = formSelectReports.IsSsqSelected();

                        if (bData || bMeans || bSsq)
                        {// (* 1 *)
                            //Open the print dialog
                            PrintDialog printDialog = new PrintDialog();
                            printDialog.Document = printSagtDocument;
                            printDialog.UseEXDialog = true;

                            //Get the document
                            if (DialogResult.OK == printDialog.ShowDialog())
                            {
                                printSagtDocument.DocumentName = nameSagtReport;
                                printSagtDocument.Print();
                            }

                            salir = true;
                        }
                        else
                        {
                            // No hay ningún elemento seleccionado, mostramos un mensaje
                            // no hay ningún elemento seleccionado.
                            ShowMessageErrorOK(txtMessageNoSelected, "", MessageBoxIcon.Stop);
                        }// end if (* 1 *)

                        break;
                }
            } while (!salir);
        }// end tsmiDataPrinter_Click

        #endregion Print Button Click Event



        #region Begin Print Event Handler
        /// <summary>
        /// Handles the begin print event of print document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printSagtDocument_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                traslationElementsReports(this.cfgApli.GetConfigLanguage(),
                    Application.StartupPath + LANG_PATH + FILE_STRING_REPORT);

                strFormat = new StringFormat();
                strFormat.Alignment = StringAlignment.Near;
                strFormat.LineAlignment = StringAlignment.Center;
                strFormat.Trimming = StringTrimming.EllipsisCharacter;

                /* Inicializamos las variables */
                iPosListMeans = 0;
                arrColumnLefts.Clear();
                arrColumnWidths.Clear();
                arrStringLine.Clear(); // inicializamos el array de lineas de texto
                iLine = 0; // inicializamos el indice de lineas de texto
                iCellHeight = 0;
                iRow = 0;
                numPag = 0;
                bFirstPage = true; // primera página
                bNewPage = true; // nueva página
                bFirstPageReportMeans = true; // primera página del informe de medias
                bFirstPageReportSsq = false; // primera página del informe de suma de cuadrados
                bFirsPageOfCommentMeans = true;
                bFirsPageOfCommentData = true;
                bFirsPageOfCommentSSq = true;

                /* Quizas debería poner una variable booleana para señalar que aun no se ha impreso la tabla de faceta
                 * y de observaciones. */
                isFirstPageObsTable = true;
                bFirstPageG_Parameters = true;
                isFinishTableFacet = false;
                isFinishTableObs = false;
                isFinishTableSsq = false;
                isFinishTableG_Parameters = false;
                isFinishTableOptResume = false;
                isPrintLineDesign = false;

                // Calculating Total Widths
                // Calculando ancho total
                // iTotalWidth = CaulatingTotalWidths(dataGridViewExFacets);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }// end printSagtDocument_BeginPrint
        

        /* Descripción:
         *  Método auxiliar que devuelve la anchura total de la tabla que se pasa como parámetro
         */
        private int CaulatingTotalWidths(DataGridViewEx.DataGridViewEx dgvEx)
        {
            int iTotalWidth = 0; // valor de retorno
            foreach (DataGridViewColumn dgvGridCol in dgvEx.Columns)
            {
                iTotalWidth += dgvGridCol.Width;
            }
            return iTotalWidth;
        }

        #endregion Begin Print Event Handler



        #region Print Page Event
        /// <summary>
        /// Handles the print page event of print document
        /// Controla el comienzo de imprimir documento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printSagtDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                // Establecemos el margen superior
                iTopMargin = e.MarginBounds.Top;
                // Si hay más páginas tiene que imprimir o no
                bool bMorePagesToPrint = false; // inicialmente no tienen porque imprimirse nuevas páginas
                // Si el valor recuperado de la configuración es true se sobrearan alternativamente las filas
                bool bShadingRows = this.cfgApli.GetShadingRows();


                // imprimimos la cabecera
                HeaderPageReport(e, fontFootersAndHeaders);

                // imprimimos el pie de página
                FootnoteOfTheReport(e, fontFootersAndHeaders);
                //************************************************************************************

                // Fuente que se empleara en el resto del informe para las tablas
                int n = this.cfgApli.GetTableFontSize();
                Font tablefontReport = new Font(this.cfgApli.GetTableFontFamily(), n, FontStyle.Bold);

                // Fuente que se empleara en el resto del informe para los textos
                n = this.cfgApli.GetTextFontSize();
                Font textfontReport = new Font(this.cfgApli.GetTextFontFamily(), n, FontStyle.Bold);

                //For the first page to print set the cell width and header height ??
                // Para la primera página para ajustar el ancho de las celdas de encabezado y altura
                if (bFirstPage)
                {// (* 1 *)

                    /*************************************************************************************************/

                    //Dibujamos cabecera del informe que solo aparecerá en la primerá página del informe
                    e.Graphics.DrawString(issuanceOfReport, textfontReport, Brushes.Black, e.MarginBounds.Left,
                        e.MarginBounds.Top - e.Graphics.MeasureString(issuanceOfReport, textfontReport, e.MarginBounds.Width).Height);

                    String strDate = (DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString()).Trim();
                    //Dibujamos fecha
                    e.Graphics.DrawString(strDate, textfontReport, Brushes.Black, e.MarginBounds.Left +
                        (e.MarginBounds.Width - e.Graphics.MeasureString(strDate, textfontReport, e.MarginBounds.Width).Width),
                        e.MarginBounds.Top - e.Graphics.MeasureString(issuanceOfReport, textfontReport, e.MarginBounds.Width).Height);
                    // actualizamos la cabecera del informe
                    iTopMargin += tablefontReport.Height + 5;
                    /***********************************************************************************************/

                }// end if (* 1 *)

                // Impresión del Informe de tabla de frecuencias
                if (bData)
                {// (* 2 *)
                    // is es true imprimimos la tabla de facetas
                    if (!isFinishTableFacet)
                    {// (* 3 *)
                        // Establecemos la anchura de la tabla
                        if (bFirstPage)
                        {// (* 4 *)
                            // Dibujamos cabecera del informe que solo aparecerá en la primera página del informe
                            string text = this.lbFileData.Text + ": " + this.tbFileName.Text; // path del fichero
                            text = text + "\n" + this.lbCommentsData.Text + ": " + this.tbDescription.Text; // Descripción
                            iLine = 0;
                            arrStringLine = ArrayOfStrings(text, textfontReport, e);
                            bNewPage = false;
                            dgvExAuxReport = CreateNewDataGridView(dataGridViewExFacets, tablefontReport, tablefontReport);
                            // Calculando ancho total
                            iTotalWidth = CaulatingTotalWidths(dgvExAuxReport);
                            SettingHeadersCell(e, dgvExAuxReport);
                        }// end if (* 4 *)
                        bMorePagesToPrint = PrintComment(e, arrStringLine, textfontReport, bMorePagesToPrint);
                        bMorePagesToPrint = PrintDataGridViewEx(e, dgvExAuxReport, tablefontReport, bShadingRows, ref isFinishTableFacet);
                        isFirstPageObsTable = true;
                    }// end if (* 3 *)


                    // Imprimimos la tabla de frecuencias;
                    if (isFinishTableFacet)
                    {// (* 5 *)
                        if (isFirstPageObsTable)
                        {// (* 6 *)
                            arrColumnLefts.Clear();
                            arrColumnWidths.Clear();
                            iCellHeight = 0;
                            iRow = 0;
                            bFirstPage = true; // primera página

                            dgvExAuxReport = CreateNewDataGridView(dataGridViewExObsTable, tablefontReport, tablefontReport);
                            // Calculando ancho total
                            iTotalWidth = CaulatingTotalWidths(dgvExAuxReport);
                            // Establecemos la anchura de la tabla
                            SettingHeadersCell(e, dgvExAuxReport);

                            // bNewPage = true;
                        }// end if (* 6 *)

                        iTopMargin += 20;
                        // Necesito calcular el margen superior a partir del cual se escibe la siguiente tabla.
                        bMorePagesToPrint = PrintDataGridViewEx(e, dgvExAuxReport, tablefontReport, bShadingRows, ref isFinishTableObs);
                        /* Si hemos finalifado la impresión de la tabla de observaciones entonces hemos finaliza
                         * el primer informe */
                        if (isFinishTableObs)
                        {
                            // imprimos el texto de comentarios
                            if (bFirsPageOfCommentData)
                            {
                                iLine = 0;
                                arrStringLine = ArrayOfStrings(richTextBoxDataComment.Text, textfontReport, e);
                                bFirsPageOfCommentData = false;
                            }
                            bMorePagesToPrint = PrintComment(e, arrStringLine, textfontReport, bMorePagesToPrint);

                            if (iLine >= arrStringLine.Count)
                            {
                                bData = false; 
                            }
                            
                        }

                    }// end if  (* 5 *)

                }// end if (* 2 *)


                // Impresión de la lista de tabla de medias
                if (bMeans && !bData)
                {// (* 1 *)
                    TabPageMeansEx tabPageMeansEx = null;
                    // Tiene que se menos dos ya que el ultimo tabPage es el de información
                    while ((iPosListMeans < tabControlMeans.TabPages.Count - 1) 
                        && (iTopMargin < e.MarginBounds.Bottom && !bMorePagesToPrint))
                    {
                        tabPageMeansEx = (TabPageMeansEx)tabControlMeans.TabPages[iPosListMeans];
                        
                        if (bNewPage)
                        {
                            iTopMargin = e.MarginBounds.Top;
                        }
                        else
                        {
                            iTopMargin += 20;
                        }

                        // Cabecera del informe de medias
                        if (bFirstPageReportMeans)
                        {
                            e.Graphics.DrawString(titleMeansReport, textfontReport, Brushes.Black, e.MarginBounds.Left, iTopMargin);
                            bFirstPageReportMeans = false;
                            arrColumnLefts.Clear();
                            arrColumnWidths.Clear();
                            iCellHeight = 0;
                            iRow = 0;
                        }

                        // imprimos el nombre de la tabla
                        if (!isPrintNameTable)
                        {
                            iTopMargin += textfontReport.Height+5;
                            bMorePagesToPrint = PrintLineText(e, stringTable + ": " + tabPageMeansEx.Text, textfontReport, ref isPrintNameTable);
                        }
                        if (isPrintNameTable)
                        {
                            DataGridViewEx.DataGridViewEx dgvExTableMean = tabPageMeansEx.GetDataGridViewEx();
                            dgvExAuxReport = CreateNewDataGridView(dgvExTableMean, tablefontReport, tablefontReport);
                            // Calculando ancho total
                            iTotalWidth = CaulatingTotalWidths(dgvExAuxReport);
                            // Establecemos la anchura de la tabla
                            SettingHeadersCell(e, dgvExAuxReport);
                            iTopMargin += 20;
                            bMorePagesToPrint = PrintDataGridViewEx(e, dgvExAuxReport, tablefontReport, bShadingRows, ref isFinishTableMean);
                            if (isFinishTableMean)
                            {
                                bool isPrintLine = false;
                                iTopMargin += 20;
                                float alturaLetra = tablefontReport.GetHeight();
                                // e.Graphics.DrawString(tabPageMeansEx.GetLabelGrandMean().Text, fontReport, Brushes.Black, e.MarginBounds.Left, iTopMargin);
                                bMorePagesToPrint = PrintLineText(e, tabPageMeansEx.GetLabelGrandMean().Text, tablefontReport, ref isPrintLine);
                                if (isPrintLine)
                                {
                                    isFinishTableMean = false;
                                    isPrintNameTable = false;
                                    iPosListMeans++;
                                    arrColumnLefts.Clear();
                                    arrColumnWidths.Clear();
                                    iCellHeight = 0;
                                    iRow = 0;
                                }
                            }
                        }
                        
                    }// end while

                    // Imprimos la pagina de información
                    if (!(iPosListMeans < tabControlMeans.TabPages.Count - 1))
                    {
                        // imprimos el texto de comentarios
                        if (bFirsPageOfCommentMeans)
                        {
                            string text = "\n" + lbFileMeanProvede.Text + ": " + tbFileMeanProvede.Text;
                            text = text + "\n" + lbDateMeanCreated.Text + ": " + tbDateMeanCreated.Text;
                            text = text + "\n" + rTxtBoxMeanInfo.Text;
                            iLine = 0;
                            arrStringLine = ArrayOfStrings(text, textfontReport, e);
                            bFirsPageOfCommentMeans = false;
                        }

                        bMorePagesToPrint = PrintComment(e, arrStringLine, textfontReport, bMorePagesToPrint);
                        if (iLine >= arrStringLine.Count)
                        {
                            bMeans = false;
                        }
                    }
                }// end if // (* 1 *)


                // Imprimimos el informe de Suma de cuadrados
                if(bSsq && !bData && !bMeans)
                {// (* 1 *)
                    if (bNewPage)
                    {
                        iTopMargin = e.MarginBounds.Top;
                    }
                    else
                    {
                        iTopMargin += 20;
                    }
                    // Imprimimos la cabecera del informe
                    if (bFirstPageReportSsq)
                    {
                        // titleSsqReport
                        bMorePagesToPrint = PrintLineText(e, titleSsqReport, textfontReport, ref bFirstPageReportSsq);
                    }
                    if (!isPrintLineDesign)
                    {
                        string lineStringDesign = lbMeasurementDesign.Text + " " + tbMeasurementDesign.Text;
                        bMorePagesToPrint = PrintLineText(e, lineStringDesign, textfontReport, ref isPrintLineDesign);
                        arrColumnLefts.Clear();
                        arrColumnWidths.Clear();
                        iCellHeight = 0;
                        iRow = 0;
                        dgvExAuxReport = CreateNewDataGridView(dataGridViewExSSQ, tablefontReport, tablefontReport);
                        // Calculando ancho total
                        iTotalWidth = CaulatingTotalWidths(dgvExAuxReport);
                        SettingHeadersCell(e, dgvExAuxReport);
                    }
                    if (!isFinishTableSsq)
                    {
                        iTopMargin += 20;
                        // Necesito calcular el margen superior a partir del cual se escibe la siguiente tabla.
                        bMorePagesToPrint = PrintDataGridViewEx(e, dgvExAuxReport, tablefontReport, bShadingRows, ref isFinishTableSsq);
                        isFinishTableSsq = iRow >= dgvExAuxReport.RowCount && !bMorePagesToPrint;
                                                    
                    }
                    // Imprimos la tabla de g_Parametros
                    if (isFinishTableSsq && !isFinishTableG_Parameters)
                    {
                        if (bFirstPageG_Parameters)
                        {
                            bMorePagesToPrint = PrintLineText(e, this.tabPageG_Parameters.Text, textfontReport, ref bFirstPageG_Parameters);
                            bFirstPageG_Parameters = !bFirstPageG_Parameters;
                            arrColumnLefts.Clear();
                            arrColumnWidths.Clear();
                            iCellHeight = 0;
                            iRow = 0;
                            dgvExAuxReport = CreateNewDataGridView(dGridViewExG_Parameters, tablefontReport, tablefontReport);
                            // Calculando ancho total
                            iTotalWidth = CaulatingTotalWidths(dgvExAuxReport);
                            SettingHeadersCell(e, dgvExAuxReport);
                        }
                        if (!bNewPage)
                        {
                            iTopMargin += 20;
                        }
                        // Necesito calcular el margen superior a partir del cual se escibe la siguiente tabla.
                        bMorePagesToPrint = PrintDataGridViewEx(e, dgvExAuxReport, tablefontReport, bShadingRows, ref isFinishTableG_Parameters);
                        bNewTable = isFinishTableG_Parameters;
                    }

                    // Imprimimos la tabla de resumen
                    if (isFinishTableSsq && isFinishTableG_Parameters && !isFinishTableOptResume)
                    {
                        if (bNewTable)
                        {
                            if (bNewPage)
                            {
                                iTopMargin = e.MarginBounds.Top;
                            }
                            else
                            {
                                iTopMargin += 20;
                            }
                            arrColumnLefts.Clear();
                            arrColumnWidths.Clear();
                            iCellHeight = 0;
                            iRow = 0;

                            dgvExAuxReport = CreateNewDataGridView(dGridViewExOptimizationResum, tablefontReport, tablefontReport);

                            // Calculando ancho total
                            if (dgvExAuxReport.ColumnCount > 4)
                            {
                                // iTotalWidth = CaulatingTotalWidths(dgvAnalysisResumOpt);
                                iTotalWidth = CaulatingTotalWidths(dgvExAuxReport);
                            }
                            // SettingHeadersCell(e, dgvAnalysisResumOpt);
                            SettingHeadersCell(e, dgvExAuxReport);
                        }
                        // bMorePagesToPrint = PrintDataGridViewEx(e, dgvAnalysisResumOpt, fontReport, bShadingRows, ref isFinishTableOptResume);
                        bMorePagesToPrint = PrintDataGridViewEx(e, dgvExAuxReport, tablefontReport, bShadingRows, ref isFinishTableOptResume);
                        bNewTable = isFinishTableOptResume;
                        bNewPage = false;
                    }
                    if (isFinishTableSsq && isFinishTableG_Parameters && isFinishTableOptResume)
                    {
                        if (bFirsPageOfCommentSSq)
                        {
                            string text = "\n" + lbNameFileSsqInfo.Text + ": " + tbNameFileSsqInfo.Text;
                            text = text + "\n" + lbDateFileSsqInfo.Text + ": " + tbDateFileSsqInfo.Text;
                            text = text + "\n" + richTextBoxSsqComment.Text;
                            iLine = 0;
                            arrStringLine = ArrayOfStrings(text, textfontReport, e);
                            bFirsPageOfCommentSSq = false;
                        }

                        bMorePagesToPrint = PrintComment(e, arrStringLine, textfontReport, bMorePagesToPrint);
                    }
                }// end if (* 1 *)

                //Si quedan más líneas dibujamos otra página.
                if (bMorePagesToPrint)
                {
                    e.HasMorePages = true;
                }
                else
                {
                    e.HasMorePages = false;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }// end printSagtDocument_PrintPage


        /* Descripción:
         *  Realiza ajustes de las celdas de encabezado de una tabla que se pasa como parámetro.
         */
        private void SettingHeadersCell(System.Drawing.Printing.PrintPageEventArgs e, 
            DataGridViewEx.DataGridViewEx dgvEx)
        {
            // Establecemos el margen izquierdo
            int iLeftMargin = e.MarginBounds.Left;
            // Establecemos el margen superior
            int iTopMargin = e.MarginBounds.Top;
            int iTmpWidth = 0;

            foreach (DataGridViewColumn GridCol in dgvEx.Columns)
            {
                iTmpWidth = (int)(Math.Floor((double)((double)GridCol.Width /
                               (double)iTotalWidth * (double)iTotalWidth *
                               ((double)e.MarginBounds.Width / (double)iTotalWidth))));


                iHeaderHeight = (int)(e.Graphics.MeasureString(GridCol.HeaderText,
                            GridCol.InheritedStyle.Font, iTmpWidth).Height) + 11; 

                // Save width and height of headers
                // Guardar anchura y altura de cabecera
                arrColumnLefts.Add(iLeftMargin);
                arrColumnWidths.Add(iTmpWidth);
                iLeftMargin += iTmpWidth;
            }
        }// end SettingHeadersCell


        /* Descripcíon:
         *  Impresión del dataGrid en el informe.
         *  Devuelve si true si se necesitan más páginas para imprimir el datagrid. False en otro caso.
         * Parámetros:
         *      System.Drawing.Printing.PrintPageEventArgs e: objeto que se desea imprimir.
         *      DataGridViewEx.DataGridViewEx dataGridViewEx: tabla que se esta imprimiendo.
         *      ref bool isFinishTable: indica si se ha terminado de imprimir la tabla actual.
         */
        private bool PrintDataGridViewEx(System.Drawing.Printing.PrintPageEventArgs e, 
            DataGridViewEx.DataGridViewEx dataGridViewEx, Font fontReport, bool shadingRows, ref bool isFinishTable)
        {
            bool bMorePagesToPrint = false;
            bool isNewTable = true;

            // Loop till all the grid rows not get printed
            // Bucle hasta que todas las filas de la tabla no sean impresas
            while (iRow <= dataGridViewEx.Rows.Count - 1)
            {
                DataGridViewRow GridRow = dataGridViewEx.Rows[iRow];
                // Set the cell height
                // Ajustamos la altura de la celda
                iCellHeight = GridRow.Height + 5;

                int iCount = 0;
                // Check whether the current page settings allo more rows to print
                // Comprueba si la configuración permite imprimir la pagina completa
                if (iTopMargin + iCellHeight >= e.MarginBounds.Height + e.MarginBounds.Top)
                {
                    bNewPage = true;
                    bFirstPage = false;
                    bMorePagesToPrint = true;
                    break;
                }
                else
                {
                    if (bNewPage)
                    {
                        // Si es una nueva página recupermos el margen superior
                        iTopMargin = e.MarginBounds.Top + 20;
                        bNewPage = false;
                    }

                    // Si es una nueva tabla imprimimos el encabezado
                    if(isNewTable)
                    {
                        // Comprobamos si cabe el encabezado
                        if ((iTopMargin + iHeaderHeight) > (e.MarginBounds.Height + e.MarginBounds.Top))
                        {
                            return true;
                        }
                        isNewTable = false;
                        //Dibujamos las columnas              
                        foreach (DataGridViewColumn GridCol in dataGridViewEx.Columns)
                        {
                            // Dibuja el fondo gris del encabezado (Color.LightGray)
                            e.Graphics.FillRectangle(new SolidBrush(Color.Silver),
                                new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                (int)arrColumnWidths[iCount], iHeaderHeight));

                            e.Graphics.DrawRectangle(Pens.Black,
                                new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                (int)arrColumnWidths[iCount], iHeaderHeight));

                            // e.Graphics.DrawString(GridCol.HeaderText, GridCol.InheritedStyle.Font,
                            e.Graphics.DrawString(GridCol.HeaderText, fontReport,
                                new SolidBrush(GridCol.InheritedStyle.ForeColor),
                                new RectangleF((int)arrColumnLefts[iCount], iTopMargin,
                                (int)arrColumnWidths[iCount], iHeaderHeight), strFormat);
                            iCount++;
                        }// end foreach
                        bNewPage = false;
                        isFirstPageObsTable = false;
                        iTopMargin += iHeaderHeight;
                    }
                    /************************************************************/
                    
                    iCount = 0;
                    // Dibujamos el contenido de las columnas                
                    foreach (DataGridViewCell Cel in GridRow.Cells)
                    {
                        // Se sombrearan las filas alternativamente si shadingRows es true
                        if (shadingRows && (iRow % 2 == 1))
                        {
                            // Dibuja el fondo gris (Color.LightGray)
                            e.Graphics.FillRectangle(new SolidBrush(Color.WhiteSmoke),
                                new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iCellHeight));
                        }
                        if (Cel.Value != null)
                        {
                            RectangleF CellBounds = new RectangleF((int)arrColumnLefts[iCount], (float)iTopMargin,
                                        (int)arrColumnWidths[iCount], (float)iCellHeight);


                            // e.Graphics.DrawString(Cel.Value.ToString(), Cel.InheritedStyle.Font,
                            e.Graphics.DrawString(Cel.Value.ToString(), fontReport,
                                        new SolidBrush(Cel.InheritedStyle.ForeColor),
                                        CellBounds, strFormat);
                        }
                        // Dibujamos el borde de las celdas
                        e.Graphics.DrawRectangle(Pens.Black, new Rectangle((int)arrColumnLefts[iCount],
                                iTopMargin, (int)arrColumnWidths[iCount], iCellHeight));
                        iCount++;
                    }// end foreach
                }
                iRow++;
                iTopMargin += iCellHeight;
            }// end while

            if (iRow == dataGridViewEx.Rows.Count)
            {
                isFinishTable = true;
            }

            return bMorePagesToPrint;
        }// end PrintDataGridViewEx


        /* Descripción:
         *  Imprime los textos de comentarios en el informe
         */
        private bool PrintComment(System.Drawing.Printing.PrintPageEventArgs e, ArrayList arrStringLine,
            Font fontReport, bool bMorePagesToPrint)
        {
            bool salir = false;
            while ((iLine < arrStringLine.Count) && !salir && !bMorePagesToPrint)
            {
                bool isPrintLine = false;
                string stringLine = (string)arrStringLine[iLine];
                bMorePagesToPrint = PrintLineText(e, stringLine, fontReport, ref isPrintLine);
                if (isPrintLine)
                {
                    iLine++;
                    isPrintLine = false;
                }
                else
                {
                    salir = true;
                    bNewTable = isPrintLine; ;
                }
            }
            return bMorePagesToPrint;
        }// end PrintComment




        /* Descrición:
         *  Imprime texto línea a línea
         */
        private bool PrintLineText(System.Drawing.Printing.PrintPageEventArgs e, string stringLine, Font f, ref bool isPrintLine)
        {
            bool bMorePagesToPrint = false;
            int iLineHeight = f.Height + 5;
            // comprobamos que hay espació para imprimir la linea de texto
            if (iTopMargin + iLineHeight >= e.MarginBounds.Height + e.MarginBounds.Top)
            {
                bMorePagesToPrint = true;
            }
            else
            {
                isPrintLine = true;
                e.Graphics.DrawString(stringLine, f, Brushes.Black, e.MarginBounds.Left, iTopMargin);
                iTopMargin += iLineHeight;
            }
            return bMorePagesToPrint;
        }


        #region Cabecera y pie de página
        /* Descripción:
         *  Imprime el encabezado de la página. Consta de el nombre del programa a la izquierda
         *  y la Universidad de Málaga a la derecha.
         */
        public void HeaderPageReport(System.Drawing.Printing.PrintPageEventArgs e, Font headerPageFount)
        {
            /* Cabecera */
            // El Nombre del Programa y versión a la derecha
            e.Graphics.DrawString(SAGT + version, headerPageFount, Brushes.Black, e.MarginBounds.Left, 50);

            /* 
             * Nota:
             *  La propiedad MarginBounds obtiene el área rectangular que representa la parte 
             *  de la página comprendida entre los márgenes.
             */

            // La universidad de málaga a la izquierda
            e.Graphics.DrawString(UNIV_UMA, headerPageFount, Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width -
                                e.Graphics.MeasureString(UNIV_UMA, headerPageFount, e.MarginBounds.Width).Width), 50);
            // Calculamos la máxima altura del texto para esa fuente.
            float alturaLetra = headerPageFount.GetHeight();
            // Línea por debajo del texto (calculamos el Left y Top de los 2 puntos extremos).
            int leftPunto1 = e.MarginBounds.Left;
            int topPunto1 = 50 + (int)alturaLetra + 6;
            int leftPunto2 = e.MarginBounds.Right; // +anchoCompleto; 
            int topPunto2 = topPunto1;
            e.Graphics.DrawLine(Pens.Black, leftPunto1, topPunto1, leftPunto2, topPunto2);
        }


        /* Descripción:
         *  Imprime el pie de la página. Consta de los nombre de los miembros del proyecto, a la izquierda
         *  y la númeración de página.
         */
        public void FootnoteOfTheReport(System.Drawing.Printing.PrintPageEventArgs e, Font headerPageFount)
        {
            /* Pie de página */
            // Calculamos la máxima altura del texto para esa fuente.
            float alturaLetra = headerPageFount.GetHeight();
            // Línea por encima del texto (calculamos el Left y Top de los 2 puntos extremos).
            int leftPunto1 = e.MarginBounds.Left;
            int topPunto1 = e.MarginBounds.Bottom + 25; // +(int)alturaLetra + 6;
            int leftPunto2 = e.MarginBounds.Right; // +anchoCompleto; 
            int topPunto2 = e.MarginBounds.Bottom + 25;
            e.Graphics.DrawLine(Pens.Black, leftPunto1, topPunto1, leftPunto2, topPunto2);
            
            // El Nombre del Programa a la derecha
            e.Graphics.DrawString(developer + ": " + NAME_STUDENT, headerPageFount, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Bottom + 25);

            /* 
             * Nota:
             *  La propiedad MarginBounds obtiene el área rectangular que representa la parte 
             *  de la página comprendida entre los márgenes.
             */

            // La universidad de málaga a la izquierda
            numPag++;
            string num_pag = pag+" " + numPag; // Concatenamos el string "Página" con el número de página
            e.Graphics.DrawString(num_pag, headerPageFount, Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width -
                                e.Graphics.MeasureString(num_pag, headerPageFount, e.MarginBounds.Width).Width), e.MarginBounds.Bottom + 25);

            // El Nombre del Programa a la derecha
            e.Graphics.DrawString(projectDirector + ": " + NAME_PROJECT_DIRECTOR , headerPageFount, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Bottom + 25 + alturaLetra);
            e.Graphics.DrawString(academicDirector + ": " + NAME_ACADEMIC_DIRECTOR, headerPageFount, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Bottom + 25 + (alturaLetra * 2));

        }// end FootnoteOfTheReport
        #endregion Cabecera y pie de página


        #endregion Print Page Event



        /* Descripción:
         *  Selecciona los elementos del informe y muestra la vista preliminar de los elementos
         */
        private void tsmiDataPrintPreview_Click(object sender, EventArgs e)
        {
            bData = !(sagtElements.GetMultiFacetsObs() == null);
            bMeans = !(sagtElements.GetListMeans() == null);
            bSsq = !(sagtElements.GetAnalysis_and_G_Study() == null);
            FormSelectReports formSelectReports = new FormSelectReports(this.LanguageActually(), bData, bMeans, bSsq);

            bool salir = false;
            do
            {
                DialogResult res = formSelectReports.ShowDialog();
                switch (res)
                {
                    case DialogResult.Cancel: salir = true; break;
                    case DialogResult.OK:
                        // comprobamos las pestañas marcadas
                        bData = formSelectReports.IsDataSelected();
                        bMeans = formSelectReports.IsMeansSelected();
                        bSsq = formSelectReports.IsSsqSelected();

                        if (bData || bMeans || bSsq)
                        {// (* 1 *)

                            PrintPreviewDialog MyPrintPreviewDialog = new PrintPreviewDialog();
                            MyPrintPreviewDialog.Document = printSagtDocument;
                            MyPrintPreviewDialog.ShowDialog();
                            salir = true;
                        }
                        else
                        {
                            // No hay ningún elemento seleccionado, mostramos un mensaje
                            // no hay ningún elemento seleccionado.
                            ShowMessageErrorOK(txtMessageNoSelected, "", MessageBoxIcon.Stop);
                        }// end if (* 1 *)

                        break;
                }
            } while (!salir);
        }// end tsmiDataPrintPreview_Click


        /* Descripción:
         *  Devuelve un DataGridView con los ajustes de fuente que se pasan como parámetro. Estos
         *  se hace para facilitar la impresión.
         *  
         * Parámetros:
         *      DataGridViewEx.DataGridViewEx dgvEx: DataGridView a partir del cual tomamos los datos
         *      Font fontTitle: Fuente para la fila de cabecera.
         *      Font fontCell: Fuente para las celdas de la tabla.
         */
        private DataGridViewEx.DataGridViewEx CreateNewDataGridView(DataGridViewEx.DataGridViewEx dgvEx, Font fontTitle, Font fontCell)
        {
            DataGridViewEx.DataGridViewEx newDgvEx = new DataGridViewEx.DataGridViewEx(); // Variable de retorno
            int numCol = dgvEx.ColumnCount; // Número de columnas del DataGrid original.
            newDgvEx.NumeroColumnas = numCol;
            newDgvEx.ColumnHeadersDefaultCellStyle.Font = fontTitle;
            newDgvEx.DefaultCellStyle.Font = fontCell;
            newDgvEx.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            
            // Agregarmos los titulos de las celdas
            for (int i = 0; i < numCol; i++)
            {
                newDgvEx.Columns[i].HeaderText = dgvEx.Columns[i].HeaderText;
                newDgvEx.Columns[i].DefaultCellStyle.Alignment = dgvEx.Columns[i].DefaultCellStyle.Alignment;
                newDgvEx.Columns[i].MinimumWidth = 60; // Anchura minima de la columna
            }

            // Agregamos las filas
            int numRows = dgvEx.Rows.Count;

            for (int i = 0; i < numRows; i++)
            {
                object[] my_Row = new object[numCol];

                DataGridViewRow dgvEx_Row = dgvEx.Rows[i];

                for (int j = 0; j < numCol; j++)
                {
                    my_Row[j] = dgvEx_Row.Cells[j].Value;
                }
                newDgvEx.Rows.Add(my_Row);
            }

            // Realizamos el ajuste de las celdas
            newDgvEx.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            return newDgvEx;
        }// end CreateNewDataGridView


        /* Descripción:
         *  Dado un string pasado como parámetro devuelve un arrayList de string, cada uno
         *  con la línea que cabrá en el margen del texto.
         */
        private ArrayList ArrayOfStrings(string txt, Font f, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Variable de retorno
            ArrayList arrayListOfLinesStrings = new ArrayList();

            /* primero dividimos el texto en lineas separadas por retorno de carro */
            char[] delimeterChars = { '\n' }; // nuestro delimitador será el caracter '/'
            string[] arrayline = txt.Split(delimeterChars);
            int n = arrayline.Length;
            
            for (int i = 0; i < n; i++)
            {
                string line = arrayline[i];
                /* si la linea cabe la incluimos, sino la partimos */
                Size textSize = TextRenderer.MeasureText(line, f);
                if (textSize.Width <= e.MarginBounds.Right - e.MarginBounds.Left)
                {
                    arrayListOfLinesStrings.Add(line);
                }
                else
                {
                    char[] delimeterChars2 = { ' ' }; // nuestro delimitador será el caracter '/'
                    string[] arrayOfSplit = line.Split(delimeterChars2, StringSplitOptions.None);
                    int lg = arrayOfSplit.Length;

                    string auxline = arrayOfSplit[0];
                    for(int j=1;j<lg;j++)
                    {
                        string auxline2 = auxline + " " + arrayOfSplit[j];
                        Size auxTextSize = TextRenderer.MeasureText(auxline2, f);

                        if (auxTextSize.Width  <= (e.MarginBounds.Width))
                        {
                            /* Si la anchura de auzxline2 es menor que la anchura de la página
                             * la añado a la linea
                             */
                            if (auxline.Equals(""))
                            {
                                auxline = arrayOfSplit[j];
                            }
                            else
                            {
                                auxline = auxline + " " + arrayOfSplit[j];
                            }
                        }
                        else
                        {
                            /* Si no cabe la nueva palabra añado la línea y añado la nueva
                             * palabra al siguiente renglón
                             */
                            arrayListOfLinesStrings.Add(string.Copy(auxline));
                            // auxline = "";
                            auxline = arrayOfSplit[j];
                        }
                    }
                    arrayListOfLinesStrings.Add(string.Copy(auxline));
                }
            }

            return arrayListOfLinesStrings;
        }// end ArrayOfStrings



        #region Traducción de elementos del informe
        /*
         * Descripción:
         *  Cambia de idioma los elementos del informe.
         * Parámetros:
         *      TransLibrary.Language lang: Idioma al que vamos a traducir los elementos de la ventana.
         */
        private void traslationElementsReports(TransLibrary.Language lang, string pathFileTrans)
        {
            TransLibrary.ReadFileTrans dic = new TransLibrary.ReadFileTrans(pathFileTrans);
            string name = "";
            try
            {
                // Traducimos los Textos del informe
                //==================================

                // Traducimos el nombre del informe SAGT
                name = "nameSagtReport";
                nameSagtReport = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // traducimos el informde de análisis
                name = "nameAnalysisDocument";
                nameAnalysisDocument = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta de ventana
                name = "pag";
                pag = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta del desarrollador
                name = "developer";
                developer = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta de director de projecto
                name = "projectDirector";
                projectDirector = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta de director academico
                name = "academicDirector";
                academicDirector = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta de titulo de informe de tabla de frecuencias
                name = "titleDataReport";
                titleDataReport = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta de informe de tablas de medias
                name = "titleMeansReport";
                titleMeansReport = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta de informe de tabla de análisis de varianza
                name = "titleSsqReport";
                titleSsqReport = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta de la tabla G-Parametros
                name = "titleG_ParametersReport";
                titleG_ParametersReport = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos la etiqueta tabla
                name = "stringTable";
                stringTable = dic.labelTraslation(name).GetTranslation(lang).ToString();
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " Se produjo un error al traducir " + name);
            }
        }// end traslationElementsReports


        #endregion Traducción de elementos del informe



    }// end public partial class FormPrincipal : Form
}// end namespace GUI_TG