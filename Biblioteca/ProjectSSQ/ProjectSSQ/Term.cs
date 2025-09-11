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
    public class Term
    {

        /*======================================================================================
         * Constantes
         *======================================================================================*/
        public const char PLUS = '+';
        public const char MINUS = '-';

        /*======================================================================================
         * Variables
         *======================================================================================*/
        char sign;
        ListFacets lf;


        /*======================================================================================
         * Constructores
         *======================================================================================*/

        //Term(ListFacets lf)
        //{
        //    if (lf == null || lf.Count() < 1)
        //    {
        //        throw new TermException("No cotiene terminos");
        //    }
        //    this.lf = lf;
        //    this.sign = PLUS;
        //}
        public Term(char sign)
        {
            if (!sign.Equals(PLUS) && !sign.Equals(MINUS))
            {
                throw new TermException("Signo incorrecto");
            }
            this.lf = new ListFacets();
            this.sign = sign;
        }

        public Term(Facet f, char sign)
            : this(sign)
        {
            this.lf.Add(f);
        }

        public Term(ListFacets lf, char sign)
        {
            this.lf = lf;
            if(!sign.Equals(PLUS) && !sign.Equals(MINUS))
            {
                throw new TermException("Signo incorrecto");
            }
            this.sign = sign;
            
        }

        /*======================================================================================
         * Métodos de consulta
         *======================================================================================*/
        public char Sign()
        {
            return this.sign;
        }

        public ListFacets ListFacets()
        {
            return this.lf;
        }


        /*======================================================================================
         * Métodos de instancia
         *======================================================================================*/

        public void Add(Facet f)
        {
            this.lf.Add(f);
        }

        public void Add(ListFacets lf)
        {
            int n = lf.Count();
            for (int i = 0; i < n; i++)
            {
                Facet f = lf.FacetInPos(i);
                this.lf.Add(f);
            }
        }

        public void Add(char sign)
        {
            if (!sign.Equals(PLUS) && !sign.Equals(MINUS))
            {
                throw new TermException("Signo incorrecto");
            }
            if (this.sign.Equals(sign))
            {
                this.sign = Term.PLUS;
            }
            else
            {
                this.sign = Term.MINUS;
            }
        }

        public void Add(Term t)
        {
            Add(t.lf);
            Add(t.sign);
        }


        /*======================================================================================
         * Métodos Redefinidos
         *======================================================================================*/
        public override string ToString()
        {
            return (this.sign + this.lf.StringOfListFactes());
        }

    }// end class Term
}// endnamespace ProjectSSQ
