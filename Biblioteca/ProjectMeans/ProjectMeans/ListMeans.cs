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
 * Fecha de revisión: 10/Jul/2012
 * 
 */
using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using MultiFacetData;

namespace ProjectMeans
{
    public class ListMeans : IEnumerable
    {
        /*======================================================================================
         * Constantes
         *======================================================================================*/

        // Constantes para usarlas como marcas en el fichero
        // Comienzo y fin de comentario de fichero de lista de medias
        const string BEGIN_COMMENT = "<file_means_comment>";
        const string END_COMMENT = "</file_means_comment>";
        // Comienzo y fin de lista de medias
        const string BEGIN_LIST_MEANS = "<listmeans>";
        const string END_LIST_MEANS = "</listmeans>";
        // internal const string STRING_NULL = "NULL";

        /*======================================================================================
         * Variables
         *======================================================================================*/
        // Variables de instancia;
        // private List<TableMeans> listTableMeans;
        private List<InterfaceTableMeans> listTableMeans;
        private DateTime dateCreation; // fecha hora de la creación.
        private string nameFileDataCreation; // Nombre del fichero de datos
        private string textComment; // texto con comentarios


        #region Constructores de la clase ListMeans
        /*======================================================================================
         * Constructores
         *======================================================================================*/

        /*
         * Descripción:
         *  Constructor por defecto de una lista de Tablas de medias;
         */
        public ListMeans()
        {
            // this.listTableMeans = new List<TableMeans>();
            this.listTableMeans = new List<InterfaceTableMeans>();
        }
        /*
         * Descripción:
         *  Constructor de la clase ListsTableSSQ. Se le pasa por parámetro una lista de Facetas.
         *  (No puede haber dos facetas con el mismo nombre).
         * Parámetros:
         *      List<Facet> listF: Lista de Facetas.
         *      MultiFacetObs mfo: contiene el objeto multifaceta con los datos con los que se 
         *              construirá la tabla.
         *      DateTime date: fecha en la que se creo el fichero de medias.
         *      String nameFile: Nombre del fichero de datos a partir del cual se optienen las medias.
         *      bool zero: Si es true se realizarán los calculos interpretando los valores nulos como
         *              ceros.
         * Excepciones:
         *      Lanza una excepción ListFacetExceptions sí la lista que se le pasa como 
         *      parámetro tiene dos facetas con el mismo nombre.
         */
        public ListMeans(List<ListFacets> llf, List<string> ldesign, MultiFacetsObs mfo, DateTime date, string nameFile, bool zero)
        {
            this.dateCreation = date;
            this.nameFileDataCreation = nameFile;
            int n = llf.Count;
            for (int i = 0; i < n; i++)
            {
                ListFacets lf = llf[i];
                string design = ldesign[i];
                this.listTableMeans.Add(new TableMeans(lf, design, mfo, zero));
            }
        }
        #endregion Constructores de la clase ListMeans



        #region Métodos de Consulta
        /*======================================================================================
         * Métodos de Consulta
         * ===================
         *  - GetDateTime
         *  - GetNameFileDataCreation
         *  - GetTextComment
         *  - TableMeansInPos
         *  - Count
         *  - RetListTableMeans
         *======================================================================================*/

        /* Descripción:
         *  Devuelve la fecha de creación.
         */
        public DateTime GetDateTime()
        {
            return this.dateCreation;
        }

        /* Descripción:
         *  Devuelve el nombre del fichero del cual proceden los datos.
         */
        public string GetNameFileDataCreation()
        {
            return this.nameFileDataCreation;
        }

        /* Descripción:
         *  Devuelve el texto con los comentarios
         */
        public string GetTextComment()
        {
            return this.textComment;
        }

        /*
         * Descripción:
         *  Devuelve la Tabla de medias existente en una posición de la lista de Tabla de medias.
         * Parámetros:
         *      int i: posición en la que se encuentra la faceta que queremos consulatar.
         */
        public InterfaceTableMeans TableMeansInPos(int pos)
        {
            if (pos < 0 || pos >= this.listTableMeans.Count)
            {
                // lanzamos una excepción de indice fuera de rango
                throw new ListMeansException("Indice fuera del rango de la lista de Tabla de medias");
            }
            return this.listTableMeans[pos];
        }

        /*
         * Descripción:
         *  Devuelve el número de Tabla de medias que contiene la lista.
         */
        public int Count()
        {
            return this.listTableMeans.Count;
        }

        /*
         * Descripción:
         *  Devuelve la lista de tabla de medias.
         */
        public List<InterfaceTableMeans> RetListTableMeans()
        {
            return this.listTableMeans;
        }

        #endregion Métodos de Consulta



        #region Métodos de instancia
        /*======================================================================================
         * Métodos de instancia
         * ====================
         *  - SetDateTime
         *  - SetNameFileDataCreation
         *  - SetTextComment
         *  - Add
         *  - Remove
         *======================================================================================*/

        /* Descripción:
         *  Asigna la fecha de creación.
         */
        public void SetDateTime(DateTime date)
        {
            this.dateCreation = date;
        }

        /* Descripción:
         *  Asigna el nombre del fichero del cual proceden los datos.
         */
        public void SetNameFileDataCreation(string nameFile)
        {
            this.nameFileDataCreation = nameFile;
        }

        /* Descripción:
         *  Asigna el texto con los comentarios
         */
        public void SetTextComment(string text)
        {
            this.textComment = text;
        }

        /*
         * Descripción:
         *  Añade una Tabla de medias al objeto de la clase ListTableMeans, lo hará en última posición.
         * Parámetros:
         *      TableMeans tms: Tabla de medias que queremos añadir a nuestro objeto de la clase ListTableMeans
         */
        /*public void Add(TableMeans tms)
        {
            this.listTableMeans.Add(tms);
        }*/

        /*
         * Descripción:
         *  Añade una Tabla de medias al objeto de la clase ListTableMeans, lo hará en última posición.
         * Parámetros:
         *      TableMeansDif tms: Tabla de medias de diferenciación que queremos añadir a nuestro objeto 
         *      de la clase ListTableMeans
         */
        /*public void Add(TableMeansDif tms)
        {
            this.listTableMeans.Add(tms);
        }*/

        /*
         * Descripción:
         *  Añade una Tabla de medias al objeto de la clase ListTableMeans, lo hará en última posición.
         * Parámetros:
         *      TableMeansDif tms: Tabla de medias de diferenciación que queremos añadir a nuestro objeto 
         *      de la clase ListTableMeans
         */
        public void Add(InterfaceTableMeans tms)
        {
            this.listTableMeans.Add(tms);
        }

        /*
         * Descripción:
         *  Elimina un elemento pasado como parámetro de la lista. Si el elemento es eliminado
         *  correctamente devuelve true, en otro caso devuelve false.
         * Parámetros:
         *      TableMeans tms: Tabla de medias que queremos eliminar.
         */
        public bool Remove(TableMeans tms)
        {
            return this.listTableMeans.Remove(tms);
        }

        #endregion Métodos de instancia



        /*
         * Descripción:
         *  Método de la interfaz IEnumerable
         */
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.listTableMeans.GetEnumerator();
        }


        #region Escritura de un fichero de medias
        /*======================================================================================
         * Escritura de ficheros de medias
         *======================================================================================*/

        /* Descripción:
         *  Método de escritura en una archivo de medias.
         * Devuelve:
         *  bool: True si se ha escrito correctamente false en otro caso;
         */
        public bool WritingFileListMeans(String fileName)
        {
            bool res = false; // variable de retorno

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                res = StringWriterFileListMeans(writer);
            }
            return res;
        }// end public bool WritingFileListMeans


        public bool StringWriterFileListMeans(StreamWriter writer)
        {
            writer.WriteLine(this.nameFileDataCreation);// escribimos el path del fichero con el que se crearon los datos
            writer.WriteLine(this.dateCreation.ToString(new CultureInfo("es-ES", true)));// fecha en la se creo el archivo
            // escribimos el comentario
            writer.WriteLine(BEGIN_COMMENT);
            // writer.WriteLine(this.textComment);
            writeString(writer, this.textComment);
            writer.WriteLine(END_COMMENT);

            // escribimos la lista de tablas de medias
            writer.WriteLine(BEGIN_LIST_MEANS);
            int n = this.listTableMeans.Count;
            for (int i = 0; i < n; i++)
            {
                this.listTableMeans[i].WritingStreamTableMeans(writer);
            }
            writer.WriteLine(END_LIST_MEANS);
            return true;
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

        #endregion Escritura de un fichero de medias



        #region Lectura de un fichero de medias
        /*======================================================================================
         * Lectura de ficheros de medias
         *======================================================================================*/
        /* Descripción:
         *  Método de Lectura en una archivo de medias. Los datos del archivo pasa al objeto
         *  ListMeans desde el que se hace la llamado por lo que se perderan los
         *  datos de este.
         */
        public static ListMeans ReadingFileListMeans(String fileName)
        {
            ListMeans retListMeans = new ListMeans();
            using (StreamReader reader = new StreamReader(fileName))
            {
                retListMeans = StreamReaderFileListMeans(reader);
            }// end using
            return retListMeans;
        }// end public static ListMeans ReadingFileListMeans


        /* Descripción:
         *  Método de Lectura en una stream de medias. Los datos del archivo pasa al objeto
         *  ListMeans desde el que se hace la llamado por lo que se perderan los
         *  datos de este.
         */
        public static ListMeans StreamReaderFileListMeans(StreamReader reader)
        {
            ListMeans retListMeans = new ListMeans();
            try
            {
                string nameFile = reader.ReadLine(); // path del fichero del que se extrajeron los datos
                retListMeans.SetNameFileDataCreation(nameFile); // asignamos nombre
                string[] timeFormats = { "d/MM/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy H:mm:ss" };
                DateTime dateFile = DateTime.ParseExact(reader.ReadLine(), timeFormats,
                                   new CultureInfo("es-ES", true), DateTimeStyles.AssumeLocal);
                retListMeans.SetDateTime(dateFile); // asignamos fecha

                // leemos el comentario
                string line;
                string txt = "";
                if (!(line = reader.ReadLine()).Equals(BEGIN_COMMENT))
                {
                    throw new ListMeansException("Error en la lectura de fichero");
                }
                
                txt = reader.ReadLine();

                while (!(line = reader.ReadLine()).Equals(END_COMMENT))
                {
                    txt = txt + "\n" + line;
                }
                // finalizamos la lectura de comentarios

                retListMeans.SetTextComment(txt); // asignamos comentario

                if ((line = reader.ReadLine()).Equals(BEGIN_LIST_MEANS))
                {
                    while (!(line = reader.ReadLine()).Equals(END_LIST_MEANS))
                    {
                        if (line.Equals(TableMeans.BEGIN_TABLE_MEANS))
                        {
                            TableMeans tb = TableMeans.ReadingStreamTableMeans(reader);
                            retListMeans.Add(tb);
                        }
                        else if (line.Equals(TableMeansDif.BEGIN_TABLE_MEANS_DIFF))
                        {
                            TableMeansDif tb = TableMeansDif.ReadingStreamTableMeans(reader);
                            retListMeans.Add(tb);
                        }
                        else if (line.Equals(TableMeansTypScore.BEGIN_TABLE_MEANS_TYPICAL_SCORE))
                        {
                            TableMeansTypScore tb = TableMeansTypScore.ReadingStreamTableMeans(reader);
                            retListMeans.Add(tb);
                        }
                    }
                }
                else
                {
                    throw new TableMeansException();
                }
            }
            catch (TableMeansException)
            {
                throw new ListMeansException("Error al leer de fichero");
            }
            catch (TableMeansTypScoreException)
            {
                throw new ListMeansException("Error al leer de fichero");
            }
            return retListMeans;
        }// end StreamReaderFileListMeans

        #endregion Lectura de un fichero de medias


        #region Conversión entre ListMeans y DataSet
        /* Descripción:
         *  Devuelve una Lista de DataSet con los datos
         */
        public DataSet[] ListMeans2DataSet()
        {
            int nMeans = this.listTableMeans.Count;
            // Variable de retorno
            DataSet[] lDataSet = new DataSet[nMeans + 1];

            // Creamos el DataSet
            DataSet dsListMeans = new DataSet("dsListMeans");
            // Creamos el Datatable
            DataTable dtListMeans = new DataTable("TbListMeans");
            // Añadimos los datos
            dtListMeans.Columns.Add(new DataColumn("name_file_data_creation" ,System.Type.GetType("System.String")));
            dtListMeans.Columns.Add(new DataColumn("date_creation", System.Type.GetType("System.DateTime")));
            dtListMeans.Columns.Add(new DataColumn("text_comment", System.Type.GetType("System.String")));
            
            // Insertamos los datos
            DataRow row = dtListMeans.NewRow();
            row["name_file_data_creation"] = this.nameFileDataCreation;
            row["date_creation"] = this.dateCreation;
            row["text_comment"] = this.textComment;
            dtListMeans.Rows.Add(row);
            dsListMeans.Tables.Add(dtListMeans);

            // Añadimos el dataTable a la lista
            lDataSet[0] = dsListMeans;

            // añadimos el resto de elementos a la lista
            for (int i = 0; i < nMeans; i++)
            {
                DataSet newDataSet = this.listTableMeans[i].TableMeans2DataSet();
                lDataSet[i+1] = newDataSet;
            }
            
            return lDataSet;
        }// end ListMeans2DataSet


        /* Descripción:
         *  Obtiene la lista de medias a partir de una lista de dataSets
         */
        public static ListMeans ListDataSet2ListMeans(DataSet[] lDataSets)
        {
            // Variable de retorno
            ListMeans listMeans = new ListMeans();

            DataSet ds = lDataSets[0];
            DataTable dt = ds.Tables["TbListMeans"];
            string name_file_creation = (string)dt.Rows[0]["name_file_data_creation"];
            listMeans.SetNameFileDataCreation(name_file_creation);
            DateTime dateCreation = (DateTime)dt.Rows[0]["date_creation"];
            listMeans.SetDateTime(dateCreation);
            string text_comment = "";
            if (dt.Rows[0]["text_comment"] != null)
            {
                text_comment = (string)dt.Rows[0]["text_comment"];
            }
            listMeans.SetTextComment(text_comment);
            
            int numDataSet = lDataSets.Length;
            for (int i = 1; i < numDataSet; i++)
            {
                ds = lDataSets[i];
                dt = ds.Tables["TbMeans"];
                string typeMeans = (string)dt.Rows[0]["type_means"];

                InterfaceTableMeans tbMeans;
                switch (typeMeans)
                {
                    case ("tableMeans"):
                        tbMeans = TableMeans.DataSet2TableMeans(ds);
                        // Añadimos la tabla de medias
                        listMeans.Add(tbMeans);
                        break;
                    case ("tableMeansDif"):
                        tbMeans = TableMeansDif.DataSet2TableMeans(ds);
                        // Añadimos la tabla de medias
                        listMeans.Add(tbMeans);
                        break;
                    case ("tableMeansTypScore"):
                        tbMeans = TableMeansTypScore.DataSet2TableMeans(ds);
                        // Añadimos la tabla de medias
                        listMeans.Add(tbMeans);
                        break;
                }
            }

            return listMeans;
        }// end ListDataSet2ListMeans


        #endregion Conversión entre ListMeans y DataSet


    }// end public class ListMeans : IEnumerable
}// end namespace ProjectMeans
