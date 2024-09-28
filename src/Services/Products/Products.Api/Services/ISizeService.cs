using DataAccess.Models;
using Products.Api.Dtos;

namespace Products.Api.Services
{
    public interface ISizeService
    {
        Task<IEnumerable<Size>> GetAllSizesAsync();
        Task InsertSize(SizeDto sizeList, Guid productId);
        Task UpdateSize(SizeDto sizeList, Guid productId);
    }
}
