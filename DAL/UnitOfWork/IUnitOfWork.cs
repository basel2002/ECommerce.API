namespace DAL
{
    public interface IUnitOfWork
    {

        public IProductRepository _productRepository {  get; }
        public IOrderRepository _orderRepository { get; }
        public ICartRepository _cartRepository { get; }
        public ICategoryRepository _categoryRepository { get; }
        Task SaveChangesAsync();
    }
}
