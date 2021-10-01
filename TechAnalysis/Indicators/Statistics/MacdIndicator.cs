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
    public class MacdIndicator
    {
        private readonly SlidingBuffer<double> fastEmas;
        private readonly SlidingBuffer<double> slowEmas;
        private readonly SlidingBuffer<double> averages;
        private readonly double constant1;
        private readonly double constant2;
        private readonly double constant3;
        private readonly double constant4;
        private readonly double constant5;
        private readonly double constant6;
        private readonly PriceToUse priceToUse;

        private int index = 0;

        public MacdIndicator(int fast, int slow, int smooth, PriceToUse priceToUse)
        {
            fastEmas = new SlidingBuffer<double>(fast, true);
            slowEmas = new SlidingBuffer<double>(slow, true);
            averages = new SlidingBuffer<double>(2, true);

            constant1 = 2.0 / (1 + fast);
            constant2 = 1 - (2.0 / (1 + fast));
            constant3 = 2.0 / (1 + slow);
            constant4 = 1 - (2.0 / (1 + slow));
            constant5 = 2.0 / (1 + smooth);
            constant6 = 1 - (2.0 / (1 + smooth));

            this.priceToUse = priceToUse;
        }

        public MacdResult AddAndCalc(ICandle candle)
        {
            var dataPoint = candle.ToBasicResult(priceToUse);

            fastEmas.Add(0.0);
            slowEmas.Add(0.0);
            averages.Add(0.0);

            if (index++ == 0)
            {
                fastEmas.Update(dataPoint.Value);
                slowEmas.Update(dataPoint.Value);
                averages.Update(0.0);

                return new MacdResult()
                {
                    OpenOn = candle.OpenOn,
                    Value = 0.0,
                    Average = 0.0,
                    Difference = 0.0
                };
            }
            else
            {
                var fastEmaValue = constant1 * dataPoint.Value + constant2 * fastEmas[1];
                var slowEmaValue = constant3 * dataPoint.Value + constant4 * slowEmas[1];
                var macd = fastEmaValue - slowEmaValue;
                var average = constant5 * macd + constant6 * averages[1];

                fastEmas.Update(fastEmaValue);
                slowEmas.Update(slowEmaValue);
                averages.Update(average);

                return new MacdResult()
                {
                    OpenOn = candle.OpenOn,
                    Value = macd,
                    Average = average,
                    Difference = macd - average,
                };
            }
        }
    }
}
