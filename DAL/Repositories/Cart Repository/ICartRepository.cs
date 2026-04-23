namespace DAL
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        
        void RemoveFromCart(CartItem cartItem);
        void UpdateCartItemQuantity(CartItem cartItem);
        Task<IEnumerable<CartItem>> GetUserCart(string UserId);
        Task<Cart?> GetCartWithItemsAsync(string userId);

        void ClearCart(Cart cart);

    }
}
