using AuxMathCalcGT;
using System;
using Xunit;

namespace Tests
{
    public class StatisticsTests
    {
        [Fact]
        public void Constructor_InitializesValuesToZero()
        {
            var stats = new Statistics();

            Assert.Null(stats.SumX());
            Assert.Null(stats.SumX2());
            Assert.Equal(0, stats.NumElem());
            Assert.Null(stats.Mean());
            Assert.Null(stats.Variance());
            Assert.Null(stats.StandardDeviation());
        }

        [Fact]
        public void Add_WithValidValue_UpdatesSumsAndCount()
        {
            var stats = new Statistics();

            stats.Add(3.0);

            Assert.Equal(1, stats.NumElem());
            Assert.Equal(3.0, stats.SumX());
            Assert.Equal(9.0, stats.SumX2());
        }

        [Fact]
        public void Add_WithMultipleValues_ComputesCorrectSums()
        {
            var stats = new Statistics();

            stats.Add(2.0);
            stats.Add(4.0);
            stats.Add(6.0);

            Assert.Equal(3, stats.NumElem());
            Assert.Equal(12.0, stats.SumX());
            Assert.Equal(56.0, stats.SumX2());
        }

        [Fact]
        public void Add_WithNull_DoesNotAffectSumsOrCount()
        {
            var stats = new Statistics();

            stats.Add(null);
            stats.Add(5.0);

            Assert.Equal(1, stats.NumElem());
            Assert.Equal(5.0, stats.SumX());
            Assert.Equal(25.0, stats.SumX2());
        }

        [Fact]
        public void Add_WithNullAndZeroTrue_TreatsNullAsZero()
        {
            var stats = new Statistics();

            stats.Add(null, zero: true);
            stats.Add(5.0, zero: true);

            Assert.Equal(2, stats.NumElem());
            Assert.Equal(5.0, stats.SumX());
            Assert.Equal(25.0, stats.SumX2());
        }

        [Fact]
        public void Add_WithNaN_PropagatesNaNToSums()
        {
            var stats = new Statistics();

            stats.Add(double.NaN);

            Assert.True(double.IsNaN(stats.SumX().Value));
            Assert.True(double.IsNaN(stats.SumX2().Value));
        }

        [Fact]
        public void Mean_WithNoElements_ReturnsNull()
        {
            var stats = new Statistics();

            Assert.Null(stats.Mean());
        }

        [Fact]
        public void Mean_ComputesAverageCorrectly()
        {
            var stats = new Statistics();

            stats.Add(2.0);
            stats.Add(4.0);
            stats.Add(6.0);

            Assert.Equal(4.0, stats.Mean());
        }

        [Fact]
        public void Variance_WithNoElements_ReturnsNull()
        {
            var stats = new Statistics();

            Assert.Null(stats.Variance());
        }

        [Fact]
        public void Variance_ComputesCorrectly()
        {
            var stats = new Statistics();

            stats.Add(2.0);
            stats.Add(4.0);
            stats.Add(6.0);

            // mean = 4, variance = ((4+16+36)/3) - 4² = 56/3 - 16 = 2.6667
            Assert.Equal(2.6666666666666665, stats.Variance().Value, precision: 10);
        }

        [Fact]
        public void StandardDeviation_WithNoElements_ReturnsNull()
        {
            var stats = new Statistics();

            Assert.Null(stats.StandardDeviation());
        }

        [Fact]
        public void StandardDeviation_ComputesCorrectly()
        {
            var stats = new Statistics();

            stats.Add(2.0);
            stats.Add(4.0);
            stats.Add(6.0);

            // std = sqrt(variance) = sqrt(2.6667) = 1.63299...
            var result = stats.StandardDeviation();

            Assert.NotNull(result);
            Assert.Equal(Math.Sqrt(2.6666666666666665), result.Value, precision: 10);
        }

        [Fact]
        public void Sequence_Add_Mean_Variance_StandardDeviation_AllConsistent()
        {
            var stats = new Statistics();

            stats.Add(1.0);
            stats.Add(2.0);
            stats.Add(3.0);
            stats.Add(4.0);
            stats.Add(5.0);

            Assert.Equal(15.0, stats.SumX());
            Assert.Equal(55.0, stats.SumX2());
            Assert.Equal(5, stats.NumElem());
            Assert.Equal(3.0, stats.Mean());
            Assert.Equal(2.0, stats.Variance().Value, precision: 10);
            Assert.Equal(Math.Sqrt(2.0), stats.StandardDeviation().Value, precision: 10);
        }
    }
}
