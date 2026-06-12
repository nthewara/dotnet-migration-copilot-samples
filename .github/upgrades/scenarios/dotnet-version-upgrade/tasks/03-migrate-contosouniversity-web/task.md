# 03-migrate-contosouniversity-web: Migrate web application assets to ASP.NET Core

Migrate the ContosoUniversity web application from System.Web/MVC patterns into the side-by-side ASP.NET Core project. This includes controllers, routing, filters, application initialization currently represented by `Global.asax.cs`, views/static assets, bundling replacement for `System.Web.Optimization`, configuration migration to `appsettings.json`/`IConfiguration`, and package/API remediation needed for the new `net10.0` target.

The assessment found 495 ASP.NET Framework issues, 536 binary incompatible API occurrences, 37 source incompatible API occurrences, 2 incompatible packages, 24 recommended package upgrades, 1 vulnerable package, and multiple binding redirect conflicts. The approved options require resolving package and API issues inline, using System.Web Adapters where helpful during incremental migration, auto-migrating configuration, and documenting binding redirects before removal.

**Done when**: The migrated ASP.NET Core project implements the ContosoUniversity web functionality in scope, legacy System.Web/MVC initialization and route/filter registration are replaced with ASP.NET Core equivalents, incompatible/deprecated/vulnerable package issues are addressed inline, binding redirect findings are reviewed before removal, the old Framework project remains present, and the solution builds without errors or warnings.

## Research Findings

### Projects Affected
- `ContosoUniversity/ContosoUniversity.csproj` — legacy ASP.NET Framework MVC project; remains in the solution and serves as source/proxy target during side-by-side migration.
- `ContosoUniversity.Core/ContosoUniversity.Core.csproj` — new ASP.NET Core MVC `net10.0` project that receives migrated code, views, assets, configuration, and package/API updates.

### Assessment Signals
- `ContosoUniversity.csproj` assessment summary: current TFM `net48`, proposed TFM `net10.0`, SDK-style `False`, project kind WAP, 84 files, 658 issues.
- Major affected technologies: ASP.NET Framework/System.Web (495 issues), MSMQ/message queuing (61 issues), and legacy configuration (16 issues).
- Key issue groups: 536 binary incompatible API occurrences, 37 source incompatible API occurrences, 2 incompatible packages, 24 package upgrades, 1 vulnerable package, and binding redirect conflicts.

### Baseline Inventory
- Old app URL/proxy target: `http://localhost:58801`.
- Routing: default MVC route `{controller}/{action}/{id}` with defaults `Home/Index/{id?}`.
- Filters: global `HandleErrorAttribute`; global authorization is commented out.
- Startup: `Global.asax.cs` registers areas, filters, routes, bundles, and initializes EF Core database using `DefaultConnection`.
- Configuration: `DefaultConnection` and `NotificationQueuePath` are the app-specific values already copied to the Core project during scaffold.
- Bundles/static assets: jQuery, jQuery validation, Modernizr, Bootstrap/respond, `Content/bootstrap.css`, and `Content/site.css` are currently managed by `System.Web.Optimization`.
- Auth: no active `[Authorize]` attributes were found in controller triage; `BaseController` uses a fixed `System` audit user.
- Pipeline: no custom `IHttpModule`, `IHttpHandler`, `.ashx`, OWIN, or Katana components found; Global.asax startup remains the main pipeline migration input.

### Controller Triage
- `HomeController`: 5 actions, no POST actions, no auth, no uploads, MVC views.
- `StudentsController`: 8 actions, 2 POST actions, no auth, no uploads, MVC CRUD views.
- `DepartmentsController`: 8 actions, 2 POST actions, no auth, no uploads, MVC CRUD views with concurrency handling.
- `CoursesController`: 8 actions, 2 POST actions, no auth, uses `HttpPostedFileBase` for teaching material upload, MVC CRUD views.
- `InstructorsController`: 8 actions, 2 POST actions, no auth, no uploads, more complex relationship/course assignment logic.
- `NotificationsController`: 3 actions, 1 POST action, no auth, JSON endpoints, depends on notification service/MSMQ behavior.
- `BaseController`: shared base controller; no actions, provides audit/user-name helper behavior and must be migrated before controller subtasks.

### Decomposition Decision
- This task must be decomposed. The side-by-side migration guidance requires one controller per subtask when more than five controllers exist, and the web migration breakdown hints require isolating System.Messaging/MSMQ replacement as its own concern.
- Additional subtasks are needed for shared Core foundation work, static asset/view migration, and final package/binding cleanup validation.
