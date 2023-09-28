using AutoMapper;
using MediatR;
using MoviesAPI.Application.Common.DTOs;
using MoviesAPI.Application.Viewers.Queries;
using MoviesAPI.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Application.Viewers.QueryHandlers
{
    public class GetUpcomingMoviesQueryHandler : IRequestHandler<GetUpcomingMoviesQuery, IEnumerable<RecommendationDto>>
    {
        private readonly IMapper _mapper;
        private readonly IViewersRepository _repository;

        public GetUpcomingMoviesQueryHandler(IMapper mapper, IViewersRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<RecommendationDto>> Handle(GetUpcomingMoviesQuery request, CancellationToken cancellationToken)
        {
            var movies = await _repository.GetUpcomingMovies(request.Date, request.Keywords, request.Genres);

            return _mapper.Map<IEnumerable<RecommendationDto>>(movies); ;
        }
    }
}
