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
        private double? sumX; // suma de las frecuencias
        private double? sumX2; // suma al cuadrado de las frecuencias
        private int numElem; // número de elementos de las frecuencias
        /*
         * Nota:
         *  Los valores null no se cuenta a la hora de realizar las medias o la suma de 
         *  cuadrados con lo que puede ocurrir que el resultado de una determinada frecuencia
         *  sea null.
         */

        /*
         * Descripción:
         *  Construtor de la clase Statistics. Inicializa las variables.
         */
        public Statistics()
        {
            sumX = null;
            sumX2 = null;
            numElem = 0;
        }

        #region Métodos de consulta
        /*=================================================================================
         * Métodos de Consulta
         *=================================================================================*/
        /*
         * Descripción:
         *  Devuelve la suma de la frecuencia.
         */
        public double? SumX()
        {
            return this.sumX;
        }


        /*
         * Descripción:
         *  Devuelve la suma de los cuadrados la frecuencia.
         */
        public double? SumX2()
        {
            return this.sumX2;
        }


        /*
         * Descripción:
         *  Devuelve el número de elementos de la frecuencia.
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
         */
        public void Add(double? d)
        {
            if (d != null)
            {
                if (sumX == null)
                {
                    sumX = d;
                    sumX2 = Math.Pow((double)d,2);
                }
                else
                {
                    sumX = sumX + d;
                    sumX2 = sumX2 + Math.Pow((double)d, 2);
                }
                numElem++;
            }
        }


        /* Descripción:
         *  Añade un elemento double a la suma total (sumX) y a la suma total de cuadrados (sumX2).
         *  Si el segundo parámetro es true el valor null se interpretará como cero.
         */
        public void Add(double? d, bool zero)
        {
            if (d==null && zero)
            {
                Add(0);
                //numElem++;
            }
            else
            {
                Add(d);
            }
        }



        /*
         * Descripción:
         *  Devuelve la media aritmética o null en el caso de que no haya ningún valor.
         */
        public double? Mean()
        {
            double? retval = sumX;
            if(retval!=null)
            {
                retval = retval/ numElem;
            }
            return retval;
        }


        /*
         *Descripción:
         * Devuelve la varianza o null en el caso de que no haya ningún valor.
         */
        public double? Variance()
        {
            double? retval = sumX;
            if (retval != null)
            {
                retval = (sumX2/numElem - Math.Pow((double)Mean(), 2));
            }
            return retval;
        }


        /*
         * Descripción:
         *  Devuelve la desvianción típica (o desviación estandar) o null 
         *  en el caso de que no haya ningún valor.
         */
        public double? StandardDeviation()
        {
            double? retval = Variance();
            if (retval != null)
            {
                retval = Math.Sqrt((double)retval);

            }
            return retval;
        }

    } // end public class Statistics
} // end namespace ProjectMathCalcGT
