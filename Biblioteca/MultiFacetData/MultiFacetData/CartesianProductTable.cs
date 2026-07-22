using AuxMathCalcGT;
using System;
using System.Collections.Generic;
using System.Data;

namespace MultiFacetData
{
    public abstract class CartesianProductTable
    {
        protected List<List<double?>> matrix;
        protected ListFacets listF; // Lista de facetas asociada a la tabla
        protected abstract Exception CPTException(string msg);

        /**
         * Descripción:
         *  Devuelve a la izquierda una tabla equivalente al producto cartesiano de
         *  las facetas en el orden que se nos dan, y a la derecha un número 
         *  arbitrario de columnas extra decidido por la clase, inicializadas a null.
         * 
         *  Por ejemplo, dándosenos la siguiente lista de facetas: 
         *  
         *      Facetas:            Niveles:
         *      ========            ========
         *      Individuos          2
         *      Observaciones       3
         *      Carateristicas      2
         *      
         *      Entonces devolvemos:
         *      
         *      Individuos  Observaciones   Características     [Columnas extra]
         *      ==========  =============   ===============     =====
         *      1           1               1                   null
         *      1           1               2                   null
         *      1           2               1                   null
         *      1           2               2                   null
         *      1           3               1                   null
         *      1           3               2                   null
         *      2           1               1                   null
         *      2           1               2                   null
         *      2           2               1                   null
         *      2           2               2                   null
         *      2           3               1                   null
         *      2           3               2                   null
         * 
         *  El producto cartesiano es de todas las facetas y todos los niveles del
         *  list_facets proveído, sin importar si están marcadas/os para omitir.
         *  
         *  Algunos hechos matemáticos a tener en cuenta:
         *      - El número de filas es el producto de los niveles de todas las facetas. En el anterior ejemplo: 2*3*2=12
         *      - número de columnas = número de facetas + número de columnas extra decidido.
         *      - El índice de la tabla en que se guardaría cierta combinación de niveles de facetas es:
         *           int index = 0;
         *           for (int i =0, i<numfacets, i++)
	     *              index += IndexRepeats[i]*(value[i] - 1);
         *        En el anterior ejemplo: (2,3,2) = 6*(2-1) + 2*(3-1) + 1*(2-1) = 11
         */

        #region Table initialization methods

        protected static List<List<double?>> IniIndexSubTable(ListFacets list_facets, int extraCols)
        {
            List<List<double?>> matrix = new List<List<double?>>();

            int numFacets = list_facets.Count();
            int cols = numFacets + extraCols;

            int[] levelOfFacets = list_facets.levelOfFacets();
            double rows = list_facets.MultOfLevels();
            int[] rep = IndexRepeats(levelOfFacets);

            for (int i = 0; i < rows; i++)
            {
                matrix.Add(new List<double?>());
                for (int j = 0; j < cols; j++)
                {
                    matrix[i].Add(null);
                }
            }

            for (int columna = 0; columna < numFacets; columna++)
            {
                int indice = 1;
                int numRep = 0;
                for (int fila = 0; fila < rows; fila++)
                {

                    matrix[fila][columna] = indice;
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

                }
            }

            return matrix;
        }

        /*
         * Descripción:
         *  Método auxiliar que devuelve un array con el número de combinaciones
         *  en que figuran los valores de cada faceta en un producto cartesiano
         *  entre ellas.
         * 
         * Entradas:
         *      int[] levelOfFacets: un array con los niveles de cada una de las facetas.
         *      
         * Devuelve:
         *      int[] res: un array con las veces que se repite cada uno de los indices.
         */
        public static int[] IndexRepeats(int[] levelOfFacets)
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
        }

        #endregion Table initialization methods

        #region SkipLevels methods

        /* Descripción:
         *  Elimina de la tabla todas las filas que contengan un nivel
         *  marcado para ser omitido en la lista de facetas correspondiente.
         *  No se requiere restaurar los índices de los niveles superiores.
         *  
         *  Parámetros:
         *          ListFacets lf: Lista de facetas que contiene los niveles a omitir.
         */
        public void SkipLevels(ListFacets lf)
        {
            this.matrix.RemoveAll(row =>
            {
                for (int j = 0; j < lf.Count(); j++)
                {
                    Facet facet = lf.FacetInPos(j);
                    int data = (int)row[j];

                    if (facet.GetSkipLevels(data))
                        return true;
                }
                return false;
            });
        }

        /* Descripción:
         *  Restaura los índices de los niveles superiores al proveido 
         *  para que no haya huecos de por medio.
         *  
         *  Parámetros:
         *          int skipLevel: nivel buscado
         *          int col: columna en la que se va a buscar el nivel
         */
        public void RestoreIndexes(int skipLevel, int col)
        {
            //Restore appropiate indexes
            for (int i = 0; i < this.TableRows(); i++)
            {
                double data = (double)this.matrix[i][col];
                if (data > skipLevel)
                {
                    this.matrix[i][col] = data - 1;
                }
            }
        }

        /*
         * Descripción:
         *  Aumenta los niveles de las facetas de esta tabla a los nuevos niveles
         *  especificados en newListFacets (que debe contener las mismas facetas,
         *  en el mismo orden, pero con niveles mayores o iguales a los actuales).
         *  
         *  Se reconstruye la tabla de producto cartesiano al completo en base a los
         *  nuevos niveles. Las filas ya existentes conservan sus columnas extra 
         *  (dato, medias, etc.), ya que su índice de fila en la tabla nueva se 
         *  calcula directamente a partir de sus valores de facetas y los nuevos 
         *  niveles. Las filas nuevas -aquellas que involucran algún nivel de faceta 
         *  que no existía previamente- quedan con las columnas extra a null, igual 
         *  que si la tabla se acabase de inicializar.
         *  
         *  Parámetros:
         *      ListFacets newListFacets: Lista con las mismas facetas y mismo orden
         *                                 que la actual, pero con niveles aumentados.
         *  Excepciones:
         *      Lanza una CPTException si newListFacets no tiene el mismo número de
         *      facetas que la lista actual, o si algún nuevo nivel es menor que el
         *      nivel actual correspondiente.
         */
        public void IncreaseLevels(ListFacets newListFacets)
        {
            int numFacets = this.listF.Count();

            if (newListFacets.Count() != numFacets)
            {
                throw CPTException("La nueva lista de facetas no tiene el mismo número de facetas que la actual");
            }

            int[] oldLevels = this.listF.levelOfFacets();
            int[] newLevels = newListFacets.levelOfFacets();

            for (int i = 0; i < numFacets; i++)
            {
                if (newLevels[i] < oldLevels[i])
                {
                    throw CPTException("Los nuevos niveles deben ser mayores o iguales a los niveles actuales");
                }
            }

            int extraCols = this.TableColumns() - numFacets;
            int[] newIndexRepeats = IndexRepeats(newLevels);

            // Tabla nueva: facetas ya rellenas, columnas extra a null (comportamiento de IniIndexSubTable)
            List<List<double?>> newMatrix = IniIndexSubTable(newListFacets, extraCols);

            // Volcamos cada fila antigua en su posición correspondiente de la tabla nueva
            foreach (List<double?> oldRow in this.matrix)
            {
                int newIndex = 0;
                for (int i = 0; i < numFacets; i++)
                {
                    newIndex += newIndexRepeats[i] * ((int)oldRow[i] - 1);
                }

                List<double?> newRow = newMatrix[newIndex];
                for (int col = numFacets; col < oldRow.Count; col++)
                {
                    newRow[col] = oldRow[col];
                }
            }

            this.matrix = newMatrix;
            this.listF = newListFacets;
        }

        #endregion SkipLevels methods

        #region Getters and Setters

        public void SetListFacets(ListFacets lf)
        {
            this.listF = lf;
        }

        /*
         * Descripción:
         *  Devuelve el número de columnas de la tabla.
         */
        public int TableColumns()
        {
            return this.matrix[0].Count;
        }

        /*
         * Descripción:
         *  Devuelve el número de filas de la tabla.
         */
        public int TableRows()
        {
            return this.matrix.Count;
        }

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
            if (row < 0 || col < 0 || row > (this.TableRows() - 1) || col > (this.TableColumns() - 1))
            {
                throw CPTException("Indice fuera de rango, posición no encontrada");
            }

            return this.matrix[row][col];
        }

        /*
         * Despcrición:
         *  Introduce un dato en la tabla en la posición que se pasa como parametro.
         *  Los datos se encuentran en la última columna de la tabla, por lo que el 
         *  parámetro columna esta fijo.
         * Parámetros:
         *      double? data: dato que se va a insertar en la última columna de la tabla.
         *      int pos: posción (fila) en la que vamos a insertar el dato. La columna de datos 
         *              es fija y es la última.
         * Excepciones:
         *      ObsTableException: En el caso de que la posición de inserción no coincida con el 
         *              rango de filas.
         */
        public void Data(double? data, int pos)
        {
            int rows = this.TableRows();
            int cols = this.TableColumns();

            if (pos < 0 || pos >= rows)
            {
                throw CPTException("La posición de inserción en la tabla se encuentra fuera del rango");
            }
            this.matrix[pos][cols - 1] = data;
        }

        /* Descripción:
         *  Añadimos la lista de datos que representa la fila de la tabla al final de la tabla
         *  TODELETE
         */
        public void Add(List<double?> row)
        {
            this.matrix.Add(row);
        }

        #endregion Getters and Setters

        #region Collapse Calculations

        /*
         * Descripción:
         *  Devuelve un array con los datos estadísticos de cada fila de esta tabla
         *  (tabla obtenida a partir de colapsar cero o más facetas de la de mfo)
         *  Usado tanto en la omisión de facetas como en el cálculo de medias.
         *  
         *  Algunos requisitos:
         *  - this.listF debe ser congruente con this.matrix.
         *  - this.listF debe ser un subconjunto de mfo.ListFacets().
         *  - SkipLevels debe haber sido aplicado tanto a esta tabla como a mfo.ObservationTable() antes de realizar esta operación.
         */
        public Statistics[] StatisticsData(MultiFacetsObs mfo, bool zero)
        {
            Statistics[] groups = new Statistics[this.TableRows()];
            for (int i = 0; i < groups.Length; i++)
            {
                groups[i] = new Statistics();
            }

            InterfaceObsTable mfo_table = mfo.ObservationTable();
            int[] indexRepeats = IndexRepeats(listF.levelOfFacets_skipped());

            int[] c_index = new int[listF.Count()];    //maps collapsed facet indices to original facet indices
            for (int i = 0; i < listF.Count(); i++)
            {
                c_index[i] = mfo.ListFacets().IndexOf(listF.FacetInPos(i));
            }

            for (int i = 0; i < mfo_table.TableRows(); i++)
            {
                //Calculate index of this row in the collapsed table. We can since 
                //row positions in our cartesian product tables are deterministic from their facets' values
                int collapsedRowIndex = 0;
                for (int c_i = 0; c_i < listF.Count(); c_i++)
                    collapsedRowIndex += indexRepeats[c_i] * (listF.FacetInPos(c_i).CollapsedValue((int)mfo_table.Data(i, c_index[c_i])) - 1);

                groups[collapsedRowIndex].Add(mfo_table.ObsData(i), zero);
            }

            return groups;
        }

        /*
         * Despcrición:
         *  Introduce la media en la fila de la tabla que se pasa como parámetro.
         *  Se asume que la columna en que se debe introducir la media es 
         *  la primera tras acabar con las facetas.
         *  listF debe ser congruente con matrix.
         */
        protected void MeanData(Statistics stc, int row)
        {
            if (row < 0 || row >= this.TableRows())
            {
                throw CPTException("La posición de inserción en la tabla se encuentra fuera del rango");
            }
            int meanCol = listF.Count();
            this.matrix[row][meanCol] = stc.Mean();
        }


        /*
         * Despcrición:
         *  Introduce la media en la fila de la tabla que se pasa como parámetro.
         *  Se asume que la columna en que se debe introducir la media es 
         *  la segunda tras acabar con las facetas.
         *  listF debe ser congruente con matrix.
         */
        protected void VarianceData(Statistics stc, int row)
        {
            if (row < 0 || row >= this.TableRows())
            {
                throw CPTException("La posición de inserción en la tabla se encuentra fuera del rango");
            }
            int meanCol = listF.Count();
            this.matrix[row][meanCol + 1] = stc.Variance();
        }


        /*
         * Despcrición:
         *  Introduce la media en la fila de la tabla que se pasa como parámetro.
         *  Se asume que la columna en que se debe introducir la media es 
         *  la tercera tras acabar con las facetas.
         *  listF debe ser congruente con matrix.
         */
        protected void Std_dev_Data(Statistics stc, int row)
        {
            if (row < 0 || row >= this.TableRows())
            {
                throw CPTException("La posición de inserción en la tabla se encuentra fuera del rango");
            }
            int meanCol = listF.Count();
            this.matrix[row][meanCol + 2] = stc.StandardDeviation();
        }

        #endregion Collapse Calculations

        public DataTable TableToDGV(bool hideNull = false)
        {
            DataTable dt = new DataTable();

            int numCols = this.TableColumns();
            int numRows = this.TableRows();

            // Define columns (nullable via DBNull)
            for (int i = 0; i < numCols; i++)
            {
                DataColumn col = new DataColumn($"Col{i}", typeof(double));
                col.AllowDBNull = true; // important to permit DBNull.Value
                dt.Columns.Add(col);
            }

            // Fill rows safely
            for (int r = 0; r < numRows; r++)
            {
                bool hasNull = false;
                DataRow row = dt.NewRow();
                for (int c = 0; c < numCols; c++)
                {
                    double? value = this.Data(r, c);
                    if (hideNull && value == null)
                    {
                        hasNull = true;
                        break;
                    }
                    row[c] = value.HasValue ? (object)value.Value : DBNull.Value;
                }

                if (!hasNull)
                    dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
