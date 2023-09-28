using MoviesAPI.Domain.Entities;
using MoviesAPI.Domain.Entities.TMDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Domain.Repositories
{
    public interface IManagersRepository
    {
        Task<IEnumerable<Movie>> GetUpcomingMovies(DateTime timePeriod, string? ageRating, string? genre);
        Task<IEnumerable<Movie>> GetBillboard(DateTime timePeriod, int numberOfScreens);
        Task<IEnumerable<RankedGenre>> GetRankedGenres(int? cityId = null);
        Task<string?> GetCityName(int cityId);
    }
}
