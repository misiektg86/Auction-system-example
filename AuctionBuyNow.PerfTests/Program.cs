using AuctionBuyNow.PerfTests.Scenarios;
using AuctionBuyNow.PerfTests.Scenarios.Setup;
using NBomber.Contracts.Stats;
using NBomber.CSharp;

namespace AuctionBuyNow.PerfTests
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var context = await PerfTestContext.CreateAsync();

            var scenarios = new[]
            {
                await BuyNowScenario.CreateAsync(context)
            };

            NBomberRunner
                .RegisterScenarios(scenarios)
                .WithReportFileName("perf_report")
                .WithReportFormats(ReportFormat.Html, ReportFormat.Txt)
                .Run();
        }
    }
}
