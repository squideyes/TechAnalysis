// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public class DemaIndicator : BasicIndicatorBase, IBasicIndicator
{
    private readonly EmaIndicator ema1;
    private readonly EmaIndicator ema2;

    public DemaIndicator(int period, PriceToUse priceToUse = PriceToUse.Close)
        : base(period, priceToUse, 2)
    {
        ema1 = new EmaIndicator(Period, priceToUse);
        ema2 = new EmaIndicator(Period, priceToUse);
    }

    public BasicResult AddAndCalc(ICandle candle)
    {
        var value1 = ema1.AddAndCalc(candle).Value;

        var result = GetBasicResult(candle.OpenOn, value1);

        var value2 = ema2.AddAndCalc(result).Value;

        var dema = (2.0 * value1) - value2;

        return GetBasicResult(candle.OpenOn, dema);
    }
}