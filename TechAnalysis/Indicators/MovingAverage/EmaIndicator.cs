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

    public EmaIndicator(int period, PriceToUse priceToUse = PriceToUse.Close)
        : base(period, priceToUse, 2)
    {
        constant1 = 2.0 / (1 + period);
        constant2 = 1.0 - (2.0 / (1 + period));
    }

    public BasicResult AddAndCalc(ICandle candle) =>
        AddAndCalc(candle.ToBasicResult(PriceToUse.Close));

    public BasicResult AddAndCalc(BasicResult result)
    {
        var ema = !lastEma.HasValue ? result.Value :
            result.Value * constant1 + constant2 * lastEma;

        lastEma = ema;

        return GetBasicResult(result.OpenOn, ema.Value);
    }
}