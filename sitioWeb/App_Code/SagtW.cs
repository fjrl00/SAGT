using System;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using MultiFacetData;
using Sagt;
using ProjectMeans;
using ProjectSSQ;
using ConnectLibrary;
using System.Collections.Generic;


/* La siguiente Clase se ha declarado como un registro de atributos públicos para permitir de 
 * una manera sencilla la serialización.
 */
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class SagtFileW
{
    // MultifacetData
    public DataSet dsMultiFacetObs;
    // Lista de tablas de medias
    public DataSet[] listDataSetMeans;
    // Analisis de varianza
    public DataSet[] ldsAnalysis;
}

/// <summary>
/// Descripción breve de Sagt
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class SagtW : System.Web.Services.WebService
{

    private string pathBD;
    // private ClassBD mi_clase_bd = new ClassBD();
    // private Class_Sucesos sucesos = new Class_Sucesos();
    private string pathBDsagt = "C:\\Inetpub\\wwwroot\\resadmin\\menpas\\menpas.com\\db\\bd_sagt.mdb;";
    // private ConnectLibrary.ConnectDB cn = new ConnectDB("C:\\Inetpub\\wwwroot\\resadmin\\menpas\\menpas.com\\db\\bd_sagt.mdb;");
    private ConnectLibrary.ConnectDB_WS cn = new ConnectDB_WS(@"e:\bd_sagt.mdb");



    
    public SagtW()
    {

        //Eliminar la marca de comentario de la línea siguiente si utiliza los componentes diseñados 
        //InitializeComponent(); 
    }


    [WebMethod]
    public int EstaRegistrado(String usuario, String contraseña)
    {
        // bool salida = false;
        // return mi_clase_bd.estaregistrado(usuario, contraseña);
        return AuxEstaRegistrado(usuario, contraseña);
    }

    /**************************************************************************************************
     * ================================================================================================
     * MÉTODO PARA PRUEBAS
     * ================================================================================================
     **************************************************************************************************/
    private int AuxEstaRegistrado(string user, string pass)
    {
        string[] arrayNames = { "Francis", "María", "Fernado" };
        string[] arrayPass = { "windows7", "macos", "ubuntu" };

        int retVal = 0;

        int l = arrayNames.Length;
        bool found = false;

        for (int i = 0; i < l && !found; i++)
        {
            if (found = arrayNames[i].ToLower().Equals(user.ToLower()) && arrayPass[i].ToLower().Equals(pass.ToLower()))
            {
                retVal = i + 1;
            }
        }
        return retVal;
    }


    //[WebMethod]
    //public string dame_perfil(String usuario)
    //{

    //    string perfil = "";


    //    if (mi_clase_bd.Existe_Usuario(usuario))
    //    {

    //        string Usuario = mi_clase_bd.ObtenerCelda(usuario, "Nombre_Usuario");// para may y min                    
    //        perfil = mi_clase_bd.ObtenerCelda(Usuario, "Perfil");

    //    }
    //    return perfil;


    //}// añadir datos modrian
    //[WebMethod]
    //public string dame_grupo(String usuario)
    //{
    //    string perfil = "";
    //    if (mi_clase_bd.Existe_Usuario(usuario))
    //    {
    //        //sucesos.insertar_suceso_RealizaAtencion(this.Context.Request.UserHostAddress, this.Context.Request.Browser.Platform, Context.Request.Browser.Type, "Atencion1.0", usuario, "Pide Grupo");
    //        string Usuario = mi_clase_bd.ObtenerCelda(usuario, "Nombre_Usuario");// para may y min                    
    //        perfil = mi_clase_bd.ObtenerCelda(Usuario, "Grupo");
    //    }
    //    return perfil;

    //}
    // añadir datos modrian
    //----------------------------------------------------------
    //[WebMethod]
    //public DataSet personasGrupo(string grupo)
    //{

    //    return mi_clase_bd.dame_UsuariosGrupoAlejandro(grupo);
    //    //sucesos.insertar_suceso_RealizaAtencion(this.Context.Request.UserHostAddress, this.Context.Request.Browser.Platform, Context.Request.Browser.Type, "Atencion1.0", "blanco", "tabla grupos");

    //}

    ////-------------------
    //[WebMethod]
    //public string dameDatos(string user)
    //{
    //    string mi_nombre = mi_clase_bd.ObtenerCelda(user, "Nombre");
    //    string mi_Apellido = mi_clase_bd.ObtenerCelda(user, "Apellidos");
    //    return (mi_nombre + " " + mi_Apellido);
    //}

    // Inserta un proyecto
    [WebMethod]
    public int Insert_Project(DataSet project)
    {
        return cn.Insert_Project(project);
    }


    // Devuelve un DataTable con los proyectos con el mismo nombre.
    [WebMethod]
    public DataSet SelectSameProyects(DataSet project)
    {
        return cn.SelectSameProyects(project);
    }


    // Devuelve un DataTable con los proyectos con el patrón de busqueda nombre y descripción.
    [WebMethod]
    public DataSet SelectLikeProyects(String name, string description)
    {
        return cn.SelectLikeProyects(name, description);
    }


    // Devuelve un DataTable con todos los proyectos a los que tiene acceso el usuario
    [WebMethod]
    public DataSet SelectProyectsForUsers()
    {
        return cn.SelectProyectsForUsers();
    }


    // Devuelve un dataTable con cargado de nombres iguales que se pasa como parámetro.
    [WebMethod]
    public DataSet SelectNameFilesProject(string name_file, int fk_project, String typeFile)
    {
        return cn.SelectNameFilesProject(name_file, fk_project, typeFile);
    }


    // Devuelve un DataTable con los datos de la Tabla TbFiles.
    [WebMethod]
    public DataSet SelectFiles(int fk_project, int fk_user, string typeFile)
    {
        return cn.SelectFiles(fk_project, fk_user, typeFile);
    }


    // Inserta un objeto Sagt en la base de datos en el proyecto especificado.
    [WebMethod]
    public int Insert_SagtFile(DataSet dsMultiFacetData, DataSet[] ldsMeans, DataSet[] ldsAnalysis, string nameFile,
        int fk_projects, int fk_user)
    {
        MultiFacetsObs mfo = MultiFacetData.MultiFacetsObs.DataSet2MultiFacetObs(dsMultiFacetData);
        ListMeans listMeans = ListMeans.ListDataSet2ListMeans(ldsMeans);
        Analysis_and_G_Study analysis = Analysis_and_G_Study.ListDataSet2Analysis_and_G_Study(ldsAnalysis);
        SagtFile sagtFile = new SagtFile(mfo, listMeans, analysis);
        return cn.Insert_SagtFile(sagtFile, nameFile, fk_projects, fk_user);
    }


    // Inserta un objeto fichero de Análisis en la base de datos en un proyecto especificado
    [WebMethod]
    public int Insert_AnalysisFile(DataSet[] arrayDataSet, string nameFile, int fk_projects, int fk_user)
    {
        Analysis_and_G_Study analysisFile = Analysis_and_G_Study.ListDataSet2Analysis_and_G_Study(arrayDataSet);
        return cn.Insert_AnalysisFile(analysisFile, nameFile, fk_projects, fk_user);
    }


    // Almacena los datos de la tabla de frecuencias en la base de datos. 
    [WebMethod]
    public int Insert_MultiFacetsObs(DataSet dsMultiFacetData)
    {
        MultiFacetsObs mfo = MultiFacetData.MultiFacetsObs.DataSet2MultiFacetObs(dsMultiFacetData);
        return cn.Insert_MultiFacetsObs(mfo);
    }


    // Inserta los datos de una lista de facetas en la base de datos y devuelve la clave primaria
    //  de esa lista.
    [WebMethod]
    public int Insert_ListFacets(DataTable dtListFacets, DataTable dtSkipLevels)
    {
        ListFacets lf = MultiFacetData.ListFacets.DataTables2ListFacets(dtListFacets, dtSkipLevels);
        return cn.Insert_ListFacets(lf);
    }


    // Inserta la faceta en la tabla TbFacets y la los niveles omitidos en TbSkipLevelFacet.
    //  Devuelve la clave primaria de esa faceta.
    //[WebMethod]
    //public int Insert_Facet(Facet f)
    //{
    //    return cn.Insert_Facet(f);
    //}


    // Insertar una tabla de medias en memoria.
    [WebMethod]
    public int Insert_ListMeans(DataSet[] ldsMeans)
    {
        ListMeans listMeans = ListMeans.ListDataSet2ListMeans(ldsMeans);
        return cn.Insert_ListMeans(listMeans);
    }


    // Inserta en la base de datos los datos del análisis de generalizabilidad.
    [WebMethod]
    public int Insert_Analysis(DataSet[] ldsAnalysis)
    {
        Analysis_and_G_Study analysis = Analysis_and_G_Study.ListDataSet2Analysis_and_G_Study(ldsAnalysis);
        return cn.Insert_Analysis(analysis);
    }


    // Devuelve un SagtFile que se recupera de la base de datos a partir de la clave primaria que
    //  se pasa como parámetro.
    [WebMethod]
    public SagtFileW ReturnSagtFile(int pk_file)
    {
        // Variable de retorno
        SagtFileW returnVal = new SagtFileW();
        SagtFile sagtFile = cn.ReturnSagtFile(pk_file);
        // MultifacetData
        returnVal.dsMultiFacetObs = sagtFile.GetMultiFacetsObs().MultiFacetObs2DataSet();
        // Lista de tablas de medias
        returnVal.listDataSetMeans = sagtFile.GetListMeans().ListMeans2DataSet();
        // Analisis de varianza
        returnVal.ldsAnalysis = sagtFile.GetAnalysis_and_G_Study().Analysis_and_G_Study2ListDataSets();

        return returnVal;
    }


    // Recupera los datos de un fichero de análisis.
    [WebMethod]
    public DataSet[] Return_AnalysisFile(int pk_file)
    {
        Analysis_and_G_Study analysis = cn.Return_AnalysisFile(pk_file);
        DataSet[] ldsAnalysis = analysis.Analysis_and_G_Study2ListDataSets();
        return ldsAnalysis;
    }


    // Devuelve una lista de enteros (los enteros que omite una determinada faceta) a partir
    //  de la clave foranea de la faceta que se pasa como parámetro.
    [WebMethod]
    public int[] Return_ListSkipLevels(int fk_facet)
    {
        List<int> l_int = cn.Return_ListSkipLevels(fk_facet);
        return l_int.ToArray();
    }


    // Devuelve un objeto que implemente la InterfaceObsTable (Tabla de frecuencias)
    [WebMethod]
    public DataSet Return_ObsTable(DataSet ListFacet, int fk_multi_facet_obs)
    {
        ListFacets lf = MultiFacetData.ListFacets.DataSet2ListFacets(ListFacet);
        InterfaceObsTable obsTable = cn.Return_ObsTable(lf, fk_multi_facet_obs);
        DataSet dsObsTable = obsTable.ObsTable2DataSet(lf);
        return dsObsTable;
    }


    // Devuelve un objeto de tablas de medias por defecto a partir de la clave foranea que se pasa
    //  como parámetro.
    [WebMethod]
    public DataSet Return_TableMeans(DataTable dtListfacet, DataTable dtSkipLevels, string facet_design, int fk_means,
        double grand_mean, double variance, double std_dev)
    {
        ListFacets lf = MultiFacetData.ListFacets.DataTables2ListFacets(dtListfacet, dtSkipLevels);
        TableMeans tbMeans = cn.Return_TableMeans(lf, facet_design, fk_means, grand_mean, variance, std_dev);
        return tbMeans.TableMeans2DataSet();
    }


    // Devuelve un objeto de tablas de medias de las desviaciones a partir de la clave foranea que se pasa
    //  como parámetro.
    [WebMethod]
    public DataSet Return_TableMeansDiff(DataSet dsListFacets, string facet_design, int fk_means,
        double grand_mean, double variance, double std_dev)
    {
        ListFacets lf = MultiFacetData.ListFacets.DataSet2ListFacets(dsListFacets);
        TableMeansDif tbMeansDif = cn.Return_TableMeansDiff(lf, facet_design, fk_means, grand_mean, variance, std_dev);
        return tbMeansDif.TableMeans2DataSet();
    }


    // Devuelve un objeto de tablas de medias de puntuaciones típicas a partir de la clave foranea que se pasa
    //  como parámetro.
    [WebMethod]
    public DataSet Return_TableMeansTypScore(DataSet dsListFacets, string facet_design, int fk_means,
        double grand_mean, double variance, double std_dev)
    {
        ListFacets lf = MultiFacetData.ListFacets.DataSet2ListFacets(dsListFacets);
        TableMeansTypScore tableMeansTypeScore = cn.Return_TableMeansTypScore(lf, facet_design, fk_means, grand_mean, variance, std_dev);
        return tableMeansTypeScore.TableMeans2DataSet();
    }


    /*=================================================================================================
     * Métodos de instancia
     *=================================================================================================*/
    // Realiza un mapeo de memoria de una tabla de medias
    [WebMethod]
    public void Insert_TableMeans(DataSet dsTbMeans, int fk_means)
    {
        TableMeans tbMeans = (TableMeans)TableMeans.DataSet2TableMeans(dsTbMeans);
        cn.Insert_TableMeans(tbMeans, fk_means);
    }

    // Realiza un mapeo de memoria de una tabla de medias de diferencias
    [WebMethod]
    public void Insert_TableMeansDif(DataSet dsTbMeansDif, int fk_means)
    {
        TableMeansDif tbMeansDif = (TableMeansDif)TableMeansDif.DataSet2TableMeans(dsTbMeansDif);
        cn.Insert_TableMeansDif(tbMeansDif, fk_means);
    }

    // Realiza un mapeo de memoria de una tabla de medias de diferencias
    [WebMethod]
    public void Insert_TableMeansTypScore(DataSet dsTbMeansTypScore, int fk_means)
    {
        TableMeansTypScore tbMeansTypScore = (TableMeansTypScore)TableMeansTypScore.DataSet2TableMeans(dsTbMeansTypScore);
        cn.Insert_TableMeansTypScore(tbMeansTypScore, fk_means);
    }

    // Operación para realizar la inserción de la posción de una faceta en la lista.
    //[WebMethod]
    //public void Aux_Insert_Facet_X_Pos(Facet f, ListFacets lf, int pk_facet, int pk_list_facets)
    //{
    //    cn.Aux_Insert_Facet_X_Pos(f, lf, pk_facet, pk_list_facets);
    //}

    // Inserta los datos de la lista de G-Parámetros.
    [WebMethod]
    public void Insert_G_ParametersList(DataSet[] lDataSetG_parameters, int fk_table_analysis_and_g_study)
    {
        List<G_ParametersOptimization> listG_P_Optimization = new List<G_ParametersOptimization>();
        int n = lDataSetG_parameters.Length;
        for (int i = 0; i < n; i++)
        {
            G_ParametersOptimization gp = G_ParametersOptimization.DataSet2G_Parameters(lDataSetG_parameters[i]);
            listG_P_Optimization.Add(gp);
        }
        cn.Insert_G_ParametersList(listG_P_Optimization, fk_table_analysis_and_g_study);
    }
}

