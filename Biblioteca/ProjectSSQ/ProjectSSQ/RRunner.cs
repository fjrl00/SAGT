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
    /* ============================================================================================
     * ARCHITECTURE: bundled R runtime + pre-populated renv library. READ THIS before changing
     * anything in RRunner, and before assuming a "Rscript.exe not found" exception is a bug.
     * ============================================================================================
     *
     * WHY: R itself is not something we can assume the end user has installed. Different R
     * versions/architectures, and missing binary packages for VCA and its dependency tree (lme4,
     * Rcpp, RcppEigen, ...), are a classic "works on my machine" failure mode. To avoid that, the
     * app ships its own private copy of R plus a fully pre-installed package library, and the
     * shipped app never installs or downloads anything on the end user's machine.
     *
     * TWO FOLDERS, BOTH SIBLINGS OF THIS PROJECT'S OUTPUT (i.e. under AppContext.BaseDirectory):
     *
     *   R\      A full, self-contained R installation (e.g. a straight copy of
     *           "C:\Program Files\R\R-x.y.z"). Must contain R\bin\x64\Rscript.exe (see
     *           GetBundledRscriptPath). Must ALSO have the 'renv' package itself pre-installed
     *           into R\library\renv - i.e. R's own base library, NOT a project library - because
     *           renv::load() (see CreateVcaAnovaRScript) can't find renv otherwise.
     *
     *   RSetup\ The renv PROJECT: renv.lock (the pinned dependency list - R version, VCA, and
     *           every transitive dependency, each at an exact version), renv\activate.R, and the
     *           actual installed package files under
     *           RSetup\renv\library\<platform>\<R version>\<arch>\.
     *
     * BOTH ARE GITIGNORED. This is deliberate, not an oversight: a populated library is tens of
     * MB and the R distribution itself is ~190 MB of binaries, neither of which belongs in source
     * control. This means A FRESH CHECKOUT WILL NOT HAVE EITHER FOLDER, and RunVcaAnova will
     * throw a clear exception (see EnsureRscriptAvailable) until a maintainer populates them once,
     * locally. To do that:
     *
     *   1. Copy a full R installation - matching the R version pinned in RSetup\renv.lock - into
     *      R\, e.g.: robocopy "C:\Program Files\R\R-4.5.2" R\ /E
     *   2. Make sure 'renv' is installed into THAT COPY'S OWN library, not just your personal
     *      one. If R\library\renv doesn't already exist, install it there explicitly:
     *        R\bin\x64\Rscript.exe -e "install.packages('renv', lib='R/library')"
     *   3. From inside RSetup\, restore the locked packages:
     *        R\bin\x64\Rscript.exe -e "source('renv/activate.R'); renv::restore(prompt=FALSE)"
     *   4. IMPORTANT - do not skip this: renv's default behaviour is to install packages as
     *      symlinks/junctions into a per-machine cache (usually under
     *      %LOCALAPPDATA%\R\cache\...) rather than as real files inside RSetup\renv\library.
     *      That works fine on your machine and then silently breaks on every other one, since the
     *      folder you ship would contain broken links pointing at a cache that doesn't exist
     *      there. Always follow restore with:
     *        R\bin\x64\Rscript.exe -e "renv::isolate(project='RSetup')"
     *      and double-check no folder under RSetup\renv\library is a reparse point before
     *      shipping (PowerShell: (Get-Item <path>).Attributes should say "Directory", not
     *      "Directory, ReparsePoint").
     *
     * WHY THE RUNTIME ONLY EVER CALLS renv::load(), NEVER renv::restore(): restore() reconciles
     * the library against the lockfile and can install/download from a repository - exactly the
     * online, "works on my machine" behaviour bundling exists to eliminate, and something we do
     * NOT want happening on an end user's machine. load() only points R's library search path
     * (.libPaths()) at the already-populated RSetup\renv\library folder; it never contacts a
     * repository. As long as steps 1-4 above were done correctly, library(VCA) inside the
     * generated script (see CreateVcaAnovaRScript) just works, fully offline, every time.
     *
     * WHEN TO REDO THIS: whenever VCA (or a dependency, or the pinned R version) needs to
     * change - e.g. a bug fix, a security patch, or a new R-based feature. Update
     * RSetup\renv.lock accordingly (renv::install()/renv::snapshot() on a normal, non-bundled R
     * install with internet access), then repeat steps 3-4 above to re-materialize
     * RSetup\renv\library, and re-copy R\ if the R version itself changed.
     * ============================================================================================
     */
    public static class RRunner
    {
        // Project containing the renv lockfile and the (pre-populated) project library.
        // See the ARCHITECTURE comment above for what must live in here and how to (re)populate it.
        private const string RProjectFolderName = "RSetup";

        // Portable R distribution bundled with the application, so that analyses do not depend
        // on the end user having R installed (or having the right version/architecture on PATH).
        // See the ARCHITECTURE comment above for what must live in here and how to (re)populate it.
        private const string RDistributionFolderName = "R";


        /// <summary>
        /// Runs a G-Theory ANOVA analysis (via the R library VCA) on the observation table of the
        /// given multi-facet observations object, and returns the resulting TableAnalysisOfVariance.
        /// </summary>
        public static TableAnalysisOfVariance RunVcaAnova(MultiFacetsObs mfo)
        {
            // ------------------------------------------------------------
            // 1. Locate the bundled R project and R runtime
            // ------------------------------------------------------------

            string rProjectPath = Path.Combine(
                AppContext.BaseDirectory,
                RProjectFolderName);

            string rscriptPath = GetBundledRscriptPath();

            // ------------------------------------------------------------
            // 2. Make sure the bundled Rscript.exe is available
            // ------------------------------------------------------------

            EnsureRscriptAvailable(rscriptPath);

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
                // 7. Execute the generated R script. The script itself activates the bundled,
                //    pre-populated project library via renv::load() (see CreateVcaAnovaRScript) -
                //    this never contacts a package repository, unlike renv::restore(). Packages
                //    are meant to already be materialized under RSetup\renv\library at build time.
                // --------------------------------------------------------

                RunRScript(
                    rscriptPath,
                    scriptPath,
                    csvPath,
                    outputPath,
                    rProjectPath,
                    model);

                // --------------------------------------------------------
                // 8. Build the TableAnalysisOfVariance from the VCA output
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
        internal static string BuildVcaModel(ListFacets listFacets)
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
# Activate the pre-populated project library (RSetup\renv\library).
# Deliberately renv::load(), never renv::restore() - load() only points
# .libPaths() at the already-installed packages and never touches a
# repository. See the ARCHITECTURE comment on the RRunner class (RRunner.cs)
# for why, and for how RSetup\renv\library gets populated in the first place.
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

        /* Descripción:
         *  Devuelve la ruta absoluta al Rscript.exe de la distribución de R incluida con la
         *  aplicación (no se usa el R que pudiera haber instalado el usuario en su sistema).
         */
        private static string GetBundledRscriptPath()
        {
            return Path.Combine(
                AppContext.BaseDirectory,
                RDistributionFolderName,
                "bin",
                "x64",
                "Rscript.exe");
        }


        private static void EnsureRscriptAvailable(string rscriptPath)
        {
            string rDistributionPath = Path.Combine(
                AppContext.BaseDirectory,
                RDistributionFolderName);

            if (!Directory.Exists(rDistributionPath))
            {
                throw new Exception(
                    "The '" + RDistributionFolderName + "' folder (the bundled R runtime) does not exist:\n\n" +
                    rDistributionPath +
                    "\n\n" +
                    "If you are building this application from source, this is expected: the bundled R " +
                    "runtime and its pre-installed package library are large binaries that are intentionally " +
                    "excluded from source control (see .gitignore). They must be populated once, locally, " +
                    "before this feature can run - see the ARCHITECTURE comment above the RRunner class in " +
                    "RRunner.cs for the exact steps.\n\n" +
                    "If you are an end user seeing this in an installed copy of the application, the " +
                    "installation is incomplete or corrupted; try reinstalling the application.");
            }

            if (!File.Exists(rscriptPath))
            {
                throw new Exception(
                    "The bundled R runtime's Rscript.exe was not found at the expected location:\n\n" +
                    rscriptPath +
                    "\n\n" +
                    "The '" + RDistributionFolderName + "' folder exists but does not look like a complete " +
                    "R installation (expected layout: " + RDistributionFolderName + "\\bin\\x64\\Rscript.exe). " +
                    "See the ARCHITECTURE comment above the RRunner class in RRunner.cs for how this folder " +
                    "should be populated.");
            }

            ProcessStartInfo startInfo =
                new ProcessStartInfo();

            startInfo.FileName = rscriptPath;
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
                        "Could not start the R runtime bundled with the application.\n\n" +
                        "The installation may be incomplete or corrupted; try reinstalling the application.",
                        ex);
                }

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new Exception(
                        "The R runtime bundled with the application was found, but could not be started correctly.");
                }
            }
        }


        private static void RunRScript(
            string rscriptPath,
            string scriptPath,
            string csvPath,
            string outputPath,
            string rProjectPath,
            string extraArgument)
        {
            ProcessStartInfo startInfo =
                new ProcessStartInfo();

            startInfo.FileName = rscriptPath;

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
                        "Could not start the R runtime bundled with the application.\n\n" +
                        "The installation may be incomplete or corrupted; try reinstalling the application.",
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