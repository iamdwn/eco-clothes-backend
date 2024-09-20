using EventBus.Events.Interfaces;
using MassTransit;
using Notification.Services;

namespace Notification.Consumers
{
    public class UserPasswordResetOccurredEventConsumer : IConsumer<IUserPasswordResetOccurredEvent>
    {
        private readonly ILogger<UserPasswordResetOccurredEventConsumer> _logger;
        private readonly MessageServices _messageService;

        public UserPasswordResetOccurredEventConsumer(ILogger<UserPasswordResetOccurredEventConsumer> logger, MessageServices messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }

        public async Task Consume(ConsumeContext<IUserPasswordResetOccurredEvent> context)
        {
            _logger.LogInformation("User reset password!");
            await _messageService.SendEmailAsync(context.Message.Email, "Reset Password",
               "Please reset your password by using code here: " + context.Message.Code);
        }
    }
}
