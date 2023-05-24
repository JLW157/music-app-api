using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicAppApi.Core.interfaces.Repositories;

namespace MusicAppApi.Core
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly MusicAppDbContext _context;

        public async Task<T> GetById(Guid Id)
        {
            return await _context.Set<T>().FindAsync(Id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task Add(T data)
        {
            await _context.Set<T>().AddAsync(data);
        }

        public void Delete(T data)
        {
            _context.Set<T>().Remove(data);
        }

        public Task Update(T data)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }
    }
}
