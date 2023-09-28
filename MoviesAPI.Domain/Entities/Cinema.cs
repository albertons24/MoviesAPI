using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Domain.Entities
{
    [Table("Cinema")]
    public class Cinema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime OpenSince { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
