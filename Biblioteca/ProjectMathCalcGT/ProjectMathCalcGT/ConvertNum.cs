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
 * Fecha de revisión: 01/Jun/2012
 * 
 * Descripción:
 *      Convierte un dato a string o un string a dato
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AuxMathCalcGT
{
    public class ConvertNum
    {
        /*=================================================================================
         * Constantes
         *=================================================================================*/
        public const string STRING_NULL = "NULL";
        public const string STRING_N_E_U_N = "NeuN";
        // CONSTANTES (separador decimal)
        public const string DECIMAL_SEPARATOR_PERIOD = ".";
        public const string DECIMAL_SEPARATOR_COMMA = ",";

        #region Procedimientos adicionales para la lectura escritura de archivos

        /* Descripción:
         *  Transforma un double a string teniendo en cuenta que si el valor es null devolvera "NULL".
         *  y si no es un número devolverá "NeuN".
         *  Esto es para facilitar su escritura/lectura en un archivo.
         */
        public static string Double2String(double? d)
        {
            string retVal = d.ToString();
            if (d == null)
            {
                retVal = STRING_NULL;
            }
            else if (double.IsNaN((double)d))
            {
                retVal = STRING_N_E_U_N;
            }
            return retVal;
        }


        /* Descripción:
         *  Transforma un String en un double teniendo en cuenta que si el valor es null devolvera "NULL".
         *  Esto es para facilitar su escritura/lectura de un archivo.
         */
        public static double? String2Double(string s)
        {
            double? d = null;
            if (!s.ToUpper().Equals(STRING_NULL) && !string.IsNullOrEmpty(s))
            {
                // d = double.Parse(s, NumberFormatInfo.InvariantInfo);
                if (s.Contains(DECIMAL_SEPARATOR_PERIOD))
                {
                    // el separador decimal es un punto
                    d = double.Parse(s, System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo("en-US"));
                }
                else if (s.Contains(DECIMAL_SEPARATOR_COMMA))
                {
                    // el separador decimal es una coma
                    d = double.Parse(s, System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo("es-ES"));
                }
                else
                {
                    // No contiene separador decimal
                    d = double.Parse(s);
                }
                
            }
            return d;
        }

        #endregion Procedimientos adicionales para la lectura escritura de archivos

        #region Métodos para el cambio de los decimales

        /* Descripción:
         *  Devuelve el string de representar el double que se pasa como parámetro con el número de decimales
         *  representados por la configuración del sistema.
         * Parámetros:
         *      double d: double al queremos normalizar el numéro de decimales.
         *      int numOfDecimal: número de decimales que empleamos para representar el d.
         *      string puntoDecimal: Separador decimal.
         */
        public static string DecimalToString(double? d, int numOfDecimal, string separator)
        {
            string retVal = "";// valor de retorno

            if (!separator.Equals(DECIMAL_SEPARATOR_PERIOD) && !separator.Equals(DECIMAL_SEPARATOR_COMMA))
            {
                throw new ConvertNumException("Error: No es un separador válido");
            }

            if (d != null)
            {
                // comprobamos si d no es un número (NeuN)
                if (!double.IsNaN((double)d))
                {
                    double auxDouble = Math.Round((double)d, numOfDecimal);
                    // string puntoDecimal = this.cfgApli.GetDecimalSeparator();
                    char[] c = { ',', '.' };
                    string[] s = (auxDouble.ToString()).Split(c);

                    if (s.Length == 1)
                    {
                        // Si tiene longitud 1 entonces solo concatenamos ceros
                        if (numOfDecimal == 0)
                        {
                            retVal = s[0];
                        }
                        else
                        {
                            retVal = s[0] + separator + StringZeros(numOfDecimal);
                        }

                    }
                    else
                    {
                        int s1Lentgh = s[1].Length;
                        if (s1Lentgh == numOfDecimal)
                        {
                            // retVal = auxDouble.ToString();
                            retVal = s[0] + separator + s[1];
                        }
                        else if (s1Lentgh < numOfDecimal)
                        {

                            retVal = s[0] + separator + s[1] + StringZeros(numOfDecimal - s1Lentgh);
                        }
                    }
                }
                else
                {
                    retVal = d.ToString();
                }

            }
            return retVal;
        }// DecimalSetting


        /* Descripción:
         *  Devuelve el string que se pasa como parámetro cambiandole el separador decimal por el que se pasa
         *  como parametros
         */
        public static string DecimalToString(double? d, string separator)
        {
            string retVal = "";// valor de retorno

            if (!separator.Equals(DECIMAL_SEPARATOR_PERIOD) && !separator.Equals(DECIMAL_SEPARATOR_COMMA))
            {
                throw new ConvertNumException("Error: No es un separador válido");
            }

            if (d != null)
            {
                // comprobamos si d no es un número (NeuN)
                if (!double.IsNaN((double)d))
                {
                    double auxDouble = (double)d;
                    
                    retVal = auxDouble.ToString();

                    retVal = retVal.Replace(DECIMAL_SEPARATOR_COMMA, separator);
                    retVal = retVal.Replace(DECIMAL_SEPARATOR_PERIOD, separator);
                }
            }
            return retVal;
        }



        /*
         * Descripción:
         *  Operación auxiliar. Devuelve un string de ceros de la longitud que se pasa como parámetro.
         * Parámetros:
         *      int n: La longitud que ha de tener la cadena de ceros que se devuelve.
         */
        private static string StringZeros(int n)
        {
            StringBuilder retVal = new StringBuilder();
            for (int i = 0; i < n; i++)
            {
                retVal.Append("0");
            }
            return retVal.ToString();
        }

        #endregion Métodos para el cambio de los decimales
    }// class ConvertNum
}// end namespace ProjectMathCalcGT
