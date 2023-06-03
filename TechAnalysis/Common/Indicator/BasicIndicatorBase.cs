// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public abstract class BasicIndicatorBase
{
    private readonly SlidingBuffer<BasicResult> results;

    private int getBasicResultCount = 0;
    public BasicIndicatorBase(int period, PriceToUse priceToUse, int maxResults)
    {
        Period = period.Validated(v => v >= 2);
        
        PriceToUse = priceToUse.Validated(v => v.IsEnumValue());
        
        results = new SlidingBuffer<BasicResult>(
            maxResults.Validated(v => v >= 2), true);
    }

    public BasicResult this[int index] => results[index];

    public int Count => results.Count;

    public bool IsPrimed => getBasicResultCount >= Period;

    protected int Period { get; }
    protected PriceToUse PriceToUse { get; }

    protected BasicResult GetBasicResult(DateTime openOn, double value)
    {
        getBasicResultCount++;

        var result = new BasicResult()
        {
            CloseOn = openOn,
            Value = value
        };

        results.Add(result);

        return result;
    }
}