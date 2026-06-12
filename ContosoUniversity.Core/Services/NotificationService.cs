using System.Diagnostics;
using System.Text.Json;
using ContosoUniversity.Models;
using Microsoft.Extensions.Options;
using MSMQ.Messaging;

namespace ContosoUniversity.Services;

public sealed class NotificationService : INotificationService
{
    private readonly string _queuePath;
    private readonly MessageQueue _queue;

    public NotificationService(IOptions<MessageQueueOptions> options)
    {
        _queuePath = string.IsNullOrWhiteSpace(options.Value.QueuePath)
            ? @".\Private$\ContosoUniversityNotifications"
            : options.Value.QueuePath;

        try
        {
            if (!MessageQueue.Exists(_queuePath))
            {
                _queue = MessageQueue.Create(_queuePath);
                _queue.SetPermissions("Everyone", MessageQueueAccessRights.FullControl);
            }
            else
            {
                _queue = new MessageQueue(_queuePath);
            }

            _queue.Formatter = new XmlMessageFormatter(new[] { typeof(string) });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Notifications disabled: {ex.Message}");
        }
    }

    public void SendNotification(string entityType, string entityId, EntityOperation operation, string userName = null)
    {
        SendNotification(entityType, entityId, null, operation, userName);
    }

    public void SendNotification(string entityType, string entityId, string entityDisplayName, EntityOperation operation, string userName = null)
    {
        try
        {
            if (_queue is null)
            {
                return;
            }

            var notification = new Notification
            {
                EntityType = entityType,
                EntityId = entityId,
                Operation = operation.ToString(),
                Message = GenerateMessage(entityType, entityId, entityDisplayName, operation),
                CreatedAt = DateTime.Now,
                CreatedBy = userName ?? "System",
                IsRead = false
            };

            var jsonMessage = JsonSerializer.Serialize(notification);
            var message = new Message(jsonMessage)
            {
                Label = $"{entityType} {operation}",
                Priority = MessagePriority.Normal
            };

            _queue.Send(message);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to send notification: {ex.Message}");
        }
    }

    public Notification ReceiveNotification()
    {
        try
        {
            if (_queue is null)
            {
                return null;
            }

            var message = _queue.Receive(TimeSpan.FromSeconds(1));
            var jsonContent = message.Body?.ToString();
            return string.IsNullOrWhiteSpace(jsonContent)
                ? null
                : JsonSerializer.Deserialize<Notification>(jsonContent);
        }
        catch (MessageQueueException ex) when (ex.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
        {
            return null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to receive notification: {ex.Message}");
            return null;
        }
    }

    public void MarkAsRead(int notificationId)
    {
        // In a real implementation, notifications would also be persisted so read state survives queue reads.
    }

    private static string GenerateMessage(string entityType, string entityId, string entityDisplayName, EntityOperation operation)
    {
        var displayText = !string.IsNullOrWhiteSpace(entityDisplayName)
            ? $"{entityType} '{entityDisplayName}'"
            : $"{entityType} (ID: {entityId})";

        return operation switch
        {
            EntityOperation.CREATE => $"New {displayText} has been created",
            EntityOperation.UPDATE => $"{displayText} has been updated",
            EntityOperation.DELETE => $"{displayText} has been deleted",
            _ => $"{displayText} operation: {operation}"
        };
    }

    public void Dispose()
    {
        _queue?.Dispose();
    }
}
