using System;

namespace SquidEyes.TechAnalysis
{
    public class AtrIndicator 
    {
        private int index = 0;
        private double lastValue = 0.0;
        private ICandle lastCandle = null;

        public AtrIndicator(int period)
        {
            Period = period;
        }

        public int Period { get; }

        public DataPoint AddAndCalc(ICandle candle)
        {
            double high0 = candle.High;
            double low0 = candle.Low;

            if (index++ == 0)
            {
                lastCandle = candle;
                lastValue = high0 - low0;

                return new DataPoint(candle.OpenOn, lastValue);
            }
            else
            {
                double close1 = lastCandle.Close;

                var trueRange = Math.Max(Math.Abs(low0 - close1),
                    Math.Max(high0 - low0, Math.Abs(high0 - close1)));

                var result = ((Math.Min(index, Period) - 1) * lastValue + trueRange) /
                    Math.Min(index, Period);

                lastCandle = candle;
                lastValue = result;

                return new DataPoint(candle.OpenOn, result);
            }
        }
    }
}
