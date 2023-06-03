// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public interface ICandle
{
    DateTime CloseOn { get; }
    float Open { get; }
    float High { get; }
    float Low { get; }
    float Close { get; }
}