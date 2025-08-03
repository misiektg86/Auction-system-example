using AuctionBuyNow.Application.Common;
using AuctionBuyNow.Application.Common.Services;
using AuctionBuyNow.Application.Items.Commands;
using AuctionBuyNow.Domain.Auctions.Entities;
using AuctionBuyNow.Domain.Auctions.Exceptions;
using AuctionBuyNow.Domain.Auctions.Repositiories;
using AuctionBuyNow.Domain.Auctions.ValueObjects;
using Moq;

namespace AuctionBuyNow.UnitTests;

public class BuyNowHandlerTests
{
    private readonly Mock<IAuctionRepository> _repoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IRedisService> _redisMock;
    private readonly BuyNowHandler _handler;

    public BuyNowHandlerTests()
    {
        _repoMock = new Mock<IAuctionRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _redisMock = new Mock<IRedisService>();

        _handler = new BuyNowHandler(
            _repoMock.Object,
            _unitOfWorkMock.Object,
            _redisMock.Object
        );
    }

    [Fact]
    public async Task Should_Return_Success_When_Item_Is_Reservable()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var command = new BuyNowCommand(itemId, userId);

        var auction = new AuctionItem(new AuctionId(itemId), "test", new Stock(5));

        _redisMock.Setup(r => r.DecrementAsync(It.IsAny<string>()))
            .ReturnsAsync(5);

        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<AuctionId>(), default))
            .ReturnsAsync(auction);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.Equal(BuyNowResult.Success, result);
        _unitOfWorkMock.Verify(u => u.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task Should_Return_SoldOut_When_Redis_Is_Negative()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var command = new BuyNowCommand(itemId, userId);

        _redisMock.Setup(r => r.DecrementAsync(It.IsAny<string>()))
            .ReturnsAsync(-1);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.Equal(BuyNowResult.SoldOut, result);
        _repoMock.Verify(r => r.GetByIdAsync(It.IsAny<AuctionId>(), default), Times.Never);
    }

    [Fact]
    public async Task Should_Increment_Redis_When_Auction_Not_Found()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var command = new BuyNowCommand(itemId, userId);

        _redisMock.Setup(r => r.DecrementAsync(It.IsAny<string>()))
            .ReturnsAsync(2);

        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<AuctionId>(), default))
            .ReturnsAsync((AuctionItem?)null);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.Equal(BuyNowResult.NotFound, result);
        _redisMock.Verify(r => r.IncrementAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Should_Increment_Redis_When_Auction_Is_Already_Sold()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var command = new BuyNowCommand(itemId, userId);

        var auction = new AuctionItem(new AuctionId(itemId), "test", new Stock(1));

        auction.TryReserve(); // Set stock sold out

        _redisMock.Setup(r => r.DecrementAsync(It.IsAny<string>()))
            .ReturnsAsync(1);

        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<AuctionId>(), default))
            .ReturnsAsync(auction);

        // Force auction to throw domain exception
        _repoMock.Setup(r => r.UpdateAsync(auction, default))
            .ThrowsAsync(new AuctionAlreadySoldException(new AuctionId(itemId)));

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.Equal(BuyNowResult.SoldOut, result);
        _redisMock.Verify(r => r.IncrementAsync(It.IsAny<string>()), Times.Once);
    }
}
