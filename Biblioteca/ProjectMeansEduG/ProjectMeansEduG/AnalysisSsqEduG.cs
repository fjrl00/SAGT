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
 * Fecha de revisión: 20/Abr/2012
 * 
 * Descripción:
 *      Lectura de informes de análisis de suma de cuadrados (*.txt, *.rtf; EduG 6.0 - e, - f).
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectSSQ;
using MultiFacetData;
using System.IO;
using System.Globalization;
using AuxMathCalcGT;

namespace ImportEduGSsq
{
    public class AnalysisSsqEduG : Analysis_and_G_Study
    {
        #region CONSTANTES DEL INFORME EN INGLES
        /**********************************************************************************************
         * CONSTANTES DEL INFORME EN INGLES
         * ================================
         **********************************************************************************************/
        // Constantes
        //===========
        /******************************************************************************************
         * Títulos y etiquetas del informe en ingles.
         ******************************************************************************************/
        
        // Etiquetas de la tablas Facetas
        //===============================
        public const string TITLE_OBS_EXT_DESING_ENG = "Observation and Estimation Designs";
        public const string LABEL_COLUM_FACET_ENG = "Facet";
        public const string LABEL_COLUM_LABEL_ENG = "Label";
        public const string LABEL_COLUM_LEV_RTF_ENG = "Levels";
        public const string LABEL_COLUM_LEV_TXT_ENG = "Lev.";
        public const string LABEL_COLUM_UNIV_ENG = "Univ.";
        // Etiquetas de la tabla analysis de varianza
        //===========================================
        public const string TITLE_ANALYSIS_OF_VAR_ENG = "Analysis of variance";
        public const string LABEL_COLUM_SOURCE_ENG = "Source";
        public const string LABEL_COLUM_SS_ENG = "SS";
        public const string LABEL_COLUM_DF_ENG = "df";
        public const string LABEL_COLUM_MS_ENG = "MS";
        public const string LABEL_COLUM_RANDOM_ENG = "Random";
        public const string LABEL_COLUM_MIXED_ENG = "Mixed";
        public const string LABEL_COLUM_CORRECT_ENG = "Corrected";
        public const string END_TABLE_ANALYSIS_ENG = "Total";
        public const string TITLE_MEASUR_DESIGN_ENG = "(Measurement design";
        
        // Etiquetas de la tabla G Parameters
        //===================================
        public const string LABEL_VARIANCE_ENG = "variance";
        public const string LABEL_VAR_ENG = "var.";
        public const string LABEL_REL_ENG = "rel.";
        public const string LABEL_ABS_ENG = "abs.";
        public const string LABEL_PERCENT_ENG = "%";
        public const string SUSPENSION_POINTS = "...";
        /* NOTA: El número de puntos puede variar entre un informe txt y uno rtf. Por ello para
         * comparar usaremos Constains en lugar de Equals.
         */
        public const string END_OF_TABLE_STUDY_ENG_RTF = "Sum of variances";
        /* NOTA: La anterior constante marca el fin de una tabla G_Study en un informe de suma 
         * de cuadrados .rft
         */
        public const string END_OF_TABLE_STUDY_TXT = "Sum of"; // informe txt
        // "variances"; // se usa para saber donde comienza el segmento de resumen de datos de la tabla G-Parametros en un informe txt
        public const string LABEL_RELATIVE_SE_ENG = "Relative SE:";
        public const string LABEL_ABSOLUTE_SE_ENG = "Absolute SE:";
        public const string COEF_G = "Coef_G";
        public const string COEF_G_RELATIVE_ENG = "Coef_G relative: ";
        public const string COEF_G_ABSOLUTE_ENG = "Coef_G absolute: ";

        // Etiquetas que determina el comienzo de la tabla de resumen de optimización
        //===========================================================================
        public const string LABEL_BEGIN_1_ENG = "Lev."; // indica que es la columna de niveles
        public const string LABEL_BEGIN_2_ENG = "Univ"; // indica que es la columna de tamaño del universo
        // Etiqueta que determina el fin del fragmento de tabla con las facetas
        public const string LABEL_END_FACETS_ENG = "Observ.";

        #endregion CONSTANTES DEL INFORME EN INGLES

        #region CONSTANTES DEL INFORME EN FRANCES
        /**********************************************************************************************
         * CONSTANTES DEL INFORME EN FRANCES
         * =================================
         **********************************************************************************************/
        // Constantes
        //===========
        /******************************************************************************************
         * Títulos y etiquetas del informe en ingles.
         ******************************************************************************************/

        // Etiquetas de la tablas Facetas
        //===============================
        public const string TITLE_OBS_EXT_DESING_FR = "Plan d’observation et d’estimation";
        public const string LABEL_COLUM_FACET_FR = "Facette";
        public const string LABEL_COLUM_LABEL_RTF_FR = "Etiquette";
        public const string LABEL_COLUM_LABEL_TXT_FR = "Étiquette";
        public const string LABEL_COLUM_LEV_RTF_FR = "Niveaux";
        public const string LABEL_COLUM_LEV_TXT_FR = "Niv.";
        public const string LABEL_COLUM_UNIV_RTF_FR = "Univers";
        public const string LABEL_COLUM_UNIV_TXT_FR = "Univ.";
        // Etiquetas de la tabla analysis de varianza
        //===========================================
        public const string TITLE_ANALYSIS_OF_VAR_FR = "Analyse de la variance";
        public const string LABEL_COLUM_SOURCE_FR = "Source";
        public const string LABEL_COLUM_SS_FR = "SC";
        public const string LABEL_COLUM_DF_FR = "dl";
        public const string LABEL_COLUM_MS_FR = "CM";
        public const string LABEL_COLUM_RANDOM_RTF_FR = "Aléatoires";
        public const string LABEL_COLUM_RANDOM_TXT_FR = "Aléat.";
        public const string LABEL_COLUM_MIXED_FR = "Mixtes";
        public const string LABEL_COLUM_CORRECT_FR = "Corrigées";
        public const string END_TABLE_ANALYSIS_FR = "Total";
        public const string TITLE_MEASUR_DESIGN_FR = "(Plan de mesure";
        // Etiquetas de la tabla G Parameters
        //===================================
        public const string LABEL_VARIANCE_FR = "Variance";
        public const string END_OF_TABLE_STUDY_FR_RTF = "Total des variances";
        public const string END_OF_TABLE_STUDY_FR_TXT = "Total des";
        /* NOTA: La anterior constante marca el fin de una tabla G_Study en un informe de suma 
         * de cuadrados .rft
         */
        public const string LABEL_RELATIVE_SE_FR = "Erreur type relative:";
        public const string LABEL_ABSOLUTE_SE_FR = "Erreur type absolue:";

        // Etiquetas que determina el comienzo de la tabla de resumen de optimización
        //===========================================================================
        public const string LABEL_BEGIN_1_FR = "Niv."; // indica que es la columna de niveles
        public const string LABEL_BEGIN_2_FR = "Univ"; // indica que es la columna de tamaño del universo 
        public const string COEF_G_RELATIVE_FR = "Coef_G relatif :";
        public const string COEF_G_ABSOLUTE_FR = "Coef_G absolu  :";

        #endregion CONSTANTES DEL INFORME EN FRANCES

        /************************************************************************************************
         * Constructores
         * =============
         ************************************************************************************************/
        public AnalysisSsqEduG(TableAnalysisOfVariance tableAnalysis, TableG_Study_Percent tableG)            
            : base(tableAnalysis, tableG)
        {
        }

        public AnalysisSsqEduG(TableAnalysisOfVariance tableAnalysis, TableG_Study_Percent tableG,
            List<G_ParametersOptimization> listG_P_O)
            : base(tableAnalysis, tableG, listG_P_O)
        {
        }


        #region Métodos de lectura de informes txt
        /************************************************************************************************
         * Métodos de lectura de informes txt
         * ==================================
         * - ReadFileReportTxtEduG
         * - ReadFileRtfPaintTextEduGAux
         * - AuxReadTableFacetsOfTextReader
         * - AuxReportSsqToTableAnalysis
         * - AuxReportG_StudyToTableG_Study
         * - AuxReaderResumG_Study
         * - AuxReadTableOptimizationResum
         ************************************************************************************************/

        /* Descripción:
         *  Devuelve una lista de Tablas de suma de cuadrados recogidos en un fichero de informe txt
         */
        public static List<AnalysisSsqEduG> ReadFileReportTxtEduG(String path)
        {
            try
            {
                List<AnalysisSsqEduG> analysisEduG = null;// valor de retorno

                using (TextReader reader = new StreamReader(path, System.Text.Encoding.Default))
                {
                    analysisEduG = ReadFileTxtEduGAux(reader);
                }// end using

                // devuelve el valor de retorno
                return analysisEduG;
            }
            catch (IOException e)
            {
                throw e;
            }
        }// end ReadFileReportTxtEduG


        /* Descripción:
         *  Método auxiliar que lee de un stream el informe suma de cuadrados según el formato de fichero txt
         */
        private static List<AnalysisSsqEduG> ReadFileTxtEduGAux(TextReader reader)
        {
            string line = reader.ReadLine();

            List<AnalysisSsqEduG> listAnalysisEduG = new List<AnalysisSsqEduG>();// valor de retorno
            DateTime sDate = DateTime.Now;

            while (line != null)
            {
                bool foundHeaderReport = ImportEduGMeans.ListMeansEduG.IsLineHeaderReportsEduG(line);
                if (!foundHeaderReport)
                {
                    line = reader.ReadLine();
                }
                else
                {
                    /* Hemos encontrado la cabecera del informe y leemos de esa línea la fecha
                     */
                    sDate = RecoveryStringDate(line);

                    AnalysisSsqEduG analysisEduG = null;

                    ListFacets lf = null;

                    // Tabla de lista de facetas
                    bool foundTableFacests = false;
                    while (line != null && !foundTableFacests)
                    {
                        foundTableFacests = IsHeaderTableFacetsTXT(line);
                        if (foundTableFacests)
                        {
                            lf = AuxReadTableFacetsOfTextReader(reader);
                        }

                        line = reader.ReadLine();
                    }

                    // Tabla de suma de cuadrados
                    TableAnalysisOfVariance tbAnalysis = null;
                    bool foundTableAnalysis = false;
                    while (line != null && !foundTableAnalysis)
                    {
                        foundTableAnalysis = IsHeaderTableSquare(line);
                        if (foundTableAnalysis)
                        {
                            tbAnalysis = AuxReportSsqToTableAnalysis(reader, lf);
                        }
                        line = reader.ReadLine();
                    }


                    // Diseño de medida
                    ListFacets diff = null;
                    ListFacets inst = null;
                    bool foundMesurementDesign = false;
                    while (line != null && !foundMesurementDesign)
                    {
                        foundMesurementDesign = IsLineMesurementDesing(line);
                        if (foundMesurementDesign)
                        {
                            diff = MesurementDesignToListFacetDiff(lf, line);
                            inst = MesurementDesignToListFacetInst(lf, line);
                        }
                        line = reader.ReadLine();
                    }

                    // Tabla de G-Parámetros
                    TableG_Study_Percent tableG_study_percent = null;
                    bool foundTableG_P = false;
                    while (line != null && !foundTableG_P)
                    {
                        foundTableG_P = IsHeaderTableG_ParametersTxtReport(line);
                        if (foundTableG_P)
                        {
                            tableG_study_percent = AuxReportG_StudyToTableG_Study(reader, diff, inst, lf);
                        }
                        line = reader.ReadLine();
                    }

                    // Tabla de Resumen de optimización
                    List<G_ParametersOptimization> listG_P = new List<G_ParametersOptimization>();
                    bool foundTableResum = false;
                    while (line != null && !foundTableResum)
                    {
                        foundTableResum = IsHeaderTableResumOpt(line);
                        if (foundTableResum)
                        {
                            listG_P = AuxReadTableOptimizationResum(reader);
                        }
                        line = reader.ReadLine();
                    }

                    if (listG_P.Count() > 0)
                    {
                        G_ParametersOptimization gp = listG_P[0];
                        tableG_study_percent = new TableG_Study_Percent(tableG_study_percent.LfDifferentiation(),
                            tableG_study_percent.LfInstrumentation(), tableG_study_percent.Target(),
                            tableG_study_percent.Error(),
                            gp, tableG_study_percent.Percent());
                        listG_P.Remove(gp);
                    }

                    analysisEduG = new AnalysisSsqEduG(tbAnalysis, tableG_study_percent, listG_P);
                    analysisEduG.SetDateTime(sDate);
                    listAnalysisEduG.Add(analysisEduG);
                }// end if
            }// end while

            return listAnalysisEduG;
        }// end ReadFileRtfPaintTextEduGAux


        /* Descripción:
         *  Lee una tabla de facetas de un informe de suma de cuadrados .rtf y devuelve una lista 
         *  de facetas.
         *  
         * NOTA: Los comentarios tendrá una longitud fija de 25 caracteres. No se puede emplear más ya
         * que el resto del comentario aparecería en la línea siguiente y dado que no somos capaces
         * de saber cuando es una línea de continuación de comentario o un comentario perteneciente a
         * una nueva faceta podría provocar un error por no cumplir el formato del archivo. La longitud
         * variable de los comentario unido al uso de caracter blanco para separar las palabra y columnas
         * provoca ambigueda para no poder distinguir cuando comienza un comienza una palabra o una columna
         */
        private static ListFacets AuxReadTableFacetsOfTextReader(TextReader reader)
        {
            string line = reader.ReadLine();
            char[] delimeterCharsFacets = { ' ' };

            ListFacets lf = new ListFacets(); // Variable de retorno
            try
            {
                while (!string.IsNullOrEmpty(line))
                {
                    string comment = line.Substring(0, 25).Trim();
                    line = line.Replace(comment, "");
                    string[] arrayOfShares = line.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
                    string name = CharNameFacet(arrayOfShares[0]);
                    string design = NormalizateDesign(arrayOfShares[0]);
                    int level = int.Parse(arrayOfShares[1]);
                    int size = ConvertStringSizeOfUnivToInt(arrayOfShares[2]);

                    Facet f = new Facet(name, level, comment, size, design);
                    lf.Add(f);

                    line = reader.ReadLine();
                    arrayOfShares = line.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            catch (FormatException e)
            {
                throw e;
            }
            catch (IndexOutOfRangeException e)
            {
                throw e;
            }

            return lf;
        }// end ReadAuxTableFacetsOfArrayLines


        /* Descripción:
         *  Devuelve un elemento TableAnalysisOfVariance recuperado de un informe su suma de cuadrados
         */
        private static TableAnalysisOfVariance AuxReportSsqToTableAnalysis(TextReader reader, ListFacets lf)
        {
            string line = reader.ReadLine();
            char[] delimeterCharsFacets = { ' ' };
            string[] arrayOfShares = line.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            // Variables
            List<string> ldesign = new List<string>();
            Dictionary<string, double?> dicSsq = new Dictionary<string, double?>();
            Dictionary<string, double> dicDf = new Dictionary<string, double>();
            Dictionary<string, double?> dicMsq = new Dictionary<string, double?>();
            Dictionary<string, double?> dicRandomComp = new Dictionary<string, double?>();
            Dictionary<string, double?> dicMixComp = new Dictionary<string, double?>();
            Dictionary<string, double?> dicCorrecComp = new Dictionary<string, double?>();
            Dictionary<string, double?> porcentage = new Dictionary<string, double?>();
            Dictionary<string, double?> standardError = new Dictionary<string, double?>();

            bool foundEndTableAnalysis = false;
            while (line != null && !foundEndTableAnalysis)
            {
                foundEndTableAnalysis = IsEndingTableSquare(line);
                if (!string.IsNullOrEmpty(line) && !foundEndTableAnalysis)
                {
                    // Fuente de variación
                    string design = NormalizateDesign(arrayOfShares[0]);
                    ldesign.Add(design);
                    // Suma de cuadrados
                    double? ssq = StringToDouble(arrayOfShares[1]);
                    dicSsq.Add(design, ssq);
                    // Grado de libertad
                    double df = (double)StringToDouble(arrayOfShares[2]);
                    dicDf.Add(design, df);
                    // Cuadrado medio
                    double? ms = StringToDouble(arrayOfShares[3]);
                    dicMsq.Add(design, ms);
                    // Componente de varianza aleatorio
                    double? random = StringToDouble(arrayOfShares[4]);
                    dicRandomComp.Add(design, random);
                    // Componete de varianza mixto
                    double? mixed = StringToDouble(arrayOfShares[5]);
                    dicMixComp.Add(design, mixed);
                    // Componente de varianza corregida
                    double? correc = StringToDouble(arrayOfShares[6]);
                    dicCorrecComp.Add(design, correc);
                    // Porcentaje de error
                    double? percent = StringToDouble(arrayOfShares[7]);
                    porcentage.Add(design, percent);
                    // Error estandar
                    double? se = StringToDouble(arrayOfShares[8]);
                    standardError.Add(design, se);
                }
                
                line = reader.ReadLine();
                arrayOfShares = line.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            }// end while

            return new TableAnalysisOfVariance(lf, ldesign, dicSsq, dicDf, dicMsq, dicRandomComp, dicMixComp,
                dicCorrecComp, porcentage, standardError);
        }// end AuxReportSsqToTableAnalysis



        /* Descripción:
         *  Devuelve la tabla G_Parámetros con los porcentajes tomados de un informe de suma de 
         *  cuadrados de EduG 6.0. (informe con extensión .txt)
         * 
         * NOTA:
         * =====
         * Como aun no ha cargado la tabla de parámetros de optimización esta contendrá el valor null.
         * Ha de ser sustituida por los datos correctos.
         */
        private static TableG_Study_Percent AuxReportG_StudyToTableG_Study(TextReader reader,
            ListFacets lf_diff, ListFacets lf_inst, ListFacets lf)
        {
            string line = reader.ReadLine();
            char[] delimeterCharsFacets = { ' ' };
            string[] arrayOfShares = line.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
            // string[] arrayOfShares = lineFacets.Split(delimeterCharsFacets);

            // Variables
            Dictionary<string, double?> differentiationVar = new Dictionary<string, double?>();
            Dictionary<string, ErrorVar> errorVar = new Dictionary<string, ErrorVar>();
            Dictionary<string, ErrorVar> percentError = new Dictionary<string, ErrorVar>();

            // while (line!=null && !IsHeaderTableResumTxtfTableG_Study(line)) 
            while (line != null && !IsEndingTableG_ParametersTxtReport(line))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    if (arrayOfShares[0].Contains(SUSPENSION_POINTS))
                    {
                        // Entonces contiene facetas de intrumentación
                        string source = NormalizateDesign(arrayOfShares[1]);
                        double? relErrorVar = StringToDouble(arrayOfShares[2]); // Varianza del error relativo
                        double? perct_rel = StringToDouble(arrayOfShares[3]);
                        
                        double? absErrorVar = StringToDouble(arrayOfShares[4]); ; // Varianza del error absoluto
                        double? perct_abs = StringToDouble(arrayOfShares[5]);
                        
                        ErrorVar error = new ErrorVar(relErrorVar, absErrorVar);
                        errorVar.Add(source, error);
                        ErrorVar perct_error = new ErrorVar(perct_rel, perct_abs);
                        percentError.Add(source, perct_error);
                    }
                    else
                    {
                        // Contiene facetas de diferenciación
                        string source = NormalizateDesign(arrayOfShares[0]);
                        double? var_diff = StringToDouble(arrayOfShares[1]);
                        differentiationVar.Add(source, var_diff);
                    }
                }
                
                line = reader.ReadLine();
                arrayOfShares = line.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
                // arrayOfShares = lineFacets.Split(delimeterCharsFacets);
            }// end while
            G_ParametersOptimization gP = AuxReaderResumG_Study(reader,  lf);
            return new TableG_Study_Percent(lf_diff, lf_inst, differentiationVar, errorVar, percentError, gP);
        }// end AuxReportG_StudyToTableG_Study


        /* Descripción:
         *  Devuelve los datos resumenes de la tabla de G_Parámetros (G-Study)
         */
        private static G_ParametersOptimization AuxReaderResumG_Study(TextReader reader,
            ListFacets gListFacets)
        {
            string line = reader.ReadLine();
            char[] delimeterCharsFacets = { ' ' };
            string[] arrayOfShares = line.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            double total_differentiation_var = (double)StringToDouble(arrayOfShares[1]); // Suma total de las varianzas de las fuentes objetivo

            double totalRelErrorVar = (double)StringToDouble(arrayOfShares[2]); // Varianza del error relativa
            double totalAbsErrorVar = (double)StringToDouble(arrayOfShares[4]); // Varianza del error absoluta

            line = reader.ReadLine();// saltamos la linea que pone "Standard"
            line = reader.ReadLine();
            // Eliminamos de la linea los textos
            // Cadenas en frances
            line = line.Replace(LABEL_RELATIVE_SE_FR, " ");
            line = line.Replace(LABEL_ABSOLUTE_SE_FR, " ");
            // Cadenas en ingles
            line = line.Replace(LABEL_RELATIVE_SE_ENG, " ");
            line = line.Replace(LABEL_ABSOLUTE_SE_ENG, " ");
            arrayOfShares = line.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            double targetStandDev = (double)StringToDouble(arrayOfShares[1]); // desviación típica de las fuentes objetivo
            double errorRelStandDev = (double)StringToDouble(GetRelativeStandarDev(arrayOfShares[2])); // Desviación típica relativa
            double errorAbsStandDev = (double)StringToDouble(GetRelativeStandarDev(arrayOfShares[3])); // Desviación típica absoluta

            while (!(line.Contains(COEF_G_RELATIVE_ENG) || line.Contains(COEF_G_RELATIVE_FR)))
            {
                line = reader.ReadLine();
            }
            line = line.Replace(COEF_G_RELATIVE_ENG, "").Trim();
            line = line.Replace(COEF_G_RELATIVE_FR, "").Trim();
            double coefG_Rel = (double)StringToDouble(line); // coeficente G relativo

            line = reader.ReadLine();
            line = line.Replace(COEF_G_ABSOLUTE_ENG, "").Trim();
            line = line.Replace(COEF_G_ABSOLUTE_FR, "").Trim();

            double coefG_Abs = (double)StringToDouble(line); // Coeficiente G absoluto


            return new G_ParametersOptimization(gListFacets, total_differentiation_var, coefG_Rel, coefG_Abs,
                totalRelErrorVar, totalAbsErrorVar, errorRelStandDev, errorAbsStandDev, targetStandDev);
        }// end AuxReaderResumG_Study


        /* Descripción:
         *  Devuelve una Lista de G_Parámetros obtenidos de la tabla resumen del final del informe.
         */
        private static List<G_ParametersOptimization> AuxReadTableOptimizationResum(TextReader reader)
        {
            // Variable de retorno
            List<G_ParametersOptimization> listG_p = new List<G_ParametersOptimization>();

            string line = reader.ReadLine();
            char[] delimeterCharsFacets = { ' ' };
            string[] arrayOfShares = line.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            int numOfColumns = arrayOfShares.Length / 2;
            List<ListFacets> listOfListFacets = new List<ListFacets>();
            // Inicializamos la estructura
            for (int j = 0; j < numOfColumns; j++)
            {
                listOfListFacets.Add(new ListFacets());
            }

            int numOfShares = arrayOfShares.Length;
            while (!line.Contains(LABEL_END_FACETS_ENG) && !string.IsNullOrEmpty(line))
            {
                int pos = 0;
                string name = CharNameFacet(arrayOfShares[0]);
                string design = NormalizateDesign(arrayOfShares[0]);
                for (int j = 1; j < numOfShares; j = j + 2)
                {
                    int level = int.Parse(arrayOfShares[j]);
                    int sizeOfUniverse = ConvertStringSizeOfUnivToInt(arrayOfShares[j + 1]);
                    Facet f = new Facet(name, level, "", sizeOfUniverse, design);
                    ListFacets lf = listOfListFacets[pos];
                    lf.Add(f);
                    pos++;
                }

                line = reader.ReadLine();
                arrayOfShares = line.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
            }// end while    

            // Tomamos el resto de parámetros
            //===============================

            // Coeficiente G relativo
            List<double> listOfCoef_G_rel = new List<double>();

            line = reader.ReadLine();// saltamos la linea del número de observaciones
            line = reader.ReadLine();
            arrayOfShares = line.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
            int limit = arrayOfShares.Length - numOfColumns;

            for (int j = arrayOfShares.Length; j > limit; j--)
            {
                listOfCoef_G_rel.Insert(0, (double)StringToDouble(arrayOfShares[j-1]));
            }

            // Coeficiente G Absoluto
            List<double> listOfCoef_G_abs = new List<double>();
            
            // saltamos la linea de redondeo
            line = reader.ReadLine();
            line = reader.ReadLine();
            arrayOfShares = line.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            limit = arrayOfShares.Length - numOfColumns;
            for (int j = arrayOfShares.Length; j > limit; j--)
            {
                listOfCoef_G_abs.Insert(0,(double)StringToDouble(arrayOfShares[j-1]));
            }

            // Varianza de error relativa
            List<double> listOfErrorVarRel = new List<double>();

            line = reader.ReadLine(); // saltamos la linea de redondeo
            line = reader.ReadLine();
            arrayOfShares = line.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
            limit = arrayOfShares.Length - numOfColumns;

            for (int j = arrayOfShares.Length; j > limit; j--)
            {
                listOfErrorVarRel.Insert(0, (double)StringToDouble(arrayOfShares[j-1]));
            }

            // Varianza de estandar relativa
            List<double> listOfStadarDevRel = new List<double>();
            
            line = reader.ReadLine();
            arrayOfShares = line.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
            limit = arrayOfShares.Length - numOfColumns;

            for (int j = arrayOfShares.Length; j > limit; j--)
            {
                listOfStadarDevRel.Insert(0, (double)StringToDouble(arrayOfShares[j-1]));
            }

            // Varianza de error absoluta
            List<double> listOfErrorVarAbs = new List<double>();
            
            line = reader.ReadLine();
            arrayOfShares = line.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
            limit = arrayOfShares.Length - numOfColumns;

            for (int j = arrayOfShares.Length; j > limit; j--)
            {
                listOfErrorVarAbs.Insert(0, (double)StringToDouble(arrayOfShares[j-1]));
            }

            // Varianza de estandar relativa
            List<double> listOfStadarDevAbs = new List<double>();
            
            line = reader.ReadLine();
            arrayOfShares = line.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
            limit = arrayOfShares.Length - numOfColumns;

            for (int j = arrayOfShares.Length; j > limit; j--)
            {
                listOfStadarDevAbs.Insert(0, (double)StringToDouble(arrayOfShares[j-1]));
            }

            for (int j = 0; j < numOfColumns; j++)
            {
                G_ParametersOptimization gp = new G_ParametersOptimization(listOfListFacets[j], 0,
                    listOfCoef_G_rel[j], listOfCoef_G_abs[j], listOfErrorVarRel[j], listOfErrorVarAbs[j],
                    listOfStadarDevRel[j], listOfStadarDevAbs[j], 0);
                listG_p.Add(gp);
            }

            return listG_p;
        }// end AuxReadTableOptimizationResum


        #endregion Métodos de lectura de informes txt



        #region Métodos de lectura de informes rtf
        /************************************************************************************************
         * Métodos de lectura de informes rtf
         * ==================================
         * - ReadFileReportRtfEduG
         * - ReadFileRtfPaintTextEduGAux
         * - AuxReadTableFacetsOfArrayLines
         * - AuxReportSsqToTableAnalysis
         * - AuxReportG_StudyToTableG_Study
         * - AuxReaderResumG_Study
         * - AuxReadTableOptimizationResum
         ************************************************************************************************/

        /* Descripción:
         *  Devuelve una objeto analysis de suma de cuadrados recogidos en un fichero de informe RTF
         */
        public static List<AnalysisSsqEduG> ReadFileReportRtfEduG(String path)
        {
            try
            {
                List<AnalysisSsqEduG> analysisEduG = null;// valor de retorno

                //Create the RichTextBox. (Requires a reference to System.Windows.Forms.dll.)
                System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox();

                // Get the contents of the RTF file. Note that when it is
                // stored in the string, it is encoded as UTF-16.
                string s = System.IO.File.ReadAllText(path);

                // Convert the RTF to plain text.
                rtBox.Rtf = s;
                string plainText = rtBox.Text;

                analysisEduG = ReadFileRtfPaintTextEduGAux(plainText);

                // devuelve el valor de retorno
                return analysisEduG;
            }catch(IOException e)
            {
                throw e;
            }
        }// end ReadFileReportRtfEduG



        /* Descripción:
         *  Método auxiliar que lee el texto del fichero como texto plano
         */
        private static List<AnalysisSsqEduG> ReadFileRtfPaintTextEduGAux(string text)
        {
            char[] delimeterLineChars = { '\n' };
            string[] lines = text.Split(delimeterLineChars);

            int numLines = lines.Length-1;

            List<AnalysisSsqEduG> listAnalysisEduG = new List<AnalysisSsqEduG>();

            int i = 0;
            string line = lines[i];

            bool foundHeaderReport = false;

            while ((line != null) && (i<numLines))
            {
                DateTime sDate = DateTime.Now;
                foundHeaderReport = ImportEduGMeans.ListMeansEduG.IsLineHeaderReportsEduG(line);
                if (!foundHeaderReport)
                {
                    i++;
                    line = lines[i];
                }
                else
                {
                    /* Hemos encontrado la cabecera del informe y leemos de esa línea la fecha
                     */
                    sDate = RecoveryStringDate(line);

                    AnalysisSsqEduG analysisEduG = null;// valor de retorno

                    ListFacets lf = null;

                    // Tabla de lista de facetas
                    bool foundTableFacests = false;
                    while (i < numLines && !foundTableFacests)
                    {
                        foundTableFacests = IsHeaderTableFacetsRTF(line);
                        if (foundTableFacests)
                        {
                            lf = AuxReadTableFacetsOfArrayLines(lines, i + 1);
                        }

                        i++;
                        line = lines[i];
                    }

                    // Tabla de suma de cuadrados
                    TableAnalysisOfVariance tbAnalysis = null;
                    bool foundTableAnalysis = false;
                    while (i < numLines && !foundTableAnalysis)
                    {
                        foundTableAnalysis = IsHeaderTableSquare(line);
                        if (foundTableAnalysis)
                        {
                            tbAnalysis = AuxReportSsqToTableAnalysis(lines, i + 1, lf);
                        }
                        i++;
                        line = lines[i];
                    }


                    // Diseño de medida
                    ListFacets diff = null;
                    ListFacets inst = null;
                    bool foundMesurementDesign = false;
                    while (i < numLines && !foundMesurementDesign)
                    {
                        foundMesurementDesign = IsLineMesurementDesing(line);
                        if (foundMesurementDesign)
                        {
                            diff = MesurementDesignToListFacetDiff(lf, line);
                            inst = MesurementDesignToListFacetInst(lf, line);
                        }
                        i++;
                        line = lines[i];
                    }

                    // Tabla de G-Parámetros
                    TableG_Study_Percent tableG_study_percent = null;
                    bool foundTableG_P = false;
                    while (i < numLines && !foundTableG_P)
                    {
                        foundTableG_P = IsHeaderTableG_ParametersRtfReport(line);
                        if (foundTableG_P)
                        {
                            tableG_study_percent = AuxReportG_StudyToTableG_Study(lines, i + 1, diff, inst, lf);
                        }
                        i++;
                        line = lines[i];
                    }

                    // Tabla de Resumen de optimización
                    List<G_ParametersOptimization> listG_P = new List<G_ParametersOptimization>();
                    bool foundTableResum = false;
                    while (i < numLines && !foundTableResum)
                    {
                        foundTableResum = IsHeaderTableResumOpt(line);
                        if (foundTableResum)
                        {
                            listG_P = AuxReadTableOptimizationResum(lines, i);
                        }
                        i++;
                        line = lines[i];
                    }

                    if (listG_P.Count() > 0)
                    {
                        G_ParametersOptimization gp = listG_P[0];
                        tableG_study_percent = new TableG_Study_Percent(tableG_study_percent.LfDifferentiation(),
                            tableG_study_percent.LfInstrumentation(), tableG_study_percent.Target(),
                            tableG_study_percent.Error(),
                            gp, tableG_study_percent.Percent());
                        listG_P.Remove(gp);
                    }

                    if (tbAnalysis != null && tableG_study_percent != null)
                    {
                        analysisEduG = new AnalysisSsqEduG(tbAnalysis, tableG_study_percent, listG_P);
                        analysisEduG.SetDateTime(sDate);
                        listAnalysisEduG.Add(analysisEduG);
                    }
                }// end if
            }// end while
            

            return listAnalysisEduG;
        }// end ReadFileRtfPaintTextEduGAux


        /* Descripción:
         *  Lee una tabla de facetas de un informe de suma de cuadrados .rtf y devuelve una lista 
         *  de facetas.
         */
        private static ListFacets AuxReadTableFacetsOfArrayLines(string[] lines, int i)
        {
            string lineFacets = lines[i];
            char[] delimeterCharsFacets = { '\t' };
            // string[] arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
            string[] arrayOfShares = lineFacets.Split(delimeterCharsFacets);

            ListFacets lf = new ListFacets(); // Variable de retorno

            while (!string.IsNullOrEmpty(lineFacets))
            {
                string comment = arrayOfShares[0];
                string name = CharNameFacet(arrayOfShares[1]);
                string design = NormalizateDesign(arrayOfShares[1]);
                int level = int.Parse(arrayOfShares[2]);
                int size = ConvertStringSizeOfUnivToInt(arrayOfShares[3]);

                Facet f = new Facet(name, level, comment, size, design);
                lf.Add(f);

                i++;
                lineFacets = lines[i];
                // arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
                arrayOfShares = lineFacets.Split(delimeterCharsFacets);
            }

            return lf;
        }// end ReadAuxTableFacetsOfArrayLines


     

        /* Descripción:
         *  Devuelve un elemento TableAnalysisOfVariance recuperado de un informe su suma de cuadrados
         */
        private static TableAnalysisOfVariance AuxReportSsqToTableAnalysis(string[] lines, int i, ListFacets lf)
        {
            string lineFacets = lines[i];
            char[] delimeterCharsFacets = { '\t' };
            string[] arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            // Variables
            List<string> ldesign = new List<string>();
            Dictionary<string, double?> dicSsq  = new Dictionary<string,double?>();
            Dictionary<string, double> dicDf = new Dictionary<string,double>();
            Dictionary<string, double?> dicMsq = new Dictionary<string,double?>();
            Dictionary<string, double?> dicRandomComp = new Dictionary<string,double?>();
            Dictionary<string, double?> dicMixComp = new Dictionary<string,double?>();
            Dictionary<string, double?> dicCorrecComp = new Dictionary<string,double?>();
            Dictionary<string, double?> porcentage = new Dictionary<string, double?>();
            Dictionary<string, double?> standardError = new Dictionary<string,double?>();

            bool foundEndTableAnalysis = false;
            while (!string.IsNullOrEmpty(lineFacets) && !foundEndTableAnalysis)
            {
                foundEndTableAnalysis = IsEndingTableSquare(lineFacets);
                if (!foundEndTableAnalysis)
                {
                    // Fuente de variación
                    string design = arrayOfShares[0];
                    // Normalizamos
                    design = NormalizateDesign(design);
                    ldesign.Add(design);
                    // Suma de cuadrados
                    double? ssq = StringToDouble(arrayOfShares[1]);
                    dicSsq.Add(design, ssq);
                    // Grado de libertad
                    double df = (double)StringToDouble(arrayOfShares[2]);
                    dicDf.Add(design, df);
                    // Cuadrado medio
                    double? ms = StringToDouble(arrayOfShares[3]);
                    dicMsq.Add(design, ms);
                    // Componente de varianza aleatorio
                    double? random = StringToDouble(arrayOfShares[4]);
                    dicRandomComp.Add(design, random);
                    // Componete de varianza mixto
                    double? mixed = StringToDouble(arrayOfShares[5]);
                    dicMixComp.Add(design, mixed);
                    // Componente de varianza corregida
                    double? correc = StringToDouble(arrayOfShares[6]);
                    dicCorrecComp.Add(design, correc);
                    // Porcentaje de error
                    double? percent = StringToDouble(arrayOfShares[7]);
                    porcentage.Add(design, percent);
                    // Error estandar
                    double? se = StringToDouble(arrayOfShares[8]);
                    standardError.Add(design, se);
                    i++;
                    lineFacets = lines[i];
                    arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
                }
                
            }// end while

            return new TableAnalysisOfVariance(lf, ldesign, dicSsq, dicDf, dicMsq, dicRandomComp, dicMixComp,
                dicCorrecComp, porcentage, standardError);
        }// end AuxReportSsqToTableAnalysis


        /* Descripción:
         *  Devuelve la tabla G_Parámetros con los porcentajes tomados de un informe de suma de 
         *  cuadrados de EduG 6.0. (informe con extensión .rtf)
         * 
         * NOTA:
         * =====
         * Como aun no ha cargado la tabla de parámetros de optimización esta contendrá el valor null.
         * Ha de ser sustituida por los datos correctos.
         */
        private static TableG_Study_Percent AuxReportG_StudyToTableG_Study(string[] lines, int i, 
            ListFacets lf_diff, ListFacets lf_inst, ListFacets lf)
        {
            string lineFacets = lines[i];
            char[] delimeterCharsFacets = { '\t' };
            string[] arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
            // string[] arrayOfShares = lineFacets.Split(delimeterCharsFacets);

            // Variables
            Dictionary<string, double?> differentiationVar = new Dictionary<string,double?>();
            Dictionary<string, ErrorVar> errorVar  = new Dictionary<string,ErrorVar>();
            Dictionary<string, ErrorVar> percentError = new Dictionary<string,ErrorVar>();

            while (!string.IsNullOrEmpty(lineFacets) && !IsHeaderTableResumRtfTableG_Study(lineFacets))
            {
                if (arrayOfShares[0].Contains(SUSPENSION_POINTS))
                {
                    // Entonces contiene facetas de intrumentación
                    string source = NormalizateDesign(arrayOfShares[1]);
                    double? relErrorVar = StringToDouble(arrayOfShares[2]) ; // Varianza del error relativo
                    double? perct_rel = null;
                    int posAbsErrorVar = 4;
                    if (relErrorVar != null)
                    {
                        perct_rel = StringToDouble(arrayOfShares[3]);
                    }
                    else
                    {
                        posAbsErrorVar = 3;
                    }

                    double? absErrorVar = StringToDouble(arrayOfShares[posAbsErrorVar]); ; // Varianza del error absoluto
                    double? perct_abs = null;
                    if (absErrorVar != null)
                    {
                        perct_abs = StringToDouble(arrayOfShares[posAbsErrorVar+1]);
                    }
                    
                    ErrorVar error = new ErrorVar(relErrorVar, absErrorVar);
                    errorVar.Add(source, error);
                    ErrorVar perct_error = new ErrorVar(perct_rel, perct_abs);
                    percentError.Add(source, perct_error);
                }
                else
                {
                    // Contiene facetas de diferenciación
                    string source = NormalizateDesign(arrayOfShares[0]);
                    double? var_diff = StringToDouble(arrayOfShares[1]);
                    differentiationVar.Add(source, var_diff);
                }

                i++;
                lineFacets = lines[i];
                arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
                // arrayOfShares = lineFacets.Split(delimeterCharsFacets);
            }// end while

            G_ParametersOptimization gP = AuxReaderResumG_Study(lines, i, lf);
            return new TableG_Study_Percent(lf_diff,lf_inst,differentiationVar, errorVar, percentError, gP);
        }// end AuxReportG_StudyToTableG_Study


        /* Descripción:
         *  Devuelve los datos resumenes de la tabla de G_Parámetros (G-Study)
         */
        private static G_ParametersOptimization AuxReaderResumG_Study(string[] lines, int i,
            ListFacets gListFacets)
        {
            string lineFacets = lines[i];
            char[] delimeterCharsFacets = { '\t' };
            string[] arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            double total_differentiation_var = (double)StringToDouble(arrayOfShares[1]); // Suma total de las varianzas de las fuentes objetivo

            double totalRelErrorVar = (double)StringToDouble(arrayOfShares[2]); // Varianza del error relativa
            double totalAbsErrorVar = (double)StringToDouble(arrayOfShares[4]); // Varianza del error absoluta

            lineFacets = lines[i+1];
            arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            double targetStandDev = (double)StringToDouble(arrayOfShares[1]); // desviación típica de las fuentes objetivo
            double errorRelStandDev = (double)StringToDouble(GetRelativeStandarDev(arrayOfShares[2])); // Desviación típica relativa
            double errorAbsStandDev = (double)StringToDouble(GetAbsoluteStandarDev(arrayOfShares[3])); // Desviación típica absoluta


            lineFacets = lines[i + 2];
            arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            double coefG_Rel = (double)StringToDouble(arrayOfShares[1]); // coeficente G relativo

            lineFacets = lines[i + 3];
            arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            double coefG_Abs = (double)StringToDouble(arrayOfShares[1]); // Coeficiente G absoluto


            return new G_ParametersOptimization(gListFacets, total_differentiation_var, coefG_Rel, coefG_Abs,
                totalRelErrorVar, totalAbsErrorVar, errorRelStandDev, errorAbsStandDev, targetStandDev);
        }// end AuxReaderResumG_Study


        /* Descripción:
         *  Devuelve una Lista de G_Parámetros obtenidos de la tabla resumen del final del informe.
         */
        private static List<G_ParametersOptimization> AuxReadTableOptimizationResum(string[] lines, int i)
        {
            // Variable de retorno
            List<G_ParametersOptimization> listG_p = new List<G_ParametersOptimization>();

            string lineFacets = lines[i];
            char[] delimeterCharsFacets = { '\t' };
            string[] arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            int numOfColumns = arrayOfShares.Length / 2;
            List<ListFacets> listOfListFacets = new List<ListFacets>();
            // Inicializamos la estructura
            for (int j = 0; j < numOfColumns; j++)
            {
                listOfListFacets.Add(new ListFacets());
            }
            // Saltamos de línea
            i++;
            lineFacets = lines[i];
            arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            int numOfShares = arrayOfShares.Length;
            while (!lineFacets.Contains(LABEL_END_FACETS_ENG))
            {
                int pos = 0;
                string name = CharNameFacet(arrayOfShares[0]);
                string design = NormalizateDesign(arrayOfShares[0]);
                for (int j = 1; j < numOfShares; j = j + 2)
                {
                    int level = int.Parse(arrayOfShares[j]);
                    int sizeOfUniverse = ConvertStringSizeOfUnivToInt(arrayOfShares[j + 1]);
                    Facet f = new Facet(name, level, "", sizeOfUniverse, design);
                    ListFacets lf = listOfListFacets[pos];
                    lf.Add(f);
                    pos++;
                }

                i++;
                lineFacets = lines[i];
                arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);
            }// end while    

            // Tomamos el resto de parámetros
            //===============================

            // Coeficiente G relativo
            List<double> listOfCoef_G_rel = new List<double>();
            i++;
            lineFacets = lines[i];
            arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            for(int j=1; j <= numOfColumns; j++)
            {
                listOfCoef_G_rel.Add((double)StringToDouble(arrayOfShares[j]));
            }

            // Coeficiente G Absoluto
            List<double> listOfCoef_G_abs = new List<double>();
            i++;
            i++; // saltamos la linea de redondeo
            lineFacets = lines[i];
            arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            for (int j = 1; j <= numOfColumns; j++)
            {
                listOfCoef_G_abs.Add((double)StringToDouble(arrayOfShares[j]));
            }

            // Varianza de error relativa
            List<double> listOfErrorVarRel = new List<double>();
            i++;
            i++; // saltamos la linea de redondeo
            lineFacets = lines[i];
            arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            for (int j = 1; j <= numOfColumns; j++)
            {
                listOfErrorVarRel.Add((double)StringToDouble(arrayOfShares[j]));
            }

            // Varianza de estandar relativa
            List<double> listOfStadarDevRel = new List<double>();
            i++;
            lineFacets = lines[i];
            arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            for (int j = 1; j <= numOfColumns; j++)
            {
                listOfStadarDevRel.Add((double)StringToDouble(arrayOfShares[j]));
            }

            // Varianza de error absoluta
            List<double> listOfErrorVarAbs = new List<double>();
            i++;
            lineFacets = lines[i];
            arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            for (int j = 1; j <= numOfColumns; j++)
            {
                listOfErrorVarAbs.Add((double)StringToDouble(arrayOfShares[j]));
            }

            // Varianza de estandar relativa
            List<double> listOfStadarDevAbs = new List<double>();
            i++;
            lineFacets = lines[i];
            arrayOfShares = lineFacets.Split(delimeterCharsFacets, StringSplitOptions.RemoveEmptyEntries);

            for (int j = 1; j <= numOfColumns; j++)
            {
                listOfStadarDevAbs.Add((double)StringToDouble(arrayOfShares[j]));
            }

            for (int j = 0; j < numOfColumns; j++)
            {
                G_ParametersOptimization gp = new G_ParametersOptimization(listOfListFacets[j], 0,
                    listOfCoef_G_rel[j], listOfCoef_G_abs[j], listOfErrorVarRel[j], listOfErrorVarAbs[j], 
                    listOfStadarDevRel[j], listOfStadarDevAbs[j], 0);
                listG_p.Add(gp);
            }
                
            return listG_p;
        }// end AuxReadTableOptimizationResum

        #endregion Métodos de lectura de informes rtf



        #region Métodos auxiliares
        /*************************************************************************************************
         * Métodos auxiliares
         * ==================
         * - CharNameFacet
         * - NormalizateDesign
         * - ConvertStringSizeOfUnivToInt
         * - StringToDouble
         * - GetRelativeStandarDev
         * - GetAbsoluteStandarDev
         * - MesurementDesignToListFacetDiff
         * - MesurementDesignToListFacetInst
         *************************************************************************************************/

        /* Descripción:
         *  Devuelve  el string (caracter) que contiene el nombre de la faceta. Como en EduG las facetas 
         *  se denotan con una letra será el primer caracter del string de diseño.
         */
        private static string CharNameFacet(string design)
        {
            return design.Substring(0, 1);
        }


        /* Descripción:
         *  Transforma el diseño de una faceta del informe de un diseño normalizado para la creación de
         *  facetas
         */
        private static string NormalizateDesign(string design)
        {
            int n = design.Length;
            string retVal = ""; ;
            for (int i = 0; i < n; i++)
            {
                char c = design[i];
                if (c.Equals(':'))
                {
                    // Si son los dos punto lo agregamos
                    retVal = retVal + c;
                }
                else
                {
                    // Si no ha de tratarse de una letra
                    retVal = retVal + "[" + c + "]";
                }
            }
            return retVal;
        }


        /* Descripción:
        *  Operación auxiliar: convierte un string en el tamaño del universo
        */
        private static int ConvertStringSizeOfUnivToInt(string s)
        {
            int retVal = 0;
            if (Facet.INFINITE.Equals(s.Trim()))
            {
                retVal = int.MaxValue;
            }
            else
            {
                retVal = int.Parse(s);
            }
            return retVal;
        }


        /* Descripción:
         *  Transforma un string en double?, se ha de tener en cuenta de los parentesis o los puntos suspensivos
         *  
         * Parámetros:
         *      string s: es el string que queremos convertir en double.
         *      
         * Excepciones:
         *  Si el string no es un número devolverá una FormatException.
         */
        private static double? StringToDouble(string s)
        {
            double? retVal = null;
            // si el string son los puntos suspensivos no podemos hacer nada

            /* NOTA: Usamos Conatins en lugar de equals por que el numero de puntos depende del tipo
             * de informe. Si es txt usa 3 pero si es rtf usara más.
             */
            if (!s.Contains(SUSPENSION_POINTS))
            {
                // si no son los puntos supensivos puede que el número este entre parentesis
                if (s.StartsWith("(") && s.EndsWith(")"))
                {
                    // si esta entre parentesis los eliminamos
                    int lng = s.Length - 1;
                    s = s.Remove(lng);
                    s = s.Substring(1);
                }
                if (s.Contains(","))
                {
                    // si el numero contiene una coma como separador decimal lo tratamos como refencia cultural en español
                    retVal = double.Parse(s, System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo("es-ES"));
                }
                else
                {
                    // en este caso suponemos que contiene un punto
                    // si el numero contiene un punto como separador decimal lo tratamos como refencia cultural internacional
                    retVal = double.Parse(s, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            return retVal;
        }// end StringToDouble


        /* Descripción:
         *  Devuelve el string que contiene la desviación standard relativa
         */
        private static string GetRelativeStandarDev(string line)
        {
            string newLine = line.Replace(LABEL_RELATIVE_SE_ENG, "");// remplazamos por la cadena vacia
            newLine = newLine.Replace(LABEL_RELATIVE_SE_FR, "");// remplazamos por la cadena vacia
            return newLine.Trim();
        }


        /* Descripción:
         *  Devuelve el string que contiene la desviación standard absoluta
         */
        private static string GetAbsoluteStandarDev(string line)
        {
            string newLine = line.Replace(LABEL_ABSOLUTE_SE_ENG, "");// remplazamos por la cadena vacia
            newLine = newLine.Replace(LABEL_ABSOLUTE_SE_FR, "");// remplazamos por la cadena vacia
            return newLine.Trim();
        }


        /* Descripción:
         *  Devuelve la lista de facetas de diferenciación a partir de la lista de facetas original y la
         *  el string de diseño del informe.
         */
        private static ListFacets MesurementDesignToListFacetDiff(ListFacets lf, string mesurement)
        {
            ListFacets lf_diff = new ListFacets();
            // Eliminamos la parte entrada de texto en ingles
            mesurement = mesurement.Replace(TITLE_MEASUR_DESIGN_ENG, "");
            // Eliminamos la parte entrada de texto en frances
            mesurement = mesurement.Replace(TITLE_MEASUR_DESIGN_FR, "");
            mesurement = mesurement.Replace(")", "").Trim();

            int pos = mesurement.IndexOf("/");

            for (int i = 0; i < pos; i++)
            {
                string c = mesurement.Substring(i, 1);
                Facet f = lf.LookingFacet(c);
                lf_diff.Add(f);
            }

            return lf_diff;
        }


        /* Descripción:
         *  Devuelve la lista de facetas de instrumentación a partir de la lista de facetas original y la
         *  el string de diseño del informe.
         */
        private static ListFacets MesurementDesignToListFacetInst(ListFacets lf, string mesurement)
        {
            ListFacets lf_inst = new ListFacets();
            // Eliminamos la parte entrada de texto en ingles
            mesurement = mesurement.Replace(TITLE_MEASUR_DESIGN_ENG, "");
            // Eliminamos la parte entrada de texto en frances
            mesurement = mesurement.Replace(TITLE_MEASUR_DESIGN_FR, "");
            mesurement = mesurement.Replace(")", "").Trim();

            int pos = mesurement.IndexOf("/") + 1;
            int l = mesurement.Length;

            for (int i = pos; i < l; i++)
            {
                string c = mesurement.Substring(i, 1);
                Facet f = lf.LookingFacet(c);
                lf_inst.Add(f);
            }

            return lf_inst;
        }

        #endregion Métodos auxiliares



        #region Métodos auxiliares de detección de cabeceras
        /*======================================================================================================
         * Métodos auxiliares de detección de cabeceras
         * ============================================
         *  - IsHeaderTableFacetsRTF
         *  - IsHeaderTableFacetsTXT
         *  - IsHeaderTableSquare
         *  - IsEndingTableSquare
         *  - IsLineMesurementDesing
         *  - IsHeaderTableG_ParametersTxtReport
         *  - IsHeaderTableG_ParametersRtfReport
         *  - IsEndingTableG_ParametersTxtReport
         *  - IsHeaderTableResumRtfTableG_Study
         *  - IsHeaderTableResumTxtfTableG_Study
         *  - IsHeaderTableResumOpt
         *======================================================================================================*/

        /* Descripción:
         *  Método auxiliar para determinar si estamos ante la cabeza de una tabla de facetas. Para
         *  un informe rtf.
         */
        private static bool IsHeaderTableFacetsRTF(string line)
        {
            return (line.Contains(LABEL_COLUM_FACET_ENG) && line.Contains(LABEL_COLUM_LABEL_ENG)
                && line.Contains(LABEL_COLUM_LEV_RTF_ENG) && line.Contains(LABEL_COLUM_UNIV_ENG))
                ||
                (line.Contains(LABEL_COLUM_FACET_FR) && line.Contains(LABEL_COLUM_LABEL_RTF_FR)
                && line.Contains(LABEL_COLUM_LEV_RTF_FR) && line.Contains(LABEL_COLUM_UNIV_RTF_FR));
        }


        /* Descripción:
        *  Método auxiliar para determinar si estamos ante la cabeza de una tabla de facetas. Para un
         *  informe txt.
        */
        private static bool IsHeaderTableFacetsTXT(string line)
        {
            return (line.Contains(LABEL_COLUM_FACET_ENG) && line.Contains(LABEL_COLUM_LABEL_ENG)
                && line.Contains(LABEL_COLUM_LEV_TXT_ENG) && line.Contains(LABEL_COLUM_UNIV_ENG))
                ||
                (line.Contains(LABEL_COLUM_FACET_FR) && line.Contains(LABEL_COLUM_LABEL_TXT_FR)
                && line.Contains(LABEL_COLUM_LEV_TXT_FR) && line.Contains(LABEL_COLUM_UNIV_TXT_FR));
        }


        /* Descripción:
         *  Método auxiliar para determinar si estamos ante la cabecera de una tabla de suma de cuadrados
         */
        private static bool IsHeaderTableSquare(string line)
        {
            return (line.Contains(LABEL_COLUM_SOURCE_ENG) && line.Contains(LABEL_COLUM_SS_ENG)
                && line.Contains(LABEL_COLUM_DF_ENG) && line.Contains(LABEL_COLUM_MS_ENG) 
                && line.Contains(LABEL_COLUM_RANDOM_ENG) && line.Contains(LABEL_COLUM_MIXED_ENG)
                && line.Contains(LABEL_COLUM_CORRECT_ENG))
                ||
                (line.Contains(LABEL_COLUM_SOURCE_FR) && line.Contains(LABEL_COLUM_SS_FR)
                && line.Contains(LABEL_COLUM_DF_FR) && line.Contains(LABEL_COLUM_MS_FR)
                && (line.Contains(LABEL_COLUM_RANDOM_RTF_FR) || line.Contains(LABEL_COLUM_RANDOM_TXT_FR)) 
                && line.Contains(LABEL_COLUM_MIXED_FR) && line.Contains(LABEL_COLUM_CORRECT_FR));
        }


        /* Descrtipción:
         *  Método auxiliar para determinar si es el final de una tabla de análisis de varianza
         */
        private static bool IsEndingTableSquare(string line)
        {
            return (line.Contains(END_TABLE_ANALYSIS_ENG) && line.Contains(LABEL_PERCENT_ENG));
        }


        /* Descripción:
         *  Indica que estamos ante la línea de diseño de medida.
         */
        private static bool IsLineMesurementDesing(string line)
        {
            return (line.Contains(TITLE_MEASUR_DESIGN_ENG) || line.Contains(TITLE_MEASUR_DESIGN_FR));
        }


        /* Descripción:
         *  Método auxiliar para determinar si estamos ante la cabecera de una tabla de G-Parámetros.
         *  
         * NOTA:
         * =====
         *  Este método solo es válido para informes txt.
         */
        private static bool IsHeaderTableG_ParametersTxtReport(string line)
        {
            return ((line.Contains(LABEL_VARIANCE_ENG) && line.Contains(LABEL_VAR_ENG) 
                && line.Contains(LABEL_REL_ENG) && line.Contains(LABEL_ABS_ENG))
                ||
                (line.Contains(LABEL_VAR_ENG) && line.Contains(LABEL_REL_ENG)
                && line.Contains(LABEL_ABS_ENG)));
        }


        /* Descripción:
         *  Método auxiliar para determinar si estamos ante la cabecera de una tabla de G-Parámetros
         *  
         * NOTA:
         * =====
         *  Este método solo es válido para informes rtf.
         */
        private static bool IsHeaderTableG_ParametersRtfReport(string line)
        {
            return (line.Contains(LABEL_VARIANCE_ENG) && line.Contains(LABEL_PERCENT_ENG))
                ||
                (line.Contains(LABEL_VARIANCE_FR) && line.Contains(LABEL_PERCENT_ENG));
        }


        /* Descripción:
         *  Contiene la cadena que "marca" el fin de la tabla de G-Parametros en un informe de
         *  suma de cuadrados con formato .txt
         */
        private static bool IsEndingTableG_ParametersTxtReport(string line)
        {
            return (line.Contains(END_OF_TABLE_STUDY_TXT)
                ||
                line.Contains(END_OF_TABLE_STUDY_FR_TXT));
        }


        /* Descripción:
         *  Determina si estamos en el segmento de la tabla que contiene el resumen de datos.
         *  Este método solo es valido para los ficheros con extensión .rtf ya que el formato
         *  del informe es distinto.
         */
        private static bool IsHeaderTableResumRtfTableG_Study(string line)
        {
            return line.Contains(END_OF_TABLE_STUDY_ENG_RTF)
                ||
                line.Contains(END_OF_TABLE_STUDY_FR_RTF);
        }


        /* Descripción:
         *  Determina si estamos en el segmento de la tabla que contiene el resumen de datos.
         *  Este método solo es valido para los ficheros con extensión .txt ya que el formato
         *  del informe es distinto.
         */
        private static bool IsHeaderTableResumTxtfTableG_Study(string line)
        {
            return (line.Contains(END_OF_TABLE_STUDY_TXT) && line.Contains(LABEL_PERCENT_ENG)); 
        }


        /* Descripción:
         *  Determina el comienzo de una tabla de resumen de optimización.
         */
        private static bool IsHeaderTableResumOpt(string line)
        {
            return (line.Contains(LABEL_BEGIN_1_ENG) && line.Contains(LABEL_BEGIN_2_ENG))
                || 
                (line.Contains(LABEL_BEGIN_1_FR) && line.Contains(LABEL_BEGIN_2_FR));
        }

        #endregion Métodos auxiliares de detección de cabeceras


        /* Descipción:
         *  Devuelve un string que representa la fecha de emisión del informe obtenido de la línea que
         *  se pasa como parámetro.
         */
        public static DateTime RecoveryStringDate(string line)
        {
            int pos = line.LastIndexOf('[');
            string sDate = line.Substring(pos);
            char[] charDelimiters = {'[', '-', ' ', ':', ']' };
            string[] arrayShares = sDate.Split(charDelimiters, StringSplitOptions.RemoveEmptyEntries);
            int year = int.Parse(arrayShares[0]);
            int month = int.Parse(arrayShares[1]);
            int day = int.Parse(arrayShares[2]);
            int hour = int.Parse(arrayShares[3]);
            int minute = int.Parse(arrayShares[4]);
            return new DateTime(year, month, day, hour, minute, 0);
        }

        /* Decripción:
         *  Importa la suma de cuadrados de un fichero texto (que contiene la suma de cuadrados) 
         *  exportado por EduG 6.0
         *  El fichero tiene el formato de un fichero .csv (valores separados por comas) en el cual
         *  el separador es el caracter ';', contiene tres columnas: 1º Diseño de faceta, 2º suma 
         *  de cuadrados y 3º grado de libertad.
         * Parámetros
         *      string pathFile: dirección y nombre del fichero del que se optienen los datos.
         * Excepciones: 
         *      AnalysisSsqEduG_Exception: si optiene errores en el formato del fichero (lectura de
         *              suma de cuadrados).
         */
        public static Dictionary<string, double?> ImportSsq_File(string pathFile)
        {
            Dictionary<string, double?> ssq_retVal = new Dictionary<string, double?>();
            using (StreamReader reader = new StreamReader(pathFile))
            {
                try
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Dividimos la línea usando el caracter ';'
                        char[] delimeterChars = { ';' };
                        string[] arrayItems = line.Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
                        string normalizateDesign = NormalizateDesign(arrayItems[0]);
                        ssq_retVal.Add(normalizateDesign, (double)ConvertNum.String2Double(arrayItems[1]));
                    }
                }
                catch (FormatException ex)
                {
                    throw new AnalysisSsqEduG_Exception("Error de formato de fichero: " + ex.Message);
                }

            }
            return ssq_retVal;
        }// end ImportSsq_File


        /* Descripción:
         *  Exporta los datos siguiendo el formato usado por EduG, lo que no garantiza la compatibilidad
         *  con este programa ya que Las facetas solo pueden ser nombradas con una sola letra, en 
         *  mayuscula y sin corchetes. Tampoco acepta valores nulos.
         * Parámetros:
         *      TableAnalysisOfVariance tbAnalysis: Tabla de analisis de varianza de la que se van a extraer
         *          los datos que vamos a exportar.
         *      string pathFile: nombre y ruta del fichero al que vamos a exportar los datos.
         */
        public static bool WritingFileExportEduG_Ssq(TableAnalysisOfVariance tbAnalysis , string pathFile)
        {
            bool res = false; // variable de retorno

            using (StreamWriter writer = new StreamWriter(pathFile))
            {
                List<string> lt_keys = tbAnalysis.ListFacets().CombinationStringWithoutRepetition();
                int numKeys = lt_keys.Count;

                for(int i =0; i < numKeys; i++)
                {
                    string key = lt_keys[i];
                    string design = key.Replace("[", "");
                    design = design.Replace("]", "");
                    writer.WriteLine(design.ToUpper() + ";" + tbAnalysis.SSQ(key) + ";" + tbAnalysis.DegreesOfFreedom(key));
                }

                res = true;
            }
            return res;
        }// end WritingFileExportEduG_Ssq

    }// end public class AnalysisSsqEduG
}// end namespace ImportEduGSsq