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

using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.SS.Formula.Functions;
using NPOI.WP.UserModel;
using NPOI.XWPF.UserModel;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

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
                        // comprobamos las pestañas marcadas (reusamos anteriores variables?)
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

                            saveDialog.DefaultExt = "docx"; // extensión por defecto del archivo
                            saveDialog.Filter = "Docx file (*.docx)|*.docx"; // filtro pdf inicado en las variables globales 
                            saveDialog.OverwritePrompt = true; // muestra advertencia si el fichero ya existe
                            saveDialog.AddExtension = true;
                            System.Windows.Forms.DialogResult resulDialog = saveDialog.ShowDialog();
                            if (resulDialog == DialogResult.OK)
                            {
                                WriterSagtWordDocument(saveDialog.FileName);
                                salir = true;

                                Process.Start(saveDialog.FileName);
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
        }// end SelectElementsAndCreateWordDocument

        /* Descripción:
         *  Genera un informe en Word a partir de los datos seleccionados para el informe.
         */
        private void WriterSagtWordDocument(string outputPath)
        {
            Font fontTableReport = new Font(this.cfgApli.GetTableFontFamily(), this.cfgApli.GetTableFontSize());

            // Crear documento Word
            XWPFDocument doc = new XWPFDocument();

            // ===== Header ===== 
            XWPFHeader header = doc.CreateHeader(HeaderFooterType.DEFAULT);

            XWPFParagraph headerParagraph = header.CreateParagraph();

            // Calculate usable width inside margins (necessary for page-wide tabulators)
            CT_SectPr sectPr = doc.Document.body.sectPr;
            ulong pageWidthTwips = sectPr.pgSz.w;
            ulong leftMarginTwips = sectPr.pgMar.left;
            ulong rightMarginTwips = sectPr.pgMar.right;
            ulong usableWidth = pageWidthTwips - leftMarginTwips - rightMarginTwips;

            // Configure tab
            CT_Tabs tabs = headerParagraph.GetCTP().pPr.AddNewTabs();
            CT_TabStop rightTab = tabs.AddNewTab();
            rightTab.val = ST_TabJc.right;
            rightTab.pos = usableWidth.ToString();

            // Add left text
            XWPFRun run1 = headerParagraph.CreateRun();
            run1.FontSize = (int)this.fontFootersAndHeaders.Size;
            run1.Underline = UnderlinePatterns.Single;
            run1.SetText(SAGT + this.version);

            // Add tab
            run1.AddTab();

            // Add right text
            XWPFRun run2 = headerParagraph.CreateRun();
            run2.FontSize = (int)this.fontFootersAndHeaders.Size;
            run2.Underline = UnderlinePatterns.Single;
            run2.SetText(UNIV_UMA);

            // ===== Fecha de emisión =====

            string strDate = (DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString()).Trim();

            XWPFParagraph paragraph = doc.CreateParagraph();

            // Configure tab
            tabs = paragraph.GetCTP().pPr.AddNewTabs();
            rightTab = tabs.AddNewTab();
            rightTab.val = ST_TabJc.right;
            rightTab.pos = usableWidth.ToString();

            // Add left text
            run1 = paragraph.CreateRun();
            run1.SetText(issuanceOfReport + ": ");

            // Add tab
            run1.AddTab();

            // Add right text
            run2 = paragraph.CreateRun();
            run2.SetText(strDate);

            /* // Invisible 2 cell table version (works badly in the header)
            // Create a table with 1 row and 2 cells
            table = doc.CreateTable(1, 2);

            // Set table to full page width (approx full width in twentieths of a point)
            table.Width = 5000;

            // Remove all borders
            table.SetInsideHBorder(XWPFTable.XWPFBorderType.NONE, 0, 0, "FFFFFF");
            table.SetInsideVBorder(XWPFTable.XWPFBorderType.NONE, 0, 0, "FFFFFF");
            table.SetLeftBorder(XWPFTable.XWPFBorderType.NONE, 0, 0, "FFFFFF");
            table.SetRightBorder(XWPFTable.XWPFBorderType.NONE, 0, 0, "FFFFFF");
            table.SetTopBorder(XWPFTable.XWPFBorderType.NONE, 0, 0, "FFFFFF");
            table.SetBottomBorder(XWPFTable.XWPFBorderType.NONE, 0, 0, "FFFFFF");

            // Left cell:
            table.GetRow(0).GetCell(0).RemoveParagraph(0);
            leftPara = table.GetRow(0).GetCell(0).AddParagraph();
            leftPara.Alignment = ParagraphAlignment.LEFT;
            leftRun = leftPara.CreateRun();
            leftRun.SetText(issuanceOfReport + ": ");

            // Right cell:
            table.GetRow(0).GetCell(1).RemoveParagraph(0);
            rightPara = table.GetRow(0).GetCell(1).AddParagraph();
            rightPara.Alignment = ParagraphAlignment.RIGHT;
            rightRun = rightPara.CreateRun();
            rightRun.SetText(strDate);*/

            // ===== Tablas de datos =====
            if (bData)
            {
                AddParagraph(doc, lbFileData.Text + ": " + tbFileName.Text);
                AddParagraph(doc, lbCommentsData.Text + ": " + tbDescription.Text);

                // Tabla de facetas
                InsertTable(doc, dataGridViewExFacets, fontTableReport, this.cfgApli.GetShadingRows());

                // Tabla de frecuencias
                InsertTable(doc, dataGridViewExObsTable, fontTableReport, this.cfgApli.GetShadingRows());

                // Comentarios
                string textDataInfoComment = richTextBoxDataComment.Text ?? "";
                AddParagraph(doc, textDataInfoComment, spacingAfter: 24);
            }

            // ===== Tablas de medias =====
            if (bMeans)
            {
                AddParagraph(doc, titleMeansReport, spacingAfter: 6);

                int n = this.tabControlMeans.TabPages.Count - 1;
                for (int i = 0; i < n; i++)
                {
                    TabPageMeansEx tabPageMeansEx = (TabPageMeansEx)tabControlMeans.TabPages[i];

                    AddParagraph(doc, stringTable + ": " + tabPageMeansEx.Text, spacingAfter: 6);

                    DataGridView dgvTableMeans = tabPageMeansEx.GetDataGridViewEx();
                    InsertTable(doc, dgvTableMeans, fontTableReport, this.cfgApli.GetShadingRows());

                    AddParagraph(doc, tabPageMeansEx.GetLabelGrandMean().Text, spacingAfter: 24);
                }

                string textMeansInfo = "\n" + lbFileMeanProvede.Text + ": " + tbFileMeanProvede.Text;
                textMeansInfo += "\n" + lbDateMeanCreated.Text + ": " + tbDateMeanCreated.Text;
                textMeansInfo += "\n" + rTxtBoxMeanInfo.Text;
                AddParagraph(doc, textMeansInfo, spacingAfter: 24);
            }

            // ===== Tablas Suma de Cuadrados =====
            if (bSsq)
            {
                AddParagraph(doc, titleSsqReport, spacingAfter: 6);
                AddParagraph(doc, lbMeasurementDesign.Text + " " + tbMeasurementDesign.Text, spacingAfter: 6);

                InsertTable(doc, dGridViewExSourceOfVar, fontTableReport, this.cfgApli.GetShadingRows());
                InsertTable(doc, dataGridViewExSSQ, fontTableReport, this.cfgApli.GetShadingRows());
                InsertTable(doc, dGridViewExG_Parameters, fontTableReport, this.cfgApli.GetShadingRows());
                InsertTable(doc, dGridViewExOptimizationResum, fontTableReport, this.cfgApli.GetShadingRows());

                string textInfoSSq = "\n" + lbNameFileSsqInfo.Text + ": " + tbNameFileSsqInfo.Text;
                textInfoSSq += "\n" + lbDateFileSsqInfo.Text + ": " + tbDateFileSsqInfo.Text;
                textInfoSSq += "\n" + richTextBoxSsqComment.Text;
                AddParagraph(doc, textInfoSSq, spacingAfter: 6);
            }

            // ===== Footer con información =====
            XWPFFooter footer = doc.CreateFooter(HeaderFooterType.DEFAULT);

            // Footer paragraph
            XWPFParagraph footerPara = footer.CreateParagraph();
            footerPara.BorderTop = Borders.Single;

            // Setup right-aligned tab stop at the usable width
            CT_PPr pPr = footerPara.GetCTP().IsSetPPr() ? footerPara.GetCTP().pPr : footerPara.GetCTP().AddNewPPr();
            tabs = pPr.AddNewTabs();
            rightTab = tabs.AddNewTab();
            rightTab.val = ST_TabJc.right;
            rightTab.pos = usableWidth.ToString(); // twips inside margins

            // === Line 1 ===
            XWPFRun runFooter = footerPara.CreateRun();
            runFooter.SetText(developer + ": " + NAME_STUDENT);
            runFooter.AddBreak();

            // === Line 2 ===
            XWPFRun runLine2 = footerPara.CreateRun();
            runLine2.SetText(projectDirector + ": " + NAME_PROJECT_DIRECTOR);
            runLine2.AddTab(); // jump to right tab

            // === 'Page X of Y' inside Line 2 ===
            CT_P ctP = footerPara.GetCTP();

            // "Page "
            CT_R ctR1 = ctP.AddNewR();
            ctR1.AddNewT().Value = "Página ";
            CT_RPr rPr1 = ctR1.AddNewRPr();
            rPr1.AddNewSz().val = (ulong)(this.fontFootersAndHeaders.Size * 2);

            // Insert PAGE field
            CT_R rNumPage = ctP.AddNewR();
            rNumPage.AddNewFldChar().fldCharType = ST_FldCharType.begin;
            CT_Text NumPage_instr = rNumPage.AddNewInstrText();
            NumPage_instr.Value = " PAGE ";
            rNumPage.AddNewFldChar().fldCharType = ST_FldCharType.end;
            CT_RPr rNumPr = rNumPage.AddNewRPr();
            rNumPr.AddNewSz().val = (ulong)(this.fontFootersAndHeaders.Size * 2);

            // " of "
            CT_R ctR2 = ctP.AddNewR();
            ctR2.AddNewT().Value = " de ";
            CT_RPr rPr2 = ctR2.AddNewRPr();
            rPr2.AddNewSz().val = (ulong)(this.fontFootersAndHeaders.Size * 2);

            // Insert NUMPAGES field
            CT_R rNumPages = ctP.AddNewR();
            rNumPages.AddNewFldChar().fldCharType = ST_FldCharType.begin;
            CT_Text NumPages_instr = rNumPages.AddNewInstrText();
            NumPages_instr.Value = " NUMPAGES ";
            rNumPages.AddNewFldChar().fldCharType = ST_FldCharType.end;
            CT_RPr rNumsPr = rNumPages.AddNewRPr();
            rNumsPr.AddNewSz().val = (ulong)(this.fontFootersAndHeaders.Size * 2);

            // Add line break after this line
            XWPFRun brRun = footerPara.CreateRun();
            brRun.AddBreak();

            // === Line 3 ===
            XWPFRun runLine3 = footerPara.CreateRun();
            runLine3.SetText(academicDirector + ": " + NAME_ACADEMIC_DIRECTOR);

            // Consistent font size for footer
            runFooter.FontSize = runLine2.FontSize = runLine3.FontSize = (int)this.fontFootersAndHeaders.Size;

            // ===== Guardar archivo =====
            using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))   //NOTA: Causa excepción autoexplicativa si tenemos abierto el archivo donde vamos a escribir
            {
                doc.Write(fs);
            }
        }

        // ===== Métodos auxiliares =====
        void AddParagraph(XWPFDocument doc, string text, int spacingAfter = 0)
        {
            var lines = text.Split(new[] { '\n' }, StringSplitOptions.None);    //a NPOI no le gusta el uso de \n, preferimos dividir los párrafos nosotros mismos
                                                                                //Como alternativa a múltiples párrafos, podríamos haber empleado setText("text", number) + run.AddBreak()
            foreach (var line in lines)
            {
                XWPFParagraph p = doc.CreateParagraph();
                if (spacingAfter > 0)
                    p.SpacingAfter = spacingAfter;

                XWPFRun run = p.CreateRun();
                run.SetText(line);
            }
        }

        private void InsertTable(XWPFDocument doc, DataGridView dgv, Font font, bool shadingRows)
        {
            int rowCount = dgv.RowCount;
            int colCount = dgv.ColumnCount;

            // Create a new table with header row
            XWPFTable table = doc.CreateTable(rowCount + 1, colCount);
            

            // Make table occupy full width
            table.Width = 5000; // 5000 = 100% full width of the page (PCT)
            var tblLayout1 = table.GetCTTbl().tblPr.AddNewTblLayout();  
            tblLayout1.type = ST_TblLayoutType.@fixed;  //disable autofit
            //ulong colWidth = 5000UL / (ulong)colCount; //all columns take equal space

            // Header row
            for (int c = 0; c < colCount; c++)
            {
                XWPFTableCell cell = table.GetRow(0).GetCell(c);
                cell.SetText(dgv.Columns[c].HeaderText);

                // Bold header
                cell.RemoveParagraph(0);    //remove default paragraph
                XWPFParagraph paragraph = cell.AddParagraph();
                XWPFRun run = paragraph.CreateRun();
                run.IsBold = true;
                run.SetText(dgv.Columns[c].HeaderText); //Replace it with a bold one



                // Header background color
                cell.SetColor("A9A9A9"); // Dark gray

            }

            // Data rows
            for (int r = 0; r < rowCount; r++)
            {
                for (int c = 0; c < colCount; c++)
                {
                    XWPFTableCell cell = table.GetRow(r + 1).GetCell(c);
                    string text = dgv.Rows[r].Cells[c].Value?.ToString() ?? "";

                    

                    XWPFParagraph paragraph = cell.Paragraphs[0];
                    paragraph.RemoveRun(0); // Remove any existing default run
                    XWPFRun run = paragraph.CreateRun();
                    run.SetText(text);

                    // Optional alternating row shading
                    if (shadingRows && r % 2 == 0)
                    {
                        cell.SetColor("D3D3D3"); // Light gray
                    }
                }
            }

            // Add an empty paragraph after the table
            XWPFParagraph spacer = doc.CreateParagraph();
            spacer.SpacingAfter = 6;   // small gap
        }
    }// end public partial class FormPrincipal : Form
}// end namespace GUI_TG