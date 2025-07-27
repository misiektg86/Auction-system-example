using AuctionBuyNow.Application.Items.Commands;
using AuctionBuyNow.Application.Items.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuctionBuyNow.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuctionController(IMediator mediator) : ControllerBase
{
    [HttpGet("items")]
    public async Task<IActionResult> GetItems()
    {
        var result = await mediator.Send(new GetItemsQuery());
        return Ok(result);
    }

    [HttpPost("buy-now")]
    public async Task<IActionResult> BuyNow([FromBody] BuyNowCommand command)
    {
        var result = await mediator.Send(command);
        return result switch
        {
            BuyNowResult.Success => Ok("OK"),
            BuyNowResult.SoldOut => BadRequest("Sold out"),
            _ => StatusCode(500)
        };
    }
}