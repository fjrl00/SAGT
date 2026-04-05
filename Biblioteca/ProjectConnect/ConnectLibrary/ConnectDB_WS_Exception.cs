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
 * Fecha de revisión: 14/Jun/2012 
 * 
 */
using System;

namespace ConnectLibrary
{
    public class ConnectDB_WS_Exception : Exception
    {
        public ConnectDB_WS_Exception()
            : base()
        {
        }

        public ConnectDB_WS_Exception(string msg)
            : base(msg)
        {
        }
    }
}
