using AutoMapper;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Core.Configuration.Profiles
{
    public class AudioProfile : Profile
    {
        public AudioProfile()
        {
            CreateMap<Audio, AudioResponse>()
                .ForMember(dest => dest.PlayedCount,
                    (opt) => opt.MapFrom(src => src.PlayedCount))
                 .ForMember(dest => dest.Id,
                 (opt) => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.Name,
                 (opt) => opt.MapFrom(src => src.Name))
                  .ForMember(dest => dest.Genre,
                 (opt) => opt.MapFrom(src => src.Genre.TypeOfGenre.ToString()))
                  .ForMember(dest => dest.Artists,
                  (opt) => opt.MapFrom(src => src.Artists.Select(x => x.UserName).ToList()))
                  .ForMember(dest => dest.AudioUrl,
                  (opt) => opt.MapFrom(src => src.AudioUrl))
                  .ForMember(dest => dest.PosterUrl,
                  (opt) => opt.MapFrom(src => src.PosterUrl));
        }
    }
}
