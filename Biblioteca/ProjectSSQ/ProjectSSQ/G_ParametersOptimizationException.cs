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
 * Fecha de revisión: 25/Jul/2011                           
 * 
 * Descripción:
 *      Libreria Suma de cuadrados, excepciones de la clase G_ParametersOptimization.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSSQ
{
    public class G_ParametersOptimizationException: Exception
    {
        public G_ParametersOptimizationException()
            : base()
        {
        }
        public G_ParametersOptimizationException(string msg)
            : base(msg)
        {
        }
    }// end G_ParametersOptimizationException
}// namespace ProjectSSQ

