# 03.06-courses-controller: Migrate CoursesController to ASP.NET Core

# 03.06-courses-controller: Migrate CoursesController to ASP.NET Core

## Objective
Migrate `CoursesController` and its direct dependencies to ASP.NET Core MVC. Scope includes CRUD actions, data access dependencies, model binding, and file upload migration.

## Research Context
Controller triage: 8 actions, 2 POST actions, no auth, uses `HttpPostedFileBase` for teaching material uploads, MVC CRUD views. `HttpPostedFileBase` has no direct Core equivalent and should migrate to `IFormFile`.

## Execution Notes
Use `get_code_dependencies` for `ContosoUniversity/Controllers/CoursesController.cs` before editing. Replace `HttpPostedFileBase` with `IFormFile`, update namespaces/action results, and ensure upload handling uses ASP.NET Core request/form-file APIs.

**Done when**: `CoursesController` compiles in `ContosoUniversity.Core`, file upload actions use Core-compatible `IFormFile` patterns, dependencies are available, and the Core project builds with zero errors and warnings.

## Research Findings

### Projects Affected
- `ContosoUniversity.Core/ContosoUniversity.Core.csproj` — receives the migrated `CoursesController`, Courses views, and teaching-material upload assets.
- `ContosoUniversity/ContosoUniversity.csproj` — remains present and unchanged as the legacy side-by-side source/proxy target.

### Assessment and Dependency Findings
- File assessment for `Controllers/CoursesController.cs` reported 146 issues, including `System.Web.Mvc` action result/status APIs, `System.Web.HttpPostedFileBase`, and `Server.MapPath` usage.
- `get_code_dependencies` showed the default conventional route `{controller}/{action}/{id}`, Courses CRUD views, `Course`, `Department`, EF Core `EntityState`, `SelectList`, and file upload dependencies.
- Create/Edit views use multipart forms and legacy `@Scripts.Render("~/bundles/jqueryval")` validation bundle calls.

### Migration Decisions
- Replaced `System.Web.Mvc` with ASP.NET Core MVC namespaces and changed action return types to `IActionResult`.
- Replaced `HttpPostedFileBase` with `IFormFile` and made create/edit POST actions asynchronous.
- Replaced `Server.MapPath` with `IWebHostEnvironment.WebRootPath` plus `Path.Combine`.
- Preserved image extension and 5 MB size validation.
- Stored uploaded teaching material images under `wwwroot/Uploads/TeachingMaterials` and returned app paths in the existing `~/Uploads/TeachingMaterials/{file}` format so existing view `Url.Content` calls continue to work.
- Replaced MVC 5 status helpers with `BadRequest()` and `NotFound()` and converted `[Bind(Include = ...)]` to ASP.NET Core `[Bind(...)]` syntax.

### Files Modified
- `ContosoUniversity.Core/Controllers/CoursesController.cs` — migrated controller and upload handling.
- `ContosoUniversity.Core/Views/Courses/*.cshtml` — copied direct Courses views and replaced validation bundle calls in create/edit views.
- `ContosoUniversity.Core/wwwroot/Uploads/TeachingMaterials/*` — copied existing teaching material upload assets for Core static file serving.

### Validation
- `dotnet build ContosoUniversity.Core.csproj` succeeded with 0 errors and 0 warnings.
- Full solution MSBuild succeeded with 0 errors and 0 warnings.
- Runtime smoke checks verified `/Courses` returns HTTP 200 and `/Courses/Create` returns HTTP 200 with `Teaching Material Image` content.

### Decomposition Decision
- This subtask was executed atomically because it migrates one controller and its direct upload/view dependencies.
