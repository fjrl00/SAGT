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
 * Fecha de revisión: 4/Abr/2011
 * 
 * Descripción: 
 *      Lanza una excepción cuando se produce un fallo en  la clase ListMeansEduG.cs. Esta clase      
 *      crea una lista de tablas de medias a partir de un fichero .txt. Este ficero contiene un
 *      informe de las medias generado por el programa EduG 6.0
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportEduGMeans
{
    public class ListMeansEduG_Exception: Exception
    {
        public ListMeansEduG_Exception()
            : base()
        {
        }
        public ListMeansEduG_Exception(string msg)
            : base(msg)
        {
        }
    }
}
