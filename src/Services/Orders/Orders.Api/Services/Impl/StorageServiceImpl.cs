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
            var orderItems = context.Message.OrderItems;
            var size = context.Message.SizeEntity;
            var productBySize = context.Message.ProductBySize;
            var existingProduct = context.Message.ExistingProduct;

            await SaveOrderToStorage(orderItems, size, productBySize, existingProduct);
        }

        private async Task SaveOrderToStorage(List<OrderItemDto> orderItems, Size size, SizeProduct productBySize, Product existingProduct)
        {
            try
            {
                int amount = 0;
                foreach (var item in orderItems)
                {
                    amount += item.Quantity;

                    productBySize.SizeQuantity -= item.Quantity;

                    _unitOfWork.SizeproductRepository.Update(productBySize);
                    _unitOfWork.Save();
                }

                existingProduct.Amount -= amount;
                _unitOfWork.ProductRepository.Update(existingProduct);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
