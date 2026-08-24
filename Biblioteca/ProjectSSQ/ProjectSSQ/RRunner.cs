using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace ProjectSSQ
{
    public static class RRunner
    {
        private const string RProjectFolderName = "RSetup";

        public static double RunExample()
        {
            // ------------------------------------------------------------
            // 1. Locate the bundled R project
            // ------------------------------------------------------------

            string rProjectPath = Path.Combine(
                AppContext.BaseDirectory,
                RProjectFolderName);

            string lockFilePath = Path.Combine(
                rProjectPath,
                "renv.lock");

            string renvActivatePath = Path.Combine(
                rProjectPath,
                "renv",
                "activate.R");

            if (!Directory.Exists(rProjectPath))
            {
                throw new DirectoryNotFoundException(
                    "The application's R project directory could not be found:\n\n" +
                    rProjectPath);
            }

            if (!File.Exists(lockFilePath))
            {
                throw new FileNotFoundException(
                    "The R environment lockfile could not be found:\n\n" +
                    lockFilePath);
            }

            if (!File.Exists(renvActivatePath))
            {
                throw new FileNotFoundException(
                    "The renv activation script could not be found:\n\n" +
                    renvActivatePath);
            }

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
                    "data.csv");

                string scriptPath = Path.Combine(
                    workingDirectory,
                    "script.R");

                // --------------------------------------------------------
                // 4. Create dynamic input
                // --------------------------------------------------------

                CreateExampleCsv(csvPath);

                // --------------------------------------------------------
                // 5. Create dynamic R script
                // --------------------------------------------------------

                CreateExampleRScript(
                    scriptPath,
                    rProjectPath);

                // --------------------------------------------------------
                // 6. Restore the renv environment
                // --------------------------------------------------------

                RestoreRenvEnvironment(rProjectPath);

                // --------------------------------------------------------
                // 7. Execute the generated R script
                // --------------------------------------------------------

                string output = RunRScript(
                    scriptPath,
                    csvPath,
                    rProjectPath);

                // --------------------------------------------------------
                // 8. Read result
                // --------------------------------------------------------

                return double.Parse(output.Trim(), CultureInfo.InvariantCulture);
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


        private static void CreateExampleCsv(
            string csvPath)
        {
            Random random = new Random();

            int value1 = random.Next(1, 11);
            int value2 = random.Next(1, 11);

            using (StreamWriter writer =
                   new StreamWriter(
                       csvPath,
                       false,
                       new UTF8Encoding(false)))
            {
                writer.WriteLine("column1");

                writer.WriteLine(
                    value1.ToString(
                        CultureInfo.InvariantCulture));

                writer.WriteLine(
                    value2.ToString(
                        CultureInfo.InvariantCulture));
            }
        }


        private static void CreateExampleRScript(
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

if (length(args) < 1) {{
    stop(""No CSV path was supplied to the R script."")
}}

csvPath <- args[1]

# ------------------------------------------------------------
# Read input
# ------------------------------------------------------------

data <- read.csv(csvPath)

# ------------------------------------------------------------
# Example calculation
# ------------------------------------------------------------

result <- sum(data$column1)

# ------------------------------------------------------------
# Return result to C#
# ------------------------------------------------------------

cat(result)
";

            File.WriteAllText(
                scriptPath,
                script,
                new UTF8Encoding(false));
        }


        private static string RunRScript(
            string scriptPath,
            string csvPath,
            string rProjectPath)
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
                QuoteArgument(csvPath);

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

                return output;
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