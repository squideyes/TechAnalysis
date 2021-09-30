using SquidEyes.TechAnalysis;
using System;
using System.Collections.Generic;

namespace SquidEyes.UnitTests
{
    internal static class TestData
    {
        public class StochasticsBaseLine
        {
            public ICandle Candle { get; init; }
            public double K { get; init; }
            public double D { get; init; }
        }

        public class ValueBaseline
        {
            public ICandle Candle { get; init; }
            public double Value { get; init; }
        }

        public class ThreeBandBaseline
        {
            public ICandle Candle { get; init; }
            public double Upper { get; init; }
            public double Middle { get; init; }
            public double Lower { get; init; }
        }

        public class MacdBaseline
        {
            public ICandle Candle { get; init; }
            public double Value { get; init; }
            public double Average { get; init; }
            public double Difference { get; init; }
        }

        public static List<StochasticsBaseLine> GetStochasticsBaselines()
        {
            var baselines = new List<StochasticsBaseLine>();

            foreach (var fields in new CsvEnumerator(
                Properties.Baselines.StochasticsBaseline.ToStream(), 7))
            {
                var baseLine = new StochasticsBaseLine()
                {
                    Candle = new TestCandle()
                    {
                        OpenOn = DateTime.Parse(fields[0]).AddMinutes(-1),
                        Open = float.Parse(fields[1]),
                        High = float.Parse(fields[2]),
                        Low = float.Parse(fields[3]),
                        Close = float.Parse(fields[4])
                    },
                    K = double.Parse(fields[5]),
                    D = double.Parse(fields[6])
                };

                baselines.Add(baseLine);
            }

            return baselines;
        }

        public static List<ValueBaseline> GetValueBaseline(string csv)
        {
            var baselines = new List<ValueBaseline>();

            foreach (var fields in new CsvEnumerator(csv.ToStream(), 8))
            {
                var hour = int.Parse(fields[0]);
                var minute = int.Parse(fields[1]);

                var openOn = new DateTime(2021, 3, 30, hour, minute, 0);
                var open = float.Parse(fields[2]);
                var high = float.Parse(fields[3]);
                var low = float.Parse(fields[4]);
                var close = float.Parse(fields[5]);

                baselines.Add(new ValueBaseline()
                {
                    Candle = new TestCandle
                    {
                        OpenOn = openOn,
                        Open = open,
                        High = high,
                        Low = low,
                        Close = close
                    },
                    Value = double.Parse(fields[7])
                });
            }

            return baselines;
        }

        public static List<MacdBaseline> GetMacdBaselines()
        {
            var baselines = new List<MacdBaseline>();

            foreach (var fields in new CsvEnumerator(
                Properties.Baselines.MacdBaseline.ToStream(), 10))
            {
                var hour = int.Parse(fields[0]);
                var minute = int.Parse(fields[1]);

                var openOn = new DateTime(2021, 3, 30, hour, minute, 0);
                var open = float.Parse(fields[2]);
                var high = float.Parse(fields[3]);
                var low = float.Parse(fields[4]);
                var close = float.Parse(fields[5]);

                baselines.Add(new MacdBaseline()
                {
                    Candle = new TestCandle
                    {
                        OpenOn = openOn,
                        Open = open,
                        High = high,
                        Low = low,
                        Close = close
                    },
                    Value = double.Parse(fields[7]),
                    Average = double.Parse(fields[8]),
                    Difference = double.Parse(fields[9])
                });
            }

            return baselines;
        }

        public static List<ThreeBandBaseline> GetThreeBandBaselines(string csv)
        {
            var baselines = new List<ThreeBandBaseline>();

            foreach (var fields in new CsvEnumerator(csv.ToStream(), 10))
            {
                var hour = int.Parse(fields[0]);
                var minute = int.Parse(fields[1]);

                var openOn = new DateTime(2021, 3, 30, hour, minute, 0);
                var open = float.Parse(fields[2]);
                var high = float.Parse(fields[3]);
                var low = float.Parse(fields[4]);
                var close = float.Parse(fields[5]);

                baselines.Add(new ThreeBandBaseline()
                {
                    Candle = new TestCandle
                    {
                        OpenOn = openOn,
                        Open = open,
                        High = high,
                        Low = low,
                        Close = close
                    },
                    Upper = double.Parse(fields[7]),
                    Middle = double.Parse(fields[8]),
                    Lower = double.Parse(fields[9])
                });
            }

            return baselines;
        }
    }
}
