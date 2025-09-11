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
 * Fecha de revisión: 14/Jun/2012                           
 * 
 * Descripción:
 *      Interface para establecer conexión con la base de datos en el servicio web 
 *      y generar consultas, inserciones y borrados.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ConnectLibrary
{
    public interface InterfaceConnectDB_WS: InterfaceConnectDB
    {
        /*=================================================================================================
         * Métodos de inserción
         *=================================================================================================*/
        // Inserta un proyecto en la base de datos
        int Insert_Project(DataSet project);
        // Devuelve un DataSet con los proyectos con el mismo nombre.
        DataSet SelectSameProyects(SagtProject project);
        // Devuelve un DataTable con los proyectos con el patron de busqueda nombre y descripción.
        DataSet SelectLikeProyects(String name, string description);
        // Devuelve todos los proyectos 
        DataSet SelectProyectsForUsers();
        // Devuelve un DataSet con todos los proyectos a los que tiene acceso el administrador
        DataSet SelectProyectsForUsers(int userId);
        // Devuelve un dataSet con cargado de nombres iguales que se pasa como parámetro.
        DataSet SelectNameFilesProject(string name_file, int fk_project, TypeFile typeFile);
        // Devuelve un DataSet con los datos de la Tabla TbFiles.
        DataSet SelectFiles(int fk_project, int fk_user, TypeFile typeFile);

    }// end interface InterfaceConnectDB_WS
}// end namespace ConnectLibrary
