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
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace MultiFacetData
{
    public class Facet : System.ICloneable, System.IComparable<Facet>
    {

        #region Variables y Constantes
        /*=================================================================================
         * Constantes
         *=================================================================================*/
        public const string INFINITE = "INF";
        public const string NEST_CHAR = ":";
        // Constantes para usarlas como marcas en el fichero
        const string BEGIN_FACET = "<facet>";
        const string END_FACET = "</facet>";
        const string BEGIN_SKIP_LEVELS = "<skip_levels>";
        const string END_SKIP_LEVELS = "</skip_levels>";

        /*=================================================================================
         * Variables de instancia
         *=================================================================================*/
        private string name; // Nombre de la faceta.
        private int level;  // Nivel de la faceta expresado como un entero positivo.
        private int sizeOfUniverse;
        private string comment; // Comentario/descripción de la faceta.
        private bool omit; // Indica si la faceta se tendrá presente para el estudio
        /*Una faceta puede estar anidada en más de una faceta por eso empleamos una lista. */
        private string list_facets_design;
        // Niveles de facetas omitidos
        private Dictionary<int, bool> skipLevels;
        #endregion Variables y Constantes


        #region Constructores
        /*=================================================================================
         * Constructores
         *=================================================================================*/


        public Facet(string name, int level)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new FacetException("La faceta no tiene nombre.");
            }
            else if (level < 1)
            {
                throw new FacetException("Faceta tiene que tener un nivel positivo (mayor que cero)");
            }
            this.name = name;
            this.level = level;
            this.list_facets_design = "[" + name + "]"; // partimos de una sola faceta
            this.skipLevels = new Dictionary<int, bool>();
            this.sizeOfUniverse = int.MaxValue;
            this.omit = false;
            this.comment = "";
        }


        /*
         * Descripción:
         *  Constructor de la clase Facet.
         *  
         * Parámetros:
         *  string name: nombre/etiqueta de la faceta.
         *  int level: nivel de la faceta. Ha de ser mayor que cero
         *  string comment: descripción de la faceta
         *  
         * Excepciones:
         *  Lanza una excepción de tipo MultiFacetException si la faceta no tiene nombre o si el
         *  nivel de la excepción es menor que 1.
         */
        public Facet(string name, int level, string comment)
            : this(name, level)
        {
            this.comment = comment;
        }



        /* Descripción:
         *  Constructor de la clase Facet.
         *  
         * Parámetros:
         *  string name: nombre/etiqueta de la faceta.
         *  int level: nivel de la faceta. Ha de ser mayor que cero
         *  string comment: descripción de la faceta
         *  bool omit = indica si la faceta esta marcada para su estudio (true) o no (false)
         *  
         * Excepciones:
         *  Lanza una excepción de tipo MultiFacetException si la faceta no tiene nombre o si el
         *  nivel de la excepción es menor que 1.
         */
        public Facet(string name, int level, string comment, bool omit)
            : this(name, level, comment)
        {
            this.omit = omit;
        }


        /* Descripción:
         *  Constructor de la clase Facet.
         *  
         * Parámetros:
         *      string name: nombre/etiqueta de la faceta.
         *      int level: nivel de la faceta. Ha de ser mayor que cero
         *      string comment: descripción de la faceta
         *      int sizeOfUniverse: Tamaño del universo
         * Excepciones:
         *      FacetException: si el nivel es mayor que el universo
         */
        public Facet(string name, int level, string comment, int sizeOfUniverse)
            : this(name, level, comment)
        {
            if (level > sizeOfUniverse)
            {
                throw new FacetException("Error: El nivel no puede ser mayor que el universo");
            }
            this.sizeOfUniverse = sizeOfUniverse;
        }


        /* Descripción:
        *  Constructor de la clase Facet.
        *  
        * Parámetros:
        *       string name: nombre/etiqueta de la faceta.
        *       int level: nivel de la faceta. Ha de ser mayor que cero
        *       string comment: descripción de la faceta
        *       int sizeOfUniverse: Tamaño del universo
        *       bool omit = indica si la faceta esta marcada para su estudio (true) o no (false)
        * Excepciones:
        *      FacetException: si el nivel es mayor que el universo
        */
        public Facet(string name, int level, string comment, int sizeOfUniverse, bool omit)
            : this(name, level, comment, sizeOfUniverse)
        {
            this.omit = omit;
        }


        /* Descripción:
        *  Constructor de la clase Facet.
        *  
        * Parámetros:
        *  string name: nombre/etiqueta de la faceta.
        *  int level: nivel de la faceta. Ha de ser mayor que cero
        *  string comment: descripción de la faceta
        *  string lfd = introducimos el diseño de faceta
        */
        public Facet(string name, int level, string comment, string lfd)
            : this(name, level, comment)
        {
            if (!lfd.StartsWith("[" + name + "]"))
            {
                throw new FacetException("El diseño de la faceta no es correcto");
            }

            this.sizeOfUniverse = int.MaxValue;
            this.list_facets_design = lfd; 
        }


        /* Descripción:
        *  Constructor de la clase Facet.
        *  
        * Parámetros:
        *       string name: nombre/etiqueta de la faceta.
        *       int level: nivel de la faceta. Ha de ser mayor que cero
        *       string comment: descripción de la faceta
        *       string lfd = introducimos el diseño de faceta
        *       bool omit = indica si la faceta esta marcada para su estudio (true) o no (false)
        *      
        */
        public Facet(string name, int level, string comment, string lfd, bool omit)
            : this(name, level, comment, lfd)
        {
            this.omit = omit;
        }



        /* Descripción:
         *  Constructor de la clase Facet.
         *  
         * Parámetros:
         *      string name: nombre/etiqueta de la faceta.
         *      int level: nivel de la faceta. Ha de ser mayor que cero
         *      string comment: descripción de la faceta
         *      int sizeOfUniverse: Tamaño del universo
         *      string lfd = introducimos el diseño de faceta
         * Excepciones:
        *      FacetException: si el nivel es mayor que el universo.
         */
        public Facet(string name, int level, string comment, int sizeOfUniverse, string lfd)
            :this(name,level,comment,lfd)
        {
            if (level > sizeOfUniverse)
            {
                throw new FacetException("Error: El nivel no puede ser mayor que el universo");
            }
            this.sizeOfUniverse = sizeOfUniverse;
        }


        /* Descripción:
         *  Constructor de la clase Facet.
         *  
         * Parámetros:
         *  string name: nombre/etiqueta de la faceta.
         *  int level: nivel de la faceta. Ha de ser mayor que cero
         *  string comment: descripción de la faceta
         *  int sizeOfUniverse: Tamaño del universo
         *  string lfd = introducimos el diseño de faceta
         *  bool omit = indica si la faceta esta marcada para su estudio (true) o no (false)
         */
        public Facet(string name, int level, string comment, int sizeOfUniverse, string lfd, bool omit)
            : this(name, level, comment, sizeOfUniverse, lfd)
        {
            this.omit = omit;
        }

       #endregion Constructores


       #region Métodos de instancia
        /*=================================================================================
         * Métodos de instancia
         * ====================
         *  - Name
         *  - Level
         *  - Comment
         *  - SizeOfUniverse
         *  - SetSkipLevels
         *  - ListFacetsDesignNesting (2 métodos)
         *  - ListFacetsDesignCrossed
         *  - ListFacetsDesignRemove
         *  - Omit
         *=================================================================================*/

        /* Descripción:
         *  Asigna un nombre a la faceta.
         */
        public void Name(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new FacetException("La faceta no tiene nombre.");
            }
            string oldname = this.name;
            this.name = name;
            this.list_facets_design = this.list_facets_design.Replace("[" + oldname + "]", "[" + name + "]");
        }


        /* Descripción:
         *  Asigna un nuevo nivel a la faceta que debe ser positivo y menor o igual que el 
         *  tamaño del universo.
         * Excepciones:
         *  FacetException: Si el nivel no es valido.
         */
        public void Level(int level)
        {
            if (level < 1 )
            {
                throw new FacetException("Faceta tiene que tener un nivel positivo (mayor que cero)");
            }
            if (level > this.sizeOfUniverse)
            {
                throw new FacetException("Error: El nivel no puede ser mayor que el universo");
            }
            this.level = level;
        }


        /* Descripción:
         *  Asigna una descripción a la faceta.
         */
        public void Comment(string comment)
        {
            this.comment = comment;
        }


        /* Descripción:
         *  Asignación del tamaño del universo de la faceta, positivo mayor o igual que el nivel de 
         *  la faceta.
         */
        public void SizeOfUniverse(int sizeOfUniverse)
        {
            if (sizeOfUniverse < 1 || sizeOfUniverse < this.level)
            {
                throw new FacetException("Error al modificar el universo");
            }
            this.sizeOfUniverse = sizeOfUniverse;
        }


        /* Descripción:
         *  Añade el estado de omisión o no omisión de los niveles.
         * 
         * NOTA: solo se añaden los niveles que se van a omitir. Los que no se omiten se determina por
         * exclusión.
         * 
         * Parámetros:
         *      int level: nivel sobre el que actuamos
         *      bool skip: estado del nivel, true se omite, false no se omite
         */
        public void SetSkipLevels(int level, bool skip)
        {
            // Si el nivel no esta dentro del rango permitido lanzamos una excepción
            if (level < 1 && level > this.level)
            {
                throw new FacetException("Error: nivel no permitido");
            }

            if (this.skipLevels.ContainsKey(level))
            {
                if (!skip)
                {
                    this.skipLevels.Remove(level);
                }
            }
            else
            {
                if (skip)
                {
                    this.skipLevels.Add(level, skip);
                }
            }
        }


        /* Descripción:
         *  Devuelve una lista ordenada (en orden descendende) con los niveles que se
         *  van a omitir.
         */
        public List<int> ListSkipLevels()
        {
            List<int> lSkip = new List<int>();
            foreach (int i in this.skipLevels.Keys)
            {
                lSkip.Add(i);
            }
            lSkip.Sort();
            lSkip.Reverse();
            return lSkip;
        }


        /* Descripción:
         *  Asigna la faceta sobre la que esta anidada de la forma "[this.name()]:[f.name()]". Una faceta puede estar anidada
         *  en más de una faceta por eso empleamos una lista.
         *  
         * Excepción:
         *  Una faceta no puede estar anidada dentro de si misma.
         */
        public void ListFacetsDesignNesting(Facet f)
        {
            string name = "["+f.Name()+"]";
            if (this.list_facets_design.Contains(name))
            {
                throw new FacetException("Error: Ya esta contenida");
            }
            this.list_facets_design = this.list_facets_design + ":[" + f.name +"]";
        }


        /* Descripción:
         *  Asigna la faceta sobre la que esta anidada de la forma "[this.name()]:[f.name()]". Una faceta puede estar anidada
         *  en más de una faceta por eso empleamos una lista.
         * NOTA: No comprueba que el diseño este permitido.
         */
        public void ListFacetsDesignNesting(string design)
        {
            int pos = design.IndexOf(']');
            string name = design.Substring(1, pos-1);
            if (!this.name.Equals(name))
            {
                throw new FacetException("Error: en el formato del diseño");
            }
            this.list_facets_design = design;
        }


        /* Descripción:
         *  Asigna la faceta sobre la que esta anidada de la forma "listFacetDesig"[f.name()]". Una faceta puede estar anidada
         *  en más de una faceta por eso empleamos una lista.
         *  
         * Excepción:
         *  Una faceta no puede estar cruzada con sigo misma, ni puede cruzarse varias veces.
         *  Una faceta no pued cruzarse si antes no esta anidada por otra.
         */
        public void ListFacetsDesignCrossed(Facet f)
        {
            string name = "[" + f.Name() + "]";
            if (this.list_facets_design.Contains(name))
            {
                throw new FacetException("Error: Ya esta contenida");
            }
            if (!this.list_facets_design.Contains(NEST_CHAR))
            {
                throw new FacetException("Error: No puede cruzarse sin anidamiento");
            }
            this.list_facets_design = this.list_facets_design + "[" + f.name + "]";
        }


        /* Descripción:
         *  Reinicaliza al diseño original de la faceta
         */
        public void ListFacetsDesignRemove()
        {
            this.list_facets_design = "[" + this.Name() + "]";
        }


        /* Descripció:
         *  Elimina si existe la faceta que se pasa como parámetro en el diseño de faceta
         */
        public void ListFacetsDesignRemove(string nameFacet)
        {
            string s = "[" + nameFacet + "]";

            if (this.list_facets_design.EndsWith(NEST_CHAR + s))
            {
                this.list_facets_design.Replace(NEST_CHAR + s, "");
            }
            else if(this.list_facets_design.Contains(NEST_CHAR+s+NEST_CHAR))
            {
                this.list_facets_design.Replace(NEST_CHAR + s, "");
            }
            else
            {
                this.list_facets_design.Replace(s, "");
            }
        }


        /* Descripción:
         *  Asign valor a la variable omit
         */
        public void Omit(bool omit)
        {
            this.omit = omit;
        }

        #endregion Métodos de instancia


        #region Métodos de Consulta
        /*=================================================================================
         * Métodos de consulta
         * ===================
         *  - Name
         *  - SetName
         *  - Level
         *  - Comment
         *  - SizeOfUniverse
         *  - GetSkipLevels
         *  - Omit
         *  - ListFacetsNesting
         *  - IsNesting
         *  - IsValidateNesting
         *  - IsInfinite
         *  - IsFixed
         *  - IsRamdonFinite
         *  - StringDesing
         *=================================================================================*/

        /* Descripción:
         *  Devuelve el nombre de la faceta.
         */
        public string Name()
        {
            return name;
        }


        /* Descripción:
         *  Devuelve el nombre de la faceta entre corchetes.
         */
        public string SetName()
        {
            return "[" + name + "]";
        }


        /* Descripción:
         *  Devuelve el nivel de la faceta.
         */
        public int Level()
        {
            return level;
        }


        /* Descripción:
         *  Devuelve el nivel de la faceta. Este será igual al número de niveles inicial menos el número
         *  de niveles excluidos para los calculos
         */
        public int LevelNoSkip()
        {
            return (level - this.skipLevels.Keys.Count);
        }


        /* Descripción:
         *  Devuelve la descripción/comentario de la faceta.
         */
        public string Comment()
        {
            return comment;
        }


        /* Descripción:
         *  Devuelve el tamaño del universo de la faceta.
         */
        public int SizeOfUniverse()
        {
            return this.sizeOfUniverse;
        }


        /* Descripción:
         *  Devuelve true si esta marcado como nivel que se omitira, false en otro caso.
         */
        public bool GetSkipLevels(int level)
        {
            return this.skipLevels.ContainsKey(level);
        }


        /* Descripción:
         *  Devuelve true si tiene algún nivel omitido
         */
        public bool HasSkipLevels()
        {
            return (this.skipLevels.Keys.Count > 0);
        }


        /* Descripción:
         *  Devuelve el valor de la variable que indica si la faceta se debe o no omitir para el studio
         */
        public bool Omit()
        {
            return omit;
        }


        /* Descripción:
         *  Devuelve la lista de facetas sobre la que esta anidada. Si la faceta no esta anidada devolverá null.
         */
        public string ListFacetsNesting()
        {
            return this.list_facets_design;
        }


        /* Descripción:
         *  Devuelve true si la faceta esta anidada, es decir, nested != null, delvolverá false en otro
         *  caso.
         */
        public bool IsNesting()
        {
            return (this.list_facets_design.Contains(":"));
        }
        

        /* Descripción:
         *  Devuelve true si la faceta aleatoria infinita. Tiene tamaño del universo infinito, 
         *  false en otro caso.
         */
        public bool IsInfinite()
        {
            return this.sizeOfUniverse.Equals(int.MaxValue);
        }


        /* Descripción:
         *  Devuelve true si la fecta es fija, es decir, el tamaño de nivel coincide con el 
         *  del universo.
         */
        public bool IsFixed()
        {
            return this.sizeOfUniverse.Equals(this.level);
        }


        /* Descripción:
         *  Devuelve true si la fecta es aleatoria finita, es decir, el tamaño del universo no es
         *  infinito y además, el nivel no coincide con el del universo.
         */
        public bool IsRamdonFinite()
        {
            return (!this.IsFixed() && !this.IsInfinite());
        }


        /* Descripción:
         *  Devuelve un string con el disposición de facetas para esa faceta.
         */
        public string ListFacetDesing()
        {
            return this.list_facets_design;
        }

        #endregion Métodos de Consulta


        #region Métodos IO
        /* Descripción:
         *  Escribe una faceta.
         */
        public bool WritingStreamFacet(StreamWriter writerFile)
        {
            bool res = false; // variable de retorno
            writerFile.WriteLine(BEGIN_FACET);
            // Escribimos los datos de la faceta
            writerFile.WriteLine(this.Name());
            writerFile.WriteLine(this.Level());
            int u = this.SizeOfUniverse();
            string universe = u.ToString();
            if (u.Equals(int.MaxValue))
            {
                universe = INFINITE;
            }
            writerFile.WriteLine(universe);
            writerFile.WriteLine(this.omit.ToString());
            writerFile.WriteLine(this.Comment());
            writerFile.WriteLine(this.ListFacetDesing());
            // Escribimos la lista lista de facetas omitidas.
            res = WritingStreamSkipLevels(writerFile);
            // ponemos el cierre
            writerFile.WriteLine(END_FACET);
            
            return res;
        }


        /* Descripción:
         *  Método auxiliar que escribe la sucesión de niveles que se omitirán en los calculos
         */
        private bool WritingStreamSkipLevels(StreamWriter writerFile)
        {
            bool res = true;
            writerFile.WriteLine(BEGIN_SKIP_LEVELS);
            string sklevel = "";
            foreach (int k in this.skipLevels.Keys)
            {
                if (sklevel.Equals(""))
                {
                    sklevel = sklevel + k;
                }
                else
                {
                    sklevel = sklevel + " " + k;
                }
            }
            if (!sklevel.Equals(""))
            {
                writerFile.WriteLine(sklevel);
            }
            writerFile.WriteLine(END_SKIP_LEVELS);
            return res;
        }


        private void ReaderAuxSkipLevel(StreamReader readerFile)
        {
            string line;
            char[] delimeterChars = { ' ' }; // nuestro delimitador será el caracter blanco
            while (!((line = readerFile.ReadLine()).Equals(END_SKIP_LEVELS)))
            {
                string[] arrayOfInt = line.Trim().Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
                int n = arrayOfInt.Length;
                for(int i =0; i<n; i++)
                {
                    int skip_level =int.Parse(arrayOfInt[i]);
                    this.SetSkipLevels(skip_level, true);
                }
            }
        }


        /* Descripción:
         *  Lectura de streamer de una faceta
         */
        public static Facet ReadingStreamFacet(StreamReader readerFile)
        { 
            try
            {
                // Escribimos los datos de la faceta
                string name = readerFile.ReadLine();
                int level = int.Parse(readerFile.ReadLine());
                string u = readerFile.ReadLine();
                int universe = int.MaxValue;
            
                if (!u.Equals(INFINITE))
                {
                    universe = int.Parse(u);
                }

                string o = readerFile.ReadLine();
                bool omit = false;
                if (o.Trim().ToLower().Equals("true"))
                {
                    omit = true;
                }

                string comment = readerFile.ReadLine();
                string lfdesign = readerFile.ReadLine();
                Facet f = new Facet(name, level, comment, universe, lfdesign, omit);

                string line;
                if ((line = readerFile.ReadLine()).Equals(BEGIN_SKIP_LEVELS))
                {
                    f.ReaderAuxSkipLevel(readerFile);
                }
                // ponemos el cierre
                line = readerFile.ReadLine();

                if (!line.Equals(END_FACET))
                {
                    throw new FacetException("Error, no se ha encontrado el fin de faceta");
                }

                return f;
            }
            catch(FormatException)
            {
                throw new FacetException("Error en el formato del archivo al leer una faceta");
            }
        }

        #endregion Métodos IO


        #region Métodos Redefinidos
        /*=================================================================================
         * Métodos redefinidos:
         *  - ToString()
         *  - Equals(Object obj)
         *  - GetHashCode()
         *=================================================================================*/
        
        /* Descripción:
         *  Redefinición del método ToString para la clase faceta.
         */
        public override string ToString()
        {
            string size = Facet.INFINITE;
            if (!this.IsInfinite())
            {
                size = this.sizeOfUniverse.ToString();
            }

            string sklevel = "";
            foreach (int k in this.skipLevels.Keys)
            {
                if (sklevel.Equals(""))
                {
                    sklevel = sklevel + k;
                }
                else
                {
                    sklevel = sklevel + ", " + k;
                }
            }

            return "[name= " + name + ", level=" + level + ", univ.=" + size + ", comment="
                + comment + ", nesting=" + this.list_facets_design + " ,SkipLevels: (" + sklevel + ")]";
        }


        /* Descripción:
         *  Redefinición del método Equals. Dos facetas seran iguales si tienen el mismo nombre.
         */
        public override Boolean Equals(Object obj)
        {
            //Variable de retorno
            Boolean res = false;
            if (!(obj == null || GetType() != obj.GetType()))
            {
                Facet facet = (Facet)obj;
                /*
                res = this.name.Equals(facet.name) &&
                        this.level.Equals(facet.level) &&
                        this.sizeOfUniverse.Equals(facet.sizeOfUniverse) &&
                        this.comment.ToUpper().Equals(facet.comment.ToUpper());
                */
                res = this.name.ToUpper().Equals(facet.name.ToUpper());
            }
            return res;
        }


        /* Descripción:
         *  Redefinición del método hashcode
         */
        public override int GetHashCode()
        {
            //return (this.name.GetHashCode() + this.level.GetHashCode() +this.sizeOfUniverse.GetHashCode() +this.comment.GetHashCode()) / 3;
            return (this.name.GetHashCode());
        }

        #endregion Métodos Redefinidos


        #region Conversiones con DataSet
        /* Descripción:
         *  Toma una faceta como parámetro y devuelve un DataSet con los datos de esta
         */
        public DataSet Facet2DataSet(Facet f)
        {
            DataSet dsFacets = new DataSet("DataSet_Facet");

            DataTable dtFacet = new DataTable("TbFacet");
            DataColumn c_pk_facet = new DataColumn("pk_facet", System.Type.GetType("System.Int32"));
            DataColumn c_name_facet = new DataColumn("name_facet", System.Type.GetType("System.String"));
            DataColumn c_level_facet = new DataColumn("level_facet", System.Type.GetType("System.Int32"));
            DataColumn c_size_of_universe = new DataColumn("size_of_universe", System.Type.GetType("System.String"));
            DataColumn c_comment = new DataColumn("comment", System.Type.GetType("System.String"));
            DataColumn c_omit = new DataColumn("omit", System.Type.GetType("System.Boolean"));
            DataColumn c_list_facet_design = new DataColumn("list_facet_design", System.Type.GetType("System.String"));

            // añadimos las columnas
            dtFacet.Columns.Add(c_pk_facet);
            dtFacet.Columns.Add(c_name_facet);
            dtFacet.Columns.Add(c_level_facet);
            dtFacet.Columns.Add(c_size_of_universe);
            dtFacet.Columns.Add(c_comment);
            dtFacet.Columns.Add(c_omit);
            dtFacet.Columns.Add(c_list_facet_design);

            // Añadimos el dataTable al DataSet
            dsFacets.Tables.Add(dtFacet);

            // Creamos una nueva fila
            DataRow newFacetRow = dsFacets.Tables["TbFacet"].NewRow();

            // Rellenamos la fila
            newFacetRow["pk_facet"] = 0;
            newFacetRow["name_facet"] = this.name;
            newFacetRow["level_facet"] = this.level;
            string sz = Facet.INFINITE;
            if(this.sizeOfUniverse < int.MaxValue)
            {
                sz = this.sizeOfUniverse.ToString();
            }
            newFacetRow["size_of_universe"] = sz;
            newFacetRow["comment"] = this.comment;
            newFacetRow["list_facet_design"] = this.list_facets_design;

            // Añadimos la fila al dataTable del dataSet
            dsFacets.Tables["TbFacet"].Rows.Add(newFacetRow);

            // Creamos el dataTable con los niveles omitidos
            DataTable dtSkipLevels = new DataTable("TbSkipLevels");
            DataColumn c_pk_skip_level = new DataColumn("pk_skip_levels", System.Type.GetType("System.Int32"));
            DataColumn c_skip_level = new DataColumn("skip_levels", System.Type.GetType("System.Int32"));
            DataColumn c_fk_facet = new DataColumn("fk_facet", System.Type.GetType("System.Int32"));

            dtSkipLevels.Columns.Add(c_pk_skip_level);
            dtSkipLevels.Columns.Add(c_skip_level);
            dtSkipLevels.Columns.Add(c_fk_facet);

            // Añadimos el dataTable al DataSet
            dsFacets.Tables.Add(dtSkipLevels);

            List<int> l_skip = this.ListSkipLevels();

            int nL = l_skip.Count;

            for (int i = 0; i < nL; i++)
            {
                // Creamos una nueva fila
                DataRow newSkipLevelRow = dsFacets.Tables["TbSkipLevels"].NewRow();

                // Rellenamos la fila
                newSkipLevelRow["pk_skip_levels"] = i;
                newSkipLevelRow["skip_levels"] = l_skip[i];
                newSkipLevelRow["fk_facet"] = 0;

                // Añadimos los datos al dataSet
                dsFacets.Tables["TbSkipLevels"].Rows.Add(newSkipLevelRow);
            }

            // Devolvemos el DataSet
            return dsFacets;
        }// end Facet2DataSet


        /* Descripción:
         *  Dado un DataSet y un entero correspondiente a la clave primaria devuelve la faceta 
         *  correspondiente.
         */
        public Facet DataSet2Facet(DataSet ds, int pk)
        {
            string select = " pk_facet = " + pk;
            DataRow r = ds.Tables["TbFacet"].Select(select)[0];
            string name = (string)r["name_facet"];
            int level = (int)r["level_facet"];
            string s = (string)r["size_of_universe"];

            int size_of_universe = int.MaxValue;
            if (!s.Equals(Facet.INFINITE))
            {
                size_of_universe = int.Parse(s);
            }
            string comment = (string)r["comment"];
            string list_design = (string)r["list_facet_design"];
            bool omit = (bool)r["omit"];

            Facet f = new Facet(name, level, comment, size_of_universe, omit);

            // añadimos los niveles omitidos
            select = " fk_facet = " + pk;
            DataRow[] rows = ds.Tables["TbFacet"].Select(select);

            int numSkip = rows.Length;

            for (int i = 0; i < numSkip; i++)
            {
                DataRow r_skip = rows[i];
                int skipLevel = (int)r_skip["skip_level"];
                f.SetSkipLevels(skipLevel, true);
            }
            return f;
        }

        #endregion Conversiones con DataSet



        #region Implementacion de la interfaz
        /*=================================================================================
         * Implementacion de la interfaz
         * =============================
         * 
         * - CompareTo
         * - Clone
         *=================================================================================*/

        public int CompareTo(Facet other)
        {
            int retval = this.name.ToLower().CompareTo(other.name.ToLower());
            if(retval==0)
            {
                retval = this.level.CompareTo(other.level);
                if (retval == 0)
                {
                    retval = this.sizeOfUniverse.CompareTo(other.sizeOfUniverse);
                    if (retval == 0)
                    {
                        retval = this.comment.CompareTo(other.comment);
                    }
                }
            }
            return retval;
        }


        public object Clone()
        {
            string name = string.Copy(this.Name());
            int l = this.Level();
            int size = this.SizeOfUniverse();
            string comment = string.Copy(this.Comment());
            string nesting = string.Copy(this.ListFacetDesing());
            bool o = this.Omit();
            Facet f = new Facet(name, l, comment, sizeOfUniverse, nesting, omit);

            foreach (int k in this.skipLevels.Keys)
            {
                f.SetSkipLevels(k, true);
            }
            return f;
        }

        #endregion Implementación de la interfaz

    }// end class
}// end namespace 