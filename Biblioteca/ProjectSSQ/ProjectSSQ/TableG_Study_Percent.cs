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
 * Fecha de revisión: 20/Jun/2012                           
 * 
 * Descripción:
 *      Clase TableG_Study_Percent que hereda de TableG_Study y que incorpora nuevas variable y operaciones 
 *      para el calculo del el porcentage de error
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using MultiFacetData;
using AuxMathCalcGT;

namespace ProjectSSQ
{
    public class TableG_Study_Percent : TableG_Study, System.ICloneable
    {
        /******************************************************************************************************
         *  Constantes de clase TableG_Study_Percent
         ******************************************************************************************************/
        // Comienzo y fin de una tabla G_Study con columnas de porcentaje de error
        public const string BEGIN_TABLE_G_STUDY_PERCENT = "<table_g_study_percent>";
        const string END_TABLE_G_STUDY_PERCENT = "</table_g_study_percent>";
        // Comienzo y fin de la lista de facetas de diferenciación
        const string BEGIN_DIFFERENTIATION_FACETS = "<differentiation_facets>";
        const string END_DIFFERENTIATION_FACETS = "</differentiation_facets>";
        // Comienzo y fin de la lista de facetas de instrumentación
        const string BEGIN_INSTRUMENTATION_FACETS = "<instrumentation_facets>";
        const string END_INSTRUMENTATION_FACETS = "</instrumentation_facets>";
        // Comienzo y fin de las "filas" de datos de la varianza de diferenciación
        const string BEGIN_DIFFERENTIATION_ROW = "<differentiation_row>";
        const string END_DIFFERENTIATION_ROW = "</differentiation_row>";
        // Comienzo y fin de las "filas" de datos de la varianza de instrumentación
        const string BEGIN_INSTRUMENTATION_ROW = "<instrumentation_row>";
        const string END_INSTRUMENTATION_ROW = "</instrumentation_row>";


        /******************************************************************************************************
         * Variables de Clase
         ******************************************************************************************************/
        private Dictionary<string, ErrorVar> percentError; // Porcentaje de error



        #region Constructores de la clase TableG_Study_Percent
        /******************************************************************************************************
         * Constructores
         ******************************************************************************************************/

        public TableG_Study_Percent()
            : base()
        {
            this.percentError = new Dictionary<string, ErrorVar>();
        }


        public TableG_Study_Percent(ListFacets diff, ListFacets inst)
            : base(diff, inst)
        {
        }


        public TableG_Study_Percent(ListFacets differentiation, ListFacets instrumentation, 
            Dictionary<string, double?> diffVar, Dictionary<string, ErrorVar> errorVar,
            G_ParametersOptimization gp, Dictionary<string, ErrorVar> percentError)
            : base(differentiation, instrumentation, diffVar, errorVar, gp)
        {
            this.percentError = percentError;
        }


        public TableG_Study_Percent(ListFacets differentiation, ListFacets instrumentation, 
            TableAnalysisOfVariance tableVariance)
            :base(differentiation, instrumentation, tableVariance)
        {
            CalcPercent();
        }


        public TableG_Study_Percent(ListFacets differentiation, ListFacets instrumentation,
            double coefG_Rel, double coefG_Abs, double totalRelErrorVar, double totalAbsErrorVar,
            double errorRelStandDev, double errorAbsStandDev)
                : base(differentiation,instrumentation, coefG_Rel, coefG_Abs, totalRelErrorVar, totalAbsErrorVar,
                    errorRelStandDev, errorAbsStandDev)
        {
            CalcPercent();
        }


        public TableG_Study_Percent(ListFacets differentiation, ListFacets instrumentation,
            Dictionary<string, double?> diffVar, Dictionary<string, ErrorVar> errorVar,
            double coefG_Rel, double coefG_Abs, double totalRelErrorVar, double totalAbsErrorVar,
            double errorRelStandDev, double errorAbsStandDev, double total_differentiation_var, 
            double targetStandDev)
            : base(differentiation, instrumentation, diffVar, errorVar, coefG_Rel, coefG_Abs,
                    totalRelErrorVar, totalAbsErrorVar, errorRelStandDev, errorAbsStandDev, total_differentiation_var, 
                    targetStandDev)
        {
            CalcPercent();
        }


        public TableG_Study_Percent(ListFacets differentiation, ListFacets instrumentation,
            Dictionary<string, double?> diffVar, Dictionary<string, ErrorVar> errorVar,
            Dictionary<string, ErrorVar> percent, G_ParametersOptimization gp)
            : base(differentiation, instrumentation, diffVar, errorVar, gp)
        {
            this.percentError = percent;
        }



        public TableG_Study_Percent(TableG_Study tableG)
            : this(tableG.LfDifferentiation(), tableG.LfInstrumentation(), tableG.Target(), tableG.Error(),
            tableG.CoefG_Rel(), tableG.CoefG_Abs(), tableG.TotalRelErrorVar(), tableG.TotalAbsErrorVar(), 
            tableG.ErrorRelStandDev(), tableG.ErrorAbsStandDev(), tableG.TotalDifferentiationVariance(),
            tableG.TargetStandDev())
        {
        }

        #endregion Constructores de la clase TableG_Study_Percent


        /******************************************************************************************************
         * Métodos de consulta
         ******************************************************************************************************/

        /* Descripción:
         *  Devuelve los porcentajes de error relativo y error absoluto para una lista de facetas dada que se usa
         *  como clave.
         */
        public ErrorVar Percent(string key)
        {
            return this.percentError[key];
        }


        /* Descripción:
         *  Devuelve los porcentajes de error relativo y error absoluto para una lista de facetas dada que se usa
         *  como clave.
         */
        public Dictionary<string, ErrorVar> Percent()
        {
            return this.percentError;
        }


        /******************************************************************************************************
         * Métodos de instancias
         ******************************************************************************************************/

        /* Descripción:
         *  Calcula los porcetaje de error para cada una de las varianzas y las introduce en la estructura.
         */
        public void CalcPercent()
        {
            this.percentError = new Dictionary<string, ErrorVar>();

            double totalVarErrorRel = this.TotalRelErrorVar();
            double totalVarErrorAbs = this.TotalAbsErrorVar();

            int n = this.Error().Keys.Count;
            List<string> list_of_keys = new List<string>(this.Error().Keys);
            for (int i = 0; i < n; i++)
            {
                string lf_key = list_of_keys[i];
                ErrorVar err_Var = this.Error(lf_key);
                double? percentErrorRel = err_Var.RelErrorVar();
                if (percentErrorRel != null)
                {
                    percentErrorRel = ((double)percentErrorRel / totalVarErrorRel)*100;
                }
                
                double? percentErrorAbs = err_Var.AbsErrorVar();
                if (percentErrorAbs != null)
                {
                    percentErrorAbs = ((double)percentErrorAbs / totalVarErrorAbs)*100;
                }
                ErrorVar percent_err_Var = new ErrorVar(percentErrorRel,percentErrorAbs);
                this.percentError.Add(lf_key, percent_err_Var);
            } 
        }



        #region Implementacion de la interfaz
        /******************************************************************************************************
         *  Implementacion de la interfaz Cloneable
         *  =======================================
         ******************************************************************************************************/

        /* Descripción:
         *  Devuelve una copy en profundidad del objeto.
         */
        public object Clone()
        {
            // Copiamos la fuentes de diferenciación, 
            ListFacets copyLfDifferentiation = (ListFacets)this.LfDifferentiation().Clone();
            // Copiamos la fuentes de instrumentación
            ListFacets copyLfInstrumentation = (ListFacets)this.LfInstrumentation().Clone();
            Dictionary<string, double?> copyDifferentiationVar = ClonarDictionary(this.Target());
            Dictionary<string, ErrorVar> copyErrorVar = ClonarDictionary(this.Error());

            G_ParametersOptimization copyG_parameterOptimization = (G_ParametersOptimization)this.G_ParametersOptimization().Clone();

            Dictionary<string, ErrorVar> copyPercentError = ClonarDictionary(percentError);

            return new TableG_Study_Percent(copyLfDifferentiation, copyLfInstrumentation,
                copyDifferentiationVar, copyErrorVar, copyPercentError, copyG_parameterOptimization);
        }


        /* Descripción:
         *  Método auxiliar de Clone. Copia los elementos de un diccionario donde la clave es un string 
         *  y el objeto es un double.
         */
        private static Dictionary<string, double?> ClonarDictionary(Dictionary<string, double?> original)
        {
            Dictionary<string, double?> copy = new Dictionary<string, double?>(); // Copia a retornar

            foreach (string skey in original.Keys)
            {
                string copyKey = string.Copy(skey);
                double? d = original[skey];

                copy.Add(copyKey, d);
            }
            return copy;
        }


        /* Descripción:
         *  Método auxiliar de Clone. Copia los elementos de un diccionario donde la clave es un string 
         *  y el objeto es un ErrorVar.
         */
        private static Dictionary<string, ErrorVar> ClonarDictionary(Dictionary<string, ErrorVar> original)
        {
            Dictionary<string, ErrorVar> copy = new Dictionary<string, ErrorVar>(); // Copia a retornar

            foreach (string skey in original.Keys)
            {
                string copyKey = string.Copy(skey);
                ErrorVar e = (ErrorVar)original[skey].Clone();

                copy.Add(copyKey, e);
            }

            return copy;
        }
        #endregion Implementacion de la interfaz


        #region Escritura de datos de una Tabla de G_Study con porcentaje de error
        /***********************************************************************************************
         * Métodos de para la escritura de datos
         *  - WritingStreamTableG_Study_Percent
         ***********************************************************************************************/

        /* Descripción:
         *  Escribe en el StreamWriter 
         */
        public bool WritingStreamTableG_Study_Percent(StreamWriter writerFile)
        {
            bool res = false; // variable de retorno

            // Escribimos la marcha de comienzo de la tabla G_Study Percent
            writerFile.WriteLine(BEGIN_TABLE_G_STUDY_PERCENT);

            // Escribimos la lista de facetas de diferenciación
            res = this.LfDifferentiation().WritingStreamListFacets(writerFile);

            // Escribimos las filas de datos de las varianzas de diferenciación
            if (res)
            {//(* 1 *)
                // Escribimos la marcha de comienzo de las varianzas de diferenciación
                writerFile.WriteLine(BEGIN_DIFFERENTIATION_ROW);

                foreach(string key in this.Target().Keys)
                {
                    // res = key.WritingStreamListFacets(writerFile);
                    writerFile.WriteLine(key + " " + this.Target(key));
                }

                // Escribimos la marcha de fin de las varianzas de diferenciación
                writerFile.WriteLine(END_DIFFERENTIATION_ROW);

                // Escribimos la lista de facetas de instrumentación
                this.LfInstrumentation().WritingStreamListFacets(writerFile);

                // Escribimos la marcha de comienzo de las varianzas de instrumentación
                writerFile.WriteLine(BEGIN_INSTRUMENTATION_ROW);

                foreach (string key in this.Error().Keys)
                {
                    /* Se produce un error con los nulos
                     */
                    // res = key.WritingStreamListFacets(writerFile);
                    ErrorVar e = this.Error(key);
                    ErrorVar p = this.percentError[key];
                    string e_rel = ConvertNum.Double2String(e.RelErrorVar());
                    string p_rel = ConvertNum.Double2String(p.RelErrorVar());
                    string e_abs = ConvertNum.Double2String(e.AbsErrorVar());
                    string p_abs = ConvertNum.Double2String(p.AbsErrorVar());
                    writerFile.WriteLine(key + " " + e_rel + " " + p_rel + " " + e_abs + " " + p_abs);
                }

                // Escribimos la marcha de fin de las varianzas de instrumentación
                writerFile.WriteLine(END_INSTRUMENTATION_ROW);

                // Escribimos los datos de los Coeficientes de generalizabilidad
                this.G_ParametersOptimization().WritingStreamGParametersOptimization(writerFile);

            }//end if (* 1 *)

            // escribimos el fin de la tabla de análisis de varianza
            writerFile.WriteLine(END_TABLE_G_STUDY_PERCENT);
            return res;
        }// end WritingStreamTableG_Study_Percent

        #endregion Escritura de datos de una Tabla de G_Study con porcentaje de error


        #region Lectura de datos de una Tabla de G_Study con porcentaje de error
        /***********************************************************************************************
         * Métodos de para la lectura de datos
         *  - ReadingStreamTableG_Study_Percent
         ***********************************************************************************************/

        /* Descripción:
         *  Lee los datos de una Tabla G_Study con porcentaje de error desde un stream y lo devuelve como 
         *  objeto.
         * Parámetros:
         *      StreamReader reader: El stream del que vamos a leer la Tabla G_Study.
         */
        public static TableG_Study_Percent ReadingStreamTableG_Study_Percent(StreamReader reader)
        {
            TableG_Study_Percent tableG_Study_P = null;
            try
            {
                string line;
                ListFacets lf_diff; // Lista de facetas de diferenciación
                ListFacets lf_inst; // Lista de facetas de instrumentación
                Dictionary<string, double?> diffVar = new Dictionary<string, double?>();
                Dictionary<string, ErrorVar> instVar = new Dictionary<string, ErrorVar>();
                Dictionary<string, ErrorVar> percent = new Dictionary<string, ErrorVar>();
                G_ParametersOptimization gp;

                // Leemos la lista de facetas de diferenciación
                if ((line = reader.ReadLine()).Equals(MultiFacetData.ListFacets.BEGIN_LISTFACETS))
                {
                    lf_diff = MultiFacetData.ListFacets.ReadingStreamListFacets(reader);
                }
                else
                {
                    throw new TableG_Study_PercentException();
                }

                // Leemos las filas de varianzas de diferenciación
                if ((line = reader.ReadLine()).Equals(BEGIN_DIFFERENTIATION_ROW))
                {
                    while (!(line = reader.ReadLine()).Equals(END_DIFFERENTIATION_ROW))
                    {
                        // Leemos la linea con los datos
                        // line = reader.ReadLine();
                        char[] delimeterChars = { ' ' }; // nuestro delimitador será el caracter blanco
                        string[] arrayOfDouble = line.Trim().Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);

                        string key = arrayOfDouble[0];
                        // double? var = double.Parse(arrayOfDouble[1]);
                        double? var = ConvertNum.String2Double(arrayOfDouble[1]);
                        diffVar.Add(key, var);
                    }
                }
                else
                {
                    throw new TableG_Study_PercentException();
                }
                
                // Leemos la lista de facetas de instrumentación
                if ((line = reader.ReadLine()).Equals(MultiFacetData.ListFacets.BEGIN_LISTFACETS))
                {
                    lf_inst = MultiFacetData.ListFacets.ReadingStreamListFacets(reader);
                }
                else
                {
                    throw new TableG_Study_PercentException();
                }
                
                 // Leemos las filas de varianzas de instrumentación
                if ((line = reader.ReadLine()).Equals(BEGIN_INSTRUMENTATION_ROW))
                {
                    //if (line.Equals(MultiFacetData.ListFacets.BEGIN_LISTFACETS))
                    //{
                    //    ListFacets lf_intrumentation = MultiFacetData.ListFacets.ReadingStreamListFacets(reader);
                    //}
                    //else
                    //{
                    //    throw new TableG_Study_PercentException();
                    //}

                    while (!(line = reader.ReadLine()).Equals(END_INSTRUMENTATION_ROW))
                    {
                        // Leemos la linea con los datos
                        // line = reader.ReadLine();
                        char[] delimeterChars = { ' ' }; // nuestro delimitador será el caracter blanco
                        string[] arrayOfDouble = line.Trim().Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);

                        string key = arrayOfDouble[0];
                        double? varInstRel = ConvertNum.String2Double(arrayOfDouble[1]);
                        double? perct_error_rel = ConvertNum.String2Double(arrayOfDouble[2]);
                        double? varInstAbs = ConvertNum.String2Double(arrayOfDouble[3]);
                        double? perct_error_abs = ConvertNum.String2Double(arrayOfDouble[4]);
                        ErrorVar e = new ErrorVar(varInstRel, varInstAbs);
                        instVar.Add(key, e);
                        ErrorVar p = new ErrorVar(perct_error_rel, perct_error_abs);
                        percent.Add(key, p);
                            
                    }
                }
                else
                {
                    throw new TableG_Study_PercentException();
                }

                // Leemos los datos de los G_ParametersOptimization
                if ((line = reader.ReadLine()).Equals(ProjectSSQ.G_ParametersOptimization.BEGIN_G_PARAMETERS_OPT))
                {
                    gp = ProjectSSQ.G_ParametersOptimization.ReadingStreamGParametersOptimization(reader);
                }
                else
                {
                    throw new TableG_Study_PercentException();
                }

                if ((line = reader.ReadLine()).Equals(END_TABLE_G_STUDY_PERCENT))
                {
                    tableG_Study_P = new TableG_Study_Percent(lf_diff, lf_inst, diffVar, instVar, percent, gp);
                }
                else
                {
                    throw new TableG_Study_PercentException();
                }
            }
            catch (FormatException)
            {
                throw new TableG_Study_PercentException("Error al leer de fichero");
            }
            catch (ListFacetsException)
            {
                throw new TableG_Study_PercentException("Error al leer de fichero");
            }

            return tableG_Study_P;
        }// end ReadingStreamTableG_Study_Percent

        #endregion Lectura de datos de una Tabla de G_Study con porcentaje de error


        #region Conversión con DataSet
        /* Descripción:
         *  Devuelve un dataSet con los datos del objeto
         */
        public DataSet TableG_Study2TableSet(List<string> ldesign)
        {
            // Creamos el dataSet
            DataSet dsTbAnalysis = new DataSet("DataSet_Table_G_Study");

            // Optenemos la lista de facetas
            DataTable dtListFacets_Dif = this.LfDifferentiation().ListFacets2DataTable("TbListFacets_Diff");
            DataTable dtSkipLevels_Dif = this.LfDifferentiation().SkipLevels2DataTable("TbSkipLevels_Diff");
            DataTable dtListFacets_Ins = this.LfInstrumentation().ListFacets2DataTable("TbListFacets_Ins");
            DataTable dtSkipLevels_Ins = this.LfInstrumentation().SkipLevels2DataTable("TbSkipLevels_Ins");
            
            // Añadimos el dataTable al dataSet;
            dsTbAnalysis.Tables.Add(dtListFacets_Dif);
            dsTbAnalysis.Tables.Add(dtSkipLevels_Dif);
            dsTbAnalysis.Tables.Add(dtListFacets_Ins);
            dsTbAnalysis.Tables.Add(dtSkipLevels_Ins);

            // Creamos el datatable para la varianza de diferenciación
            DataTable dtDiff = new DataTable("DataTableDiff");
            // Añadimos las columnas
            dtDiff.Columns.Add(new DataColumn("source", System.Type.GetType("System.String")));
            dtDiff.Columns.Add(new DataColumn("var_diff", System.Type.GetType("System.Double")));
            // Añadimos el dataTable al dataSet;
            dsTbAnalysis.Tables.Add(dtDiff);

            // Creamos el dataTable para la varianza de instrumentación
            DataTable dtIns = new DataTable("DataTableIns");
            // Añadimos las columnas
            dtIns.Columns.Add(new DataColumn("source", System.Type.GetType("System.String")));
            dtIns.Columns.Add(new DataColumn("rel_error_var", System.Type.GetType("System.Double")));
            dtIns.Columns.Add(new DataColumn("rel_error_percent", System.Type.GetType("System.Double")));
            dtIns.Columns.Add(new DataColumn("abs_error_var", System.Type.GetType("System.Double")));
            dtIns.Columns.Add(new DataColumn("abs_error_percent", System.Type.GetType("System.Double")));
            // Añadimos el dataTable al dataSet;
            dsTbAnalysis.Tables.Add(dtIns);

            int numOfSource = ldesign.Count;

            for (int i = 0; i < numOfSource; i++)
            {
                string key = ldesign[i];
                if (this.TargetContainsKey(key))
                {
                    // Es una fuente de diferenciación
                    DataRow row = dtDiff.NewRow();
                    row["source"] = key;
                    row["var_diff"] = Target(key);
                    dtDiff.Rows.Add(row);
                }
                else if (this.ErrorContainsKey(key))
                {
                    // Es una fuente de instrumentación
                    DataRow row = dtIns.NewRow();
                    row["source"] = key;
                    // Si pertenece a las facetas de instrumentación
                    ErrorVar error_var = this.Error(key);
                    double? rel_error_var = error_var.RelErrorVar();
                    double? abs_error_var = error_var.AbsErrorVar();
                    if (rel_error_var != null)
                    {
                        row["rel_error_var"] = rel_error_var;
                    }
                    if (abs_error_var != null)
                    {
                        row["abs_error_var"] = abs_error_var;
                    }
                    ErrorVar error_var_percent = this.percentError[key];
                    double? rel_error_percent = error_var_percent.RelErrorVar();
                    if (rel_error_percent != null)
                    {
                        row["rel_error_percent"] = rel_error_percent;
                    }
                    double? abs_error_percent = error_var_percent.AbsErrorVar();
                    if (abs_error_percent != null)
                    {
                        row["abs_error_percent"] = abs_error_percent;
                    }

                    dtIns.Rows.Add(row);
                }
            }

            return dsTbAnalysis;
        }// end TableG_Study2TableSet


        /* Descripción:
         *  Devuelve un objeto con los datos extraidos del dataSet que se pasa como parámetro
         */
        public static TableG_Study_Percent DataSet2TableG_Study(DataSet dsG_Study, ListFacets lf)
        {
            // Obtenemos la lista de facetas de differenciación
            DataTable dtListFacets_Dif = dsG_Study.Tables["TbListFacets_Diff"];
            DataTable dtSkipLevels_Dif = dsG_Study.Tables["TbSkipLevels_Diff"];
            ListFacets lfDiff = MultiFacetData.ListFacets.DataTables2ListFacets(dtListFacets_Dif, dtSkipLevels_Dif);

            // Obtenemos la lista de facetas de instrumentación
            DataTable dtListFacets_Ins = dsG_Study.Tables["TbListFacets_Ins"];
            DataTable dtSkipLevels_Ins = dsG_Study.Tables["TbSkipLevels_Ins"];
            ListFacets lfInst = MultiFacetData.ListFacets.DataTables2ListFacets(dtListFacets_Ins, dtSkipLevels_Ins);

            Dictionary<string, double?> diffVar = new Dictionary<string, double?>();
            double totalDiff_var = 0;

            DataTable dtDiffVar = dsG_Study.Tables["DataTableDiff"];

            int numRows = dtDiffVar.Rows.Count;
            for (int i = 0; i < numRows; i++)
            {
                DataRow row = dtDiffVar.Rows[i];
                string source = (string)row["source"];
                double? diff_var = (double?)row["var_diff"];
                if (diff_var != null)
                {
                    totalDiff_var += (double)diff_var;
                }
                diffVar.Add(source, diff_var);
            }

            DataTable dtInstVar = dsG_Study.Tables["DataTableIns"];
            
            Dictionary<string, ErrorVar> errorVar = new Dictionary<string,ErrorVar>();
            Dictionary<string, ErrorVar> percent = new Dictionary<string, ErrorVar>();
            double totalErrorRel = 0;
            double totalErrorAbs = 0;

            numRows = dtInstVar.Rows.Count;
            for(int i = 0; i < numRows; i++)
            {
                DataRow row = dtInstVar.Rows[i];
                string source = (string)row["source"];
                double? rel_error_var = null;
                if (!string.IsNullOrEmpty(row["rel_error_var"].ToString()))
                {
                    rel_error_var = (double?)row["rel_error_var"];
                }
                // double? rel_error_var = (double?)row["rel_error_var"];
                if (rel_error_var != null)
                {
                    totalErrorRel += (double)rel_error_var;
                }

                double? abs_error_var = null;
                if (!string.IsNullOrEmpty(row["abs_error_var"].ToString()))
                {
                    abs_error_var = (double?)row["abs_error_var"];
                }
                
                if (abs_error_var != null)
                {
                    totalErrorAbs += (double)abs_error_var;
                }
                ErrorVar var = new ErrorVar(rel_error_var, abs_error_var);
                errorVar.Add(source, var);
                double? rel_error_percent = null;
                if(!string.IsNullOrEmpty(row["rel_error_percent"].ToString()))
                {
                    rel_error_percent = (double?)row["rel_error_percent"];
                }
                double? abs_error_percent = null;
                if (!string.IsNullOrEmpty(row["abs_error_percent"].ToString()))
                {
                    abs_error_percent = (double?)row["abs_error_percent"];
                }
                ErrorVar per = new ErrorVar(rel_error_percent, abs_error_percent);
                percent.Add(source, per);
            }

            G_ParametersOptimization gp = new G_ParametersOptimization(lf, totalDiff_var, totalErrorRel, totalErrorAbs);

            return new TableG_Study_Percent(lfDiff, lfInst, diffVar, errorVar, gp, percent); ;
        }// end DataSet2TableG_Study

        #endregion Conversión con DataSet

    }// end public class TableG_Study_Percent : TableG_Study
}// end namespace ProjectSSQ
