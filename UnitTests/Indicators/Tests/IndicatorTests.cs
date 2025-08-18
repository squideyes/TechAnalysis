// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System;
using System.Collections.Generic;
using SquidEyes.TechAnalysis;
using Xunit;
using FluentAssertions;
using static SquidEyes.UnitTests.TestData;

namespace SquidEyes.UnitTests;

/// <summary>
/// Legacy baseline tests for indicators - maintained for backward compatibility
/// For comprehensive testing, see the new test classes: MovingAverageIndicatorTests,
/// OscillatorIndicatorTests, ChannelIndicatorTests, StatisticsIndicatorTests, etc.
/// </summary>
public class LegacyIndicatorBaselineTests
{
    [Fact]
    public void KeltnerChannelIndicator_WithBaselineData_ShouldCalculateExpectedValues()
    {
        // Arrange
        var indicator = new KeltnerChannelIndictor(20, PriceToUse.Close, 2, 1.5);
        var baselines = GetKeltnerChannelBaselines();

        // Act & Assert
        foreach (var baseline in baselines)
        {
            var result = indicator.AddAndCalc(baseline.Candle);

            IsGoodCalc(result.Upper, baseline.Upper).Should().BeTrue(
                $"Upper band should match baseline: expected {baseline.Upper}, got {result.Upper}");
            IsGoodCalc(result.Middle, baseline.Middle).Should().BeTrue(
                $"Middle line should match baseline: expected {baseline.Middle}, got {result.Middle}");
            IsGoodCalc(result.Lower, baseline.Lower).Should().BeTrue(
                $"Lower band should match baseline: expected {baseline.Lower}, got {result.Lower}");
        }
    }

    [Fact]
    public void BollingerBandsIndicator_WithBaselineData_ShouldCalculateExpectedValues()
    {
        // Arrange
        var indicator = new BollingerBandsIndictor(10, PriceToUse.Close, 2, 2.0);
        var baselines = GetBollingerBandsBaselines();

        // Act & Assert
        foreach (var baseline in baselines)
        {
            var result = indicator.AddAndCalc(baseline.Candle);

            IsGoodCalc(result.Upper, baseline.Upper).Should().BeTrue(
                $"Upper band should match baseline: expected {baseline.Upper}, got {result.Upper}");
            IsGoodCalc(result.Middle, baseline.Middle).Should().BeTrue(
                $"Middle line should match baseline: expected {baseline.Middle}, got {result.Middle}");
            IsGoodCalc(result.Lower, baseline.Lower).Should().BeTrue(
                $"Lower band should match baseline: expected {baseline.Lower}, got {result.Lower}");
        }
    }

    [Fact]
    public void MacdIndicator_WithBaselineData_ShouldCalculateExpectedValues()
    {
        // Arrange
        var indicator = new MacdIndicator(10, 15, 3, PriceToUse.Close, 10);
        var baselines = GetMacdBaselines();

        // Act & Assert
        foreach (var baseline in baselines)
        {
            var result = indicator.AddAndCalc(baseline.Candle);

            IsGoodCalc(result.Average, baseline.Average).Should().BeTrue(
                $"MACD Average should match baseline: expected {baseline.Average}, got {result.Average}");
            IsGoodCalc(result.Value, baseline.Value).Should().BeTrue(
                $"MACD Value should match baseline: expected {baseline.Value}, got {result.Value}");
            IsGoodCalc(result.Difference, baseline.Difference).Should().BeTrue(
                $"MACD Difference should match baseline: expected {baseline.Difference}, got {result.Difference}");
        }
    }

    [Fact]
    public void AtrIndicator_WithBaselineData_ShouldCalculateExpectedValues() =>
        BasicIndicatorBaselineTest(new AtrIndicator(10, PriceToUse.Close, 2), GetAtrBaselines());

    [Fact]
    public void CciIndicator_WithBaselineData_ShouldCalculateExpectedValues() =>
        BasicIndicatorBaselineTest(new CciIndicator(20, PriceToUse.Close, 2), GetCciBaselines());

    [Fact]
    public void DemaIndicator_WithBaselineData_ShouldCalculateExpectedValues() =>
        BasicIndicatorBaselineTest(new DemaIndicator(10, PriceToUse.Close, 2), GetDemaBaselines());

    [Fact]
    public void EmaIndicator_WithBaselineData_ShouldCalculateExpectedValues() =>
        BasicIndicatorBaselineTest(new EmaIndicator(10, PriceToUse.Close, 2), GetEmaBaselines());

    [Fact]
    public void SmaIndicator_WithBaselineData_ShouldCalculateExpectedValues() =>
        BasicIndicatorBaselineTest(new SmaIndicator(10, PriceToUse.Close, 2), GetSmaBaselines());

    [Fact]
    public void TemaIndicator_WithBaselineData_ShouldCalculateExpectedValues() =>
        BasicIndicatorBaselineTest(new TemaIndicator(10, PriceToUse.Close, 2), GetTemaBaselines());

    [Fact]
    public void WmaIndicator_WithBaselineData_ShouldCalculateExpectedValues() =>
        BasicIndicatorBaselineTest(new WmaIndicator(10, PriceToUse.Close, 2), GetWmaBaselines());

    [Fact]
    public void LinRegIndicator_WithBaselineData_ShouldCalculateExpectedValues() =>
        BasicIndicatorBaselineTest(new LinRegIndicator(10, PriceToUse.Close, 2), GetLinRegBaselines());

    [Fact]
    public void StdDevIndicator_WithBaselineData_ShouldCalculateExpectedValues() =>
        BasicIndicatorBaselineTest(new StdDevIndicator(10, PriceToUse.Close, 2), GetStdDevBaselines());

    [Fact]
    public void KamaIndicator_WithBaselineData_ShouldCalculateExpectedValues() =>
        BasicIndicatorBaselineTest(new KamaIndicator(10, 5, 15, PriceToUse.Close, 2), GetKamaBaselines());

    [Fact]
    public void SmmaIndicator_WithBaselineData_ShouldCalculateExpectedValues() =>
        BasicIndicatorBaselineTest(new SmmaIndicator(10, PriceToUse.Close, 2), GetSmmaBaselines());

    [Fact]
    public void StochasticsIndicator_WithBaselineData_ShouldCalculateExpectedValues()
    {
        // Arrange
        var indicator = new StochasticsIndicator(14, 7, 3);
        var baselines = GetStochasticsBaselines();

        // Act & Assert
        foreach (var baseline in baselines)
        {
            var addResult = indicator.AddAndCalc(baseline.Candle);
            addResult.K.Should().BeApproximately(baseline.K, 0.00000001,
                $"AddAndCalc K value should match baseline: expected {baseline.K}, got {addResult.K}");
            addResult.D.Should().BeApproximately(baseline.D, 0.00000001,
                $"AddAndCalc D value should match baseline: expected {baseline.D}, got {addResult.D}");

            var updateResult = indicator.UpdateAndCalc(baseline.Candle);
            updateResult.K.Should().BeApproximately(baseline.K, 0.00000001,
                $"UpdateAndCalc K value should match baseline: expected {baseline.K}, got {updateResult.K}");
            updateResult.D.Should().BeApproximately(baseline.D, 0.00000001,
                $"UpdateAndCalc D value should match baseline: expected {baseline.D}, got {updateResult.D}");
        }
    }

    private static void BasicIndicatorBaselineTest(
        IBasicIndicator indicator, List<ValueBaseline> baselines)
    {
        foreach (var baseline in baselines)
        {
            var result = indicator.AddAndCalc(baseline.Candle);

            result.Should().Be(indicator[0], "Result should match the most recent value from indexer");
            IsGoodCalc(result.Value, baseline.Value).Should().BeTrue(
                $"Indicator value should match baseline: expected {baseline.Value}, got {result.Value}");
        }
    }

    private static bool IsGoodCalc(double actual, double expected) =>
        Math.Round(actual, expected.ToDigits()) == expected;
}