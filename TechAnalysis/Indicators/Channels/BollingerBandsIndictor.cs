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
    public class BollingerBandsIndictor : BasicIndicatorBase
    {
        private readonly SmaIndicator smaIndicator;
        private readonly StdDevIndicator stdDevIndicator;
        private readonly double stdDevFactor;

        public BollingerBandsIndictor(int period, PriceToUse priceToUse, double stdDevFactor)
            : base(period, priceToUse, 2)
        {
            if (stdDevFactor <= 0.0)
                throw new ArgumentOutOfRangeException(nameof(stdDevFactor));

            this.stdDevFactor = stdDevFactor;

            smaIndicator = new SmaIndicator(period, priceToUse);

            stdDevIndicator = new StdDevIndicator(period, priceToUse);
        }

        public ChannelResult AddAndCalc(ICandle candle)
        {
            var smaValue = smaIndicator.AddAndCalc(candle).Value;

            var stdDevValue = stdDevIndicator.AddAndCalc(candle).Value;

            var delta = stdDevFactor * stdDevValue;

            return new ChannelResult()
            {
                OpenOn = candle.OpenOn,
                Upper = smaValue + delta,
                Middle = smaValue,
                Lower = smaValue - delta
            };
        }
    }
}
