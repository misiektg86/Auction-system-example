using AuctionBuyNow.Application.Common.Services;
using AuctionBuyNow.Domain.Repositiories;
using MediatR;

namespace AuctionBuyNow.Application.Items.Commands;

public class BuyNowHandler(IAuctionRepository repository, IRedisService redis)
    : IRequestHandler<BuyNowCommand, BuyNowResult>
{
    public async Task<BuyNowResult> Handle(BuyNowCommand request, CancellationToken cancellationToken)
    {
        var redisKey = $"stock:{request.ItemId}";

        var remaining = await redis.DecrementAsync(redisKey);
        if (remaining < 0)
        {
            await redis.IncrementAsync(redisKey);
            return BuyNowResult.SoldOut;
        }

        // Rezerwacja w bazie danych
        var success = await repository.ReserveAsync(request.ItemId);

        if (!success)
        {
            await redis.IncrementAsync(redisKey);
            return BuyNowResult.SoldOut;
        }

        return BuyNowResult.Success;
    }
}