namespace AuctionBuyNow.Application.Common.Services;

public interface IRedisService
{
    Task<long> DecrementAsync(string key);
    Task IncrementAsync(string key);
}