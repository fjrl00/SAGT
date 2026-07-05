using AuxMathCalcGT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MultiFacetData
{
    public class ImportCSV
    {
        /***************************************************************************************************
        * MÉTODOS PÚBLICOS
        ***************************************************************************************************/

        public static List<string> ReadColumns(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string headerLine = reader.ReadLine();
                if (string.IsNullOrEmpty(headerLine))
                    throw new Exception("No se encontró la línea de cabecera en el fichero CSV.");

                char delimiter = DetectDelimiter(headerLine);
                List<string> columns = ParseCSVHeader(headerLine, delimiter);
                return columns;
            }
        }

        /// <summary>
        /// Determines whether a variable in a CSV file is a text (character) variable.
        /// A column is considered text if any of its non‑empty cells cannot be parsed as a number.
        /// Empty cells are ignored (they don't determine type).
        /// </summary>
        public static bool isTextVariable(string variableName, string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                // 1. Read header and find the column index
                string headerLine = reader.ReadLine();
                if (string.IsNullOrEmpty(headerLine))
                    return false;

                char delimiter = DetectDelimiter(headerLine);
                List<string> headers = ParseCSVHeader(headerLine, delimiter);
                int colIndex = headers.IndexOf(variableName);
                if (colIndex < 0)
                    return false;

                // 2. Scan data rows until we find a non‑numeric, non‑empty value
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] tokens = ParseCSVLine(line, delimiter);
                    if (tokens.Length <= colIndex)
                        continue;

                    string value = tokens[colIndex].Trim();

                    // Empty cell → skip, doesn't tell us anything
                    if (string.IsNullOrEmpty(value))
                        continue;

                    // Try parsing as double; if it fails → text column
                    if (!double.TryParse(value,
                                         System.Globalization.NumberStyles.Any,
                                         System.Globalization.CultureInfo.InvariantCulture,
                                         out _))
                    {
                        return true;
                    }
                }
            }

            // All non‑empty cells were numeric → not a text column
            return false;
        }

        /* Descripción:
        *  Importa la tabla de observaciones desde un fichero .csv
        *  y devuelve un objeto multifaceta para la variable dependiente especificada.
        *  
        * Parámetros:
        *  path: Ruta al fichero .csv
        *  facetVariables: Lista de TODAS las variables que serán tratadas como facetas
        *  dependentVariable: La variable dependiente que se usará como medición
        *  
        * Nota: Cualquier variable en el CSV que no esté en facetVariables ni sea dependentVariable
        *       se ignora (son otras variables dependientes no seleccionadas).
        */
        public static MultiFacetsObs ImportCSV_to_MultiFacetsObs(string path,
                                                                List<string> facetVariables,
                                                                string dependentVariable)
        {
            MultiFacetsObs retVal = null;
            using (StreamReader reader = new StreamReader(path))
            {
                retVal = ParseCSV_to_MultiFacetsObs(reader, path, facetVariables, dependentVariable);
            }
            return retVal;
        }

        /***************************************************************************************************
        * MÉTODOS PRIVADOS
        ***************************************************************************************************/

        private static MultiFacetsObs ParseCSV_to_MultiFacetsObs(StreamReader reader,
                                                                string path,
                                                                List<string> facetVariables,
                                                                string dependentVariable)
        {
            // 1. Leer la línea de cabecera y detectar delimitador
            string headerLine = reader.ReadLine();
            if (string.IsNullOrEmpty(headerLine))
                throw new Exception("No se encontró la línea de cabecera en el fichero CSV.");

            char delimiter = DetectDelimiter(headerLine);
            List<string> allVariables = ParseCSVHeader(headerLine, delimiter);
            if (allVariables.Count == 0)
                throw new Exception("No se encontraron columnas en la cabecera del fichero CSV.");

            // 2. Validaciones
            //    - La variable dependiente debe existir en la cabecera
            if (!allVariables.Contains(dependentVariable))
                throw new Exception($"La variable dependiente '{dependentVariable}' no existe en la cabecera del CSV.");

            //    - Todas las facetas especificadas deben existir en la cabecera
            var missingFacets = facetVariables
                .Where(f => !allVariables.Contains(f))
                .ToList();
            if (missingFacets.Any())
                throw new Exception($"Las siguientes facetas no existen en la cabecera del CSV: {string.Join(", ", missingFacets)}");

            //    - La variable dependiente no debe estar en la lista de facetas
            if (facetVariables.Contains(dependentVariable))
                throw new Exception($"La variable dependiente '{dependentVariable}' no puede ser también una faceta.");

            // 3. Determinar qué variables se ignoran
            var facetSet = new HashSet<string>(facetVariables);
            var ignoredVariables = allVariables
                .Where(v => !facetSet.Contains(v) && v != dependentVariable)
                .ToList();

            int numFacets = facetVariables.Count;

            // 4. Determinar el orden de las columnas
            Dictionary<string, int> variablePosition = new Dictionary<string, int>();
            for (int i = 0; i < allVariables.Count; i++)
            {
                variablePosition[allVariables[i]] = i;
            }

            // 5. Estructuras para almacenar niveles únicos por faceta
            Dictionary<string, int>[] facetLevelDicts = new Dictionary<string, int>[numFacets];
            for (int i = 0; i < numFacets; i++)
                facetLevelDicts[i] = new Dictionary<string, int>();

            // Tabla de observaciones
            ObsTable tableObs = new ObsTable();

            // Posición de la variable dependiente seleccionada
            int dependentPosition = variablePosition[dependentVariable];

            // Posiciones de las facetas (en el orden especificado por el usuario)
            int[] facetPositions = facetVariables.Select(f => variablePosition[f]).ToArray();

            // 5.1. Leer y procesar cada línea de datos
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] tokens = ParseCSVLine(line, delimiter);

                // Verificar que la línea tiene suficientes columnas
                if (tokens.Length < allVariables.Count)
                    continue;

                List<double?> row = new List<double?>();

                // Procesar cada faceta (según su posición real)
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
                string depToken = tokens[dependentPosition].Trim();

                // Empty cell → skip this observation
                if (string.IsNullOrEmpty(depToken))
                    continue;

                if (double.TryParse(depToken,
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

            // 5.2. Crear tabla plantilla donde insertar los datos
            ObsTable tableTemplate = new ObsTable(lf);

            // 5.3. Calcular medias usando Statistics
            Statistics[] groups = new Statistics[tableTemplate.TableRows()];
            for (int i = 0; i < groups.Length; i++)
            {
                groups[i] = new Statistics();
            }

            int[] indexRepeats = CartesianProductTable.IndexRepeats(lf.levelOfFacets());

            for (int i = 0; i < tableObs.TableRows(); i++)
            {
                int index = 0;
                for (int j = 0; j < lf.Count(); j++)
                    index += indexRepeats[j] * ((int)tableObs.Data(i, j) - 1);

                groups[index].Add(tableObs.ObsData(i), true);
            }

            // 5.4. Pasar medias a la tabla plantilla
            List<double?> ldata = new List<double?>();
            for (int i = 0; i < tableTemplate.TableRows(); i++)
            {
                ldata.Add(groups[i].Mean());
            }
            tableTemplate.AssignListData(ldata);

            return new MultiFacetsObs(lf, tableTemplate, path, dependentVariable, BuildInfo(facetVariables, facetLevelDicts));
        }

        /// <summary>
        /// Detecta el delimitador del CSV basándose en la línea de cabecera.
        /// Prueba primero con coma (,); si produce menos de 2 columnas, usa punto y coma (;).
        /// </summary>
        private static char DetectDelimiter(string headerLine)
        {
            if (headerLine.Split(',').Length >= 2)
                return ',';
            return ';';
        }

        /// <summary>
        /// Parsea la línea de cabecera del CSV y devuelve la lista de nombres de columna.
        /// </summary>
        private static List<string> ParseCSVHeader(string headerLine, char delimiter)
        {
            List<string> columns = new List<string>();
            string[] parts = headerLine.Split(delimiter);

            foreach (string part in parts)
            {
                string trimmed = part.Trim().Trim('"');
                if (string.IsNullOrEmpty(trimmed))
                    throw new Exception("La cabecera del CSV contiene una columna sin nombre.");

                columns.Add(trimmed);
            }
            return columns;
        }

        /// <summary>
        /// Parsea una línea de datos CSV, manejando comillas opcionales.
        /// </summary>
        private static string[] ParseCSVLine(string line, char delimiter)
        {
            List<string> tokens = new List<string>();
            bool inQuotes = false;
            string currentToken = "";

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == delimiter && !inQuotes)
                {
                    tokens.Add(currentToken.Trim());
                    currentToken = "";
                }
                else
                {
                    currentToken += c;
                }
            }
            tokens.Add(currentToken.Trim());

            return tokens.ToArray();
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
                    sb.AppendLine($"\t{kvp.Key} = {kvp.Value}");    //      [título original] = [ID numérico]
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}// end namespace MultiFacetData
