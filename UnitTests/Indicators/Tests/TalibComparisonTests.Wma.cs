// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System;
using System.Collections.Generic;
using FluentAssertions;
using SquidEyes.TechAnalysis;
using TALib;
using Xunit;
using static SquidEyes.UnitTests.TestData;

namespace SquidEyes.UnitTests;

public class TalibComparisonTests_Wma
{
    private static (double[] inReal, double[] outReal, Range outRange)
        RunTA(double[] prices, int period)
    {
        var output = new double[prices.Length];
        var ret = Functions.Wma<double>(prices, new Range(0, prices.Length - 1), output, out var outRange, period);
        ret.Should().Be(TALib.Core.RetCode.Success);
        return (prices, output, outRange);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(10)]
    public void WMA_ShouldMatch_TALib_NETCore(int period)
    {
        const double Eps = 1e-8;
        var baselines = GetWmaBaselines();

        var prices = new List<double>(baselines.Count);
        var ours = new List<double>(baselines.Count);

        var wma = new WmaIndicator(period, PriceToUse.Close, maxResults: baselines.Count);

        foreach (var b in baselines)
        {
            prices.Add(b.Candle.Close);
            ours.Add(wma.AddAndCalc(b.Candle).Value);
        }

        var (_, ta, outRange) = RunTA(prices.ToArray(), period);

        for (int i = outRange.Start.Value, j = 0; i < outRange.End.Value; i++, j++)
            ours[i].Should().BeApproximately(ta[j], Eps);
    }
}

