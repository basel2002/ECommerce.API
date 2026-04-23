using Common;
using DAL.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProductRepository:GenericRepository<Product>,IProductRepository
    {
        public ProductRepository(AppDbContext context):base(context)
        {
           
            
        }

        
        public async Task<IEnumerable<Product>> GetAllWithCategory()
        {
            return await _context.Products.Include(p => p.Categorie).ToListAsync();
            
        }
        
        public async Task<Product> GetProductWithCategory(int id)
        {
            var product =  await _context.Products.Include(p => p.Categorie).FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }
        public async Task<PageResult<Product>> GetAllPaginationAsync(
      PaginationParameters? paginationParameters,
      ProductFilterParameters? productFilterParameters)
        {
            IQueryable<Product> query = _context.Set<Product>()
                .Include(p => p.Categorie)
                .AsNoTracking()
                .AsQueryable();

            // ✅ Apply filters BEFORE counting and paginating
            if (productFilterParameters != null)
                query = ApplyFilter(query, productFilterParameters);

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

            return new PageResult<Product>
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
        private IQueryable<Product> ApplyFilter(IQueryable<Product> query, ProductFilterParameters productFilterParameters)
        {
            if (productFilterParameters.CategoryId > 0)
            {
                query = query.Where(p => p.CategorieId == productFilterParameters.CategoryId);
            }

            if (!string.IsNullOrEmpty(productFilterParameters.Search))
            {
                query = query.Where(p => p.Name.Contains(productFilterParameters.Search));
            }

            return query;
        }







    }
}
