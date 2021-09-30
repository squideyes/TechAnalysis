using System;

namespace SquidEyes.TechAnalysis
{
    public class StdDevIndicator : BasicIndicatorBase, IBasicIndicator
    {
        private readonly SlidingBuffer<double> prices;
        private readonly SlidingBuffer<double> sumSeries;

        private int index = 0;

        public StdDevIndicator(int period, PriceToUse priceToUse)
            : base(period, priceToUse, 2)
        {
            prices = new SlidingBuffer<double>(period + 1, true);
            sumSeries = new SlidingBuffer<double>(period + 1, true);
        }

        public DataPoint AddAndCalc(ICandle candle)
        {
            var dataPoint = candle.ToDataPoint(PriceToUse);

            prices.Add(dataPoint.Value);

            sumSeries.Add(0.0);

            if (index < 1)
            {
                sumSeries.Update(dataPoint.Value);

                index++;

                return new DataPoint(dataPoint.OpenOn, 0.0);
            }
            else
            {
                sumSeries.Update(dataPoint.Value + sumSeries[1] - (index >= Period ? prices[Period] : 0.0));

                var average = sumSeries[0] / Math.Min(index + 1, Period);

                var sum = 0.0;

                for (var barsBack = Math.Min(index, Period - 1); barsBack >= 0; barsBack--)
                    sum += (prices[barsBack] - average) * (prices[barsBack] - average);

                var result = new DataPoint(dataPoint.OpenOn, Math.Sqrt(sum / Math.Min(index + 1, Period)));

                index++;

                return result;
            }
        }
    }
}
