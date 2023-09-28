using MediatR;
using MoviesAPI.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Application.Viewers.Queries
{
    public class GetAllTimeMovieRecommendationsQuery : IRequest<IEnumerable<RecommendationDto>>
    {
        public string[] Keywords { get; set; }
        public string[] Genres { get; set; }
    }
}
