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
 * Fecha de revisión: 02/Dic/2012
 * 
 */
using System;
// using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MultiFacetData
{
    public class ImportCSV
    {
        /***************************************************************************************************
         * CONSTANTES
         ***************************************************************************************************/
        public const char CHAR_DELIMITER_CELL = '"';
        

        /***************************************************************************************************
         * MÉTODOS
         ***************************************************************************************************/
        
        /* Descripción:
         *  Importa la tabla de frecuencias de un fichero .csv y devuelve un objeto multifaceta.
         */
        public static MultiFacetsObs ImportFileCSV_to_MultiFacetsObs(string path)
        {
            MultiFacetsObs retVal = null;
            using (StreamReader reader = new StreamReader(path))
            {
                retVal = ReaderCSV_to_MultiFacetsObs(reader, path);
            }
            return retVal;
        }// end ImportFileCSV_to_MultiFacetsObs


        /* Descripción:
         *  Importa los datos de una tabla de frecuencias con formato csv (del inglés comma-separated values)
         *  y devuelve un objeto multifaceta.
         */
        private static MultiFacetsObs ReaderCSV_to_MultiFacetsObs(StreamReader reader, string path)
        {
            MultiFacetsObs retVal = null;// valor de retorno
            // Leemos la primera linea (encabezado de columnas)
            string line = reader.ReadLine();

            string[] stringDelimiter = { "\",\"", "\"" }; // ",", " "
            /* Nota: El orden en que se definen los delimitadores influye en la manera en la que se realizan 
             * las particiones.
             */
            string[] arrayHeadersColumns = line.Split(stringDelimiter, StringSplitOptions.RemoveEmptyEntries);
            int numColumns = arrayHeadersColumns.Length;
            if (numColumns<2)   //in case we've detected that we weren't able to split the header line correctly (with ",", " ")
            {
                stringDelimiter = new string[1];
                stringDelimiter[0]= ";";            //we try again but with ";". Which we'll then also use in the table body
                arrayHeadersColumns = line.Split(stringDelimiter, StringSplitOptions.RemoveEmptyEntries);
                numColumns = arrayHeadersColumns.Length;
            }

            /* Los n-1 valores contendrá los nombres de las facetas (el último valor es la frecuencia/medición)
             */
            int numFacet = arrayHeadersColumns.Length-1;

            int[] arrayLevels = new int[numFacet];// almacenará el nivel de cada faceta.

            ObsTable tableObs = new ObsTable();// construimos la tabla por filas

            // Creamos la estructura que almacerá los valores de los niveles (one whole dictionary for each facet)
            Dictionary<string, int>[] arrayLevelVal = new Dictionary<string, int>[numFacet];

            // La inicializamos
            for (int i = 0; i < numFacet; i++)
            {
                arrayLevelVal[i] = new Dictionary<string, int>();  
            }

            /* Nos encontramos en un bucle de lectura de tabla
             */
            while ((line = reader.ReadLine()) != null)                                                          //Iterate over the entire document
            {
                string[] arrayLineShare = line.Split(stringDelimiter, StringSplitOptions.RemoveEmptyEntries);   //split line
                List<double?> row = new List<double?>();                                                        //prepare to store it as a row fitting for our ObsTable class

                for (int i = 0; i < numColumns; i++)                                                            //iterate over line
                {
                    if (i < numColumns - 1)                                                                     //facet data
                    {
                        int l = arrayLevelVal[i].Count;                                                         //read number of possible different symbols of this facet seen thus far
                        bool contains = arrayLevelVal[i].ContainsKey(arrayLineShare[i]);   
                        if (!contains)                                                                          //if this symbol is new
                        {
                            arrayLevelVal[i].Add(arrayLineShare[i], l + 1);                                     //we add the entry to the dictionary (symbol -> assigned numeric code)
                        }
                        row.Add(arrayLevelVal[i][arrayLineShare[i]]);                                           //dictionary for our column -> entry corresponding to the current symbol -> append to the row its numeric code
                    }                                               
                    else
                    {
                        row.Add(double.Parse(arrayLineShare[i]));                                               // frequency/measure data
                    }
                }
                tableObs.Add(row);                                                                              //store in obstable

            }// end while
            string comment = BuildInfo(arrayHeadersColumns, arrayLevelVal);

            ListFacets lf = new ListFacets();

            for (int i = 0; i < numFacet; i++)
            {
                Facet f = new Facet(arrayHeadersColumns[i], arrayLevelVal[i].Keys.Count);
                lf.Add(f);
            }
            int mul = (int)lf.MultiOfLevel(); // devuelve el número de filas que tendrá la tabla
            if (mul == tableObs.ObsTableRows()) //if we have data for all rows
            {
                // entonces la tablaObs es producto cartesiano (aka there's data for each combination of facet values)
                retVal = new MultiFacetsObs(lf, tableObs, path, "", comment);
            }
            else
            {
                // entonces la tablaObs no es producto cartesiano y debemos ajustarla
                retVal = new MultiFacetsObs(lf, path, "", comment); 

                InterfaceObsTable obsT = retVal.ObservationTable(); //full observation table for these facets (but without frequency/measure)

                int n_items = tableObs.ObsTableRows();              //number of entries for which there's data

                //int pos = 0;
                //for (int i = 0; (i < mul) && (pos < n_items); i++)
                //{
                //    bool b = true;
                //    for (int j = 0; j < numFacet && b; j++)
                //    {
                //        b = obsT.Data(i, j).Equals(tableObs.Data(pos, j));
                //    }
                //    if (b)
                //    {
                //        double d = (double)tableObs.Data(pos);
                //        obsT.Data(d, i);
                //        pos++;
                //    }
                //}

                for (int i = 0; i < n_items; i++)                   //iterate over tableObs (the entries for which there's data)
                {
                    for (int r = 0; r < mul; r++)                   //iterate looking for in which row of obsT to insert it
                    {
                        bool b = true;                              //seems promising
                        for (int j = 0; j < numFacet && b; j++)     //iterate over facets of this row, so long as it still seems promising
                        {
                            b = obsT.Data(r, j).Equals(tableObs.Data(i, j));    //we'll go over the entire facet data looking up whether all values match
                        }
                        if (b)                                      //if we've found a hit
                        {
                            double d = (double)tableObs.Data(i);
                            obsT.Data(d, r);                        //we asign it the frequency/measure value
                        }
                    }
                    
                }
            }// end if

           
            return retVal;
        }// ReaderCSV_to_MultiFacetsObs


        /* Descripción:
         *  Toma el diccionario con los valores y devuelve la leyenda para mostrar en la pestaña de 
         *  información.
         */
        private static string BuildInfo(string[] arrayHeadersColumns, Dictionary<String, int>[] lDic)
        {
            string text = ""; // Variable de retorno
            int n = lDic.Length;
            
            for (int i = 0; i < n; i++)
            {
                string nameFacet = arrayHeadersColumns[i];
                text = text + nameFacet + ":\n";

                Dictionary<String, int> dic = lDic[i];
                
                foreach (string key in dic.Keys)
                {
                    text = text + "\t" + key + " = " + dic[key] +"\n";
                }
                text = text + "\n";
            }

            return text;
        }//end BuildInfo

    }// end class ImportCSV
}// end namespace MultiFacetData
