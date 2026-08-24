using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSSQ
{
    public static class RRunner
    {

        public static double RunExample()
        {
            string workingDirectory = Path.Combine(
                Path.GetTempPath(),
                "SAGT_R");

            Directory.CreateDirectory(workingDirectory);

            string csvPath = Path.Combine(
                workingDirectory,
                "data.csv");

            string scriptPath = Path.Combine(
                workingDirectory,
                "script.R");

            // 1. Create the CSV
            CreateExampleCsv(csvPath);

            // 2. Create the R script
            CreateExampleRScript(scriptPath);

            // 3. Execute R
            string output = RunRScript(scriptPath, csvPath);

            // 4. Read result
            return double.Parse(
                output.Trim(),
                CultureInfo.InvariantCulture);
        }


        private static void CreateExampleCsv(string csvPath)
        {
            Random random = new Random();

            int value1 = random.Next(1, 11);
            int value2 = random.Next(1, 11);

            using (StreamWriter writer = new StreamWriter(
                csvPath,
                false,
                new UTF8Encoding(false))) // Using Encoding.UTF8 causes an encoding error
            {
                writer.WriteLine("column1");
                writer.WriteLine(value1.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine(value2.ToString(CultureInfo.InvariantCulture));
            }
        }


        private static void CreateExampleRScript(string scriptPath)
        {
            string script = @"
args <- commandArgs(trailingOnly = TRUE)

csvPath <- args[1]

data <- read.csv(csvPath)

result <- sum(data$column1)

cat(result)
";

            File.WriteAllText(
                scriptPath,
                script,
                new UTF8Encoding(false));   // Using Encoding.UTF8 causes an encoding error
        }


        private static string RunRScript(string scriptPath, string csvPath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.FileName = "Rscript.exe";

            startInfo.Arguments =
                "--vanilla " +
                QuoteArgument(scriptPath) + " " +
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
                catch (System.ComponentModel.Win32Exception ex)
                {
                    throw new Exception(
                        "Could not start R because Rscript.exe could not be found.\n\nPlease make sure R is installed and that its bin\\x64 directory is included in the PATH environment variable.",
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
                        $"R script failed with exit code {process.ExitCode}.\n\n" +
                        error);
                }

                return output;
            }
        }


        private static string QuoteArgument(string argument)
        {
            return "\"" +
                   argument.Replace("\"", "\\\"") +
                   "\"";
        }
    }
}
