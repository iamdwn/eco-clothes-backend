using DataAccess.Models;
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
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            return await _orderService.GetOrderByIdAsync(id);
        }

        [HttpGet("by-buyer/{userId}")]
        public async Task<IEnumerable<Order>> GetOrderByBuyerId(Guid userId)
        {
            return await _orderService.GetOrderByBuyerIdAsync(userId);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto order)
        {
            return await _orderService.CreateOrderAsync(order);
        }

        [HttpPut]
        public async Task UpdateOrder(OrderDto order)
        {
            await _orderService.UpdateOrderAsync(order);
        }

        [HttpDelete("{id}")]
        public async Task DeleteOrder(Guid id)
        {
            await _orderService.DeleteOrderAsync(id);
        }
    }
}
