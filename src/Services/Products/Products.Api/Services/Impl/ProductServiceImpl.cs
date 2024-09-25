using DataAccess.Base;
using DataAccess.Models;
using Products.Api.Dtos.Request;

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
                    var pointSize = _unitOfWork.SizeRepository.Get(
                        filter: s => s.Name.Equals(item.SizeName)
                        ).FirstOrDefault();

                    if (pointSize == null)
                    {
                        throw new KeyNotFoundException($"Size with name {item.SizeName} not found.");
                    }

                    var insertSize = new SizeProduct()
                    {
                        ProductId = insertProduct.ProductId,
                        SizeId = pointSize.SizeId,
                        SizeQuantity = item.SizeQuantity
                    };

                    amount += item.SizeQuantity;

                    _unitOfWork.SizeproductRepository.Insert(insertSize);
                    _unitOfWork.Save();
                };

                existingProduct = _unitOfWork.ProductRepository.GetByID(insertProduct.ProductId);
                existingProduct.Amount = amount;
                await UpdateProductAsync(existingProduct);

                return insertProduct;
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
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
