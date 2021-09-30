using System;

namespace SquidEyes.TechAnalysis
{
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

        public static DataPoint ToDataPoint(this ICandle candle, PriceToUse priceToUse) =>
            new(candle.OpenOn, candle.GetPrice(priceToUse));
    }
}
