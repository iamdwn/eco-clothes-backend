using MassTransit;
using EventBus.Events.Interfaces;
using Users.Api.Services.Interfaces;
using AutoMapper;
using DataAccess.Models;
using Users.Api.Dtos.Request;

namespace Users.Api.Consumers
{
    public class UserCreatedEventConsumer : IConsumer<IUserCreatedEvent>
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserCreatedEventConsumer> _logger;

        public UserCreatedEventConsumer(ILogger<UserCreatedEventConsumer> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public async Task Consume(ConsumeContext<IUserCreatedEvent> context)
        {
            var createUserDTO = new CreateUserDTO
            {
                Email = context.Message.Email,
                FullName = context.Message.FullName,
                ImgUrl = context.Message.ImgUrl,
                Password = context.Message.Password,
                Phone = context.Message.Phone,
                Role = context.Message.Role,
                UserId = context.Message.UserId
            };
            await _userService.CreateUser(createUserDTO);
            _logger.LogInformation("User created successfully!");
        }
    }
}
