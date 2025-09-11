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
 * Fecha de revisión: 31/Mar/2011                           
 * 
 * Descripción:
 *      Excepciones para la lectura de fichero suma de cuadrados (.ssq, GT E 2.0).
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportEduGSsq
{
    public class AnalysisSsqEduG_Exception: Exception
    {
        public AnalysisSsqEduG_Exception()
            : base()
        {
        }
        public AnalysisSsqEduG_Exception(string msg)
            : base(msg)
        {
        }
    }
}
