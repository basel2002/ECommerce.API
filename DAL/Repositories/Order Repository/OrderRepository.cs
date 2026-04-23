using DAL.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {

        public OrderRepository(AppDbContext context) : base(context) { }
       
        public async Task<IEnumerable<Order>> ViewOrdersHistory(string userId)
        {
            return await _context.Orders.Include(o => o.OrderItems).Where(o => o.UserId == userId).ToListAsync();
        
        }


        public async Task<Order?> GetOrderDetails(string userId ,int orderId)
        {
            return  await _context.Orders.Include(o => o.OrderItems).ThenInclude(o => o.Product).FirstOrDefaultAsync(o =>o.UserId == userId && o.Id == orderId);

        }


    }
}
