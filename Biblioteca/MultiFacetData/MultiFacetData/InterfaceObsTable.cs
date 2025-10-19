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
 * Fecha de revisión: 18/Jun/2012                           
 * 
 * Descripción:
 *      Interface para tabla de frecuencias Libreria de MultiFacetData
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace MultiFacetData
{
    public interface InterfaceObsTable
    {
        /*=================================================================================================
         * Métodos de consulta
         *=================================================================================================*/
        // Devuelve el dato de la frecuencia que se encuentra en la fila row.
        double? ObsData(int row);
        // Devuelve el dato que se encuentra en la tabla de frecuancias en la fila row, columna col.
        double? Data(int row, int col);
        // Devuelve el número de columnas de la tabla
        int TableColumns();
        // Devuelve el número de filas de la tabla
        int TableRows();
        // Devuelve el valor de las suma de las frecuencias
        double? SumOfData();
        // Escribe la tabla de datos de frecuencias.
        bool WritingStreamObsTable(StreamWriter writerFile);
        // Tranforma la tabla de observaciones en un DataTable
        DataTable ObsTable2DataTable(ListFacets lf);
        // Transforma la tabla de observaciones en un DataSet
        DataSet ObsTable2DataSet(ListFacets lf);


        /*=================================================================================================
         * Métodos de instancia
         *=================================================================================================*/
        // Añade una fila a la tabla con los valores de la lista que se pasan como parámetros a la tabla
        void Add(List<double?> row);
        // Añade el dato (data)de la frecuencia en la fila (pos) que se pasa como parámetro
        void Data(double? data, int pos);
        // Asigna los valores de la lista que se pasa como parámetro como frecuencias de la tabla
        void AssignListData(List<double?> ldata);
        // Elimina un nivel para una determinada columna
        void RestoreIndexes(int skipLevel, int col);
        // Elimina las filas donde se encuentre el nivel actual para la columna especificada.
        void SkipLevels(ListFacets lf);
        // Deep clone
        ObsTable Clone();

    }// end interface InterfaceObsTable
}// end namespace MultiFacetData
