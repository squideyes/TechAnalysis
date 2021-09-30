**SquidEyes.TechAnalysis** is a collection of high-performance C#/.NET 5.0 technical indicators with a hand-curated set of matching unit-tests.

|Indicator|Name|Kind|
|---|---|---|
|**ATR**|Average True Range|Statistics|
|**BBAND**|Bollinger Bands|Channels|
|**CCI**|Commodity Channel Index|Oscillator|
|**DEMA**|Double Exponential Moving Average|Moving Average|
|**EMA**|Exponential Moving Average|Moving Average|
|**KAMA**|Kaufman's Adaptive Moving Average|Moving Average|
|**KELTNER**|Keltner Channel|Channels|
|**LINNREG**|Linear Regression|Regression|
|**MACD**|Moving Average Convergence Divergence|Statistics|
|**SMA**|Simple Moving Average|Moving Average|
|**SMMA**|Smoothed Moving Average|Moving Average|
|**STDDEV**|Standard Deviation|Statistics|
|**TEMA**|Triple Exponential Moving Average|Moving Average|
|**WMA**|Weighted Moving Average|Moving Average|

To be clear, there are a good number of C#-based technical indicator libraries (i.e. <a href="https://github.com/karlwancl/Trady" target="_blank">Trady</a>, <a href="https://github.com/DaveSkender/Stock.Indicators" target="_blank">Stock Indicators</a>, <a href="https://github.com/hmG3/TA-Lib.NETCore" target="_blank">TA-Lib.NETCore</a>, and <a href="https://github.com/anilca/NetTrader.Indicator" target="_blank">NetTrader</a>, just to name a few), but the author found each of these to be objectionable for one or more reasons:

* Poor performance (whether from use of decimals or otherwise)
* Poor standardization 
* No provision for float-based candles
* Reliance upon inflexible and inbuilt result-persistence layers

To take a rather simple example, an EmaIndicator might be used as follows:

```csharp
// OpenOn,Open,High,Low,Close data in CSV multi-line format
const string CSV = "..."; 

var indicator = new EmaIndicator(
    period: 21, priceToUse: PriceToUse.Close);

var buffer = new SlidingBuffer<DataPoint>(
    size: 100, reversed: true);

foreach (var fields in new CsvEnumerator(CSV.ToStream(), 5))
{
    var candle = new TestCandle()
    {
        OpenOn = DateTime.Parse(fields[0]),
        Open = float.Parse(fields[1]),
        High = float.Parse(fields[2]),
        Low = float.Parse(fields[3]),
        Close = float.Parse(fields[4])
    };

    buffer.Add(indicator.AddAndCalc(candle));

    if (buffer.IsPrimed)
        AddToChart(buffer[0].OpenOn, buffer[0].Value);
}
```

**Supper-Duper Extra-Important Caveat**:  THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.

More to the point, your use of this code may (literally!) lead to your losing thousands of dollars and more.  Be careful, and remember: Caveat Emptor!!



