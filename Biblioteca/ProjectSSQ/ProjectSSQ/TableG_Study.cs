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
 * Fecha de revisión: 19/Jun/2012
 * 
 * Descripción:
 *      Clase TableG_Study 
 *      La tabla que contiene los valores de la varianza de diferenciación para dichas fuentes y 
 *      las varianzas de instrumentación de error relativos y absolutos para las fuentes de instrumentación
 */

using MultiFacetData;
using System.Collections.Generic;

namespace ProjectSSQ
{
    public class TableG_Study
    {

        /*======================================================================================
         * Variables de instancia
         *=====================================================================================*/
        // Variables de instancias
        private ListFacets lfDifferentiation; // Fuentes de diferenciación, 
        private ListFacets lfInstrumentation; // Fuentes de instrumentación
        private Dictionary<string, double?> differentiationVar;
        private Dictionary<string, ErrorVar> errorVar;

        private G_ParametersOptimization g_parameterOptimization;


        #region Constructores de la clase TableG_Study
        /*======================================================================================
         * Constructores (5 constructores)
         *=====================================================================================*/

        public TableG_Study()
        {
            this.lfDifferentiation = new ListFacets();
            this.lfInstrumentation = new ListFacets();

            this.differentiationVar = new Dictionary<string, double?>();
            this.errorVar = new Dictionary<string, ErrorVar>();
            this.g_parameterOptimization = null;
        }


        public TableG_Study(ListFacets diff, ListFacets inst) : this()
        {
            this.lfDifferentiation = diff;
            this.lfInstrumentation = inst;
        }


        public TableG_Study(ListFacets differentiation, ListFacets instrumentation, TableAnalysisOfVariance lTSSQ)
            : this(differentiation, instrumentation)
        {
            List<string> llf_diff_cwr = differentiation.CombinationStringWithoutRepetition();

            // We officially take the differentiation variance to be the corrected variance component, and consider negative variance components to be 0.
            int n = llf_diff_cwr.Count;
            for (int i = 0; i < n; i++)
            {
                string key = llf_diff_cwr[i];
                double? tVar = lTSSQ.CorrectedComp(key);
                if ((tVar != null) && (double)tVar < 0)
                {
                    tVar = 0;
                }
                differentiationVar.Add(key, tVar);
            }

            ListFacets totalFacets = this.lfDifferentiation.Concatenate(this.lfInstrumentation);
            List<string> ldesign = lTSSQ.ListFacets().CombinationStringWithoutRepetition(); // We would kinda prefer to also use totalFacets here to present the designs in that order? But we would need to change code around in order to achieve that


            // Now we calculate error variances
            n = ldesign.Count;
            for (int i = 0; i < n; i++)
            {
                string key = ldesign[i];
                double? relError = null;
                double? absError = null;

                if (!differentiationVar.ContainsKey(key))   // Only applies when not all facets in this design are differentiation facets
                {
                    ListFacets lf = totalFacets.ListDesignFacets(key);

                    if (lTSSQ.MixedComp(key) < 0)      //if less than 0, we directly interpret it as 0 and skip the computation
                        absError = 0.0;
                    else
                        absError = CalcDStudyVarComp(ldesign, key, lTSSQ, totalFacets, differentiation, lf);

                    if (lf.ContainsAnyOf(differentiation)) // Only calculate relative error if the design contains any differentiation facets
                        relError = absError;

                    ErrorVar error_variance = new ErrorVar(relError, absError);
                    this.errorVar.Add(key, error_variance);
                }
            }

            double total_differentiation_var = CalcTotalTarget();
            double totalRelErrorVar = CalcTotalRelErrorVar();
            double totalAbsErrorVar = CalcTotalAbsErrorVar();

            this.g_parameterOptimization = new G_ParametersOptimization(totalFacets, total_differentiation_var,
            totalRelErrorVar, totalAbsErrorVar);

        }// end constructor G_Parameters


        public TableG_Study(ListFacets differentiation, ListFacets instrumentation,
            Dictionary<string, double?> diffVar, Dictionary<string, ErrorVar> errorVar,
            G_ParametersOptimization gp)
        {
            this.lfDifferentiation = differentiation;
            this.lfInstrumentation = instrumentation;
            this.differentiationVar = diffVar;
            this.errorVar = errorVar;
            this.g_parameterOptimization = gp;
        }


        public TableG_Study(ListFacets differentiation, ListFacets instrumentation,
            double coefG_Rel, double coefG_Abs,
            double totalRelErrorVar, double totalAbsErrorVar,
            double errorRelStandDev, double errorAbsStandDev)
                : this(differentiation, instrumentation)
        {

            ListFacets totalFacets = differentiation.Concatenate(instrumentation);

            this.g_parameterOptimization = new G_ParametersOptimization(totalFacets, 0, coefG_Rel, coefG_Abs,
            totalRelErrorVar, totalAbsErrorVar, errorRelStandDev, errorAbsStandDev, 0);
        }

        #endregion Constructores de la clase TableG_Study



        #region Métodos de consulta de la clase TableG_Study
        /*======================================================================================
         * Métodos de consulta
         * ===================
         *  - LfDifferentiation
         *  - LfInstrumentation
         *  - Error (2 métodos)
         *  - Target (2 métodos)
         *  - TargetContainsKey
         *  - ErrorContainsKey
         *  - TotalDifferentiationVariance
         *  - TotalRelErrorVar
         *  - TotalAbsErrorVar
         *  - CoefG_Rel
         *  - CoefG_Abs
         *=====================================================================================*/


        /* Descripción:
         *  Devuelve la lista de facetas de diferenciación;
         */
        public ListFacets LfDifferentiation()
        {
            return this.lfDifferentiation; // Fuentes de diferenciación, 
        }


        /* Descripción:
         *  Devuelve la lista de facetas de instrumentación;
         */
        public ListFacets LfInstrumentation()
        {
            return this.lfInstrumentation; // Fuentes de instrumentación
        }


        /* Descripción:
         *  Devuelve un objeto de tipo G_ParametersOptimization con los G_Parametros de optimización.
         */
        public G_ParametersOptimization G_ParametersOptimization()
        {
            return this.g_parameterOptimization;
        }


        /*
         * Descripción:
         *  Devulve un objeto de tipo Error con las varianzas del error absoluto y relativo para
         *  una clave dada (lista de facetas) que se pasa como parámetro.
         */
        public ErrorVar Error(string design)
        {
            return this.errorVar[design];
        }


        /* Descripción
         *  Devuelve la estructura (Dictionary) con las varianzas de error
         */
        public Dictionary<string, ErrorVar> Error()
        {
            return this.errorVar;
        }


        /* Descripción:
         *  Devuelve el valor de target para una clave dada (lista de facetas) que se pasa como parámetro.
         */
        public double? Target(string design)
        {
            return this.differentiationVar[design];
        }


        /* Descripción:
         *  Devuelve el valor de target para una clave dada (lista de facetas) que se pasa como parámetro.
         */
        public Dictionary<string, double?> Target()
        {
            return this.differentiationVar;
        }


        /* Descripción:
         *  Devuelve true si la clave se encuentra contenida en la estructura de datos.
         */
        public bool TargetContainsKey(string design)
        {
            return this.differentiationVar.ContainsKey(design);
        }


        /* Descripción:
         *  Devuelve true si la clave se encuentra contenida en la estructura de datos.
         */
        public bool ErrorContainsKey(string design)
        {
            return this.errorVar.ContainsKey(design);
        }


        /*
         * Descripción:
         *  Devuelve la suma total de todas las varianzas de las fuentes objetivo.
         */
        public double TotalDifferentiationVariance()
        {
            return this.g_parameterOptimization.Total_differentiation_var();
        }


        /* Descripción:
         *  Devuelve la suma de todas la varianzas de error relativas
         */
        public double TotalRelErrorVar()
        {
            return this.g_parameterOptimization.TotalRelErrorVar();
        }


        /* Descripción:
         *  Devuelve la suma de todas la varianzas de error relativas
         */
        public double TotalAbsErrorVar()
        {
            return this.g_parameterOptimization.TotalAbsErrorVar();
        }


        /*
         * Descripción:
         *  Devuelve la desviación estandar de la suma de las varianzas objetivo
         */
        public double TargetStandDev()
        {
            return this.g_parameterOptimization.TargetStandDev();
        }


        /* Descripción:
         *  Devuelve la desviación estandar de la suma de las varianzas del error relativo
         */
        public double ErrorRelStandDev()
        {
            return this.g_parameterOptimization.ErrorRelStandDev();
        }


        /* Descripción
         *  Devuelve la desviación estandar de la suma de las varianzas del error absoluto
         */
        public double ErrorAbsStandDev()
        {
            return this.g_parameterOptimization.ErrorAbsStandDev();
        }


        /*
         * Despcrición:
         *  Devuelve el coeficiente G relativo
         */
        public double CoefG_Rel()
        {
            return this.g_parameterOptimization.CoefG_Rel();
        }


        /*
        * Despcrición:
        *  Calcula el coeficiente G absoluto
        */
        public double CoefG_Abs()
        {
            return this.g_parameterOptimization.CoefG_Abs();
        }

        #endregion Métodos de consulta de la clase TableG_Study

        /* Descripción:
         *  Calcula el componente de varianza del D estudio 
         *  Fuente: Brennan (2001) 5.1.1 (5.6)
         *  
         * lTSSQ contiene los antiguos datos
         * totalFacets contiene los nuevos
         *  
         * lTSSQ.MixedComp(key) * primaryLf.FPCFactor(differentiation) / lf.SubstractFacets(differentiation).MultOfLevels();  // Simplified version only valid for universe size staying unchanged
         */
        private double? CalcDStudyVarComp(List<string> ldesign, string key, TableAnalysisOfVariance lTSSQ, ListFacets totalFacets, ListFacets differentiation, ListFacets lf)
        {
            ListFacets primaryLf = totalFacets.ListDesignPrimaryFacets(key);

            double? retVal = 0;

            int n = ldesign.Count;
            for (int i = 0; i < n; i++)
            {
                string key_aux = ldesign[i];
                ListFacets lf_aux = totalFacets.ListDesignFacets(key_aux);
                ListFacets primaryLf_aux = totalFacets.ListDesignPrimaryFacets(key_aux);

                if (lf_aux.ContainsList(lf))
                {
                    double K = primaryLf_aux.FPCFactor_N(lf, lTSSQ.ListFacets());
                    double pi = lf_aux.SubstractFacets(lf).MultSizeOfUniverse();

                    if(pi != 0) // i.e. pi didn't come up infinite, in which case we'dd add 0
                    {
                        retVal = retVal + lTSSQ.MixedComp(key_aux) * K / pi;
                    }
                    
                }
            }

            retVal = retVal * primaryLf.FPCFactor(differentiation) / lf.SubstractFacets(differentiation).MultOfLevels();

            return retVal;
        }

        /* Descripción:
         *  Calcula el valor total de todos los target contenidos en el objeto
         */
        private double CalcTotalTarget()
        {
            double retVal = 0;
            int n = this.differentiationVar.Keys.Count;
            List<string> ldesign = new List<string>(this.differentiationVar.Keys);
            for (int i = 0; i < n; i++)
            {
                string key = ldesign[i];
                double? v = this.differentiationVar[key];
                if (v != null)
                {
                    if (retVal == 0)
                    {
                        retVal = (double)v;
                    }
                    else
                    {
                        retVal += (double)v;
                    }
                }
            }
            return retVal;
        }


        /* Descripción:
         *  Calcula la suma de todas la varianzas de error relativas
         */
        private double CalcTotalRelErrorVar()
        {
            double retVal = 0;
            int n = this.errorVar.Keys.Count;
            List<string> ldesign = new List<string>(this.errorVar.Keys);
            for (int i = 0; i < n; i++)
            {
                string key = ldesign[i];
                double? e = this.errorVar[key].RelErrorVar();
                if (e != null)
                {
                    if (retVal == 0)
                    {
                        retVal = (double)e;
                    }
                    else
                    {
                        retVal += (double)e;
                    }
                }
            }
            return retVal;
        }


        /* Descripción:
         *  Calcula la suma de todas la varianzas de error absolutas
         */
        private double CalcTotalAbsErrorVar()
        {
            double retVal = 0;
            int n = this.errorVar.Keys.Count;
            List<string> ldesign = new List<string>(this.errorVar.Keys);
            for (int i = 0; i < n; i++)
            {
                string key = ldesign[i];
                double? e = this.errorVar[key].AbsErrorVar();
                if (e != null)
                {
                    if (retVal == 0)
                    {
                        retVal = (double)e;
                    }
                    else
                    {
                        retVal += (double)e;
                    }
                }
            }
            return retVal;
        }


        #region Clonación

        /* Descripción:
         *  Devuelve una copy en profundidad del objeto.
         */
        public virtual TableG_Study Clone()
        {
            // Copiamos la fuentes de diferenciación, 
            ListFacets copyLfDifferentiation = this.lfDifferentiation.DeepClone();
            // Copiamos la fuentes de instrumentación
            ListFacets copyLfInstrumentation = this.lfInstrumentation.DeepClone();
            Dictionary<string, double?> copyDifferentiationVar = ClonarDictionary(this.differentiationVar);
            Dictionary<string, ErrorVar> copyErrorVar = ClonarDictionary(this.errorVar);

            G_ParametersOptimization copyG_parameterOptimization = this.g_parameterOptimization.Clone();
            return new TableG_Study(copyLfDifferentiation, copyLfInstrumentation, copyDifferentiationVar,
                copyErrorVar, copyG_parameterOptimization);
        }


        /* Descripción:
         *  Método auxiliar de Clone. Copia los elementos de un dicionario donde la clave es un string 
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
         *  Método auxiliar de Clone. Copia los elementos de un dicionario donde la clave es un string 
         *  y el objeto es un ErrorVar.
         */
        private static Dictionary<string, ErrorVar> ClonarDictionary(Dictionary<string, ErrorVar> original)
        {
            Dictionary<string, ErrorVar> copy = new Dictionary<string, ErrorVar>(); // Copia a retornar

            foreach (string skey in original.Keys)
            {
                string copyKey = string.Copy(skey);
                ErrorVar e = original[skey].Clone();

                copy.Add(copyKey, e);
            }

            return copy;
        }

        #endregion Clonación

    } // end public class TableG_Study
} // end namespace ProjectSSQ