using Common;
using DAL.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Set<T>()
                .Add(entity);
            
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>()
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>()
                .FindAsync(id);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }


        public async Task<PageResult<T>> GetAllPaginationAsync(PaginationParameters? paginationParameters = null)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking().AsQueryable();

            var totalCount = await query.CountAsync();

            var pageNumber = paginationParameters?.PageNumber ?? 1;
            var pageSize = paginationParameters?.PageSize ?? totalCount;

            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PageResult<T>
            {
                Items = items,
                Metadata = new PaginationMetaData
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    TotalCount = totalCount,
                    HasNext = pageNumber < totalPages,
                    HasPrevious = pageNumber > 1,
                }
            };
        }

    }
}
