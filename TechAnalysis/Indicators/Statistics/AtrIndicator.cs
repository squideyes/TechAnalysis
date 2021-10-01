// Copyright © 2021 by Louis S. Berman
// 
// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the “Software”),
// to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software
// is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
// IN THE SOFTWARE.

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
