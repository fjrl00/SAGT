using System;
using Xunit;
using MultiFacetData;
using System.Collections.Generic;
using ProjectMeans;

namespace Tests
{
    public class TableTests
    {
        private void AssertTableEquals(List<List<double?>> expected, CartesianProductTable table)
        {
            // Check dimensions
            Assert.Equal(expected.Count, table.TableRows());
            Assert.Equal(expected[0].Count, table.TableColumns());

            // Check each cell
            for (int row = 0; row < table.TableRows(); row++)
            {
                for (int col = 0; col < table.TableColumns(); col++)
                {
                    double? expectedValue = expected[row][col];
                    double? actualValue = table.Data(row, col);

                    if(expectedValue.HasValue && actualValue.HasValue)
                    {
                        Assert.Equal(expectedValue.Value, actualValue.Value, precision: 2);
                    } else
                    {
                        Assert.Equal(expectedValue, actualValue);
                    }
                }
            }
        }

        [Fact]
        public void BasicConstructor_Null()
        {
            ListFacets lf = null;

            var exception = Assert.Throws<ObsTableException>(() => new ObsTable(lf));

            // Assert
            Assert.Equal("Error: no hay facetas", exception.Message);
        }

        [Fact]
        public void BasicConstructor_One()
        {
            Facet f = new Facet("faceta1", 3);
            ListFacets lf = new ListFacets();
            lf.Add(f);

            var exception = Assert.Throws<ObsTableException>(() => new ObsTable(lf));

            // Assert
            Assert.Equal("Error: al menos debe haber 2 facetas", exception.Message);
        }

        [Fact]
        public void BasicConstructor_BaseCase()
        {
            //Example from CartesianProductTable.cs
            var e_matrix = new List<List<double?>>
            {
                new List<double?> { 1, 1, 1, null },
                new List<double?> { 1, 1, 2, null },
                new List<double?> { 1, 2, 1, null },
                new List<double?> { 1, 2, 2, null },
                new List<double?> { 1, 3, 1, null },
                new List<double?> { 1, 3, 2, null },
                new List<double?> { 2, 1, 1, null },
                new List<double?> { 2, 1, 2, null },
                new List<double?> { 2, 2, 1, null },
                new List<double?> { 2, 2, 2, null },
                new List<double?> { 2, 3, 1, null },
                new List<double?> { 2, 3, 2, null },
            };

            Facet f1 = new Facet("Individuos", 2);
            Facet f2 = new Facet("Observaciones", 3);
            Facet f3 = new Facet("Caracteristicas", 2);

            ListFacets lf = new ListFacets();
            lf.Add(f1);
            lf.Add(f2);
            lf.Add(f3);

            ObsTable table = new ObsTable(lf);

            AssertTableEquals(e_matrix, table);
        }

        //OMITTESTING.cs
        private MultiFacetsObs OmitTestingSagt()
        {
            Facet f1 = new Facet("Escuela", 3);
            Facet f2 = new Facet("Género", 2);
            Facet f3 = new Facet("Medidor", 2);

            ListFacets lf = new ListFacets();
            lf.Add(f1);
            lf.Add(f2);
            lf.Add(f3);

            MultiFacetsObs mfo = new MultiFacetsObs(lf, "a", "a");

            var matrix = new List<List<double?>>
            {
                new List<double?> { 1, 1, 1, 202 },
                new List<double?> { 1, 1, 2, 198 },
                new List<double?> { 1, 2, 1, 159 },
                new List<double?> { 1, 2, 2, 161 },
                new List<double?> { 2, 1, 1, 191.5 },
                new List<double?> { 2, 1, 2, 999 },
                new List<double?> { 2, 2, 1, 150 },
                new List<double?> { 2, 2, 2, 1 },
                new List<double?> { 3, 1, 1, 190.5 },
                new List<double?> { 3, 1, 2, 189.5 },
                new List<double?> { 3, 2, 1, 171 },
                new List<double?> { 3, 2, 2, 169 },
            };
            ObsTable table = new ObsTable(matrix);
            mfo.ObservationTable(table);

            return mfo;
        }

        //Test omitting nothing from OMITTESTING
        [Fact]
        public void CollapseConstructor_BaseCase()
        {
            var e_matrix = new List<List<double?>>
            {
                new List<double?> { 1, 1, 1, 202 },
                new List<double?> { 1, 1, 2, 198 },
                new List<double?> { 1, 2, 1, 159 },
                new List<double?> { 1, 2, 2, 161 },
                new List<double?> { 2, 1, 1, 191.5 },
                new List<double?> { 2, 1, 2, 999 },
                new List<double?> { 2, 2, 1, 150 },
                new List<double?> { 2, 2, 2, 1 },
                new List<double?> { 3, 1, 1, 190.5 },
                new List<double?> { 3, 1, 2, 189.5 },
                new List<double?> { 3, 2, 1, 171 },
                new List<double?> { 3, 2, 2, 169 },
            };

            MultiFacetsObs mfo = OmitTestingSagt();

            ObsTable table = new ObsTable(mfo.ListFacets(), mfo);

            AssertTableEquals(e_matrix, table);
        }

        //Test omitting level 2 of Escuela from OMITTESTING
        [Fact]
        public void CollapseConstructor_SkipOneLevel()
        {
            var e_matrix = new List<List<double?>>
            {
                new List<double?> { 1, 1, 1, 202 },
                new List<double?> { 1, 1, 2, 198 },
                new List<double?> { 1, 2, 1, 159 },
                new List<double?> { 1, 2, 2, 161 },
                new List<double?> { 3, 1, 1, 190.5 },
                new List<double?> { 3, 1, 2, 189.5 },
                new List<double?> { 3, 2, 1, 171 },
                new List<double?> { 3, 2, 2, 169 },
            };

            MultiFacetsObs mfo = OmitTestingSagt();
            mfo.ListFacets().LookingFacet("Escuela").SetSkipLevels(2);

            ObsTable table = new ObsTable(mfo.ListFacets(), mfo);

            AssertTableEquals(e_matrix, table);
        }

        //Test omitting Escuela from OMITTESTING
        [Fact]
        public void CollapseConstructor_OmitOneFacet()
        {
            var e_matrix = new List<List<double?>>
            {
                new List<double?> { 1, 1, 194.667 },
                new List<double?> { 1, 2, 462.167 },
                new List<double?> { 2, 1, 160 },
                new List<double?> { 2, 2, 110.333 },
            };

            MultiFacetsObs mfo = OmitTestingSagt();
            mfo.ListFacets().LookingFacet("Escuela").Omit(true);
            ListFacets lf = mfo.ListFacets().ListFacetsWithoutOmit();

            ObsTable table = new ObsTable(lf, mfo);

            AssertTableEquals(e_matrix, table);
        }

        //Test omitting Escuela and level2 of Escuela from OMITTESTING
        [Fact]
        public void CollapseConstructor_SkipLevelFromFacetToAlsoOmit()
        {
            var e_matrix = new List<List<double?>>
            {
                new List<double?> { 1, 1, 196.25 },
                new List<double?> { 1, 2, 193.75 },
                new List<double?> { 2, 1, 165 },
                new List<double?> { 2, 2, 165 },
            };

            MultiFacetsObs mfo = OmitTestingSagt();
            mfo.ListFacets().LookingFacet("Escuela").Omit(true);
            mfo.ListFacets().LookingFacet("Escuela").SetSkipLevels(2);
            ListFacets lf = mfo.ListFacets().ListFacetsWithoutOmit();

            ObsTable table = new ObsTable(lf, mfo);

            AssertTableEquals(e_matrix, table);
        }

        #region TableMeans

        //Test omitting nothing from OMITTESTING
        [Fact]
        public void TMConstructor_BaseCase()
        {
            var e_matrix = new List<List<double?>>
            {
                new List<double?> { 1, 1, 1, 202.000, 0.000, 0.000 },
                new List<double?> { 1, 1, 2, 198.000, 0.000, 0.000 },
                new List<double?> { 1, 2, 1, 159.000, 0.000, 0.000 },
                new List<double?> { 1, 2, 2, 161.000, 0.000, 0.000 },
                new List<double?> { 2, 1, 1, 191.500, 0.000, 0.000 },
                new List<double?> { 2, 1, 2, 999.000, 0.000, 0.000 },
                new List<double?> { 2, 2, 1, 150.000, 0.000, 0.000 },
                new List<double?> { 2, 2, 2, 1.000, 0.000, 0.000 },
                new List<double?> { 3, 1, 1, 190.500, 0.000, 0.000 },
                new List<double?> { 3, 1, 2, 189.500, 0.000, 0.000 },
                new List<double?> { 3, 2, 1, 171.000, 0.000, 0.000 },
                new List<double?> { 3, 2, 2, 169.000, 0.000, 0.000 },
            };

            MultiFacetsObs mfo = OmitTestingSagt();

            TableMeans table = new TableMeans(mfo.ListFacets(), "", mfo);

            AssertTableEquals(e_matrix, table);
        }

        //Test omitting level 2 of Escuela from OMITTESTING
        [Fact]
        public void TMConstructor_SkipOneLevel()
        {
            var e_matrix = new List<List<double?>>
            {
                new List<double?> { 1, 1, 1, 202.000, 0.000, 0.000 },
                new List<double?> { 1, 1, 2, 198.000, 0.000, 0.000 },
                new List<double?> { 1, 2, 1, 159.000, 0.000, 0.000 },
                new List<double?> { 1, 2, 2, 161.000, 0.000, 0.000 },
                new List<double?> { 3, 1, 1, 190.500, 0.000, 0.000 },
                new List<double?> { 3, 1, 2, 189.500, 0.000, 0.000 },
                new List<double?> { 3, 2, 1, 171.000, 0.000, 0.000 },
                new List<double?> { 3, 2, 2, 169.000, 0.000, 0.000 },
            };

            MultiFacetsObs mfo = OmitTestingSagt();
            mfo.ListFacets().LookingFacet("Escuela").SetSkipLevels(2);
            mfo = mfo.SkipIndexLevelFacetInDataTable(); //line not needed with obstable

            TableMeans table = new TableMeans(mfo.ListFacets(), "", mfo);

            AssertTableEquals(e_matrix, table);
        }

        //Test omitting Escuela from OMITTESTING
        [Fact]
        public void TMConstructor_OmitOneFacet()
        {
            var e_matrix = new List<List<double?>>
            {
                new List<double?> { 1, 1, 194.667, 27.056, 5.201 },
                new List<double?> { 1, 2, 462.167, 144107.056, 379.614 },
                new List<double?> { 2, 1, 160.000, 74.000, 8.602 },
                new List<double?> { 2, 2, 110.333, 5987.556, 77.379 },
            };

            MultiFacetsObs mfo = OmitTestingSagt();
            mfo.ListFacets().LookingFacet("Escuela").Omit(true);
            ListFacets lf = mfo.ListFacets().ListFacetsWithoutOmit();

            TableMeans table = new TableMeans(lf, "", mfo);

            AssertTableEquals(e_matrix, table);
        }

        //Test omitting Escuela and level2 of Escuela from OMITTESTING
        [Fact]
        public void TMConstructor_SkipLevelFromFacetToAlsoOmit()
        {
            var e_matrix = new List<List<double?>>
            {
                new List<double?> { 1, 1, 196.250, 33.063, 5.750 },
                new List<double?> { 1, 2, 193.750, 18.063, 4.250 },
                new List<double?> { 2, 1, 165.000, 36.000, 6.000 },
                new List<double?> { 2, 2, 165.000, 16.000, 4.000 },
            };

            MultiFacetsObs mfo = OmitTestingSagt();
            mfo.ListFacets().LookingFacet("Escuela").Omit(true);
            mfo.ListFacets().LookingFacet("Escuela").SetSkipLevels(2);
            mfo = mfo.SkipIndexLevelFacetInDataTable(); //line not needed with obstable
            ListFacets lf = mfo.ListFacets().ListFacetsWithoutOmit();

            TableMeans table = new TableMeans(lf, "", mfo);

            AssertTableEquals(e_matrix, table);
        }

        #endregion TableMeans



        //todo: test trying to omit everything or being given a wrong lF and such
    }
}
