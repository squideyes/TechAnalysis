// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public class SmaIndicator : BasicIndicatorBase, IBasicIndicator
{
    private readonly SlidingBuffer<double> buffer;

    private int index = 0;
    private double sum = 0;
    private double priorSum = 0;

    public SmaIndicator(int period, 
        PriceToUse priceToUse = PriceToUse.Close, int maxResults = 10)
        : base(period, priceToUse, maxResults)
    {
        buffer = new SlidingBuffer<double>(period + 1);
    }

    public BasicResult AddAndCalc(ICandle candle) =>
        AddAndCalc(candle.CloseOn, candle.GetPrice(PriceToUse));

    public BasicResult AddAndCalc(DateTime openOn, double price)
    {
        buffer.Add(price);

        sum = priorSum + price - (index >= Period ? buffer[0] : 0);

        var sma = sum / (index < Period ? index + 1 : Period);

        priorSum = sum;

        index++;

        return GetBasicResult(openOn, sma);
    }
}