// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public class KeltnerChannelIndictor : BasicIndicatorBase
{
    private readonly double offsetMultiplier;
    private readonly SmaIndicator smaDiff;
    private readonly SmaIndicator smaTypical;
    private readonly SlidingBuffer<double> diff;
    private readonly SlidingBuffer<double> typical;

    public KeltnerChannelIndictor(int period, PriceToUse priceToUse = PriceToUse.Close, 
        int maxResults = 10, double offsetMultiplier = 1.5)
        : base(period, priceToUse, maxResults)
    {
        this.offsetMultiplier = offsetMultiplier
            .Validated(v => v.IsBetween(0.5, 5.0));

        diff = new SlidingBuffer<double>(period, true);
        typical = new SlidingBuffer<double>(period, true);
        smaDiff = new SmaIndicator(period, priceToUse, 2);
        smaTypical = new SmaIndicator(period, priceToUse, 2);
    }

    public ChannelResult AddAndCalc(ICandle candle)
    {
        diff.Add(candle.High - candle.Low);

        typical.Add(candle.Convert(c => (c.High + c.Low + c.Close) / 3.0));

        var middle = smaTypical.AddAndCalc(
            candle.CloseOn, typical[0]).Value;

        var x = smaDiff.AddAndCalc(candle.CloseOn, diff[0]).Value;

        var offset = x * offsetMultiplier;

        return new ChannelResult()
        {
            CloseOn = candle.CloseOn,
            Upper = middle + offset,
            Middle = middle,
            Lower = middle - offset
        };
    }
}