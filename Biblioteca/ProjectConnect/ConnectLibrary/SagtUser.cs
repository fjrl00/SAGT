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
 * Fecha de revisión: 25/Jun/2012
 * 
 * Descripción:
 *      Datos de usuario
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectLibrary
{
    public class SagtUser
    {
        /* Se corresponde con los roles de MenPas
         */
        public enum UserAccess
        {
            Ad_Restringido, Administrador, AD_Cuestionarios, AD_Paises, Usuario
        }

        /*=================================================================================
         * Variables
         *=================================================================================*/
        private int userID;
        private string nameUser;
        private UserAccess authorizationToAccess;
        private string group;


        /*=================================================================================
         * Constructores
         *=================================================================================*/
        public SagtUser(int userId, string nameUser, UserAccess user, string group)
        {
            if (string.IsNullOrEmpty(nameUser))
            {
                throw new SagtUser_Exception("Error: valores incorrectos");
            }
            this.userID = userId;
            this.nameUser = nameUser;
            this.authorizationToAccess = user;
            this.group = group;
        }

        /*=================================================================================
         * Métodos de consulta
         *=================================================================================*/
        public int GetUserID()
        {
            return this.userID;
        }


        public string GetNameUser()
        {
            return this.nameUser;
        }


        public UserAccess GetAuthorizationToAccess()
        {
            return this.authorizationToAccess;
        }

        public string GetGroup()
        {
            return this.group;
        }

        /*=================================================================================
         * Métodos de Instancia
         *=================================================================================*/
        public void SetUserID(int userId)
        {
            this.userID = userId;
        }
        

        public void SetNameUser(string nameUser)
        {
            this.nameUser = nameUser;
        }


        public void SetAuthorizationToAccess(UserAccess authorization)
        {
            this.authorizationToAccess = authorization;
        }

        public void SetGroup(string group)
        {
            this.group = group;
        }

        /*=================================================================================
         * Métodos Redefinidos
         *=================================================================================*/

        public override string ToString()
        {
            return "[userId= " + this.userID + ", name= " + this.nameUser + ", Access=" + this.authorizationToAccess.ToString() + ", Group= " +this.group +"]";
        }


        public override Boolean Equals(Object obj)
        {
            // Variable de retorno
            Boolean res = false;
            if (!(obj == null || GetType() != obj.GetType()))
            {
                SagtUser user = (SagtUser)obj;

                res = this.userID.Equals(user.userID);
            }
            return res;
        }


        public override int GetHashCode()
        {
            return this.userID.GetHashCode();
        }

    }// end public class SagtUser
}// end namespace ConnectLibrary
