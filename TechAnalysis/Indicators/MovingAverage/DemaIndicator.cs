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

namespace SquidEyes.TechAnalysis
{
    public class DemaIndicator : BasicIndicatorBase, IBasicIndicator
    {
        private readonly EmaIndicator ema1;
        private readonly EmaIndicator ema2;

        public DemaIndicator(int period, PriceToUse priceToUse)
            : base(period, priceToUse, 2)
        {
            ema1 = new EmaIndicator(Period, priceToUse);
            ema2 = new EmaIndicator(Period, priceToUse);
        }

        public DataPoint AddAndCalc(ICandle candle)
        {
            var value1 = ema1.AddAndCalc(candle).Value;

            var dataPoint = new DataPoint(candle.OpenOn, value1);

            var value2 = ema2.AddAndCalc(dataPoint).Value;

            var dema = (2.0 * value1) - value2;

            return new DataPoint(candle.OpenOn, dema);
        }
    }
}