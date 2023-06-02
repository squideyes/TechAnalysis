// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.TechAnalysis;
using System;

namespace SquidEyes.UnitTests
{
    public class TestCandle : ICandle
    {
        public DateTime OpenOn { get; init; }
        public float Open { get; init; }
        public float High { get; init; }
        public float Low { get; init; }
        public float Close { get; init; }
    }
}