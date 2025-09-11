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
 * Fecha de revisión: 25/May/2012 
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectLibrary
{
    public class SagtUser_Exception: Exception
    {
        public SagtUser_Exception()
            : base()
        {
        }

        public SagtUser_Exception(string msg)
            : base(msg)
        {
        }
    }
}
