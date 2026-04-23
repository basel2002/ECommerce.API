using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IGenericRepository<T> where T : class
    {
        Task<PageResult<T>> GetAllPaginationAsync(PaginationParameters? paginationParameters = null);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);

        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
