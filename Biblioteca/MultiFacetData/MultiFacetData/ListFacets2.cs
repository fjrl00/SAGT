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
 * Fecha de revisión: 18/Abr/2012
 * 
 */
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace MultiFacetData
{
    public partial class ListFacets : IEnumerable, IComparable<ListFacets> 
    {

        #region Operaciones con listas de facetas
        /**************************************************************************************************
         * Operaciones con listas de facetas
         * - Concatenate (Concatena dos listas en una nueva lista)
         * - ConcatenateWithoutRepetitions (concatena dos listas omitiendo los repetidos)
         * - DegreeOfFreedom (Devuelve el grado de libertad)
         * - MultiOfLevel (Producto de los niveles)
         * - SourcesOfVariabilityAbsent (Lista de las facetas ausentes)
         * - MultipSourcesOfVariabilityAbsent (Producto de la lista de facetas ausentes)
         * - MultipSourcesOfVariabilityAbsentForTypeOfFacet (Producto de la lista de facetas ausentes,
         *              depende del tipo de facetas)
         * - SumLevelOfListFacets (Suma de niveles de la lista de facetas)
         * - Contains (True si la faceta esta contenida en la lista)
         * - ContainsList (True si todas las facetas de la lista estan contenidas)
         * - ReplaceLevels (Remplaza los niveles de las facetas)
         * - RetPosInListFacets (Devuelve la posición de la faceta en la lista)
         * - MultSizeOfUniverse (Producto de los universos)
         * - HasAllFacetsSizeInfinite (True si todas las facetas tienen universo infinito)
         * - HasAllFacetsFinite (true si todas las facetas son finitas)
         * - HasAllFacetsFixed (true si todas las facetas son fijas)
         * - HasSkipLevels (true si tiene alguna faceta con algún nivel omitido)
         * - AtLeastOneIsFixed (true si almenos una es fija)
         * - AtLeastOneIsFixed (comprueba que los universos sean iguales)
         * - HopeOfVariance (devuelve el producto (n-1)/n siendo n el nivel de la faceta)
         * - StringOfListFactes (string que concatena el nombre de las facetas entre corchetes. Ej: [A][B] )
         * - SortByListFacets (ordena las facetas según sun nombre en función del orden que tenga en otra lista)
         * - ListDesignFacets (A partir de un string de diseños devueve una lista de listas de facetas)
         * - ListFacetWithoutOmit
         * - ReplaceNameFacet (Remplaza el nombre de una faceta)
         **************************************************************************************************/

        /* Descripción:
         *  Devuelve un nueva lista como resultado de concatenar la lista implicita y la explicita. Primero
         *  introduce la lista implicita y luego la explicita.
         *  
         * Excepciones:
         *  ListFacetException: si existe alguna faceta de la lista implicita que se encuentra en la lista
         *  explicita.
         */
        public ListFacets Concatenate(ListFacets lf)
        {
            ListFacets totalFacets = new ListFacets();
            int n = this.Count();
            for (int i = 0; i < n; i++)
            {
                totalFacets.Add(this.FacetInPos(i));
            }

            n = lf.Count();
            for (int i = 0; i < n; i++)
            {
                totalFacets.Add(lf.FacetInPos(i));
            }

            return totalFacets;
        }


        /* Descripción:
         *  Devuelve un nueva lista como resultado de concatenar la lista implicita y la explicita. Primero
         *  introduce la lista implicita y luego la explicita. No saltan excepciones si existen facetas
         *  en la lista implicita que se encuentra en la explicita ya que cada faceta se asigna una vez.
         */
        public ListFacets ConcatenateWithoutRepetitions(ListFacets lf)
        {
            ListFacets totalFacets = new ListFacets();
            int n = this.Count();
            for (int i = 0; i < n; i++)
            {
                totalFacets.Add(this.FacetInPos(i));
            }

            n = lf.Count();
            for (int i = 0; i < n; i++)
            {
                Facet f = lf.FacetInPos(i);
                if (!totalFacets.Contains(f))
                {
                    totalFacets.Add(f);
                }
            }

            return totalFacets;
        }


        /*
         * Descripción:
         *  Devuelve (int) el grado de libertad de una lista de facetas. Si una faceta no esta anidada,
         * Excepciones:
         *      ListFacetsException: Si la lista de facetas esta vacia.
         */
        //public int DegreeOfFreedom()
        //{
        //    if (this.Count() == 0)
        //    {
        //        throw new ListFacetsException("Error: No se puede calcular el grado de libertad a una lista vacia");
        //    }
        //    int retVal = 1;
        //    foreach (Facet f in this.listFacets)
        //    {
        //        if (f.IsNesting())
        //        {
        //            retVal = retVal * f.Level();
        //        }
        //        else
        //        {
        //            retVal = retVal * (f.Level() - 1);
        //        }
                
        //    }
        //    return retVal;
        //}


        /*
        * Descripción:
        *  Devuelve (int) el grado de libertad de una lista de facetas. Si una faceta no esta anidada,
        * Excepciones:
        *      ListFacetsException: Si la lista de facetas esta vacia.
        */
        public int DegreeOfFreedom(string design)
        {
            if (this.Count() == 0)
            {
                throw new ListFacetsException("Error: No se puede calcular el grado de libertad a una lista vacia");
            }
            int retVal = 1;

            char[] delimeterChars = { '[' , ']'}; // nuestro delimitador serán los corchetes
            string[] arrayOfstring = design.Trim().Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
            int num = arrayOfstring.Length;

            bool  nest_char = false;

            for (int i = 0; i < num; i++)
            {
                if (!nest_char)
                {
                    nest_char = arrayOfstring[i].Equals(Facet.NEST_CHAR);
                }
                
                if (!arrayOfstring[i].Equals(Facet.NEST_CHAR))
                {
                    Facet f = this.LookingFacet(arrayOfstring[i]);
                    if (nest_char)
                    {
                        retVal = retVal * f.Level();
                    }
                    else
                    {
                        retVal = retVal * (f.Level()-1);
                    }
                }
            }

            return retVal;
        }

        
        /* Descripcíón:
         *  Devuelve true si la faceta que se pasa como parámetro no contiene otra faceta en su interior. 
         *  Es decir, no existe ninguna faceta en la lista que la referencie desde la lista de facetas
         *  anidadas. Si la faceta no contiene niguna otra devolvera false,
         */
        /*
        private bool DoesNotContainToYetAnotherFacet(Facet f)
        {
            bool retVal = false;
            int n = this.Count();
            for (int i = 0; i < n && !retVal; i++)
            {
                Facet f_pos_i = this.FacetInPos(i);
                retVal = f_pos_i.ListFacetsNesting().ExistNameFacet(f);
            }
            return retVal;
        }
         */


        /*
         * Descripción:
         *  Contiene la multiplicación de los niveles de la lista de facetas.
         */
        public double MultiOfLevel()
        {
            double retVal = 1; // valor de retorno
            foreach (Facet f in this.listFacets)
            {
                retVal *= f.Level();
            }
            return retVal;
        }


        /* Descripción: 
         *  Devuelve una copia de la lista de facetas como parámetro implicito a la que le hemos
         *  sustraido las facetas que aparecen como parametro explicito.
         * Parámetros:
         *      ListFacets lf: es la lista de las facetas que vamos a sustraer.
         */
        public ListFacets SourcesOfVariabilityAbsent(ListFacets lf)
        {
            ListFacets aux = clonaListaF(this);
            foreach (Facet f in lf)
            {
                aux.Remove(f);
            }
            return aux;
        }


        /*
         * Descripción:
         *  Devuelve el producto de los nivel de las fuentes de variación (facetas) "ausentes". Si
         *  al eliminar todas las facetas presentes en lista de facetas que se pasa como parámetro
         *  obtenemos la lista vacia entonces devolveremos 1 como resultado de la función. Para
         *  el cálculo no influye el tipo de universo.
         * Parámetros:
         *      ListFacets lf: es la lista de las facetas que vamos a sustraer
         */
        public double MultipSourcesOfVariabilityAbsent(ListFacets lf)
        {
            ListFacets aux = clonaListaF(this);
            foreach (Facet f in lf)
            {
                aux.Remove(f);
            }
            double retVal = 1; // valor de retorno
            /* Se le inicializa valor uno para el caso en que la lista que se pasa como parámetro
             * contenga las misma facetas que la lista del parámetro implícito */
            if (aux.Count() != 0)
            {
                retVal = aux.MultiOfLevel();
            }
            return retVal;
        }


        /*
         * Descripción:
         *  Devuelve el producto de los nivel de las fuentes de variación (facetas) "ausentes". Si
         *  al eliminar todas las facetas presentes en lista de facetas que se pasa como parámetro
         *  obtenemos la lista vacia entonces devolveremos 1 como resultado de la función. Dependiendo
         *  del tipo de faceta (infinitas, finitas o fijas) el producto entre facetas será distinto.
         * Parámetros:
         *      ListFacets lf: es la lista de las facetas que vamos a sustraer.
         */
        public double MultipSourcesOfVariabilityAbsentForTypeOfFacet(ListFacets lf)
        {
            ListFacets aux = this.SourcesOfVariabilityAbsent(lf);

            double retVal = 1; // valor de retorno
            /* Se le inicializa valor uno para el caso en que la lista que se pasa como parametro
             * contenga las misma facetas que la lista del parámetro implicito */
            int n = aux.Count();
            if (n != 0)
            {
                for (int i = 0; i < n; i++)
                {
                    Facet f = aux.FacetInPos(i);
                    if (f.IsInfinite())
                    {
                        retVal = retVal * (1 / (double)f.Level());
                    }
                    else if (f.IsRamdonFinite())
                    {
                        double l = f.Level();
                        double u = f.SizeOfUniverse();
                        retVal = retVal * (1 / l) * ((u - l) / (u - 1));
                    }
                    else if (f.IsFixed())
                    {
                        retVal = 0;
                    }
                }
            }
            return retVal;
        }


        /*
         * Descripción:
         *  Devuelve la suma los niveles contenidos en una lista de facetas.
         */
        public int SumLevelOfListFacets()
        {
            int retVal = 0;
            int numfacet = this.Count();
            for (int i = 0; i < numfacet; i++)
            {
                retVal = retVal + this.FacetInPos(i).Level();
            }
            return retVal;
        }


        /*
         * Descripción:
         *  Devuelve true si la faceta se encuentra en la lista, false en caso contrario.
         * Parámetros:
         *      Facet f: faceta que queremos comprobar si pertenece o no a la lista
         */
        public bool Contains(Facet f)
        {
            return this.listFacets.Contains(f);
        }


        /*
         * Descripción:
         *  Devuelve true si la lista de facetas que se pasa como parámetro tiene todas sus facetas
         *  contenidas en la lista de facetas que se pasa como parámetro implicito. False en caso de
         *  que alguna de sus facetas no este contenida.
         * Parámetros:
         *      ListFacets lf: Lista de facetas que queremos comprobar.
         */
        public bool ContainsList(ListFacets lf)
        {
            bool retVal = true; // valor de retorno
            int lg = lf.Count();
            for (int i = 0; i < lg && retVal; i++)
            {
                retVal = this.listFacets.Contains(lf.FacetInPos(i));
            }
            return retVal;
        }


        /* Descripción:
         *  Devuelve una lista a la que se le han sustituido los niveles de aquellas facetas que  
         *  presentes en la lista de parametros implicitos. Los niveles antiguios son sustituidos por 
         *  los por los niveles de las facetas homonimas.
         *
         * Parámetros:
         *      ListFacets lf: es la lista de faceta que tomaremos como base, devolveremos una copia
         *              de esta con los nuevos niveles.
         */
        public ListFacets ReplaceLevels(ListFacets lf)
        {
            ListFacets retList = new ListFacets();
            int numFacets = lf.Count();

            for (int i = 0; i < numFacets; i++)
            {
                Facet f1 = lf.FacetInPos(i);
                int pos = this.IndexOf(f1);
                if (pos == -1)
                {
                    throw new ListFacetsException("Error La facena no se encuentra en la cadena");
                }
                Facet f2 = this.FacetInPos(pos);
                Facet newFacet = new Facet(f1.Name(), f2.Level(), f1.Comment(), f1.SizeOfUniverse());
                retList.Add(newFacet);
            }
            return retList;
        }


        /* Descripción:
         *  Devuelve el producto de los universo de una lista. Devuelve cero si existe alguna faceta
         *  con tamaño del universo igual a infinito
         */
        public double MultSizeOfUniverse()
        {
            double retVal = 1;

            bool isInfinite = false;
            int n = this.Count();

            for (int i = 0; i < n && !isInfinite; i++)
            {
                Facet f = this.listFacets[i];
                if (isInfinite = f.IsInfinite())
                {
                    retVal = 0;
                }
                else
                {
                    retVal = retVal * f.SizeOfUniverse();
                }
            }

            return retVal;
        }



        /* Descripción:
         *  Devuelve true si el tamaño del universo de todas las facetas es aleatorio infinito. En otro
         *  caso devuelve false.
         */
        public bool HasAllFacetsSizeInfinite()
        {
            bool isInfinite = true;
            int n = this.Count();
            for (int i = 0; i < n && isInfinite; i++)
            {
                Facet f = this.listFacets[i];
                isInfinite = f.IsInfinite();
            }
            return isInfinite;
        }


        /* Descripción:
         *  Devuelve true si entre la lista de facetas hay al menos una faceta con algún nivel omitido
         */ 
        public bool HasSkipLevels()
        {
            bool hasSkip = false;
            int numFacet = this.Count();
            for (int i = 0; i < numFacet && !hasSkip; i++)
            {
                Facet f = this.FacetInPos(i);
                hasSkip = f.HasSkipLevels();
            }
            return hasSkip;
        }


        /* Descripción:
         *  Devuelve true si todas las facetas son "aleatorio finitas".
         */
        public bool HasAllFacetsFinite()
        {
            bool isFinite = true;
            int n = this.Count();
            for (int i = 0; i < n && isFinite; i++)
            {
                Facet f = this.listFacets[i];
                isFinite = f.IsRamdonFinite();
            }
            return isFinite;
        }


        /* Descripción:
         *  Devuelve true si todas las facetas son "fijas".
         */
        public bool HasAllFacetsFixed()
        {
            bool isFixed = true;
            int n = this.Count();
            for (int i = 0; i < n && isFixed; i++)
            {
                Facet f = this.listFacets[i];
                isFixed = f.Level().Equals(f.SizeOfUniverse());
            }
            return isFixed;
        }


        /* Descripción:
         *  Devuelve true si al menos una faceta es fija, false en otro caso.
         */
        public bool AtLeastOneIsFixed()
        {
            bool retval = false;
            int n = this.Count();
            for (int i = 0; i < n && !retval; i++)
            {
                retval = this.FacetInPos(i).IsFixed();
            }
            return retval;
        }


        /* Descripción:
         *  Devuelve true si los universos de la lista implicita y explicita coinciden
         */
        public bool EqualsSizeOfUniverse(ListFacets lf)
        {
            int s1 = this.Count();
            int s2 = lf.Count();
            bool retval = (s1 == s2);
            for (int i = 0; i < s1 && retval; i++)
            {
                Facet f1 = this.FacetInPos(i);
                Facet f2 = lf.FacetInPos(lf.IndexOf(f1));
                retval = f1.SizeOfUniverse().Equals(f2.SizeOfUniverse());
            }
            return retval;
        }


        /* Descripción:
         *  Devuelve la esperanza de varianza. El produto de (n-1)/n siendo e 'n' el nivel de las facetas.
         */
        public double HopeOfVariance()
        {
            double d = 1;
            int n = this.Count();
            for (int i = 0; i < n; i++)
            {
                Facet f = this.listFacets[i];
                double level = f.Level();
                d = d * ((level - 1) / level);
            }
            return d;
        }


        /*
         * Descripción:
         *  Devuelve un string con los nombres de la lista de facetas entre corchetes.
         */
        public string StringOfListFactes()
        {
            int num = this.listFacets.Count();
            string stringListOfFacets = ""; // valor de retorno
            for (int j = 0; j < num; j++)
            {
                stringListOfFacets = stringListOfFacets + "[" + this.listFacets[j].Name() + "] ";

            }
            return stringListOfFacets;
        }


        /* Descripción:
         *  Reordena la lista como parámetro explicito en el mismo orden de la lista que se pasa 
         *  como parámetro implicito.
         * Nota: Ambas listas deben contener las mismas facetas (facetas con el mismo nombre).
         */
        public ListFacets SortByListFacets(ListFacets lf)
        {
            if (!this.Equals(lf))
            {
                throw new ListFacetsException("Error: No se puede ordenar porque no son iguales");
            }
            ListFacets retLf = new ListFacets();
            int n = this.Count();
            for (int i = 0; i < n; i++)
            {
                Facet f = this.FacetInPos(i);
                int j = lf.IndexOf(f);
                retLf.Add(lf.FacetInPos(j));
            }
            return retLf;
        }


        /* Descripción: 
         *  Devuelve la lista de facetas que contiene todas las facetas del diseño que se pasa como 
         *  parámetro.
         *  NOTA: No comprueba que todas las facetas del diseño esten contenida en la lista de facetas
         *  (parámetro implicito).
         */
        public ListFacets ListDesignFacets(string lDesignFacets)
        {
            ListFacets listReturn = new ListFacets();

            char[] delimeterChars = { '[', ']', ':'}; // nuestro delimitador serán los corchetes y los dos puntos
            string[] arrayOfNamesFacets = lDesignFacets.Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);

            int n = arrayOfNamesFacets.Length;
            for (int i = 0; i < n; i++)
            {
                string name = arrayOfNamesFacets[i];
                Facet f = this.LookingFacet(name);
                listReturn.Add(f);
            }
            return listReturn;
        }


        /* Descripción:
         *  Devuelve una lista de facetas donde todas las facetas estes sin omitir y donde los diseños
         *  de estas no contienen anidamientos o cruces con facetas omitidas.
         */
        public ListFacets ListFacetWithoutOmit()
        {
            ListFacets retLf = new ListFacets();
            List<string> lsNameFacet = new List<string>();
            
            int n = this.Count();
            for (int i = 0; i < n; i++)
            {
                Facet f = (Facet)this.FacetInPos(i).Clone();
                if (f.Omit() == false)
                {
                    retLf.Add(f);
                }
                else
                {
                    lsNameFacet.Add(f.Name());
                }
            }

            n = lsNameFacet.Count;
            int m = retLf.Count();

            for (int i = 0; i < n; i++)
            {
                string s = lsNameFacet[i];
                for (int j = 0; j < m; j++)
                {
                    Facet f = retLf.FacetInPos(j);
                    f.ListFacetsDesignRemove(s);
                }
                
            }

            return retLf;
        }// end ListFacetWithoutOmit


        /* Descripción:
         *  Replaza el nombre de una faceta por uno nuevo cambiandolo si aparece en el diseño de las 
         *  demás facetas.
         * Parámetros:
         *      string oldName: Nombre que se va ha sustituir.
         *      string newName: Nombre por el que se va a sustituir.
         */
        public ListFacets ReplaceNameFacet(string oldName, string newName)
        {
            ListFacets retListFacets = new ListFacets();
            int numFacet = this.Count();

            for(int i = 0; i < numFacet; i++)
            {
                Facet f = this.FacetInPos(i);
                if (f.Name().ToLower().Equals(oldName.ToLower()))
                {
                    f = (Facet)f.Clone();
                    f.Name(newName);
                }
                else
                {
                    string design = f.ListFacetDesing();
                    design = design.Replace("[" + oldName + "]", "[" + newName + "]");
                    f = new Facet(f.Name(), f.Level(), f.Comment(), f.SizeOfUniverse(), design, f.Omit());
                }
                retListFacets.Add(f);
            }

            return retListFacets;
        }// end ReplaceNameFacet


        /* Descripción:
         *  Devuelve la lista de facetas de sustituir los nombres y los comentarios por la lista de facetas
         *  que se pasa como parámetros en el mismo orden.
         * NOTA: Los nombres se sustituyen los diseños se mantienen.
         * Parámetros:
         *      ListFacets lf: Lista de facetas que contiene los nombres y los comentarios por los que se
         *              va a remplazar.
         */
        public ListFacets RemplaceListFacets(ListFacets lf)
        {
            ListFacets lf_retval = this; // variable de retorno
            if(this.Count() != lf.Count())
            {
                // si el número de facetas no coincide lanzamos una excepción
                throw new ListFacetsException("No se puede aplicar la operación porque no coincide el número de facetas");
            }
            
            int n = lf.Count();
            
            for (int i = 0; i < n; i++)
            {
                Facet f_new_name = lf.FacetInPos(i);
                Facet f_old_name = this.FacetInPos(i);
                lf_retval = lf_retval.ReplaceNameFacet(f_old_name.Name(), f_new_name.Name());
            }

            // Ahora sustituimos los comentarios
            for (int i = 0; i < n; i++)
            {
                lf_retval.FacetInPos(i).Comment(lf.FacetInPos(i).Comment());
            }

            return lf_retval;
        }// end RemplaceListFacets


        #endregion Operaciones con listas de facetas



        #region Combinación sin repetición de lista de facetas ordenadas
        /*
         * Descripción:
         *  Devuelve una lista que contiene las Combinaciones sin repetición de la lista de 
         *  facetas del objeto multifaceta que se pasa como parámetro implicito.
         */
        public List<ListFacets> CombinationWithoutRepetition()
        {
            List<ListFacets> llf = new List<ListFacets>();
            int numOfElem = this.Count();
            for (int i = 0; i < numOfElem; i++)
            {
                Facet f = this.FacetInPos(i);
                ListFacets lf_aux = new ListFacets();
                lf_aux.Add(f);
                llf.Add(lf_aux);
                int numOfElem2 = llf.Count - 1;
                for (int j = 0; j < numOfElem2; j++)
                {
                    ListFacets lf_aux2 = CopyListFacets(llf[j]);
                    lf_aux2.Add(f);
                    llf.Add(lf_aux2);
                }
            }
            llf.Sort();
            return llf;
        }


        #region Lista con las combinaciones de los diseños de las facetas


        /* Descripción:
         *  Devuelve una lista de string donde cada uno representa un diseño posible como combinación
         *  de facetas.
         */
        public List<string> CombinationStringWithoutRepetition()
        {
            List<string> l_string_facets = new List<string>();
            int numOfElem = this.Count();
            for (int i = 0; i < numOfElem; i++)
            {
                Facet f = this.FacetInPos(i);
                string desing = string.Copy(f.ListFacetDesing());
                
                l_string_facets.Add(desing);
                int numOfElem2 = l_string_facets.Count - 1;
                for (int j = 0; j < numOfElem2; j++)
                {
                    string lf_aux2 = string.Copy(l_string_facets[j]);

                    lf_aux2 = CrussedDesing(desing,lf_aux2);
                    if (!string.IsNullOrEmpty(lf_aux2))
                    {
                        l_string_facets.Add(lf_aux2);
                    }
                }
            }
            // l_string_facets.Sort();
            return l_string_facets; 
        }


        /* Descripción:
         *  Cruza dos diseños de facetas y llama a unificar el resto, entendiendo como
         *  resto al diseño de facetas que se encuentra despues del primer caracter ':'.
         */
        private string CrussedDesing(string design1, string design2)
        {
            string retVal = ""; // Valor de retorno cadena vacia;
            string d1_part1 = design1;
            string d1_rest = ""; // Cadena vacia
            /* Compruebo si los diseños son anidados y si la primera parte
             * se puede combinar. Será combinable si no se repiten facetas. */
            if (design1.Contains(Facet.NEST_CHAR))
            {
                // dividimos el string en dos partes
                int pos = design1.IndexOf(Facet.NEST_CHAR);
                d1_part1 = design1.Substring(0, pos);
                int l = design1.Length - pos-1;
                d1_rest = design1.Substring(pos+1, l);
            }

            string d2_part1 = design2;
            string d2_rest = "";// Cadena vacia
            if (design2.Contains(Facet.NEST_CHAR))
            {
                // dividimos el string en dos partes
                int pos = design2.IndexOf(Facet.NEST_CHAR);
                d2_part1 = design2.Substring(0, pos);
                int l = design2.Length - pos-1;
                d2_rest = design2.Substring(pos + 1, l);
            }

            /* Comprobamos que no hay facetas repetidas
             */
            bool notFound = false; // variable booleana falso si no hay facetas repetidas
            char[] delimeterChars = { '[', ']', ':'}; // nuestro delimitador serán los corchetes y los dos puntos
            string[] arrayOfNamesFacets= d1_part1.Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
            int n = arrayOfNamesFacets.Length;
            for (int i = 0; i < n && !notFound; i++)
            {
                string name = "[" + arrayOfNamesFacets[i] + "]";
                notFound = design2.Contains(name);
            }
            arrayOfNamesFacets = d2_part1.Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
            n = arrayOfNamesFacets.Length;
            for (int i = 0; i < n && !notFound; i++)
            {
                string name = "[" + arrayOfNamesFacets[i] + "]";
                notFound = design1.Contains(name);
            }

            /* Si notFound == False entonces no hay facetas repetidas en la primera parte
             * y se pueden unificar. */
            if (!notFound)
            {
                /* Unimos las dos primeras partes más el resto de la primera parte*/
                retVal = d2_part1 + d1_part1;
                // rellenamos con el resto de facetas no incluidas
                retVal = retVal + unificar(d1_rest,d2_rest);

            }
            
            return retVal;
        }// end CrussedDesing


        /* Descripción:
         *  Unifica los restos de dos diseños de facetas;
         */
        private string unificar(string restDesing1, string restDesing2)
        {
            string retVal = "";
            if (string.IsNullOrEmpty(restDesing1))
            {
                if (!string.IsNullOrEmpty(restDesing2))
                {
                    retVal = Facet.NEST_CHAR + restDesing2;
                }
            }
            else if (string.IsNullOrEmpty(restDesing2))
            {
                if (!string.IsNullOrEmpty(restDesing1))
                {
                    retVal = Facet.NEST_CHAR + restDesing1;
                }
            }
            else
            {
                /* Unificamos el primer termino y llamamos recursivamente al resto */
                char[] delimeterChars = { ':' }; // nuestro delimitador será el caracter blanco
                string[] arrayFacets = restDesing1.Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
                string parte1 = arrayFacets[0];
                string resto1 = restDesing1.Replace(parte1, "");
                if (!string.IsNullOrEmpty(resto1) && resto1[0].Equals(':'))
                {
                    resto1 = resto1.Substring(1);
                }
                arrayFacets = restDesing2.Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
                string parte2 = arrayFacets[0];
                string resto2 = restDesing2.Replace(parte2, "");
                if (!string.IsNullOrEmpty(resto2) && resto2[0].Equals(':'))
                {
                    resto2 = resto2.Substring(1);
                }
                
                resto1 = eliminarFacetas(parte2, resto1);
                resto2 = eliminarFacetas(parte1, resto2);
                retVal = Facet.NEST_CHAR + unificar2(parte1, parte2) + unificar(resto1, resto2);
            }
            return retVal;
        }


        /* Descripción: 
         *  Devuelve el string resultande de unificar las dos listas de facetas
         */
        private string unificar2(string restDesing1, string restDesing2)
        {
            char[] delimeterChars = { '[', ']' }; // nuestro delimitador será el caracter blanco
            string[] arrayFacets = restDesing1.Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
            int n = arrayFacets.Length;
            for (int i = 0; i < n; i++)
            {
                string name = "[" + arrayFacets[i] + "]";
                if (!restDesing2.Contains(name))
                {
                    restDesing2 = restDesing2 + name;
                }
            }
            return restDesing2;
        }


        /* Descripción:
         *  Elimina las facetas del primer string del segundo si los hubiera y
         *  devuelve el string resultante.
         */
        private string eliminarFacetas(string restDesing1, string restDesing2)
        {
            char[] delimeterChars = { '[',']' }; // nuestro delimitador será el caracter blanco
            string[] arrayFacets = restDesing1.Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
            int n = arrayFacets.Length;
            for (int i = 0; i < n; i++)
            {
                string name = "["+ arrayFacets[i] +"]";
                restDesing2 = restDesing2.Replace(name, "");
            }
            return restDesing2;
        }

        #endregion Lista con las combinaciones de los diseños de las facetas



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

        #endregion Combinación sin repetición de lista de facetas ordenadas



        /*
         * Descripción:
         *  Clona una lista de facetas.
         * NOTA:
         *  Solo clona la lista no las facetas.
         */

        private static ListFacets clonaListaF(ListFacets lf)
        {
            ListFacets cloneLF = new ListFacets();
            List<Facet> auxLista = lf.RetListFacets();
            foreach (Facet f in auxLista)
            {
                cloneLF.Add(f);
            }
            return cloneLF;
        }


        /* Descripción:
         *  Devuelve un double que es la corrección necesaria para realizar la estimación.
         * Precondición: La lista de facetas no puede ser vacía.
         */
        public double estimateBrennan()
        {
            double retVal = 1;
            int n = this.Count();
            for (int i = 0; i < n; i++)
            {
                Facet f = this.FacetInPos(i);
                if (f.SizeOfUniverse().Equals(int.MaxValue))
                {
                    double x = f.Level();
                    double y = (2 - 1)/2;
                    Console.WriteLine(y.ToString());
                    retVal = retVal * ((x - 1) / x);
                }
                else
                {
                    retVal = retVal * (1 / f.Level());
                }
            }
            return retVal;
        }


        /* Descripción:
         *  Devuelve una lista de facetas que pertenecen a la lista de facetas que se pasa como parámetro
         *  implicito y que no pertenecen a la lista que se pasa como parámetro explicito.
         * Parámetros:
         *      ListFacets lf: Lista de Facetas que no debe pertenecer a la lista de facetas resultante
         */
        public ListFacets Difference(ListFacets lf)
        {
            ListFacets retVal = new ListFacets();
            int n = this.Count();
            for (int i = 0; i < n; i++)
            {
                Facet f = this.FacetInPos(i);
                if (!lf.ExistNameFacet(f))
                {
                    retVal.Add(f);
                }
            }
            return retVal;
        }
        

        /*
         * Descipción:
         *  Imprimer por pantalla una lista de listas de Facetas.
         */
        private static void toStringListOfListFacets(List<ListFacets> llf)
        {
            string linea = "";
            foreach (ListFacets lf in llf)
            {
                List<Facet> auxLista = lf.RetListFacets();
                foreach (Facet f in auxLista)
                {
                    if (linea.Equals(""))
                    {
                        linea = linea + f.Name();
                    }
                    else
                    {
                        linea = linea + "," + f.Name();
                    }
                }
                Console.WriteLine(linea);
                linea = "";
            }
        }


        /* Descripción:
         *  Devuelve una lista de facetas como resultado de unir las dos lista, una como parámetro
         *  implicito y otra como parámetro explicito. No contiene facetas repetidas.
         */
        public ListFacets Union(ListFacets lf)
        {
            ListFacets retVal = new ListFacets();
            int n = this.Count();
            for (int i = 0; i < n; i++)
            {
                Facet f = this.FacetInPos(i);
                if (!lf.ExistNameFacet(f))
                {
                    retVal.Add(f);
                }
            }
            retVal = retVal.Concatenate(lf);
            return retVal;
        }



        #region Operaciones para definir el anidamiento de facetas
        /*=================================================================================================
         * Métodos para definir el anidamiento de facetas
         * ==============================================
         * 
         * La idea es separar la lista de facetas del anidamiento y después recorrer una lista de elementos
         * compuestos por pares de nombres de facetas, el primero representa a la faceta origen, y el segundo
         * a la faceta anidada. Para ello necesito un método que dado el nombre de una faceta encuentre esa
         * Faceta en la lista y la devuelva.
         *=================================================================================================*/

        /* Descripción:
         *  Devuelve si esta en la lista una faceta con el nombre que se pasa como parámetro. Devuelve null
         *  si la faceta no se encuentra en la lista.
         */
        public Facet LookingFacet(String name)
        {
            Facet retVal = null;
            int n = this.listFacets.Count;
            bool find = false;
            for (int i = 0; i < n && !find; i++)
            {
                Facet f = this.FacetInPos(i);
                find = f.Name().Equals(name);
                if (find)
                {
                    retVal = f;
                }
            }
            return retVal;
        }

        #endregion Operaciones para definir el anidamiento de facetas


        #region Conversiones con DataTable
        /* Descripción:
         *  Dada una lista de facetas devuelve un dataTable con los datos de la lista
         */
        public DataTable ListFacets2DataTable(string nameTabla)
        {
            DataTable dtListFacets = new DataTable(nameTabla);
            // Creamos las columnas
            DataColumn c_pk_facet = new DataColumn("pk_facet", System.Type.GetType("System.Int32"));
            DataColumn c_name_facet = new DataColumn("name_facet", System.Type.GetType("System.String"));
            DataColumn c_level_facet = new DataColumn("level_facet", System.Type.GetType("System.Int32"));
            DataColumn c_size_of_universe = new DataColumn("size_of_universe", System.Type.GetType("System.String"));
            DataColumn c_comment = new DataColumn("comment", System.Type.GetType("System.String"));
            DataColumn c_omit = new DataColumn("omit", System.Type.GetType("System.Boolean"));
            DataColumn c_list_facet_design = new DataColumn("list_facet_design", System.Type.GetType("System.String"));

            // añadimos las columnas
            dtListFacets.Columns.Add(c_pk_facet);
            dtListFacets.Columns.Add(c_name_facet);
            dtListFacets.Columns.Add(c_level_facet);
            dtListFacets.Columns.Add(c_size_of_universe);
            dtListFacets.Columns.Add(c_comment);
            dtListFacets.Columns.Add(c_omit);
            dtListFacets.Columns.Add(c_list_facet_design);
            
            int numFacets = this.listFacets.Count;

            for (int i = 0; i < numFacets; i++)
            {
                Facet f = this.FacetInPos(i);
                // Creamos una nueva fila
                DataRow newFacetRow = dtListFacets.NewRow();
                // Rellenamos la fila
                newFacetRow["pk_facet"] = 0;
                newFacetRow["name_facet"] = f.Name();
                newFacetRow["level_facet"] = f.Level();
                string sz = Facet.INFINITE;
                if (f.SizeOfUniverse()< int.MaxValue)
                {
                    sz = f.SizeOfUniverse().ToString();
                }
                newFacetRow["size_of_universe"] = sz;
                newFacetRow["comment"] = f.Comment();
                newFacetRow["omit"] = f.Omit();
                newFacetRow["list_facet_design"] = f.ListFacetDesing();

                // Añadimos la fila al dataTable
                dtListFacets.Rows.Add(newFacetRow);
            }

            return dtListFacets;
        }// end ListFacets2DataTable


        /* Descripción:
         *  Devuelve un dataTable con los niveles omitidos para cada faceta.
         */
        public DataTable SkipLevels2DataTable(string nameTable)
        {
            DataTable dtSkipLevels = new DataTable(nameTable); // valor de retorno
            // Creamos las columnas
            DataColumn c_name_facet = new DataColumn("name_facet", System.Type.GetType("System.String"));
            DataColumn c_skip_level = new DataColumn("skip_level", System.Type.GetType("System.Int32"));
            // Añadimos las columnas
            dtSkipLevels.Columns.Add(c_name_facet);
            dtSkipLevels.Columns.Add(c_skip_level);
            // Para cada faceta añadimos los niveles omitidos
            int numFacet = this.Count();
            for (int i = 0; i < numFacet; i++)
            {
                Facet f = this.FacetInPos(i);
                List<int> lSkipLevels = f.ListSkipLevels();
                int n_skip = lSkipLevels.Count;
                for (int j = 0; j < n_skip; j++)
                {
                    int skip_level = lSkipLevels[j];
                    DataRow row = dtSkipLevels.NewRow();
                    row["name_facet"] = f.Name();
                    row["skip_level"] = skip_level;
                    dtSkipLevels.Rows.Add(row);
                }
            }

            return dtSkipLevels;
        }// end SkipLevels2DataTable


        /* Descripción: 
         *  Dados dos dataTable con el patrón de conversión de esta clase devuelve la lista de facetas
         */
        public static ListFacets DataTables2ListFacets(DataTable dtListFacets, DataTable dtSkipLevels)
        {
            ListFacets returnList = new ListFacets(); // Variable de retorno
            
            int numFacets = dtListFacets.Rows.Count;

            for (int i = 0; i < numFacets; i++)
            {
                DataRow row = dtListFacets.Rows[i];
                string name = (string)row["name_facet"];
                int level = (int)row["level_facet"];
                string s = (string)row["size_of_universe"];
                int size_of_universe = int.MaxValue;
                if(!s.Equals(Facet.INFINITE))
                {
                    size_of_universe = int.Parse(s);
                }
                string comment = (string)row["comment"];
                string ldesign = (string)row["list_facet_design"];
                bool omit = (bool)row["omit"];
                
                // Creamos la faceta;
                Facet f = new Facet(name, level, comment, size_of_universe, ldesign, omit);
                
                // añadimos los niveles omitidos
                string select = " name_facet = '" + name +"'";
                DataRow[] rows = dtSkipLevels.Select(select);
                int numSkipLevels = rows.Length;
                for (int j = 0; j < numSkipLevels; j++)
                {
                    int skip = (int)rows[j]["skip_level"];
                    f.SetSkipLevels(skip, true);
                }

                // Añadimos la faceta a la lista
                returnList.Add(f);
            }// end for

            return returnList;
        }// end DataTables2ListFacets


        /* Descripción:
         *  Devuelve un DataSet con los datos de la lista de facetas
         */
        public DataSet ListFacets2DataSet()
        {
            DataSet dsListFacets = new DataSet("ds_ListFacets");
            DataTable dtListFacets = this.ListFacets2DataTable("TbFacets");
            dsListFacets.Tables.Add(dtListFacets);
            DataTable dtSkipLevels = this.SkipLevels2DataTable("TbSkipLevels");
            dsListFacets.Tables.Add(dtSkipLevels);
            return dsListFacets;
        }


        /* Descripción:
         *  Dado un DataTable devuelve un lista de facetas siempre que el data table respete el formato
         *  de esportación de la clase.
         */
        public static ListFacets DataSet2ListFacets(DataSet dsListFacets)
        {
            DataTable dtListFacets = dsListFacets.Tables["TbFacets"];
            DataTable dtSkipLevels = dsListFacets.Tables["TbSkipLevels"];

            return DataTables2ListFacets(dtListFacets, dtSkipLevels);
        }// end DataSet2ListFacets

        #endregion Conversiones con DataTable

    } // end public partial class ListFacets
} // end namespace MultiFacetData
