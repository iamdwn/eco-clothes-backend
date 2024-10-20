namespace EventBus.Constants
{
    public class QueuesConsts
    {
        //event
        public const string UserCreatedEventQueueName = "user-created-event-queue";
        public const string UserPasswordResetOccurredQueueName = "user-password-reset-occurred-event-queue";
        public const string OrderCreated = "order-created-queue";
        public const string PaymentApproval = "order-approval-queue";
        public const string OrderInformationForPayment = "order-information-for-payment";
    }
}
