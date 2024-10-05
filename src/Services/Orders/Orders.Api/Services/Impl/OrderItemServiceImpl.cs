using DataAccess.Base;
using DataAccess.Models;
using Orders.Api.Dtos;

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
            //check stock ??
            try
            {
                var insertOrderItem = new OrderItem()
                {
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice,
                };

                _unitOfWork.OrderitemRepository.Insert(insertOrderItem);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateOrderItem(List<OrderItemDto> orderItemList, Guid orderId)
        {
            await DeleteOrderItem(orderId);

            foreach (var item in orderItemList)
            {
                //check stock ??

                await InsertOrderItem(item, orderId);
            }
        }
    }
}
