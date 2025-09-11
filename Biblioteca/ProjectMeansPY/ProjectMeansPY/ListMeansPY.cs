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
 * Fecha de revisión: 17/Mar/2011 
 * 
 * Descripción: 
 *      Crea una lista de tabla de medias a partir de un fichero resultado de las medias de GT E 2.0 (1996)
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiFacetData;
using ProjectMeans;
using System.IO;
using System.Globalization;

namespace ListMeansPY
{
    public class ListMeansPY: ListMeans
    {
        // Constantes
        const string NEST_CHAR = "/";


        public static MultiFacetsObs ReadFileRsmPY(String nameFile)
        {
            // Variable de retorno
            MultiFacetsObs multFacets = null;

            // variables para declarar cada faceta
            string nameFacet;
            int level;
            string comment = ""; // el fichero no contien la descripción de la faceta por tanto no se modificara esta variable
            string design = "";
            // variable para declarar la lista de facetas
            ListFacets lFacets = new ListFacets();
            // variables para crear el fichero
            string descriptionFile;

            // string con la concatenación de los nombres de las facetas. Simplifica la busqueda
            string stListFacets = "";
            List<double?> dataObs;
            int multOfLevel = 1;

            using (TextReader reader = new StreamReader(nameFile))
            {
                try
                {
                    // la primera linea es el path del fichero de medias lo saltamos
                    string line = reader.ReadLine();
                    /* la siguiente línea puede ser una descripción del fichero o contener un 
                     * número. es la descripción del archivo
                     */
                    descriptionFile = reader.ReadLine();
                    /* saltamos las siguientes líneas de datos hasta encontrar la linea que contiene "N"
                     * mayuscula y que sus dos líneas siguientes son números.
                     */
                    line = reader.ReadLine();
                    while(!line.Trim().Equals("N"))
                    {
                        line = reader.ReadLine();
                    }
                    line = reader.ReadLine(); // la presente linea es un número, su uso la desconozco
                    // Leemos el número de facetas
                    int numFacets = int.Parse(reader.ReadLine());
                    if (numFacets < 1 && numFacets > 9)
                    {
                        throw new ListMeansPYException("Error en el formato de archivo");
                    }

                    // iteramos creado las facetas y añadiendosela a la lista
                    for (int i = 0; i < numFacets; i++)
                    {

                        // nameFacet = reader.ReadLine();
                        design = reader.ReadLine();
                        nameFacet = design.Substring(0, 1);// de esta linea se ha de obtener el nombre y el diseño
                        design = StringDesign(design);
                        stListFacets = stListFacets + nameFacet;
                        level = int.Parse(reader.ReadLine());
                        multOfLevel = multOfLevel * level;
                        line = reader.ReadLine(); // saltamos la linea de nivel de proceso
                        lFacets.Add(new Facet(nameFacet, level, comment, design));
                    }
                    // creamos el elemento multifaceta
                    multFacets = new MultiFacetsObs(lFacets, nameFile, descriptionFile);
                    // leemos la lista de datos
                    while ((line = reader.ReadLine()) != null)
                    {
                        //if (line.Equals(stListFacets))
                        if (FacetContains(line, stListFacets))
                        {//(* 1 *)
                            line = reader.ReadLine();
                            if (line.Trim().Equals("0"))
                            {// (* 2 *)
                                dataObs = new List<double?>();
                                bool encontrado = true;
                                for (int i = 0; i < numFacets && encontrado; i++)
                                {
                                    encontrado = reader.ReadLine().Equals(lFacets.FacetInPos(i).Name());
                                }
                                if (int.Parse(line = reader.ReadLine()).Equals(multOfLevel))
                                {// (* 3 *)
                                    while ((line = reader.ReadLine()) != null)
                                    {
                                        for (int i = 0; i < numFacets; i++)
                                        {
                                            line = reader.ReadLine();
                                        }
                                        if (line.Contains("."))
                                        {
                                            if (line.StartsWith("-"))
                                            {
                                                // es un numero negativo
                                                line = line.Insert(1, "0");
                                            }
                                            else
                                            {
                                                line = "0" + line.Trim();
                                            }
                                        }
                                        dataObs.Add(double.Parse(line, NumberFormatInfo.InvariantInfo));
                                    }

                                }// end if (* 3 *)
                                // la asignamos
                                multFacets.AssignDataToTheTableObs(dataObs);
                            }// end if (* 2 *)
                        }// end if (* 1 *)
                    }// end while
                }
                catch (FormatException ex)
                {
                    throw new ListMeansPYException("Error en el formato del fichero");
                }// end try


                // devolvemos el resultado
                return multFacets;
            }
        }


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
                if (s.Equals(NEST_CHAR))
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

        /* Descripción:
         *  Comprueba que la linea contiene todas las facetas
         */
        private static bool FacetContains(string line, string facets)
        {
            bool c = true;
            int n = facets.Length;
            for (int i = 0; i < n && c; i++)
            {
                string character = facets.Substring(i, 1);
                c = line.Contains(character);
            }
            return c;
        }
    }// public class ListMeansPY: ListMeans
}// end namespace ListMeansPY
