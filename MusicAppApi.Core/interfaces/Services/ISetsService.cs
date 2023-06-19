using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.DTO_s;
using MusicAppApi.Models.DTO_s.Sets;
using MusicAppApi.Models.Requests;
using MusicAppApi.Models.Responses;

namespace MusicAppApi.Core.interfaces.Services
{
    public interface ISetsService
    {
        Task<CreateSetResponse> CreateSet(CreateSetRequestForService createSetRequest);
        Task<IEnumerable<SetDto>> GetUserSets(Guid userId);
        Task<IEnumerable<SetDto>> GetSetsByUsername(string username);

        Task<AudioResponse?> AddAudioToSet(AddAudioToSetRequest addAudioToSetRequest);

        Task<AudioResponse?> RemoveAudioFromSet(RemoveAudioFromSetRequest removeAudioFromSetRequest);
    }
}
