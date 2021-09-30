﻿// Copyright © 2021 by Louis S. Berman
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