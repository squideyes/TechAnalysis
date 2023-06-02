// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public interface IBasicIndicator
{
    BasicResult AddAndCalc(ICandle candle);
}