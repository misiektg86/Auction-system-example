using AuctionBuyNow.Domain.Auctions.Exceptions;
using AuctionBuyNow.Domain.Auctions.ValueObjects;

namespace AuctionBuyNow.Domain.Auctions.Entities;

public class AuctionItem
{
        public AuctionId Id { get; private set; }
        public string Name { get; private set; }
        public Stock Stock { get; private set; }

        public bool IsSoldOut => Stock.IsEmpty;

        public AuctionItem(AuctionId id, string title, Stock stock)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required", nameof(title));

            if (stock.IsEmpty)
                throw new ArgumentException("Stock must be greater than 0", nameof(stock));

            Id = id;
            Name = title;
            Stock = stock;
        }

        private AuctionItem() { }

        public void TryReserve()
        {
            if (IsSoldOut)
                throw new AuctionAlreadySoldException(Id);

            Stock = Stock.Decrease(1);
        }
}