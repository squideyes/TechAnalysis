﻿using System;

namespace SquidEyes.TechAnalysis
{
    public class BollingerBandsIndictor : BasicIndicatorBase
    {
        private readonly SmaIndicator smaIndicator;
        private readonly StdDevIndicator stdDevIndicator;
        private readonly double stdDevFactor;

        public BollingerBandsIndictor(int period, PriceToUse priceToUse, double stdDevFactor)
            : base(period, priceToUse, 2)
        {
            if (stdDevFactor <= 0.0)
                throw new ArgumentOutOfRangeException(nameof(stdDevFactor));

            this.stdDevFactor = stdDevFactor;

            smaIndicator = new SmaIndicator(period, priceToUse);

            stdDevIndicator = new StdDevIndicator(period, priceToUse);
        }

        public ChannelResult AddAndCalc(ICandle candle)
        {
            var smaValue = smaIndicator.AddAndCalc(candle).Value;

            var stdDevValue = stdDevIndicator.AddAndCalc(candle).Value;

            var delta = stdDevFactor * stdDevValue;

            return new ChannelResult(candle.OpenOn, smaValue + delta, smaValue, smaValue - delta);
        }
    }
}