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
using Autofac;

namespace TDDRelativeRanking
{
    [TestFixture]
    public class RelativeRankingTests
    {
        public class DataFactoryClass
        {
            public static IEnumerable RelativeRankingTestCases
            {
                get
                {
                    var loadedList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Sheet>>(System.IO.File.ReadAllText(@"data\extracted.json"));

                    foreach (IGrouping<string, Sheet> grouping in loadedList
                                                                    .Where(p => p.ScoringType.Equals("RelativeRanking"))
                                                                    .GroupBy(p => p.Title))
                    {
                        int newId = 1;
                        List<Sheet> list = grouping.ToList();
                        list.ForEach(p => p.Id = newId++);

                        yield return new TestCaseData(list)
                                                .SetCategory("RelativeRanking")
                                                .SetName(grouping.Key);
                    }
                }
            }
        }

        private static IContainer container;

        [TestFixtureSetUp]
        public void Setup()
        {
            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Is(Serilog.Events.LogEventLevel.Debug)
                            .WriteTo.Seq("http://127.0.0.1:5341/")
                            .CreateLogger();

            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                .AssignableTo<IResultsCalculator>()
                .Keyed<IResultsCalculator>(t => GetMessageType(t));

            container = builder.Build();
        }

        static ScoringType GetMessageType(Type type)
        {
            var att = type.GetCustomAttributes(true).OfType<ScoringHandlerAttribute>().FirstOrDefault();
            if (att == null)
            {
                throw new Exception("Somone forgot to put the ScoringHandlerAttribute on an IResultsCalculator!");
            }

            return att.ScoringType;
        }


        [Test, TestCaseSource(typeof(DataFactoryClass), "RelativeRankingTestCases")]
        public void RelativeRankingDataDrivenValidationTests(List<Sheet> list)
        {
            using (var lifetime = container.BeginLifetimeScope())
            {
                var handler = lifetime.ResolveKeyed<IResultsCalculator>(ScoringType.RelativeRanking);

                foreach (Sheet sheet in handler.GetResults(list))
                    sheet.Placing.ShouldBe(sheet.ExpectedResult);
            }
        }

        [Test]
        [Category("RelativeRankingCalculator")]
        public void HasSomeTiesPlacingsShouldBeCorrect()
        {
            var list = new List<Sheet> {
              new Sheet(100,1, 1, 1, 3, 2, 3),
              new Sheet(200,2, 3, 3, 6, 4, 1),
              new Sheet(300,3, 2, 4, 1, 5, 5),
              new Sheet(400,4, 6, 5, 4, 1, 2),
              new Sheet(500,5, 5, 6, 2, 3, 4),
              new Sheet(600,6, 4, 2, 5, 6, 6)
                };

            RelativeRankingDataDrivenValidationTests(list);
        }

        [Test]
        [Category("RelativeRankingCalculator")]
        public void FiveJudgesStandardPlacingsShouldBeCorrect()
        {
            var list = new List<Sheet> {
              new Sheet(100,1,1,4,2,2,5),
              new Sheet(200,2,3,1,3,4,2),
              new Sheet(300,4,4,3,5,3,1),
              new Sheet(400,3,2,5,1,5,3),
              new Sheet(500,5,5,2,4,1,4)
                };

            RelativeRankingDataDrivenValidationTests(list);
        }

        [Test]
        [Category("RelativeRankingCalculator")]
        public void StandardPlacingsShouldBeCorrect()
        {
            var list = new List<Sheet> {
                new Sheet(100,1,1,1,1,1,1,1,1),
                new Sheet(200,2,2,2,2,3,3,3,4),
                new Sheet(300,3,3,3,3,2,2,2,5),
                new Sheet(400,4,4,4,4,4,4,4,2),
                new Sheet(500,5,5,5,5,5,5,5,3),
                new Sheet(600,6,6,6,6,6,6,6,6),
                new Sheet(700,7,7,7,7,7,7,7,7),
                new Sheet(800,8,8,8,8,8,8,8,8),
                new Sheet(900,9,9,9,9,9,9,9,9),
                new Sheet(1000,10,10,10,10,10,10,10,10),
            };

            RelativeRankingDataDrivenValidationTests(list);
        }

        [Test]
        [Category("RelativeRankingCalculator")]
        public void PlacingsShouldBeCorrect()
        {
            var list = new List<Sheet> {
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

            RelativeRankingDataDrivenValidationTests(list);
        }

        [Test]
        [Category("RelativeRankingCalculator")]
        public void DeadTieShouldMarkAsTied()
        {
            var list = new List<Sheet> {
                new Sheet(100,1,1,1,1,1,1),
                new Sheet(200,2,2,4,3,2,3),
                new Sheet(300,3,3,2,2,3,4),
                new Sheet(400,4,4,3,4,5,2),
                new Sheet(500,5,5,5,5,4,5),
            };

            RelativeRankingDataDrivenValidationTests(list);
        }

        [TestCase(1, Result = 1, Category = "JudgeMajority")]
        [TestCase(2, Result = 2, Category = "JudgeMajority")]
        [TestCase(3, Result = 2, Category = "JudgeMajority")]
        [TestCase(4, Result = 3, Category = "JudgeMajority")]
        [TestCase(5, Result = 3, Category = "JudgeMajority")]
        [TestCase(6, Result = 4, Category = "JudgeMajority")]
        [TestCase(7, Result = 4, Category = "JudgeMajority")]
        [TestCase(8, Result = 5, Category = "JudgeMajority")]
        [TestCase(9, Result = 5, Category = "JudgeMajority")]
        public int GetJudgeMajority(int judgeCount)
        {
            return (int)Math.Floor(judgeCount / 2.0) + 1;
        }
    }
}
