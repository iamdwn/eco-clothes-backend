using DataAccess.Base;
using DataAccess.Models;
using Products.Api.Dtos;

namespace Products.Api.Services.Impl
{
    public class CategoryServiceImpl : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryServiceImpl(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteCategory(Guid productId)
        {
            var productCategories = _unitOfWork.ProductcategoryRepository
                 .Get(filter: sp => sp.ProductId == productId)
                 .ToList();

            if (!productCategories.Any())
            {
                {
                    throw new KeyNotFoundException($"ProductCategory with id {productId} not found.");
                }
            }
            _unitOfWork.ProductcategoryRepository.DeleteRange(productCategories);

            _unitOfWork.Save();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return _unitOfWork.CategoryRepository.Get().ToList();
        }

        public async Task InsertCategory(CategoryDto item, Guid productId)
        {
            var pointCategory = _unitOfWork.CategoryRepository.Get(
                filter: s => s.Name.Equals(item.CategoryName)
                ).FirstOrDefault();

            if (pointCategory == null)
            {
                throw new KeyNotFoundException($"Category with name {item.CategoryName} not found.");
            }

            var insertCategory = new ProductCategory()
            {
                ProductId = productId,
                CategoryId = pointCategory.CategoryId
            };

            _unitOfWork.ProductcategoryRepository.Insert(insertCategory);
            _unitOfWork.Save();
        }

        public async Task UpdateCategory(List<CategoryDto> categoryList, Guid productId)
        {
            await DeleteCategory(productId);

            foreach (var item in categoryList)
            {
                var pointCategory = _unitOfWork.CategoryRepository.Get(
                           filter: s => s.Name.Equals(item.CategoryName)
                           ).FirstOrDefault();

                if (pointCategory == null)
                {
                    throw new KeyNotFoundException($"Category with name {item.CategoryName} not found.");
                }

                await InsertCategory(item, productId);
            }
        }
    }
}
