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
 * Fecha de revisión: 09/Sep/2011       Versión: 1.002                     
 * 
 */
/*
 * Control de cambios:
 * 
 * Fecha de revisión: 05/Nov/2010       Versión: 1.001                     
 *  Se modifico el método 'ReadFileTrans(String nameFile)' para que ya no influlla el numero
 *  de lineas en blanco que debe haber entre traducción. Antes forzosamente debía ser 1.
 * Fecha de revisión: 09/Sep/2011       Versión: 1.002                     
 *  Se hicieron modificaciónes en el método ReadFileTrans y se elimino el método ReadWordTranslation.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TransLibrary
{
    public class ReadFileTrans : LabelTranslation
    {
        // Constantes
        private const string START_LABEL = "[";
        private const string END_LABEL = "]";
        // Etiquetas para cada uno de los idiomas
        private string[] LANG_LABELS = {"es", "en", "fr", "po" };

        public ReadFileTrans(String nameFile)
        {
            using (TextReader reader = new StreamReader(nameFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // lectura de fichero
                    /*
                     * Introducimos los delimitadores de la cadena.
                     * Lo primero que leemos es el nombre del elemento de la interfaz gráfica 
                     * entre cochetes. Por tanto tenemos que eliminar los corchetes y asegurarnos
                     * de que no tiene espacios en blanco entre medio.
                     */

                    if (!line.Trim().Equals(""))
                    {
                        // Comprobamos que en la primera línea se encuentra la etiqueta con el siguiente formato
                        string labelKey = line.Trim();
                        if (labelKey.StartsWith(START_LABEL) && labelKey.EndsWith(END_LABEL))
                        {
                            labelKey = labelKey.Substring(1, labelKey.Length - 2);
                        }
                        else
                        {
                            throw new LabelTranslationException
                                (String.Format("Formato erroneo: {0}", line));
                        }

                        WordTranslation trasl = new WordTranslation();
                        // leemos las palabras en los distintos idiomas
                        while (!trasl.CompleteTraslationWord())
                        {// (*2*)
                            line = reader.ReadLine();
                            if (!line.Equals(""))
                            {
                                string res = "";
                                string sub = line.Substring(0, 2);
                                sub = CodeLabelToLang(sub);

                                if (Enum.IsDefined(typeof(Language), sub))
                                {
                                    Language l = (Language)Enum.Parse(typeof(Language), sub);
                                    res = line.Substring(3);
                                    trasl.SetTranslation(l, res);
                                }
                                else
                                {
                                    throw new LabelTranslationException("Error: la expresion no pertence a ningún idioma");
                                }

                            }
                            
                        } // end while(*2*)

                        this.AddToDic(labelKey, trasl);

                    }// end if
                       
                } // end while
            } // end using
        } // end ReadFileTrans


        /* Descripción:
         *  Tranforma el código en un string del enumerado correspondiente con el idioma.
         */
        private string CodeLabelToLang(string code)
        {
            string[] lnames = Enum.GetNames(typeof(Language));
            string retVal = lnames[0];
            int n = LANG_LABELS.Length;
            
            bool found = false;
            
            for (int i = 0; i < n && !found; i++)
            {
                found = code.ToUpper().Equals(LANG_LABELS[i].ToUpper());
                retVal = lnames[i];
            }

            if (!found)
            {
                throw new LabelTranslationException("Error: la expresion no pertence a ningún idioma");
            }
            
            return retVal;
        }
 
    } // end Class
} // end nameSpace
