using Common;

namespace BLL
{
    public interface ICategoryManager
    {
        Task<GeneralResult<IEnumerable<CategoryReadDto>>> GetAllCategoriesAsync();
        Task<GeneralResult<CategoryReadDto>> GetCategoryByIdAsync(int id);

        Task<GeneralResult<CategoryReadDto>> CreateCategory(CategoryCreateDto categoryCreateDto);
        Task<GeneralResult> UpdateCategory(int id, CategoryEditDto categoryEditDto);
        Task<GeneralResult<CategoryReadDto>> DeleteCategory(int id);
        Task<GeneralResult> AddCategoryImage(int id, string imageUrl);


    }
}
