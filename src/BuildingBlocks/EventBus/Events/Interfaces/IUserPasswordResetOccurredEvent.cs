namespace EventBus.Events.Interfaces
{
    public interface IUserPasswordResetOccurredEvent
    {
        string Email { get; set; }
        string Code { get; set; }
    }
}
