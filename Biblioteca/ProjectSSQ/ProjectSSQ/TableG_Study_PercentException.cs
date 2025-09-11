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
 * Fecha de revisión: 22/Sep/2011
 * 
 * Descripción:
 *      Libreria de suma de cuadrados, Excepciones para la clase TableG_Study_Percent
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSSQ
{
    public class TableG_Study_PercentException : Exception
    {
        public TableG_Study_PercentException()
            : base()
        {
        }
        public TableG_Study_PercentException(string msg)
            : base(msg)
        {
        }
    }
}
