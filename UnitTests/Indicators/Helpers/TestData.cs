// Copyright © 2021 by Louis S. Berman
// 
// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the “Software”),
// to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software
// is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
// IN THE SOFTWARE.

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

        private static ValueBaseline GetValueBaseline(int hour, int minute,
            float open, float high, float low, float close, int _, double value) => new()
            {
                Candle = new TestCandle()
                {
                    OpenOn = new DateTime(2021, 3, 30, hour, minute, 0),
                    Open = open,
                    High = high,
                    Low = low,
                    Close = close
                },
                Value = value
            };

        internal static List<ValueBaseline> GetAtrBaselines()
        {
            var baselines = new List<ValueBaseline>();

            void Add(int hour, int minute, float open, float high,
                float low, float close, int volume, double value)
            {
                baselines.Add(GetValueBaseline(
                    hour, minute, open, high, low, close, volume, value));
            }

            Add(14, 36, 12872.75f, 12873.75f, 12866, 12870.75f, 1068, 7.75);
            Add(14, 37, 12871.25f, 12871.5f, 12864.75f, 12869.5f, 427, 7.25);
            Add(14, 38, 12869.25f, 12871.75f, 12867.5f, 12871.25f, 414, 6.25);
            Add(14, 39, 12871, 12873.75f, 12869.5f, 12872.75f, 333, 5.75);
            Add(14, 40, 12872.75f, 12873.75f, 12867.75f, 12870.5f, 378, 5.8);
            Add(14, 41, 12870.25f, 12872.25f, 12864.5f, 12865.75f, 601, 6.125);
            Add(14, 42, 12866, 12870.75f, 12866, 12870.25f, 374, 5.96428571);
            Add(14, 43, 12869.5f, 12883, 12869.5f, 12877.25f, 1369, 6.90625);
            Add(14, 44, 12877.75f, 12878.25f, 12866.25f, 12868.75f, 1047, 7.47222222);
            Add(14, 45, 12869.25f, 12870.75f, 12861, 12867.5f, 881, 7.7);
            Add(14, 46, 12867.25f, 12872, 12865.5f, 12866, 533, 7.58);
            Add(14, 47, 12866, 12870.5f, 12863.25f, 12868.25f, 446, 7.547);
            Add(14, 48, 12868.5f, 12873.75f, 12867, 12873, 407, 7.4673);
            Add(14, 49, 12873, 12873.25f, 12866.5f, 12868.5f, 368, 7.39557);
            Add(14, 50, 12868.25f, 12872.5f, 12866.5f, 12872, 363, 7.256013);
            Add(14, 51, 12871.75f, 12877, 12871.25f, 12874, 450, 7.1054117);
            Add(14, 52, 12874.5f, 12875.25f, 12866.5f, 12868.75f, 879, 7.26987053);
            Add(14, 53, 12868.5f, 12871, 12865, 12870.5f, 452, 7.14288348);
            Add(14, 54, 12870.25f, 12881.5f, 12869.5f, 12878, 812, 7.62859513);
            Add(14, 55, 12878.75f, 12881, 12873, 12874.5f, 463, 7.66573562);
            Add(14, 56, 12874.75f, 12880.5f, 12866.25f, 12870, 1317, 8.32416205);
            Add(14, 57, 12869.5f, 12877.5f, 12869, 12876, 580, 8.34174585);
            Add(14, 58, 12876, 12878.25f, 12874.5f, 12877.75f, 357, 7.88257126);
            Add(14, 59, 12878, 12896.25f, 12877, 12895, 2297, 9.01931414);
            Add(15, 00, 12894.75f, 12899, 12888.5f, 12893.75f, 2107, 9.16738272);
            Add(15, 01, 12893.75f, 12895, 12887.75f, 12887.75f, 762, 8.97564445);
            Add(15, 02, 12888, 12891.25f, 12883.25f, 12886, 1034, 8.87808001);
            Add(15, 03, 12886.25f, 12886.5f, 12881.25f, 12883.75f, 518, 8.51527201);
            Add(15, 04, 12884, 12893.5f, 12884, 12892.25f, 560, 8.63874481);
            Add(15, 05, 12892.5f, 12900.5f, 12888.5f, 12896.75f, 1139, 8.97487032);
            Add(15, 06, 12897, 12897.25f, 12892.25f, 12895, 485, 8.57738329);
            Add(15, 07, 12894.5f, 12897.5f, 12892.5f, 12897.5f, 353, 8.21964496);
            Add(15, 08, 12897.5f, 12898.5f, 12895, 12897.5f, 387, 7.74768047);
            Add(15, 09, 12898, 12902.25f, 12896.5f, 12898.25f, 550, 7.54791242);
            Add(15, 10, 12898, 12900.75f, 12893.5f, 12893.5f, 458, 7.51812118);
            Add(15, 11, 12893.75f, 12897, 12890.75f, 12893.75f, 631, 7.39130906);
            Add(15, 12, 12894, 12896.75f, 12889.25f, 12893.25f, 667, 7.40217815);
            Add(15, 13, 12893.5f, 12897, 12889, 12889, 684, 7.46196034);
            Add(15, 14, 12889, 12895.25f, 12888.5f, 12890.25f, 574, 7.3907643);
            Add(15, 15, 12890.25f, 12897.75f, 12890, 12896.5f, 448, 7.42668787);
            Add(15, 16, 12896.75f, 12905, 12896.75f, 12903.25f, 898, 7.53401909);
            Add(15, 17, 12903, 12906, 12899.5f, 12901.25f, 549, 7.43061718);
            Add(15, 18, 12901.25f, 12904.5f, 12899.75f, 12901.5f, 479, 7.16255546);
            Add(15, 19, 12901, 12902.5f, 12891.75f, 12895.25f, 1203, 7.52129991);
            Add(15, 20, 12895.25f, 12899.75f, 12895.25f, 12898, 422, 7.21916992);
            Add(15, 21, 12897.75f, 12904.75f, 12897.25f, 12899.5f, 509, 7.24725293);
            Add(15, 22, 12899.75f, 12902.75f, 12893, 12894, 1094, 7.49752764);
            Add(15, 23, 12893.5f, 12896.5f, 12891.5f, 12894.25f, 388, 7.24777487);
            Add(15, 24, 12893.75f, 12902.25f, 12893.25f, 12901.25f, 472, 7.42299739);
            Add(15, 25, 12901, 12906.25f, 12899, 12905.5f, 655, 7.40569765);
            Add(15, 26, 12905.25f, 12907.5f, 12897.25f, 12898, 817, 7.69012788);
            Add(15, 27, 12897.75f, 12901.25f, 12889, 12889.25f, 580, 8.14611509);
            Add(15, 28, 12889.5f, 12896.75f, 12889.5f, 12895, 503, 8.08150359);
            Add(15, 29, 12895, 12899, 12892.75f, 12895.75f, 436, 7.89835323);
            Add(15, 30, 12896.5f, 12898.5f, 12894.75f, 12896.5f, 235, 7.4835179);
            Add(15, 31, 12896.75f, 12903.25f, 12896.75f, 12901.5f, 569, 7.41016611);
            Add(15, 32, 12902, 12905, 12900.25f, 12900.5f, 427, 7.1441495);
            Add(15, 33, 12900.5f, 12909, 12898.75f, 12908.25f, 752, 7.45473455);
            Add(15, 34, 12908.75f, 12909.5f, 12902.5f, 12903.75f, 1357, 7.4092611);
            Add(15, 35, 12903.5f, 12904.75f, 12898.25f, 12901.5f, 851, 7.31833499);
            Add(15, 36, 12901.25f, 12911, 12900.75f, 12909.75f, 857, 7.61150149);
            Add(15, 37, 12909.25f, 12914.5f, 12902.75f, 12904.25f, 991, 8.02535134);
            Add(15, 38, 12903.75f, 12905.25f, 12897.5f, 12899.25f, 907, 7.99781621);
            Add(15, 39, 12899, 12901, 12891, 12898, 1387, 8.19803459);
            Add(15, 40, 12898, 12905, 12892.5f, 12897, 1581, 8.62823113);
            Add(15, 41, 12897.5f, 12901.75f, 12895.25f, 12901.25f, 562, 8.41540801);
            Add(15, 42, 12901.5f, 12901.5f, 12894.5f, 12900.25f, 495, 8.27386721);
            Add(15, 43, 12900.5f, 12905.75f, 12899, 12905, 582, 8.12148049);
            Add(15, 44, 12905.25f, 12905.25f, 12893.5f, 12893.5f, 730, 8.48433244);
            Add(15, 45, 12893.5f, 12894.75f, 12887, 12890.75f, 1016, 8.4108992);
            Add(15, 46, 12890.75f, 12895.75f, 12889.75f, 12892.75f, 499, 8.16980928);
            Add(15, 47, 12893.5f, 12895.75f, 12889.75f, 12895, 556, 7.95282835);
            Add(15, 48, 12894.25f, 12900.25f, 12891, 12896.5f, 589, 8.08254552);
            Add(15, 49, 12896.25f, 12896.75f, 12890.25f, 12891.75f, 509, 7.92429096);
            Add(15, 50, 12892, 12895.75f, 12884.25f, 12886.25f, 745, 8.28186187);
            Add(15, 51, 12886.25f, 12886.5f, 12862.5f, 12863, 3268, 9.85367568);
            Add(15, 52, 12863, 12873, 12860.75f, 12871.5f, 1473, 10.09330811);
            Add(15, 53, 12871.5f, 12874.5f, 12869.5f, 12873, 678, 9.5839773);
            Add(15, 54, 12873, 12877.5f, 12873, 12874.25f, 857, 9.07557957);
            Add(15, 55, 12874, 12875.75f, 12852, 12856.5f, 2807, 10.54302161);
            Add(15, 56, 12856.25f, 12859, 12840.25f, 12842.25f, 2622, 11.36371945);
            Add(15, 57, 12842.5f, 12848.5f, 12839.75f, 12847.25f, 1550, 11.10234751);
            Add(15, 58, 12847.25f, 12851.25f, 12843.75f, 12844.5f, 1105, 10.74211276);
            Add(15, 59, 12844, 12854, 12841.5f, 12853.25f, 1632, 10.91790148);
            Add(16, 00, 12853.5f, 12900.5f, 12852.25f, 12899, 5587, 14.65111133);
            Add(16, 01, 12898.75f, 12901.5f, 12884.5f, 12887.5f, 2869, 14.8860002);
            Add(16, 02, 12887.25f, 12897, 12886.75f, 12889.25f, 767, 14.42240018);
            Add(16, 03, 12889, 12894.25f, 12882.75f, 12882.75f, 594, 14.13016016);
            Add(16, 04, 12883.25f, 12889, 12882.25f, 12885.25f, 460, 13.39214415);
            Add(16, 05, 12885.75f, 12889, 12885.25f, 12887.5f, 281, 12.42792973);
            Add(16, 06, 12887, 12888.75f, 12886.25f, 12887.75f, 312, 11.43513676);
            Add(16, 07, 12887.25f, 12887.75f, 12883.5f, 12884.5f, 201, 10.71662308);
            Add(16, 08, 12884.25f, 12884.5f, 12881, 12882.5f, 192, 9.99496077);
            Add(16, 09, 12882.25f, 12886.75f, 12881, 12884.25f, 353, 9.5704647);
            Add(16, 10, 12884.25f, 12885.5f, 12884, 12885, 106, 8.76341823);
            Add(16, 11, 12885, 12886, 12883, 12885.5f, 121, 8.1870764);
            Add(16, 12, 12886, 12887.75f, 12885.25f, 12887.75f, 98, 7.61836876);
            Add(16, 13, 12887.75f, 12888.75f, 12886.5f, 12888.5f, 107, 7.08153189);
            Add(16, 14, 12888, 12895.75f, 12888, 12895.25f, 368, 7.1483787);

            return baselines;
        }
    }
}
