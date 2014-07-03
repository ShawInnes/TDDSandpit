using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TDDRelativeRanking
{
    [TestFixture]
    public class Tests
    {
        public List<Sheet> list { get; set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            list = new List<Sheet> {
                    new Sheet(119,1,1,2,1,1,1,2),
                    new Sheet(112,5,3,8,7,4,4,3),
                    new Sheet(135,7,4,3,3,5,10,4),
                    new Sheet(181,2,5,10,2,6,6,1),
                    new Sheet(190,6,2,5,8,3,3,6),
                    new Sheet(142,4,10,6,9,2,2,8),
                    new Sheet(172,9,9,1,4,9,5,7),
                    new Sheet(157,3,7,9,5,8,8,5),
                    new Sheet(117,10,8,7,6,7,7,9),
                    new Sheet(171,8,6,4,10,10,9,10)
                };
        }

        [Test]
        public void Test()
        {

        }
    }
}
