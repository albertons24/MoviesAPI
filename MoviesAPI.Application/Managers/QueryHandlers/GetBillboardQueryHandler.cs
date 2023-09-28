using AutoMapper;
using MediatR;
using MoviesAPI.Application.Common.DTOs;
using MoviesAPI.Application.Managers.Queries;
using MoviesAPI.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Application.Managers.QueryHandlers
{
    public class GetBillboardQueryHandler : IRequestHandler<GetBillboardQuery, IEnumerable<RecommendationDto>>
    {
        private readonly IMapper _mapper;
        private readonly IManagersRepository _repository;

        public GetBillboardQueryHandler(IMapper mapper, IManagersRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<RecommendationDto>> Handle(GetBillboardQuery request, CancellationToken cancellationToken)
        {

            var movies = await _repository.GetBillboard(
                request.TimePeriod,
                request.NumberOfScreens
            );


            return _mapper.Map<IEnumerable<RecommendationDto>>(movies);
        }
    }
}
