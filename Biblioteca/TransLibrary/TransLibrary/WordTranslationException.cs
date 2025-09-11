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
    public class WordTranslationException: Exception
    {
        public WordTranslationException() : base()
        {
            // no es necesario añadir codigo ya que lo hace de forma automática
        }

        public WordTranslationException(string msg)
            : base(msg)
        {
            // no es necesario añadir codigo ya que lo hace de forma automática
        }
    }
}
