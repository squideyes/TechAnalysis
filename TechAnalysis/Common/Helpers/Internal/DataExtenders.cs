// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public static class DataExtenders
{
    public static float GetPrice(this ICandle candle, PriceToUse priceToUse) => 
        priceToUse switch
        {
            PriceToUse.Open => candle.Open,
            PriceToUse.High => candle.High,
            PriceToUse.Low => candle.Low,
            PriceToUse.Close => candle.Close,
            _ => throw new ArgumentOutOfRangeException(nameof(priceToUse))
        };

    public static BasicResult ToBasicResult(this ICandle candle, PriceToUse priceToUse) =>
        new()
        {
            OpenOn = candle.OpenOn,
            Value = candle.GetPrice(priceToUse)
        };
}