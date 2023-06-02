// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System;

namespace SquidEyes.TechAnalysis
{
    public abstract class BasicIndicatorBase 
    {
        public BasicIndicatorBase(int period, PriceToUse priceToUse, int minPeriod)
        {
            Period = period.Validated(nameof(period), v => v >= minPeriod);

            PriceToUse = priceToUse.Validated(nameof(priceToUse), v => v.IsEnumValue());
        }

        protected int Period { get; }
        protected PriceToUse PriceToUse { get; }

        static protected BasicResult GetBasicResult(DateTime openOn, double value) => new()
        {
            OpenOn = openOn,
            Value = value
        };
    }
}