// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public class ObvIndicator : BasicIndicatorBase, IBasicIndicator
{
    private double? lastPrice;
    private double obv = 0.0;

    public ObvIndicator(int period = 2,
        PriceToUse priceToUse = PriceToUse.Close, int maxResults = 10)
        : base(period, priceToUse, maxResults)
    {
    }

    public BasicResult AddAndCalc(ICandle candle)
    {
        var price = candle.GetPrice(PriceToUse);

        if (!lastPrice.HasValue)
        {
            lastPrice = price;

            // Convention: first value is 0 for OBV
            return GetBasicResult(candle.CloseOn, obv);
        }

        if (price > lastPrice.Value)
            obv += candle.Volume;
        else if (price < lastPrice.Value)
            obv -= candle.Volume;
        // else price == lastPrice: obv unchanged

        lastPrice = price;

        return GetBasicResult(candle.CloseOn, obv);
    }
}
