# Migration Plan: Microsoft Message Queuing (MSMQ) to Azure Service Bus

## Migration Request

Migrate this codebase from **Microsoft Message Queuing (MSMQ)** to **Azure Service Bus**, focusing **exclusively** on code-level changes required for successful compilation.

**Autonomous Execution Mode**: This migration MUST be executed autonomously without pausing for user confirmation at any step. Once you begin, continue until all tasks are complete or an unrecoverable error occurs.

## Project Overview

### Projects in Solution
1. **ContosoUniversity** (.NET Framework 4.8)
   - ASP.NET MVC application
   - Uses System.Messaging for MSMQ
   - Configuration in Web.config

2. **ContosoUniversity.Core** (.NET 10.0)
   - ASP.NET Core application
   - Uses MSMQ.Messaging package (version 1.0.4)
   - Configuration in appsettings.json

### Current MSMQ Implementation Analysis

#### Files Using MSMQ:
1. **ContosoUniversity\Services\NotificationService.cs**
   - Uses System.Messaging namespace
   - Creates/manages local MSMQ queue: .\Private$\ContosoUniversityNotifications
   - Sends messages using MessageQueue.Send()
   - Receives messages using MessageQueue.Receive() with timeout
   - Uses XML message formatter
   - Serializes messages as JSON strings

2. **ContosoUniversity.Core\Services\NotificationService.cs**
   - Uses MSMQ.Messaging namespace
   - Same queue management logic as .NET Framework version
   - Implements INotificationService interface
   - Uses dependency injection with IOptions<MessageQueueOptions>

3. **ContosoUniversity.Core\Services\INotificationService.cs**
   - Interface defining notification service contract
   - Methods: SendNotification (2 overloads), ReceiveNotification, MarkAsRead

4. **ContosoUniversity.Core\Services\MessageQueueOptions.cs**
   - Configuration class for queue path

#### Configuration Files:
1. **ContosoUniversity\Web.config**
   - AppSetting: NotificationQueuePath = .\Private$\ContosoUniversityNotifications

2. **ContosoUniversity.Core\appsettings.json**
   - NotificationQueuePath setting
   - MessageQueue section with QueuePath

#### Usage Pattern:
- **Sending**: Controllers inherit from BaseController which uses INotificationService to send notifications when entities are created/updated/deleted
- **Receiving**: NotificationsController polls for messages using ReceiveNotification()
- **Message Format**: JSON-serialized Notification objects with properties: EntityType, EntityId, Operation, Message, CreatedAt, CreatedBy, IsRead
- **Queue Type**: Simple queue (no routing keys, no topics)

### Azure Service Bus Migration Strategy

#### Target Architecture:
- **Queue-based approach** (not Topic/Subscription) since no routing keys are used
- **Queue Name**: contoso-university-notifications (lowercase, Azure-friendly naming)
- **Authentication**: Azure Managed Identity (DefaultAzureCredential)
- **Connection**: Fully Qualified Namespace format

#### Required Changes:

1. **Dependencies**:
   - Add Azure.Messaging.ServiceBus version 7.19.0
   - Add Azure.Identity version 1.14.0 (already present in ContosoUniversity.Core)
   - Remove System.Messaging reference (ContosoUniversity)
   - Remove MSMQ.Messaging package (ContosoUniversity.Core)

2. **Configuration**:
   - Replace queue path with Service Bus namespace
   - Add AzureServiceBus:FullyQualifiedNamespace setting
   - Format: {namespace}.servicebus.windows.net

3. **Code Changes**:
   - Replace MessageQueue with ServiceBusClient
   - Replace Message with ServiceBusMessage
   - Replace synchronous operations with async/await pattern
   - Update message sending logic
   - Update message receiving logic
   - Handle connection management differently (ServiceBusClient is long-lived)

4. **Message Format Compatibility**:
   - Keep JSON serialization
   - Message body will be UTF-8 encoded string
   - No need for XML formatter

5. **Interface Updates**:
   - Change methods to async: SendNotificationAsync, ReceiveNotificationAsync
   - Update all usages in controllers

## Required Packages

### ContosoUniversity (.NET Framework 4.8)
- **Add**: 
  - Azure.Messaging.ServiceBus version 7.19.0
  - Azure.Identity version 1.14.0
- **Remove**: 
  - System.Messaging reference

### ContosoUniversity.Core (.NET 10.0)
- **Add**: 
  - Azure.Messaging.ServiceBus version 7.19.0
- **Keep**: 
  - Azure.Identity version 1.21.0 (already present)
- **Remove**: 
  - MSMQ.Messaging version 1.0.4

## Scope

* DO - Maintain .NET Framework 4.8 for ContosoUniversity project
* DO - Maintain .NET 10.0 for ContosoUniversity.Core project
* DO - Replace all MSMQ dependencies with Azure Service Bus
* DO - Update configuration files for Azure Service Bus
* DO - Convert synchronous queue operations to asynchronous Service Bus operations
* DO - Update interface to support async operations
* DO - Update all controllers to use async notification methods
* DO - Maintain existing business logic and notification functionality
* DO NOT - Add new features beyond migration requirements
* DO NOT - Change the notification data model
* DO NOT - Perform deployment or infrastructure setup
* DO NOT - Add database persistence for notifications (out of scope)

## Success Criteria

1. All MSMQ dependencies are removed from both projects
2. Azure Service Bus packages are properly installed
3. All code files using MSMQ are updated to use Azure Service Bus
4. Configuration files are updated with Service Bus settings
5. Interface and implementations are converted to async patterns
6. All controller usages are updated to async/await
7. Both projects compile successfully
8. All migration tasks are tracked and completed
9. Changes are committed to version control

## Migration Tasks Overview

1. Version control setup and baseline
2. Remove old MSMQ dependencies
3. Add Azure Service Bus dependencies
4. Update configuration files
5. Migrate service interface to async
6. Migrate NotificationService implementation (ContosoUniversity)
7. Migrate NotificationService implementation (ContosoUniversity.Core)
8. Update MessageQueueOptions to ServiceBusOptions
9. Update BaseController to use async notifications
10. Update all controller usages
11. Build verification and error fixes
12. Completeness validation
13. Consistency validation
14. CVE vulnerability check
15. Final verification and commit
