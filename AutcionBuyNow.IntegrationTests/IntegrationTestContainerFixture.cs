
using AuctionBuyNow.Domain.Entities;
using AuctionBuyNow.Infrastructure.Persistance;
using AuctionBuyNow.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Testcontainers.Redis;

namespace AutcionBuyNow.IntegrationTests;

public class IntegrationTestContainerFixture : IAsyncLifetime
{
    public HttpClient Client { get; private set; } = null!;
    private MsSqlContainer _sqlContainer = null!;
    private RedisContainer _redisContainer = null!;
    private WebApplicationFactory<Program> _factory = null!;
    private IServiceScope _scope = null!;

    private static readonly Guid TestItemId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    public async Task InitializeAsync()
    {
        _sqlContainer = new MsSqlBuilder()
            .WithPassword("Your_password123")
            .WithCleanUp(true)
            .Build();

        _redisContainer = new RedisBuilder()
            .WithImage("redis:7")
            .WithCleanUp(true)
            .Build();

        await _sqlContainer.StartAsync();
        await _redisContainer.StartAsync();

        var sqlConnectionString = _sqlContainer.GetConnectionString() + ";TrustServerCertificate=true";
        var redisAddress = _redisContainer.GetConnectionString(); // hostname:port

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseSetting("ConnectionStrings:DefaultConnection", sqlConnectionString);
                builder.UseSetting("ConnectionStrings:Redis", redisAddress);
            });

        Client = _factory.CreateClient();

        _scope = _factory.Services.CreateScope();
        var db = _scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
        await db.Database.EnsureCreatedAsync();

        db.AuctionItems.Add(new AuctionItem
        {
            Id = TestItemId,
            Name = "Test Item",
            TotalStock = 10,
            Reserved = 0
        });

        await db.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        if (_scope != null)
            _scope.Dispose();

        if (_factory != null)
            _factory.Dispose();

        if (_sqlContainer != null)
            await _sqlContainer.DisposeAsync();

        if (_redisContainer != null)
            await _redisContainer.DisposeAsync();
    }

    public Guid GetTestItemId() => TestItemId;
}