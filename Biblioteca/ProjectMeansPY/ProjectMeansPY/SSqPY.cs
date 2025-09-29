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
 * Fecha de revisión: 11/Mar/2011                           
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
    public class SSqPY: TableAnalysisOfVariance
    {
        // Variables

        string descriptionFile; // descrpción del fichero
        // lista de facetas dependientes
        ListFacets lfDepend;
        // lista de facetas independientes
        ListFacets lfIndepend;
        // G_Parameters gParametersPY;


        // Constructores
        public SSqPY()
            : base()
        {
            lfDepend = new ListFacets();
            lfIndepend = new ListFacets();
        }
        /*
        public SSqPY(String nameFile)
            : this()
        {
            this.ReadFileRsmPY(nameFile);
        }
        */
        public SSqPY(string descriptionFile, ListFacets lf, Dictionary<string, double?> ssq, ListFacets lfDepend, ListFacets lfIndepend)
            : base(lf, ssq)
        {
            this.descriptionFile = descriptionFile;
            this.lfDepend = lfDepend;
            this.lfIndepend = lfIndepend;
        }

        public SSqPY(string descriptionFile, ListFacets lf, ListFacets lfDepend, ListFacets lfIndepend,
            Dictionary<string, double?> ssq, Dictionary<string, double> d_df,
            Dictionary<string, double?> rcomp, Dictionary<string, double?> mixcomp,
            Dictionary<string, double?> ccomp)
            :base(lf, ssq, d_df, rcomp, mixcomp, ccomp)
        {
            this.descriptionFile = descriptionFile;
            this.lfDepend = lfDepend;
            this.lfIndepend = lfIndepend;
        }
        

        /* Descripción:
         *  Lee un fichero .ssq (fichero de suma de cuadrados, GT E 2.0, 1996) para obtener el objeto
         *  que contiene la suma de cuadrados
         */
        public static SSqPY ReadFileSsqPY(String nameFile)
        {
            // ListTableSSQ lSSQ = new ListTableSSQ(); // objeto que vamos a devolver
            SSqPY ssqPY = new SSqPY();

            using (TextReader reader = new StreamReader(nameFile))
            {
                try
                {
                    ListFacets lf = new ListFacets();
                    Dictionary<string, double?> ssq = new Dictionary<string, double?>();
                    ListFacets lfDepend = new ListFacets();
                    ListFacets lfIndepend = new ListFacets();

                    string descriptionFile = reader.ReadLine(); // descripción del fichero
                    string line = reader.ReadLine(); // segunda linea
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
                            int level = int.Parse(reader.ReadLine());
                            int levelProces = int.Parse(reader.ReadLine());
                            string comment = "";
                            Facet f = new Facet(name,level,comment);
                            lf.Add(f);
                        }
                        // Ahora debemos saltarnos las lineas siguientes hasta encontra la marca N
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
                                    throw new SSqPY_Exception("Error al leer el archivo");
                                    // break;
                            }
                        }
                        // ahora leemos las sumas de cuadrados
                        List<string> llf = CombSinRepPY(lf);

                        int numOfListFacets = llf.Count;
                        
                        // Dictionary<ListFacets, int> degreeOfFreedom = new Dictionary<ListFacets, int>();

                        for (int i = 0; i < numOfListFacets; i++)
                        {
                            line = reader.ReadLine(); // leemos la suma de cuadrados
                            double d = double.Parse(line, NumberFormatInfo.InvariantInfo);
                            ssq.Add(llf[i],d);
                            line = reader.ReadLine(); // leemos el grado de libertad
                            int df = int.Parse(line);
                            // degreeOfFreedom.Add(llf[i], df);
                        }
                    }
                    ssqPY = new SSqPY(descriptionFile, lf, ssq, lfDepend, lfIndepend);
                }
                catch (FormatException)
                {
                    throw new SSqPY_Exception("Error en el formato del fichero");
                }
                return ssqPY;
            }// end using
        } // end public void ReadFileRsmPY(String nameFile)

        #region Métodos de consulta
        /* Descripción:
         *  Devuelve un string con la desripción del fichero.
         */
        public string DescriptionFile()
        {
            return descriptionFile;
        }

        /* Descripción:
         *  Devuelve la lista de facetas dependiente (las que estan a la izquierda en el diseño de
         *  medida).
         */
        public ListFacets SourceOfVarDepend()
        {
            return this.lfDepend;
        }

        /* Descripción:
         *  Devuelve la lista de facetas independiente (las que estan a la derecha en el diseño de
         *  medida).
         */
        public ListFacets SourceOfVarInDepend()
        {
            return this.lfIndepend;
        }
        #endregion Métodos de consulta

        #region Combinación sin repetición de lista de facetas en el mismo orden que el programa GT E 2.0
        /* Descripción:
         *  Generá una lista de listas de facetas que son combinaciones sin repetición de la lista que
         *  se pasa como parámetro, el orden será el mismo en que se muestran los datos en el programa
         *  GT E 2.0.
         *  
         * Ejemplo:
         *  Para las facetas: [A][B][C] la salida será:
         *  [A], [B], [A][B], [C], [A][C], [B][C], [A][B][C]
         */
        public static List<string> CombSinRepPY(ListFacets lf)
        {
            List<string> ldesign = new List<string>();
            int numOfElem = lf.Count();
            for (int i = 0; i < numOfElem; i++)
            {
                Facet f = lf.FacetInPos(i);
                // ListFacets lf_aux = new ListFacets();
                // lf_aux.Add(f);
                // llf.Add(lf_aux);
                string name = "[" + f.Name() + "]";
                ldesign.Add(name);
                int numOfElem2 = ldesign.Count - 1;
                for (int j = 0; j < numOfElem2; j++)
                {
                    string lf_aux2 = string.Copy(ldesign[j]);
                    lf_aux2 = lf_aux2 + name;
                    ldesign.Add(lf_aux2);
                }
            }
            return ldesign;
        }

        /* Descripción:
         *  Copia una lista de facetas. La copia de facetas es supercial, se mantienen las referncias
         *  a las facetas.
         * Parámetros:
         *      ListFacets lf: Es la lista de facetas que deseamos copiar.
         */
        public static ListFacets CopyListFacets(ListFacets lf)
        {
            ListFacets retVal = new ListFacets();
            int numElems = lf.Count();
            for (int i = 0; i < numElems; i++)
            {
                retVal.Add(lf.FacetInPos(i));
            }
            return retVal;
        }
        #endregion Combinación sin repetición de lista de facetas en el mismo orden que el programa GT E 2.

    }// end public class SSqPY: ListTableSSQ
}// namespace SsqPY
