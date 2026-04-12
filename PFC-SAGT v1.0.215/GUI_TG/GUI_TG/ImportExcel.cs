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
using AuxMathCalcGT;
using MultiFacetData;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using ProjectMeans;
using ProjectSSQ;
using System;
using System.Collections.Generic;
using System.IO;
using TransLibrary;

namespace GUI_GT
{
    public class ImportExcel
    {
        #region Workbook / Sheet helpers (NPOI)

        private static IWorkbook OpenWorkbook(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                if (Path.GetExtension(path).Equals(".xls", StringComparison.OrdinalIgnoreCase))
                    return new HSSFWorkbook(fs);
                else
                    return new XSSFWorkbook(fs);
            }
        }

        //returns all sheet names from workbook
        public static List<string> GetTableExcel(string strFileName)
        {
            var result = new List<string>();
            IWorkbook workbook;
            try
            {
                workbook = OpenWorkbook(strFileName);
            }
            catch (Exception)   //todo
            {
                throw;
            }

            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                string sheetName = workbook.GetSheetName(i);

                result.Add(sheetName);
            }

            return result;
        }

        //returns sheet from workbook with the given name
        private static ISheet GetSheetByOleDbName(IWorkbook workbook, string oleName)
        {
            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                if (workbook.GetSheetName(i).Equals(oleName, StringComparison.OrdinalIgnoreCase))
                {
                    return workbook.GetSheetAt(i);
                }
            }
            return null;
        }

        //returns cell value as a string, taking into account null/empty cases
        private static string GetCellString(ICell cell)
        {
            if (cell == null) return string.Empty;
            try
            {
                switch (cell.CellType)
                {
                    case CellType.String:
                        return cell.StringCellValue ?? string.Empty;
                    case CellType.Numeric:
                        return cell.NumericCellValue.ToString();
                    default:
                        return string.Empty;
                }
            }
            catch
            {
                return cell.ToString();
            }
        }

        private static List<string> GetHeaderNames(ISheet sheet)
        {
            var headers = new List<string>();
            IRow headerRow = sheet.GetRow(0);
            int last = headerRow.LastCellNum;
            for (int c = 0; c < last; c++)
            {
                var cell = headerRow.GetCell(c);
                string colname = GetCellString(cell) ?? $"Column{c + 1}";
                headers.Add(colname);
            }
            return headers;
        }

        // Returns all rows in the sheet as a list of string arrays for ease of use
        private static List<string[]> GetSheetRows(ISheet sheet, out List<string> headers)
        {
            headers = GetHeaderNames(sheet);
            var rows = new List<string[]>();

            int lastRow = sheet.LastRowNum;
            int colCount = headers.Count;

            // iterate rows starting at 1 to skip header
            for (int r = 1; r <= lastRow; r++)
            {
                var row = sheet.GetRow(r);
                if (row == null) continue;
                var arr = new string[colCount];
                for (int c = 0; c < colCount; c++)
                {
                    var cell = row.GetCell(c);
                    arr[c] = GetCellString(cell) ?? string.Empty;
                }
                rows.Add(arr);
            }

            return rows;
        }

        #endregion

        #region Importar los datos de un archivo Excel

        public static MultiFacetsObs ImportFileXLS_to_MultiFacetsObs(string path)
        {
            List<string> namesTables = ImportExcel.GetTableExcel(path);
            if (namesTables.Count != 2)
            {
                // No esta en el formato correcto
                throw new ObsTableException();
            }

            IWorkbook workbook = OpenWorkbook(path);

            // the first table contains facets
            ISheet sheetFacets = GetSheetByOleDbName(workbook, namesTables[0]);
            ListFacets lf = Sheet2ListFacets(sheetFacets);
            MultiFacetsObs mfo = new MultiFacetsObs(lf, path, "");
            // second sheet contains observation table
            ISheet sheetObs = GetSheetByOleDbName(workbook, namesTables[1]);
            InterfaceObsTable obsTb = mfo.ObservationTable();
            obsTb = Sheet2Observation(sheetObs, obsTb);
            mfo.ObservationTable(obsTb);

            return mfo;
        }

        private static ListFacets Sheet2ListFacets(ISheet sheet)
        {
            var lf = new ListFacets();
            // header on row 0, data start on row 1
            var rows = GetSheetRows(sheet, out var headers); // rows as string[] for data rows
            try
            {
                foreach (var row in rows)
                {
                    // expect: design (col0), level (col1), size (col2), description (col3)
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
                    //we ignore the omit

                    Facet f = new Facet(name, level, description, size, design);
                    lf.Add(f);
                }
            }
            catch (FormatException e)
            {
                throw e;
            }
            return lf;
        }

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

        private static InterfaceObsTable Sheet2Observation(ISheet sheet, InterfaceObsTable obsTb)
        {
            var res = obsTb;
            var rows = GetSheetRows(sheet, out var headers);
            int c = headers.Count;
            try
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    string s = row[c - 1];
                    double? d = ConvertNum.String2Double(s);
                    res.Data(d, i);
                }
            }
            catch (FormatException)
            {
                throw new ObsTableException();
            }
            return res;
        }

        #endregion Importar los datos de un archivo Excel

        #region Importar medias

        public static ListMeans ImportFileXLS_to_ListMeans(string path, WordTranslation tras)
        {
            List<string> namesTables = ImportExcel.GetTableExcel(path);
            if(namesTables.Count < 2 || !namesTables.Contains("Grand Mean"))
            {
                // No esta en el formato correcto. Note: not fully robust, and bad language practices
                throw new ListMeansException();
            }

            IWorkbook workbook = OpenWorkbook(path);

            ListMeans lm = new ListMeans();
            int n = namesTables.Count;
            ISheet sheetGrandMeans = GetSheetByOleDbName(workbook, "Grand Mean");

            var grandRows = GetSheetRows(sheetGrandMeans, out var grandHeaders);
            for (int i = 0; i < n; i++)
            {
                string nameTable = namesTables[i];

                if (nameTable.Contains("Grand Mean")) continue; // skips Grand Mean sheet

                ISheet sheetMeansTable = GetSheetByOleDbName(workbook, nameTable);

                string tbDesign = grandRows[i][1].ToString();
                double? gm = ConvertNum.String2Double((string)grandRows[i][2].ToString());
                double? variance = ConvertNum.String2Double((string)grandRows[i][3].ToString());
                double? stdDev = ConvertNum.String2Double((string)grandRows[i][4].ToString());
                InterfaceTableMeans tbMeans = DataSheet2TableMeans(sheetMeansTable, gm, variance, stdDev, tbDesign, tras);
                lm.Add(tbMeans);
            }

            lm.SetNameFileDataCreation(path);
            lm.SetDateTime(DateTime.Now);

            return lm;
        }

        private static InterfaceTableMeans DataSheet2TableMeans(ISheet sheet, double? grandMean,
            double? variance, double? stdDev, string tbDesign, WordTranslation trans)
        {
            // read header and data rows
            var rows = GetSheetRows(sheet, out var headers);
            int r = rows.Count;
            int c = headers.Count;
            int pos = c - 1;
            bool found = false;

            while (pos >= 0 && !found)
            {
                string sMeans = headers[pos];
                found = trans.TranslationIncluded(sMeans);
                if (!found) pos--;
            }

            InterfaceTableMeans tm = null;
            // If pos == c-3 => default means (original logic used pos == c-3)
            if (pos == (c - 3))
            {
                tm = new TableMeans(ConvertRowsToDataTableLike(rows, headers), grandMean, variance, stdDev, tbDesign);
            }
            if (pos == (c - 5))
            {
                tm = new TableMeansTypScore(ConvertRowsToDataTableLike(rows, headers), grandMean, variance, stdDev, tbDesign);
            }
            if (pos == (c - 6))
            {
                tm = new TableMeansDif(ConvertRowsToDataTableLike(rows, headers), grandMean, variance, stdDev, tbDesign);
            }

            return tm;
        }

        //Temporary patchwork since the datatable constructor is the one that has the level-guessing logic right now (it needs to be inferred from the table)
        private static System.Data.DataTable ConvertRowsToDataTableLike(List<string[]> rows, List<string> headers)
        {
            var dt = new System.Data.DataTable();
            foreach (var h in headers)
                dt.Columns.Add(h);

            foreach (var row in rows)
            {
                var dr = dt.NewRow();
                for (int j = 0; j < headers.Count; j++)
                {
                    dr[j] = j < row.Length ? row[j] : string.Empty;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        #endregion

        #region Importar cuadrados (AAGS)

        public static Analysis_and_G_Study ImportFileXLS_to_AAGS(string path,
            WordTranslation transFacets, WordTranslation transSSq, WordTranslation transG_p, WordTranslation transResum)
        {
            List<string> namesTables = ImportExcel.GetTableExcel(path);
            Analysis_and_G_Study tAnalysisSsq = null;
            if (namesTables.Count != 4) //NOTE: This is not robust. A cryptic error will pop up if we try to interpret a means table with 4 sheets as a AAGS one. Tofix
            {
                throw new Analysis_and_G_Study_Exception();
            }

            IWorkbook workbook = OpenWorkbook(path);

            string nameTableFacets = ""; // Contendrá el nombre de la tabla de facetas
            string nameTableAnalysisOfVariance = ""; // Contendrá el nombre de la tabla de análisis de varianza
            string nameTableG_p = ""; // Contendrá el nombre de la tabla G Study
            string nameTableResum = ""; // Contendrá el nombre de la tabla de optimización

            int n = namesTables.Count;

            for (int i = 0; i < n; i++)
            {
                string nameTable = namesTables[i];

                if (transFacets.TranslationIncluded(nameTable))
                {
                    nameTableFacets = nameTable;
                }

                if (transSSq.TranslationIncluded(nameTable))
                {
                    nameTableAnalysisOfVariance = nameTable;
                }

                if (transG_p.TranslationIncluded(nameTable))
                {
                    nameTableG_p = nameTable;
                }

                if (transResum.TranslationIncluded(nameTable))
                {
                    nameTableResum = nameTable;
                }
            }

            // Tabla con la lista de facetas
            ISheet sheetFacets = GetSheetByOleDbName(workbook, nameTableFacets);
            ListFacets lf = Sheet2ListFacets(sheetFacets);

            // Tabla de análisis de suma de cuadrados
            ISheet sheetSsq = GetSheetByOleDbName(workbook, nameTableAnalysisOfVariance);
            TableAnalysisOfVariance tableAnalysis = DataSheet2TableAnalysisOfVariance(sheetSsq, lf);

            // Tabla resumen
            ISheet sheetResum = GetSheetByOleDbName(workbook, nameTableResum);
            List<G_ParametersOptimization> tableResum = DataSheet2TableResum(sheetResum, lf);

            G_ParametersOptimization g_p_op = tableResum[0];
            tableResum.Remove(g_p_op);

            // Tabla de G-Parámetros
            ISheet sheetG_p = GetSheetByOleDbName(workbook, nameTableG_p);
            TableG_Study_Percent tableG = DataSheet2TableG_Study(sheetG_p, lf, g_p_op);

            tAnalysisSsq = new Analysis_and_G_Study(tableAnalysis, tableG, tableResum);


            return tAnalysisSsq;
        }

        private static TableAnalysisOfVariance DataSheet2TableAnalysisOfVariance(ISheet sheet, ListFacets lf)
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

            var rows = GetSheetRows(sheet, out var headers);
            int r = rows.Count;
            for (int i = 0; i < r; i++)
            {
                var row = rows[i];
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

            return new TableAnalysisOfVariance(lf, ldesign, ssq, df, msq, randomComp, mixComp, correcComp,
                porcentage, standardError);
        }

        private static TableG_Study_Percent DataSheet2TableG_Study(ISheet sheet, ListFacets lf,
            G_ParametersOptimization g_p)
        {
            ListFacets lfDifferentiation = new ListFacets();
            ListFacets lfInstrumentation = new ListFacets();
            Dictionary<string, double?> differentiationVar = new Dictionary<string, double?>();
            Dictionary<string, ErrorVar> errorVar = new Dictionary<string, ErrorVar>();
            Dictionary<string, ErrorVar> percentError = new Dictionary<string, ErrorVar>();

            var rows = GetSheetRows(sheet, out var headers);
            int r = rows.Count;
            for (int i = 0; i < r; i++)
            {
                var row = rows[i];
                if (row.Length > 0 && !string.IsNullOrEmpty(row[0]))
                {
                    string design = row[0];

                    Facet newF = lf.LookingFacet(ExtractNameOfDesign(design));  //fetch facet that goes first (Avoids problems with nested facets. AFAIK it should be impossible for a facet that isn't a differentiation facet to appear first in these designs)
                    ListFacets newLF = new ListFacets
                    {
                        newF
                    };
                    lfDifferentiation = lfDifferentiation.Union(newLF); //add it to lfDifferentiation only in case it's not already in the list

                    double? d = ConvertNum.String2Double((string)row[1].ToString());
                    differentiationVar.Add(design, d);
                }
                if (!string.IsNullOrEmpty(row[2]))
                {
                    string design = row[2];
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
            lfInstrumentation = lf.Difference(lfDifferentiation);

            return new TableG_Study_Percent(lfDifferentiation, lfInstrumentation, differentiationVar,
                errorVar, percentError, g_p);
        }

        private static List<G_ParametersOptimization> DataSheet2TableResum(ISheet sheet, ListFacets lf)
        {
            var listG_p = new List<G_ParametersOptimization>();
            var rows = GetSheetRows(sheet, out var headers);
            int r = rows.Count;
            int c = headers.Count;

            for (int j = 1; j < c; j++)
            {
                ListFacets opListFacets = new ListFacets();
                for (int i = 0; i < (r - 7); i++)
                {
                    var row = rows[i];
                    Facet f = lf.LookingFacet(row[0]);
                    string levelAndUniverse = row[j];
                    char[] delimeterChars2 = { ' ', '(', ';', ')' };
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

                double total_differentiation_var = 0;
                double coefG_Rel = (double)ConvertNum.String2Double(rows[r - 6][j]);
                double coefG_Abs = (double)ConvertNum.String2Double(rows[r - 5][j]);
                double totalRelErrorVar = (double)ConvertNum.String2Double(rows[r - 4][j]);
                double totalAbsErrorVar = (double)ConvertNum.String2Double(rows[r - 3][j]);
                double errorRelStandDev = (double)ConvertNum.String2Double(rows[r - 2][j]);
                double errorAbsStandDev = (double)ConvertNum.String2Double(rows[r - 1][j]);
                double targetStandDev = 0;

                G_ParametersOptimization g_p = new G_ParametersOptimization(opListFacets, total_differentiation_var,
                    coefG_Rel, coefG_Abs, totalRelErrorVar, totalAbsErrorVar, errorRelStandDev, errorAbsStandDev,
                    targetStandDev);
                listG_p.Add(g_p);
            }

            return listG_p;
        }

        #endregion
    }
}

