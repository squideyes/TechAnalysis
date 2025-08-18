// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public class DemaIndicator : BasicIndicatorBase, IBasicIndicator
{
    private readonly EmaIndicator ema1;
    private readonly double c1;
    private readonly double c2;
    private double? ema2;
    private readonly SlidingBuffer<double> ema2Seed;

    public DemaIndicator(int period,
        PriceToUse priceToUse = PriceToUse.Close, int maxResults = 10)
        : base(period, priceToUse, maxResults)
    {
        ema1 = new EmaIndicator(Period, priceToUse, 2);
        c1 = 2.0 / (1 + Period);
        c2 = 1.0 - c1;
        ema2Seed = new SlidingBuffer<double>(Period, true);
    }

    public BasicResult AddAndCalc(ICandle candle)
    {
        var value1 = ema1.AddAndCalc(candle).Value;

        if (!ema2.HasValue)
        {
            if (!ema1.IsPrimed)
            {
                // Before EMA1 lookback, mirror EMA1 output for continuity
                return GetBasicResult(candle.CloseOn, value1);
            }

            // Collect first Period EMA1 values to seed EMA2 with SMA
            ema2Seed.Add(value1);
            if (ema2Seed.Count < Period)
                return GetBasicResult(candle.CloseOn, value1);

            double sum = 0.0;
            for (int i = 0; i < ema2Seed.Count; i++)
                sum += ema2Seed[i];
            ema2 = sum / Period;
        }
        else
        {
            ema2 = c1 * value1 + c2 * ema2;
        }

        var dema = (2.0 * value1) - ema2.Value;
        return GetBasicResult(candle.CloseOn, dema);
    }
}