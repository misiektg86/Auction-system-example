namespace AuctionBuyNow.Application.Items.Commands;

using MediatR;
public record BuyNowCommand(Guid ItemId, Guid UserId) : IRequest<BuyNowResult>;

public enum BuyNowResult
{
    Success,
    SoldOut
}
