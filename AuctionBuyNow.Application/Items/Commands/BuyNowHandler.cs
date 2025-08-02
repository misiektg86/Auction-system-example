using AuctionBuyNow.Application.Common;
using AuctionBuyNow.Application.Common.Services;
using AuctionBuyNow.Domain.Auctions.Exceptions;
using AuctionBuyNow.Domain.Auctions.Repositiories;
using AuctionBuyNow.Domain.Auctions.ValueObjects;
using MediatR;

namespace AuctionBuyNow.Application.Items.Commands;

public class BuyNowHandler(IAuctionRepository repository, IUnitOfWork unitOfWork, IRedisService redis)
    : IRequestHandler<BuyNowCommand, BuyNowResult>
{
    public async Task<BuyNowResult> Handle(BuyNowCommand request, CancellationToken cancellationToken)
    {
        var redisKey = $"stock:{request.ItemId}";

        var remaining = await redis.DecrementAsync(redisKey);
        if (remaining < 0)
        {
            await redis.IncrementAsync(redisKey); // Rollback Redis decrement
            return BuyNowResult.SoldOut;
        }

        // Try to reserve in domain & DB
        var id = new AuctionId(request.ItemId);
        var auctionItem = await repository.GetByIdAsync(id, cancellationToken);

        if (auctionItem is null)
        {
            await redis.IncrementAsync(redisKey); // Rollback Redis decrement
            return BuyNowResult.NotFound;
        }

        try
        {
            auctionItem.TryReserve();
            await repository.UpdateAsync(auctionItem, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return BuyNowResult.Success;
        }
        catch (AuctionAlreadySoldException)
        {
            await redis.IncrementAsync(redisKey); // Rollback Redis decrement
            return BuyNowResult.SoldOut;
        }
        catch
        {
            await redis.IncrementAsync(redisKey); // Rollback on unexpected failure
            throw;
        }
    }
}