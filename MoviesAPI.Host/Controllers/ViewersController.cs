using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Application.Common.DTOs;
using MoviesAPI.Application.Viewers.Queries;

namespace MoviesAPI.Host.Controllers
{
    [ApiController]
    [Route("recomendations/viewers")]
    public class ViewersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ViewersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("movies/all-time")]
        public async Task<ActionResult<IEnumerable<RecommendationDto>>> GetAllTimeMovies([FromQuery] string[] keywords, [FromQuery] string[] genres)
        {
            var query = new GetAllTimeMovieRecommendationsQuery
            {
                Keywords = keywords,
                Genres = genres
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("movies/upcoming")]
        public async Task<ActionResult<IEnumerable<RecommendationDto>>> GetUpcomingMovies(DateTime timePeriodFromNow, [FromQuery] string[] keywords, [FromQuery] string[] genres)
        {
            var query = new GetUpcomingMoviesQuery
            {
                Date = timePeriodFromNow,
                Keywords = keywords,
                Genres = genres
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("tv-shows/all-time")]
        public async Task<ActionResult<IEnumerable<TVShowRecommendationDto>>> GetAllTimeTvShows([FromQuery] string[] keywords, [FromQuery] string[] genres)
        {
            var query = new GetAllTimeTVShowsQuery
            {
                Keywords = keywords,
                Genres = genres
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("documentaries/all-time")]
        public async Task<ActionResult<IEnumerable<RecommendationDto>>> GetAllTimeDocumentaries([FromQuery] string[] topics)
        {
            var query = new GetAllTimeDocumentariesQuery
            {
                Topics = topics
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
