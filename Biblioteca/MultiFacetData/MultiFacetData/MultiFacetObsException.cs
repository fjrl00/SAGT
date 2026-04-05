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
 * Fecha de revisión: 15/SEP/2010       Versión: 1.0                     
 * 
 */
using System;

namespace MultiFacetData
{
    public class MultiFacetObsException : Exception
    {
        public MultiFacetObsException()
            : base()
        {
        }

        public MultiFacetObsException(string mns) : base(mns)
        {
        }

        public MultiFacetObsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
