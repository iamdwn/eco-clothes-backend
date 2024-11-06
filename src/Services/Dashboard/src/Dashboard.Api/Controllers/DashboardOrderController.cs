using Dashboard.Api.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardOrderController : ControllerBase
    {
        private readonly IOrderAnalytics _orderAnalytics;

        public DashboardOrderController(IOrderAnalytics orderAnalytics)
        {
            _orderAnalytics = orderAnalytics;
        }

        [HttpGet("get-orders-being-delivered")]
        public async Task<IEnumerable<Order>> GetOrdersBeingDelivered()
        {
            return await _orderAnalytics.GetOrdersBeingDeliveredAsync();
        }

        [HttpGet("get-orders-delivered")]
        public async Task<IEnumerable<Order>> GetOrdersDelivered()
        {
            return await _orderAnalytics.GetOrdersDeliveredAsync();
        }

        [HttpGet("get-totals-orders")]
        public async Task<IEnumerable<Order>> GetTotalOrders()
        {
            return await _orderAnalytics.GetTotalOrdersAsync();
        }

        [HttpGet("count-orders-delivered")]
        public async Task<int> CountOrdersDelivered()
        {
            return await _orderAnalytics.CountOrdersDeliveredAsync();
        }

        [HttpGet("count-orders-being-delivered")]
        public async Task<int> CountOrdersBeingDelivered()
        {
            return await _orderAnalytics.CountOrdersBeingDeliveredAsync();
        }

        [HttpGet("count-totals-orders")]
        public async Task<int> CountTotalOrders()
        {
            return await _orderAnalytics.CountTotalOrdersAsync();
        }

        [HttpGet("count-daily-orders-delivered")]
        public async Task<int> CountDailyOrdersDelivered(DateTime dateTime)
        {
            return await _orderAnalytics.CountDailyOrdersDeliveredAsync(dateTime);
        }

        [HttpGet("count-daily-orders-being-delivered")]
        public async Task<int> CountDailyOrdersBeingDelivered(DateTime dateTime)
        {
            return await _orderAnalytics.CountDailyOrdersBeingDeliveredAsync(dateTime);
        }
    }
}
