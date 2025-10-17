using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MultiFacetData
{
    public abstract class CartesianProductTable
    {
        protected List<List<double?>> matrix;
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
         */
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
        protected static int[] IndexRepeats(int[] levelOfFacets)
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
         *  Elimina las filas que en la columna especificada contengan el nivel especificado.
         *  Después, restaura los índices de los niveles superiores para que no haya huecos
         *  de por medio.
         *  
         *  Parámetros:
         *          int skipLevel: nivel que se va a eliminar
         *          int col: columna en la que se va a buscar el nivel a eliminar
         */
        public void SkipLevelAndRestoreIndex(int skipLevel, int col)
        {
            //Step 1: remove the appropiate rows
            this.matrix.RemoveAll(row => (double)row[col] == skipLevel);

            //Step 2: Restore appropiate indexes
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
    }
}
