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
 * Fecha de revisión: 21/SEP/2010       Versión: 1.0                     
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransLibrary
{
    public class LabelTranslation
    {

         private Dictionary<String, WordTranslation> dic;
        // private Dictionary<Language, Translation> d;
        /*
         * Constructor de la clase.
         * Genera un diccionario vacio
         */
        public LabelTranslation()
        {
            dic = new Dictionary<String, WordTranslation>();
        }

        /*
         * Introduce una elemento en el diccionario
         */
        public void AddToDic(String s, WordTranslation wt)
        {
            if (!dic.ContainsKey(s))
            {
                dic.Add(s, wt);
            }
            else
            {
                throw new LabelTranslationException("La clave no se encuentra en el dicionario");
            }
        }
        /*
         * Comprueba que la clave ya se encuentra en la estructura de datos
         */
        public Boolean IsKeyIncluded(string s)
        {
            Boolean res = false;
            if (dic.ContainsKey(s))
            {
                res = true;
            }
            return res;
        }
        /*
         * Obtiene la traducción de una etiqueta (clave) que se pasa como parametro.
         * Si no se encuentra la clave lanza una excepcion del tipo LabelTranslationException
         */
        public WordTranslation labelTraslation(string key)
        {
            if (dic.ContainsKey(key))
            {
                return dic[key];
            }
            else
            {
                // no se encuentra la palabra en el diccionario
                throw new LabelTranslationException("No se encuentra la etiqueta en el diccionario");
            }
        }

        /*
         * Métodos redefinidos
         */
        public override string ToString()
        {
            StringBuilder res= new StringBuilder();
            foreach(string key in dic.Keys)
            {   res.Append("[");
                res.Append(key);
                res.Append("]\n");
                res.Append(this.labelTraslation(key).ToString()+"\n");
            }
            return res.ToString();
        }

    }
}
