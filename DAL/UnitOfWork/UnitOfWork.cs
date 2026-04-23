using DAL.Data.Context;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        public IProductRepository _productRepository { get; }
        public IOrderRepository _orderRepository { get; }
        public ICartRepository _cartRepository { get; }
        public ICategoryRepository _categoryRepository { get; }
        private readonly AppDbContext _context;

        public UnitOfWork(IProductRepository productRepository,IOrderRepository orderRepository,ICartRepository cartRepository,ICategoryRepository categoryRepository,AppDbContext context)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _categoryRepository = categoryRepository;
            _context = context;
        }
        public async Task SaveChangesAsync()
        {

           await _context.SaveChangesAsync();

        }
    }
}
