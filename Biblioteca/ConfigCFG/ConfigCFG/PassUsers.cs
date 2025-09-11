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
 * Fecha de revisión: 17/May/2012
 * 
 * Descripción:
 *  Almacena y recupera las claves asociadas al nombre de los usuarios.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ConfigCFG
{
    public class PassUsers
    {
        /*=========================================================================================
         *  Constantes
         *=========================================================================================*/
        // Constantes
        public const string COLUMN_USER_NAME = "userName";
        public const string COLUMN_USER_PASS = "userPass";
        private const string PASSWORD = "Rijndael";
        public const string FILE_USER_PASS = "user.pas";
        

        /*=========================================================================================
         *  Variables
         *=========================================================================================*/
        // Variables
        DataTable dtUser;
        

        /*=========================================================================================
         *  Constructores
         *=========================================================================================*/
        // Constructores
        public PassUsers()
        {
            this.dtUser = new DataTable();

            DataColumn userName = new DataColumn(COLUMN_USER_NAME, System.Type.GetType("System.String"));
            DataColumn userPass = new DataColumn(COLUMN_USER_PASS, System.Type.GetType("System.String"));

            this.dtUser.Columns.Add(userName);
            this.dtUser.Columns.Add(userPass);
        }

        /*=========================================================================================
         *  Métodos de consulta
         *=========================================================================================*/
        // Métodos de consulta

        /* Descipción:
         *  Devuelve el dataTable que contiene el nombre de los usuarios y las contraseñas.
         */
        public DataTable DataTable()
        {
            return this.dtUser;
        }


        /* Descripción:
         *  Devuelve el password correspondiente al nombre de usuario que se pasa como parámetro.
         *  En el caso de que no se encuentre se devuelve la cadena vacia.
         */
        public string ReturnPass(String userName)
        {
            int numRow = this.dtUser.Rows.Count;
            bool found = false;
            DataRow row;
            string retVal = "";
            for (int i = 0; i < numRow && !found; i++)
            {
                row = this.dtUser.Rows[i];
                found = row[COLUMN_USER_NAME].ToString().Equals(userName);
                if (found)
                {
                    retVal = row[COLUMN_USER_PASS].ToString();
                }
            }
            return retVal;
        }

        /*=========================================================================================
         *  Métodos de instancia
         *=========================================================================================*/
        /* Descripción:
         *  Amacena el usuario y la clave en el dataTable. Si ya se encontraba la contraseña
         *  esta se actualiza siempre en el dataTable, Si no se encuentra entonces para que se
         *  almacene remenbePass tiene que estar a true.
         */
        public void UpdateDataRow(string name, string pass, bool rememberPass)
        {
            int numRow = this.dtUser.Rows.Count;
            bool found = false;
            DataRow row;
            
            for (int i = 0; i < numRow && !found; i++)
            {
                row = this.dtUser.Rows[i];
                found = row[COLUMN_USER_NAME].ToString().Equals(name);

                if (found)
                {
                    this.dtUser.Rows.Remove(row);
                }
            }

            if (!(found || rememberPass))
            {
                pass = "";
            }
            row = this.dtUser.NewRow();
            row[COLUMN_USER_NAME] = name;
            row[COLUMN_USER_PASS] = pass;
            this.dtUser.Rows.Add(row);
        }



        /* Descripción:
         *  Toma los datos que se pasán en el string y lo escribe en un fichero. encryptando los datos
         * Parámetros:
         *      String data: string de datos que se quiere encryptar
         *      String outputFileName: Nombre de fichero de salida en el que se guardarán los datos 
         *      byte[] key: Establece la clave secreta del algoritmo simétrico.
         *      byte[] iv: Establece el vector de inicialización (IV) del algoritmo simétrico.
         *      
         * Excepciones:
         *      CryptographicException:
         *      UnauthorizedAccessException:
         */
        public static void EncryptTextToFile(String data, String outputFileName)
        {
            // convert string to stream
            byte[] byteArray = Encoding.ASCII.GetBytes(data);
            MemoryStream stream = new MemoryStream(byteArray); 

            FileStream fsEncrypted = new FileStream(outputFileName,
               FileMode.Create,
               FileAccess.Write);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(PASSWORD);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(PASSWORD);
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptostream = new CryptoStream(fsEncrypted,
               desencrypt,
               CryptoStreamMode.Write);

            byte[] bytearrayinput = new byte[stream.Length];
            stream.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Close();            
            fsEncrypted.Close();
        }// end EncryptTextToFile


        /* Descripción:
         *  Devuelve los datos desencriptados de un fichero de datos encriptado que se pasa como parámetro.
         * Parámetros:
         *      String inputFileName: Nombre de fichero de salida en el que se guardarán los datos 
         *      byte[] key: Establece la clave secreta del algoritmo simétrico.
         *      byte[] iv: Establece el vector de inicialización (IV) del algoritmo simétrico.
         *      
         * Excepciones:
         *      CryptographicException:
         *      UnauthorizedAccessException:
         */


        static string Descrypter(string sInputFilename, string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //A 64 bit key and IV is required for this provider.
            //Set secret key For DES algorithm.
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            //Set initialization vector.
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            //Create a file stream to read the encrypted file back.
            FileStream fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);

            //Create a DES decryptor from the DES instance.
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            //Create crypto stream set to read and do a 
            //DES decryption transform on incoming bytes.
            CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);

            StreamReader reader = new StreamReader(cryptostreamDecr);
            string textoDesencritado = "";
            string line = reader.ReadLine();

            while (line != null)
            {
                textoDesencritado = textoDesencritado + line;
                line = reader.ReadLine();
            }
            fsread.Close();
            return textoDesencritado;
        }
        


        /* Descripción:
         *  Toma lee los datos de un fichero y lo devuelve en un DataTable que contiene el 
         *  usuario y la contraseña.
         */
        public static PassUsers LoadDataTable(string inputFile)
        {
            PassUsers dt = new PassUsers();

            Rijndael RijndaelAlg = Rijndael.Create(PASSWORD);
            string text = Descrypter(inputFile, PASSWORD); // DecryptTextFromFile(inputFile, RijndaelAlg.Key, RijndaelAlg.IV);

            // Dividimos el texto en líneas
            char[] delimeterLineChars = { '\n',';'};
            string[] lines = text.Split(delimeterLineChars, StringSplitOptions.RemoveEmptyEntries);

            // El texto tiene el formato "[nombre_usuario],[pase];"
            char[] delimiters = { '[', ']', ',' };
            int numLines = lines.Length;

            for (int i = 0; i < numLines; i++)
            {
                string pass = "";
                
                string[] line = lines[i].Split(delimiters,StringSplitOptions.RemoveEmptyEntries);
                switch (line.Length)
                {
                    case(1):
                        break;
                    case(2):
                        pass = line[1];
                        break;
                    default:
                        // error
                        break;
                }
                DataRow row = dt.DataTable().NewRow();
                row[COLUMN_USER_NAME] = line[0];
                row[COLUMN_USER_PASS] = pass;
                dt.DataTable().Rows.Add(row);
            }

            return dt;
        }// end LoadDataTable


        /* Descripción:
         *  Guarda en un archivo encriptado los datos del DataTable.
         */
        public void EncryptDataTableToFile(String outputFile)
        {
            // Transformamos los datos en un string con el fomato correcto
            string text = "";
            int numRow = this.dtUser.Rows.Count;
            for (int i = 0; i < numRow; i++)
            {
                DataRow row = this.dtUser.Rows[i];
                string line = "[" + row[COLUMN_USER_NAME] +"],[" + row[COLUMN_USER_PASS] + "];\n";
                text = text + line;
            }

            // Rijndael RijndaelAlg = Rijndael.Create(PASSWORD);
            EncryptTextToFile(text, outputFile);
        }


        
    }// end class PassUsers
}// end namespace ConfigCFG
