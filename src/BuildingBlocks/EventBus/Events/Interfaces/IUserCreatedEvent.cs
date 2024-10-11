namespace EventBus.Events.Interfaces
{
    public interface IUserCreatedEvent
    {
        //string UserId { get; set; }
        //string Email { get; set; }
        //string UserName { get; set; }
        //string Address { get; set; }
        string CallbackUrl { get; set; }

        public Guid UserId { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Password { get; set; }

        public string? Role { get; set; }

        public string? ImgUrl { get; set; }
    }
}
