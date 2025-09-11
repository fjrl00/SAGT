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
 * Fecha de revisión: 31/Ene/2012                           
 * 
 * Descripción:
 *      Tabla de medias de diferenciación.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using MultiFacetData;
using AuxMathCalcGT;

namespace ProjectMeans
{

    public class TableMeansDif : InterfaceTableMeans
    {
        /*
         * Descripción:
         *  Representa la tabla de medias de diferencias. Basicamente es lo mismo que la tabla de medias 
         *  normal (La tabla de medias normal contiene además de la columna de medias, la columna de varianza
         *  y la columna de desviación típica). La tabla de medias de diferencias contiene además tres nuevas
         *  columnas:
         *      - Columna de diferencia de medias, expresada como la diferencia entre la media de esa fila 
         *        y la gran media de la tabla.
         *      - Columna de diferencia de varianzas, expresada como la diferencia entre la varianza  de esa
         *        fila y la varianza de la tabla.
         *      - Columna de diferencia entre desviaciones típicas, expresada como la diferencia de esa fila
         *        y la desviación típica de esa tabla.
         *        
         *  Los datos estan en las 6 últimas columnas, las n-7 columnas restantes representan el nivel de las 
         *  respectivas facetas. La columna n-6 representa la media, la columna n-5 la varianza, la n-4 la 
         *  desviación típica, la n-3 la diferencia de medias, la n-2 la diferencia entre varianzas y la
         *  n-1 la diferencia entre desviaciones típicas.
         * 
         *      
         *      Se representa como:
         *                                                              (Pos:n-3)   (Pos:n-2)   
         *                                (Pos:             (Pos:n-4)  Diferencia  Diferencia   Diferencia 
         *      (Pos:0)         (Pos:n-7) n-6)   (Pos:n-5) Desviación      de        entre      entre desv.
         *      Faceta1  (...)  Faceta_m  media  Varianza    típica      medias     varianzas     típica
         *      =======  =====  ========  =====  ========  ==========  ==========  ===========  ===========
         *         1       1        1       x1      v1       dt1          dm1         dv1          ddt1
         *         1       1        2       x2      v2       dt2          dm2         dv2          ddt2
         *         1       2        3       x3      v3       dt3          dm3         dv3          ddt3
         *       (...)   (...)    (...)   (...)   (...)     (...)        (...)       (...)         (...)
         *      
         *      Los datos puede representarese en una matriz. El número de filas se obtine de la 
         *      multiplicación los niveles de todas las facetas (en nuestro ejemplo: 2x3x2 = 12 
         *      filas). El número de columnas es igual al número de facetas más 6.
         *      
         *      Para representar los datos se usará un array bidimensional o matriz de tipo double.
         */

        /*=================================================================================
         * Constantes
         *=================================================================================*/
        internal const string BEGIN_TABLE_MEANS_DIFF = "<table_means_diff>";
        const string END_TABLE_MEANS_DIFF  = "</table_means_diff>";
        const string BEGIN_LIST_OF_DATAMEANS_DIFF  = "<list_of_data_means_diff>";
        const string END_LIST_OF_DATAMEANS_DIFF  = "</list_of_data_means_diff>";
        const string STRING_NULL = "NULL";

        /*=================================================================================
         * Variables de instancia
         *=================================================================================*/
        // Variables 
        private List<List<double?>> meansMatrix; // matriz de medias.
        private ListFacets listF; // lista de facetas sobre la que construiremos la tabla de medias
        private double? grandMean; // Gran media o media general
        private double? variance; // Varianza
        private double? stdDev; // Desviación típica
        private string facetDesign; // Diseño de las facetas, el texto que aparece en la pestaña


        #region Constructores de TableMeansDif
        /*==============================================================================================
         * Constructores
         *==============================================================================================*/

        public TableMeansDif()
        {
            listF = new ListFacets();
            meansMatrix = new List<List<double?>>();
        }

        public TableMeansDif(ListFacets lf, string design, int rows)
        {
            this.listF = lf;
            this.facetDesign = design;
            int cols = lf.Count() + 6; // el número de columnas exptras para los datos
            /* +6 ya que tenemos que sumar las columnas de:
             * - Media
             * - Varianza
             * - Desviación típica
             * - Diferencia de medias
             * - Diferencia de varianzas
             * - Diferencia de desviación típica
             */
            this.meansMatrix = new List<List<double?>>();
            for (int i = 0; i < rows; i++)
            {
                this.meansMatrix.Add(new List<double?>());
                for (int j = 0; j < cols; j++)
                {
                    this.meansMatrix[i].Add(null);
                }
            }
        }


        /* Descripción:
         *  Contructor de la clase
         * Parámetros:
         *      ListFacets lF: Lista de facetas
         *      string design: Texto que identifica la tabla de medias
         *      MultiFacetsObs mfo: Objeto multifaceta con la tabla de frecuencias
         *      bool zero: Si es true se realizarán los calculos interpretando los valores 
         *              nulos como ceros
         */
        public TableMeansDif(ListFacets lF, string design, MultiFacetsObs mfo,bool zero)
        {
            if (lF.Count() < 1)
            {
                throw new ObsTableException("Error: no hay facetas");
            }

            this.listF = lF;
            this.facetDesign = design;

            int rows = 1; // Guarda el nº de filas de la matriz de medias.

            int[] levelOfFacets = new int[lF.Count()];
            /* Este array nos ayudará a contruir la estructura en el caso de que haya mas de una faceta
             * en la lista.
             * En este array insertaremos los niveles de cada faceta.
             */

            int i = 0; // Inicializamos al valor cero. 
            // Será la dimensión del array y nuestro indice.

            foreach (Facet f in lF)
            {
                levelOfFacets[i++] = f.Level();
                rows = rows * f.Level();
            }

            int cols = lF.Count() + 6; // nº de columnas de la matriz de observaciones

            // creamos la matriz
            this.meansMatrix = new List<List<double?>>();

            // inicializamos las columnas de indices
            int[] repIndexs = RepeatedIndex(levelOfFacets);
            // rellenamos la tabla con los indices
            this.IniIndexSubTable(levelOfFacets, repIndexs, rows, cols);

            // eliminamos las filas omitidas
            if (this.listF.HasSkipLevels())
            {
                this.SkipLevel(this.listF);
            }

            // Calculamos las medias
            this.CalculateMeans(mfo, zero);

            // Calculamos la gran media
            this.Calc_GrandMean_Variance_StdDev(mfo, zero);

            // Añadimos los datos de las tres últimas columnas que se calculan a partir de los ya calculados.
            this.CalcColumsOfDifference();
        }// end public TableMeansDif(ListFacets lF, MultiFacetsObs mfo)


        public TableMeansDif(ListFacets lf, string design, double? grandMean, double? variance, double? std_dev, 
            List<double?> listMeans, List<double?> listVariances, List<double?> listStd_dev,
            List<double?> listDifMeans, List<double?> listDifVariances, List<double?> listDifStd_dev)
        {
            if (lf.Count() < 1)
            {
                throw new ObsTableException("Error: no hay facetas");
            }
            this.listF = lf;
            this.facetDesign = design;
            this.grandMean = grandMean;
            this.variance = variance;
            this.stdDev = std_dev;

            int rows = 1; // Guarda el nº de filas de la matriz de medias.

            int[] levelOfFacets = new int[lf.Count()];
            /* Este array nos ayudará a contruir la estructura en el caso de que haya mas de una faceta
             * en la lista.
             * En este array insertaremos los niveles de cada faceta.
             */

            int i = 0; // Inicializamos al valor cero. 
            // Será la dimensión del array y nuestro indice.

            foreach (Facet f in lf)
            {
                levelOfFacets[i++] = f.Level();
                rows = rows * f.Level();
            }

            int cols = lf.Count() + 6; // nº de columnas de la matriz de observaciones

            // creamos la matriz
            this.meansMatrix = new List<List<double?>>();

            // inicializamos las columnas de indices
            int[] repIndexs = RepeatedIndex(levelOfFacets);
            // rellenamos la tabla con los indices
            this.IniIndexSubTable(levelOfFacets, repIndexs, rows, cols);

            // Ahora introducimos los valores en la tabla
            if (!(listMeans.Count == listStd_dev.Count && listMeans.Count == listVariances.Count
                && listDifMeans.Count == listDifMeans.Count && listMeans.Count == listDifVariances.Count
                && listMeans.Count == listDifStd_dev.Count))
            {
                throw new ObsTableException("Error: en la cardinalidad de los datos de las medias");
            }

            int n = listMeans.Count;
            for (int j = 0; j < n; j++)
            {
                // sustituimos el valor i por el j
                meansMatrix[j][cols - 6] = listMeans[j];
                meansMatrix[j][cols - 5] = listVariances[j];
                meansMatrix[j][cols - 4] = listStd_dev[j];
                meansMatrix[j][cols - 3] = listDifMeans[j];
                meansMatrix[j][cols - 2] = listDifVariances[j];
                meansMatrix[j][cols - 1] = listDifStd_dev[j];
            }
        }// end public TableMeansDif


        public TableMeansDif(ListFacets lf, string design, double? grandMean, double? variance, double? std_dev,
            List<List<double?>> meansMatrix)
        {
            if (lf.Count() < 1)
            {
                throw new ObsTableException("Error: no hay facetas");
            }
            this.listF = lf;
            this.facetDesign = design;
            this.grandMean = grandMean;
            this.variance = variance;
            this.stdDev = std_dev;
            this.meansMatrix = meansMatrix;

        }// end public TableMeansDif


        public TableMeansDif(DataTable dt, double? grandMean, double? variance, double? stdDev, string facetDesign)
            :this()
        {
            this.listF = new ListFacets();
            int r = dt.Rows.Count;
            int c = dt.Columns.Count;

            // Inicializamos la estructura
            for (int i = 0; i < r; i++)
            {
                List<double?> ld = new List<double?>();
                this.meansMatrix.Add(ld);
            }

            // Rellenamos los datos columna a columna
            for (int j = 0; j < c; j++)
            {
                int level = 1;
                
                for (int i = 0; i < r; i++)
                {
                    List<double?> ld = this.meansMatrix[i];
                    double? d = null;
                    DataRow row = dt.Rows[i];
                    object o = row[j];

                    if (o != null)
                    {
                        d = ConvertNum.String2Double((string)o.ToString());
                        if (d != null && j < c - 6)
                        {
                            level = Convert.ToInt32(Math.Max(level, (double)d));
                        }
                    }
                    
                    ld.Add(d);
                }

                if (j < c - 6)
                {
                    string nameFacet = dt.Columns[j].ColumnName;
                    Facet f = new Facet(nameFacet, level, "");
                    this.listF.Add(f);
                }
            }

            this.grandMean = grandMean;
            this.variance = variance;
            this.stdDev = stdDev;
            this.facetDesign = facetDesign;
        }

        #endregion Constructores de TableMeansDif


        #region Operaciones auxiliares del constructor
        /*=================================================================================
         * Operaciones auxiliares del constructor:
         *      - RepeatedIndex --> array de indices 
         *      - IniIndexSubTable --> Inicializa la tabla con los indices
         *      - SkipLevel --> Elimina las filas con niveles omitidos
         *=================================================================================*/

        /*
         * Descripción:
         *  Método auxiliar que devuelve un array con las veces que se repite cada uno 
         *  de los indices de una columna antes de pasar al siguiente indice. Su utilidad 
         *  es facilitar la contrucción de la tabla de medias.
         * 
         * Entradas:
         *      int[] levelOfFacets: un array con los niveles de cada una de las facetas.
         *      
         * Devuelve:
         *      double[] res: un array con las veces que se repite cada uno de los indices.
         */
        private static int[] RepeatedIndex(int[] levelOfFacets)
        {
            // Necesitamos saber la longitud de vector para crear el nuevo vector
            int sizeVector = levelOfFacets.Length;

            // Variable de retorno
            int[] res = new int[sizeVector];
            res[sizeVector - 1] = 1;

            for (int i = sizeVector - 1; i > 0; i--)
            {
                res[i - 1] = res[i] * levelOfFacets[i];

            }
            return res;
        }// end private static double[] RepeatedIndex(double[] levelOfFacets)


        /**
         * Descripción:
         *  Rellena las n-2 primeras columnas de la tabla con los indices que se deducen a
         *  partir de los niveles de las facetas.
         * Entrada:
         *      int[] levelOfFacets: Array de enteros con el nivel de cada una de las facetas.
         *      int[] rep: Array de enteros con el número de veces que se repite el indice en 
         *                  esa columna.
         *      int rows: Número de columnas que tiene el array bidimensional:     
         */
        private void IniIndexSubTable(int[] levelOfFacets, int[] rep, int rows, int cols)
        {
            int anchura = levelOfFacets.Length;

            for (int i = 0; i < rows; i++)
            {
                this.meansMatrix.Add(new List<double?>());
                for (int j = 0; j < cols; j++)
                {
                    this.meansMatrix[i].Add(null);
                }
            }

            for (int columna = 0; columna < anchura; columna++)
            { // * for 1 *
                int indice = 1;
                int numRep = 0;
                for (int fila = 0; fila < rows; fila++)
                { // * for 2*

                    this.meansMatrix[fila][columna] = indice;
                    numRep++;
                    if (numRep == rep[columna])
                    {
                        indice++;
                        numRep = 0;
                    }
                    if (indice > levelOfFacets[columna])
                    {
                        indice = 1;
                        numRep = 0;
                    }

                } // * for 2 *
            } // * for 1 *
        }// end private void IniIndexSubTable


        /* Descripción:
         *  Elimina de la tabla la filas donde haya algún indice omitido
         */
        private void SkipLevel(ListFacets lf)
        {
            int rows = this.MeansTableRows();
            int numFacets = lf.Count();
            ArrayList arrayL = new ArrayList();

            for (int i = 0; i < rows; i++)
            {
                List<double?> lrow = this.meansMatrix[i];
                bool skip = false;
                for (int j = 0; j < numFacets && !skip; j++)
                {
                    Facet f = lf.FacetInPos(j);
                    int data = (int)lrow[j];// conversión explicita
                    skip = f.GetSkipLevels(data);
                    if (skip)
                    {
                        arrayL.Add(lrow);
                    }
                }
            }

            int n = arrayL.Count;
            for (int i = 0; i < n; i++)
            {
                this.meansMatrix.Remove((List<double?>)arrayL[i]);
            }
        }// end SkipLevel

        #endregion Operaciones auxiliares del constructor

        /*
         * Descripción:
         *  Recorre la tabla comparando los indices y cuando encuentra el correcto calcula los
         *  datos estadisticos (media, varianza desviación típica).
         * Parámetros:
         *  ListFacets lf: lista de facetas, contiene las facetas sobre la que queremos calcular 
         *              la media y otros datos
         *  MultiFacetsObs mfo: es el objeto multifaceta que contiene la tabla de datos.
         *  bool zero: true si se realizarán los calculos de la media interpretando los valores 
         *          null como ceros.
         */
        private void CalculateMeans(MultiFacetsObs mfo, bool zero)
        {
            /* determinamos el número de facetas de la lista que pasamos como parámetro
             * estas se corresponden con las columnas que necesitamos comparar para calcular los
             * datos estadisticos*/
            int longListFacet = this.listF.Count();

            // este array determinna las columnas de la tabla a comparar
            int[] posicionesEnColumnas = new int[longListFacet];
            
            // obtenemos la lista de facetas del objeto mutifaceta para luego comparar y 
            // obtener las posiciones
            ListFacets mfo_listFacet = mfo.ListFacets();

            // Rellenamos el array
            for (int i = 0; i < longListFacet; i++)
            {
                posicionesEnColumnas[i] = mfo_listFacet.IndexOf(this.listF.FacetInPos(i));
            }

            // necesito la tabla de observaciones
            InterfaceObsTable mfo_obsTable = mfo.ObservationTable();
            // necesito el número de filas de la tabla de observaciones para el bucle interior
            int num_rows_obsTable = mfo_obsTable.ObsTableRows();

            int rows = this.MeansTableRows();

            // Ahora usaremos los valores de la tabla de medias para recorrer la tabla de 
            // observaciones y obtener los datos
            for (int i = 0; i < rows; i++)
            {
                // Creamos el elemento stadistica que contendra las sumas
                Statistics stc = new Statistics();

                for (int j = 0; j < num_rows_obsTable; j++)
                {
                    bool v = true;
                    /* Ahora comparo los valores de los indices de la tabla de medias con sus
                     * corespondecia en la tabla de observaciones si coincide lo agrego a la 
                     * tabla de estadistica. */

                    
                    for (int k = 0; k < longListFacet && v; k++)
                    {
                        /* Comparo el valor de laa columnas de indices en la tabla de medias con 
                         * los indices de la tabla de datos. Las columnas de la tabla de datos
                         * las he guardado en el arrary posicionesEnColumnas. */
                        v = (this.Data(i, k).Equals(mfo_obsTable.Data(j, posicionesEnColumnas[k])));
                    }

                    if (v)
                    {
                        stc.Add(mfo_obsTable.Data(j), zero);
                    }
                }

                // ahora asignamos la media
                this.MeanData(stc.Mean(),i);
                
                // asignamos la varianza
                this.VarianceData(stc.Variance(), i);

                // asignamos la desviación tipica
                this.Std_dev_Data(stc.StandardDeviation(),i);
            }
        }// end CalculateMeans


        /* Descripción:
         *  Devuelve la Gran media o media general.
         *  
         *  Parámetros
         *      MultiFacetsObs mfo: Para obtener los datos de la tabla de frecuencias de los que se optiene la grand media
         *      bool zero: true si se quiere realizar los calculos interpretando los valores nulos
         *              como ceros.
         */
        public void Calc_GrandMean_Variance_StdDev(MultiFacetsObs mfo, bool zero)
        {
            // Creamos el elemento stadistica que contendra las sumas
            Statistics stc = new Statistics();
            InterfaceObsTable observationTable = mfo.ObservationTable();
            // int r = this.MeansTableRows();
            // int c = (this.MeansTableColumns()-6);
            int r = observationTable.ObsTableRows();
            for (int i = 0; i < r; i++)
            {
                // stc.Add(this.Data(i, c), zero);
                stc.Add(observationTable.Data(i), zero);
            }
            this.grandMean = stc.Mean();
            this.variance = stc.Variance();
            this.stdDev = stc.StandardDeviation();
        }

        #region Métodos de consulta
        /*=================================================================================
         * Métodos de Consulta
         *=================================================================================*/

        /*
         * Descripción:
         *  Consulta el valor de un elemento de la tabla y lo devuelve
         * Parámetros:
         *      int row: fila de la que se obtiene el dato.
         *      int col: Columna de la que se obtiene el dato.
         * Excepciones:
         *      Lanza una excepción TableMeansException si alguno de los valores que se pasa como
         *      argumento tienen el indice fuera del rango de las dimensiones de la tabla.
         */
        public double? Data(int row, int col)
        {
            if (row < 0 || col < 0 || row > (this.MeansTableRows() - 1) || col > (this.MeansTableColumns() - 1))
            {
                throw new TableMeansDifException("Indice fuera de rango, posición no encontrada");
            }

            return this.meansMatrix[row][col];
        }

        /*
         * Descripción:
         *  Devuelve la media contenido en la antepenúltima columna en la fila que se pasa
         *  como parámetros.
         * Parámetro:
         *      int row: es la posición (fila) de donde vamos a leer la media que siempre 
         *              estará en la antepenúltima columna columna de datos.
         * Excepciones:
         *  Lanza una excepción TableMeansException si esta dentro del rango de filas de la tabla
         *  el parámetro de entrada.
         */
        public double? MeanData(int row)
        {
            if (row < 0 || row > this.MeansTableRows() - 1)
            {
                throw new TableMeansDifException("La fila no petenece al rango de columnas de la tabla.");
            }
            // variable de retorno
            double? res = this.meansMatrix[row][this.MeansTableColumns() - 6];

            return res;
        }

        /*
         * Descripción:
         *  Devuelve la varianza contenido en la penúltima columna en la fila que se pasa
         *  como parametros.
         * Parámetro:
         *      int row: es la posición (fila) de donde vamos a leer la varianza que siempre 
         *              estará en la penúltima columna columna de datos.
         * Excepciones:
         *  Lanza una excepción TableMeansException si esta dentro del rango de filas de la tabla
         *  el parámetro de entrada.
         */
        public double? VarianceData(int row)
        {
            if (row < 0 || row > this.MeansTableRows() - 1)
            {
                throw new TableMeansDifException("La fila no petenece al rango de columnas de la tabla.");
            }
            // variable de retorno
            double? res = this.meansMatrix[row][this.MeansTableColumns() - 5];

            return res;
        }

        /*
         * Descripción:
         *  Devuelve la desviación típica contenida en la última columna en la fila que se pasa
         *  como parametros.
         * Parámetro:
         *      int row: es la posición (fila) de donde vamos a leer la media que siempre 
         *              estará en la antepenúltima columna columna de datos.
         * Excepciones:
         *  Lanza una excepción TableMeansException si esta dentro del rango de filas de la tabla
         *  el parámetro de entrada.
         */
        public double? Std_dev_Data(int row)
        {
            if (row < 0 || row > this.MeansTableRows() - 1)
            {
                throw new TableMeansDifException("La fila no petenece al rango de columnas de la tabla.");
            }
            // variable de retorno
            double? res = this.meansMatrix[row][this.MeansTableColumns() - 4];

            return res;
        }


        /*
         * Descripción:
         *  Devuelve el número de columnas de la tabla de medias.
         */
        public int MeansTableColumns()
        {
            return this.meansMatrix[0].Count;
        }


        /*
         * Descripción:
         *  Devuelve el número de filas de la tabla medias.
         */
        public int MeansTableRows()
        {
            return this.meansMatrix.Count;
        }

        /*
         * Descripción:
         *  Devuelve la lista de facetas de la media
         */
        public ListFacets ListFacets()
        {
            return this.listF;
        }

        /* Descripción:
         *  Devuelve un double? que representa la Gran Media
         */
        public double? GrandMean()
        {
            return this.grandMean;
        }

        /* Descripción:
         *  Devuelve un double? que representa la varianza
         */
        public double? Variance()
        {
            return this.variance;
        }

        /* Descripción:
         *  Devuelve un double? que representa la Desviación típica
         */
        public double? StdDev()
        {
            return this.stdDev;
        }

        /* Drescripción:
         *  Devuelve un string con el diseño de las facetas de la tabla
         */
        public string FacetDesign()
        {
            return facetDesign;
        }

        #endregion Métodos de consulta

        #region Métodos de instancia
        /*=================================================================================
         * Métodos de instancia
         *=================================================================================*/

        /*
         * Despcrición:
         *  Introduce la media en la tabla en la posición que se pasa como parametro.
         *  Los datos se encuentran en la última columna de la tabla, por lo que el 
         *  parametro columna esta fijo.
         * Parametros:
         *      double? data: dato que se va a insertar en la última columna de la tabla.
         *      int pos: posción (fila) en la que vamos a insertar la media. La columna de datos 
         *              es fija y es la antepenúltima.
         * Excepciones:
         *      TableMeansException: En el caso de que la posición de inserción no coincida con el 
         *              rango de filas.
         */
        public void MeanData(double? data, int row)
        {
            if (row < 0 || row >= this.MeansTableRows())
            {
                throw new TableMeansDifException("La posición de inserción en la tabla de medias se encuentra fuera del rango");
            }
            int cols = this.MeansTableColumns();
            this.meansMatrix[row][cols - 6] = data;
        }

        /*
         * Despcrición:
         *  Introduce la desviación tipica en la tabla en la posición que se pasa como parametro.
         *  Los datos se encuentran en la última columna de la tabla, por lo que el 
         *  parametro columna esta fijo.
         * Parámetros:
         *      double? data: dato que se va a insertar en la última columna de la tabla.
         *      int pos: posción (fila) en la que vamos a insertar la varianza. La columna de datos 
         *              es fija y es la penúltima.
         * Excepciones:
         *      TableMeansException: En el caso de que la posición de inserción no coincida con el 
         *              rango de filas.
         */
        public void VarianceData(double? data, int row)
        {
            if (row < 0 || row >= this.MeansTableRows())
            {
                throw new TableMeansDifException("La posición de inserción en la tabla de medias se encuentra fuera del rango");
            }
            int cols = this.MeansTableColumns();
            this.meansMatrix[row][cols - 5] = data;
        }


        /*
         * Despcrición:
         *  Introduce un dato en la tabla en la posición que se pasa como parámetro.
         *  Los datos se encuentran en la última columna de la tabla, por lo que el 
         *  parámetro columna esta fijo.
         * Parámetros:
         *      double? data: dato que se va a insertar en la última columna de la tabla.
         *      int pos: posción (fila) en la que vamos a insertar la desviación tipica. 
         *              La columna de desviación tipica es fija y es la última.
         * Excepciones:
         *      TableMeansException: En el caso de que la posición de inserción no coincida con el 
         *              rango de filas.
         */
        public void Std_dev_Data(double? data, int row)
        {
            if (row < 0 || row >= this.MeansTableRows())
            {
                throw new TableMeansDifException("La posición de inserción en la tabla de medias se encuentra fuera del rango");
            }
            int cols = this.MeansTableColumns();
            this.meansMatrix[row][cols - 4] = data;
        }


        /* Descripción:
         *  Inserta un dato en la posición indicada en los parámetros.
         * Parámetros:
         *      double? data: Dato que queremos insertar.
         *      int row: Fila donde se insetará el dato.
         *      int col: Columna donde se insetará el dato.
         * Excepciones:
         *      TableMeansException: En el caso de que la posición de inserción no coincida con el 
         *              rango de filas.
         */
        public void InsertDataInPos(double? data, int row, int col)
        {
            if ((row < 0 || row >= this.MeansTableRows()) || (col < 0 || col >= this.MeansTableColumns()))
            {
                throw new TableMeansDifException("La posición de inserción en la tabla de medias se encuentra fuera del rango");
            }
            this.meansMatrix[row][col] = data;
        }


        /* Descripción:
         *  Asigna un double? que representa la Gran Media a la variable grandMean
         */
        public void GrandMean(double? d)
        {
            this.grandMean = d;
        }

        /* Descripción:
         *  Asigna un double? que representa la varianza de la tabla.
         */
        public void Variance(double? d)
        {
            this.variance = d;
        }

        /* Descripción:
         *  Asigna un double? que representa la desviación estandar de la tabla.
         */
        public void StdDev(double? d)
        {
            this.stdDev = d;
        }


        /* Descripción:
         *  Calcula la diferencia de la media, la diferencia de varianza y la diferencia de la desviación 
         *  típica de la tabla.
         */
        private void CalcColumsOfDifference()
        {
            int row = this.MeansTableRows();
            int col = this.MeansTableColumns();
            for (int i = 0; i < row; i++)
            {
                this.meansMatrix[i][col - 3] = (Data(i, col - 6) - this.grandMean);
                this.meansMatrix[i][col - 2] = (Data(i, col - 5) - this.variance);
                this.meansMatrix[i][col - 1] = (Data(i, col - 4) - this.stdDev);
            }
        }

        #endregion Métodos de instancia


        /* Descripción:
         *  Escribe en el StreamWriter 
         */
        public bool WritingStreamTableMeans(StreamWriter writerFile)
        {
            bool res = false; // variable de retorno

            writerFile.WriteLine(BEGIN_TABLE_MEANS_DIFF);
            // Escribimos la lista de facetas
            ListFacets lf = this.ListFacets();
            res = lf.WritingStreamListFacets(writerFile);
            writerFile.WriteLine(this.facetDesign);

            // Escribimos los datos de la tabla (media, varianza y desviación típica)
            if (res)
            {
                res = this.WritingStreamListDataMeans(writerFile);
            }
            // escribimos Gran Media, varianza y desviación típica.
            writerFile.WriteLine(ConvertNum.Double2String(this.GrandMean()));
            writerFile.WriteLine(ConvertNum.Double2String(this.Variance()));
            writerFile.WriteLine(ConvertNum.Double2String(this.StdDev()));
            // escribimos el fin
            writerFile.WriteLine(END_TABLE_MEANS_DIFF);
            return res;
        }

       /* Descripción:
        *  Escribe la lista de datos de media.
        */
        public bool WritingStreamListDataMeans(StreamWriter writerFile)
        {
            bool res = false; // variable de retorno
            writerFile.WriteLine(BEGIN_LIST_OF_DATAMEANS_DIFF);
            // Escribimos los datos
            int row = this.MeansTableRows();

            for (int i = 0; i < row; i++)
            {
                List<double?> row_data = meansMatrix[i];
                int col = row_data.Count;
                string line = "";
                for (int j = 0; j < col; j++)
                {
                    line = line + ConvertNum.Double2String(row_data[j]) + " ";
                }
                writerFile.WriteLine(line);
            }

            // ponemos el cierre
            writerFile.WriteLine(END_LIST_OF_DATAMEANS_DIFF);
            res = true;
            return res;
        }

        /* Descripción:
         *  Lee los datos de una tableMeans de un stream y lo devuelve como objeto.
         * Parámetros:
         *      StreamReader reader: El stream del que vamos a leer la tabla de medias.
         */
        public static TableMeansDif ReadingStreamTableMeans(StreamReader reader)
        {
            TableMeansDif tb = null;
            try
            {
                string line;
                ListFacets lf;
                string design;
                // Leemos la lista de facetas
                if ((line = reader.ReadLine()).Equals(MultiFacetData.ListFacets.BEGIN_LISTFACETS))
                {
                    lf = MultiFacetData.ListFacets.ReadingStreamListFacets(reader);
                    design = reader.ReadLine();
                }
                else
                {
                    throw new TableMeansDifException();
                }

                // Leemos la lista de datos
                List<List<double?>> meansMatrix = new List<List<double?>>();

                if ((line = reader.ReadLine()).Equals(BEGIN_LIST_OF_DATAMEANS_DIFF))
                {
                    char[] delimeterChars = { ' ' }; // nuestro delimitador será el caracter blanco

                    while (!(line = reader.ReadLine()).Equals(END_LIST_OF_DATAMEANS_DIFF))
                    {
                        string[] arrayOfDouble = line.Trim().Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);

                        List<double?> row_data = new List<double?>();
                        int numData = arrayOfDouble.Length;
                        for (int i = 0; i < numData; i++)
                        {
                            row_data.Add(ConvertNum.String2Double(arrayOfDouble[i]));
                        }
                        meansMatrix.Add(row_data);
                    }

                    double? gm = ConvertNum.String2Double(reader.ReadLine());
                    double? v = ConvertNum.String2Double(reader.ReadLine());
                    double? stdv = ConvertNum.String2Double(reader.ReadLine());
                    // llamamos al constructor;
                    tb = new TableMeansDif(lf, design, gm, v, stdv, meansMatrix);

                    if (!(line = reader.ReadLine()).Equals(END_TABLE_MEANS_DIFF))
                    {
                        throw new TableMeansDifException("Error al leer de fichero");
                    }
                }
                else
                {
                    throw new TableMeansDifException("Error al leer de fichero");
                }

            }
            catch (FormatException)
            {
                throw new TableMeansDifException("Error en el formato de los datos");
            }
            catch (ListFacetsException)
            {
                throw new TableMeansDifException("Error al leer de fichero");
            }
            return tb;
        }// end private static TableMeans ReadingStreamTableMeans


        #region Conversión entre tabla de medias y DataSet
        /* Descripción:
         *  Convierte una tabla de medias en un DataSet
         */
        public DataSet TableMeans2DataSet()
        {
            // Creamos el  dataSet que será nuestra variable de retorno
            DataSet dsTableMeans = new DataSet("DataSet_TableMeans");
            // Creamos el dataTable con la lista de facetas
            DataTable dtListFacets = this.listF.ListFacets2DataTable("TbFacets");
            // Creamos el dataTable con los niveles omitidos
            DataTable dtSkipLevels = this.listF.SkipLevels2DataTable("TbSkipLevels");
            // añadimos los dataTable al dataSet
            dsTableMeans.Tables.Add(dtListFacets);
            dsTableMeans.Tables.Add(dtSkipLevels);

            DataTable dtTableMeans = new DataTable("TbMeans");
            // Añadimos las columnas
            dtTableMeans.Columns.Add(new DataColumn("grand_mean", System.Type.GetType("System.Double")));
            dtTableMeans.Columns.Add(new DataColumn("variance", System.Type.GetType("System.Double")));
            dtTableMeans.Columns.Add(new DataColumn("std_dev", System.Type.GetType("System.Double")));
            dtTableMeans.Columns.Add(new DataColumn("facet_design", System.Type.GetType("System.String")));
            dtTableMeans.Columns.Add(new DataColumn("type_means", System.Type.GetType("System.String")));

            // tomamos los datos
            DataRow row = dtTableMeans.NewRow();
            row["grand_mean"] = this.grandMean;
            row["variance"] = this.variance;
            row["std_dev"] = this.stdDev;
            row["facet_design"] = this.facetDesign;
            row["type_means"] = "tableMeansDif";
            dtTableMeans.Rows.Add(row);
            // añadimos el dataTable
            dsTableMeans.Tables.Add(dtTableMeans);
            DataTable dtTable = this.AuxDataTableMeans();
            dsTableMeans.Tables.Add(dtTable);

            return dsTableMeans;
        }// end TableMeans2DataSet


        /* Descripción:
         *  Operación auxiliar de TableMeans2DataSet que devuelve un dataTable con la tabla de medias
         */
        private DataTable AuxDataTableMeans()
        {
            DataTable dtTableMeans = new DataTable("Tb_Table_Means_Dif"); // valor de retorno

            int numFacet = this.listF.Count();

            for (int i = 0; i < numFacet; i++)
            {
                Facet f = this.listF.FacetInPos(i);
                string name_col = f.Name();
                dtTableMeans.Columns.Add(new DataColumn(name_col, System.Type.GetType("System.Double")));
            }
            // Añadimos la columna de las frecuencias
            dtTableMeans.Columns.Add(new DataColumn("means", System.Type.GetType("System.Double")));
            dtTableMeans.Columns.Add(new DataColumn("variance", System.Type.GetType("System.Double")));
            dtTableMeans.Columns.Add(new DataColumn("stand_dev", System.Type.GetType("System.Double")));
            dtTableMeans.Columns.Add(new DataColumn("means_dif", System.Type.GetType("System.Double")));
            dtTableMeans.Columns.Add(new DataColumn("variance_dif", System.Type.GetType("System.Double")));
            dtTableMeans.Columns.Add(new DataColumn("stand_dev_dif", System.Type.GetType("System.Double")));

            // rellenamos el dataTable
            int numRows = this.MeansTableRows();
            int numCols = this.MeansTableColumns();
            for (int i = 0; i < numRows; i++)
            {
                // Creamos una fila
                DataRow row = dtTableMeans.NewRow();
                // Rellenamos la fila
                for (int j = 0; j < numCols; j++)
                {
                    string name_col = "means";
                    if (j == numCols - 1)
                    {
                        name_col = "stand_dev_dif";
                    }
                    else if (j == numCols - 2)
                    {
                        name_col = "variance_dif";
                    }
                    else if (j == numCols - 3)
                    {
                        name_col = "means_dif";
                    }
                    else if (j == numCols - 4)
                    {
                        name_col = "stand_dev";
                    }
                    else if (j == numCols - 5)
                    {
                        name_col = "variance";
                    }
                    else if (j == numCols - 6)
                    {
                        name_col = "means";
                    }
                    else if (j < this.listF.Count())
                    {
                        Facet f = this.listF.FacetInPos(j);
                        name_col = f.Name();
                    }
                    row[name_col] = this.Data(i, j);
                }
                // Añadimos la fila al dataTable
                dtTableMeans.Rows.Add(row);
            }

            return dtTableMeans;
        }// end AuxDataTableMeans


        /* Descripción
         *  Dado un dataSet con el formato de la clase devuelve un TableMeans
         */
        public static InterfaceTableMeans DataSet2TableMeans(DataSet dsTableMeans)
        {
            DataTable dtListFacets = dsTableMeans.Tables["TbFacets"];
            DataTable dtSkipLevels = dsTableMeans.Tables["TbSkipLevels"];
            ListFacets lf = MultiFacetData.ListFacets.DataTables2ListFacets(dtListFacets, dtSkipLevels);

            DataTable Means = dsTableMeans.Tables["TbMeans"];
            DataRow row = Means.Rows[0];
            double? grandMean = (double?)row["grand_mean"];
            double? variance = (double?)row["variance"];
            double? std_dev = (double?)row["std_dev"];
            string facet_design = (string)row["facet_design"];

            DataTable dtMeansMatix = dsTableMeans.Tables["Tb_Table_Means_dif"];

            return new TableMeansDif(dtMeansMatix, grandMean, variance, std_dev, facet_design);
        }// end DataSet2TableMeans

        #endregion Conversión entre tabla de medias y DataSet


        #region Métodos redefinidos
        /*=================================================================================
         * Métodos redefinidos
         *=================================================================================*/

        /*
         * Descripción:
         *  Redefinición de la operación ToString para la clase ObsTable.
         */
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            //string res = ""; // variable de retorno
            int rows = this.MeansTableRows();
            int cols = this.MeansTableColumns();

            for (int i = 0; i < rows; i++)
            {/*for 1*/
                for (int j = 0; j < cols; j++)
                {/*for 2*/
                    if (this.meansMatrix[i][j] == null)
                    {
                        res.Append("- ");
                    }
                    else
                    {
                        res.Append(this.meansMatrix[i][j].ToString() + " ");
                    }
                }/*end for 2*/
                res.Append("\n");
            }/*end for 1*/
            return res.ToString();
        } // public override string ToString()

        #endregion Métodos redefinidos

    }// end public class TableMeansDif
}// end namespace ProjectMeans
