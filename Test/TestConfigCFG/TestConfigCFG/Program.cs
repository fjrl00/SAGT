using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfigCFG;

namespace TestConfigCFG
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test de la clase CongigCFG");
            Console.WriteLine("==========================\n\n");
            /*
            Console.WriteLine("Creamos el valor por defecto: ");
            Console.WriteLine("la nueva variable se llama cfg\n");
            
            ConfigCFG cfg = new ConfigCFG();

            Console.WriteLine("Resultado esperado: language=english");
            Console.WriteLine("Resultado obtenido: {0}\n",cfg.ToString());

            Console.WriteLine("Asignamos a cfg2 el valor de cfg");
            ConfigCFG cfg2 = cfg;
            Console.WriteLine("Creamos una nueva variable con el mismo valor que cfg llamada cfg3");
            ConfigCFG cfg3 = new ConfigCFG();
             */
            bool retVal = testToString();
            Console.WriteLine("Test ToString: {0}", ConvertTrueToOK(retVal));
            retVal = testEquals();
            Console.WriteLine("Test Equals: {0}", ConvertTrueToOK(retVal));
            retVal = testGetHashCode();
            Console.WriteLine("Test GetHashCode: {0}", ConvertTrueToOK(retVal));
            retVal = testGetAndSet();
            Console.WriteLine("Test de consulta y asignación: {0}", ConvertTrueToOK(retVal));
            retVal = testWriteFile();
            Console.WriteLine("Test de lectura y escritura: {0}", ConvertTrueToOK(retVal));
        }

        private static string ConvertTrueToOK(bool res)
        {
            string retVal = "OK";
            if (!res)
            {
                retVal = "ERROR";
            }
            return retVal;
        }

        #region Métosdos de lectura y escritura de fichero
        /*
         * Descripción:
         *  Test de escritura y lectura de fichero.
         */
        private static bool testWriteFile()
        {
            string file = "prueba.cfg";
            string file2 = "prueba.cfg";
            ConfigCFG.ConfigCFG cfg = new ConfigCFG.ConfigCFG();
            cfg.WriteFileConfig(file);
            ConfigCFG.ConfigCFG cfg2 = new ConfigCFG.ConfigCFG();
            cfg2.SetConfigLanguage(TransLibrary.Language.french);
            cfg2.WriteFileConfig();
            cfg.ReadFileConfig();
            return cfg.Equals(cfg2);
        }
        private static bool testReaderFile()
        {
            return true;
        }
        #endregion Métosdos de lectura y escritura de fichero
        #region Métodos de asignanción y consulta
        /*
         * Descripción:
         *  Método de asignación y consulta.
         */
        private static bool testGetAndSet()
        {
            ConfigCFG.ConfigCFG cfg = new ConfigCFG.ConfigCFG();
            cfg.SetConfigLanguage(TransLibrary.Language.french);
            string val = cfg.GetConfigLanguage().ToString();

            return (TransLibrary.Language.french.ToString()).Equals(val);
        }
        #endregion Métodos de asignanción y consulta

        #region Métodos redefinidos: ToString, Equals y GetHashCode
        /*
         * Descripción:
         *  Test de Método ToString() y constructor por defecto.
         */
        private static bool testToString()
        {
            ConfigCFG.ConfigCFG cfg1 = new ConfigCFG.ConfigCFG();
            String val = "Language=english";
            return (cfg1.ToString()).Equals(val);
        }

        /*
         * Descripción:
         *  Test Método Equals. Debuelve true si se ha realizado el test correctamente false 
         *  en otro caso.
         */
        private static bool testEquals()
        {
            // Creamos una variable con los valores por defecto
            ConfigCFG.ConfigCFG cfg1 = new ConfigCFG.ConfigCFG();
            ConfigCFG.ConfigCFG cfg2 = cfg1;
            ConfigCFG.ConfigCFG cfg3 = new ConfigCFG.ConfigCFG();
            ConfigCFG.ConfigCFG cfg4 = new ConfigCFG.ConfigCFG();
            cfg4.SetConfigLanguage(TransLibrary.Language.french);

            bool val1 = cfg1.Equals(cfg2); // esperado true
            bool val2 = cfg1.Equals(cfg2); // esperado true
            bool val3 = cfg1.Equals(cfg3); // esperado true
            bool val4 = cfg1.Equals(cfg4); // esperado false


            return (val1 && val2 && val3 && !val4);
        }

        /*
         * Descripción:
         *  Test Método testGetHashCode(). Debuelve true si se ha realizado el test correctamente false 
         *  en otro caso.
         */
        private static bool testGetHashCode()
        {
            // Creamos una variable con los valores por defecto
            ConfigCFG.ConfigCFG cfg1 = new ConfigCFG.ConfigCFG();
            ConfigCFG.ConfigCFG cfg2 = cfg1;
            ConfigCFG.ConfigCFG cfg3 = new ConfigCFG.ConfigCFG();
            ConfigCFG.ConfigCFG cfg4 = new ConfigCFG.ConfigCFG();
            cfg4.SetConfigLanguage(TransLibrary.Language.french);

            bool val1 = (cfg1.GetHashCode()).Equals(cfg2.GetHashCode()); // esperado true
            bool val2 = (cfg1.GetHashCode()).Equals(cfg2.GetHashCode()); // esperado true
            bool val3 = (cfg1.GetHashCode()).Equals(cfg3.GetHashCode()); // esperado true
            bool val4 = (cfg1.GetHashCode()).Equals(cfg4.GetHashCode()); // esperado false
            // el último caso no tiene porque ser siempre cierto

            return (val1 && val2 && val3 && !val4);
        }
        #endregion Métodos redefinidos: ToString, Equals y GetHashCode
    }
}
