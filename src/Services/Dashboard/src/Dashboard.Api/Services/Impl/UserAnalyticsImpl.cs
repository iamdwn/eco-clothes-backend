using DataAccess.Base;
using DataAccess.Models;

namespace Dashboard.Api.Services.Impl
{

    public class UserAnalyticsImpl : IUserAnalytics
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserAnalyticsImpl(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CountActiveUsersAsync()
        {
            return _unitOfWork.UserRepository.Get(
                filter: u => u.Status.Equals(true)
                ).Count();
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return _unitOfWork.UserRepository.Get(
                filter: u => u.Status.Equals(true)
                ).ToList();
        }

        public async Task<IEnumerable<User>> GetNewUsersThisMonthAsync()
        {
            var currentDate = DateTime.Now;
            var startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            return _unitOfWork.UserRepository.Get(
                filter: u => u.DateCreated >= startOfMonth && u.DateCreated <= endOfMonth
            ).ToList();
        }

        public async Task<IEnumerable<User>> GetTotalUsersAsync()
        {
            return _unitOfWork.UserRepository.Get().ToList();
        }
    }
}
