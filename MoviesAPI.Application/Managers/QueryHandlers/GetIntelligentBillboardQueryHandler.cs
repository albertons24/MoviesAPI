using AutoMapper;
using MediatR;
using MoviesAPI.Application.Common.DTOs;
using MoviesAPI.Application.Common.Exceptions;
using MoviesAPI.Application.Managers.Queries;
using MoviesAPI.Domain.Dictionaries;
using MoviesAPI.Domain.Entities;
using MoviesAPI.Domain.Entities.TMDB;
using MoviesAPI.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Application.Managers.QueryHandlers
{
    public class GetIntelligentBillboardQueryHandler : IRequestHandler<GetIntelligentBillboardQuery, IntelligentBillboardDto>
    {
        private readonly IMapper _mapper;
        private readonly IManagersRepository _managersRepository;
        private readonly ITMDBMovieRepository _tmdbRepository;

        public GetIntelligentBillboardQueryHandler(IMapper mapper, IManagersRepository managersRepository, ITMDBMovieRepository tmdbRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _managersRepository = managersRepository ?? throw new ArgumentNullException(nameof(managersRepository));
            _tmdbRepository = tmdbRepository ?? throw new ArgumentNullException(nameof(tmdbRepository));
        }

        public async Task<IntelligentBillboardDto> Handle(GetIntelligentBillboardQuery request, CancellationToken cancellationToken)
        {
            var numberOfWeeks = GetNumberOfWeeks(request.StartDate, request.EndDate);

            var allGenres = await _managersRepository.GetRankedGenres(request.CityId);

            if(request.CityId != null && allGenres.Any() && allGenres.Average(g2 => g2.SeatsSold) == 0)
                allGenres = await _managersRepository.GetRankedGenres();

            var popularGenres = allGenres.Any() && allGenres.Average(g2 => g2.SeatsSold) != 0 ?
                allGenres.Where(g => g.SeatsSold >= allGenres.Average(g2 => g2.SeatsSold))
                : new List<RankedGenre>();
                

            var result = new IntelligentBillboardDto
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                BigScreens = request.NumberOfBigRoomScreens,
                SmallScreens = request.NumberOfSmallRoomScreens,
                NumberOfWeeks = numberOfWeeks,
                PopularGenres = popularGenres.Select(g => g.GenreName).ToArray()
            };

            var billboards = new List<WeeklyBillboard>();

            if(request.CityId != null)
            {
                result.City = await _managersRepository.GetCityName(request.CityId.Value);

                if (result.City == null)
                    throw new NotFoundException($"City with ID {request.CityId.Value} not found.");
            }

            var popularGenresTMDBIds = popularGenres.Select(g => TMDBGenres.GetTMDBGenreId(g.GenreId)).Where(g => g.HasValue).Select(g => g.Value);

            var bigScreenRecomendations = await GetMovieRecommendationList(request.NumberOfBigRoomScreens, numberOfWeeks, allGenres, popularGenresTMDBIds, request.StartDate);
            var smallScreenRecomendations = await GetMovieRecommendationList(request.NumberOfSmallRoomScreens, numberOfWeeks, allGenres, popularGenresTMDBIds, request.StartDate, false);

            if(!ValidateDuplicatesAndCoincidences(bigScreenRecomendations, smallScreenRecomendations))
                throw new BillboardCreationException("Duplicate movies found");
           

            for (var i = 0; i< numberOfWeeks; i++){
                billboards.Add(new WeeklyBillboard
                {
                    BigScreenMovies = bigScreenRecomendations.Skip(i * request.NumberOfBigRoomScreens).Take(request.NumberOfBigRoomScreens).ToArray(),
                    SmallScreenMovies = smallScreenRecomendations.Skip(i * request.NumberOfSmallRoomScreens).Take(request.NumberOfSmallRoomScreens).ToArray()
                });
            }

            result.Billboards = billboards;
            return result;
        }

        private async Task<IEnumerable<RecommendationDto>> GetMovieRecommendationList(
            int numberOfScreens,
            int numberOfWeeks,
            IEnumerable<RankedGenre> genres,
            IEnumerable<int> popularGenres,
            DateTime startDate,
            bool bigScreen = true)
        {
            IEnumerable<int>? include = null;
            IEnumerable<int>? exclude = null;


            if (bigScreen)
                include = popularGenres;
            else
                exclude = popularGenres;

            var movies = new List<TMDBMovie>();

            IEnumerable<TMDBMovie> discoveryMovies = new List<TMDBMovie>();
            for (var i = 1; movies.Count < numberOfScreens * numberOfWeeks; i++)
            {
                if (movies.Count > numberOfScreens * i)
                    startDate = startDate.AddDays(7);

                discoveryMovies = await
                    _tmdbRepository.GetDiscoverMovies(
                            page: i,
                            genres: include,
                            excludedGenres: exclude,
                            endDate: startDate, // Limit the release date of the movies to the beginning of the week
                            popularityDesc: (popularGenres.Any() || bigScreen)
                        );

                if(!discoveryMovies.Any())
                    throw new BillboardCreationException("TMDB API returned no results for the specified criteria");

                movies.AddRange(discoveryMovies);
            }

            foreach(var movie in movies)
            {
                movie.RankedGenres = genres.Where(g => movie.GenreIds.Any(gi => TMDBGenres.GetGenreId(gi) == g.GenreId));
            }


            return _mapper.Map<IEnumerable<RecommendationDto>>(movies);
        }

        private int GetNumberOfWeeks(DateTime startDate, DateTime endDate)
        {
            int daysDifference = (endDate - startDate).Days;

            return (int)Math.Ceiling(daysDifference / 7.0);
        }

        public bool ValidateDuplicatesAndCoincidences(IEnumerable<RecommendationDto> bigScreenMovies, IEnumerable<RecommendationDto> smallScreenMovies)
        {
            if (bigScreenMovies.GroupBy(x => x.Id).Any(g => g.Count() > 1))
                return false;

            if (smallScreenMovies.GroupBy(x => x.Id).Any(g => g.Count() > 1))
                return false;

            if (bigScreenMovies.Select(x => x.Id).Intersect(smallScreenMovies.Select(y => y.Id)).Any())
                return false;

            return true;
        }
    }
}
