using AuctionBuyNow.Domain.Repositiories;
using MediatR;

namespace AuctionBuyNow.Application.Items.Commands;

public class BuyNowHandler(IAuctionRepository repository) : IRequestHandler<BuyNowCommand, BuyNowResult>
{
    public async Task<BuyNowResult> Handle(BuyNowCommand request, CancellationToken cancellationToken)
    {
        var success = await repository.ReserveAsync(request.ItemId);
        return success ? BuyNowResult.Success : BuyNowResult.SoldOut;
    }
}