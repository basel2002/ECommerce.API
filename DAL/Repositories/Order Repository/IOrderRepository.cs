namespace DAL
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        //Task PlaceOrder(string userId);
        Task<IEnumerable<Order>> ViewOrdersHistory(string userId);
        Task<Order?> GetOrderDetails(string userId, int orderId);
    }
}
