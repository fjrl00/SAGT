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
 * Fecha de revisión: 01/Jun/2012
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuxMathCalcGT
{
    public class ConvertNumException: Exception
    {
        public ConvertNumException()
            : base()
        {
        }

        public ConvertNumException(string msg)
            : base(msg)
        {
        }
    }
}
