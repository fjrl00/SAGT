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
 * Fecha de revisión: 21/SEP/2010                     
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransLibrary
{
    /*
     * Crea las excepciones de la clases Dictionary.
     * Extiende de laq clase Exception
     */
    public class LabelTranslationException : Exception
    {
        public LabelTranslationException() : base()
        {
            // no es necesario añadir codigo ya que lo hace de forma automática
        }

        public LabelTranslationException(string msg) : base(msg)
        {
            // no es necesario añadir codigo ya que lo hace de forma automática
        }
    }
}
