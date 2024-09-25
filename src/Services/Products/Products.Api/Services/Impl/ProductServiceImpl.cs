using DataAccess.Base;
using DataAccess.Models;

namespace Products.Api.Services.Impl
{
    public class ProductServiceImpl : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductServiceImpl(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return _unitOfWork.ProductRepository.Get().ToList();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            var existingProduct = _unitOfWork.ProductRepository.GetByID(id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }
            return existingProduct;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            var existingProduct = _unitOfWork.ProductRepository.GetByID(product.ProductId);
            if (existingProduct != null
                && product.ProductName.Equals(existingProduct.ProductName))
            {
                throw new KeyNotFoundException($"Product with name {product.ProductName} is exist.");
            }

            _unitOfWork.ProductRepository.Insert(product);
            _unitOfWork.Save();
            return product;
        }

        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = _unitOfWork.ProductRepository.GetByID(product.ProductId);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {product.ProductId} not found.");
            }

            _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.Save();
        }


        public async Task DeleteProductAsync(string id)
        {
            var existingProduct = _unitOfWork.ProductRepository.GetByID(id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }

            _unitOfWork.ProductRepository.Delete(id);
            _unitOfWork.Save();
        }

        private bool ProductExists(int id)
        {
            return _unitOfWork.ProductRepository.GetByID(id) != null ? true : false;
        }
    }

}
