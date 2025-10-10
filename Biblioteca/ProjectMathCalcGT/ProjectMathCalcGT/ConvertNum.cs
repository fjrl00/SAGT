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
        // CONSTANTES (separador decimal)
        public const string DECIMAL_SEPARATOR_PERIOD = ".";
        public const string DECIMAL_SEPARATOR_COMMA = ",";

        #region Procedimientos adicionales para la lectura escritura de archivos

        /* Descripción:
         *  Transforma un double a string teniendo en cuenta que si el valor es null devolvera "NULL".
         *  Esto es para facilitar su escritura/lectura en un archivo.
         *  Si el valor es NaN (Not a Number) se devuelve "NaN".
         */
        public static string Double2String(double? d)
        {
            return d?.ToString() ?? STRING_NULL;
        }

        /* Descripción:
         *  Transforma un String en un double teniendo en cuenta que si el valor es null devolvera "NULL".
         *  Esto es para facilitar su escritura/lectura de un archivo.
         */
        public static double? String2Double(string s)
        {
            double? d = null;
            if (!string.IsNullOrEmpty(s) && !string.Equals(s, STRING_NULL, StringComparison.OrdinalIgnoreCase))
            {
                // d = double.Parse(s, NumberFormatInfo.InvariantInfo);
                if (s.Contains(DECIMAL_SEPARATOR_PERIOD))
                {
                    // el separador decimal es un punto
                    d = double.Parse(s, System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture);
                }
                else if (s.Contains(DECIMAL_SEPARATOR_COMMA))
                {
                    // el separador decimal es una coma
                    d = double.Parse(s, System.Globalization.NumberStyles.Float, new CultureInfo("es-ES"));
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
         * NOTA: si d es null devuelve "", si d es NaN devuelve "NaN".
         */
        public static string DecimalToString(double? d, int numOfDecimal, string separator)
        {
            string retVal = "";// valor de retorno

            if (d != null)
            {
                if(separator.Equals(DECIMAL_SEPARATOR_COMMA))
                {
                    retVal = d.Value.ToString($"F{numOfDecimal}", new CultureInfo("es-ES"));
                }
                else if (separator.Equals(DECIMAL_SEPARATOR_PERIOD))
                {
                    retVal = d.Value.ToString($"F{numOfDecimal}", CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new ConvertNumException("Error: No es un separador válido");
                }
            }
            return retVal;
        }// DecimalSetting


        /* Descripción:
         *  Devuelve el string que se pasa como parámetro cambiandole el separador decimal por el que se pasa
         *  como parametros
         * NOTA: si d es null devuelve "", si d es NaN devuelve "". Conducta diferente a la del otro método DecimalToString. INVESTIGAR
         */
        public static string DecimalToString(double? d, string separator)
        {
            string retVal = "";// valor de retorno

            if (d != null)
            {
                // comprobamos si d no es un número (NeuN)
                if (!double.IsNaN((double)d))
                {
                    if (!separator.Equals(DECIMAL_SEPARATOR_PERIOD) && !separator.Equals(DECIMAL_SEPARATOR_COMMA))
                    {
                        throw new ConvertNumException("Error: No es un separador válido");
                    }

                    retVal = d.ToString();
                    retVal = retVal.Replace(DECIMAL_SEPARATOR_COMMA, separator);
                    retVal = retVal.Replace(DECIMAL_SEPARATOR_PERIOD, separator);
                }
            }
            return retVal;
        }

        #endregion Métodos para el cambio de los decimales
    }// class ConvertNum
}// end namespace ProjectMathCalcGT
