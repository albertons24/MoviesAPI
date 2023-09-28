using MediatR;
using MoviesAPI.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Application.Managers.Queries
{
    public class GetIntelligentBillboardQuery : IRequest<IntelligentBillboardDto>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfBigRoomScreens { get; set; }
        public int NumberOfSmallRoomScreens { get; set; }
        public int? CityId { get; set; }
    }
}
