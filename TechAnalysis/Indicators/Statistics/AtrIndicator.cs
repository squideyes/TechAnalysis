// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public class AtrIndicator : BasicIndicatorBase, IBasicIndicator
{
    private int index = 0;
    private double lastValue = 0.0;
    private ICandle lastCandle = null!;
    private readonly SlidingBuffer<double> trSeed;


    public AtrIndicator(int period,
        PriceToUse priceToUse = PriceToUse.Close, int maxResults = 10)
        : base(period, priceToUse, maxResults)
    {
        trSeed = new SlidingBuffer<double>(Period, true);
    }

    public BasicResult AddAndCalc(ICandle candle)
    {
        double high0 = candle.High;
        double low0 = candle.Low;

        if (index++ == 0)
        {
            // Initialize prev close context and include first TR in the seed
            lastCandle = candle;
            trSeed.Add(high0 - low0);
            lastValue = 0.0;
            return GetBasicResult(candle.CloseOn, 0.0);
        }
        else
        {
            double close1 = lastCandle.Close;
            var tr = Math.Max(Math.Abs(low0 - close1),
                Math.Max(high0 - low0, Math.Abs(high0 - close1)));

            // Accumulate first Period TRs; at lookback, output SMA(TR) per TA-Lib
            if (trSeed.Count < Period)
            {
                trSeed.Add(tr);
                lastCandle = candle;

                if (trSeed.Count < Period)
                {
                    // Before lookback, TA-Lib returns 0s (no ATR yet)
                    lastValue = 0.0;
                    return GetBasicResult(candle.CloseOn, 0.0);
                }
                else
                {
                    double sum = 0.0;
                    for (int i = 0; i < trSeed.Count; i++)
                        sum += trSeed[i];
                    var smaTr = sum / Period;
                    lastValue = smaTr;
                    return GetBasicResult(candle.CloseOn, smaTr);
                }
            }

            // After lookback, Wilder smoothing
            var result = ((Period - 1) * lastValue + tr) / Period;

            lastCandle = candle;
            lastValue = result;

            return GetBasicResult(candle.CloseOn, result);
        }
    }
}