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

            await ApproveOrder(response, transaction);
        }

        private async Task ApproveOrder(bool response, string? transaction)
        {
            Payment? pointTransaction = null;

            if (response)
            {
                pointTransaction = _unitOfWork.PaymentRepository.Get(
                    filter: p => p.TransactionId.Equals(transaction)
                    ).FirstOrDefault();

                pointTransaction.Status = "Success";
                return;
            }

            pointTransaction.Status = "Fail";
        }
    }
}
