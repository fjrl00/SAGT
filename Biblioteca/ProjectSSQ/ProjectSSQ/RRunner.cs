using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using MultiFacetData;

namespace ProjectSSQ
{
    public static class RRunner
    {
        private const string RProjectFolderName = "RSetup";


        /// <summary>
        /// Runs a G-Theory ANOVA analysis (via the R library VCA) on the observation table of the
        /// given multi-facet observations object, and returns the resulting TableAnalysisOfVariance.
        /// </summary>
        public static TableAnalysisOfVariance RunVcaAnova(MultiFacetsObs mfo)
        {
            // ------------------------------------------------------------
            // 1. Locate the bundled R project
            // ------------------------------------------------------------

            string rProjectPath = Path.Combine(
                AppContext.BaseDirectory,
                RProjectFolderName);

            // ------------------------------------------------------------
            // 2. Make sure Rscript.exe is available
            // ------------------------------------------------------------

            EnsureRscriptAvailable();

            // ------------------------------------------------------------
            // 3. Create a unique temporary working directory
            // ------------------------------------------------------------

            string workingDirectory = Path.Combine(
                Path.GetTempPath(),
                "SAGT_R",
                Guid.NewGuid().ToString());

            Directory.CreateDirectory(workingDirectory);

            try
            {
                string csvPath = Path.Combine(
                    workingDirectory,
                    "obsTable.csv");

                string scriptPath = Path.Combine(
                    workingDirectory,
                    "anovaVCA.R");

                string outputPath = Path.Combine(
                    workingDirectory,
                    "output.csv");

                // --------------------------------------------------------
                // 4. Generate the dataset as a CSV of the observation table
                // --------------------------------------------------------

                mfo.WritingFileObsTableCsv(csvPath);

                // --------------------------------------------------------
                // 5. Build the anovaVCA model (right-hand side of the formula)
                // --------------------------------------------------------

                string model = BuildVcaModel(mfo.ListFacets());

                // --------------------------------------------------------
                // 6. Create the R script for the ANOVA analysis
                // --------------------------------------------------------

                CreateVcaAnovaRScript(
                    scriptPath,
                    rProjectPath);

                // --------------------------------------------------------
                // 7. Restore the renv environment
                // --------------------------------------------------------

                RestoreRenvEnvironment(rProjectPath);

                // --------------------------------------------------------
                // 8. Execute the generated R script
                // --------------------------------------------------------

                RunRScript(
                    scriptPath,
                    csvPath,
                    outputPath,
                    rProjectPath,
                    model);

                // --------------------------------------------------------
                // 9. Build the TableAnalysisOfVariance from the VCA output
                // --------------------------------------------------------

                return new TableAnalysisOfVariance(mfo.ListFacets(), outputPath, true);
            }
            finally
            {
                // Don't let a cleanup failure hide the actual error.
                try
                {
                    if (Directory.Exists(workingDirectory))
                    {
                        Directory.Delete(
                            workingDirectory,
                            true);
                    }
                }
                catch
                {
                    // Ignore cleanup errors.
                }
            }
        }


        /* Descripción:
         *  Construye el lado derecho de la fórmula de anovaVCA (p. ej. "O + I + C + O:I + O:C + I:C + O:I:C")
         *  a partir de la lista de facetas, traduciendo el formato interno de diseños (con corchetes y ':')
         *  al formato usado por la librería VCA (nombres de facetas separados por ':').
         *  Las fuentes de variación se ordenan de menos facetas implicadas a más.
         */
        private static string BuildVcaModel(ListFacets listFacets)
        {
            List<string> designs = listFacets.CombinationStringWithoutRepetition();

            List<string> orderedDesigns = designs
                .OrderBy(d => CountFacetsInDesign(d))
                .ToList();

            List<string> vcaTerms = orderedDesigns
                .Select(d => TranslateDesignToVcaTerm(listFacets, d))
                .ToList();

            return string.Join(" + ", vcaTerms);
        }


        /* Descripción:
         *  Cuenta el número de facetas implicadas en un diseño (formato con corchetes y ':').
         */
        private static int CountFacetsInDesign(string design)
        {
            char[] delimeterChars = { '[', ']', ':' };
            return design
                .Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries)
                .Length;
        }


        /* Descripción:
         *  Traduce un diseño (formato con corchetes y ':', p.ej. "[O]:[I][C]") al término
         *  correspondiente en notación de la librería VCA (p.ej. "O:I:C"), manteniendo el orden
         *  en el que las facetas aparecen en la lista de facetas original.
         */
        private static string TranslateDesignToVcaTerm(ListFacets listFacets, string design)
        {
            char[] delimeterChars = { '[', ']', ':' };
            HashSet<string> namesInDesign = new HashSet<string>(
                design.Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries));

            List<string> orderedNames = new List<string>();
            int n = listFacets.Count();
            for (int i = 0; i < n; i++)
            {
                Facet f = listFacets.FacetInPos(i);
                if (namesInDesign.Contains(f.Name()))
                {
                    orderedNames.Add(f.Name());
                }
            }

            return string.Join(":", orderedNames);
        }


        private static void CreateVcaAnovaRScript(
            string scriptPath,
            string rProjectPath)
        {
            string projectPathForR =
                rProjectPath.Replace("\\", "/");

            string script = $@"
# ------------------------------------------------------------
# Load the application's isolated renv environment
# ------------------------------------------------------------

renv::load(
    project = ""{EscapeRString(projectPathForR)}"",
    quiet = TRUE
)

# ------------------------------------------------------------
# Load required packages
# ------------------------------------------------------------

library(VCA)

# ------------------------------------------------------------
# Read command-line arguments
# ------------------------------------------------------------

args <- commandArgs(trailingOnly = TRUE)

if (length(args) < 3) {{
    stop(""Expected three arguments: CSV path, output path and model."")
}}

csvPath <- args[1]
outputPath <- args[2]
model <- args[3]

# ------------------------------------------------------------
# Read input
# ------------------------------------------------------------

dat <- read.csv(csvPath)

# ------------------------------------------------------------
# ANOVA analysis with VCA
# ------------------------------------------------------------

fit <- anovaVCA(
    as.formula(paste(""Measurement.Variable ~"", model)),
    Data = dat
)

# ------------------------------------------------------------
# Write result for C#
# ------------------------------------------------------------

write.csv(fit$aov.tab, outputPath)
";

            File.WriteAllText(
                scriptPath,
                script,
                new UTF8Encoding(false));
        }

        private static void EnsureRscriptAvailable()
        {
            ProcessStartInfo startInfo =
                new ProcessStartInfo();

            startInfo.FileName = "Rscript.exe";
            startInfo.Arguments = "--version";

            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;

                try
                {
                    process.Start();
                }
                catch (Win32Exception ex)
                {
                    throw new Exception(
                        "Could not start R because Rscript.exe could not be found.\n\n" +
                        "Please make sure R is installed and that its bin\\x64 " +
                        "directory is included in the PATH environment variable.",
                        ex);
                }

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new Exception(
                        "Rscript.exe was found, but could not be started correctly.");
                }
            }
        }


        private static void RestoreRenvEnvironment(
            string rProjectPath)
        {
            ProcessStartInfo startInfo =
                new ProcessStartInfo();

            startInfo.FileName = "Rscript.exe";

            startInfo.WorkingDirectory = rProjectPath;

            startInfo.Arguments =
                "--vanilla -e " +
                QuoteArgument(
                    "source('renv/activate.R'); " +
                    "renv::restore(prompt=FALSE);");

            startInfo.UseShellExecute = false;

            // Let the R console window be visible.
            startInfo.CreateNoWindow = false;

            // Do NOT redirect output when we want the console to display it.
            startInfo.RedirectStandardOutput = false;
            startInfo.RedirectStandardError = false;

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;

                try
                {
                    process.Start();
                }
                catch (Win32Exception ex)
                {
                    throw new Exception(
                        "Could not start R while restoring the R environment.",
                        ex);
                }

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new Exception(
                        "The R environment could not be restored.\n\n" +
                        "R exited with code " +
                        process.ExitCode +
                        ".");
                }
            }
        }


        private static void RunRScript(
            string scriptPath,
            string csvPath,
            string outputPath,
            string rProjectPath,
            string extraArgument)
        {
            ProcessStartInfo startInfo =
                new ProcessStartInfo();

            startInfo.FileName = "Rscript.exe";

            // Use the R project as the working directory.
            startInfo.WorkingDirectory = rProjectPath;

            startInfo.Arguments =
                "--vanilla " +
                QuoteArgument(scriptPath) +
                " " +
                QuoteArgument(csvPath) +
                " " +
                QuoteArgument(outputPath);

            if (extraArgument != null)
            {
                startInfo.Arguments +=
                    " " +
                    QuoteArgument(extraArgument);
            }

            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;

                try
                {
                    process.Start();
                }
                catch (Win32Exception ex)
                {
                    throw new Exception(
                        "Could not start R because Rscript.exe could not be found.\n\n" +
                        "Please make sure R is installed and that its bin\\x64 " +
                        "directory is included in the PATH environment variable.",
                        ex);
                }

                string output =
                    process.StandardOutput.ReadToEnd();

                string error =
                    process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new Exception(
                        "R script failed with exit code " +
                        process.ExitCode +
                        ".\n\n" +
                        "R output:\n" +
                        output +
                        "\n\n" +
                        "R error:\n" +
                        error);
                }

                // stdout/stderr are now diagnostic only.
            }
        }


        private static string QuoteArgument(
            string argument)
        {
            return "\"" +
                   argument.Replace(
                       "\"",
                       "\\\"") +
                   "\"";
        }


        private static string EscapeRString(
            string value)
        {
            return value
                .Replace("\\", "/")
                .Replace("\"", "\\\"");
        }
    }
}