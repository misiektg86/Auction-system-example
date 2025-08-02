using AuctionBuyNow.Application.Common;
using AuctionBuyNow.Application.Common.Services;
using AuctionBuyNow.Domain.Auctions.Repositiories;
using AuctionBuyNow.Infrastructure.Persistance;
using AuctionBuyNow.Infrastructure.Repositories;
using AuctionBuyNow.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace AuctionBuyNow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // services.AddSingleton<IAuctionRepository, InMemoryAuctionRepository>();

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        services.AddDbContext<AuctionDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAuctionRepository, EfAuctionRepository>();
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(config.GetConnectionString("Redis")!));
        services.AddSingleton<IRedisService, RedisService>(); 

        return services;
    }
}