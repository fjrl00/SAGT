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
 * Fecha de revisión: 2/Nov/2011                 
 * 
 */
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

namespace MultiFacetData
{
    public partial class ListFacets : System.ICloneable, IEnumerable, IComparable<ListFacets>
    {
        /*======================================================================================
         * Constantes
         *======================================================================================*/
        // Constantes para usarlas como marcas en el fichero
        public const string BEGIN_LISTFACETS = "<list_facets>";
        const string END_LISTFACETS = "</list_facets>";
        public const string BEGIN_LIST_NESTING = "<list_nesting>";
        const string END_LIST_NESTING = "</list_nesting>";
        const string BEGIN_NESTING = "<nesting>";
        const string END_NESTING = "</nesting>";


        /*======================================================================================
         * Variables de instancia
         *======================================================================================*/
        // Variables de instancia;
        private List<Facet> listFacets;



        #region Constructores de la clase ListFacets
        /*======================================================================================
         * Constructores
         *======================================================================================*/

        /*
         * Descripción:
         *  Constructor por defecto de una lista de facetas;
         */
        public ListFacets()
        {
            listFacets = new List<Facet>();
        }


        /*
         * Descripción:
         *  Constructor de la clase ListsFacets. Se le pasa por parámetro una lista de Facetas.
         *  (No puede haber dos facetas con el mismo nombre).
         * Parámetros:
         *      List<Facet> listF: Lista de Facetas.
         * Excepciones:
         *      Lanza una excepción ListFacetExceptions sí la lista que se le pasa como 
         *      parámetro tiene dos facetas con el mismo nombre.
         */
        public ListFacets(List<Facet> listF)
        {
            // comprobamos que la lista no tiene dos parámetros con el mismo nombre
            if (!CheckListNameFacets(listF))
            {
                // tiene nombres repetido y por tanto lanzamos la excepción
                throw new ListFacetsException("No se permiten Listas de Facetas con nombres duplicados");
            }
            this.listFacets = listF;
        }
        #endregion Constructores de la clase ListFacets



        #region Métodos Auxiliares

        /*======================================================================================
         * Métodos Auxiliares
         * ==================
         *  - CheckListNameFacets
         *  - ExistNameFacet
         *======================================================================================*/

        /*
         * Descripción:
         *  Operación auxiliar. Comprueba que la lista de facetas sea válida. Para ello no puede
         *  haber dos facetas con el mismo nombre/etiqueta.
         * Devuelve:
         *  bool: True si la lista de facetas no contiene nombres repetidos. False en otro caso.
         */
        private static bool CheckListNameFacets(List<Facet> lFacets)
        {
            bool retval = true; // variable de retorno
            if (lFacets.Count > 1) // si la lista de facetas tiene menos de un elemento 
                                   // no tengo porque chequearla.
            {
                List<string> lNames = new List<string>();
                foreach (Facet f in lFacets)
                {
                    lNames.Add(f.Name().ToUpper());
                }

                lNames.Sort();

                int numFacets = lNames.Count - 1; // restamos uno para evitar que se salga del rango
                for (int i = 0; i < numFacets && retval; i++)
                {
                    //res = lNames[i].Equals(lNames[i+1]);
                    if (lNames[i].Equals(lNames[i + 1]))
                    {
                        retval = false;
                    }
                }
            }
            
            return retval;
        }// private static bool CheckListNameFacets(List<Facet> lFacets)


        /*
         * Descripción:
         *  Devuelve true si existe una faceta con el mismo nombre en la lista de facetas, false en 
         *  caso contrario.
         * Parámetro: 
         *      Facet f: Es la faceta que queremos comprobar.
         */
        private bool ExistNameFacet(Facet f)
        {
            bool retval = false;
            int n = this.listFacets.Count;
            for (int i = 0; i < n && !retval; i++)
            {
                retval = this.listFacets[i].Name().ToLower().Equals(f.Name().ToLower());
            }
            return retval;
        }
        #endregion Métodos Auxiliares



        #region Métodos de Consulta
        /*======================================================================================
         * Métodos de Consulta
         * ===================
         *  - FacetInPos
         *  - Count
         *  - RetListFacets 
         *  - IndexOf
         *======================================================================================*/

        /*
         * Descripción:
         *  Devuelve la faceta existente en una posición de la lista de Facetas.
         * Parámetros:
         *      int i: posición en la que se encuentra la faceta que queremos consulatar.
         */
        public Facet FacetInPos(int pos)
        {
            if (pos < 0 || pos >= this.listFacets.Count)
            {
                // lanzamos una excepción de indice fuera de rango
                throw new ListFacetsException("Indice fuera del rango de la lista de facetas");
            }
            return this.listFacets[pos];
        }


        /*
         * Descripción:
         *  Devuelve el número de facetas que contiene la lista.
         */
        public int Count()
        {
            return this.listFacets.Count;
        }


        /*
         * Descripción:
         *  Devuelve la lista de facetas.
         */
        public List<Facet> RetListFacets()
        {
            return this.listFacets;
        }

        public int IndexOf(Facet f)
        {
            return this.listFacets.IndexOf(f);
        }

        #endregion Métodos de Consulta

        #region Métodos de instancia
        /*======================================================================================
         * Métodos de instancia:
         *  - Add
         *  - ParentOrderAdd
         *  - Remove
         *  - IsEmpty
         *  - WritingStreamListFacets
         *  - ReadingStreamListFacets
         *======================================================================================*/

        /*
         * Descripción:
         *  Añade una faceta a la clase ListFacets, lo hará en última posición.
         * Parámetros:
         *      Facet f: faceta que queremos añadir a nuestro objeto de la clase ListFacets
         * Excepciónes:
         *      Lanza una excepción de tipo ListFacetsException si ya existe una faceta con el
         *      mismo nombre en la lista.
         */
        public void Add(Facet f)
        {
            // primero debemos comprobar que no existe otra faceta con el mismo nombre.
            if (this.ExistNameFacet(f))
            {
                throw new ListFacetsException("Existe una faceta con el mismo nombre.");
            }
            listFacets.Add(f);
        }

        /*
         * Descripción:
         *  Añade una faceta a la clase ListFacets, siguiendo el orden que sigue 'parent'
         */
        public void ParentOrderAdd(Facet f, ListFacets parent)
        {
            if(!parent.ContainsList(this) || !parent.Contains(f))
            {
                throw new ListFacetsException("Las listas de facetas de instrumentación y diferenciación no están sincronizadas con la original");
            }
            if (this.ExistNameFacet(f))
            {
                throw new ListFacetsException("Existe una faceta con el mismo nombre.");
            }

            int parentIndex = parent.IndexOf(f);
            
            // Find where this item should go in sublist
            int insertIndex = this.listFacets.Count; // append at the end by default
            for (int i = 0; i < this.listFacets.Count; i++) //begin iterating through the sublist
            {
                int sublistItemIndex = parent.IndexOf(this.listFacets[i]);  // get this sublist item's position in the parent list
                if (parentIndex < sublistItemIndex) // if f should appear before this sublist item (based on parent order)
                {
                    insertIndex = i;                // aim to put the facet just before the current iterated item
                    break;
                }
            }

            this.listFacets.Insert(insertIndex, f);
        }

        /*
         * Descripción:
         *  Elimina un elemento pasado como parámetro de la lista. Si el elemento es eliminado
         *  correctamente devuelve true, en otro caso devuelve false.
         * Parámetros:
         *      Facet f: Faceta que queremos eliminar.
         */
        public bool Remove(Facet f)
        {
            return this.listFacets.Remove(f);
        }

        /*
         * Descripción:
         *  Devuelve true si la lista esta vacia, false en otro caso.
         */
        public bool IsEmpty()
        {
            return this.Count() == 0;
        }

        /* Descripción:
         *  Realiza un anidamiento de la lista de facetas, es decir cada faceta estará anidada 
         *  en la siguiente.
         */
        public void NestingAllFacet()
        {
            int n = this.Count();
            for (int i = 1; i < n; i++)
            {
                Facet f = this.FacetInPos(i-1);
                Facet s = this.FacetInPos(i);
                string design = "["+ s.Name() +"]:" + f.ListFacetDesing();
                s.ListFacetsDesignNesting(design);
            }
        }


        #endregion Métodos de instancia

        /* Descripción:
         *  Escribe una lista de facetas.
         */
        public bool WritingStreamListFacets(StreamWriter writer)
        {
            bool res = true; // variable de retorno

            writer.WriteLine(BEGIN_LISTFACETS);
            // Escribimos la lista de facetas
            int n = this.Count();
            for (int i = 0; i < n && res; i++)
            {
                Facet f = this.FacetInPos(i);
                res = f.WritingStreamFacet(writer);
            }
            // escribimos la lista con los datos
            writer.WriteLine(END_LISTFACETS);

            return res;
        }


        /* Descripción:
         *  Lee una lista de facetas.
         */
        public static ListFacets ReadingStreamListFacets(StreamReader reader)
        {
            ListFacets lf = new ListFacets();
            try
            {
                string line;
                while (!(line = reader.ReadLine()).Equals(END_LISTFACETS) && line!=null)
                {
                    lf.Add(Facet.ReadingStreamFacet(reader));
                }
                if (line == null)
                {
                    throw new ListFacetsException("Error en el formato del fichero");
                }
            }
            catch (FacetException)
            {
                throw new ListFacetsException("Error en la lectura de facetas");
            }
            
            return lf;
        }



        #region Implementación de Interfaces
        /*======================================================================================
         * Implementación de Interfaces
         *  - GetEnumerator()
         *  - CompareTo(ListFacets other) 
         *  - Clonable
         *======================================================================================*/

        /*
         * Descripción:
         *  Método de la interfaz IEnumerable
         */
        IEnumerator IEnumerable.GetEnumerator()
        {
            return listFacets.GetEnumerator();
        }


        /*
         * Descipción:
         *  Método de la interfaz IComparable
         */
        public int CompareTo(ListFacets other)
        {
            int retval = this.Count().CompareTo(other.Count());
            if(retval==0)
            {
                int longOther = other.Count();
                for (int i = 0; i < longOther && (retval == 0); i++)
                {
                    retval = this.listFacets[i].CompareTo(other.listFacets[i]);
                }
            }
            return retval;
        }


        /* Descripción:
         *  Método de la interfaz ICloneable
         */
        public object Clone()
        {
            ListFacets lf = new ListFacets(); // Variable de retorno
            int n = this.listFacets.Count;
            for (int i = 0; i < n; i++)
            {
                Facet f = this.FacetInPos(i);
                Facet newF = (Facet)f.Clone();
                lf.Add(newF);
            }
            return lf;
        }

        #endregion Implementación de Interfaces



        #region Métodos redefinidos: ToString, Equals, GetHashCode
        /*======================================================================================
         * Métodos Redefinidos
         *======================================================================================*/

        /*
         * Redefinición del método ToString
         */
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            // primero incluimos la lista de facetas
            foreach (Facet f in this.listFacets)
            {
                res.Append(f.ToString() + "\n");
            }
            return res.ToString();
        }

        /* Redefinición del método Equals
         */
        public override bool Equals(object obj)
        {
            // Variable de retorno
            bool res = false;
            if (!(obj == null || GetType() != obj.GetType()))
            {// (* 1 *)
                ListFacets oList = (ListFacets)obj;
                res = (oList.Count() == this.Count());
                if (res)
                {
                    int n = this.Count();
                    for (int i = 0; i < n && res; i++)
                    {
                        Facet f1 = this.listFacets[i];
                        int pos = oList.IndexOf(f1);
                        res = pos > (-1);
                    }
                }
            } // end if (* 1 *)
            return res;
        }

        /*
         * Redefinición del método GetHashCode
         */
        public override int GetHashCode()
        {
            int retval = 0;
            foreach (Facet f in this.listFacets)
            {
                retval += f.GetHashCode();
            }
            return (retval/3);
        }

        #endregion Métodos redefinidos: ToString, Equals, GetHashCode

    }// end public partial class ListFacets
}// end namespace MultiFacetData
