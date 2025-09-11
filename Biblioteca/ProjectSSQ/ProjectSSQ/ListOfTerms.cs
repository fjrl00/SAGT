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
 * Fecha de revisión: 15/Nov/2011                     
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiFacetData;

namespace ProjectSSQ
{
    public class ListOfTerms
    {
        /*======================================================================================
         * Variables
         *======================================================================================*/
        List<Term> listOfTerm; // List de terminos


        /*======================================================================================
         * Constructores
         *======================================================================================*/

        public ListOfTerms()
        {
            this.listOfTerm = new List<Term>();
        }

        public ListOfTerms(ListFacets lf, string design)
            : this()
        {
            char[] delimeterChars = { '[', ']'}; // nuestro delimitador serán los cochetes
            string[] arrayOfstring = design.Trim().Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);
            int num = arrayOfstring.Length;

            bool first_nest_char = false;
            for (int i = 0; i < num; i++)
            {
                string name = arrayOfstring[i];
                bool nest_char = name.Equals(Facet.NEST_CHAR);
                if (!first_nest_char)
                {
                    first_nest_char = nest_char;
                }

                if (!nest_char)
                {
                    Facet f = lf.LookingFacet(name);
                    Term t1 = new Term(f, Term.PLUS);
                    if (!first_nest_char)
                    {
                        Term t2 = new Term(new ListFacets(), Term.MINUS);
                        ListOfTerms newlt = this.CopyListTerm();
                        this.Add(t1);
                        newlt.Add(t2);
                        this.Concatenate(newlt);
                    }
                    else
                    {
                        Add(t1);
                    }
                }

            }// end for
        }// end ListOfTerms

        private void Add(Term t1)
        {
            int num = this.listOfTerm.Count;
            if (num == 0)
            {
                this.listOfTerm.Add(t1);
            }
            for (int i = 0; i < num; i++)
            {
                Term term = this.listOfTerm[i];
                term.Add(t1);
            }
        }


        /* Descripción:
         *  Copia una lista de terminos.
         */
        private ListOfTerms CopyListTerm()
        {
            ListOfTerms retVal = new ListOfTerms();
            int numTerm = this.listOfTerm.Count;

            for(int i=0;i<numTerm;i++)
            {
                Term t = this.listOfTerm[i];
                ListFacets lf = t.ListFacets();
                char s = t.Sign();
                ListFacets newLf = ListFacets.CopyListFacets(lf);//new ListFacets();
                Term new_t = new Term(newLf, s);
                retVal.listOfTerm.Add(new_t);
            }

            return retVal;
        }


        /* Descripción:
         *  Concatena dos listas de terminos.
         */
        private void Concatenate(ListOfTerms l_o_t)
        {
            int n = l_o_t.listOfTerm.Count;
            for (int i = 0; i < n; i++)
            {
                this.listOfTerm.Add(l_o_t.listOfTerm[i]);
            }
        }

        /*======================================================================================
         * Métodos de consulta
         *======================================================================================*/

        /* Descripción:
         *  Devuelve el número de terminos del que se comone la lista de terminos.
         */
        public int Count()
        {
            return this.listOfTerm.Count;
        }


        /* Descripción:
         *  Devuelve el termino de la posición i-esima que se pasa como parámetro.
         */
        public Term TermInPos(int i)
        {
            return this.listOfTerm[i];
        }

    }// end class ListOfTerms
}// end namespace ProjectSSQ