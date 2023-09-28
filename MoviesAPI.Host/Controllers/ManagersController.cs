using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Application.Common.DTOs;
using MoviesAPI.Application.Managers.Queries;

namespace MoviesAPI.Host.Controllers
{
    [ApiController]
    [Route("recommendations/managers")]
    public class ManagersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ManagersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("movies/upcoming")]
        public async Task<ActionResult<IEnumerable<RecommendationDto>>> GetUpcomingMovies(DateTime timePeriodFromNow, string? ageRating = null, string? genre = null)
        {
            var query = new GetUpcomingMoviesQuery
            {
                TimePeriod = timePeriodFromNow,
                AgeRating = ageRating,
                Genre = genre
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("billboard")]
        public async Task<ActionResult<IEnumerable<RecommendationDto>>> GetBillboard(DateTime timePeriod, int numberOfScreens)
        {
            var query = new GetBillboardQuery
            {
                TimePeriod = timePeriod,
                NumberOfScreens = numberOfScreens
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("intelligent-billboard")]
        public async Task<ActionResult<IntelligentBillboardDto>> GetIntelligentBillboard(DateTime startDate, 
            DateTime endDate, 
            int numberOfBigRoomScreens, 
            int numberOfSmallRoomScreens,
            int? cityId = null)
        {
            var query = new GetIntelligentBillboardQuery
            {
                StartDate = startDate,
                EndDate = endDate,
                NumberOfBigRoomScreens = numberOfBigRoomScreens,
                NumberOfSmallRoomScreens = numberOfSmallRoomScreens,
                CityId = cityId
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
