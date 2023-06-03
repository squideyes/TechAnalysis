// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public class BollingerBandsIndictor : BasicIndicatorBase
{
    private readonly SmaIndicator smaIndicator;
    private readonly StdDevIndicator stdDevIndicator;
    private readonly double stdDevFactor;

    public BollingerBandsIndictor(int period, PriceToUse priceToUse = 
        PriceToUse.Close, int maxResults = 10, double stdDevFactor = 2.0)
        : base(period, priceToUse, maxResults)
    {
        if (stdDevFactor <= 0.0)
            throw new ArgumentOutOfRangeException(nameof(stdDevFactor));

        this.stdDevFactor = stdDevFactor;

        smaIndicator = new SmaIndicator(period, priceToUse, 2);

        stdDevIndicator = new StdDevIndicator(period, priceToUse, 2);
    }

    public ChannelResult AddAndCalc(ICandle candle)
    {
        var smaValue = smaIndicator.AddAndCalc(candle).Value;

        var stdDevValue = stdDevIndicator.AddAndCalc(candle).Value;

        var delta = stdDevFactor * stdDevValue;

        return new ChannelResult()
        {
            CloseOn = candle.CloseOn,
            Upper = smaValue + delta,
            Middle = smaValue,
            Lower = smaValue - delta
        };
    }
}