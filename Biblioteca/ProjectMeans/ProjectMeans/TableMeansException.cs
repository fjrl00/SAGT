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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectMeans
{
    public class TableMeansException : Exception
    {
        public TableMeansException()
            : base()
        {
        }
        public TableMeansException(string msg)
            : base(msg)
        {
        }

        public TableMeansException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
