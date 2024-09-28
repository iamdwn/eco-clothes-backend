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

        public async Task UpdateSize(SizeDto item, Guid productId)
        {
            var pointSize = _unitOfWork.SizeRepository.Get(
                       filter: s => s.Name.Equals(item.SizeName)
                       ).FirstOrDefault();

            if (pointSize == null)
            {
                throw new KeyNotFoundException($"Size with name {item.SizeName} not found.");
            }

            var sizeProduct = _unitOfWork.SizeproductRepository.Get(
                    filter: s => s.SizeId.Equals(pointSize.SizeId)
                                   && s.ProductId.Equals(productId))
                    .FirstOrDefault();

            if (sizeProduct == null)
            {
                throw new KeyNotFoundException($"SizeProduct not found.");
            }

            sizeProduct.SizeQuantity = item.SizeQuantity;

            _unitOfWork.SizeproductRepository.Update(sizeProduct);
            _unitOfWork.Save();
        }
    }
}
