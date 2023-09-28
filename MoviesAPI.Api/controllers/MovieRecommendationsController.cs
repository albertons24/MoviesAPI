using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[ApiController]
[Route("api/recommendations/movies")]
public class MovieRecommendationsController : ControllerBase
{
    private readonly MediatR.IMediator _mediator;

    public MovieRecommendationsController(MediatR.IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("all-time")]
    public async Task<IActionResult> GetAllTimeRecommendations([FromQuery] string[] keywords, [FromQuery] string[] genres)
    {
        var query = new GetAllTimeMovieRecommendationsQuery
        {
            Keywords = keywords,
            Genres = genres
        };

        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
