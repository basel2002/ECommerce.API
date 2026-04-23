using Common;
using DAL;

namespace BLL
{
    public class OrderManager : IOrderManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResult<IEnumerable<OrderReadDto>>> GetAllOrders(string userId)
        {
            var allOrders = await _unitOfWork._orderRepository.ViewOrdersHistory(userId);
            if (allOrders == null)
            {
                return GeneralResult<IEnumerable<OrderReadDto>>.NotFound();
            }


            var ordersReadDto = allOrders.Select(o =>

            new OrderReadDto()
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status.ToString(),
                TotalPrice = o.TotalPrice,

            }
            ).ToList();

            return GeneralResult<IEnumerable<OrderReadDto>>.SuccessResult( ordersReadDto );
        }

        

        public async Task<GeneralResult<OrderReadDto>> PlaceOrderAsync(string userId)
        {
            var userCart = await _unitOfWork._cartRepository.GetCartWithItemsAsync( userId );
            if (userCart == null || !userCart.CartItems.Any())
                return GeneralResult<OrderReadDto>.FailResult("Cart is empty.");

            var orderItems = userCart.CartItems.Select(cartItem => new OrderItem()
            {
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.product.Price,
                TotalPrice = cartItem.Quantity * cartItem.product.Price
            }).ToList();

            var totalPrice = orderItems.Sum(item => item.Quantity * item.UnitPrice);

            var order = new Order()
            {
                UserId = userId,
                TotalPrice = totalPrice,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                OrderItems = orderItems
            };

            _unitOfWork._orderRepository.Add(order);
            _unitOfWork._cartRepository.ClearCart(userCart);
            await _unitOfWork.SaveChangesAsync();

            var orderReadDto = new OrderReadDto()
            {
                Id = order.Id,
                OrderDate = DateTime.UtcNow,
                Status = order.Status.ToString(),
                TotalPrice = order.TotalPrice,
                UserId = order.UserId

            };
           
           
            return GeneralResult<OrderReadDto>.SuccessResult( orderReadDto,"Order placed successfully.");




        }


        public async Task<GeneralResult<OrderReadDto>> ViewOrder(string userId,int id)
        {
            var order = await _unitOfWork._orderRepository.GetOrderDetails(userId, id);

            if(order == null)
            {
                return GeneralResult<OrderReadDto>.NotFound();
            }

            var orderReadDto = new OrderReadDto()
            {
                Id = order.Id,
                OrderDate = order.CreatedAt,
                Status = order.Status.ToString(),
                TotalPrice = order.TotalPrice,
                UserId = order.UserId
            };
            return GeneralResult<OrderReadDto>.SuccessResult(orderReadDto);
        }
    }
}
