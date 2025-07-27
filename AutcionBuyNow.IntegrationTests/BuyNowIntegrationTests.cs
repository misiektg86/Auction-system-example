using System.Net.Http.Json;
using AuctionBuyNow.Application.Items.Commands;
using AuctionBuyNow.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AutcionBuyNow.IntegrationTests
{
    public class BuyNowTests(IntegrationTestContainerFixture fixture) : IClassFixture<IntegrationTestContainerFixture>
    {
        private readonly HttpClient _client = fixture.Client;
        private readonly Guid _testItemId = fixture.GetTestItemId();


        [Fact]
        public async Task Should_Buy_Item_When_Available()
        {
            // Arrange
            var request = new BuyNowCommand(
                _testItemId,
                Guid.NewGuid()
            );

            // Act
            var response = await _client.PostAsJsonAsync("/api/auction/buy-now", request);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.True(response.IsSuccessStatusCode, $"Failed: {content}");
        }

        [Fact]
        public async Task Should_Return_Items()
        {
            // Act
            var response = await _client.GetAsync("/api/auction/items");

            // Assert
            response.EnsureSuccessStatusCode();
            var items = await response.Content.ReadFromJsonAsync<List<object>>();
            Assert.NotNull(items);
            Assert.NotEmpty(items);
        }
    }
}
