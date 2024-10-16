using Dashboard.Api.Dtos;
using DataAccess.Base;
using DataAccess.Models;

namespace Dashboard.Api.Services.Impl
{
    public class RevenueAnalyticsImpl : IRevenueAnalytics
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderAnalytics _orderAnalytics;

        public RevenueAnalyticsImpl(IUnitOfWork unitOfWork, IOrderAnalytics orderAnalytics)
        {
            _unitOfWork = unitOfWork;
            _orderAnalytics = orderAnalytics;
        }

        public IEnumerable<CategoryRevenueDto> GetRevenueByCategory()
        {
            var orderItems = _unitOfWork.OrderitemRepository.Get(
                filter: oi => oi.Order.Status == "Đã Giao",
                includeProperties: "Order"
            );

            var productIds = orderItems.Select(oi => oi.ProductId).Distinct();

            var productCategories = _unitOfWork.ProductcategoryRepository.Get(
                filter: pc => productIds.Contains(pc.ProductId),
                includeProperties: "Category"
            );

            var revenueByCategory = orderItems
                .Join(productCategories,
                      orderItem => orderItem.ProductId,
                      productCategory => productCategory.ProductId,
                      (orderItem, productCategory) => new { orderItem, productCategory.Category })
                .GroupBy(x => x.Category)
                .Select(group => new CategoryRevenueDto
                {
                    CategoryName = group.Key.Name,
                    TotalRevenue = group.Sum(x => x.orderItem.TotalPrice ?? 0)
                })
                .ToList();

            return revenueByCategory;
        }

        public Task<decimal> GetRevenueByDateRange(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetRevenueThisMonth()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetTopSellingProducts()
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetTotalRevenue()
        {
            throw new NotImplementedException();
        }
    }
}
