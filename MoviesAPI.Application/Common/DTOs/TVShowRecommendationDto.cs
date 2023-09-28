using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Application.Common.DTOs
{
    public class TVShowRecommendationDto : RecommendationDto
    {

        public int NumberOfSeasons { get; set; }
        public int NumberOfEpisodes { get; set; }
        public bool Concluded { get; set; }

    }
}
