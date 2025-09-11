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
 * Fecha de revisión: 29/Jun/2012
 * 
 * Descripción:
 *      Se construye con los parámetros optenidos tras una optimización.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using AuxMathCalcGT;
using MultiFacetData;

namespace ProjectSSQ
{
    public class G_ParametersOptimization : System.ICloneable 
    {
        /******************************************************************************************************
         *  Constantes de clase G_ParametersOptimization
         ******************************************************************************************************/
        // Comienzo y fin de un G Parámetro de Optimización
        public const string BEGIN_G_PARAMETERS_OPT = "<g_parameters_opt>";
        const string END_G_PARAMETERS_OPT = "</g_parameters_opt>";


        /******************************************************************************************************
         *  Variables de clase Analysis_and_G_Study
         ******************************************************************************************************/
        private ListFacets gListFacets; // Lista de Facetas a la que pertenecen los G_Parámetros

        private double total_differentiation_var; // Suma total de las varianzas de las fuentes objetivo
        private double coefG_Rel; // coeficente G relativo
        private double coefG_Abs; // Coeficiente G absoluto
        private double totalRelErrorVar; // Varianza del error relativa
        private double totalAbsErrorVar; // Varianza del error absoluta
        private double errorRelStandDev; // Desviación típica relativa
        private double errorAbsStandDev; // Desviación típica absoluta
        private double targetStandDev;   // desviación típica de las fuentes objetivo



        /***********************************************************************************************
         * Constructores
         ***********************************************************************************************/

        public G_ParametersOptimization(ListFacets lf, double total_differentiation_var, double totalRelErrorVar,
            double totalAbsErrorVar)
        {
            this.gListFacets = lf;
            this.total_differentiation_var = total_differentiation_var;
            this.totalRelErrorVar = totalRelErrorVar;
            this.totalAbsErrorVar = totalAbsErrorVar;

            this.coefG_Rel = (total_differentiation_var / (total_differentiation_var + totalRelErrorVar));
            this.coefG_Abs = (total_differentiation_var / (total_differentiation_var + totalAbsErrorVar));

            this.errorAbsStandDev = Math.Sqrt(totalAbsErrorVar);
            this.errorRelStandDev = Math.Sqrt(totalRelErrorVar);
            this.targetStandDev = Math.Sqrt(total_differentiation_var);

        }


        public G_ParametersOptimization(ListFacets lf, double total_differentiation_var, double coefG_Rel, double coefG_Abs,
            double totalRelErrorVar, double totalAbsErrorVar, double errorRelStandDev, double errorAbsStandDev,
            double targetStandDev)
        {
            this.gListFacets = lf;
            this.total_differentiation_var = total_differentiation_var;
            this.coefG_Rel = coefG_Rel;
            this.coefG_Abs = coefG_Abs;
            this.totalRelErrorVar = totalRelErrorVar;
            this.totalAbsErrorVar = totalAbsErrorVar;
            this.errorAbsStandDev = errorAbsStandDev;
            this.errorRelStandDev = errorRelStandDev;
            this.targetStandDev = targetStandDev;
        }



        /***********************************************************************************************
         * Métodos de Consulta
         ***********************************************************************************************/
        
        /* Descripción:
         *  Devuelve la lista de facetas a la que pertenece los parámetros.
         */
        public ListFacets G_ListFacets()
        {
            return this.gListFacets;
        }


        /* Descripción:
         *  Devuelve la suma total de las varianzas de las fuentes objetivo.
         */
        public double Total_differentiation_var()
        {
            return this.total_differentiation_var;
        }
        
        
        /* Descripción:
         *  Devuelve el coeficente G relativo;
         */
        public double CoefG_Rel()
        {
            return this.coefG_Rel;
        }


        /* Descripción:
         *  Devuelve el Coeficiente G absoluto
         */
        public double CoefG_Abs()
        {
            return this.coefG_Abs; 
        }

        
        /* Descripción:
         *  Devuelve la varianza del error relativa
         */
        public double TotalRelErrorVar()
        {
            return this.totalRelErrorVar; 
        }

        
        /* Descripción:
         *  Devuelve la varianza del error absoluta
         */
        public double TotalAbsErrorVar()
        {
            return this.totalAbsErrorVar; 
        }

        
        /* Descripción:
         *  Devuelve la desviación típica relativa
         */ 
        public double ErrorRelStandDev()
        {
            return this.errorRelStandDev;         
        }


        /* Descripción:
         *  Devuelve la desviación típica absoluta
         */
        public double ErrorAbsStandDev()
        {
            return this.errorAbsStandDev; 
        }

        
        /* Descripción:
         *  Devuelve la desviación típica de las fuentes objetivo (Varianza de diferenciación)
         */
        public double TargetStandDev()
        {
            return this.targetStandDev;
        }


        /* Descripción:
         *  Devuelve una copia del objeto tras actualizar las listas de facetas por la nueva que se 
         *  pasa como parámetro.
         *  
         *  NOTA:
         *  Se aprovecha de que el orden de la lista de los diseños será el mismo para
         *  el antiguo que para el nuevo.
         *  
         * @Precondición:
         *  La nueva lista tiene que tener el mismo número de facetas y para cada una de ellas
         *  debe tener la misma jerarquia de anidamientos.
         */
        public G_ParametersOptimization ReplaceListFacets(ListFacets newListFacets)
        {
            return new G_ParametersOptimization(newListFacets, this.total_differentiation_var,
                this.coefG_Rel, this.coefG_Abs, this.totalRelErrorVar, this.totalAbsErrorVar,
                this.errorRelStandDev, this.errorAbsStandDev, this.targetStandDev);
        }


        #region Escritura de datos de G_Parametros de optimización
        /***********************************************************************************************
         * Métodos de para la escritura de datos
         *  - WritingStreamGParametersOptimization
         ***********************************************************************************************/

        /* Descripción:
         *  Escribe en el StreamWriter 
         */
        public bool WritingStreamGParametersOptimization(StreamWriter writerFile)
        {
            bool res = false; // variable de retorno

            // Escribimos la marcha de comienzo de parámetro de optimización
            writerFile.WriteLine(BEGIN_G_PARAMETERS_OPT);

            // Escribimos la lista de facetas
            res = this.gListFacets.WritingStreamListFacets(writerFile);

            // Escribimos los G_Parámetros de datos
            if (res)
            {
                writerFile.WriteLine(total_differentiation_var); // Suma total de las varianzas de las fuentes objetivo
                writerFile.WriteLine(coefG_Rel); // coeficente G relativo
                writerFile.WriteLine(coefG_Abs); // Coeficiente G absoluto
                writerFile.WriteLine(totalRelErrorVar); // Varianza del error relativa
                writerFile.WriteLine(totalAbsErrorVar); // Varianza del error absoluta
                writerFile.WriteLine(errorRelStandDev); // Desviación tipica relativa
                writerFile.WriteLine(errorAbsStandDev); // Desviación tipica absoluta
                writerFile.WriteLine(targetStandDev); // desviación típica de las fuentes objetivo
            }

            // escribimos el fin de parámetro de optimización
            writerFile.WriteLine(END_G_PARAMETERS_OPT);
            return res;
        }

        #endregion Escritura de datos de G_Parametros de optimización



        #region Lectura de datos de G_Parametros de optimización
        /***********************************************************************************************
         * Métodos de para la lectura de datos
         *  - ReadingStreamGParametersOptimization
         ***********************************************************************************************/

        /* Descripción:
         *  Lee los datos de un G Parámetros de optimización de un stream y lo devuelve como objeto.
         * Parámetros:
         *      StreamReader reader: El stream del que vamos a leer los G Parametros de optimización.
         */
        public static G_ParametersOptimization ReadingStreamGParametersOptimization(StreamReader reader)
        {
            G_ParametersOptimization gp = null;
            try
            {
                string line;
                ListFacets lf;
                // Leemos la lista de facetas
                if ((line = reader.ReadLine()).Equals(MultiFacetData.ListFacets.BEGIN_LISTFACETS))
                {
                    lf = MultiFacetData.ListFacets.ReadingStreamListFacets(reader);
                }
                else
                {
                    throw new G_ParametersOptimizationException();
                }

                // Leemos los G_Parámetros de datos
                // double total_differentiation_var = double.Parse(reader.ReadLine()); // Suma total de las varianzas de las fuentes objetivo
                double total_differentiation_var = (double)ConvertNum.String2Double(reader.ReadLine()); 
                // double coefG_Rel = double.Parse(reader.ReadLine()); // coeficente G relativo
                double coefG_Rel = (double)ConvertNum.String2Double(reader.ReadLine()); // coeficente G 
                // double coefG_Abs = double.Parse(reader.ReadLine()); // Coeficiente G absoluto
                double coefG_Abs = (double)ConvertNum.String2Double(reader.ReadLine()); 
                // double totalRelErrorVar = double.Parse(reader.ReadLine()); // Varianza del error relativa
                double totalRelErrorVar = (double)ConvertNum.String2Double(reader.ReadLine()); // Varianza del error relativa
                // double totalAbsErrorVar = double.Parse(reader.ReadLine()); // Varianza del error absoluta
                double totalAbsErrorVar = (double)ConvertNum.String2Double(reader.ReadLine()); // Varianza del error absoluta
                // double errorRelStandDev = double.Parse(reader.ReadLine()); // Desviación tipica relativa
                double errorRelStandDev = (double)ConvertNum.String2Double(reader.ReadLine()); // Desviación tipica relativa
                // double errorAbsStandDev = double.Parse(reader.ReadLine()); // Desviación tipica absoluta
                double errorAbsStandDev = (double)ConvertNum.String2Double(reader.ReadLine()); // Desviación tipica absoluta
                // double targetStandDev = double.Parse(reader.ReadLine()); // desviación típica de las fuentes objetivo
                double targetStandDev = (double)ConvertNum.String2Double(reader.ReadLine()); // desviación típica de las fuentes objetivo

                if ((line = reader.ReadLine()).Equals(END_G_PARAMETERS_OPT))
                {
                    gp = new G_ParametersOptimization(lf, total_differentiation_var, coefG_Rel, coefG_Abs,
                    totalRelErrorVar, totalAbsErrorVar, errorRelStandDev, errorAbsStandDev, targetStandDev);
                }
                else
                {
                    throw new G_ParametersOptimizationException("Error al leer de fichero");
                }
                
            }
            catch (ListFacetsException)
            {
                throw new G_ParametersOptimizationException("Error al leer de fichero");
            }
            return gp;
        }// end private static TableMeans ReadingStreamTableMeans

        #endregion Lectura de datos de G_Parametros de optimización


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
            // Lista de Facetas a la que pertenecen los G_Parámetros
            ListFacets copyGListFacets = (ListFacets)this.gListFacets.Clone(); 
            // Suma total de las varianzas de las fuentes objetivo
            double copyTotal_differentiation_var = this.total_differentiation_var; 
            double copyCoefG_Rel = this.coefG_Rel; // coeficente G relativo
            double copyCoefG_Abs = this.coefG_Abs; // Coeficiente G absoluto
            double copyTotalRelErrorVar  = this.totalRelErrorVar; // Varianza del error relativa
            double copyTotalAbsErrorVar  = this.totalAbsErrorVar; // Varianza del error absoluta
            double copyErrorRelStandDev = this.errorRelStandDev; // Desviación tipica relativa
            double copyErrorAbsStandDev = this.errorAbsStandDev; // Desviación tipica absoluta
            double copyTargetStandDev = this.targetStandDev; // desviación típica de las fuentes objetivo

            return new G_ParametersOptimization(copyGListFacets, copyTotal_differentiation_var, copyCoefG_Rel, copyCoefG_Abs,
                    copyTotalRelErrorVar, copyTotalAbsErrorVar, copyErrorRelStandDev, copyErrorAbsStandDev, copyTargetStandDev);
        }
        #endregion Implementacion de la interfaz



        #region Métodos redefinidos
        /***********************************************************************************************
         * Métodos de redefinidos
         *  - ToString
         ***********************************************************************************************/
        public override string ToString()
        {
            StringBuilder retVal = new StringBuilder();
            retVal.Append("List Facets: /n");
            retVal.Append(this.gListFacets.ToString()); retVal.Append("/n");
            retVal.Append("Coeff_G rel.: "); retVal.Append(this.coefG_Rel); retVal.Append("/n");
            retVal.Append("Coeff_G abs.: "); retVal.Append(this.coefG_Abs); retVal.Append("/n");
            retVal.Append("Total error rel.: "); retVal.Append(this.totalRelErrorVar); retVal.Append("/n");
            retVal.Append("Total error abs.: "); retVal.Append(this.totalAbsErrorVar); retVal.Append("/n");
            retVal.Append("Relative SE: "); retVal.Append(this.errorRelStandDev); retVal.Append("/n");
            retVal.Append("Absolute SE: "); retVal.Append(this.errorAbsStandDev); retVal.Append("/n");
            return retVal.ToString();
        }

        #endregion Métodos redefinidos

        #region Conversión con DataSet
        /* Descripción:
         *  Convierte un objeto de la clase en un dataSet.
         */
        public DataSet G_Parameters2DataSet()
        {
            // Creamos un dataSet
            DataSet dsG_Parameters = new DataSet("DataSet_G_Parameters");
            // Obtenmos el Data table de la lista de facetas
            DataTable dtListFacets = this.gListFacets.ListFacets2DataTable("TbFacets");
            dsG_Parameters.Tables.Add(dtListFacets);
            DataTable dtSkipLevels = this.gListFacets.SkipLevels2DataTable("TbSkipLevels");
            dsG_Parameters.Tables.Add(dtSkipLevels);

            // Creamos el dataTable para almacenar los datos
            DataTable dtG_Parameters = new DataTable("DataTable_G_Parameters");
            dtG_Parameters.Columns.Add(new DataColumn("total_differentiation_var", System.Type.GetType("System.Double")));
            dtG_Parameters.Columns.Add(new DataColumn("coefG_Rel", System.Type.GetType("System.Double")));
            dtG_Parameters.Columns.Add(new DataColumn("coefG_Abs", System.Type.GetType("System.Double")));
            dtG_Parameters.Columns.Add(new DataColumn("totalRelErrorVar", System.Type.GetType("System.Double")));
            dtG_Parameters.Columns.Add(new DataColumn("totalAbsErrorVar", System.Type.GetType("System.Double")));
            dtG_Parameters.Columns.Add(new DataColumn("errorRelStandDev", System.Type.GetType("System.Double")));
            dtG_Parameters.Columns.Add(new DataColumn("errorAbsStandDev", System.Type.GetType("System.Double")));
            dtG_Parameters.Columns.Add(new DataColumn("targetStandDev", System.Type.GetType("System.Double")));

            DataRow row = dtG_Parameters.NewRow();
            row["total_differentiation_var"] = this.total_differentiation_var;
            row["coefG_Rel"] = this.coefG_Rel;
            row["coefG_Abs"] = this.coefG_Abs;
            row["totalRelErrorVar"] = this.totalRelErrorVar;
            row["totalAbsErrorVar"] = this.totalAbsErrorVar;
            row["errorRelStandDev"] = this.errorRelStandDev;
            row["errorAbsStandDev"] = this.errorAbsStandDev;
            row["targetStandDev"] = this.targetStandDev;
            dtG_Parameters.Rows.Add(row);
            dsG_Parameters.Tables.Add(dtG_Parameters);

            return dsG_Parameters;
        }// end G_Parameters2DataSet


        /* Descripción:
         *  Toma un dataSet y devuelve un objeto de la clase
         */
        public static G_ParametersOptimization DataSet2G_Parameters(DataSet dsG_Parameters)
        {
            DataTable dtListFacets = dsG_Parameters.Tables["TbFacets"];
            DataTable dtSkipLevels = dsG_Parameters.Tables["TbSkipLevels"];
            ListFacets lf = MultiFacetData.ListFacets.DataTables2ListFacets(dtListFacets, dtSkipLevels);
            DataTable dtGParameters = dsG_Parameters.Tables["DataTable_G_Parameters"];
            DataRow row = dtGParameters.Rows[0];

            double total_differentiation_var = (double)row["total_differentiation_var"];
            double coefG_Rel = (double)row["coefG_Rel"];
            double coefG_Abs = (double)row["coefG_Abs"];
            double totalRelErrorVar = (double)row["totalRelErrorVar"];
            double totalAbsErrorVar = (double)row["totalAbsErrorVar"];
            double errorRelStandDev = (double)row["errorRelStandDev"];
            double errorAbsStandDev = (double)row["errorAbsStandDev"];
            double targetStandDev = (double)row["targetStandDev"];

            return new G_ParametersOptimization(lf, total_differentiation_var, coefG_Rel, coefG_Abs,
                totalRelErrorVar, totalAbsErrorVar, errorRelStandDev, errorAbsStandDev, targetStandDev);
        }// DataSet2G_Parameters
        #endregion Convierte un objeto en un DataSet


    }// end class G_ParametersOptimization
}// end namespace ProjectSSQ
