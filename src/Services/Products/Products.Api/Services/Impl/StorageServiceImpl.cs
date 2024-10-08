using DataAccess.Base;
using DataAccess.Models;
using EventBus.Events.Interfaces;
using MassTransit;

namespace Orders.Api.Services.Impl
{
    public class StorageServiceImpl : IConsumer<IOrderCreatedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public StorageServiceImpl(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<IOrderCreatedEvent> context)
        {
            var item = context.Message.OrderItem;
            var size = context.Message.SizeEntity;
            var productBySize = context.Message.ProductBySize;
            var existingProduct = context.Message.ExistingProduct;

            await SaveOrderToStorage(item, size, productBySize, existingProduct);
        }

        private async Task SaveOrderToStorage(OrderItemDto item, Size? size, SizeProduct? productBySize, Product? existingProduct)
        {
            try
            {
                size = _unitOfWork.SizeRepository.Get(
                        filter: s => s.Name.Equals(item.SizeName)
                        ).FirstOrDefault();

                productBySize = _unitOfWork.SizeproductRepository.Get(
                    filter: p => p.ProductId.Equals(item.ProductId) && p.SizeId.Equals(size.SizeId)
                    ).FirstOrDefault();

                existingProduct = _unitOfWork.ProductRepository.Get(
                    filter: p => p.ProductId.Equals(item.ProductId),
                    noTracking: true
                    ).FirstOrDefault();

                if (size == null)
                {
                    throw new Exception($"Size not found.");
                }

                if (productBySize == null)
                {
                    throw new Exception($"Product with ID {item.ProductId} and size {size.SizeId} not found.");
                }

                if (existingProduct == null)
                {
                    throw new KeyNotFoundException($"Product with id {item.ProductId} is not found.");
                }

                if (item.Quantity > existingProduct.Amount)
                {
                    throw new Exception("Not enough amount of product.");
                }

                if (item.Quantity > productBySize.SizeQuantity)
                {
                    throw new Exception($"Not enough size quantity {size.Name} of product.");
                }
                existingProduct.Amount -= item.Quantity;
                _unitOfWork.ProductRepository.Update(existingProduct);
                _unitOfWork.Save();

                productBySize.SizeQuantity -= item.Quantity;
                _unitOfWork.SizeproductRepository.Update(productBySize);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
