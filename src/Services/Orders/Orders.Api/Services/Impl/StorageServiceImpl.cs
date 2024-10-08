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
            var amount = context.Message.Amount;

            await SaveOrderToStorage(item, size, productBySize, existingProduct, amount);
        }

        private async Task SaveOrderToStorage(OrderItemDto item, Size size, SizeProduct productBySize, Product existingProduct, int amount)
        {
            try
            {
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
