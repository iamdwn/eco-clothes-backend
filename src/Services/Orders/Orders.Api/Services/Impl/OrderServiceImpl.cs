using DataAccess.Base;
using DataAccess.Models;
using EventBus.Events;
using EventBus.Events.Interfaces;
using MassTransit;
using Orders.Api.Dtos;

namespace Orders.Api.Services.Impl
{
    public class OrderServiceImpl : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderItemService _orderItemService;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderServiceImpl(IUnitOfWork unitOfWork, IOrderItemService orderItemService, IPublishEndpoint publishEndpoint)
        {
            _unitOfWork = unitOfWork;
            _orderItemService = orderItemService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Order> CreateOrderAsync(OrderDto order)
        {
            try
            {
                int amount = 0;
                Size? size = null;
                SizeProduct? productBySize = null;
                Product? existingProduct = null;

                var existingUser = _unitOfWork.UserRepository.GetByID(order.UserId);

                if (existingUser == null)
                {
                    throw new KeyNotFoundException($"User with ID {order.UserId} not found.");
                }

                var insertOrder = new Order()
                {
                    UserId = order.UserId,
                    StartDate = DateOnly.FromDateTime(DateTime.Now),
                    EndDate = DateOnly.FromDateTime(DateTime.Now).AddDays(7),
                    Address = order.Address,
                    Username = existingUser.FullName
                };

                _unitOfWork.OrderRepository.Insert(insertOrder);
                _unitOfWork.Save();

                foreach (var item in order.OrderItems)
                {
                    await _orderItemService.InsertOrderItem(item, insertOrder.OrderId);

                    await _publishEndpoint.Publish(new OrderCreatedEvent
                    {
                        OrderItem = item,
                        SizeEntity = size,
                        ProductBySize = productBySize,
                        ExistingProduct = existingProduct
                    });
                }

                await _publishEndpoint.Publish(new OrderInformationForPaymentEvent
                {
                    UserId = order.UserId.ToString(),
                    Amount = amount,
                    OrderId = order.OrderId.ToString(),
                    PaymentMethod = order.PaymentMethod.Equals("VNPay")
                                            ? PaymentMethod.VNPay
                                            : (order.PaymentMethod.Equals("MoMo") ? PaymentMethod.MoMo
                                                                                  : PaymentMethod.Unidentified)
                }
                    );

                return insertOrder;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            var existingOrder = _unitOfWork.OrderRepository.GetByID(id);

            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Order with ID {id} not found.");
            }

            await _orderItemService.DeleteOrderItem(existingOrder.OrderId);

            _unitOfWork.OrderRepository.Delete(id);
            _unitOfWork.Save();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return _unitOfWork.OrderRepository.Get(
                includeProperties: "OrderItems"
                ).ToList();
        }

        public async Task<IEnumerable<Order>> GetOrderByBuyerIdAsync(Guid userId)
        {
            var existingOrder = _unitOfWork.OrderRepository.Get(
                filter: p => p.UserId.Equals(userId),
                includeProperties: "OrderItems"
                ).ToList();

            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Orders of user has ID is {userId} not found.");
            }

            return existingOrder;
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            var existingOrder = _unitOfWork.OrderRepository.Get(
                filter: p => p.OrderId.Equals(id),
                includeProperties: "OrderItems"
                ).FirstOrDefault();

            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Order with ID {id} not found.");
            }

            return existingOrder;
        }

        public async Task UpdateOrderAsync(OrderDto order)
        {
            try
            {
                var existingOrder = _unitOfWork.OrderRepository.GetByID(order.OrderId);

                if (existingOrder == null)
                {
                    throw new KeyNotFoundException($"Order with id {order.OrderId} not found.");
                }

                var existingUser = _unitOfWork.UserRepository.GetByID(order.UserId);

                if (existingUser == null)
                {
                    throw new KeyNotFoundException($"User with ID {order.UserId} not found.");
                }

                await _orderItemService.UpdateOrderItem(order.OrderItems, existingOrder.OrderId);

                existingOrder.StartDate = order.StartDate ?? existingOrder.StartDate;
                existingOrder.EndDate = order.EndDate ?? existingOrder.EndDate;
                existingOrder.Address = order.Address ?? existingOrder.Address;
                existingOrder.Username = existingUser.FullName ?? "";

                _unitOfWork.OrderRepository.Update(existingOrder);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool OrdertExists(Guid id)
        {
            return _unitOfWork.OrderRepository.GetByID(id) != null ? true : false;
        }
    }
}
