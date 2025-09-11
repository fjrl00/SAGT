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
 * Fecha de revisión: 11/Jun/2012                           
 * 
 * Descripción:
 *      Interface para establecer conexión con la base de datos y generar consultas, inserciones y borrados
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MultiFacetData;
using Sagt;
using ProjectMeans;
using ProjectSSQ;

namespace ConnectLibrary
{
    public interface InterfaceConnectDB
    {
        /*=================================================================================================
         * Métodos de Inserción 
         *=================================================================================================*/
        // Inserta el identificador de usuario
        int Add_SagtUser(string user);
        // Inserta un proyecto en la base de datos
        int Insert_Project(SagtProject project);
        // Inserta un objeto Sagt en la base de datos en el proyecto especificado.
        int Insert_SagtFile(SagtFile sagtFile, string nameFile, int fk_projects, int fk_user);
        // Inserta un objeto fichero de Análisis en la base de datos en un proyecto especificado
        int Insert_AnalysisFile(Analysis_and_G_Study analysisFile, string nameFile, int fk_projects, int fk_user);
        // Almacena los datos de la tabla de frecuencias en la base de datos. 
        int Insert_MultiFacetsObs(MultiFacetsObs mfo);
        // Inserta los datos de una lista de facetas en la base de datos y devuelve la clave primaria
        //  de esa lista.
        int Insert_ListFacets(ListFacets lf);
        // Inserta la faceta en la tabla TbFacets y la los niveles omitidos en TbSkipLevelFacet.
        //  Devuelve la clave primaria de esa faceta.
        int Insert_Facet(Facet f);
        // Insertar una tabla de medias en memoria.
        int Insert_ListMeans(ListMeans listMeans);
        // Inserta en la base de datos los datos del análisis de generalizabilidad.
        int Insert_Analysis(Analysis_and_G_Study analysis);
        // Realiza un mapeo de memoria de una tabla de medias
        void Insert_TableMeans(TableMeans tbMeans, int fk_means);
        // Realiza un mapeo de memoria de una tabla de medias de diferencias
        void Insert_TableMeansDif(TableMeansDif tbMeansDif, int fk_means);
        // Realiza un mapeo de memoria de una tabla de medias de diferencias
        void Insert_TableMeansTypScore(TableMeansTypScore tbMeansTypScore, int fk_means);
        // Operación para realizar la inserción de la posción de una faceta en la lista.
        void Aux_Insert_Facet_X_Pos(Facet f, ListFacets lf, int pk_facet, int pk_list_facets);
        // Inserta los datos de la lista de G-Parámetros.
        void Insert_G_ParametersList(List<G_ParametersOptimization> listG_P_Optimization, int fk_table_analysis_and_g_study);

        /*=================================================================================================
         * Métodos de Consulta
         *=================================================================================================*/
        //Devuelve el nombre de usuario que se usa como clave primaria en la base de datos de MenPas
        String ReturnNameMenPasUser(int idUser);
        // Devuelve un SagtFile que se recupera de la base de datos a partir de la clave primaria que
        //  se pasa como parámetro.
        SagtFile ReturnSagtFile(int pk_file);
        // Recupera los datos de un fichero de análisis.
        Analysis_and_G_Study Return_AnalysisFile(int pk_file);
        // Devuelve una lista de enteros (los enteros que omite una determinada faceta) a partir
        //  de la clave foranea de la faceta que se pasa como parametro.
        List<int> Return_ListSkipLevels(int fk_facet);
        // Devuelve un objeto que implemente la InterfaceObsTable (Tabla de frecuencias)
        InterfaceObsTable Return_ObsTable(ListFacets lf, int fk_multi_facet_obs);
        // Devuelve un objeto de tablas de medias por defecto a partir de la clave foranea que se pasa
        //  como parámetro.
        TableMeans Return_TableMeans(ListFacets lf, string facet_design, int fk_means,
            double grand_mean, double variance, double std_dev);
        // Devuelve un objeto de tablas de medias de las desviaciones a partir de la clave foranea que se pasa
        //  como parámetro.
        TableMeansDif Return_TableMeansDiff(ListFacets lf, string facet_design, int fk_means,
            double grand_mean, double variance, double std_dev);
        // Devuelve un objeto de tablas de medias de puntuaciones típicas a partir de la clave foranea que se pasa
        //  como parámetro.
        TableMeansTypScore Return_TableMeansTypScore(ListFacets lf, string facet_design, int fk_means,
            double grand_mean, double variance, double std_dev);
        // Devuelve el numero identificado de usuario de Sagt a traves del identificador de MenPas
        int ReturnMenPasuser2SagtUser(string nameIdeUser);


        /*=================================================================================================
         * Actualización de datos de proyecto
         *=================================================================================================*/
        void UpdateSagtProject(SagtProject project);

    }// end interface InterfaceConnectDB
}// namespace ConnectLibrary
