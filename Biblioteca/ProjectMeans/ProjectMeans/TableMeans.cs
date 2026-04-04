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
 * Fecha de revisión: 03/Mar/2012
 * 
 * Descripción:
 *      Libreria de medias
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
    /*
     * Descripción:
     * Representa la tabla de medias (aunque también contiene varianza y desviación típica) donde los 
     * datos estan en la última columna, las n-4 columnas restantes representan el nivel de las 
     * respectivas facetas. La columna n-3 representa la media, la columna n-2 la varianza, y la
     * n-1 la desviación tipica.
     * 
     *      
     *      Se representa como:
     *                                (Pos:
     *      (Pos:0)         (Pos:n-4) n-3)   (Pos:n-2) (Pos:n-1)
     *      Faceta1  (...)  Faceta_m  media  Varianza  Desviación tipica
     *      =======  =====  ========  =====  ========  =================
     *      1           1       1       x1      v1            dt1
     *      1           1       2       x2      v2            dt2
     *      1           2       3       x3      v3            dt3
     *      (...)     (...)   (...)   (...)   (...)          (...)
     */
    public class TableMeans : CartesianProductTable, InterfaceTableMeans
    {
        /*=================================================================================
         * Constantes
         *=================================================================================*/
        internal const string BEGIN_TABLE_MEANS = "<table_means>";
        const string END_TABLE_MEANS = "</table_means>";
        const string BEGIN_LIST_OF_DATAMEANS = "<list_of_data_means>";
        const string END_LIST_OF_DATAMEANS = "</list_of_data_means>";


        /*=================================================================================
         * Variables de instancia
         *=================================================================================*/
        //private List<List<double?>> matrix; // matriz de medias.
        //private ListFacets listF; // lista de facetas sobre la que construiremos la tabla de medias
        protected override Exception CPTException(string msg)
        {
            return new TableMeansException(msg);
        }
        private double? grandMean; // Gran media o media general
        private double? variance; // Varianza
        private double? stdDev; // Desviación típica
        private string facetDesign; // Diseño de las facetas, el texto que aparece en la pestaña


        /*=================================================================================
         * Constructores
         *=================================================================================*/
        public TableMeans()
        {
            listF = new ListFacets();
            this.matrix = new List<List<double?>>();
        }

        //Lazy constructor used by ConnectDB. Do not use otherwise
        public TableMeans(ListFacets lf, string design, int rows)
            :this()
        {
            if (lf.Count() < 1)
            {
                throw new TableMeansException("Error: no hay facetas");
            }
            this.listF = lf;
            this.facetDesign = design;
            int cols = lf.Count() + 3;

            for (int i = 0; i < rows; i++)
            {
                this.matrix.Add(new List<double?>());
                for (int j = 0; j < cols; j++)
                {
                    this.matrix[i].Add(null);
                }
            }
        }


        /* Descripción:
         *  Constructor principal de la clase.
         * Parámetros:
         *      ListFacets lF: Lista de facetas
         *          Niveles marcados para omitir serán omitidos.
         *          Facetas marcadas para omitir no serán omitidas.
         *      string design: Texto que identifica la tabla de medias
         *      MultiFacetsObs mfo: Objeto multifaceta con la tabla de observaciones
         *      bool zero: Si es true se realizarán los calculos interpretando los valores 
         *              nulos como ceros
         */
        public TableMeans(ListFacets lF, string design, MultiFacetsObs mfo, bool zero=false)
        {

            if (lF.Count() < 1)
            {
                throw new TableMeansException("Error: no hay facetas");
            }
            this.listF = lF;    //debe ser un subconjunto de mfo.listFacets
            this.facetDesign = design;

            // creamos la matriz
            matrix = IniIndexSubTable(lF);

            // eliminamos las filas omitidas
            if (this.listF.HasSkipLevels())
            {
                this.SkipLevels(this.listF);
            }

            // Calculamos las medias
            Statistics[] stats = this.StatisticsData(mfo, zero);
            for (int i = 0; i < this.TableRows(); i++)
            {
                this.MeanData(stats[i], i);
                this.VarianceData(stats[i], i);
                this.Std_dev_Data(stats[i], i);
            }

            // Calculamos la gran media
            this.Calc_GrandMean_Variance_StdDev(mfo, zero);
        }

        //ReadingStreamTableMeans's constructor
        public TableMeans(ListFacets lf, string design, double? grandMean, double? variance, double? std_dev,
            List<List<double?>> meansMatrix)
        {
            if (lf.Count() < 1)
            {
                throw new TableMeansException("Error: no hay facetas");
            }
            this.listF = lf;
            this.facetDesign = design;
            this.grandMean = grandMean;
            this.variance = variance;
            this.stdDev = std_dev;
            this.matrix = meansMatrix;

        }// end public TableMeans

        //DataTable constructor
        public TableMeans(DataTable dt, double? grandMean, double? variance, double? stdDev, string facetDesign)
            :this()
        {
            this.listF = new ListFacets();
            int r = dt.Rows.Count;
            int c = dt.Columns.Count;

            // Inicializamos la estructura
            for (int i = 0; i < r; i++)
            {
                List<double?> ld = new List<double?>();
                this.matrix.Add(ld);
            }

            // Rellenamos los datos columna a columna
            for (int j = 0; j < c; j++)
            {
                int level = 1;
                
                for (int i = 0; i < r; i++)
                {
                    List<double?> ld = this.matrix[i];
                    double? d = null;
                    DataRow row = dt.Rows[i];
                    object o = row[j];

                    if (o != null)
                    {
                        d = ConvertNum.String2Double((string)o.ToString());
                        if (d != null && j < c - 3)
                        {
                            level = Convert.ToInt32(Math.Max(level, (double)d));
                        }
                    }
                    
                    ld.Add(d);
                }

                if (j < c - 3)
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



        #region Operaciones auxiliares del constructor
        /*=================================================================================
         * Operaciones auxiliares del constructor:
         *      - IndexRepeats --> array de indices 
         *      - IniIndexSubTable --> Inicializa la tabla con los indices
         *      - SkipLevels --> Elimina las filas con niveles omitidos
         *=================================================================================*/

        /**
         * Descripción:
         *  Devuelve un esqueleto de la tabla correspondiente a la lista de facetas proveída.
         *  La tabla es de todas las facetas y todos los niveles, sin importar si están marcados para omitir.    
         */
        private static List<List<double?>> IniIndexSubTable(ListFacets list_facets)
        {
            return IniIndexSubTable(list_facets, 3);
        }// end private void IniIndexSubTable(double[] levelOfFacets, double[] rep,int rows)

        #endregion Operaciones auxiliares del constructor



        


        /* Descripción:
         *  Devuelve la Gran media o media general.
         * Parámetros
         *      MultiFacetsObs mfo: Para obtener los datos de la tabla de observaciones de los que se optiene la grand media
         *      bool zero: true si se quiere realizar los calculos interpretando los valores nulos
         *              como ceros.
         */
        public void Calc_GrandMean_Variance_StdDev(MultiFacetsObs mfo, bool zero)
        {
            // Creamos el elemento stadistica que contendra las sumas
            Statistics stc = new Statistics();
            InterfaceObsTable observationTable = mfo.ObservationTable();
            // int r = this.TableRows();
            int r = observationTable.TableRows();
            // int c = (this.TableColumns()-3);
            for (int i = 0; i < r; i++)
            {
                // stc.Add(this.Data(i, c), zero);
                stc.Add(observationTable.ObsData(i), zero);
            }
            this.grandMean = stc.Mean();
            this.variance = stc.Variance();
            this.stdDev = stc.StandardDeviation();
        }


        #region Métodos de consulta
        /*=================================================================================
         * Métodos de Consulta
         *  - Data
         *  - MeanData
         *  - VarianceData
         *  - Std_dev_Data
         *  - TableColumns
         *  - TableRows
         *  - ListFacets
         *  - GrandMean
         *  - StdDev
         *  - FacetDesign
         *=================================================================================*/

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
            if (row < 0 || row > this.TableRows() - 1)
            {
                throw new TableMeansException("La fila no petenece al rango de columnas de la tabla.");
            }
            // variable de retorno
            double? res = this.matrix[row][this.TableColumns() - 3];

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
            if (row < 0 || row > this.TableRows() - 1)
            {
                throw new TableMeansException("La fila no petenece al rango de columnas de la tabla.");
            }
            // variable de retorno
            double? res = this.matrix[row][this.TableColumns() - 2];

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
            if (row < 0 || row > this.TableRows() - 1)
            {
                throw new TableMeansException("La fila no petenece al rango de columnas de la tabla.");
            }
            // variable de retorno
            double? res = this.matrix[row][this.TableColumns() - 1];

            return res;
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
            if ((row < 0 || row >= this.TableRows()) || (col < 0 || col >= this.TableColumns()))
            {
                throw new TableMeansException("La posición de inserción en la tabla de medias se encuentra fuera del rango");
            }
            this.matrix[row][col] = data;
        }


        /* Descripción:
         *  Asigna un double? que representa la Gran Media de la tabla.
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


        #endregion Métodos de instancia


        /* Descripción:
         *  Escribe en el StreamWriter 
         */
        public bool WritingStreamTableMeans(StreamWriter writerFile)
        {
            bool res = false; // variable de retorno

            writerFile.WriteLine(BEGIN_TABLE_MEANS);
            // Escribimos el diseño de las facetas
            writerFile.WriteLine(this.facetDesign);
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
            writerFile.WriteLine(END_TABLE_MEANS);
            return res;
        }


       /* Descripción:
        *  Escribe la lista de datos de media.
        */
        public bool WritingStreamListDataMeans(StreamWriter writerFile)
        {
            bool res = false; // variable de retorno
            writerFile.WriteLine(BEGIN_LIST_OF_DATAMEANS);
            // Escribimos los datos
            int row = this.TableRows();

            for (int i = 0; i < row; i++)
            {
                List<double?> row_data = matrix[i];
                int col = row_data.Count;
                string line = "";
                for (int j = 0; j < col; j++)
                {
                    line = line + ConvertNum.Double2String(row_data[j]) + " ";
                }

                writerFile.WriteLine(line);
                    
            }

            // ponemos el cierre
            writerFile.WriteLine(END_LIST_OF_DATAMEANS);
            res = true;
            return res;
        }


        /* Descripción:
         *  Lee los datos de una tableMeans de un stream y lo devuelve como objeto.
         * Parámetros:
         *      StreamReader reader: El stream del que vamos a leer la tabla de medias.
         */
        public static TableMeans ReadingStreamTableMeans(StreamReader reader)
        {
            try
            {
                string line;
                // Read the initial design line
                string design = reader.ReadLine();

                // Read BEGIN_LISTFACETS marker
                if ((line = reader.ReadLine()) == null || !line.Equals(MultiFacetData.ListFacets.BEGIN_LISTFACETS))
                {
                    throw new TableMeansException(
                        $"Expected '{MultiFacetData.ListFacets.BEGIN_LISTFACETS}' but found '{line}' while reading table of means.");
                }

                // Parse list of facets
                ListFacets lf = MultiFacetData.ListFacets.ReadingStreamListFacets(reader);

                // Read design line again (after facets)
                design = reader.ReadLine();

                // Read BEGIN_LIST_OF_DATAMEANS marker
                if ((line = reader.ReadLine()) == null || !line.Equals(BEGIN_LIST_OF_DATAMEANS))
                {
                    throw new TableMeansException(
                        $"Expected '{BEGIN_LIST_OF_DATAMEANS}' but found '{line}' while reading table of means.");
                }

                // Parse means data
                var meansMatrix = new List<List<double?>>();
                char[] delimiterChars = { ' ' };
                while ((line = reader.ReadLine()) != null && !line.Equals(END_LIST_OF_DATAMEANS))
                {
                    string[] arrayOfDouble = line.Trim().Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);

                    List<double?> row_data = new List<double?>();
                    int numData = arrayOfDouble.Length;
                    for (int i = 0; i < numData; i++)
                    {
                        row_data.Add(ConvertNum.String2Double(arrayOfDouble[i]));
                    }
                    meansMatrix.Add(row_data);
                }
                if (line == null)
                {
                    throw new TableMeansException("Unexpected end of file while reading table of means.");
                }

                // Read summary values
                double? gm = ConvertNum.String2Double(reader.ReadLine());
                double? v = ConvertNum.String2Double(reader.ReadLine());
                double? stdv = ConvertNum.String2Double(reader.ReadLine());

                // Construct TableMeans object
                TableMeans tb = new TableMeans(lf, design, gm, v, stdv, meansMatrix);

                // Check END_TABLE_MEANS marker
                if ((line = reader.ReadLine()) == null || !line.Equals(END_TABLE_MEANS))
                {
                    throw new TableMeansException(
                        $"Expected '{END_TABLE_MEANS}' but found '{line}' while reading table of means.");
                }

                return tb;
            }
            catch (FormatException ex)
            {
                throw new TableMeansException($"Error parsing numeric values in table means: {ex.Message}");
            }
            catch (ListFacetsException ex)
            {
                throw new TableMeansException("Error in table of means.", ex);
            }
        }// end private static TableMeans ReadingStreamTableMeans


        #region Conversión entre tabla de medias y DataSet

        /* Descripción:
         * Convierte una tabla de medias en un DataSet
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
            row["type_means"] = "tableMeans";
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
            DataTable dtTableMeans = new DataTable("Tb_Table_Means"); // valor de retorno

            int numFacet = this.listF.Count();

            for (int i = 0; i < numFacet; i++)
            {
                Facet f = this.listF.FacetInPos(i);
                string name_col = f.Name();
                dtTableMeans.Columns.Add(new DataColumn(name_col, System.Type.GetType("System.Double")));
            }
            // Añadimos la columna de las observaciones
            dtTableMeans.Columns.Add(new DataColumn("mean", System.Type.GetType("System.Double")));
            dtTableMeans.Columns.Add(new DataColumn("variance", System.Type.GetType("System.Double")));
            dtTableMeans.Columns.Add(new DataColumn("std_dev", System.Type.GetType("System.Double")));

            // rellenamos el dataTable
            int numRows = this.TableRows();
            int numCols = this.TableColumns();
            for (int i = 0; i < numRows; i++)
            {
                // Creamos una fila
                DataRow row = dtTableMeans.NewRow();
                // Rellenamos la fila
                for (int j = 0; j < numCols; j++)
                {
                    string name_col = "mean";
                    if (j == numCols - 1)
                    {
                        name_col = "std_dev";
                    }
                    else if (j == numCols - 2)
                    {
                        name_col = "variance";
                    }
                    else if (j == numCols - 3)
                    {
                        name_col = "mean";
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

            DataTable dtMeansMatix = dsTableMeans.Tables["Tb_Table_Means"];

            return new TableMeans(dtMeansMatix, grandMean, variance, std_dev, facet_design);
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
            int rows = this.TableRows();
            int cols = this.TableColumns();

            for (int i = 0; i < rows; i++)
            {/*for 1*/
                for (int j = 0; j < cols; j++)
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
                res.Append("\n");
            }/*end for 1*/
            return res.ToString();
        } // public override string ToString()

        #endregion Métodos redefinidos

    } // end public class TableMeans
} // namespace ProjectMeans
