# .NET Version Upgrade — Report

**Scenario:** Upgrade ContosoUniversity from .NET Framework 4.8 ASP.NET Framework/System.Web to a modern ASP.NET Core application targeting `net10.0`.
**Outcome:** ✅ Fully completed
**Projects affected:** 2
**Tasks:** 13/13 completed

---

## Summary

ContosoUniversity was upgraded using a side-by-side migration strategy. The original `ContosoUniversity` .NET Framework 4.8 ASP.NET Framework project remains in the solution, while a new `ContosoUniversity.Core` ASP.NET Core project was created and migrated to `net10.0`.

The migration moved controllers, shared data/model code, views, configuration, static assets, and notification infrastructure into the new Core project. Final validation passed with zero build errors and zero warnings, and the upgrade was committed as a single end-of-scenario commit.

---

## What Changed

### Projects

| Project | Role | Target |
|---------|------|--------|
| `ContosoUniversity/ContosoUniversity.csproj` | Original Framework web app retained for side-by-side operation and cutover comparison | .NET Framework 4.8 |
| `ContosoUniversity.Core/ContosoUniversity.Core.csproj` | New ASP.NET Core migration target | `net10.0` |

### Packages

| Project | Package | Change | From → To |
|---------|---------|--------|-----------|
| `ContosoUniversity.Core` | `Microsoft.EntityFrameworkCore.SqlServer` | Added | none → `10.0.9` |
| `ContosoUniversity.Core` | `MSMQ.Messaging` | Added | `System.Messaging` pattern → `MSMQ.Messaging` `1.0.4` |
| `ContosoUniversity.Core` | `System.Drawing.Common` | Added direct pin | transitive vulnerable `4.7.0` → direct `10.0.0` |
| `ContosoUniversity.Core` | `Yarp.ReverseProxy` | Added/updated | scaffolded `2.0.1` → `2.3.0` |
| `ContosoUniversity.Core` | `Azure.Identity` | Added direct pin | transitive deprecated `1.14.2` → direct `1.21.0` |
| `ContosoUniversity.Core` | `Microsoft.Identity.Client` | Added direct pin | transitive deprecated `4.73.1` → direct `4.84.2` |
| `ContosoUniversity.Core` | `Microsoft.AspNetCore.SystemWebAdapters.CoreServices` | Removed after cleanup | `2.3.0` → removed |

### Code Modifications

- **Project structure** — Added `ContosoUniversity.Core`, an SDK-style ASP.NET Core MVC project targeting `net10.0`, and added it to `ContosoUniversity.sln`.
- **Proxy setup** — Configured YARP forwarding from the Core project to the original Framework app at `http://localhost:58801` for side-by-side migration.
- **Startup and configuration** — Moved Core startup concerns into `Program.cs`, added `/health`, configured EF Core DI, message queue options, static files, routes, and app settings in `appsettings.json`.
- **Data and models** — Copied shared models, view models, `SchoolContext`, `DbInitializer`, and `PaginatedList<T>` into the Core project.
- **Controllers** — Migrated `HomeController`, `StudentsController`, `DepartmentsController`, `CoursesController`, `InstructorsController`, and `NotificationsController` to ASP.NET Core MVC patterns with constructor injection and `IActionResult` results.
- **MVC API migrations** — Replaced MVC 5 result/status helpers with ASP.NET Core equivalents such as `BadRequest()`, `NotFound()`, `RedirectToAction(nameof(Index))`, and Core `Json(...)` responses.
- **Model binding** — Converted MVC 5 `[Bind(Include = ...)]` usage to ASP.NET Core `[Bind(...)]` syntax and preserved over-posting protection.
- **File uploads** — Replaced `HttpPostedFileBase` and `Server.MapPath` with `IFormFile`, `IWebHostEnvironment.WebRootPath`, and async file copy APIs.
- **Notifications/MSMQ** — Replaced `System.Messaging` usage in the Core app with an `MSMQ.Messaging`-based `NotificationService` configured through `IConfiguration`/options.
- **Views and static assets** — Migrated Razor views, added Core `_ViewImports.cshtml` and `_ViewStart.cshtml`, added a shared layout, moved legacy `Content/` and `Scripts/` assets to `wwwroot/css` and `wwwroot/js`, and removed `@Scripts.Render`/`@Styles.Render` dependencies.
- **Binding redirects** — Reviewed and documented legacy Framework `Web.config` binding redirects in `tasks/03.10-cleanup-package-binding-validation/binding-redirect-review.md`; no redirects were copied to the Core project.

### Git Commits

| SHA | Message |
|-----|---------|
| `483ff65` | `upgrade: migrate ContosoUniversity to .NET 10 side-by-side` |

---

## Task Breakdown

| Task | Description | Outcome | Content | Details |
|------|-------------|---------|---------|---------|
| `01-verify-toolchain-and-project-state` | Verify upgrade prerequisites | ✅ Validated `net10.0` SDK, branch state, project shape, and baseline build. | [task.md](tasks/01-verify-toolchain-and-project-state/task.md) | [progress-details.md](tasks/01-verify-toolchain-and-project-state/progress-details.md) |
| `02-scaffold-contosouniversity-core` | Scaffold ASP.NET Core side-by-side project | ✅ Created `ContosoUniversity.Core`, configured YARP, and verified `/health`. | [task.md](tasks/02-scaffold-contosouniversity-core/task.md) | [progress-details.md](tasks/02-scaffold-contosouniversity-core/progress-details.md) |
| `03-migrate-contosouniversity-web` | Migrate web app assets | ✅ Decomposed into focused migration subtasks and completed all of them. | [task.md](tasks/03-migrate-contosouniversity-web/task.md) | [progress-details.md](tasks/03-migrate-contosouniversity-web/progress-details.md) |
| `03.01-core-foundation` | Migrate shared Core foundation | ✅ Added shared models/data, EF Core DI, DB initialization, and Core `BaseController`. | [task.md](tasks/03.01-core-foundation/task.md) | [progress-details.md](tasks/03.01-core-foundation/progress-details.md) |
| `03.02-notification-service-msmq` | Replace/adapt MSMQ notification service | ✅ Migrated notification service to `MSMQ.Messaging` and options-based configuration. | [task.md](tasks/03.02-notification-service-msmq/task.md) | [progress-details.md](tasks/03.02-notification-service-msmq/progress-details.md) |
| `03.03-home-controller` | Migrate HomeController | ✅ Migrated Home controller and views; verified `/` and `/Home/About`. | [task.md](tasks/03.03-home-controller/task.md) | [progress-details.md](tasks/03.03-home-controller/progress-details.md) |
| `03.04-students-controller` | Migrate StudentsController | ✅ Migrated CRUD, paging/search/sort, validation, TempData, and views. | [task.md](tasks/03.04-students-controller/task.md) | [progress-details.md](tasks/03.04-students-controller/progress-details.md) |
| `03.05-departments-controller` | Migrate DepartmentsController | ✅ Migrated CRUD and preserved concurrency/RowVersion behavior. | [task.md](tasks/03.05-departments-controller/task.md) | [progress-details.md](tasks/03.05-departments-controller/progress-details.md) |
| `03.06-courses-controller` | Migrate CoursesController | ✅ Migrated CRUD and file upload handling to `IFormFile`. | [task.md](tasks/03.06-courses-controller/task.md) | [progress-details.md](tasks/03.06-courses-controller/progress-details.md) |
| `03.07-instructors-controller` | Migrate InstructorsController | ✅ Migrated instructor/course relationship assignment behavior and views. | [task.md](tasks/03.07-instructors-controller/task.md) | [progress-details.md](tasks/03.07-instructors-controller/progress-details.md) |
| `03.08-notifications-controller` | Migrate NotificationsController | ✅ Migrated JSON endpoints and dashboard view. | [task.md](tasks/03.08-notifications-controller/task.md) | [progress-details.md](tasks/03.08-notifications-controller/progress-details.md) |
| `03.09-views-and-static-assets` | Migrate Razor views and static assets | ✅ Moved CSS/JS to `wwwroot`, added layout infrastructure, and removed bundle helpers. | [task.md](tasks/03.09-views-and-static-assets/task.md) | [progress-details.md](tasks/03.09-views-and-static-assets/progress-details.md) |
| `03.10-cleanup-package-binding-validation` | Cleanup packages, binding redirects, validation | ✅ Removed unused adapters, resolved package health issues, documented redirects, and smoke-tested Core routes. | [task.md](tasks/03.10-cleanup-package-binding-validation/task.md) | [progress-details.md](tasks/03.10-cleanup-package-binding-validation/progress-details.md) |
| `04-final-validation-and-handoff` | Final validation and handoff | ✅ Final solution build passed and handoff documentation was written. | [task.md](tasks/04-final-validation-and-handoff/task.md) | [progress-details.md](tasks/04-final-validation-and-handoff/progress-details.md) |

---

## Decisions Made

- **Guided mode** — User chose Guided mode, so the workflow paused after assessment, planning, breakdown, and task completions.
- **Target framework** — The upgrade targeted `net10.0`.
- **Strategy** — All-at-once, because the assessed application was a single .NET Framework web project with no project dependency graph to phase.
- **Project approach** — Side-by-side migration, so the old Framework app remains live while `ContosoUniversity.Core` is introduced and migrated.
- **Package/API handling** — Incompatible packages and API changes were resolved inline; no stubs were left behind.
- **Binding redirects** — Binding redirects were documented and reviewed before any removal decision. They remain in the old Framework project because the old app remains in the solution.
- **Commit strategy** — Single Commit at End, producing commit `483ff65`.
- **Build tools** — Visual Studio MSBuild for the legacy Framework project/solution; `dotnet build` for the new SDK-style Core project.

---

## Build & Test Results

| Project | Build | Tests | Warnings |
|---------|-------|-------|----------|
| `ContosoUniversity` | ✅ Passed via solution MSBuild | Not run — no test project discovered | 0 |
| `ContosoUniversity.Core` | ✅ Passed via `dotnet build` and solution MSBuild | Not run — no test project discovered | 0 |

Final validation also confirmed:

- `ContosoUniversity.Core` has no known vulnerable packages from configured NuGet sources.
- `ContosoUniversity.Core` has no deprecated packages from configured NuGet sources.
- Core source/project files have no remaining `System.Web`, `ConfigurationManager`, `adapter-cleanup`, `// STUB:`, `Microsoft.AspNetCore.SystemWebAdapters`, `AddSystemWebAdapters`, `UseSystemWebAdapters`, `Scripts.Render`, or `Styles.Render` references.
- Smoke checks returned HTTP 200 for `/`, `/Students`, `/Departments`, `/Courses`, `/Instructors`, `/Notifications`, `/Notifications/GetNotifications`, and `/health`.

---

## Known Gaps & Follow-up Items

- **Old Framework project remains intentionally** — This is by design for the side-by-side migration. Production cutover and eventual deletion of `ContosoUniversity` should happen only after user-controlled verification.
- **No automated tests were present** — Build and smoke validation were completed, but no unit/integration test projects were available to run.
- **Local HTTPS developer certificate** — The ASP.NET Core development certificate is not trusted on this machine. Trust it if browser-based HTTPS testing is needed locally.
- **Production verification** — Before retiring the old app, verify production routes, deployment settings, database behavior, authentication/authorization expectations, and any environment-specific configuration.
