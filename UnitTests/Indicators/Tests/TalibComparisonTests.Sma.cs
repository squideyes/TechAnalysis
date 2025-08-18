// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System;
using System.Collections.Generic;
using FluentAssertions;
using SquidEyes.TechAnalysis;
using Xunit;
using TALib;
using System.Linq;
using static SquidEyes.UnitTests.TestData;

namespace SquidEyes.UnitTests;

public class TalibComparisonTests_Sma
{
    private static (double[] prices, double[] taValues, int startIdx, int endIdx)
        RunTALibSMA(double[] prices, int period)
    {
        var output = new double[prices.Length];
        var inRange = new System.Range(0, prices.Length - 1);
        var ret = Functions.Sma<double>(prices, inRange, output, out var outRange, period);
        ret.Should().Be(TALib.Core.RetCode.Success);
        outRange.Start.Value.Should().BeGreaterThanOrEqualTo(0);
        outRange.End.Value.Should().BeGreaterThan(outRange.Start.Value);

        // Return TA output along with its valid output index range for alignment
        var startIdx = outRange.Start.Value;
        var endIdx = outRange.End.Value - 1; // outRange.End is exclusive
        return (prices, output, startIdx, endIdx);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(10)]
    public void SMA_ShouldMatch_TALib_NETCore(int period)
    {
        const double Eps = 1e-8;
        var baselines = GetSmaBaselines();

        var prices = new List<double>(baselines.Count);
        var techValues = new List<double>(baselines.Count);

        var sma = new SmaIndicator(period, PriceToUse.Close, maxResults: baselines.Count);

        foreach (var b in baselines)
        {
            prices.Add(b.Candle.Close);
            var r = sma.AddAndCalc(b.Candle);
            techValues.Add(r.Value);
        }

        var (_, talibValues, startIdx, endIdx) = RunTALibSMA(prices.ToArray(), period);

        // Compare only within the valid TA-Lib range, align indices
        for (int srcIndex = startIdx, j = 0; srcIndex <= endIdx; srcIndex++, j++)
        {
            techValues[srcIndex].Should().BeApproximately(talibValues[j], Eps);
        }
    }
}
