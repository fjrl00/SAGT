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
 *      
 *      Para cada fuente de variabilidad que se corresponda con una fuente de 
 *      instrumentación esta tendrá dos pares de valores que pueden ser nulos. 
 *      Un valor se corresponde con el valor relativo y otro con el valor absoluto.
 */
namespace ProjectSSQ
{
    public class ErrorVar
    {
        // Variables de instancia
        private double? relErrorVar; // Varianza del error relativo
        private double? absErrorVar; // Varianza del error absoluto

        // Constructor de la clase
        public ErrorVar(double? rel, double? abs)
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
            return "Varianza de error absoluta: " + absString + "\n" +
                "Varianza de error relativo: " + relString;
        }

        #region Clonación

        /* Descripción:
         *  Devuelve una copy en profundidad del objeto.
         */
        public ErrorVar Clone()
        {
            double? copyRelErrorVar = this.relErrorVar; // Varianza del error relativo
            double? copyAbsErrorVar = this.absErrorVar; // Varianza del error absoluto

            return new ErrorVar(copyRelErrorVar, copyAbsErrorVar);
        }

        #endregion Clonación

    } // end public class ErrorVar
}// enb namespace ProjectSSQ
