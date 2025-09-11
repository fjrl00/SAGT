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
 * Fecha de revisión: 30/May/2012                           
 * 
 * Descripción:
 *  Contiene los datos de un proyecto
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Data;

namespace ConnectLibrary
{
    public class SagtProject
    {
        /*=================================================================================
         * Variables
         *=================================================================================*/
        private int pk_project; // Clave primaria de la tabla proyecto
        private string name_project; // Nombre del proyecto
        private DateTime dateCreation; // fecha hora de la creación.
        private int id_director_project; // Número que identifica al administrador de un proyecto en la base de datos
        private string name_director_project; // Nombre del director de proyecto
        private string description; // Descripción del proyecto


        #region Constructores
        /*=================================================================================
         * Constructores
         *=================================================================================*/
        public SagtProject()
        {
            this.pk_project = 0; // Si es cero es que no ha sido asignado aun.
            this.name_project = "";
            this.dateCreation = DateTime.Now;
            this.id_director_project = 0;
            this.name_director_project = "";
            this.description = "";
        }


        public SagtProject(string name_project, string director, string description)
            : this()
        {
            if (String.IsNullOrEmpty(name_project))
            {
                throw new SagtProjectException("Necesita al menos un nombre de proyecto");
            }
            this.name_project = name_project;
            this.dateCreation = DateTime.Now;
            this.name_director_project = director;
            this.description = description;
        }


        public SagtProject(int pk, string name_project, DateTime dateCreation, string director, string description)
            : this(name_project, director, description)
        {
            this.pk_project = pk;
            this.dateCreation = dateCreation;
        }


        public SagtProject(int pk, string name_project, DateTime dateCreation, int id_director, string director, string description)
            : this(pk, name_project, dateCreation, director, description)
        {
            this.id_director_project = id_director;
        }
        #endregion Constructores

        /*=================================================================================
         * Métodos de consulta
         *=================================================================================*/
        public int GetPK_Project()
        {
            return this.pk_project;
        }
        
        
        public string GetNameProject()
        {
            return this.name_project;
        }


        public DateTime GetDateCreation()
        {
            return this.dateCreation;
        }


        public string GetNameDirectorProject()
        {
            return this.name_director_project;
        }


        public string GetDescription()
        {
            return this.description;
        }


        public int GetId_Director()
        {
            return this.id_director_project;
        }

        /*=================================================================================
         * Métodos de consulta
         *=================================================================================*/
        public void SetPK_Project(int pk)
        {
            this.pk_project = pk;
        }

        
        public void SetNameProject(string name_project)
        {
            this.name_project = name_project;
        }


        public void SetDateCreation(DateTime dateCreation)
        {
            this.dateCreation = dateCreation;
        }


        public void SetNameDirectorProject(string name_director_project)
        {
            this.name_director_project = name_director_project;
        }


        public void SetDescription(string description)
        {
            this.description  = description;
        }


        public void SetId_Director(int id_director)
        {
            this.id_director_project = id_director;
        }


        /*=================================================================================
         * DataSet
         *=================================================================================*/

        /* Descripción:
         *  Para permitir la serialización de los objetos construimos un DataSet que contiene los
         *  datos de la clase siguiendo la estructura de la base de datos en cuanto a nombre y tipos
         *  de tablas y columnas.
         */
        public DataSet Proyect2DataSet()
        {
            DataSet dsProject = new DataSet("DataSet_Project");

            DataTable dtProject = new DataTable("TbProjects");

            string nameTable = dtProject.TableName;
            DataColumn c_pk_projects = new DataColumn("pk_projects", System.Type.GetType("System.Int32"));
            DataColumn c_fk_adminst = new DataColumn("fk_administ", System.Type.GetType("System.Int32"));
            DataColumn c_name_director = new DataColumn("name_director", System.Type.GetType("System.String"));
            DataColumn c_name_project = new DataColumn("name_project", System.Type.GetType("System.String"));
            DataColumn c_date_project = new DataColumn("date_project", System.Type.GetType("System.DateTime"));
            DataColumn c_description = new DataColumn("description", System.Type.GetType("System.String"));

            // añadimos las columnas
            dtProject.Columns.Add(c_pk_projects);
            dtProject.Columns.Add(c_fk_adminst);
            dtProject.Columns.Add(c_name_director);
            dtProject.Columns.Add(c_name_project);
            dtProject.Columns.Add(c_date_project);
            dtProject.Columns.Add(c_description);

            // Añadimos el dataTable al DataSet
            dsProject.Tables.Add(dtProject);

            // Creamos una nueva fila
            DataRow newProjectRow = dsProject.Tables["TbProjects"].NewRow();

            // Rellenamos la fila
            newProjectRow["pk_projects"] = this.GetPK_Project();
            newProjectRow["fk_administ"] = this.GetId_Director();
            newProjectRow["name_director"] = this.GetNameDirectorProject();
            newProjectRow["name_project"] = this.GetNameProject();
            newProjectRow["date_project"] = this.GetDateCreation();
            newProjectRow["description"] = this.GetDescription();

            // Añadimos los datos al dataSet
            dsProject.Tables["TbProjects"].Rows.Add(newProjectRow);

            string nameDataTable = dsProject.Tables[0].TableName;
            // Devolvemos el DataSet
            return dsProject;
        }// end Proyect2DataSet


        /* Descripción:
         *  Dado un DataTable construye si el posible un objetoSagtProject si no devuelve null
         */
        public static SagtProject DataSet2Project(DataSet dataSetProject, string nameDirector)
        {
            int numTables = dataSetProject.Tables.Count;
            DataTable dtPrueba = dataSetProject.Tables[0];
            string nameTabla = dtPrueba.TableName;
            int numCols = dtPrueba.Columns.Count;
            for (int i = 0; i < numCols; i++ )
            {
                string nameCol = dtPrueba.Columns[i].ColumnName;
            }

            SagtProject newSagtProject = null;
            DataTable dt = dataSetProject.Tables["TbProjects"];

            if (dt.Rows.Count == 1)
            {
                DataRow row = dt.Rows[0]; // linea de prueba borrame
                int pk_projects = (int)dt.Rows[0]["pk_projects"];
                string adminst = nameDirector; // Aun no esta implementado
                int fk_adminst = (int)dt.Rows[0]["fk_administ"];
                string name_project = (string)dt.Rows[0]["name_project"];
                DateTime date_project = (DateTime)dt.Rows[0]["date_project"];
                string description = (string)dt.Rows[0]["description"];

                newSagtProject = new SagtProject(pk_projects, name_project, date_project, fk_adminst, adminst, description);
            }
            // Devolvemos el valor
            return newSagtProject;
        }// end SagtProject DataSet2Project

        /*=================================================================================
         * Métodos Redefinidos
         *=================================================================================*/

        public override string ToString()
        {
            return "[name= " + this.name_project + ", date=" + this.dateCreation.ToString() 
                + ", name_director=" + this.name_director_project + ", description=" + this.description
                + "]";
        }


        public override Boolean Equals(Object obj)
        {
            // Variable de retorno
            Boolean res = false;
            if (!(obj == null || GetType() != obj.GetType()))
            {
                SagtProject project = (SagtProject)obj;

                res =  this.pk_project.Equals(project.pk_project)
                        && this.name_project.ToUpper().Equals(project.name_project.ToUpper());
            }
            return res;
        }


        public override int GetHashCode()
        {
            return (this.pk_project + this.name_project.GetHashCode())/3;
        }


    }// end class SagtProject
}// end namespace ConnectLibrary