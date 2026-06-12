# 03.08-notifications-controller: Migrate NotificationsController to ASP.NET Core

# 03.08-notifications-controller: Migrate NotificationsController to ASP.NET Core

## Objective
Migrate `NotificationsController` and its JSON endpoints to ASP.NET Core MVC after notification service/MSMQ replacement work is complete. Scope includes JSON result behavior, POST action migration, and service dependency wiring.

## Research Context
Controller triage: 3 actions, 1 POST action, no auth, JSON endpoints, depends on notification service behavior. Serializer behavior may change from MVC 5/Newtonsoft-style JSON to ASP.NET Core/System.Text.Json defaults.

## Execution Notes
Use `get_code_dependencies` for `ContosoUniversity/Controllers/NotificationsController.cs` before editing. Verify JSON property casing/shape and update `JsonResult` usage as needed. This subtask depends on `03.02-notification-service-msmq`.

**Done when**: `NotificationsController` compiles in `ContosoUniversity.Core`, JSON endpoints use ASP.NET Core result patterns with acceptable response shape, notification service dependencies are wired, and the Core project builds with zero errors and warnings.

## Research Findings

### Projects Affected
- `ContosoUniversity.Core/ContosoUniversity.Core.csproj` — receives the migrated `NotificationsController`, Notifications view, and a minimal shared layout required by the view.
- `ContosoUniversity/ContosoUniversity.csproj` — remains present and unchanged as the legacy side-by-side source/proxy target.

### Assessment and Dependency Findings
- File assessment for `Controllers/NotificationsController.cs` reported 23 API issues, primarily `System.Web.Mvc.JsonResult`, `JsonRequestBehavior.AllowGet`, MVC attributes, and `ActionResult`/`View()` usage.
- `get_code_dependencies` showed the default conventional route `{controller}/{action}/{id}`, Notifications `Index` view, `Notification`, and `NotificationService` dependencies.
- The controller depends on notification service send/receive/mark-read behavior completed in `03.02-notification-service-msmq`.

### Migration Decisions
- Replaced `System.Web.Mvc` with ASP.NET Core MVC and changed action return types to `IActionResult`.
- Removed `JsonRequestBehavior.AllowGet` because ASP.NET Core allows JSON GET responses through `Json(...)` without that parameter.
- Preserved the JSON response shape used by the existing UI: `success`, `notifications`, `count`, and `message` remain lower-case property names in anonymous objects.
- Copied the Notifications view into Core and added a minimal shared layout because the view explicitly references `~/Views/Shared/_Layout.cshtml`. Broader layout/static asset refinement remains in `03.09-views-and-static-assets`.

### Files Modified
- `ContosoUniversity.Core/Controllers/NotificationsController.cs` — migrated controller.
- `ContosoUniversity.Core/Views/Notifications/Index.cshtml` — direct Notifications dashboard view.
- `ContosoUniversity.Core/Views/Shared/_Layout.cshtml` — minimal shared layout needed by the copied Notifications view and already-migrated views.

### Validation
- `dotnet build ContosoUniversity.Core.csproj` succeeded with 0 errors and 0 warnings.
- Full solution MSBuild succeeded with 0 errors and 0 warnings.
- Runtime smoke checks verified `/Notifications` returns HTTP 200 with `Admin Notifications` content and `/Notifications/GetNotifications` returns HTTP 200 with JSON `{\"success\":true,\"notifications\":[],\"count\":0}`.

### Decomposition Decision
- This subtask was executed atomically because it migrates one small controller, its direct view, and JSON endpoint behavior.
