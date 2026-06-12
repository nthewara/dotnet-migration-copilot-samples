# 03.07-instructors-controller: Migrate InstructorsController to ASP.NET Core

# 03.07-instructors-controller: Migrate InstructorsController to ASP.NET Core

## Objective
Migrate `InstructorsController` and its direct dependencies to ASP.NET Core MVC. Scope includes instructor/course relationship management, selected course binding, CRUD actions, and data access dependencies.

## Research Context
Controller triage: 8 actions, 2 POST actions, no auth, no uploads, relatively complex relationship/course assignment logic. It inherits from `BaseController`.

## Execution Notes
Use `get_code_dependencies` for `ContosoUniversity/Controllers/InstructorsController.cs` before editing. Preserve selected-course update behavior and adapt MVC 5 binding/action results to ASP.NET Core patterns.

**Done when**: `InstructorsController` compiles in `ContosoUniversity.Core`, instructor/course relationship update behavior is represented in Core-compatible code, dependencies are available, and the Core project builds with zero errors and warnings.

## Research Findings

### Projects Affected
- `ContosoUniversity.Core/ContosoUniversity.Core.csproj` — receives the migrated `InstructorsController` and direct Instructors views.
- `ContosoUniversity/ContosoUniversity.csproj` — remains present and unchanged as the legacy side-by-side source/proxy target.

### Assessment and Dependency Findings
- File assessment for `Controllers/InstructorsController.cs` reported 67 API issues, primarily `System.Web.Mvc` action results, attributes, redirects, and status helpers.
- `get_code_dependencies` showed the default conventional route `{controller}/{action}/{id}`, Instructors CRUD views, `Instructor`, `OfficeAssignment`, `CourseAssignment`, `Course`, `InstructorIndexData`, `AssignedCourseData`, and EF Core relationship dependencies.
- The controller maintains instructor/course many-to-many assignments through `selectedCourses` checkboxes and `UpdateInstructorCourses`.

### Migration Decisions
- Replaced `System.Web.Mvc` with ASP.NET Core MVC namespaces and changed action return types to `IActionResult`.
- Replaced MVC 5 status helpers with `BadRequest()` and `NotFound()`.
- Converted `[Bind(Include = ...)]` to ASP.NET Core `[Bind(...)]` syntax while preserving over-posting protection.
- Replaced MVC 5 `TryUpdateModel` usage with explicit assignment from a bound `Instructor` object to the tracked instructor entity.
- Preserved selected-course update behavior and null office-assignment cleanup.
- Added null handling for edit/delete lookups before modifying tracked entities.

### Files Modified
- `ContosoUniversity.Core/Controllers/InstructorsController.cs` — migrated controller and relationship update behavior.
- `ContosoUniversity.Core/Views/Instructors/*.cshtml` — copied direct Instructors views and replaced validation bundle calls in create/edit views.

### Validation
- `dotnet build ContosoUniversity.Core.csproj` succeeded with 0 errors and 0 warnings.
- Full solution MSBuild succeeded with 0 errors and 0 warnings.
- Runtime smoke checks verified `/Instructors` returns HTTP 200 and `/Instructors/Create` returns HTTP 200.

### Decomposition Decision
- This subtask was executed atomically because it migrates one controller and its direct views/dependencies.
