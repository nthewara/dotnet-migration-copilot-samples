using ContosoUniversity.Models;

namespace ContosoUniversity.Services;

public interface INotificationService : IDisposable
{
    Task SendNotificationAsync(string entityType, string entityId, EntityOperation operation, string userName = null);
    Task SendNotificationAsync(string entityType, string entityId, string entityDisplayName, EntityOperation operation, string userName = null);
    Task<Notification> ReceiveNotificationAsync();
    void MarkAsRead(int notificationId);
}
