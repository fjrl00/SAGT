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
 * Fecha de revisión: 28/Mar/2011       Versión: 1.0
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SsqPY
{
    public class RsaSsqPY_Exception: Exception
    {
        public RsaSsqPY_Exception()
            : base()
        {
        }
        public RsaSsqPY_Exception(string msg)
            : base(msg)
        {
        }
    } // end public class RsaSsqPY_Exception: Exception
}// end namespace SsqPY
