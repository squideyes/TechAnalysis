![NuGet Version](https://img.shields.io/nuget/v/SquidEyes.TechAnalysis)
![Downloads](https://img.shields.io/nuget/dt/squideyes.techanalysis)
![License](https://img.shields.io/github/license/squideyes/TechAnalysis)

**SquidEyes.TechAnalysis** is a collection of high-performance C#/.NET 7.0 technical indicators with a hand-curated set of matching unit-tests.

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
// Add the result to a chart, perform a calculation, etc.
static void PrintResult(BasicResult result) =>
    Debug.WriteLine($"{result.CloseOn} ={result.Value}");

// CloseOn,Open,High,Low,Close data in CSV multi-line format
const string CSV = """
    05/15/2023 14:36:00,12872.75,12873.75,12866,12870.75
    05/15/2023 14:37:00,12871.25,12871.5,12864.75,12869.5
    05/15/2023 14:38:00,12869.25,12871.75,12867.5,12871.25
    05/15/2023 14:39:00,12871,12873.75,12869.5,12872.75
    05/15/2023 14:40:00,12872.75,12873.75,12867.75,12870.5 
    """;

// Most of the indicators return BasicResult, but a few return
// custom results (BollingerIndicator, MacddIndicator, etc.)
var indicator = new EmaIndicator(period: 3,
    priceToUse: PriceToUse.Close, maxResults: 10000);

// Parse the data then 
foreach (var fields in new CsvEnumerator(CSV.ToStream(), 5))
{
    var candle = new TestCandle()
    {
        CloseOn = DateTime.Parse(fields[0]),
        Open = float.Parse(fields[1]),
        High = float.Parse(fields[2]),
        Low = float.Parse(fields[3]),
        Close = float.Parse(fields[4])
    };

    // Adds the current result to the indicator's results
    // collection (a reversed SlidingBuffer) then returns 
    // that result directly
    _ = indicator.AddAndCalc(candle);

    if (!indicator.IsPrimed)
        continue;

    // A result may also be obtained via a [0..maxResults - 1]
    // index on the indicator, with zero being the most recent
    PrintResult(indicator[2]);
}
```

#
Contributions are always welcome (see [CONTRIBUTING.md](https://github.com/squideyes/TechAnalysis/blob/master/CONTRIBUTING.md) for details)

**Supper-Duper Extra-Important Caveat**:  THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.

**FINAL WARNING**: The use of **this code may (literally!) lead to your losing thousands of dollars** or more.  **Caveat Emptor!!**



