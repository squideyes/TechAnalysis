// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System;
using SquidEyes.TechAnalysis;
using SquidEyes.Fundamentals;
using Xunit;
using System.Diagnostics;

namespace SquidEyes.UnitTests;

public class ReadmeHowTo
{
    [Fact]
    public void SimpleTest()
    {
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
    }
}