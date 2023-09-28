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
    public class GetAllTimeMovieRecommendationsHandler : IRequestHandler<GetAllTimeMovieRecommendationsQuery, IEnumerable<RecommendationDto>>
    {
        private readonly IMapper _mapper;
        private readonly IViewersRepository _repository;

        public GetAllTimeMovieRecommendationsHandler(IMapper mapper, IViewersRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<RecommendationDto>> Handle(GetAllTimeMovieRecommendationsQuery request, CancellationToken cancellationToken)
        {
            var movies = await _repository.GetAllTimeRecommendations(request.Keywords, request.Genres);

            return _mapper.Map<IEnumerable<RecommendationDto>>(movies); ;
        }
    }
}
