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
 * Fecha de revisión: 27/Jul/2011                           
 * 
 * Descripción:
 *      Genera las excepciones de la clase Analysis_and_G_Study.cs.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSSQ
{
    public class Analysis_and_G_Study_Exception : Exception
    {
        public Analysis_and_G_Study_Exception()
            : base()
        {
        }
        public Analysis_and_G_Study_Exception(string msg)
            : base(msg)
        {
        }

    }// end public class Analysis_and_G_Study_Exception
}// end namespace ProjectSSQ
