using DataAccess.Models;
using DataAccess.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Orders.Api.Dtos;
using Orders.Api.Services;

namespace Orders.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _orderService.GetAllOrdersAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseObject>> GetOrder(Guid id)
        {
            try
            {
                return ResponseObject.Success<Order>(await _orderService.GetOrderByIdAsync(id));
            }
            catch (Exception ex)
            {
                return ResponseObject.Failure(ex.Message);
            }
        }

        [HttpGet("by-buyer/{userId}")]
        public async Task<ResponseObject> GetOrderByBuyerId(Guid userId)
        {
            try
            {
                return ResponseObject.Success<IEnumerable<Order>>(await _orderService.GetOrderByBuyerIdAsync(userId));
            }
            catch (Exception ex)
            {
                return ResponseObject.Failure(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResponseObject>> CreateOrder(OrderDto order)
        {
            try
            {
                return ResponseObject.Success<Order>(await _orderService.CreateOrderAsync(order));
            }
            catch (Exception ex)
            {
                return ResponseObject.Failure(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ResponseObject> UpdateOrder(OrderDto order)
        {
            try
            {
                await _orderService.UpdateOrderAsync(order);
                return ResponseObject.Success();
            }
            catch (Exception ex)
            {
                return ResponseObject.Failure(ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<ResponseObject> DeleteOrder(Guid id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return ResponseObject.Success();
            }
            catch (Exception ex)
            {
                return ResponseObject.Failure(ex.Message);
            }
        }
    }
}
