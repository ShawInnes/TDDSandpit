using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using Serilog;
using Seq;

namespace TDDRelativeRanking
{
    [ScoringHandler(ScoringType.Callback)]
    public class CallbackCalculator : IResultsCalculator
    {
        private List<Sheet> _list;

        public List<Sheet> GetResults(List<Sheet> input)
        {
            _list = input;

            foreach (Sheet sheet in _list)
                sheet.Total = sheet.Scores.Sum(p => p);

            int place = 1;
            foreach (Sheet sheet in _list.OrderByDescending(p => p.Total))
                sheet.Placing = place++;

            return _list;
        }
    }
}
