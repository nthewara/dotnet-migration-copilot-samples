# 03.01-core-foundation: Migrate shared Core foundation, startup, config, data, and BaseController

# 03.01-core-foundation: Migrate shared Core foundation, startup, config, data, and BaseController

## Objective
Prepare `ContosoUniversity.Core` for controller migration by moving shared MVC foundation concerns into the ASP.NET Core project. Scope includes `Program.cs`, EF Core data access registration, database initialization currently in `Global.asax.cs`, configuration access patterns, common models/data/services needed by controllers, and shared `BaseController` behavior.

## Research Context
The parent task found the old app uses default MVC routing, global `HandleErrorAttribute`, `Global.asax.cs` database initialization, `DefaultConnection`, `NotificationQueuePath`, and a shared `BaseController` with fixed audit user behavior. No active auth attributes or custom modules/handlers were found. The new Core project already has `/health`, YARP forwarding, System.Web adapters, `IHttpContextAccessor`, controller routing, and initial config values.

## Execution Notes
Query assessment/project data before editing. Keep the old Framework project present. Use `dotnet build` for `ContosoUniversity.Core` and full-solution MSBuild for final validation. Do not migrate individual concrete controllers here except shared base/foundation code.

**Done when**: Core project has necessary shared data/config/startup registrations for migrated controllers, database initialization is represented in ASP.NET Core startup/service patterns, `BaseController` or equivalent shared behavior compiles in Core, and the Core project plus full solution build with zero errors and warnings.

## Research Findings

### Projects Affected
- `ContosoUniversity.Core/ContosoUniversity.Core.csproj` — receives shared models, EF Core data access, controller base class, notification abstraction, and startup registrations.
- `ContosoUniversity/ContosoUniversity.csproj` — remains unchanged for this subtask and continues to build as the legacy Framework app.

### Assessment and Source Signals
- Assessment identifies legacy configuration, System.Web/MVC startup, and EF Core package upgrades as migration concerns.
- Existing `SchoolContext` already uses EF Core `DbContextOptions<SchoolContext>` and is suitable for ASP.NET Core DI registration.
- `Global.asax.cs` initializes the database by creating a `SchoolContext` from `DefaultConnection` and calling `DbInitializer.Initialize(context)`.
- `BaseController` in the Framework project directly creates `SchoolContextFactory.Create()` and `NotificationService`; Core needs DI-managed equivalents.

### Files Added to Core Foundation
- `ContosoUniversity.Core/Models/**` — copied domain and view model classes from the legacy project.
- `ContosoUniversity.Core/Data/SchoolContext.cs` — copied EF Core context.
- `ContosoUniversity.Core/Data/DbInitializer.cs` — copied seeding/initialization logic.
- `ContosoUniversity.Core/PaginatedList.cs` — copied pagination helper used by controllers.
- `ContosoUniversity.Core/Controllers/BaseController.cs` — migrated shared controller base to ASP.NET Core MVC and constructor injection.
- `ContosoUniversity.Core/Services/INotificationService.cs` and `NullNotificationService.cs` — temporary notification abstraction for controller migration; MSMQ-backed behavior is handled by `03.02-notification-service-msmq`.

### Startup and Package Changes
- Added `Microsoft.EntityFrameworkCore.SqlServer` `10.0.9` to the Core project.
- Set nullable analysis to disabled in the Core project during migration to avoid warning noise from legacy model nullability annotations; nullable migration is not in the approved scope.
- Registered `SchoolContext` with `AddDbContext` using `DefaultConnection` from configuration.
- Registered `INotificationService` to `NullNotificationService` so migrated controllers can compile before MSMQ replacement is complete.
- Moved database initialization into `Program.cs` via a scoped service provider and `DbInitializer.Initialize(context)`, with startup warning logging if local database initialization is unavailable.

### Validation
- `dotnet build ContosoUniversity.Core.csproj` succeeded with 0 errors and 0 warnings.
- Full solution MSBuild succeeded with 0 errors and 0 warnings.
- Runtime smoke check started `ContosoUniversity.Core` and verified `https://localhost:7024/health` returned HTTP 200 with body `"ok"`.

### Decomposition Decision
- This subtask was executed atomically because it only establishes shared foundation code and startup registrations. Controller-specific migration remains isolated in later subtasks.
