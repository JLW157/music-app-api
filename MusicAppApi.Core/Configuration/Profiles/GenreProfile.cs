using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.DTO_s;
using MusicAppApi.Models.DTO_s.Genres;

namespace MusicAppApi.Core.Configuration.Profiles
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<Genre, GenreResponse>()
                .ForMember(dest => dest.Genre,
                    (opt) =>
                        opt.MapFrom(src => src.TypeOfGenre.ToString()));

            CreateMap<GenreResponse, Genre>()
                .ForMember(dest => dest.TypeOfGenre,
                    opt =>
                        opt.MapFrom(src => Enum.Parse<Genres>(src.Genre)));

        }
    }
}
