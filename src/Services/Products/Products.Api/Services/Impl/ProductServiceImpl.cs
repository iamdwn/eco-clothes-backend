using DataAccess.Base;
using DataAccess.Models;
using Products.Api.Dtos.Request;

namespace Products.Api.Services.Impl
{
    public class ProductServiceImpl : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISizeService _sizeService;

        public ProductServiceImpl(IUnitOfWork unitOfWork, ISizeService sizeService)
        {
            _unitOfWork = unitOfWork;
            _sizeService = sizeService;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return _unitOfWork.ProductRepository.Get(
                includeProperties: "SizeProducts"
                ).ToList();
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            var existingProduct = _unitOfWork.ProductRepository.GetByID(id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }
            return existingProduct;
        }

        public async Task<Product> CreateProductAsync(RequestProduct product)
        {
            try
            {
                var existingProduct = _unitOfWork.ProductRepository.Get(
                    filter: p => p.ProductName.Equals(product.ProductName)
                    ).FirstOrDefault();

                if (existingProduct != null)
                {
                    throw new KeyNotFoundException($"Product with name {product.ProductName} is exist.");
                }

                var insertProduct = new Product()
                {
                    ProductName = product.ProductName,
                    OldPrice = product.OldPrice,
                    NewPrice = product.NewPrice,
                    NumberOfSold = 0,
                    ImgUrl = product.ImgUrl,
                    Description = product.Description
                };

                _unitOfWork.ProductRepository.Insert(insertProduct);
                _unitOfWork.Save();

                int amount = 0;

                foreach (var item in product.Sizes)
                {
                    _sizeService.InsertSize(item, insertProduct.ProductId);
                    amount += item.SizeQuantity;
                };

                insertProduct = _unitOfWork.ProductRepository.GetByID(insertProduct.ProductId);
                insertProduct.Amount = amount;
                _unitOfWork.ProductRepository.Update(insertProduct);
                _unitOfWork.Save();

                return insertProduct;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
            }
        }

        public async Task UpdateProductAsync(RequestProduct product)
        {
            try
            {
                var existingProduct = _unitOfWork.ProductRepository.GetByID(product.ProductId);

                if (existingProduct == null)
                {
                    throw new KeyNotFoundException($"Product with id {product.ProductId} not found.");
                }

                var newAmount = product.Sizes.Sum(s => s.SizeQuantity);

                foreach (var item in product.Sizes)
                {
                    _sizeService.UpdateSize(item, existingProduct.ProductId);
                };

                existingProduct.ProductName = product.ProductName ?? existingProduct.ProductName;
                existingProduct.OldPrice = product.OldPrice ?? existingProduct.OldPrice;
                existingProduct.NewPrice = product.NewPrice ?? existingProduct.NewPrice;
                existingProduct.ImgUrl = product.ImgUrl ?? existingProduct.ImgUrl;
                existingProduct.Description = product.Description ?? existingProduct.Description;
                existingProduct.Amount = newAmount;

                _unitOfWork.ProductRepository.Update(existingProduct);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task DeleteProductAsync(Guid id)
        {
            var existingProduct = _unitOfWork.ProductRepository.GetByID(id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }

            _unitOfWork.ProductRepository.Delete(id);
            _unitOfWork.Save();
        }

        private bool ProductExists(Guid id)
        {
            return _unitOfWork.ProductRepository.GetByID(id) != null ? true : false;
        }
    }

}
