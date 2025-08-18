// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System;
using System.Collections.Generic;
using FluentAssertions;
using SquidEyes.TechAnalysis;
using Xunit;

namespace SquidEyes.UnitTests;

public class VolumeIndicatorTests
{
    [Fact]
    public void ObvIndicator_BasicBehavior_ShouldFollowPriceDirectionWithVolume()
    {
        var indicator = new ObvIndicator(period: 2, priceToUse: PriceToUse.Close, maxResults: 10);

        var candles = new List<TestCandle>
        {
            new() { CloseOn = new DateTime(2024,01,01), Open = 100, High = 101, Low = 99, Close = 100, Volume = 1000 },
            new() { CloseOn = new DateTime(2024,01,02), Open = 100, High = 102, Low = 100, Close = 101, Volume = 1500 },
            new() { CloseOn = new DateTime(2024,01,03), Open = 101, High = 103, Low = 100, Close = 100.5, Volume = 1200 },
            new() { CloseOn = new DateTime(2024,01,04), Open = 100.5, High = 101, Low = 98, Close = 99, Volume = 2000 },
            new() { CloseOn = new DateTime(2024,01,05), Open = 99, High = 100, Low = 95, Close = 99, Volume = 1800 }
        };

        var r0 = indicator.AddAndCalc(candles[0]);
        r0.Value.Should().Be(0); // initial baseline

        var r1 = indicator.AddAndCalc(candles[1]);
        r1.Value.Should().Be(1500);

        var r2 = indicator.AddAndCalc(candles[2]);
        r2.Value.Should().Be(1500 - 1200);

        var r3 = indicator.AddAndCalc(candles[3]);
        r3.Value.Should().Be(1500 - 1200 - 2000);

        var r4 = indicator.AddAndCalc(candles[4]);
        r4.Value.Should().Be(1500 - 1200 - 2000);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-1)]
    public void ObvIndicator_WithInvalidMaxResults_ShouldThrowArgumentOutOfRangeException(int invalidMaxResults)
    {
        Action act = () => new ObvIndicator(period: 2, priceToUse: PriceToUse.Close, maxResults: invalidMaxResults);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
