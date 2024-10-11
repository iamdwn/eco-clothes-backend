using DataAccess.Base;
using DataAccess.Models;
using EventBus.Events.Interfaces;

namespace Orders.Api.Services.Impl
{
    public class OrderItemServiceImpl : IOrderItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderItemServiceImpl(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteOrderItem(Guid orderId)
        {
            var orderItems = _unitOfWork.OrderitemRepository
                .Get(filter: o => o.OrderId == orderId)
                .ToList();

            if (!orderItems.Any())
            {
                {
                    throw new KeyNotFoundException($"OrderItems with id {orderId} not found.");
                }
            }

            _unitOfWork.OrderitemRepository.DeleteRange(orderItems);
            _unitOfWork.Save();
        }

        public async Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync()
        {
            return _unitOfWork.OrderitemRepository.Get().ToList();
        }

        public async Task InsertOrderItem(OrderItemDto item, Guid orderId)
        {
            try
            {
                var size = _unitOfWork.SizeRepository.Get(
                    filter: s => s.Name.Equals(item.SizeName)
                    ).FirstOrDefault();

                if (size == null) throw new Exception($"Not found size with name {item.SizeName}");

                var insertOrderItem = new OrderItem()
                {
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.Quantity * item.UnitPrice,
                    SizeId = size.SizeId
                };

                _unitOfWork.OrderitemRepository.Insert(insertOrderItem);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task UpdateOrderItem(List<OrderItemDto> orderItemList, Guid orderId)
        {
            await DeleteOrderItem(orderId);

            foreach (var item in orderItemList)
            {
                await InsertOrderItem(item, orderId);
            }
        }
    }
}
