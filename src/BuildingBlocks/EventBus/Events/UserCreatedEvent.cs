using EventBus.Events.Interfaces;

namespace EventBus.Events
{
    public class UserCreatedEvent : IUserCreatedEvent
    {
        //public string UserId { get; set; }
        //public string Email { get; set; }
        //public string UserName { get; set; }
        //public string Address { get; set; }
        //public string CallbackUrl { get; set; }

        public Guid UserId { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Password { get; set; }

        public string? Role { get; set; }

        public string? ImgUrl { get; set; }
    }
}
