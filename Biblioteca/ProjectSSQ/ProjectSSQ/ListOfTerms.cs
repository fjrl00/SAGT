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
 * Lista de términos que definen la suma de cuadrados.
 */
using MultiFacetData;
using System;
using System.Collections.Generic;

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

        /*
         * For a design of facets contained in the ListFacets lf, creates a ListOfTerms according to the design string,
         * used in CalcSSq for computing sums of squares.
         * 
         * Behavior:
         *  Before finding any nesting, any crossing will generate two branches: 
         *   one with everything given a + (everyone maintains their sign), and every term being given the new element
         *   one with everything negative (everyone's sign is flipped).
         *   They'll be concatenated.
         *   We'll be kept at a state of 50% positive terms, 50% negative.
         *  Once any nesting is found, everything will just maintain signs instead and is added the new facet to their listfacets.
         *  
         *  [A]                 -> {+([A]) -() } The 2 branches generated and added
         *  
         *  [A]:[B]             -> {+([A] [B]) -([B]) } B is given to every element, maintaining signs as they were
         *  [A]:[B]:[C]         -> {+([A] [B] [C]) -([B] [C]) } same conduct as [A]:[B]
         *  [A]:[B][C]          -> {+([A] [B] [C]) -([B] [C]) } same conduct as [A]:[B]
         *  
         *  [A][B]              -> {+([A] [B]) -([B]) -([A]) +() } The positive branch given B, the other one flipped
         *  [A][B]:[C]          -> {+([A] [B] [C]) -([B] [C]) -([A] [C]) +([C]) } same conduct as [A]:[B]
         *  [A][B][C]           -> {+([A] [B] [C]) -([B] [C]) -([A] [C]) +([C]) -([A] [B]) +([B]) +([A]) -() } Same conduct as [A][B]
         */
        public ListOfTerms(ListFacets lf, string design)
            : this()
        {
            char[] delimeterChars = { '[', ']' };
            string[] arrayOfstring = design.Trim().Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);    //e.g. "[O]:[I][C]" -> { "O", ":", "I", "C" };.
            int num = arrayOfstring.Length;

            bool first_nest_char = false;
            for (int i = 0; i < num; i++)                           //for each arrayofstring character
            {
                string name = arrayOfstring[i];
                bool nest_char = name.Equals(Facet.NEST_CHAR);
                if (nest_char && !first_nest_char)                      //if it's a nest char and we hadn't found any yet
                {
                    first_nest_char = nest_char;                            //then we set that flag
                }
                if (!nest_char)                                         //meanwhile if it's not a nest char
                {
                    Facet f = lf.LookingFacet(name);                        //we fetch in that lF the facet with that name/design
                    Term t1 = new Term(f, Term.PLUS);                       //we store it in a term with PLUS sign
                    if (!first_nest_char)                                   //if we haven't found a nest char yet
                    {
                        ListOfTerms newlot = this.Clone();                       //copy this list of terms
                        this.Add(t1);                                            //to this list add t1. All terms in this will be turned positive, and we'll add f to their listfacets
                        newlot.Add(new Term(new ListFacets(), Term.MINUS));      //to the copy instead add empty negative. All terms in newlot will be turned negative, with no changes to their listfacets
                        this.Concatenate(newlot);                                //concatenate
                    }
                    else
                    {
                        Add(t1);                                            //if we have found it tho, we just add t1 to this list
                    }
                }

            }// end for
        }// end ListOfTerms

        private void Add(Term t1)
        {
            int num = this.listOfTerm.Count;
            if (num == 0)                           //only if the list is empty
            {
                this.listOfTerm.Add(t1);                //Add it to this list (with List<T>.Add(), not with ListOfTerms.Add())
            }
            for (int i = 0; i < num; i++)           //else (if num was calc'd as 0 then i<0 can only be false) for each term in our current list
            {
                Term term = this.listOfTerm[i];
                term.Add(t1);                           //Add it to that term's ListFacets, colonize its sign
            }
        }


        /* Descripción:
         *  Clone of list terms. the references to the facets of ListFacets are shallow copies 
         *  (in order to save memory resources). Everything else is deeply copied.
         */
        private ListOfTerms Clone()
        {
            ListOfTerms retVal = new ListOfTerms();
            int numTerm = this.listOfTerm.Count;

            for (int i = 0; i < numTerm; i++)
            {
                Term t = this.listOfTerm[i];
                ListFacets lf = t.ListFacets();
                char s = t.Sign();
                ListFacets newLf = lf.ShallowClone();
                Term new_t = new Term(newLf, s);
                retVal.listOfTerm.Add(new_t);
            }

            return retVal;
        }


        /* Descripción:
         *  Concatena dos listas de terminos.
         *  Simple concatenation with List<T>.Add(), not ListOfTerms.Add().
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

        public override string ToString()
        {
            string retVal = "";
            foreach (Term t in this.listOfTerm)
            {
                retVal += t.ToString() + " ";
            }

            return retVal;
        }

    }// end class ListOfTerms
}// end namespace ProjectSSQ