namespace AuctionBuyNow.Domain.Auctions.ValueObjects;

public sealed record AuctionId(Guid Value)
{
    public static AuctionId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(AuctionId id) => id.Value;
    public static implicit operator AuctionId(Guid value) => new(value);
}