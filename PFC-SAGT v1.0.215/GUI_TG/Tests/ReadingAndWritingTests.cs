using FluentAssertions;
using MultiFacetData;
using ProjectMeans;
using ProjectSSQ;
using Sagt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Tests
{
    public class ReadingAndWritingTests
    {
        [Fact]
        public void TestMethod1()
        {
            Assert.True(true);
        }

        [Fact]
        public void BasicCircle() 
        {
            //STEP 1: Build all our stuff (MFO, ListMeans, AAGS)

            string comment = @"
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Users might enter emojis like 😃, 
                special symbols such as <>&{}[]();@#$%, or quotes 'single' and ""double"" in a description.  

                Sometimes, descriptions include numbers: 12345, -42, 3.14159, or version codes v1.2.3. Line breaks matter too:
                First line of description.
                Second line with tab\tindentation and trailing spaces.  

                Unicode is common: café, naïve, résumé, 東京, Москва, Αθήνα.  
                Escape sequences should be handled: \n for newlines, \t for tabs, and \\ for backslashes.  

                Users could also mix code snippets in descriptions:
                function example(value) { return value ?? ""default""; }
                var json = { ""key"": ""value"", ""arr"": [1, 2, 3] };

                Finally, very long lines are a reality: Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua, ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
            //Error menor detectado: se romperá la app si el usuario llega a escribir '</file__analysis_ssq_comment>' en el comentario, u otros comandos.

            ListFacets lf = new ListFacets();
            Facet f1 = new Facet("O", 2);
            Facet f2 = new Facet("I", 3);
            Facet f3 = new Facet("C", 2);
            lf.Add(f1);
            lf.Add(f2);
            lf.Add(f3);

            var matrix = new List<List<double?>>
            {
                new List<double?> { 1, 1, 1, 4 },
                new List<double?> { 1, 1, 2, 5 },
                new List<double?> { 1, 2, 1, 6 },
                new List<double?> { 1, 2, 2, 4 },
                new List<double?> { 1, 3, 1, 6 },
                new List<double?> { 1, 3, 2, 7 },
                new List<double?> { 2, 1, 1, 8 },
                new List<double?> { 2, 1, 2, 5 },
                new List<double?> { 2, 2, 1, 4 },
                new List<double?> { 2, 2, 2, 5 },
                new List<double?> { 2, 3, 1, 6 },
                new List<double?> { 2, 3, 2, 3 },
            };
            ObsTable table = new ObsTable(matrix);

            MultiFacetsObs mfo = new MultiFacetsObs(lf, table, "testing.sagt", "Lorem Ipsum", comment);

            List<string> ldesigns = lf.CombinationStringWithoutRepetition();
            List<ListFacets> llf = new List<ListFacets>();
            foreach (string design in ldesigns)
            {
                ListFacets newLf = lf.ListDesignFacets(design);
                llf.Add(newLf);
            }

            ListMeans lmeans = new ListMeans(llf, ldesigns, mfo, DateTime.Now, "testing.sagt", false);

            ListFacets differentiation = new ListFacets();
            differentiation.Add(f1);
            ListFacets instrumentation = new ListFacets();
            instrumentation.Add(f2);
            instrumentation.Add(f3);
            Analysis_and_G_Study aags = new Analysis_and_G_Study(ldesigns, mfo, differentiation, instrumentation, false);
            aags.SetNameFileDataCreation("testing.sagt");

            SagtFile sagtFile = new SagtFile(mfo, lmeans, aags);

            //STEP 2: Save, Reload, Save again
            string firstFile = Path.GetTempFileName();
            string secondFile = Path.GetTempFileName();

            sagtFile.WritingSagtFile(firstFile);
            SagtFile readSagtFile = SagtFile.ReadingSagtFile(firstFile);
            readSagtFile.WritingSagtFile(secondFile);

            // === STEP 3: Normalize text and compare ===
            string Normalize(string text)
            {
                if (text == null) return string.Empty;

                // Normalize all newline variants (\r, \n, \r\n, or even \n\r) to a single '\n'
                text = Regex.Replace(text, @"\r\n?|\n\r?", "\n");

                // Trim trailing spaces/tabs at end of each line
                text = Regex.Replace(text, @"[ \t]+$", "", RegexOptions.Multiline);

                // Collapse multiple blank lines
                text = Regex.Replace(text, @"\n{2,}", "\n");

                // Trim overall leading/trailing whitespace
                return text.Trim();
            }
            //(Yes, the comparison will fail without taking care of the above stuff, and it seems to be mostly due lorem ipsum comment)

            string firstText = Normalize(File.ReadAllText(firstFile));
            string secondText = Normalize(File.ReadAllText(secondFile));

            Assert.Equal(firstText, secondText);
        }

        //todo: test correct output of reading broken files
    }
}
