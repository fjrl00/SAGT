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
 * Fecha de revisión: 15/Nov/2011                     
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSSQ
{
    public class TermException : Exception
    {
        public TermException ()
            : base()
        {
            // no es necesario añadir codigo
        }

        public TermException(string mns)
            : base(mns)
        {
            // no es necesario añadir codigo
        }

    }// end class TermException
}// end namespace ProjectSSQ
