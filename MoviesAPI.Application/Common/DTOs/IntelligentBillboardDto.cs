using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Application.Common.DTOs
{
    public class IntelligentBillboardDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string[] PopularGenres { get; set; }
        public string? City { get; set; }
        public int NumberOfWeeks { get; set; }
        public int BigScreens { get; set; }
        public int SmallScreens { get; set; }
        public IEnumerable<WeeklyBillboard> Billboards { get; set; }
    }

    public class WeeklyBillboard
    {
        public RecommendationDto[] BigScreenMovies { get; set; }
        public RecommendationDto[] SmallScreenMovies { get; set; }
    }
}
