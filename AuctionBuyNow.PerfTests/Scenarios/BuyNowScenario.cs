using System.Net.Http.Json;
using AuctionBuyNow.Domain.Auctions.Entities;
using AuctionBuyNow.Domain.Auctions.ValueObjects;
using AuctionBuyNow.PerfTests.Scenarios.Setup;
using NBomber.Contracts;
using NBomber.CSharp;

namespace AuctionBuyNow.PerfTests.Scenarios;

public static class BuyNowScenario
{
    public static async Task<ScenarioProps> CreateAsync(PerfTestContext ctx)
    {
        var itemId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        ctx.DbContext.AuctionItems.RemoveRange(ctx.DbContext.AuctionItems);
        ctx.DbContext.AuctionItems.Add(new AuctionItem(
            new AuctionId(itemId),
            "Preload item",
            new Stock(50)
        ));

        await ctx.DbContext.SaveChangesAsync(); 
        await ctx.Redis.SetStockAsync(itemId, 50);

        var scenario = Scenario.Create("buy_now", async stepCtx =>
            {
                var client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
                var response = await client.PostAsJsonAsync("/api/auction/buy-now", new
                {
                    itemId,
                    userId = Guid.NewGuid()
                });

                return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
            })
            .WithLoadSimulations(Simulation.Inject(rate: 50, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(30)));

        return scenario;
    }
}