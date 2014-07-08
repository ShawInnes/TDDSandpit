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
    public class CallbackTests
    {
        public class DataFactoryClass
        {
            public static IEnumerable CallbackTestCases
            {
                get
                {
                    var loadedList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Sheet>>(System.IO.File.ReadAllText(@"data\extracted.json"));

                    foreach (IGrouping<string, Sheet> grouping in loadedList
                                                                    .Where(p => p.ScoringType.Equals("Callback"))
                                                                    .GroupBy(p => p.Title))
                    {
                        int newId = 1;
                        List<Sheet> list = grouping.ToList();
                        list.ForEach(p => p.Id = newId++);

                        yield return new TestCaseData(list)
                                                .SetCategory("Callback")
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

        [Test, TestCaseSource(typeof(DataFactoryClass), "CallbackTestCases")]
        public void CallbackDataDrivenValidationTests(List<Sheet> list)
        {
            using (var lifetime = container.BeginLifetimeScope())
            {
                var handler = lifetime.ResolveKeyed<IResultsCalculator>(ScoringType.Callback);

                foreach (Sheet sheet in handler.GetResults(list))
                    sheet.Total.ShouldBe(sheet.ExpectedResult);
            }
        }
    }
}