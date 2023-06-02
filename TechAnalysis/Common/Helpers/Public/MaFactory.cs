// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public static class MaFactory
{
    public static IBasicIndicator GetMaIndicator(MaKind kind,
        int period, PriceToUse priceToUse = PriceToUse.Close)
    {
        return kind switch
        {
            MaKind.Dema => new DemaIndicator(period, priceToUse),
            MaKind.Ema => new EmaIndicator(period, priceToUse),
            MaKind.Sma => new SmaIndicator(period, priceToUse),
            MaKind.Smma => new SmmaIndicator(period, priceToUse),
            MaKind.Tema => new TemaIndicator(period, priceToUse),
            MaKind.Wma => new WmaIndicator(period, priceToUse),
            _ => throw new ArgumentOutOfRangeException(nameof(period))
        };
    }
}