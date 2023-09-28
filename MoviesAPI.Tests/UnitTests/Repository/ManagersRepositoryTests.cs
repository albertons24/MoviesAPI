using MoviesAPI.Infrastructure.Data;
using MoviesAPI.Infrastructure.Repositories;
using MoviesAPI.Tests.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Tests.UnitTests.Repository
{
    public class ManagersRepositoryTests : IClassFixture<DbContextFixture>
    {
        private readonly AppDbContext _context;

        public ManagersRepositoryTests(DbContextFixture fixture)
        {
            _context = fixture.DbContext;
        }


        [Fact]
        public async void GetRankedGenres_ReturnsData()
        {
            var repository = new ManagersRepository(_context);
            var result = await repository.GetRankedGenres();
            Assert.NotNull(result);
            Assert.True(result.Any());
        }

        [Fact]
        public async void GetRankedGenres_CorrectRankingBySeatsSold()
        {
            var repository = new ManagersRepository(_context);
            var result = await repository.GetRankedGenres();
            var firstGenre = result.First();
            var lastGenre = result.Last();
            Assert.True(firstGenre.SeatsSold >= lastGenre.SeatsSold);
        }

        [Fact]
        public async void GetRankedGenres_FilterByCity()
        {
            var cityId = _context.Cities.First(c => c.Name == "New York").Id;
            var repository = new ManagersRepository(_context);
            var result = await repository.GetRankedGenres(cityId);
            Assert.NotNull(result);
            Assert.True(result.Any());
        }

        [Fact]
        public async void GetCityName_ReturnsCorrectCityName()
        {
            var testCityId = _context.Cities.First().Id;
            var repository = new ManagersRepository(_context);
            var result = await repository.GetCityName(testCityId);
            Assert.NotNull(result);
            Assert.Equal("New York", result);
        }

        [Fact]
        public async void GetCityName_InvalidCityIdReturnsNull()
        {
            var testCityId = 99999; // Some invalid ID
            var repository = new ManagersRepository(_context);
            var result = await repository.GetCityName(testCityId);
            Assert.Null(result);
        }

        [Fact]
        public async void GetRankedGenres_ReturnsAllGenresEvenIfNoSessions()
        {
            var repository = new ManagersRepository(_context);
            var result = await repository.GetRankedGenres();
            var allGenres = _context.Genres.ToList();
            Assert.True(result.Count() == allGenres.Count);
        }

        [Fact]
        public async void GetRankedGenres_NoDoubleCountingGenres()
        {
            var repository = new ManagersRepository(_context);
            var result = await repository.GetRankedGenres();
            var distinctGenresCount = result.Select(r => r.GenreId).Distinct().Count();
            Assert.Equal(result.Count(), distinctGenresCount);
        }
    }
}
