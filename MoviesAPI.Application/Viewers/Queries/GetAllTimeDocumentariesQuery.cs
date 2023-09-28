using MediatR;
using MoviesAPI.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Application.Viewers.Queries
{
    public class GetAllTimeDocumentariesQuery : IRequest<IEnumerable<RecommendationDto>>
    {
        public string[] Topics { get; set; }
    }
}
