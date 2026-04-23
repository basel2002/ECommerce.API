using BLL;
using Common;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiFinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageManager _imageManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductManager _productManager;
        private readonly ICategoryManager _categoryManager;
      
      

        public ImagesController(IImageManager imageManager, IWebHostEnvironment webHostEnvironment,IProductManager productManager,ICategoryManager categoryManager)
        {
            _imageManager = imageManager;
            _webHostEnvironment = webHostEnvironment;
            _productManager = productManager;
            _categoryManager = categoryManager;
           
        
        }

        [HttpPost]
        [Route("upload")]
        public async Task<ActionResult<GeneralResult<ImageUploadResultDto>>> UploadImageAsync(
            [FromForm] ImageUploadDto imageUploadDto)
        {
            var schema = Request.Scheme;
            var host = Request.Host.Value;
            var basePath = _webHostEnvironment.ContentRootPath;

            var result = await _imageManager.UploadAsync(imageUploadDto, basePath, schema, host);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("products/{id}/image")]
        public async Task<ActionResult> AssignProductImage(int id , [FromBody]ImageUploadResultDto imageUploadResultDto)
        {
            var result = await _productManager.AddProductImage(id, imageUploadResultDto.ImageURL);

            if(!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }
        [HttpPost("categories/{id}/image")]
        public async Task<ActionResult> AssignCategoryImage(int id, [FromBody] ImageUploadResultDto imageUploadResultDto)
        {
            var result = await _categoryManager.AddCategoryImage(id, imageUploadResultDto.ImageURL);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

    }
}
