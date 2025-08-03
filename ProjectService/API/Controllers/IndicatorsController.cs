using Application.Queries;
using Application.Queries.GetMostPopularIndicatorsQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PopularIndicatorsController(IMediator mediator)
    : ControllerBase
{
    [HttpGet("{subscriptionType}")]
    public async Task<IActionResult> GetPopularIndicators(string subscriptionType)
    {
        var result = await mediator.Send(new GetPopularIndicatorsQuery(subscriptionType));
        return Ok(new { indicators = result.Select(r => new { r.Name, r.UsedCount }) });
    }
}