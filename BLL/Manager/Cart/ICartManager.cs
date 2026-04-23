using Common;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface ICartManager
    {
        Task<GeneralResult<CartReadDto>> AddToCartAsync(string userId, int productID);

        Task<GeneralResult<IEnumerable<CartItemReadDto>>> GetUserCart(string userId);
        Task<GeneralResult<IEnumerable<CartItemReadDto>>> RemoveFromCart(string userId, int productId);
        Task<GeneralResult> UpdateCartItemQuantity(string userId, CartItemUpdateDto cartItemUpdateDto);
    }
}
