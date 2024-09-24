namespace EventBus.Events.Interfaces
{
    public interface IUserCreatedEvent
    {
        string UserId { get; set; }
        string Email { get; set; }
        string UserName { get; set; }
        string Address { get; set; }
        string CallbackUrl { get; set; }
    }
}
