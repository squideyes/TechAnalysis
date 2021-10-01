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
    public class WmaIndicator : BasicIndicatorBase, IBasicIndicator
    {
        private readonly SlidingBuffer<float> buffer;

        private int index = 0;

        private double priorSum;
        private double priorWsum;

        public WmaIndicator(int period, PriceToUse priceToUse)
            : base(period, priceToUse, 2)
        {
            buffer = new SlidingBuffer<float>(period + 1);
        }

        public BasicResult AddAndCalc(ICandle candle)
        {
            var price = candle.GetPrice(PriceToUse);

            buffer.Add(price);

            var factor = Math.Min(index + 1, Period);

            var wsum = priorWsum -
                (index >= Period ? priorSum : 0.0) + factor * price;

            var sum = priorSum + price -
                (index >= Period ? buffer[0] : 0.0);

            var wma = wsum / (0.5 * factor * (factor + 1));

            index++;

            priorWsum = wsum;
            priorSum = sum;

            return GetBasicResult(candle.OpenOn, wma);
        }
    }
}
