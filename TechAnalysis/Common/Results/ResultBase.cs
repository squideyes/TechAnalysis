// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System;

namespace SquidEyes.TechAnalysis
{
    public abstract class ResultBase : IResult
    {
        public ResultBase(ResultKind kind)
        {
            Kind = kind;
        }

        public ResultKind Kind { get; }

        public DateTime OpenOn { get; init; }
    }
}