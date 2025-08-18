// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.TechAnalysis;
using System;

namespace SquidEyes.UnitTests;

public class TestCandle : ICandle
{
    public DateTime CloseOn { get; init; }
    public double Open { get; init; }
    public double High { get; init; }
    public double Low { get; init; }
    public double Close { get; init; }
    public double Volume { get; init; }
}