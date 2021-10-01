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
    public class LinRegIndicator : BasicIndicatorBase, IBasicIndicator
    {
        private readonly SlidingBuffer<double> values;

        public int index = 0;
        private double avg = 0.0;
        private double divisor = 0.0;
        private double intercept = 0.0;
        private double period = 0.0;
        private double priorSumXY = 0.0;
        private double priorSumY = 0.0;
        private double slope = 0.0;
        private double sumX2 = 0.0;
        private double sumX = 0.0;
        private double sumXY = 0.0;
        private double sumY = 0.0;

        public LinRegIndicator(int period, PriceToUse priceToUse)
            : base(period, priceToUse, 2)
        {
            values = new SlidingBuffer<double>(period + 1);
        }

        public BasicResult AddAndCalc(ICandle candle)
        {
            var dataPoint = candle.ToBasicResult(PriceToUse);

            values.Add(dataPoint.Value);

            priorSumY = sumY;

            priorSumXY = sumXY;

            period = Math.Min(index + 1, Period);

            sumX = period * (period - 1) * 0.5;

            sumX2 = period * (period + 1) * 0.5;

            divisor = period * (period + 1) * (2 * period + 1) / 6 - sumX2 * sumX2 / period;

            sumXY = priorSumXY - (index >= Period ? priorSumY : 0) + period * dataPoint.Value;

            sumY = priorSumY + dataPoint.Value - (index >= Period ? values[0] : 0);

            avg = sumY / period;

            slope = (sumXY - sumX2 * avg) / divisor;

            intercept = (values.TakeLast(Period).Sum() - slope * sumX) / period;

            var result = new BasicResult()
            {
                OpenOn = dataPoint.OpenOn,
                Value = index == 0 ? dataPoint.Value : (intercept + slope * (period - 1))
            };

            index++;

            return result;
        }
    }
}
