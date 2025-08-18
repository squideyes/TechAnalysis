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

public class TalibComparisonTests_Bbands
{
    private static (double[] inReal, double[] outUpper, double[] outMiddle, double[] outLower, Range outRange)
        RunTA(double[] prices, int period, double nbDevUp, double nbDevDn)
    {
        var outUpper = new double[prices.Length];
        var outMiddle = new double[prices.Length];
        var outLower = new double[prices.Length];
        var ret = Functions.Bbands<double>(prices, new Range(0, prices.Length - 1), outUpper, outMiddle, outLower, out var outRange,
            period, nbDevUp, nbDevDn, TALib.Core.MAType.Sma);
        ret.Should().Be(TALib.Core.RetCode.Success);
        return (prices, outUpper, outMiddle, outLower, outRange);
    }

    [Theory]
    [InlineData(20, 2.0, 2.0)]
    public void BBANDS_ShouldMatch_TALib_NETCore(int period, double up, double dn)
    {
        const double Eps = 1e-8;
        var baselines = GetBollingerBandsBaselines();

        var prices = new List<double>(baselines.Count);
        var upper = new List<double>(baselines.Count);
        var middle = new List<double>(baselines.Count);
        var lower = new List<double>(baselines.Count);

        // Our BBandsIndicator uses (period, priceToUse, maxResults, stdDevFactor)
        var bb = new BollingerBandsIndicator(period, PriceToUse.Close, baselines.Count, up);

        foreach (var b in baselines)
        {
            prices.Add(b.Candle.Close);
            var r = bb.AddAndCalc(b.Candle);
            upper.Add(r.Upper);
            middle.Add(r.Middle);
            lower.Add(r.Lower);
        }

        var (_, taU, taM, taL, outRange) = RunTA(prices.ToArray(), period, up, dn);

        for (int i = outRange.Start.Value, j = 0; i < outRange.End.Value; i++, j++)
        {
            upper[i].Should().BeApproximately(taU[j], Eps);
            middle[i].Should().BeApproximately(taM[j], Eps);
            lower[i].Should().BeApproximately(taL[j], Eps);
        }
    }
}

