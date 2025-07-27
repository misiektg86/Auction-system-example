using AuctionBuyNow.Domain.Entities;

namespace AuctionBuyNow.Domain.Repositiories;


public interface IAuctionRepository
{
    Task<List<AuctionItem>> GetAllAsync();
    Task<AuctionItem?> GetByIdAsync(Guid id);
    Task<bool> ReserveAsync(Guid itemId);
}