using AuctionBuyNow.Application.Common.Services;
using StackExchange.Redis;
using IDatabase = StackExchange.Redis.IDatabase;

namespace AuctionBuyNow.Infrastructure.Services;

public class RedisService(IConnectionMultiplexer redis) : IRedisService
{
    private readonly IDatabase _db = redis.GetDatabase();

    public Task<long> DecrementAsync(string key)
        => _db.StringDecrementAsync(key);

    public Task IncrementAsync(string key)
        => _db.StringIncrementAsync(key);
}