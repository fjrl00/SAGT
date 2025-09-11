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
 * Fecha de revisión: 10/DIC/2010                           
 * 
 * Descripción:
 *      Libreria de suma de cuadrados
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSSQ
{
    public class TableSSQException: Exception
    {
        public TableSSQException()
            : base()
        {
        }

        public TableSSQException(string msg)
            : base(msg)
        {
        }
        
    }
}
