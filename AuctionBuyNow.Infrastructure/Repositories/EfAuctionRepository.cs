using AuctionBuyNow.Domain.Entities;
using AuctionBuyNow.Domain.Repositiories;
using AuctionBuyNow.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace AuctionBuyNow.Infrastructure.Repositories;

public class EfAuctionRepository(AuctionDbContext db) : IAuctionRepository
{
    public async Task<List<AuctionItem>> GetAllAsync()
        => await db.AuctionItems.ToListAsync();

    public async Task<AuctionItem?> GetByIdAsync(Guid id)
        => await db.AuctionItems.FindAsync(id);

    public async Task<bool> ReserveAsync(Guid itemId)
    {
        var item = await db.AuctionItems.FindAsync(itemId);
        if (item == null || !item.TryReserve()) return false;

        await db.SaveChangesAsync();
        return true;
    }
}