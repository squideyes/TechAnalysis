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
    public class KeltnerChannelIndictor : BasicIndicatorBase
    {
        private readonly double offsetMultiplier;
        private readonly SmaIndicator smaDiff;
        private readonly SmaIndicator smaTypical;
        private readonly SlidingBuffer<double> diff;
        private readonly SlidingBuffer<double> typical;

        public KeltnerChannelIndictor(int period, PriceToUse priceToUse, double offsetMultiplier = 1.5)
            : base(period, priceToUse, 2)
        {
            this.offsetMultiplier = offsetMultiplier
                .Validated(nameof(offsetMultiplier), v => v.InRange(0.5, 5.0));

            diff = new SlidingBuffer<double>(period, true);
            typical = new SlidingBuffer<double>(period, true);
            smaDiff = new SmaIndicator(period, priceToUse);
            smaTypical = new SmaIndicator(period, priceToUse);
        }

        public ChannelResult AddAndCalc(ICandle candle)
        {
            diff.Add(candle.High - candle.Low);

            typical.Add(candle.Funcify(c => (c.High + c.Low + c.Close) / 3.0));

            var middle = smaTypical.AddAndCalc(
                candle.OpenOn, typical[0]).Value;

            var x = smaDiff.AddAndCalc(candle.OpenOn, diff[0]).Value;

            var offset = x * offsetMultiplier;
            
            var upper = middle + offset;

            var lower = middle - offset;

            return new ChannelResult(candle.OpenOn, upper, middle, lower);
        }
    }
}
