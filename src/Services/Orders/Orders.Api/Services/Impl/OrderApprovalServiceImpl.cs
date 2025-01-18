using DataAccess.Base;
using DataAccess.Models;
using EventBus.Events.Interfaces;
using MassTransit;

namespace Orders.Api.Services.Impl
{
    public class OrderApprovalServiceImpl : IConsumer<IPaymentResponseEvent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderApprovalServiceImpl(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<IPaymentResponseEvent> context)
        {
            var response = context.Message.PaymentStatus;
            var transaction = context.Message.Transaction;
            var orderCode = context.Message.OrderCode;

            await ApproveOrder(response, transaction, orderCode);
        }

        private async Task ApproveOrder(bool response, string? transaction, long orderCode)
        {
            Order? pointOrder = null;

            if (response)
            {
                pointOrder = _unitOfWork.OrderRepository.Get(
                    filter: p => orderCode == Math.Abs(BitConverter.ToInt32(p.OrderId.ToByteArray(), 8))
                    ).FirstOrDefault();

                pointOrder.Status = "Paid";

                _unitOfWork.OrderRepository.Update(pointOrder);
                _unitOfWork.Save();

                return;
            }

            pointOrder.Status = "Failed";
            _unitOfWork.OrderRepository.Update(pointOrder);
            _unitOfWork.Save();
            return;
        }
    }
}
