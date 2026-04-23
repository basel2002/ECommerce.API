using BLL;
using Common;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiFinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartManager _cartManager;

        public CartsController(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GeneralResult>> AddToCart([FromBody] CartAddToDto dto)
        {
            var productID = dto.productId;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartManager.AddToCartAsync(userId, productID);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = "UserOnly")]
        public  async Task<ActionResult<GeneralResult< IEnumerable< CartItem>>>> GetUserCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartManager.GetUserCart(userId);

            if(!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);


        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "UserOnly")]

        public async Task<ActionResult<GeneralResult<CartItemReadDto>>> RemoveFromCart(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _cartManager.RemoveFromCart(userId, id);
            if(!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpPut]
        [Authorize(Policy = "UserOnly")]

        public async Task<ActionResult<GeneralResult>> UpdatCartItemQuantity(CartItemUpdateDto cartItemUpdateDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _cartManager.UpdateCartItemQuantity(userId, cartItemUpdateDto);

            if(!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
