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
 * Fecha de revisión: 21/Feb/2012                           
 * 
 * Descripción:
 *      Libreria de suma de cuadrados.
 *      Contiene los valores de las varianzas de error o de su porcetanje de error.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSSQ
{
    public class ErrorVar : System.ICloneable
    {
        // Variables de instancia
        private double? relErrorVar; // Varianza del error relativo
        private double? absErrorVar; // Varianza del error absoluto

        // Constructor de la clase
        public ErrorVar(double? rel,double? abs)
        {
            this.relErrorVar = rel;
            this.absErrorVar = abs;
        }


        // Metodos de consulta
        public double? RelErrorVar()
        {
            return this.relErrorVar;
        }


        public double? AbsErrorVar()
        {
            return this.absErrorVar;
        }


        // Métodos de instancia
        public void RelErrorVar(double? rel)
        {
            this.relErrorVar = rel;
        }


        public void AbsErrorVar(double? abs)
        {
            this.absErrorVar = abs;
        }


        // Métodos redefinidos
        public override string ToString()
        {
            string absString = "";
            if (this.absErrorVar != null)
            {
                absString = this.absErrorVar.ToString();
            }
            string relString = "";
            if (this.relErrorVar != null)
            {
                relString = this.relErrorVar.ToString();
            }
            return "Varianza de error absoluta: "+absString+"\n"+
                "Varianza de error relativo: "+relString;
        }

         #region Implementacion de la interfaz
        /******************************************************************************************************
         *  Implementacion de la interfaz Cloneable
         *  =======================================
         ******************************************************************************************************/

        /* Descripción:
         *  Devuelve una copy en profundidad del objeto.
         */
        public object Clone()
        {
            double? copyRelErrorVar = this.relErrorVar; // Varianza del error relativo
            double? copyAbsErrorVar = this.absErrorVar; // Varianza del error absoluto

            return new ErrorVar(copyRelErrorVar, copyAbsErrorVar);
        }

        #endregion Implementacion de la interfaz

    } // end public class ErrorVar
}// enb namespace ProjectSSQ
