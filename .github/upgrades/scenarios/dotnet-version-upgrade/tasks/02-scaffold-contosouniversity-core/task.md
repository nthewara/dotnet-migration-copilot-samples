# 02-scaffold-contosouniversity-core: Scaffold ASP.NET Core side-by-side project

Create a new ASP.NET Core `net10.0` project alongside the existing `ContosoUniversity` Framework web project and configure it for side-by-side migration. The new project should use modern SDK-style project format, reference the appropriate ASP.NET Core framework packages, and include a reverse-proxy/YARP setup that lets the old Framework app remain live while routes are migrated incrementally.

This task is necessary because the selected Project Approach is Side-by-side and the assessment identified extensive ASP.NET Framework/System.Web usage. The old web project is excluded from SDK-style conversion and direct TFM replacement; instead, the new Core project becomes the migration target.

**Done when**: The new ASP.NET Core project is added to the solution, targets `net10.0`, builds successfully, has proxy routing configured to the old Framework application, and can serve a minimal/stub response without deleting or breaking the existing Framework project.

## Research Findings

### Projects Affected
- `ContosoUniversity/ContosoUniversity.csproj` — existing .NET Framework 4.8 ASP.NET Framework MVC web application; remains old-style and live as the proxy target.
- `ContosoUniversity.Core/ContosoUniversity.Core.csproj` — new SDK-style ASP.NET Core MVC project targeting `net10.0`, created as a sibling project.

### Assessment Signals
- Project assessment summary: old-style WAP project, current TFM `net48`, proposed TFM `net10.0`, 84 total files, 658 compatibility issues.
- Affected technologies: ASP.NET Framework/System.Web, legacy configuration, MSMQ/message queue usage.
- Package signals relevant to scaffold: `Microsoft.AspNetCore.SystemWebAdapters.CoreServices` and `Yarp.ReverseProxy` are required for the side-by-side adapter/proxy host.

### Baseline Capture
- Old app URL: `http://localhost:58801`, verified from `ContosoUniversity.csproj` WebProjectProperties and IIS Express `applicationhost.config`.
- Default route: `{controller}/{action}/{id}` with defaults `Home/Index/{id?}` from `App_Start/RouteConfig.cs`.
- Global filters: `HandleErrorAttribute`; global authorization filter is commented out.
- Bundles to migrate later: jQuery, jQuery validation, Modernizr, Bootstrap/respond, and `Content/bootstrap.css` plus `Content/site.css`.
- Startup pipeline: `Global.asax.cs` registers areas, filters, routes, bundles, then initializes the EF Core database from `DefaultConnection`.
- Configuration values migrated into the scaffold: `DefaultConnection` and `NotificationQueuePath`.
- Controllers inventoried for later migration: `HomeController`, `StudentsController`, `CoursesController`, `DepartmentsController`, `InstructorsController`, `NotificationsController`, and shared `BaseController`.
- Authentication/authorization: no active global authorization and no `[Authorize]` usage found in controller inventory; `BaseController` currently uses a fixed `System` user name for auditing.
- Pipeline components: no custom `HttpModule` or `HttpHandler` implementations found; Web.config contains request limit configuration and binding redirects.

### Scaffold Parameters and Package Versions
- New project name: `ContosoUniversity.Core`.
- Project type: MVC.
- Target framework: `net10.0`.
- Supported package versions resolved: `Microsoft.AspNetCore.SystemWebAdapters.CoreServices` `2.3.0`, `Yarp.ReverseProxy` `2.3.0`.

### Files and Configuration Created
- `ContosoUniversity.Core/ContosoUniversity.Core.csproj` — SDK-style ASP.NET Core web project targeting `net10.0` with System.Web adapters and YARP package references.
- `ContosoUniversity.Core/Program.cs` — registers System.Web adapters, YARP forwarder, `IHttpContextAccessor`, controllers with views, `/health`, controller routes, default MVC route, and catch-all proxy route.
- `ContosoUniversity.Core/appsettings.json` — includes logging, `ProxyTo`, `DefaultConnection`, and `NotificationQueuePath`.
- `ContosoUniversity.Core/appsettings.Development.json` and `Properties/launchSettings.json` — configure development environment and proxy target.
- `ContosoUniversity.Core/wwwroot/.gitkeep` — ensures the scaffold has a web root for later static asset migration.
- `ContosoUniversity/ContosoUniversity.sln` — now includes the Core project.
- `ContosoUniversity/ContosoUniversity.csproj` — includes `_MigrateToProjectGuid` pointing to the new Core project.

### Decomposition Decision
- This scaffold task is executable as a single task. It creates one new project, configures proxy/routing/basic config, and validates build plus smoke response; the larger web asset migration remains the next top-level task and will be decomposed at execution time.
