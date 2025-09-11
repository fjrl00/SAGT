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
 * Fecha de revisión: 27/Abr/2012                     
 * 
 * Nota: 
 * Se a creado un enumerado en lugar de usar el de la clase Charting porque este incluye "None" que
 * no se encuentra en el enumerado original.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigCFG
{
    public enum LabelAlignmentStyles
    {
        None, TopLeft, Top, TopRight, Right, BottomRight, Bottom, BottomLeft, Left, Center
    }
}