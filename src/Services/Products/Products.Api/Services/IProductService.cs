using DataAccess.Models;
using Products.Api.Dtos.Request;

namespace Products.Api.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetProductBySellerIdAsync(Guid id);
        Task<Product> CreateProductAsync(RequestProduct product);
        Task UpdateProductAsync(RequestProduct product);
        Task DeleteProductAsync(Guid id);
    }
}
