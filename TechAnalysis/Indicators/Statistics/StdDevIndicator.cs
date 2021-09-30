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
    public class StdDevIndicator : BasicIndicatorBase, IBasicIndicator
    {
        private readonly SlidingBuffer<double> prices;
        private readonly SlidingBuffer<double> sumSeries;

        private int index = 0;

        public StdDevIndicator(int period, PriceToUse priceToUse)
            : base(period, priceToUse, 2)
        {
            prices = new SlidingBuffer<double>(period + 1, true);
            sumSeries = new SlidingBuffer<double>(period + 1, true);
        }

        public DataPoint AddAndCalc(ICandle candle)
        {
            var dataPoint = candle.ToDataPoint(PriceToUse);

            prices.Add(dataPoint.Value);

            sumSeries.Add(0.0);

            if (index < 1)
            {
                sumSeries.Update(dataPoint.Value);

                index++;

                return new DataPoint(dataPoint.OpenOn, 0.0);
            }
            else
            {
                sumSeries.Update(dataPoint.Value + sumSeries[1] - (index >= Period ? prices[Period] : 0.0));

                var average = sumSeries[0] / Math.Min(index + 1, Period);

                var sum = 0.0;

                for (var barsBack = Math.Min(index, Period - 1); barsBack >= 0; barsBack--)
                    sum += (prices[barsBack] - average) * (prices[barsBack] - average);

                var result = new DataPoint(dataPoint.OpenOn, Math.Sqrt(sum / Math.Min(index + 1, Period)));

                index++;

                return result;
            }
        }
    }
}
