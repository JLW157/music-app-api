using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicAppApi.Core.interfaces.Repositories;
using MusicAppApi.Core.interfaces.Services;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.DTO_s;
using Org.BouncyCastle.Asn1.X509;

namespace MusicAppApi.Core.Services
{
    public class AudioService : IAudioService
    {
        private readonly MusicAppDbContext _context;
        private readonly IMapper _mapper;

        public AudioService(MusicAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateAudio(Audio audioCreationDto)
        {
            await _context.AddAsync(audioCreationDto);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<AudioResponse>> GetAllAudios()
        {
            var audios = await _context.Audios.Include(x => x.Artists)
                .Include(x => x.Genre)
                .ToListAsync();

            return _mapper.Map<List<Audio>, List<AudioResponse>>(audios);
        }

        public async Task<IEnumerable<AudioResponse>> GetUserAudios(Guid userId)
        {
            var audios = await _context.Audios
                .Include(x => x.Artists)
                .Include(x => x.Genre)
                .Where(a => a.Artists.Any(a => a.Id == userId))
                .ToListAsync();

            var mappedAudios = _mapper.Map<List<Audio>, List<AudioResponse>>(audios);

            return mappedAudios;
        }

        public async Task<AudioResponse> GetAudioByName(string username)
        {
            var audio = await _context.Audios.Include(x => x.Artists).Include(x => x.Genre)
                .FirstOrDefaultAsync(a => a.Name == username);

            return _mapper.Map<Audio, AudioResponse>(audio);
        }

        public async Task<IEnumerable<AudioResponse>?> GetAudiosByName(string username)
        {
            var audio = await _context.Audios.Include(x => x.Artists).Include(x => x.Genre)
                .Where(a => a.Name == username).ToListAsync();

            return _mapper.Map<List<Audio>, List<AudioResponse>>(audio);
        }

        public async Task<IEnumerable<AudioResponse>?> GetAudiosWithLimit(int limit)
        {
            return _mapper.Map<List<Audio>, List<AudioResponse>>(await _context.Audios.Include(x => x.Genre).Include(x => x.Artists).Take(limit).ToListAsync());
        }
    }
}
