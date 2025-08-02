using AuctionBuyNow.Domain.Auctions.Entities;
using AuctionBuyNow.Domain.Auctions.ValueObjects;

namespace AuctionBuyNow.Domain.Auctions.Repositiories;


public interface IAuctionRepository
{
    Task<List<AuctionItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AuctionItem?> GetByIdAsync(AuctionId id, CancellationToken cancellationToken = default);
    Task AddAsync(AuctionItem auctionItem, CancellationToken cancellationToken = default);
    Task UpdateAsync(AuctionItem auctionItem, CancellationToken cancellationToken = default);
}