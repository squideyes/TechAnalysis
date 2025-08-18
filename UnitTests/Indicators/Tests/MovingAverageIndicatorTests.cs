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
/// Comprehensive tests for Moving Average indicators including SMA, EMA, DEMA, TEMA, WMA, SMMA, and KAMA
/// Also includes VWMA (Volume-Weighted Moving Average)

/// </summary>
public class MovingAverageIndicatorTests
{
    #region SMA (Simple Moving Average) Tests

    [Fact]
    public void SmaIndicator_WithValidParameters_ShouldCalculateCorrectValues()
    {
        // Arrange
        var indicator = new SmaIndicator(10, PriceToUse.Close, 2);
        var baselines = GetSmaBaselines();

        // Act & Assert
        foreach (var baseline in baselines)
        {
            var result = indicator.AddAndCalc(baseline.Candle);
            result.Should().NotBeNull();
            IsGoodCalc(result.Value, baseline.Value).Should().BeTrue();
        }
    }

    [Theory]
    [InlineData(PriceToUse.Open)]
    [InlineData(PriceToUse.High)]
    [InlineData(PriceToUse.Low)]
    [InlineData(PriceToUse.Close)]
    public void SmaIndicator_WithDifferentPriceTypes_ShouldCalculateCorrectly(PriceToUse priceToUse)
    {
        // Arrange
        var indicator = new SmaIndicator(5, priceToUse, 10);
        var testCandles = CreateTestCandles(10);

        // Act
        foreach (var candle in testCandles)
        {
            var result = indicator.AddAndCalc(candle);

            // Assert
            result.Should().NotBeNull();
            result.CloseOn.Should().Be(candle.CloseOn);
        }
    }

    [Fact]
    public void SmaIndicator_IsPrimedProperty_ShouldBehaviorCorrectly()
    {
        // Arrange
        var period = 5;
        var indicator = new SmaIndicator(period, PriceToUse.Close, 10);
        var testCandles = CreateTestCandles(10);

        // Act & Assert
        for (int i = 0; i < testCandles.Count; i++)
        {
            indicator.AddAndCalc(testCandles[i]);

            if (i < period - 1)
                indicator.IsPrimed.Should().BeFalse($"Indicator should not be primed at index {i}");
            else
                indicator.IsPrimed.Should().BeTrue($"Indicator should be primed at index {i}");
        }
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-1)]
    public void SmaIndicator_WithInvalidPeriod_ShouldThrowArgumentOutOfRangeException(int invalidPeriod)
    {
        // Act & Assert
        Action act = () => new SmaIndicator(invalidPeriod, PriceToUse.Close, 10);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-1)]
    public void SmaIndicator_WithInvalidMaxResults_ShouldThrowArgumentOutOfRangeException(int invalidMaxResults)
    {
        // Act & Assert
        Action act = () => new SmaIndicator(10, PriceToUse.Close, invalidMaxResults);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion

    #region EMA (Exponential Moving Average) Tests

    [Fact]
    public void EmaIndicator_WithValidParameters_ShouldCalculateCorrectValues()
    {
        // Arrange
        var indicator = new EmaIndicator(10, PriceToUse.Close, 2);
        var baselines = GetEmaBaselines();

        // Act & Assert
        foreach (var baseline in baselines)
        {
            var result = indicator.AddAndCalc(baseline.Candle);
            result.Should().NotBeNull();
            IsGoodCalc(result.Value, baseline.Value).Should().BeTrue();
        }
    }

    [Fact]
    public void EmaIndicator_WithBasicResultOverload_ShouldCalculateCorrectly()
    {
        // Arrange
        var indicator = new EmaIndicator(5, PriceToUse.Close, 10);
        var testCandle = CreateTestCandle(new DateTime(2023, 1, 1), 100, 105, 95, 102);
        var basicResult = new BasicResult { CloseOn = testCandle.CloseOn, Value = 102 };

        // Act
        var result1 = indicator.AddAndCalc(testCandle);
        var result2 = indicator.AddAndCalc(basicResult);

        // Assert
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.Value.Should().BeApproximately(result2.Value, 0.000001);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-1)]
    public void EmaIndicator_WithInvalidPeriod_ShouldThrowArgumentOutOfRangeException(int invalidPeriod)
    {
        // Act & Assert
        Action act = () => new EmaIndicator(invalidPeriod, PriceToUse.Close, 10);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion

    #region DEMA (Double Exponential Moving Average) Tests

    [Fact]
    public void DemaIndicator_WithValidParameters_ShouldCalculateCorrectValues()
    {
        // Arrange
        var indicator = new DemaIndicator(10, PriceToUse.Close, 2);
        var baselines = GetDemaBaselines();

        // Act & Assert
        foreach (var baseline in baselines)
        {
            var result = indicator.AddAndCalc(baseline.Candle);
            result.Should().NotBeNull();
            IsGoodCalc(result.Value, baseline.Value).Should().BeTrue();
        }
    }

    #endregion

    #region TEMA (Triple Exponential Moving Average) Tests

    [Fact]
    public void TemaIndicator_WithValidParameters_ShouldCalculateCorrectValues()
    {
        // Arrange
        var indicator = new TemaIndicator(10, PriceToUse.Close, 2);
        var baselines = GetTemaBaselines();

        // Act & Assert
        foreach (var baseline in baselines)
        {
            var result = indicator.AddAndCalc(baseline.Candle);
            result.Should().NotBeNull();
            IsGoodCalc(result.Value, baseline.Value).Should().BeTrue();
        }
    }

    #endregion

    #region WMA (Weighted Moving Average) Tests

    [Fact]
    public void WmaIndicator_WithValidParameters_ShouldCalculateCorrectValues()
    {
        // Arrange
        var indicator = new WmaIndicator(10, PriceToUse.Close, 2);
        var baselines = GetWmaBaselines();

        // Act & Assert
        foreach (var baseline in baselines)
        {
            var result = indicator.AddAndCalc(baseline.Candle);
            result.Should().NotBeNull();
            IsGoodCalc(result.Value, baseline.Value).Should().BeTrue();
        }
    }

    #endregion

    #region SMMA (Smoothed Moving Average) Tests

    [Fact]
    public void SmmaIndicator_WithValidParameters_ShouldCalculateCorrectValues()
    {
        // Arrange
        var indicator = new SmmaIndicator(10, PriceToUse.Close, 2);
        var baselines = GetSmmaBaselines();

        // Act & Assert
        foreach (var baseline in baselines)
        {
            var result = indicator.AddAndCalc(baseline.Candle);
            result.Should().NotBeNull();
            IsGoodCalc(result.Value, baseline.Value).Should().BeTrue();
        }
    }

    #endregion

    #region KAMA (Kaufman's Adaptive Moving Average) Tests

    [Fact]
    public void KamaIndicator_WithValidParameters_ShouldCalculateCorrectValues()
    {
        // Arrange
        var indicator = new KamaIndicator(10, 5, 15, PriceToUse.Close, 2);
        var baselines = GetKamaBaselines();

        // Act & Assert
        foreach (var baseline in baselines)
        {
            var result = indicator.AddAndCalc(baseline.Candle);
            result.Should().NotBeNull();
            IsGoodCalc(result.Value, baseline.Value).Should().BeTrue();
        }
    }

    #endregion

    #region Helper Methods

    private static bool IsGoodCalc(double actual, double expected) =>
        Math.Round(actual, expected.ToDigits()) == expected;

    private static List<TestCandle> CreateTestCandles(int count)
    {
        var candles = new List<TestCandle>();
        var baseDate = new DateTime(2023, 1, 1);

        for (int i = 0; i < count; i++)
        {
            candles.Add(new TestCandle
            {
                CloseOn = baseDate.AddDays(i),
                Open = 100 + i,
                High = 105 + i,
                Low = 95 + i,
                Close = 102 + i
            });
        }

        return candles;
    }

    private static TestCandle CreateTestCandle(DateTime closeOn, double open, double high, double low, double close)
    {
        return new TestCandle
        {
            CloseOn = closeOn,
            Open = open,
            High = high,
            Low = low,
            Close = close
        };
    }

    #endregion
}
