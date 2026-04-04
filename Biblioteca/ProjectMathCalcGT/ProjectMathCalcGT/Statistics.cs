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
 * Fecha de revisión: 26/Ene/2012
 * 
 * Descripción:
 *      Cálculo de valores estadisticos: Media, varianza, desviación tipica
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuxMathCalcGT
{
    public class Statistics
    {
        // Variables de instancia
        private double sumX; // suma de las observaciones
        private double sumX2; // suma al cuadrado de las observaciones
        private int numElem; // número de elementos de las observaciones
        /*
         * Nota:
         *  Los valores null no se cuenta a la hora de realizar las medias o la suma de 
         *  cuadrados con lo que puede ocurrir que el resultado de una determinada medición
         *  sea null.
         */

        /*
         * Descripción:
         *  Construtor de la clase Statistics. Inicializa las variables.
         */
        public Statistics()
        {
            sumX = 0;
            sumX2 = 0;
            numElem = 0;
        }

        #region Métodos de consulta
        /*=================================================================================
         * Métodos de Consulta
         *=================================================================================*/
        /*
         * Descripción:
         *  Devuelve la suma de las observaciones.
         */
        public double? SumX()
        {
            return numElem == 0 ? (double?)null : sumX;
        }


        /*
         * Descripción:
         *  Devuelve la suma de los cuadrados las observaciones.
         */
        public double? SumX2()
        {
            return numElem == 0 ? (double?)null : sumX2;
        }


        /*
         * Descripción:
         *  Devuelve el número de elementos de las observaciones.
         */
        public int NumElem()
        {
            return this.numElem;
        }

        #endregion Métodos de consulta



        /*
         * Descripción:
         *  Añade un elemento double a la suma total (sumX) y a la suma total de cuadrados (sumX2),
         *  excluyendo los valores nulos.
         * NOTA: Si pasamos NaN, convertiremos sumX y sumX2 en NaN.
         */
        public void Add(double? d)
        {
            if (d != null)
            {
                double value = d.Value; //faster than operating with double? d
                sumX += value;
                sumX2 += value * value;
                numElem++;
            }
        }


        /* Descripción:
         *  Añade un elemento double a la suma total (sumX) y a la suma total de cuadrados (sumX2).
         *  Si el segundo parámetro es true el valor null se interpretará como cero.
         */
        public void Add(double? d, bool zero)
        {
            if (d != null || zero)
            {
                double value = d.GetValueOrDefault(); // 0 if null
                sumX += value;
                sumX2 += value * value;
                numElem++;
            }
        }




        /*
         * Descripción:
         *  Devuelve la media aritmética o null en el caso de que no haya ningún valor.
         */
        public double? Mean()
        {
            if (numElem == 0)
                return null;

            return sumX / numElem;
        }


        /*
         *Descripción:
         * Devuelve la varianza o null en el caso de que no haya ningún valor.
         */
        public double? Variance()
        {
            if (numElem == 0)
                return null;

            double mean = sumX / numElem;
            return (sumX2 / numElem) - (mean * mean);
        }


        /*
         * Descripción:
         *  Devuelve la desvianción típica (o desviación estandar) o null 
         *  en el caso de que no haya ningún valor.
         */
        public double? StandardDeviation()
        {
            if (numElem == 0)
                return null;

            return Math.Sqrt(Variance().Value);
        }

    } // end public class Statistics
} // end namespace ProjectMathCalcGT
