// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Linq;

namespace SquidEyes.TechAnalysis
{
    public class SmmaIndicator : BasicIndicatorBase, IBasicIndicator
    {
        private readonly SlidingBuffer<float> buffer;

        private int index = 0;
        private double lastSmma = 0;
        private double sum = 0;
        private double prevsum = 0;
        private double prevsmma = 0;

        public SmmaIndicator(int period, PriceToUse priceToUse)
            : base(period, priceToUse, 2)
        {
            buffer = new SlidingBuffer<float>(period);
        }

        public BasicResult AddAndCalc(ICandle candle)
        {
            var price = candle.GetPrice(PriceToUse);

            buffer.Add(price);

            double smma;

            if (index++ <= Period)
            {
                sum = buffer.Sum();

                lastSmma = sum / Period;

                smma = lastSmma;
            }
            else
            {
                prevsum = sum;

                prevsmma = lastSmma;

                smma = (prevsum - prevsmma + price) / Period;

                sum = prevsum - prevsmma + price;

                lastSmma = (sum - prevsmma + price) / Period;
            }

            return GetBasicResult(candle.OpenOn, smma);
        }
    }
}