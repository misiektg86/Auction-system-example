using AuctionBuyNow.Domain.Auctions.ValueObjects;

namespace AuctionBuyNow.Domain.Auctions.Exceptions;

public class AuctionAlreadySoldException(AuctionId auctionId)
    : Exception($"Auction item {auctionId} is already sold out.")
{
    public AuctionId AuctionId { get; } = auctionId;
}