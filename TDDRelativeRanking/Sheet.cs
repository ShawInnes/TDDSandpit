using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics;

namespace TDDRelativeRanking
{
    [DebuggerDisplay("{Id} : {Tally} : {Total} : {Placing} ({ExpectedPlacing}) ")]
    public class Sheet
    {
        public int Id { get; set; }
        public int[] Scores { get; set; }
        public int Tally { get; set; }
        
        public int Total { get; set; }
        public int Placing { get; set; }
        public int ExpectedPlacing { get; set; }

        /// <summary>
        /// Initializes a new instance of the Sheet class.
        /// </summary>
        public Sheet(int id, int expectedPlace, params int[] scores)
        {
            Id = id;
            ExpectedPlacing = expectedPlace;
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
