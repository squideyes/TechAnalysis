// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public class ChannelResult : ResultBase
{
    public ChannelResult()
        : base(ResultKind.ChannelResult)
    {
    }

    public double Upper { get; init; }
    public double Middle { get; init; }
    public double Lower { get; init; }

    public override string ToString() =>
        $"{CloseOn:MM/dd/yyyy HH:mm},{Upper},{Middle},{Lower}";
}