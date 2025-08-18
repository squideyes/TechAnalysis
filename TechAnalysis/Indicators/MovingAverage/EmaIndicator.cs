// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public class EmaIndicator : BasicIndicatorBase, IBasicIndicator
{
    private readonly double constant1;
    private readonly double constant2;
    private double? lastEma;
    private readonly SlidingBuffer<double> seed;


    public EmaIndicator(int period,
        PriceToUse priceToUse = PriceToUse.Close, int maxResults = 10)
        : base(period, priceToUse, maxResults)
    {
        constant1 = 2.0 / (1 + period);
        constant2 = 1.0 - (2.0 / (1 + period));
        seed = new SlidingBuffer<double>(Period, true);
    }

    public BasicResult AddAndCalc(ICandle candle) =>
        AddAndCalc(candle.ToBasicResult(PriceToUse.Close));

    public BasicResult AddAndCalc(BasicResult result)
    {
        if (!lastEma.HasValue)
        {
            seed.Add(result.Value);

            if (seed.Count < Period)
            {
                return GetBasicResult(result.CloseOn, result.Value);
            }
            else
            {
                double sum = 0.0;
                for (int i = 0; i < seed.Count; i++)
                    sum += seed[i];
                var sma = sum / Period;
                lastEma = sma;
                return GetBasicResult(result.CloseOn, sma);
            }
        }
        else
        {
            var ema = result.Value * constant1 + constant2 * lastEma;
            lastEma = ema;
            return GetBasicResult(result.CloseOn, ema.Value);
        }
    }
}