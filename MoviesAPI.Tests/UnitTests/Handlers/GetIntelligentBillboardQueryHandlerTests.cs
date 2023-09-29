using AutoMapper;
using Moq;
using MoviesAPI.Application.Common.DTOs;
using MoviesAPI.Application.Common.Exceptions;
using MoviesAPI.Application.Managers.Queries;
using MoviesAPI.Application.Managers.QueryHandlers;
using MoviesAPI.Domain.Entities;
using MoviesAPI.Domain.Entities.TMDB;
using MoviesAPI.Domain.Repositories;
using MoviesAPI.Tests.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Tests.UnitTests.Handlers
{
    public class GetIntelligentBillboardQueryHandlerTests
    {
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();
        private readonly Mock<IManagersRepository> _mockManagersRepo = new Mock<IManagersRepository>();
        private readonly Mock<ITMDBMovieRepository> _mockTMDBRepo = new Mock<ITMDBMovieRepository>();

        private GetIntelligentBillboardQueryHandler CreateHandler()
        {
            return new GetIntelligentBillboardQueryHandler(_mockMapper.Object, _mockManagersRepo.Object, _mockTMDBRepo.Object);
        }

        [Fact]
        public async void Handle_ReturnsCorrectNumberOfWeeks()
        {
            var handler = CreateHandler();
            var query = new GetIntelligentBillboardQuery
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(13)
            };

            var result = await handler.Handle(query, new CancellationToken());

            Assert.Equal(2, result.NumberOfWeeks);
        }

        [Fact]
        public async void Handle_PopularGenresAreCorrect()
        {
            var rankedGenres = new List<RankedGenre>
            {
                new RankedGenre { SeatsSold = 10, GenreName = "Action", GenreId = 1},
                new RankedGenre { SeatsSold = 5, GenreName = "Romance", GenreId = 14},
                new RankedGenre { SeatsSold = 15, GenreName = "Adventure", GenreId = 2}
            };

            _mockManagersRepo.Setup(r => r.GetRankedGenres(null)).ReturnsAsync(rankedGenres);

            var handler = new GetIntelligentBillboardQueryHandler(_mockMapper.Object, _mockManagersRepo.Object, _mockTMDBRepo.Object);
            var query = new GetIntelligentBillboardQuery();

            var result = await handler.Handle(query, default);

            var expectedGenres = new[] { "Action", "Adventure" };
            Assert.True(result.PopularGenres.SequenceEqual(expectedGenres));
        }

        [Fact]
        public async void Handle_ReturnsEmptyGenres_WhenAverageSeatsSoldIsZero()
        {
            var rankedGenres = new List<RankedGenre>
            {
                new RankedGenre { SeatsSold = 0, GenreName = "Action", GenreId = 1},
                new RankedGenre { SeatsSold = 0, GenreName = "Romance", GenreId = 2}
            };

            _mockManagersRepo.Setup(r => r.GetRankedGenres(null)).ReturnsAsync(rankedGenres);

            var handler = CreateHandler();
            var query = new GetIntelligentBillboardQuery();

            var result = await handler.Handle(query, default);

            Assert.Empty(result.PopularGenres);
        }

        [Fact]
        public async void Handle_IncludesCityInformation_WhenCityIdIsProvided()
        {
            _mockManagersRepo.Setup(r => r.GetCityName(It.IsAny<int>())).ReturnsAsync("Sample City");

            var handler = CreateHandler();
            var query = new GetIntelligentBillboardQuery { CityId = 1 };

            var result = await handler.Handle(query, new CancellationToken());

            Assert.Equal("Sample City", result.City);
        }


        [Fact]
        public async void Handle_ThrowsNotFoundException_WhenCityIsNotFound()
        {
            _mockManagersRepo.Setup(r => r.GetCityName(It.IsAny<int>())).ReturnsAsync((string)null);

            var handler = CreateHandler();
            var query = new GetIntelligentBillboardQuery { CityId = 1 };

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, new CancellationToken()));
        }

        [Fact]
        public async void Handle_ReturnsEmptyPopularGenres_WhenNoGenresAreAvailable()
        {
            _mockManagersRepo.Setup(r => r.GetRankedGenres(null)).ReturnsAsync(new List<RankedGenre>());

            var handler = CreateHandler();
            var query = new GetIntelligentBillboardQuery();

            var result = await handler.Handle(query, default);

            Assert.Empty(result.PopularGenres);
        }

        [Fact]
        public async void Handle_ThrowsBillboardCreationError_WhenTMDBReturnsEmptyList()
        {
            _mockTMDBRepo.Setup(r => r.GetDiscoverMovies(It.IsAny<int>(), It.IsAny<IEnumerable<int>>(), It.IsAny<IEnumerable<int>>(), It.IsAny<DateTime>(), It.IsAny<bool>()))
                        .ReturnsAsync(new List<TMDBMovie>());

            var handler = CreateHandler();
            var query = new GetIntelligentBillboardQuery
            {
                NumberOfBigRoomScreens = 1,
                NumberOfSmallRoomScreens = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7)
            };

            await Assert.ThrowsAsync<BillboardCreationException>(() => handler.Handle(query, default));
        }

        [Fact]
        public async void Handle_ThrowsBillboardCreationError_WhenDuplicatesFoundBetweenBigAndSmallScreens()
        {
            _mockTMDBRepo.Setup(r => r.GetDiscoverMovies(It.IsAny<int>(), It.IsAny<IEnumerable<int>>(), It.IsAny<IEnumerable<int>>(), It.IsAny<DateTime>(), true))
                        .ReturnsAsync(new List<TMDBMovie> { new TMDBMovie { Id = 1 } });

            _mockTMDBRepo.Setup(r => r.GetDiscoverMovies(It.IsAny<int>(), It.IsAny<IEnumerable<int>>(), It.IsAny<IEnumerable<int>>(), It.IsAny<DateTime>(), false))
                        .ReturnsAsync(new List<TMDBMovie> { new TMDBMovie { Id = 1 } });

            _mockMapper.Setup(m => m.Map<IEnumerable<RecommendationDto>>(It.IsAny<IEnumerable<TMDBMovie>>()))
                .Returns((IEnumerable<TMDBMovie> sourceMovies) =>
                {
                    return sourceMovies.Select(movie => new RecommendationDto
                    {
                        Id = movie.Id,
                    }).ToList();
                });

            var handler = CreateHandler();
            var query = new GetIntelligentBillboardQuery
            {
                NumberOfBigRoomScreens = 1,
                NumberOfSmallRoomScreens = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7)
            };

            await Assert.ThrowsAsync<BillboardCreationException>(() => handler.Handle(query, default));
        }
    }
}
