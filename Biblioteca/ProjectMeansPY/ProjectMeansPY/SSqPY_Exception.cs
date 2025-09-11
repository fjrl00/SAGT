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
 * Fecha de revisión: 28/Mar/2011
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SsqPY
{
    public class SSqPY_Exception: Exception
    {
        public SSqPY_Exception()
            : base()
        {
        }
        public SSqPY_Exception(string msg)
            : base(msg)
        {
        }
    }
}
