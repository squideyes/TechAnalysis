// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Runtime.CompilerServices;

namespace SquidEyes.TechAnalysis;

internal static class MiscExtenders
{
    private const double DOUBLE_EPSILON = 0.00000001;

    public static bool Approximates(this double a, double b) =>
        Math.Abs(a - b) < DOUBLE_EPSILON;

    public static int ToDigits(this double value)
    {
        var digits = 0;

        while (Math.Round(value, digits) != value)
            digits++;

        return digits;
    }

    public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
    {
        foreach (var item in items)
            action(item);
    }

    public static bool IsEnumValue<T>(this T value)
        where T : struct, Enum => Enum.IsDefined(value);

    public static T Validated<T>(this T value, 
        Func<T, bool> isValid, [CallerMemberName] string fieldName = "")
    {
        if (string.IsNullOrWhiteSpace(fieldName))
            throw new ArgumentNullException(nameof(fieldName));

        if (isValid(value))
            return value;
        else
            throw new ArgumentOutOfRangeException(fieldName);
    }

    public static bool IsBetween<T>(
        this T value, T minValue, T maxValue, bool inclusive = true)
        where T : IComparable<T>
    {
        if (inclusive)
        {
            if (value.CompareTo(minValue) < 0 || value.CompareTo(maxValue) > 0)
                return false;
        }
        else
        {
            if (value.CompareTo(minValue) <= 0 || value.CompareTo(maxValue) >= 0)
                return false;
        }

        return true;
    }

    public static R Convert<T, R>(this T value, Func<T, R> func) => func(value);
}