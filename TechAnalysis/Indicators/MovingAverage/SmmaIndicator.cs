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

using System.Linq;

namespace SquidEyes.TechAnalysis
{
    public class SmmaIndicator : BasicIndicatorBase, IBasicIndicator
    {
        private readonly SlidingBuffer<float> buffer;

        private int index = 0;
        private double lastSmma = 0;
        private double sum = 0;
        private double prevsum = 0;
        private double prevsmma = 0;

        public SmmaIndicator(int period, PriceToUse priceToUse)
            : base(period, priceToUse, 2)
        {
            buffer = new SlidingBuffer<float>(period);
        }

        public DataPoint AddAndCalc(ICandle candle)
        {
            var price = candle.GetPrice(PriceToUse);

            buffer.Add(price);

            double smma;

            if (index++ <= Period)
            {
                sum = buffer.Sum();

                lastSmma = sum / Period;

                smma = lastSmma;
            }
            else
            {
                prevsum = sum;

                prevsmma = lastSmma;

                smma = (prevsum - prevsmma + price) / Period;

                sum = prevsum - prevsmma + price;

                lastSmma = (sum - prevsmma + price) / Period;
            }

            return new DataPoint(candle.OpenOn, smma);
        }
    }
}
