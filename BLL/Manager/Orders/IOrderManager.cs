using Common;

namespace BLL
{
    public interface IOrderManager
    {

        Task<GeneralResult<IEnumerable<OrderReadDto>>> GetAllOrders(string userId);
        Task<GeneralResult<OrderReadDto>> PlaceOrderAsync(string userId);
        Task<GeneralResult<OrderReadDto>> ViewOrder(string userId, int id);
    }
}
