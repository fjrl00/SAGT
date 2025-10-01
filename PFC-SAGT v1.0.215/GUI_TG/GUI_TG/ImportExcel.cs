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
 * Fecha de revisión: 01/Mar/2012
 * 
 * Descripción:
 *      Exporta los datos de un data gridView a Excel usando las librerias de Interoperabilidad.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADOX;
using System.Data;

namespace GUI_GT
{
    public class ImportExcel
    {
        public static DataTable GetDataTableExcel(string strFileName, string Table)
        {
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source = " + strFileName + "; Extended Properties = \"Excel 8.0;HDR=Yes;IMEX=1\";");
            conn.Open();
            string strQuery = "SELECT * FROM [" + Table + "]";
            System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter(strQuery, conn);
            System.Data.DataSet ds = new System.Data.DataSet();
            adapter.Fill(ds);
            return ds.Tables[0];
        }


        public static List<string> GetTableExcel(string strFileName)
        {
            // string[] strTables = new string[100];
            List<string> strTables = new List<string>();
            Catalog oCatlog = new Catalog();
            ADOX.Table oTable = new ADOX.Table();
            // ADODB.Connection oConn = new ADODB.Connection();
            ADODB.Connection oConn = new ADODB.Connection();
            //oConn.Open("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + strFileName + 
            //    "; Extended Properties = \"Excel 8.0;HDR=Yes;IMEX=1\";", "", "", 0);
            oConn.Open(
                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFileName +
                ";Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\";"
);
            oCatlog.ActiveConnection = oConn;
            if (oCatlog.Tables.Count > 0)
            {
                // int item = 0;
                foreach (ADOX.Table tab in oCatlog.Tables)
                {
                    if (tab.Type == "TABLE")
                    {
                        strTables.Add(tab.Name);
                        // item++;
                    }
                }
            }
            return strTables;
        }

    }// end ImportExcel
}// end GUI_TG
