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
 * Fecha de revisión: 03/Feb/2012                           
 * 
 * Descripción:
 *      Clase parcial ("partial") del FormPrincipal. Contiene los métodos referentes a la parte de
 *      generación de informes de la opción de Análisis.
 */
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
        /*=================================================================================================
         * Variables
         *=================================================================================================*/
       
        // Fuente para encabezados y pies de páginas
        Font fontFootersAndHeaders = new Font("Verdana", 9F, FontStyle.Regular);

        // Nombre del documento de analysis
        DataGridViewEx.DataGridViewEx dgvExAuxReport = null;
        private string nameAnalysisDocument = "Análisis";
        private bool bNewTable = true;
        private bool isFinishTableOptResume = false;
        private bool isFinishAnalysisComment = false;
        // Las ariables para la impresión de los comentarios de análisis han sido definidas en PrintSagtDocuments.cs


        /* Descripción:
         *  Se ejecuta al seleccionar la impción imprimir del menú vertical de análisis. Muestra
         *  la ventana de seleción de impresora e imprime el informe de análisis.
         */
        private void tsmiAnalysisPrinter_Click(object sender, EventArgs e)
        {
            //Open the print dialog
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printAnalysisDocument;
            printDialog.UseEXDialog = true;

            //Get the document
            if (DialogResult.OK == printDialog.ShowDialog())
            {
                if (this.anl_tAnalysis_G_study_opt != null)
                {// (* 1 *)

                    printSagtDocument.DocumentName = nameAnalysisDocument;
                    printSagtDocument.Print();
                }
                else
                {
                    // No hay ningún elemento seleccionado, mostramos un mensaje
                    // no hay ningún elemento seleccionado.
                    ShowMessageErrorOK(txtMessageNoSelected, "", MessageBoxIcon.Stop);
                }// end if (* 1 *)
            }
        }


        /* Descripción:
         *  Se ejecuta al seleccionar la opción vista previa del menú vertical de análisis. Muestra
         *  la vista previa del informe de análisis de varianzas.
         */
        private void tsmiAnalysisPrintPreview_Click(object sender, EventArgs e)
        {
            if (this.anl_tAnalysis_G_study_opt != null)
            {// (* 1 *)

                PrintPreviewDialog MyPrintPreviewDialog = new PrintPreviewDialog();
                MyPrintPreviewDialog.Document = printAnalysisDocument;
                MyPrintPreviewDialog.ShowDialog();
            }
            else
            {
                // No hay ningún elemento seleccionado, mostramos un mensaje
                // no hay ningún elemento seleccionado.
                ShowMessageErrorOK(txtMessageNoSelected, "", MessageBoxIcon.Stop);
            }// end if (* 1 *)
        }


        /* Descripción:
         *  Evento BeginPrint de la impresión del informe de la opción de análisis.
         */
        private void printAnalysisDocument_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                traslationElementsReports(this.cfgApli.GetConfigLanguage(), Application.StartupPath + LANG_PATH + FILE_STRING_REPORT);

                strFormat = new StringFormat();
                strFormat.Alignment = StringAlignment.Near;
                strFormat.LineAlignment = StringAlignment.Center;
                strFormat.Trimming = StringTrimming.EllipsisCharacter;
                /* Inicializamos las variables */
                iPosListMeans = 0;
                arrColumnLefts.Clear();
                arrColumnWidths.Clear();

                // Iniciamos la variables para la impresión de los textos de comentarios
                arrStringLine.Clear(); // inicializamos el array de lineas de texto
                iLine = 0; // inicializamos el indice de lineas de texto
                iCellHeight = 0;
                iRow = 0;
                numPag = 0;

                
                /// Indican si estamos ante la primera página de un elemento
                bFirstPage = true; // Primera página
                bNewPage = true; // Nueva página
                bNewTable = true;

                bFirstPageReportSsq = false; // Primera página del informe de suma de cuadrados
                /* Quizas debería poner una variable booleana para señalar que aun no se ha impreso la tabla de faceta
                 * y de observaciones. */
                bFirstPageG_Parameters = true;
                isFinishTableFacet = false;
                isFinishTableSsq = false;
                isFinishTableG_Parameters = false;
                isFinishTableOptResume = false;
                isPrintLineDesign = false;
                isFinishAnalysisComment = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }// end printAnalysisDocument_BeginPrint


        /* Descripción:
         *  Evento que se lanza una vez terminada la impresión
         */
        private void printAnalysisDocument_EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            dgvExAuxReport = null; // lo ponesmos a null para ahorrar memoria.
        }


        /* Descripción:
         *  Evento PrintPage para la impresión del informe de la opción de análisis.
         */
        private void printAnalysisDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                // Es una nueva página
                bool bNewPage = true;
                // Establecemos el margen superior
                iTopMargin = e.MarginBounds.Top;
                // Si hay más páginas tiene que imprimir o no
                bool bMorePagesToPrint = false; // inicialmente no tienen poque imprimirse nuevas páginas
                // Si el valor recuperado de la configuración es true se sobrearan alternativamente las filas
                bool bShadingRows = this.cfgApli.GetShadingRows();


                // imprimimos la cabecera
                HeaderPageReport(e, this.fontFootersAndHeaders);
                
                // imprimimos el pie de página
                FootnoteOfTheReport(e, this.fontFootersAndHeaders);
                //************************************************************************************

                // Fuente que se empleara en el resto del informe en las tablas
                int n = this.cfgApli.GetTableFontSize();
                Font fontTableReport = new Font(this.cfgApli.GetTableFontFamily(), n, FontStyle.Bold);

                // Fuente que se empleara en el resto del informe en los textos
                n = this.cfgApli.GetTextFontSize();
                Font fontTextReport = new Font(this.cfgApli.GetTextFontFamily(), n, FontStyle.Bold);

                // For the first page to print set the cell width and header height ??
                // Para la primera página para ajustar el ancho de las celdas de encabezado y altura
                if (bFirstPage)
                {// (* 1 *)

                    /*************************************************************************************************/

                    //Dibujamos cabecera del informe que solo aparecerá en la primerá pagina del informe
                    e.Graphics.DrawString(issuanceOfReport, fontTextReport, Brushes.Black, e.MarginBounds.Left,
                        e.MarginBounds.Top - e.Graphics.MeasureString(issuanceOfReport, fontTextReport, e.MarginBounds.Width).Height);

                    String strDate = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();
                    //Dibujamos fecha
                    e.Graphics.DrawString(strDate, fontTextReport, Brushes.Black, e.MarginBounds.Left +
                        (e.MarginBounds.Width - e.Graphics.MeasureString(strDate, fontTextReport, e.MarginBounds.Width).Width),
                        e.MarginBounds.Top - e.Graphics.MeasureString(titleDataReport, fontTextReport, e.MarginBounds.Width).Height);

                    /***********************************************************************************************/

                }// end if (* 1 *)

                // Imprimimos la tabla de facetas
                if (!isFinishTableFacet)
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
                        // Calculando ancho total
                        dgvExAuxReport = CreateNewDataGridView(dgvExAnalysisSourceOfVarSsq, fontTableReport, fontTableReport);
                        // iTotalWidth = CaulatingTotalWidths(dgvExAnalysisSourceOfVarSsq);
                        iTotalWidth = CaulatingTotalWidths(dgvExAuxReport);
                        // SettingHeadersCell(e, dgvExAnalysisSourceOfVarSsq);
                        SettingHeadersCell(e, dgvExAuxReport);
                        bNewTable = false;
                    }
                    // bMorePagesToPrint = PrintDataGridViewEx(e, dgvExAnalysisSourceOfVarSsq, fontReport, bShadingRows, ref isFinishTableFacet);
                    bMorePagesToPrint = PrintDataGridViewEx(e, dgvExAuxReport, fontTableReport, bShadingRows, ref isFinishTableFacet);
                    bNewTable = isFinishTableFacet;
                    bNewPage = false;
                }

                if (isFinishTableFacet && !isFinishTableSsq)
                {
                    if (bNewTable)
                    {
                        if (bNewPage)
                        {
                            iTopMargin = e.MarginBounds.Top;
                            bNewPage = false;
                        }
                        else
                        {
                            iTopMargin += 20;
                        }
                        arrColumnLefts.Clear();
                        arrColumnWidths.Clear();
                        iCellHeight = 0;
                        iRow = 0;

                        dgvExAuxReport = CreateNewDataGridView(dgvExAnalysisSSq, fontTableReport, fontTableReport);
                        // Calculando ancho total
                        // iTotalWidth = CaulatingTotalWidths(dgvExAnalysisSSq);
                        iTotalWidth = CaulatingTotalWidths(dgvExAuxReport);

                        // SettingHeadersCell(e, dgvExAnalysisSSq);   
                        SettingHeadersCell(e, dgvExAuxReport); 
                    }
                    // bMorePagesToPrint = PrintDataGridViewEx(e, dgvExAnalysisSSq, fontReport, bShadingRows, ref isFinishTableSsq);
                    bMorePagesToPrint = PrintDataGridViewEx(e, dgvExAuxReport, fontTableReport, bShadingRows, ref isFinishTableSsq);
                    bNewTable = isFinishTableSsq;
                    bNewPage = false;
                }

                if (isFinishTableFacet && isFinishTableSsq && !isFinishTableG_Parameters)
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

                        dgvExAuxReport = CreateNewDataGridView(dgvExAnalysis_GP, fontTableReport, fontTableReport);
                        // Calculando ancho total
                        // iTotalWidth = CaulatingTotalWidths(dgvExAnalysis_GP);
                        iTotalWidth = CaulatingTotalWidths(dgvExAuxReport);
                        // SettingHeadersCell(e, dgvExAnalysis_GP);
                        SettingHeadersCell(e, dgvExAuxReport);
                    }
                    // bMorePagesToPrint = PrintDataGridViewEx(e, dgvExAnalysis_GP, fontReport, bShadingRows, ref isFinishTableG_Parameters);
                    bMorePagesToPrint = PrintDataGridViewEx(e, dgvExAuxReport, fontTableReport, bShadingRows, ref isFinishTableG_Parameters);
                    bNewTable = isFinishTableG_Parameters;
                    bNewPage = false;
                }


                if (isFinishTableFacet && isFinishTableSsq && isFinishTableG_Parameters && !isFinishTableOptResume)
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

                        dgvExAuxReport = CreateNewDataGridView(dgvAnalysisResumOpt, fontTableReport, fontTableReport);

                        // Calculando ancho total
                        if (dgvAnalysisResumOpt.ColumnCount > 4)
                        {
                            // iTotalWidth = CaulatingTotalWidths(dgvAnalysisResumOpt);
                            iTotalWidth = CaulatingTotalWidths(dgvExAuxReport);
                        }
                        // SettingHeadersCell(e, dgvAnalysisResumOpt);
                        SettingHeadersCell(e, dgvExAuxReport);
                    }
                    // bMorePagesToPrint = PrintDataGridViewEx(e, dgvAnalysisResumOpt, fontReport, bShadingRows, ref isFinishTableOptResume);
                    bMorePagesToPrint = PrintDataGridViewEx(e, dgvExAuxReport, fontTableReport, bShadingRows, ref isFinishTableOptResume);
                    bNewTable = isFinishTableOptResume;
                    bNewPage = false;
                }

                // Impresión de cometarios
                if (isFinishTableFacet && isFinishTableSsq && isFinishTableG_Parameters && isFinishTableOptResume && !isFinishAnalysisComment)
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
                        string text = "\n" + lbFileAnalysisProvede.Text + ": " + tbFileAnalysisProvede.Text;
                        text = text + "\n" + lbDateAnalysisCreated.Text + ": " + tbDateAnalysisCreated.Text;
                        text = text + "\n" + rTextBoxAnalysisInfo.Text;
                        iLine = 0;
                        arrStringLine = ArrayOfStrings(text, fontTextReport, e);
                    }

                    bMorePagesToPrint = PrintComment(e, arrStringLine, fontTextReport, bMorePagesToPrint);
                    bNewPage = false;
                }


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
        }// end printAnalysisDocument_PrintPage

    }// end partial class FormPrincipal : Form
}// end namespace GUI_TG
