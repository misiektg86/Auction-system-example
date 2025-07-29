using AuctionBuyNow.Application.Common.Services;
using AuctionBuyNow.Infrastructure.Persistance;
using AuctionBuyNow.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace AuctionBuyNow.PerfTests.Scenarios.Setup;

public class PerfTestContext
{
    public AuctionDbContext DbContext { get; init; }
    public IRedisService Redis { get; init; }

    private PerfTestContext(AuctionDbContext dbContext, IRedisService redis)
    {
        DbContext = dbContext;
        Redis = redis;
    }

    public static async Task<PerfTestContext> CreateAsync()
    {
        var services = new ServiceCollection();

        services.AddDbContext<AuctionDbContext>(options =>
            options.UseSqlServer("Server=localhost,1433;Database=AuctionDb;User Id=sa;Password=Your_strong_password123;TrustServerCertificate=True"));

        var redis = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
        services.AddSingleton<IConnectionMultiplexer>(redis);
        services.AddScoped<IRedisService, RedisService>();

        var provider = services.BuildServiceProvider();
        var scope = provider.CreateScope();

        return new PerfTestContext(
            scope.ServiceProvider.GetRequiredService<AuctionDbContext>(), scope.ServiceProvider.GetRequiredService<IRedisService>()
        );
    }
}