using MultiFacetData;
using ProjectSSQ;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class RRunnerTests
    {
        /* Descripción:
         *  Construye una faceta sin anidamiento/cruce (diseño por defecto "[name]").
         */
        private static Facet TopLevelFacet(string name)
        {
            return new Facet(name, 2);
        }

        /* Descripción:
         *  Construye una faceta con un diseño de anidamiento/cruce explícito (formato con
         *  corchetes y ':', p.ej. "[i]:[p]" o "[O]:[I][C]").
         */
        private static Facet NestedFacet(string name, string design)
        {
            return new Facet(name, 2, "", design);
        }

        private static ListFacets Lf(params Facet[] facets)
        {
            ListFacets lf = new ListFacets();
            foreach (Facet f in facets)
            {
                lf.Add(f);
            }
            return lf;
        }

        /* Descripción:
         *  Normaliza un único término del modelo (p.ej. "i:p" y "p:i" deben considerarse
         *  equivalentes) ordenando alfabéticamente los nombres de faceta que lo componen.
         */
        private static string NormalizeTerm(string term)
        {
            return string.Join(":", term.Split(':').OrderBy(n => n, StringComparer.Ordinal));
        }

        /* Descripción:
         *  Normaliza el modelo completo: cada término se normaliza individualmente y la lista
         *  de términos se ordena (el orden entre términos con el mismo número de facetas no
         *  es relevante).
         */
        private static List<string> NormalizeModel(string model)
        {
            return model
                .Split(new[] { " + " }, StringSplitOptions.None)
                .Select(NormalizeTerm)
                .OrderBy(t => t.Split(':').Length)
                .ThenBy(t => t, StringComparer.Ordinal)
                .ToList();
        }

        /* Descripción:
         *  Comprueba que los términos del modelo generado aparecen ordenados de menos facetas
         *  implicadas a más (restricción de anovaVCA).
         */
        private static void AssertAscendingFacetCount(string model)
        {
            List<int> facetCounts = model
                .Split(new[] { " + " }, StringSplitOptions.None)
                .Select(t => t.Split(':').Length)
                .ToList();

            for (int i = 1; i < facetCounts.Count; i++)
            {
                Assert.True(
                    facetCounts[i] >= facetCounts[i - 1],
                    $"Las fuentes de variación deben estar ordenadas de menos facetas a más: '{model}'");
            }
        }

        /* Descripción:
         *  Comprueba que el modelo generado por BuildVcaModel es equivalente al esperado:
         *  mismo conjunto de términos (sin importar el orden entre términos con igual número
         *  de facetas, ni el orden de las facetas dentro de un término de interacción), y que
         *  los términos aparecen ordenados de menos facetas a más.
         */
        private static void AssertVcaModel(string expectedModel, ListFacets listFacets)
        {
            string actualModel = RRunner.BuildVcaModel(listFacets);

            AssertAscendingFacetCount(actualModel);
            Assert.Equal(NormalizeModel(expectedModel), NormalizeModel(actualModel));
        }


        // i:p (p/i: i anidada en p)
        [Fact]
        public void BuildVcaModel_Nested_TwoFacets()
        {
            ListFacets lf = Lf(
                TopLevelFacet("p"),
                NestedFacet("i", "[i]:[p]"));

            AssertVcaModel("p + i:p", lf);
        }


        // p x i (cruzado)
        [Fact]
        public void BuildVcaModel_Crossed_TwoFacets()
        {
            ListFacets lf = Lf(
                TopLevelFacet("p"),
                TopLevelFacet("i"));

            AssertVcaModel("p + i + p:i", lf);
        }


        // p x i x o (cruzado)
        [Fact]
        public void BuildVcaModel_Crossed_ThreeFacets()
        {
            ListFacets lf = Lf(
                TopLevelFacet("p"),
                TopLevelFacet("i"),
                TopLevelFacet("o"));

            AssertVcaModel("p + i + o + p:i + p:o + i:o + p:i:o", lf);
        }


        // pfsen (cruzado, 5 facetas)
        [Fact]
        public void BuildVcaModel_Crossed_FiveFacets()
        {
            ListFacets lf = Lf(
                TopLevelFacet("p"),
                TopLevelFacet("f"),
                TopLevelFacet("s"),
                TopLevelFacet("e"),
                TopLevelFacet("n"));

            string expected = string.Join(" + ", new[]
            {
                "p", "f", "s", "e", "n",
                "p:f", "p:s", "p:e", "p:n", "f:s", "f:e", "f:n", "s:e", "s:n", "e:n",
                "p:f:s", "p:f:e", "p:f:n", "p:s:e", "p:s:n", "p:e:n",
                "f:s:e", "f:s:n", "f:e:n", "s:e:n",
                "p:f:s:e", "p:f:s:n", "p:f:e:n", "p:s:e:n", "f:s:e:n",
                "p:f:s:e:n"
            });

            AssertVcaModel(expected, lf);
        }


        // O:IC (O anidada en el cruce de I y C)
        [Fact]
        public void BuildVcaModel_NestedInCrossedPair()
        {
            ListFacets lf = Lf(
                NestedFacet("O", "[O]:[I][C]"),
                TopLevelFacet("I"),
                TopLevelFacet("C"));

            AssertVcaModel("C + I + I:C + O:I:C", lf);
        }


        // OI:C (O sin anidar, I anidada en C, C sin anidar)
        [Fact]
        public void BuildVcaModel_PartiallyNestedPair()
        {
            ListFacets lf = Lf(
                TopLevelFacet("O"),
                NestedFacet("I", "[I]:[C]"),
                TopLevelFacet("C"));

            AssertVcaModel("C + O + O:C + C:I + O:C:I", lf);
        }
    }
}
