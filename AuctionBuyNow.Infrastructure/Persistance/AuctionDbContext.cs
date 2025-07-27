using AuctionBuyNow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionBuyNow.Infrastructure.Persistance;

public class AuctionDbContext(DbContextOptions<AuctionDbContext> options) : DbContext(options)
{
    public DbSet<AuctionItem> AuctionItems => Set<AuctionItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuctionItem>().HasKey(a => a.Id);
        modelBuilder.Entity<AuctionItem>().Property(a => a.Name).IsRequired();
    }
}