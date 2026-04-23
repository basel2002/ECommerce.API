using Common;
using DAL;
using FluentValidation;

namespace BLL
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CategoryCreateDto> _categoryCreateDtoValidator;
        private readonly IValidator<CategoryEditDto> _categoryEditDtoVaildator;
        private readonly ICategoryMapper _categoryMapper;

        public CategoryManager(IUnitOfWork unitOfWork,IValidator<CategoryCreateDto> categoryCreateDtoValidator,IValidator<CategoryEditDto> categoryEditDtoVaildator,ICategoryMapper categoryMapper)
        {
            _unitOfWork = unitOfWork;
            _categoryCreateDtoValidator = categoryCreateDtoValidator;
            _categoryEditDtoVaildator = categoryEditDtoVaildator;
            _categoryMapper = categoryMapper;
        }


        public async Task<GeneralResult< IEnumerable<CategoryReadDto>>> GetAllCategoriesAsync()
        { 
            var categories = await _unitOfWork._categoryRepository.GetAllAsync();
            if (categories == null)
            {
                return GeneralResult<IEnumerable<CategoryReadDto>>.NotFound();
            }
            var allCategories = categories.Select(c =>
            new CategoryReadDto
            {
                Name = c.Name,
                ImageUrl = c.ImageUrl,
                Id = c.Id,


            }).ToList();

            return GeneralResult<IEnumerable<CategoryReadDto>>.SuccessResult(allCategories);

        }

        public async Task<GeneralResult<CategoryReadDto>> GetCategoryByIdAsync(int id)
        {
            var category  = await _unitOfWork._categoryRepository.GetByIdAsync(id);
            if(category is null)
            {
                return GeneralResult<CategoryReadDto>.FailResult("No such category");
                
            }

            var categorReadDto = new CategoryReadDto()
            {
                Name = category.Name,
                Id = category.Id,
                ImageUrl = category.ImageUrl,
            };

            return GeneralResult<CategoryReadDto>.SuccessResult(categorReadDto);
        }
      
        public async Task<GeneralResult<CategoryReadDto>> DeleteCategory(int id)
        {
            var categoryToBeDeleted = await _unitOfWork._categoryRepository.GetByIdAsync(id);
            if (categoryToBeDeleted is null)
            {
                return GeneralResult<CategoryReadDto>.NotFound();
            }
            var categorReadDto = new CategoryReadDto()
            {
                Name = categoryToBeDeleted.Name,
                Id = categoryToBeDeleted.Id,
                ImageUrl = categoryToBeDeleted.ImageUrl,


            };
            _unitOfWork._categoryRepository.Delete(categoryToBeDeleted);
            await _unitOfWork.SaveChangesAsync();
            return GeneralResult<CategoryReadDto>.SuccessResult(categorReadDto, "Product Deleted Successfully");


        }


        public async Task<GeneralResult<CategoryReadDto>> CreateCategory(CategoryCreateDto categoryCreateDto)
        {
            var result = await  _categoryCreateDtoValidator.ValidateAsync(categoryCreateDto);
            if(!result.IsValid)
            {
                return GeneralResult<CategoryReadDto>.FailResult("Some Validation Error Happened");
            }

            var category = new Categorie()
            {
                CreatedAt = DateTime.Now,
                Description = categoryCreateDto.Description,
                ImageUrl = categoryCreateDto.ImageUrl,
                Name = categoryCreateDto.Name,

            };
             _unitOfWork._categoryRepository.Add(category);
            await _unitOfWork.SaveChangesAsync();


            var categoryReadDto = new CategoryReadDto()
            {
                Name = category.Name,
                Id = category.Id,
                ImageUrl = category.ImageUrl,
            };

            return GeneralResult<CategoryReadDto>.SuccessResult(categoryReadDto, "Category Created Successfully");

            
        }


        public async Task<GeneralResult> UpdateCategory(int id ,CategoryEditDto categoryEditDto)
        {
            var result  = await _categoryEditDtoVaildator.ValidateAsync(categoryEditDto);
            if(!result.IsValid)
            {
                return GeneralResult.FailResult("Validation Error ");
            }

            var categoryToBeUpdated = await _unitOfWork._categoryRepository.GetByIdAsync(id);

            categoryToBeUpdated.Name = categoryEditDto.Name;
            categoryToBeUpdated.Description = categoryEditDto.Description;
            categoryToBeUpdated.ImageUrl = categoryEditDto.ImageUrl;
            categoryToBeUpdated.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            return GeneralResult.SuccessResult($"Category With ID = {id} updated successfully");


        }


        public async Task<GeneralResult> AddCategoryImage(int id, string imageUrl)
        {
            var category = await _unitOfWork._categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return GeneralResult.NotFound("No Such Category");

            }

            category.ImageUrl = imageUrl;
            _unitOfWork._categoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();
            return GeneralResult.SuccessResult("Image Added Successfully");

        }
    }
}
