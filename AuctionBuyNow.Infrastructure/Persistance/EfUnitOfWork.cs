using AuctionBuyNow.Application.Common;

namespace AuctionBuyNow.Infrastructure.Persistance;

public class EfUnitOfWork(AuctionDbContext context) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}