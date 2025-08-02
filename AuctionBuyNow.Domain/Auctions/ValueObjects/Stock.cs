namespace AuctionBuyNow.Domain.Auctions.ValueObjects;


public sealed record Stock(int Value)
{
    public static Stock Zero => new(0);

    public Stock Decrease(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Decrease amount must be positive");

        if (Value < amount)
            throw new InvalidOperationException("Not enough stock");

        return new Stock(Value - amount);
    }

    public bool IsEmpty => Value <= 0;
}