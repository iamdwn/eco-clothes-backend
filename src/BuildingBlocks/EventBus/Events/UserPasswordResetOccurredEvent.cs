using EventBus.Events.Interfaces;

namespace EventBus.Events
{
    public class UserPasswordResetOccurredEvent : IUserPasswordResetOccurredEvent
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
