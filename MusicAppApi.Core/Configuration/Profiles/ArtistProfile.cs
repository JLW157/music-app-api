using AutoMapper;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.DTO_s.Artist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Core.Configuration.Profiles
{
    internal class ArtistProfile : Profile
    {
        public ArtistProfile()
        {
            CreateMap<User, ArtistsResponse>()
                .ForMember(dest => dest.Name,
                opts => opts.MapFrom(src => src.UserName))
                .ForMember(dest => dest.AudioUrls,
                opts =>
                    opts.MapFrom(src => src.Audios == null
                    ? new List<string>()
                    : src.Audios.Select(x => x.Name)
                    .ToList())).ReverseMap();
        }
    }
}
