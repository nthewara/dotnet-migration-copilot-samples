# 03.05-departments-controller: Migrate DepartmentsController to ASP.NET Core

# 03.05-departments-controller: Migrate DepartmentsController to ASP.NET Core

## Objective
Migrate `DepartmentsController` and its direct dependencies to ASP.NET Core MVC. Scope includes CRUD actions, concurrency/RowVersion behavior, model binding, and data access dependencies.

## Research Context
Controller triage: 8 actions, 2 POST actions, no auth, no uploads, MVC CRUD views with concurrency handling. It inherits from `BaseController` and depends on EF Core model/data types.

## Execution Notes
Use `get_code_dependencies` for `ContosoUniversity/Controllers/DepartmentsController.cs` before editing. Preserve concurrency behavior while replacing MVC 5 namespaces and action result patterns with ASP.NET Core equivalents.

**Done when**: `DepartmentsController` compiles in `ContosoUniversity.Core`, concurrency/model binding behavior is represented in Core-compatible code, dependencies are available, and the Core project builds with zero errors and warnings.

## Research Findings

### Projects Affected
- `ContosoUniversity.Core/ContosoUniversity.Core.csproj` — receives the migrated `DepartmentsController` and direct Departments views.
- `ContosoUniversity/ContosoUniversity.csproj` — remains present and unchanged as the legacy side-by-side source/proxy target.

### Assessment and Dependency Findings
- File assessment for `Controllers/DepartmentsController.cs` reported 94 API issues, primarily `System.Web.Mvc` action results, attributes, redirects, `SelectList`, and status result helpers.
- `get_code_dependencies` showed the default conventional route `{controller}/{action}/{id}`, Departments CRUD views, `Department`, `Instructor`, EF Core `EntityState`, `DbUpdateConcurrencyException`, and `SelectList` dependencies.
- The controller has concurrency/RowVersion handling that must preserve current database-values comparison and ModelState error behavior.

### Migration Decisions
- Replaced `System.Web.Mvc` with `Microsoft.AspNetCore.Mvc` and `Microsoft.AspNetCore.Mvc.Rendering`.
- Changed action return types from `ActionResult` to `IActionResult`.
- Replaced MVC 5 status helpers with ASP.NET Core `BadRequest()` and `NotFound()`.
- Converted `[Bind(Include = "...")]` to ASP.NET Core `[Bind("...")]` syntax while preserving over-posting protection.
- Preserved `DbUpdateConcurrencyException` handling, `GetDatabaseValues()`, current-value ModelState errors, and RowVersion reassignment.
- Added `PopulateInstructorsDropDownList` helper to keep `SelectList` population consistent for create/edit redisplay.
- Added null handling in delete POST before removing the department.

### Files Modified
- `ContosoUniversity.Core/Controllers/DepartmentsController.cs` — migrated controller.
- `ContosoUniversity.Core/Views/Departments/*.cshtml` — direct Departments views for compile/runtime validation, with validation bundle calls replaced in create/edit views.

### Validation
- `dotnet build ContosoUniversity.Core.csproj` succeeded with 0 errors and 0 warnings.
- Full solution MSBuild succeeded with 0 errors and 0 warnings.
- Runtime smoke checks verified `/Departments` returns HTTP 200 and `/Departments/Create` returns HTTP 200.

### Decomposition Decision
- This subtask was executed atomically because it migrates one controller and its direct views/dependencies.
