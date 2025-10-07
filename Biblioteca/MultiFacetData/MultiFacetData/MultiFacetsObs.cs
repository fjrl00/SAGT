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
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using AuxMathCalcGT;

namespace MultiFacetData
{
    public class MultiFacetsObs
    {
        #region Constantes y variables de instancia
        /*=================================================================================
         * Constantes
         *=================================================================================*/
        // internal const string BEGIN_MULTI_FACET_OBS = "<multi_facet_obs>";
        public const string BEGIN_MULTI_FACET_OBS = "<multi_facet_obs>";
        public const string END_MULTI_FACET_OBS = "</multi_facet_obs>";
        // Comienzo y fin de comentario de una tabla de frecuencias
        const string BEGIN_COMMENT = "<file_data_comment>";
        const string END_COMMENT = "</file_data_comment>";

        /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
         * Variables de instancia
         *++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/
        private ListFacets listFacets; // lista de facetas
        private string nameFileObs; // Nombre del fichero de observaciones
        private string description; // posible descripción del archivo
        // private ObsTable observationTable; // tabla de observacion.
        private InterfaceObsTable observationTable; // tabla de observacion.

        private string comment; //;
        #endregion

        #region Constructores
        /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
         * Constructores
         *++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        // Constructor vacio para que sea serializable
        public MultiFacetsObs()
        {
        }

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
            this.CreateTableObs();
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
            :this(listFacets, nameFileObs, description)
        {
            this.comment = comment;
        }


        /*
         * Descripción:
         *  Operación auxiliar. Comprueba que la lista de facetas sea válida. Para ello no puede
         *  haber dos facetas con el mismo nombre/etiqueta.
         * Devuelve:
         *  bool: True si la lista de facetas no contiene nombres repetidos. False en otro caso.
         */
        private static bool CheckListNameFacets(List<Facet> lFacets)
        {
            bool res = true; // variable de retorno

            List<string> lNames = new List<string>();
            foreach (Facet f in lFacets)
            {
                lNames.Add(f.Name().ToUpper());
            }

            lNames.Sort();

            int numFacets = lNames.Count-1; // restamos uno para evitar que se salga del rango
            for (int i = 0; i < numFacets; i++)
            {
                //res = lNames[i].Equals(lNames[i+1]);
                if (lNames[i].Equals(lNames[i + 1]))
                {
                    return false;
                }
            }

            return res;
        }


        /*
         * Descripción:
         *  Método auxiliar. Inicializa la tabla de observaciones. Los datos estan inicialmente a null.
         * Excepciones:
         *  MultiFacetException: si tiene menos de 2 facetas la lista de facetas.   
         */
        private void CreateTableObs()
        {
            if (this.listFacets.Count() < 2)
            {
                throw new MultiFacetObsException("No se puede crear la tabla. Debe haber al menos 2 facetas.");
            }
            this.observationTable = new ObsTable(this.listFacets);
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
         *  Devuelve los comentarios de la tabla de frecuencias
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
         *  Cambia la lista de facetas del objeto MultFaceObs. Se permite modificar los nombres
         * Parámetros:
         *      List<Facet> list: Lista de facetas;
         * Excepciones:
         *      Lanza una excepción MultiFacetException:
         *          Si no tiene al menos dos facetas.
         *          Si la lista de facetas no coincide en número con la facetas asignada.
         *          Si los niveles de la nueva lista no coinciden con los de la lista 
         *              del objeto.
         *      Lanza una excepción ListFacetsException
         *          Si tiene el nombre de alguna faceta repetido (no puede haber dos 
         *              facetas con el mismo nombre).
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
            }           this.listFacets = list;
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
            bool res = (this.listFacets.Count() == list.Count()); // variable de retorno
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
         * Excepciones:
         *  Lanza una excepción de tipo MultiFacetException si la lista de datos no coincide con el
         *  número de columnas de la aplicación.
         */
        public void AssignDataToTheTableObs(List<Double?> listDataObs)
        {
            if (listDataObs.Count != this.observationTable.ObsTableRows())
            {
                throw new MultiFacetObsException("Los datos no coinciden con la dimensión de la tabla.");
            }
            this.observationTable.AssignListData(listDataObs);
        }


        /*
         * Descripción:
         *  Devuelve una lista de tipo double? con las observaciones que se encuentran en la 
         *  última columna de la tabla
         */
        public List<double?> ListObs()
        {
            return this.observationTable.ListObs();
        }

        #endregion Métodos de instancia


        #region Métodos para devolver una tabla de frecuencias donde no aparezcan las facetas omitidas
        /********************************************************************************************
         * Métodos para devolver una tabla de frecuencias donde no aparezcan las facetas omitidas
         * 
         *  - OmitFacetInDataTable()
         *  - AuxCalculateTableObsOmit(MultiFacetsObs mfo)
         ********************************************************************************************/

        /* Descripción:
         *  Operación que devuelve un MultiFacetObs al que se le ha aplicada la omisión de las facetas que no
         *  estan como parámetro.
         */
        public MultiFacetsObs OmitFacetInDataTable()
        {
            ListFacets lfOmit = this.ListFacets().ListFacetWithoutOmit();
            string comment = this.description;
            string nameFile = this.nameFileObs;
            MultiFacetsObs retVal = new MultiFacetsObs(lfOmit, nameFile, comment);
            // aplicamos la reducción de la tabla de frecuencias
            retVal.AuxCalculateTableObsOmit(this);
            return retVal;
        }


        /* Descripción:
         *  Operción auxiliar que se emplea para recalcular la tabla de observaciones eliminando las
         *  facetas omitidas. Los valores de la s filas redundandantes se suman.
         */
        private void AuxCalculateTableObsOmit(MultiFacetsObs mfo)
        {
            /* Determinamos el número de facetas de la lista que pasamos como parámetro
             * estas se corresponden con las columnas que necesitamos comparar para calcular los
             * datos estadisticos*/
            int longListFacet = this.listFacets.Count();

            // este array determinna las columnas de la tabla a comparar
            int[] posicionesEnColumnas = new int[longListFacet];

            // obtenemos la lista de facetas del objeto mutifaceta para luego comparar y 
            // obtener las posiciones
            ListFacets mfo_listFacet = mfo.ListFacets();

            // Rellenamos el array
            for (int i = 0; i < longListFacet; i++)
            {
                posicionesEnColumnas[i] = mfo_listFacet.IndexOf(this.listFacets.FacetInPos(i));
            }

            // necesito la tabla de observaciones
            InterfaceObsTable mfo_obsTable = mfo.ObservationTable();
            // necesito el número de filas de la tabla de observaciones para el bucle interior
            int num_rows_mfo_obsTable = mfo_obsTable.ObsTableRows();
            int num_rows_obsTable = this.observationTable.ObsTableRows();

            // Ahora usaremos los valores de la tabla de medias para recorrer la tabla de 
            // observaciones y obtener los datos
            for (int i = 0; i < num_rows_mfo_obsTable; i++)
            {
                bool found = false;
                for (int j = 0; j < num_rows_obsTable && !found; j++)
                {
                    bool v = true;
                    /* Ahora comparo los valores de los indices de la tabla de medias con sus
                     * corespondecia en la tabla de observaciones si coincide lo agrego a la 
                     * tabla de estadistica. */
                    for (int k = 0; k < longListFacet && v; k++)
                    {
                        /* Comparo el valor de laa columnas de indices en la tabla de medias con 
                         * los indices de la tabla de datos. Las columnas de la tabla de datos
                         * las he guardado en el array posicionesEnColumnas. */
                        v = (this.observationTable.Data(j, k).Equals(mfo_obsTable.Data(i, posicionesEnColumnas[k])));
                    }

                    if (v)
                    {
                        found = v;
                        double? d1 = this.observationTable.Data(j);
                        double? d2 = mfo_obsTable.Data(i);
                        if (d1 != null)
                        {
                            if (d2 == null)
                            {
                                this.observationTable.Data(d1, j);
                            }
                            else
                            {
                                d1 = d1 + d2;
                                this.observationTable.Data(d1, j);
                            }
                        }
                        else
                        {
                            if (d2 != null)
                            {
                                this.observationTable.Data(d2, j);
                            }
                        }
                    }
                }
            }
        }// end AuxCalculateTableObsOmit
        #endregion Métodos para devolver una tabla de frecuencias donde no aparezcan las facetas omitidas



        #region Métodos para la eliminación de niveles de una tabla de frecuencias
        /********************************************************************************************
         * Métodos para la eliminación de niveles de una tabla de frecuencias
         ********************************************************************************************/

        /* Descripción:
         *  Operación que devuelve un MultiFacetObs al que se le ha aplicada la eliminación de los 
         *  niveles marcados como omitidos en sus respectivas facetas.
         */
        public MultiFacetsObs SkipIndexLevelFacetInDataTable()
        {
            ListFacets lfSkipLevels = (ListFacets)this.ListFacets().Clone();
            string comment = this.description;
            string nameFile = this.nameFileObs;
            MultiFacetsObs retVal = new MultiFacetsObs(lfSkipLevels, nameFile, comment);
            retVal.AssignDataToTheTableObs(this.ObservationTable().ListObs());
            // aplicamos la reducción de la tabla de frecuencias
            InterfaceObsTable obsTable = retVal.ObservationTable();
            obsTable.SkipLevelIndex(lfSkipLevels);
            return retVal;
        }


        /* Descripción:
         *  Operación que devuelve un MultiFacetObs al que se le ha aplicado la eliminación de los 
         *  niveles marcados como omitidos en sus respectivas facetas.
         */
        public MultiFacetsObs SkipAndRestoreIndexLevelFacetInDataTable()
        {
            ListFacets lfSkipLevels = (ListFacets)this.ListFacets().Clone();
            string comment = this.description;
            string nameFile = this.nameFileObs;
            MultiFacetsObs retVal = new MultiFacetsObs(lfSkipLevels, nameFile, comment);
            retVal.AssignDataToTheTableObs(this.ObservationTable().ListObs());
            // aplicamos la reducción de la tabla de frecuencias
            retVal.AuxSkipLevelFacetInDataTable();
            return retVal;
        }


        /* Descripción:
         *  Reconstruye el objeto eliminando los niveles omitidos de cada faceta.
         */
        private void AuxSkipLevelFacetInDataTable()
        {
            int numFacet = this.listFacets.Count();
            for (int i = 0; i < numFacet; i++)
            {
                Facet f = this.listFacets.FacetInPos(i);
                int level = f.Level();
                List<int> lSkipLevels = f.ListSkipLevels();
                int n = lSkipLevels.Count;

                for(int j = 0; j < n; j++)
                {
                    int skipLevel = lSkipLevels[j];
                    /* Entonces eliminamos todas las apariciones de j en la columna i y
                        * ademas a los elementos mayores que j los sustituimos por j-1
                        */
                    this.observationTable.SkipLevelAndRestoreIndex(skipLevel, i);

                    // actualizamos el nivel
                    f.Level(f.Level() - 1);
                    f.SetSkipLevels(skipLevel, false);
                }
            }
        }// end AuxSkipLevelFacetInDataTable

        #endregion Métodos para la eliminación de niveles de una tabla de frecuencias

        

        #region Escritura y Lectura de ficheros
        /********************************************************************************************
         * Escritura y Lectura de ficheros 
         ********************************************************************************************/

        /*
         * Descripción:
         *  Método de escritura en una archivo.
         * Devuelve:
         *  bool: True si se ha escrito correctamente false en otro caso;
         */
        public bool WritingFileObsData(String fileName)
        {
            bool res = false; // variable de retorno

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                res = this.WritingFileObsData(writer);
                 
            }
            return res;
        }


        /*
         * Descripción:
         *  Método de Lectura en una archivo. Los datos del archivo pasa al objeto
         *  MutiFacetObs desde el que se hace la llamado por lo que se perderan los
         *  datos de este.
         * Devuelve:
         *  bool: True si se ha leido correctamente false en otro caso;
         */
        public static MultiFacetsObs ReadingFileObsData(String fileName)
        {
            MultiFacetsObs res = null; // Variable de retorno
            
            using (StreamReader reader = new StreamReader(fileName))
            {
                try
                {
                    string line;
                    if ((line = reader.ReadLine()).Equals(BEGIN_MULTI_FACET_OBS))
                    {
                        res = MultiFacetsObs.ReadingFileObsData(reader, fileName);
                    }
                    else
                    {
                        throw new MultiFacetObsException("Error al leer fichero");
                    }
                }
                catch (FormatException ex)
                {
                    throw new MultiFacetObsException("Error de formato de fichero: " + ex.Message);
                }
                               
            }
            return res;
        }


        /* Descripción:
         *  Escribe un fichero que contiene las puntuaciones almacenadas en la tabla de observaciones.
         *  Dicho fichero almacena los datos secuencialmente, uno por línea, y además los valores nulos
         *  son exportados como 0, de esta mamera los datos pueden ser luego usado por EduG 6.0.
         */
        public bool WritingFileDataScore(String fileName)
        {
            bool res = false; // variable de retorno

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                InterfaceObsTable table = this.ObservationTable();
                int n = table.ObsTableRows();
                for (int i = 0; i < n; i++)
                {
                    double? d = table.Data(i);
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
         *  Es una operación auxiliar que se usa para escribir una a una cada linea del comentario y
         *  manterner el retorno de carro.
         * Nota: Si se envia un string null como parámetro, escribira la cadena vacía.
         */
        private void writeString(StreamWriter writer, string txt)
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
            writer.WriteLine(BEGIN_MULTI_FACET_OBS);
            writer.WriteLine(this.DescriptionFile());
            res = this.listFacets.WritingStreamListFacets(writer);
            // this.listFacets.WritingListNesting(writer);
            if (res)
            {
                res = this.observationTable.WritingStreamObsTable(writer);
                if (res)
                {
                    // escribimos el comentario
                    writer.WriteLine(BEGIN_COMMENT);
                    writeString(writer, this.comment);
                    writer.WriteLine(END_COMMENT);
                    writer.WriteLine(END_MULTI_FACET_OBS);
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
         *  Devuelve true si todas las facetas de la lista de facetas que se pasa como parámetro 
         *  pertenecen al objeto, false en caso contrario.
         */
        public bool CheckMembershipOfFacets(ListFacets lf)
        {
            bool retVal=true;
            int n = lf.Count();
            for (int i = 0; i < n && retVal; i++)
            {
                retVal = this.listFacets.Contains(lf.FacetInPos(i));
            }
            return retVal;
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
            // Obtenemos el dataTable con la tabla de frecuencias
            DataTable dtObsTable = this.observationTable.ObsTable2DataTable(this.listFacets);
            // Añadimos el dataTable con la tabla de frecuencias
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