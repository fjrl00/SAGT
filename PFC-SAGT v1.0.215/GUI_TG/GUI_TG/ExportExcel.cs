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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using myExcel = Microsoft.Office.Interop.Excel; // permite interactuar con ficheros Excel
using System.Reflection; //para el valor missing
using AuxMathCalcGT;

namespace GUI_GT
{
    public class ExportExcel
    {
        myExcel.Application xlsApp;

        /* Descripción:
         *  Contructor de la clase.
         */
        public ExportExcel()
        {
            try
            {
                xlsApp = new myExcel.Application();//creo una aplicación Excel
                xlsApp.DisplayAlerts = false;
                myExcel.Worksheet xlsSheet; //creo una hoja
                myExcel.Workbook xlsBook; //creo un libro
                xlsApp.Visible = false; //la aplicación no es visible
                xlsBook = xlsApp.Workbooks.Add(true);//añado el libro a la aplicación
                xlsSheet = (myExcel.Worksheet)xlsBook.ActiveSheet; //activo la hoja, para el libro
            }
            catch(Exception e)
            {
                throw e;
            }
        }


        /* Descripción:
         *  Guarda los datos del dataGrid una nueva hoja que se añade al principio.
         */
        public void addNewXlsWorksheet(System.Windows.Forms.DataGridView dgvConsulta, string title)
        {
            try
            {
                myExcel.Worksheet xlsSheet; // Creo una hoja
                // myExcel.Workbook xlsBook = this.xlsApp.ActiveWorkbook;
                // xlsSheet = (myExcel.Worksheet)xlsBook.ActiveSheet; //activo la hoja, para el libro

                // Excel.Worksheet newWorksheet;
                xlsSheet = (myExcel.Worksheet)this.xlsApp.Worksheets.Add(
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                xlsSheet.Name = title;// Asignamos el nombre de la hoja

                int iColumnas = 0;
                int numCol = dgvConsulta.ColumnCount;
                // introducimos los titulos de las columnas
                for (int i = 1; i <= numCol; i++)
                {
                    xlsSheet.Cells[1, i] = dgvConsulta.Columns[i-1].HeaderText;
                    iColumnas++;
                }

                // Ahora copiamos el contenido de la tabla
                int numRow = dgvConsulta.RowCount;
                for (int j = 1; j <= numRow; j++)
                {
                    System.Windows.Forms.DataGridViewRow my_row = dgvConsulta.Rows[j - 1];
                    for (int i = 1; i <= numCol; i++)
                    {
                        if (my_row.Cells[i - 1].Value != null)
                        {
                            string s = my_row.Cells[i - 1].Value.ToString();
                            double r = 0;
                            bool result = double.TryParse(s, out r);
                            if (result)
                            {
                                double? d = ConvertNum.String2Double(s);
                                if (d!= null && !double.IsNaN((double)d))
                                {
                                    xlsSheet.Cells[j + 1, i] = d;
                                }
                                else
                                {
                                    xlsSheet.Cells[j + 1, i] = s;
                                }
                            }
                            else
                            {
                                xlsSheet.Cells[j + 1, i] = s;
                            }
                        }
                        iColumnas++;
                    }
                }

                //Definir el rango y aplicarle un formato.
                // myExcel.Range rango = xlsSheet.get_Range(xlsSheet.Cells[1, 1], xlsSheet.Cells[dgvConsulta.Rows.Count + 3, iColumnas]);
                // rango.Cells.AutoFormat(myExcel.XlRangeAutoFormat.xlRangeAutoFormatList1, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                xlsSheet.Columns.AutoFit(); //Ajusta ancho de todas las columnas
                // xlsApp.Visible = true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /* Descripción:
         *  Guarda los datos del dataGrid una nueva hoja que se añade al principio.
         */
        public void addXlsWorksheet(System.Windows.Forms.DataGridView dgvConsulta, string title)
        {
            try
            {
                myExcel.Worksheet xlsSheet; // Creo una hoja
                myExcel.Workbook xlsBook = this.xlsApp.ActiveWorkbook;
                xlsSheet = (myExcel.Worksheet)xlsBook.ActiveSheet; //activo la hoja, para el libro

                xlsSheet.Name = title;// Asignamos el nombre de la hoja

                int iColumnas = 0;
                int numCol = dgvConsulta.ColumnCount;
                // introducimos los titulos de las columnas
                for (int i = 1; i <= numCol; i++)
                {
                    xlsSheet.Cells[1, i] = dgvConsulta.Columns[i - 1].HeaderText;
                    iColumnas++;
                }

                // Ahora copiamos el contenido de la tabla
                int numRow = dgvConsulta.RowCount;
                for (int j = 1; j <= numRow; j++)
                {
                    System.Windows.Forms.DataGridViewRow my_row = dgvConsulta.Rows[j - 1];
                    for (int i = 1; i <= numCol; i++)
                    {
                        if (my_row.Cells[i - 1].Value != null)
                        {
                            //string s = my_row.Cells[i - 1].Value.ToString();
                            //double? d = ConvertNum.String2Double(s);
                            //xlsSheet.Cells[j + 1, i] = d;// my_row.Cells[i - 1].Value.ToString();

                            string s = my_row.Cells[i - 1].Value.ToString();
                            double r = 0;
                            bool result = double.TryParse(s, out r);
                            if (result)
                            {
                                double? d = ConvertNum.String2Double(s);
                                xlsSheet.Cells[j + 1, i] = d;
                            }
                            else
                            {
                                xlsSheet.Cells[j + 1, i] = s;
                            }
                        }
                        iColumnas++;
                    }
                }

                //Definir el rango y aplicarle un formato.
                // myExcel.Range rango = xlsSheet.get_Range(xlsSheet.Cells[1, 1], xlsSheet.Cells[dgvConsulta.Rows.Count + 3, iColumnas]);
                // rango.Cells.AutoFormat(myExcel.XlRangeAutoFormat.xlRangeAutoFormatList1, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                xlsSheet.Columns.AutoFit(); //Ajusta ancho de todas las columnas
                //xlsApp.Visible = true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /* Descripción:
         *  Salvar en un archivo.
         */
        public void saveFileExcel(string nameFile)
        {
            // this.xlsApp.Save(nameFile);
            // this.xlsApp.ActiveWorkbook.SaveCopyAs(nameFile);
            Missing missing = Missing.Value;
            this.xlsApp.ActiveWorkbook.SaveAs(nameFile,
                missing, missing, missing, missing, missing,
                myExcel.XlSaveAsAccessMode.xlNoChange, missing, missing, missing,
                missing, missing);
            /// this.xlsApp.ActiveWorkbook.Saved = true;
        }

        /* Descripción:
         *  Cerrar aplicación Excel
         */
        public void aplicationExcelQuit()
        {
            this.xlsApp.Quit();
        }

        public void exportaExcel(System.Windows.Forms.DataGridView dgvConsulta)
        {
            try
            {
                string temp;
                int iColumnas = 0;
                myExcel.Application xlsApp = new myExcel.Application();//creo una aplicación Excel
                xlsApp.DisplayAlerts = false;
                myExcel.Worksheet xlsSheet; //creo una hoja
                myExcel.Workbook xlsBook; //creo un libro
                xlsApp.Visible = false; //la aplicación no es visible
                xlsBook = xlsApp.Workbooks.Add(true);//añado el libro a la aplicación
                xlsSheet = (myExcel.Worksheet)xlsBook.ActiveSheet; //activo la hoja, para el libro
                //titulo
                xlsSheet.Cells[1, 1] = "UTM";
                xlsSheet.Cells[2, 1] = "Lista de Alumnos";
                for (int iCol = 0; iCol < dgvConsulta.Columns.Count; iCol++)
                {
                    if (dgvConsulta.Columns[iCol].Visible == true)
                    {
                        xlsSheet.Cells[3, iCol + 1] = dgvConsulta.Columns[iCol].HeaderText;
                        iColumnas++;
                    }
                }

                for (int iRow = 0; iRow < dgvConsulta.Rows.Count; iRow++)
                {
                    for (int iCol = 0; iCol < dgvConsulta.Columns.Count; iCol++)
                    {
                        if (dgvConsulta.Columns[iCol].Visible == true)
                        {
                            temp = dgvConsulta[iCol, iRow].Value.ToString();
                            xlsSheet.Cells[iRow + 4, iCol + 1] = temp;
                        }
                    }
                }
                //Definir el rango y aplicarle un formato.
                myExcel.Range rango = xlsSheet.get_Range(xlsSheet.Cells[1, 1], xlsSheet.Cells[dgvConsulta.Rows.Count + 3, iColumnas]);
                rango.Cells.AutoFormat(myExcel.XlRangeAutoFormat.xlRangeAutoFormatList1, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                xlsSheet.Columns.AutoFit(); //Ajusta ancho de todas las columnas
                // xlsApp.Visible = true;
                // xlsApp.Quit();
            }
            catch (Exception e)
            {
                throw e;
            }

        }

    } // public class ExportExcel
}// end namespace GUI_TG
