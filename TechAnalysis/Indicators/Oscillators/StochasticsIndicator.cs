using System;
using System.Linq;

namespace SquidEyes.TechAnalysis
{
    public class StochasticsIndicator
    {
        private readonly SlidingBuffer<ICandle> candles;
        private readonly SlidingBuffer<(DateTime OpenOn, double Value)> fastK;
        private readonly int periodK;
        private readonly int periodD;
        private readonly int smooth;

        public StochasticsIndicator(int periodK, int periodD, int smooth)
        {
            this.periodK = periodK.Validated(nameof(periodK), v => v >= 2);
            this.periodD = periodD.Validated(nameof(periodD), v => v >= 2);
            this.smooth = smooth.Validated(nameof(smooth), v => v >= 1);

            candles = new SlidingBuffer<ICandle>(this.periodK, true);

            fastK = new SlidingBuffer<(DateTime, double)>(this.periodK, true);
        }

        public bool IsPrimed => candles.IsPrimed;

        public (double K, double D) AddAndCalc(ICandle candle)
        {
            if (candle == null)
                throw new ArgumentNullException(nameof(candle));

            candles.Add(candle);

            fastK.Add(GetFastK());

            return GetKD();
        }

        public (double K, double D) UpdateAndCalc(ICandle candle)
        {
            if (candle == null)
                throw new ArgumentNullException(nameof(candle));

            candles.Update(candle);

            fastK.Update(GetFastK());

            return GetKD();
        }

        private (double, double) GetKD()
        {
            var smaFastK = new SmaIndicator(smooth, PriceToUse.Close);
            var smaK = new SmaIndicator(periodD, PriceToUse.Close);

            double k = 0;
            double d = 0;

            for (var i = fastK.Count - 1; i >= 0; i--)
            {
                k = smaFastK.AddAndCalc(fastK[i].OpenOn, fastK[i].Value).Value;
                d = smaK.AddAndCalc(fastK[i].OpenOn, k).Value;
            }

            return (k, d);
        }

        private (DateTime, double) GetFastK()
        {
            var openOn = candles[0].OpenOn;
            var close = candles[0].Close;
            var low = candles.Select(c => c.Low).Min();
            var high = candles.Select(c => c.High).Max();

            var den = high - low;

            if (Math.Abs(den) < 0.00001f)
            {
                if (candles.Count == 1)
                    return (openOn, 50.0);
                else
                    return fastK[0];
            }
            else
            {
                var nom = close - low;

                return (openOn, Math.Min(100.0, Math.Max(0.0, 100.0 * nom / den)));
            }
        }
    }
}
