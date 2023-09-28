using MoviesAPI.Application.Common.Interfaces;
using MoviesAPI.Domain.Entities.TMDB;
using MoviesAPI.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Infrastructure.Repositories
{
    public class TMDBMovieRepository : ITMDBMovieRepository
    {
        private readonly IHttpClientService _httpClientService;

        public TMDBMovieRepository(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public async Task<IEnumerable<TMDBMovie>> GetDiscoverMovies(
            int? page = null, 
            IEnumerable<int>? genres = null,
            IEnumerable<int>? excludedGenres = null,
            DateTime? endDate = null,
            bool popularityDesc = true
            )
        {
            var url = $"?include_adult=false&include_video=false&language=en-US&page={page ?? 1}&sort_by=popularity.";

            url += popularityDesc ? "desc" : "asc";

            if(genres != null && genres.Any())
            {
                url += "&with_genres=" + string.Join("|", genres);
            }

            if (excludedGenres != null && excludedGenres.Any())
            {
                url += "&without_genres=" + string.Join("|", excludedGenres);
            }

            if(endDate != null)
            {
                url += $"&release_date.lte={endDate.Value.ToString("yyyy-MM-dd")}";
            }

            return (await _httpClientService.GetAsync<TMDBMovieResponse>(url)).Results;
        }
    }
}
