using BLL;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiFinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryManager _categoryManager;

        public CategoriesController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        [HttpGet]
        public async Task<ActionResult<GeneralResult<IEnumerable<CategoryReadDto>>>> GetAllCategories()
        {
            var categories = await _categoryManager.GetAllCategoriesAsync();
            
            if(!categories.Success)
            {
                return BadRequest();
            }

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GeneralResult<CategoryReadDto>>> GetCategoryDetails(int id)
        {
            var result  = await _categoryManager.GetCategoryByIdAsync(id);
            if(!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        [HttpPost]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<GeneralResult<CategoryReadDto>>> CreateCategory(CategoryCreateDto categoryCreateDto)
        {
            var result = await _categoryManager.CreateCategory(categoryCreateDto);

            if(!result.Success)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<GeneralResult<CategoryReadDto>>> DeleteCategory(int id) {

            var result = await _categoryManager.DeleteCategory(id);

            if(!result.Success)
            {
                return BadRequest();
            }

            return Ok(result);
        
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<GeneralResult>> UpdateCategory(int id,CategoryEditDto categoryEditDto)
        {

            var result = await _categoryManager.UpdateCategory(id,categoryEditDto);

            if(!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }



    }
}
