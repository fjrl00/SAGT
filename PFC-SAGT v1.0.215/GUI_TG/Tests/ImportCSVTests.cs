using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using MultiFacetData;

namespace Tests
{
    public class ImportCSVTests : IDisposable
    {
        private readonly List<string> _tempFiles = new List<string>();

        public void Dispose()
        {
            foreach (var file in _tempFiles)
            {
                try { File.Delete(file); } catch { }
            }
        }

        private string CreateTempCsvFile(string content)
        {
            string path = Path.GetTempFileName() + ".csv";
            File.WriteAllText(path, content);
            _tempFiles.Add(path);
            return path;
        }

        // --------------------------------------------------------------------------------
        // isTextVariable tests
        // --------------------------------------------------------------------------------
        [Fact]
        public void IsTextVariable_ColumnWithText_ReturnsTrue()
        {
            string csv = "name,age,height\nJohn,30,180\nJane,25,165";
            string path = CreateTempCsvFile(csv);

            bool result = ImportCSV.isTextVariable("name", path);

            Assert.True(result);
        }

        [Fact]
        public void IsTextVariable_ColumnAllNumeric_ReturnsFalse()
        {
            string csv = "name,age,height\nJohn,30,180\nJane,25,165";
            string path = CreateTempCsvFile(csv);

            bool result = ImportCSV.isTextVariable("age", path);

            Assert.False(result);
        }

        [Fact]
        public void IsTextVariable_ColumnWithEmptyCellsThenText_ReturnsTrue()
        {
            // Empty cells are skipped; the text further down determines it's text
            string csv = "a,b,c\n,,\n,,\n,hello,";
            string path = CreateTempCsvFile(csv);

            bool result = ImportCSV.isTextVariable("b", path);

            Assert.True(result);
        }

        [Fact]
        public void IsTextVariable_ColumnNotFound_ReturnsFalse()
        {
            string csv = "a,b,c\n1,2,3";
            string path = CreateTempCsvFile(csv);

            bool result = ImportCSV.isTextVariable("nonexistent", path);

            Assert.False(result);
        }

        [Fact]
        public void IsTextVariable_AllCellsEmpty_ReturnsFalse()
        {
            string csv = "a,b,c\n,,\n,,";
            string path = CreateTempCsvFile(csv);

            bool result = ImportCSV.isTextVariable("b", path);

            Assert.False(result);
        }

        // --------------------------------------------------------------------------------
        // ReadColumns tests
        // --------------------------------------------------------------------------------
        [Fact]
        public void ReadColumns_ReturnsAllColumnNames()
        {
            string csv = "a,b,c,d\n1,2,3,4";
            string path = CreateTempCsvFile(csv);

            List<string> columns = ImportCSV.ReadColumns(path);

            Assert.Equal(new[] { "a", "b", "c", "d" }, columns);
        }

        [Fact]
        public void ReadColumns_QuotedHeaders_StripsQuotes()
        {
            string csv = "\"a\",\"b\",\"c\"\n1,2,3";
            string path = CreateTempCsvFile(csv);

            List<string> columns = ImportCSV.ReadColumns(path);

            Assert.Equal(new[] { "a", "b", "c" }, columns);
        }

        [Fact]
        public void ReadColumns_EmptyFile_ThrowsException()
        {
            string csv = "";
            string path = CreateTempCsvFile(csv);

            var ex = Assert.Throws<Exception>(() => ImportCSV.ReadColumns(path));
            Assert.Contains("No se encontró la línea de cabecera", ex.Message);
        }

        [Fact]
        public void ReadColumns_SemicolonDelimiter_Works()
        {
            string csv = "a;b;c\n1;2;3";
            string path = CreateTempCsvFile(csv);

            List<string> columns = ImportCSV.ReadColumns(path);

            Assert.Equal(new[] { "a", "b", "c" }, columns);
        }

        // --------------------------------------------------------------------------------
        // BuildInfo
        // --------------------------------------------------------------------------------
        [Fact]
        public void ImportCSV_BuildInfo_ContainsLevelMappings()
        {
            string csv = "a,b,y\nasdf,1,10\nqwerty,5,20";
            string path = CreateTempCsvFile(csv);

            MultiFacetsObs mfo = ImportCSV.ImportCSV_to_MultiFacetsObs(
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
        // ImportCSV_to_MultiFacetsObs tests – base and arithmetic mean
        // --------------------------------------------------------------------------------
        [Fact]
        public void ImportCSV_BaseCase_SingleObservationPerCombination()
        {
            string csv = "a,b,y\n1,1,10\n1,2,20\n2,1,30\n2,2,40";
            string path = CreateTempCsvFile(csv);

            MultiFacetsObs mfo = ImportCSV.ImportCSV_to_MultiFacetsObs(
                path,
                new List<string> { "a", "b" },
                "y");

            Assert.Equal(2, mfo.ListFacets().Count());
            Assert.Equal("a", mfo.ListFacets().FacetInPos(0).Name());
            Assert.Equal(2, mfo.ListFacets().FacetInPos(0).Level());
            Assert.Equal("b", mfo.ListFacets().FacetInPos(1).Name());
            Assert.Equal(2, mfo.ListFacets().FacetInPos(1).Level());

            var table = mfo.ObservationTable();
            Assert.Equal(4, table.TableRows());
            Assert.Equal(10.0, table.Data(0, 2)); // (1,1)
            Assert.Equal(20.0, table.Data(1, 2)); // (1,2)
            Assert.Equal(30.0, table.Data(2, 2)); // (2,1)
            Assert.Equal(40.0, table.Data(3, 2)); // (2,2)
        }

        [Fact]
        public void ImportCSV_MultipleObservationsForSameFacets_AveragesCorrectly()
        {
            string csv = "a,b,y\n1,1,10\n1,1,12\n1,2,20\n2,1,30\n2,2,40";
            string path = CreateTempCsvFile(csv);

            MultiFacetsObs mfo = ImportCSV.ImportCSV_to_MultiFacetsObs(
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
        public void ImportCSV_FacetOrderSwitch_RespectsUserSpecifiedOrder()
        {
            string csv = "a,b,y\n1,1,10\n1,2,20\n2,1,30\n2,2,40";
            string path = CreateTempCsvFile(csv);

            MultiFacetsObs mfo = ImportCSV.ImportCSV_to_MultiFacetsObs(
                path,
                new List<string> { "b", "a" },
                "y");

            Assert.Equal("b", mfo.ListFacets().FacetInPos(0).Name());
            Assert.Equal("a", mfo.ListFacets().FacetInPos(1).Name());

            var table = mfo.ObservationTable();
            Assert.Equal(10.0, table.Data(0, 2)); // (b=1,a=1)
            Assert.Equal(30.0, table.Data(1, 2)); // (b=1,a=2)
            Assert.Equal(20.0, table.Data(2, 2)); // (b=2,a=1)
            Assert.Equal(40.0, table.Data(3, 2)); // (b=2,a=2)
        }

        [Fact]
        public void ImportCSV_MeasurementVariableInTheMiddle_WorksCorrectly()
        {
            string csv = "a,y,b\n1,10,1\n1,20,2\n2,30,1\n2,40,2";
            string path = CreateTempCsvFile(csv);

            MultiFacetsObs mfo = ImportCSV.ImportCSV_to_MultiFacetsObs(
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
        // Collapsing facets and ignoring extra measurement variables
        // --------------------------------------------------------------------------------
        [Fact]
        public void ImportCSV_OmitSomeFacets_AveragesOverThem()
        {
            string csv = "a,b,c,y\n1,1,1,10\n1,1,2,20\n1,2,1,30\n2,1,1,40\n2,2,1,50";
            string path = CreateTempCsvFile(csv);

            MultiFacetsObs mfo = ImportCSV.ImportCSV_to_MultiFacetsObs(
                path,
                new List<string> { "a", "b" },
                "y");

            Assert.Equal(2, mfo.ListFacets().Count());
            var table = mfo.ObservationTable();
            Assert.Equal(4, table.TableRows());
            Assert.Equal(15.0, table.Data(0, 2)); // (1,1) avg of 10,20
            Assert.Equal(30.0, table.Data(1, 2)); // (1,2)
            Assert.Equal(40.0, table.Data(2, 2)); // (2,1)
            Assert.Equal(50.0, table.Data(3, 2)); // (2,2)
        }

        [Fact]
        public void ImportCSV_IgnoredMeasurementVariable_IsNotUsed()
        {
            string csv = "a,b,y,z\n1,1,10,100\n1,2,20,200\n2,1,30,300\n2,2,40,400";
            string path = CreateTempCsvFile(csv);

            MultiFacetsObs mfo = ImportCSV.ImportCSV_to_MultiFacetsObs(
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
        public void ImportCSV_DependentVariableNotInHeader_Throws()
        {
            string csv = "a,b,y\n1,1,10";
            string path = CreateTempCsvFile(csv);

            var ex = Assert.Throws<Exception>(() =>
                ImportCSV.ImportCSV_to_MultiFacetsObs(path, new List<string> { "a", "b" }, "nonexistent"));
            Assert.Contains("no existe en la cabecera", ex.Message);
        }

        [Fact]
        public void ImportCSV_FacetNotInHeader_Throws()
        {
            string csv = "a,b,y\n1,1,10";
            string path = CreateTempCsvFile(csv);

            var ex = Assert.Throws<Exception>(() =>
                ImportCSV.ImportCSV_to_MultiFacetsObs(path, new List<string> { "a", "x" }, "y"));
            Assert.Contains("no existen en la cabecera", ex.Message);
            Assert.Contains("x", ex.Message);
        }

        [Fact]
        public void ImportCSV_DependentVariableAlsoFacet_Throws()
        {
            string csv = "a,b,y\n1,1,10";
            string path = CreateTempCsvFile(csv);

            var ex = Assert.Throws<Exception>(() =>
                ImportCSV.ImportCSV_to_MultiFacetsObs(path, new List<string> { "a", "y" }, "y"));
            Assert.Contains("no puede ser también una faceta", ex.Message);
        }

        [Fact]
        public void ImportCSV_MissingDependentValue_SkipsRow()
        {
            string csv = "a,b,y\n1,1,10\n1,2,\n2,1,30\n2,2,40";
            string path = CreateTempCsvFile(csv);

            MultiFacetsObs mfo = ImportCSV.ImportCSV_to_MultiFacetsObs(
                path,
                new List<string> { "a", "b" },
                "y");

            var table = mfo.ObservationTable();
            Assert.Equal(4, table.TableRows());
            Assert.Equal(10.0, table.Data(0, 2)); // (1,1)
            Assert.Null(table.Data(1, 2));        // (1,2) – skipped, mean is null
            Assert.Equal(30.0, table.Data(2, 2)); // (2,1)
            Assert.Equal(40.0, table.Data(3, 2)); // (2,2)
        }

        [Fact]
        public void ImportCSV_EmptyFile_Throws()
        {
            string csv = "";
            string path = CreateTempCsvFile(csv);

            var ex = Assert.Throws<Exception>(() =>
                ImportCSV.ImportCSV_to_MultiFacetsObs(path, new List<string> { "a", "b" }, "y"));
            Assert.Contains("No se encontró la línea de cabecera", ex.Message);
        }

        [Fact]
        public void ImportCSV_ShortRow_Skipped()
        {
            // Row with fewer tokens than header columns → skipped
            string csv = "a,b,y\n1,1,10\n2,2\n3,3,30";
            string path = CreateTempCsvFile(csv);

            MultiFacetsObs mfo = ImportCSV.ImportCSV_to_MultiFacetsObs(
                path,
                new List<string> { "a", "b" },
                "y");

            var table = mfo.ObservationTable();
            Assert.Equal(4, table.TableRows());
            Assert.Equal(10.0, table.Data(0, 2)); // (1,1) from first row
            Assert.Equal(30.0, table.Data(3, 2)); // (3,3) → (a=2,b=2) from third row
            Assert.Null(table.Data(1, 2));        // (1,2) – no data
            Assert.Null(table.Data(2, 2));        // (2,1) – no data
        }

        // --------------------------------------------------------------------------------
        // Others
        // --------------------------------------------------------------------------------
        [Fact]
        public void ImportCSV_QuotedFields_ParsedCorrectly()
        {
            // Quoted fields with commas inside quotes and surrounding whitespace
            string csv = "a,b,y\n\"1\",\"Smith, John\",10\n\"2\",\"Doe, Jane\",20";
            string path = CreateTempCsvFile(csv);

            MultiFacetsObs mfo = ImportCSV.ImportCSV_to_MultiFacetsObs(
                path,
                new List<string> { "a", "b" },
                "y");

            string comment = mfo.Comment();
            Assert.Contains("\tSmith, John = 1", comment);
            Assert.Contains("\tDoe, Jane = 2", comment);

            var table = mfo.ObservationTable();
            Assert.Equal(10.0, table.Data(0, 2)); // (1, Smith, John)
            Assert.Equal(20.0, table.Data(3, 2)); // (2, Doe, Jane) → position depends on level order
        }

        [Fact]
        public void ImportCSV_SemicolonDelimiter_Works()
        {
            string csv = "a;b;y\n1;1;10\n1;2;20\n2;1;30\n2;2;40";
            string path = CreateTempCsvFile(csv);

            MultiFacetsObs mfo = ImportCSV.ImportCSV_to_MultiFacetsObs(
                path,
                new List<string> { "a", "b" },
                "y");

            var table = mfo.ObservationTable();
            Assert.Equal(4, table.TableRows());
            Assert.Equal(10.0, table.Data(0, 2));
            Assert.Equal(20.0, table.Data(1, 2));
            Assert.Equal(30.0, table.Data(2, 2));
            Assert.Equal(40.0, table.Data(3, 2));
        }

        [Fact]
        public void ImportCSV_SemicolonDelimiterWithQuotes_Works()
        {
            string csv = "\"a\";\"b\";\"y\"\n\"1\";\"1\";\"10\"\n\"2\";\"2\";\"20\"";
            string path = CreateTempCsvFile(csv);

            MultiFacetsObs mfo = ImportCSV.ImportCSV_to_MultiFacetsObs(
                path,
                new List<string> { "a", "b" },
                "y");

            var table = mfo.ObservationTable();
            Assert.Equal(4, table.TableRows());
            Assert.Equal(10.0, table.Data(0, 2));
            Assert.Equal(20.0, table.Data(3, 2));
        }

        [Fact]
        public void ImportCSV_WhitespaceInCells_Trimmed()
        {
            string csv = "a,b,y\n 1 , 1 , 10 \n 2 , 2 , 20 ";
            string path = CreateTempCsvFile(csv);

            MultiFacetsObs mfo = ImportCSV.ImportCSV_to_MultiFacetsObs(
                path,
                new List<string> { "a", "b" },
                "y");

            var table = mfo.ObservationTable();
            Assert.Equal(10.0, table.Data(0, 2));
            Assert.Equal(20.0, table.Data(3, 2));
        }

        [Fact]
        public void ImportCSV_TextFacetValues_AssignedNumericIds()
        {
            string csv = "group,trial,score\ncontrol,1,10\ncontrol,2,20\ntreatment,1,30\ntreatment,2,40";
            string path = CreateTempCsvFile(csv);

            MultiFacetsObs mfo = ImportCSV.ImportCSV_to_MultiFacetsObs(
                path,
                new List<string> { "group", "trial" },
                "score");

            Assert.Equal(2, mfo.ListFacets().FacetInPos(0).Level()); // control, treatment
            Assert.Equal(2, mfo.ListFacets().FacetInPos(1).Level()); // 1, 2

            string comment = mfo.Comment();
            Assert.Contains("control = 1", comment);
            Assert.Contains("treatment = 2", comment);

            var table = mfo.ObservationTable();
            Assert.Equal(10.0, table.Data(0, 2)); // (control,1)
            Assert.Equal(20.0, table.Data(1, 2)); // (control,2)
            Assert.Equal(30.0, table.Data(2, 2)); // (treatment,1)
            Assert.Equal(40.0, table.Data(3, 2)); // (treatment,2)
        }

        [Fact]
        public void ImportCSV_ReadColumns_SemicolonDelimiter_ReturnsColumns()
        {
            string csv = "a;b;c\n1;2;3";
            string path = CreateTempCsvFile(csv);

            List<string> columns = ImportCSV.ReadColumns(path);

            Assert.Equal(new[] { "a", "b", "c" }, columns);
        }

        [Fact]
        public void ImportCSV_IsTextVariable_SemicolonDelimiter_Works()
        {
            string csv = "name;age;height\nJohn;30;180";
            string path = CreateTempCsvFile(csv);

            Assert.True(ImportCSV.isTextVariable("name", path));
            Assert.False(ImportCSV.isTextVariable("age", path));
        }
    }
}