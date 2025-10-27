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
 * Fecha de revisión: 12/Jun/2011                           
 * 
 * Descripción:
 *      Exporta los datos de un data gridView a Excel usando las librerias de Interoperabilidad.
 */
using AuxMathCalcGT;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUI_GT
{
    public static class ExportExcel
    {
        /// <summary>
        /// Exports multiple DataGridViews to a single Excel workbook with full sheet order control.
        /// </summary>
        /// <param name="sheets">A list of tuples where Item1 = sheet name, Item2 = DataGridView.</param>
        /// <param name="filePath">Output file path (.xlsx)</param>
        public static void ExportMultipleSheets(List<(string SheetName, DataGridView Grid)> sheets, string filePath)
        {
            IWorkbook workbook = new HSSFWorkbook();

            int index = 0;
            foreach (var (sheetName, grid) in sheets)
            {
                ISheet sheet = workbook.CreateSheet(sheetName);
                AddGridToSheet(sheet, grid);

                // Ensure correct order explicitly
                workbook.SetSheetOrder(sheetName, index++);
            }

            // Make first sheet active (optional)
            workbook.SetActiveSheet(0);

            // Save to disk
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
        }

        /// <summary>
        /// Writes a DataGridView into an existing NPOI sheet.
        /// </summary>
        private static void AddGridToSheet(ISheet sheet, DataGridView grid)
        {
            // === Header row ===
            IRow headerRow = sheet.CreateRow(0);
            for (int c = 0; c < grid.Columns.Count; c++)
            {
                headerRow.CreateCell(c).SetCellValue(grid.Columns[c].HeaderText);
            }

            // === Data rows ===
            int rowIndex = 1;
            foreach (DataGridViewRow gridRow in grid.Rows)
            {
                if (gridRow.IsNewRow) continue;

                IRow row = sheet.CreateRow(rowIndex++);
                for (int c = 0; c < grid.Columns.Count; c++)
                {
                    object cellValue = gridRow.Cells[c].Value;

                    string s = cellValue?.ToString() ?? string.Empty;
                    bool result = double.TryParse(s, out _);
                    if (result)
                    {
                        double? d = ConvertNum.String2Double(s);
                        row.CreateCell(c).SetCellValue((double)d);
                    }
                    else
                    {
                        row.CreateCell(c).SetCellValue(s);
                    }
                }
            }

            // === Auto-size columns ===
            for (int i = 0; i < grid.Columns.Count; i++)
                sheet.AutoSizeColumn(i);
        }

    } // public class ExportExcel
}// end namespace GUI_TG
