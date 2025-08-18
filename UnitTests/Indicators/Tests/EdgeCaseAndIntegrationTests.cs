// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System;
using System.Collections.Generic;
using SquidEyes.TechAnalysis;
using Xunit;
using FluentAssertions;

namespace SquidEyes.UnitTests;

/// <summary>
/// Edge case tests and integration tests for all indicators
/// </summary>
public class EdgeCaseAndIntegrationTests
{
    #region Edge Case Tests

    [Theory]
    [InlineData(typeof(SmaIndicator))]
    [InlineData(typeof(EmaIndicator))]
    [InlineData(typeof(DemaIndicator))]
    [InlineData(typeof(TemaIndicator))]
    [InlineData(typeof(WmaIndicator))]
    [InlineData(typeof(SmmaIndicator))]
    [InlineData(typeof(CciIndicator))]
    [InlineData(typeof(AtrIndicator))]
    [InlineData(typeof(StdDevIndicator))]
    [InlineData(typeof(LinRegIndicator))]
    public void BasicIndicators_WithExtremeValues_ShouldHandleGracefully(Type indicatorType)
    {
        // Arrange
        var indicator = CreateBasicIndicator(indicatorType, 5, PriceToUse.Close, 10);
        var extremeCandles = CreateExtremePriceCandles();

        // Act & Assert
        foreach (var candle in extremeCandles)
        {
            Action act = () => indicator.AddAndCalc(candle);
            act.Should().NotThrow($"{indicatorType.Name} should handle extreme values gracefully");
            
            var result = indicator.AddAndCalc(candle);
            result.Should().NotBeNull();
            result.Value.Should().NotBe(double.NaN, "Result should not be NaN");
            result.Value.Should().NotBe(double.PositiveInfinity, "Result should not be positive infinity");
            result.Value.Should().NotBe(double.NegativeInfinity, "Result should not be negative infinity");
        }
    }

    [Theory]
    [InlineData(typeof(SmaIndicator))]
    [InlineData(typeof(EmaIndicator))]
    [InlineData(typeof(CciIndicator))]
    [InlineData(typeof(AtrIndicator))]
    public void BasicIndicators_WithIdenticalPrices_ShouldHandleCorrectly(Type indicatorType)
    {
        // Arrange
        var indicator = CreateBasicIndicator(indicatorType, 5, PriceToUse.Close, 10);
        var identicalPriceCandles = CreateIdenticalPriceCandles(10, 100.0);

        // Act & Assert
        foreach (var candle in identicalPriceCandles)
        {
            var result = indicator.AddAndCalc(candle);
            result.Should().NotBeNull();
            result.Value.Should().NotBe(double.NaN, "Result should not be NaN with identical prices");
        }
    }

    [Fact]
    public void StochasticsIndicator_WithIdenticalHighLowPrices_ShouldHandleDivisionByZero()
    {
        // Arrange
        var indicator = new StochasticsIndicator(5, 3, 3);
        var flatCandles = CreateFlatPriceCandles(10, 100.0);

        // Act & Assert
        foreach (var candle in flatCandles)
        {
            Action act = () => indicator.AddAndCalc(candle);
            act.Should().NotThrow("Stochastics should handle flat prices gracefully");
            
            var result = indicator.AddAndCalc(candle);
            result.Should().NotBeNull();
            result.K.Should().NotBe(double.NaN, "K value should not be NaN");
            result.D.Should().NotBe(double.NaN, "D value should not be NaN");
        }
    }

    [Fact]
    public void BollingerBandsIndicator_WithZeroStandardDeviation_ShouldHandleCorrectly()
    {
        // Arrange
        var indicator = new BollingerBandsIndicator(5, PriceToUse.Close, 10, 2.0);
        var constantPriceCandles = CreateIdenticalPriceCandles(10, 100.0);

        // Act & Assert
        foreach (var candle in constantPriceCandles)
        {
            var result = indicator.AddAndCalc(candle);
            result.Should().NotBeNull();
            
            if (indicator.IsPrimed)
            {
                // With zero standard deviation, all bands should converge to the same value
                result.Upper.Should().BeApproximately(result.Middle, 0.000001);
                result.Lower.Should().BeApproximately(result.Middle, 0.000001);
            }
        }
    }

    [Theory]
    [InlineData(double.MaxValue)]
    [InlineData(double.MinValue)]
    [InlineData(1e10)]
    [InlineData(-1e10)]
    public void Indicators_WithExtremeDoubleValues_ShouldNotOverflow(double extremeValue)
    {
        // Arrange
        var smaIndicator = new SmaIndicator(3, PriceToUse.Close, 5);
        var candle = new TestCandle
        {
            CloseOn = DateTime.Now,
            Open = extremeValue,
            High = extremeValue,
            Low = extremeValue,
            Close = extremeValue
        };

        // Act & Assert
        Action act = () => smaIndicator.AddAndCalc(candle);
        act.Should().NotThrow("Indicator should handle extreme double values");
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void MaFactory_ShouldCreateAllMovingAverageTypes()
    {
        // Arrange & Act & Assert
        var sma = MaFactory.GetMaIndicator(MaKind.Sma, 10, 5, PriceToUse.Close);
        sma.Should().BeOfType<SmaIndicator>();

        var ema = MaFactory.GetMaIndicator(MaKind.Ema, 10, 5, PriceToUse.Close);
        ema.Should().BeOfType<EmaIndicator>();

        var dema = MaFactory.GetMaIndicator(MaKind.Dema, 10, 5, PriceToUse.Close);
        dema.Should().BeOfType<DemaIndicator>();

        var tema = MaFactory.GetMaIndicator(MaKind.Tema, 10, 5, PriceToUse.Close);
        tema.Should().BeOfType<TemaIndicator>();

        var wma = MaFactory.GetMaIndicator(MaKind.Wma, 10, 5, PriceToUse.Close);
        wma.Should().BeOfType<WmaIndicator>();

        var smma = MaFactory.GetMaIndicator(MaKind.Smma, 10, 5, PriceToUse.Close);
        smma.Should().BeOfType<SmmaIndicator>();
    }

    [Fact]
    public void MaFactory_WithInvalidMaKind_ShouldThrowArgumentOutOfRangeException()
    {
        // Act & Assert
        Action act = () => MaFactory.GetMaIndicator((MaKind)999, 10, 5, PriceToUse.Close);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void BollingerBandsIndicator_ShouldUseCorrectUnderlyingIndicators()
    {
        // Arrange
        var bbIndicator = new BollingerBandsIndicator(10, PriceToUse.Close, 5, 2.0);
        var smaIndicator = new SmaIndicator(10, PriceToUse.Close, 5);
        var stdDevIndicator = new StdDevIndicator(10, PriceToUse.Close, 5);
        
        var testCandles = CreateTestCandles(15);

        // Act
        ChannelResult? bbResult = null;
        BasicResult? smaResult = null;
        BasicResult? stdDevResult = null;

        foreach (var candle in testCandles)
        {
            bbResult = bbIndicator.AddAndCalc(candle);
            smaResult = smaIndicator.AddAndCalc(candle);
            stdDevResult = stdDevIndicator.AddAndCalc(candle);
        }

        // Assert
        bbResult.Should().NotBeNull();
        smaResult.Should().NotBeNull();
        stdDevResult.Should().NotBeNull();

        // The middle band should equal the SMA
        bbResult!.Middle.Should().BeApproximately(smaResult!.Value, 0.000001,
            "Bollinger Bands middle line should equal SMA");

        // The band width should be related to standard deviation
        var bandWidth = (bbResult.Upper - bbResult.Lower) / 2;
        var expectedBandWidth = 2.0 * stdDevResult!.Value; // 2.0 is the stdDevFactor
        bandWidth.Should().BeApproximately(expectedBandWidth, 0.000001,
            "Bollinger Bands width should be 2 * standard deviation");
    }

    [Fact]
    public void MultipleIndicators_WithSameData_ShouldProduceConsistentResults()
    {
        // Arrange
        var sma1 = new SmaIndicator(10, PriceToUse.Close, 5);
        var sma2 = new SmaIndicator(10, PriceToUse.Close, 5);
        var testCandles = CreateTestCandles(15);

        // Act
        var results1 = new List<BasicResult>();
        var results2 = new List<BasicResult>();

        foreach (var candle in testCandles)
        {
            results1.Add(sma1.AddAndCalc(candle));
            results2.Add(sma2.AddAndCalc(candle));
        }

        // Assert
        results1.Should().HaveCount(results2.Count);
        for (int i = 0; i < results1.Count; i++)
        {
            results1[i].Value.Should().BeApproximately(results2[i].Value, 0.000001,
                $"Results should be identical at index {i}");
        }
    }

    [Fact]
    public void IndicatorChaining_EmaOfSma_ShouldWorkCorrectly()
    {
        // Arrange
        var smaIndicator = new SmaIndicator(5, PriceToUse.Close, 10);
        var emaIndicator = new EmaIndicator(3, PriceToUse.Close, 10);
        var testCandles = CreateTestCandles(15);

        // Act
        foreach (var candle in testCandles)
        {
            var smaResult = smaIndicator.AddAndCalc(candle);
            if (smaIndicator.IsPrimed)
            {
                var emaResult = emaIndicator.AddAndCalc(smaResult);
                emaResult.Should().NotBeNull();
            }
        }

        // Assert
        emaIndicator.IsPrimed.Should().BeTrue("EMA should be primed after processing SMA results");
    }

    #endregion

    #region Performance Tests

    [Trait("TestCategory","Performance")]
    [Fact]
    public void Indicators_WithLargeDataset_ShouldPerformReasonably()
    {
        // Arrange
        var largeDataset = CreateTestCandles(10000);
        var indicators = new IBasicIndicator[]
        {
            new SmaIndicator(20, PriceToUse.Close, 100),
            new EmaIndicator(20, PriceToUse.Close, 100),
            new CciIndicator(20, PriceToUse.Close, 100),
            new AtrIndicator(20, PriceToUse.Close, 100)
        };

        // Act & Assert
        var startTime = DateTime.UtcNow;
        
        foreach (var indicator in indicators)
        {
            foreach (var candle in largeDataset)
            {
                var result = indicator.AddAndCalc(candle);
                result.Should().NotBeNull();
            }
        }
        
        var elapsed = DateTime.UtcNow - startTime;
        elapsed.Should().BeLessThan(TimeSpan.FromSeconds(5), 
            "Processing 10,000 candles with 4 indicators should complete within 5 seconds");
    }

    #endregion

    #region Helper Methods

    private static IBasicIndicator CreateBasicIndicator(Type indicatorType, int period, PriceToUse priceToUse, int maxResults)
    {
        return indicatorType.Name switch
        {
            nameof(SmaIndicator) => new SmaIndicator(period, priceToUse, maxResults),
            nameof(EmaIndicator) => new EmaIndicator(period, priceToUse, maxResults),
            nameof(DemaIndicator) => new DemaIndicator(period, priceToUse, maxResults),
            nameof(TemaIndicator) => new TemaIndicator(period, priceToUse, maxResults),
            nameof(WmaIndicator) => new WmaIndicator(period, priceToUse, maxResults),
            nameof(SmmaIndicator) => new SmmaIndicator(period, priceToUse, maxResults),
            nameof(CciIndicator) => new CciIndicator(period, priceToUse, maxResults),
            nameof(AtrIndicator) => new AtrIndicator(period, priceToUse, maxResults),
            nameof(StdDevIndicator) => new StdDevIndicator(period, priceToUse, maxResults),
            nameof(LinRegIndicator) => new LinRegIndicator(period, priceToUse, maxResults),
            nameof(KamaIndicator) => new KamaIndicator(period, 5, 15, priceToUse, maxResults),
            _ => throw new ArgumentException($"Unknown indicator type: {indicatorType.Name}")
        };
    }

    private static List<TestCandle> CreateTestCandles(int count)
    {
        var candles = new List<TestCandle>();
        var baseDate = new DateTime(2023, 1, 1);
        var random = new Random(42);
        
        for (int i = 0; i < count; i++)
        {
            var basePrice = 100 + i * 0.1;
            var volatility = 2;
            
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

    private static List<TestCandle> CreateExtremePriceCandles()
    {
        var baseDate = new DateTime(2023, 1, 1);
        return new List<TestCandle>
        {
            new() { CloseOn = baseDate, Open = 1e6, High = 1e6, Low = 1e6, Close = 1e6 },
            new() { CloseOn = baseDate.AddDays(1), Open = 1e-6, High = 1e-6, Low = 1e-6, Close = 1e-6 },
            new() { CloseOn = baseDate.AddDays(2), Open = 0, High = 0, Low = 0, Close = 0 },
            new() { CloseOn = baseDate.AddDays(3), Open = 999999, High = 999999, Low = 999999, Close = 999999 }
        };
    }

    private static List<TestCandle> CreateIdenticalPriceCandles(int count, double price)
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

    private static List<TestCandle> CreateFlatPriceCandles(int count, double price)
    {
        var candles = new List<TestCandle>();
        var baseDate = new DateTime(2023, 1, 1);
        
        for (int i = 0; i < count; i++)
        {
            candles.Add(new TestCandle
            {
                CloseOn = baseDate.AddDays(i),
                Open = price,
                High = price, // Same high and low creates flat price
                Low = price,
                Close = price
            });
        }
        
        return candles;
    }

    #endregion
}
