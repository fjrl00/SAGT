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
 * Fecha de revisión: 10/Jun/2012                           
 * 
 * Descripción:
 *      Interface para el calculo de la suma de cuadrados
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiFacetData;

namespace ProjectSSQ
{
    public interface InterfaceTableSSQ
    {
        /*=================================================================================================
         * Métodos de consulta
         *=================================================================================================*/

        // Devuelve la suma de cuadrados sobre la que se construye la tabla
        ListFacets ListFacet();
        // Devuelve el valor de la suma de cuadrados
        double? Sum_sumX();
        // Devuelve el valor de la suma de cuadrados al cuadrado.
        double? Sum_sumX2();
        // Devuelve la varianza.
        double? Variance();
        // Consulta el valor de un elemento de la tabla y lo devuelve
        double? Data(int row, int col);
        // Devuelve la suma de cuadrados
        double? SumXData(int row);
        // Devuelve el cuadrado de la suma de cuadrados 
        double? SumX2Data(int row);

        /*=================================================================================================
         * Métodos de instancia
         *=================================================================================================*/
        // Introduce el valor en la posición que se pasa como parámetro.
        void SumXData(double? data, int pos);
        // Introduce la desviación tipica en la tabla en la posición que se pasa como parámetro.
        void SumX2Data(double? data, int pos);


    }// interface InterfaceTableSSQ
}// namespace ProjectSSQ
