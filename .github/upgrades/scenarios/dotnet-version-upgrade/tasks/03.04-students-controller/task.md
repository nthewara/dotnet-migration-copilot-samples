# 03.04-students-controller: Migrate StudentsController to ASP.NET Core

# 03.04-students-controller: Migrate StudentsController to ASP.NET Core

## Objective
Migrate `StudentsController` and its direct dependencies to ASP.NET Core MVC. Scope includes CRUD actions, paging/sorting/search parameters, model binding changes, and any views/models required for compile-time correctness.

## Research Context
Controller triage: 8 actions, 2 POST actions, no auth, no uploads, MVC CRUD views. It inherits from `BaseController` and depends on data access/model types.

## Execution Notes
Use `get_code_dependencies` for `ContosoUniversity/Controllers/StudentsController.cs` before editing. Convert `System.Web.Mvc` imports/attributes to ASP.NET Core equivalents, update binding attributes where needed, and verify ModelState behavior remains explicit where the original action expects it.

**Done when**: `StudentsController` compiles in `ContosoUniversity.Core`, CRUD action signatures and model binding are migrated to ASP.NET Core patterns, required dependencies are available, and the Core project builds with zero errors and warnings.

## Research Findings

### Projects Affected
- `ContosoUniversity.Core/ContosoUniversity.Core.csproj` — receives the migrated `StudentsController` and direct Students views.
- `ContosoUniversity/ContosoUniversity.csproj` — remains present and unchanged as the legacy side-by-side source/proxy target.

### Assessment and Dependency Findings
- File assessment for `Controllers/StudentsController.cs` reported 91 API issues, primarily `System.Web.Mvc` action results, attributes, TempData, redirects, and status result helpers.
- `get_code_dependencies` showed the default conventional route `{controller}/{action}/{id}`, Students CRUD views, `Student`, `Enrollment`, `Course`, `PaginatedList<T>`, `SchoolContext`, `EntityState`, and MVC helper dependencies.
- Students views copied from the legacy project used `@Scripts.Render("~/bundles/jqueryval")`, which is not available in ASP.NET Core and was replaced with direct script references as an interim view migration.

### Migration Decisions
- Replaced `System.Web.Mvc` with `Microsoft.AspNetCore.Mvc` and changed action return types from `ActionResult` to `IActionResult`.
- Replaced `HttpStatusCodeResult(HttpStatusCode.BadRequest)` with `BadRequest()` and `HttpNotFound()` with `NotFound()`.
- Converted MVC 5 `[Bind(Include = "...")]` usage to ASP.NET Core `[Bind("...")]` syntax while preserving over-posting protection.
- Kept explicit `ModelState.IsValid` and enrollment date validation behavior.
- Changed details lookup from `Single()` to `SingleOrDefault()` so null handling works as intended in ASP.NET Core.
- Added null handling for delete POST before removing a student.

### Files Modified
- `ContosoUniversity.Core/Controllers/StudentsController.cs` — migrated controller.
- `ContosoUniversity.Core/Views/Students/*.cshtml` — direct Students views for compile/runtime validation, with validation bundle calls replaced in create/edit views.

### Validation
- `dotnet build ContosoUniversity.Core.csproj` succeeded with 0 errors and 0 warnings.
- Full solution MSBuild succeeded with 0 errors and 0 warnings.
- Runtime smoke checks verified `/Students` returns HTTP 200 and `/Students/Create` returns HTTP 200.

### Decomposition Decision
- This subtask was executed atomically because it migrates one controller and its direct views/dependencies.
