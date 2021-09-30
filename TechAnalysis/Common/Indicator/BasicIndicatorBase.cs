﻿namespace SquidEyes.TechAnalysis
{
    public abstract class BasicIndicatorBase
    {
        public BasicIndicatorBase(int period, PriceToUse priceToUse, int minPeriod)
        {
            Period = period.Validated(
                nameof(period), v => v >= minPeriod);

            PriceToUse = priceToUse.Validated(
                nameof(priceToUse), v => v.IsEnumValue());
        }

        protected int Period { get; }
        protected PriceToUse PriceToUse { get; }
    }
}