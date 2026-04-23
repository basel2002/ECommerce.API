using BLL;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiFinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderManager _orderManager;

        public OrdersController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        [HttpGet]
        [Authorize(Policy = "UserOnly")]


        public async Task<ActionResult<GeneralResult<IEnumerable<OrderReadDto>>>> ViewOrdersHistory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderManager.GetAllOrders(userId);

            if(!orders.Success)
            {
                return BadRequest();
            }

            return Ok(orders);
           
        }
        [HttpPost]
        [Authorize(Policy = "UserOnly")]


        public async Task<ActionResult<GeneralResult<OrderReadDto>>> PlaceOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result  = await _orderManager.PlaceOrderAsync(userId);

            if (!result.Success) { 
            
                return BadRequest(result);
            }
            return Ok(result);
        }



        [HttpGet("{id}")]
        [Authorize(Policy = "UserOnly")]

        public async Task<ActionResult<GeneralResult<OrderReadDto>>> GetOrderDetails(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _orderManager.ViewOrder(userId, id);

            if(!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
