// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public class StdDevIndicator : BasicIndicatorBase, IBasicIndicator
{
    private readonly SlidingBuffer<double> prices;
    private readonly SlidingBuffer<double> sumSeries;

    private int index = 0;

    public StdDevIndicator(int period, 
        PriceToUse priceToUse = PriceToUse.Close, int maxResults = 10)
        : base(period, priceToUse, maxResults)
    {
        prices = new SlidingBuffer<double>(period + 1, true);
        sumSeries = new SlidingBuffer<double>(period + 1, true);
    }

    public BasicResult AddAndCalc(ICandle candle)
    {
        var (openOn, value) = candle.ToBasicResult(PriceToUse)
            .Convert(r => (r.CloseOn, r.Value));

        prices.Add(value);

        sumSeries.Add(0.0);

        if (index < 1)
        {
            sumSeries.Update(value);

            index++;

            return GetBasicResult(openOn, 0.0);
        }
        else
        {
            sumSeries.Update(value + sumSeries[1] - (index >= Period ?
                prices[Period] : 0.0));

            var average = sumSeries[0] / Math.Min(index + 1, Period);

            var sum = 0.0;

            for (var barsBack = Math.Min(index, Period - 1); barsBack >= 0; barsBack--)
                sum += (prices[barsBack] - average) * (prices[barsBack] - average);

            var result = GetBasicResult(openOn, 
                Math.Sqrt(sum / Math.Min(index + 1, Period)));

            index++;

            return result;
        }
    }
}