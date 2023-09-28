using MoviesAPI.Domain.Entities;
using MoviesAPI.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Infrastructure.Repositories
{
    public class ViewersRepository : IViewersRepository
    {
        public Task<IEnumerable<Documentary>> GetAllTimeDocumentaries(string[] topics)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Movie>> GetAllTimeRecommendations(string[] keywords, string[] genres)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TVShow>> GetAllTimeTVShows(string[] keywords, string[] genres)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Movie>> GetUpcomingMovies(DateTime date, string[] keywords, string[] genres)
        {
            throw new NotImplementedException();
        }
    }
}
