﻿using System;
using SquidEyes.TechAnalysis;
using Xunit;
using static SquidEyes.UnitTests.Properties.Baselines;
using static SquidEyes.UnitTests.TestData;

namespace SquidEyes.UnitTests
{
    public class IndicatorTests
    {
        [Fact]
        public void KeltnerChannelIndicatorShouldMatchBaseline()
        {
            var indicator = new KeltnerChannelIndictor(20, PriceToUse.Close, 1.5);

            foreach (var baseline in GetThreeBandBaselines(KeltnerChannelBaseline))
            {
                var result = indicator.AddAndCalc(baseline.Candle);

                if (!result.Upper.Approximates(baseline.Upper))
                    throw new ArgumentOutOfRangeException(nameof(result));

                if (!result.Middle.Approximates(baseline.Middle))
                    throw new ArgumentOutOfRangeException(nameof(result));

                if (!result.Lower.Approximates(baseline.Lower))
                    throw new ArgumentOutOfRangeException(nameof(result));
            }
        }

        [Fact]
        public void BollingerBandsIndicatorShouldMatchBaseline()
        {
            var indicator = new BollingerBandsIndictor(10, PriceToUse.Close, 2.0);

            foreach (var baseline in GetThreeBandBaselines(BollingerBandsBaseline))
            {
                var result = indicator.AddAndCalc(baseline.Candle);

                if (!result.Upper.Approximates(baseline.Upper))
                    throw new ArgumentOutOfRangeException(nameof(result));

                if (!result.Middle.Approximates(baseline.Middle))
                    throw new ArgumentOutOfRangeException(nameof(result));

                if (!result.Lower.Approximates(baseline.Lower))
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

                if (!result.Value.Approximates(baseline.Value))
                    throw new ArgumentOutOfRangeException(nameof(result));

                if (!result.Average.Approximates(baseline.Average))
                    throw new ArgumentOutOfRangeException(nameof(result));

                if (!result.Difference.Approximates(baseline.Difference))
                    throw new ArgumentOutOfRangeException(nameof(result));
            }
        }

        [Fact]
        public void CciIndicatorShouldMatchBaseline()
        {
            var indicator = new CciIndicator(20, PriceToUse.Close);

            foreach (var baseline in GetValueBaseline(CciBaseline))
            {
                var result = indicator.AddAndCalc(baseline.Candle);

                if (!result.Value.Approximates(baseline.Value))
                    throw new ArgumentOutOfRangeException(nameof(result));
            }
        }

        [Fact]
        public void AtrIndicatorShouldMatchBaseline()
        {
            var indicator = new AtrIndicator(10);

            foreach (var baseline in GetValueBaseline(AtrBaseline))
            {
                var result = indicator.AddAndCalc(baseline.Candle);

                if (!result.Value.Approximates(baseline.Value))
                    throw new ArgumentOutOfRangeException(nameof(result));
            }
        }

        [Fact]
        public void DemaIndicatorShouldMatchBaseline() =>
            ValueBaselineTest(new DemaIndicator(10, PriceToUse.Close), DemaBaseline);

        [Fact]
        public void EmaIndicatorShouldMatchBaseline() =>
            ValueBaselineTest(new EmaIndicator(10, PriceToUse.Close), EmaBaseline);

        [Fact]
        public void SmaIndicatorShouldMatchBaseline() =>
            ValueBaselineTest(new SmaIndicator(10, PriceToUse.Close), SmaBaseline);

        [Fact]
        public void TemaIndicatorShouldMatchBaseline() =>
            ValueBaselineTest(new TemaIndicator(10, PriceToUse.Close), TemaBaseline);

        [Fact]
        public void WmaIndicatorShouldMatchBaseline() =>
            ValueBaselineTest(new WmaIndicator(10, PriceToUse.Close), WmaBaseline);

        [Fact]
        public void LinRegIndicatorShouldMatchBaseline() =>
            ValueBaselineTest(new LinRegIndicator(10, PriceToUse.Close), LinRegBaseline);

        [Fact]
        public void StdDevIndicatorShouldMatchBaseline() =>
            ValueBaselineTest(new StdDevIndicator(10, PriceToUse.Close), StdDevBaseline);

        [Fact]
        public void KamaIndicatorShouldMatchBaseline() =>
            ValueBaselineTest(new KamaIndicator(10, 5, 15, PriceToUse.Close), KamaBaseline);

        [Fact]
        public void SmmaIndicatorShouldMatchBaseline() =>
            ValueBaselineTest(new SmmaIndicator(10, PriceToUse.Close), SmaaBaseline);

        [Fact]
        public void StochasticsIndicatorShouldMatchBaseline()
        {
            var indicator = new StochasticsIndicator(14, 7, 3);

            foreach (var baseline in GetStochasticsBaselines())
            {
                var add = indicator.AddAndCalc(baseline.Candle);

                if(!add.K.Approximates(baseline.K))
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

        private static void ValueBaselineTest(IBasicIndicator indicator, string csv)
        {
            foreach (var baseline in GetValueBaseline(csv))
            {
                var result = indicator.AddAndCalc(baseline.Candle);

                if (!result.Value.Approximates(baseline.Value))
                    throw new ArgumentOutOfRangeException(nameof(csv));
            }
        }
    }
}