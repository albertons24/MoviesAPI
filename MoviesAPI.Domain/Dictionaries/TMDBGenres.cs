using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Domain.Dictionaries
{
    public abstract class TMDBGenres
    {
        private static readonly Dictionary<int, int> genreIds = new Dictionary<int, int>
        {
            {1, 28},     // Action
            {2, 12},     // Adventure
            {3, 16},     // Animation
            {4, 35},     // Comedy
            {5, 80},     // Crime
            {6, 99},     // Documentary
            {7, 18},     // Drama
            {8, 10751},  // Family
            {9, 14},     // Fantasy
            {10, 36},    // History
            {11, 27},    // Horror
            {12, 10402}, // Music
            {13, 9648},  // Mystery
            {14, 10749}, // Romance
            {15, 878},   // Science Fiction
            {16, 10770}, // TV Movie
            {17, 53},    // Thriller
            {18, 10752}, // War
            {19, 37}     // Western
        };

        public static int? GetTMDBGenreId(int localId)
        {
            if (genreIds.TryGetValue(localId, out var externalId))
            {
                return externalId;
            }
            return null;
        }

        public static int? GetGenreId(int externalId)
        {
            var localId = genreIds.FirstOrDefault(kvp => kvp.Value == externalId).Key;
            return localId == 0 ? (int?)null : localId;
        }
    }
}
