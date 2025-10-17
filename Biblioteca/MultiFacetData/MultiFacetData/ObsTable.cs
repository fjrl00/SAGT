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
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using AuxMathCalcGT;

namespace MultiFacetData
{
    /*
     * Descripción:
     * Representa la tabla de datos donde los datos estan en la última columna, las n-2 columnas restantes
     * representan el nivel de las respectivas facetas. 
     * 
     * Por ejemplo: 
     *      Facetas:            Niveles:
     *      ========            ========
     *      Individuos          2
     *      Observaciones       3
     *      Carateristicas      2
     *      
     *      Se representa como:
     *      
     * 
     *      Individuos  Observaciones   Características     Datos
     *      ==========  =============   ===============     =====
     *      1           1               1                   4
     *      1           1               2                   3
     *      1           2               1                   4
     *      1           2               2                   0
     *      1           3               1                   7
     *      1           3               2                   7
     *      2           1               1                   0
     *      2           1               2                   1
     *      2           2               1                   1
     *      2           2               2                   9
     *      2           3               1                   2
     *      2           3               2                   5
     *      
     *      Los datos puede representarese en una matriz. El número de filas se obtine de la 
     *      multiplicación los niveles de todas las facetas (en nuestro ejemplo: 2x3x2 = 12 
     *      filas). El número de columnas es igual al número de facetas más 1.
     *      
     *      Para representar los datos se usará un array bidimensional o matriz de tipo double.
     */
    public class ObsTable : InterfaceObsTable
    {
        /*=================================================================================
         * Constantes
         *=================================================================================*/
        internal const string BEGIN_OBS_TABLE = "<obs_table>";
        const string END_OBS_TABLE = "</obs_table>";

        /*=================================================================================
         * Variables de instancia
         *=================================================================================*/
        // Variables
        private List<List<double?>> obsMatrix;



        /*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
         * Constructores
         *+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        /* Descripción:
         *  Constructor por defecto
         */
        public ObsTable()
        {
            obsMatrix = new List<List<double?>>();
        }

        public ObsTable(List<List<double?>> obsMatrix)
        {
            this.obsMatrix = obsMatrix;
        }

        /*
         * Descripción:
         *  Constructor de la clase ObsTable. Genera un array multidimensional a partir de
         *  la lista de facetas. Calcula la dimensión a partir de los niveles de las facetas.
         *  Igualmente calucula los indices que se almacenarán en las n-2 primeras columnas
         *  (comenzando desde la posición cero).
         * 
         * Excepciones:
         *  ObsTableException: en el caso de que la lista de facetas sea null o menor que 2.
         *                      También se lanza esta excepción si al construir la tabla tiene
         *                      un número de filas superior al indice de los enteros.
         */
        public ObsTable(ListFacets list_facets)
        {
            if (list_facets == null)
            {
                throw new ObsTableException("Error: no se hay facetas");
            }

            int numFacets = list_facets.Count();
            if (numFacets < 2)
            {
                throw new ObsTableException("Error: al menos debe haber 2 facetas");
            }

            this.obsMatrix = IniIndexSubTable(list_facets);

        } // end public ObsTable(LinkedList<Facet> facets)



        /*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
         * Métodos Auxiliares
         *+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/


        /*
         * Descripción:
         *  Método auxiliar que devuelve un array con las veces que se repite cada uno 
         *  de los indices de una columna antes de pasar al siguiente indice. Su utilidad 
         *  es facilitar la contrucción de la tabla de observaciones.
         * 
         * Entradas:
         *      int[] levelOfFacets: un array con los niveles de cada una de las facetas.
         *      
         * Devuelve:
         *      int[] res: un array con las veces que se repite cada uno de los indices.
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
        }// end private static int[] RepeatedIndex(int[] levelOfFacets)



        /**
         * Descripción:
         *  Devuelve un esqueleto de una tabla de observaciones correspondiente a la lista
         *  de facetas proveída.
         *  La tabla de observaciones es de todas las facetas y todos los niveles, sin 
         *  importar si están marcados para omitir.   
         */
        private static List<List<double?>> IniIndexSubTable(ListFacets list_facets)
        {
            List<List<double?>> matrix = new List<List<double?>>();

            int numFacets = list_facets.Count();
            int[] levelOfFacets = new int[numFacets];
            int f_i = 0;
            int rows = 1; // Guarda el nº de filas de la matriz. Inicializada con la identidad multiplicativa (IM*x=x)
            foreach (Facet f in list_facets)
            {
                levelOfFacets[f_i++] = f.Level();
                rows *= f.Level();
            }

            int cols = numFacets + 1;

            int[] rep = RepeatedIndex(levelOfFacets);

            int anchura = levelOfFacets.Length;

            for (int i = 0; i < rows; i++)
            {
                matrix.Add(new List<double?>());
                for (int j = 0; j < cols; j++)
                {
                    matrix[i].Add(null);
                }
            }

            for (int columna = 0; columna < anchura; columna++)
            { // * for 1 *
                int indice = 1;
                int numRep = 0;
                for (int fila = 0; fila < rows; fila++)
                { // * for 2*


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

                } // * for 2 *
            } // * for 1 *

            return matrix;
        }// end private void IniIndexSubTable(double[] levelOfFacets, double[] rep,int rows)

        /*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
         * Métodos de consulta
         *+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        /*
         * Descripción:
         *  Devuelve el dato contenido en la ultima columna en la fila que se pasa
         *  como parametros.
         * Parámetro:
         *      int row: es la posición (fila) de donde vamos a leer el dato que siempre 
         *              estará en la última columna columna de datos.
         * Excepciones:
         *  Lanza una excepción ObsTableException si esta dentro del rango de filas de la tabla
         *  el parámetro de entrada.
         */
        public double? Data(int row)
        {
            if (row < 0 || row > this.ObsTableRows() - 1)
            {
                throw new ObsTableException("La fila no petenece al rango de columnas de la tabla.");
            }
            // variable de retorno
            double? res = this.obsMatrix[row][this.ObsTableColumns() - 1];

            return res;
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
            if (row < 0 || col < 0 || row > (this.ObsTableRows() - 1) || col > (this.ObsTableColumns() - 1))
            {
                throw new ObsTableException("Indice fuera de rango, posición no encontrada");
            }

            return this.obsMatrix[row][col];
        }


        /*
         * Descripción:
         *  Devuelve el número de columnas de la tabla.
         */
        public int ObsTableColumns()
        {
            return this.obsMatrix[0].Count;
        }


        /*
         * Descripción:
         *  Devuelve el número de filas de la tabla.
         */
        public int ObsTableRows()
        {
            return this.obsMatrix.Count;
        }


        /*
         * Descripción:
         *  Devuelve una lista de tipo double? con las observaciones que se encuentran en la 
         *  última columna de la tabla
         *  
         */
        public List<double?> ListObs()
        {
            int cols = this.ObsTableColumns();
            List<double?> listRes = new List<double?>(); //variable de retorno
            int rows = this.ObsTableRows();
            for (int i = 0; i < rows; i++)
            {
                listRes.Add(this.obsMatrix[i][cols - 1]);
            }
            return listRes;
        }


        /*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
         * Métodos de instancia
         *+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        /* Descripción:
         *  Añadimos la lista de datos que representa la fila de la tabla al final de la tabla
         */
        public void Add(List<double?> row)
        {
            this.obsMatrix.Add(row);
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
            int rows = this.ObsTableRows();
            int cols = this.ObsTableColumns();

            if (pos < 0 || pos >= rows)
            {
                throw new ObsTableException("La posición de inserción en la tabla de observaciones se encuentra fuera del rango");
            }
            this.obsMatrix[pos][cols - 1] = data;
        }


        /*
         * Despcrición:
         *  Introduce un dato en la tabla en la posición que se pasa como parámetro.
         *  Los datos se encuentran en la última columna de la tabla, por lo que el 
         *  parámetro columna esta fijo.
         * Parámetros:
         *      List<double?> ldata: lista de datos que se insertan en la última columna de la tabla.
         * Excepciones:
         *      ObsTableException: En el caso de que no coincida la cantidad de datos con la 
         *              dimensión de la columna
         */
        public void AssignListData(List<double?> ldata)
        {
            int rows = this.ObsTableRows();
            if (ldata.Count != rows)
            {
                throw new ObsTableException("La cantidad de datos no coincide con la dimensión de la tabla");
            }

            int cols = this.ObsTableColumns();
            int pos = 0;
            foreach (double? d in ldata)
            {
                this.obsMatrix[pos][cols - 1] = d;
                pos++;
            }
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
            this.obsMatrix.RemoveAll(row => (double)row[col] == skipLevel);

            //Step 2: Restore appropiate indexes
            for (int i = 0; i < this.ObsTableRows(); i++)
            {
                double data = (double)this.obsMatrix[i][col];
                if (data > skipLevel)
                {
                    this.obsMatrix[i][col] = data - 1;
                }
            }
        }


        /* Descripción:
         *  Elimina de la tabla de observaciones todas las filas que contengan un nivel
         *  marcado para ser omitido en la lista de facetas correspondiente.
         *  No se requiere restaurar los índices de los niveles superiores.
         *  
         *  Parámetros:
         *          ListFacets lf: Lista de facetas que contiene los niveles a omitir.
         */
        public void SkipLevelIndex(ListFacets lf)
        {
            this.obsMatrix.RemoveAll(row =>
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
         *  Genera una nueva tabla de observaciones a partir de esta omitiendo/colapsando 
         *  las facetas que están marcadas para omitir.
         *  
         *  Parámetros:
         *          ListFacets lf: Lista de facetas 
         *              (debe coincidir con las de esta tabla)
         */
        public ObsTable CollapsedTable(ListFacets lf)
        {
            //Phase 0: check if the given list of facets is congruent with this table. If not, throw an exception.
            //todo

            //Phase 1: Build skeleton of new table
            List<List<double?>> collapsedTable = IniIndexSubTable(lf.ListFacetWithoutOmit());

            //Phase 2: Build dictionary of surviving facet value combinations and the list of measurements for each.
            // Begin building table too (maintains order of first appearance)
            var groups = new Dictionary<List<double>, AuxMathCalcGT.Statistics>(new ListDoubleComparer());
            HashSet<int> omittedIndexes = lf.OmittedIndexes();

            int measurementCol = this.ObsTableColumns() - 1;
            foreach (var row in obsMatrix)
            {
                //extract the key (facet values) for this row, ignoring omitted facets
                var key = new List<double>();
                for (int i = 0; i < measurementCol; i++)
                    if (!omittedIndexes.Contains(i))
                        key.Add((double)row[i]);

                if (!groups.TryGetValue(key, out var measurements)) // if first time we see this combination, then initialize it
                {
                    measurements = new AuxMathCalcGT.Statistics();
                    groups[key] = measurements;
                }

                // Add measurement to the correct group
                measurements.Add(row[measurementCol]);
            }

            // Phase 3: Fill in the measurements
            int collapsedMeasurementCol = collapsedTable[0].Count - 1;
            for (int i = 0; i < collapsedTable.Count; i++)
            {
                var key = collapsedTable[i].Take(collapsedMeasurementCol).Select(d => (double)d).ToList();
                collapsedTable[i][collapsedMeasurementCol] = groups[key].Mean();
            }

            return new ObsTable(collapsedTable);
        }

        /* Descripción:
         *  Clase auxiliar para CollapsedTable. Comparador de valores de facetas en filas
         *  de la tabla de observaciones.
         */
        class ListDoubleComparer : IEqualityComparer<List<double>>
        {
            public bool Equals(List<double> x, List<double> y)
            {
                if (x.Count != y.Count) return false;
                for (int i = 0; i < x.Count; i++)
                {
                    if (x[i] != y[i]) return false;
                }
                return true;
            }

            public int GetHashCode(List<double> obj)
            {
                unchecked
                {
                    int hash = 1;
                    foreach (var d in obj)
                    {
                        hash = hash * 31 + d.GetHashCode();
                    }
                    return hash;
                }
            }
        }


        #region Escritura Lectura en un stream

        /* Descripción:
         *  Devuelve la suma de los datos de la tabla.
         *  Suma de nulls equivale a null. Sin embargo sumar null a un número equivale a ese número.
         */
        public double? SumOfData()
        {
            double? sum_X = null;

            int n = this.ObsTableRows();
            for (int i = 0; i < n; i++)
            {
                double? data = this.Data(i);
                if (sum_X != null && data != null)
                {
                    sum_X += data;
                }
                else if (sum_X == null && data != null)
                {
                    sum_X = data;
                }

            }
            return sum_X;
        }


        /* Descripción:
         *  Escribe la tabla de datos de frecuencias.
         */
        public bool WritingStreamObsTable(StreamWriter writerFile)
        {
            bool res = false; // variable de retorno
            writerFile.WriteLine(BEGIN_OBS_TABLE);
            // Escribimos los datos
            int row = this.ObsTableRows();
            int col = this.ObsTableColumns();
            for (int i = 0; i < row; i++)
            {
                string line = "";
                for (int j = 0; j < col; j++)
                {
                    double? d = this.Data(i, j);
                    string valor = ConvertNum.Double2String(d); ;
                    line = line + valor + " ";
                }


                writerFile.WriteLine(line);
            }

            // ponemos el cierre
            writerFile.WriteLine(END_OBS_TABLE);
            res = true;
            return res;
        }


        /* Descripción:
         *  Lee un vector de datos de un stream y lo usa para rellenar la tabla.
         * Parámetros:
         *      StreamReader reader: El stream del que vamos a leer los vectores de datos.
         */
        public static ObsTable ReadingStreamObsTable(StreamReader reader)
        {
            try
            {
                ObsTable res = new ObsTable(); // valor de retorno
                List<double?> ldata = new List<double?>();
                char[] delimeterChars2 = { ' ' }; // nuestro delimitador será el caracter '/'

                string line;
                while ((line = reader.ReadLine()) != null && !line.Equals(END_OBS_TABLE))
                {
                    string[] arrayOfSplit = line.Split(delimeterChars2, StringSplitOptions.RemoveEmptyEntries);
                    int n = arrayOfSplit.Length;
                    List<double?> listDouble = new List<double?>();
                    for (int i = 0; i < n; i++)
                    {
                        listDouble.Add(ConvertNum.String2Double(arrayOfSplit[i]));
                    }
                    res.obsMatrix.Add(listDouble);
                }
                if (line == null)
                {
                    throw new ObsTableException("Unexpected end of file while reading observation table.");
                }

                return res;
            }
            catch (FormatException ex)
            {
                throw new ObsTableException($"Unexpected value found when parsing observation table: {ex.Message}");
            }
        }

        #endregion Escritura Lectura en un stream


        #region Conversión a DataTable o DataSet
        /*===============================================================================================
         * Conversión a DataTable o DataSet
         * - Tabla de frecuencias a DataTable (ObsTable2DataTable)
         * - Tabla de frecuencias a DataSet (ObsTable2DataSet)
         * - De DataTable a tabla de frecuencias (DataTable2ObsTable)
         * - De DataSet a tabla de frecuencias (DataSet2ObsTable)
         *===============================================================================================/
        /* Descripción:
         *  Devuelve un dataTable con los datos de la tabla de frecuencias
         */
        public DataTable ObsTable2DataTable(ListFacets lf)
        {
            if (lf.Count() + 1 != this.ObsTableColumns())
            {
                throw new ObsTableException("Lista de facetas no coincide con el número de columnas indice");
            }
            DataTable dtObsTable = new DataTable("TbObsTable"); // valor de retorno

            int numFacet = lf.Count();

            for (int i = 0; i < numFacet; i++)
            {
                Facet f = lf.FacetInPos(i);
                string name_col = f.Name();
                dtObsTable.Columns.Add(new DataColumn(name_col, System.Type.GetType("System.Double")));
            }
            // Añadimos la columna de las frecuencias
            dtObsTable.Columns.Add(new DataColumn("obs_data", System.Type.GetType("System.Double")));

            // rellenamos el dataTable
            int numRows = this.ObsTableRows();
            int numCols = this.ObsTableColumns();
            for (int i = 0; i < numRows; i++)
            {
                // Creamos una fila
                DataRow row = dtObsTable.NewRow();
                // Rellenamos la fila
                for (int j = 0; j < numCols; j++)
                {
                    string name_col = "obs_data";
                    if (j < lf.Count())
                    {
                        Facet f = lf.FacetInPos(j);
                        name_col = f.Name();
                    }
                    if (this.Data(i, j) != null)
                    {
                        row[name_col] = this.Data(i, j);
                    }
                }
                // Añadimos la fila al dataTable
                dtObsTable.Rows.Add(row);
            }

            return dtObsTable;
        }// end ObsTable2DataTable


        /* Descripción:
        *  Devuelve un dataTable con los datos de la tabla de frecuencias
        */
        public DataSet ObsTable2DataSet(ListFacets lf)
        {
            DataTable dt = this.ObsTable2DataTable(lf);
            DataSet ds = new DataSet("DataSet_ObsTable");
            ds.Tables.Add(dt);
            return ds;
        }

        /* Descripción:
         *  Toma un DataTable como argumento y devuelve una Tabla de Frecuencias
         */
        public static ObsTable DataTable2ObsTable(DataTable dtObsTable)
        {
            ObsTable obsTable = new ObsTable();

            int numRows = dtObsTable.Rows.Count;
            int numColums = dtObsTable.Columns.Count;

            for (int i = 0; i < numRows; i++)
            {
                // Creamos la fila
                List<double?> row = new List<double?>();
                for (int j = 0; j < numColums; j++)
                {
                    double? d_value = null;
                    if (!string.IsNullOrEmpty(dtObsTable.Rows[i][j].ToString()))
                    {
                        d_value = (double?)dtObsTable.Rows[i][j];
                    }
                    row.Add(d_value);
                }

                // Añadimos la fila
                obsTable.Add(row);
            }

            return obsTable;
        }// end DataTable2ObsTable


        /* Descripción:
         *  Toma un DataSet como argumento y devuelve una Tabla de Frecuencias
         */
        public static ObsTable DataSet2ObsTable(DataSet dsObsTable)
        {
            DataTable dt = dsObsTable.Tables["TbObsTable"];
            return DataTable2ObsTable(dt);
        }
        #endregion Conversión a DataTable o DataSet

        #region Clonación

        public ObsTable Clone()
        {
            List<List<double?>> newMatrix = new List<List<double?>>();
            foreach (var row in this.obsMatrix)
            {
                List<double?> newRow = new List<double?>(row);
                newMatrix.Add(newRow);
            }
            return new ObsTable(newMatrix);
        }

        #endregion Clonación

        #region Métodos redefinidos (ToString, Equals, GetHashCode)
        /*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ 
         * Métodos redefinidos
         * - ToString
         * - Equals
         * - GetHashCode
         *+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        /*
         * Descripción:
         *  Redefinición de la operación ToString para la clase ObsTable.
         */
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            //string res = ""; // variable de retorno
            for (int i = 0; i < this.ObsTableRows(); i++)
            {/*for 1*/
                for (int j = 0; j < this.ObsTableColumns(); j++)
                {/*for 2*/
                    if (this.obsMatrix[i][j] == null)
                    {
                        res.Append("- ");
                    }
                    else
                    {
                        res.Append(this.obsMatrix[i][j].ToString() + " ");
                    }
                }/*end for 2*/
                res.Append("; \n");
            }/*end for 1*/
            return res.ToString();
        } // public override string ToString()


        /*
         * Descripción:
         *  Redefinición del método Equals.
         */
        public override bool Equals(object obj)
        {
            // Variable de retorno
            bool res = false;
            if (obj is ObsTable obsT)
            {// (* 1 *)
                if (this.ObsTableRows().Equals(obsT.ObsTableRows())
                    && this.ObsTableColumns().Equals(obsT.ObsTableColumns()))
                {// (* 2 *)
                    res = true;

                    for (int i = 0; i < this.ObsTableRows() && res; i++)
                    {
                        for (int j = 0; j < this.ObsTableColumns() && res; j++)
                        {
                            res = this.obsMatrix[i][j].Equals(obsT.obsMatrix[i][j]);
                        }
                    }
                }// (* 2 *)

            } // end if (* 1 *)
            return res;
        } // public override bool Equals(object obj)


        /*
         * Descripción:
         *  Redefinición del método GetHashCode.
         */
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 1;
                int rows = this.ObsTableRows();
                int cols = this.ObsTableColumns();

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        double? val = this.obsMatrix[i][j];
                        hash = hash * 31 + (val?.GetHashCode() ?? 0);
                    }
                }

                hash = hash * 31 + rows.GetHashCode();
                hash = hash * 31 + cols.GetHashCode();

                return hash;
            }
        }// public override int GetHashCode()

        #endregion Métodos redefinidos (ToString, Equals, GetHashCode)

    } // end class ObsTable
} // end nameSpace MultFacetData
