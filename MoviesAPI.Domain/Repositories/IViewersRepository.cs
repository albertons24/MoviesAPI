using MoviesAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Domain.Repositories
{
    public interface IViewersRepository
    {
        Task<IEnumerable<Movie>> GetAllTimeRecommendations(string[] keywords, string[] genres);
        Task<IEnumerable<Movie>> GetUpcomingMovies(DateTime date, string[] keywords, string[] genres);
        Task<IEnumerable<TVShow>> GetAllTimeTVShows(string[] keywords, string[] genres);
        Task<IEnumerable<Documentary>> GetAllTimeDocumentaries(string[] topics);
    }
}
