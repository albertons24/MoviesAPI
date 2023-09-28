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
    public class GetAllTimeTVShowsQueryHandler : IRequestHandler<GetAllTimeTVShowsQuery, IEnumerable<TVShowRecommendationDto>>
    {
        private readonly IMapper _mapper;
        private readonly IViewersRepository _repository;

        public GetAllTimeTVShowsQueryHandler(IMapper mapper, IViewersRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<TVShowRecommendationDto>> Handle(GetAllTimeTVShowsQuery request, CancellationToken cancellationToken)
        {
            var movies = await _repository.GetAllTimeTVShows(request.Keywords, request.Genres);

            return _mapper.Map<IEnumerable<TVShowRecommendationDto>>(movies); ;
        }
    }
}
