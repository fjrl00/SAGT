using AuxMathCalcGT;
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

        /// <summary>
        /// Determines whether a variable in a SAS INPUT statement is a text (character) variable.
        /// Handles formats like $, $15., $CHAR10., etc.
        /// </summary>
        /// <returns>True if the variable is a text/character variable, false otherwise.</returns>
        public static bool isTextVariable(string variableName, string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string sasContent = reader.ReadToEnd();

                string inputStatement = ExtractInputStatement(sasContent);
                if (string.IsNullOrEmpty(inputStatement))
                    throw new Exception("No se encontró la sentencia INPUT en el fichero SAS.");

                return IsTextVariable(variableName, inputStatement);
            }
        }

        private static bool IsTextVariable(string variableName, string inputStatement)
        {
            if (string.IsNullOrWhiteSpace(variableName) || string.IsNullOrWhiteSpace(inputStatement))
                return false;

            // Remove the "input" keyword and the trailing semicolon
            string cleaned = Regex.Replace(inputStatement, @"^\s*input\s+", "", RegexOptions.IgnoreCase);
            cleaned = Regex.Replace(cleaned, @";\s*$", "");
            
            // Split by whitespace and commas
            string[] tokens = Regex.Split(cleaned.Trim(), @"[\s,]+");
            
            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];
                
                // Check if this token is the variable name (case-insensitive)
                if (string.Equals(token, variableName, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Look ahead to check for $ or $format
                    if (i + 1 < tokens.Length)
                    {
                        string nextToken = tokens[i + 1];
                        // $ by itself, or $ followed by format like $15., $CHAR10., etc.
                        if (nextToken.StartsWith("$", StringComparison.InvariantCultureIgnoreCase))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                
                // Skip $ and format tokens (they belong to previous variable)
                if (token.StartsWith("$", StringComparison.InvariantCultureIgnoreCase))
                    continue;
            }
            
            // Variable not found in INPUT statement
            return false;
        }

        /* Descripción:
         *  Importa la primera tabla de observaciones desde un fichero .sas (con bloque datalines)
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

            // 7.1. Sweep inicial convirtiendo el bloque en ObsTable por conveniencia y colectando datos de facetas
            // (Pueden haber múltiples obsesrvaciones para cada combinación de facetas a la que luego debemos hacer la media)
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

                string depToken = tokens[dependentPosition].Trim();
                if (depToken == ".")    // SAS missing numeric value → skip this observation
                    continue;
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

            // Construir objetos Facet
            ListFacets lf = new ListFacets();
            for (int i = 0; i < numFacets; i++)
            {
                Facet f = new Facet(facetVariables[i], facetLevelDicts[i].Count);
                lf.Add(f);
            }

            // 7.2 Crear tabla plantilla donde insertar los datos
            ObsTable tableTemplate = new ObsTable(lf);

            // 7.3 Segundo sweep llenando el array de StatisticsData (inspirado por la función en CartesianProductTable)
            Statistics[] groups = new Statistics[tableTemplate.TableRows()];
            for (int i = 0; i < groups.Length; i++)
            {
                groups[i] = new Statistics();
            }

            int[] indexRepeats = CartesianProductTable.IndexRepeats(lf.levelOfFacets());

            for (int i = 0; i < tableObs.TableRows(); i++)
            {
                //Calculate index of this row in the new table. We can since 
                //row positions in our cartesian product tables are deterministic from their facets' values
                int index = 0;
                for (int j = 0; j < lf.Count(); j++)
                    index += indexRepeats[j]*((int)tableObs.Data(i,j) - 1);

                groups[index].Add(tableObs.ObsData(i), true);
            }

            // 7.4 pasar StatisticsData a la tabla plantilla para conseguir la tabla final
            List<double?> ldata = new List<double?>();
            for (int i = 0; i < tableTemplate.TableRows(); i++)
            {
                ldata.Add(groups[i].Mean());
            }
            tableTemplate.AssignListData(ldata);


            return new MultiFacetsObs(lf, tableTemplate, path, dependentVariable, BuildInfo(facetVariables,facetLevelDicts));
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
            // Detect the line‑hold specifier @@ which is not supported
            if (inputStatement.Contains("@@"))
                throw new Exception("El specificador '@@' en la sentencia Input de SAS no es soportado.");

            List<string> variables = new List<string>();
            // Split by spaces, tabs, and commas
            string[] parts = Regex.Split(inputStatement, @"[\s,]+");

            foreach (string part in parts)
            {
                if (string.IsNullOrWhiteSpace(part))
                    continue;

                // Remove leading/trailing non-identifier characters, but keep digits and underscores
                string varName = Regex.Replace(part, @"^[^a-zA-Z_]+", "");           // strip leading non‑alpha/underscore
                varName = Regex.Replace(varName, @"[^a-zA-Z0-9_]+$", "");           // strip trailing non‑identifier
                                                                                    // Skip tokens that are pure formats (e.g. $, $15., 8.)
                if (varName.Length == 0 || Regex.IsMatch(varName, @"^[\d\.]+$"))
                    continue;

                if (!variables.Contains(varName, StringComparer.InvariantCultureIgnoreCase))
                    variables.Add(varName);
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

        /* Descripción:
         *  Toma el diccionario con los valores y devuelve la leyenda para mostrar en la pestaña de 
         *  información.
         */
        private static string BuildInfo(List<String> arrayHeadersColumns, Dictionary<String, int>[] lDic)
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < lDic.Length; i++)
            {
                sb.AppendLine($"{arrayHeadersColumns[i]}:");        // FacetName:
                foreach (var kvp in lDic[i])
                {
                    sb.AppendLine($"\t{kvp.Key} = {kvp.Value}");    //      [ID numérico] = [título original]
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }//end BuildInfo
    }
}