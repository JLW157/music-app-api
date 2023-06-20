using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.DTO_s.Sets;
using MusicAppApi.Models.Requests;

namespace MusicAppApi.Core.interfaces.Repositories
{
    public interface ISetsRepository
    {
        Task<Set?> CreateSet(CreateSetRequestForDb set);
        Task<Set?> RemoveSet(Guid setId);
        Task<Audio?> AddAudioToSet(Guid audioId, Guid setId);
        Task<Audio?> RemoveAudioFromSet(Guid audioId, Guid setId);
        Task<IEnumerable<Set>> GetUserSets(Guid userId);

        Task<IEnumerable<Set>> GetSetsByUsername(string username);

        Task<Set?> GetByNameOfSet(string nameOfSet);

        Task<IEnumerable<Audio>> GetAudiosFromSet(Guid setId);

        Task SaveChanges();
    }
}
