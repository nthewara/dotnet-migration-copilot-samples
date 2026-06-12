# Projects and dependencies analysis

This document provides a comprehensive overview of the projects and their dependencies in the context of upgrading to .NETCoreApp,Version=v10.0.

## Table of Contents

- [Executive Summary](#executive-Summary)
  - [Highlevel Metrics](#highlevel-metrics)
  - [Projects Compatibility](#projects-compatibility)
  - [Package Compatibility](#package-compatibility)
  - [API Compatibility](#api-compatibility)
  - [Binding Redirect Configuration](#binding-redirect-configuration)
- [Aggregate NuGet packages details](#aggregate-nuget-packages-details)
- [Top API Migration Challenges](#top-api-migration-challenges)
  - [Technologies and Features](#technologies-and-features)
  - [Most Frequent API Issues](#most-frequent-api-issues)
- [Projects Relationship Graph](#projects-relationship-graph)
- [Project Details](#project-details)

  - [ContosoUniversity.csproj](#contosouniversitycsproj)


## Executive Summary

### Highlevel Metrics

| Metric | Count | Status |
| :--- | :---: | :--- |
| Total Projects | 1 | All require upgrade |
| Total NuGet Packages | 45 | 26 need upgrade |
| Total Code Files | 56 |  |
| Total Code Files with Incidents | 24 |  |
| Total Lines of Code | 3409 |  |
| Total Number of Issues | 658 |  |
| Estimated LOC to modify | 573+ | at least 16.8% of codebase |

### Projects Compatibility

| Project | Target Framework | Difficulty | Package Issues | API Issues | Binding Issues | Est. LOC Impact | Description |
| :--- | :---: | :---: | :---: | :---: | :---: | :---: | :--- |
| [ContosoUniversity.csproj](#contosouniversitycsproj) | net48 | 🔴 High | 41 | 573 | 26 | 573+ | Wap, Sdk Style = False |

### Package Compatibility

| Status | Count | Percentage |
| :--- | :---: | :---: |
| ✅ Compatible | 19 | 42.2% |
| ⚠️ Incompatible | 2 | 4.4% |
| 🔄 Upgrade Recommended | 24 | 53.3% |
| ***Total NuGet Packages*** | ***45*** | ***100%*** |

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 536 | High - Require code changes |
| 🟡 Source Incompatible | 37 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 1884 |  |
| ***Total APIs Analyzed*** | ***2457*** |  |

### Binding Redirect Configuration

| Severity | Count | Description |
| :--- | :---: | :--- |
| 🔴Mandatory | 7 | Must be fixed to avoid runtime failures |
| 🟡Potential | 19 | May cause issues in certain scenarios |
| ***Total Binding Issues*** | ***26*** | ***Across 1 project(s)*** |

## Aggregate NuGet packages details

| Package | Current Version | Suggested Version | Projects | Description |
| :--- | :---: | :---: | :--- | :--- |
| Antlr | 3.4.1.9004 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | Needs to be replaced with Replace with new package Antlr4=4.6.6 |
| bootstrap | 5.3.3 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | ✅Compatible |
| jQuery | 3.7.1 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | ✅Compatible |
| jQuery.Validation | 1.21.0 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | ✅Compatible |
| Microsoft.AspNet.Mvc | 5.2.9 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package functionality is included with framework reference |
| Microsoft.AspNet.Razor | 3.2.9 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package functionality is included with framework reference |
| Microsoft.AspNet.Web.Optimization | 1.1.3 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | ⚠️NuGet package is incompatible |
| Microsoft.AspNet.WebPages | 3.2.9 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package functionality is included with framework reference |
| Microsoft.Bcl.AsyncInterfaces | 1.1.1 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.Bcl.HashCode | 1.1.1 | 6.0.0 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.CodeDom.Providers.DotNetCompilerPlatform | 2.0.1 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package functionality is included with framework reference |
| Microsoft.Data.SqlClient | 2.1.4 | 7.0.1 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package contains security vulnerability |
| Microsoft.Data.SqlClient.SNI.runtime | 2.1.1 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | ✅Compatible |
| Microsoft.EntityFrameworkCore | 3.1.32 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.EntityFrameworkCore.Abstractions | 3.1.32 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.EntityFrameworkCore.Analyzers | 3.1.32 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.EntityFrameworkCore.Relational | 3.1.32 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.EntityFrameworkCore.SqlServer | 3.1.32 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.EntityFrameworkCore.Tools | 3.1.32 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.Extensions.Caching.Abstractions | 3.1.32 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.Extensions.Caching.Memory | 3.1.32 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.Extensions.Configuration | 3.1.32 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.Extensions.Configuration.Abstractions | 3.1.32 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.Extensions.Configuration.Binder | 3.1.32 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.Extensions.DependencyInjection | 3.1.32 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.Extensions.DependencyInjection.Abstractions | 3.1.32 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.Extensions.Logging | 3.1.32 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.Extensions.Logging.Abstractions | 3.1.32 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.Extensions.Options | 3.1.32 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.Extensions.Primitives | 3.1.32 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| Microsoft.Identity.Client | 4.21.1 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | ⚠️NuGet package is deprecated |
| Microsoft.jQuery.Unobtrusive.Validation | 4.0.0 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | ✅Compatible |
| Microsoft.Web.Infrastructure | 2.0.1 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package functionality is included with framework reference |
| Modernizr | 2.6.2 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | ✅Compatible |
| NETStandard.Library | 2.0.3 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package functionality is included with framework reference |
| Newtonsoft.Json | 13.0.3 | 13.0.4 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| System.Buffers | 4.5.1 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package functionality is included with framework reference |
| System.Collections.Immutable | 1.7.1 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| System.ComponentModel.Annotations | 4.7.0 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package functionality is included with framework reference |
| System.Diagnostics.DiagnosticSource | 4.7.1 | 10.0.9 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| System.Memory | 4.5.4 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package functionality is included with framework reference |
| System.Numerics.Vectors | 4.5.0 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package functionality is included with framework reference |
| System.Runtime.CompilerServices.Unsafe | 4.5.3 | 6.1.2 | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package upgrade is recommended |
| System.Threading.Tasks.Extensions | 4.5.4 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | NuGet package functionality is included with framework reference |
| WebGrease | 1.5.2 |  | [ContosoUniversity.csproj](#contosouniversitycsproj) | ✅Compatible |

## Top API Migration Challenges

### Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |
| ASP.NET Framework (System.Web) | 495 | 86.4% | Legacy ASP.NET Framework APIs for web applications (System.Web.*) that don't exist in ASP.NET Core due to architectural differences. ASP.NET Core represents a complete redesign of the web framework. Migrate to ASP.NET Core equivalents or consider System.Web.Adapters package for compatibility. |
| MSMQ & Message Queuing | 61 | 10.6% | Microsoft Message Queue (MSMQ) APIs for Windows-based message queuing that are not supported in .NET Core/.NET. MSMQ is a Windows-specific technology. Migrate to RabbitMQ, Azure Service Bus, or other modern message queues. |
| Legacy Configuration System | 16 | 2.8% | Legacy XML-based configuration system (app.config/web.config) that has been replaced by a more flexible configuration model in .NET Core. The old system was rigid and XML-based. Migrate to Microsoft.Extensions.Configuration with JSON/environment variables; use System.Configuration.ConfigurationManager NuGet package as interim bridge if needed. |

### Most Frequent API Issues

| API | Count | Percentage | Category |
| :--- | :---: | :---: | :--- |
| T:System.Web.Mvc.ViewResult | 40 | 7.0% | Binary Incompatible |
| T:System.Web.Mvc.ActionResult | 38 | 6.6% | Binary Incompatible |
| M:System.Web.Mvc.Controller.View(System.Object) | 34 | 5.9% | Binary Incompatible |
| T:System.Web.Mvc.ModelStateDictionary | 26 | 4.5% | Binary Incompatible |
| P:System.Web.Mvc.Controller.ModelState | 26 | 4.5% | Binary Incompatible |
| P:System.Web.Mvc.ControllerBase.ViewBag | 23 | 4.0% | Binary Incompatible |
| T:System.Messaging.MessageQueue | 22 | 3.8% | Binary Incompatible |
| M:System.Web.Mvc.ModelStateDictionary.AddModelError(System.String,System.String) | 19 | 3.3% | Binary Incompatible |
| T:System.Web.Mvc.SelectList | 14 | 2.4% | Binary Incompatible |
| M:System.Web.Mvc.HttpPostAttribute.#ctor | 13 | 2.3% | Binary Incompatible |
| T:System.Web.Mvc.HttpPostAttribute | 13 | 2.3% | Binary Incompatible |
| T:System.Web.Mvc.RedirectToRouteResult | 13 | 2.3% | Binary Incompatible |
| M:System.Web.Mvc.Controller.RedirectToAction(System.String) | 13 | 2.3% | Binary Incompatible |
| T:System.Web.Mvc.HttpStatusCodeResult | 13 | 2.3% | Binary Incompatible |
| M:System.Web.Mvc.HttpStatusCodeResult.#ctor(System.Net.HttpStatusCode) | 13 | 2.3% | Binary Incompatible |
| M:System.Web.Mvc.ValidateAntiForgeryTokenAttribute.#ctor | 12 | 2.1% | Binary Incompatible |
| T:System.Web.Mvc.ValidateAntiForgeryTokenAttribute | 12 | 2.1% | Binary Incompatible |
| T:System.Web.Mvc.HttpNotFoundResult | 12 | 2.1% | Binary Incompatible |
| M:System.Web.Mvc.Controller.HttpNotFound | 12 | 2.1% | Binary Incompatible |
| M:System.Web.Mvc.SelectList.#ctor(System.Collections.IEnumerable,System.String,System.String,System.Object) | 12 | 2.1% | Binary Incompatible |
| M:System.Web.Mvc.BindAttribute.#ctor | 7 | 1.2% | Binary Incompatible |
| T:System.Web.Mvc.BindAttribute | 7 | 1.2% | Binary Incompatible |
| P:System.Web.Mvc.ModelStateDictionary.IsValid | 7 | 1.2% | Binary Incompatible |
| M:System.Web.Mvc.Controller.View | 6 | 1.0% | Binary Incompatible |
| T:System.Web.Mvc.JsonResult | 6 | 1.0% | Binary Incompatible |
| T:System.Web.Optimization.Bundle | 5 | 0.9% | Binary Incompatible |
| M:System.Web.Optimization.BundleCollection.Add(System.Web.Optimization.Bundle) | 5 | 0.9% | Binary Incompatible |
| T:System.Messaging.MessageQueueAccessRights | 4 | 0.7% | Binary Incompatible |
| T:System.Configuration.ConfigurationManager | 4 | 0.7% | Source Incompatible |
| M:System.Web.Mvc.ActionNameAttribute.#ctor(System.String) | 4 | 0.7% | Binary Incompatible |
| T:System.Web.Mvc.ActionNameAttribute | 4 | 0.7% | Binary Incompatible |
| T:System.Web.HttpServerUtilityBase | 4 | 0.7% | Source Incompatible |
| P:System.Web.Mvc.Controller.Server | 4 | 0.7% | Binary Incompatible |
| M:System.Web.HttpServerUtilityBase.MapPath(System.String) | 4 | 0.7% | Source Incompatible |
| P:System.Web.HttpPostedFileBase.ContentLength | 4 | 0.7% | Source Incompatible |
| T:System.Web.Mvc.JsonRequestBehavior | 4 | 0.7% | Binary Incompatible |
| T:System.Web.Optimization.ScriptBundle | 4 | 0.7% | Binary Incompatible |
| M:System.Web.Optimization.ScriptBundle.#ctor(System.String) | 4 | 0.7% | Binary Incompatible |
| T:System.Messaging.MessageQueueErrorCode | 3 | 0.5% | Binary Incompatible |
| T:System.Messaging.MessagePriority | 3 | 0.5% | Binary Incompatible |
| T:System.Web.Mvc.UrlParameter | 3 | 0.5% | Binary Incompatible |
| M:System.Web.Optimization.Bundle.Include(System.String,System.Web.Optimization.IItemTransform[]) | 3 | 0.5% | Binary Incompatible |
| T:System.Messaging.Message | 2 | 0.3% | Binary Incompatible |
| T:System.Messaging.XmlMessageFormatter | 2 | 0.3% | Binary Incompatible |
| M:System.Messaging.XmlMessageFormatter.#ctor(System.Type[]) | 2 | 0.3% | Binary Incompatible |
| T:System.Messaging.IMessageFormatter | 2 | 0.3% | Binary Incompatible |
| P:System.Messaging.MessageQueue.Formatter | 2 | 0.3% | Binary Incompatible |
| M:System.Messaging.MessageQueue.#ctor(System.String) | 2 | 0.3% | Binary Incompatible |
| F:System.Messaging.MessageQueueAccessRights.FullControl | 2 | 0.3% | Binary Incompatible |
| M:System.Messaging.MessageQueue.SetPermissions(System.String,System.Messaging.MessageQueueAccessRights) | 2 | 0.3% | Binary Incompatible |

## Projects Relationship Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart LR
    P1["<b>⚙️&nbsp;ContosoUniversity.csproj</b><br/><small>net48</small>"]
    click P1 "#contosouniversitycsproj"

```

## Project Details

<a id="contosouniversitycsproj"></a>
### ContosoUniversity.csproj

#### Project Info

- **Current Target Framework:** net48
- **Proposed Target Framework:** net10.0
- **SDK-style**: False
- **Project Kind:** Wap
- **Dependencies**: 0
- **Dependants**: 0
- **Number of Files**: 84
- **Number of Files with Incidents**: 24
- **Lines of Code**: 3409
- **Estimated LOC to modify**: 573+ (at least 16.8% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["ContosoUniversity.csproj"]
        MAIN["<b>⚙️&nbsp;ContosoUniversity.csproj</b><br/><small>net48</small>"]
        click MAIN "#contosouniversitycsproj"
    end

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 536 | High - Require code changes |
| 🟡 Source Incompatible | 37 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 1884 |  |
| ***Total APIs Analyzed*** | ***2457*** |  |

#### Binding Redirect Configuration

| Rule | Severity | Details | Recommendation |
| :--- | :---: | :--- | :--- |
| Missing binding redirect for referenced assembly | 🟡Potential | Manual redirects exist but none covers Microsoft.EntityFrameworkCore (referenced v3.1.32.0, package v3.1.32) | Add a binding redirect for the missing assembly. |
| Missing binding redirect for referenced assembly | 🟡Potential | Manual redirects exist but none covers Microsoft.EntityFrameworkCore.SqlServer (referenced v3.1.32.0, package v3.1.32) | Add a binding redirect for the missing assembly. |
| Missing binding redirect for referenced assembly | 🟡Potential | Manual redirects exist but none covers Microsoft.EntityFrameworkCore.Relational (referenced v3.1.32.0, package v3.1.32) | Add a binding redirect for the missing assembly. |
| Missing binding redirect for referenced assembly | 🟡Potential | Manual redirects exist but none covers Microsoft.Bcl.AsyncInterfaces (referenced v1.0.0.0, package v1.1.1) | Add a binding redirect for the missing assembly. |
| Missing binding redirect for referenced assembly | 🟡Potential | Manual redirects exist but none covers Microsoft.Extensions.Caching.Memory (referenced v3.1.32.0, package v3.1.32) | Add a binding redirect for the missing assembly. |
| Missing binding redirect for referenced assembly | 🟡Potential | Manual redirects exist but none covers Microsoft.Extensions.Configuration (referenced v3.1.32.0, package v3.1.32) | Add a binding redirect for the missing assembly. |
| Missing binding redirect for referenced assembly | 🟡Potential | Manual redirects exist but none covers Microsoft.Extensions.Configuration.Binder (referenced v3.1.32.0, package v3.1.32) | Add a binding redirect for the missing assembly. |
| Missing binding redirect for referenced assembly | 🟡Potential | Manual redirects exist but none covers Microsoft.Extensions.Logging (referenced v3.1.32.0, package v3.1.32) | Add a binding redirect for the missing assembly. |
| Missing binding redirect for referenced assembly | 🟡Potential | Manual redirects exist but none covers System.Diagnostics.DiagnosticSource (referenced v4.0.5.0, package v4.7.1) | Add a binding redirect for the missing assembly. |
| Missing binding redirect for referenced assembly | 🟡Potential | Manual redirects exist but none covers System.Collections.Immutable (referenced v1.2.5.0, package v1.7.1) | Add a binding redirect for the missing assembly. |
| Missing binding redirect for referenced assembly | 🟡Potential | Manual redirects exist but none covers Microsoft.Identity.Client (referenced v4.21.1.0, package v4.21.1) | Add a binding redirect for the missing assembly. |
| Missing binding redirect for referenced assembly | 🟡Potential | Manual redirects exist but none covers Microsoft.CodeDom.Providers.DotNetCompilerPlatform (referenced v2.0.1.0, package v2.0.1) | Add a binding redirect for the missing assembly. |
| Manual redirect conflicts with auto-generated version | 🔴Mandatory | Manual redirect for Newtonsoft.Json targets 13.0.0.0 but auto-generation would target 13.0.3 (MSB3836 conflict) | Remove the conflicting manual binding redirect or disable auto-generation. |
| Manual redirect conflicts with auto-generated version | 🔴Mandatory | Manual redirect for System.Threading.Tasks.Extensions targets 4.2.0.1 but auto-generation would target 4.5.4 (MSB3836 conflict) | Remove the conflicting manual binding redirect or disable auto-generation. |
| Manual redirect conflicts with auto-generated version | 🔴Mandatory | Manual redirect for Microsoft.Bcl.HashCode targets 1.0.0.0 but auto-generation would target 1.1.1 (MSB3836 conflict) | Remove the conflicting manual binding redirect or disable auto-generation. |
| Manual redirect conflicts with auto-generated version | 🔴Mandatory | Manual redirect for System.ComponentModel.Annotations targets 4.2.1.0 but auto-generation would target 4.7.0 (MSB3836 conflict) | Remove the conflicting manual binding redirect or disable auto-generation. |
| Manual redirect conflicts with auto-generated version | 🔴Mandatory | Manual redirect for System.Runtime.CompilerServices.Unsafe targets 4.0.6.0 but auto-generation would target 4.5.3 (MSB3836 conflict) | Remove the conflicting manual binding redirect or disable auto-generation. |
| Manual redirect conflicts with auto-generated version | 🔴Mandatory | Manual redirect for System.Memory targets 4.0.1.1 but auto-generation would target 4.5.4 (MSB3836 conflict) | Remove the conflicting manual binding redirect or disable auto-generation. |
| Manual redirect conflicts with auto-generated version | 🔴Mandatory | Manual redirect for Microsoft.Data.SqlClient targets 2.0.20168.4 but auto-generation would target 2.1.4 (MSB3836 conflict) | Remove the conflicting manual binding redirect or disable auto-generation. |
| Binding redirect forces version downgrade | 🟡Potential | Binding redirect for Microsoft.Bcl.HashCode targets 1.0.0.0 but package provides 1.1.1 | Update the binding redirect newVersion to match the version provided by the NuGet package. |
| Binding redirect forces version downgrade | 🟡Potential | Binding redirect for Microsoft.Data.SqlClient targets 2.0.20168.4 but package provides 2.1.4 | Update the binding redirect newVersion to match the version provided by the NuGet package. |
| Binding redirect forces version downgrade | 🟡Potential | Binding redirect for Newtonsoft.Json targets 13.0.0.0 but package provides 13.0.3 | Update the binding redirect newVersion to match the version provided by the NuGet package. |
| Binding redirect forces version downgrade | 🟡Potential | Binding redirect for System.ComponentModel.Annotations targets 4.2.1.0 but package provides 4.7.0 | Update the binding redirect newVersion to match the version provided by the NuGet package. |
| Binding redirect forces version downgrade | 🟡Potential | Binding redirect for System.Memory targets 4.0.1.1 but package provides 4.5.4 | Update the binding redirect newVersion to match the version provided by the NuGet package. |
| Binding redirect forces version downgrade | 🟡Potential | Binding redirect for System.Runtime.CompilerServices.Unsafe targets 4.0.6.0 but package provides 4.5.3 | Update the binding redirect newVersion to match the version provided by the NuGet package. |
| Binding redirect forces version downgrade | 🟡Potential | Binding redirect for System.Threading.Tasks.Extensions targets 4.2.0.1 but package provides 4.5.4 | Update the binding redirect newVersion to match the version provided by the NuGet package. |

#### Project Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |
| MSMQ & Message Queuing | 61 | 10.6% | Microsoft Message Queue (MSMQ) APIs for Windows-based message queuing that are not supported in .NET Core/.NET. MSMQ is a Windows-specific technology. Migrate to RabbitMQ, Azure Service Bus, or other modern message queues. |
| Legacy Configuration System | 16 | 2.8% | Legacy XML-based configuration system (app.config/web.config) that has been replaced by a more flexible configuration model in .NET Core. The old system was rigid and XML-based. Migrate to Microsoft.Extensions.Configuration with JSON/environment variables; use System.Configuration.ConfigurationManager NuGet package as interim bridge if needed. |
| ASP.NET Framework (System.Web) | 495 | 86.4% | Legacy ASP.NET Framework APIs for web applications (System.Web.*) that don't exist in ASP.NET Core due to architectural differences. ASP.NET Core represents a complete redesign of the web framework. Migrate to ASP.NET Core equivalents or consider System.Web.Adapters package for compatibility. |

