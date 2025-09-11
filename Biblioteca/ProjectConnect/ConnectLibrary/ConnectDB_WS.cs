using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ConnectLibrary
{
    public class ConnectDB_WS : ConnectDB, InterfaceConnectDB_WS
    {
        /*=================================================================================
         * Constructores
         *=================================================================================*/
        public ConnectDB_WS()
            :base()
        {
        }


        public ConnectDB_WS(string pathDB)
            :base(pathDB)
        {
        }


        /*=================================================================================
         * Métodos de inserción
         *=================================================================================*/
        // Inserta un proyecto en la base de datos
        public int Insert_Project(DataSet project)
        {
            // Inicialmente un projecto no tiene director por tanto
            SagtProject sagtProject = SagtProject.DataSet2Project(project, "");
            int pk_project = base.Insert_Project(sagtProject);
            return pk_project;
        }



        /* Descripción:
         *  Devuelve un DataSet con los proyectos con el mismo nombre.
         */
        public DataSet SelectSameProyects(DataSet project, string nameDirector)
        {
            SagtProject sagtProject = SagtProject.DataSet2Project(project, nameDirector);
            DataSet ds = base.SelectSameProyects(sagtProject);
            return ds;
        }


        /* Descripción:
         *  Devuelve un DataSet cargado con los nombres de los archivos iguales al que se pasa como parámetro.
         */
        public DataSet SelectNameFilesProject(string name_file, int fk_project, string type_file)
        {
            TypeFile typeFile = (TypeFile)Enum.Parse(typeof(TypeFile), type_file);
            return base.SelectNameFilesProject(name_file, fk_project, typeFile);
        }


        // Devuelve un DataSet con los datos de la Tabla TbFiles.
        public DataSet SelectFiles(int fk_project, int fk_user, string type_file)
        {
            TypeFile typeFile = (TypeFile)Enum.Parse(typeof(TypeFile), type_file);
            return base.SelectFiles(fk_project, fk_user, typeFile);
        }

    }// end class ConnectDB_WS
}// namespace ConnectLibrary
