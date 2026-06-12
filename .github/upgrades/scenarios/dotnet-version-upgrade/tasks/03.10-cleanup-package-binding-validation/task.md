# 03.10-cleanup-package-binding-validation: Clean up packages, binding redirects, and validate migrated web app

# 03.10-cleanup-package-binding-validation: Clean up packages, binding redirects, and validate migrated web app

## Objective
Complete the migrate task by addressing remaining package/API issues, documenting binding redirect review findings, validating no unintended System.Web dependencies remain in the Core project, and confirming the old Framework project remains present. Scope includes final package cleanup in the Core project, adapter cleanup notes, build validation, and migration handoff notes for final validation.

## Research Context
Assessment found 2 incompatible packages, 24 package upgrades, 1 vulnerable package, deprecated packages, packages now included by framework references, and multiple binding redirect conflicts. Binding redirects belong to the old Framework config and should be reviewed/documented before removal decisions; the old project is not deleted by the agent.

## Execution Notes
Search for remaining `System.Web`, `ConfigurationManager`, `adapter-cleanup`, and `// STUB:` markers in the Core project. Run Core project build and full solution MSBuild. Document any remaining manual follow-up for final validation.

**Done when**: Core project has no unintended System.Web/package leftovers, binding redirect review is documented, old Framework project remains in the solution, full solution builds with zero errors and warnings, and parent migrate task completion criteria are satisfied.

## Research Findings

### Projects Affected
- `ContosoUniversity.Core/ContosoUniversity.Core.csproj` — final package and adapter cleanup target.
- `ContosoUniversity/ContosoUniversity.csproj` — old Framework project remains in the solution as required by side-by-side migration.

### Package Cleanup Findings
- Core project package inventory was standard non-CPM `PackageReference` mode.
- Initial Core package health check had no vulnerable packages but had deprecated transitive packages: `Azure.Identity` `1.14.2` and `Microsoft.Identity.Client` `4.73.1`.
- Supported package lookup returned `Azure.Identity` `1.21.0` and `Microsoft.Identity.Client` `4.84.2`; both were pinned directly to resolve deprecation warnings.
- `Microsoft.AspNetCore.SystemWebAdapters.CoreServices` and `AddSystemWebAdapters()` / `UseSystemWebAdapters()` were still present, but source scans found no `System.Web`, `ConfigurationManager`, `adapter-cleanup`, or `// STUB:` usage in Core source files. The adapter package/registrations were removed as unused migration scaffolding.

### Binding Redirect Review
- Reviewed `ContosoUniversity/Web.config` binding redirects and wrote `binding-redirect-review.md` in this task folder.
- Decision: do not remove legacy `Web.config` binding redirects in this agent-run because the old Framework project remains live and deployable; no redirects are needed or copied into `ContosoUniversity.Core`.

### Validation
- Core project clean/build succeeded with 0 errors and 0 warnings after package cleanup.
- `dotnet list package --include-transitive --vulnerable` reports no vulnerable Core packages.
- `dotnet list package --include-transitive --deprecated` reports no deprecated Core packages.
- Source scans found no `System.Web`, `ConfigurationManager`, `adapter-cleanup`, `// STUB:`, or System.Web adapter references in Core source/project files.
- Full solution MSBuild succeeded with 0 errors and 0 warnings.
- Smoke checks returned HTTP 200 for `/`, `/Students`, `/Departments`, `/Courses`, `/Instructors`, `/Notifications`, `/Notifications/GetNotifications`, and `/health`.

### Decomposition Decision
- This subtask was executed atomically because it covered final cleanup, documentation, and validation without independent implementation units.
