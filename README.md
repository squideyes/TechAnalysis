![GitHub Workflow Status](https://img.shields.io/github/workflow/status/squideyes/techanalysis/Deploy%20to%20NuGet?label=build)
![NuGet Version](https://img.shields.io/nuget/v/SquidEyes.TechAnalysis)
![Downloads](https://img.shields.io/nuget/dt/squideyes.techanalysis)
![License](https://img.shields.io/github/license/squideyes/TechAnalysis)

**SquidEyes.TechAnalysis** is a collection of high-performance C#/.NET 5.0 technical indicators with a hand-curated set of matching unit-tests.

|Code|Name|Indicator|Kind|
|---|---|---|---|
|**ATR**|Average True Range|AtrIndicator|Statistics|
|**BBAND**|Bollinger Bands|BollingerBandsIndicator|Channels|
|**CCI**|Commodity Channel Index|CciIndicator|Oscillator|
|**DEMA**|Double Exponential Moving Average|DemaIndicator|Moving Average|
|**EMA**|Exponential Moving Average|EmaIndicator|Moving Average|
|**KAMA**|Kaufman's Adaptive Moving Average|KamaIndicator|Moving Average|
|**KLTNR**|Keltner Channel|KelnerChannelIndicator|Channels|
|**LINREG**|Linear Regression|LinRegIndicator|Regression|
|**MACD**|Moving Average Convergence Divergence|MacdIndicator|Statistics|
|**SMA**|Simple Moving Average|SmaIndicator|Moving Average|
|**SMMA**|Smoothed Moving Average|SmmaIndicator|Moving Average|
|**STDDEV**|Standard Deviation|StdDevIndicator|Statistics|
|**STOCH**|Stochastics Oscillator|StochasticsIndicator|Oscillator|
|**TEMA**|Triple Exponential Moving Average|TemaIndicator|Moving Average|
|**WMA**|Weighted Moving Average|WmaIndicator|Moving Average|

To be clear, there are a good number of C#-based technical indicator libraries (i.e. [Trady](https://github.com/karlwancl/Trady), [Stock Indicators](https://github.com/DaveSkender/Stock.Indicators), [TA-Lib.NETCore](https://github.com/hmG3/TA-Lib.NETCore), and [NetTrader]( https://github.com/anilca/NetTrader.Indicator), just to name a few), but the author found each of these to be objectionable for one or more reasons:

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

#
Contributions are always welcome (see [CONTRIBUTING.md](https://github.com/squideyes/TechAnalysis/blob/master/CONTRIBUTING.md) for details)

**Supper-Duper Extra-Important Caveat**:  THE SOFTWARE IS PROVIDED ???AS IS???, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.

More to the point, your use of this code may (literally!) lead to your losing thousands of dollars and more.  Be careful, and remember: Caveat Emptor!!



