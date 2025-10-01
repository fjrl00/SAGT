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
 * Fecha de revisión: 04/Jun/2012                           
 * 
 * Descripción:
 *      Clase parcial ("partial") del FormPrincipal. Contiene los métodos referentes a la parte de
 *      generación de informes en PDF.
 */

// using System.ComponentModel;
// using System.Data;

using MultiFacetData;
using ProjectMeans;
using ImportEduGMeans;
using Sagt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
/// Estos son los namespace de iTextSharp
using iTextSharp.text;
using iTextSharp.text.pdf;
using myApp.ns.pages; // para poder usar la clase pdfPage y poder escribir encabezado y pie de página

namespace GUI_GT
{
    public partial class FormPrincipal : Form
    {
        /*=================================================================================
         * Variables
         *=================================================================================*/
        // Variables
        string pdfFilter = "Pdf file (*.pdf)|*.pdf";
        string pdfDefaultExt = "pdf";


        /*=================================================================================
         * Métodos
         *=================================================================================*/

        /* Descripción:
         *  Muestra al  usuario los elementos seleccionables para el informe y una vez seleccionados por
         *  este costruye documento PDF.
         */
        private void SelectElementsAndCreatePDFDocument()
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
                            SaveFileDialog saveDialog = new SaveFileDialog();

                            if (Directory.Exists(cfgApli.Get_Path_Workspace()))
                            {
                                saveDialog.InitialDirectory = cfgApli.Get_Path_Workspace();
                            }

                            saveDialog.DefaultExt = pdfDefaultExt; // extensión por defecto del archivo
                            saveDialog.Filter = pdfFilter; // filtro pdf inicado en las variables globales 
                            saveDialog.OverwritePrompt = true; // muestra advertencia si el fichero ya existe
                            saveDialog.AddExtension = true;
                            System.Windows.Forms.DialogResult resulDialog = saveDialog.ShowDialog();
                            if (resulDialog == DialogResult.OK)
                            {
                                WriterSagtPdfDocument(saveDialog.FileName);
                                salir = true;
                            }
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
        }// end SelectElementsAndCreatePDFDocument


        /* Descripción:
         *  Crea un fichero pdf de análisis.
         */
        private void CreateAnalysisPdfDocument()
        {
            if (this.anl_tAnalysis_G_study_opt == null)
            {
                ShowMessageErrorOK(errorNoSSQ);
            }
            else
            {
                SaveFileDialog saveDialog = new SaveFileDialog();

                if (Directory.Exists(this.cfgApli.Get_Path_Workspace()))
                {
                    saveDialog.InitialDirectory = this.cfgApli.Get_Path_Workspace();
                }

                saveDialog.DefaultExt = pdfDefaultExt; // extensión por defecto del archivo
                saveDialog.Filter = pdfFilter; // filtro pdf inicado en las variables globales 
                saveDialog.OverwritePrompt = true; // muestra advertencia si el fichero ya existe
                saveDialog.AddExtension = true;
                System.Windows.Forms.DialogResult resulDialog = saveDialog.ShowDialog();
                if (resulDialog == DialogResult.OK)
                {
                    WriterAnalysisPdfDocument(saveDialog.FileName);
                }
            }
        }


        /* Descripción:
         *  Genera un informe en Pdf a partir de los datos seleccionados para el informe.
         */
        private void WriterSagtPdfDocument(string nameFilePdf)
        {
            try
            {
                // Document doc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                Document doc = new Document(PageSize.A4, 50, 50, 60, 60);
                // Margenes: izquierdo, derecho, superior e inferior


                FileStream file = new FileStream(nameFilePdf, FileMode.Create,
                    FileAccess.ReadWrite, FileShare.ReadWrite);

                PdfWriter pdfWriter = PdfWriter.GetInstance(doc, file);

                // OnEndPage(writer, doc);
                pdfPage ev = new pdfPage(SAGT + this.version, UNIV_UMA,
                    developer + ": " + NAME_STUDENT, projectDirector + ": " + NAME_PROJECT_DIRECTOR,
                    academicDirector + ": " + NAME_ACADEMIC_DIRECTOR, pag);
                pdfWriter.PageEvent = ev; //After the Open

                doc.Open();

                GenerarDocumento(doc);

                doc.Close();

                Process.Start(nameFilePdf);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }// end private void WriterSagtPdfDocument

        
        /* Descripción:
         *   Genera el cuerpo del documento pdf de análisis.
         */
        private void WriterAnalysisPdfDocument(string nameFilePdf)
        {
            try
            {
                // Document doc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                Document doc = new Document(PageSize.A4, 50, 50, 60, 60);
                // Margenes: izquierdo, derecho, superior e inferior


                FileStream file = new FileStream(nameFilePdf, FileMode.Create,
                    FileAccess.ReadWrite, FileShare.ReadWrite);

                PdfWriter pdfWriter = PdfWriter.GetInstance(doc, file);

                // OnEndPage(writer, doc);
                pdfPage ev = new pdfPage(SAGT + " " + this.version, UNIV_UMA,
                    developer + ": " + NAME_STUDENT, projectDirector + ": " + NAME_PROJECT_DIRECTOR,
                    academicDirector + ": " + NAME_ACADEMIC_DIRECTOR, pag);
                pdfWriter.PageEvent = ev; //After the Open

                doc.Open();

                GenerarDocumentoAnalysis(doc);

                doc.Close();

                Process.Start(nameFilePdf);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /* Descripción:
         *  Genera un documento Pdf de Análisis.
         */
        private void GenerarDocumentoAnalysis(Document document)
        {
            // Fuente que se empleara en las tablas del informe
            iTextSharp.text.Font fontTableReport = 
                iTextSharp.text.FontFactory.GetFont(this.cfgApli.GetTableFontFamily(),
                this.cfgApli.GetTableFontSize(), iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            // Fuente que se empleara en las textos del informe
            iTextSharp.text.Font fontTextReport = 
                iTextSharp.text.FontFactory.GetFont(this.cfgApli.GetTextFontFamily(),
                this.cfgApli.GetTextFontSize(), iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // Linea de emisión del informe
            PdfPTable data = FechaDeEmision(fontTextReport);
            document.Add(data);

            // Título
            Paragraph paragraphSsq1 = new Paragraph(titleSsqReport, fontTextReport);
            document.Add(paragraphSsq1);

            // Linea de diseño de medida
            Paragraph paragraphSsq2 = new Paragraph(lbAnalysis_M_Desing_EditSsq.Text 
                + " " + tbAnalysisMdesign.Text, fontTextReport);
            document.Add(paragraphSsq2);
            // Tabla de facetas
            PdfPTable datatableSsqFacet = GeneratePdfPTable(dgvExAnalysisSourceOfVarSsq,
                this.cfgApli.GetShadingRows(), fontTableReport);
            document.Add(datatableSsqFacet);

            // Tabla de suma de cuadrados 
            PdfPTable datatableSsq_SSQ = GeneratePdfPTable(dgvExAnalysisSSq,
                this.cfgApli.GetShadingRows(), fontTableReport);
            document.Add(datatableSsq_SSQ);

            // Tabla de G-Parámetros
            PdfPTable datatableSsq_Gp = GeneratePdfPTable(dgvExAnalysis_GP,
                this.cfgApli.GetShadingRows(), fontTableReport);
            document.Add(datatableSsq_Gp);

            // Tabla de resumen datos optimizados
            PdfPTable datatableSsq_opt = GeneratePdfPTable(dgvAnalysisResumOpt,
                this.cfgApli.GetShadingRows(), fontTableReport);
            document.Add(datatableSsq_opt);

            // Linea de fichero de procedencia de los datos
            Paragraph paragraphSsq3 = new Paragraph(lbFileAnalysisProvede.Text
                + ": " + tbFileAnalysisProvede.Text, fontTextReport);
            document.Add(paragraphSsq3);

            // Linea de fecha de obtencíón de los datos
            Paragraph paragraphSsq4 = new Paragraph(lbDateAnalysisCreated.Text
                + ": " + tbDateAnalysisCreated.Text, fontTextReport);
            document.Add(paragraphSsq4);

            // Comentarios
            string textSsqInfoComment = "";
            if (rTextBoxAnalysisInfo.Text != null)
            {
                textSsqInfoComment = rTextBoxAnalysisInfo.Text;
            }
            Paragraph paragraphSsq5 = new Paragraph(textSsqInfoComment, fontTextReport);
            document.Add(paragraphSsq5);
        }// end GenerarDocumentoAnalysis


        //Función que genera el documento Pdf
        private void GenerarDocumento(Document document)
        {
            // Fuente que se empleará en las tablas de documento PDF
            iTextSharp.text.Font fontTableReport = 
                iTextSharp.text.FontFactory.GetFont(this.cfgApli.GetTableFontFamily(),
                this.cfgApli.GetTableFontSize(), iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            // Fuente que se empleará en las textos de documento PDF
            iTextSharp.text.Font fontTextReport = 
                iTextSharp.text.FontFactory.GetFont(this.cfgApli.GetTextFontFamily(),
                this.cfgApli.GetTextFontSize(), iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // Linea de emisión del informe
            PdfPTable data = FechaDeEmision(fontTextReport);
            document.Add(data);

            if (bData)
            {
                // Titulo

                // Nombre de fichero
                string textNameFile = this.lbFileData.Text + ": " + this.tbFileName.Text; // path del fichero
                Paragraph paragraphData1 = new Paragraph(textNameFile, fontTextReport);
                document.Add(paragraphData1);

                // Descripción
                string textFileDataDesc = this.lbCommentsData.Text + ": " + this.tbDescription.Text; // Descripción
                Paragraph paragraphData2 = new Paragraph(textFileDataDesc, fontTextReport);
                document.Add(paragraphData2);

                // Tabla de facetas
                PdfPTable datatableData1 = GeneratePdfPTable(this.dataGridViewExFacets,
                    this.cfgApli.GetShadingRows(), fontTableReport);
                document.Add(datatableData1);

                // Tabla de frecuencias
                PdfPTable datatableData2 = GeneratePdfPTable(this.dataGridViewExObsTable,
                    this.cfgApli.GetShadingRows(), fontTableReport);
                document.Add(datatableData2);

                // Comentarios (Pestaña de información de tabla de frecuencias)
                string textDataInfoComment = "";
                if (richTextBoxDataComment.Text != null)
                {
                    textDataInfoComment = richTextBoxDataComment.Text;
                }
                Paragraph paragraphData3 = new Paragraph(textDataInfoComment, fontTextReport);
                document.Add(paragraphData3);
            }

            // Informe de Tablas de Medias
            if (bMeans)
            {
                // Título

                // Bucle de tablas
                int numTabs = this.tabControlMeans.TabPages.Count - 1; // uno menos para no usar la pestaña de información

                for (int i = 0; i < numTabs; i++)
                {
                    TabPageMeansEx tabPageMeansEx = (TabPageMeansEx)tabControlMeans.TabPages[i];
                    // Título de tabla
                    Paragraph paragraphMean1 = new Paragraph(stringTable + ": " + tabPageMeansEx.Text,
                        fontTextReport);
                    document.Add(paragraphMean1);

                    // Tabla de medias
                    DataGridView dgvTableMeans = tabPageMeansEx.GetDataGridViewEx();
                    PdfPTable datatableMean = GeneratePdfPTable(dgvTableMeans,
                    this.cfgApli.GetShadingRows(), fontTableReport);
                    document.Add(datatableMean);

                    // Línea de Gran media
                    Paragraph paragraphMean2 = new Paragraph(tabPageMeansEx.GetLabelGrandMean().Text,
                       fontTextReport);
                    document.Add(paragraphMean2);
                }
                
                // Línea de fichero de procedencia de los datos
                Paragraph paragraphMean3 = new Paragraph(lbFileMeanProvede.Text + ": " + tbFileMeanProvede.Text,
                    fontTextReport);
                document.Add(paragraphMean3);
                // Línea de fecha de obtencíón de las tablas de medias
                Paragraph paragraphMean4 = new Paragraph(lbDateMeanCreated.Text + ": " + tbDateMeanCreated.Text,
                    fontTextReport);
                document.Add(paragraphMean4);
                // Commentarios
                string textMeanInfoComment = "";
                if (rTxtBoxMeanInfo.Text != null)
                {
                    textMeanInfoComment = rTxtBoxMeanInfo.Text;
                }
                Paragraph paragraphMean5 = new Paragraph(textMeanInfoComment, fontTextReport);
                document.Add(paragraphMean5);
            }

            // Informe de Suma de cuadrados
            if (bSsq)
            {
                // Título
                Paragraph paragraphSsq1 = new Paragraph(titleSsqReport, fontTextReport);
                document.Add(paragraphSsq1);
                
                // Línea de diseño de medida
                Paragraph paragraphSsq2 = new Paragraph(lbMeasurementDesign.Text + " "
                    + tbMeasurementDesign.Text, fontTextReport);
                document.Add(paragraphSsq2);
                // Tabla de facetas
                PdfPTable datatableSsqFacet = GeneratePdfPTable(dGridViewExSourceOfVar,
                    this.cfgApli.GetShadingRows(), fontTableReport);
                document.Add(datatableSsqFacet);

                // Tabla de suma de cuadrados 
                PdfPTable datatableSsq_SSQ = GeneratePdfPTable(dataGridViewExSSQ,
                    this.cfgApli.GetShadingRows(), fontTableReport);
                document.Add(datatableSsq_SSQ);

                // Tabla de G-Parámetros
                PdfPTable datatableSsq_Gp = GeneratePdfPTable(dGridViewExG_Parameters,
                    this.cfgApli.GetShadingRows(), fontTableReport);
                document.Add(datatableSsq_Gp);

                // Tabla de resumen datos optimizados
                PdfPTable datatableSsq_opt = GeneratePdfPTable(dGridViewExOptimizationResum,
                    this.cfgApli.GetShadingRows(), fontTableReport);
                document.Add(datatableSsq_opt);

                // Línea de fichero de procedencia de los datos
                Paragraph paragraphSsq3 = new Paragraph(lbNameFileSsqInfo.Text + ": "
                    + tbNameFileSsqInfo.Text, fontTextReport);
                document.Add(paragraphSsq3);

                // Línea de fecha de obtencíón de las tablas de medias
                Paragraph paragraphSsq4 = new Paragraph(lbDateFileSsqInfo.Text + ": "
                    + tbDateFileSsqInfo.Text, fontTextReport);
                document.Add(paragraphSsq4);

                // Commentarios
                string textSsqInfoComment = "";
                if (richTextBoxSsqComment.Text != null)
                {
                    textSsqInfoComment = richTextBoxSsqComment.Text;
                }
                Paragraph paragraphSsq5 = new Paragraph(textSsqInfoComment, fontTextReport);
                document.Add(paragraphSsq5);
            }
        }// end GenerarDocumento


        /* Descripción:
         *  Toma un dataGridView y lo pinta
         * Parámetros:
         *      DataGridView dataGridView: DataGrid que vamos a representar
         *      bool shadingRows: indica si se van a sombrear o no las filas alternas
         */
        private PdfPTable GeneratePdfPTable(DataGridView dataGridView, bool shadingRows, iTextSharp.text.Font fontTable)
        {
            //se crea un objeto PdfTable con el numero de columnas del
            //dataGridView
            PdfPTable datatable = new PdfPTable(dataGridView.ColumnCount);
            
            //asignamos algunas propiedades para el diseño del pdf
            datatable.DefaultCell.Padding = 3;
            float[] headerwidths = GetWidthOfColumns(dataGridView);
            datatable.SetWidths(headerwidths);
            datatable.WidthPercentage = 100;
            datatable.DefaultCell.BorderWidth = 2;
            datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            
            
            //SE GENERA EL ENCABEZADO DE LA TABLA EN EL PDF
            int numCol = dataGridView.ColumnCount;
            
            for (int i = 0; i < numCol; i++)
            {
                PdfPCell cell = new PdfPCell(new Phrase(dataGridView.Columns[i].HeaderText, fontTable));
                cell.BackgroundColor = BaseColor.GRAY; // new BaseColor(0, 150, 0);
                datatable.AddCell(cell);
            }

            datatable.HeaderRows = 1;
            datatable.DefaultCell.BorderWidth = 1;

            //SE GENERA EL CUERPO DE LA TABLA PDF
            for (int i = 0; i < dataGridView.RowCount; i++)
            {
                for (int j = 0; j < numCol; j++)
                {
                    string phrase = "";
                    if (dataGridView[j, i].Value != null)
                    {
                        phrase = dataGridView[j, i].Value.ToString();
                    }
                    PdfPCell cell = new PdfPCell(new Phrase(phrase, fontTable));
                    if (shadingRows && (i % 2 == 1))
                    {
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    }
                    datatable.AddCell(cell);
                }
                datatable.CompleteRow();
            }

            datatable.SpacingBefore = 20f;
            datatable.SpacingAfter = 20f;

            return datatable;
        }// GeneratePdfPTable


        /* Descripción:
         *  Función que obtiene los tamaños de las columnas del grid
         */
        public float[] GetWidthOfColumns(DataGridView dg)
        {
            float[] values = new float[dg.ColumnCount];

            for (int i = 0; i < dg.ColumnCount; i++)
            {
                values[i] = (float)dg.Columns[i].Width;
            }

            return values;
        }


        /* Descripción:
         *  Devuelve una tabla que contiene la fecha de emisión de informe
         *  (Exactamente, dibuja una tabla de una fila y dos celdas, de bordes invisibles, una alineada a la izq y otra a la der)
         */
        private PdfPTable FechaDeEmision(iTextSharp.text.Font font)
        {
            PdfPTable datatable = new PdfPTable(2);
            datatable.WidthPercentage = 100;

            // Obtenemos la fecha de emisión del informe
            String strDate = (DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString()).Trim();

            PdfPCell cell = new PdfPCell(new Phrase(issuanceOfReport + ":", font));
            cell.Border = 0;
            datatable.AddCell(cell);
            PdfPCell cell2 = new PdfPCell(new Phrase(strDate, font));
            cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell2.Border = 0;
            datatable.AddCell(cell2);
            datatable.DefaultCell.BorderWidth = 0;
            return datatable;
        }

    }// end public partial class FormPrincipal : Form
}// end namespace GUI_TG