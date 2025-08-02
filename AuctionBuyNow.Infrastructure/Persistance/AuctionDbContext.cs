using AuctionBuyNow.Domain.Auctions.Entities;
using AuctionBuyNow.Domain.Auctions.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AuctionBuyNow.Infrastructure.Persistance;

public class AuctionDbContext(DbContextOptions<AuctionDbContext> options) : DbContext(options)
{
    public DbSet<AuctionItem> AuctionItems => Set<AuctionItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuctionItem>(builder =>
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(id => id.Value, value => new AuctionId(value));

            builder.Property(x => x.Stock)
                .HasConversion(s => s.Value, v => new Stock(v));

            builder.Property(x => x.Name).IsRequired();
        });
    }
}