using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Core.interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetById(Guid Id);
        Task<IEnumerable<T>?> GetAll();
        Task Add(T data);
        void Delete(T data);
        Task Update(T data);
        Task Save();
    }
}
