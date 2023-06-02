// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System;
using System.Collections.Generic;
using SquidEyes.TechAnalysis;
using Xunit;
using static SquidEyes.UnitTests.TestData;

namespace SquidEyes.UnitTests
{
    public class IndicatorTests
    {
        [Fact]
        public void KeltnerChannelIndicatorShouldMatchBaseline()
        {
            var indicator = new KeltnerChannelIndictor(20, PriceToUse.Close, 1.5);

            foreach (var baseline in GetKeltnerChannelBaselines())
            {
                var result = indicator.AddAndCalc(baseline.Candle);

                if (!IsGoodCalc(result.Upper, baseline.Upper))
                    throw new ArgumentOutOfRangeException(nameof(result));

                if (!IsGoodCalc(result.Middle, baseline.Middle))
                    throw new ArgumentOutOfRangeException(nameof(result));

                if (!IsGoodCalc(result.Lower, baseline.Lower))
                    throw new ArgumentOutOfRangeException(nameof(result));
            }
        }

        [Fact]
        public void BollingerBandsIndicatorShouldMatchBaseline()
        {
            var indicator = new BollingerBandsIndictor(10, PriceToUse.Close, 2.0);

            foreach (var baseline in GetBollingerBandsBaselines())
            {
                var result = indicator.AddAndCalc(baseline.Candle);

                if (!IsGoodCalc(result.Upper, baseline.Upper))
                    throw new ArgumentOutOfRangeException(nameof(result));

                if (!IsGoodCalc(result.Middle, baseline.Middle))
                    throw new ArgumentOutOfRangeException(nameof(result));

                if (!IsGoodCalc(result.Lower, baseline.Lower))
                    throw new ArgumentOutOfRangeException(nameof(result));
            }
        }

        [Fact]
        public void MacdIndicatorShouldMatchBaseline()
        {
            var indicator = new MacdIndicator(10, 15, 3, PriceToUse.Close);

            foreach (var baseline in GetMacdBaselines())
            {
                var result = indicator.AddAndCalc(baseline.Candle);

                if (!IsGoodCalc(result.Average, baseline.Average))
                    throw new ArgumentOutOfRangeException(nameof(result));

                if (!IsGoodCalc(result.Value, baseline.Value))
                    throw new ArgumentOutOfRangeException(nameof(result));

                if (!IsGoodCalc(result.Difference, baseline.Difference))
                    throw new ArgumentOutOfRangeException(nameof(result));
            }
        }

        [Fact]
        public void AtrIndicatorShouldMatchBaseline() => BasicIndicatorBaselineTest(
            new AtrIndicator(10, PriceToUse.Close), GetAtrBaselines());

        [Fact]
        public void CciIndicatorShouldMatchBaseline() => BasicIndicatorBaselineTest(
            new CciIndicator(20, PriceToUse.Close), GetCciBaselines());

        [Fact]
        public void DemaIndicatorShouldMatchBaseline() => BasicIndicatorBaselineTest(
            new DemaIndicator(10, PriceToUse.Close), GetDemaBaselines());

        [Fact]
        public void EmaIndicatorShouldMatchBaseline() => BasicIndicatorBaselineTest(
            new EmaIndicator(10, PriceToUse.Close), GetEmaBaselines());

        [Fact]
        public void SmaIndicatorShouldMatchBaseline() => BasicIndicatorBaselineTest(
            new SmaIndicator(10, PriceToUse.Close), GetSmaBaselines());

        [Fact]
        public void TemaIndicatorShouldMatchBaseline() => BasicIndicatorBaselineTest(
            new TemaIndicator(10, PriceToUse.Close), GetTemaBaselines());

        [Fact]
        public void WmaIndicatorShouldMatchBaseline() => BasicIndicatorBaselineTest(
            new WmaIndicator(10, PriceToUse.Close), GetWmaBaselines());

        [Fact]
        public void LinRegIndicatorShouldMatchBaseline() => BasicIndicatorBaselineTest(
            new LinRegIndicator(10, PriceToUse.Close), GetLinRegBaselines());

        [Fact]
        public void StdDevIndicatorShouldMatchBaseline() => BasicIndicatorBaselineTest(
            new StdDevIndicator(10, PriceToUse.Close), GetStdDevBaselines());

        [Fact]
        public void KamaIndicatorShouldMatchBaseline() => BasicIndicatorBaselineTest(
            new KamaIndicator(10, 5, 15, PriceToUse.Close), GetKamaBaselines());

        [Fact]
        public void SmmaIndicatorShouldMatchBaseline() => BasicIndicatorBaselineTest(
            new SmmaIndicator(10, PriceToUse.Close), GetSmmaBaselines());

        [Fact]
        public void StochasticsIndicatorShouldMatchBaseline()
        {
            var indicator = new StochasticsIndicator(14, 7, 3);

            foreach (var baseline in GetStochasticsBaselines())
            {
                var add = indicator.AddAndCalc(baseline.Candle);

                if (!add.K.Approximates(baseline.K))
                    throw new ArgumentOutOfRangeException(nameof(add.K));

                if (!add.D.Approximates(baseline.D))
                    throw new ArgumentOutOfRangeException(nameof(add.D));

                var update = indicator.UpdateAndCalc(baseline.Candle);

                if (!update.K.Approximates(baseline.K))
                    throw new ArgumentOutOfRangeException(nameof(update.K));

                if (!update.D.Approximates(baseline.D))
                    throw new ArgumentOutOfRangeException(nameof(update.D));
            }
        }

        private static void BasicIndicatorBaselineTest(
            IBasicIndicator indicator, List<ValueBaseline> baselines)
        {
            foreach (var baseline in baselines)
            {
                var result = indicator.AddAndCalc(baseline.Candle);

                if (!IsGoodCalc(result.Value, baseline.Value))
                    throw new ArgumentOutOfRangeException(nameof(result));
            }
        }

        private static bool IsGoodCalc(double actual, double expected) =>
            Math.Round(actual, expected.ToDecimalDigits()) == expected;
    }
}