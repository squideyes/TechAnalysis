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

public class TalibComparisonTests_Atr
{
    private static (double[] high, double[] low, double[] close, double[] outReal, Range outRange)
        RunTA(double[] high, double[] low, double[] close, int period)
    {
        var output = new double[close.Length];
        var ret = Functions.Atr<double>(high, low, close, new Range(0, close.Length - 1), output, out var outRange, period);
        ret.Should().Be(TALib.Core.RetCode.Success);
        return (high, low, close, output, outRange);
    }

    [Theory]
    [InlineData(14)]
    public void ATR_ShouldMatch_TALib_NETCore(int period)
    {
        const double Eps = 1e-8;
        var baselines = GetAtrBaselines();

        var high = new List<double>(baselines.Count);
        var low = new List<double>(baselines.Count);
        var close = new List<double>(baselines.Count);
        var ours = new List<double>(baselines.Count);

        var atr = new AtrIndicator(period, maxResults: baselines.Count);

        foreach (var b in baselines)
        {
            high.Add(b.Candle.High);
            low.Add(b.Candle.Low);
            close.Add(b.Candle.Close);
            ours.Add(atr.AddAndCalc(b.Candle).Value);
        }

        var (_, _, _, ta, outRange) = RunTA(high.ToArray(), low.ToArray(), close.ToArray(), period);

        for (int i = outRange.Start.Value, j = 0; i < outRange.End.Value; i++, j++)
            ours[i].Should().BeApproximately(ta[j], Eps);
    }
}

