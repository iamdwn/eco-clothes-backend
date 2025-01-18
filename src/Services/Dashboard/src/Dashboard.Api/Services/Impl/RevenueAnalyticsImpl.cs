using Dashboard.Api.Dtos;
using DataAccess.Base;
using DataAccess.Models;
using Orders.Api.Enums;

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

        public IEnumerable<CategoryRevenueDto> GetRevenueByCategoryAsync()
        {
            var orderItems = _unitOfWork.OrderitemRepository.Get(
                filter: oi => oi.Order.Status.Equals(OrderStatus.Paid.ToString()),
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

        public async Task<decimal> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var orders = _unitOfWork.OrderRepository.Get(
                filter: o => o.Status.Equals(OrderStatus.Paid.ToString()) &&
                             o.EndDate >= DateOnly.FromDateTime(startDate) &&
                             o.EndDate <= DateOnly.FromDateTime(endDate),
                includeProperties: "OrderItems"
            );

            var totalRevenue = orders
                .SelectMany(order => order.OrderItems)
                .Sum(orderItem => orderItem.TotalPrice ?? 0);

            return totalRevenue;
        }


        public async Task<decimal> GetRevenueThisMonthAsync()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            return await GetRevenueByDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<Product>> GetTopSellingProductsAsync()
        {
            var orderItems = _unitOfWork.OrderitemRepository.Get(
                filter: oi => oi.Order.Status.Equals(OrderStatus.Paid.ToString())
            );

            var productIds = orderItems.Select(oi => oi.ProductId).Distinct();

            var products = _unitOfWork.ProductRepository.Get(
                filter: p => productIds.Contains(p.ProductId)
            );

            var topProducts = orderItems
                .GroupBy(oi => oi.ProductId)
                .Select(group => new
                {
                    ProductId = group.Key,
                    TotalQuantitySold = group.Sum(oi => oi.Quantity ?? 0)
                })
                .OrderByDescending(x => x.TotalQuantitySold)
                .Take(10)
                .ToList();

            var topSellingProducts = topProducts
                .Join(products,
                      topProduct => topProduct.ProductId,
                      product => product.ProductId,
                      (topProduct, product) => product)
                .ToList();

            return topSellingProducts;
        }


        public async Task<decimal> GetTotalRevenueAsync()
        {
            var orders = _unitOfWork.OrderRepository.Get(
                filter: o => o.Status.Equals(OrderStatus.Paid.ToString()),
                includeProperties: "OrderItems"
            );

            var totalRevenue = orders
                .SelectMany(order => order.OrderItems)
                .Sum(orderItem => orderItem.TotalPrice ?? 0);

            return totalRevenue;
        }
    }
}
