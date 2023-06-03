// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public class KamaIndicator : BasicIndicatorBase, IBasicIndicator
{
    private readonly double fastCF;
    private readonly double slowCF;
    private readonly SlidingBuffer<double> diffs;
    private readonly SlidingBuffer<double> values;

    private double lastResult = 0;

    private int index = 0;

    public KamaIndicator(int period, int fast, int slow,
        PriceToUse priceToUse = PriceToUse.Close, int maxResults = 10)
        : base(period, priceToUse, maxResults)
    {
        fastCF = 2.0 / (fast + 1);
        slowCF = 2.0 / (slow + 1);

        diffs = new SlidingBuffer<double>(period + 2, true);

        values = new SlidingBuffer<double>(period + 2, true);
    }

    public BasicResult AddAndCalc(ICandle candle)
    {
        var dataPoint = candle.ToBasicResult(PriceToUse);

        BasicResult UpdateIndexAndLastResultThenGetResult(double value)
        {
            index++;

            return GetBasicResult(candle.CloseOn, lastResult = value);
        }

        values.Add(dataPoint.Value);

        diffs.Add(index > 0 ? Math.Abs(dataPoint.Value - values[1]) : dataPoint.Value);

        if (index < Period)
            return UpdateIndexAndLastResultThenGetResult(dataPoint.Value);

        var signal = Math.Abs(dataPoint.Value - values[Period]);

        var noise = diffs.Take(Period).Sum();

        if (noise == 0.0)
            return UpdateIndexAndLastResultThenGetResult(dataPoint.Value);

        var result = lastResult + Math.Pow(signal / noise *
            (fastCF - slowCF) + slowCF, 2) * (dataPoint.Value - lastResult);

        return UpdateIndexAndLastResultThenGetResult(result);
    }
}