using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicAppApi.Core.interfaces;
using MusicAppApi.Models.DbModels;

namespace MusicAppApi.Core.Services
{
    public class GenreService : IGenreService
    {
        private readonly MusicAppDbContext _context;

        public GenreService(MusicAppDbContext context)
        {
            _context = context;
        }

        public async Task<Genre?> GetGenreById(Guid id)
        {
            return await _context.Genres.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
