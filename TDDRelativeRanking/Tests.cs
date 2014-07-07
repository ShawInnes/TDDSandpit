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
    [TestFixture]
    public class Tests
    {
        public List<Sheet> list { get; set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Is(Serilog.Events.LogEventLevel.Debug)
                            .WriteTo.Seq("http://127.0.0.1:5341/")
                            .CreateLogger();

            list = new List<Sheet> {
                    new Sheet(119,1,1,1,2,1,1,1,2),
                    new Sheet(112,2,5,3,8,7,4,4,3),
                    new Sheet(135,3,7,4,3,3,5,10,4),
                    new Sheet(181,4,2,5,10,2,6,6,1),
                    new Sheet(190,5,6,2,5,8,3,3,6),
                    new Sheet(142,6,4,10,6,9,2,2,8),
                    new Sheet(172,7,7,9,9,1,4,9,5,7),
                    new Sheet(157,8,3,7,9,5,8,8,5),
                    new Sheet(117,9,10,8,7,6,7,7,9),
                    new Sheet(171,10,8,6,4,10,10,9,10)
                };

            //list = new List<Sheet> {
            //  new Sheet(100,1,1,4,2,2,5),
            //  new Sheet(200,2,3,1,3,4,2),
            //  new Sheet(300,4,4,3,5,3,1),
            //  new Sheet(400,3,2,5,1,5,3),
            //  new Sheet(500,5,5,2,4,1,4)
            //    };

            //list = new List<Sheet> {
            //  new Sheet(100,1, 1, 1, 3, 2, 3),
            //  new Sheet(200,2, 3, 3, 6, 4, 1),
            //  new Sheet(300,3, 2, 4, 1, 5, 5),
            //  new Sheet(400,4, 6, 5, 4, 1, 2),
            //  new Sheet(500,5, 5, 6, 2, 3, 4),
            //  new Sheet(600,6, 4, 2, 5, 6, 6)
            //    };

            //list = new List<Sheet> {
            //    new Sheet(100,1,1,1,1,1,1,1,1),
            //    new Sheet(200,2,2,2,2,3,3,3,4),
            //    new Sheet(300,3,3,3,3,2,2,2,5),
            //    new Sheet(400,4,4,4,4,4,4,4,2),
            //    new Sheet(500,5,5,5,5,5,5,5,3),
            //    new Sheet(600,6,6,6,6,6,6,6,6),
            //    new Sheet(700,7,7,7,7,7,7,7,7),
            //    new Sheet(800,8,8,8,8,8,8,8,8),
            //    new Sheet(900,9,9,9,9,9,9,9,9),
            //    new Sheet(1000,10,10,10,10,10,10,10,10),
            //};

            //list = new List<Sheet> {
            //    new Sheet(100,1,1,2,3,4,5,6,7),
            //    new Sheet(200,2,7,1,2,3,4,5,6),
            //    new Sheet(300,3,6,7,1,2,3,4,5),
            //    new Sheet(400,4,5,6,7,1,2,3,4),
            //    new Sheet(500,5,4,5,6,7,1,2,3),
            //    new Sheet(600,6,3,4,5,6,7,1,2),
            //    new Sheet(700,7,2,3,4,5,6,7,1),
            //    new Sheet(800,8,8,9,10,8,9,10,8),
            //    new Sheet(900,9,9,8,9,10,8,9,9),
            //    new Sheet(1000,10,10,10,8,9,10,8,10),
            //};

        }

        public List<Sheet> GetRemainingPlaces(List<Sheet> sourceList, int placeIndex, int majority)
        {
            List<Sheet> internalList = sourceList
                                 .Where(p => p.Placing == 0)
                                 .Select(p => new Sheet
                                     {
                                         Id = p.Id,
                                         Tally = p.Scores.Where(s => s <= placeIndex).Count(),
                                         Total = p.Scores.Where(s => s <= placeIndex).Sum(),
                                         Scores = p.Scores
                                     })
                                 .Where(p => p.Tally >= majority)
                                 .ToList();

            return internalList;
        }

        private void SetPlace(int id)
        {
            list.Single(p => p.Id == id).Placing = _place++;
        }

        public static List<T> Shuffle<T>(List<T> list)
        {
            return Shuffle(list.ToArray<T>()).ToList<T>();
        }

        public static T[] Shuffle<T>(T[] array)
        {
            T[] retArray = new T[array.Length];
            array.CopyTo(retArray, 0);

            Random random = new Random();
            for (int i = 0; i < array.Length; i += 1)
            {
                int swapIndex = random.Next(i, array.Length);
                if (swapIndex != i)
                {
                    T temp = retArray[i];
                    retArray[i] = retArray[swapIndex];
                    retArray[swapIndex] = temp;
                }
            }

            return retArray;
        }

        private void BreakTie(List<Sheet> tieList, int placeIndex, int majority)
        {
            if (placeIndex > _teamCount)
                throw new InvalidOperationException("I can't break this tie");
            
            List<Sheet> nextList = GetRemainingPlaces(tieList, placeIndex, majority);

            while (nextList.Any())
            {
                var tallyGroup = nextList.GroupBy(p => p.Tally).ToList();
                int nextMajority = tallyGroup.Max(p => p.Key);

                if (tallyGroup.Any(p => p.Key == nextMajority && p.Count() == 1))
                {
                    Log.Debug("Found #{Place} (bigger majority) on iteration #{PlaceIndex}", _place, placeIndex);

                    SetPlace(tallyGroup.Single(g => g.Key == nextMajority).FirstOrDefault().Id);
                }
                else
                {
                    var totalGroup = tallyGroup
                                        .Where(p => p.Key == nextMajority)
                                        .SelectMany(p => p)
                                        .GroupBy(p => p.Total)
                                        .OrderBy(p => p.Key)
                                        .FirstOrDefault();

                    if (totalGroup.Count() == 1)
                    {
                        Log.Debug("Found #{Place} (lowest total score) on iteration #{PlaceIndex}", _place, placeIndex);

                        SetPlace(totalGroup.FirstOrDefault().Id);
                    }
                    else
                    {
                        Log.Debug("Attempting to resolve complex tie on iteration #{PlaceIndex}", placeIndex);
                        BreakTie(totalGroup.ToList(), placeIndex + 1, majority);
                    }
                }

                nextList = GetRemainingPlaces(list.Where(p => tieList.Select(q => q.Id).Contains(p.Id)).ToList(), placeIndex, majority);
            }
            //var totalGroup = tieList.GroupBy(p => p.Total).ToList();

            Log.Debug("Exiting Tie Breaker");
        }

        private int _place = 1;
        private int _teamCount = 0;

        [Test]
        public void PlacingsShouldBeCorrect()
        {
            int judgeCount = list.FirstOrDefault().Scores.Count();
            int majority = (int)Math.Ceiling(judgeCount / 2.0);

            _teamCount = list.Count();

            for (int placeIndex = 1; placeIndex <= _teamCount; placeIndex++)
            {
                var remainingList = GetRemainingPlaces(list, placeIndex, majority);

                int candidateCount = remainingList.Count();

                if (candidateCount == 0)
                {
                    Log.Debug("Indeterminate on iteration #{PlaceIndex}", placeIndex);
                }
                else if (candidateCount == 1)
                {
                    Log.Debug("Found #{Place} on iteration #{PlaceIndex}", _place, placeIndex);
                    SetPlace(remainingList.Single().Id);
                }
                else
                {
                    Log.Debug("There's a tie for #{Place} on iteration #{PlaceIndex}", _place, placeIndex);

                    BreakTie(remainingList, placeIndex, majority);
                }
            }

            foreach (Sheet sheet in list)
            {
                sheet.Placing.ShouldBe(sheet.ExpectedPlacing);
            }
        }
    }
}
