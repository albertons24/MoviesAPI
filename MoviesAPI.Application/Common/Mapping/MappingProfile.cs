using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Application.Common.Mapping
{
    namespace MoviesAPI.Application.Mapping
    {
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Domain.Entities.Movie, DTOs.RecommendationDto>()
                    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.OriginalTitle))
                    .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.OriginalLanguage))
                    .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate))
                    .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.MovieGenres.Select(g => g.Genre.Name).ToArray()));

                CreateMap<Domain.Entities.TMDB.TMDBMovie, DTOs.RecommendationDto>()
                    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                    .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.OriginalLanguage))
                    .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate))
                    .ForMember(dest => dest.WebSite, opt => opt.MapFrom(src => $"https://www.themoviedb.org/movie/{src.Id}"))
                    .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.RankedGenres.Any() ?
                        src.RankedGenres.OrderByDescending(g => g.SeatsSold).First().GenreName : null))
                    .ForMember(dest => dest.Keywords, opt => opt.MapFrom(src => src.RankedGenres.Select(rg => rg.GenreName).ToArray()))
                    .ForMember(dest => dest.Overview, opt => opt.MapFrom(src => src.Overview));
            }
        }
    }

}
