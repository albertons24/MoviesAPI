using Microsoft.EntityFrameworkCore;
using MoviesAPI.Application.Common.Interfaces;
using MoviesAPI.Domain.Entities;
using MoviesAPI.Domain.Entities.TMDB;
using MoviesAPI.Domain.Repositories;
using MoviesAPI.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Infrastructure.Repositories
{
    public class ManagersRepository : IManagersRepository
    {
        private readonly AppDbContext _context;

        public ManagersRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Movie>> GetUpcomingMovies(DateTime timePeriod, string? ageRating, string? genre)
        {

            throw new NotImplementedException();
        }

        public Task<IEnumerable<Movie>> GetBillboard(DateTime timePeriod, int numberOfScreens)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RankedGenre>> GetRankedGenres(int? cityId = null)
        {
            return await (from g in _context.Genres
                        join mg in _context.MovieGenres on g.Id equals mg.GenreId into tempMG
                        from mgLeft in tempMG.DefaultIfEmpty()

                        join s in _context.Sessions on mgLeft.MovieId equals s.MovieId into tempS
                        from sLeft in tempS.DefaultIfEmpty()

                        join r in _context.Rooms on sLeft.RoomId equals r.Id into tempR
                        from rLeft in tempR.DefaultIfEmpty()

                        join c in _context.Cinemas on rLeft.CinemaId equals c.Id into tempC
                        from cLeft in tempC.DefaultIfEmpty()

                        where cityId == null || cLeft == null || cLeft.CityId == cityId

                        group sLeft.SeatsSold by new { g.Id, g.Name } into groupedGenres
                        
                        select new RankedGenre
                        {
                            GenreId = groupedGenres.Key.Id,
                            GenreName = groupedGenres.Key.Name,
                            SeatsSold = groupedGenres.Sum() ?? 0
                        }).ToListAsync();
        }

        public async Task<string?> GetCityName(int cityId)
        {
            return await _context.Cities.Where(c => c.Id == cityId).Select(c => c.Name).FirstOrDefaultAsync();
        }
    }
}
