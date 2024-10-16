using Dashboard.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardGrowthController : Controller
    {
        private readonly IGrowthAnalytics _growthAnalytics;

        public DashboardGrowthController(IGrowthAnalytics growthAnalytics)
        {
            _growthAnalytics = growthAnalytics;
        }

        [HttpGet("growth-order-delivered")]
        public async Task<double> GetOrderDeliveredGrowthComparedToYesterday()
        {
            return await _growthAnalytics.GetOrderDeliveredGrowthComparedToYesterdayAsync();
        }

        [HttpGet("growth-order-being-delivered")]
        public async Task<double> GetOrderBeingDeliveredGrowthComparedToYesterday()
        {
            return await _growthAnalytics.GetOrderBeingDeliveredGrowthComparedToYesterdayAsync();
        }

        [HttpGet("growth-product")]
        public async Task<double> GetProductGrowthComparedToYesterda()
        {
            return await _growthAnalytics.GetProductGrowthComparedToYesterdayAsync();
        }

        [HttpGet("growth-user")]
        public async Task<double> GetUserGrowthComparedToYesterday()
        {
            return await _growthAnalytics.GetUserGrowthComparedToYesterdayAsync();
        }
    }
}
