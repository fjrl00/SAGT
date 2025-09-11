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
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MultiFacetData;
using ProjectMeans;
using ProjectSSQ;

namespace Sagt
{
    public class SagtFile
    {
        /******************************************************************************************************
         *  Constantes de clase SagtFile
         ******************************************************************************************************/
        const string BEGIN_MULTIFACETSOBS = "<file_sagt_multifacetobs>";
        const string END_MULTIFACETSOBS = "</file_sagt_multifacetobs>";
        const string BEGIN_LISTMEANS = "<file_sagt_listmeans>";
        const string END_LISTMEANS = "</file_sagt_listmeans>";
        const string BEGIN_ANALYSIS_AND_G_STUDY = "<file_sagt_analysis_and_g_study>";
        const string END_ANALYSIS_AND_G_STUDY = "</file_sagt_analysis_and_g_study>";


        /******************************************************************************************************
         *  Variables de clase SagtFile
         ******************************************************************************************************/
        private MultiFacetsObs multiFacets;
        private ListMeans listMeans;
        private Analysis_and_G_Study tAnalysis_tG_Study_Opt;


        #region Constructores
        /******************************************************************************************************
         *  Constructores
         ******************************************************************************************************/

        public SagtFile()
        {
            this.multiFacets = null;
            this.listMeans = null;
            this.tAnalysis_tG_Study_Opt = null;
        }


        public SagtFile(MultiFacetsObs multiFacets, ListMeans listMeans, Analysis_and_G_Study tAnalysis_tG_Study_Opt)
        {
            this.multiFacets = multiFacets;
            this.listMeans = listMeans;
            this.tAnalysis_tG_Study_Opt = tAnalysis_tG_Study_Opt;
        }

        #endregion Constructores


        #region Métodos de instancia
        /******************************************************************************************************
         *  Métodos de instancia
         ******************************************************************************************************/
        

        public void SetMultiFacetsObs(MultiFacetsObs multiFacets)
        {
            this.multiFacets = multiFacets;
        }


        public void SetListMeans(ListMeans listMeans)
        {
            this.listMeans = listMeans;
        }


        public void SetAnalysis_and_G_Study(Analysis_and_G_Study tAnalysis_tG_Study_Opt)
        {
            this.tAnalysis_tG_Study_Opt = tAnalysis_tG_Study_Opt;
        }

        #endregion Métodos de instancia


        #region Métodos de Consulta

        /******************************************************************************************************
         *  Métodos de Consulta
         ******************************************************************************************************/


        public MultiFacetsObs GetMultiFacetsObs()
        {
            return this.multiFacets;
        }


        public ListMeans GetListMeans()
        {
            return this.listMeans;
        }


        public Analysis_and_G_Study GetAnalysis_and_G_Study()
        {
            return this.tAnalysis_tG_Study_Opt;
        }

        #endregion Métodos de Consulta


        /* Descripción:
         *  Devuelve la copia del objeto de tabla de Analysis
         */
        public Analysis_and_G_Study CopyTablesOfAnalysis()
        {
            return (Analysis_and_G_Study)this.tAnalysis_tG_Study_Opt.Clone();
        }


        #region Escritura de ficheros
        /******************************************************************************************************
         *  Escritura de ficheros
         ******************************************************************************************************/

        /* Descripción:
         *  Método de escritura en una archivo de sagt que puede contener: la tabla de observaciones,
         *  la lista de tabla de medias, la tablas de análisis varianza.
         * Devuelve:
         *  bool: True si se ha escrito correctamente, false en otro caso;
         */
        public bool WritingSagtFile(String path)
        {
            bool res = false; // variable de retorno

            using (StreamWriter writer = new StreamWriter(path))
            {
                if(this.multiFacets!=null)
                {
                    // writer.WriteLine(BEGIN_MULTIFACETSOBS);
                    res = this.multiFacets.WritingFileObsData(writer);
                    // writer.WriteLine(END_MULTIFACETSOBS);
                }
                if (this.listMeans != null)
                {
                    writer.WriteLine(BEGIN_LISTMEANS);
                    res = res | this.listMeans.StringWriterFileListMeans(writer);
                    writer.WriteLine(END_LISTMEANS);
                }
                if (this.tAnalysis_tG_Study_Opt != null)
                {
                    writer.WriteLine(BEGIN_ANALYSIS_AND_G_STUDY);
                    res = res | this.tAnalysis_tG_Study_Opt.StreamWriterFileAnalysisSSQ(writer);
                    writer.WriteLine(END_ANALYSIS_AND_G_STUDY);
                }
            }
            return res;
        }// end WritingSagtFile

        #endregion Escritura de ficheros

        #region Lectura de ficheros
        /******************************************************************************************************
         *  Lectura de ficheros
         ******************************************************************************************************/

        /* Descripción:
         *  Devuelve un objeto SagtFile tras la lectura de un fichero que puede contener los datos de la 
         *  tabla de frecuencias, las tablas de medias y/o las tablas de analisis de varianzas.
         * Parámetros:
         *      string path: String con el path del fichero.
         */
        public static SagtFile ReadingSagtFile(string path)
        {
            MultiFacetsObs multiFacets = null;
            ListMeans listMeans = null;
            Analysis_and_G_Study tAnalysis_tG_Study_Opt = null;
            using (StreamReader reader = new StreamReader(path))
            {
                try
                {
                    string line = reader.ReadLine();
                    // if (line != null && line.Equals(BEGIN_MULTIFACETSOBS))
                    if (line != null && line.Equals(MultiFacetsObs.BEGIN_MULTI_FACET_OBS))
                    {
                        multiFacets = MultiFacetsObs.ReadingFileObsData(reader, path);
                        // line = reader.ReadLine();
                        // if ((line = reader.ReadLine()).Equals(END_MULTIFACETSOBS))
                        if ((line = reader.ReadLine()).Equals(MultiFacetsObs.END_MULTI_FACET_OBS))
                        {
                            line = reader.ReadLine();
                        }
                        else
                        {
                            throw new SagtFileException("Error al leer la tabla de frecuencias en un fichero SAGT");
                        }
                    }
                    if (line != null && line.Equals(BEGIN_LISTMEANS))
                    {
                        listMeans = ListMeans.StreamReaderFileListMeans(reader);
                        if ((line = reader.ReadLine()).Equals(END_LISTMEANS))
                        {
                            line = reader.ReadLine();
                        }
                        else
                        {
                            throw new SagtFileException("Error al leer las tablas de medias en un fichero SAGT");
                        }
                    }
                    if (line != null && line.Equals(BEGIN_ANALYSIS_AND_G_STUDY))
                    {
                        tAnalysis_tG_Study_Opt = Analysis_and_G_Study.StreamReaderAnalysisSSQ(reader);
                        if ((line = reader.ReadLine()).Equals(END_ANALYSIS_AND_G_STUDY))
                        {
                            line = reader.ReadLine();
                        }
                        else
                        {
                            throw new SagtFileException("Error al leer las tablas de análisis de vairanza en un fichero SAGT");
                        }
                    }
                }
                catch (MultiFacetObsException)
                {
                    throw new SagtFileException("Error al leer un fichero SAGT");
                }
                catch (ListMeansException)
                {
                    throw new SagtFileException("Error al leer un fichero SAGT");
                }
                catch (Analysis_and_G_Study_Exception)
                {
                    throw new SagtFileException("Error al leer un fichero SAGT");
                }
            }// end using
            return new SagtFile(multiFacets, listMeans, tAnalysis_tG_Study_Opt);
        }// end ReadingSagtFile

        #endregion Lectura de ficheros

    }// end public class SagtFile
}// namespace Sagt
