using MediatR;

namespace AuctionBuyNow.Application.Items.Queries;

using AuctionBuyNow.Application.Items.DTOs;

public record GetItemsQuery : IRequest<List<AuctionItemDto>>;
