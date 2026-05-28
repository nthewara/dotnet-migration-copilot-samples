# .NET 10 Upgrade Plan - ContosoUniversity

## Table of Contents

- [1. Executive Summary](#1-executive-summary)
- [2. Migration Strategy](#2-migration-strategy)
- [3. Detailed Dependency Analysis](#3-detailed-dependency-analysis)
- [4. Implementation Timeline](#4-implementation-timeline)
- [5. Detailed Execution Steps](#5-detailed-execution-steps)
- [6. Project-by-Project Plans](#6-project-by-project-plans)
- [7. Package Update Reference](#7-package-update-reference)
- [8. Breaking Changes Catalog](#8-breaking-changes-catalog)
- [9. Risk Management](#9-risk-management)
- [10. Testing & Validation Strategy](#10-testing--validation-strategy)
- [11. Complexity & Effort Assessment](#11-complexity--effort-assessment)
- [12. Source Control Strategy](#12-source-control-strategy)
- [13. Success Criteria](#13-success-criteria)
- [14. Assumptions and Open Items](#14-assumptions-and-open-items)

## 1. Executive Summary

### Scenario Description
Upgrade `ContosoUniversity.csproj` from `.NET Framework 4.8` (`net48`) to `.NET 10.0` (`net10.0`) and migrate the application from legacy ASP.NET MVC/System.Web patterns to ASP.NET Core-compatible architecture.

### Scope

| Metric | Value |
| :--- | :---: |
| Total projects | 1 |
| Project type | Web Application Project (`Wap`) |
| Current target framework | `net48` |
| Proposed target framework | `net10.0` |
| SDK-style project | No |
| Total NuGet packages | 45 |
| Packages needing upgrade/replacement/removal | 26+ |
| Code files | 56 assessed; 83 in project details |
| Files with incidents | 23 |
| Lines of code | 3,409 |
| Estimated LOC to modify | 573+ |
| Total issues | 632 |
| API compatibility issues | 536 binary incompatible; 37 source incompatible |

### Selected Strategy
**All-At-Once Strategy** - The single project and its package/API migration should be upgraded as one coordinated operation with no intermediate target-framework states.

**Rationale**:
- The solution contains only one project, so there are no inter-project dependency phases to coordinate.
- The dependency graph has depth `0` and no circular project dependencies.
- Framework, package, MVC, configuration, and messaging changes are tightly coupled; partial migration would leave the application in a non-buildable state.
- The assessment provides a clear target framework (`net10.0`) and explicit package remediation guidance.

### Complexity Classification
**High complexity single-project migration**.

Although the solution is small, the project is high risk because it is a classic ASP.NET Framework web application with significant architectural migration requirements:
- `495` ASP.NET Framework/System.Web issues (`86.4%` of technology issues).
- `61` MSMQ/System.Messaging issues.
- `16` legacy configuration issues.
- Classic non-SDK-style project requiring SDK-style conversion.
- Security vulnerability in `Microsoft.Data.SqlClient`.

### Recommended Approach
Use an atomic migration specification:
1. Prepare prerequisites and branch state.
2. Convert the project and framework references in a coordinated update.
3. Update, replace, or remove all packages identified by the assessment.
4. Migrate ASP.NET MVC/System.Web application startup, routing, filters, controllers, views, bundling, configuration, and messaging patterns.
5. Validate with restore, full solution build, automated tests if present, and focused web application verification.

### Iteration Strategy Used
The plan is generated with mandatory discovery/foundation iterations followed by a consolidated detail iteration because there is one high-complexity project and no project dependency phases.

## 2. Migration Strategy

### Approach Selection

**Selected approach: All-At-Once migration.**

This plan uses one coordinated upgrade for `ContosoUniversity.csproj`. Project format conversion, target framework update, package remediation, and code migration are interdependent and should be planned as a unified operation.

### Why Incremental Project Migration Is Not Appropriate

Incremental project-by-project migration does not apply because the solution contains only one project. Splitting the work by project would create artificial boundaries and no independently buildable intermediate solution.

### All-At-Once Strategy Considerations

The atomic upgrade must account for the following:

- Convert the classic WAP project to SDK-style before or as part of target framework changes.
- Update all target framework and package references in one coordinated batch.
- Remove ASP.NET Framework packages whose functionality is replaced by `Microsoft.AspNetCore.App` framework references.
- Replace unsupported System.Web APIs with ASP.NET Core MVC equivalents.
- Replace `System.Web.Optimization` bundling with direct static asset references or an ASP.NET Core-compatible bundling approach.
- Replace `System.Messaging` usage with a supported queue implementation or an explicit compatibility design.
- Migrate XML/web.config-based runtime configuration to `appsettings.json`, environment variables, and `Microsoft.Extensions.Configuration` patterns.

### Dependency-Based Ordering Within the Atomic Operation

Although the project is migrated as one unit, implementation should follow this logical order:

1. Project system and target framework specification.
2. Framework references and NuGet package remediation.
3. Hosting/startup/routing/filter architecture migration.
4. Controller, view, validation, and MVC API migration.
5. Configuration and static asset migration.
6. MSMQ/message queue migration.
7. Restore/build verification and compilation fix pass.
8. Automated test execution and functional validation.

### Parallel vs. Sequential Work

Work can be split by technical area for developer ownership, but the repository should not be considered upgraded until all coordinated changes land together:

| Workstream | Can proceed in parallel? | Coordination Notes |
| :--- | :---: | :--- |
| Project/package conversion | Limited | Establishes shared references and compile context. |
| ASP.NET Core startup/routing/filter migration | Yes | Must align with controller and view changes. |
| Controller/view MVC API migration | Yes | Depends on selected ASP.NET Core MVC patterns. |
| Configuration migration | Yes | Must align with startup and data access. |
| Messaging migration | Yes | Requires architectural decision for MSMQ replacement. |
| Validation | Sequential after atomic upgrade | Requires coherent migrated codebase. |

## 4. Implementation Timeline

### Phase 0: Preparation

- Confirm `.NET 10.0` SDK availability for the execution environment.
- Confirm the upgrade branch is `upgrade-to-NET10`.
- Review the assessment findings and this plan before execution begins.
- Identify any automated tests or manual validation scripts available for the application.

### Phase 1: Atomic Upgrade

**Operations performed as one coordinated upgrade:**

- Convert `ContosoUniversity.csproj` to SDK-style.
- Change target framework from `net48` to `net10.0`.
- Add appropriate ASP.NET Core framework references.
- Update, remove, or replace all packages listed in the package update reference.
- Migrate `Global.asax.cs`, routing, filters, MVC controllers/views, bundling, configuration, and messaging.
- Restore dependencies and resolve package conflicts.
- Build the solution and fix all compilation errors introduced by the framework/package/API migration.

**Deliverable:** `ContosoUniversity.sln` builds successfully with `ContosoUniversity.csproj` targeting `net10.0`.

### Phase 2: Test Validation

**Operations:**

- Execute available automated tests if present or add targeted validation coverage if the repository has no test project.
- Validate key Contoso University workflows: home page, student/course/instructor CRUD, enrollment flows, validation errors, and data access.
- Validate static assets and client-side validation.
- Validate queue-dependent functionality after the MSMQ replacement decision is implemented.

**Deliverable:** All automated tests pass and critical web workflows are verified.

## 5. Detailed Execution Steps

### Step 1: Prepare Project System Conversion

Update `ContosoUniversity/ContosoUniversity.csproj` from classic WAP format to SDK-style targeting `net10.0`.

Expected project-file changes:

- Use an SDK-style project declaration appropriate for ASP.NET Core web applications.
- Set `TargetFramework` to `net10.0`.
- Remove obsolete .NET Framework assembly references and WAP-specific imports that do not apply to SDK-style projects.
- Preserve content files, views, static assets, configuration files, and data files required by the application.
- Review any `Directory.Build.props`, `Directory.Build.targets`, or `Directory.Packages.props` files if present to ensure target framework and package versions are not overridden elsewhere.

### Step 2: Apply Package Remediation

Apply all package actions listed in [Package Update Reference](#7-package-update-reference):

- Upgrade packages with suggested versions.
- Replace `Antlr` with `Antlr4` `4.6.6` if still required after bundling migration.
- Remove packages whose functionality is included in the ASP.NET Core framework reference.
- Remove or replace incompatible ASP.NET Framework-only packages.
- Address deprecated and vulnerable packages.

### Step 3: Migrate Application Startup and Hosting

Replace ASP.NET Framework startup patterns with ASP.NET Core hosting:

- Move application initialization from `Global.asax.cs` into `Program.cs` and service/middleware registration.
- Convert route registration from `RouteCollection` to endpoint routing on the ASP.NET Core application object.
- Convert global filters from `GlobalFilterCollection` to ASP.NET Core MVC options, filters, or middleware.
- Replace `Application_Start` initialization patterns with dependency injection and host configuration.

### Step 4: Migrate MVC Controllers, Actions, and Views

Replace `System.Web.Mvc` APIs with ASP.NET Core MVC equivalents:

- `System.Web.Mvc.Controller` → `Microsoft.AspNetCore.Mvc.Controller`.
- `ActionResult`, `ViewResult`, `JsonResult`, `RedirectToRouteResult`, `HttpNotFoundResult`, and status-code results → ASP.NET Core MVC result types and helper methods.
- `ModelStateDictionary` usage → ASP.NET Core `ModelStateDictionary` patterns.
- `BindAttribute`, `HttpPostAttribute`, `ValidateAntiForgeryTokenAttribute`, `ActionNameAttribute`, and related attributes → ASP.NET Core MVC equivalents.
- `SelectList` usages → `Microsoft.AspNetCore.Mvc.Rendering.SelectList`.
- `JsonRequestBehavior` usage → remove; ASP.NET Core does not use this setting.
- `Server.MapPath` and `HttpServerUtilityBase` → `IWebHostEnvironment.WebRootPath` or `ContentRootPath`.
- `HttpPostedFileBase` → `IFormFile`.

### Step 5: Migrate Static Assets and Bundling

Replace `System.Web.Optimization` usage:

- Remove `BundleConfig` and `System.Web.Optimization` dependencies.
- Use direct script/link tags for existing assets or select an ASP.NET Core-compatible bundling/minification mechanism.
- Ensure Bootstrap, jQuery, jQuery Validation, and unobtrusive validation assets are served from `wwwroot` or an approved package/CDN pattern.
- Validate `_Layout` and view references after migration.

### Step 6: Migrate Configuration

Replace legacy `web.config`/XML configuration patterns:

- Move application settings and connection strings to `appsettings.json`, user secrets for local secrets, and environment variables for deployment-specific values.
- Use `Microsoft.Extensions.Configuration` abstractions.
- Retain minimal `web.config` only if needed for IIS hosting of ASP.NET Core.
- Avoid carrying forward unsupported `System.Web` configuration sections.

### Step 7: Migrate Data Access

Upgrade Entity Framework Core packages from `3.1.32` to `10.0.8` and validate:

- `DbContext` registration in dependency injection.
- SQL Server provider configuration using upgraded `Microsoft.Data.SqlClient`.
- LINQ queries and includes affected by EF Core behavior changes.
- Existing migrations and database initialization/seeding behavior.

### Step 8: Replace MSMQ/System.Messaging

The assessment identifies `61` MSMQ/message queuing issues. `System.Messaging` is unsupported in modern .NET.

Preferred planning options:

1. Replace with a modern supported queue such as RabbitMQ or Azure Service Bus.
2. If queue use is local/demo-only, isolate behind an interface and provide a no-op or in-memory implementation for development.
3. If Windows-only MSMQ interoperability is mandatory, document and validate a dedicated compatibility approach outside direct `System.Messaging` APIs.

### Step 9: Restore, Build, and Resolve Compilation Errors

After the coordinated code and project changes are applied, restore dependencies and build the solution to identify remaining errors. Resolve all compilation errors caused by framework and package migration before moving to test validation.

### Step 10: Validate Tests and Functional Workflows

Run available tests and validate critical Contoso University application workflows as described in [Testing & Validation Strategy](#10-testing--validation-strategy).

## 6. Project-by-Project Plans

### Project: `ContosoUniversity.csproj`

**Path:** `ContosoUniversity/ContosoUniversity.csproj`

#### Current State

| Attribute | Value |
| :--- | :--- |
| Target framework | `net48` |
| Project format | Classic non-SDK-style Web Application Project |
| Project kind | `Wap` |
| Project dependencies | 0 |
| Dependants | 0 |
| Files | 83 |
| Files with incidents | 23 |
| Lines of code | 3,409 |
| Estimated LOC to modify | 573+ |
| Risk level | High |

#### Target State

| Attribute | Value |
| :--- | :--- |
| Target framework | `net10.0` |
| Project format | SDK-style |
| Web framework | ASP.NET Core MVC on `.NET 10.0` |
| Package state | All incompatible, vulnerable, deprecated, and redundant packages remediated |
| Build state | Solution builds without errors |

#### Package/Dependency Updates

Apply the package actions in [Package Update Reference](#7-package-update-reference). The project has `45` NuGet packages; `24` have explicit upgrade recommendations, `2` are incompatible/replacement candidates, and several ASP.NET Framework-era packages should be removed because the functionality is provided by ASP.NET Core framework references.

#### Expected Breaking Changes

- ASP.NET Framework MVC namespace and type changes from `System.Web.Mvc` to ASP.NET Core MVC namespaces.
- Removal of `Global.asax` lifecycle as the application startup mechanism.
- Endpoint routing replaces `RouteCollection` registration.
- Global filters move to MVC options, endpoint filters, or middleware.
- Static file and bundling behavior changes.
- `System.Web` server abstractions, request/response helpers, uploaded file types, and path mapping APIs require ASP.NET Core replacements.
- EF Core `3.1` to `10.0` may introduce query translation and provider behavior changes.
- `System.Messaging` APIs require replacement or redesign.

#### Code Modifications

| Area | Planned Change |
| :--- | :--- |
| Project file | Convert to SDK-style and target `net10.0`. |
| Startup | Create ASP.NET Core hosting entry point and service registrations. |
| Routing | Convert route table registration to endpoint routing. |
| Filters | Convert global MVC filters to ASP.NET Core filters/middleware. |
| Controllers | Update namespaces, base types, action result types, helpers, attributes, and model state usage. |
| Views | Update Razor imports, tag helpers, layout references, script/style references, and validation assets. |
| Static assets | Move/serve assets from `wwwroot`; remove `System.Web.Optimization`. |
| Configuration | Move settings to `appsettings.json` and configuration abstractions. |
| Data access | Update EF Core provider/packages and DI registration. |
| Messaging | Replace direct `System.Messaging` usage. |

#### Validation Checklist

- [ ] Project targets `net10.0`.
- [ ] Project is SDK-style.
- [ ] Unsupported ASP.NET Framework packages removed or replaced.
- [ ] All package upgrades from the assessment applied.
- [ ] `Microsoft.Data.SqlClient` security vulnerability remediated.
- [ ] No direct `System.Web.Mvc`, `System.Web.Optimization`, or `System.Messaging` dependencies remain unless explicitly isolated and justified.
- [ ] Solution restores successfully.
- [ ] Solution builds without errors.
- [ ] Automated tests pass if present.
- [ ] Core web workflows validate successfully.

## 7. Package Update Reference

### Critical and Required Package Actions

| Package | Current Version | Target / Action | Reason | Affected Project |
| :--- | :---: | :--- | :--- | :--- |
| `Microsoft.Data.SqlClient` | `2.1.4` | Upgrade to `7.0.1` | Security vulnerability | `ContosoUniversity.csproj` |
| `Antlr` | `3.4.1.9004` | Replace with `Antlr4` `4.6.6` if parser dependency remains required | Package replacement recommended | `ContosoUniversity.csproj` |
| `Microsoft.AspNet.Web.Optimization` | `1.1.3` | Remove/replace | Incompatible; replace bundling/minification | `ContosoUniversity.csproj` |
| `Microsoft.Identity.Client` | `4.21.1` | Review replacement/upgrade path | Deprecated package | `ContosoUniversity.csproj` |

### Framework and Extensions Package Upgrades

| Package | Current Version | Suggested Version | Reason |
| :--- | :---: | :---: | :--- |
| `Microsoft.Bcl.AsyncInterfaces` | `1.1.1` | `10.0.8` | Upgrade recommended |
| `Microsoft.Bcl.HashCode` | `1.1.1` | `6.0.0` | Upgrade recommended |
| `Microsoft.EntityFrameworkCore` | `3.1.32` | `10.0.8` | Upgrade recommended |
| `Microsoft.EntityFrameworkCore.Abstractions` | `3.1.32` | `10.0.8` | Upgrade recommended |
| `Microsoft.EntityFrameworkCore.Analyzers` | `3.1.32` | `10.0.8` | Upgrade recommended |
| `Microsoft.EntityFrameworkCore.Relational` | `3.1.32` | `10.0.8` | Upgrade recommended |
| `Microsoft.EntityFrameworkCore.SqlServer` | `3.1.32` | `10.0.8` | Upgrade recommended |
| `Microsoft.EntityFrameworkCore.Tools` | `3.1.32` | `10.0.8` | Upgrade recommended |
| `Microsoft.Extensions.Caching.Abstractions` | `3.1.32` | `10.0.8` | Upgrade recommended |
| `Microsoft.Extensions.Caching.Memory` | `3.1.32` | `10.0.8` | Upgrade recommended |
| `Microsoft.Extensions.Configuration` | `3.1.32` | `10.0.8` | Upgrade recommended |
| `Microsoft.Extensions.Configuration.Abstractions` | `3.1.32` | `10.0.8` | Upgrade recommended |
| `Microsoft.Extensions.Configuration.Binder` | `3.1.32` | `10.0.8` | Upgrade recommended |
| `Microsoft.Extensions.DependencyInjection` | `3.1.32` | `10.0.8` | Upgrade recommended |
| `Microsoft.Extensions.DependencyInjection.Abstractions` | `3.1.32` | `10.0.8` | Upgrade recommended |
| `Microsoft.Extensions.Logging` | `3.1.32` | `10.0.8` | Upgrade recommended |
| `Microsoft.Extensions.Logging.Abstractions` | `3.1.32` | `10.0.8` | Upgrade recommended |
| `Microsoft.Extensions.Options` | `3.1.32` | `10.0.8` | Upgrade recommended |
| `Microsoft.Extensions.Primitives` | `3.1.32` | `10.0.8` | Upgrade recommended |
| `Newtonsoft.Json` | `13.0.3` | `13.0.4` | Upgrade recommended |
| `System.Collections.Immutable` | `1.7.1` | `10.0.8` | Upgrade recommended |
| `System.Diagnostics.DiagnosticSource` | `4.7.1` | `10.0.8` | Upgrade recommended |
| `System.Runtime.CompilerServices.Unsafe` | `4.5.3` | `6.1.2` | Upgrade recommended |

### Packages Likely Removed Because Functionality Is Included with Framework Reference

These packages are assessment-marked as functionality included with framework references and should generally not remain as explicit package references after migration to ASP.NET Core/.NET 10 unless a specific compatibility need is documented.

| Package | Current Version | Planned Action |
| :--- | :---: | :--- |
| `Microsoft.AspNet.Mvc` | `5.2.9` | Remove; use ASP.NET Core MVC framework reference. |
| `Microsoft.AspNet.Razor` | `3.2.9` | Remove; use ASP.NET Core Razor support. |
| `Microsoft.AspNet.WebPages` | `3.2.9` | Remove; use ASP.NET Core/Razor support. |
| `Microsoft.CodeDom.Providers.DotNetCompilerPlatform` | `2.0.1` | Remove; SDK compiler toolchain provides compilation. |
| `Microsoft.Web.Infrastructure` | `2.0.1` | Remove; ASP.NET Framework infrastructure package not used in ASP.NET Core. |
| `NETStandard.Library` | `2.0.3` | Remove explicit reference. |
| `System.Buffers` | `4.5.1` | Remove explicit reference unless required transitively. |
| `System.ComponentModel.Annotations` | `4.7.0` | Remove explicit reference unless required by code after migration. |
| `System.Memory` | `4.5.4` | Remove explicit reference unless required transitively. |
| `System.Numerics.Vectors` | `4.5.0` | Remove explicit reference unless required transitively. |
| `System.Threading.Tasks.Extensions` | `4.5.4` | Remove explicit reference unless required transitively. |

### Compatible Packages to Review/Retain as Needed

| Package | Current Version | Assessment Status | Planned Action |
| :--- | :---: | :---: | :--- |
| `bootstrap` | `5.3.3` | Compatible | Retain static/client asset strategy. |
| `jQuery` | `3.7.1` | Compatible | Retain if views still use it. |
| `jQuery.Validation` | `1.21.0` | Compatible | Retain if client validation uses it. |
| `Microsoft.Data.SqlClient.SNI.runtime` | `2.1.1` | Compatible | Review after `Microsoft.Data.SqlClient` upgrade; remove if no longer explicitly needed. |
| `Microsoft.jQuery.Unobtrusive.Validation` | `4.0.0` | Compatible | Retain if unobtrusive validation remains. |
| `Modernizr` | `2.6.2` | Compatible | Review need; retain only if views use it. |
| `WebGrease` | `1.5.2` | Compatible | Review after bundling migration; likely remove if no longer needed. |

## 8. Breaking Changes Catalog

### Technology-Level Breaking Changes

| Technology Area | Issue Count | Impact | Migration Direction |
| :--- | :---: | :--- | :--- |
| ASP.NET Framework (`System.Web`) | 495 | High | Migrate to ASP.NET Core MVC, middleware, endpoint routing, static files, and dependency injection. |
| MSMQ & Message Queuing | 61 | High | Replace `System.Messaging` with supported queue abstraction/provider. |
| Legacy Configuration System | 16 | Medium | Migrate to `Microsoft.Extensions.Configuration`, `appsettings.json`, environment variables, and options pattern. |

### Frequent API Replacements

| Legacy API / Pattern | Frequency | Replacement Direction |
| :--- | :---: | :--- |
| `System.Web.Mvc.ViewResult` | 40 | `Microsoft.AspNetCore.Mvc.ViewResult` / controller `View(...)`. |
| `System.Web.Mvc.ActionResult` | 38 | `Microsoft.AspNetCore.Mvc.IActionResult` or `ActionResult<T>`. |
| `Controller.View(object)` | 34 | ASP.NET Core MVC `View(model)`. |
| `System.Web.Mvc.ModelStateDictionary` and `Controller.ModelState` | 52 combined | ASP.NET Core MVC model state APIs. |
| `ControllerBase.ViewBag` | 23 | ASP.NET Core MVC `ViewBag` or typed view models. |
| `System.Messaging.MessageQueue` | 22 | Queue abstraction over supported provider. |
| `ModelStateDictionary.AddModelError` | 19 | ASP.NET Core `ModelState.AddModelError`. |
| `System.Web.Mvc.SelectList` | 14 | `Microsoft.AspNetCore.Mvc.Rendering.SelectList`. |
| `HttpPostAttribute` | 13 | `Microsoft.AspNetCore.Mvc.HttpPostAttribute`. |
| `RedirectToRouteResult` / `RedirectToAction` | 26 combined | ASP.NET Core redirect result helpers. |
| `HttpStatusCodeResult` / `HttpNotFoundResult` | 25 combined | `StatusCode(...)`, `NotFound()`, `IActionResult`. |
| `ValidateAntiForgeryTokenAttribute` | 12 | ASP.NET Core antiforgery attribute/filter. |
| `BindAttribute` | 7 | ASP.NET Core binding attributes/view models. |
| `JsonResult` / `JsonRequestBehavior` | 10 combined | ASP.NET Core `JsonResult`; remove `JsonRequestBehavior`. |
| `System.Web.Optimization.Bundle` / `ScriptBundle` | 14+ | Static asset references or ASP.NET Core-compatible bundling. |
| `Server.MapPath` / `HttpServerUtilityBase` | 8 combined | `IWebHostEnvironment.WebRootPath` or `ContentRootPath`. |
| `HttpPostedFileBase` | 4 | `IFormFile`. |
| `System.Configuration.ConfigurationManager` | 4 | `Microsoft.Extensions.Configuration`; optional bridge only if needed. |

### Project System Breaking Changes

- Classic WAP imports, build targets, and explicit file includes may not map directly to SDK-style defaults.
- ASP.NET Framework `web.config` runtime configuration does not define ASP.NET Core middleware or service registration.
- Views and static assets must be included using SDK-style conventions and ASP.NET Core static file middleware.

### Package Breaking Changes

- `Microsoft.AspNet.Mvc`, `Microsoft.AspNet.Razor`, and `Microsoft.AspNet.WebPages` are ASP.NET Framework packages and do not carry forward to ASP.NET Core.
- EF Core `3.1.32` to `10.0.8` is a major version jump; query translation, provider behavior, migrations, and nullable/reference behavior may need review.
- `Microsoft.Data.SqlClient` `2.1.4` to `7.0.1` may require connection-string/provider behavior validation.
- `Microsoft.Identity.Client` is marked deprecated in the assessment; authentication flows must be reviewed if used.

### Runtime Behavior Changes to Validate

- Routing URL generation and route matching.
- Antiforgery token generation and validation.
- Model binding and validation messages.
- JSON serialization output and casing if using ASP.NET Core defaults versus `Newtonsoft.Json`.
- Static file paths and cache behavior.
- Database query behavior after EF Core upgrade.
- Message queue behavior after replacing MSMQ APIs.

## 9. Risk Management

### High-Level Risk Summary

| Risk Area | Risk Level | Description | Mitigation |
| :--- | :---: | :--- | :--- |
| ASP.NET Framework to ASP.NET Core migration | High | `System.Web.Mvc` and related APIs account for most compatibility issues. | Use ASP.NET Core MVC equivalents; migrate startup/routing/filters first; then controllers and views. |
| Classic WAP to SDK-style conversion | High | Project file format and web asset handling change significantly. | Convert project structure deliberately and validate included content/static assets. |
| Package remediation | High | 26+ packages require upgrade, replacement, or removal. | Apply all assessment-recommended changes together; resolve dependency conflicts during restore/build. |
| Security vulnerability | High | `Microsoft.Data.SqlClient` `2.1.4` has a security vulnerability. | Upgrade to `7.0.1` as part of the atomic migration. |
| MSMQ/System.Messaging | High | `System.Messaging` is unsupported on modern .NET. | Select a supported messaging provider or isolate/replace queue access. |
| Legacy configuration | Medium | XML/web.config configuration patterns differ from ASP.NET Core. | Move runtime settings to `appsettings.json` and `Microsoft.Extensions.Configuration`. |
| EF Core major version jump | Medium | EF Core `3.1.32` to `10.0.8` may include breaking changes. | Review LINQ queries, migrations, provider behavior, and data access tests. |

### Security Vulnerabilities

| Package | Current Version | Target Version | Severity Handling |
| :--- | :---: | :---: | :--- |
| `Microsoft.Data.SqlClient` | `2.1.4` | `7.0.1` | Must be remediated during the atomic upgrade; do not defer. |

### Contingency Plans

| Blocking Condition | Contingency |
| :--- | :--- |
| ASP.NET Core migration produces extensive controller/view errors | Prioritize shared base patterns, namespaces, imports, and common result helpers before addressing individual actions. |
| EF Core `10.0.8` introduces query/runtime incompatibilities | Isolate failing queries, add focused tests, and adjust LINQ/provider usage while preserving package target from assessment. |
| MSMQ replacement decision is not available | Introduce an internal queue abstraction and defer provider selection behind configuration only if runtime functionality can be validated safely. |
| Static asset migration breaks UI | Prefer direct known asset references first, then introduce bundling/minification after functional parity. |
| Authentication package deprecation affects sign-in flows | Review actual `Microsoft.Identity.Client` usage and select supported replacement/version consistent with `.NET 10.0`. |

## 10. Testing & Validation Strategy

### Automated Validation

If test projects or test commands exist in the repository, they should be run after the atomic upgrade builds successfully. No separate test project was identified in the assessment, so execution should first discover available tests before assuming coverage exists.

### Build Validation

- Restore dependencies for `ContosoUniversity.sln`.
- Build the entire solution.
- Resolve all compilation errors caused by SDK-style conversion, package changes, and API migration.
- Treat warnings related to obsolete APIs, vulnerable packages, or unsupported framework usage as migration blockers unless explicitly accepted.

### Functional Validation Checklist

| Area | Validation |
| :--- | :--- |
| Application startup | App starts under ASP.NET Core hosting without startup exceptions. |
| Routing | Default route and controller/action routes resolve correctly. |
| Home pages | Home and shared layout render without missing assets. |
| Student workflows | List, details, create, edit, delete, and validation flows work. |
| Course workflows | List, details, create, edit, delete, and validation flows work. |
| Instructor workflows | List, details, create, edit, delete, and validation flows work. |
| Enrollment flows | Enrollment relationships load and update correctly. |
| Validation | Server-side and client-side validation display expected messages. |
| Static assets | Bootstrap, jQuery, validation scripts, CSS, and images load from the migrated static asset strategy. |
| Data access | Database connection, queries, updates, and migrations/seeding behavior work with EF Core `10.0.8`. |
| Messaging | Queue-related behavior works according to the selected MSMQ replacement strategy. |
| Configuration | Environment-specific settings and connection strings resolve through ASP.NET Core configuration. |

### Regression Focus Areas

- Route names and generated URLs.
- Antiforgery behavior on POST actions.
- JSON responses and serialization behavior.
- File upload/path mapping logic if present.
- Error pages and exception handling middleware.
- Database provider behavior after `Microsoft.Data.SqlClient` upgrade.

## 11. Complexity & Effort Assessment

### Solution Complexity

| Dimension | Rating | Evidence |
| :--- | :---: | :--- |
| Project count | Low | 1 project |
| Dependency graph | Low | No project dependencies or cycles |
| API migration | High | 573 API compatibility issues |
| Package migration | High | 45 packages, 26+ requiring action |
| Architecture migration | High | ASP.NET Framework/System.Web to ASP.NET Core |
| Data access migration | Medium | EF Core major-version upgrade |
| Messaging migration | High | MSMQ unsupported in modern .NET |
| Overall | High | Single-project scope with major web framework architecture shift |

### Per-Project Complexity

| Project | Complexity | Risk | Key Drivers |
| :--- | :---: | :--- | :--- |
| `ContosoUniversity.csproj` | High | High | Classic WAP, System.Web MVC APIs, SDK-style conversion, package remediation, MSMQ, configuration migration. |

### Skill Areas Needed

- ASP.NET Core MVC application architecture.
- SDK-style project migration.
- Razor view migration.
- EF Core major-version upgrade validation.
- Configuration and dependency injection patterns.
- Messaging/queue abstraction design.

## 12. Source Control Strategy

### Branching Strategy

- Use the upgrade branch `upgrade-to-NET10` for planning and execution artifacts.
- Keep the source branch `example2` unchanged except through normal review/merge practices.
- Do not mix unrelated feature work with the migration branch.

### Commit Strategy

For the execution stage, prefer a single cohesive migration commit or pull request representing the atomic upgrade when practical. If repository policy requires multiple commits, keep them logically grouped while ensuring the branch is only considered complete after all atomic upgrade changes build and validate together.

Suggested commit scope:

- SDK-style and target framework conversion.
- Package remediation.
- ASP.NET Core architecture/code migration.
- Validation fixes and test updates.

### Review Checklist

- Project targets `net10.0` and is SDK-style.
- All package actions from this plan are applied.
- Security vulnerability is remediated.
- No unsupported ASP.NET Framework runtime dependencies remain.
- Build and test evidence is attached to the review.
- Functional validation checklist has been completed or documented with exceptions.

## 13. Success Criteria

### Technical Criteria

- `ContosoUniversity.csproj` targets `net10.0`.
- `ContosoUniversity.csproj` is SDK-style.
- All required package upgrades, replacements, and removals from the assessment are applied.
- `Microsoft.Data.SqlClient` is upgraded from `2.1.4` to `7.0.1`.
- ASP.NET Framework MVC/System.Web application patterns are migrated to ASP.NET Core equivalents.
- `System.Web.Optimization` bundling is removed or replaced.
- `System.Messaging` usage is replaced, isolated, or otherwise resolved with a supported strategy.
- Legacy configuration is migrated to ASP.NET Core configuration patterns.
- Solution restore completes without package conflicts.
- Solution build completes without errors.
- Automated tests pass if present.
- Critical Contoso University workflows validate successfully.

### Quality Criteria

- No unresolved high-risk package vulnerabilities remain from the assessment.
- Migration avoids introducing unsupported compatibility shims unless explicitly justified.
- Code follows ASP.NET Core conventions for dependency injection, configuration, middleware, routing, and MVC.
- Static assets and views are maintainable under ASP.NET Core project conventions.
- Data access behavior is validated after EF Core upgrade.

### Process Criteria

- All-at-once strategy is followed: project format, framework, packages, and API migration are coordinated as a single upgrade outcome.
- Migration changes are reviewed together for consistency.
- Validation evidence is captured before merge.
- Any deviations from assessment recommendations are documented with rationale.

## 14. Assumptions and Open Items

### Assumptions

- The target framework is `.NET 10.0` (`net10.0`).
- The active upgrade branch is `upgrade-to-NET10`.
- The assessment package versions are authoritative for this plan.
- No project dependency cycles exist because the solution contains one project.

### Open Items Requiring Execution-Stage Decisions

| Item | Decision Needed |
| :--- | :--- |
| MSMQ replacement | Choose RabbitMQ, Azure Service Bus, in-memory/no-op, or another supported provider based on actual runtime needs. |
| Bundling/minification | Choose direct static asset references or an ASP.NET Core-compatible asset pipeline. |
| `Microsoft.Identity.Client` deprecation | Confirm whether the app uses this package and choose supported remediation. |
| Test coverage | Discover whether automated tests exist; if not, define minimum validation coverage for critical workflows. |
| Hosting model | Confirm IIS, Kestrel, or other hosting requirements for final deployment. |
