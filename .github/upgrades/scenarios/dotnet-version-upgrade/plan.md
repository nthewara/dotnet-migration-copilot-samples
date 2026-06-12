# .NET Version Upgrade Plan

## Overview

**Target**: Upgrade ContosoUniversity from .NET Framework 4.8 ASP.NET Framework/System.Web to a modern ASP.NET Core application targeting `net10.0`.
**Scope**: 1 web application project with 658 assessment issues across 24 affected files. The migration will use a side-by-side ASP.NET Core project so the existing Framework application remains available while web assets are migrated.

### Selected Strategy
**All-At-Once** — The assessed solution has a single project, so dependency-tier phasing is not needed.
**Rationale**: 1 .NET Framework 4.8 ASP.NET Framework web project, no project-to-project dependency graph, and a side-by-side web migration approach that isolates the new ASP.NET Core `net10.0` project while keeping the existing application intact.

## Tasks

### 01-verify-toolchain-and-project-state: Verify upgrade prerequisites

Verify the repository, solution, installed .NET SDK support for `net10.0`, and the current state of `ContosoUniversity.csproj` before making migration changes. This task should confirm the source branch and working branch are correct, check `global.json` compatibility if present, confirm the legacy web project remains buildable before migration, and inventory the project shape needed by the side-by-side scaffold.

The assessment found an old-style .NET Framework web application with legacy `System.Web`/ASP.NET Framework features, packages.config-style migration risks, and binding redirect conflicts. This prerequisite task captures the starting point so later scaffold and migration work can be validated against a known baseline.

**Done when**: The `net10.0` SDK is validated, repository branch and workflow files are consistent, the existing solution state is documented, and any blocking prerequisite issue is either fixed or recorded before scaffold work begins.

---

### 02-scaffold-contosouniversity-core: Scaffold ASP.NET Core side-by-side project

Create a new ASP.NET Core `net10.0` project alongside the existing `ContosoUniversity` Framework web project and configure it for side-by-side migration. The new project should use modern SDK-style project format, reference the appropriate ASP.NET Core framework packages, and include a reverse-proxy/YARP setup that lets the old Framework app remain live while routes are migrated incrementally.

This task is necessary because the selected Project Approach is Side-by-side and the assessment identified extensive ASP.NET Framework/System.Web usage. The old web project is excluded from SDK-style conversion and direct TFM replacement; instead, the new Core project becomes the migration target.

**Done when**: The new ASP.NET Core project is added to the solution, targets `net10.0`, builds successfully, has proxy routing configured to the old Framework application, and can serve a minimal/stub response without deleting or breaking the existing Framework project.

---

### 03-migrate-contosouniversity-web: Migrate web application assets to ASP.NET Core

Migrate the ContosoUniversity web application from System.Web/MVC patterns into the side-by-side ASP.NET Core project. This includes controllers, routing, filters, application initialization currently represented by `Global.asax.cs`, views/static assets, bundling replacement for `System.Web.Optimization`, configuration migration to `appsettings.json`/`IConfiguration`, and package/API remediation needed for the new `net10.0` target.

The assessment found 495 ASP.NET Framework issues, 536 binary incompatible API occurrences, 37 source incompatible API occurrences, 2 incompatible packages, 24 recommended package upgrades, 1 vulnerable package, and multiple binding redirect conflicts. The approved options require resolving package and API issues inline, using System.Web Adapters where helpful during incremental migration, auto-migrating configuration, and documenting binding redirects before removal.

**Done when**: The migrated ASP.NET Core project implements the ContosoUniversity web functionality in scope, legacy System.Web/MVC initialization and route/filter registration are replaced with ASP.NET Core equivalents, incompatible/deprecated/vulnerable package issues are addressed inline, binding redirect findings are reviewed before removal, the old Framework project remains present, and the solution builds without errors or warnings.

---

### 04-final-validation-and-handoff: Validate upgraded solution and document follow-up

Run final validation across the upgraded solution after the side-by-side migration task completes. This includes restore, full solution build, available tests, and review of assessment success criteria for `net10.0` readiness. The task should also document any intentional post-upgrade follow-up, especially that the agent does not delete the old Framework project as part of this migration.

This final task confirms that the new ASP.NET Core `net10.0` project is the validated migration target and that the existing Framework project remains available for comparison or production cutover decisions. It should capture any remaining manual verification or deployment/cutover actions for the user.

**Done when**: Restore/build/test validation succeeds with zero errors and warnings for modified projects, tasks are complete, any deferred or manual follow-up is documented, and the upgrade state is ready for user review and source-control commit according to the Single Commit at End strategy.
