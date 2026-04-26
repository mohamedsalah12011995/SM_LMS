namespace Notification.Consumer.Interfaces
{
    public interface INotificationService
    {
        Task ReadMessage(CancellationToken cancellationToken);

    }
}
