using AuctionBuyNow.Application.Common.Services;
using AuctionBuyNow.Application.Items.Commands;
using AuctionBuyNow.Domain.Repositiories;
using Moq;

namespace AuctionBuyNow.UnitTests
{
    public class BuyNowHandlerTests
    {
        private readonly Mock<IAuctionRepository> _repoMock;
        private readonly Mock<IRedisService> _redisMock;
        private readonly BuyNowHandler _handler;

        public BuyNowHandlerTests()
        {
            _repoMock = new Mock<IAuctionRepository>();
            _redisMock = new Mock<IRedisService>();

            _handler = new BuyNowHandler(_repoMock.Object, _redisMock.Object);
        }

        [Fact]
        public async Task Should_Return_Success_When_Item_Is_Reservable()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var command = new BuyNowCommand(itemId, userId);

            _redisMock.Setup(r => r.DecrementAsync(It.IsAny<string>()))
                .ReturnsAsync(10);

            _repoMock.Setup(r => r.ReserveAsync(itemId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.Equal(BuyNowResult.Success, result);
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
            _repoMock.Verify(r => r.ReserveAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task Should_Increment_Redis_When_Db_Fails()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var command = new BuyNowCommand(itemId, userId);

            _redisMock.Setup(r => r.DecrementAsync(It.IsAny<string>()))
                .ReturnsAsync(5);

            _repoMock.Setup(r => r.ReserveAsync(itemId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.Equal(BuyNowResult.SoldOut, result);
            _redisMock.Verify(r => r.IncrementAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
