namespace SquidEyes.TechAnalysis
{
    public interface IBasicIndicator
    {
        DataPoint AddAndCalc(ICandle candle);
    }
}
