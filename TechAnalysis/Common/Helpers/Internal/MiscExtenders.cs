using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SquidEyes.TechAnalysis
{
    internal static class MiscExtenders
    {
        private const double DOUBLE_EPSILON = 0.00000001;

        public static bool Approximates(this double a, double b) =>
            Math.Abs(a - b) < DOUBLE_EPSILON;

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }

        public static Stream ToStream(this string value)
        {
            var stream = new MemoryStream();

            var writer = new StreamWriter(stream, Encoding.UTF8, -1, true);

            writer.Write(value);

            writer.Flush();

            stream.Position = 0;

            return stream;
        }

        public static bool IsEnumValue<T>(this T value) where T : struct, Enum =>
            Enum.IsDefined(value);

        public static T Validated<T>(
            this T value, string fieldName, Func<T, bool> isValid)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentNullException(nameof(fieldName));

            if (isValid(value))
                return value;
            else
                throw new ArgumentOutOfRangeException(fieldName);
        }

        public static bool InRange<T>(
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

        public static R Funcify<T, R>(this T value, Func<T, R> func) => func(value);
    }
}
