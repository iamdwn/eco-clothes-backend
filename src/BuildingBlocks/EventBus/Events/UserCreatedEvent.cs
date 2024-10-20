using EventBus.Events.Interfaces;

namespace EventBus.Events
{
    public class UserCreatedEvent : IUserCreatedEvent
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
    }
}
