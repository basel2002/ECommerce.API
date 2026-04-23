using DAL.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DAL
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context)
        {


        }

        public void RemoveFromCart(CartItem cartItem)
        {     
                _context.CartItems.Remove(cartItem);
          
        }


        public void UpdateCartItemQuantity(CartItem cartItem)
        {
                _context.CartItems.Update(cartItem);
        }



        public async Task<IEnumerable<CartItem>> GetUserCart(string UserId)
        {
            var cartExist = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == UserId);
            if (cartExist == null)
                return null;

            return _context.CartItems
                .Include(c => c.product)
                .Where(c => c.CartId == cartExist.Id)
                .ToList();

        }


        public async Task<Cart?> GetCartWithItemsAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public void ClearCart(Cart cart)
        {
            _context.CartItems.RemoveRange(cart.CartItems);
        }


    }
}
