// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System;

namespace SquidEyes.TechAnalysis
{
    public class AtrIndicator : BasicIndicatorBase, IBasicIndicator
    {
        private int index = 0;
        private double lastValue = 0.0;
        private ICandle lastCandle = null;

        public AtrIndicator(int period, PriceToUse priceToUse)
            : base(period, priceToUse, 2)
        {
        }

        public BasicResult AddAndCalc(ICandle candle)
        {
            double high0 = candle.High;
            double low0 = candle.Low;

            if (index++ == 0)
            {
                lastCandle = candle;
                lastValue = high0 - low0;

                return GetBasicResult(candle.OpenOn, lastValue);
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

                return GetBasicResult(candle.OpenOn, result);
            }
        }
    }
}