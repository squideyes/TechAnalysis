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
/// Comprehensive tests for Statistics indicators including ATR, MACD, and Standard Deviation
/// </summary>
public class StatisticsIndicatorTests
{
    #region ATR (Average True Range) Tests

    [Fact]
    public void AtrIndicator_WithValidParameters_ShouldCalculateCorrectValues()
    {
        // Arrange
        var indicator = new AtrIndicator(10, PriceToUse.Close, 2);
        var baselines = GetAtrBaselines();

        // Act & Assert
        foreach (var baseline in baselines)
        {
            var result = indicator.AddAndCalc(baseline.Candle);
            result.Should().NotBeNull();
            IsGoodCalc(result.Value, baseline.Value).Should().BeTrue();
        }
    }

    [Fact]
    public void AtrIndicator_ShouldAlwaysReturnPositiveValues()
    {
        // Arrange
        var indicator = new AtrIndicator(5, PriceToUse.Close, 10);
        var testCandles = CreateTestCandles(10);

        // Act & Assert
        foreach (var candle in testCandles)
        {
            var result = indicator.AddAndCalc(candle);
            result.Should().NotBeNull();
            result.Value.Should().BeGreaterThanOrEqualTo(0, "ATR should always be non-negative");
        }
    }

    [Fact]
    public void AtrIndicator_WithHighVolatilityCandles_ShouldReturnHigherValues()
    {
        // Arrange
        var lowVolatilityIndicator = new AtrIndicator(5, PriceToUse.Close, 10);
        var highVolatilityIndicator = new AtrIndicator(5, PriceToUse.Close, 10);
        
        var lowVolatilityCandles = CreateLowVolatilityCandles(10);
        var highVolatilityCandles = CreateHighVolatilityCandles(10);

        // Act
        double lowVolatilityAtr = 0;
        foreach (var candle in lowVolatilityCandles)
        {
            var result = lowVolatilityIndicator.AddAndCalc(candle);
            if (lowVolatilityIndicator.IsPrimed)
                lowVolatilityAtr = result.Value;
        }

        double highVolatilityAtr = 0;
        foreach (var candle in highVolatilityCandles)
        {
            var result = highVolatilityIndicator.AddAndCalc(candle);
            if (highVolatilityIndicator.IsPrimed)
                highVolatilityAtr = result.Value;
        }

        // Assert
        highVolatilityAtr.Should().BeGreaterThan(lowVolatilityAtr, 
            "High volatility candles should produce higher ATR values");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-1)]
    public void AtrIndicator_WithInvalidPeriod_ShouldThrowArgumentOutOfRangeException(int invalidPeriod)
    {
        // Act & Assert
        Action act = () => new AtrIndicator(invalidPeriod, PriceToUse.Close, 10);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion

    #region MACD (Moving Average Convergence Divergence) Tests

    [Fact]
    public void MacdIndicator_WithValidParameters_ShouldCalculateCorrectValues()
    {
        // Arrange
        var indicator = new MacdIndicator(10, 15, 3, PriceToUse.Close, 10);
        var baselines = GetMacdBaselines();

        // Act & Assert
        foreach (var baseline in baselines)
        {
            var result = indicator.AddAndCalc(baseline.Candle);
            
            result.Should().NotBeNull();
            result.CloseOn.Should().Be(baseline.Candle.CloseOn);
            IsGoodCalc(result.Average, baseline.Average).Should().BeTrue();
            IsGoodCalc(result.Value, baseline.Value).Should().BeTrue();
            IsGoodCalc(result.Difference, baseline.Difference).Should().BeTrue();
        }
    }

    [Theory]
    [InlineData(PriceToUse.Open)]
    [InlineData(PriceToUse.High)]
    [InlineData(PriceToUse.Low)]
    [InlineData(PriceToUse.Close)]
    public void MacdIndicator_WithDifferentPriceTypes_ShouldCalculateCorrectly(PriceToUse priceToUse)
    {
        // Arrange
        var indicator = new MacdIndicator(5, 10, 3, priceToUse, 10);
        var testCandles = CreateTestCandles(15);

        // Act & Assert
        foreach (var candle in testCandles)
        {
            var result = indicator.AddAndCalc(candle);
            result.Should().NotBeNull();
            result.CloseOn.Should().Be(candle.CloseOn);
        }
    }

    [Fact]
    public void MacdIndicator_DifferenceShouldEqualValueMinusAverage()
    {
        // Arrange
        var indicator = new MacdIndicator(5, 10, 3, PriceToUse.Close, 10);
        var testCandles = CreateTestCandles(15);

        // Act & Assert
        foreach (var candle in testCandles)
        {
            var result = indicator.AddAndCalc(candle);
            
            // The difference should equal MACD value minus signal line (average)
            var expectedDifference = result.Value - result.Average;
            result.Difference.Should().BeApproximately(expectedDifference, 0.000001,
                "MACD Difference should equal Value minus Average");
        }
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-1)]
    public void MacdIndicator_WithInvalidMaxResults_ShouldThrowArgumentOutOfRangeException(int invalidMaxResults)
    {
        // Act & Assert
        Action act = () => new MacdIndicator(5, 10, 3, PriceToUse.Close, invalidMaxResults);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion

    #region Standard Deviation Tests

    [Fact]
    public void StdDevIndicator_WithValidParameters_ShouldCalculateCorrectValues()
    {
        // Arrange
        var indicator = new StdDevIndicator(10, PriceToUse.Close, 2);
        var baselines = GetStdDevBaselines();

        // Act & Assert
        foreach (var baseline in baselines)
        {
            var result = indicator.AddAndCalc(baseline.Candle);
            result.Should().NotBeNull();
            IsGoodCalc(result.Value, baseline.Value).Should().BeTrue();
        }
    }

    [Fact]
    public void StdDevIndicator_ShouldAlwaysReturnNonNegativeValues()
    {
        // Arrange
        var indicator = new StdDevIndicator(5, PriceToUse.Close, 10);
        var testCandles = CreateTestCandles(10);

        // Act & Assert
        foreach (var candle in testCandles)
        {
            var result = indicator.AddAndCalc(candle);
            result.Should().NotBeNull();
            result.Value.Should().BeGreaterThanOrEqualTo(0, 
                "Standard deviation should always be non-negative");
        }
    }

    [Fact]
    public void StdDevIndicator_WithConstantPrices_ShouldReturnZero()
    {
        // Arrange
        var indicator = new StdDevIndicator(5, PriceToUse.Close, 10);
        var constantPriceCandles = CreateConstantPriceCandles(10, 100);

        // Act
        BasicResult? lastResult = null;
        foreach (var candle in constantPriceCandles)
        {
            lastResult = indicator.AddAndCalc(candle);
        }

        // Assert
        lastResult.Should().NotBeNull();
        if (indicator.IsPrimed)
        {
            lastResult!.Value.Should().BeApproximately(0, 0.000001,
                "Standard deviation of constant prices should be zero");
        }
    }

    [Fact]
    public void StdDevIndicator_WithHighVariabilityPrices_ShouldReturnHigherValues()
    {
        // Arrange
        var lowVariabilityIndicator = new StdDevIndicator(5, PriceToUse.Close, 10);
        var highVariabilityIndicator = new StdDevIndicator(5, PriceToUse.Close, 10);
        
        var lowVariabilityCandles = CreateLowVolatilityCandles(10);
        var highVariabilityCandles = CreateHighVolatilityCandles(10);

        // Act
        double lowVariabilityStdDev = 0;
        foreach (var candle in lowVariabilityCandles)
        {
            var result = lowVariabilityIndicator.AddAndCalc(candle);
            if (lowVariabilityIndicator.IsPrimed)
                lowVariabilityStdDev = result.Value;
        }

        double highVariabilityStdDev = 0;
        foreach (var candle in highVariabilityCandles)
        {
            var result = highVariabilityIndicator.AddAndCalc(candle);
            if (highVariabilityIndicator.IsPrimed)
                highVariabilityStdDev = result.Value;
        }

        // Assert
        highVariabilityStdDev.Should().BeGreaterThan(lowVariabilityStdDev,
            "High variability prices should produce higher standard deviation");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-1)]
    public void StdDevIndicator_WithInvalidPeriod_ShouldThrowArgumentOutOfRangeException(int invalidPeriod)
    {
        // Act & Assert
        Action act = () => new StdDevIndicator(invalidPeriod, PriceToUse.Close, 10);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion

    #region Helper Methods

    private static bool IsGoodCalc(double actual, double expected) =>
        Math.Round(actual, expected.ToDigits()) == expected;

    private static List<TestCandle> CreateTestCandles(int count)
    {
        var candles = new List<TestCandle>();
        var baseDate = new DateTime(2023, 1, 1);
        var random = new Random(42);
        
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

    private static List<TestCandle> CreateLowVolatilityCandles(int count)
    {
        var candles = new List<TestCandle>();
        var baseDate = new DateTime(2023, 1, 1);
        
        for (int i = 0; i < count; i++)
        {
            var basePrice = 100;
            var volatility = 0.5; // Low volatility
            
            candles.Add(new TestCandle
            {
                CloseOn = baseDate.AddDays(i),
                Open = basePrice,
                High = basePrice + volatility,
                Low = basePrice - volatility,
                Close = basePrice + (i % 2 == 0 ? volatility / 2 : -volatility / 2)
            });
        }
        
        return candles;
    }

    private static List<TestCandle> CreateHighVolatilityCandles(int count)
    {
        var candles = new List<TestCandle>();
        var baseDate = new DateTime(2023, 1, 1);
        
        for (int i = 0; i < count; i++)
        {
            var basePrice = 100;
            var volatility = 10; // High volatility
            
            candles.Add(new TestCandle
            {
                CloseOn = baseDate.AddDays(i),
                Open = basePrice,
                High = basePrice + volatility,
                Low = basePrice - volatility,
                Close = basePrice + (i % 2 == 0 ? volatility : -volatility)
            });
        }
        
        return candles;
    }

    private static List<TestCandle> CreateConstantPriceCandles(int count, double price)
    {
        var candles = new List<TestCandle>();
        var baseDate = new DateTime(2023, 1, 1);
        
        for (int i = 0; i < count; i++)
        {
            candles.Add(new TestCandle
            {
                CloseOn = baseDate.AddDays(i),
                Open = price,
                High = price,
                Low = price,
                Close = price
            });
        }
        
        return candles;
    }

    #endregion
}
