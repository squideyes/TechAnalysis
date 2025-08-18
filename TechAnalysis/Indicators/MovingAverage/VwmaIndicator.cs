// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

// Volume-Weighted Moving Average (VWMA)
// VWMA = sum(price_i * volume_i) / sum(volume_i) over last N bars
public class VwmaIndicator : BasicIndicatorBase, IBasicIndicator
{
    private readonly SlidingBuffer<double> prices;
    private readonly SlidingBuffer<double> volumes;

    private int index = 0;
    private double sumPV = 0.0;   // rolling sum of price*volume
    private double sumV = 0.0;    // rolling sum of volume

    public VwmaIndicator(int period,
        PriceToUse priceToUse = PriceToUse.Close, int maxResults = 10)
        : base(period, priceToUse, maxResults)
    {
        prices = new SlidingBuffer<double>(period + 1);
        volumes = new SlidingBuffer<double>(period + 1);
    }

    public BasicResult AddAndCalc(ICandle candle)
    {
        var price = candle.GetPrice(PriceToUse);
        var volume = candle.Volume;

        prices.Add(price);
        volumes.Add(volume);

        // Add current bar contributions
        sumPV += price * volume;
        sumV += volume;

        // Remove contributions falling out of window once >= Period
        if (index >= Period)
        {
            sumPV -= prices[0] * volumes[0];
            sumV -= volumes[0];
        }

        var periodDiv = Math.Min(index + 1, Period);

        // When not yet primed, denominator should be sumV over partial window
        // Handle degenerate zero-volume case by returning 0 (or last value)
        var vwma = sumV.Approximates(0.0) ? 0.0 : (sumPV / sumV);

        index++;

        return GetBasicResult(candle.CloseOn, vwma);
    }
}
