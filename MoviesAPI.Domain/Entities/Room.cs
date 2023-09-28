using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Domain.Entities
{
    [Table("Room")]
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public int Seats { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }

}
