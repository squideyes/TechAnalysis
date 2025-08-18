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

public class TalibComparisonTests_Stoch
{
    private static (double[] high, double[] low, double[] close, double[] outK, double[] outD, Range outRange)
        RunTA(double[] high, double[] low, double[] close, int fastK, int slowK, int slowD)
    {
        var outK = new double[close.Length];
        var outD = new double[close.Length];
        var ret = Functions.Stoch<double>(high, low, close, new Range(0, close.Length - 1), outK, outD, out var outRange,
            fastK, slowK, TALib.Core.MAType.Sma, slowD, TALib.Core.MAType.Sma);
        ret.Should().Be(TALib.Core.RetCode.Success);
        return (high, low, close, outK, outD, outRange);
    }

    [Theory]
    [InlineData(14, 3, 3)]
    public void Stoch_ShouldMatch_TALib_NETCore(int fastK, int slowK, int slowD)
    {
        const double Eps = 1e-8;
        var baselines = GetStochasticsBaselines();

        var high = new List<double>(baselines.Count);
        var low = new List<double>(baselines.Count);
        var close = new List<double>(baselines.Count);
        var k = new List<double>(baselines.Count);
        var d = new List<double>(baselines.Count);

        var stoch = new StochasticsIndicator(fastK, slowK, slowD, maxResults: baselines.Count);

        foreach (var b in baselines)
        {
            high.Add(b.Candle.High);
            low.Add(b.Candle.Low);
            close.Add(b.Candle.Close);
            var r = stoch.AddAndCalc(b.Candle);
            k.Add(r.K);
            d.Add(r.D);
        }

        var (_, _, _, taK, taD, outRange) = RunTA(high.ToArray(), low.ToArray(), close.ToArray(), fastK, slowK, slowD);

        for (int i = outRange.Start.Value, j = 0; i < outRange.End.Value; i++, j++)
        {
            k[i].Should().BeApproximately(taK[j], Eps);
            d[i].Should().BeApproximately(taD[j], Eps);
        }
    }
}

