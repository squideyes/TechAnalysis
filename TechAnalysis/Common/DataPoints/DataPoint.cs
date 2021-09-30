using System;

namespace SquidEyes.TechAnalysis
{
    public class DataPoint
    {
        internal DataPoint(DateTime openOn, double value)
        {
            OpenOn = openOn;
            Value = value;
        }

        public DateTime OpenOn { get; }
        public double Value { get; }
    }
}
