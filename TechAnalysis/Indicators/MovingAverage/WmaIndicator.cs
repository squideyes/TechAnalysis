using System;

namespace SquidEyes.TechAnalysis
{
    public class WmaIndicator : BasicIndicatorBase, IBasicIndicator
    {
        private readonly SlidingBuffer<float> buffer;

        private int index = 0;

        private double priorSum;
        private double priorWsum;

        public WmaIndicator(int period, PriceToUse priceToUse)
            : base(period, priceToUse, 2)
        {
            buffer = new SlidingBuffer<float>(period + 1);
        }

        public DataPoint AddAndCalc(ICandle candle)
        {
            var price = candle.GetPrice(PriceToUse);

            buffer.Add(price);

            var factor = Math.Min(index + 1, Period);

            var wsum = priorWsum -
                (index >= Period ? priorSum : 0.0) + factor * price;

            var sum = priorSum + price -
                (index >= Period ? buffer[0] : 0.0);

            var wma = wsum / (0.5 * factor * (factor + 1));

            index++;

            priorWsum = wsum;
            priorSum = sum;

            return new DataPoint(candle.OpenOn, wma);
        }
    }
}
