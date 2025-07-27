using AuctionBuyNow.Domain.Entities;
using AuctionBuyNow.Domain.Repositiories;

namespace AuctionBuyNow.Infrastructure.Repositories;

public class InMemoryAuctionRepository : IAuctionRepository
{
    private readonly List<AuctionItem> _items =
    [
        new()
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Test Item", TotalStock = 2, Reserved = 0
        }
    ];

    public Task<List<AuctionItem>> GetAllAsync()
        => Task.FromResult(_items);

    public Task<AuctionItem?> GetByIdAsync(Guid id)
        => Task.FromResult(_items.FirstOrDefault(x => x.Id == id));

    public Task<bool> ReserveAsync(Guid itemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);
        if (item is null) return Task.FromResult(false);

        var result = item.TryReserve();
        return Task.FromResult(result);
    }
}