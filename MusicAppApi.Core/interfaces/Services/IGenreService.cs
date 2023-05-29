using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicAppApi.Models.DbModels;

namespace MusicAppApi.Core.interfaces.Services
{
    public interface IGenreService
    {
        public Task<Genre?> GetGenreById(Guid id);
    }
}
