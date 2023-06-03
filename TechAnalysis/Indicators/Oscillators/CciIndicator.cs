// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public class CciIndicator : BasicIndicatorBase, IBasicIndicator
{
    private readonly SmaIndicator sma;
    private readonly SlidingBuffer<double> typical;

    private int barCount = 0;

    public CciIndicator(int period, 
        PriceToUse priceToUse = PriceToUse.Close, int maxResults = 10)
        : base(period, priceToUse, maxResults)
    {
        sma = new SmaIndicator(Period, priceToUse, 2);

        typical = new SlidingBuffer<double>(period, true);
    }

    public BasicResult AddAndCalc(ICandle candle)
    {
        double result;

        typical.Add(candle.Convert(c => (c.High + c.Low + c.Close) / 3.0));

        var sma0 = sma.AddAndCalc(candle.CloseOn, typical[0]).Value;

        if (barCount == 0)
        {
            result = 0;
        }
        else
        {
            var mean = 0.0;

            for (var idx = Math.Min(barCount, Period - 1); idx >= 0; idx--)
                mean += Math.Abs(typical[idx] - sma0);

            result = (typical[0] - sma0) / (mean.Approximates(0.0) ?
                1 : (0.015 * (mean / Math.Min(Period, barCount + 1))));
        }

        barCount++;

        return GetBasicResult(candle.CloseOn, result);
    }
}