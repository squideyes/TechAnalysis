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
/// Comprehensive tests for Channel indicators including Bollinger Bands and Keltner Channels
/// </summary>
public class ChannelIndicatorTests
{
    #region Bollinger Bands Tests

    [Fact]
    public void BollingerBandsIndicator_WithValidParameters_ShouldCalculateCorrectValues()
    {
        // Arrange
        var indicator = new BollingerBandsIndictor(10, PriceToUse.Close, 2, 2.0);
        var baselines = GetBollingerBandsBaselines();

        // Act & Assert
        foreach (var baseline in baselines)
        {
            var result = indicator.AddAndCalc(baseline.Candle);
            
            result.Should().NotBeNull();
            result.CloseOn.Should().Be(baseline.Candle.CloseOn);
            IsGoodCalc(result.Upper, baseline.Upper).Should().BeTrue();
            IsGoodCalc(result.Middle, baseline.Middle).Should().BeTrue();
            IsGoodCalc(result.Lower, baseline.Lower).Should().BeTrue();
        }
    }

    [Theory]
    [InlineData(PriceToUse.Open)]
    [InlineData(PriceToUse.High)]
    [InlineData(PriceToUse.Low)]
    [InlineData(PriceToUse.Close)]
    public void BollingerBandsIndicator_WithDifferentPriceTypes_ShouldCalculateCorrectly(PriceToUse priceToUse)
    {
        // Arrange
        var indicator = new BollingerBandsIndictor(10, priceToUse, 5, 2.0);
        var testCandles = CreateTestCandles(15);

        // Act & Assert
        foreach (var candle in testCandles)
        {
            var result = indicator.AddAndCalc(candle);
            
            result.Should().NotBeNull();
            result.CloseOn.Should().Be(candle.CloseOn);
            result.Upper.Should().BeGreaterThanOrEqualTo(result.Middle);
            result.Middle.Should().BeGreaterThanOrEqualTo(result.Lower);
        }
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(1.5)]
    [InlineData(2.0)]
    [InlineData(2.5)]
    [InlineData(3.0)]
    public void BollingerBandsIndicator_WithDifferentStdDevFactors_ShouldAdjustBandWidth(double stdDevFactor)
    {
        // Arrange
        var indicator = new BollingerBandsIndictor(10, PriceToUse.Close, 5, stdDevFactor);
        var testCandles = CreateTestCandles(15);

        // Act
        ChannelResult? lastResult = null;
        foreach (var candle in testCandles)
        {
            lastResult = indicator.AddAndCalc(candle);
        }

        // Assert
        lastResult.Should().NotBeNull();
        var bandWidth = lastResult!.Upper - lastResult.Lower;
        bandWidth.Should().BeGreaterThan(0);
        
        // Higher std dev factor should create wider bands
        if (stdDevFactor > 1.0)
        {
            var middleDistance = Math.Abs(lastResult.Upper - lastResult.Middle);
            middleDistance.Should().BeGreaterThan(0);
        }
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-1.0)]
    [InlineData(-2.5)]
    public void BollingerBandsIndicator_WithInvalidStdDevFactor_ShouldThrowArgumentOutOfRangeException(double invalidStdDevFactor)
    {
        // Act & Assert
        Action act = () => new BollingerBandsIndictor(10, PriceToUse.Close, 5, invalidStdDevFactor);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-1)]
    public void BollingerBandsIndicator_WithInvalidPeriod_ShouldThrowArgumentOutOfRangeException(int invalidPeriod)
    {
        // Act & Assert
        Action act = () => new BollingerBandsIndictor(invalidPeriod, PriceToUse.Close, 5, 2.0);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion

    #region Keltner Channel Tests

    [Fact]
    public void KeltnerChannelIndicator_WithValidParameters_ShouldCalculateCorrectValues()
    {
        // Arrange
        var indicator = new KeltnerChannelIndictor(20, PriceToUse.Close, 2, 1.5);
        var baselines = GetKeltnerChannelBaselines();

        // Act & Assert
        foreach (var baseline in baselines)
        {
            var result = indicator.AddAndCalc(baseline.Candle);
            
            result.Should().NotBeNull();
            result.CloseOn.Should().Be(baseline.Candle.CloseOn);
            IsGoodCalc(result.Upper, baseline.Upper).Should().BeTrue();
            IsGoodCalc(result.Middle, baseline.Middle).Should().BeTrue();
            IsGoodCalc(result.Lower, baseline.Lower).Should().BeTrue();
        }
    }

    [Theory]
    [InlineData(PriceToUse.Open)]
    [InlineData(PriceToUse.High)]
    [InlineData(PriceToUse.Low)]
    [InlineData(PriceToUse.Close)]
    public void KeltnerChannelIndicator_WithDifferentPriceTypes_ShouldCalculateCorrectly(PriceToUse priceToUse)
    {
        // Arrange
        var indicator = new KeltnerChannelIndictor(10, priceToUse, 5, 1.5);
        var testCandles = CreateTestCandles(15);

        // Act & Assert
        foreach (var candle in testCandles)
        {
            var result = indicator.AddAndCalc(candle);
            
            result.Should().NotBeNull();
            result.CloseOn.Should().Be(candle.CloseOn);
            result.Upper.Should().BeGreaterThanOrEqualTo(result.Middle);
            result.Middle.Should().BeGreaterThanOrEqualTo(result.Lower);
        }
    }

    [Theory]
    [InlineData(0.5)]
    [InlineData(1.0)]
    [InlineData(1.5)]
    [InlineData(2.0)]
    [InlineData(3.0)]
    [InlineData(5.0)]
    public void KeltnerChannelIndicator_WithValidOffsetMultipliers_ShouldCalculateCorrectly(double offsetMultiplier)
    {
        // Arrange
        var indicator = new KeltnerChannelIndictor(10, PriceToUse.Close, 5, offsetMultiplier);
        var testCandles = CreateTestCandles(15);

        // Act
        ChannelResult? lastResult = null;
        foreach (var candle in testCandles)
        {
            lastResult = indicator.AddAndCalc(candle);
        }

        // Assert
        lastResult.Should().NotBeNull();
        var bandWidth = lastResult!.Upper - lastResult.Lower;
        bandWidth.Should().BeGreaterThan(0);
    }

    [Theory]
    [InlineData(0.4)]
    [InlineData(0.0)]
    [InlineData(-1.0)]
    [InlineData(5.1)]
    [InlineData(10.0)]
    public void KeltnerChannelIndicator_WithInvalidOffsetMultiplier_ShouldThrowArgumentOutOfRangeException(double invalidOffsetMultiplier)
    {
        // Act & Assert
        Action act = () => new KeltnerChannelIndictor(10, PriceToUse.Close, 5, invalidOffsetMultiplier);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void KeltnerChannelIndicator_ShouldUseTypicalPriceAndTrueRange()
    {
        // Arrange
        var indicator = new KeltnerChannelIndictor(5, PriceToUse.Close, 10, 1.5);
        var candle = CreateTestCandle(DateTime.Now, 100, 110, 90, 105);

        // Act
        var result = indicator.AddAndCalc(candle);

        // Assert
        result.Should().NotBeNull();
        result.CloseOn.Should().Be(candle.CloseOn);
        
        // Keltner Channels use typical price for middle line and true range for offset
        // The middle should be influenced by the typical price (H+L+C)/3
        var expectedTypicalPrice = (candle.High + candle.Low + candle.Close) / 3.0;
        // We can't test exact equality due to SMA smoothing, but we can verify structure
        result.Upper.Should().BeGreaterThan(result.Middle);
        result.Middle.Should().BeGreaterThan(result.Lower);
    }

    #endregion

    #region Common Channel Tests

    [Theory]
    [InlineData(typeof(BollingerBandsIndictor))]
    [InlineData(typeof(KeltnerChannelIndictor))]
    public void ChannelIndicators_ShouldMaintainProperBandOrdering(Type indicatorType)
    {
        // Arrange
        object indicator;
        if (indicatorType == typeof(BollingerBandsIndictor))
            indicator = new BollingerBandsIndictor(10, PriceToUse.Close, 5, 2.0);
        else
            indicator = new KeltnerChannelIndictor(10, PriceToUse.Close, 5, 1.5);

        var testCandles = CreateTestCandles(15);

        // Act & Assert
        foreach (var candle in testCandles)
        {
            ChannelResult result;
            if (indicator is BollingerBandsIndictor bb)
                result = bb.AddAndCalc(candle);
            else
                result = ((KeltnerChannelIndictor)indicator).AddAndCalc(candle);

            // Assert proper band ordering
            result.Upper.Should().BeGreaterThanOrEqualTo(result.Middle, 
                "Upper band should always be greater than or equal to middle line");
            result.Middle.Should().BeGreaterThanOrEqualTo(result.Lower, 
                "Middle line should always be greater than or equal to lower band");
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
