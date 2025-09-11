//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using TransLibrary;

//namespace TestTrasLation
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            PruebaClaseTraslation();
//            PruebaClaseLabelTranslation();
//            PruebaClaseReadFileTrans();
//        }

//        private static void PruebaClaseReadFileTrans()
//        {
//            Console.WriteLine("Test de la clase ReadFileTrans");
//            Console.WriteLine("==============================\n");

//            ReadFileTrans diccionarioDeFichero = new ReadFileTrans("idioma_menu.txt");
//            Console.WriteLine(diccionarioDeFichero.ToString());

//            WordTranslation perro = new WordTranslation("perro", "dog", "perro frances", "can portugues");
//            WordTranslation gato = new WordTranslation("gato", "cat", "gato frances", "gato portugues");
//            WordTranslation conejo = new WordTranslation("conejo", "bunny", "conejo portugues", "conejo frances");
//            WordTranslation caballo = new WordTranslation("caballo", "horse", "caballo luso", "caballo frances");

//            string keyGato = "gato";
//            string keyPerro = "perro";
//            string keyCaballo = "caballo";
//            string keyConejo = "conejo";

//            diccionarioDeFichero.AddToDic(keyPerro, perro);
//            diccionarioDeFichero.AddToDic(keyGato, gato);
//            diccionarioDeFichero.AddToDic(keyCaballo, caballo);
//            diccionarioDeFichero.AddToDic(keyConejo, conejo);

//            Console.WriteLine("Esta incluida la clave \"gato\": {0}", diccionarioDeFichero.IsKeyIncluded(keyGato));
//            Console.WriteLine("Esta incluida la clave \"pollo\": {0}", diccionarioDeFichero.IsKeyIncluded("pollo"));
//            Console.WriteLine("Esta incluida la clave \"tsmiConnect\": {0}", diccionarioDeFichero.IsKeyIncluded("tsmiConnect"));
//            Console.WriteLine("Esta incluida la clave \"tsmiHelp\": {0}", diccionarioDeFichero.IsKeyIncluded("tsmiHelp"));
//        }

//        private static void PruebaClaseLabelTranslation()
//        {
//            Console.WriteLine("Test de la clase LabelTraslation");
//            Console.WriteLine("================================\n");

//            WordTranslation perro = new WordTranslation("perro", "dog", "perro frances", "can portugues");
//            WordTranslation gato = new WordTranslation("gato", "cat", "gato frances", "gato portugues");
//            WordTranslation conejo = new WordTranslation("conejo", "bunny", "conejo portugues", "conejo frances");
//            WordTranslation caballo = new WordTranslation("caballo", "horse", "caballo luso", "caballo frances");

//            // Creamos la estructura
//            LabelTranslation diccionario = new LabelTranslation();

//            string keyGato = "gato";
//            string keyPerro = "perro";
//            string keyCaballo = "caballo";
//            string keyConejo = "conejo";

//            diccionario.AddToDic(keyPerro, perro);
//            diccionario.AddToDic(keyGato, gato);
//            diccionario.AddToDic(keyCaballo, caballo);
//            diccionario.AddToDic(keyConejo, conejo);

//            Console.WriteLine(diccionario.ToString());

//            Console.WriteLine("Esta incluida la clave \"gato\": {0}", diccionario.IsKeyIncluded(keyGato));
//            Console.WriteLine("Esta incluida la clave \"pollo\": {0}", diccionario.IsKeyIncluded("pollo"));
//        } // end PruebaClaseLabelTranslation()


//        private static void PruebaClaseTraslation()
//        {
//            Console.WriteLine("Test de la clase traslation");
//            Console.WriteLine("===========================\n");
//            WordTranslation word1 = new WordTranslation("perro", "dog", "perro frances", "can portugues");
//            WordTranslation word2 = new WordTranslation("gato", "cat", "gato frances", "gato portugues");
//            WordTranslation word3 = word1;

//            Console.WriteLine("Test método word1.ToString:\n{0}\n", word1);
//            Console.WriteLine("Test método word2.ToString:\n{0}\n", word2);

//            Console.WriteLine("\nPrueba de los métodos de consulta de la clase Traslation\n");
//            Console.WriteLine("Asinamos word1.spanish(): {0}", word1.GetTranslation(Language.spanish));
//            Console.WriteLine("Asinamos word1.English(): {0}", word1.GetTranslation(Language.english));
//            Console.WriteLine("Asinamos word1.French(): {0}", word1.GetTranslation(Language.french));
//            Console.WriteLine("Asinamos word1.Portuguese(): {0}", word1.GetTranslation(Language.portuguese));

//            Console.WriteLine("\nPrueba de los asignación de consulta de la clase Traslation\n");
//            word1.SetTranslation(Language.spanish, "Perro");
//            Console.WriteLine("Asinamos word1.spanish(\"Perro\"): {0}", word1.GetTranslation(Language.spanish));
//            word1.SetTranslation(Language.english, "Dog");
//            Console.WriteLine("Asinamos word1.English(\"Dog\"): {0}", word1.GetTranslation(Language.english));
//            word1.SetTranslation(Language.french, "Perro Frances");
//            Console.WriteLine("Asinamos word1.French(\"Perro Frances\"): {0}", word1.GetTranslation(Language.french));
//            word1.SetTranslation(Language.portuguese, "Perro Portugues");
//            Console.WriteLine("Asinamos word1.Portuguese(\"Perro Portugues\"): {0}", word1.GetTranslation(Language.portuguese));

//            Console.WriteLine("\nPrueba del método LangTraslation()\n");
//            Console.WriteLine("word1.LangTraslation(Language.english): {0}", word1.LangTraslation(Language.english));
//            Console.WriteLine("word1.LangTraslation(Language.spanish): {0}", word1.LangTraslation(Language.spanish));
//            Console.WriteLine("word1.LangTraslation(Language.french): {0}", word1.LangTraslation(Language.french));
//            Console.WriteLine("word1.LangTraslation(Language.portuguese): {0}", word1.LangTraslation(Language.portuguese));

//            Console.WriteLine("\nProbamos los métodos sobrescritos\n");
//            Console.WriteLine("Método Equals");
//            Console.WriteLine("word1 == word2: {0}", word1.Equals(word2).ToString());
//            Console.WriteLine("word1 == word1: {0}", word1.Equals(word1).ToString());
//            Console.WriteLine("word1 == word3: {0}", word1.Equals(word3).ToString());

//            WordTranslation word4 = new WordTranslation("Perro", "Dog", "Perro Frances", "Perro Portugues");
//            Console.WriteLine("Test método word4.ToString:\n{0}\n", word4);
//            Console.WriteLine("word1 == word4: {0}", word1.Equals(word4).ToString());

//            Console.WriteLine("\nProbamos el método GetHashCode()\n");
//            Console.WriteLine("word1.GetHashCode() == word4.GetHashCode(): {0}",
//                (word1.GetHashCode().Equals(word4.GetHashCode())).ToString());
//            Console.WriteLine("word1.GetHashCode() == word3.GetHashCode(): {0}",
//                (word1.GetHashCode().Equals(word3.GetHashCode())).ToString());
//            Console.WriteLine("word1.GetHashCode() == word2.GetHashCode(): {0}",
//                (word1.GetHashCode().Equals(word2.GetHashCode())).ToString());
//            Console.WriteLine("word2.GetHashCode() == word2.GetHashCode(): {0}",
//                (word2.GetHashCode().Equals(word2.GetHashCode())).ToString());
//        } // PruebaClaseTraslation()
//    }
//}
