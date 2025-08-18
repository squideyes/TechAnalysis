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

public class VwmaIndicatorTests
{
    [Fact]
    public void VwmaIndicator_WithValidParameters_ShouldCalculateCorrectly()
    {
        var indicator = new VwmaIndicator(3, PriceToUse.Close, 10);

        var candles = new List<TestCandle>
        {
            new() { CloseOn = new DateTime(2024,01,01), Open = 10, High = 11, Low = 9, Close = 10, Volume = 100 },
            new() { CloseOn = new DateTime(2024,01,02), Open = 11, High = 12, Low = 10, Close = 11, Volume = 200 },
            new() { CloseOn = new DateTime(2024,01,03), Open = 12, High = 13, Low = 11, Close = 12, Volume = 300 },
            new() { CloseOn = new DateTime(2024,01,04), Open = 13, High = 14, Low = 12, Close = 13, Volume = 400 },
        };

        var r0 = indicator.AddAndCalc(candles[0]); // (10*100)/(100) = 10
        r0.Value.Should().Be(10);

        var r1 = indicator.AddAndCalc(candles[1]); // (10*100+11*200)/(100+200) = (100+220)/300 = 1.0666? -> 320/300 = 1.0666? wrong
        Math.Round(r1.Value, 6).Should().Be(Math.Round((10*100.0 + 11*200.0) / (100.0 + 200.0), 6));

        var r2 = indicator.AddAndCalc(candles[2]); // (10*100+11*200+12*300)/(100+200+300)
        Math.Round(r2.Value, 6).Should().Be(Math.Round((10*100.0 + 11*200.0 + 12*300.0) / (100.0 + 200.0 + 300.0), 6));

        var r3 = indicator.AddAndCalc(candles[3]); // last 3: (11*200+12*300+13*400)/(200+300+400)
        Math.Round(r3.Value, 6).Should().Be(Math.Round((11*200.0 + 12*300.0 + 13*400.0) / (200.0 + 300.0 + 400.0), 6));
    }
}
