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
 * Fecha de revisión: 08/May/2012
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransLibrary
{
    public class WordTranslation
    {
        /*================================================================================================
         * Variables de clase
         *================================================================================================*/

        private Dictionary<Language, string> wordTranslation;

        /*================================================================================================
         * Constructores
         *================================================================================================*/
        public WordTranslation()
        {
            wordTranslation = new Dictionary<Language, string>();
        }


        public WordTranslation(string spanish, string english, string french, string portuguese)
        {
            wordTranslation.Add(Language.spanish, spanish);
            wordTranslation.Add(Language.english, english);
            wordTranslation.Add(Language.french, french);
            wordTranslation.Add(Language.portuguese, portuguese);
        }

        /*================================================================================================
         * Métodos de Consulta
         *================================================================================================*/
        
        /* Descripción:
         *  Devuelve la palabra en el idioma especificado como parámetro
         * Excepción:
         *  Si no encuentra la tradución lanza una excepción WordTranslationException
         */
        public string GetTranslation(Language lang)
        {
            if (!wordTranslation.ContainsKey(lang))
            {
                throw new WordTranslationException("No se encuentra la traducción");
            }
            return wordTranslation[lang];
        }


        /*================================================================================================
         * Métodos de instancia
         *================================================================================================*/

        /* Descripción:
         *  Introduce la tradución de una palabra, en el idioma especificado en el diccionario.
         * Parámetros:
         *      Language lang: idoma
         *      string word: palabra en dicho idioma.
         */
        public void SetTranslation(Language lang, string word)
        {
            if (!wordTranslation.ContainsKey(lang))
            {
                this.wordTranslation.Add(lang, word);
            }
            else
            {
                this.wordTranslation.Remove(lang);
                this.wordTranslation.Add(lang, word);
            }
        }



        /*================================================================================================
         * Métodos adicionales
         *================================================================================================*/

        /* Descripción:
         *  Devuelve true se la palabra se corresponde con alguna de las traducciones. No hace distinción
         *  entre mayúculas y minúsculas.
         */
        public bool TranslationIncluded(string w)
        {
            bool found = false;
            foreach (Language l in this.wordTranslation.Keys)
            {
                found = found || this.wordTranslation[l].ToUpper().Equals(w.ToUpper());
            }
            return found;
        }


        /* Descripción:
         *  Devuelve true si el diccionario contiene una traducción para la palabra en cada idioma.
         */
        public bool CompleteTraslationWord()
        {
            bool found = true;
            // int n = Language
            foreach (Language l in Enum.GetValues(typeof(Language)))
            {
                found = found && this.wordTranslation.ContainsKey(l);
            }
            return found;
        }

        /*================================================================================================
         * Metodos redefinidos
         *================================================================================================*/

        public override Boolean Equals(Object obj)
        {
            //Variable de retorno
            Boolean res = false;
            if (!(obj == null || GetType() != obj.GetType()))
            {
                WordTranslation trans = (WordTranslation)obj;
                int n = this.wordTranslation.Keys.Count;
                int m = trans.wordTranslation.Keys.Count;
                res = n == m;
                if (res)
                {
                    try
                    {
                        foreach (Language l in this.wordTranslation.Keys)
                        {
                            res = res && this.wordTranslation[l].ToUpper().Equals(trans.wordTranslation[l].ToUpper());
                        }
                    }
                    catch (WordTranslationException)
                    {
                        res = false;
                    }
                }
            }
            return res;
        }// end Equals


        public override int GetHashCode()
        {
            return this.wordTranslation.GetHashCode();
        }


        public override string ToString()
        {
            string tras = "";
            foreach(Language l in this.wordTranslation.Keys)
            {
                tras = tras + l.ToString() + "=" + this.wordTranslation[l] + ";";
            }
            return tras;
        }

    }// end public class WordTranslation
}// end namespace TransLibrary
