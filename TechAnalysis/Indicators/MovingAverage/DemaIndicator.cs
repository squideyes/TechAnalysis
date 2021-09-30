namespace SquidEyes.TechAnalysis
{
    public class DemaIndicator : BasicIndicatorBase, IBasicIndicator
    {
        private readonly EmaIndicator ema1;
        private readonly EmaIndicator ema2;

        public DemaIndicator(int period, PriceToUse priceToUse)
            : base(period, priceToUse, 2)
        {
            ema1 = new EmaIndicator(Period, priceToUse);
            ema2 = new EmaIndicator(Period, priceToUse);
        }

        public DataPoint AddAndCalc(ICandle candle)
        {
            var value1 = ema1.AddAndCalc(candle).Value;

            var dataPoint = new DataPoint(candle.OpenOn, value1);

            var value2 = ema2.AddAndCalc(dataPoint).Value;

            var dema = (2.0 * value1) - value2;

            return new DataPoint(candle.OpenOn, dema);
        }
    }
}