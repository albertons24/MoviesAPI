using MoviesAPI.Domain.Entities;
using MoviesAPI.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Tests.Data
{
    public static class TestDataSeeder
    {
        public static void Seed(AppDbContext context)
        {
            var cities = new[]
            {
                new City { Name = "New York", Population = 8175133 },
                new City { Name = "Los Angeles", Population = 3792621 },
                new City { Name = "London", Population = 8908081 },
                new City { Name = "Tokyo", Population = 13929286 }
            };

            var cinemas = new[]
            {
                new Cinema { Name = "Empire 25", OpenSince = new DateTime(2000, 1, 1), City = cities[0] },
                new Cinema { Name = "Regal LA Live", OpenSince = new DateTime(2010, 1, 1), City = cities[1] },
                new Cinema { Name = "Odeon Luxe", OpenSince = new DateTime(1990, 1, 1), City = cities[2] },
                new Cinema { Name = "TOHO Cinemas Shinjuku", OpenSince = new DateTime(1985, 1, 1), City = cities[3] }
            };

            var rooms = new[]
            {
                new Room { Name = "Main Hall", Size = "Large", Seats = 300, Cinema = cinemas[0] },
                new Room { Name = "Auditorium 1", Size = "Medium", Seats = 150, Cinema = cinemas[1] },
                new Room { Name = "Gallery", Size = "Small", Seats = 75, Cinema = cinemas[2] },
                new Room { Name = "Cinema 4", Size = "Medium", Seats = 200, Cinema = cinemas[3] }
            };

            var genres = new[]
            {
                new Genre { Name = "Action" },
                new Genre { Name = "Romance" },
                new Genre { Name = "Comedy" },
                new Genre { Name = "Drama" },
                new Genre { Name = "Sci-Fi" }
            };

            var movies = new[]
            {
                new Movie { OriginalTitle = "The Dark Knight", ReleaseDate = new DateTime(2008, 7, 18), OriginalLanguage = "English", Adult = false },
                new Movie { OriginalTitle = "La La Land", ReleaseDate = new DateTime(2016, 12, 9), OriginalLanguage = "English", Adult = false },
                new Movie { OriginalTitle = "The Grand Budapest Hotel", ReleaseDate = new DateTime(2014, 2, 26), OriginalLanguage = "English", Adult = false },
                new Movie { OriginalTitle = "Inception", ReleaseDate = new DateTime(2010, 7, 16), OriginalLanguage = "English", Adult = false },
                new Movie { OriginalTitle = "Blade Runner 2049", ReleaseDate = new DateTime(2017, 10, 6), OriginalLanguage = "English", Adult = false }
            };

            var movieGenres = new[]
            {
                new MovieGenre { Movie = movies[0], Genre = genres[0] },  // The Dark Knight - Action
                new MovieGenre { Movie = movies[1], Genre = genres[1] },  // La La Land - Romance
                new MovieGenre { Movie = movies[2], Genre = genres[2] },  // The Grand Budapest Hotel - Comedy
                new MovieGenre { Movie = movies[3], Genre = genres[4] },  // Inception - Sci-Fi
                new MovieGenre { Movie = movies[4], Genre = genres[0] },  // Blade Runner 2049 - Action
                new MovieGenre { Movie = movies[4], Genre = genres[4] }   // Blade Runner 2049 - Sci-Fi
            };

            var sessions = new[]
            {
                new Session { Room = rooms[0], Movie = movies[0], StartTime = new DateTime(2023, 1, 1, 20, 0, 0), EndTime = new DateTime(2023, 1, 1, 22, 30, 0), SeatsSold = 250 },
                new Session { Room = rooms[1], Movie = movies[1], StartTime = new DateTime(2023, 1, 2, 19, 0, 0), EndTime = new DateTime(2023, 1, 2, 21, 30, 0), SeatsSold = 120 },
                new Session { Room = rooms[2], Movie = movies[2], StartTime = new DateTime(2023, 1, 3, 18, 0, 0), EndTime = new DateTime(2023, 1, 3, 20, 0, 0), SeatsSold = 70 },
                new Session { Room = rooms[3], Movie = movies[3], StartTime = new DateTime(2023, 1, 4, 21, 0, 0), EndTime = new DateTime(2023, 1, 4, 23, 30, 0), SeatsSold = 180 },
                new Session { Room = rooms[0], Movie = movies[4], StartTime = new DateTime(2023, 1, 5, 19, 30, 0), EndTime = new DateTime(2023, 1, 5, 22, 0, 0), SeatsSold = 210 }
            };

            context.Cities.AddRange(cities);
            context.Cinemas.AddRange(cinemas);
            context.Rooms.AddRange(rooms);
            context.Genres.AddRange(genres);
            context.Movies.AddRange(movies);
            context.MovieGenres.AddRange(movieGenres);
            context.Sessions.AddRange(sessions);

            context.SaveChanges();
        }
    }
}
