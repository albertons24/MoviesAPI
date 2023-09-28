using MoviesAPI.Domain.Entities.TMDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Domain.Repositories
{
    public interface ITMDBMovieRepository
    {
        Task<IEnumerable<TMDBMovie>> GetDiscoverMovies(
            int? page = null, 
            IEnumerable<int>? genres = null,
            IEnumerable<int>? excludedGenres = null,
            DateTime? endDate = null,
            bool popularityDesc = false);
    }
}
