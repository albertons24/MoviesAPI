using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Domain.Entities
{
    public class RankedGenre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public int SeatsSold { get; set; }
    }
}
