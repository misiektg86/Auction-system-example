using AuctionBuyNow.Domain.Auctions.Entities;
using AuctionBuyNow.Domain.Auctions.Repositiories;
using AuctionBuyNow.Domain.Auctions.ValueObjects;
using AuctionBuyNow.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace AuctionBuyNow.Infrastructure.Repositories;

public class EfAuctionRepository(AuctionDbContext db) : IAuctionRepository
{
    public async Task<List<AuctionItem>> GetAllAsync(CancellationToken cancellationToken = default)
        => await db.AuctionItems.ToListAsync(cancellationToken: cancellationToken);

    public async Task<AuctionItem?> GetByIdAsync(AuctionId id, CancellationToken cancellationToken = default) =>
        await db.AuctionItems
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task AddAsync(AuctionItem auctionItem, CancellationToken cancellationToken = default) => await db.AuctionItems.AddAsync(auctionItem, cancellationToken);

    public Task UpdateAsync(AuctionItem auctionItem, CancellationToken cancellationToken = default)
    {
        db.AuctionItems.Update(auctionItem);
        return Task.CompletedTask;
    }
}