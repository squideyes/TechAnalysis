**SquidEyes.TechAnalysis** is a collection of high-performance C#/.NET 5.0 technical indicators with a hand-curated set of matching unit-tests.

|Indicator|Name|Class|Kind|
|---|---|---|
|**ATR**|Average True Range|AtrIndicator|Statistics|
|**BBAND**|Bollinger Bands|BollingerBandsIndictor|Channels|
|**CCI**|Commodity Channel Index|CciIndicator|Oscillator|
|**DEMA**|Double Exponential Moving Average|DemaIndicator|Moving Average|
|**EMA**|Exponential Moving Average|EmaIndicator|Moving Average|
|**KAMA**|Kaufman's Adaptive Moving Average|KamaIndicator|Moving Average|
|**KELTNER**|Keltner Channel|KelnerChannelIndictor|Channels|
|**LINREG**|Linear Regression|LinRegIndicator|Regression|
|**MACD**|Moving Average Convergence Divergence|MacdIndicator|Statistics|
|**SMA**|Simple Moving Average|SmaIndicator|Moving Average|
|**SMMA**|Smoothed Moving Average|SmmaIndicator|Moving Average|
|**STDDEV**|Standard Deviation|StdDevIndicator|Statistics|
|**STOCH**|Stochastics Oscillator|StochasticsIndicator|Oscillator|
|**TEMA**|Triple Exponential Moving Average|TemaIndicator|Moving Average|
|**WMA**|Weighted Moving Average|WmaIndicator|Moving Average|

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



