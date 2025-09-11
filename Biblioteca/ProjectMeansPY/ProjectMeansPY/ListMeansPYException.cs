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
 * Fecha de revisión: 17/Mar/2011       Versión: 1.0
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ListMeansPY
{
    public class ListMeansPYException : Exception
    {
        public ListMeansPYException()
            : base()
        {
        }

        public ListMeansPYException(string msg)
            : base(msg)
        {
        }
    }
}
