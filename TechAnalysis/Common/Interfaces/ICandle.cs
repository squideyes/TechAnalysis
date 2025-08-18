// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public interface ICandle
{
    DateTime CloseOn { get; }
    double Open { get; }
    double High { get; }
    double Low { get; }
    double Close { get; }
}