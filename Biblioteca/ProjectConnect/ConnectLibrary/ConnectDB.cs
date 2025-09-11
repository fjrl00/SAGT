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
 * Fecha de revisión: 12/Jun/2012
 * 
 * Descripción:
 *      Inserta y recupera los datos de la base de datos
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient; // Para usar sqlParameter
using System.Globalization;
using Sagt;
using MultiFacetData;
using ProjectMeans;
using ProjectSSQ;
using AuxMathCalcGT;
using ConfigCFG;
// using System.Windows.Forms;


namespace ConnectLibrary
{
    public class ConnectDB : InterfaceConnectDB
    {
        /*=================================================================================
         * Variables
         *=================================================================================*/
        // public string fileDataBase = Application.StartupPath + "bd_sagt.mdb;";
        public string pathDataBase = @"E:\bd_sagt.mdb;";

        private AccessDB dataBase;

        // Constante que determina la columna de variables observadas
        private string COLMN_VAR_OBS = "var_obs";
        // Constante que determina la columna de medias
        private string COLMN_MEANS = "means";
        // Constante que determina la columna de varianza
        private string COLMN_VARIANCE = "variance";
        // Constante que determina la columna de desviación típica
        private string COLUMN_STAND_DEV = "stand_dev";
        // Constante que determina la columna de diferencia de medias
        private string COLUMN_MEANS_DIF = "means_dif";
        // Constante que determina la columna de diferencia de varianzas
        private string COLUMN_VARIANCE_DIF = "variance_dif";
        // Constante que determina la columna de diferencia de desviación típica
        private string COLUMN_STAND_DEV_DIF = "stand_dev_dif";
        // Constante que determina la columna de puntuación típica
        private string COLUMN_TYPSCORE = "typScore";
        // Nombre del DataSet proyectos 
        private string DATASET_PROJECTS = "DataSet_Project";
        // Nombre de el dataTable Projectos
        private string DATATABLE_PROJECTS = "TbProjects";

        



        /*=================================================================================
         * Constructores
         *=================================================================================*/
        public ConnectDB()
        {
            this.dataBase = new AccessDB(pathDataBase);
        }


        public ConnectDB(string pathDB)
        {
            this.pathDataBase = pathDB;
            this.dataBase = new AccessDB(this.pathDataBase);
        }


        /*=================================================================================
         * Métodos de inserción
         *=================================================================================*/



        #region Operaciones con usuarios
        /* Descripción:
         *  Inserta un usuario en la base de datos solo en el caso de que no se encuentre. Devuelve la clave primaria.
         */
        public int Add_SagtUser(string name_users)
        {
            // Valor de retorno
            int pk_user = 0;
            // Primero compruebo si se encuentra insertado
            string consulta = "  SELECT  pk_users   FROM    TbUsers WHERE( name_users = '" + name_users + "') ";

            DataTable dt = this.dataBase.Select2DataTable(consulta);
            if (dt.Rows.Count != 0)
            {
                // ya esta insertada
                pk_user = (int)dt.Rows[0][0];
            }
            else
            {
                // No esta insertada y debemos insertarla
                string insertar = "Insert into TbUsers(name_users) VALUES('" + name_users + "')";
                this.dataBase.Insert(insertar);

                dt = this.dataBase.Select2DataTable(consulta);
                pk_user = (int)dt.Rows[0][0];
            }

            return pk_user;
        }// end Insert_SagtUser

        #endregion Operaciones con usuarios



        #region Operaciones con proyectos

        /* Descripción:
         *  Inserta un proyecto en la base de datos
         */
        public int Insert_Project(SagtProject project)
        {
            String cadenaInsert;


            cadenaInsert = "Insert into TbProjects(name_projects, date_project, description) ";
            //cadenaOleDb = "Insert into TbProjects(name_projects, date_project, description, fk_administ) ";

            cadenaInsert += "VALUES('"+ project.GetNameProject() 
                + "', '" + project.GetDateCreation().ToString() 
                + "', '" + project.GetDescription() + "');";

            
            string cadenaSelect = "  SELECT  pk_projects   FROM    TbProjects " 
                + "WHERE(TbProjects.name_projects = '" + project.GetNameProject()
                + "' AND TbProjects.date_project like '" + project.GetDateCreation().ToString()
                + "' AND TbProjects.description = '" + project.GetDescription() + "')";


            this.dataBase.Insert(cadenaInsert);

            DataTable dt = this.dataBase.Select2DataTable(cadenaSelect);
            int pk = (int)dt.Rows[0][0];

            return pk;
        }// end Insert_Project

        //---------------------------------------------------------------------------------------------------------

        /* Descripción:
         *  Devuelve un DataSet con los proyectos dirigidos por un usuario
         */
        public DataSet SelectSameProyects(int userID)
        {
            string sentenciaSQL = "  SELECT  *     FROM    TbProjects WHERE (TbProjects.fk_administ = " + userID+ ")";

            DataTable dt = this.dataBase.Select2DataTable(sentenciaSQL);// creo un datatable con todas las tablas de mi base de datos
            dt.TableName = DATATABLE_PROJECTS;
            DataSet ds = new DataSet(DATASET_PROJECTS);
            ds.Tables.Add(dt);
            return ds;
        }


        /* Descripción:
         *  Devuelve un DataSet con los proyectos con el mismo nombre.
         */
        public DataSet SelectSameProyects(SagtProject project)
        { 
            string sentenciaSQL = "  SELECT  *     FROM    TbProjects WHERE (TbProjects.name_projects = '" + project.GetNameProject() + "')";

            DataTable dt = this.dataBase.Select2DataTable(sentenciaSQL);// creo un datatable con todas las tablas de mi base de datos
            dt.TableName = DATATABLE_PROJECTS;
            DataSet ds = new DataSet(DATASET_PROJECTS);
            ds.Tables.Add(dt);
            return ds;
        }



        /* Descripción:
         *  Devuelve un DataSet con los proyectos con el patron de busqueda nombre y descripción.
         */
        public DataSet SelectLikeProyects(String name, string description)
        {
            string sentenciaSQL;

            string stringLike = "";
            if (!string.IsNullOrEmpty(name))
            {
                stringLike += "TbProjects.name_projects LIKE '" + name + "%'";
            }
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(description))
            {
                stringLike += " AND "; 
            }

            if (!string.IsNullOrEmpty(description))
            {
                stringLike += "TbProjects.description LIKE '" + description + "%'";            
            }

            // sentenciaSQL = "  SELECT Alumnos.*     FROM    ((Alumnos INNER JOIN  Matricula ON Alumnos.ID_Alumno_DNI = Matricula.ID_Alumno_DNI) INNER JOIN   Cursos ON Matricula.ID_Curso = Cursos.ID_Curso) WHERE        (cursos.ID_Curso =" + codigoFPE + ")";
            
            if(!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(description))
            {
                stringLike = " WHERE (" + stringLike + ")";
            }

            // sentenciaSQL = "  SELECT  *     FROM    TbProjects " + stringLike; 
            sentenciaSQL = "  SELECT  *     FROM    TbProjects WHERE (TbProjects.name_projects LIKE '%" + name
                + "%' AND TbProjects.description LIKE '%" + description + "%');";


            DataTable dt = this.dataBase.Select2DataTable(sentenciaSQL);// creo un datatable con todas las tablas de mi base de datos
            dt.TableName = DATATABLE_PROJECTS;
            DataSet ds = new DataSet(DATASET_PROJECTS);
            ds.Tables.Add(dt);
            return ds;
        }// end SelectLikeProyects 


        /* Descripción:
         *  Devuelve un DataSet con todos los proyectos
         */
        public DataSet SelectProyectsForUsers()
        {
            string sentenciaSQL;

            sentenciaSQL = "  SELECT  *     FROM    TbProjects ;";

            DataTable dt = this.dataBase.Select2DataTable(sentenciaSQL);// creo un datatable con todas las tablas de mi base de datos
            dt.TableName = DATATABLE_PROJECTS;
            DataSet ds = new DataSet(DATASET_PROJECTS);
            ds.Tables.Add(dt);
            return ds;
        }


        /* Descripción:
         *  Devuelve un DataSet con todos los proyectos a los que puede administrar un usuario
         */
        public DataSet SelectProyectsForUsers(int userId)
        {
            string sentenciaSQL;

            sentenciaSQL = "  SELECT  *     FROM    TbProjects WHERE ( fk_administ = " + userId + ");";

            DataTable dt = this.dataBase.Select2DataTable(sentenciaSQL);// creo un datatable con todas las tablas de mi base de datos
            dt.TableName = DATATABLE_PROJECTS;
            DataSet ds = new DataSet(DATASET_PROJECTS);
            ds.Tables.Add(dt);
            return ds;
        }


        /* Descripción:
         *  Devuelve un DataSet cargado con los nombres de los archivos iguales al que se pasa como parámetro.
         */
        public DataSet SelectNameFilesProject(string name_file, int fk_project, TypeFile typeFile)
        {
            string sentenciaSQL;

            sentenciaSQL = "  SELECT  TbFiles.name_file     FROM    TbFiles   WHERE ( TbFiles.fk_project = " + fk_project
                + " AND TbFiles.name_file = '" + name_file + "' AND "
                + "TbFiles.type_file = '" + typeFile.ToString() + "');";

            DataTable dt = this.dataBase.Select2DataTable(sentenciaSQL);// creo un datatable con todas las tablas de mi base de datos
            dt.TableName = DATATABLE_PROJECTS;
            DataSet ds = new DataSet(DATASET_PROJECTS);
            ds.Tables.Add(dt);
            return ds;
        }


        /* Descripción:
         *  Actualiza los datos de un proyecto
         */
        public void UpdateSagtProject(SagtProject project)
        {
            String sentenciaSQL;

            sentenciaSQL = "Update TbProjects SET fk_administ= " + project.GetId_Director() +
                ", name_projects = '" + project.GetNameProject() +
                "', date_project = '" + project.GetDateCreation().ToString() +
                "', description = '" + project.GetDescription() +
                 "'  WHERE pk_projects = " + project.GetPK_Project() + " ";   // mete la clave primaria que es autonumerica

            this.dataBase.Update(sentenciaSQL);
        }

        #endregion Operaciones con Proyectos



        #region Operaciones con ficheros SAGT

        /* Descripción:
         *  Devuelve un DataSet con los datos de la Tabla TbFiles. El dataTable contendrá la clave primaria
         *  de la tabla, el nombre y la fecha del archivo.
         */
        public DataSet SelectFiles(int fk_project, int fk_user, TypeFile typeFile)
        {
            string sentenciaSQL = "  SELECT  TbFiles.pk_file ,TbFiles.name_file, TbFiles.date_file     FROM    TbFiles WHERE ("
                + " TbFiles.fk_project = " + fk_project + " AND TbFiles.fk_user = " + fk_user 
                + " AND  TbFiles.type_file = '"  + typeFile.ToString() + "');";

            DataSet dt = this.dataBase.Select2DataSet(sentenciaSQL);
            return dt;
        }


        /* Descripción:
         *  Inserta un objeto Sagt en la base de datos en el proyecto especificado.
         */
        public int Insert_SagtFile(SagtFile sagtFile, string nameFile, int fk_projects, int fk_user)
        {
            String cadenaInsertSagtFile;

            // Tabla de frecuencias
            int fk_muti_facet_obs = 0;
            MultiFacetsObs mfo = sagtFile.GetMultiFacetsObs();

            if (mfo != null)
            {
                // Insertamos y recuperamos la clave primaria para usarla como foranea
                fk_muti_facet_obs = Insert_MultiFacetsObs(mfo);
            }

            // Tabla de Medias
            int fk_list_means = 0;
            ListMeans listMeans = sagtFile.GetListMeans();

            if (listMeans!=null)
            {
                fk_list_means = Insert_ListMeans(listMeans);
            }

            // Tabla de análisis
            int fk_analysis_and_g_study = 0;
            Analysis_and_G_Study analysis = sagtFile.GetAnalysis_and_G_Study();
            if (analysis != null)
            {
                fk_analysis_and_g_study = this.Insert_Analysis(analysis);
            }


            // la clave primaria de la tabla TbFiles es: "pk_file"
            // la fecha de inserción se generá automaticamente "date_file"
            cadenaInsertSagtFile = "Insert into TbFiles(name_file, type_file, fk_project, fk_multi_facet_obs, fk_list_means, fk_analysis_and_g_study, fk_user) ";

            cadenaInsertSagtFile += " VALUES('" + nameFile + "', '" + TypeFile.sagt.ToString() + "', " + fk_projects
                + ", " + fk_muti_facet_obs + ", " + fk_list_means + ", " + fk_analysis_and_g_study + ", " + fk_user + ");";


            // Devolvemos la clave primaria
            this.dataBase.Insert(cadenaInsertSagtFile);
            string select = "SELECT MAX(pk_File) FROM TbFiles";
            DataTable dt = this.dataBase.Select2DataTable(select);

            int pk = (int)dt.Rows[0][0];
            return pk;
        }// end Insert_Project


         /* Descripción:
         *  Inserta un objeto fichero Analysis en la base de datos en el proyecto especificado.
         */
        public int Insert_AnalysisFile(Analysis_and_G_Study analysisFile, string nameFile, int fk_projects, int fk_user)
        {
            String cadenaInsertSagtFile;

            // Tabla de análisis
            int fk_analysis_and_g_study = 0;

            if (analysisFile != null)
            {
                fk_analysis_and_g_study = this.Insert_Analysis(analysisFile);
            }
            else
            {
                throw new ConnectDB_Exception("No hay analysis para guardar");
            }

            // la clave primaria de la tabla TbFiles es: "pk_file"
            // la fecha de inserción se generá automáticamente "date_file"
            cadenaInsertSagtFile = "Insert into TbFiles(name_file, type_file, fk_project, fk_analysis_and_g_study, fk_user) ";

            cadenaInsertSagtFile += " VALUES('" + nameFile + "', '" + TypeFile.anls.ToString() + "', " + fk_projects
                + ", " + fk_analysis_and_g_study + ", " + fk_user + ");";


            // Devolvemos la clave primaria
            this.dataBase.Insert(cadenaInsertSagtFile);
            string select = "SELECT MAX(pk_File) FROM TbFiles";
            DataTable dt = this.dataBase.Select2DataTable(select);

            int pk = (int)dt.Rows[0][0];
            return pk;
        }


        /* Descripción:
         *  Almacena los datos de la tabla de frecuencias en la base de datos. Los datos se almacenan
         *  en la tabla TbDataObservation. Se realiza un mapeo de los datos de memoria.
         */
        public int Insert_MultiFacetsObs(MultiFacetsObs mfo)
        {
            // primero inserto la lista de facetas
            int fk_list_facets = Insert_ListFacets(mfo.ListFacets());

            string nameFile = mfo.NameFileObs();
            int pos = nameFile.LastIndexOf("\\") + 1; // le sumamos uno para obtener la posición 
            // siguiente a la de la barra invertida
            nameFile = nameFile.Substring(pos);

            // Inserto los datos de mfo en la tabla TbMultiFacetObs
            string insertMultiFacet = "Insert into TbMultiFacetObs (name_file, description, comment, fk_list_facets) ";
            insertMultiFacet += " VALUES ('" + nameFile + "', '" + mfo.DescriptionFile() + "', '" + mfo.Comment() + "', " + fk_list_facets + ")";

            this.dataBase.Insert(insertMultiFacet);
            string select = "SELECT MAX(pk_multi_facet_obs) FROM TbMultiFacetObs ";
            
            DataTable dt = this.dataBase.Select2DataTable(select);
            int fk_multi_facet_obs= (int)dt.Rows[0][0];


            // Inserto la tabla de Observaciones
            InterfaceObsTable obs = mfo.ObservationTable();

            int nRows = obs.ObsTableRows();
            int nCols = obs.ObsTableColumns();

            ListFacets lf = mfo.ListFacets();

            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    string nameColumn = COLMN_VAR_OBS;
                    if (j < nCols - 1)
                    {
                        nameColumn = lf.FacetInPos(j).Name();
                    }

                    double? d = obs.Data(i, j);

                    string insertData = "Insert into TbDataObservation (fk_multi_facet_obs, row, name_column";
                        if(d!=null){
                            insertData += ", data_cell";
                        }
                        insertData += ") ";
                        insertData += " VALUES (" + fk_multi_facet_obs + ", " + i + ", '" + nameColumn + "'";
                        if(d!=null)
                        {
                            insertData += ", " + ConvertNum.DecimalToString(d, ConvertNum.DECIMAL_SEPARATOR_PERIOD);
                        }
                        insertData += ")";
                    this.dataBase.Insert(insertData);
                }
            }
            
            return fk_multi_facet_obs;
        }// end Insert_MultiFacetsObs



        /* Descripción:
         *  Inserta los datos de una lista de facetas en la base de datos y devuelve la clave primaria
         *  de esa lista.
         */
        public int Insert_ListFacets(ListFacets lf)
        {
            int pk_list_facets = AuxInsert_ListFacets();

            int numFacets = lf.Count();

            for (int i = 0; i < numFacets; i++)
            {
                Facet f = lf.FacetInPos(i);
                int pk_facet = Insert_Facet(f);
                /*
                 */
                string insertFacet_X_pos = "INSERT into TbPosFacets(fk_facets, fk_list_facets, position_facets) ";
                insertFacet_X_pos += " VALUES (" + pk_facet + ", " + pk_list_facets + ", " + i + ")";
                this.dataBase.Insert(insertFacet_X_pos);
            }

            return pk_list_facets;
        }// end Insert_ListFacets


        /* Descripción:
         *  Operación auxiliar para la inserción de lista de facetas
         */
        private int AuxInsert_ListFacets()
        {
            string insertLf = "Insert into TbListFacets(ghost) VALUES (0)";
            this.dataBase.Insert(insertLf);
            string select = "SELECT MAX(TbListFacets.pk_list_facets) FROM TbListFacets";
            DataTable dt = this.dataBase.Select2DataTable(select);
            int pk_list_facets = (int)dt.Rows[0][0];
            return pk_list_facets;
        }



        /* Descripción:
         *  Inserta la faceta en la tabla TbFacets y la los niveles omitidos en TbSkipLevelFacet.
         *  Devuelve la clave primaria de esa faceta.
         */
        public int Insert_Facet(Facet f)
        {
            string size = f.SizeOfUniverse().ToString();
            if(f.SizeOfUniverse().Equals(int.MaxValue))
            {
                size = Facet.INFINITE;
            }

            string insertFacet = "Insert into TbFacets (name_facet, level_facet, size_of_universe, comment, omit, list_facet_design)";
            // insertFacet += "VALUES('" + f.Name() + "', " + f.Level() + " , '" + size + "', '" + f.Comment() + "', '" + f.Omit() + "', '" + f.ListFacetDesing() + "')"; 
            insertFacet += " VALUES('" + f.Name() + "', " + f.Level() + " , '" + size + "', '" + f.Comment() + "', " + f.Omit() + ", '" + f.ListFacetDesing() + "')"; 
            this.dataBase.Insert(insertFacet);
            string selectFacet = "SELECT MAX(TbFacets.pk_facet) FROM TbFacets";
            DataTable dt = this.dataBase.Select2DataTable(selectFacet);
            int pk_facet = (int)dt.Rows[0][0];
            
            List<int> lSkipLevels = f.ListSkipLevels();
            int numSkipLevel = lSkipLevels.Count;
            for (int i = 0; i < numSkipLevel; i++)
            {
                int l = lSkipLevels[i];
                string insertSkipLevel = "Insert into TbSkipLevels(skip_level, fk_facet) VALUES(" + l + " , " + pk_facet + ")";
                this.dataBase.Insert(insertSkipLevel);
            }
            
            return pk_facet;
        }// end Insert_Facet

        #endregion Operaciones con ficheros SAGT



        #region Operaciones de inserción de Tabla de Medias
        /**************************************************************************************************
         * ================================================================================================
         * Operaciones de inserción de Tabla de Medias
         * 
         * Insertar Lista de medias (Insert_ListMeans)
         * Insertar Tabla de medias por defecto (Insert_TableMeans)
         * Insertar Tabla de medias de diferencias (Insert_TableMeansDif)
         * Insertar Tabla de medias de puntuaciones (Insert_TableMeansTypScore)
         * ================================================================================================
         **************************************************************************************************/

        /* Descripción:
         *  Insertar una tabla de medias en memoria.
         */
        public int Insert_ListMeans(ListMeans listMeans)
        {
            DateTime date = listMeans.GetDateTime();
            string nameFileCreation = listMeans.GetNameFileDataCreation();
            int pos = nameFileCreation.LastIndexOf("\\") + 1; // le sumamos uno para obtener la posición 
            // siguiente a la de la barra invertida
            nameFileCreation = nameFileCreation.Substring(pos);
            string comment = listMeans.GetTextComment();

            string insert_list_means = "INSERT into TbListMeans(date_creation, name_file_data_creation, text_comment)";
            insert_list_means += " VALUES ('"+ date.ToString() + "', '" + nameFileCreation + "','" + comment + "')";

            this.dataBase.Insert(insert_list_means);

            string selectMax = "SELECT MAX(pk_list_means) FROM TbListMeans";
            DataTable dt = this.dataBase.Select2DataTable(selectMax);
            int pk_list_means = (int)dt.Rows[0][0];

            int numTables = listMeans.Count();

            for (int i = 0; i < numTables; i++)
            {
                InterfaceTableMeans tbMeans = listMeans.TableMeansInPos(i);
                string typeTbMeans;

                // para poder comparar el tipo
                TableMeans tbM = new TableMeans();
                TableMeansDif tbDiff = new TableMeansDif();
                TableMeansTypScore tbPointScore = new TableMeansTypScore();

                string insert_table_means = "INSERT into TbMeans (fk_list_means, fk_list_facets, grand_mean, variance, std_dev, facet_Design, type_means, position_means) ";

                if (tbMeans.GetType() == tbM.GetType())
                {
                    // Es una tabla de medias por defecto
                    typeTbMeans = TypeOfTableMeans.Default.ToString();
                    tbM = (TableMeans)tbMeans;
                    ListFacets lf = tbMeans.ListFacets();
                    int fk_list_facets = Insert_ListFacets(lf);
                    insert_table_means += " VALUES (" + pk_list_means + ", " + fk_list_facets + ", "
                        + ConvertNum.DecimalToString((double)tbM.GrandMean(), ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                        + ", " + ConvertNum.DecimalToString((double)tbM.Variance(), ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                        + ", " + ConvertNum.DecimalToString((double)tbM.StdDev(), ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                        + ", '" + tbM.FacetDesign()+ "', '" + typeTbMeans + "', " + i + ")";
                    int pk_means = AuxInsertListMeans(insert_table_means);
                    // Hacemos el Mapeo de la tabla
                    Insert_TableMeans(tbM, pk_means);
                }
                else if (tbMeans.GetType() == tbDiff.GetType())
                {
                    // Es una tabla de medias de diferencias
                    typeTbMeans = TypeOfTableMeans.TableMeansDif.ToString();
                    tbDiff = (TableMeansDif)tbMeans;
                    ListFacets lf = tbDiff.ListFacets();
                    int fk_list_facets = Insert_ListFacets(lf);
                    insert_table_means += " VALUES (" + pk_list_means + ", " + fk_list_facets + ", "
                        + ConvertNum.DecimalToString((double)tbDiff.GrandMean(), ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                        + ", " + ConvertNum.DecimalToString((double)tbDiff.Variance(), ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                        + ", " + ConvertNum.DecimalToString((double)tbDiff.StdDev(), ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                        + ", '" + tbDiff.FacetDesign() + "', '" + typeTbMeans + "', " + i + ")";
                    int pk_means = AuxInsertListMeans(insert_table_means);
                    // Hacemos el Mapeo de la tabla
                    Insert_TableMeansDif(tbDiff, pk_means);
                }
                else if (tbMeans.GetType() == tbPointScore.GetType())
                {
                    // Es una tabla de medias de puntuaciones
                    typeTbMeans = TypeOfTableMeans.TableMeansTipPoint.ToString();
                    tbPointScore = (TableMeansTypScore)tbMeans;
                    ListFacets lf = tbPointScore.ListFacets();
                    int fk_list_facets = Insert_ListFacets(lf);
                    insert_table_means += " VALUES (" + pk_list_means + ", " + fk_list_facets + ", "
                        + ConvertNum.DecimalToString((double)tbPointScore.GrandMean(), ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                        + ", " + ConvertNum.DecimalToString((double)tbPointScore.Variance(), ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                        + ", " + ConvertNum.DecimalToString((double)tbPointScore.StdDev(), ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                        + ", '" + tbPointScore.FacetDesign() + "', '" + typeTbMeans + "', " + i + ")";
                    int pk_means = AuxInsertListMeans(insert_table_means);
                    // Hacemos el Mapeo de la tabla
                    Insert_TableMeansTypScore(tbPointScore, pk_means);
                }
                else
                {
                    // Se lanza un mensaje de error por no ser un tipo reconocido
                    throw new ConnectDB_Exception("Error: tabla de medias no reconocida");
                }    
            }

            return pk_list_means;
        }// end Insert_ListMeans


        /* Descripción:
         *  Operación auxiliar para la inserción de tabla de medias
         */
        private int AuxInsertListMeans(string insert_table_means)
        {
            this.dataBase.Insert(insert_table_means);
            string selectMax = "SELECT MAX(pk_means) FROM TbMeans";
            DataTable dt = this.dataBase.Select2DataTable(selectMax);
            return (int)dt.Rows[0][0];
        }


        /* Descripción:
         *  Realiza un mapeo de memoria de una tabla de medias
         */
        public void Insert_TableMeans(TableMeans tbMeans, int fk_means)
        {
            int nRows = tbMeans.MeansTableRows();
            int nCols = tbMeans.MeansTableColumns();

            ListFacets lf = tbMeans.ListFacets();

            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    string nameColumn;

                    if (j == nCols - 1)
                    {
                        // Desviación típica
                        nameColumn = COLUMN_STAND_DEV; // Columna de desviación típica
                    }
                    else if (j == nCols - 2)
                    {
                        // Varianza
                        nameColumn = COLMN_VARIANCE; // Columna de varianza
                    }
                    else if (j == nCols - 3)
                    {
                        // Media
                        nameColumn = COLMN_MEANS; // Columna de medias
                    }
                    else
                    {
                        nameColumn = lf.FacetInPos(j).Name(); 
                    }

                    Insert_TbMapMeans(fk_means, i, nameColumn, tbMeans.Data(i, j));
                    
                }
            }
        }// end Insert_TableMeans


        /* Descripción:
         *  Realiza un mapeo de memoria de una tabla de medias de diferencias
         */
        public void Insert_TableMeansDif(TableMeansDif tbMeansDif, int fk_means)
        {
            int nRows = tbMeansDif.MeansTableRows();
            int nCols = tbMeansDif.MeansTableColumns();

            ListFacets lf = tbMeansDif.ListFacets();

            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    string nameColumn;

                    if(j==nCols - 1)
                    {
                        // Diferencia de desviación típica
                        nameColumn = COLUMN_STAND_DEV_DIF;
                    }
                    else if(j==nCols - 2)
                    {
                        // Diferencia de varianzas
                        nameColumn = COLUMN_VARIANCE_DIF;
                    }
                    else if(j== nCols - 3)
                    {
                        // Diferencia de Medias
                        nameColumn = COLUMN_MEANS_DIF;
                    }
                    else if(j== nCols - 4)
                    {
                        // Desviación típica
                        nameColumn = COLUMN_STAND_DEV; // Desviación típica
                    }
                    else if(j==nCols - 5)
                    {
                        // Varianza
                        nameColumn = COLMN_VARIANCE; // Columna de varianza
                    }
                    else if (j == nCols - 6)
                    {
                        // Media
                        nameColumn = COLMN_MEANS; // Columna de medias
                    }
                    else
                    {
                         nameColumn = lf.FacetInPos(j).Name();
                    }

                    Insert_TbMapMeans(fk_means, i, nameColumn, tbMeansDif.Data(i, j));
                }
            }
        }// end Insert_TableMeansDif



        /* Descripción:
         *  Realiza un mapeo de memoria de una tabla de medias de diferencias
         */
        public void Insert_TableMeansTypScore(TableMeansTypScore tbMeansTtpScore, int fk_means)
        {
            int nRows = tbMeansTtpScore.MeansTableRows();
            int nCols = tbMeansTtpScore.MeansTableColumns();

            ListFacets lf = tbMeansTtpScore.ListFacets();

            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    string nameColumn;

                    if (j == nCols - 1)
                    {
                        // Puntuación típica
                        nameColumn = COLUMN_TYPSCORE;
                    }
                    else if (j == nCols - 2)
                    {
                        // Diferencia de Medias
                        nameColumn = COLUMN_MEANS_DIF;
                    }
                    else if (j == nCols - 3)
                    {
                        // Desviación típica
                        nameColumn = COLUMN_STAND_DEV; // Desviación típica
                    }
                    else if (j == nCols - 4)
                    {   
                        // Varianza
                        nameColumn = COLMN_VARIANCE; // Columna de varianza
                    }
                    else if (j == nCols - 5)
                    {
                        // Media
                        nameColumn = COLMN_MEANS; // columna de medias
                    }
                    else
                    {
                        nameColumn = lf.FacetInPos(j).Name();
                    }

                    Insert_TbMapMeans(fk_means, i, nameColumn, tbMeansTtpScore.Data(i, j));
                }
            }
        }// end Insert_TableMeansTypScore


        private void Insert_TbMapMeans(int fk_means, int i, string nameColumn, double? data)
        {
            string insertData = "Insert into TbMapMeans (fk_means, row, name_column, data_cell) ";
            insertData += " VALUES (" + fk_means + ", " + i + ", '" + nameColumn + "', " + ConvertNum.DecimalToString(data, ConvertNum.DECIMAL_SEPARATOR_PERIOD) + ")";
            this.dataBase.Insert(insertData);
        }

        #endregion Operaciones de inserción de Tabla de Medias



        #region Operaciones de insercion de Tablas de Análisis
        /*************************************************************************************************
         *================================================================================================
         * Operaciones de insercion de Tablas de Análisis:
         * 
         *      - Insertar tabla de análisis (Insert_Analysis)
         *      - Operación auxiliar para insertar la faceta en su posición (Aux_Insert_Facet_X_Pos)
         *      - Inserta los datos de la tabla de análisis de varianza (Insert_TbAnalysis)
         *      - Inserta los datos de la tabla G_Study (Insert_G_Study)
         *      - Insertar la faceta en la tabla de facetas (Inserta las facetas)
         *      - Inserta los datos de la lista de G_Parámetros (Insert_G_ParametersList)
         *================================================================================================
         *************************************************************************************************/

        /* Descripción:
         *  Inserta en la base de datos los datos del análisis de generalizabilidad.
         */
        public int Insert_Analysis(Analysis_and_G_Study analysis)
        {
            // Obtenemos la lista de facetas
            ListFacets lf = analysis.GetListFacets();
            int pk_list_facets = AuxInsert_ListFacets();

            // Obtenemos lista las facetas de instrumentación
            ListFacets lf_inst = analysis.List_Facets_Intrumentation();
            int pk_list_facets_inst = AuxInsert_ListFacets();

            // Obtenemos lista las facetas de difereciación
            ListFacets lf_diff = analysis.List_Facets_Differentiation();
            int pk_list_facets_diff = AuxInsert_ListFacets();

            int numFacet = lf.Count();

            for (int i = 0; i < numFacet; i++)
            {
                Facet f = lf.FacetInPos(i);
                // Insertamos la tabla de facetas
                int pk_facet = Insert_Facet(f);
                /* Insertamos en la tabla de posiciones de facetas
                 */
                string insertFacet_X_pos = "INSERT into TbPosFacets(fk_facets, fk_list_facets, position_facets) ";
                insertFacet_X_pos += " VALUES (" + pk_facet + ", " + pk_list_facets + ", " + i + ")";
                this.dataBase.Insert(insertFacet_X_pos);

                // Si es una faceta de instrumentación insertamos la posición en la lista de instrumentación
                Aux_Insert_Facet_X_Pos(f, lf_inst, pk_facet, pk_list_facets_inst);

                // Si es una faceta de diferenciación insertamos la posición en la lista de diferenciación
                Aux_Insert_Facet_X_Pos(f, lf_diff, pk_facet, pk_list_facets_diff);
            }// end 

            DateTime dateCreation = analysis.GetDateTime();
            string nameFileCreation = analysis.GetNameFileDataCreation();
            string textComment = analysis.GetTextComment();

            string insert_analysis_g_study = "INSERT into TbAnalysisAndG_Study(date_creation, name_file_creation, text_comment, fk_list_facets, fk_list_facets_diff, fk_list_facets_inst)";
            insert_analysis_g_study += " VALUES ('" + dateCreation.ToString() + "', '" + nameFileCreation + "','" + textComment + "'," + pk_list_facets + ", " + pk_list_facets_diff + ", " + pk_list_facets_inst + ")";
            this.dataBase.Insert(insert_analysis_g_study);

            // recuperamos la clave primaria de la fila insertada.
            string select_analysis_g_study = "  SELECT  pk_analysis_and_g_study   FROM    TbAnalysisAndG_Study "
                + " WHERE(TbAnalysisAndG_Study.name_file_creation = '" + nameFileCreation
                + "' AND TbAnalysisAndG_Study.date_creation like '" + dateCreation.ToString()
                + "' AND TbAnalysisAndG_Study.text_comment = '" + textComment
                + "' AND TbAnalysisAndG_Study.fk_list_facets = " + pk_list_facets
                + " AND TbAnalysisAndG_Study.fk_list_facets_diff = " + pk_list_facets_diff
                + " AND TbAnalysisAndG_Study.fk_list_facets_inst = " + pk_list_facets_inst + ")";

            DataTable dt = this.dataBase.Select2DataTable(select_analysis_g_study);
            int pk_analysis_and_g_study = (int)dt.Rows[0][0];

            // Insertamos la tabla de análisis
            Insert_TbAnalysis(analysis.TableAnalysisVariance(), pk_analysis_and_g_study);

            // Insertamos la tabla G_study
            Insert_G_Study(analysis.TableG_Study_Percent(), analysis.TableAnalysisVariance().SourcesOfVar(), pk_analysis_and_g_study);

            // Insertamos la lista de G_Parámetros
            Insert_G_ParametersList(analysis.ListG_P_Optimization(), pk_analysis_and_g_study);

            return pk_analysis_and_g_study;

        }// end Insert_Analysis


        /* Descripción:
         *  Operación para realizar la inserción de la posción de una faceta en la lista.
         */ 
        public void Aux_Insert_Facet_X_Pos(Facet f, ListFacets lf, int pk_facet, int pk_list_facets)
        {
            // encontramos la posición de la faceta si es que esta, si no esta no la insertamos
            Facet found_f = lf.LookingFacet(f.Name());

            if (found_f != null)
            {
                int pos = lf.IndexOf(found_f);
                /* Insertamos en la tabla de posiciones de facetas */
                string insertFacet_X_pos = "INSERT into TbPosFacets(fk_facets, fk_list_facets, position_facets) ";
                insertFacet_X_pos += " VALUES (" + pk_facet + ", " + pk_list_facets + ", " + pos + ")";
                this.dataBase.Insert(insertFacet_X_pos);
            }
        }


        /* Descripción:
         *  Inserta en la base de datos una tabla de análisis de varianza
         */
        private void Insert_TbAnalysis(TableAnalysisOfVariance analysisOfVar, int fk_table_analysis_and_g_study)
        {
            List<string> ldesign = analysisOfVar.SourcesOfVar();
            int numOfSources = ldesign.Count;

            for (int i = 0; i < numOfSources; i++)
            {
                string source_of_var = ldesign[i];
                double df = analysisOfVar.DegreesOfFreedom(source_of_var);
                double ssq = (double)analysisOfVar.SSQ(source_of_var);
                double msq = (double)analysisOfVar.MSQ(source_of_var);
                double random_comp = (double)analysisOfVar.RandomComp(source_of_var);
                double mix_comp = (double)analysisOfVar.MixModComp(source_of_var);
                double correc_comp = (double)analysisOfVar.CorretedComp(source_of_var);
                double porcentage = (double)analysisOfVar.Porcentage(source_of_var);
                double standard_error = (double)analysisOfVar.StandardError(source_of_var);

                // Insertamos en la tabla TbAnalysisOfVariance
                string insert_analysis_of_var = "INSERT into TbAnalysisOfVariance(source_of_var, df, ssq, msq, random_comp, mix_comp, correc_comp, porcentage, standard_error, fk_table_analysis_and_g_study) ";
                insert_analysis_of_var += " VALUES ('" + source_of_var + "', " 
                    + ConvertNum.DecimalToString(df, ConvertNum.DECIMAL_SEPARATOR_PERIOD) 
                    + ", " + ConvertNum.DecimalToString(ssq, ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                    + ", " + ConvertNum.DecimalToString(msq, ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                    + ", " + ConvertNum.DecimalToString(random_comp, ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                    + ", " + ConvertNum.DecimalToString(mix_comp, ConvertNum.DECIMAL_SEPARATOR_PERIOD) 
                    + ", " + ConvertNum.DecimalToString(correc_comp, ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                    + ", " + ConvertNum.DecimalToString(porcentage, ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                    + ", " + ConvertNum.DecimalToString(standard_error, ConvertNum.DECIMAL_SEPARATOR_PERIOD) 
                    + ", " + fk_table_analysis_and_g_study + ")";
                this.dataBase.Insert(insert_analysis_of_var);
            }

        }// end Insert_TbAnalysis


        /* Descripción:
         *  Almacena los datos de la tabla G_study en la base de datos
         */
        private void Insert_G_Study(TableG_Study_Percent gStudy, List<string> ldesign, int fk_table_analysis_and_g_study)
        {
            int numOfSources = ldesign.Count;

            for (int i = 0; i < numOfSources; i++)
            {
                string sourceOfVar = ldesign[i];
                
                Dictionary<string, double?> differentiationVar = gStudy.Target();
                Dictionary<string, ErrorVar> instrumentationVar = gStudy.Error();
                Dictionary<string, ErrorVar> percent = gStudy.Percent();
 
                if (differentiationVar.ContainsKey(sourceOfVar))
                {
                    // Si pertenece a las facetas de diferenciación
                    double? data = differentiationVar[sourceOfVar];
                    string insert_differentiation_var = "INSERT into TbDifferentiationVar(source_of_var, ";
                    if(data!=null)
                    {
                        insert_differentiation_var +=  " differentiation_var, ";
                    }
                    insert_differentiation_var +=  " fk_table_analysis_and_g_study) ";
                    insert_differentiation_var += " VALUES ('" + sourceOfVar + "', ";
                    if (data != null)
                    {
                        insert_differentiation_var += ConvertNum.DecimalToString(data, ConvertNum.DECIMAL_SEPARATOR_PERIOD) + ", ";
                    }
                    insert_differentiation_var +=  fk_table_analysis_and_g_study + ")";
                    this.dataBase.Insert(insert_differentiation_var);
                }
                else if (instrumentationVar.ContainsKey(sourceOfVar))
                {
                    // Si pertenece a las facetas de instrumentación
                    ErrorVar error_var = instrumentationVar[sourceOfVar];
                    double? rel_error_var = error_var.RelErrorVar();
                    double? abs_error_var = error_var.AbsErrorVar();
                    ErrorVar error_var_percent = percent[sourceOfVar];
                    double? rel_error_percent = error_var_percent.RelErrorVar();
                    double? abs_error_percent = error_var_percent.AbsErrorVar();

                    string insert_differentiation_var = "INSERT into TbInstrumentationVar(source_of_var,"; 

                    if (rel_error_var != null)
                    {
                        insert_differentiation_var += " rel_error_var,"; 
                    }

                    if (rel_error_percent != null)
                    {
                        insert_differentiation_var += " rel_error_percent,"; 
                    }

                    if (abs_error_var  != null)
                    {
                        insert_differentiation_var += " abs_error_var,"; 
                    }

                    if (abs_error_percent   != null)
                    {
                        insert_differentiation_var += " abs_error_percent,"; 
                    }
                     
                    insert_differentiation_var += " fk_table_analysis_and_g_study) ";

                    insert_differentiation_var += "VALUES ('" + sourceOfVar + "', ";

                    if (rel_error_var != null)
                    {
                        insert_differentiation_var += ConvertNum.DecimalToString(rel_error_var, ConvertNum.DECIMAL_SEPARATOR_PERIOD) + ", ";
                    }

                    if (rel_error_percent != null)
                    {
                        insert_differentiation_var += ConvertNum.DecimalToString(rel_error_percent, ConvertNum.DECIMAL_SEPARATOR_PERIOD) + ", ";
                    }

                    if (abs_error_var != null)
                    {
                        insert_differentiation_var += ConvertNum.DecimalToString(abs_error_var, ConvertNum.DECIMAL_SEPARATOR_PERIOD) + ", ";
                    }

                    if (abs_error_percent != null)
                    {
                        insert_differentiation_var += ConvertNum.DecimalToString(abs_error_percent, ConvertNum.DECIMAL_SEPARATOR_PERIOD) + ", ";
                    }
 
                    insert_differentiation_var +=  fk_table_analysis_and_g_study + ")";

                    this.dataBase.Insert(insert_differentiation_var);
                }
            }
        }// end Insert_G_Study


        /* Descripción:
         *  Inserta los datos de la lista de G-Parámetros.
         */
        public void Insert_G_ParametersList(List<G_ParametersOptimization> listG_P_Optimization, int fk_table_analysis_and_g_study)
        {
            int numGp = listG_P_Optimization.Count;
            for (int i = 0; i < numGp; i++)
            {
                G_ParametersOptimization gp = listG_P_Optimization[i];
                Aux_Insert_G_Parameters(gp, fk_table_analysis_and_g_study);
            }
        }


        /* Descripción:
         *  Operación auxiliar que inserta un G-Parámetros en la tabla.
         */
        private void Aux_Insert_G_Parameters(G_ParametersOptimization gp, int fk_table_analysis_and_g_study)
        {
            // Insertamos la lista de facetas
            ListFacets lf = gp.G_ListFacets();
            int fk_list_facets = Insert_ListFacets(lf);

            double total_differentiation_var = gp.Total_differentiation_var();
            double coef_g_rel = gp.CoefG_Rel();
            double coef_g_abs = gp.CoefG_Abs();
            double total_abs_error_var = gp.TotalAbsErrorVar();
            double total_rel_error_var = gp.TotalRelErrorVar();
            double error_rel_stand_dev = gp.ErrorAbsStandDev();
            double error_abs_stand_dev = gp.ErrorRelStandDev();
            double target_stand_dev = gp.TargetStandDev();

            // Insertamos en la tabla TbAnalysisOfVariance
            string insert_analysis_of_var = "INSERT into TbG_ParametersOptimization(total_differentiation_var, coef_g_rel, coef_g_abs, total_abs_error_var, total_rel_error_var, error_rel_stand_dev, error_abs_stand_dev, target_stand_dev, fk_list_facets, fk_table_analysis_and_g_study) ";
            insert_analysis_of_var += "VALUES (" + ConvertNum.DecimalToString(total_differentiation_var, ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                + ", " + ConvertNum.DecimalToString(coef_g_rel, ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                + ", " + ConvertNum.DecimalToString(coef_g_abs, ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                + ", " + ConvertNum.DecimalToString(total_abs_error_var, ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                + ", " + ConvertNum.DecimalToString(total_rel_error_var, ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                + ", " + ConvertNum.DecimalToString(error_rel_stand_dev, ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                + ", " + ConvertNum.DecimalToString(error_abs_stand_dev, ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                + ", " + ConvertNum.DecimalToString(target_stand_dev, ConvertNum.DECIMAL_SEPARATOR_PERIOD)
                + ", " + fk_list_facets + ", " + fk_table_analysis_and_g_study + ")";

            this.dataBase.Insert(insert_analysis_of_var);

        }// Aux_Insert_G_Parámeters

        #endregion Operaciones de inserción de Tablas de Análisis



        #region Operación para recuperar un Objeto Sagt
        /* Descripción:
         *  Devuelve un SagtFile que se recupera de la base de datos a partir de la clave primaria que
         *  se pasa como parámetro.
         */
        public SagtFile ReturnSagtFile(int pk_file)
        {
            string cadenaSelect = "  SELECT  TbFiles.name_file, TbFiles.date_file, TbFiles.type_file,"+
                " TbFiles.fk_multi_facet_obs, TbFiles.fk_list_means, TbFiles.fk_analysis_and_g_study FROM    TbFiles "
                + "WHERE(TbFiles.pk_file = " + pk_file + ")";


            DataTable dt = this.dataBase.Select2DataTable(cadenaSelect);

            string name_file = dt.Rows[0]["name_file"].ToString();


            DateTime date_file = (DateTime)dt.Rows[0]["date_file"]; 
            //DateTime date_file = DateTime.ParseExact(dt.Rows[0]["date_file"].ToString(), "dd/MM/yyyy HH:mm:ss",
            //                       new CultureInfo("es-ES", false));

            int pk_multi_facet_obs = (int)dt.Rows[0]["fk_multi_facet_obs"];
            MultiFacetsObs mfo = null;
            if (pk_multi_facet_obs != 0)
            {
                // Leemos la tabla de frecuencias de la base de datos
                mfo = Return_MultiFacetObs(pk_multi_facet_obs);
            }

            int pk_list_means = (int)dt.Rows[0]["fk_list_means"];
            ListMeans listMeans = null;
            if (pk_list_means != 0)
            {
                // Leemos la tabla de medias
                listMeans = Return_ListMeans(pk_list_means);
            }

            int pk_analysis_and_g_study = (int)dt.Rows[0]["fk_analysis_and_g_study"];
            Analysis_and_G_Study analysis = null;
            if (pk_analysis_and_g_study != 0)
            {
                // Leemos la tabla de análisis
                analysis = Return_Analysis_and_G_Study(pk_analysis_and_g_study);
            }

            return new SagtFile(mfo, listMeans, analysis);
        }// end ReturnSagtFile



        /* Descripción:
         *  Recupera los datos de un fichero de análisis.
         */
        public Analysis_and_G_Study Return_AnalysisFile(int pk_file)
        {
            string cadenaSelect = "  SELECT  TbFiles.name_file, TbFiles.date_file, TbFiles.type_file,"+
                " TbFiles.fk_multi_facet_obs, TbFiles.fk_list_means, TbFiles.fk_analysis_and_g_study FROM    TbFiles "
                + "WHERE(TbFiles.pk_file = " + pk_file + ")";

            DataTable dt = this.dataBase.Select2DataTable(cadenaSelect);

            int pk_analysis_and_g_study = (int)dt.Rows[0]["fk_analysis_and_g_study"];
            Analysis_and_G_Study analysis = Return_Analysis_and_G_Study(pk_analysis_and_g_study);
            return analysis;
        }


        /* Descripción:
         *  Devuelve La tabla de frecuencias que recupera a partir de la clave primaria.
         */
        private MultiFacetsObs Return_MultiFacetObs(int pk_multi_facet_obs)
        {
            string cadenaSelect = "  SELECT  TbMultiFacetObs.name_file, TbMultiFacetObs.description, TbMultiFacetObs.comment, TbMultiFacetObs.fk_list_facets FROM    TbMultiFacetObs "
               + "WHERE(TbMultiFacetObs.pk_multi_facet_obs = " + pk_multi_facet_obs + ")";

            DataTable dt = this.dataBase.Select2DataTable(cadenaSelect);
            
            string name_file = dt.Rows[0]["name_file"].ToString();
            string description = dt.Rows[0]["description"].ToString();
            string comment = dt.Rows[0]["comment"].ToString();
            int pk_list_facets = (int)dt.Rows[0]["fk_list_facets"];

            ListFacets lf = Return_ListFacets(pk_list_facets);

            InterfaceObsTable oTable = Return_ObsTable(lf, pk_multi_facet_obs);
            
            return new MultiFacetsObs(lf, oTable, name_file, description, comment); ;
        }// end Return_MultiFacetObs


        /* Descripción:
         *  Devuelve una lista de facetas a partir de su clave primaria pasada como parámetro.
         */
        private ListFacets Return_ListFacets(int pk_list_facets)
        {
            string cadenaSelect = "  SELECT  TbPosFacets.fk_facets, TbPosFacets.position_facets "
               + " FROM    TbPosFacets "
               + "WHERE(TbPosFacets.fk_list_facets= " + pk_list_facets + ") ORDER BY TbPosFacets.position_facets ASC ";

            DataTable dt = this.dataBase.Select2DataTable(cadenaSelect);

            ListFacets lf = new ListFacets();
            int n = dt.Rows.Count;

            for (int i = 0; i < n; i++)
            {
                int pk_facet = (int)dt.Rows[i]["fk_facets"];
                Facet f = Return_Facet(pk_facet);
                lf.Add(f);
            }

            return lf;
        }// end Return_ListFacets


        /* Descripción:
         *  Devuelve una faceta a partir de la clave primaria que se pasa como parámetro.
         */
        private Facet Return_Facet(int pk_facet)
        {
            string cadenaSelect = "  SELECT  TbFacets.name_facet, TbFacets.level_facet, "
               + "TbFacets.size_of_universe, TbFacets.comment, TbFacets.omit, TbFacets.list_facet_design " 
               + " FROM    TbFacets "
               + "WHERE(TbFacets.pk_facet = " + pk_facet + ")";

            DataTable dt = this.dataBase.Select2DataTable(cadenaSelect);

            string name = dt.Rows[0]["name_facet"].ToString();
            int level = (int)dt.Rows[0]["level_facet"];
            
            string size = dt.Rows[0]["size_of_universe"].ToString();
            int size_of_universe = int.MaxValue;
            if (!size.Equals(Facet.INFINITE))
            {
                size_of_universe = int.Parse(size);
            }

            string comment = dt.Rows[0]["comment"].ToString();
            // leer un valor bool de la base de datos (Omit)

            bool omit = (bool)dt.Rows[0]["omit"];

            string list_facet_design = dt.Rows[0]["list_facet_design"].ToString();

            List<int> skipLevels = Return_ListSkipLevels(pk_facet);

            Facet f =  new Facet(name, level, comment, size_of_universe, list_facet_design, omit);
            
            int n = skipLevels.Count;
            
            for(int i = 0; i < n; i++)
            {
                int sl = skipLevels[i];
                f.SetSkipLevels(sl, true);
            }
            return f;
        }// end Return_Facet


        /* Descripción:
         *  Devuelve una lista de enteros (los enteros que omite una determinada faceta) a partir
         *  de la clave foranea de la faceta que se pasa como parametro.
         */
        public List<int> Return_ListSkipLevels(int fk_facet)
        {
            List<int> retVal = new List<int>();

            string cadenaSelect = "  SELECT  TbSkipLevels.skip_level "
               + " FROM    TbSkipLevels "
               + "WHERE(TbSkipLevels.fk_facet = " + fk_facet + ")";

            DataTable dt = this.dataBase.Select2DataTable(cadenaSelect);

            int n = dt.Rows.Count;
            for (int i = 0; i < n; i++)
            {
                int skipLevel = (int)dt.Rows[i]["skip_level"];
                retVal.Add(skipLevel);
            }

            retVal.Sort();

            return retVal;
        }// end Return_ListSkipLevels


        /* Descripción: 
         *  Devuelve un objeto que implemente la InterfaceObsTable (Tabla de frecuencias)
         */
        public InterfaceObsTable Return_ObsTable(ListFacets lf, int fk_multi_facet_obs)
        {
            InterfaceObsTable oTable = new ObsTable(lf);
            int r = oTable.ObsTableRows();
            string cadenaSelect = "  SELECT  TbDataObservation.row, TbDataObservation.name_column, TbDataObservation.data_cell "
                   + " FROM    TbDataObservation "
                   + " WHERE (TbDataObservation.fk_multi_facet_obs = " + fk_multi_facet_obs + ")";

            DataTable dt = this.dataBase.Select2DataTable(cadenaSelect);

            int nCol = lf.Count();

            for (int i = 0; i < r; i++ )
            {
                for (int j = 0; j < nCol; j++)
                {
                    string name_column = COLMN_VAR_OBS;
                    if (j < nCol - 1)
                    {
                        name_column = lf.FacetInPos(j).Name();
                    }
                    string seletDataTable = "row = " + i + " AND name_column = '" + name_column + "'";
                    DataRow[] result = dt.Select(seletDataTable);
                    double? d = null;
                    object o = result[0]["data_cell"]; // 2 se coresponde con la columna data_cell
                    string s = o.ToString();

                    if (!string.IsNullOrEmpty(s))
                    {
                        d = (double)o;
                    }

                    oTable.Data(d, i);
                }
                
            }
            return oTable;
        }// end Return_ObsTable


        /* Descripción:
         *  Devuelve una lista de tablas de medias
         */
        private ListMeans Return_ListMeans(int pk_list_means)
        {
            string cadenaSelect_listMeans = "  SELECT  TbListMeans.date_creation, TbListMeans.name_file_data_creation, TbListMeans.text_comment "
                   + " FROM    TbListMeans "
                   + " WHERE (TbListMeans.pk_list_means = " + pk_list_means + ")";

            DataTable dtListMeans = this.dataBase.Select2DataTable(cadenaSelect_listMeans);

            ListMeans listMeans = new ListMeans();
            string[] timeFormats = { "d/MM/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy H:mm:ss" };
            DateTime date_creation = DateTime.ParseExact(dtListMeans.Rows[0]["date_creation"].ToString(), timeFormats,
                                   new CultureInfo("es-ES", false), DateTimeStyles.AssumeLocal);
            listMeans.SetDateTime(date_creation);

            string name_file_data_creation = dtListMeans.Rows[0]["name_file_data_creation"].ToString();
            listMeans.SetNameFileDataCreation(name_file_data_creation);

            string text_comment = dtListMeans.Rows[0]["text_comment"].ToString();
            listMeans.SetTextComment(text_comment);


            // realizo un select para recuperar los datos TbMeans
            string cadenaSelect_Means = "  SELECT  TbMeans.[pk_means], TbMeans.[fk_list_means], TbMeans.[fk_list_facets], "
                   + " TbMeans.[grand_mean], TbMeans.[variance], TbMeans.[std_dev], TbMeans.[facet_design], "
                   + " TbMeans.[type_means] "
                   + " FROM    TbMeans "
                   + " WHERE (TbMeans.[fk_list_means] = " + pk_list_means + ") ORDER BY TbMeans.[position_means] ASC ";

            DataTable dtMeans = this.dataBase.Select2DataTable(cadenaSelect_Means);
            // recorro uno a uno los elementos y añado la tabla
            int numRows = dtMeans.Rows.Count;
            for (int i = 0; i < numRows; i++ )
            {
                DataRow dtRow = dtMeans.Rows[i];

                int pk_means = (int)dtRow["pk_means"];
                // int fk_list_means = (int)dtRow["fk_list_means"];
                int fk_list_facets = (int)dtRow["fk_list_facets"];
                ListFacets lf = Return_ListFacets(fk_list_facets); 
                double grand_mean = (double)dtRow["grand_mean"];
                double variance = (double)dtRow["variance"];
                double std_dev = (double)dtRow["std_dev"];
                string facet_design = dtRow["facet_design"].ToString();
                string type_table_means = dtRow["type_means"].ToString();

                if (type_table_means.Equals(TypeOfTableMeans.Default.ToString()))
                {
                    TableMeans tbMeans = Return_TableMeans(lf, facet_design, pk_means, grand_mean, variance, std_dev);
                    listMeans.Add(tbMeans);
                }
                else if (type_table_means.Equals(TypeOfTableMeans.TableMeansDif.ToString()))
                {
                    TableMeansDif tbMeans = Return_TableMeansDiff(lf, facet_design, pk_means, grand_mean, variance, std_dev);
                    listMeans.Add(tbMeans);
                }
                else if (type_table_means.Equals(TypeOfTableMeans.TableMeansTipPoint.ToString()))
                {
                    TableMeansTypScore tbMeans = Return_TableMeansTypScore(lf, facet_design, pk_means, grand_mean, variance, std_dev);
                    listMeans.Add(tbMeans);
                }
                else
                {
                    // Tipo de tabla de medias no reconocido lanzamos una excepción
                    throw new ConnectDB_Exception("Error al recuperar las tablas de medias");
                }
                
            }// end for

            return listMeans;
        }// end Return_ListMeans


        /* Descripción:
         *  Devuelve un objeto de tablas de medias por defecto a partir de la clave foranea que se pasa
         *  como parámetro.
         */
        public TableMeans Return_TableMeans(ListFacets lf, string facet_design, int fk_means,
            double grand_mean, double variance, double std_dev)
        {
            int numCols = lf.Count() + 3; // Número de columnas que tiene la tabla.

            string cadenaSelect = "  SELECT  TbMapMeans.row, TbMapMeans.name_column, TbMapMeans.data_cell "
                   + " FROM    TbMapMeans "
                   + " WHERE (TbMapMeans.fk_means = " + fk_means + ")";
            
            DataTable dt = this.dataBase.Select2DataTable(cadenaSelect);
            int numCells = dt.Rows.Count;
            // El número de fila será el número de celdas entre el número de columnas
            int numRows = numCells / numCols;
            TableMeans tableMeans = new TableMeans(lf, facet_design, numRows);

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    string name_column = COLUMN_STAND_DEV;
                    if (j == numCols - 1)
                    {
                        name_column = COLUMN_STAND_DEV;
                    }
                    else if (j == numCols - 2)
                    {
                        name_column = COLMN_VARIANCE;
                    }
                    else if (j == numCols - 3)
                    {
                        name_column = COLMN_MEANS;
                    }
                    else
                    {
                        name_column = lf.FacetInPos(j).Name();
                    }
                    string seletDataTable = "row = " + i + " AND name_column = '" + name_column + "'";
                    DataRow[] result = dt.Select(seletDataTable);
                    double? d = null;
                    object o = result[0][2]; // 2 se coresponde con la columna data_cell
                    string s = o.ToString();

                    if (!string.IsNullOrEmpty(s))
                    {
                        d = (double)o;
                    }

                    tableMeans.InsertDataInPos(d, i, j);
                }

            }

            tableMeans.GrandMean(grand_mean);// Asignamos la Gran Media
            tableMeans.Variance(variance);// Asignamos la varianza de la tabla
            tableMeans.StdDev(std_dev);// Asignamos la desviación tipica de la tabla

            return tableMeans;
        }// end Return_TableMeans


        /* Descripción:
        *  Devuelve un objeto de tablas de medias de las desviaciones a partir de la clave foranea que se pasa
        *  como parámetro.
        */
        public TableMeansDif Return_TableMeansDiff(ListFacets lf, string facet_design, int fk_means,
            double grand_mean, double variance, double std_dev)
        {
            int numCols = lf.Count() + 6; // Número de columnas que tiene la tabla.

            string cadenaSelect = "  SELECT  TbMapMeans.row, TbMapMeans.name_column, TbMapMeans.data_cell "
                   + " FROM    TbMapMeans "
                   + " WHERE (TbMapMeans.fk_means = " + fk_means + ")";

            DataTable dt = this.dataBase.Select2DataTable(cadenaSelect);
            int numCells = dt.Rows.Count;
            // El número de fila será el número de celdas entre el número de columnas
            int numRows = numCells / numCols;
            TableMeansDif tableMeansDif = new TableMeansDif(lf, facet_design, numRows);

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    string name_column = COLUMN_STAND_DEV_DIF;
                    if (j == numCols - 1)
                    {
                        name_column = COLUMN_STAND_DEV_DIF;
                    }
                    else if (j == numCols - 2)
                    {
                        name_column = COLUMN_VARIANCE_DIF;
                    }
                    else if (j == numCols - 3)
                    {
                        name_column = COLUMN_MEANS_DIF;
                    }
                    else if (j == numCols - 4)
                    {
                        name_column = COLUMN_STAND_DEV;
                    }
                    else if (j == numCols - 5)
                    {
                        name_column = COLMN_VARIANCE;
                    }
                    else if (j == numCols - 6)
                    {
                        name_column = COLMN_MEANS;
                    }
                    else
                    {
                        name_column = lf.FacetInPos(j).Name();
                    }
                    string seletDataTable = "row = " + i + " AND name_column = '" + name_column + "'";
                    DataRow[] result = dt.Select(seletDataTable);
                    double? d = null;
                    object o = result[0][2]; // 2 se coresponde con la columna data_cell
                    string s = o.ToString();

                    if (!string.IsNullOrEmpty(s))
                    {
                        d = (double)o;
                    }

                    tableMeansDif.InsertDataInPos(d, i, j);
                }

            }

            tableMeansDif.GrandMean(grand_mean);// Asignamos la Gran Media
            tableMeansDif.Variance(variance);// Asignamos la varianza de la tabla
            tableMeansDif.StdDev(std_dev);// Asignamos la desviación tipica de la tabla

            return tableMeansDif;
        }// end Return_TableMeansDiff



        /* Descripción:
        *  Devuelve un objeto de tablas de medias de puntuaciones típicas a partir de la clave foranea que se pasa
        *  como parámetro.
        */
        public TableMeansTypScore Return_TableMeansTypScore(ListFacets lf, string facet_design, int fk_means,
            double grand_mean, double variance, double std_dev)
        {
            int numCols = lf.Count() + 5; // Número de columnas que tiene la tabla.

            string cadenaSelect = "  SELECT  TbMapMeans.row, TbMapMeans.name_column, TbMapMeans.data_cell "
                   + " FROM    TbMapMeans "
                   + " WHERE (TbMapMeans.fk_means = " + fk_means + ")";

            DataTable dt = this.dataBase.Select2DataTable(cadenaSelect);
            int numCells = dt.Rows.Count;
            // El número de fila será el número de celdas entre el número de columnas
            int numRows = numCells / numCols;
            TableMeansTypScore tableMeansTypScore = new TableMeansTypScore(lf, facet_design, numRows);

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    string name_column = COLUMN_STAND_DEV_DIF;
                    if (j == numCols - 1)
                    {
                        name_column = COLUMN_TYPSCORE;
                    }
                    else if (j == numCols - 2)
                    {
                        name_column = COLUMN_MEANS_DIF;
                    }
                    else if (j == numCols - 3)
                    {
                        name_column = COLUMN_STAND_DEV;
                    }
                    else if (j == numCols - 4)
                    {
                        name_column = COLMN_VARIANCE;
                    }
                    else if (j == numCols - 5)
                    {
                        name_column = COLMN_MEANS;
                    }
                    else
                    {
                        name_column = lf.FacetInPos(j).Name();
                    }
                    string seletDataTable = "row = " + i + " AND name_column = '" + name_column + "'";
                    DataRow[] result = dt.Select(seletDataTable);
                    double? d = null;
                    object o = result[0][2]; // 2 se coresponde con la columna data_cell
                    string s = o.ToString();

                    if (!string.IsNullOrEmpty(s))
                    {
                        d = (double)o;
                    }

                    tableMeansTypScore.InsertDataInPos(d, i, j);
                }

            }

            tableMeansTypScore.GrandMean(grand_mean);// Asignamos la Gran Media
            tableMeansTypScore.Variance(variance);// Asignamos la varianza de la tabla
            tableMeansTypScore.StdDev(std_dev);// Asignamos la desviación tipica de la tabla

            return tableMeansTypScore;
        }// end Return_TableMeansTypScore


        /* Descripción:
         *  Recupera las tablas de análisis de generalizabilidad
         */
        private Analysis_and_G_Study Return_Analysis_and_G_Study(int pk_analysis_and_g_study)
        {
            string cadenaSelect = "  SELECT  TbAnalysisAndG_Study.date_creation, " 
                   + " TbAnalysisAndG_Study.name_file_creation, TbAnalysisAndG_Study.text_comment, "
                   + " TbAnalysisAndG_Study.fk_list_facets, TbAnalysisAndG_Study.fk_list_facets_diff, "
                   + " TbAnalysisAndG_Study.fk_list_facets_inst "
                   + " FROM    TbAnalysisAndG_Study "
                   + " WHERE (TbAnalysisAndG_Study.pk_analysis_and_g_study = " + pk_analysis_and_g_study + ")";

            DataTable dt = this.dataBase.Select2DataTable(cadenaSelect);

            string[] timeFormats = { "d/MM/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy H:mm:ss" };
            DateTime date_creation = DateTime.ParseExact(dt.Rows[0]["date_creation"].ToString(), timeFormats,
                                   new CultureInfo("es-ES", false), DateTimeStyles.AssumeLocal);

            string name_file_creation = dt.Rows[0]["name_file_creation"].ToString();
            string text_comment = dt.Rows[0]["text_comment"].ToString();

            int fk_list_facets = (int)dt.Rows[0]["fk_list_facets"];
            ListFacets lf = Return_ListFacets(fk_list_facets);
            
            int fk_list_facets_diff = (int)dt.Rows[0]["fk_list_facets_diff"];
            ListFacets lf_diff = Return_ListFacets(fk_list_facets_diff);

            int fk_list_facets_inst = (int)dt.Rows[0]["fk_list_facets_inst"];
            ListFacets lf_inst = Return_ListFacets(fk_list_facets_inst);

            // Tabla de análisis de varianza
            TableAnalysisOfVariance analysis = Return_TableAnalysisOfVar(pk_analysis_and_g_study, lf);

            // Tabla de G_Study
            TableG_Study_Percent table_G_Study = Return_Table_G_Study(pk_analysis_and_g_study, lf, lf_diff, lf_inst);

            // Lista de G_parámetros de optimización
            List<G_ParametersOptimization> list_G_P = Return_ListG_Parameters(pk_analysis_and_g_study);

            Analysis_and_G_Study retVal = new Analysis_and_G_Study(analysis, table_G_Study, list_G_P);

            retVal.SetNameFileDataCreation(name_file_creation);
            retVal.SetDateTime(date_creation);
            retVal.SetTextComment(text_comment);

            return retVal;
        }// end Return_Analysis_and_G_Study


        /* Descripción:
         *  Devuelve un objeto TablaAnalysisOfVariance recuperado de la base de datos a traves de la
         *  clave foranea que se pasa como parámetro.
         */
        private TableAnalysisOfVariance Return_TableAnalysisOfVar(int fk_table_analysis_and_g_study,
            ListFacets lf)
        {
            string cadenaSelect = "  SELECT  TbAnalysisOfVariance.source_of_var, TbAnalysisOfVariance.df,"
                   + " TbAnalysisOfVariance.ssq, TbAnalysisOfVariance.msq, TbAnalysisOfVariance.random_comp,"
                   + " TbAnalysisOfVariance.mix_comp, TbAnalysisOfVariance.correc_comp, "
                   + " TbAnalysisOfVariance.porcentage, TbAnalysisOfVariance.standard_error"
                   + " FROM    TbAnalysisOfVariance "
                   + " WHERE (TbAnalysisOfVariance.fk_table_analysis_and_g_study = " + fk_table_analysis_and_g_study + ")";

            DataTable dt = this.dataBase.Select2DataTable(cadenaSelect);

            List<string> ldesign = new List<string>();
            Dictionary<string, double> df = new Dictionary<string,double>();
            Dictionary<string, double?> ssq = new Dictionary<string,double?>();
            Dictionary<string, double?> msq = new Dictionary<string, double?>();
            Dictionary<string, double?> randomComp = new Dictionary<string,double?>();
            Dictionary<string, double?> mixComp = new Dictionary<string,double?>();
            Dictionary<string, double?> correcComp = new Dictionary<string,double?>();
            Dictionary<string, double?> porcentage = new Dictionary<string,double?>();
            Dictionary<string, double?> standardError = new Dictionary<string,double?>();


            int numRows = dt.Rows.Count;

            for (int i = 0; i < numRows; i++)
            {
                string source_of_var = dt.Rows[i]["source_of_var"].ToString();
                ldesign.Add(source_of_var);
                double d_df = (double)dt.Rows[i]["df"];
                df.Add(source_of_var,d_df);
                double d_ssq = (double)dt.Rows[i]["ssq"];
                ssq.Add(source_of_var, d_ssq);
                double d_msq = (double)dt.Rows[i]["msq"];
                msq.Add(source_of_var, d_msq);
                double d_random_comp = (double)dt.Rows[i]["random_comp"];
                randomComp.Add(source_of_var, d_random_comp);
                double d_mix_comp = (double)dt.Rows[i]["mix_comp"];
                mixComp.Add(source_of_var, d_mix_comp);
                double d_correc_comp = (double)dt.Rows[i]["correc_comp"];
                correcComp.Add(source_of_var, d_correc_comp);
                double d_percent = (double)dt.Rows[i]["porcentage"];
                porcentage.Add(source_of_var, d_percent);
                double d_standard_error = (double)dt.Rows[i]["standard_error"];
                standardError.Add(source_of_var, d_standard_error);
            }

            return new TableAnalysisOfVariance(lf, ldesign, ssq, df, msq, randomComp, mixComp, correcComp,
                porcentage, standardError);
        }// end Return_TableAnalysisOfVar


        /* Descripción:
         *  Devuelve un objeto que contiene la tabla G_Study recuperada de la base de datos a traves
         *  de la clave foranea
         */
        private TableG_Study_Percent Return_Table_G_Study(int fk_table_analysis_and_g_study, ListFacets lf, 
            ListFacets lf_diff, ListFacets lf_inst)
        {
            // Hacemos la consulata en la tabla de varianzas de diferenciación
            string cadenaSelect = "  SELECT  TbDifferentiationVar.source_of_var, TbDifferentiationVar.differentiation_var"
                   + " FROM    TbDifferentiationVar "
                   + " WHERE (TbDifferentiationVar.fk_table_analysis_and_g_study = " + fk_table_analysis_and_g_study + ")";

            DataTable dt = this.dataBase.Select2DataTable(cadenaSelect);

            Dictionary<string, double?> differentiationVar = new Dictionary<string,double?>();

            double total_diff_var = 0;
            int nRows = dt.Rows.Count;

            for (int i = 0; i < nRows; i++)
            {
                string source_of_var = dt.Rows[i]["source_of_var"].ToString();
                double d_diff_var = (double)dt.Rows[i]["differentiation_var"];
                total_diff_var += d_diff_var;
                differentiationVar.Add(source_of_var, d_diff_var);
            }


            // Hacemos la consulta en la tabla de varianzas de instrumentación
            cadenaSelect = "  SELECT  TbInstrumentationVar.source_of_var, TbInstrumentationVar.rel_error_var, "
                   + " TbInstrumentationVar.rel_error_percent, TbInstrumentationVar.abs_error_var, "
                   + " TbInstrumentationVar.abs_error_percent "
                   + " FROM    TbInstrumentationVar "
                   + " WHERE (TbInstrumentationVar.fk_table_analysis_and_g_study = " + fk_table_analysis_and_g_study + ")";

            dt = this.dataBase.Select2DataTable(cadenaSelect);

            Dictionary<string, ErrorVar> errorVar = new Dictionary<string,ErrorVar>();
            Dictionary<string, ErrorVar> percentError = new Dictionary<string,ErrorVar>();

            double total_rel_error_var = 0;
            double total_abs_error_var = 0;
            nRows = dt.Rows.Count;

            for (int i = 0; i < nRows; i++)
            {
                string source_of_var = dt.Rows[i]["source_of_var"].ToString();
                double? rel_error_var = null;
                object o = dt.Rows[i]["rel_error_var"];
                string s = o.ToString();
                if (!string.IsNullOrEmpty(s))
                {
                    rel_error_var = (double)o;
                }

                if (rel_error_var != null)
                {
                    total_rel_error_var += (double)rel_error_var;
                }


                double? abs_error_var = null;
                o = dt.Rows[i]["abs_error_var"];
                s = o.ToString();
                if (!string.IsNullOrEmpty(s))
                {
                    abs_error_var = (double)o;
                }

                if (abs_error_var != null)
                {
                    total_abs_error_var += (double)abs_error_var;
                }

                ErrorVar rel_and_abs_errors = new ErrorVar(rel_error_var, abs_error_var);

                errorVar.Add(source_of_var, rel_and_abs_errors);

                double? rel_error_percent = null;
                o = dt.Rows[i]["rel_error_percent"];
                s = o.ToString();
                if (!string.IsNullOrEmpty(s))
                {
                    rel_error_percent = (double)o;
                }

                double? abs_error_percent = null;
                o = dt.Rows[i]["abs_error_percent"];
                s = o.ToString();
                if (!string.IsNullOrEmpty(s))
                {
                    abs_error_percent = (double)o;
                }

                ErrorVar rel_and_abs_percent = new ErrorVar(rel_error_percent, abs_error_percent);

                percentError.Add(source_of_var, rel_and_abs_percent);
            }

            G_ParametersOptimization gp = new G_ParametersOptimization(lf, total_diff_var, total_rel_error_var,
                total_abs_error_var);

            return new TableG_Study_Percent(lf_diff, lf_inst, differentiationVar,errorVar, gp, percentError);
        }// end Return_Table_G_Study


        /* Descripción:
         *  Devuelve la lista de G_parámetros de optimización a partir de la clave foranea que se
         *  pasa como parámetros.
         */
        private List<G_ParametersOptimization> Return_ListG_Parameters(int fk_table_analysis_and_g_study)
        {
            // variable de retorno
            List<G_ParametersOptimization> listG_P = new List<G_ParametersOptimization>();

            // Hacemos la consulta en la tabla de varianzas de instrumentación
            string cadenaSelect = "  SELECT  TbG_ParametersOptimization.total_differentiation_var, "
                   + " TbG_ParametersOptimization.coef_g_rel, TbG_ParametersOptimization.coef_g_abs, "
                   + " TbG_ParametersOptimization.total_abs_error_var, TbG_ParametersOptimization.total_rel_error_var, "
                   + " TbG_ParametersOptimization.error_rel_stand_dev, TbG_ParametersOptimization.error_abs_stand_dev, "
                   + " TbG_ParametersOptimization.target_stand_dev, TbG_ParametersOptimization.fk_list_facets"
                   + " FROM    TbG_ParametersOptimization "
                   + " WHERE (TbG_ParametersOptimization.fk_table_analysis_and_g_study = " + fk_table_analysis_and_g_study + ")";

            DataTable dt = this.dataBase.Select2DataTable(cadenaSelect);

            int nRows = dt.Rows.Count;

            for (int i = 0; i < nRows; i++)
            {
                int fk_list_facets = (int)dt.Rows[i]["fk_list_facets"];
                ListFacets lf = Return_ListFacets(fk_list_facets);

                double total_differentiation_var = (double)dt.Rows[i]["total_differentiation_var"];
                double coef_g_rel = (double)dt.Rows[i]["coef_g_rel"];
                double coef_g_abs = (double)dt.Rows[i]["coef_g_abs"];
                double total_abs_error_var = (double)dt.Rows[i]["total_abs_error_var"];
                double total_rel_error_var = (double)dt.Rows[i]["total_rel_error_var"];
                double error_rel_stand_dev = (double)dt.Rows[i]["error_rel_stand_dev"];
                double error_abs_stand_dev = (double)dt.Rows[i]["error_abs_stand_dev"];
                double target_stand_dev = (double)dt.Rows[i]["target_stand_dev"];

                G_ParametersOptimization gp = new G_ParametersOptimization(lf, total_differentiation_var,
                    coef_g_rel, coef_g_abs, total_rel_error_var, total_abs_error_var, error_rel_stand_dev,
                    error_abs_stand_dev, target_stand_dev);

                listG_P.Add(gp);
            }

            return listG_P;
        }// end Return_ListG_Parameters

        #endregion Operación para recuperar un Objeto Sagt


        /* Descripción:
         *  Devuelve el nombre de usuario de la tabla TbUser
         */
        public string ReturnNameMenPasUser(int idUser)
        {
            string consultaSQL = "  SELECT  TbUsers.name_users"
                   + " FROM    TbUsers "
                   + " WHERE (TbUsers.pk_users = " + idUser + ")";

            DataTable dt = this.dataBase.Select2DataTable(consultaSQL);
            return (string)dt.Rows[0]["name_users"];
        }


        /* Descripción:
         *  Devuelve el numero que identifica el proyecto de un usuario, 0 en el caso de que no tenga ninguno
         */
        public int ReturnMenPasuser2SagtUser(string nameIdeUser)
        {
            string consultaSQL = "  SELECT  TbUsers.pk_users"
                   + " FROM    TbUsers "
                   + " WHERE (TbUsers.name_users = '" + nameIdeUser + "')";

            DataTable dt = this.dataBase.Select2DataTable(consultaSQL);
            return (int)dt.Rows[0]["pk_users"];
        }


    }// end class ConnectDB
}// end namespace ConnectLibrary
