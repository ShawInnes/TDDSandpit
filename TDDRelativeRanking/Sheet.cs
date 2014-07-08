using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics;

namespace TDDRelativeRanking
{
    [DebuggerDisplay("{Id} : {Tally} : {Total} : {Placing} ({ExpectedResult}) {IsTie}")]
    public class Sheet
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public string ScoringType { get; set; }
        public string Name { get; set; }
        public string Leader { get; set; }
        public string Follower { get; set; }
        public int[] Scores { get; set; }
        public int Tally { get; set; }
        public int Total { get; set; }
        public int Placing { get; set; }
        public bool IsTie { get; set; }
        public int ExpectedResult { get; set; }

        /// <summary>
        /// Initializes a new instance of the Sheet class.
        /// </summary>
        public Sheet(int id, int expected, params int[] scores)
        {
            Id = id;
            ExpectedResult = expected;
            Scores = scores;
        }

        /// <summary>
        /// Initializes a new instance of the Sheet class.
        /// </summary>
        public Sheet()
        {
            
        }
    }
}
