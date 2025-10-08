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
 * Fecha de revisión: 13/Dic/2011
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sagt
{
    public class SagtFileException: Exception
    {
        public SagtFileException()
            : base()
        {
        }
        public SagtFileException(string msg)
            : base(msg)
        {
        }

        public SagtFileException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
