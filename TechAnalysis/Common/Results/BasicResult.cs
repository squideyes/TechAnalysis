// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis;

public class BasicResult : ResultBase, IEquatable<BasicResult>
{
    internal BasicResult()
        : base(ResultKind.BasicResult)
    {
    }

    public double Value { get; init; }

    public bool Equals(BasicResult? other)
    {
        if (other is null)
            return false;

        return Value == other.Value
            && Kind == other.Kind
            && CloseOn == other.CloseOn;
    }

    public override bool Equals(object? other) =>
        other is BasicResult && Equals(other as BasicResult);

    public override int GetHashCode() =>
        HashCode.Combine(Value, Kind, CloseOn);
}