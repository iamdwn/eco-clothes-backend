using DataAccess.Models;

namespace Dashboard.Api.Services
{
    public interface IUserAnalytics
    {
        Task<IEnumerable<User>> GetTotalUsersAsync();
        Task<IEnumerable<User>> GetNewUsersThisMonthAsync();
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<int> CountActiveUsersAsync();
    }
}
