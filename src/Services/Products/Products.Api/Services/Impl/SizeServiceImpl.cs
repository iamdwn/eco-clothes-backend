using DataAccess.Base;
using DataAccess.Models;
using Products.Api.Dtos;

namespace Products.Api.Services.Impl
{
    public class SizeServiceImpl : ISizeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SizeServiceImpl(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteSize(Guid productId)
        {
            var sizeProducts = _unitOfWork.SizeproductRepository
                    .Get(filter: sp => sp.ProductId == productId)
                    .ToList();

            if (!sizeProducts.Any())
            {
                {
                    throw new KeyNotFoundException($"SizeProduct with id {productId} not found.");
                }
            }
            _unitOfWork.SizeproductRepository.DeleteRange(sizeProducts);

            _unitOfWork.Save();
        }

        public async Task<IEnumerable<Size>> GetAllSizesAsync()
        {
            return _unitOfWork.SizeRepository.Get().ToList();
        }

        public async Task InsertSize(SizeDto item, Guid productId)
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
                ProductId = productId,
                SizeId = pointSize.SizeId,
                SizeQuantity = item.SizeQuantity
            };

            _unitOfWork.SizeproductRepository.Insert(insertSize);
            _unitOfWork.Save();
        }

        public async Task UpdateSize(List<SizeDto> sizeList, Guid productId)
        {
            await DeleteSize(productId);

            foreach (var item in sizeList)
            {
                var pointSize = _unitOfWork.SizeRepository.Get(
                           filter: s => s.Name.Equals(item.SizeName)
                           ).FirstOrDefault();

                if (pointSize == null)
                {
                    throw new KeyNotFoundException($"Size with name {item.SizeName} not found.");
                }

                await InsertSize(item, productId);
            }
        }
    }
}
