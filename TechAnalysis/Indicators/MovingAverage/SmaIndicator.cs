using System;

namespace SquidEyes.TechAnalysis
{
    public class SmaIndicator : BasicIndicatorBase, IBasicIndicator
    {
        private readonly SlidingBuffer<double> buffer;

        private int index = 0;
        private double sum = 0;
        private double priorSum = 0;

        public SmaIndicator(int period, PriceToUse priceToUse)
            : base(period, priceToUse, 2)
        {
            buffer = new SlidingBuffer<double>(period + 1);
        }

        public DataPoint AddAndCalc(ICandle candle) =>
            AddAndCalc(candle.OpenOn, candle.GetPrice(PriceToUse));

        public DataPoint AddAndCalc(DateTime openOn, double price)
        {
            buffer.Add(price);

            sum = priorSum + price - (index >= Period ? buffer[0] : 0);

            var sma = sum / (index < Period ? index + 1 : Period);

            priorSum = sum;

            index++;

            return new DataPoint(openOn, sma);
        }
    }
}
