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
 * Fecha de revisión: 08/NOV/2010       Versión: 1.0                     
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiFacetData
{
    public class ListFacetsException: Exception
    {
        public ListFacetsException()
            : base()
        {
        }

        public ListFacetsException(string msg)
            : base(msg)
        {
        }

        public ListFacetsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
