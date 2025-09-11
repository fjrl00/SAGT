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
 * Fecha de revisión: 07/SEP/2011       Versión: 1.0                     
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectMeans
{
    public class TableMeansTypScoreException: Exception
    {
        public TableMeansTypScoreException()
            : base()
        {
        }
        public TableMeansTypScoreException(string msg)
            : base(msg)
        {
        }
    }
}
