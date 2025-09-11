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
 *      Iterface para tabla de medias Libreria de medias
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using MultiFacetData;

namespace ProjectMeans
{
    public interface InterfaceTableMeans
    {
        /*=================================================================================================
         * Métodos de consulta
         *=================================================================================================*/
        
        double? Data(int row, int col); // Devuelve el dato que se encuentra en la fila row, columna col
        double? MeanData(int row); // Devuelve el dato de media que se encuentra en la fila row
        double? VarianceData(int row); // Devuelve el dato de varianza que se encuentra en la fila row
        double? Std_dev_Data(int row); // Devuelve el dato de desviación típica que se encuentra en la fila row
        int MeansTableColumns(); // número de fila de la tabla
        int MeansTableRows();    // número de columnas de la tabla
        ListFacets ListFacets(); // Devuelve la lista de facetas de la tabla de medias
        string FacetDesign(); // Devuelve el diseño de la tabla de la lista de facetas
        double? GrandMean(); // Devuelve la gran media de la tabla
        double? Variance(); // Devuelve la varianza total de la tabla
        double? StdDev(); // Devuelve la desviación típica de la tabla
        DataSet TableMeans2DataSet(); // Convierte la tabla de medias en un DataSet


        /*=================================================================================================
         * Métodos de instancia
         *=================================================================================================*/
        
        void MeanData(double? data, int row); // Asigna el valor de una media en una fila.
        void VarianceData(double? data, int row); // Asigna el valor de una varianza en una fila. 
        void Std_dev_Data(double? data, int row); // Asigna el valor de una desviación típica en una fila.
        void InsertDataInPos(double? data, int row, int col); //inserta el dato en la posición especificada en los
                                                              // parámetros
        void Calc_GrandMean_Variance_StdDev(MultiFacetsObs mfo, bool zero); // calcula la media la varianza y la desviación típica.
        void GrandMean(double? d); // asigna el valor de la gran media
        void Variance(double? d); // Asigna el valor de la varianza de la tabla
        void StdDev(double? d); // Asigna el valor de la desviación típica de la tabla

        bool WritingStreamTableMeans(StreamWriter writerFile);

        // Convierte un dataSet en un Tabla de medias
        // InterfaceTableMeans DataSet2TableMeans(DataSet dsTableMeans);

    }// end interface InterfaceTableMeans
}// end namespace ProjectMeans