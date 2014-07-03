using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TDDRelativeRanking
{
    public class Sheet
    {
        public int Id { get; set; }
        public int[] Scores { get; set; }

        /// <summary>
        /// Initializes a new instance of the Sheet class.
        /// </summary>
        public Sheet(int id, params int[] scores)
        {
            Id = id;
            Scores = scores;
        }
    }
}
