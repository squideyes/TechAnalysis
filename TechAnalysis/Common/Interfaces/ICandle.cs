using System;

namespace SquidEyes.TechAnalysis
{
    public interface ICandle
    {
        DateTime OpenOn { get; }
        float Open { get; }
        float High { get; }
        float Low { get; }
        float Close { get; }
    }
}