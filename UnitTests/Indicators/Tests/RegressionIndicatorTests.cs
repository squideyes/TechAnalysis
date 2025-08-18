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
/// Comprehensive tests for Regression indicators including Linear Regression
/// </summary>
public class RegressionIndicatorTests
{
    #region Linear Regression Tests

    [Fact]
    public void LinRegIndicator_WithValidParameters_ShouldCalculateCorrectValues()
    {
        // Arrange
        var indicator = new LinRegIndicator(10, PriceToUse.Close, 2);
        var baselines = GetLinRegBaselines();

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
    public void LinRegIndicator_WithDifferentPriceTypes_ShouldCalculateCorrectly(PriceToUse priceToUse)
    {
        // Arrange
        var indicator = new LinRegIndicator(5, priceToUse, 10);
        var testCandles = CreateTestCandles(10);

        // Act & Assert
        foreach (var candle in testCandles)
        {
            var result = indicator.AddAndCalc(candle);
            result.Should().NotBeNull();
            result.CloseOn.Should().Be(candle.CloseOn);
        }
    }

    [Fact]
    public void LinRegIndicator_WithLinearTrendData_ShouldFollowTrend()
    {
        // Arrange
        var indicator = new LinRegIndicator(5, PriceToUse.Close, 10);
        var linearTrendCandles = CreateLinearTrendCandles(10, 100, 1); // Upward trend

        // Act
        var results = new List<BasicResult>();
        foreach (var candle in linearTrendCandles)
        {
            var result = indicator.AddAndCalc(candle);
            if (indicator.IsPrimed)
                results.Add(result);
        }

        // Assert
        results.Should().NotBeEmpty();
        
        // For a perfect linear trend, the linear regression should closely follow the trend
        for (int i = 1; i < results.Count; i++)
        {
            results[i].Value.Should().BeGreaterThan(results[i-1].Value - 0.1, 
                "Linear regression should follow upward trend");
        }
    }

    [Fact]
    public void LinRegIndicator_IsPrimedProperty_ShouldBehaviorCorrectly()
    {
        // Arrange
        var period = 5;
        var indicator = new LinRegIndicator(period, PriceToUse.Close, 10);
        var testCandles = CreateTestCandles(10);

        // Act & Assert
        for (int i = 0; i < testCandles.Count; i++)
        {
            indicator.AddAndCalc(testCandles[i]);
            
            if (i < period - 1)
                indicator.IsPrimed.Should().BeFalse($"LinReg should not be primed at index {i}");
            else
                indicator.IsPrimed.Should().BeTrue($"LinReg should be primed at index {i}");
        }
    }

    [Fact]
    public void LinRegIndicator_IndexerAccess_ShouldReturnCorrectResults()
    {
        // Arrange
        var indicator = new LinRegIndicator(5, PriceToUse.Close, 10);
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
            
            // Test accessing multiple indices
            if (indicator.Count > 1)
            {
                var previousResult = indicator[1];
                previousResult.Should().NotBeNull();
                previousResult.CloseOn.Should().Be(testCandles[^2].CloseOn);
            }
        }
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-1)]
    public void LinRegIndicator_WithInvalidPeriod_ShouldThrowArgumentOutOfRangeException(int invalidPeriod)
    {
        // Act & Assert
        Action act = () => new LinRegIndicator(invalidPeriod, PriceToUse.Close, 10);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-1)]
    public void LinRegIndicator_WithInvalidMaxResults_ShouldThrowArgumentOutOfRangeException(int invalidMaxResults)
    {
        // Act & Assert
        Action act = () => new LinRegIndicator(10, PriceToUse.Close, invalidMaxResults);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void LinRegIndicator_CountProperty_ShouldReflectNumberOfResults()
    {
        // Arrange
        var maxResults = 5;
        var indicator = new LinRegIndicator(3, PriceToUse.Close, maxResults);
        var testCandles = CreateTestCandles(10);

        // Act & Assert
        for (int i = 0; i < testCandles.Count; i++)
        {
            indicator.AddAndCalc(testCandles[i]);
            
            var expectedCount = Math.Min(i + 1, maxResults);
            indicator.Count.Should().Be(expectedCount, 
                $"Count should be {expectedCount} after adding {i + 1} candles");
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

    private static List<TestCandle> CreateLinearTrendCandles(int count, double startPrice, double increment)
    {
        var candles = new List<TestCandle>();
        var baseDate = new DateTime(2023, 1, 1);
        
        for (int i = 0; i < count; i++)
        {
            var price = startPrice + (i * increment);
            var volatility = 0.1; // Minimal volatility for clean trend
            
            candles.Add(new TestCandle
            {
                CloseOn = baseDate.AddDays(i),
                Open = price - volatility,
                High = price + volatility,
                Low = price - volatility,
                Close = price
            });
        }
        
        return candles;
    }

    #endregion
}
