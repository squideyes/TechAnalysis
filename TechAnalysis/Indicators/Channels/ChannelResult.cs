using System;

namespace SquidEyes.TechAnalysis
{
    public class ChannelResult
    {
        public ChannelResult(DateTime openOn, double upper, double middle, double lower)
        {
            OpenOn = openOn;
            Upper = upper;
            Middle = middle;
            Lower = lower;
        }

        public DateTime OpenOn { get; }
        public double Upper { get; }
        public double Middle { get; }
        public double Lower { get; }

        public override string ToString() =>
            $"{OpenOn:MM/dd/yyyy HH:mm},{Upper},{Middle},{Lower}";
    }
}
