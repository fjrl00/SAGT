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
 * Fecha de revisión:  18/Jun/2012
 * 
 * Descripción:
 *      Clase de Tabla de Análisis de Varianza.
 */

using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using AuxMathCalcGT;
using MultiFacetData;
using ProjectMeans;

namespace ProjectSSQ
{
    public class TableAnalysisOfVariance : System.ICloneable //: IEnumerable
    {

        /******************************************************************************************************
         *  Constantes de clase TableAnalysisOfVariance
         ******************************************************************************************************/

        // Comienzo y fin de una tabla de análisis de varianza
        public const string BEGIN_TABLE_ANALYSIS_OF_VARIANCE = "<table_analysis_of_variance>";
        const string END_TABLE_ANALYSIS_OF_VARIANCE = "</table_analysis_of_variance>";
        // Comienzo y fin de una lista de lista de facetas
        const string BEGIN_LIST_OF_LIST_FACETS = "<list_of_list_facets>";
        const string END_LIST_OF_LIST_FACETS = "</list_of_list_facets>";


        /******************************************************************************************************
         *  Variables de clase TableAnalysisOfVariance
         ******************************************************************************************************/

        private ListFacets listFacets; // lista de facetas (Fuentes de variación)
        // fuente de variación (una o más facetas) la que queremos calular
        // fuente de variación (una o más facetas) a partir de las cuales queremos calular
        // private List<ListFacets> llf; // Combinación de listas de facetas
        private List<string> ldesign; // Lista de diseños contendrá las claves

        private Dictionary<string, double> df; // grado de libertad 
        private Dictionary<string, double?> ssq; // suma de cuadrados
        private Dictionary<string, double?> msq; // Suma de cuadrados medios (M.S.C.)

        // Varianzas
        // =========
        // Componente de Varianza aleatorio
        private Dictionary<string, double?> randomComp; // Componente de Varianza Aleatorio
        // Componente de Varianza Mixto
        private Dictionary<string, double?> mixComp; // Componentes de Varianza Mixtos
        // Componente de Varianza Corregido
        private Dictionary<string, double?> correcComp;

        // Porcentaje
        private Dictionary<string, double?> porcentage;
        // Error estandar
        private Dictionary<string, double?> standardError;

        #region Constructores de la clase TableAnalysisOfVariance

        /*======================================================================================
         * Constructores
         *======================================================================================*/

        /*
         * Descripción:
         *  Constructor por defecto de una lista de Tablas de suma de cuadrados;
         */
        public TableAnalysisOfVariance()
        {
            /* inicalizamos el vector suma de cuadrados de las desviaciones (ssq)*/
            this.ssq = new Dictionary<string, double?>();
            /* inicializamos el grado de libertad */
            this.df = new Dictionary<string, double>();
            /* inicalizamos el vector cuadrados medios de las desviaciones (msq)*/
            this.msq = new Dictionary<string, double?>();
            /* inicializamos el vector de componentes de varianza aleatorios */
            this.randomComp = new Dictionary<string, double?>();
            this.mixComp = new Dictionary<string, double?>();
            this.correcComp = new Dictionary<string, double?>();
            this.porcentage = new Dictionary<string, double?>();
            this.standardError = new Dictionary<string, double?>();
        }


        /*
         * Descripción:
         *  Constructor de la clase ListsTableSSQ. Se le pasa por parámetro una lista de Facetas.
         *  (No puede haber dos facetas con el mismo nombre).
         * Parámetros:
         *      List<Facet> listF: Lista de Facetas.
         *      MultiFacetObs mfo: contiene el objeto multifaceta con los datos con los que se 
         *              construirá la tabla.
         *      bool zero: Si es true se interpretarán los valores nulos como ceros al calcular
         *          la suma de cuadrados
         * Excepciones:
         *      Lanza una excepción ListFacetExceptions sí la lista que se le pasa como 
         *      parámetro tiene dos facetas con el mismo nombre.
         */
        public TableAnalysisOfVariance(List<string> ldesign, MultiFacetsObs mfo, bool zero)
            : this()
        {
            /* inicializamos el grado de libertad */
            this.df = new Dictionary<string, double>();

            /* Inicializamos la estructura de suma de cuadrados*/
            Dictionary<string, double?> sum_x = new Dictionary<string, double?>();

            /* Inicializamos la estructura de suma de cuadrados al cuadrado*/
            Dictionary<string, double?> sum_x2 = new Dictionary<string, double?>();

            /* inicializamos el vector de varianza */
            Dictionary<string, double?> variance = new Dictionary<string, double?>();

            //this.sourceLeft = sLeft;
            //this.sourceRight = sRight;

            // this.llf = llf; // contiene las variaciones de la lista
            if (ldesign == null || ldesign.Count < 2)
            {
                throw new TableAnalysisOfVarianceException("Error en la lista de diseños");
            }
            this.ldesign = ldesign;

            // inicializamos la lista de facetas
            this.listFacets = mfo.ListFacets();

            // Calculo la suma de cuadrados
            CalcSSq(mfo, zero);
            
            // Calculos parciales de la suma de cuadrados
            /* inicializamos la suma de cuadrados parciales*/
            // Dictionary<string, double?> partialSSQ = PartialSumOfSquares(sum_x2);

            /*Ahora ya podemos calcular la suma de cuadrados de las desviaciones*/
            // CalcSSQ(partialSSQ, residue);


            // ahora calcula los cuadrados medios
            CalcMSQ();
            // Calculamos los componentes de varianza aleatorios
            CalcRandomComp();
            // Calculamos los componentes de varianza mixtos
            CalcMixComp();
            // Calculamos los componentes de varianza corregida
            CalcCorrecComp();
            // Caculamos el porcentaje 
            CalcPorcentage();
            // Calculamos el error standar
            CalcStandardError();
        }// end TableAnalysisOfVariance


        /* Descripción:
         *  Calcula la suma de cuadrados y la guarda según la fuente de variavilidad en la variable ssq
         */
        private void CalcSSq(MultiFacetsObs mfo, bool zero)
        {
            // Estructura de datos que almacenará cada uno de los terminos de la suma de cuadrados,
            // también llamado suma de cuadrados parciales
            Dictionary<ListFacets, double?> terms = new Dictionary<ListFacets, double?>();

            // inicializamos la variable que contiene el residuo
            // string s_residue = DesignResidue(this.ldesign);
            double? residue = CalcResidue(mfo.ObservationTable());

            // int n = this.llf.Count;
            int n = this.ldesign.Count;
            for (int i = 0; i < n; i++)
            {// (* 1 *)
                string key_design = this.ldesign[i];

                df.Add(key_design, this.listFacets.DegreeOfFreedom(key_design));

                ListOfTerms lot = new ListOfTerms(this.listFacets, key_design); // lot (list of terms)

                double sumsOfSquares = 0;

                int numOfTerm = lot.Count();
                for (int j = 0; j < numOfTerm; j++)
                {// (* 2 *)
                    Term t = lot.TermInPos(j);
                    ListFacets facetsOfTerm = t.ListFacets();
                    char sign = t.Sign();

                    double? partial = null; // almacena el valor de la suma parcial.

                    // Si "facetsOfTerm"  es la lista vacia entonces debe calcularse el residuo
                    if (facetsOfTerm.IsEmpty())
                    {
                        // asignamos a partial el valor del residuo
                        partial = residue;
                    }
                    else
                    {
                        if (terms.ContainsKey(facetsOfTerm))
                        {
                            // Si la clave esta contenida recupero el valor 
                            partial = terms[facetsOfTerm];
                        }
                        else
                        {
                            /* Si la clave no está contenida, calcularé el valor de la suma parcial
                             * y lo introduciré en la estructura.
                             */
                            // partial = PartialSumOfSquares(facetsOfTerm, mfo);
                            partial = PartialSumOfSquaresByMeans(facetsOfTerm, mfo, zero);
                            terms[facetsOfTerm] = partial;
                        }
                    }

                    switch (sign)
                    {
                        case (Term.MINUS):
                            if (partial != null)
                            {
                                sumsOfSquares = (sumsOfSquares - (double)partial);
                            }
                            break;
                        case (Term.PLUS):
                            if (partial != null)
                            {
                                sumsOfSquares = (sumsOfSquares + (double)partial);
                            }
                            break;
                        default:
                            throw new TableAnalysisOfVarianceException("Error al calcular el termino");
                    }

                }// end for (* 2 *)

                // Almacenamos el valor de la suma de cuadrados
                this.ssq[key_design] = sumsOfSquares;
            }// end for (* 1 *)
        }// end CalcSSq


        /* Descripción:
         *  Devuelve de la lista de diseños aquel que se corresponde con el diseño del residuo.
         *  Devolverá el diseño que tenga mayor número de facetas. Si hay varios con el mismo 
         *  número devolverá el último que encuentre.
         */
        private static string DesignResidue(List<string> ldesign)
        {
            if (ldesign == null || ldesign.Count < 2)
            {
                throw new TableAnalysisOfVarianceException("Error en la lista de diseños");
            }
            string res = ldesign[0];
            char[] delimeterChars = { '[' }; // nuestro delimitador será el caracter blanco
            string[] arrayOfstring = res.Trim().Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
            int num = arrayOfstring.Length;
            int n = ldesign.Count;
            for (int i = 1; i < n; i++)
            {
                string next = ldesign[i];
                arrayOfstring = next.Trim().Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
                int num2 = arrayOfstring.Length;
                if (num <= num2)
                {
                    res = next;
                    num = num2;
                }
            }
            return res;
        }


        /* Descripción:
         *  Constructor de la clase. Construye una lista de tablas de sumas de cuadrados a partir 
         *  de un objeto multifaceta.
         * Parámetros:
         *      MultiFacetsObs mfo: Objeto multifaceta a partir del cual se optienen los datos. La
         *              lista de facetas se obtiene tambián a partir de este objeto.
         *      bool zero: Si es true se tomarán los valores nulos como ceros al clacular la sumas
         *              de cuadrados parciales.
         */
        public TableAnalysisOfVariance(MultiFacetsObs mfo, bool zero)
            : this(mfo.ListFacets().CombinationStringWithoutRepetition(), mfo, zero)
        {
        }


        /*
         *Descripción:
         * Contruye un objeto de la clase a partir de una lista de facetas y una lista de
         * suma de cuadrados.
         */
        public TableAnalysisOfVariance(ListFacets listF, Dictionary<string, double?> ssq)
            : this()
        {
            this.listFacets = listF;
            this.ldesign = this.listFacets.CombinationStringWithoutRepetition();
            int num_ldesign = this.ldesign.Count();
            int num_listOSSQ = ssq.Keys.Count;

            Dictionary<string, double?> newssq = new Dictionary<string, double?>();

            if (num_ldesign.Equals(num_listOSSQ))
            {
                bool incluido = true;
                for (int i = 0; i < num_listOSSQ && incluido; i++)
                {
                    string key_design = this.ldesign[i];
                    df.Add(key_design, this.listFacets.DegreeOfFreedom(key_design));
                    incluido = ssq.ContainsKey(key_design);
                    double? d = ssq[key_design];
                    newssq.Add(key_design, d);
                }
                if (!incluido)
                {
                    throw new TableAnalysisOfVarianceException
                    ("Error al crear listTableSSQ: hay correspondencia entre lista de combinancion de facetas y lista de suma de cuadrados");
                }
                
            }
            else
            {
                throw new TableAnalysisOfVarianceException
                    ("Error al crear listTableSSQ: hay correspondencia entre lista de combinancion de facetas y lista de suma de cuadrados");
            }

            this.ssq = newssq;
            // ahora calcula los cuadrados medios
            CalcMSQ();
            // Calculamos los componentes de varianza aleatorios
            CalcRandomComp();
            // Calculamos los componentes de varianza mixtos
            CalcMixComp();
            // Calculamos los componentes de varianza corregida
            CalcCorrecComp();
            // Calculamos el porcentaje
            CalcPorcentage();
            // Calculamos el error standar
            CalcStandardError();
        }// end TableAnalysisOfVariance


        /*
         *Descripción:
         * Contruye un objeto de la clase a partir de una lista de facetas y una lista de
         * suma de cuadrados.
         */
        public TableAnalysisOfVariance(ListFacets listF_mod, TableAnalysisOfVariance lssqOriginal)
            : this()
        {
            this.listFacets = listF_mod;
            List<string> listOfSourceOfVarOriginal = lssqOriginal.SourcesOfVar();
            // List<ListFacets> newListOfSourceOfVar = listF_mod.CombinationWithoutRepetition();
            this.ldesign = listF_mod.CombinationStringWithoutRepetition();
            int n_sourceOfVarOrig = listOfSourceOfVarOriginal.Count;

            for (int i = 0; i < n_sourceOfVarOrig; i++)
            {
                string key_design_original = listOfSourceOfVarOriginal[i];
                string key_design_mod = this.ldesign[i];
                if (key_design_original.Equals(key_design_mod))
                {
                    // entonce realizamos asignación
                    this.ssq.Add(key_design_mod,lssqOriginal.ssq[key_design_original]);
                }
                else
                {
                    // lanzamos una excepción
                    throw new TableAnalysisOfVarianceException
                    ("Error al crear listTableSSQ: hay correspondencia entre lista de combinancion de facetas y lista de suma de cuadrados");
                }
            }
            // ahora calcula los cuadrados medios
            CalcMSQ();
            // Calculamos los componentes de varianza aleatorios
            CalcRandomComp();
            // Calculamos los componentes de varianza mixtos
            CalcMixComp();
            // Calculamos los componentes de varianza corregida
            CalcCorrecComp();
            // Calculamos el porcentaje
            CalcPorcentage();
            // Calculamos el error standar
            CalcStandardError();
        }// end TableAnalysisOfVariance


        /* Descripción:
         *  Constructor. Le pasamos como argumento la suma de cuadrados y las componentes: aleatorias
         *  mixtas y corregidas.
         */
        public TableAnalysisOfVariance(ListFacets listF, Dictionary<string, double?> ssq, 
            Dictionary<string,double> d_df, 
            Dictionary<string, double?> random,
            Dictionary<string, double?> mix, Dictionary<string, double?> correc)
            : this()
        {
            this.listFacets = listF;
            this.ldesign = listF.CombinationStringWithoutRepetition();
            this.ssq = ssq;
            this.df = d_df;
            int n = this.ldesign.Count;
            for (int i = 0; i < n; i++)
            {
                double df = this.df[this.ldesign[i]];
                double? msq_aux = 0;
                double? ssq_aux = ssq[this.ldesign[i]];
                if (df > 0 && ssq_aux != null)
                {
                    msq_aux = (double)ssq_aux / df;
                }
                else
                {
                    throw new TableAnalysisOfVarianceException("Error en los parámetros");
                }
                this.msq.Add(this.ldesign[i], msq_aux);
            }
            this.randomComp = random;
            this.mixComp = mix;
            this.correcComp = correc;
            // Calculamos el porcentaje
            CalcPorcentage();
            // Calculamos el error standar
            CalcStandardError();
        }// end TableAnalysisOfVariance


        /* Descripción:
         *  Constructor. Le pasamos como argumento la suma de cuadrados y las componentes: aleatorias
         *  mixtas y corregidas.
         */
        public TableAnalysisOfVariance(ListFacets listF, Dictionary<string, double?> ssq,
            Dictionary<string, int> d_df,
            Dictionary<string, double?> msq,
            Dictionary<string, double?> random,
            Dictionary<string, double?> mix, Dictionary<string, double?> correc)
            : this()
        {
            this.listFacets = listF;
            this.ldesign = listF.CombinationStringWithoutRepetition();
            this.ssq = ssq;
            this.msq = msq;
            this.randomComp = random;
            this.mixComp = mix;
            this.correcComp = correc;
            // Calculamos el porcentaje
            CalcPorcentage();
            // Calculamos el error standar
            CalcStandardError();
        }// end TableAnalysisOfVariance


        /* Descripción:
         *  Constructor. Le pasamos como argumento la suma de cuadrados y las componentes: aleatorias
         *  mixtas y corregidas.
         */
        public TableAnalysisOfVariance(ListFacets listF, List<string> ldesign,
            Dictionary<string, double?> ssq,
            Dictionary<string, double> df,
            Dictionary<string, double?> msq,
            Dictionary<string, double?> random,
            Dictionary<string, double?> mix,
            Dictionary<string, double?> correc,
            Dictionary<string, double?> porc,
            Dictionary<string, double?> stad_error)
            : this()
        {
            this.listFacets = listF;
            this.ldesign = ldesign;
            this.ssq = ssq;
            this.df = df;
            this.msq = msq;
            this.randomComp = random;
            this.mixComp = mix;
            this.correcComp = correc;
            this.porcentage = porc;
            this.standardError = stad_error;
        }
        #endregion Constructores de la clase TableAnalysisOfVariance



        #region Métodos de Consulta
        /*======================================================================================
         * Métodos de Consulta
         * ===================
         *  - SourcesOfVar: Listas de fuentes de variación.
         *  - ListFacets: Devuelve la lista de facetas de la tabla de análisis
         *  - DegreesOfFreedom: Devuelve el grado de libertad
         *  - SSQ: la suma de cuadrados.
         *  - MSQ: Suma de cuadrados medios.
         *  - RandomComp: Componente de varianza aleatorio.
         *  - MixModComp: Componente de vaianza mixto.
         *  - CorretedComp: Componente de vaianza corregido.
         *  - Porcentage: Tanto por ciento.
         *  - StandardError: Error standar.
         *======================================================================================*/

        /*
         * Descripción:
         *  Devuelve la lista de fuentes de variación
         */
        public List<string> SourcesOfVar()
        {
            return this.ldesign;
        }


        /*
         * Descripción:
         *  Devuelve la lista de facetas.
         */
        public ListFacets ListFacets()
        {
            return this.listFacets;
        }

        /* Descripción:
         *  Devuelve el grado de libertad para el diseño dado como parametro
         */
        public double DegreesOfFreedom(string design)
        {
            return this.df[design];
        }

        /* Descripción:
         *  Devuelve la suma de cuadrados para la fuente de vartiación que se pasa como parámetro.
         * Parámetro:
         *      string key: Fuente de variación.
         */
        public double? SSQ(string key)
        {
            return this.ssq[key];
        }


        /*
         * Descripción:
         *  Devuelve la suma de cuadrados medios para la fuente de vartiación que se pasa como 
         *  parámetro.
         * Parámetro:
         *      string key: Fuente de variación.
         */
        public double? MSQ(string key)
        {
            return this.msq[key];
        }


        /*
         * Descripción:
         *  Devuelve la componente de varianza aleatoria para la fuente de vartiación que se pasa como 
         *  parámetro.
         * Parámetro:
         *      string key: Fuente de variación.
         */
        public double? RandomComp(string key)
        {
            return this.randomComp[key];
            // en principio son los mismos que los aleatorios
        }


        /*
         * Descripción:
         *  Devuelve la componente de varianza mixtos para la fuente de variación que se pasa como 
         *  parámetro.
         * Parámetro:
         *      string key: Fuente de variación.
         */
        public double? MixModComp(string key)
        {
            return this.mixComp[key];
        }


        /*
         * Descripción:
         *  Devuelve la componente de varianza corregidos para la fuente de vartiación que se pasa como 
         *  parámetro.
         * Parámetro:
         *      string key: Fuente de variación.
         */
        public double? CorretedComp(string key)
        {
            return this.correcComp[key];
        }


        /* Descripicón:
         *  Devuelvel los porcentajes
         */
        public double? Porcentage(string key)
        {
            return this.porcentage[key];
        }


        /* Descripicón:
         *  Devuelvel el error estandar
         */
        public double? StandardError(string key)
        {
            return this.standardError[key];
        }


        #endregion Métodos de Consulta



        #region Métodos de instancia
        /*======================================================================================
         * Métodos de instancia
         *======================================================================================*/

        /*
         * Descripción:
         *  Elimina un elemento pasado como parámetro de la lista. Si el elemento es eliminado
         *  correctamente devuelve true, en otro caso devuelve false.
         * Parámetros:
         *      TableMeans tms: Tabla de suma de cuadrados que queremos eliminar.
         */
        /*
        public bool Remove(TableSSQ tms)
        {
            return this.listTableSSQ.Remove(tms);
        }
        */

        #endregion Métodos de instancia



        #region Operaciones auxiliares (calculos parciales para la suma de cuadrados)
        /*==============================================================================================
         * Operaciones auxiliares (calculos parciales para la suma de cuadrados)
         * =====================================================================
         * 
         * - Calculo del residuio
         * - Calculo parcial de la suma de cuadrados
         * - Calculo de suma de cuadrados
         * - Calculo de cuadrado medio
         * - Calculo de componente de varianza aleatorio
         * - Calculo de componente de varianza mixto
         * - Calculo de componente de varianza corregido
         * - Suma total de las componente de varianza positivas
         * - Calculo el porcentaje
         *==============================================================================================*/

        /*
         * Descripción:
         *  Calculo del Residuo.
         * Parámetros:
         *      ListFacets lf: es la lista de fatetas para la que vamos a calular el residuo.
         */
        private double? CalcResidue(InterfaceObsTable table)
        {
            double? retVal = 1 / (this.listFacets.MultiOfLevel());
            //double? sum_X = this.listTableSSQ[this.listTableSSQ.Count - 1].Sum_sumX();
            double? sum_X = table.SumOfData();

            if (sum_X == null)
            {
                retVal = null;
            }
            else
            {
                retVal = retVal * Math.Pow((double)sum_X, 2);
            }
   
            return retVal;
        }// end CalcResidue



        /*
         * Descripción:
         *  Devuelve el diccionario(clave,valor) de Suma de cuadrados parciales. La clave es una lista de facetas.
         * Parámetros:
         *      Dictionary<string, double?> sum_x2: suma de cuadrados al cuadrado.
         */
        private double? PartialSumOfSquares(ListFacets lf, MultiFacetsObs mfo)
        {
            InterfaceTableSSQ table = new TableSSQ(lf, mfo);
            double? sumX_2 = table.Sum_sumX2();
            sumX_2 = (1 / this.listFacets.MultipSourcesOfVariabilityAbsent(lf)) * sumX_2;
            return sumX_2;
        }


        /* Descripción:
         *  Suma de cuadrados parciales mediante la media al cuadrado, tal como viene en la tesis
         *  doctoral de Joann Lynn Moore
         * Parámetros:
         *      ListFacets lf: Lista de facetas
         *      MultiFacetsObs mfo: Tabla de frecuencias a partir de la cuals se calculará la suma de
         *              cuadrados parcial.
         *      bool zero: Si es bool se calculará la media tomando los valores nulos como ceros.
         */
        private double? PartialSumOfSquaresByMeans(ListFacets lf, MultiFacetsObs mfo, bool zero)
        {
            TableMeans  tableMeans = new TableMeans(lf, "", mfo, zero);
            int rows = tableMeans.MeansTableRows();

            double? sumX_2 = 0;
            for (int i = 0; i < rows; i++)
            {
                double? mean = tableMeans.MeanData(i);
                if (mean != null)
                {
                    sumX_2 = sumX_2 + Math.Pow((double)mean,2);
                }
            }

            sumX_2 = this.listFacets.MultipSourcesOfVariabilityAbsent(lf) * sumX_2;
            return sumX_2;
        }


        /*
         * Descripción:
         *  Calcula la suma de cuadrados de las desviaciones
         */
        //private void CalcSSQ(Dictionary<string, double?> partialSSQ, double? residue)
        //{
        //    int n = this.ldesign.Count;
        //    for (int i = 0; i < n; i++)
        //    {
        //        string design = this.ldesign[i];
        //        ListFacets lf = this.listFacets.ListDesignFacets(design);
        //        List<string> llf_aux = lf.CombinationStringWithoutRepetition();
        //        llf_aux.Reverse();
        //        int sign = 1;
        //        int num = llf_aux[0].Count();
        //        double d = 0;
        //        int n2 = llf_aux.Count;
        //        for(int j=0; j <n2; j++)
        //        {
        //            string lf2 = llf_aux[j];
        //            if (num != lf2.Count())
        //            {
        //                sign *= (-1);
        //                num = lf2.Count();
        //            }

        //            //int num_facets = lf.Count(); /* el número de facetas determina el signo */
        //            //int pos = llf.IndexOf(lf_aux[j]); /* posición en la que esta el dato parcial */
        //            d = d + (sign *(double)partialSSQ[lf2]);
        //        }
        //        if ((lf.Count() % 2) == 0)
        //        {
        //            sign = 1;
        //        }
        //        else
        //        {
        //            sign = (-1);
        //        }
        //        this.ssq[design] = d + (sign * residue);
        //    }
        //}// end public void CalcSSQ()



        /*
         * Descripción:
         *  Calcula la suma de cuadrados de las desviaciones
         */
        //private void CalcSSQ(MultiFacetsObs mfo, List<string> ldesign)
        //{
        //    Dictionary<string, double?> terms;
        //    double? residue;

        //    int num = ldesign.Count;
        //    for (int i = 0; i < num; i++)
        //    {
        //        string design = ldesign[i];
        //    }
        //}


        /*
         * Descripción:
         *  Calcula la suma de cuadrados medios.
         */
        private void CalcMSQ()
        {
            int n = this.ldesign.Count;
            for (int i = 0; i < n; i++)
            {
                string design = this.ldesign[i];
                double df = this.df[design];
                if (df == 0)
                {
                    /* En el caso de que el grado de libertad sea 0, como no podemos
                     * dividir por cero entonces el cuadrado medio será 0 también. */
                    this.msq.Add(design, 0);
                }
                else
                {
                    this.msq.Add(design, this.ssq[design] / df);
                }
            }
        }


        /*
         * Descripción:
         *  Calculo del componente de varianza aleatorio
         */
        private void CalcRandomComp()
        {
            int n = this.ldesign.Count;
            List<string> sortl = SortDesing(this.ldesign, this.listFacets.Count());

            for (int j = 0; j < n; j++)
            {
                // PASO 0
                string design = sortl[j];
                double? d = this.msq[design];
                ListFacets lf = this.listFacets.ListDesignFacets(design);
                int sign = (-1);

                // cuento el número de facetas de la lista lf para compararlo posteriormente
                int numF = lf.Count();

                // inicializamos la lista de facetas que será usada en el paso 1
                ListFacets l_facets_used = new ListFacets();

                // PASO 1
                if (numF != this.listFacets.Count())
                {
                    int numOfTerm = 0;
                    bool end = false;
                    numF++;
                    for (int i = 0; i < n && !end; i++)
                    {
                        string design_aux = sortl[i];
                        ListFacets lf_aux = this.listFacets.ListDesignFacets(design_aux);
                        int numFacet_of_lf_aux = lf_aux.Count();
                        end = !(numFacet_of_lf_aux <= numF);

                        if ((numF== numFacet_of_lf_aux) && lf_aux.ContainsList(lf))
                        {
                            d = d + (sign * this.msq[design_aux]);
                            l_facets_used = l_facets_used.Union(lf_aux);
                            numOfTerm++;
                        }
                    }

                    // PASO 2
                    /* Si el número de terminos es mayor que uno ejecuto el paso 2
                     */
                    while (numOfTerm > 1)
                    {
                        sign = ((-1) * sign);
                        numF++;
                        numOfTerm = 0;
                        // inicializamos la lista de facetas que será usada en el paso i+1
                        ListFacets l_facets_used2 = new ListFacets();
                        end = false;

                        for (int i = 0; i < n && !end; i++)
                        {
                            string design_aux = sortl[i];
                            ListFacets lf_aux = this.listFacets.ListDesignFacets(design_aux);
                            int numFacet_of_lf_aux = lf_aux.Count();
                            end = !(numFacet_of_lf_aux <= numF);

                            if ((numF == numFacet_of_lf_aux) && l_facets_used.ContainsList(lf_aux) && lf_aux.ContainsList(lf))
                            {
                                d = d + (sign * this.msq[design_aux]);
                                l_facets_used2 = l_facets_used2.Union(lf_aux);
                                numOfTerm++;
                            }
                        }
                        l_facets_used = l_facets_used2;
                    }
                }

                // Calculo la componente de varianza aleatoria y la añado a la estructura de datos
                d = (1 / this.listFacets.MultipSourcesOfVariabilityAbsent(lf)) * d;
                this.randomComp.Add(design, d);
            }
        }// end CalcRandomComp


        /* Descripción:
         *  Calculo de componetes de varianza mixtos. Estos serán iguales a los aleatorios en el caso
         *  de que todas las facetas sean aleatorias infinita.
         */
        public void CalcMixComp()
        {
            if (this.listFacets.HasAllFacetsSizeInfinite())
            {
                /* todas las facetas tienen tamaño de universo infinito y no es necesario calcularlas
                 * basta con que la igualesmos con las aleatorias. */
                this.mixComp = this.randomComp;
            }
            else
            {
                /* Al menos una faceta no es aleatoria infinita. */
                int n = this.ldesign.Count;
                for (int j = 0; j < n; j++)
                {
                    string design = this.ldesign[j];
                    ListFacets lf = this.listFacets.ListDesignFacets(design);
                    double d = 0;
                    for (int i = 0; i < n; i++)
                    {
                        string key_design_aux = this.ldesign[i];
                        ListFacets lf_aux = this.listFacets.ListDesignFacets(key_design_aux);
                        if(lf_aux.ContainsList(lf))
                        {
                            double d2 = (double)this.randomComp[key_design_aux];
                            // lf_aux = lf_aux.Difference(lf);
                            lf_aux = lf_aux.SourcesOfVariabilityAbsent(lf);
                            double mult = lf_aux.MultSizeOfUniverse();
                            if (mult > 0)
                            {
                                d = d + (1 / mult) * d2;
                            }
                        }
                    }
                    this.mixComp.Add(design, d);
                }
            }
        }// end CalcMixComp()


        /* Descripción
         *  Calcula el componente de varianza corregida
         */
        public void CalcCorrecComp()
        {
            int n = this.ldesign.Count;
            for (int i = 0; i < n; i++)
            {
                string design = this.ldesign[i];
                ListFacets lf = this.listFacets.ListDesignFacets(design);
                double d = (double)this.mixComp[design];
                if (lf.HasAllFacetsFixed())
                {
                    // Se trata de una faceta fija
                    d = d * lf.HopeOfVariance();
                }
                this.correcComp.Add(design, d);
            }
        } // end CalcCorrecComp()


        /* Descripción:
         *  Suma total de las componente de varianza positivas.
         */
        private double SumTotalPositiveCompOfVar()
        {
            double retVal = 0;
            int pos = this.ldesign.Count;
            for (int i = 0; i < pos; i++)
            {
                string key = this.ldesign[i];
                double n = (double)this.correcComp[key];
                if (n > 0)
                {
                    retVal = retVal + n;
                }
            }
            return retVal;
        }


        /* Descripción:
         *  Calculo de porcentaje
         */
        private void CalcPorcentage()
        {
            double sumTotal = SumTotalPositiveCompOfVar();
            int pos = this.ldesign.Count;
            for (int i = 0; i < pos; i++)
            {
                string key = this.ldesign[i];
                double n = (double)this.correcComp[key];
                if (n <= 0)
                {
                    this.porcentage[key] = 0;
                }
                else
                {
                    this.porcentage[key] = (n * 100) / sumTotal;
                }
            } 
        }


        /* Descripción:
         *  Calculamos del error estándar.
         */
        private void CalcStandardError()
        {
            int pos = this.ldesign.Count;
            for (int i = 0; i < pos; i++)
            {
                string key = this.ldesign[i];
                ListFacets lf = this.listFacets.ListDesignFacets(key);
                double aux = 0;
                for (int j = 0; j < pos; j++)
                {
                    string key_aux = this.ldesign[j];
                    ListFacets lf_aux = this.listFacets.ListDesignFacets(key_aux);
                    if (lf_aux.ContainsList(lf))
                    {
                        double d1 = (double)this.msq[key_aux]; 
                        double d = (Math.Pow(d1,2)* 2);
                        double d2 = lf_aux.DegreeOfFreedom(key_aux);
                        aux = aux + (d / (d2 + 2));
                    }
                }
                
                this.standardError[key] = (1 / this.listFacets.MultipSourcesOfVariabilityAbsent(lf)) * Math.Sqrt(aux);
            }
        }

        #endregion Operaciones auxiliares (calculos parciales para la suma de cuadrados)



        #region Calculo de totales por columnas
        /*==============================================================================================
         * Calculo se totales por columnas
         *  - Total suma de cuadrados
         *  - Total grado de libertad
         *==============================================================================================*/
        
        /*
         * Descripción:
         *  Devuelve la suma total de las sumas de los cuadrados
         */
        public double? CalcTotalSSQ()
        {
            double? retVal = null;
            int n = this.ldesign.Count;
            for (int i = 0; i < n; i++)
            {
                string key = this.ldesign[i];
                double? d = this.ssq[key];
                if (d != null)
                {
                    if (retVal == null)
                    {
                        retVal = d;
                    }
                    else
                    {
                        retVal = retVal + d;
                    }
                }
            }
            return retVal;
        }


        /*
         * Descripción:
         *  Devuelve la suma total de todos los grados de libertad
         */
        public double? CalcTotalDF()
        {
            double? retVal = null;
            int n = this.ldesign.Count;
            for (int i = 0; i < n; i++)
            {
                string design = this.ldesign[i];
                double? d = this.df[design];
                if (d != null)
                {
                    if (retVal == null)
                    {
                        retVal = d;
                    }
                    else
                    {
                        retVal = retVal + d;
                    }
                }
            }
            return retVal;
        }

        #endregion Calculo se totales por columnas


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
            // Copiamos la lista de facetas
            ListFacets copyListFacet = (ListFacets)this.listFacets.Clone(); // copia en profundidad
            // Copiamos la lista de diseños
            List<string> copyLdesign = new List<string>();
            
            int n = this.ldesign.Count;
            for (int i = 0; i < n; i++)
            {
                copyLdesign.Add(string.Copy(this.ldesign[i]));
            }

            // Copiamos el grado de libertad
            Dictionary<string, double> copydf = ClonarDictionary(this.df);

            // Copiamos la suma de cuadrados
            Dictionary<string, double?> copySSq = ClonarDictionary(this.ssq);
            // Copiamos los cuadrados medios
            Dictionary<string, double?> copyMSq = ClonarDictionary(this.msq);
            // Copiamos el componente de Varianza aleatorio
            Dictionary<string, double?> copyRandomComp = ClonarDictionary(this.randomComp); 
            // Copiamos el componente de Varianza Mixto
            Dictionary<string, double?> copyMixComp = ClonarDictionary(this.mixComp);
            // Copiamos el componente de Varianza Corregido
            Dictionary<string, double?> copyCorrecComp = ClonarDictionary(this.correcComp);

            // Copiamos los Porcentajes
            Dictionary<string, double?> copyPorcentage = ClonarDictionary(this.porcentage);
            // Error estandar
            Dictionary<string, double?> copyStandardError = ClonarDictionary(this.standardError);

            return new TableAnalysisOfVariance(copyListFacet, copyLdesign, copySSq, copydf, copyMSq,
                copyRandomComp, copyMixComp, copyCorrecComp, copyPorcentage, copyStandardError);

        }// end Clone


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
         *  y el objeto es un double.
         */
        private static Dictionary<string, double> ClonarDictionary(Dictionary<string, double> original)
        {
            Dictionary<string, double> copy = new Dictionary<string, double>(); // Copia a retornar

            foreach (string skey in original.Keys)
            {
                string copyKey = string.Copy(skey);
                double d = original[skey];

                copy.Add(copyKey, d);
            }

            return copy;
        }
        #endregion Implementacion de la interfaz


        #region Escritura de datos de una Tabla de análisis de varianza
        /***********************************************************************************************
         * Métodos de para la escritura de datos
         *  - WritingFileDataSumOfSquares
         *  - WritingStreamGTableAnalysisOfVariance
         ***********************************************************************************************/


        /* Descripción:
         *  Escribe un fichero que contiene las sumas de cuadrados de la tabla de análisis. Los datos
         *  se almacenarán uno por línea. Si el dato es null se almacenará cero en el fichero.
         */
        public bool WritingFileDataSumOfSquares(String fileName)
        {
            bool res = false; // variable de retorno

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                Dictionary<string,double?> sumOfSquares = this.ssq;
                int n = this.ldesign.Count;
                for (int i = 0; i < n; i++)
                {
                    string design = this.ldesign[i];
                    double? d = sumOfSquares[design];
                    if (d == null)
                    {
                        d = 0;
                    }
                    writer.WriteLine(d.ToString());
                }
                res = true;
            }
            return res;
        }

        /* Descripción:
         *  Escribe en el StreamWriter 
         */
        public bool WritingStreamGTableAnalysisOfVariance(StreamWriter writerFile)
        {
            bool res = false; // variable de retorno

            // Escribimos la marcha de comienzo de la tabla de análisis de varianza
            writerFile.WriteLine(BEGIN_TABLE_ANALYSIS_OF_VARIANCE);

            // Escribimos la lista de facetas
            res = this.listFacets.WritingStreamListFacets(writerFile);

            // Escribimos la lista de lista de facetas y el resto de los datos
            if (res)
            {//(* 1 *)
                int n = this.ldesign.Count;
                // fila de datos
                for (int i = 0; i < n && res; i++)
                {
                    // Escribimos la lista de facetas
                    string key = this.ldesign[i];

                    double? ssq = this.ssq[key]; // suma de cuadrados
                    double? df = this.df[key]; // grado de libertad
                    double? msq = this.msq[key]; // Suma de cuadrados medios (M.S.C.)
                    double? randomComp = this.randomComp[key]; // Componente de Varianza Aleatorio
                    double? mixComp = this.mixComp[key]; // Componentes de Varianza Mixtos
                    double? correcComp = this.correcComp[key];
                    double? porcentage = this.porcentage[key];
                    double? standardError = this.standardError[key];

                    string line = key + " " + ssq.ToString() + " " + df.ToString() + " " 
                         + msq.ToString() + " " + randomComp.ToString()
                         + " " + mixComp.ToString() + " " + correcComp.ToString() + " "
                         + porcentage.ToString() + " " + standardError.ToString();

                    writerFile.WriteLine(line);
                }
            }//end if (* 1 *)

            // escribimos el fin de la tabla de análisis de varianza
            writerFile.WriteLine(END_TABLE_ANALYSIS_OF_VARIANCE);
            return res;
        }// end WritingStreamGTableAnalysisOfVariance

        #endregion Escritura de datos de una Tabla de análisis de varianza



        #region Lectura de datos de una Tabla de Análisis de Varianza
        /***********************************************************************************************
         * Métodos de para la lectura de datos
         *  - ReadingStreamTableAnalysisOfVariance
         ***********************************************************************************************/

        /* Descripción:
         *  Lee los datos de una Tabla de Análisis de Varianza de un stream y lo devuelve como objeto.
         * Parámetros:
         *      StreamReader reader: El stream del que vamos a leer la Tabla de Análisis de Varianza.
         */
        public static TableAnalysisOfVariance ReadingStreamTableAnalysisOfVariance(StreamReader reader)
        {
            TableAnalysisOfVariance tableAnalysis = null;
            try
            {
                string line;
                ListFacets lf; // Lista de facetas
                //ListFacets lf_aux; // Lista de facetas auxiliar
                List<string> ldesing = new List<string>();

                /* inicalizamos el vector suma de cuadrados de las desviaciones (ssq)*/
                Dictionary<string, double?> d_ssq = new Dictionary<string, double?>();
                /* inicializamos el grado de libertad */
                Dictionary<string, double> d_df = new Dictionary<string, double>();
                /* inicalizamos el vector cuadrados medios de las desviaciones (msq)*/
                Dictionary<string, double?> d_msq = new Dictionary<string, double?>();
                /* inicializamos el vector de componentes de varianza aleatorios */
                Dictionary<string, double?> d_randomComp = new Dictionary<string, double?>();
                Dictionary<string, double?> d_mixComp = new Dictionary<string, double?>();
                Dictionary<string, double?> d_correcComp = new Dictionary<string, double?>();
                Dictionary<string, double?> d_porcentage = new Dictionary<string, double?>();
                Dictionary<string, double?> d_standardError = new Dictionary<string, double?>();

                // Leemos la lista de facetas
                if ((line = reader.ReadLine()).Equals(MultiFacetData.ListFacets.BEGIN_LISTFACETS))
                {
                    lf = MultiFacetData.ListFacets.ReadingStreamListFacets(reader);
                }
                else
                {
                    throw new TableAnalysisOfVarianceException();
                }

                
                while (!(line = reader.ReadLine()).Equals(END_TABLE_ANALYSIS_OF_VARIANCE))
                {// (* 1 *)
                    
                    // Leemos la linea con los datos
                    // line = reader.ReadLine();
                    char[] delimeterChars = { ' ' }; // nuestro delimitador será el caracter blanco
                    string[] arrayOfDouble = line.Trim().Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);

                    if (arrayOfDouble.Length == 9)
                    {
                        string key = arrayOfDouble[0];
                        ldesing.Add(key);
                        // double? ssq = double.Parse(arrayOfDouble[1]); // suma de cuadrados
                        double? ssq = ConvertNum.String2Double(arrayOfDouble[1]); // suma de cuadrados

                        d_ssq.Add(key, ssq);
                        // double df = double.Parse(arrayOfDouble[2]); // suma de cuadrados
                        double df = (double)ConvertNum.String2Double(arrayOfDouble[2]); // suma de 
                        d_df.Add(key, df);
                        // double? msq = double.Parse(arrayOfDouble[3]); // Suma de cuadrados medios (M.S.C.)
                        double? msq = ConvertNum.String2Double(arrayOfDouble[3]); // Suma de cuadrados medios (M.S.C.)
                        d_msq.Add(key, msq);
                        // double? randomComp = double.Parse(arrayOfDouble[4]); // Componente de Varianza Aleatorio
                        double? randomComp = ConvertNum.String2Double(arrayOfDouble[4]); // Componente de Varianza Aleatorio
                        d_randomComp.Add(key, randomComp);
                        // double? mixComp = double.Parse(arrayOfDouble[5]); // Componentes de Varianza Mixtos
                        double? mixComp = ConvertNum.String2Double(arrayOfDouble[5]); // Componentes de Varianza Mixtos
                        d_mixComp.Add(key, mixComp);
                        // double? correcComp = double.Parse(arrayOfDouble[6]);
                        double? correcComp = ConvertNum.String2Double(arrayOfDouble[6]);
                        d_correcComp.Add(key, correcComp);
                        // double? porcentage = double.Parse(arrayOfDouble[7]);
                        double? porcentage = ConvertNum.String2Double(arrayOfDouble[7]);
                        d_porcentage.Add(key, porcentage);
                        // double? standardError = double.Parse(arrayOfDouble[8]);
                        double? standardError = ConvertNum.String2Double(arrayOfDouble[8]);
                        d_standardError.Add(key, standardError);
                    }
                    else
                    {
                        throw new TableAnalysisOfVarianceException("Error al leer de fichero");
                    }

                }// end while (* 1 *)

                tableAnalysis = new TableAnalysisOfVariance(lf, ldesing, d_ssq, d_df, d_msq, d_randomComp, 
                    d_mixComp, d_correcComp,d_porcentage, d_standardError);
            }
            catch (FormatException)
            {
                throw new TableAnalysisOfVarianceException("Error al leer de fichero");
            }
            catch (ListFacetsException)
            {
                throw new TableAnalysisOfVarianceException("Error al leer de fichero");
            }

            return tableAnalysis;

        }// end ReadingStreamTableAnalysisOfVariance

        #endregion Lectura de datos de una Tabla de Análisis de Varianza


        #region Métodos redefinidos: ToString, Equals, GetHashCode
        /*======================================================================================
         * Métodos Redefinidos
         *======================================================================================*/
        /*
         * Descripción:
         *  Redefinición del método Tostring().
         */
        public override string ToString()
        {
            StringBuilder res = new StringBuilder(); // variable de retorno
            int n = this.ldesign.Count;

            res.Append("\n\nSuma de cuadrados:\n");

            for (int i = 0; i < n; i++)
            {
                string key = this.ldesign[i];
                res.Append(key + ": "+ this.ssq[key] + "\n");
            }
            res.Append("\n\n");

            res.Append("\n\nGrado de libertad:\n");
            for (int i = 0; i < n; i++)
            {
                string key = this.ldesign[i];
                res.Append(key + ": " + this.DegreesOfFreedom(key) + "\n");
            }
            res.Append("\n\n");

            res.Append("\n\nCuadrado medio:\n");
            for (int i = 0; i < n; i++)
            {
                string key = this.ldesign[i];
                res.Append(key + ": " + this.msq[key] + "\n");
            }
            res.Append("\n\n");

            res.Append("\n\nComponenetes de Varianza aleatorios:\n");

            for (int i = 0; i < n; i++)
            {
                string key = this.ldesign[i];
                res.Append(key + ": " + this.randomComp[key] + "\n");
            }
            res.Append("\n\n");

            return res.ToString();
        }
        #endregion Métodos redefinidos: ToString, Equals, GetHashCode


        /* Descripción:
         *  Ordena una lista de diseños en función exclusivamente del número de facetas del que
         *  se compone el diseño sin tener en cuenta nada más. El orden es ascendente, empezaría
         *  por aquellos diseños que solo tienen una faceta, continuaría con las de dos y así
         *  sucesivamente.
         * Parámetros
         *  List<string> ldesign: listas de diseños.
         *  int s: número de facetas que puede tener como maximo el diseño.
         */
        private static List<string> SortDesing(List<string> ldesign, int s)
        {
            List<string> sort_ldesign = new List<string>();

            List<List<string>> l_ldesign = new List<List<string>>();
            for (int i = 0; i < s; i++)
            {
                List<string> l = new List<string>();
                l_ldesign.Add(l);
            }

            int n = ldesign.Count;
            char[] delimeterChars = { '[', ']', ':' }; // nuestro delimitador será el caracter blanco

            for (int i = 0; i < n; i++)
            {
                string design = ldesign[i];
                
                string[] arrayOfString = design.Trim().Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
                int numOfFacet = arrayOfString.Length; // námero de facetas
                List<string> l = l_ldesign[numOfFacet - 1];
                l.Add(design);
            }

            // juntamos todas las listas
            for (int i = 0; i < s; i++)
            {
                n = l_ldesign[i].Count;
                for (int j = 0; j < n; j++)
                {
                    sort_ldesign.Add(l_ldesign[i][j]);
                }
            }

            // devolvemos la lista ordenada
            return sort_ldesign;
        }// end SortDesing


        /* Descripción:
         *  Remplaza la lista de facetas por la nueva que se pasa como parámetro. 
         *  
         *  NOTA:
         *  Se aprovecha de que el orden de la lista de los diseños será el mismo para
         *  el antiguo que para el nuevo.
         *  
         * @Precondición:
         *  La nueva lista tiene que tener el mismo número de facetas y para cada una de ellas
         *  debe tener la misma jerarquia de anidamientos.
         */
        public TableAnalysisOfVariance ReplaceListFacets(ListFacets newListFacets)
        {
            List<string> newldesign = newListFacets.CombinationStringWithoutRepetition();

            int num = newldesign.Count;
   
            // Copia actualizada de la suma de cuadrados
            Dictionary<string, double?> newSsq = new Dictionary<string, double?>();
            for (int i = 0; i < num; i++)
            {
                newSsq.Add(newldesign[i], this.ssq[this.ldesign[i]]);
            }

            return new TableAnalysisOfVariance(newListFacets, newSsq);
        }// end;



        #region Conversión con DataSet
        /* Descripción:
         *  Toma los datos de un objeto analysis de varianza y lo devuelve en un dataSet
         */
        public DataSet TbAnalysis2DataSet()
        {
            // Creamos el dataSet
            DataSet dsTbAnalysis = new DataSet("DataSet_Table_Analysis");
            // Obtenemos el dataTable de lista de facetas
            DataTable dtListFacets = this.listFacets.ListFacets2DataTable("TbFacets");
            dsTbAnalysis.Tables.Add(dtListFacets);
            // Obtenemos eñ dataTable
            DataTable dtSkipLevels = this.listFacets.SkipLevels2DataTable("TbSkipLevels");
            dsTbAnalysis.Tables.Add(dtSkipLevels);
            // Creamos el dataTable de Análysis de Varianza
            DataTable dtAnalysisOfVar = new DataTable("TbAnalysis");
            // Añadimos los datos
            dtAnalysisOfVar.Columns.Add(new DataColumn("source_of_var", System.Type.GetType("System.String")));
            dtAnalysisOfVar.Columns.Add(new DataColumn("df", System.Type.GetType("System.Double")));
            dtAnalysisOfVar.Columns.Add(new DataColumn("ssq", System.Type.GetType("System.Double")));
            dtAnalysisOfVar.Columns.Add(new DataColumn("msq", System.Type.GetType("System.Double")));
            dtAnalysisOfVar.Columns.Add(new DataColumn("random_comp", System.Type.GetType("System.Double")));
            dtAnalysisOfVar.Columns.Add(new DataColumn("mix_comp", System.Type.GetType("System.Double")));
            dtAnalysisOfVar.Columns.Add(new DataColumn("correc_comp", System.Type.GetType("System.Double")));
            dtAnalysisOfVar.Columns.Add(new DataColumn("porcentage", System.Type.GetType("System.Double")));
            dtAnalysisOfVar.Columns.Add(new DataColumn("standard_error", System.Type.GetType("System.Double")));

            int nSource = this.ldesign.Count;
            for (int i = 0; i < nSource; i++)
            {
                string source = this.ldesign[i];
                // Insertamos los datos
                DataRow row = dtAnalysisOfVar.NewRow();
                row["source_of_var"] = source;
                row["df"] = this.df[source];
                row["ssq"] = this.ssq[source];
                row["msq"] = this.msq[source];
                row["random_comp"] = this.randomComp[source];
                row["mix_comp"] = this.mixComp[source];
                row["correc_comp"] = this.correcComp[source];
                row["porcentage"] = this.porcentage[source];
                row["standard_error"] = this.standardError[source];
                // Añadimos la fila
                dtAnalysisOfVar.Rows.Add(row);
            }

            // Añadimos el DataTable
            dsTbAnalysis.Tables.Add(dtAnalysisOfVar);
            return dsTbAnalysis;
        }// end TbAnalysis2DataSet


        /* Descripción:
         *  Toma los datos de un DataSet con el formato generado por esta clase y devuelve una
         *  tabla de análisis de varianza.
         */
        public static TableAnalysisOfVariance DataSet2TbAnalysisOfVar(DataSet dsAnalysis)
        {
            // Extraemos el dataTable de lista de facetas
            DataTable dtListFacets = dsAnalysis.Tables["TbFacets"];
            DataTable dtSkipLevels = dsAnalysis.Tables["TbSkipLevels"];
            ListFacets lf = MultiFacetData.ListFacets.DataTables2ListFacets(dtListFacets, dtSkipLevels);
            
            // Extraemos el DataTable con la tabla de análisis
            DataTable dtAnalysis = dsAnalysis.Tables["TbAnalysis"];
            List<string> ldesign = new List<string>();
            
            Dictionary<string, double?> ssq = new Dictionary<string,double?>();
            Dictionary<string, double> df = new Dictionary<string,double>();
            Dictionary<string, double?> msq = new Dictionary<string,double?>();
            Dictionary<string, double?> random = new Dictionary<string,double?>();
            Dictionary<string, double?> mix = new Dictionary<string,double?>();
            Dictionary<string, double?> correc = new Dictionary<string,double?>();
            Dictionary<string, double?> porc = new Dictionary<string,double?>();
            Dictionary<string, double?> stad_error = new Dictionary<string, double?>();
            
            int numRows = dtAnalysis.Rows.Count;
            for(int i=0; i< numRows; i++)
            {
                DataRow row = dtAnalysis.Rows[i];
                // Extraemos los valores
                string source = (string)row["source_of_var"];
                ldesign.Add(source);
                double v_df = (double)row["df"];
                df.Add(source, v_df);
                double? v_ssq = (double?)row["ssq"];
                ssq.Add(source, v_ssq);
                double? v_msq = (double?)row["msq"];
                msq.Add(source, v_msq);
                double? v_random_comp = (double?)row["random_comp"];
                random.Add(source, v_random_comp);
                double? v_mix_comp = (double?)row["mix_comp"];
                mix.Add(source, v_mix_comp);
                double? v_correc_comp = (double?)row["correc_comp"];
                correc.Add(source, v_correc_comp);
                double? v_porcentage = (double?)row["porcentage"];
                porc.Add(source, v_porcentage);
                double? v_standard_error = (double?)row["standard_error"];
                stad_error.Add(source, v_standard_error);
            }

            return new TableAnalysisOfVariance(lf, ldesign, ssq, df, msq, random, mix, correc, porc, stad_error);
        }// DataSet2TbAnalysisOfVar

        #endregion Conversión con DataSet

    }// end public class TableAnalysisOfVariance
} // end namespace ProjectSSQ
