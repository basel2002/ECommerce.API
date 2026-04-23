
using Common;

namespace BLL
{
    public interface IProductManager
    {
        Task< GeneralResult<ProductReadDto>> GetProductDetailsAsync(int  productId);
        Task<GeneralResult<IEnumerable<ProductReadDto>>> GetAllProductsAsync();

        Task<GeneralResult<ProductReadDto>> CreateProductAsync(ProductCreateDto productCreateDto);

        Task<GeneralResult<ProductReadDto>> UpdateProductAsync(int id, ProductEditDto productEditDto);
        Task<GeneralResult<ProductReadDto>> DeleteProductAsync(int productId);
        Task<GeneralResult> AddProductImage(int id, string imageUrl);
        Task<GeneralResult<PageResult<ProductReadDto>>> GetProductsPaginationAsync(PaginationParameters paginationParameters, ProductFilterParameters productFilterParameters);


    }

}
