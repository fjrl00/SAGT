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
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiFacetData;

namespace ProjectSSQ
{
    public class TableG_Study : System.ICloneable
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


        public TableG_Study(ListFacets diff, ListFacets inst): this()
        {
            this.lfDifferentiation = diff;
            this.lfInstrumentation = inst;
        }


        public TableG_Study(ListFacets differentiation, ListFacets instrumentation, TableAnalysisOfVariance lTSSQ)
            :this(differentiation,instrumentation)
        {
            List<string> llf_diff_cwr = differentiation.CombinationStringWithoutRepetition();
            List<string> llf_inst_cwr = instrumentation.CombinationStringWithoutRepetition();

            int n = llf_diff_cwr.Count;
            for (int i = 0; i < n; i++)
            {
                string key = llf_diff_cwr[i];
                // Si varianaza corregida es negativa entonces la varianza de diferenciación debe
                // valer cero.
                double? tVar = lTSSQ.CorretedComp(key);
                if ((tVar != null) && (double)tVar<0)
                {
                    tVar = 0;
                }
                differentiationVar.Add(key, tVar);
            }

            ListFacets totalFacets = this.lfDifferentiation.Concatenate(this.lfInstrumentation);
            // ListFacets totalFacets = lTSSQ.ListFacets();
            // List<string> ldesing = totalFacets.CombinationStringWithoutRepetition();
            List<string> ldesing = lTSSQ.ListFacets().CombinationStringWithoutRepetition(); 


            n = ldesing.Count;
            for (int i = 0; i < n; i++)
            {
                string key = ldesing[i];
                double? relError = null;
                double? absError = null;

                /* Introdiremos aquellas entradas que no sean combinación de las fuentes de 
                 * Diferenciación. */
                if (!differentiationVar.ContainsKey(key))
                {
                    /* Compruebo si son combinacion de las fuentes de instrumentatión
                     * ya que esta si el valor del error negativo pasa a ser null*/
                    if (llf_inst_cwr.Contains(key))
                    {
                        if (lTSSQ.MixModComp(key) < 0)
                        {
                            // si es negativo vale 0
                            relError = 0.0;
                        }
                        else
                        {
                            // en otro caso vale
                            // relError = lTSSQ.RandomComp(lf) / lf.MultipSourcesOfVariabilityAbsent(depend);
                            ListFacets lf = totalFacets.ListDesignFacets(key);
                            absError = lTSSQ.MixModComp(key) * lf.MultipSourcesOfVariabilityAbsentForTypeOfFacet(differentiation);
                        }
                    }
                    else
                    {
                        // en este caso no esta contenida
                        if (lTSSQ.MixModComp(key) < 0)
                        {
                            // si es negativo vale 0
                            absError = 0.0;
                            relError = 0.0;

                        }
                        else
                        {
                            // en otro caso vale
                            ListFacets lf = totalFacets.ListDesignFacets(key);
                            relError = lTSSQ.MixModComp(key) * lf.MultipSourcesOfVariabilityAbsentForTypeOfFacet(differentiation);
                            absError = relError;
                        }
                    }/* end if 1*/
                    ErrorVar error_variance = new ErrorVar(relError,absError);
                    this.errorVar.Add(key, error_variance);
                }
            } // end if

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
                :this(differentiation,instrumentation)
        {

            ListFacets totalFacets = differentiation.Concatenate(instrumentation);

            this.g_parameterOptimization = new G_ParametersOptimization(totalFacets, 0, coefG_Rel, coefG_Abs,
            totalRelErrorVar, totalAbsErrorVar, errorRelStandDev, errorAbsStandDev, 0);
        }


        public TableG_Study(ListFacets differentiation, ListFacets instrumentation,
            Dictionary<string, double?> targetVar,
            Dictionary<string, ErrorVar> errorVar,
            double coefG_Rel, double coefG_Abs,
            double totalRelErrorVar, double totalAbsErrorVar,
            double errorRelStandDev, double errorAbsStandDev,
            double total_differentiation_var, double targetStandDev)
            : this(differentiation, instrumentation)
        {
            ListFacets totalFacets = differentiation.Concatenate(instrumentation);
            this.g_parameterOptimization = new G_ParametersOptimization(totalFacets, total_differentiation_var, coefG_Rel, coefG_Abs,
            totalRelErrorVar, totalAbsErrorVar, errorRelStandDev, errorAbsStandDev, targetStandDev);
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
         *  Calcula el valor total de todos los target contenidos en el objeto
         */
        private double CalcTotalTarget()
        {
            double retVal = 0;
            int n = this.differentiationVar.Keys.Count;
            List<string> ldesign = new List<string>(this.differentiationVar.Keys);
            for (int i = 0; i < n;i++ )
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
         *  Calcula la suma de todas la varianzas de error relativas
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

        
        /*
         * Despcrición:
         *  Calcula el coeficiente G relativo
         */
        private double CalcCoefG_Rel()
        {
            double retVal = 0;
            double totalTarget = this.TotalDifferentiationVariance();
            double totalRelErr = this.TotalRelErrorVar();
            try
            {
                retVal = totalTarget / (totalTarget + totalRelErr);
            }
            catch (DivideByZeroException)
            {
                throw new TableG_StudyException("Error: división por cero");
            }
            
            return retVal;
        }

        
        /*
        * Despcrición:
        *  Calcula el coeficiente G absoluto
        */
        private double CalcCoefG_Abs()
        {
            double retVal = 0;
            double totalTarget = this.TotalDifferentiationVariance();
            double totalRelAbs = this.TotalAbsErrorVar();
            try
            {
                retVal = totalTarget / (totalTarget + totalRelAbs);
            }
            catch (DivideByZeroException)
            {
                throw new TableG_StudyException("Error: división por cero");
            }
            
            return retVal;
        }

        
        /* Descripción:
         *  Devuelve la desviación estandar de la suma de las varianzas objetivo
         */
        private double CalcTargetStandDev()
        {
            double retVal = this.TotalDifferentiationVariance();
            retVal = Math.Sqrt(retVal);
            return retVal;
        }


        /* Descripción:
         *  Devuelve la desviación estandar de la suma de las varianzas del error relativo
         */
        private double CalcErrorRelStandDev()
        {
            double retVal = this.TotalRelErrorVar();
            retVal = Math.Sqrt(retVal);
            return retVal;
        }


        /* Descripción:
         *  Devuelve la desviación estandar de la suma de las varianzas del error absoluto
         */
        private double CalcErrorAbsStandDev()
        {
            double retVal = this.TotalAbsErrorVar();
            retVal = Math.Sqrt(retVal);
            return retVal;
        }


        /* Descripción:
         *  Plan de optimización. Se efectuan las correcciónes necesarias. Devuelve un G_Parmeters
         */
        public void CorrectionOfBrennan(ListFacets differentiation, ListFacets instrumentation, TableAnalysisOfVariance lTSSQ)
        {

            List<string> ldesign = lTSSQ.SourcesOfVar();

            Dictionary<string, double?> targetVar = this.differentiationVar;
            Dictionary<string, ErrorVar> errorVar = this.errorVar;

            int n = ldesign.Count;

            for (int i = 0; i < n; i++)
            {
                string key = ldesign[i];
                ListFacets lf = lTSSQ.ListFacets();
                ListFacets lf_kety = lf.ListDesignFacets(key);
                if (targetVar.ContainsKey(key))
                {
                    double? d = targetVar[key];
                    if (d != null)
                    {
                        d = d * lf.estimateBrennan();
                        targetVar.Remove(key);
                        targetVar.Add(key, d);
                    }
                }

                if (errorVar.ContainsKey(key))
                {
                    ErrorVar e = errorVar[key];
                    double? d1 = e.RelErrorVar();
                    double? d2 = e.AbsErrorVar();
                    
                    if (d1 != null)
                    {
                        d1 = d1 * lf_kety.Difference(differentiation).estimateBrennan();
                        e.RelErrorVar(d1);
                    }

                    if (d2 != null)
                    {
                        d2 = d2 * lf_kety.Difference(differentiation).estimateBrennan();
                        e.AbsErrorVar(d2);
                    }
                    errorVar.Remove(key);
                    errorVar.Add(key, e);
                }
                
            }// end for

            ListFacets totalFacets = differentiation.Concatenate(instrumentation);

            double total_differentiation_var = CalcTotalTarget();
            double totalRelErrorVar = CalcTotalRelErrorVar();
            double totalAbsErrorVar = CalcTotalAbsErrorVar();
            double targetStandDev = CalcTargetStandDev();
            double errorRelStandDev = CalcErrorRelStandDev();
            double errorAbsStandDev = CalcErrorAbsStandDev();
            double coefG_Rel = CalcCoefG_Rel();
            double coefG_Abs = CalcCoefG_Abs();

            this.g_parameterOptimization = new G_ParametersOptimization(totalFacets, total_differentiation_var, coefG_Rel, coefG_Abs,
            totalRelErrorVar, totalAbsErrorVar, errorRelStandDev, errorAbsStandDev, targetStandDev);


        }// end estimación de resultados
        

        /*==============================================================================================
         * PROVISIONAL
         *==============================================================================================*/

        /* Descripción:
         *  
         */
        public void Corrección2(TableG_Study gAntiguo)
        {
            //Dictionary<ListFacets, double?> targetAntiguo = gAntiguo.targetVar;
            this.differentiationVar = gAntiguo.differentiationVar;
        }


        /* Constructor especial
         * 
         */
        public TableG_Study G_P_special(ListFacets differentiation, ListFacets instrumentation, TableAnalysisOfVariance lTSSQ)
        {
            TableG_Study retVal = new TableG_Study(differentiation, instrumentation);
            List<string> llf_diff_cwr = differentiation.CombinationStringWithoutRepetition();
            List<string> llf_inst_cwr = instrumentation.CombinationStringWithoutRepetition();

            int n = llf_diff_cwr.Count;
            for (int i = 0; i < n; i++)
            {
                string key = llf_diff_cwr[i];
                // Si varianaza corregida es negativa entonces la varianza de diferenciación debe
                // valer cero.
                double? tVar = lTSSQ.CorretedComp(key);
                if ((tVar != null) && (double)tVar < 0)
                {
                    tVar = 0;
                }
                retVal.differentiationVar.Add(key, tVar);
            }

            ListFacets totalFacets = new ListFacets();
            n = differentiation.Count();
            for (int i = 0; i < n; i++)
            {
                totalFacets.Add(differentiation.FacetInPos(i));
            }

            n = instrumentation.Count();
            for (int i = 0; i < n; i++)
            {
                totalFacets.Add(instrumentation.FacetInPos(i));
            }

            List<string> ldesign = totalFacets.CombinationStringWithoutRepetition();

            n = ldesign.Count;
            for (int i = 0; i < n; i++)
            {
                string key = ldesign[i];
                ListFacets lf = lTSSQ.ListFacets();
                ListFacets lf_key = lf.ListDesignFacets(key);
                double? relError = null;
                double? absError = null;

                /* Introdiremos aquellas entradas que no sean conbinación de las fuentes de 
                 * Diferenciación. */
                if (!retVal.differentiationVar.ContainsKey(key))
                {
                    /* Compruebo si son combinacion de las fuentes de instrumentatión
                     * ya que esta si el valor del error negativo pasa a ser null*/
                    if (llf_inst_cwr.Contains(key))
                    {
                        if (lTSSQ.MixModComp(key) < 0)
                        {
                            // si es negativo vale 0
                            relError = 0.0;
                        }
                        else
                        {
                            // en otro caso vale
                            // relError = lTSSQ.RandomComp(lf) / lf.MultipSourcesOfVariabilityAbsent(depend);
                            absError = lTSSQ.MixModComp(key) * lf_key.MultipSourcesOfVariabilityAbsentForTypeOfFacet(differentiation);
                        }
                    }
                    else
                    {
                        // en este caso no esta contenida
                        if (lTSSQ.MixModComp(key) < 0)
                        {
                            // si es negativo vale 0
                            absError = 0.0;
                            relError = 0.0;

                        }
                        else
                        {
                            // en otro caso vale
                            relError = lTSSQ.MixModComp(key) * lf_key.MultipSourcesOfVariabilityAbsentForTypeOfFacet(differentiation);
                            absError = relError;
                        }
                    }/* end if 1*/
                    ErrorVar error_variance = new ErrorVar(relError, absError);
                    retVal.errorVar.Add(key, error_variance);
                }
            } // end if

            retVal.CalcTotalTarget();
            retVal.CalcTotalRelErrorVar();
            retVal.CalcTotalAbsErrorVar();
            retVal.CalcTargetStandDev();
            retVal.CalcErrorRelStandDev();
            retVal.CalcErrorAbsStandDev();
            retVal.CalcCoefG_Rel();
            retVal.CalcCoefG_Abs();

            return retVal;
        }// end constructor G_P_special


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
            ListFacets copyLfDifferentiation = (ListFacets)this.lfDifferentiation.Clone();
            // Copiamos la fuentes de instrumentación
            ListFacets copyLfInstrumentation = (ListFacets)this.lfInstrumentation.Clone();
            Dictionary<string, double?> copyDifferentiationVar = ClonarDictionary(this.differentiationVar);
            Dictionary<string, ErrorVar> copyErrorVar = ClonarDictionary(this.errorVar);

            G_ParametersOptimization copyG_parameterOptimization = (G_ParametersOptimization)this.g_parameterOptimization.Clone();
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
                ErrorVar e = (ErrorVar)original[skey].Clone();

                copy.Add(copyKey, e);
            }

            return copy;
        }

        #endregion Implementacion de la interfaz

    } // end public class TableG_Study
} // end namespace ProjectSSQ