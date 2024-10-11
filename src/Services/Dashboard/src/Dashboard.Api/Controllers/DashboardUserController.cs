using Dashboard.Api.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardUserController : Controller
    {
        private readonly IUserAnalytics _userAnalytics;

        public DashboardUserController(IUserAnalytics userAnalytics)
        {
            _userAnalytics = userAnalytics;
        }

        [HttpGet("total-users")]
        public async Task<IEnumerable<User>> GetTotalUsers()
        {
            return await _userAnalytics.GetTotalUsersAsync();
        }

        [HttpGet("new-user-this-month")]
        public async Task<IEnumerable<User>> GetNewUsersThisMonth()
        {
            return await _userAnalytics.GetNewUsersThisMonthAsync();
        }

        [HttpGet("active-users")]
        public async Task<IEnumerable<User>> GetActiveUsers()
        {
            return await _userAnalytics.GetActiveUsersAsync();
        }

        [HttpGet("count-active-users")]
        public async Task<int> CountActiveUsers()
        {
            return await _userAnalytics.CountActiveUsersAsync();
        }
    }
}
