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

        /* Descripción:
         *  Importa la tabla de observaciones desde un fichero .sas (con bloque datalines)
         *  y devuelve un objeto multifaceta para la variable dependiente especificada.
         *  
         * Parámetros:
         *  path: Ruta al fichero .sas
         *  allDependentVars: Lista de TODAS las variables dependientes (para excluirlas de las facetas)
         *  selectedDependentVar: La variable dependiente que se usará como medición
         */
        public static MultiFacetsObs ImportSAS_to_MultiFacetsObs(string path,
                                                                   List<string> allDependentVars,
                                                                   string selectedDependentVar)
        {
            MultiFacetsObs retVal = null;
            using (StreamReader reader = new StreamReader(path))
            {
                retVal = ParseSAS_to_MultiFacetsObs(reader, path, allDependentVars, selectedDependentVar);
            }
            return retVal;
        }

        /***************************************************************************************************
         * MÉTODOS PRIVADOS
         ***************************************************************************************************/

        private static MultiFacetsObs ParseSAS_to_MultiFacetsObs(StreamReader reader,
                                                                   string path,
                                                                   List<string> allDependentVars,
                                                                   string selectedDependentVar)
        {
            string sasContent = reader.ReadToEnd();

            // 1. Extraer la sentencia INPUT para conocer todas las variables
            string inputStatement = ExtractInputStatement(sasContent);
            if (string.IsNullOrEmpty(inputStatement))
                throw new Exception("No se encontró la sentencia INPUT en el fichero SAS.");

            // 2. Obtener lista de todas las variables en el INPUT
            List<string> allVariables = GetAllVariablesFromInput(inputStatement);

            // 3. Las facetas son todas las variables que NO son dependientes
            List<string> facetVariables = allVariables
                .Where(v => !allDependentVars.Contains(v, StringComparer.InvariantCultureIgnoreCase))
                .ToList();

            // Validar que la variable seleccionada sea una dependiente
            if (!allDependentVars.Contains(selectedDependentVar, StringComparer.InvariantCultureIgnoreCase))
                throw new Exception($"La variable '{selectedDependentVar}' no está en la lista de variables dependientes.");

            int numFacets = facetVariables.Count;

            // 4. Extraer el bloque de datos (datalines)
            string datalinesBlock = ExtractDatalinesBlock(sasContent);
            if (string.IsNullOrEmpty(datalinesBlock))
                throw new Exception("No se encontró el bloque DATALINES en el fichero SAS.");

            // 5. Determinar el orden de las columnas en el INPUT
            //    Necesitamos saber en qué posición está cada variable
            Dictionary<string, int> variablePosition = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            for (int i = 0; i < allVariables.Count; i++)
            {
                variablePosition[allVariables[i]] = i;
            }

            // 6. Parsear las líneas de datos
            string[] lines = datalinesBlock.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Estructuras para almacenar niveles únicos por faceta
            Dictionary<string, int>[] facetLevelDicts = new Dictionary<string, int>[numFacets];
            for (int i = 0; i < numFacets; i++)
                facetLevelDicts[i] = new Dictionary<string, int>();

            // Tabla de observaciones
            ObsTable tableObs = new ObsTable();

            // Posición de la variable dependiente seleccionada
            int selectedDependentPosition = variablePosition[selectedDependentVar];
            // Posiciones de las facetas
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
                    // Algunas líneas podrían tener valores faltantes; se omite o loguea
                    continue;
                }

                List<double?> row = new List<double?>();

                // Procesar cada faceta (según su posición real en el INPUT).
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
                if (double.TryParse(tokens[selectedDependentPosition],
                                    System.Globalization.NumberStyles.Any,
                                    System.Globalization.CultureInfo.InvariantCulture,
                                    out double depValue))
                {
                    row.Add(depValue);
                }
                else
                {
                    row.Add(null); // Valor no válido
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

            // Crear MultiFacetsObs (producto cartesiano o no)
            MultiFacetsObs retVal;
            int expectedRows = (int)lf.MultOfLevels();
            if (expectedRows == tableObs.TableRows())
            {
                retVal = new MultiFacetsObs(lf, tableObs, path, selectedDependentVar, "");
            }
            else
            {
                retVal = new MultiFacetsObs(lf, path, selectedDependentVar, "");
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