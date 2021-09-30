namespace SquidEyes.TechAnalysis
{
    public class EmaIndicator : BasicIndicatorBase, IBasicIndicator
    {
        private readonly double constant1;
        private readonly double constant2;
        private double? lastEma;

        public EmaIndicator(int period, PriceToUse priceToUse)
            : base(period, priceToUse, 2)
        {
            constant1 = 2.0 / (1 + period);
            constant2 = 1.0 - (2.0 / (1 + period));
        }

        public DataPoint AddAndCalc(ICandle candle) =>
            AddAndCalc(candle.ToDataPoint(PriceToUse.Close));

        public DataPoint AddAndCalc(DataPoint dataPoint)
        {
            var ema = !lastEma.HasValue ? dataPoint.Value :
                dataPoint.Value * constant1 + constant2 * lastEma;

            lastEma = ema;

            return new DataPoint(dataPoint.OpenOn, ema.Value);
        }
    }
}