namespace AuctionBuyNow.Domain.Entities;

public class AuctionItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int TotalStock { get; set; }
    public int Reserved { get; set; }

    public bool IsAvailable => Reserved < TotalStock;

    public bool TryReserve()
    {
        if (!IsAvailable) return false;
        Reserved++;
        return true;
    }
}