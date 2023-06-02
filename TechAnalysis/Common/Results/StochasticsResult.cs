// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.TechAnalysis
{
    public class StochasticsResult : ResultBase
    {
        public StochasticsResult()
            : base(ResultKind.StochasticsResult)
        {
        }

        public double K { get; init; }
        public double D { get; init; }
    }
}