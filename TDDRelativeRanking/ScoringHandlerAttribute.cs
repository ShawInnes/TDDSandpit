using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using Serilog;
using Seq;
using System.Collections;

namespace TDDRelativeRanking
{
    public class ScoringHandlerAttribute : Attribute
    {
        public ScoringHandlerAttribute(ScoringType scoringType)
        {
            ScoringType = scoringType;
        }

        public ScoringType ScoringType { get; private set; }
    }
}
