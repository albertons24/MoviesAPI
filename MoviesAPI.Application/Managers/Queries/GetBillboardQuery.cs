using MediatR;
using MoviesAPI.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Application.Managers.Queries
{
    public class GetBillboardQuery : IRequest<IEnumerable<RecommendationDto>>
    {
        public DateTime TimePeriod { get; set; }
        public int NumberOfScreens { get; set; }
    }
}
