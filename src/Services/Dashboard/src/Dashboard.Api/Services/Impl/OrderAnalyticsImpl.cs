using DataAccess.Base;
using DataAccess.Models;

namespace Dashboard.Api.Services.Impl
{
    public class OrderAnalyticsImpl : IOrderAnalytics
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderAnalyticsImpl(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CountOrdersBeingDeliveredAsync()
        {
            return _unitOfWork.OrderRepository.Get(
                    filter: u => u.Status.Equals("Đang Giao")
                    ).Count();
        }

        public async Task<int> CountOrdersDeliveredAsync()
        {
            return _unitOfWork.OrderRepository.Get(
                    filter: u => u.Status.Equals("Đã Giao")
                    ).Count();
        }

        public async Task<IEnumerable<Order>> GetOrdersBeingDeliveredAsync()
        {
            return _unitOfWork.OrderRepository.Get(
                    filter: u => u.Status.Equals("Đang Giao")
                    ).ToList();
        }

        public async Task<IEnumerable<Order>> GetOrdersDeliveredAsync()
        {
            return _unitOfWork.OrderRepository.Get(
                    filter: u => u.Status.Equals("Đã Giao")
                    ).ToList();
        }
    }
}
