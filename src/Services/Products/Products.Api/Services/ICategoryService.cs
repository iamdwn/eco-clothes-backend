using DataAccess.Models;
using Products.Api.Dtos;

namespace Products.Api.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task InsertCategory(CategoryDto categoryList, Guid productId);
        Task UpdateCategory(List<CategoryDto>? categoryList, Guid productId);
        Task DeleteCategory(Guid productId);
    }
}
