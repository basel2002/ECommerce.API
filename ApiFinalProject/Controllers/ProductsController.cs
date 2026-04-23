using BLL;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace ApiFinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductManager _productManager;

        public ProductsController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GeneralResult<ProductReadDto>>> GetProductDetails([FromRoute]int  id)
        {
            var Product = await _productManager.GetProductDetailsAsync(id);
            if(!Product.Success)
            {
                return NotFound(Product);
            }
            return Ok(Product);

        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<GeneralResult>> CreateProduct(ProductCreateDto productCreateDto)
        {
           var result  =await _productManager.CreateProductAsync(productCreateDto);
            if(!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<GeneralResult<ProductReadDto>>> DeleteProduct([FromRoute]int id)
        {
            var result = await _productManager.DeleteProductAsync(id);
            if(!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        [HttpGet]
        public async Task<ActionResult<GeneralResult<IEnumerable<ProductReadDto>>>> GetAllProducts()
        {
            var result = await _productManager.GetAllProductsAsync();
             if(!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }


        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<GeneralResult<ProductReadDto>>> UpdateProduct([FromRoute] int id ,ProductEditDto productEditDto)
        {
            var result = await _productManager.UpdateProductAsync(id, productEditDto);
            if(!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("Pagination")]
        public async Task<ActionResult<GeneralResult<IEnumerable<ProductReadDto>>>> GetAllPaginationAsync([FromQuery] PaginationParameters paginationParameters, [FromQuery] ProductFilterParameters productFilterParameters)
        {
            var result = await _productManager.GetProductsPaginationAsync(paginationParameters, productFilterParameters);
            return Ok(result);
        }

    }
}
