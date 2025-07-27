using AuctionBuyNow.Application.Items.DTOs;
using AuctionBuyNow.Domain.Repositiories;
using MediatR;

namespace AuctionBuyNow.Application.Items.Queries;

public class GetItemsHandler(IAuctionRepository repository) : IRequestHandler<GetItemsQuery, List<AuctionItemDto>>
{
    public async Task<List<AuctionItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetAllAsync();
        return items
            .Select(i => new AuctionItemDto(i.Id, i.Name, i.TotalStock, i.Reserved))
            .ToList();
    }
}