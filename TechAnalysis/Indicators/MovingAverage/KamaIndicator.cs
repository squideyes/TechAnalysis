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
using System.Linq;

namespace SquidEyes.TechAnalysis
{
    public class KamaIndicator : BasicIndicatorBase, IBasicIndicator
    {
        private readonly double fastCF;
        private readonly double slowCF;
        private readonly SlidingBuffer<double> diffs;
        private readonly SlidingBuffer<double> values;

        private double lastResult = 0;

        private int index = 0;

        public KamaIndicator(int period, int fast, int slow, PriceToUse priceToUse)
            : base(period, priceToUse, 2)
        {
            fastCF = 2.0 / (fast + 1);
            slowCF = 2.0 / (slow + 1);

            diffs = new SlidingBuffer<double>(period + 1, true);
            values = new SlidingBuffer<double>(period + 1, true);
        }

        public DataPoint AddAndCalc(ICandle candle)
        {
            var dataPoint = candle.ToDataPoint(PriceToUse);

            DataPoint UpdateIndexAndLastResultThenGetResult(double value)
            {
                index++;

                return new DataPoint(candle.OpenOn, lastResult = value);
            }

            values.Add(dataPoint.Value);

            diffs.Add(index > 0 ? Math.Abs(dataPoint.Value - values[1]) : dataPoint.Value);

            if (index < Period)
                return UpdateIndexAndLastResultThenGetResult(dataPoint.Value);

            var signal = Math.Abs(dataPoint.Value - values[Period]);

            var noise = diffs.Take(Period).Sum();

            if (noise == 0.0)
                return UpdateIndexAndLastResultThenGetResult(dataPoint.Value);

            var result = lastResult + Math.Pow(signal / noise *
                (fastCF - slowCF) + slowCF, 2) * (dataPoint.Value - lastResult);

            return UpdateIndexAndLastResultThenGetResult(result);
        }
    }
}
