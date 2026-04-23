using Common;
using DAL;
using FluentValidation;

namespace BLL
{
    public class ProductManager : IProductManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ProductCreateDto> _productCreateDtoValidator;
        private readonly IValidator<ProductEditDto> _productEditDtoValidator;
        private readonly IProductMapper _productErrorMapper;

        public ProductManager(IUnitOfWork unitOfWork,IValidator<ProductCreateDto> ProductCreateDtoValidator, IValidator<ProductEditDto> ProductEditDtoValidator, IProductMapper productErrorMapper)
        {
            _unitOfWork = unitOfWork;
            _productCreateDtoValidator = ProductCreateDtoValidator;
            _productEditDtoValidator = ProductEditDtoValidator;
            _productErrorMapper = productErrorMapper;
        }
        public async  Task<GeneralResult<ProductReadDto>> CreateProductAsync(ProductCreateDto productCreateDto)
        {
            //validation
            var validatedResult = await _productCreateDtoValidator.ValidateAsync(productCreateDto);
            if(!validatedResult.IsValid)
            {
                var errors = _productErrorMapper.MapError(validatedResult);
                return GeneralResult<ProductReadDto>.FailResult(errors);
            }

            //mapping to businness model 
            //check categoryid
            var categoryid = await _unitOfWork._categoryRepository.GetByIdAsync(productCreateDto.CategoryId);
            if(categoryid is null)
            {
                return GeneralResult<ProductReadDto>.FailResult("No Such Category Id");
            }

            var Product = new Product()
            {
                Name = productCreateDto.Name,
                Description = productCreateDto.Description,
                Price = productCreateDto.Price,
                StockQuantity = productCreateDto.StockQty,
                CategorieId = productCreateDto.CategoryId,
                ImageUrl = productCreateDto.ImageUrl,
                



            };

             _unitOfWork._productRepository.Add(Product);
            await _unitOfWork.SaveChangesAsync();
         
            var categoryName  = await _unitOfWork._categoryRepository.GetByIdAsync(Product.CategorieId);
            var stringcategoryName = categoryName.Name;
            var productReadDto = new ProductReadDto()
            {
                Id = Product.Id,
                Name = Product.Name,
                Price = Product.Price,
                StockQty = Product.StockQuantity,
                Description = Product.Description,
                Category = stringcategoryName,
                ImageUrl = Product.ImageUrl,


            };
            return GeneralResult<ProductReadDto>.SuccessResult(productReadDto);

        }

        public async Task<GeneralResult<ProductReadDto>> DeleteProductAsync(int productId)
        {
            var productToBeDeleted = await _unitOfWork._productRepository.GetProductWithCategory(productId);
            if(productToBeDeleted is  null)
            {
                return GeneralResult<ProductReadDto>.NotFound();
            }
            var productReadDto = new ProductReadDto()
            {
                Id = productId,
                Name = productToBeDeleted.Name,
                Category = productToBeDeleted.Categorie.Name,
                Description = productToBeDeleted.Description,
                ImageUrl = productToBeDeleted.ImageUrl,
                Price = productToBeDeleted.Price,
                StockQty = productToBeDeleted.StockQuantity,

            };
            _unitOfWork._productRepository.Delete(productToBeDeleted);
            await _unitOfWork.SaveChangesAsync();
            return GeneralResult<ProductReadDto>.SuccessResult(productReadDto, "Product Deleted Successfully");


        }

    
        public async Task<GeneralResult<ProductReadDto>> GetProductDetailsAsync(int productId)
        {
            var product = await _unitOfWork._productRepository.GetProductWithCategory(productId);
            if(product == null)
            {
                return GeneralResult<ProductReadDto>.NotFound();

            }

            var productReadDto = new ProductReadDto()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQty = product.StockQuantity,
                Category = product.Categorie.Name,
                ImageUrl= product.ImageUrl


            };

            return GeneralResult<ProductReadDto>.SuccessResult(productReadDto);

            
        }



        public async Task<GeneralResult<IEnumerable<ProductReadDto>>> GetAllProductsAsync()
        {
            var allProduct = await _unitOfWork._productRepository.GetAllWithCategory();
            var productList  = allProduct.Select(p => new ProductReadDto
            { 
                Name = p.Name, 
                Description = p.Description,
                Price = p.Price,
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                StockQty = p.StockQuantity,
                Category = p.Categorie.Name,
                 
            
            }).ToList();

            if(allProduct == null)
            {
                return GeneralResult < IEnumerable < ProductReadDto >>.NotFound();
            }
            return GeneralResult<IEnumerable<ProductReadDto>>.SuccessResult(productList);


        }

        public async Task<GeneralResult<ProductReadDto>> UpdateProductAsync(int id, ProductEditDto productEditDto)
        {

            var edited = await _productEditDtoValidator.ValidateAsync(productEditDto);
            if(!edited.IsValid)
            {
                var errors = _productErrorMapper.MapError(edited);
                return GeneralResult<ProductReadDto>.FailResult(errors);
            }


            var oldProduct = await _unitOfWork._productRepository.GetProductWithCategory(id);
            oldProduct.Name = productEditDto.Name;
            oldProduct.Price = productEditDto.Price;
            oldProduct.Description = productEditDto.Description;
            oldProduct.StockQuantity = productEditDto.StockQty;
            
            oldProduct.ImageUrl = productEditDto.ImageUrl;

            await _unitOfWork.SaveChangesAsync();

            var updatedProduct = new ProductReadDto()
            {
                Name = oldProduct.Name,
                Price = oldProduct.Price,
                ImageUrl = oldProduct.ImageUrl,
                Description = oldProduct.Description,
                Id = oldProduct.Id,
                StockQty = oldProduct.StockQuantity,
                Category = oldProduct.Categorie.Name


            };
            
            return GeneralResult<ProductReadDto>.SuccessResult(updatedProduct);


        }



        public async Task<GeneralResult> AddProductImage(int id,string imageUrl)
        {
            var product = await _unitOfWork._productRepository.GetByIdAsync(id);
            if(product == null)
            {
                return GeneralResult.NotFound("No Such Product");

            }

            product.ImageUrl = imageUrl;
            _unitOfWork._productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();
            return GeneralResult.SuccessResult("Image Added Successfully");

        }

        public async Task<GeneralResult<PageResult<ProductReadDto>>> GetProductsPaginationAsync(PaginationParameters paginationParameters, ProductFilterParameters productFilterParameters)
        {

            var pageResult = await _unitOfWork._productRepository.GetAllPaginationAsync(paginationParameters, productFilterParameters);

            var productReadDto = pageResult.Items.Select(p =>
                                new ProductReadDto
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Description = p.Description,
                                    StockQty = p.StockQuantity,
                                    Price = p.Price,
                                    Category = p.Categorie.Name
                                }).ToList();
            var pageResultDto = new PageResult<ProductReadDto>
            {
                Items = productReadDto,
                Metadata = pageResult.Metadata
            };
            return GeneralResult<PageResult<ProductReadDto>>.SuccessResult(pageResultDto);
        }
    }
}
