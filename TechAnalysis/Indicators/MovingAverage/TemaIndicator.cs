// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public class TemaIndicator : BasicIndicatorBase, IBasicIndicator
{
    private readonly EmaIndicator ema1;
    private readonly EmaIndicator ema2;
    private readonly EmaIndicator ema3;

    public TemaIndicator(int period,
        PriceToUse priceToUse = PriceToUse.Close, int maxResults = 10)
        : base(period, priceToUse, maxResults)
    {
        ema1 = new EmaIndicator(Period, priceToUse, 2);
        ema2 = new EmaIndicator(Period, priceToUse, 2);
        ema3 = new EmaIndicator(Period, priceToUse, 2);
    }

    public BasicResult AddAndCalc(ICandle candle)
    {
        var value1 = ema1.AddAndCalc(candle).Value;

        double value2 = value1;
        if (ema1.IsPrimed)
        {
            value2 = ema2.AddAndCalc(new BasicResult { CloseOn = candle.CloseOn, Value = value1 }).Value;
        }

        double value3 = value2;
        if (ema1.IsPrimed && ema2.IsPrimed)
        {
            value3 = ema3.AddAndCalc(new BasicResult { CloseOn = candle.CloseOn, Value = value2 }).Value;
        }

        var tema = (3 * value1) - (3 * value2) + value3;
        return GetBasicResult(candle.CloseOn, tema);
    }
}