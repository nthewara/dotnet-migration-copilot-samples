# 03.03-home-controller: Migrate HomeController to ASP.NET Core

# 03.03-home-controller: Migrate HomeController to ASP.NET Core

## Objective
Migrate `HomeController` and its direct dependencies from System.Web MVC to ASP.NET Core MVC in `ContosoUniversity.Core`. This is the simplest concrete controller and should establish the controller migration pattern.

## Research Context
Controller triage: 5 actions, no POST actions, no auth, no uploads, MVC views. It inherits from `BaseController`. Baseline default route maps `Home/Index` as the root.

## Execution Notes
Use `get_code_dependencies` for `ContosoUniversity/Controllers/HomeController.cs` before editing. Replace `System.Web.Mvc` APIs with `Microsoft.AspNetCore.Mvc` equivalents. Copy or adapt any required models/views only as needed for this controller; broader view/static asset migration remains a later subtask.

**Done when**: `HomeController` or its Core equivalent compiles in `ContosoUniversity.Core`, root/default route can reach the migrated controller, dependencies are ready, and the Core project builds with zero errors and warnings.

## Research Findings

### Projects Affected
- `ContosoUniversity.Core/ContosoUniversity.Core.csproj` — receives the migrated `HomeController` and minimal direct views needed for runtime smoke validation.
- `ContosoUniversity/ContosoUniversity.csproj` — remains present and unchanged as the legacy side-by-side source/proxy target.

### Assessment and Dependency Findings
- File assessment for `Controllers/HomeController.cs` reported 17 mandatory API incompatibilities, all related to `System.Web.Mvc.ActionResult`, `View()`, and `ViewBag` usage.
- `get_code_dependencies` showed the default conventional route `{controller}/{action}/{id}`, direct view dependencies for `Home/Index`, `Home/About`, `Home/Contact`, `Home/Unauthorized`, shared error view, and model dependency on `EnrollmentDateGroup`.
- `HomeController` depends on the migrated `BaseController`, `SchoolContext`, `INotificationService`, and `EnrollmentDateGroup`, which were prepared by earlier subtasks.

### Migration Decisions
- Replaced `System.Web.Mvc` with `Microsoft.AspNetCore.Mvc` and changed action return types from `ActionResult` to `IActionResult`.
- Preserved `ViewBag` usage because ASP.NET Core MVC supports it and it is sufficient for this controller.
- Renamed the `Unauthorized` method to `UnauthorizedPage` with `[ActionName("Unauthorized")]` to preserve the `/Home/Unauthorized` route without hiding `ControllerBase.Unauthorized()`.
- Copied/adapted only the Home views and shared error view needed for this controller. Broader layout/static asset migration remains in `03.09-views-and-static-assets`.

### Files Modified
- `ContosoUniversity.Core/Controllers/HomeController.cs` — migrated controller.
- `ContosoUniversity.Core/Views/Home/Index.cshtml`, `About.cshtml`, `Contact.cshtml`, `Unauthorized.cshtml` — direct Home views for runtime validation.
- `ContosoUniversity.Core/Views/Shared/Error.cshtml` — Core-compatible simple error view.
- `ContosoUniversity.Core/Views/_ViewImports.cshtml` — Core MVC view imports and tag helper registration.

### Validation
- `dotnet build ContosoUniversity.Core.csproj` succeeded with 0 errors and 0 warnings.
- Full solution MSBuild succeeded with 0 errors and 0 warnings.
- Runtime smoke checks verified `/` returns HTTP 200 with `Contoso University` content and `/Home/About` returns HTTP 200 with `Student Body Statistics` content.

### Decomposition Decision
- This subtask was executed atomically because it migrates one controller and its direct views/dependencies.
