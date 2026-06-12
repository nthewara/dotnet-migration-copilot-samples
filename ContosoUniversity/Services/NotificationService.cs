using System;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using ContosoUniversity.Models;
using Newtonsoft.Json;

namespace ContosoUniversity.Services
{
    public class NotificationService
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusSender _sender;
        private readonly ServiceBusReceiver _receiver;
        private readonly string _queueName;

        public NotificationService()
        {
            // Get Azure Service Bus configuration
            var fullyQualifiedNamespace = ConfigurationManager.AppSettings["AzureServiceBus:FullyQualifiedNamespace"];
            _queueName = ConfigurationManager.AppSettings["AzureServiceBus:QueueName"] ?? "contoso-university-notifications";

            try
            {
                if (string.IsNullOrWhiteSpace(fullyQualifiedNamespace))
                {
                    System.Diagnostics.Debug.WriteLine("Azure Service Bus not configured. Notifications disabled.");
                    return;
                }

                var credential = new DefaultAzureCredential();
                _serviceBusClient = new ServiceBusClient(fullyQualifiedNamespace, credential);
                _sender = _serviceBusClient.CreateSender(_queueName);
                _receiver = _serviceBusClient.CreateReceiver(_queueName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Notifications disabled: {ex.Message}");
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
                if (_sender == null)
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

                var jsonMessage = JsonConvert.SerializeObject(notification);
                var serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
                {
                    Subject = $"{entityType} {operation}",
                    ContentType = "application/json"
                };

                // Use synchronous send (blocking call)
                _sender.SendMessageAsync(serviceBusMessage).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                // Log error but don't break the main operation
                System.Diagnostics.Debug.WriteLine($"Failed to send notification: {ex.Message}");
            }
        }

        public Notification ReceiveNotification()
        {
            try
            {
                if (_receiver == null)
                {
                    return null;
                }

                // Receive message with timeout
                var message = _receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(1)).GetAwaiter().GetResult();

                if (message == null)
                {
                    return null;
                }

                var jsonContent = Encoding.UTF8.GetString(message.Body.ToArray());
                var notification = JsonConvert.DeserializeObject<Notification>(jsonContent);

                // Complete the message to remove it from the queue
                _receiver.CompleteMessageAsync(message).GetAwaiter().GetResult();

                return notification;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to receive notification: {ex.Message}");
                return null;
            }
        }

        public void MarkAsRead(int notificationId)
        {
            // In a real implementation, you might want to store notifications in database as well
            // for persistence and tracking read status
        }

        private string GenerateMessage(string entityType, string entityId, string entityDisplayName, EntityOperation operation)
        {
            var displayText = !string.IsNullOrWhiteSpace(entityDisplayName) 
                ? $"{entityType} '{entityDisplayName}'" 
                : $"{entityType} (ID: {entityId})";

            switch (operation)
            {
                case EntityOperation.CREATE:
                    return $"New {displayText} has been created";
                case EntityOperation.UPDATE:
                    return $"{displayText} has been updated";
                case EntityOperation.DELETE:
                    return $"{displayText} has been deleted";
                default:
                    return $"{displayText} operation: {operation}";
            }
        }

        public void Dispose()
        {
            _sender?.DisposeAsync().GetAwaiter().GetResult();
            _receiver?.DisposeAsync().GetAwaiter().GetResult();
            _serviceBusClient?.DisposeAsync().GetAwaiter().GetResult();
        }
    }
}
