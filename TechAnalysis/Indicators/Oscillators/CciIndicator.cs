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
    public class CciIndicator : BasicIndicatorBase, IBasicIndicator
    {
        private readonly SmaIndicator sma;
        private readonly SlidingBuffer<double> typical;

        private int barCount = 0;

        public CciIndicator(int period, PriceToUse priceToUse)
            : base(period, priceToUse, 2)
        {
            sma = new SmaIndicator(Period, priceToUse);

            typical = new SlidingBuffer<double>(period, true);
        }

        public DataPoint AddAndCalc(ICandle candle)
        {
            double result;

            typical.Add(candle.Funcify(c => (c.High + c.Low + c.Close) / 3.0));

            var sma0 = sma.AddAndCalc(candle.OpenOn, typical[0]).Value;

            if (barCount == 0)
            {
                result = 0;
            }
            else
            {
                var mean = 0.0;

                for (var idx = Math.Min(barCount, Period - 1); idx >= 0; idx--)
                    mean += Math.Abs(typical[idx] - sma0);

                result = (typical[0] - sma0) / (mean.Approximates(0.0) ?
                    1 : (0.015 * (mean / Math.Min(Period, barCount + 1))));
            }

            barCount++;

            return new DataPoint(candle.OpenOn, result);
        }
    }
}
