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
using ADOX;
using AuxMathCalcGT;
using MultiFacetData;
using ProjectMeans;
using ProjectSSQ;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TransLibrary;

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

        #region Importar los datos de un archivo Excel
        /*================================================================================================
         * Importa los datos de un archivo Excel
         * 
         * NOTA:
         *  Para ello emplea ADOX y oledb
         *================================================================================================*/

        public static MultiFacetsObs ImportFileXLS_to_MultiFacetsObs(string path)
        {
            List<string> namesTables = ImportExcel.GetTableExcel(path);
            if (namesTables.Count != 2)
            {
                // No esta en el formato correcto
                throw new ObsTableException();
            }

            // la primera tabla debe contener las facetas.
            DataTable dtFacets = ImportExcel.GetDataTableExcel(path, namesTables[0]);
            ListFacets lf = DataTable2ListFacets(dtFacets);
            MultiFacetsObs mfo = new MultiFacetsObs(lf, path, "");
            // La segunda tabla debe contener la tabla de frecuencias
            DataTable dtObsTable = ImportExcel.GetDataTableExcel(path, namesTables[1]);
            InterfaceObsTable obsTb = mfo.ObservationTable();
            obsTb = DataTable2Observation(dtObsTable, obsTb);
            mfo.ObservationTable(obsTb);

            return mfo;
        }

        /* Descripción:
         *  Toma los datos de un DataTable y genera con ellos una lista de facetas
         *  
         *  PUBLIC ONLY TEMPORARILY
         */
        public static ListFacets DataTable2ListFacets(DataTable dt)
        {
            ListFacets lf = null;
            try
            {
                lf = new ListFacets();

                int r = dt.Rows.Count;

                for (int i = 0; i < r; i++)
                {
                    DataRow row = dt.Rows[i];

                    string design = (string)row[0].ToString();
                    string name = ExtractNameOfDesign(design);
                    int level = int.Parse((string)row[1].ToString());
                    int size = int.MaxValue;
                    string stSize = ((string)row[2].ToString()).Trim();
                    if (!stSize.Equals(Facet.INFINITE))
                    {
                        size = int.Parse(stSize);
                    }
                    string description = (string)row[3].ToString();

                    Facet f = new Facet(name, level, description, size, design);
                    lf.Add(f);
                }
            }
            catch (FormatException e)
            {
                // contiene un campo incorrecto
                throw e;
            }
            return lf;
        }// end DataTable2ListFacets


        /* Descripción:
         *  Método auxiliar. De un diseño obtiene el nombre de la faceta de la faceta
         *  
         *  SHOULD MOVE THIS TO FACET.CS
         */
        private static string ExtractNameOfDesign(string design)
        {
            int posI = design.IndexOf("[");
            int posf = design.IndexOf("]");
            return (design.Substring(posI + 1, posf - 1));
        }


        /* Descripción:
         *  Devuelve la lista de observaciones leidas de un datatable
         */
        private static InterfaceObsTable DataTable2Observation(DataTable dt, InterfaceObsTable obsTb)
        {
            InterfaceObsTable res = null;
            try
            {
                res = obsTb;
                int r = dt.Rows.Count;
                int c = dt.Columns.Count;

                for (int i = 0; i < r; i++)
                {
                    DataRow row = dt.Rows[i];
                    double? d = ConvertNum.String2Double((string)row[c - 1].ToString());
                    res.Data(d, i);
                }
            }
            catch (FormatException)
            {
                // Hemos cometido un error al leer
                throw new ObsTableException();
            }
            return res;
        }

        #endregion Importar los datos de un archivo Excel

        #region Importar medias

        public static ListMeans ImportFileXLS_to_ListMeans(string path, WordTranslation tras)
        {
            List<string> namesTables = ImportExcel.GetTableExcel(path);
            if (namesTables.Count < 2)
            {
                // No esta en el formato correcto
                throw new ListMeansException();
            }
            
            ListMeans lm = new ListMeans();
            int n = namesTables.Count;
            List<string> namesTablesaux = ImportExcel.GetTableExcel(path);
            DataTable dtGrandMeans = ImportExcel.GetDataTableExcel(path, "Grand Mean$");

            for (int i = 0; i < n; i++)
            {
                string nameTable = namesTables[i];
                if (!nameTable.Contains("Grand Mean"))
                {
                    DataTable dtMeansTable = ImportExcel.GetDataTableExcel(path, nameTable);

                    string tbDesign = dtGrandMeans.Rows[i - 1][1].ToString();
                    double? gm = ConvertNum.String2Double((string)dtGrandMeans.Rows[i - 1][2].ToString());
                    double? variance = ConvertNum.String2Double((string)dtGrandMeans.Rows[i - 1][3].ToString());
                    double? stdDev = ConvertNum.String2Double((string)dtGrandMeans.Rows[i - 1][4].ToString());
                    InterfaceTableMeans tbMeans = ImportExcel.DataTable2TableMeans(dtMeansTable, gm, variance, stdDev, tbDesign, tras);
                    lm.Add(tbMeans);
                }
            }
            lm.SetNameFileDataCreation(path);
            DateTime date = DateTime.Now;
            lm.SetDateTime(date);
            

            return lm;
        }

        /* Descripción:
         *  Devuelve una tabla de medias
         */
        public static InterfaceTableMeans DataTable2TableMeans(DataTable dt, double? grandMean,
            double? variance, double? stdDev, string tbDesign, TransLibrary.WordTranslation trans)
        {
            InterfaceTableMeans tm = null;

            /* Necesito averiguar que tipo de tabla de medias es. Lo averiguare mediante la posición
             * de la columna media.
             */
            int r = dt.Rows.Count;
            int c = dt.Columns.Count;
            int pos = c - 1;

            bool found = false;

            while (pos >= 0 && !found)
            {
                string sMeans = dt.Columns[pos].ColumnName;
                found = trans.TranslationIncluded(sMeans);
                if (!found)
                {
                    pos--;
                }
            }

            // Si es pos == c-4 entonces medias por defecto
            if (pos == (c - 3))
            {
                tm = new TableMeans(dt, grandMean, variance, stdDev, tbDesign);
            }
            // Si es pos == c-5 entonces puntuación típica
            if (pos == (c - 5))
            {
                tm = new TableMeansTypScore(dt, grandMean, variance, stdDev, tbDesign);
            }
            // Si es pos == c-6 entonces medias de las desviaciones
            if (pos == (c - 6))
            {
                tm = new TableMeansDif(dt, grandMean, variance, stdDev, tbDesign);
            }

            return tm;
        }// end DataTable2TableMeans

        #endregion Importar medias

        #region Importar cuadrados

        /* Descripción:
         *  Importa un fichero con los resultado de suma de cuadrados .xls de excel generados a partir
         *  de este mismo programa.
         */
        public static Analysis_and_G_Study ImportFileXLS_to_AAGS(string path,
            WordTranslation transFacets, WordTranslation transSSq, WordTranslation transG_p, WordTranslation transResum)
        {
            List<string> namesTables = ImportExcel.GetTableExcel(path);
            Analysis_and_G_Study tAnalysisSsq = null;
            if (namesTables.Count != 4)
            {
                // No esta en el formato correcto
                throw new Analysis_and_G_Study_Exception();
            }
            
            ListFacets lf = null;
            TableAnalysisOfVariance tableAnalysis = null;
            TableG_Study_Percent tableG = null;
            List<G_ParametersOptimization> tableResum = null;


            string nameTableFacets = ""; // Contendrá el nombre de la tabla de facetas
            string nameTableAnalysisOfVariance = ""; // Contendrá el nombre de la tabla de análisis de varianza
            string nameTableG_p = ""; // Contendrá el nombre de la tabla G Study
            string nameTableResum = ""; // Contendrá el nombre de la tabla de optimización

            int n = namesTables.Count;

            for (int i = 0; i < n; i++)
            {
                string nameTable = namesTables[i];
                if (nameTable[0].Equals('\''))
                {
                    nameTable = nameTable.Substring(1);
                    if (nameTable.LastIndexOf('\'') == (nameTable.Length - 1))
                    {
                        nameTable = nameTable.Remove((nameTable.Length - 1));
                    }
                }
                int num = nameTable.LastIndexOf("$");
                string auxNameTable = nameTable.Remove(num);// eliminamos la última posición

                if (transFacets.TranslationIncluded(auxNameTable))
                {
                    nameTableFacets = nameTable;
                }

                if (transSSq.TranslationIncluded(auxNameTable))
                {
                    nameTableAnalysisOfVariance = nameTable;
                }

                if (transG_p.TranslationIncluded(auxNameTable))
                {
                    nameTableG_p = nameTable;
                }

                if (transResum.TranslationIncluded(auxNameTable))
                {
                    nameTableResum = nameTable;
                }
            }// end for


            // Tabla con la lista de facetas
            DataTable dtFacets = ImportExcel.GetDataTableExcel(path, nameTableFacets);
            lf = ImportExcel.DataTable2ListFacets(dtFacets);

            // Tabla de análisis de suma de cuadrados
            DataTable dtSsqTable = ImportExcel.GetDataTableExcel(path, nameTableAnalysisOfVariance);
            tableAnalysis = DataTable2TableAnalysisOfVariance(dtSsqTable, lf);

            // Tabla resumen
            DataTable dtResumTable = ImportExcel.GetDataTableExcel(path, nameTableResum);
            tableResum = DataTable2TableResum(dtResumTable, lf);

            G_ParametersOptimization g_p_op = tableResum[0];
            tableResum.Remove(g_p_op);

            // Tabla de G-Parámetros
            DataTable dtG_pTable = ImportExcel.GetDataTableExcel(path, nameTableG_p);
            tableG = DataTable2TableG_Study(dtG_pTable, lf, g_p_op);

            tAnalysisSsq = new Analysis_and_G_Study(tableAnalysis, tableG, tableResum);
            

            return tAnalysisSsq;
        }


        /* Descripción:
         *  Toma un dataTable y una lista de facetas y devuelve una tabla de análisis de varianza.
         */
        private static TableAnalysisOfVariance DataTable2TableAnalysisOfVariance(DataTable dt, ListFacets lf)
        {
            List<string> ldesign = new List<string>(); // Lista de diseños contendrá las claves

            Dictionary<string, double> df = new Dictionary<string, double>(); // grado de libertad 
            Dictionary<string, double?> ssq = new Dictionary<string, double?>(); // suma de cuadrados
            Dictionary<string, double?> msq = new Dictionary<string, double?>(); // Suma de cuadrados medios (M.S.C.)

            // Componente de Varianza aleatorio
            Dictionary<string, double?> randomComp = new Dictionary<string, double?>();
            // Componente de Varianza Mixto
            Dictionary<string, double?> mixComp = new Dictionary<string, double?>();
            // Componente de Varianza Corregido
            Dictionary<string, double?> correcComp = new Dictionary<string, double?>();

            // Porcentaje
            Dictionary<string, double?> porcentage = new Dictionary<string, double?>();
            // Error estandar
            Dictionary<string, double?> standardError = new Dictionary<string, double?>();

            // Bucle en el que cargamos los datos
            int r = dt.Rows.Count;
            for (int i = 0; i < r; i++)
            {
                DataRow row = dt.Rows[i];
                string design = (string)row[0].ToString();
                ldesign.Add(design);
                double? c_ssq = ConvertNum.String2Double((string)row[1].ToString());
                ssq.Add(design, c_ssq);
                double? c_df = ConvertNum.String2Double((string)row[2].ToString());
                df.Add(design, (double)c_df);
                double? c_msq = ConvertNum.String2Double((string)row[3].ToString());
                msq.Add(design, c_msq);
                double? c_randon = ConvertNum.String2Double((string)row[4].ToString());
                randomComp.Add(design, c_randon);
                double? c_mix = ConvertNum.String2Double((string)row[5].ToString());
                mixComp.Add(design, c_mix);
                double? c_correc = ConvertNum.String2Double((string)row[6].ToString());
                correcComp.Add(design, c_correc);
                double? c_porcen = ConvertNum.String2Double((string)row[7].ToString());
                porcentage.Add(design, c_porcen);
                double? c_standard = ConvertNum.String2Double((string)row[8].ToString());
                standardError.Add(design, c_standard);
            }

            // Valor de retorno 
            return new TableAnalysisOfVariance(lf, ldesign, ssq, df, msq, randomComp, mixComp, correcComp,
                porcentage, standardError);
        }// end DataTable2TableAnalysisOfVariance


        /* Descripción:
         *  Genera una tabla de análisis de varianza a partir de un dataTable.
         */
        private static TableG_Study_Percent DataTable2TableG_Study(DataTable dt, ListFacets lf,
            G_ParametersOptimization g_p)
        {
            ListFacets lfDifferentiation = new ListFacets();
            ListFacets lfInstrumentation = new ListFacets();
            Dictionary<string, double?> differentiationVar = new Dictionary<string, double?>();
            Dictionary<string, ErrorVar> errorVar = new Dictionary<string, ErrorVar>();
            Dictionary<string, ErrorVar> percentError = new Dictionary<string, ErrorVar>();

            int r = dt.Rows.Count;
            for (int i = 0; i < r; i++)
            {
                DataRow row = dt.Rows[i];
                if (!string.IsNullOrEmpty((string)row[0].ToString()))
                {
                    string design = (string)row[0].ToString();
                    ListFacets newLf = lf.ListDesignFacets(design);
                    lfDifferentiation = lfDifferentiation.ConcatenateWithoutRepetitions(newLf);
                    double? d = ConvertNum.String2Double((string)row[1].ToString());
                    differentiationVar.Add(design, d);
                }
                if (!string.IsNullOrEmpty((string)row[2].ToString()))
                {
                    string design = (string)row[2].ToString();
                    ListFacets newLf = lf.ListDesignFacets(design);
                    lfInstrumentation = lfInstrumentation.ConcatenateWithoutRepetitions(newLf);
                    double? e1 = ConvertNum.String2Double((string)row[3].ToString());
                    double? p1 = ConvertNum.String2Double((string)row[4].ToString());
                    double? e2 = ConvertNum.String2Double((string)row[5].ToString());
                    double? p2 = ConvertNum.String2Double((string)row[6].ToString());
                    ErrorVar error = new ErrorVar(e1, e2);
                    ErrorVar percen = new ErrorVar(p1, p2);
                    errorVar.Add(design, error);
                    percentError.Add(design, percen);
                }
            }
            lfInstrumentation = lfInstrumentation.Difference(lfDifferentiation);

            return new TableG_Study_Percent(lfDifferentiation, lfInstrumentation, differentiationVar,
                errorVar, percentError, g_p);
        }// end DataTable2TableG_Study


        /* Descripción:
         *  Toma del dataTable con la tabla resumen que se pasa como parámetro, y construye la lista 
         *  de G_Parámetros.
         */
        private static List<G_ParametersOptimization> DataTable2TableResum(DataTable dt, ListFacets lf)
        {
            List<G_ParametersOptimization> listG_p = new List<G_ParametersOptimization>();
            int r = dt.Rows.Count;
            int c = dt.Columns.Count;

            for (int j = 1; j < c; j++)
            {
                ListFacets opListFacets = new ListFacets();

                for (int i = 0; i < (r - 7); i++)
                {
                    Facet f = lf.LookingFacet((string)dt.Rows[i][0].ToString());
                    string levelAndUniverse = (string)dt.Rows[i][j].ToString();

                    char[] delimeterChars2 = { ' ', '(', ';', ')' }; // nuestro delimitador será el caracter '/'
                    string[] arrayOfSplit = levelAndUniverse.Split(delimeterChars2, StringSplitOptions.RemoveEmptyEntries);
                    int level = int.Parse(arrayOfSplit[0]);
                    f.Level(level);
                    if (arrayOfSplit[1].Equals(Facet.INFINITE))
                    {
                        f.SizeOfUniverse(int.MaxValue);
                    }
                    else
                    {
                        int size = int.Parse(arrayOfSplit[1]);
                        f.SizeOfUniverse(size);
                    }
                    opListFacets.Add(f);
                }

                // Suma total de las varianzas de las fuentes objetivo
                double total_differentiation_var = 0;
                // coeficente G relativo
                double coefG_Rel = (double)ConvertNum.String2Double((string)dt.Rows[r - 6][j].ToString()); ;
                // Coeficiente G absoluto
                double coefG_Abs = (double)ConvertNum.String2Double((string)dt.Rows[r - 5][j].ToString()); ;
                // Varianza del error relativa
                double totalRelErrorVar = (double)ConvertNum.String2Double((string)dt.Rows[r - 4][j].ToString());
                // Varianza del error absoluta
                double totalAbsErrorVar = (double)ConvertNum.String2Double((string)dt.Rows[r - 3][j].ToString());
                // Desviación típica relativa
                double errorRelStandDev = (double)ConvertNum.String2Double((string)dt.Rows[r - 2][j].ToString()); ;
                // Desviación típica absoluta
                double errorAbsStandDev = (double)ConvertNum.String2Double((string)dt.Rows[r - 1][j].ToString());

                // desviación típica de las fuentes objetivo
                double targetStandDev = 0;

                G_ParametersOptimization g_p = new G_ParametersOptimization(opListFacets, total_differentiation_var,
                    coefG_Rel, coefG_Abs, totalRelErrorVar, totalAbsErrorVar, errorRelStandDev, errorAbsStandDev,
                    targetStandDev);
                listG_p.Add(g_p);
            }

            return listG_p;
        }// end DataTable2TableResum

        #endregion Importar cuadrados

    }// end ImportExcel
}// end GUI_TG
