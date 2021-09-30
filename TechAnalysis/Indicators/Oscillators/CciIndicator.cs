using System;

namespace SquidEyes.TechAnalysis
{
    public class CciIndicator : BasicIndicatorBase, IBasicIndicator
    {
        private readonly SmaIndicator sma;
        private readonly SlidingBuffer<double> typical;

        private int barCount = 0;

        public CciIndicator(int period, PriceToUse priceToUse)
            : base(period, priceToUse, 2)
        {
            sma = new SmaIndicator(Period, priceToUse);

            typical = new SlidingBuffer<double>(period, true);
        }

        public DataPoint AddAndCalc(ICandle candle)
        {
            double result;

            typical.Add(candle.Funcify(c => (c.High + c.Low + c.Close) / 3.0));

            var sma0 = sma.AddAndCalc(candle.OpenOn, typical[0]).Value;

            if (barCount == 0)
            {
                result = 0;
            }
            else
            {
                var mean = 0.0;

                for (var idx = Math.Min(barCount, Period - 1); idx >= 0; idx--)
                    mean += Math.Abs(typical[idx] - sma0);

                result = (typical[0] - sma0) / (mean.Approximates(0.0) ?
                    1 : (0.015 * (mean / Math.Min(Period, barCount + 1))));
            }

            barCount++;

            return new DataPoint(candle.OpenOn, result);
        }
    }
}
