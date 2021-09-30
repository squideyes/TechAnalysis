﻿namespace SquidEyes.TechAnalysis
{
    public class TemaIndicator : BasicIndicatorBase, IBasicIndicator
    {
        private readonly EmaIndicator ema1;
        private readonly EmaIndicator ema2;
        private readonly EmaIndicator ema3;

        public TemaIndicator(int period, PriceToUse priceToUse)
            : base(period, priceToUse, 2)
        {
            ema1 = new EmaIndicator(Period, priceToUse);
            ema2 = new EmaIndicator(Period, priceToUse);
            ema3 = new EmaIndicator(Period, priceToUse);
        }

        public DataPoint AddAndCalc(ICandle candle)
        {
            var value1 = ema1.AddAndCalc(candle).Value;
            var dataPoint1 = new DataPoint(candle.OpenOn, value1);

            var value2 = ema2.AddAndCalc(dataPoint1).Value;
            var dataPoint2 = new DataPoint(candle.OpenOn, value2);

            var value3 = ema3.AddAndCalc(dataPoint2).Value;
            var tema = (3 * value1) - (3 * value2) + value3;

            return new DataPoint(candle.OpenOn, tema);
        }
    }
}