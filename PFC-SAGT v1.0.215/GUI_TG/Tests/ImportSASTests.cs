using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using MultiFacetData;

namespace Tests
{
    public class ImportSASTests
    {
        private readonly List<string> _tempFiles = new List<string>();

        public void Dispose()
        {
            foreach (var file in _tempFiles)
            {
                File.Delete(file);
            }
        }

        private string CreateTempSasFile(string content)
        {
            string path = Path.GetTempFileName() + ".sas";
            File.WriteAllText(path, content);
            _tempFiles.Add(path);
            return path;
        }


        // --------------------------------------------------------------------------------
        // isTextVariable tests
        // --------------------------------------------------------------------------------
        [Fact]
        public void IsTextVariable_VariableWithDollarSign_ReturnsTrue()
        {
            string sas = "data test;\ninput name $ age height;\ndatalines;\nJohn 30 180\n;";
            string path = CreateTempSasFile(sas);

            bool result = ImportSAS.isTextVariable("name", path);

            Assert.True(result);
        }

        [Fact]
        public void IsTextVariable_VariableWithDollarFormat_ReturnsTrue()
        {
            string sas = "data test;\ninput name $15. age height;\ndatalines;\nJohn 30 180\n;";
            string path = CreateTempSasFile(sas);

            bool result = ImportSAS.isTextVariable("name", path);

            Assert.True(result);
        }

        [Fact]
        public void IsTextVariable_NumericVariable_ReturnsFalse()
        {
            string sas = "data test;\ninput name $ age height;\ndatalines;\nJohn 30 180\n;";
            string path = CreateTempSasFile(sas);

            bool result = ImportSAS.isTextVariable("age", path);

            Assert.False(result);
        }

        // --------------------------------------------------------------------------------
        // ReadColumns tests
        // --------------------------------------------------------------------------------
        [Fact]
        public void ReadColumns_ReturnsAllVariableNamesStrippedOfFormats()
        {
            string sas = "data test;\ninput a b $ c $20. d;\ndatalines;\n1 x 2 3\n;";
            string path = CreateTempSasFile(sas);

            List<string> columns = ImportSAS.ReadColumns(path);

            Assert.Equal(new[] { "a", "b", "c", "d" }, columns);
        }

        [Fact]
        public void ReadColumns_NoInputStatement_ThrowsException()
        {
            string sas = "data test;\nx y z;\ndatalines;\n1 2 3\n;"; // no INPUT
            string path = CreateTempSasFile(sas);

            var ex = Assert.Throws<Exception>(() => ImportSAS.ReadColumns(path));
            Assert.Contains("No se encontró la sentencia INPUT", ex.Message);
        }

        // --------------------------------------------------------------------------------
        // BuildInfo
        // --------------------------------------------------------------------------------
        [Fact]
        public void ImportSAS_BuildInfo_ContainsLevelMappings()
        {
            string sas = @"
data test;
input a b y;
datalines;
asdf 1 10
qwerty 5 20
;
";
            string path = CreateTempSasFile(sas);

            MultiFacetsObs mfo = ImportSAS.ImportSAS_to_MultiFacetsObs(
                path,
                new List<string> { "a", "b" },
                "y");

            string comment = mfo.Comment();
            Assert.Contains("a:", comment);
            Assert.Contains("\tasdf = 1", comment);
            Assert.Contains("\tqwerty = 2", comment);
            Assert.Contains("b:", comment);
            Assert.Contains("\t1 = 1", comment);
            Assert.Contains("\t5 = 2", comment);
        }



        // --------------------------------------------------------------------------------
        // ImportSAS_to_MultiFacetsObs tests – base and arithmetic mean
        // --------------------------------------------------------------------------------
        [Fact]
        public void ImportSAS_BaseCase_SingleObservationPerCombination()
        {
            string sas = @"
data test;
input a b y;
datalines;
1 1 10
1 2 20
2 1 30
2 2 40
;
";
            string path = CreateTempSasFile(sas);

            MultiFacetsObs mfo = ImportSAS.ImportSAS_to_MultiFacetsObs(
                path,
                new List<string> { "a", "b" },
                "y");

            // Facets: a (2 levels), b (2 levels)
            Assert.Equal(2, mfo.ListFacets().Count());
            Assert.Equal("a", mfo.ListFacets().FacetInPos(0).Name());
            Assert.Equal(2, mfo.ListFacets().FacetInPos(0).Level());
            Assert.Equal("b", mfo.ListFacets().FacetInPos(1).Name());
            Assert.Equal(2, mfo.ListFacets().FacetInPos(1).Level());

            // Table: 4 rows, last column = y values in order (a slowest, b fastest)
            var table = mfo.ObservationTable();
            Assert.Equal(4, table.TableRows());
            Assert.Equal(10.0, table.Data(0, 2)); // (1,1)
            Assert.Equal(20.0, table.Data(1, 2)); // (1,2)
            Assert.Equal(30.0, table.Data(2, 2)); // (2,1)
            Assert.Equal(40.0, table.Data(3, 2)); // (2,2)
        }

        [Fact]
        public void ImportSAS_MultipleObservationsForSameFacets_AveragesCorrectly()
        {
            // Two rows for (a=1,b=1): 10 and 12 → mean 11.
            string sas = @"
data test;
input a b y;
datalines;
1 1 10
1 1 12
1 2 20
2 1 30
2 2 40
;
";
            string path = CreateTempSasFile(sas);

            MultiFacetsObs mfo = ImportSAS.ImportSAS_to_MultiFacetsObs(
                path,
                new List<string> { "a", "b" },
                "y");

            var table = mfo.ObservationTable();
            Assert.Equal(4, table.TableRows());
            Assert.Equal(11.0, table.Data(0, 2)); // averaged
            Assert.Equal(20.0, table.Data(1, 2));
            Assert.Equal(30.0, table.Data(2, 2));
            Assert.Equal(40.0, table.Data(3, 2));
        }

        // --------------------------------------------------------------------------------
        // Facet order switch
        // --------------------------------------------------------------------------------
        [Fact]
        public void ImportSAS_FacetOrderSwitch_RespectsUserSpecifiedOrder()
        {
            // Input order: a b y. User asks for (b, a).
            string sas = @"
data test;
input a b y;
datalines;
1 1 10
1 2 20
2 1 30
2 2 40
;
";
            string path = CreateTempSasFile(sas);

            MultiFacetsObs mfo = ImportSAS.ImportSAS_to_MultiFacetsObs(
                path,
                new List<string> { "b", "a" },
                "y");

            // Facet order: b first, a second
            Assert.Equal("b", mfo.ListFacets().FacetInPos(0).Name());
            Assert.Equal("a", mfo.ListFacets().FacetInPos(1).Name());

            // Product order: (b=1,a=1), (b=1,a=2), (b=2,a=1), (b=2,a=2)
            // Values from input: b=1,a=1 → 10; b=1,a=2 → 30; b=2,a=1 → 20; b=2,a=2 → 40
            var table = mfo.ObservationTable();
            Assert.Equal(10.0, table.Data(0, 2));
            Assert.Equal(30.0, table.Data(1, 2));
            Assert.Equal(20.0, table.Data(2, 2));
            Assert.Equal(40.0, table.Data(3, 2));
        }

        [Fact]
        public void ImportSAS_MeasurementVariableInTheMiddle_WorksCorrectly()
        {
            // y is in the middle of INPUT
            string sas = @"
data test;
input a y b;
datalines;
1 10 1
1 20 2
2 30 1
2 40 2
;
";
            string path = CreateTempSasFile(sas);

            MultiFacetsObs mfo = ImportSAS.ImportSAS_to_MultiFacetsObs(
                path,
                new List<string> { "a", "b" },
                "y");

            Assert.Equal(2, mfo.ListFacets().Count());
            var table = mfo.ObservationTable();
            Assert.Equal(10.0, table.Data(0, 2));
            Assert.Equal(20.0, table.Data(1, 2));
            Assert.Equal(30.0, table.Data(2, 2));
            Assert.Equal(40.0, table.Data(3, 2));
        }

        // --------------------------------------------------------------------------------
        // Collapsing facets (omitting some) and ignoring extra measurement variables
        // --------------------------------------------------------------------------------
        [Fact]
        public void ImportSAS_OmitSomeFacets_AveragesOverThem()
        {
            // a, b, c, y. Keep a and b as facets → collapse c (averaged over)
            string sas = @"
data test;
input a b c y;
datalines;
1 1 1 10
1 1 2 20
1 2 1 30
2 1 1 40
2 2 1 50
;
";
            string path = CreateTempSasFile(sas);

            MultiFacetsObs mfo = ImportSAS.ImportSAS_to_MultiFacetsObs(
                path,
                new List<string> { "a", "b" },   // only a and b as facets
                "y");

            Assert.Equal(2, mfo.ListFacets().Count());          // we must keep at least 2 facets
                                                                // a=1,b=1: values 10 and 20 → mean 15
                                                                // a=1,b=2: value 30 → mean 30
                                                                // a=2,b=1: value 40 → mean 40
                                                                // a=2,b=2: value 50 → mean 50
            var table = mfo.ObservationTable();
            Assert.Equal(4, table.TableRows());                 // 2x2 cartesian product
            Assert.Equal(15.0, table.Data(0, 2));               // (1,1)
            Assert.Equal(30.0, table.Data(1, 2));               // (1,2)
            Assert.Equal(40.0, table.Data(2, 2));               // (2,1)
            Assert.Equal(50.0, table.Data(3, 2));               // (2,2)
        }

        [Fact]
        public void ImportSAS_IgnoredMeasurementVariable_IsNotUsed()
        {
            // SAS has y and z; we import y, z is ignored.
            string sas = @"
data test;
input a b y z;
datalines;
1 1 10 100
1 2 20 200
2 1 30 300
2 2 40 400
;
";
            string path = CreateTempSasFile(sas);

            MultiFacetsObs mfo = ImportSAS.ImportSAS_to_MultiFacetsObs(
                path,
                new List<string> { "a", "b" },
                "y");

            var table = mfo.ObservationTable();
            Assert.Equal(10.0, table.Data(0, 2));
            Assert.Equal(20.0, table.Data(1, 2));
            Assert.Equal(30.0, table.Data(2, 2));
            Assert.Equal(40.0, table.Data(3, 2));
        }

        // --------------------------------------------------------------------------------
        // Edge cases and exceptions
        // --------------------------------------------------------------------------------
        [Fact]
        public void ImportSAS_DependentVariableNotInInput_Throws()
        {
            string sas = @"
data test;
input a b y;
datalines;
1 1 10
;
";
            string path = CreateTempSasFile(sas);

            var ex = Assert.Throws<Exception>(() =>
                ImportSAS.ImportSAS_to_MultiFacetsObs(path, new List<string> { "a", "b" }, "nonexistent"));
            Assert.Contains("no existe en la sentencia INPUT", ex.Message);
        }

        [Fact]
        public void ImportSAS_FacetNotInInput_Throws()
        {
            string sas = @"
data test;
input a b y;
datalines;
1 1 10
;
";
            string path = CreateTempSasFile(sas);

            var ex = Assert.Throws<Exception>(() =>
                ImportSAS.ImportSAS_to_MultiFacetsObs(path, new List<string> { "a", "x" }, "y"));
            Assert.Contains("no existen en la sentencia INPUT", ex.Message);
            Assert.Contains("x", ex.Message);
        }

        [Fact]
        public void ImportSAS_DependentVariableAlsoFacet_Throws()
        {
            string sas = @"
data test;
input a b y;
datalines;
1 1 10
;
";
            string path = CreateTempSasFile(sas);

            var ex = Assert.Throws<Exception>(() =>
                ImportSAS.ImportSAS_to_MultiFacetsObs(path, new List<string> { "a", "y" }, "y"));
            Assert.Contains("no puede ser también una faceta", ex.Message);
        }

        [Fact]
        public void ImportSAS_MissingDatalinesBlock_Throws()
        {
            string sas = @"
data test;
input a b y;
";
            string path = CreateTempSasFile(sas);

            var ex = Assert.Throws<Exception>(() =>
                ImportSAS.ImportSAS_to_MultiFacetsObs(path, new List<string> { "a", "b" }, "y"));
            Assert.Contains("No se encontró el bloque DATALINES", ex.Message);
        }

        [Fact]
        public void ImportSAS_MultipleDatasets_ImportsTheFirst()
        {
            string sas = @"
data first;
input a b y;
datalines;
1 1 10
2 2 20
;
data second;
input x z w;
datalines;
100 200 300
;
";
            string path = CreateTempSasFile(sas);

            // Import facets (a,b) and dependent variable y from the first dataset.
            MultiFacetsObs mfo = ImportSAS.ImportSAS_to_MultiFacetsObs(
                path,
                new List<string> { "a", "b" },
                "y");

            // Verify facets from first dataset only.
            Assert.Equal(2, mfo.ListFacets().Count());
            Assert.Equal("a", mfo.ListFacets().FacetInPos(0).Name());
            Assert.Equal("b", mfo.ListFacets().FacetInPos(1).Name());
            Assert.Equal(2, mfo.ListFacets().FacetInPos(0).Level());
            Assert.Equal(2, mfo.ListFacets().FacetInPos(1).Level());

            // Verify data values from first dataset only.
            var table = mfo.ObservationTable();
            Assert.Equal(4, table.TableRows());
            Assert.Equal(10.0, table.Data(0, 2)); // (a=1,b=1)
            Assert.Null(table.Data(1, 2));        // (a=1,b=2)
            Assert.Equal(20.0, table.Data(3, 2)); // (a=2,b=2)
        }

        [Fact]
        public void ImportSAS_VariableNameWithDigits_PreservesDigits()
        {
            // b1 is a valid SAS variable name with a digit
            string sas = @"
data test;
input a b1 y;
datalines;
1 1 10
2 2 20
;
";
            string path = CreateTempSasFile(sas);

            MultiFacetsObs mfo = ImportSAS.ImportSAS_to_MultiFacetsObs(
                path,
                new List<string> { "a", "b1" },
                "y");

            Assert.Equal("b1", mfo.ListFacets().FacetInPos(1).Name());
        }

        [Fact]
        public void ImportSAS_MissingDependentValue_SkipsRow()
        {
            // y contains a dot (missing) in one row
            string sas = @"
data test;
input a b y;
datalines;
1 1 10
1 2 .
2 1 30
2 2 40
;
";
            string path = CreateTempSasFile(sas);

            MultiFacetsObs mfo = ImportSAS.ImportSAS_to_MultiFacetsObs(
                path,
                new List<string> { "a", "b" },
                "y");

            // The row with missing y is skipped → for (a=1,b=2) there is no data, so the mean should be null
            var table = mfo.ObservationTable();
            Assert.Equal(4, table.TableRows());
            Assert.Equal(10.0, table.Data(0, 2)); // (1,1)
            Assert.Null(table.Data(1, 2));        // (1,2) – no observation, mean is null
            Assert.Equal(30.0, table.Data(2, 2)); // (2,1)
            Assert.Equal(40.0, table.Data(3, 2)); // (2,2)
        }

        [Fact]
        public void ImportSAS_DatalinesBlock_EndsWithSemicolonOnOwnLine()
        {
            // Trailing ; immediately after data, no newline – still parsed
            string sas = "data test;\ninput a b y;\ndatalines;\n1 1 10\n;";
            string path = CreateTempSasFile(sas);

            MultiFacetsObs mfo = ImportSAS.ImportSAS_to_MultiFacetsObs(
                path,
                new List<string> { "a", "b" },
                "y");

            var table = mfo.ObservationTable();
            Assert.Equal(1, table.TableRows()); // only one row
            Assert.Equal(10.0, table.Data(0, 2));
        }

        [Fact]
        public void ImportSAS_TrailingAtSign_ThrowsNotSupportedException()
        {
            string sas = @"
data test;
input a b y @@;
datalines;
1 1 10 2 2 20
;
";
            string path = CreateTempSasFile(sas);

            var ex = Assert.Throws<Exception>(() =>
                ImportSAS.ImportSAS_to_MultiFacetsObs(path, new List<string> { "a", "b" }, "y"));

            Assert.Contains("@@", ex.Message);
        }
    }
}
