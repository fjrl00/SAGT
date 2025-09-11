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
 * Fecha de revisión: 30/Ene/2012                           
 * 
 * Descripción:
 *      Libreria de suma de cuadrados
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiFacetData;
using AuxMathCalcGT;

namespace ProjectSSQ
{
    public class TableSSQ : InterfaceTableSSQ
    {
        /*=================================================================================
         * Variables de instancia
         *=================================================================================*/
        // Variables 
        private ListFacets listF;     // Lista de facetas sobre la que construimos la tabla
        private List<List<double?>> ssqMatrix; // matriz de medias.
        private int rows;             // número de filas de la tabla
        private int cols;             // número de columnas de la tabla de observaciones.
        private double? sum_sumX;     // vector suma de cuadrados
        private double? sum_sumX2;    // vector suma de cuadrados al cuadrado
        private double multiOfLevel;  // multiplicación de los niveles de la lista de facetas
        private double? variance;     // devuelve la varianza
        

       

        /*=================================================================================
         * Constructores
         *=================================================================================*/

        /*
         * Descripción:
         *  Construye una tabla de cuadrados a partir de una lista de facetas y el objeto multifaceta que
         *  contiene los datos observados.
         * Parámetros:
         *      ListFacets lF: Lista de facetas
         *      MultiFacetsObs mfo: Objeto multifaceta con los datos observados.
         * Excepciones:
         *      TableSSQException: Si no hay facetas en la lista que se pasa como parámetro, en dicha lista
         *              esta contenida alguna faceta que no pretenece la objeto de las observaciones.
         */
        public TableSSQ(ListFacets lF, MultiFacetsObs mfo)
        {
            // al menos la lista debe contener un elemento
            if (lF.Count() < 1)
            {
                throw new TableSSQException("Error: no hay facetas");
            }
            // Debe comprobar que la facetas de la lista pertenecen al objeto multifaceta.
            if (!mfo.CheckMembershipOfFacets(lF))
            {
                throw new TableSSQException("Error: Las facetas no pertenecen a la tabla de observaciones");
            }


            this.listF = lF;

            // Calculmos el producto de las facetas
            this.multiOfLevel = lF.MultiOfLevel();

            this.rows = 1; // Guarda el nº de filas de la matriz de medias.

            int[] levelOfFacets = new int[lF.Count()];
            /* Este array nos ayudará a contruir la estructura en el caso de que haya mas de una faceta
             * en la lista.
             * En este array insertaremos los niveles de cada faceta.
             */

            int i = 0; // Inicializamos al valor cero. 
            // Será la dimensión del array y nuestro indice.

            int n = lF.Count();
            
            for (int j = 0; j < n; j++)
            {
                Facet f = lF.FacetInPos(j);
                levelOfFacets[i++] = f.Level();
                this.rows = this.rows * f.Level();
            }


            this.cols = lF.Count() + 2; // nº de columnas de la matriz de Suma de cuadrados

            // creamos la matriz
            this.ssqMatrix = new List<List<double?>>(); 
            
            // Inicializamos la matriz
            for (int t = 0; t < this.rows; t++)
            {
                this.ssqMatrix.Add(new List<double?>());
                for (int j = 0; j < this.cols; j++)
                {
                    this.ssqMatrix[t].Add(null);
                }
            }

            // inicializamos las columnas de indices
            int[] repIndexs = RepeatedIndex(levelOfFacets);
            // rellenamos la tabla con los indices
            this.IniIndexSubTable(levelOfFacets, repIndexs, this.rows);

            // Calculamos frecuencias, cuadrados y suma de cuadrados
            this.CalculateTableSSQ(mfo);

            this.sum_sumX = this.Calc_Sum_sumX();
            this.sum_sumX2 = this.Calc_Sum_sumX2();
        }


        #region Operaciones auxiliares del constructor
        /*=================================================================================
         * Operaciones auxiliares del constructor:
         *      - RepeatedIndex --> array de indices 
         *      - IniIndexSubTable --> rellena la tabla con los indices
         *      - Calc_Sum_sumX --> Devuelve la suma de todos los cuadrados
         *      - Calc_Sum_sumX2 --> Devuelve la suma de todos los cuadrados al cuadrado
         *=================================================================================*/

        /*
         * Descripción:
         *  Método auxiliar que devuelve un array con las veces que se repite cada uno 
         *  de los indices de una columna antes de pasar al siguiente indice. Su utilidad 
         *  es facilitar la contrucción de la tabla de suma de cuadrados.
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
         *      int[] rep: Array de enteros con el numero de veces que se repite el indice en 
         *                  esa columna.
         *      int rows: Número de columnas que tiene el array bidimensional:     
         */
        private void IniIndexSubTable(int[] levelOfFacets, int[] rep, int rows)
        {
            int anchura = levelOfFacets.Length;

            for (int columna = 0; columna < anchura; columna++)
            { // * for 1 *
                int indice = 1;
                int numRep = 0;
                for (int fila = 0; fila < rows; fila++)
                { // * for 2*

                    this.ssqMatrix[fila][columna] = indice;
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
        }// end private void IniIndexSubTable(double[] levelOfFacets, double[] rep,int rows)


        /*
        * Descripción:
        *  Devuelve la suma de todos los cuadrados
        */
        private double? Calc_Sum_sumX()
        {
            double? retVal = 0;
            double? n = 0;
            for (int i = 0; i < this.rows; i++)
            {
                n = this.SumXData(i);
                if (n != null)
                {
                    retVal += n;
                }
            }
            if (n == null)
            {
                retVal = null;
            }
            return retVal;
        }


        /*
         * Descripción:
         *  Devuelve la suma de todos los cuadrados al cuadrado
         */
        private double? Calc_Sum_sumX2()
        {
            double? retVal = 0;
            double? n = 0;
            for (int i = 0; i < this.rows; i++)
            {
                n = this.SumX2Data(i);
                if (n != null)
                {
                    retVal += n;
                }
            }
            if (n == null)
            {
                retVal = null;
            }
            return retVal;
        }

        #endregion Operaciones auxiliares del constructor


        /*
         * Descripción:
         *  Recorre la tabla comparando los indices y cuando encuentra el correcto calcula los
         *  datos estadisticos (el cuadrado y la suma de cuadrados y la varianza).
         * Parámetros:
         *  ListFacets lf: lista de facetas, contiene las facetas sobre la que queremos calcular 
         *              la media y otros datos
         *  MultiFacetsObs mfo: es el objeto multifaceta que contiene la tabla de datos.
         */
        private void CalculateTableSSQ(MultiFacetsObs mfo)
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

            // Ahora usaremos los valores de la tabla de medias para recorrer la tabla de 
            // observaciones y obtener los datos
            for (int i = 0; i < this.rows; i++)
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
                        stc.Add(mfo_obsTable.Data(j));
                    }
                }

                double? x =stc.SumX();
                // ahora asignamos la suma de X
                this.SumXData(x, i);

                // asignamos la suma al cuadrado de X
                if (x != null)
                {
                    this.SumX2Data(Math.Pow((double)x, 2.0), i);
                }

                this.variance = stc.Variance();
            }
        } // end private void CalculateTableSSQ(MultiFacetsObs mfo)


        #region Métodos de consulta
        /*=================================================================================
         * Métodos de Consulta
         *=================================================================================*/

        /*
         * Descripción:
         *  Devuelve la lista de facetas sobre la que se construye la tabla.
         */
        public ListFacets ListFacet()
        {
            return listF;
        }


        /*
         * Descripción:
         *  Devuelve el valor de la suma de cuadrados
         */
        public double? Sum_sumX()
        {
            return this.sum_sumX;
        }


        /*
         * Descripción:
         *  Devuelve el valor de la suma de cuadrados al cuadrado.
         */
        public double? Sum_sumX2()
        {
            return this.sum_sumX2;
        }


        /*
         * Descripción:
         *  Devuelve la varianza.
         */
        public double? Variance()
        {
            return variance;
        }


        /*
         * Descripción:
         *  Consulta el valor de un elemento de la tabla y lo devuelve
         * Parámetros:
         *      int row: fila de la que se obtiene el dato.
         *      int col: Columna de la que se obtiene el dato.
         * Excepciones:
         *      Lanza una excepción TableSSQException si alguno de los valores que se pasa como
         *      argumento tienen el indice fuera del rango de las dimensiones de la tabla.
         */
        public double? Data(int row, int col)
        {
            if (row < 0 || col < 0 || row > (this.rows - 1) || col > (this.cols - 1))
            {
                throw new TableSSQException("Indice fuera de rango, posición no encontrada");
            }

            return this.ssqMatrix[row][col];
        }


        /*
         * Descripción:
         *  Devuelve la suma de cuadrados contenido en la penúltima columna en la fila que se pasa
         *  como parametros.
         * Parámetro:
         *      int row: es la posición (fila) de donde vamos a leer la suma de cuadrados que siempre 
         *              estará en la penúltima columna columna de datos.
         * Excepciones:
         *  Lanza una excepción TableSSQException si esta dentro del rango de filas de la tabla
         *  el parámetro de entrada.
         */
        public double? SumXData(int row)
        {
            if (row < 0 || row > this.rows - 1)
            {
                throw new TableSSQException("La fila no petenece al rango de columnas de la tabla.");
            }
            // variable de retorno
            double? res = this.ssqMatrix[row][this.cols - 2];

            return res;
        }


        /*
         * Descripción:
         *  Devuelve el cuadrado de la suma de cuadrados contenida en la última columna en la 
         *  fila que se pasa como parámetros.
         * Parámetro:
         *      int row: es la posición (fila) de donde vamos a leer la media que siempre 
         *              estará en la antepenúltima columna columna de datos.
         * Excepciones:
         *  Lanza una excepción TableSSQException si esta dentro del rango de filas de la tabla
         *  el parámetro de entrada.
         */
        public double? SumX2Data(int row)
        {
            if (row < 0 || row > this.rows - 1)
            {
                throw new TableSSQException("La fila no petenece al rango de columnas de la tabla.");
            }
            // variable de retorno
            double? res = this.ssqMatrix[row][this.cols - 1];

            return res;
        }


        #endregion Métodos de consulta


        #region Métodos de instancia
        /*=================================================================================
         * Métodos de instancia
         *=================================================================================*/
        /*
         * Despcrición:
         *  Introduce el valor en la posición que se pasa como parámetro.
         *  Los datos se encuentran en la última columna de la tabla, por lo que el 
         *  parámetro columna esta fijo.
         * Parámetros:
         *      double? data: dato que se va a insertar en la última columna de la tabla.
         *      int pos: posción (fila) en la que vamos a insertar la media. La columna de datos 
         *              es fija y es la antepenúltima.
         * Excepciones:
         *      TableSSQException: En el caso de que la posición de inserción no coincida con el 
         *              rango de filas.
         */
        public void SumXData(double? data, int pos)
        {
            if (pos < 0 || pos >= this.rows)
            {
                throw new TableSSQException("La posición de inserción en la tabla de cuadrados se encuentra fuera del rango");
            }
            this.ssqMatrix[pos][cols - 2] = data;
        }


        /*
         * Despcrición:
         *  Introduce la desviación tipica en la tabla en la posición que se pasa como parámetro.
         *  Los datos se encuentran en la última columna de la tabla, por lo que el 
         *  parámetro columna esta fijo.
         * Parámetros:
         *      double? data: dato que se va a insertar en la última columna de la tabla.
         *      int pos: posción (fila) en la que vamos a insertar la varianza. La columna de datos 
         *              es fija y es la penúltima.
         * Excepciones:
         *      TableSSQException: En el caso de que la posición de inserción no coincida con el 
         *              rango de filas.
         */
        public void SumX2Data(double? data, int pos)
        {
            if (pos < 0 || pos >= this.rows)
            {
                throw new TableSSQException("La posición de inserción en la tabla de cuadrados se encuentra fuera del rango");
            }
            this.ssqMatrix[pos][cols - 1] = data;
        }

        #endregion Métodos de instancia


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
            for (int i = 0; i < this.rows; i++)
            {/*for 1*/
                for (int j = 0; j < this.cols; j++)
                {/*for 2*/
                    if (this.ssqMatrix[i][j] == null)
                    {
                        res.Append("-");
                    }
                    else
                    {
                        res.Append(this.ssqMatrix[i][j].ToString() + " ");
                    }
                }/*end for 2*/
                res.Append("\n");
            }/*end for 1*/
            res.Append("\n\nSuma_X= " + this.sum_sumX.ToString()+"\nSuma_X2= "+ this.sum_sumX2.ToString());
            return res.ToString();
        }

        #endregion Métodos redefinidos


    }// end public class TableSSQ
} // end namespace ProjectSSQ
