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
 * Fecha de revisión: 16/May/2012                           
 * 
 * Descripción:
 *      Clase que hereda de iTextSharp.text.pdf.PdfPageEventHelper. Su cometido es añadir al los documentos
 *      PDF encabezado y pie de página de una manera sencilla.
 */

using System;
using System.Web;
using System.Windows.Forms;
// iTextSharp
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace myApp.ns.pages
{
    public class pdfPage : iTextSharp.text.pdf.PdfPageEventHelper
    {
        /*=================================================================================
         * Variables
         *=================================================================================*/
        // Variables
        private string sagt;
        private string uma;
        private string developer;
        private string projectDirector;
        private string academicDirector;
        private string page;
        

        /*=================================================================================
         * Constructores redefinidos
         *=================================================================================*/
        public pdfPage(string sagt, string uma, string developer, string projectDirector, 
            string academicDirector, string page)
            :base()

        {
            this.sagt = sagt;
            this.uma = uma;
            this.developer = developer;
            this.projectDirector = projectDirector;
            this.academicDirector = academicDirector;
            this.page = page;
        }

        /*=================================================================================
         * Métodos
         *=================================================================================*/

        //I create a font object to use within my footer
        protected Font footer
        {
            get
            {
                // create a basecolor to use for the footer font, if needed.
                BaseColor grey = new BaseColor(128, 128, 128);
                Font font = FontFactory.GetFont("Verdana", 9, Font.NORMAL, grey);
                return font;
            }
        }


        //override the OnStartPage event handler to add our header
        public override void OnStartPage(PdfWriter writer, Document doc)
        {
            //I use a PdfPtable with 1 column to position my header where I want it
            PdfPTable headerTbl = new PdfPTable(2);

            //set the width of the table to be the same as the document
            headerTbl.TotalWidth = doc.PageSize.Width - (50 * 2);
            // footerTbl.WidthPercentage = 80;

            //Center the table on the page
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            Paragraph para = new Paragraph(sagt + " ", footer);

            //create a cell instance to hold the text
            PdfPCell cell = new PdfPCell(para);

            //set cell border to 0
            cell.Border = 0;
            cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER; // añadimos el borde inferior a la primera celda

            //add some padding to bring away from the edge
            cell.PaddingLeft = 10;

            //add cell to table
            headerTbl.AddCell(cell);
            //create new instance of Paragraph for 2nd cell text
            para = new Paragraph(uma, footer);

            //create new instance of cell to hold the text
            cell = new PdfPCell(para);

            //align the text to the right of the cell
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //set border to 0
            cell.Border = 0;
            cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER; // añadimos el borde inferior a la 2º celda

            // add some padding to take away from the edge of the page
            cell.PaddingRight = 10;

            //add the cell to the table
            headerTbl.AddCell(cell);

            //write the rows out to the PDF output stream. I use the height of the document to position the table. Positioning seems quite strange in iTextSharp and caused me the biggest headache.. It almost seems like it starts from the bottom of the page and works up to the top, so you may ned to play around with this.
            headerTbl.WriteSelectedRows(0, -1, 50, (doc.PageSize.Height - 30), writer.DirectContent);
        }// end OnStartPage


        //override the OnPageEnd event handler to add our footer
        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            //I use a PdfPtable with 2 columns to position my footer where I want it
            PdfPTable footerTbl = new PdfPTable(2);

            //set the width of the table to be the same as the document
            footerTbl.TotalWidth = doc.PageSize.Width - (50*2);

            //Center the table on the page
            footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            
            Paragraph para = new Paragraph(developer, footer);

            //add a carriage return
            para.Add(Environment.NewLine);
            para.Add(projectDirector);
            para.Add(Environment.NewLine);
            para.Add(academicDirector);

            //create a cell instance to hold the text
            PdfPCell cell = new PdfPCell(para);

            //set cell border to 0
            cell.Border = 0;
            cell.Border = iTextSharp.text.Rectangle.TOP_BORDER; // añadimos el borde superior a la primera celda

            //add some padding to bring away from the edge
            cell.PaddingLeft = 10;

            //add cell to table
            footerTbl.AddCell(cell);

            // Numero de página
            int pageNumber = writer.PageNumber;

            //create new instance of Paragraph for 2nd cell text
            para = new Paragraph(page + " " + Convert.ToString(pageNumber), footer);

            //create new instance of cell to hold the text
            cell = new PdfPCell(para);

            //align the text to the right of the cell
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //set border to 0
            cell.Border = 0; 
            cell.Border = iTextSharp.text.Rectangle.TOP_BORDER; // añadimos el borde superior a la 2º celda

            // add some padding to take away from the edge of the page
            cell.PaddingRight = 10;

            //add the cell to the table
            footerTbl.AddCell(cell);

            //write the rows out to the PDF output stream.
            footerTbl.WriteSelectedRows(0, -1, 50, (doc.BottomMargin-5), writer.DirectContent);
        }// end OnEndPage

    }// end class pdfPage : iTextSharp.text.pdf.PdfPageEventHelper
}// end namespace myApp.ns.pages
