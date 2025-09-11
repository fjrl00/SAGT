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
 * Fecha de revisión: 29/SEP/2010       Versión: build 0.029
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiFacetPY
{
    public class MultiFacetPYException:Exception
    {
        public MultiFacetPYException()
            : base()
        {
        }

        public MultiFacetPYException(string mns)
            : base(mns)
        {
        }
    }
}
