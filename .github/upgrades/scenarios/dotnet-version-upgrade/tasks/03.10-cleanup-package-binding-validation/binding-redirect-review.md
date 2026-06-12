# Binding Redirect Review

## Scope
Reviewed legacy Framework binding redirects in `ContosoUniversity/Web.config` before cleanup decisions, per the approved option **Document and Review Before Removing**.

## Findings

| Assembly | oldVersion | newVersion | Migration note |
|---|---:|---:|---|
| Microsoft.Web.Infrastructure | 0.0.0.0-2.0.1.0 | 2.0.1.0 | Legacy ASP.NET MVC infrastructure; not used by `ContosoUniversity.Core`. |
| Antlr3.Runtime | 0.0.0.0-3.4.1.9004 | 3.4.1.9004 | Legacy WebGrease/bundling dependency; not used by `ContosoUniversity.Core`. |
| Newtonsoft.Json | 0.0.0.0-13.0.0.0 | 13.0.0.0 | Legacy project dependency; Core notification serialization uses `System.Text.Json`. |
| System.Web.Optimization | 1.0.0.0-1.1.0.0 | 1.1.0.0 | Replaced in Core by direct static asset references under `wwwroot`. |
| WebGrease | 0.0.0.0-1.5.2.14234 | 1.5.2.14234 | Legacy bundling dependency; not used by `ContosoUniversity.Core`. |
| System.Web.Helpers | 1.0.0.0-3.0.0.0 | 3.0.0.0 | Legacy MVC helper assembly; not used by `ContosoUniversity.Core`. |
| System.Web.WebPages | 1.0.0.0-3.0.0.0 | 3.0.0.0 | Legacy Razor/WebPages assembly; not used by `ContosoUniversity.Core`. |
| System.Web.Mvc | 1.0.0.0-5.2.9.0 | 5.2.9.0 | Replaced by ASP.NET Core MVC in `ContosoUniversity.Core`. |
| System.Threading.Tasks.Extensions | 0.0.0.0-4.2.0.1 | 4.2.0.1 | Legacy transitive dependency redirect; Core resolves via NuGet/runtime assets. |
| Microsoft.Bcl.HashCode | 0.0.0.0-1.0.0.0 | 1.0.0.0 | Legacy transitive dependency redirect; Core resolves via NuGet/runtime assets. |
| Microsoft.Extensions.DependencyInjection.Abstractions | 0.0.0.0-3.1.32.0 | 3.1.32.0 | Legacy EF Core 3.1 dependency redirect; Core uses current package graph. |
| Microsoft.Extensions.DependencyInjection | 0.0.0.0-3.1.32.0 | 3.1.32.0 | Legacy EF Core 3.1 dependency redirect; Core uses current package graph. |
| Microsoft.EntityFrameworkCore.Abstractions | 0.0.0.0-3.1.32.0 | 3.1.32.0 | Legacy EF Core 3.1 dependency redirect; Core uses EF Core 10.0.9. |
| Microsoft.Extensions.Caching.Abstractions | 0.0.0.0-3.1.32.0 | 3.1.32.0 | Legacy EF Core 3.1 dependency redirect; Core uses current package graph. |
| Microsoft.Extensions.Configuration.Abstractions | 0.0.0.0-3.1.32.0 | 3.1.32.0 | Legacy EF Core 3.1 dependency redirect; Core uses ASP.NET Core configuration. |
| Microsoft.Extensions.Logging.Abstractions | 0.0.0.0-3.1.32.0 | 3.1.32.0 | Legacy EF Core 3.1 dependency redirect; Core uses current package graph. |
| Microsoft.Extensions.Options | 0.0.0.0-3.1.32.0 | 3.1.32.0 | Legacy dependency redirect; Core uses options from current package graph. |
| Microsoft.Extensions.Primitives | 0.0.0.0-3.1.32.0 | 3.1.32.0 | Legacy dependency redirect; Core resolves via current package graph. |
| System.ComponentModel.Annotations | 0.0.0.0-4.2.1.0 | 4.2.1.0 | Legacy data annotations redirect; Core uses framework/package assets. |
| System.Runtime.CompilerServices.Unsafe | 0.0.0.0-4.0.6.0 | 4.0.6.0 | Legacy transitive dependency redirect; Core resolves via NuGet/runtime assets. |
| System.Memory | 0.0.0.0-4.0.1.1 | 4.0.1.1 | Legacy transitive dependency redirect; Core resolves via NuGet/runtime assets. |
| Microsoft.Data.SqlClient | 0.0.0.0-2.0.20168.4 | 2.0.20168.4 | Legacy vulnerable package line from assessment; Core uses current EF/SqlClient dependency graph. |
| netstandard | 0.0.0.0-2.0.0.0 | 2.0.0.0 | .NET Framework compatibility redirect; not used by modern .NET output. |

## Decision
- Do not remove `Web.config` redirects from the old Framework project during this agent-run because the old project remains live and deployable in the side-by-side migration.
- No binding redirects are required or copied into `ContosoUniversity.Core`; modern .NET resolves assemblies through the SDK/runtime dependency graph.
- When the user is ready to retire the old Framework project, these redirects can be removed together with the old project and its `packages.config` dependency model.
