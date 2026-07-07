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
 * Fecha de revisión: 14/Jun/2012                
 * 
 * Cambios recientes:
 *  Se ha cambiado la variable de instancia lisFacets de una List<Facet> implementada en la
 *  propia clase a una clase externa ListFacets (esta se ha parcializado en dos clases: 
 *  LisFacets y ListFacets2. Esta última contiene las operaciones necesarias para generar las
 *  combinaciones sin repetición de una lista de facetas).
 */
using AuxMathCalcGT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace MultiFacetData
{
    public class MultiFacetsObs
    {
        #region Constantes y variables de instancia
        /*=================================================================================
         * Constantes
         *=================================================================================*/
        // Comienzo y fin de comentario de una tabla de observaciones
        const string BEGIN_COMMENT = "<file_data_comment>";
        const string END_COMMENT = "</file_data_comment>";

        /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
         * Variables de instancia
         *++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/
        private ListFacets listFacets; // lista de facetas
        private string nameFileObs; // Nombre del fichero de observaciones
        private string description; // posible descripción del archivo
        private InterfaceObsTable observationTable; // tabla de observacion.
        private string comment;
        #endregion

        #region Constructores
        /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
         * Constructores
         *++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        /*
         * Decripción:
         *  Crea un objeto observaciones multifaceta. Como mínimo debe tener un nombre.
         * Parametros: 
         *      ListFacets listFacets: Lista de parámetros.
         *      string nameFileObs: Nombre del fichero.
         *      string description: descripción del elemento multifaceta.
         *      
         * Excepciones:
         *  Lanza una excepción MultiFacetException:
         *      Si la lista de facetas no contiene al menos dos facetas.
         *      Si no se ha introducido el nombre del archivo.
         */
        public MultiFacetsObs(ListFacets listFacets, string nameFileObs, string description)
        {
            if (listFacets.Count() < 2)
            {
                throw new MultiFacetObsException("Debe contener al menos dos facetas");
            }
            if (nameFileObs == null || nameFileObs.Length == 0)
            {
                throw new MultiFacetObsException("No ha introducido nombre de fichero");
            }
            this.listFacets = listFacets;
            this.nameFileObs = nameFileObs;
            this.description = description;
            this.observationTable = new ObsTable(this.listFacets);
            this.comment = "";
        }


        public MultiFacetsObs(ListFacets listFacets, InterfaceObsTable table, string nameFileObs, string description, string comment)
        {
            this.listFacets = listFacets;
            this.observationTable = table;
            this.nameFileObs = nameFileObs;
            this.description = description;
            this.comment = comment;
        }


        public MultiFacetsObs(ListFacets listFacets, string nameFileObs, string description, string comment)
            : this(listFacets, nameFileObs, description)
        {
            this.comment = comment;
        }

        #endregion Constructores




        #region Métodos de consulta
        /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
         * Métodos de consulta
         *++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        /*
         * Descripción:
         *  Devuelve la lista de facetas del objeto MultFaceObs.
         */
        public ListFacets ListFacets()
        {
            return this.listFacets;
        }


        /* Descripción:
         *  Devuelve el nombre del fichero.
         */
        public string NameFileObs()
        {
            return this.nameFileObs;
        }


        /* Descripción:
         *  Devuelve la descrición/comentario del fichero.
         */
        public string DescriptionFile()
        {
            return this.description;
        }


        /* Descripción:
         *  Devuelve los comentarios de la tabla de observaciones
         */
        public string Comment()
        {
            return this.comment;
        }


        /* Descripción:
         *  Devuelve la tabla de observaciones
         */
        // public ObsTable ObservationTable()
        public InterfaceObsTable ObservationTable()
        {
            return this.observationTable;
        }

        #endregion Métodos de consulta



        #region Métodos de instancia
        /*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
         * Métodos de instancia 
         *+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        /*
         * Descripción:
         *  Cambia la lista de facetas por otra que debe ser compatible con la actual 
         *  ObservationTable. Igual número de facetas y cada faceta teniendo el mismo nivel que 
         *  aquella en la misma posición.
         *  NOTA: Creo que este método está siendo actualmente usurpado en la funcionalidad de
         *  renombramiento de facetas.
         * 
         * Parámetros:
         *      List<Facet> list: Nueva lista de facetas;
         * Excepciones:
         *      Lanza una excepción MultiFacetException:
         *          Si no tiene al menos dos facetas.
         *          Si la lista de facetas no coincide en número con la facetas asignada.
         *          Si los niveles de la nueva lista no coinciden con los de la lista 
         *              del objeto.
         */
        public void ListFacets(ListFacets list)
        {
            // if (listFacets.Count() < 2)
            if (list.Count() < 2)
            {
                throw new MultiFacetObsException("Debe contener al menos dos facetas");
            }
            if (listFacets.Count() != list.Count())
            {
                throw new MultiFacetObsException("No se puede asignar la facetas ya que tienen el mismo número");
            }
            if (!this.CompareLevelsOfFacetsList(list))
            {
                throw new MultiFacetObsException("No tiene el mismo número de niveles");
            }
            this.listFacets = list;
        }


        /*
         * Descripción:
         *  Método auxiliar. Compara los niveles de las facetas de la lista que se pasa como 
         *  parámetros con los niveles de la facetas de la lista del objeto que realiza la llamada.
         *  Si son iguales y en el mismo orden devuelve true, en otro caso devuelve false.
         * Parámetros:
         *  List<Facet> list: Lista de facetas.
         */
        private bool CompareLevelsOfFacetsList(ListFacets list)
        {
            /*
             * La variable de retorno res la inicializamos comparando el número de facetas.
             * Si no son iguales no hace falta que comparemos.
             */
            bool res = this.listFacets.Count() == list.Count(); // variable de retorno
            if (res)
            {
                // si tienen el mismo número comparamos uno a uno los nivelse
                int size = list.Count();
                for (int i = 0; i < size; i++)
                {
                    if (!this.listFacets.FacetInPos(i).Level().Equals(list.FacetInPos(i).Level()))
                    {
                        // si no son iguales devolvemo false y no es necesario comparar el resto
                        return res;
                    }
                }
            }
            return res;
        }


        /*
         * Descripción:
         *  Cambia el nombre del fichero por el nuevo que se pasa como parámetro.
         * Parámetros:
         *      string nameFileObs: Nombre del fichero.
         * Excepciones:
         *      Lanza una excepción MultiFacetException si el parámetro es la cadena vacía o null.
         */
        public void NameFileObs(string nameFileObs)
        {
            if (nameFileObs == null || nameFileObs.Length == 0)
            {
                throw new MultiFacetObsException("No ha introducido nombre de fichero");
            }
            this.nameFileObs = nameFileObs;
        }


        /* Descripción:
         *  Introduce asigna el texto de comentario.
         */
        public void Comment(string comment)
        {
            this.comment = comment;
        }


        /*
         * Descripción:
         *  Cambia la descripción/comentario del fichero.
         * Parámetros:
         *      string desc: Descripción/comentario del fichero.
         */
        public void DescriptionFile(string desc)
        {
            this.description = desc;
        }


        /*
         * Descripción:
         *  Cambia la tabla de observaciones.
         */
        public void ObservationTable(InterfaceObsTable obsTable)
        {
            this.observationTable = obsTable;
        }


        /*
         * Descripción:
         *  Añade los datos a la tabla de observaciones multifaceta. La última columna es la que
         *  contiene los datos.
         */
        public void AssignDataToTheTableObs(List<Double?> listDataObs)
        {
            this.observationTable.AssignListData(listDataObs);
        }

        #endregion Métodos de instancia


        #region Métodos para devolver una tabla de observaciones donde no aparezcan las facetas omitidas
        /********************************************************************************************
         * Métodos para devolver una tabla de observaciones donde no aparezcan las facetas omitidas
         * 
         *  - OmitFacetInDataTable()
         ********************************************************************************************/

        /* Descripción:
         *  Operación que devuelve un MultiFacetObs al que se le ha aplicada la omisión de las facetas 
         *  marcadas para ser omitidas (nota: se ignora 'zero', no se pueden tomar nulos como cero).
         *  Se hace la media de los valores de las filas colapsadas.
         */
        public MultiFacetsObs OmitFacetInDataTable()
        {
            ListFacets c_lf = this.listFacets.ListFacetsWithoutOmit();
            ObsTable c_Table = new ObsTable(c_lf, this);

            return new MultiFacetsObs(c_lf, c_Table, this.nameFileObs, this.description, this.comment);
        }

        #endregion Métodos para devolver una tabla de observaciones donde no aparezcan las facetas omitidas



        #region Métodos para la eliminación de niveles de una tabla de observaciones
        /********************************************************************************************
         * Métodos para la eliminación de niveles de una tabla de observaciones
         ********************************************************************************************/

        /* Descripción:
         *  Operación que devuelve un MultiFacetObs al que se le ha aplicada la eliminación de los 
         *  niveles marcados como omitidos en sus respectivas facetas.
         */
        public MultiFacetsObs SkipIndexLevelFacetInDataTable()
        {
            MultiFacetsObs retVal = this.Clone();
            retVal.ObservationTable().SkipLevels(retVal.ListFacets());

            return retVal;
        }


        /* Descripción:
         *  Operación que devuelve un MultiFacetObs al que se le ha 
         *  eliminado todo rastro de omisión de niveles de la tabla y lista de facetas.
         */
        public MultiFacetsObs RestoreIndexLevelFacetInDataTable()
        {
            MultiFacetsObs retVal = this.Clone();
            retVal.AuxRestoreIndexLevelFacetInDataTable();

            return retVal;
        }


        /* Descripción:
         *  Elimina todo rastro de omisión de niveles de la tabla y lista de facetas.
         */
        private void AuxRestoreIndexLevelFacetInDataTable()
        {
            int numFacet = this.listFacets.Count();
            for (int i = 0; i < numFacet; i++)              //for each facet
            {
                Facet f = this.listFacets.FacetInPos(i);
                List<int> lSkipLevels = f.ListSkipLevels();
                int n = lSkipLevels.Count;

                for (int j = 0; j < n; j++)                  //for each level to skip
                {
                    int skipLevel = lSkipLevels[j];
                    // los elementos mayores que j los sustituimos por j-1
                    this.observationTable.RestoreIndexes(skipLevel, i);

                    f.Level(f.Level() - 1);             // actualizamos el nivel
                    f.SetSkipLevels(skipLevel);  // eliminamos el nivel de la lista de niveles omitidos
                }
            }
        }// end AuxSkipLevelFacetInDataTable

        #endregion Métodos para la eliminación de niveles de una tabla de observaciones



        #region Escritura y Lectura de ficheros
        /********************************************************************************************
         * Escritura y Lectura de ficheros 
         ********************************************************************************************/

        /* Descripción:
         *  Escribe un fichero CSV que contiene la tabla de observaciones completa
         *  (facetas + medición), incluyendo una fila de cabecera con los nombres de las facetas
         *  y la variable dependiente. Los valores nulos se escriben como celdas vacías.
         *  
         * Parámetros:
         *  fileName: Ruta del fichero CSV de salida.
         *  lf: Lista de facetas (para los nombres de columna).
         */
        public void WritingFileObsTableCsv(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                InterfaceObsTable table = this.ObservationTable();

                // 1. Escribir cabecera: nombres de facetas + variable dependiente
                List<string> headers = new List<string>();
                for (int i = 0; i < this.listFacets.Count(); i++)
                {
                    headers.Add(EscapeCsvField(this.listFacets.FacetInPos(i).Name()));
                }
                headers.Add(EscapeCsvField("Measurement Variable"));
                writer.WriteLine(string.Join(",", headers));

                // 2. Escribir filas de datos
                int numFacets = this.listFacets.Count();
                int rows = table.TableRows();
                int cols = table.TableColumns(); // facets + 1 (measurement)

                for (int r = 0; r < rows; r++)
                {
                    List<string> fields = new List<string>();

                    for (int c = 0; c < cols; c++)
                    {
                        double? value = table.Data(r, c);
                        if (value.HasValue)
                        {
                            // Facet columns are integers, measurement may be fractional
                            if (c < numFacets)
                                fields.Add(((int)value.Value).ToString());
                            else
                                fields.Add(value.Value.ToString(CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            fields.Add(""); // null → empty cell
                        }
                    }

                    writer.WriteLine(string.Join(",", fields));
                }
            }
        }

        /*  Descripción:
         *   Escapa un valor para CSV: si contiene comas, comillas o saltos de línea, 
         *   lo envuelve entre comillas y duplica las comillas internas.
         */
        private static string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return "";

            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
            {
                return "\"" + field.Replace("\"", "\"\"") + "\"";
            }

            return field;
        }

        /* Descripción:
         *  Escribe un fichero que contiene las puntuaciones almacenadas en la tabla de observaciones.
         *  Dicho fichero almacena los datos secuencialmente, uno por línea, y además los valores nulos
         *  son exportados como 0, de esta mamera los datos pueden ser luego usado por EduG 6.0.
         */
        public void WritingFileDataScore(String fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                InterfaceObsTable table = this.ObservationTable();
                int n = table.TableRows();
                for (int i = 0; i < n; i++)
                {
                    double d = table.ObsData(i) ?? 0;
                    writer.WriteLine(d.ToString());
                }
            }
        }


        /* Descripción:
         *  Es una operación auxiliar que se usa para escribir una a una cada linea del comentario y
         *  manterner el retorno de carro.
         * Nota: Si se envia un string null como parámetro, escribira la cadena vacía.
         */
        private void WriteString(StreamWriter writer, string txt)
        {
            if (txt == null)
            {
                writer.WriteLine("");
            }
            else
            {
                char[] delimeterChars = { '\n' }; // nuestro delimitador será el caracter '/'
                string[] arrayline = txt.Split(delimeterChars);
                int n = arrayline.Length;
                for (int i = 0; i < n; i++)
                {
                    writer.WriteLine(arrayline[i]);
                }
            }
        }

        /* Descripción:
         *  Devuelve una lista de double correspondiente a las puntuaciones recuperadas del
         *  fichero de datos. Una puntuación por línea para que pueda recuperar los datos 
         *  exportados desde EduG.
         */
        public static List<double> ReadingFileDataScore(String fileName)
        {
            List<double> res = new List<double>(); // Variable de retorno

            using (StreamReader reader = new StreamReader(fileName))
            {
                try
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {

                        res.Add((Double)ConvertNum.String2Double(line));
                    }
                }
                catch (FormatException ex)
                {
                    throw new MultiFacetObsException("Error de formato de fichero: " + ex.Message);
                }

            }
            return res;
        }

        #endregion Escritura y Lectura de ficheros


        #region Escritura y lectura de stream

        /* Descripción:
         *  Escribe los datos de un objeto de multiples fuentes de variabilidad en un stream.
         */
        public bool WritingFileObsData(StreamWriter writer)
        {
            bool res = false;
            writer.WriteLine(this.DescriptionFile());
            res = this.listFacets.WritingStreamListFacets(writer);
            if (res)
            {
                res = this.observationTable.WritingStreamObsTable(writer);
                if (res)
                {
                    // escribimos el comentario
                    writer.WriteLine(BEGIN_COMMENT);
                    WriteString(writer, this.comment);
                    writer.WriteLine(END_COMMENT);
                    return res;
                }
                else
                {
                    throw new MultiFacetObsException("Error al escribir los datos");
                }
            }
            else
            {
                throw new MultiFacetObsException("Error al escribir los datos");
            }
        }


        /*
        * Descripción:
        *  Devuelve un objeto multifaceta. El método de Lectura en una streamreader. Los datos 
        *  del elemento multifaceta se leen a través del streamReader y a partir de ellos se 
        *  construye un objeto MutiFacetObs.
        * Devuelve:
        *  MultiFacetsObs: El objeto que se construye con los datos obtenidos en el stream;
        */
        public static MultiFacetsObs ReadingFileObsData(StreamReader reader, string nameFile)
        {
            try
            {
                string line;

                // Read description line
                string descriptionFile = reader.ReadLine();

                // Read BEGIN_LISTFACETS marker
                if ((line = reader.ReadLine()) == null || !line.Equals(MultiFacetData.ListFacets.BEGIN_LISTFACETS))
                {
                    throw new MultiFacetObsException(
                        $"Expected '{MultiFacetData.ListFacets.BEGIN_LISTFACETS}' but found '{line}' while reading multifacets.");
                }

                // Parse list of facets
                ListFacets lf = MultiFacetData.ListFacets.ReadingStreamListFacets(reader);
                var res = new MultiFacetsObs(lf, nameFile, descriptionFile);

                // Read BEGIN_OBS_TABLE marker
                if ((line = reader.ReadLine()) == null || !line.Equals(MultiFacetData.ObsTable.BEGIN_OBS_TABLE))
                {
                    throw new MultiFacetObsException(
                        $"Expected '{MultiFacetData.ObsTable.BEGIN_OBS_TABLE}' but found '{line}' while reading multifacets.");
                }

                // Parse obs table
                res.observationTable = ObsTable.ReadingStreamObsTable(reader);

                // Read BEGIN_COMMENT marker
                if ((line = reader.ReadLine()) == null || !line.Equals(BEGIN_COMMENT))
                {
                    throw new MultiFacetObsException(
                        $"Expected '{BEGIN_COMMENT}' but found '{line}' while reading multifacets.");
                }

                // Read comment content
                StringBuilder commentBuilder = new System.Text.StringBuilder();
                while ((line = reader.ReadLine()) != null && !line.Equals(END_COMMENT))
                {
                    if (commentBuilder.Length > 0)
                        commentBuilder.Append('\n');
                    commentBuilder.Append(line);
                }
                if (line == null)
                {
                    throw new MultiFacetObsException("Unexpected end of file while reading comment section in multifacets.");
                }

                res.Comment(commentBuilder.ToString());
                return res;
            }
            catch (ListFacetsException ex)
            {
                throw new MultiFacetObsException("Error in multifacets.", ex);
            }
            catch (ObsTableException ex)
            {
                throw new MultiFacetObsException("Error in multifacets.", ex);
            }

        }


        #endregion Escritura y lectura de stream



        /*
         * Descripción:
         *  Devuelve true si la lista de facetas que se pasa como parámetro tiene todas sus facetas
         *  contenidas en la lista de facetas que se pasa como parámetro implicito. False en caso de
         *  que alguna de sus facetas no este contenida.
         * Parámetros:
         *      ListFacets lf: Lista de facetas que queremos comprobar.
         */
        public bool CheckMembershipOfFacets(ListFacets lf)
        {
            return this.listFacets.ContainsList(lf);
        }

        #region Converstión en DataSet
        /* Descripción:
         *  Devuelve un DataSet con los datos del objeto
         */
        public DataSet MultiFacetObs2DataSet()
        {
            // Creamos el dataSet que se devolvera
            DataSet dsMultiFacetData = new DataSet("DataSet_MultiFacetData");
            // Creamos el DataTable
            DataTable dt_mfo = new DataTable("TbMultiFacetObs");
            // creamos los columnas
            DataColumn c_name_file = new DataColumn("name_file", System.Type.GetType("System.String"));
            DataColumn c_description = new DataColumn("description", System.Type.GetType("System.String"));
            DataColumn c_comment = new DataColumn("comment", System.Type.GetType("System.String"));
            // Añadimos las columnas
            dt_mfo.Columns.Add(c_name_file);
            dt_mfo.Columns.Add(c_description);
            dt_mfo.Columns.Add(c_comment);
            // Añadimos el DataTable al dataSet
            dsMultiFacetData.Tables.Add(dt_mfo);
            // Creamos una nueva fila
            DataRow new_mfo_Row = dsMultiFacetData.Tables["TbMultiFacetObs"].NewRow();
            // Rellenamos la fila
            new_mfo_Row["name_file"] = this.nameFileObs;
            new_mfo_Row["description"] = this.description;
            new_mfo_Row["comment"] = this.comment;
            // Añadimos la fila al dataTable del dataSet
            dsMultiFacetData.Tables["TbMultiFacetObs"].Rows.Add(new_mfo_Row);

            // Obtenemos el dataTable con las facetas
            DataTable dtListFacets = this.listFacets.ListFacets2DataTable("TbFacets");
            // Añadimos el dataTable de las facetas
            dsMultiFacetData.Tables.Add(dtListFacets);
            // Obtenemos el dataTable con los nivels omitidos
            DataTable dtSkipLevels = this.listFacets.SkipLevels2DataTable("TbSkipLevels");
            // Añadimos el dataTable con los niveles omitidos
            dsMultiFacetData.Tables.Add(dtSkipLevels);
            // Obtenemos el dataTable con la tabla de observaciones
            DataTable dtObsTable = this.observationTable.ObsTable2DataTable(this.listFacets);
            // Añadimos el dataTable con la tabla de observaciones
            dsMultiFacetData.Tables.Add(dtObsTable);

            return dsMultiFacetData;
        }// end MultiFacetObs2DataTable


        /* Descripción:
         *  Dado un DataSet con el formato especicado en la clase devuelve un objeto multifaceta
         */
        public static MultiFacetsObs DataSet2MultiFacetObs(DataSet dsMultiFacetData)
        {
            DataTable dt_mfo = dsMultiFacetData.Tables["TbMultiFacetObs"];
            string name_file = (string)dt_mfo.Rows[0]["name_file"];
            string description = (string)dt_mfo.Rows[0]["description"];
            string comment = (string)dt_mfo.Rows[0]["comment"];
            DataTable dtListFacets = dsMultiFacetData.Tables["TbFacets"];
            DataTable dtSkipLevels = dsMultiFacetData.Tables["TbSkipLevels"];
            ListFacets lf = MultiFacetData.ListFacets.DataTables2ListFacets(dtListFacets, dtSkipLevels);
            DataTable dtObsTable = dsMultiFacetData.Tables["TbObsTable"];
            InterfaceObsTable obsTable = ObsTable.DataTable2ObsTable(dtObsTable);

            return new MultiFacetsObs(lf, obsTable, name_file, description, comment);
        }// end DataSet2MultiFacetObs

        #endregion Converstión en DataSet

        #region Clonación
        public MultiFacetsObs Clone()
        {
            ListFacets lf = this.listFacets.DeepClone();
            string nameFile = this.nameFileObs;
            string comment = this.comment;
            string description = this.description;
            ObsTable obsTable = this.observationTable.Clone();
            return new MultiFacetsObs(lf, obsTable, nameFile, description, comment);
        }
        #endregion Clonación

        #region Métodos redefinidos
        /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
         * Métodos rededifinidos
         *++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        /*
         * Descripción:
         *  Redefinición del método ToString
         */
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            // primero incluimos la lista de facetas
            res.Append(this.nameFileObs + "\n");
            res.Append(this.description + "\n");
            foreach (Facet f in this.listFacets)
            {
                res.Append(f.ToString() + "\n");
            }
            res.Append(this.comment + "\n");
            res.Append(this.observationTable.ToString());
            return res.ToString();
        }

        #endregion Métodos redefinidos

    }// end public class MultiFacetsObs
}// end namespace MultiFacetData