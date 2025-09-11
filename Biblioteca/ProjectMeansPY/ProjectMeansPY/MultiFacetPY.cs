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
 * Fecha de revisión: 21/Mar/2011
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiFacetData;
using System.IO;
using System.Globalization;

namespace MultiFacetPY
{
    public class MultiFacetPY: MultiFacetsObs
    {
        // Constantes
        /*
         * BEGINNING_OF_FACETS es el contenido de la línea me marca el inicio de las
         * facetas. Lo siguiente que leemos es un entero con el número de facetas.
         */
        const string BEGINNING_OF_FACETS = "??.??";
        const string FORMAT_NUMBER_SIMPLE = "#";

        // variables de instancia de la clase MultiFacetsObs
        private string pathFileObsData;

        /*
         * Descripción:
         *  Constructor de la clase MultiFactPY, clase heredera de MultiFacetObs que incorpora la
         *  variable pathFileObsData, que contiene una posible dirección del fichero de datos.
         * Parametros:
         *      string path: linea que indica donde se guarda los datos de la matriz de observaciones,
         *              (fichero .dat).
         *      List<Facet> lF: Lista de facetas. con que se construirá la tabla de observaciones.
         *      string nameFileObs: Nombre del fichero.
         *      string description: Descripción del fichero.
         */
        public MultiFacetPY(string path, ListFacets lF, string nameFileObs, string description) : 
            base(lF, nameFileObs, description)
        {
            //base(lF,nameFile,description);
            this.pathFileObsData = path;
            // comprobamos si existe en el path
            /*
            if(File.Exists(path))
            {
                // leemos el fichero data si lo hay
                string ficheroDeDatos = pathFileDatPY(extractFileNamePath(this.PathFileObsData()), path);
                List<double?> ld = MultiFacetPY.ReadFileDataPY(ficheroDeDatos);
                this.AssignDataToTheTableObs(ld);
                
            }
            */
        }


        /*
         * Descripción:
         *  Devueve el nombre del fichero.
         * Parámetros:
         *  string path: path del fichero que estamos leyendo.
         *  
         * NOTA:
         *  Si desea incluir una barra diagonal inversa, ésta debe estar precedida de otra 
         *  barra diagonal inversa.
         */
        public string extractFileNamePath(string path)
        {
            int pos = path.LastIndexOf("\\") + 1; // le sumamos uno para obtener la posición 
            // siguiente a la de la barra invertida
            return path.Substring(pos);
        }


        /*
         * Descripción:
         *  Devuelve la posible dirección del fichero de datos.
         */
        public string PathFileObsData()
        {
            return pathFileObsData;
        }


        /*
         * Descripción
         *  Lee el contenido de un fichero de facetas (con extension obs). Estos Contienen las
         *  facetas el formato y una descripción del documento.Se corresponde con los ficheros 
         *  del antiguo programa "G T Software for Generalizability  Studies" (Pierre  Ysewijn - 1996). 
         *  
         *  NOTA:
         *   Si la lectura de fichero se ha realizado correctamente se habrá creado el objeto
         *   que contiene la lista facetas y la tabla de observaciones sin los datos. En caso
         *   contrario devuelve NULL;
         *   
         * Parámetros:
         *  String nameFile: nombre del fichero de observaciones (con extensión .obs)
         */
        public static MultiFacetPY ReadFileObsPY(String nameFile)
        {
            // variable de retorno
            MultiFacetPY res = null;

            // variables para declarar cada faceta
            string nameFacet;
            int level;
            string comment;
            string design;
            // variable para declarar la lista de facetas
            ListFacets lFacets = new ListFacets();
            // variables para crear el fichero
            string descriptionFile;
            string pathFileObsData;


            using (TextReader reader = new StreamReader(nameFile))
            {
                bool formatNumberSimply = false;
                string line;
                while ((line = reader.ReadLine()) != null && !line.Equals(BEGINNING_OF_FACETS))
                {
                    if (line.Equals(FORMAT_NUMBER_SIMPLE))
                    {
                        formatNumberSimply = true;
                    }
                }// end while
                if (!line.Equals(BEGINNING_OF_FACETS))
                {
                    // error en el formato del archivo
                    throw new MultiFacetPYException("Error de formato de fichero");
                }
                else
                {
                    // saltamos la line y leemos el numero de facetas
                    line = reader.ReadLine();
                    int num_facets = int.Parse(line);
                    if (num_facets < 2)
                    {
                        // error en el formato
                        throw new MultiFacetPYException("Error en el número de facetas");
                    }
                    for (int i = 0; i < num_facets; i++)
                    {
                        design = reader.ReadLine();
                        nameFacet = design.Substring(0, 1);// de esta linea se ha de obtener el nombre y el diseño
                        design = StringDesign(design);
                        level = int.Parse(reader.ReadLine());
                        comment = reader.ReadLine();
                        lFacets.Add(new Facet(nameFacet,level,comment,design));
                    }
                    // leemos la linea de descripción de fichero
                    descriptionFile = reader.ReadLine();
                    // leemos el path donde posiblemente se encuentra el fichero de datos (.dat)
                    pathFileObsData = reader.ReadLine();
                    // leemos el fichero data si lo hay
                    string ficheroDeDatos = pathFileDatPY(pathFileObsData, nameFile);
                    if (File.Exists(ficheroDeDatos))
                    {
                        res = new MultiFacetPY(ficheroDeDatos, lFacets, nameFile, descriptionFile);
                        List<double?> ld = null;
                        if (formatNumberSimply)
                        {
                            ld = MultiFacetPY.ReadFileD2D_DataPY(ficheroDeDatos);
                        }
                        else
                        {
                            ld = MultiFacetPY.ReadFileDataPY(ficheroDeDatos);
                        }

                        if (ld != null)
                        {
                            res.AssignDataToTheTableObs(ld);
                        }
                    }
                    
                }

                return res;
            }
        } // end public MultiFactPY ReadFileObsPY(String nameFile)



        /* Descripción:
         *  Combierte el diseño en una cadena de diseño valida, poniendo cada faceta
         *  entre corchetes.
         */
        private static string StringDesign(string design)
        {
            string retVal = ""; // Valor de retorno
            int n = design.Length;
            
            for (int i = 0; i < n; i++)
            {
                string s = design.Substring(i, 1);
                if (s.Equals(Facet.NEST_CHAR))
                {
                    retVal = retVal + Facet.NEST_CHAR;
                }
                else
                {
                    retVal = retVal + "[" + s + "]";
                }
            }
            return retVal;
        }


        /*
         * Descripción:
         *  Lee el contenido de un fichero de datos (.dat) del antiguo programa "G T Software for  
         *  Generalizability  Studies" (Pierre  Ysewijn - 1996). Este tipo de ficheros contiene 
         *  los datos en una sola linea separados por espacios. Además no contiene valores null.
         * Parametros:
         *      String nameFile: es el nombre del fichero de datos
         * Devuelve una lista de tipo double con los datos del fichero.
         */
        public static List<double?> ReadFileDataPY(String nameFile)
        {
            // Variable de retorno
            List<double?> lDouble = new List<double?>();

            // lectura de fichero
            using (TextReader reader = new StreamReader(nameFile))
            {
                try
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        char[] delimeterChars = { ' ' }; // nuestro delimitador será el caracter blanco
                        string[] arrayOfDouble = line.Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string d in arrayOfDouble)
                        {
                            /*
                             * NOTA:
                             * Al pasar el string a double había un problema, se saltaba
                             * la coma flotante y se representaba un entero. Así 9.5 pasaba 
                             * a ser 95 y 10.5 eran 105.
                             * 
                             * Para evitar esto se añadio un nuevo parámetro al método
                             * "Double.Parse":
                             * "NumberFormatInfo.InvariantInfo", con ello ahora el valor
                             * double se representa con "," 9.5 --> 9,5
                             * 
                             * Es necesario incluir en la cabecera: using System.Globalization;
                             */
                            lDouble.Add(Double.Parse(d, NumberFormatInfo.InvariantInfo));
                        } // end foreach
                    } // end while
                }
                catch (FormatException)
                {
                    return null;
                }
                
            } // end using 
            return lDouble;
        } // public static List<double?> ReadFileData(String nameFile)


        /*
         * Descripción:
         *  Lee el contenido de un fichero de datos (.dat) del antiguo programa "G T Software for  
         *  Generalizability  Studies" (Pierre  Ysewijn - 1996). Este tipo de ficheros contiene 
         *  los datos SIN ESPACIOS. Cada caracter (Número) es un dato (entero entre 0 y 9). Además 
         *  no contiene valores null.
         * Parametros:
         *      String nameFile: es el nombre del fichero de datos
         * Devuelve una lista de tipo double con los datos del fichero.
         */
        public static List<double?> ReadFileD2D_DataPY(String nameFile)
        {
            // Variable de retorno
            List<double?> lDouble = new List<double?>();

            // lectura de fichero
            using (TextReader reader = new StreamReader(nameFile))
            {
                try
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        int l = line.Length;
                        for (int i = 0; i < l; i++)
                        {
                            string d = line.Substring(i, 1);
                            lDouble.Add(int.Parse(d));
                        } // end foreach
                    } // end while
                }
                catch (FormatException)
                {
                    return null;
                }
            } // end using 
            return lDouble;
        } // public static List<double?> ReadFileData(String nameFile)


        /*
         * Descripción:
         *  Toma el path del fichero de observaciones y el nombre de fichero de datos y construye 
         *  el path del fichero de datos.
         * Parametros:
         *      string nameFileDat: nombre del fichero de datos
         *      string path: path del fichero de observaciones
         */
        private static string pathFileDatPY(string nameFileDat, string path)
        {
            int pos = path.LastIndexOf("\\") + 1;
            string retVal = path.Remove(pos);
            pos = nameFileDat.LastIndexOf("\\") + 1;
            nameFileDat = nameFileDat.Substring(pos);
            return (retVal + nameFileDat);
        }


    } // public class MultiFactPY: MultiFacetsObs
} // end namespace MultiFactPY
