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

public class TalibComparisonTests_Macd
{
    private static (double[] inReal, double[] macd, double[] signal, double[] hist, Range outRange)
        RunTA(double[] prices, int fast, int slow, int signal)
    {
        var macd = new double[prices.Length];
        var signalArr = new double[prices.Length];
        var hist = new double[prices.Length];
        var ret = Functions.Macd<double>(prices, new Range(0, prices.Length - 1), macd, signalArr, hist, out var outRange, fast, slow, signal);
        ret.Should().Be(TALib.Core.RetCode.Success);
        return (prices, macd, signalArr, hist, outRange);
    }

    [Theory]
    [InlineData(12, 26, 9)]
    public void MACD_ShouldMatch_TALib_NETCore(int fast, int slow, int signal)
    {
        const double Eps = 1e-8;
        var baselines = GetMacdBaselines();

        var prices = new List<double>(baselines.Count);
        var oursValue = new List<double>(baselines.Count);
        var oursAvg = new List<double>(baselines.Count);
        var oursDiff = new List<double>(baselines.Count);

        var macdInd = new MacdIndicator(fast, slow, signal, maxResults: baselines.Count);

        foreach (var b in baselines)
        {
            prices.Add(b.Candle.Close);
            var r = macdInd.AddAndCalc(b.Candle);
            oursValue.Add(r.Value);
            oursAvg.Add(r.Average);
            oursDiff.Add(r.Difference);
        }

        var (_, taMacd, taSignal, taHist, outRange) = RunTA(prices.ToArray(), fast, slow, signal);

        for (int i = outRange.Start.Value, j = 0; i < outRange.End.Value; i++, j++)
        {
            oursValue[i].Should().BeApproximately(taMacd[j], Eps);
            oursAvg[i].Should().BeApproximately(taSignal[j], Eps);
            oursDiff[i].Should().BeApproximately(taHist[j], Eps);
        }
    }
}

