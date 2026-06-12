# 03.02-notification-service-msmq: Replace or adapt MSMQ notification service for ASP.NET Core

# 03.02-notification-service-msmq: Replace or adapt MSMQ notification service for ASP.NET Core

## Objective
Isolate the System.Messaging/MSMQ migration needed by notification functionality. Scope includes `Services/NotificationService.cs`, notification models used by `NotificationsController`, `NotificationQueuePath` configuration, and package/API decisions needed because `System.Messaging` is not directly available as a cross-platform .NET Core API.

## Research Context
The assessment flags MSMQ/message queuing issues. `NotificationService` uses `MessageQueue`, creates the queue when absent, sets permissions, catches `MessageQueueException`, and reads `ConfigurationManager.AppSettings["NotificationQueuePath"]`. The approved options require resolving API/package incompatibilities inline rather than leaving stubs unless blocked.

## Execution Notes
Research available replacement approach before coding. Prefer a small local abstraction that preserves app behavior for this sample if no production queue replacement is required; document any behavioral tradeoff. Update Core configuration access to use `IConfiguration`. Build after changes.

**Done when**: Notification service functionality needed by the Core app compiles without `System.Messaging` references in `ContosoUniversity.Core`, configuration comes from `IConfiguration`, notification endpoints have a service implementation they can call, and affected projects build with zero errors and warnings.

## Research Findings

### Projects Affected
- `ContosoUniversity.Core/ContosoUniversity.Core.csproj` — receives the migrated notification service and MSMQ package replacement.
- `ContosoUniversity/ContosoUniversity.csproj` — remains unchanged and continues to use the existing Framework implementation while the side-by-side migration is in progress.

### Source Findings
- Legacy `ContosoUniversity/Services/NotificationService.cs` used `System.Messaging`, `MessageQueue`, `Message`, `XmlMessageFormatter`, `MessageQueueException`, `ConfigurationManager.AppSettings["NotificationQueuePath"]`, and Newtonsoft.Json serialization.
- `NotificationsController` depends on `ReceiveNotification()` and `MarkAsRead(int)`, so the Core service abstraction needed these members before controller migration.
- Existing Core configuration already had `NotificationQueuePath`; the MSMQ migration skill recommends an options-bound `MessageQueue` section.

### Package and API Decisions
- Replaced `System.Messaging` with `MSMQ.Messaging` `1.0.4` in `ContosoUniversity.Core`.
- Added a direct `System.Drawing.Common` `10.0.0` package reference to override the vulnerable transitive `4.7.0` version introduced by `MSMQ.Messaging`.
- Used `System.Text.Json` for notification serialization in the Core implementation instead of adding Newtonsoft.Json to the Core project.
- Added `MessageQueueOptions` and bound it from `IConfiguration` via `builder.Services.Configure<MessageQueueOptions>(builder.Configuration.GetSection("MessageQueue"))`.

### Files Modified
- `ContosoUniversity.Core/Services/MessageQueueOptions.cs` — queue options model.
- `ContosoUniversity.Core/Services/NotificationService.cs` — MSMQ.Messaging-backed Core implementation.
- `ContosoUniversity.Core/Services/INotificationService.cs` — expanded contract for send/receive/mark-read operations used by controllers.
- `ContosoUniversity.Core/Program.cs` — registers options and `NotificationService`.
- `ContosoUniversity.Core/appsettings.json` — adds `MessageQueue:QueuePath`.
- `ContosoUniversity.Core/ContosoUniversity.Core.csproj` — adds package references for `MSMQ.Messaging` and patched `System.Drawing.Common`.
- Removed the temporary `NullNotificationService` that was only used for the foundation migration.

### Validation
- `dotnet build ContosoUniversity.Core.csproj` succeeded with 0 errors and 0 warnings.
- Full solution MSBuild succeeded with 0 errors and 0 warnings.
- Search in `ContosoUniversity.Core/**/*.cs` found no remaining `System.Messaging`, `ConfigurationManager`, or `NullNotificationService` references.

### Decomposition Decision
- This subtask was executed atomically because the scope was a single service/API replacement plus configuration and DI registration.
