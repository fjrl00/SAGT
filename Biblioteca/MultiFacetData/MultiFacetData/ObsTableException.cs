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
 * Fecha de revisión: 10/OCT/2010       Versión: 1.0                     
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiFacetData
{
    public class ObsTableException : Exception
    {
        public ObsTableException()
            : base()
        {
            // no es necesario añadir codigo
        }

        public ObsTableException(string mns)
            : base(mns)
        {
            // no es necesario añadir codigo
        }
    }
}
