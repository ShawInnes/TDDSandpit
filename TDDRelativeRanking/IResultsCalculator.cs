using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDDRelativeRanking
{
    public interface IResultsCalculator
    {
        List<Sheet> GetResults(List<Sheet> input);
    }
}
