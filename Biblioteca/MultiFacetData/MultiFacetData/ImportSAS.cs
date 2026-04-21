/* 
 * Proyecto: SOFTWARE PARA LA APLICACIÓN DE LA TEORÍA DE LA GENERALIZABILIDAD
 * Nº de orden: 4778
 * 
 * Alumno:   Francisco Jesús Ramos Pérez
 * 
 * Extensión: Importador desde ficheros SAS (datalines)
 * 
 * Fecha de revisión: 14/Abr/2026
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MultiFacetData
{
    public class ImportSAS
    {
        /***************************************************************************************************
         * CONSTANTES
         ***************************************************************************************************/
        private const string DATALINES_KEYWORD = "datalines";
        private const string CARDS_KEYWORD = "cards";
        private const string INPUT_KEYWORD = "input";
        private const string SEMICOLON = ";";

        /***************************************************************************************************
         * MÉTODOS PÚBLICOS
         ***************************************************************************************************/

        public static List<string> ReadColumns(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string sasContent = reader.ReadToEnd();

                // 1. Extraer la sentencia INPUT para conocer todas las variables
                string inputStatement = ExtractInputStatement(sasContent);
                if (string.IsNullOrEmpty(inputStatement))
                    throw new Exception("No se encontró la sentencia INPUT en el fichero SAS.");

                // 2. Obtener lista de todas las variables en el INPUT
                List<string> allVariables = GetAllVariablesFromInput(inputStatement);

                return allVariables;
            }
        }

        /* Descripción:
         *  Importa la tabla de observaciones desde un fichero .sas (con bloque datalines)
         *  y devuelve un objeto multifaceta para la variable dependiente especificada.
         *  
         * Parámetros:
         *  path: Ruta al fichero .sas
         *  facetVariables: Lista de TODAS las variables que serán tratadas como facetas
         *  dependentVariable: La variable dependiente que se usará como medición
         *  
         * Nota: Cualquier variable en el INPUT que no esté en facetVariables ni sea dependentVariable
         *       se ignora (son otras variables dependientes no seleccionadas).
         */
        public static MultiFacetsObs ImportSAS_to_MultiFacetsObs(string path,
                                                                   List<string> facetVariables,
                                                                   string dependentVariable)
        {
            MultiFacetsObs retVal = null;
            using (StreamReader reader = new StreamReader(path))
            {
                retVal = ParseSAS_to_MultiFacetsObs(reader, path, facetVariables, dependentVariable);
            }
            return retVal;
        }

        /***************************************************************************************************
         * MÉTODOS PRIVADOS
         ***************************************************************************************************/

        private static MultiFacetsObs ParseSAS_to_MultiFacetsObs(StreamReader reader,
                                                                   string path,
                                                                   List<string> facetVariables,
                                                                   string dependentVariable)
        {
            string sasContent = reader.ReadToEnd();

            // 1. Extraer la sentencia INPUT para conocer todas las variables
            string inputStatement = ExtractInputStatement(sasContent);
            if (string.IsNullOrEmpty(inputStatement))
                throw new Exception("No se encontró la sentencia INPUT en el fichero SAS.");

            // 2. Obtener lista de todas las variables en el INPUT (en orden)
            List<string> allVariables = GetAllVariablesFromInput(inputStatement);

            /*
            // 3. Validaciones
            //    - La variable dependiente debe existir en el INPUT
            if (!allVariables.Contains(dependentVariable, StringComparer.InvariantCultureIgnoreCase))
                throw new Exception($"La variable dependiente '{dependentVariable}' no existe en la sentencia INPUT.");

            //    - Todas las facetas especificadas deben existir en el INPUT
            var missingFacets = facetVariables
                .Where(f => !allVariables.Contains(f, StringComparer.InvariantCultureIgnoreCase))
                .ToList();
            if (missingFacets.Any())
                throw new Exception($"Las siguientes facetas no existen en la sentencia INPUT: {string.Join(", ", missingFacets)}");

            //    - La variable dependiente no debe estar en la lista de facetas
            if (facetVariables.Contains(dependentVariable, StringComparer.InvariantCultureIgnoreCase))
                throw new Exception($"La variable dependiente '{dependentVariable}' no puede ser también una faceta.");
            */

            // 4. Determinar qué variables se ignoran (otras dependientes no seleccionadas)
            var facetSet = new HashSet<string>(facetVariables, StringComparer.InvariantCultureIgnoreCase);
            var ignoredVariables = allVariables
                .Where(v => !facetSet.Contains(v) && 
                           !string.Equals(v, dependentVariable, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            int numFacets = facetVariables.Count;

            // 5. Extraer el bloque de datos (datalines)
            string datalinesBlock = ExtractDatalinesBlock(sasContent);
            if (string.IsNullOrEmpty(datalinesBlock))
                throw new Exception("No se encontró el bloque DATALINES en el fichero SAS.");

            // 6. Determinar el orden de las columnas en el INPUT
            Dictionary<string, int> variablePosition = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            for (int i = 0; i < allVariables.Count; i++)
            {
                variablePosition[allVariables[i]] = i;
            }

            // 7. Parsear las líneas de datos
            string[] lines = datalinesBlock.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Estructuras para almacenar niveles únicos por faceta
            Dictionary<string, int>[] facetLevelDicts = new Dictionary<string, int>[numFacets];
            for (int i = 0; i < numFacets; i++)
                facetLevelDicts[i] = new Dictionary<string, int>();

            // Tabla de observaciones
            ObsTable tableObs = new ObsTable();

            // Posición de la variable dependiente seleccionada
            int dependentPosition = variablePosition[dependentVariable];
            
            // Posiciones de las facetas (en el orden especificado por el usuario)
            int[] facetPositions = facetVariables.Select(f => variablePosition[f]).ToArray();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith(";"))
                    continue;

                // Dividir por espacios o tabs (separación estándar en SAS)
                string[] tokens = Regex.Split(line.Trim(), @"\s+");

                // Verificar que la línea tiene suficientes columnas
                if (tokens.Length < allVariables.Count)
                {
                    // Algunas líneas podrían tener valores faltantes; se omite o registra advertencia
                    continue;
                }

                List<double?> row = new List<double?>();

                // Procesar cada faceta (según su posición real en el INPUT)
                for (int i = 0; i < numFacets; i++)
                {
                    int pos = facetPositions[i];
                    string facetValue = tokens[pos];
                    int levelId;
                    if (!facetLevelDicts[i].ContainsKey(facetValue))
                    {
                        levelId = facetLevelDicts[i].Count + 1;
                        facetLevelDicts[i].Add(facetValue, levelId);
                    }
                    else
                    {
                        levelId = facetLevelDicts[i][facetValue];
                    }
                    row.Add(levelId);
                }

                // Procesar variable dependiente seleccionada
                if (double.TryParse(tokens[dependentPosition],
                                    System.Globalization.NumberStyles.Any,
                                    System.Globalization.CultureInfo.InvariantCulture,
                                    out double depValue))
                {
                    row.Add(depValue);
                }
                else
                {
                    throw new Exception($"El valor de la variable dependiente '{dependentVariable}' en la línea '{line}' no es un número válido.");
                }

                tableObs.Add(row);
            }

            // 8. Construir objetos Facet
            ListFacets lf = new ListFacets();
            for (int i = 0; i < numFacets; i++)
            {
                Facet f = new Facet(facetVariables[i], facetLevelDicts[i].Count);
                lf.Add(f);
            }

            // 9. Crear MultiFacetsObs (producto cartesiano o no)
            MultiFacetsObs retVal;
            int expectedRows = (int)lf.MultOfLevels();
            if (expectedRows == tableObs.TableRows())
            {
                retVal = new MultiFacetsObs(lf, tableObs, path, dependentVariable, "");
            }
            else
            {
                retVal = new MultiFacetsObs(lf, path, dependentVariable, "");
                InterfaceObsTable obsT = retVal.ObservationTable();
                int n_items = tableObs.TableRows();

                int pos = 0;
                for (int i = 0; (i < expectedRows) && (pos < n_items); i++)
                {
                    bool match = true;
                    for (int j = 0; j < numFacets && match; j++)
                    {
                        match = obsT.Data(i, j).Equals(tableObs.Data(pos, j));
                    }
                    if (match)
                    {
                        double? d = tableObs.ObsData(pos);
                        if (d.HasValue)
                            obsT.Data(d.Value, i);
                        pos++;
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// Extrae la sentencia INPUT completa (hasta el punto y coma).
        /// </summary>
        private static string ExtractInputStatement(string sasContent)
        {
            var inputMatch = Regex.Match(sasContent, @"\binput\b\s+(.*?);", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return inputMatch.Success ? inputMatch.Groups[1].Value.Trim() : null;
        }

        /// <summary>
        /// Obtiene la lista de TODAS las variables (facetas + dependientes)
        /// desde la sentencia INPUT, manteniendo el orden original.
        /// </summary>
        private static List<string> GetAllVariablesFromInput(string inputStatement)
        {
            List<string> variables = new List<string>();

            // Dividir por espacios, tabs, o comas
            string[] parts = Regex.Split(inputStatement, @"[\s,]+");

            foreach (string part in parts)
            {
                if (string.IsNullOrWhiteSpace(part))
                    continue;

                // Eliminar posibles especificaciones de formato (ej. $, 5., etc.)
                string varName = Regex.Replace(part, @"\$", "");
                varName = Regex.Replace(varName, @"\d+\.?\d*", "").Trim();

                // Eliminar caracteres no alfabéticos al inicio/fin
                varName = Regex.Replace(varName, @"^[^a-zA-Z]+|[^a-zA-Z0-9_]+$", "");

                if (!string.IsNullOrEmpty(varName) && !variables.Contains(varName, StringComparer.InvariantCultureIgnoreCase))
                {
                    variables.Add(varName);
                }
            }

            return variables;
        }

        /// <summary>
        /// Extrae el bloque entre 'datalines;' y el punto y coma solitario que lo cierra.
        /// </summary>
        private static string ExtractDatalinesBlock(string sasContent)
        {
            string pattern = @"\b(datalines|cards)\s*;\s*(.*?)\s*;";
            var match = Regex.Match(sasContent, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (match.Success)
            {
                return match.Groups[2].Value.Trim();
            }
            return null;
        }
    }
}