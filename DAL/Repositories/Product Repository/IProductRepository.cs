using Common;

namespace DAL
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product> GetProductWithCategory(int id);
        Task<PageResult<Product>> GetAllPaginationAsync(PaginationParameters? paginationParameters, ProductFilterParameters? productFilterParameters);
        Task<IEnumerable<Product>> GetAllWithCategory();

    }

}
