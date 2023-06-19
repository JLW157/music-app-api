using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.DTO_s.Genres;
using MusicAppApi.Models.DTO_s.Sets;

namespace MusicAppApi.Core.Configuration.Profiles
{
    public class SetsProfile : Profile
    {
        public SetsProfile()
        {
            CreateMap<Set, SetDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PosterUrl,
                    opt => opt.MapFrom(src => src.PosterUrl))
                .ForMember(dest => dest.User,
                    opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Audios,
                    opt => opt.MapFrom(src => src.Audios));
        }
    }
}
