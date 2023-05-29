using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicAppApi.Core.interfaces.Repositories;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.DTO_s;

namespace MusicAppApi.Core.interfaces.Services
{
    public interface IAudioService
    {
        Task<bool> CreateAudio(Audio audioCreationDto);

        Task<IEnumerable<AudioResponse>?> GetAllAudios();

        Task<IEnumerable<AudioResponse>?> GetUserAudios(Guid userId);

        Task<AudioResponse?> GetAudioByName(string username);
        Task<IEnumerable<AudioResponse>?> GetAudiosByName(string username);
        Task<IEnumerable<AudioResponse>?> GetAudiosWithLimit(int limit);
    }
}
