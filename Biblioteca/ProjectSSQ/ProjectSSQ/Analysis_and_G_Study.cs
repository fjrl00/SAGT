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
 * Fecha de revisión: 10/Jul/2012                           
 * 
 * Descripción:
 *      Clase maestra de la libreria.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using MultiFacetData;

namespace ProjectSSQ
{
    public class Analysis_and_G_Study : System.ICloneable
    {
        /******************************************************************************************************
         *  Constantes de clase Analysis_and_G_Study para la escritura de archivo de datos
         ******************************************************************************************************/
        // Comienzo y fin del comentario de un fichero de análisis de suma de cuadrados
        const string BEGIN_COMMENT = "<file__analysis_ssq_comment>";
        const string END_COMMENT = "</file__analysis_ssq_comment>";
        // Comienzo y fin de un fichero de suma de cuadrados
        const string BEGIN_ANALYSIS_SSQ = "<file_analysis_ssq>";
        const string END_ANALYSIS_SSQ = "</file_analysis_ssq>";
        // Comienzo y fin de una lista de G Parámetros de Optimización
        const string BEGIN_LIST_G_PARAMETERS_OPT = "<list_g_parameters_opt>";
        const string END_LIST_G_PARAMETERS_OPT = "</list_g_parameters_opt>";

        /******************************************************************************************************
         *  Variables de clase Analysis_and_G_Study
         ******************************************************************************************************/
        private TableAnalysisOfVariance tableAnalysisVariance; // Tabla de análisis de varianza: 
                                                               // la que contiene la suma de cuadrados.
        private TableG_Study_Percent tableG_Study_Percent; // Tabla de G Parámetros: Varianzas de error y de diferenciación
        private List<G_ParametersOptimization> listG_P_Optimization; // lista con las sucesivas optimizaciones

        private DateTime dateCreation; // fecha hora de la creación.
        private string nameFileDataCreation; // Nombre del fichero de datos
        private string textComment; // texto con comentarios


        #region Constructores
        /******************************************************************************************************
         *  Constructores
         ******************************************************************************************************/

        public Analysis_and_G_Study()
        {
            this.dateCreation = DateTime.Now;
            this.textComment = "";
        }


        public Analysis_and_G_Study(List<string> l_design, MultiFacetsObs mfo, ListFacets differentiation,
            ListFacets instrumentation, bool zero)
            : this()
        {
            this.tableAnalysisVariance = new TableAnalysisOfVariance(l_design, mfo, zero);
            this.tableG_Study_Percent = new TableG_Study_Percent(differentiation, instrumentation, this.tableAnalysisVariance);
            this.listG_P_Optimization = new List<G_ParametersOptimization>();
        }


        public Analysis_and_G_Study(TableAnalysisOfVariance tableAnalysis, TableG_Study_Percent tableG)            
            : this()
        {
            this.tableAnalysisVariance = tableAnalysis;
            this.tableG_Study_Percent = tableG;
            this.listG_P_Optimization = new List<G_ParametersOptimization>();
        }


        public Analysis_and_G_Study(TableAnalysisOfVariance tableAnalysis, TableG_Study_Percent tableG,
            List<G_ParametersOptimization> listG_P_O)
            : this()
        {
            this.tableAnalysisVariance = tableAnalysis;
            this.tableG_Study_Percent = tableG;
            this.listG_P_Optimization = listG_P_O;
        }

        #endregion Constructores


        #region Métodos de Consulta
        /******************************************************************************************************
         *  Métodos de constulta
         *  ====================
         *  - TableAnalysisVariance
         *  - TableG_Study
         *  - ListG_P_Optimization
         *  - List_Facets_Intrumentation
         *  - List_Facets_Differentiation
         *  - GetNameFileDataCreation
         *  - GetDateTime
         *  - GetTextComment
         ******************************************************************************************************/

        /* Descripción:
         *  Devuelve la tabla de análisis de varianza
         */
        public TableAnalysisOfVariance TableAnalysisVariance()
        {
            return this.tableAnalysisVariance;
        }


        /* Descripción:
         *  Devuelve la tabla G_Study.
         */
        public TableG_Study_Percent TableG_Study_Percent()
        {
            return this.tableG_Study_Percent;
        }


        /* Descripción:
         *  Devuelve la lista con los resultados de los parámetros de optimización (Coef_G relativo
         *  y absoluto entre otros).
         */
        public List<G_ParametersOptimization> ListG_P_Optimization()
        {
            return this.listG_P_Optimization;
        }


        /* Descripción: 
         *  Devuelve la lista de facetas de instrumentación
         */
        public ListFacets List_Facets_Intrumentation()
        {
            return this.tableG_Study_Percent.LfInstrumentation();
        }


        /* Descripción: 
         *  Devuelve la lista de facetas de diferenciación
         */
        public ListFacets List_Facets_Differentiation()
        {
            return this.tableG_Study_Percent.LfDifferentiation();
        }


        /* Descripción:
         *  Devuelve el nombre del fichero que contiene las tablas de frecuencias.
         */
        public string GetNameFileDataCreation()
        {
            return nameFileDataCreation;
        }


        /* Descripción:
         *  Devuelve la fecha de creación.
         */
        public DateTime GetDateTime()
        {
            return this.dateCreation;
        }


        /* Descripción:
         *  Devuelve el texto con los comentarios
         */
        public string GetTextComment()
        {
            return this.textComment;
        }


        /* Descripción:
         *  Devuelve la lista de facetas completa. Instrumentación y diferenciación juntas.
         */
        public ListFacets GetListFacets()
        {
            return this.tableAnalysisVariance.ListFacets();
        }

        #endregion Métodos de Consulta



        #region Métodos de instancia
        /******************************************************************************************************
         *  Métodos de instancia
         *  ====================
         *   - AddG_Parameter
         *   - SetNameFileDataCreation
         *   - SetDateTime
         *   - SetTextComment
         *   - UpdateSsq
         ******************************************************************************************************/

        /* Descripción:
         *  Añade una objeto de la clase G_Parameters que contiene los resultados de una optimización en
         *  la lista de parámetros de optimización.
         * Parámetros:
         *      G_ParametersOptimization g_Parameters: Objeto que contiene los parámetros de optimización.
         */
        public void AddG_Parameter(G_ParametersOptimization g_Parameters)
        {
            this.listG_P_Optimization.Add(g_Parameters);
        }


        /* Descripción:
         *  Devuelve el nombre del fichero que contiene las tablas de frecuencias.
         */
        public void SetNameFileDataCreation(string nameFile)
        {
            if (string.IsNullOrEmpty(nameFile))
            {
                throw new Analysis_and_G_Study_Exception("No se asigna el nombre del fichero de creación");
            }
            this.nameFileDataCreation = nameFile;
        }


        /* Descripción:
         *  Asigna la fecha de creación.
         */
        public void SetDateTime(DateTime date)
        {
            this.dateCreation = date;
        }


        /* Descripción:
         *  Asigna el texto con los comentarios
         */
        public void SetTextComment(string text)
        {
            this.textComment = text;
        }


        /* Descripción:
         *  Actualiza la suma de cuadrados actualizando todos los datos calculados
         * Parámetros:
         *      Dictionary<string, double?> newSsq la nueva suma de cuadrados que se va a actualizar
         */
        public Analysis_and_G_Study UpdateSsq(Dictionary<string, double?> newSsq)
        {
            // Obtenemos la tabla de análisis varianza actualizada.
            ListFacets lf = this.tableAnalysisVariance.ListFacets();
            TableAnalysisOfVariance tb = new TableAnalysisOfVariance(lf, newSsq);
            // Obtenemos la tabla de G-Parámetros actualizada
            ListFacets differentiation = this.tableG_Study_Percent.LfDifferentiation();
            ListFacets instrumentation = this.tableG_Study_Percent.LfInstrumentation();
            TableG_Study_Percent TableG = new TableG_Study_Percent(differentiation, instrumentation, tb);
            // Obtenemos la lista de G-Parámetros actualizada
            List<G_ParametersOptimization> l_Gp = new List<G_ParametersOptimization>();
            Analysis_and_G_Study anal_and_G_Stududy = new Analysis_and_G_Study(tb, TableG);
            int numGP_Op = this.listG_P_Optimization.Count;
            for (int i = 0; i < numGP_Op; i++)
            {
                ListFacets lf_Opt_level = this.listG_P_Optimization[i].G_ListFacets();
                G_ParametersOptimization upDateGp = anal_and_G_Stududy.Calc_G_ParametersOptimización(lf_Opt_level);
                l_Gp.Add(upDateGp);
            }

            // Devolvemos el objeto actualizado
            return new Analysis_and_G_Study(tb, TableG, l_Gp);

        }// end UpdateSsq


        #endregion Métodos de instancia




        /* Descripción:
         *  Calcula un nuevo nivel de optimización a partir de una lista de facetas que contiene los 
         *  nuevos niveles de estimación.
         */
        public G_ParametersOptimization Calc_G_ParametersOptimización(ListFacets newLf)
        {
            TableAnalysisOfVariance tableAnalysis = this.tableAnalysisVariance.ReplaceListFacets(newLf);

            ListFacets newLfDiff = new ListFacets();
            ListFacets newLfInst = new ListFacets();

            ListFacets lf = this.tableAnalysisVariance.ListFacets();

            ListFacets lfDiff = this.tableG_Study_Percent.LfDifferentiation();
            int n = lfDiff.Count();

            for (int i = 0; i < n; i++)
            {
                Facet f = lfDiff.FacetInPos(i);
                newLfDiff.Add(newLf.LookingFacet(f.Name()));
            }

            ListFacets lfInst = this.tableG_Study_Percent.LfInstrumentation();
            n = lfInst.Count();

            for (int i = 0; i < n; i++)
            {
                Facet f = lfInst.FacetInPos(i);
                newLfInst.Add(newLf.LookingFacet(f.Name()));
            }

            return this.Calc_G_ParametersOptimización(newLf, newLfDiff, newLfInst);
        }// end Calc_G_ParametersOptimización


        /* Descripción:
         *  Calcula los G_Parámetros de optimización a partir de los nuevos niveles pasados como parámetros.
         *  
         * NOTA 1: No comprueba que las lista de facetas que se pasan como parámetros sean "legales" para su uso
         * 
         * ¡¡MUY IMPORTANTE!!
         * ==================
         * NOTA 2: Es necesario revisar la estimación de los nuevos niveles cuando se trata de fijo o finito aleatorio.
         */
        public G_ParametersOptimization Calc_G_ParametersOptimización(ListFacets newlf, ListFacets lfDiff, ListFacets lfInst)
        {
            /* Tenemos que ordenar la lista de facetas newlf en el mismo orden que la original
             * para luego no tener problemas para encontrar los datos.
             */
            TableAnalysisOfVariance tableAnalysis = this.TableAnalysisVariance();

            /* Reordeno la nueva lista de facetas para que tenga en mismo orden que la original ya que de otro
             * modo obtendréamos errores al mostrar los datos en la Tabla de resumen (opción de optimización).
             */
            ListFacets originalLF = tableAnalysis.ListFacets(); // lista de facetas original
            newlf = originalLF.SortByListFacets(newlf); // nueva lista de facetas ordenada
            lfDiff = this.List_Facets_Differentiation().SortByListFacets(lfDiff);
            lfInst = this.List_Facets_Intrumentation().SortByListFacets(lfInst);

            TableG_Study_Percent newGp = new TableG_Study_Percent(lfDiff, lfInst);

            /* Si todas las facetas de análisis y de optimización son infinitas
             * entoces uso la tabla de análisis original 
             * o 
             * si ninguna de las facetas es fija y todas tienen el mismo universo que el original
             */
            if ((originalLF.HasAllFacetsSizeInfinite() && newlf.HasAllFacetsSizeInfinite())
                || (!originalLF.AtLeastOneIsFixed() && (originalLF.EqualsSizeOfUniverse(newlf))))
            {
                newGp = new TableG_Study_Percent(lfDiff, lfInst, this.tableAnalysisVariance);
            }
            /* Si ni las facetas de análisis ni las de optimización tienen facetas fijas y 
             * se produce en cambio de facetas finita a infinita entonces se utilizan las componentes de
             * varianza aleatorios no los infinitos
             */
            else
            {
                // obtenemos la suma de cuadrados para crear la nueva tabla de análisis
                Dictionary<string, double?> ssq = new Dictionary<string, double?>();
                List<string> newllf = newlf.CombinationStringWithoutRepetition();

                int numllf = newllf.Count;
                for (int i = 0; i < numllf; i++)
                {
                    string iLF = newllf[i];
                    ssq.Add(iLF, tableAnalysis.SSQ(iLF));
                }

                TableAnalysisOfVariance newTbAnalysisVar = new TableAnalysisOfVariance(newlf, ssq);
                // this.listOfListTableSSQ.Add(newTbAnalysisVar);
                newGp = new TableG_Study_Percent(lfDiff, lfInst, newTbAnalysisVar);
            }

            return newGp.G_ParametersOptimization();
        }// Calc_G_ParametersOptimización


        
        /* Descripción:
         *  Sustituye el nombre de una faceta y propaga los cambios al resto de los elementos.
         * Parámetros:
         *  string oldName: Nombre antiguo de la faceta que vamos a sustituir.
         *  string newName: Nombre nuevo de la faceta por el cual sustituimos
         */
        public Analysis_and_G_Study ReplaceNameOfFacet(string oldName, string newName)
        {
            // Clonamos
            Analysis_and_G_Study analysisReplace = (Analysis_and_G_Study)this.Clone();

            ListFacets lf = analysisReplace.GetListFacets();

            Facet f = lf.LookingFacet(oldName);
            if (f != null)
            {
                // sustituimos la lista de facetas
                
                // sustituimos la tabla de analysis
                TableAnalysisOfVariance tableAnalyis = analysisReplace.TableAnalysisVariance();

            }

            return analysisReplace;
        }// end ReplaceNameOfFacet


        #region Remplazar lista de facetas
        /* Descripción:
         *  Remplaza la lista de facetas que se pasa como parámetro por la lista presente 
         *  en la estructura de datos.
         * @Precondición:
         *  La nueva lista tiene que tener el mismo número de facetas y para cada una de ellas
         *  debe tener la misma jerarquia de anidamientos.
         */
        public Analysis_and_G_Study ReplaceListOfFacet(ListFacets newLf)
        {
            TableAnalysisOfVariance tableAnalysis = this.tableAnalysisVariance.ReplaceListFacets(newLf);

            ListFacets newLfDiff = new ListFacets();
            ListFacets newLfInst = new ListFacets();

            ListFacets lf = this.tableAnalysisVariance.ListFacets();

            ListFacets lfDiff = this.tableG_Study_Percent.LfDifferentiation();
            int n = lfDiff.Count();
            
            for (int i = 0; i < n; i++)
            {
                Facet f = lfDiff.FacetInPos(i);
                int pos = lf.IndexOf(f);
                newLfDiff.Add(newLf.FacetInPos(pos));
            }

            ListFacets lfInst = this.tableG_Study_Percent.LfInstrumentation();
            n = lfInst.Count();

            for (int i = 0; i < n; i++)
            {
                Facet f = lfInst.FacetInPos(i);
                int pos = lf.IndexOf(f);
                newLfInst.Add(newLf.FacetInPos(pos));
            }

            TableG_Study_Percent tableG_study = new TableG_Study_Percent(newLfDiff, newLfInst, tableAnalysis);

            
            List<G_ParametersOptimization> newList_gp = new List<G_ParametersOptimization>();
            Analysis_and_G_Study newAnalysis_and_G_Study = new Analysis_and_G_Study(tableAnalysis, tableG_study, newList_gp);
            n = this.listG_P_Optimization.Count;
            for (int i = 0; i < n; i++ )
            {
                G_ParametersOptimization gp = this.listG_P_Optimization[i];
                ListFacets lf_Of_levels = gp.G_ListFacets();
                ListFacets newG_ListFacets = new ListFacets();
                int numFacets = lf_Of_levels.Count();
                for (int j = 0; j < numFacets; j++)
                {
                    Facet f_new = newLf.FacetInPos(j);
                    Facet f_old = lf_Of_levels.FacetInPos(j);
                    newG_ListFacets.Add(new Facet(f_new.Name(),f_old.Level(),f_new.Comment(),f_old.SizeOfUniverse(),f_new.Omit()));
                }

                G_ParametersOptimization newGp = newAnalysis_and_G_Study.Calc_G_ParametersOptimización(newG_ListFacets);
                newList_gp.Add(newGp);
            }

            Analysis_and_G_Study retVal = new Analysis_and_G_Study(tableAnalysis, tableG_study, newList_gp);
            retVal.SetTextComment(this.GetTextComment());
            return retVal;
                
        }// end ReplaceListOfFacet

        #endregion Remplazar lista de facetas


        #region Implementacion de la interfaz
        /******************************************************************************************************
         *  Implementacion de la interfaz Cloneable
         *  =======================================
         ******************************************************************************************************/

        /* Descripción:
         *  Devuelve una copy en profundidad del objeto, menos la fecha de creción.
         */
        public object Clone()
        {
            TableAnalysisOfVariance copyTableAnalysis = (TableAnalysisOfVariance)this.tableAnalysisVariance.Clone();
            TableG_Study_Percent copyTableG_Study_Percent = (TableG_Study_Percent)this.tableG_Study_Percent.Clone();

            List<G_ParametersOptimization> copyListG_P_Optimization = new List<G_ParametersOptimization>();

            int n = this.listG_P_Optimization.Count;
            for (int i = 0; i < n; i++)
            {
                G_ParametersOptimization copyGp = (G_ParametersOptimization)this.listG_P_Optimization[i].Clone();
                copyListG_P_Optimization.Add(copyGp);
            }

            Analysis_and_G_Study copyAnalysis_and_G_Study = new Analysis_and_G_Study(copyTableAnalysis,
                copyTableG_Study_Percent, copyListG_P_Optimization);

            copyAnalysis_and_G_Study.dateCreation = this.dateCreation;
            copyAnalysis_and_G_Study.nameFileDataCreation = string.Copy(this.nameFileDataCreation);
            copyAnalysis_and_G_Study.textComment = string.Copy(this.textComment);

            return copyAnalysis_and_G_Study;
        }

        #endregion Implementacion de la interfaz


        #region Escritura en archivo
        /******************************************************************************************************
         *  Métodos de escritura en archivo
         *  ===============================
         *   - WritingFileAnalysisSSQ
         ******************************************************************************************************/


        /* Descripción:
        *  Método de escritura en una archivo de análisis de suma de cuadrados.
        * Devuelve:
        *  bool: True si se ha escrito correctamente, false en otro caso;
        */
        public bool WritingFileAnalysisSSQ(String fileName)
        {
            bool res = false; // variable de retorno

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                res = StreamWriterFileAnalysisSSQ(writer);
            }
            return res;
        }// end WritingFileAnalysisSSQ


        /* Decripción:
         *  Escritura de datos de un fichero a traves de un StreamWriter
         */
        public bool StreamWriterFileAnalysisSSQ(StreamWriter writer)
        {
            writer.WriteLine(this.nameFileDataCreation);// escribimos el path del fichero con el que se crearon los datos
            writer.WriteLine(this.dateCreation.ToString(new CultureInfo("es-ES", true)));// fecha en la se creo el archivo

            // escribimos el comentario del fichero de analisis de suma de cuadrados
            writer.WriteLine(BEGIN_COMMENT);
            // writer.WriteLine(this.textComment);
            writeString(writer, this.textComment);
            writer.WriteLine(END_COMMENT);

            // escribimos el resto de objetos del que se compone el fichero
            writer.WriteLine(BEGIN_ANALYSIS_SSQ);

            // Escribimos la tabla de Análisis
            this.tableAnalysisVariance.WritingStreamGTableAnalysisOfVariance(writer);

            // Escribimos la tabla G_Study
            this.tableG_Study_Percent.WritingStreamTableG_Study_Percent(writer);

            // Escribimos la lista de parámetros de optimización
            writer.WriteLine(BEGIN_LIST_G_PARAMETERS_OPT);
            int n = this.listG_P_Optimization.Count;
            for (int i = 0; i < n; i++)
            {
                this.listG_P_Optimization[i].WritingStreamGParametersOptimization(writer);
            }
            writer.WriteLine(END_LIST_G_PARAMETERS_OPT);

            // Final del fichero de análisis de suma de cuadrados
            writer.WriteLine(END_ANALYSIS_SSQ);
            return true;
        }

        /* Descripción:
         *  Es una operación auxiliar que se usa para escribir una a una cada linea del comentario y
         *  manterner el retorno de carro.
         */
        private void writeString(StreamWriter writer, string txt)
        {
            if (txt == null)
            {
                writer.WriteLine("");
            }
            else
            {
                char[] delimeterChars = { '\n' }; // nuestro delimitador será el caracter '/'
                string[] arrayline = txt.Split(delimeterChars);
                int n = arrayline.Length;
                for (int i = 0; i < n; i++)
                {
                    writer.WriteLine(arrayline[i]);
                }
            }
        }

        #endregion Escrittura en archivo


        #region Lectura de un archivo
        /******************************************************************************************************
         *  Métodos de Lectura en un archivo
         *  ================================
         *   - ReadingFileAnalysisSSQ
         ******************************************************************************************************/


        /* Descripción:
         *  Método de Lectura en una archivo de Tablas de análisis de varianza, G_Study y lista de
         *  G_Parametros de optimización. Los datos del archivo pasa al objeto
         *  ListMeans desde el que se hace la llamado por lo que se perderan los
         *  datos de este.
         */
        public static Analysis_and_G_Study ReadingFileAnalysisSSQ(String fileName)
        {
            Analysis_and_G_Study tables = new Analysis_and_G_Study();

            using (StreamReader reader = new StreamReader(fileName))
            {
                tables = StreamReaderAnalysisSSQ(reader);
            }// end using
            
            return tables;

        }// end ReadingFileAnalysisSSQ


        /* Descripción:
         *  Lectura de datos a traves de un streamReader
         */
        public static Analysis_and_G_Study StreamReaderAnalysisSSQ(StreamReader reader)
        {
            Analysis_and_G_Study tables = new Analysis_and_G_Study();
            List<G_ParametersOptimization> listG_P = new List<G_ParametersOptimization>();
            TableAnalysisOfVariance tb_analysis = null;
            TableG_Study_Percent tb_G_Study = null;

            try
            {

                string nameFile = reader.ReadLine(); // path del fichero del que se extrajeron los datos
                DateTime dateFile = DateTime.ParseExact(reader.ReadLine(), "dd/MM/yyyy H:mm:ss",
                                   new CultureInfo("es-ES", false), DateTimeStyles.AssumeLocal);

                string line;

                // leemos el comentario
                if ((line = reader.ReadLine()) == null || !line.Equals(BEGIN_COMMENT))
                {
                    throw new Analysis_and_G_Study_Exception($"Expected '{BEGIN_COMMENT}' but found '{line}' when parsing Analysis_and_G_Study.");
                }

                StringBuilder commentBuilder = new System.Text.StringBuilder();
                while ((line = reader.ReadLine()) != null && !line.Equals(END_COMMENT))
                {
                    if (commentBuilder.Length > 0)
                        commentBuilder.Append('\n');
                    commentBuilder.Append(line);
                }
                if (line == null)
                {
                    throw new Analysis_and_G_Study_Exception("Unexpected end of file while reading Analysis_and_G_Study.");
                }

                if ((line = reader.ReadLine()) == null || !line.Equals(BEGIN_ANALYSIS_SSQ))
                {
                    throw new Analysis_and_G_Study_Exception($"Expected '{BEGIN_ANALYSIS_SSQ}' but found '{line}' when parsing Analysis_and_G_Study.");
                }



                // Lectura de tablas análisis
                if ((line = reader.ReadLine()) == null || !line.Equals(TableAnalysisOfVariance.BEGIN_TABLE_ANALYSIS_OF_VARIANCE))
                {
                    throw new Analysis_and_G_Study_Exception(
                        $"Expected '{TableAnalysisOfVariance.BEGIN_TABLE_ANALYSIS_OF_VARIANCE}' but found '{line}' when parsing Analysis_and_G_Study.");
                }

                tb_analysis = TableAnalysisOfVariance.ReadingStreamTableAnalysisOfVariance(reader);

                // Lectura de tabla G_Study
                if ((line = reader.ReadLine()) == null || !line.Equals(ProjectSSQ.TableG_Study_Percent.BEGIN_TABLE_G_STUDY_PERCENT))
                {
                    throw new Analysis_and_G_Study_Exception(
                        $"Expected '{ProjectSSQ.TableG_Study_Percent.BEGIN_TABLE_G_STUDY_PERCENT}' but found '{line}' when parsing Analysis_and_G_Study.");
                }

                tb_G_Study = ProjectSSQ.TableG_Study_Percent.ReadingStreamTableG_Study_Percent(reader);

                // lectura de lista de parámetros de optimización
                if ((line = reader.ReadLine()) == null || !line.Equals(BEGIN_LIST_G_PARAMETERS_OPT))
                {
                    throw new Analysis_and_G_Study_Exception($"Expected '{BEGIN_LIST_G_PARAMETERS_OPT}' but found '{line}' when parsing Analysis_and_G_Study.");
                }

                while ((line = reader.ReadLine()) != null && !line.Equals(END_LIST_G_PARAMETERS_OPT))
                {
                    if ((line = reader.ReadLine()).Equals(G_ParametersOptimization.BEGIN_G_PARAMETERS_OPT))
                    {
                        throw new Analysis_and_G_Study_Exception(
                            $"Expected '{G_ParametersOptimization.BEGIN_G_PARAMETERS_OPT}' but found '{line}' when parsing Analysis_and_G_Study.");
                    }

                    G_ParametersOptimization gp = G_ParametersOptimization.ReadingStreamGParametersOptimization(reader);
                    listG_P.Add(gp);
                }
                if (line == null)
                {
                    throw new Analysis_and_G_Study_Exception("Unexpected end of file while reading Analysis_and_G_Study.");
                }

                if (!(line = reader.ReadLine()).Equals(END_ANALYSIS_SSQ))
                {
                    throw new Analysis_and_G_Study_Exception($"Expected '{END_ANALYSIS_SSQ}' but found '{line}' when parsing Analysis_and_G_Study.");
                }

                tables = new Analysis_and_G_Study(tb_analysis, tb_G_Study, listG_P);
                tables.SetNameFileDataCreation(nameFile); // asignamos nombre
                tables.SetDateTime(dateFile); // asignamos fecha
                tables.SetTextComment(commentBuilder.ToString()); // asignamos comentario
            }
            catch(FormatException ex)
            {
	            throw new Analysis_and_G_Study_Exception($"Unexpected value found when parsing Analysis_and_G_Study: {ex.Message}");
            }
            catch (TableAnalysisOfVarianceException ex)
            {
                throw new Analysis_and_G_Study_Exception("Error in Analysis_and_G_Study.", ex);
            }
            catch (TableG_Study_PercentException ex)
            {
                throw new Analysis_and_G_Study_Exception("Error in Analysis_and_G_Study.", ex);
            }
            catch (G_ParametersOptimizationException ex)
            {
                throw new Analysis_and_G_Study_Exception("Error in Analysis_and_G_Study.", ex);
            }

            return tables;
        }// end StreamReaderAnalysisSSQ

        #endregion Lectura de un archivo


        #region Conversion con DataSet
        /* Descripción:
         *  Devuelve un DataSet con los datos de la tabla de análisis
         */
        public DataSet TableAnalysis2DataSet()
        {
            return this.tableAnalysisVariance.TbAnalysis2DataSet();
        }

        /* Descripción:
         *  Devuelve un DataSet Con los datos de la Tabla G_Study();
         */
        public DataSet TableG_Study2DataSet()
        {
            List<String> ldesign = this.tableAnalysisVariance.SourcesOfVar();
            return this.tableG_Study_Percent.TableG_Study2TableSet(ldesign);
        }

        /* Descripción:
         *  Devuelve un lista de dataSets. El primero contiene los datos del objeto(nombre, fecha, 
         *  comentarios), el segundo la tabla de análisis, el tercero la tabla G_study, a partir del
         *  cuarto contiene los G_parametros de optimización.
         */
        public DataSet[] Analysis_and_G_Study2ListDataSets()
        {
            int numG_P = this.listG_P_Optimization.Count;
            DataSet[] lDataSet = new DataSet[numG_P + 3]; // Variable de retorno

            DataSet dsAnalysis_and_G_study = new DataSet("dsAnalysis_and_G_study");
            DataTable dtAnalysis_and_G_Study = new DataTable("dt_analysis_and_G_study");
            dtAnalysis_and_G_Study.Columns.Add(new DataColumn("nameFileDataCreation", System.Type.GetType("System.String")));
            dtAnalysis_and_G_Study.Columns.Add(new DataColumn("dateCreation", System.Type.GetType("System.DateTime")));
            dtAnalysis_and_G_Study.Columns.Add(new DataColumn("textComment", System.Type.GetType("System.String")));

            DataRow row = dtAnalysis_and_G_Study.NewRow();

            row["nameFileDataCreation"] = this.nameFileDataCreation;
            row["dateCreation"] = this.dateCreation;
            row["textComment"] = this.textComment;

            dtAnalysis_and_G_Study.Rows.Add(row);
            dsAnalysis_and_G_study.Tables.Add(dtAnalysis_and_G_Study);
            lDataSet[0] = dsAnalysis_and_G_study;
            lDataSet[1] = this.tableAnalysisVariance.TbAnalysis2DataSet();
            List<String> ldesign = this.tableAnalysisVariance.SourcesOfVar();
            lDataSet[2] = this.tableG_Study_Percent.TableG_Study2TableSet(ldesign);

            for (int i = 0; i < numG_P; i++)
            {
                G_ParametersOptimization gp = this.listG_P_Optimization[i];
                DataSet ds = gp.G_Parameters2DataSet();
                lDataSet[i+3] = ds;
            }

            return lDataSet;
        }// end Analysis_and_G_Study


        /* Descripción:
         *  Construye un objeto de la clase a través de una lista de dataSet que contiene los datos necesarios
         *  en el formato de exportación de la propia clase.
         */
        public static Analysis_and_G_Study ListDataSet2Analysis_and_G_Study(DataSet[] lDataSets)
        {
            DataTable dt_analysis_and_G_study = lDataSets[0].Tables["dt_analysis_and_G_study"];
            DataRow row = dt_analysis_and_G_study.Rows[0];
            string nameFileCreation = (string)row["nameFileDataCreation"];
            DateTime datetime = (DateTime)row["dateCreation"];
            string textComment = "";
            if (row["textComment"] != null)
            {
                textComment = (string)row["textComment"];
            }

            DataSet dsTableAnalysis = lDataSets[1];
            TableAnalysisOfVariance tbAnlyisis = TableAnalysisOfVariance.DataSet2TbAnalysisOfVar(dsTableAnalysis);
            ListFacets lf = tbAnlyisis.ListFacets();
            DataSet dsTableG_Study = lDataSets[2];
            TableG_Study_Percent tbG_Study = ProjectSSQ.TableG_Study_Percent.DataSet2TableG_Study(dsTableG_Study, lf);

            List<G_ParametersOptimization> listG_P = new List<G_ParametersOptimization>();
            int numDataSets = lDataSets.Length;
            for (int i = 3; i < numDataSets; i++)
            {
                DataSet dsGP = lDataSets[i];
                G_ParametersOptimization gp = G_ParametersOptimization.DataSet2G_Parameters(dsGP);
                listG_P.Add(gp);
            }
            // Variable de retorno
            Analysis_and_G_Study analysis_and_g_study = new Analysis_and_G_Study(tbAnlyisis, tbG_Study, listG_P);

            analysis_and_g_study.SetNameFileDataCreation(nameFileCreation);
            analysis_and_g_study.SetDateTime(datetime);
            analysis_and_g_study.SetTextComment(textComment);

            return analysis_and_g_study;
        }// end ListDataSet2Analysis_and_G_Study
        #endregion Conversion con DataSet

    }// end public class Analysis_and_G_Study
}// namespace ProjectSSQ
