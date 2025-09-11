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
 *      generación de informes en word.
 */

using MultiFacetData;
using ProjectMeans;
using ImportEduGMeans;
using Sagt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word; // Para usar documentos de Word

namespace GUI_GT
{
    public partial class FormPrincipal : Form
    {

        /* Descripción:
         *  Selecciona los elementos del informe que luego se escribirán en un documento word.
         */
        private void SelectElementsAndCreateWordDocument()
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

                            WriterSagtWordDocument();
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
        }// end SelectElementsAndCreateWordDocument


        /* Descripción:
         *  Genera un informe en word a partir de los datos seleccionados para el informe.
         */
        private void WriterSagtWordDocument()
        {
            Font fontTableReport = new Font(this.cfgApli.GetTableFontFamily(), this.cfgApli.GetTableFontSize());

            //Objeto del Tipo Word Application
            Word.Application objWordApplication;
            //Objeto del Tipo Word Document
            Word.Document objWordDocument;
            // Objeto para interactuar con el Interop
            Object oMissing = System.Reflection.Missing.Value;
            object oEndOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */

            //Creamos una instancia de una Aplicación Word.
            objWordApplication = new Word.Application();
            //A la aplicación Word, le añadimos un documento.
            objWordDocument = objWordApplication.Documents.Add(ref oMissing,
                                                             ref oMissing,
                                                             ref oMissing, ref oMissing);

            //Activamos el documento recien creado, de forma que podamos escribir en el
            objWordDocument.Activate();

            //Hace visible la Aplicacion para que veas lo que se ha escrito
            objWordApplication.Visible = true;

            // Introducimos un tabulador derecho
            Object alignment = Word.WdTabAlignment.wdAlignTabRight; // Tipo de Tabulador, en este caso derecho
            Object leader = Word.WdTabLeader.wdTabLeaderSpaces; // relleno del tabulador
            objWordApplication.Selection.Range.ParagraphFormat.TabStops.Add(425, ref alignment, ref leader);
            objWordApplication.Selection.Font.Name = this.cfgApli.GetTextFontFamily(); //Cambiamos el nombre
            objWordApplication.Selection.Font.Size = this.cfgApli.GetTextFontSize(); //Cambiamos el tamaño

            // Obtenemos la fecha de emisión del informe
            String strDate = (DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString()).Trim();

            //Empezamos a escribir
            objWordApplication.Selection.TypeText(issuanceOfReport + ": \t" + strDate + "\n");
            //Indicamos que el texto anterior es parte de un párrafo.
            objWordApplication.Selection.TypeParagraph();

            // Imprimos la tabla de frecuencias si BData es true
            if (bData)
            {
                // Nuevo parrafo
                string textNameFile = this.lbFileData.Text + ": " + this.tbFileName.Text; // path del fichero
                objWordApplication.Selection.TypeText(textNameFile + "\n");
                objWordApplication.Selection.TypeParagraph();// fin de parrafo

                // Nuevo parrafo
                string textFileDataDesc = this.lbCommentsData.Text + ": " + this.tbDescription.Text; // Descripción
                objWordApplication.Selection.TypeText(textFileDataDesc + "\n");
                objWordApplication.Selection.TypeParagraph();// fin de parrafo

                //Ahora veamos como cambiar el tipo y tamaño de la letra
                objWordApplication.Selection.Font.Name = this.cfgApli.GetTextFontFamily(); //Cambiamos el nombre
                objWordApplication.Selection.Font.Size = this.cfgApli.GetTextFontSize(); //Cambiamos el tamaño

                // Introducimos la tabla de facetas
                wordDocumentInsertTable(ref objWordDocument, ref oEndOfDoc, this.dataGridViewExFacets,
                    fontTableReport, this.cfgApli.GetShadingRows());

                objWordApplication.Selection.TypeParagraph();// fin de parrafo

                // Introducimos la tabla de frecuencias
                wordDocumentInsertTable(ref objWordDocument, ref oEndOfDoc, this.dataGridViewExObsTable,
                     fontTableReport, this.cfgApli.GetShadingRows());
                objWordApplication.Selection.TypeParagraph();// fin de parrafo

                // cometarios de la pestaña de información
                string textDataInfoComment = "";
                if (richTextBoxDataComment.Text != null)
                {
                    textDataInfoComment = richTextBoxDataComment.Text;
                }

                //Insertamos un parrafo de comentario de los datos del documento.
                Word.Paragraph oPara1; // variable que contendrá el primer parrafo
                oPara1 = objWordDocument.Content.Paragraphs.Add(ref oMissing); // añadimos el parrafo al documento
                oPara1.Range.Text = textDataInfoComment; // Texto del primer parrafo
                oPara1.Range.Font.Bold = 0; // 
                oPara1.Format.SpaceAfter = 24;    //24 pt de spacio despues del parrafo.
                oPara1.Range.InsertParagraphAfter(); // insertamos al final del parrafo.
            }

            // Añadimos al documento las tablas de Medias si bMeans es true
            if (bMeans)
            {
                // Imprimimos el titulo del informe
                Word.Paragraph oPara2; // variable que contendrá el primer parrafo
                oPara2 = objWordDocument.Content.Paragraphs.Add(ref oMissing); // añadimos el parrafo al documento
                oPara2.Range.Text = titleMeansReport; // título del informe de medias
                oPara2.Range.Font.Bold = 0; // 
                oPara2.Format.SpaceAfter = 6;    //24 pt de spacio despues del parrafo.
                oPara2.Range.InsertParagraphAfter(); // insertamos al final del parrafo.

                // Entramos en el bucle de tablas de medias
                int n = this.tabControlMeans.TabPages.Count - 1; // uno menos para no usar la pestaña de información

                for (int i = 0; i < n; i++)
                {
                    TabPageMeansEx tabPageMeansEx = (TabPageMeansEx)tabControlMeans.TabPages[i];

                    // Titulo de la Tabla de medias
                    Word.Paragraph oPara3; // variable que contendrá el primer parrafo
                    oPara3 = objWordDocument.Content.Paragraphs.Add(ref oMissing); // añadimos el parrafo al documento
                    oPara3.Range.Text = stringTable + ": " + tabPageMeansEx.Text; // Título de tabla
                    oPara3.Range.Font.Bold = 0; // 
                    oPara3.Format.SpaceAfter = 6;    //6 pt de spacio despues del parrafo.
                    oPara3.Range.InsertParagraphAfter(); // insertamos al final del parrafo.

                    // Tabla de medias
                    DataGridView dgvTableMeans = tabPageMeansEx.GetDataGridViewEx();
                    wordDocumentInsertTable(ref objWordDocument, ref oEndOfDoc, dgvTableMeans,
                    fontTableReport, this.cfgApli.GetShadingRows());

                    // Linea de gran media
                    Word.Paragraph oPara4; // variable que contendrá el primer parrafo
                    oPara4 = objWordDocument.Content.Paragraphs.Add(ref oMissing); // añadimos el parrafo al documento
                    oPara4.Range.Text = tabPageMeansEx.GetLabelGrandMean().Text; // Título de tabla
                    oPara4.Range.Font.Bold = 0; // 
                    oPara4.Format.SpaceAfter = 24;    //24 pt de spacio despues del parrafo.
                    oPara4.Range.InsertParagraphAfter(); // insertamos al final del parrafo.
                }

                // Imprimimos la pestaña de información
                string textMeanInfoComment = "";
                if (rTxtBoxMeanInfo.Text != null)
                {
                    textMeanInfoComment = rTxtBoxMeanInfo.Text;
                }
                string textMeansInfo = "\n" + lbFileMeanProvede.Text + ": " + tbFileMeanProvede.Text;
                            textMeansInfo = textMeansInfo + "\n" + lbDateMeanCreated.Text + ": " + tbDateMeanCreated.Text;
                            textMeansInfo = textMeansInfo + "\n" + textMeanInfoComment;

                Word.Paragraph oPara5; // variable que contendrá el primer parrafo
                oPara5 = objWordDocument.Content.Paragraphs.Add(ref oMissing); // añadimos el parrafo al documento
                oPara5.Range.Text = textMeansInfo; // Texto de Informe de medias
                oPara5.Range.Font.Bold = 0; // 
                oPara5.Format.SpaceAfter = 24;    //24 pt de spacio despues del parrafo.
                oPara5.Range.InsertParagraphAfter(); // insertamos al final del parrafo.
            
            }

            // Añadimos las tablas de análisis de suma de cuadrados si bSsq es true
            if (bSsq)
            {
                // Imprimos el título del informe de suma de cuadrados
                Word.Paragraph oPara6; // variable que contendrá el primer parrafo
                oPara6 = objWordDocument.Content.Paragraphs.Add(ref oMissing); // añadimos el parrafo al documento
                oPara6.Range.Text = titleSsqReport; // Texto de Informe de medias
                oPara6.Range.Font.Bold = 0; // 
                oPara6.Format.SpaceAfter = 6;    // 6 pt de spacio despues del parrafo.
                oPara6.Range.InsertParagraphAfter(); // insertamos al final del parrafo.

                // Imprimimos el diseño del estudio
                Word.Paragraph oPara7; // variable que contendrá el primer parrafo
                oPara7 = objWordDocument.Content.Paragraphs.Add(ref oMissing); // añadimos el parrafo al documento
                oPara7.Range.Text = lbMeasurementDesign.Text + " " + tbMeasurementDesign.Text; // Texto de Informe de medias
                oPara7.Range.Font.Bold = 0; // 
                oPara7.Format.SpaceAfter = 6;    // 6 pt de spacio despues del parrafo.
                oPara7.Range.InsertParagraphAfter(); // insertamos al final del parrafo.

                // Imprimimos la tabla de facetas
                wordDocumentInsertTable(ref objWordDocument, ref oEndOfDoc, dGridViewExSourceOfVar,
                    fontTableReport, this.cfgApli.GetShadingRows());

                // Imprimimos la tabla de suma de cuadrados
                wordDocumentInsertTable(ref objWordDocument, ref oEndOfDoc, dataGridViewExSSQ,
                    fontTableReport, this.cfgApli.GetShadingRows());

                // Imprimimos la tabla de G-Parámetros
                wordDocumentInsertTable(ref objWordDocument, ref oEndOfDoc, dGridViewExG_Parameters,
                    fontTableReport, this.cfgApli.GetShadingRows());

                // Imprimimos la tabla de Optimización
                wordDocumentInsertTable(ref objWordDocument, ref oEndOfDoc, dGridViewExOptimizationResum,
                    fontTableReport, this.cfgApli.GetShadingRows());

                // Imprimos la pestaña de información
                string textSSqInfoComment = "";
                if (richTextBoxSsqComment.Text != null)
                {
                    textSSqInfoComment = richTextBoxSsqComment.Text;
                }
                string textInfoSSq = "\n" + lbNameFileSsqInfo.Text + ": " + tbNameFileSsqInfo.Text;
                textInfoSSq = textInfoSSq + "\n" + lbDateFileSsqInfo.Text + ": " + tbDateFileSsqInfo.Text;
                textInfoSSq = textInfoSSq + "\n" + textSSqInfoComment;

                Word.Paragraph oPara8; // variable que contendrá el primer parrafo
                oPara8 = objWordDocument.Content.Paragraphs.Add(ref oMissing); // añadimos el parrafo al documento
                oPara8.Range.Text = textInfoSSq; // Texto de Informe de medias
                oPara8.Range.Font.Bold = 0; // 
                oPara8.Format.SpaceAfter = 6;    // 6 pt de spacio despues del parrafo.
                oPara8.Range.InsertParagraphAfter(); // insertamos al final del parrafo.
            }

            // Encabezado y pie de página
            wordDocumentHeaders(objWordDocument, this.fontFootersAndHeaders);
            // wordDocumentFooters(objWordDocument, this.fontFootersAndHeaders);

            wordDocumentFootersWithPageNumber(objWordApplication, this.fontFootersAndHeaders);


            //Alinearemos el texto que vamos a escribir al centro
            // objWordApplication.Selection.ParagraphFormat.Alignment =
            //    Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            
            //Agregamos al texto
            // objWordApplication.Selection.TypeText("Hecho por Copstone 2010");
            //Indicamos que el texto anterior es un párrafo
            // objWordApplication.Selection.TypeParagraph();
        }// end WriterSagtWordDocument


        /* Descripción:
         *  Genera un informe en word a partir de los datos de análisis de suma de cuadrados.
         */
        private void WriterAnalysisWordDocument()
        {
            if (this.anl_tAnalysis_G_study_opt != null)
            {// (* 1 *)
                Font fontTableReport = new Font(this.cfgApli.GetTableFontFamily(), this.cfgApli.GetTableFontSize());

                //Objeto del Tipo Word Application
                Word.Application objWordApplication;
                //Objeto del Tipo Word Document
                Word.Document objWordDocument;
                // Objeto para interactuar con el Interop
                Object oMissing = System.Reflection.Missing.Value;
                object oEndOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */

                //Creamos una instancia de una Aplicación Word.
                objWordApplication = new Word.Application();
                //A la aplicación Word, le añadimos un documento.
                objWordDocument = objWordApplication.Documents.Add(ref oMissing,
                                                                 ref oMissing,
                                                                 ref oMissing, ref oMissing);

                //Activamos el documento recien creado, de forma que podamos escribir en el
                objWordDocument.Activate();

                //Hace visible la Aplicacion para que veas lo que se ha escrito
                objWordApplication.Visible = true;

                // Introducimos un tabulador derecho
                Object alignment = Word.WdTabAlignment.wdAlignTabRight; // Tipo de Tabulador, en este caso derecho
                Object leader = Word.WdTabLeader.wdTabLeaderSpaces; // relleno del tabulador
                objWordApplication.Selection.Range.ParagraphFormat.TabStops.Add(425, ref alignment, ref leader);

                // Obtenemos la fecha de emisión del informe
                String strDate = (DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString()).Trim();

                //Empezamos a escribir
                objWordApplication.Selection.TypeText(issuanceOfReport + ": \t" + strDate + "\n");
                //Indicamos que el texto anterior es parte de un párrafo.
                objWordApplication.Selection.TypeParagraph();

                /*======================
                 * Cuerpo del documento
                 *======================*/
                // Imprimos el título del informe de suma de cuadrados
                Word.Paragraph oPara1; // variable que contendrá el primer parrafo
                oPara1 = objWordDocument.Content.Paragraphs.Add(ref oMissing); // añadimos el parrafo al documento
                oPara1.Range.Text = titleSsqReport; // Titulos del informe
                oPara1.Range.Font.Bold = 0; // 
                oPara1.Format.SpaceAfter = 6;    // 6 pt de spacio despues del parrafo.
                oPara1.Range.InsertParagraphAfter(); // insertamos al final del parrafo.

                // Imprimimos el diseño del estudio
                Word.Paragraph oPara2; // variable que contendrá el primer parrafo
                oPara2 = objWordDocument.Content.Paragraphs.Add(ref oMissing); // añadimos el parrafo al documento
                oPara2.Range.Text = lbAnalysis_M_Desing_EditSsq.Text + " " + tbAnalysisMdesign.Text; // Texto de Informe de medias
                oPara2.Range.Font.Bold = 0; // 
                oPara2.Format.SpaceAfter = 6;    // 6 pt de espacio después del parrafo.
                oPara2.Range.InsertParagraphAfter(); // insertamos al final del parrafo.

                // Imprimimos la tabla de facetas
                wordDocumentInsertTable(ref objWordDocument, ref oEndOfDoc, dgvExAnalysisSourceOfVarSsq,
                    fontTableReport, this.cfgApli.GetShadingRows());

                // Imprimimos la tabla de suma de cuadrados
                wordDocumentInsertTable(ref objWordDocument, ref oEndOfDoc, dgvExAnalysisSSq,
                    fontTableReport, this.cfgApli.GetShadingRows());

                // Imprimimos la tabla de G-Parámetros
                wordDocumentInsertTable(ref objWordDocument, ref oEndOfDoc, dgvExAnalysis_GP,
                    fontTableReport, this.cfgApli.GetShadingRows());

                // Imprimimos la tabla de Optimización
                wordDocumentInsertTable(ref objWordDocument, ref oEndOfDoc, dgvAnalysisResumOpt,
                    fontTableReport, this.cfgApli.GetShadingRows());

                // Imprimos la pestaña de información

                string textInfoSSq = "\n" + lbFileAnalysisProvede.Text + ": " + tbFileAnalysisProvede.Text;
                textInfoSSq = textInfoSSq + "\n" + lbDateAnalysisCreated.Text + ": " + tbDateAnalysisCreated.Text;
                textInfoSSq = textInfoSSq + "\n" + rTextBoxAnalysisInfo.Text;

                Word.Paragraph oPara3; // variable que contendrá el primer parrafo
                oPara3 = objWordDocument.Content.Paragraphs.Add(ref oMissing); // añadimos el parrafo al documento
                oPara3.Range.Text = textInfoSSq; // Texto de Informe de medias
                oPara3.Range.Font.Bold = 0; // 
                oPara3.Format.SpaceAfter = 6;    // 6 pt de spacio despues del parrafo.
                oPara3.Range.InsertParagraphAfter(); // insertamos al final del parrafo.

                // Encabezado y pie de página
                wordDocumentHeaders(objWordDocument, this.fontFootersAndHeaders);
                // wordDocumentFooters(objWordDocument, this.fontFootersAndHeaders);

                wordDocumentFootersWithPageNumber(objWordApplication, this.fontFootersAndHeaders);
                
            }
            else
            {
                // No hay ningún elemento seleccionado, mostramos un mensaje
                // no hay ningún elemento seleccionado.
                ShowMessageErrorOK(txtMessageNoSelected, "", MessageBoxIcon.Stop);
            }// end if (* 1 *)

        }// end WriterAnalysisWordDocument


        /* Descripción:
         *  Pinta la tabla correspondiente al dataGrid que se pasa como parámetro.
         * Parámetros:
         *      Word.Document oDoc: Documento word.
         *      ref Object oEndOfDoc, 
         *      DataGridView dgv: DataGridView que se representara en la tabla.
         *      Font fontReport: Fuente empleada para representar los datos.
         *      bool shadingRows: Indica si se sombrearán o no las filas alternas.
         */
        private void wordDocumentInsertTable(ref Word.Document oDoc, ref Object oEndOfDoc, DataGridView dgv, Font fontReport, bool shadingRows)
        {
            int row = dgv.RowCount;
            int col = dgv.ColumnCount;
            Object oMissing = System.Reflection.Missing.Value;

            //Definimos la tabla
            oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range.InsertParagraphAfter();
            Word.Range wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
            Word.Table table = oDoc.Tables.Add(wrdRng, row+1, col, ref oMissing, ref oMissing);
            table.Range.ParagraphFormat.SpaceAfter = 6;
            
            
            // Rellenamos la cabecera de la tabla
            for (int c = 1; c <= col; c++)
            {
                //Este es el texto que vamos a colocar en cada Celda
                string strText = dgv.Columns[c-1].HeaderText;
                table.Cell(1, c).Range.Text = strText;
            }
            // Rellenamos el resto de la tabla
            for (int r = 1; r <= row; r++)
            {
                for (int c = 1; c <= col; c++)
                {
                    string strText = "";
                    //Este es el texto que vamos a colocar en cada Celda
                    if (dgv.Rows[r - 1].Cells[c - 1].Value != null)
                    {
                        strText = dgv.Rows[r - 1].Cells[c - 1].Value.ToString();
                    }
                    table.Cell(r+1, c).Range.Text = strText;
                    table.Cell(r + 1, c).Range.Bold = 0;
                }

                // Sombreamos las filas
                if (shadingRows && ((r+1) % 2 == 1))
                    table.Rows[r+1].Shading.BackgroundPatternColor = Word.WdColor.wdColorGray10;
            }
            table.Rows[1].Range.Font.Bold = 1;// ponemos en negrita la cabecera de la tabla
            table.Rows[1].Shading.BackgroundPatternColor = Word.WdColor.wdColorGray40;

            // visualización de bordes
            //Visualizando los bordes Verticales y Horizontales
            table.Borders[Word.WdBorderType.wdBorderVertical].Visible=true ;
            table.Borders[Word.WdBorderType.wdBorderHorizontal].Visible = true;
            table.Borders[Word.WdBorderType.wdBorderBottom].Visible = true;
            table.Borders[Word.WdBorderType.wdBorderTop].Visible = true;
            table.Borders[Word.WdBorderType.wdBorderRight].Visible = true;
            table.Borders[Word.WdBorderType.wdBorderLeft].Visible = true;
        }// end wordDocumentInsertTable


        /* Descripción:
         *  Escribe el encabezado de página del documento
         */
        private void wordDocumentHeaders(Word.Document document, Font fontReport)
        {
            foreach (Word.Section section in document.Sections)
            {
                section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary]
                    .Range.Font.Size = fontReport.Size;
                // Introduce el subrallado de la linea
                // section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary]
                //    .Range.Font.Underline = Word.WdUnderline.wdUnderlineSingle; 
                // Introduce un bore inferior
                section.Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary]
                    .Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary]
                    .Range.Text = SAGT + " " + this.version + "\t" + "\t" + UNIV_UMA; //  Universidad de Málaga;
                section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary]
                    .Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;                
            }
        }



        /* Descripción:
         *  Escribe el pie de página del documento
         */
        private void wordDocumentFooters(Word.Document document, Font fontReport)
        {
            foreach (Word.Section wordSection in document.Sections)
            {
                // Word.WdFieldType.wdFieldNumPages.ToString();
                string text = developer + ": " + NAME_STUDENT + "\n" +
                    projectDirector + ": " + NAME_PROJECT_DIRECTOR + "\n" +
                    academicDirector + ": " + NAME_ACADEMIC_DIRECTOR;

                wordSection.Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary]
                    .Range.Font.Size = fontReport.Size; // 20;
                wordSection.Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary]
                    .Range.Text = text;
                // Añadimos el borde superior
                wordSection.Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary]
                    .Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;              
            }
        }


        /* Descripción:
         *  Pie de página con numeración.
         */
        private void wordDocumentFootersWithPageNumber(Word.Application wordApp, Font fontReport)
        {
            object objMissing = System.Reflection.Missing.Value; // Missing.Value;

            wordApp.ActiveWindow.ActivePane.View.SeekView = Word.WdSeekView.wdSeekCurrentPageFooter;
            wordApp.Selection.TypeParagraph();
 
            wordApp.Selection.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            // Introducimos una linea en el borde superior
            wordApp.Selection.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;              
            wordApp.ActiveWindow.Selection.Font.Name = fontReport.Name;
            wordApp.ActiveWindow.Selection.Font.Size = fontReport.Size;
            wordApp.ActiveWindow.Selection.TypeText(developer + ": " + NAME_STUDENT);
            wordApp.ActiveWindow.Selection.TypeText("\t");
            wordApp.ActiveWindow.Selection.TypeText(pag+" ");
            Object CurrentPage = Word.WdFieldType.wdFieldPage;
            wordApp.ActiveWindow.Selection.Fields.Add(wordApp.Selection.Range, ref CurrentPage, ref objMissing, ref objMissing);
            string text = "\n" + projectDirector + ": " + NAME_PROJECT_DIRECTOR + "\n" +
                    academicDirector + ": " + NAME_ACADEMIC_DIRECTOR;

            wordApp.ActiveWindow.Selection.TypeText(text);
            // Las dos lineas siguientes sirven para introducir el total de páginas
            //Object TotalPages = Word.WdFieldType.wdFieldNumPages;
            //wordApp.ActiveWindow.Selection.Fields.Add(wordApp.Selection.Range, ref TotalPages, ref objMissing, ref objMissing);
            wordApp.ActiveWindow.ActivePane.View.SeekView = Word.WdSeekView.wdSeekMainDocument;
        }

    }// end public partial class FormPrincipal : Form
}// end namespace GUI_TG