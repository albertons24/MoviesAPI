using MediatR;
using MoviesAPI.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Application.Viewers.Queries
{
    public class GetAllTimeTVShowsQuery : IRequest<IEnumerable<TVShowRecommendationDto>>
    {
        public string[] Keywords { get; set; }
        public string[] Genres { get; set; }
    }
}
