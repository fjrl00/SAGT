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
 * Fecha de revisión: 27/Mar/2012
 * 
 * Descripción: 
 *      Crea una lista de tabla de medias a partir de un fichero .txt informe de las medias de EduG 6.0
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MultiFacetData;
using ProjectMeans;
using System.IO;
using System.Globalization;
using System.Windows.Forms;
using AuxMathCalcGT;
using ImportEduGSsq;

namespace ImportEduGMeans
{
    public class ListMeansEduG : ListMeans
    {
        // Variables y Constantes
        //=======================

        /*===================================================================================================
         * Constantes
         *===================================================================================================*/
        /* Grand Means */
        private const string STR_GRAND_MEANS_EN = "Grand mean:";
        private const string STR_GRAND_MEANS_FR = "Moyenne générale:";

        /* Variance */
        private const string STR_VARIANCE_EN = "Variance:";
        private const string STR_VARIANCE_FR = "Variance:"; 

        /* Standard Dev */
        private const string STR_STANDARD_SEV_EN = "Standard Dev.";
        private const string STR_STANDARD_SEV_FR = "Ecart-type:";

        /* Etiquetas de las columnas (ingles) */
        private const string TITLE_MEAN_EN = "Mean";
        private const string TITLE_VARIANCE_EN = "Variance";
        private const string TITLE_STD_DEV_EN = "Std. Dev.";
        /* Etiquetas de las columnas (frances) */
        private const string TITLE_MEAN_FR = "Moyenne";
        private const string TITLE_VARIANCE_FR = "Variance";
        private const string TITLE_STD_DEV_FR = "Ecart-type";

        // Marca de cabecera de Informe (ingles/frances)
        public const string BEGIN_REPORT_FILE_EN = "File";
        public const string BEGIN_REPORT_FILE_FR = "Fichier";
        public const string BEGIN_REPORT_MARK = ".gen  -  [";

        /*===================================================================================================
         * Métodos
         *===================================================================================================*/

        #region Lectura de ficheros .txt
        /*==================================================================================================
         * Lectura de ficheros TXT
         * =======================
         * 
         *  - ReadFileReportTxtEduG(String nameFile)
         *  - ReadTextReaderEduG(TextReader reader)
         *  - ReadAuxTableMeansOfArrayLines(string line, TextReader reader)
         *==================================================================================================*/
        /* Descripción:
         *  Devuelve una lista de Tablas de medias recogidos en un fichero de informe TXT
         */
        public static List<ListMeansEduG> ReadFileReportTxtEduG(String nameFile, ConfigCFG.TypeOfTableMeans typeMeans)
        {
            List<ListMeansEduG> lm = null; // valor de retorno

            using (TextReader reader = new StreamReader(nameFile, System.Text.Encoding.Default))
            {
                lm = ReadTextReaderEduG(reader, typeMeans);
            }// end using

            return lm; // devuelve el valor
        }// end public static ListMeans ReadFileReportTxtEduG(String nameFile)



        /* Descripción:
         *  Devuelve una lista de tablas de medias leidas desde el string reader.
         */
        public static List<ListMeansEduG> ReadTextReaderEduG(TextReader reader, ConfigCFG.TypeOfTableMeans typeMeans)
        {
            List<ListMeansEduG> listMeansOfReports = new List<ListMeansEduG>();

            bool found = false;
            string line = reader.ReadLine();

            bool foundHeaderReport = false;
            DateTime sDate = DateTime.Now;

            while (line != null)
            {
                double grand_meand = 0;
                double variance = 0;
                double std_dev = 0;
                foundHeaderReport = IsLineHeaderReportsEduG(line);
                /* Hemos encontrado la cabecera de un informe y lo procesamos */
                if (foundHeaderReport)
                {

                    sDate = AnalysisSsqEduG.RecoveryStringDate(line);
                    foundHeaderReport = false;
                    ListMeansEduG listMeans = new ListMeansEduG(); // inicializamos
                    line = reader.ReadLine();
                    
                    while (line != null && !foundHeaderReport)
                    {
                        found = IsLineHeadersTableMeansEduG(line);
                        foundHeaderReport = IsLineHeaderReportsEduG(line);
                        
                        if (line.StartsWith(STR_GRAND_MEANS_EN) || line.StartsWith(STR_GRAND_MEANS_FR))
                        {
                            string str = line.Replace(STR_GRAND_MEANS_EN, "");
                            str = str.Replace(STR_GRAND_MEANS_FR, "");
                            str = str.Replace(":", "");
                            grand_meand = (double)ConvertNum.String2Double(str.Trim());
                        }

                        if (line.StartsWith(STR_VARIANCE_EN) || line.StartsWith(STR_VARIANCE_FR))
                        {
                            string str = line.Replace(STR_VARIANCE_EN, "");
                            str = str.Replace(STR_VARIANCE_FR, "");
                            str = str.Replace(":", "");
                            variance = (double)ConvertNum.String2Double(str.Trim());
                        }

                        if (line.StartsWith(STR_STANDARD_SEV_EN) || line.StartsWith(STR_STANDARD_SEV_FR))
                        {
                            string str = line.Replace(STR_STANDARD_SEV_EN, "");
                            str = str.Replace(STR_STANDARD_SEV_FR, "");
                            str = str.Replace(":", "");
                            std_dev = (double)ConvertNum.String2Double(str.Trim());
                        }

                        if (!foundHeaderReport)
                        {
                            if (found)
                            {
                                switch(typeMeans)
                                {
                                    case(ConfigCFG.TypeOfTableMeans.Default):
                                        TableMeans tbMeans = ReadAuxTableMeansOfArrayLines(line, reader, grand_meand, variance, std_dev);
                                        listMeans.Add(tbMeans);
                                        break;
                                    case(ConfigCFG.TypeOfTableMeans.TableMeansDif):
                                        TableMeansDif tbMeansDif = ReadAuxTableMeansDifOfArrayLines(line, reader, grand_meand, variance, std_dev);
                                        listMeans.Add(tbMeansDif);
                                        break;
                                    case (ConfigCFG.TypeOfTableMeans.TableMeansTipPoint):
                                        TableMeansTypScore tbMeansTypScore = ReadAuxTableMeansTypScoreOfArrayLines(line, reader, grand_meand, variance, std_dev);
                                        listMeans.Add(tbMeansTypScore);
                                        break;
                                    default:
                                        throw new ListMeansEduG_Exception("Error: No es un tipo de tabla de media reconocido");
                                }
                            }
                            line = reader.ReadLine();
                        }
                        
                    }
                    if (listMeans.Count() > 0)
                    {
                        listMeans.SetDateTime(sDate);
                        listMeansOfReports.Add(listMeans);
                    }
                }
                else
                {
                    line = reader.ReadLine();
                }
                
            }// end while

            return listMeansOfReports;
        }// end ReadTextReaderEduG


        /* Descripción:
         *  Devuelve una Tabla de medias leida del textReader que se pasa como parámetro. Coresponde
         *  un informe de un fichero .txt
         * Parámetros:
         *  string line: es la primera línea de la tabla, la cabecera.
         *  TextReader reader: el resto de la tabla.
         *  double grandMean: Grand media
         *  double  variance: Varianza
         *  double std_dev: desviación típica
         */
        private static TableMeans ReadAuxTableMeansOfArrayLines(string line, TextReader reader, double grandMean, 
            double  variance, double std_dev)
        {
            char[] delimeterChars = { '\t', ' ' };
            string[] arrayOfShares = line.Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
            /* El numero de columnas será el número de facetas más 3, determinare el número de facetas mediante
             * la posición de la columna media
             */
            int numFacet = 0;
            while (!(arrayOfShares[numFacet].Contains(TITLE_MEAN_EN)
                 || arrayOfShares[numFacet].Contains(TITLE_MEAN_FR)))
            {
                numFacet++;
            }

            DataTable dt = new DataTable();
            ListFacets lf = new ListFacets();
            for (int j = 0; j < numFacet; j++)
            {
                string nameFacet = arrayOfShares[j].Substring(0, 1);
                DataColumn col = new DataColumn(nameFacet);
                dt.Columns.Add(col);
                string comments = arrayOfShares[j].Substring(1).Trim();
                Facet facet = new Facet(nameFacet, 2, comments);
                lf.Add(facet);
            }

            int[] arrayLevels = new int[numFacet];

            DataColumn col2 = new DataColumn(TITLE_MEAN_EN);
            dt.Columns.Add(col2);
            col2 = new DataColumn(TITLE_VARIANCE_EN);
            dt.Columns.Add(col2);
            col2 = new DataColumn(TITLE_STD_DEV_EN);
            dt.Columns.Add(col2);


            // rellenamos la tabla de medias
            line = reader.ReadLine();
            while (line.Trim().Equals(""))
            {
                line = reader.ReadLine();
            }

            while (!string.IsNullOrEmpty(line))
            {
                string[] arrayTableLine = line.Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);

                // introducimos la línea
                DataRow row = dt.NewRow();
                int n = arrayTableLine.Length;
                for (int j = 0; j < n; j++)
                {
                    row[j] = arrayTableLine[j];
                }
                dt.Rows.Add(row);

                // incrementamos la fila
                line = reader.ReadLine();
            }
            string design = lf.StringOfListFactes();
            TableMeans tbMeans = new TableMeans(dt, grandMean, variance, std_dev, design);
            
            // tbMeans.Calc_GrandMean_Variance_StdDev(true);
            /* Nota: el último parámetro de la sentencia anterior es true ya que EduG interpreta
             * los valores null como ceros.
             */

            return tbMeans;
        }// end ReadAuxTableMeansOfArrayLines



        /* Descripción:
         *  Devuelve una Tabla diferencia de medias leida del textReader que se pasa como parámetro. Coresponde
         *  un informe de un fichero .txt
         * Parámetros:
         *  string line: es la primera línea de la tabla, la cabecera.
         *  TextReader reader: el resto de la tabla.
         *  double grandMean: Grand media
         *  double  variance: Varianza
         *  double std_dev: desviación típica
         */
        private static TableMeansDif ReadAuxTableMeansDifOfArrayLines(string line, TextReader reader, double grandMean,
            double variance, double std_dev)
        {
            char[] delimeterChars = { '\t', ' ' };
            string[] arrayOfShares = line.Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
            /* El numero de columnas será el número de facetas más 3, determinare el número de facetas mediante
             * la posición de la columna media
             */
            int numFacet = 0;
            while (!(arrayOfShares[numFacet].Contains(TITLE_MEAN_EN)
                 || arrayOfShares[numFacet].Contains(TITLE_MEAN_FR)))
            {
                numFacet++;
            }

            DataTable dt = new DataTable();
            ListFacets lf = new ListFacets();
            for (int j = 0; j < numFacet; j++)
            {
                string nameFacet = arrayOfShares[j].Substring(0, 1);
                DataColumn col = new DataColumn(nameFacet);
                dt.Columns.Add(col);
                string comments = arrayOfShares[j].Substring(1).Trim();
                Facet facet = new Facet(nameFacet, 2, comments);
                lf.Add(facet);
            }

            int[] arrayLevels = new int[numFacet];

            DataColumn col2 = new DataColumn(TITLE_MEAN_EN);
            dt.Columns.Add(col2);
            col2 = new DataColumn(TITLE_VARIANCE_EN);
            dt.Columns.Add(col2);
            col2 = new DataColumn(TITLE_STD_DEV_EN);
            dt.Columns.Add(col2);
            col2 = new DataColumn("dif_means");
            dt.Columns.Add(col2);
            col2 = new DataColumn("dif_variance");
            dt.Columns.Add(col2);
            col2 = new DataColumn("dif_std_dev");
            dt.Columns.Add(col2);


            // rellenamos la tabla de medias
            line = reader.ReadLine();
            while (line.Trim().Equals(""))
            {
                line = reader.ReadLine();
            }

            while (!string.IsNullOrEmpty(line))
            {
                string[] arrayTableLine = line.Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
                int n = arrayTableLine.Length;
                string[] arrayTableLine2 = new string[arrayTableLine.Length + 3];

                for(int j=0; j<n; j++)
                {
                    arrayTableLine2[j]=arrayTableLine[j];
                }
                arrayTableLine2[n] = ((double)ConvertNum.String2Double(arrayTableLine2[n-3])-grandMean).ToString();
                arrayTableLine2[n+1] = ((double)ConvertNum.String2Double(arrayTableLine2[n-2])-variance).ToString();
                arrayTableLine2[n+2] = ((double)ConvertNum.String2Double(arrayTableLine2[n-1])-std_dev).ToString();

                arrayTableLine = arrayTableLine2;

                // introducimos la línea
                DataRow row = dt.NewRow();
                n = arrayTableLine.Length;
                for (int j = 0; j < n; j++)
                {
                    row[j] = arrayTableLine[j];
                }
                dt.Rows.Add(row);

                // incrementamos la fila
                line = reader.ReadLine();
            }
            string design = lf.StringOfListFactes();
            TableMeansDif tbMeans = new TableMeansDif(dt, grandMean, variance, std_dev, design);

            // tbMeans.Calc_GrandMean_Variance_StdDev(true);
            /* Nota: el último parámetro de la sentencia anterior es true ya que EduG interpreta
             * los valores null como ceros.
             */

            return tbMeans;
        }// end ReadAuxTableMeansDifOfArrayLines



        /* Descripción:
         *  Devuelve una Tabla puntuación típica leida del textReader que se pasa como parámetro. Coresponde
         *  un informe de un fichero .txt
         * Parámetros:
         *  string line: es la primera línea de la tabla, la cabecera.
         *  TextReader reader: el resto de la tabla.
         *  double grandMean: Grand media
         *  double  variance: Varianza
         *  double std_dev: desviación típica
         */
        private static TableMeansTypScore ReadAuxTableMeansTypScoreOfArrayLines(string line, TextReader reader, double grandMean,
            double variance, double std_dev)
        {
            char[] delimeterChars = { '\t', ' ' };
            string[] arrayOfShares = line.Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
            /* El numero de columnas será el número de facetas más 3, determinare el número de facetas mediante
             * la posición de la columna media
             */
            int numFacet = 0;
            while (!(arrayOfShares[numFacet].Contains(TITLE_MEAN_EN)
                 || arrayOfShares[numFacet].Contains(TITLE_MEAN_FR)))
            {
                numFacet++;
            }

            DataTable dt = new DataTable();
            ListFacets lf = new ListFacets();
            for (int j = 0; j < numFacet; j++)
            {
                string nameFacet = arrayOfShares[j].Substring(0, 1);
                DataColumn col = new DataColumn(nameFacet);
                dt.Columns.Add(col);
                string comments = arrayOfShares[j].Substring(1).Trim();
                Facet facet = new Facet(nameFacet, 2, comments);
                lf.Add(facet);
            }

            int[] arrayLevels = new int[numFacet];

            DataColumn col2 = new DataColumn(TITLE_MEAN_EN);
            dt.Columns.Add(col2);
            col2 = new DataColumn(TITLE_VARIANCE_EN);
            dt.Columns.Add(col2);
            col2 = new DataColumn(TITLE_STD_DEV_EN);
            dt.Columns.Add(col2);
            col2 = new DataColumn("dif_means");
            dt.Columns.Add(col2);
            col2 = new DataColumn("typ_score");
            dt.Columns.Add(col2);



            // rellenamos la tabla de medias
            line = reader.ReadLine();
            while (line.Trim().Equals(""))
            {
                line = reader.ReadLine();
            }

            while (!string.IsNullOrEmpty(line))
            {
                string[] arrayTableLine = line.Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
                int n = arrayTableLine.Length;
                string[] arrayTableLine2 = new string[arrayTableLine.Length + 2];

                for (int j = 0; j < n; j++)
                {
                    arrayTableLine2[j] = arrayTableLine[j];
                }
                arrayTableLine2[n] = ((double)ConvertNum.String2Double(arrayTableLine2[n - 3]) - grandMean).ToString();
                arrayTableLine2[n + 1] = (((double)ConvertNum.String2Double(arrayTableLine2[n - 3]) - grandMean)/std_dev).ToString();


                arrayTableLine = arrayTableLine2;

                // introducimos la línea
                DataRow row = dt.NewRow();
                n = arrayTableLine.Length;
                for (int j = 0; j < n; j++)
                {
                    row[j] = arrayTableLine[j];
                }
                dt.Rows.Add(row);

                // incrementamos la fila
                line = reader.ReadLine();
            }
            string design = lf.StringOfListFactes();
            TableMeansTypScore tbMeansTypScore = new TableMeansTypScore(dt, grandMean, variance, std_dev, design);

            // tbMeans.Calc_GrandMean_Variance_StdDev(true);
            /* Nota: el último parámetro de la sentencia anterior es true ya que EduG interpreta
             * los valores null como ceros.
             */

            return tbMeansTypScore;
        }// end ReadAuxTableMeansTypScoreOfArrayLines

        #endregion Lectura de ficheros .txt


        #region Lectura de ficheros .RTF
        /*==================================================================================================
         * Lectura de ficheros RTF
         * =======================
         * 
         *  - ReadFileReportRtfEduG(String path)
         *  - ReadFileRtfPaintTextEduGAux(string text)
         *  - TableMeans ReadAuxTableMeansOfArrayLines(string[] lines, int i)
         *==================================================================================================*/

        /* Descripción:
         *  Devuelve una lista de Tablas de medias recogidos en un fichero de informe RTF
         */
        public static List<ListMeansEduG> ReadFileReportRtfEduG(String path, ConfigCFG.TypeOfTableMeans typeMeans)
        {
            try
            {
                List<ListMeansEduG> listMeansEduG = new List<ListMeansEduG>();// valor de retorno

                //Create the RichTextBox. (Requires a reference to System.Windows.Forms.dll.)
                System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox();

                // Get the contents of the RTF file. Note that when it is
                // stored in the string, it is encoded as UTF-16.
                string s = System.IO.File.ReadAllText(path);

                // Convert the RTF to plain text.
                rtBox.Rtf = s;
                string plainText = rtBox.Text;

                listMeansEduG = ReadFileRtfPaintTextEduGAux(plainText, typeMeans);

                // devuelve el valor de retorno
                return listMeansEduG;
            }catch(IOException e)
            {
                throw e;
            }
        }// end public static ListMeans ReadFileReportTxtEduG(String nameFile)


        /* Descripción:
         *  Realiza la lectura de rtf que se ha pasado como texto plano en el argumento
         */
        private static List<ListMeansEduG> ReadFileRtfPaintTextEduGAux(string text, ConfigCFG.TypeOfTableMeans typeMeans)
        {
            char[] delimeterLineChars = { '\n' };
            string[] lines = text.Split(delimeterLineChars);

            int numLines = lines.Length;

            List<ListMeansEduG> listOfListMeans = new List<ListMeansEduG>();

            DateTime sDate = DateTime.Now;

            bool foundHeaderTable = false;
            bool foundHeaderReport = false;

            int i = 0;
            string line = lines[i];
            foundHeaderReport = IsLineHeaderReportsEduG(line);
            
            while (i < numLines)
            {
                if (foundHeaderReport)
                {
                    double grand_meand = 0;
                    double variance = 0;
                    double std_dev = 0;

                    sDate = AnalysisSsqEduG.RecoveryStringDate(line);

                    foundHeaderReport = false;
                    i++;
                    ListMeansEduG listMeans = new ListMeansEduG(); // inicializamos
                    while (i < numLines && !foundHeaderReport)
                    {
                        line = lines[i];
                        foundHeaderReport = IsLineHeaderReportsEduG(line);
                        foundHeaderTable = IsLineHeadersTableMeansEduG(line);

                        if (line.StartsWith(STR_GRAND_MEANS_EN) || line.StartsWith(STR_GRAND_MEANS_FR))
                        {
                            string str = line.Replace(STR_GRAND_MEANS_EN, "");
                            str = str.Replace(STR_GRAND_MEANS_FR, "");
                            str = str.Replace(":", "");
                            grand_meand = (double)ConvertNum.String2Double(str.Trim());
                        }

                        if (line.StartsWith(STR_VARIANCE_EN) || line.StartsWith(STR_VARIANCE_FR))
                        {
                            string str = line.Replace(STR_VARIANCE_EN, "");
                            str = str.Replace(STR_VARIANCE_FR, "");
                            str = str.Replace(":", "");
                            variance = (double)ConvertNum.String2Double(str.Trim());
                        }

                        if (line.StartsWith(STR_STANDARD_SEV_EN) || line.StartsWith(STR_STANDARD_SEV_FR))
                        {
                            string str = line.Replace(STR_STANDARD_SEV_EN, "");
                            str = str.Replace(STR_STANDARD_SEV_FR, "");
                            str = str.Replace(":", "");
                            std_dev = (double)ConvertNum.String2Double(str.Trim());
                        }

                        if (foundHeaderTable)
                        {
                            switch (typeMeans)
                            {
                                case (ConfigCFG.TypeOfTableMeans.Default):
                                    TableMeans tbMeans = ReadAuxTableMeansOfArrayLines(lines, i, grand_meand, variance, std_dev);
                                    listMeans.Add(tbMeans);
                                    break;
                                case (ConfigCFG.TypeOfTableMeans.TableMeansDif): 
                                    TableMeansDif tbMeansDif = ReadAuxTableMeansDifOfArrayLines(lines, i, grand_meand, variance, std_dev);
                                    listMeans.Add(tbMeansDif);
                                    break;
                                case (ConfigCFG.TypeOfTableMeans.TableMeansTipPoint):
                                    TableMeansTypScore tbMeansTypScore = ReadAuxTableMeansTypScoreOfArrayLines(lines, i, grand_meand, variance, std_dev);
                                    listMeans.Add(tbMeansTypScore);
                                    break;
                                default:
                                    throw new ListMeansEduG_Exception("Error: No es un tipo de tabla de media reconocido");
                            }
                            
                        }
                        i++;
                    }
                    if (listMeans.Count() > 0)
                    {
                        listMeans.SetDateTime(sDate);
                        listOfListMeans.Add(listMeans);
                    }
                }
                else
                {
                    i++;
                }
            }

            return listOfListMeans;
        }// end ReadFileRtfPaintTextEduGAux


        /* Descripción:
         *  Devuelve una Tabla de medias leida  del array de lineas que se pasa como primer parámetro. 
         *  Cada tabla termina en la línea vacía.
         *  
         * Parámetros:
         *  string[] lines: Es el documento separado por líneas.
         *  int i: es la linea donde comienza la tabla que se va a devolver.
         *  double grandMean: Gran media
         *  double variance: Varianza
         *  double std_dev: Desviación típica
         */
        private static TableMeans ReadAuxTableMeansOfArrayLines(string[] lines, int i, double grandMean, double variance,
            double std_dev)
        {
            string lineFacets = lines[i];
            char[] delimeterCharsFacets = { '\t' };
            string[] arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
            // la longitud del array el numero de columnas
            int c = arrayOfShares.Length;
            // El numero de facetas c-3;
            int numFacet = c - 3;
            DataTable dt = new DataTable();
            ListFacets lf = new ListFacets();
            for (int j = 0; j < numFacet; j++)
            {
                string nameFacet = arrayOfShares[j].Substring(0, 1);
                DataColumn col = new DataColumn(nameFacet);
                dt.Columns.Add(col);
                string comments = arrayOfShares[j].Substring(1).Trim();
                Facet facet = new Facet(nameFacet, 2, comments);
                lf.Add(facet);
            }

            int[] arrayLevels = new int[numFacet];

            DataColumn col2 = new DataColumn(TITLE_MEAN_EN);
            dt.Columns.Add(col2);
            col2 = new DataColumn(TITLE_VARIANCE_EN);
            dt.Columns.Add(col2);
            col2 = new DataColumn(TITLE_STD_DEV_EN);
            dt.Columns.Add(col2);

            char[] delimeterChars = { '\t' };

            // rellenamos la tabla de medias
            i++;
            while (!string.IsNullOrEmpty(lines[i]))
            {
                string[] arrayTableLine = lines[i].Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);

                // introducimos la línea
                DataRow row = dt.NewRow();
                int n = arrayTableLine.Length;
                for (int j = 0; j < n; j++)
                {
                    row[j] = arrayTableLine[j];
                }
                dt.Rows.Add(row);

                // incrementamos la fila
                i++;
            }
            string design = lf.StringOfListFactes();
            TableMeans tbMeans = new TableMeans(dt, grandMean, variance, std_dev, design);
            // tbMeans.Calc_GrandMean_Variance_StdDev(true);
            /* Nota: el último parámetro de la sentencia anterior es true ya que EduG interpreta
             * los valores null como ceros.
             */
            
            return tbMeans;
        }// end ReadAuxTableMeansOfArrayLines

        /* Descripción:
         *  Devuelve una Tabla de diferencia de medias leida  del array de lineas que se pasa como primer parámetro. 
         *  Cada tabla termina en la línea vacía.
         *  
         * Parámetros:
         *  string[] lines: Es el documento separado por líneas.
         *  int i: es la linea donde comienza la tabla que se va a devolver.
         *  double grandMean: Gran media
         *  double variance: Varianza
         *  double std_dev: Desviación típica
         */
        private static TableMeansDif ReadAuxTableMeansDifOfArrayLines(string[] lines, int i, double grandMean, double variance,
            double std_dev)
        {
            string lineFacets = lines[i];
            char[] delimeterCharsFacets = { '\t' };
            string[] arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
            // la longitud del array el numero de columnas
            int c = arrayOfShares.Length;
            // El numero de facetas c-3;
            int numFacet = c - 3;
            DataTable dt = new DataTable();
            ListFacets lf = new ListFacets();
            for (int j = 0; j < numFacet; j++)
            {
                string nameFacet = arrayOfShares[j].Substring(0, 1);
                DataColumn col = new DataColumn(nameFacet);
                dt.Columns.Add(col);
                string comments = arrayOfShares[j].Substring(1).Trim();
                Facet facet = new Facet(nameFacet, 2, comments);
                lf.Add(facet);
            }

            int[] arrayLevels = new int[numFacet];

            DataColumn col2 = new DataColumn(TITLE_MEAN_EN);
            dt.Columns.Add(col2);
            col2 = new DataColumn(TITLE_VARIANCE_EN);
            dt.Columns.Add(col2);
            col2 = new DataColumn(TITLE_STD_DEV_EN);
            dt.Columns.Add(col2);
            col2 = new DataColumn("dif_means");
            dt.Columns.Add(col2);
            col2 = new DataColumn("dif_variance");
            dt.Columns.Add(col2);
            col2 = new DataColumn("dif_std_dev");
            dt.Columns.Add(col2);


            char[] delimeterChars = { '\t' };

            // rellenamos la tabla de medias
            i++;
            while (!string.IsNullOrEmpty(lines[i]))
            {
                string[] arrayTableLine = lines[i].Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
                int n = arrayTableLine.Length;
                string[] arrayTableLine2 = new string[arrayTableLine.Length + 3];

                for (int j = 0; j < n; j++)
                {
                    arrayTableLine2[j] = arrayTableLine[j];
                }
                arrayTableLine2[n] = ((double)ConvertNum.String2Double(arrayTableLine2[n - 3]) - grandMean).ToString();
                arrayTableLine2[n + 1] = ((double)ConvertNum.String2Double(arrayTableLine2[n - 2]) - variance).ToString();
                arrayTableLine2[n + 2] = ((double)ConvertNum.String2Double(arrayTableLine2[n - 1]) - std_dev).ToString();

                arrayTableLine = arrayTableLine2;



                // introducimos la línea
                DataRow row = dt.NewRow();

                n = arrayTableLine.Length;
                for (int j = 0; j < n; j++)
                {
                    row[j] = arrayTableLine[j];
                }
                dt.Rows.Add(row);

                // incrementamos la fila
                i++;
            }
            string design = lf.StringOfListFactes();
            TableMeansDif tbMeansDif = new TableMeansDif(dt, grandMean, variance, std_dev, design);
            // tbMeans.Calc_GrandMean_Variance_StdDev(true);
            /* Nota: el último parámetro de la sentencia anterior es true ya que EduG interpreta
             * los valores null como ceros.
             */

            return tbMeansDif;
        }// end ReadAuxTableMeansDifOfArrayLines


        /* Descripción:
         *  Devuelve una Tabla de puntuación típica leida  del array de lineas que se pasa como primer parámetro. 
         *  Cada tabla termina en la línea vacía.
         *  
         * Parámetros:
         *  string[] lines: Es el documento separado por líneas.
         *  int i: es la linea donde comienza la tabla que se va a devolver.
         *  double grandMean: Gran media
         *  double variance: Varianza
         *  double std_dev: Desviación típica
         */
        private static TableMeansTypScore ReadAuxTableMeansTypScoreOfArrayLines(string[] lines, int i, double grandMean, double variance,
            double std_dev)
        {
            string lineFacets = lines[i];
            char[] delimeterCharsFacets = { '\t' };
            string[] arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
            // la longitud del array el numero de columnas
            int c = arrayOfShares.Length;
            // El numero de facetas c-3;
            int numFacet = c - 3;
            DataTable dt = new DataTable();
            ListFacets lf = new ListFacets();
            for (int j = 0; j < numFacet; j++)
            {
                string nameFacet = arrayOfShares[j].Substring(0, 1);
                DataColumn col = new DataColumn(nameFacet);
                dt.Columns.Add(col);
                string comments = arrayOfShares[j].Substring(1).Trim();
                Facet facet = new Facet(nameFacet, 2, comments);
                lf.Add(facet);
            }

            int[] arrayLevels = new int[numFacet];

            DataColumn col2 = new DataColumn(TITLE_MEAN_EN);
            dt.Columns.Add(col2);
            col2 = new DataColumn(TITLE_VARIANCE_EN);
            dt.Columns.Add(col2);
            col2 = new DataColumn(TITLE_STD_DEV_EN);
            dt.Columns.Add(col2);
            col2 = new DataColumn("dif_means");
            dt.Columns.Add(col2);
            col2 = new DataColumn("typ_score");
            dt.Columns.Add(col2);



            char[] delimeterChars = { '\t' };

            // rellenamos la tabla de medias
            i++;
            while (!string.IsNullOrEmpty(lines[i]))
            {
                string[] arrayTableLine = lines[i].Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
                int n = arrayTableLine.Length;
                string[] arrayTableLine2 = new string[arrayTableLine.Length + 2];

                for (int j = 0; j < n; j++)
                {
                    arrayTableLine2[j] = arrayTableLine[j];
                }
                arrayTableLine2[n] = ((double)ConvertNum.String2Double(arrayTableLine2[n - 3]) - grandMean).ToString();
                arrayTableLine2[n + 1] = (((double)ConvertNum.String2Double(arrayTableLine2[n - 3]) - grandMean) / std_dev).ToString();


                arrayTableLine = arrayTableLine2;



                // introducimos la línea
                DataRow row = dt.NewRow();

                n = arrayTableLine.Length;
                for (int j = 0; j < n; j++)
                {
                    row[j] = arrayTableLine[j];
                }
                dt.Rows.Add(row);

                // incrementamos la fila
                i++;
            }
            string design = lf.StringOfListFactes();
            TableMeansTypScore tbMeansTypScore = new TableMeansTypScore(dt, grandMean, variance, std_dev, design);
            // tbMeans.Calc_GrandMean_Variance_StdDev(true);
            /* Nota: el último parámetro de la sentencia anterior es true ya que EduG interpreta
             * los valores null como ceros.
             */

            return tbMeansTypScore;
        }// end ReadAuxTableMeansTypScoreOfArrayLines

        #endregion Lectura de ficheros RTF



        #region Operaciones auxiliares de lectura de informes de medias
        /*==============================================================================================
         * Operaciones auxiliares de lectura de informes de medias
         * =======================================================
         *  - IsLineHeadersTableMeansEduG
         *  - IsLineHeaderReportsEduG
         *==============================================================================================*/

        /* Descripción
         *  Devuelve true si es la cabecera de encabezado de una tabla de medias de un informe EduG, con
         *  independencia de que dicho informe este en íngles o en frances.
         */
        private static bool IsLineHeadersTableMeansEduG(string line)
        {
            return (line.Contains(TITLE_MEAN_EN) && line.Contains(TITLE_VARIANCE_EN)
                    && line.Contains(TITLE_STD_DEV_EN))
                    || line.Contains(TITLE_MEAN_FR) && line.Contains(TITLE_VARIANCE_FR)
                    && line.Contains(TITLE_STD_DEV_FR);
        }


        /* Descripción:
         *  Nos dice si estamos o no a la cabeza de un informe por que la línea contiene las marcas.
         */
        public static bool IsLineHeaderReportsEduG(string line)
        {
            return (line.Contains(BEGIN_REPORT_FILE_EN) && line.Contains(BEGIN_REPORT_MARK)
                || line.Contains(BEGIN_REPORT_FILE_FR) && line.Contains(BEGIN_REPORT_MARK));
        }

        #endregion Operaciones auxiliares de lectura de informes de medias


    }// end public class ListMeansEduG : ListMeans
}// end namespace ImportEduGMeans