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
 * Fecha de revisión: 07/Nov/2011                           
 * 
 * Descripción:
 *      Lectura de fichero suma de cuadrados (.ssq, GT E 2.0).
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectSSQ;
using MultiFacetData;
using System.IO;
using System.Globalization;

namespace SsqPY
{
    public class RsaSsqPY
    {
        /*================================================================================================
         * Constantes
         *================================================================================================*/
        public const char CHAR_NEST = '/';

        /*================================================================================================
         * Variables
         *================================================================================================*/
        private List<SSqPY> list_ssqOfFile;
        private List<TableG_Study_Percent> list_G_Paramertes;
        

        /*================================================================================================
         * Constructores
         *================================================================================================*/
        public RsaSsqPY()
        {
            this.list_ssqOfFile = new List<SSqPY>();
            this.list_G_Paramertes = new List<TableG_Study_Percent>();
        }

        public RsaSsqPY(SSqPY ssq)
            :this()
        {
            this.list_ssqOfFile.Add(ssq);
            this.list_G_Paramertes.Add(new TableG_Study_Percent(ssq.SourceOfVarDepend(), ssq.SourceOfVarInDepend(), ssq));
        }

        public RsaSsqPY(SSqPY ssq, TableG_Study_Percent gParam)
            :this()
        {
            this.list_ssqOfFile.Add(ssq);
            this.list_G_Paramertes.Add(gParam);
        }


        #region Métodos de consulta

        /*================================================================================================
         * Métodos de consulta
         *================================================================================================*/

        /* Descripción:
         *  Devuelve el objeto que contiene la suma de cuadrados que se ha leido desde el archivo
         */
        public List<SSqPY> List_SsqOfFile()
        {
            return this.list_ssqOfFile;
        }

        /* Descripción:
         *  Devuelve la lista de G_Parameters.
         */
        public List<TableG_Study_Percent> SssqListOfG_Parameters()
        {
            return this.list_G_Paramertes;
        }
        #endregion Métodos de consulta

        /* Descripción:
         *  Añade a cada variable un elemento.
         * Parámetros:
         *      SSqPY ssq: 
         *      G_Parameters gParam
         */
        public void Add(SSqPY ssq, TableG_Study_Percent gParam) 
        {
            this.list_ssqOfFile.Add(ssq);
            this.list_G_Paramertes.Add(gParam);
        }

        /* Descripción:
         *  Añade al objeto la tabla suma de cudrados y la tabla G_Parametros. El Objeto G_Parametros
         *  se calcula a partir de los datos de suma de cuadrados.
         * Parámetros:
         *      SSqPY ssq: Tabla de suma de cuadrados
         *
        public void Add(SSqPY ssq)
        {
            this.list_ssqOfFile.Add(ssq);
            this.list_G_Paramertes.Add(gParam);
        }
         */

        /* Descripción:
         *  Lee un fichero .ssq (fichero de suma de cuadrados, GT E 2.0, 1996) para obtener el objeto
         *  que contiene la suma de cuadrados
         */
        public static RsaSsqPY ReadFileRsaPY(String nameFile)
        {
            RsaSsqPY retVal = null;
            using (TextReader reader = new StreamReader(nameFile))
            {
                try
                {
                    string line = reader.ReadLine(); // path del fichero 

                    /* Hay que analizar el path para saber que tipo de fichero es. Si el path termina con
                     * extension .anl es un fichero de de analisis. Si tiene extension .ssq procede de un
                     * archivo de suma de cuadrados*/
                    string xt = fileExtension(line);
                    xt = xt.ToLower();

                    /* La siguiente linea puede ser la descripción del fichero o un número que
                             * indica el tipo de fichero. Por los ejemplos que he visto debería ser un dos
                             * Entonces salto lineas hasta encontrar un 2.
                             */
                    line = reader.ReadLine();
                    string descriptionFile = "";

                    bool isN = IsNumeric(line);
                    if (!isN)
                    {
                        descriptionFile = line; // contiene la descripción del fichero
                    }
                    while (!isN)
                    {
                        line = reader.ReadLine();
                        isN = IsNumeric(line);
                    }
                    // Así saltamos la posible linea de descripción de fichero
                    // lo siguiente es leer la linea con un número que indica el tipo de archivo.
                    switch (xt)
                    {
                        case ("anl"):
                            // nos saltamos el path del fichero de datos y otras lineas hasta llegar al número de facetas
                            while(!line.Equals("N"))
                            {
                                line = reader.ReadLine();
                            }
                            line = reader.ReadLine();
                            retVal = ReadFileGT_RsaSsqPY_Type1(reader, descriptionFile);
                            break;
                        case ("ssq"):
                            
                            if (isN)
                            {
                                int typeFile = int.Parse(line);
                                switch (typeFile)
                                {
                                    case (1):
                                        retVal = ReadFileGT_RsaSsqPY_Type1(reader, descriptionFile);
                                        break;
                                    case (2):
                                        retVal = ReadFileGT_RsaSsqPY_Type2(reader, descriptionFile);
                                        break;
                                    default:
                                        throw new RsaSsqPY_Exception("Error en el formato del archivo");
                                }
                            }
                            break;
                        default:
                            throw new RsaSsqPY_Exception("Error en el formato del archivo");
                    }

                    
                }
                catch (FormatException)
                {
                    throw new RsaSsqPY_Exception("Error en el formato del fichero");
                }

            }// end using
            return retVal;
        }// end public static SSqPY ReadFileRsaPY(String nameFile)


        /* Descrpción
         *  Devuelve un objeto RsaSsqPY extraido del fichero Lectura de un fichero de análisis tipo 1
         * Parámetros:
         *      TextReader textRd: Fragmento de texto que queremos leer.
         */
        private static RsaSsqPY ReadFileGT_RsaSsqPY_Type1(TextReader textRd, string descripF)
        {
            RsaSsqPY retVal = null;
            using (TextReader reader = textRd)
            {
                ListFacets lf = new ListFacets();
                Dictionary<string, double?> ssq = new Dictionary<string, double?>();
                Dictionary<string, double?> cRandom = new Dictionary<string, double?>();
                Dictionary<string, double?> cMix = new Dictionary<string, double?>();
                Dictionary<string, double?> cCorrect = new Dictionary<string, double?>();
                Dictionary<string, double> degreeOfFreedom = new Dictionary<string, double>();


                ListFacets lfDepend = new ListFacets();
                ListFacets lfIndepend = new ListFacets();

                string descriptionFile = descripF; // descripción del fichero

                string line = ""; // Para lectura linea a linea del fichero.

                // La siguiente linea debe contener el número de facetas
                int numOfFacets = int.Parse(reader.ReadLine());
                if (numOfFacets < 2 || numOfFacets > 8)
                {
                    throw new SSqPY_Exception("Error en el formato del fichero");
                }
                else
                {
                    for (int i = 0; i < numOfFacets; i++)
                    {
                        string name = reader.ReadLine();
                        string design = "[" + name + "]";
                        if (IsNumeric(name))
                        {
                            name = reader.ReadLine();
                        }

                        if (name.Contains(CHAR_NEST))
                        {
                            char[] delimeterChars = { CHAR_NEST }; // nuestro delimitador será el caracter '/'
                            string[] arrayOfDouble = name.Trim().Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
                            name = arrayOfDouble[0];
                            char[] charArray = arrayOfDouble[1].ToCharArray();
                            int n = arrayOfDouble[1].Length;
                            design = "[" + name + "]:";
                            for (int j = 0; j < n; j++)
                            {
                                char c = charArray[j];
                                design = design + "[" + c + "]";
                            }
                        }
                        int level = int.Parse(reader.ReadLine());
                        int levelProces = int.Parse(reader.ReadLine());
                        string comment = "";
                        Facet f = new Facet(name, level, comment, design);
                        lf.Add(f);
                    }
                    // saltamos las lineas siquientes hasta encontrar la marca "N"
                    line = reader.ReadLine();
                    while (!line.Equals("N"))
                    {
                        line = reader.ReadLine();
                    }
                    for (int i = 0; i < numOfFacets; i++)
                    {
                        Facet f_aux = lf.FacetInPos(i);
                        int sizeOfUnivers = int.Parse(reader.ReadLine());
                        if (sizeOfUnivers > 0)
                        {
                            f_aux.SizeOfUniverse(sizeOfUnivers);
                        }
                        int pos = int.Parse(reader.ReadLine());
                        switch (pos)
                        {
                            case (1):
                                // la faceta pertenece a la lista de facetas dependientes
                                lfDepend.Add(f_aux);
                                break;
                            case (2):
                                // la faceta pertenece a la lista de facetas independientes
                                lfIndepend.Add(f_aux);
                                break;
                            default:
                                throw new RsaSsqPY_Exception("Error al leer el archivo");
                            // break;
                        }
                    }
                    line = reader.ReadLine(); // número de observaciones
                    line = reader.ReadLine(); // gran media

                    // ahora leemos las sumas de cuadrados
                    // List<string> llf = SSqPY.CombSinRepPY(lf);
                    List<string> llf = lf.CombinationStringWithoutRepetition();

                    int numOfListFacets = llf.Count;

                    bool correct = true;

                    for (int i = 0; i < numOfListFacets && correct; i++)
                    {
                        // verificamos que es la linea correcta
                        line = reader.ReadLine();
                        while(line.Trim().Equals("/"))
                        {
                            /* En los ficheros que contienen anidamientos apareceran lineas
                             * que solo contienen el caracter '/'
                             */
                            line = reader.ReadLine();
                        }

                        string key_design = NormalizateDesign(line);

                        correct = ContainsDesign(llf, key_design);
                        if (correct)
                        {
                            line = reader.ReadLine(); // leemos la suma de cuadrados
                            double d = StringLinePY_To_Double(line);// double.Parse(line, NumberFormatInfo.InvariantInfo);
                            ssq.Add(key_design, d);
                            line = reader.ReadLine(); // leemos el grado de libertad
                            int df = int.Parse(line);
                            degreeOfFreedom.Add(key_design, df);
                            line = reader.ReadLine(); // leemos la componenete aleatoria
                            double compRandom = StringLinePY_To_Double(line);// double.Parse(line, NumberFormatInfo.InvariantInfo);
                            cRandom.Add(key_design, compRandom);
                            line = reader.ReadLine(); // leemos la componente mixtta
                            double compMix = StringLinePY_To_Double(line);// double.Parse(line, NumberFormatInfo.InvariantInfo);
                            cMix.Add(key_design, compMix);
                            line = reader.ReadLine(); // leemos la componente corregida
                            double compCorrect = StringLinePY_To_Double(line);// double.Parse(line, NumberFormatInfo.InvariantInfo);
                            cCorrect.Add(key_design, compCorrect);
                        }
                    }

                    // SSqPY ssqOfFileAux = new SSqPY(descriptionFile, lf, ssq, lfDepend, lfIndepend);
                    SSqPY ssqOfFileAux = new SSqPY(descriptionFile, lf, lfDepend, lfIndepend,
                            ssq, degreeOfFreedom, cRandom, cMix, cCorrect);

                    // Ahora tenemos que leer el número de niveles de optimización que contiene el archivo
                    // "D :" es la marca con la que comienza el resumen de resultados
                    while (!(line = reader.ReadLine()).Contains("D :") && (line != null))
                    {
                        // line = reader.ReadLine();
                    }

                    // Saltamos las tres lineas siguientes
                    line = reader.ReadLine();
                    line = reader.ReadLine();
                    line = reader.ReadLine();

                    // la siguiente linea contiene el número de niveles de optimización
                    int numOfNewLevels = int.Parse(reader.ReadLine());

                    //List<G_Parameters> listG_Parameters = new List<G_Parameters>();
                    retVal = new RsaSsqPY(ssqOfFileAux);

                    for (int i = 0; i < numOfNewLevels; i++)
                    {
                        ListFacets lfLevelOpt = new ListFacets();
                        for (int j = 0; j < numOfFacets; j++)
                        {
                            Facet f_orig = lf.FacetInPos(j);
                            string f_mod_Name = f_orig.Name();
                            int f_mod_level = int.Parse(reader.ReadLine()); // leemos el nuevo nivel
                            int f_mod_sizeofUniverse = f_orig.SizeOfUniverse();
                            string f_mod_comment = f_orig.Comment();
                            Facet f_mod = new Facet(f_mod_Name, f_mod_level, f_mod_comment, f_mod_sizeofUniverse);
                            lfLevelOpt.Add(f_mod);
                        }

                        lfDepend = lfLevelOpt.ReplaceLevels(lfDepend);
                        lfIndepend = lfLevelOpt.ReplaceLevels(lfIndepend);

                        // SSqPY newSSqPY = new SSqPY(descriptionFile, lf, ssq, lfDepend, lfIndepend);
                        SSqPY newSSqPY = new SSqPY(descriptionFile, lfLevelOpt, ssq, lfDepend, lfIndepend);

                        // saltamos la linea con los niveles de observacion
                        line = reader.ReadLine();

                        // leemos los gparametros (los que formaran el resumen)
                        line = reader.ReadLine(); // leemos el coeficiente g absoluto
                        double coefG_Abs = StringLinePY_To_Double(line);
                        line = reader.ReadLine(); // coeficiente g relativo
                        double coefG_Rel = StringLinePY_To_Double(line);
                        line = reader.ReadLine(); // varianza del error absoluto
                        double totalAbsErrorVar = StringLinePY_To_Double(line);
                        line = reader.ReadLine(); // varianza del error relativa
                        double totalRelErrorVar = StringLinePY_To_Double(line);
                        line = reader.ReadLine(); // desviación estandar relativa
                        double errorRelStandDev = StringLinePY_To_Double(line);
                        line = reader.ReadLine(); // desviación estandar absoluta
                        double errorAbsStandDev = StringLinePY_To_Double(line);

                        // ahora creamos el nuevo G_parameter con la lista de facetas
                        TableG_Study_Percent newG_P = new TableG_Study_Percent(lfDepend, lfIndepend, coefG_Rel, coefG_Abs,
                            totalRelErrorVar, totalAbsErrorVar, errorRelStandDev, errorAbsStandDev);

                        // añadimos los elementos
                        retVal.Add(newSSqPY, newG_P);

                    } // end for

                }// end if
            }

            return retVal;

        }// end private RsaSsqPY ReadFileGT_RsaSsqPY_Type1


        /* Descrpción
         *  Devuelve un objeto RsaSsqPY extraido del fichero Lectura de un fichero de análisis tipo 1
         * Parámetros:
         *      TextReader textRd: Fragmento de texto que queremos leer.
         */
        private static RsaSsqPY ReadFileGT_RsaSsqPY_Type2(TextReader textRd, string descripF)
        {
            RsaSsqPY retVal = null;
            using (TextReader reader = textRd)
            {
                ListFacets lf = new ListFacets();
                Dictionary<string, double?> ssq = new Dictionary<string, double?>();
                Dictionary<string, double?> cRandom = new Dictionary<string, double?>();
                Dictionary<string, double?> cMix = new Dictionary<string, double?>();
                Dictionary<string, double?> cCorrect = new Dictionary<string, double?>();
                Dictionary<string, double> degreeOfFreedom = new Dictionary<string, double>();


                ListFacets lfDepend = new ListFacets();
                ListFacets lfIndepend = new ListFacets();

                string descriptionFile = descripF; // descripción del fichero

                string line = ""; // Para lectura linea a linea del fichero.

                // La siguiente linea debe contener el número de facetas
                int numOfFacets = int.Parse(reader.ReadLine());
                if (numOfFacets < 2 || numOfFacets > 8)
                {
                    throw new SSqPY_Exception("Error en el formato del fichero");
                }
                else
                {
                    for (int i = 0; i < numOfFacets; i++)
                    {
                        string name = reader.ReadLine();
                        if (IsNumeric(name))
                        {
                            name = reader.ReadLine();
                        }
                        int level = int.Parse(reader.ReadLine());
                        int levelProces = int.Parse(reader.ReadLine());
                        string comment = "";
                        Facet f = new Facet(name, level, comment);
                        lf.Add(f);
                    }
                    // saltamos las lineas siquientes hasta encontrar la marca "N"
                    line = reader.ReadLine();
                    while (!line.Equals("N"))
                    {
                        line = reader.ReadLine();
                    }
                    for (int i = 0; i < numOfFacets; i++)
                    {
                        Facet f_aux = lf.FacetInPos(i);
                        int sizeOfUnivers = int.Parse(reader.ReadLine());
                        if (sizeOfUnivers > 0)
                        {
                            f_aux.SizeOfUniverse(sizeOfUnivers);
                        }
                        int pos = int.Parse(reader.ReadLine());
                        switch (pos)
                        {
                            case (1):
                                // la faceta pertenece a la lista de facetas dependientes
                                lfDepend.Add(f_aux);
                                break;
                            case (2):
                                // la faceta pertenece a la lista de facetas independientes
                                lfIndepend.Add(f_aux);
                                break;
                            default:
                                throw new RsaSsqPY_Exception("Error al leer el archivo");
                            // break;
                        }
                    }
                    line = reader.ReadLine(); // número de observaciones
                    //line = reader.ReadLine(); // gran media
                    

                    // ahora leemos las sumas de cuadrados
                    List<string> llf = SSqPY.CombSinRepPY(lf);

                    int numOfListFacets = llf.Count;

                    bool correct = true;

                    for (int i = 0; i < numOfListFacets && correct; i++)
                    {
                        // verificamos que es la linea correcta
                        line = reader.ReadLine();
                        if (IsNumeric(line))
                        {
                            line = reader.ReadLine(); // gran media

                        }
                        else
                        {
                            /* Es una etiqueta de la lista de facetas*/
                            int st_l = line.Length;
                            string ss = "";
                            for (int j = 0; j < st_l; j++)
                            {
                                ss = ss + "[" + line[j] +"]";
                            }
                            line = ss;
                        }
                        correct = line.Equals(llf[i]);
                        if (correct)
                        {
                            line = reader.ReadLine(); // leemos la suma de cuadrados
                            double d = StringLinePY_To_Double(line);// double.Parse(line, NumberFormatInfo.InvariantInfo);
                            ssq.Add(llf[i], d);
                            line = reader.ReadLine(); // leemos el grado de libertad
                            int df = int.Parse(line);
                            degreeOfFreedom.Add(llf[i], df);
                            line = reader.ReadLine(); // leemos la componenete aleatoria
                            double compRandom = StringLinePY_To_Double(line);// double.Parse(line, NumberFormatInfo.InvariantInfo);
                            cRandom.Add(llf[i], compRandom);
                            line = reader.ReadLine(); // leemos la componente mixtta
                            double compMix = StringLinePY_To_Double(line);// double.Parse(line, NumberFormatInfo.InvariantInfo);
                            cMix.Add(llf[i], compMix);
                            line = reader.ReadLine(); // leemos la componente corregida
                            double compCorrect = StringLinePY_To_Double(line);// double.Parse(line, NumberFormatInfo.InvariantInfo);
                            cCorrect.Add(llf[i], compCorrect);
                        }
                    }
                    // SSqPY ssqOfFileAux = new SSqPY(descriptionFile, lf, ssq, lfDepend, lfIndepend);
                    SSqPY ssqOfFileAux = new SSqPY(descriptionFile, lf, lfDepend, lfIndepend,
                            ssq, degreeOfFreedom, cRandom, cMix, cCorrect);

                    // Ahora tenemos que leer el número de niveles de optimización que contiene el archivo
                    // "D :" es la marca con la que comienza el resumen de resultados
                    while (!(line = reader.ReadLine()).Contains("D :"))
                    {
                        line = reader.ReadLine();
                    }
                    // Saltamos las tres lineas siguientes
                    line = reader.ReadLine();
                    line = reader.ReadLine();
                    line = reader.ReadLine();

                    // la siguiente linea contiene el número de niveles de optimización
                    int numOfNewLevels = int.Parse(reader.ReadLine());

                    //List<G_Parameters> listG_Parameters = new List<G_Parameters>();
                    retVal = new RsaSsqPY(ssqOfFileAux);

                    for (int i = 0; i < numOfNewLevels; i++)
                    {
                        ListFacets lfLevelOpt = new ListFacets();
                        for (int j = 0; j < numOfFacets; j++)
                        {
                            Facet f_orig = lf.FacetInPos(j);
                            string f_mod_Name = f_orig.Name();
                            int f_mod_level = int.Parse(reader.ReadLine()); // leemos el nuevo nivel
                            int f_mod_sizeofUniverse = f_orig.SizeOfUniverse();
                            string f_mod_comment = f_orig.Comment();
                            Facet f_mod = new Facet(f_mod_Name, f_mod_level, f_mod_comment, f_mod_sizeofUniverse);
                            lfLevelOpt.Add(f_mod);
                        }

                        lfDepend = lfLevelOpt.ReplaceLevels(lfDepend);
                        lfIndepend = lfLevelOpt.ReplaceLevels(lfIndepend);

                        // SSqPY newSSqPY = new SSqPY(descriptionFile, lf, ssq, lfDepend, lfIndepend);
                        SSqPY newSSqPY = new SSqPY(descriptionFile, lfLevelOpt, ssq, lfDepend, lfIndepend);

                        // saltamos la linea con los niveles de observacion
                        line = reader.ReadLine();

                        // leemos los gparametros (los que formaran el resumen)
                        line = reader.ReadLine(); // leemos el coeficiente g absoluto
                        double coefG_Abs = StringLinePY_To_Double(line);
                        line = reader.ReadLine(); // coeficiente g relativo
                        double coefG_Rel = StringLinePY_To_Double(line);
                        line = reader.ReadLine(); // varianza del error absoluto
                        double totalAbsErrorVar = StringLinePY_To_Double(line);
                        line = reader.ReadLine(); // varianza del error relativa
                        double totalRelErrorVar = StringLinePY_To_Double(line);
                        line = reader.ReadLine(); // desviación estandar relativa
                        double errorRelStandDev = StringLinePY_To_Double(line);
                        line = reader.ReadLine(); // desviación estandar absoluta
                        double errorAbsStandDev = StringLinePY_To_Double(line);

                        // ahora creamos el nuevo G_parameter con la lista de facetas
                        TableG_Study_Percent newG_P = new TableG_Study_Percent(lfDepend, lfIndepend, coefG_Rel, coefG_Abs,
                            totalRelErrorVar, totalAbsErrorVar, errorRelStandDev, errorAbsStandDev);

                        // añadimos los elementos
                        retVal.Add(newSSqPY, newG_P);

                    } // end for

                }// end if
            }
            return retVal;
        }// end private RsaSsqPY ReadFileGT_RsaSsqPY_Type2


        /* Descripción:
         *  Normaliza un diseño empleando los corchetes y sustituyendo el caracter de anidamiento.
         * Parámetros:
         *      string design: El string contiene el diseño que deseamos normalizar. 
         */
        private static string NormalizateDesign(string design)
        {
            int l = design.Length;
            char[] arrayChar = design.ToCharArray();

            string retVal = ""; // valor de retorno
            for (int i = 0; i < l; i++)
            {
                char c = arrayChar[i];
                if (c.Equals(CHAR_NEST))
                {
                    retVal = retVal + Facet.NEST_CHAR;
                }
                else
                {
                    retVal = retVal + "[" + c + "]";
                }
            }
            return retVal;
        }


        /*
         * Descripción:
         *  Devuelve la extensión de un fichero, en el caso de que el fichero no tenga 
         *  extensión devuelve la cadena vacía.
         * Parámetros:
         *      String fileName: Nombre del fichero.
         */
        private static string fileExtension(string path)
        {
            string retVal = "";
            int pos = path.LastIndexOf(".") + 1; // le sumamos uno para obtener la posición 
            // siguiente a la de la barra invertida
            if (pos > (-1))
            {
                // hemos encontrado la posición
                retVal = path.Substring(pos);
            }
            return retVal;
        }

        /*
         * Descripción:
         *  Devuelve un string con los nombres de la lista de facetas.
         */
        private static string StringOfListFactes(ListFacets lf)
        {
            int num = lf.Count();
            string stringListOfFacets = ""; // valor de retorno
            for (int j = 0; j < num; j++)
            {
                stringListOfFacets = stringListOfFacets + lf.FacetInPos(j).Name();

            }
            return stringListOfFacets;
        }

        
        /* Descripción:
         *  Devuelve true si la cadena que se pasa como argumento puede convertirse en un número.
         */
        public static bool IsNumeric(object st)
        {
            bool isNumber;
            double isItNumeric;
            isNumber = Double.TryParse(Convert.ToString(st), System.Globalization.NumberStyles.Any,System.Globalization.NumberFormatInfo.InvariantInfo, out isItNumeric );
            return isNumber;
        }



        /* Descripción:
         *  Devuelve un double, en el caso de que el doble este representado como .xxx pasará a ser:
         *  0.xxx y en el caso de que sea -.xxx pasará a ser -0.xxx.
         */
        private static  double StringLinePY_To_Double(string line)
        {
            if (line.Contains("."))
            {
                if (line.StartsWith("-"))
                {
                    // es un numero negativo
                    line = line.Insert(1, "0");
                }
                else
                {
                    line = "0" + line.Trim();
                }
            }
            return(double.Parse(line, NumberFormatInfo.InvariantInfo));
        }


        private static bool ContainsDesign(List<string> l_design, string design)
        {
            bool found = false;
            int n = l_design.Count;
            for (int i = 0; i < n && !found; i++)
            {
                found = design.Equals(l_design[i]);
            }
            return found;
        }

    }// end class
}// end namespace SsqPY
