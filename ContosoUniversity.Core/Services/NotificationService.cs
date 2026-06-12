using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using ContosoUniversity.Models;
using Microsoft.Extensions.Options;

namespace ContosoUniversity.Services;

public sealed class NotificationService : INotificationService
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly ServiceBusSender _sender;
    private readonly ServiceBusReceiver _receiver;
    private readonly string _queueName;

    public NotificationService(IOptions<ServiceBusOptions> options)
    {
        var fullyQualifiedNamespace = options.Value.FullyQualifiedNamespace;
        _queueName = string.IsNullOrWhiteSpace(options.Value.QueueName)
            ? "contoso-university-notifications"
            : options.Value.QueueName;

        try
        {
            if (string.IsNullOrWhiteSpace(fullyQualifiedNamespace))
            {
                Debug.WriteLine("Azure Service Bus not configured. Notifications disabled.");
                return;
            }

            var credential = new DefaultAzureCredential();
            _serviceBusClient = new ServiceBusClient(fullyQualifiedNamespace, credential);
            _sender = _serviceBusClient.CreateSender(_queueName);
            _receiver = _serviceBusClient.CreateReceiver(_queueName);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Notifications disabled: {ex.Message}");
        }
    }

    public async Task SendNotificationAsync(string entityType, string entityId, EntityOperation operation, string userName = null)
    {
        await SendNotificationAsync(entityType, entityId, null, operation, userName);
    }

    public async Task SendNotificationAsync(string entityType, string entityId, string entityDisplayName, EntityOperation operation, string userName = null)
    {
        try
        {
            if (_sender is null)
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
            var serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                Subject = $"{entityType} {operation}",
                ContentType = "application/json"
            };

            await _sender.SendMessageAsync(serviceBusMessage);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to send notification: {ex.Message}");
        }
    }

    public async Task<Notification> ReceiveNotificationAsync()
    {
        try
        {
            if (_receiver is null)
            {
                return null;
            }

            var message = await _receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(1));

            if (message is null)
            {
                return null;
            }

            var jsonContent = Encoding.UTF8.GetString(message.Body.ToArray());
            var notification = JsonSerializer.Deserialize<Notification>(jsonContent);

            // Complete the message to remove it from the queue
            await _receiver.CompleteMessageAsync(message);

            return notification;
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
        _sender?.DisposeAsync().AsTask().GetAwaiter().GetResult();
        _receiver?.DisposeAsync().AsTask().GetAwaiter().GetResult();
        _serviceBusClient?.DisposeAsync().AsTask().GetAwaiter().GetResult();
    }
}
