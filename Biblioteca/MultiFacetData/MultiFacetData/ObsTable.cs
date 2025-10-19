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
     */
    public class ObsTable : CartesianProductTable, InterfaceObsTable
    {
        /*=================================================================================
         * Constantes
         *=================================================================================*/
        internal const string BEGIN_OBS_TABLE = "<obs_table>";
        const string END_OBS_TABLE = "</obs_table>";

        /*=================================================================================
         * Variables de instancia
         *=================================================================================*/

        //private List<List<double?>> matrix;       //inherited from CartesianProductTable
        //private ListFacets listF;                 //inherited from CartesianProductTable. POORLY IMPLEMENTED HERE RN
        protected override Exception CPTException(string msg)
        {
            return new ObsTableException(msg);
        }

        /*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
         * Constructores
         *+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        /* Descripción:
         *  Constructor por defecto
         */
        public ObsTable()
        {
            matrix = new List<List<double?>>();
        }

        //Cloning constructor
        public ObsTable(List<List<double?>> obsMatrix)
        {
            this.matrix = obsMatrix;
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
                throw new ObsTableException("Error: no hay facetas");
            }

            int numFacets = list_facets.Count();
            if (numFacets < 2)
            {
                throw new ObsTableException("Error: al menos debe haber 2 facetas");
            }

            this.listF = list_facets;
            this.matrix = IniIndexSubTable(list_facets);

        } // end public ObsTable(LinkedList<Facet> facets)

        //OmitFacetInDataTable's constructor
        public ObsTable(ListFacets lF, MultiFacetsObs mfo)
        {
            this.matrix = IniIndexSubTable(lF);
            this.listF = lF;

            this.SkipLevels(lF);
            Statistics[] stats = this.StatisticsData(mfo.SkipIndexLevelFacetInDataTable(), false);  //Note that SkipIndexLevelFacetInDataTable doesn't modify mfo, it instead returns a modified clone. Wonky, I know
            for (int i = 0; i < this.TableRows(); i++)
            {
                this.MeanData(stats[i], i);
            }
        }


        /*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
         * Métodos Auxiliares
         *+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        private static List<List<double?>> IniIndexSubTable(ListFacets list_facets)
        {
            return IniIndexSubTable(list_facets, 1);
        }

        /*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
         * Métodos de consulta
         *+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        /*
         * Descripción:
         *  Devuelve la observación contenida en la ultima columna en la fila que se pasa
         *  como parametros.
         * Parámetro:
         *      int row: es la posición (fila) de donde vamos a leer el dato que siempre 
         *              estará en la última columna columna de datos.
         * Excepciones:
         *  Lanza una excepción ObsTableException si esta dentro del rango de filas de la tabla
         *  el parámetro de entrada.
         */
        public double? ObsData(int row)
        {
            if (row < 0 || row > this.TableRows() - 1)
            {
                throw new ObsTableException("La fila no petenece al rango de columnas de la tabla.");
            }
            // variable de retorno
            double? res = this.matrix[row][this.TableColumns() - 1];

            return res;
        }


        /*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
         * Métodos de instancia
         *+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

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
            int rows = this.TableRows();
            if (ldata.Count != rows)
            {
                throw new ObsTableException("La cantidad de datos no coincide con la dimensión de la tabla");
            }

            int cols = this.TableColumns();
            int pos = 0;
            foreach (double? d in ldata)
            {
                this.matrix[pos][cols - 1] = d;
                pos++;
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

            int n = this.TableRows();
            for (int i = 0; i < n; i++)
            {
                double? data = this.ObsData(i);
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
            int row = this.TableRows();
            int col = this.TableColumns();
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
                    res.matrix.Add(listDouble);
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
            if (lf.Count() + 1 != this.TableColumns())
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
            int numRows = this.TableRows();
            int numCols = this.TableColumns();
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
            foreach (var row in this.matrix)
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
            for (int i = 0; i < this.TableRows(); i++)
            {/*for 1*/
                for (int j = 0; j < this.TableColumns(); j++)
                {/*for 2*/
                    if (this.matrix[i][j] == null)
                    {
                        res.Append("- ");
                    }
                    else
                    {
                        res.Append(this.matrix[i][j].ToString() + " ");
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
                if (this.TableRows().Equals(obsT.TableRows())
                    && this.TableColumns().Equals(obsT.TableColumns()))
                {// (* 2 *)
                    res = true;

                    for (int i = 0; i < this.TableRows() && res; i++)
                    {
                        for (int j = 0; j < this.TableColumns() && res; j++)
                        {
                            res = this.matrix[i][j].Equals(obsT.matrix[i][j]);
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
                int rows = this.TableRows();
                int cols = this.TableColumns();

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        double? val = this.matrix[i][j];
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
