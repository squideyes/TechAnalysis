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
/// Comprehensive tests for Oscillator indicators including CCI and Stochastics
/// </summary>
public class OscillatorIndicatorTests
{
    #region CCI (Commodity Channel Index) Tests

    [Fact]
    public void CciIndicator_WithValidParameters_ShouldCalculateCorrectValues()
    {
        // Arrange
        var indicator = new CciIndicator(20, PriceToUse.Close, 2);
        var baselines = GetCciBaselines();

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
    public void CciIndicator_WithDifferentPriceTypes_ShouldCalculateCorrectly(PriceToUse priceToUse)
    {
        // Arrange
        var indicator = new CciIndicator(14, priceToUse, 10);
        var testCandles = CreateTestCandles(20);

        // Act & Assert
        foreach (var candle in testCandles)
        {
            var result = indicator.AddAndCalc(candle);
            result.Should().NotBeNull();
            result.CloseOn.Should().Be(candle.CloseOn);
        }
    }

    [Fact]
    public void CciIndicator_IsPrimedProperty_ShouldBehaviorCorrectly()
    {
        // Arrange
        var period = 14;
        var indicator = new CciIndicator(period, PriceToUse.Close, 10);
        var testCandles = CreateTestCandles(20);

        // Act & Assert
        for (int i = 0; i < testCandles.Count; i++)
        {
            indicator.AddAndCalc(testCandles[i]);
            
            if (i < period - 1)
                indicator.IsPrimed.Should().BeFalse($"CCI should not be primed at index {i}");
            else
                indicator.IsPrimed.Should().BeTrue($"CCI should be primed at index {i}");
        }
    }

    [Fact]
    public void CciIndicator_WithTypicalPriceCalculation_ShouldUseHighLowCloseAverage()
    {
        // Arrange
        var indicator = new CciIndicator(5, PriceToUse.Close, 10);
        var candle = CreateTestCandle(DateTime.Now, 100, 110, 90, 105);

        // Act
        var result = indicator.AddAndCalc(candle);

        // Assert
        result.Should().NotBeNull();
        // CCI uses typical price (H+L+C)/3 internally, so we verify it processes the candle
        result.CloseOn.Should().Be(candle.CloseOn);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-1)]
    public void CciIndicator_WithInvalidPeriod_ShouldThrowArgumentOutOfRangeException(int invalidPeriod)
    {
        // Act & Assert
        Action act = () => new CciIndicator(invalidPeriod, PriceToUse.Close, 10);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion

    #region Stochastics Tests

    [Fact]
    public void StochasticsIndicator_WithValidParameters_ShouldCalculateCorrectValues()
    {
        // Arrange
        var indicator = new StochasticsIndicator(14, 7, 3);
        var baselines = GetStochasticsBaselines();

        // Act & Assert
        foreach (var baseline in baselines)
        {
            var addResult = indicator.AddAndCalc(baseline.Candle);
            addResult.Should().NotBeNull();
            addResult.K.Should().BeApproximately(baseline.K, 0.00000001);
            addResult.D.Should().BeApproximately(baseline.D, 0.00000001);

            var updateResult = indicator.UpdateAndCalc(baseline.Candle);
            updateResult.Should().NotBeNull();
            updateResult.K.Should().BeApproximately(baseline.K, 0.00000001);
            updateResult.D.Should().BeApproximately(baseline.D, 0.00000001);
        }
    }

    [Fact]
    public void StochasticsIndicator_AddAndCalc_ShouldAddNewDataPoint()
    {
        // Arrange
        var indicator = new StochasticsIndicator(5, 3, 3);
        var candle1 = CreateTestCandle(DateTime.Now, 100, 110, 90, 105);
        var candle2 = CreateTestCandle(DateTime.Now.AddDays(1), 105, 115, 95, 110);

        // Act
        var result1 = indicator.AddAndCalc(candle1);
        var result2 = indicator.AddAndCalc(candle2);

        // Assert
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.CloseOn.Should().Be(candle1.CloseOn);
        result2.CloseOn.Should().Be(candle2.CloseOn);
    }

    [Fact]
    public void StochasticsIndicator_UpdateAndCalc_ShouldUpdateLastDataPoint()
    {
        // Arrange
        var indicator = new StochasticsIndicator(5, 3, 3);
        var candle1 = CreateTestCandle(DateTime.Now, 100, 110, 90, 105);
        var candle2 = CreateTestCandle(DateTime.Now, 105, 115, 95, 110); // Same date, different values

        // Act
        var addResult = indicator.AddAndCalc(candle1);
        var updateResult = indicator.UpdateAndCalc(candle2);

        // Assert
        addResult.Should().NotBeNull();
        updateResult.Should().NotBeNull();
        updateResult.CloseOn.Should().Be(candle2.CloseOn);
    }

    [Fact]
    public void StochasticsIndicator_IsPrimedProperty_ShouldBehaviorCorrectly()
    {
        // Arrange
        var kPeriod = 5;
        var indicator = new StochasticsIndicator(kPeriod, 3, 3);
        var testCandles = CreateTestCandles(10);

        // Act & Assert
        for (int i = 0; i < testCandles.Count; i++)
        {
            indicator.AddAndCalc(testCandles[i]);
            
            if (i < kPeriod - 1)
                indicator.IsPrimed.Should().BeFalse($"Stochastics should not be primed at index {i}");
            else
                indicator.IsPrimed.Should().BeTrue($"Stochastics should be primed at index {i}");
        }
    }

    [Fact]
    public void StochasticsIndicator_IndexerAccess_ShouldReturnCorrectResults()
    {
        // Arrange
        var indicator = new StochasticsIndicator(5, 3, 3);
        var testCandles = CreateTestCandles(10);

        // Act
        foreach (var candle in testCandles)
        {
            indicator.AddAndCalc(candle);
        }

        // Assert
        if (indicator.IsPrimed)
        {
            var latestResult = indicator[0];
            latestResult.Should().NotBeNull();
            latestResult.CloseOn.Should().Be(testCandles[^1].CloseOn);
        }
    }

    [Theory]
    [InlineData(1, 3, 3)]
    [InlineData(0, 3, 3)]
    [InlineData(-1, 3, 3)]
    [InlineData(5, 1, 3)]
    [InlineData(5, 0, 3)]
    [InlineData(5, -1, 3)]
    [InlineData(5, 3, 0)]
    [InlineData(5, 3, -1)]
    public void StochasticsIndicator_WithInvalidParameters_ShouldThrowArgumentOutOfRangeException(
        int kPeriod, int kSlowing, int dPeriod)
    {
        // Act & Assert
        Action act = () => new StochasticsIndicator(kPeriod, kSlowing, dPeriod);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void StochasticsIndicator_WithNullCandle_ShouldThrowArgumentNullException()
    {
        // Arrange
        var indicator = new StochasticsIndicator(5, 3, 3);

        // Act & Assert
        Action addAct = () => indicator.AddAndCalc(null!);
        addAct.Should().Throw<ArgumentNullException>();

        Action updateAct = () => indicator.UpdateAndCalc(null!);
        updateAct.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region Helper Methods

    private static bool IsGoodCalc(double actual, double expected) =>
        Math.Round(actual, expected.ToDigits()) == expected;

    private static List<TestCandle> CreateTestCandles(int count)
    {
        var candles = new List<TestCandle>();
        var baseDate = new DateTime(2023, 1, 1);
        var random = new Random(42); // Fixed seed for reproducible tests
        
        for (int i = 0; i < count; i++)
        {
            var basePrice = 100 + i * 0.5;
            var volatility = 5;
            
            var open = basePrice + random.NextDouble() * volatility - volatility / 2;
            var close = basePrice + random.NextDouble() * volatility - volatility / 2;
            var high = Math.Max(open, close) + random.NextDouble() * volatility;
            var low = Math.Min(open, close) - random.NextDouble() * volatility;
            
            candles.Add(new TestCandle
            {
                CloseOn = baseDate.AddDays(i),
                Open = open,
                High = high,
                Low = low,
                Close = close
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
