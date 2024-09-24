using EventBus.Events.Interfaces;
using MassTransit;
using Notification.Services;

namespace Notification.Consumers
{
    public class UserCreatedEventConsumer : IConsumer<IUserCreatedEvent>
    {
        private readonly ILogger<UserCreatedEventConsumer> _logger;
        private readonly MessageServices _messageServices;

        public UserCreatedEventConsumer(ILogger<UserCreatedEventConsumer> logger, MessageServices messageServices)
        {
            _logger = logger;
            _messageServices = messageServices;
        }

        public async Task Consume(ConsumeContext<IUserCreatedEvent> context)
        {
            _logger.LogInformation("User created successfully!");
            await _messageServices.SendEmailAsync(context.Message.Email, "Confirm your account",
                "Please confirm your account by clicking this link: <a href=\"" + context.Message.CallbackUrl + "\">link</a>");
        }
    }
}
