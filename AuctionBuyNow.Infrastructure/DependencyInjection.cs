using AuctionBuyNow.Domain.Repositiories;
using AuctionBuyNow.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionBuyNow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IAuctionRepository, InMemoryAuctionRepository>();
        return services;
    }
}