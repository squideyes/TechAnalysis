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

        public DataPoint AddAndCalc(ICandle candle)
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

            return new DataPoint(candle.OpenOn, smma);
        }
    }
}
