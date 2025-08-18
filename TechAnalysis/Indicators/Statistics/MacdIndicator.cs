// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public class MacdIndicator
{
    private readonly int fast;
    private readonly int slow;
    private readonly int smooth;
    private readonly PriceToUse priceToUse;

    private readonly EmaIndicator emaFast;
    private readonly EmaIndicator emaSlow;
    private readonly EmaIndicator emaSignal;

    private readonly SlidingBuffer<MacdResult> results;

    public MacdIndicator(int fast, int slow, int smooth,
        PriceToUse priceToUse = PriceToUse.Close, int maxResults = 10)
    {
        this.fast = fast.Validated(v => v >= 2);
        this.slow = slow.Validated(v => v >= 2);
        this.smooth = smooth.Validated(v => v >= 1);

        if (this.fast >= this.slow)
            throw new ArgumentOutOfRangeException(nameof(fast));

        this.priceToUse = priceToUse.Validated(v => v.IsEnumValue());

        emaFast = new EmaIndicator(this.fast, this.priceToUse, 2);
        emaSlow = new EmaIndicator(this.slow, this.priceToUse, 2);
        emaSignal = new EmaIndicator(this.smooth, PriceToUse.Close, 2);

        results = new SlidingBuffer<MacdResult>(
            maxResults.Validated(v => v >= 2), true);
    }

    public MacdResult this[int index] => results[index];

    public MacdResult AddAndCalc(ICandle candle)
    {
        var dataPoint = candle.ToBasicResult(priceToUse);

        var fastValue = emaFast.AddAndCalc(dataPoint).Value;
        var slowValue = emaSlow.AddAndCalc(dataPoint).Value;

        double macd = 0.0;
        var macdValid = emaFast.IsPrimed && emaSlow.IsPrimed;
        if (macdValid)
            macd = fastValue - slowValue;

        double avg = 0.0;
        if (macdValid)
        {
            var signal = emaSignal.AddAndCalc(new BasicResult { CloseOn = candle.CloseOn, Value = macd }).Value;
            if (emaSignal.IsPrimed)
                avg = signal;
        }

        var diff = macd - avg;

        var result = new MacdResult()
        {
            CloseOn = candle.CloseOn,
            Value = macd,
            Average = avg,
            Difference = diff
        };

        results.Add(result);
        return result;
    }
}