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
    [ScoringHandler(ScoringType.RelativeRanking)]
    public class RelativeRankingCalculator : IResultsCalculator
    {
        private int _place = 1;
        private int _teamCount = 0;
        private List<Sheet> _list;

        private List<Sheet> GetRemainingPlaces(List<Sheet> sourceList, int placeIndex, int majority)
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

        private void SetPlace(int id, bool isTie = false)
        {
            Sheet item = _list.Single(p => p.Id == id);
            item.Placing = _place++;
            item.IsTie = isTie;
        }

        private void BreakTie(List<Sheet> tieList, int placeIndex, int majority)
        {
            if (placeIndex > _teamCount)
            {
                Log.Warning("Unable to break tie for {Place} place.  There are {TieList} entries in this group.", _place, tieList.Count);
                
                foreach (Sheet sheet in tieList)
                    SetPlace(sheet.Id, isTie: true);

                return;
            }

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

                nextList = GetRemainingPlaces(_list.Where(p => tieList.Select(q => q.Id).Contains(p.Id)).ToList(), placeIndex, majority);
            }
        }

        public List<Sheet> GetResults(List<Sheet> input)
        {
            _list = input;
            _teamCount = _list.Count();

            int judgeCount = _list.FirstOrDefault().Scores.Count();
            int majority = (int)(Math.Floor(judgeCount / 2.0) + (judgeCount % 2));

            for (int placeIndex = 1; placeIndex <= _teamCount; placeIndex++)
            {
                var remainingList = GetRemainingPlaces(_list, placeIndex, majority);

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

            return _list;
        }
    }
}
