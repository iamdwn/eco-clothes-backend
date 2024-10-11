using DataAccess.Base;
using DataAccess.Models;

namespace Dashboard.Api.Services.Impl
{
    public class ProductAnalyticsImpl : IProductAnalytics
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductAnalyticsImpl(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CountInStockProductsAsync()
        {
            return _unitOfWork.ProductRepository.Get(
                filter: u => u.Amount > 0
                ).Count();
        }

        public async Task<int> CountProductsAsync()
        {
            return _unitOfWork.ProductRepository.Get().Count();
        }

        public async Task<IEnumerable<Product>> GetInStockProductsAsync()
        {
            return _unitOfWork.ProductRepository.Get(
                filter: u => u.Amount > 0
                ).ToList();
        }

        public async Task<IEnumerable<Product>> GetTotalProductsAsync()
        {
            return _unitOfWork.ProductRepository.Get().ToList();
        }
    }
}
