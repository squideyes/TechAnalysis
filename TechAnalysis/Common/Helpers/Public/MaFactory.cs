// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public static class MaFactory
{
    public static IBasicIndicator GetMaIndicator(MaKind kind,
        int period, int maxResults = 10, PriceToUse priceToUse = PriceToUse.Close)
    {
        return kind switch
        {
            MaKind.Dema => new DemaIndicator(period, priceToUse, maxResults),
            MaKind.Ema => new EmaIndicator(period, priceToUse, maxResults),
            MaKind.Sma => new SmaIndicator(period, priceToUse, maxResults),
            MaKind.Smma => new SmmaIndicator(period, priceToUse, maxResults),
            MaKind.Tema => new TemaIndicator(period, priceToUse, maxResults),
            MaKind.Wma => new WmaIndicator(period, priceToUse, maxResults),
            _ => throw new ArgumentOutOfRangeException(nameof(period))
        };
    }
}