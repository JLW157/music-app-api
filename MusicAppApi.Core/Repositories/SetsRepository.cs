using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicAppApi.Core.interfaces.Repositories;
using MusicAppApi.Core.interfaces.Services;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.Requests;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace MusicAppApi.Core.Repositories
{
    public class SetsRepository : ISetsRepository
    {
        private readonly MusicAppDbContext _context;

        public SetsRepository(MusicAppDbContext context)
        {
            _context = context;
        }

        public async Task<Set?> CreateSet(CreateSetRequestForDb set)
        {
            if (await _context.Set<User>().AnyAsync(x => x.Id == set.UserId))
            {
                if (await _context.Set<Set>().AnyAsync(x => x.UserId == set.UserId && x.Name == set.Name))
                {
                    return null;
                }

                var user = await _context.Set<User>().SingleOrDefaultAsync(u => u.Id == set.UserId);

                Set setToAdd = new Set()
                {
                    Name = set.Name,
                    PosterUrl = set.PosterUrl,
                    CreatedDate = DateTime.Now,
                    UserId = set.UserId,
                    User = user
                };

                var res = await _context.Set<Set>().AddAsync(setToAdd);

                return res.Entity;
            }

            return null;
        }

        public async Task<Set?> RemoveSet(Guid setId)
        {
            var res = await _context.Set<Set>().FirstOrDefaultAsync(x => x.Id == setId);
            if (res is null)
            {
                return null;
            }

            var removeRes = _context.Set<Set>().Remove(res);

            return removeRes.Entity;
        }

        public async Task<Audio?> AddAudioToSet(Guid audioId, Guid setId)
        {
            if (await _context.Set<Audio>().AnyAsync(x => x.Id == audioId)
                && await _context.Set<Set>().AnyAsync(x => x.Id == setId))
            {

                var foundSet = await _context.Set<Set>().Include(x => x.Audios).FirstOrDefaultAsync(x => x.Id == setId);

                if (foundSet.Audios.Any(a => a.Id == audioId))
                {
                    throw new ArgumentException("Audio Alredy Addded!");
                }


                var foundAudio = await _context.Set<Audio>().FindAsync(audioId);

                foundSet!.Audios.Add(foundAudio!);

                return foundAudio;
            }

            return null;
        }

        public async Task<Audio?> RemoveAudioFromSet(Guid audioId, Guid setId)
        {
            if (await _context.Set<Audio>().AnyAsync(x => x.Id == audioId)
                && await _context.Set<Set>().AnyAsync(x => x.Id == setId))
            {
                var foundSet = await _context.Set<Set>().Include(x => x.Audios).FirstOrDefaultAsync(x => x.Id == setId);

                if (!foundSet.Audios.ToList().Any(a => a.Id == audioId))
                {
                    throw new ArgumentException("Audio already deleted!");
                }

                var foundAudio = await _context.Set<Audio>().FindAsync(audioId);

                foundSet!.Audios.Remove(foundAudio!);
                return foundAudio;
            }

            return null;
        }

        public async Task<IEnumerable<Set>> GetUserSets(Guid userId)
            => await _context.Sets
                .Include(x => x.User)
                .Include(x => x.Audios)
                .Where(x => x.User.Id == userId).ToListAsync();

        public async Task<IEnumerable<Set>> GetSetsByUsername(string username)
            => await _context.Set<Set>().Include(s => s.User).Include(s => s.Audios)
                .Where(s => s.User.UserName == username).ToListAsync();

        public async Task<Set?> GetByNameOfSet(string nameOfSet)
            => await _context.Sets.Include(x => x.Audios).ThenInclude(x => x.Artists)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Name == nameOfSet);


        public async Task<IEnumerable<Audio>> GetAudiosFromSet(Guid setId)
        {
            var res = await _context.Set<Set>()
                .Include(x => x.Audios)
                .ThenInclude(x => x.Artists).FirstOrDefaultAsync(x => x.Id == setId);

            return res.Audios.ToList();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
