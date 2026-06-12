# Upgrade Options — ContosoUniversity

Assessment: 1 .NET Framework 4.8 ASP.NET Framework web project targeting net10.0, with 658 issues across 24 files, System.Web migration requirements, 2 incompatible packages, 24 package upgrades, 1 vulnerable package, and binding redirect conflicts.

## Strategy

### Upgrade Strategy
A single .NET Framework web project was assessed, so the upgrade does not need dependency-tier phasing.

| Value | Description |
|-------|-------------|
| **All-at-Once** (selected) | Upgrade the project in one coordinated pass and validate the full solution after the migration. |

## Project Structure

### Project Approach
The project is an ASP.NET Framework/System.Web web application with high-risk migration signals, so an incremental side-by-side ASP.NET Core migration is the safer default.

| Value | Description |
|-------|-------------|
| **Side-by-side** (selected) | Create a new ASP.NET Core project alongside the existing Framework project and migrate assets incrementally while the old project stays live. |
| In-place rewrite | Replace the Framework web project entirely in one pass; faster for small low-risk projects but higher risk here. |

## Compatibility

### Unsupported Packages
The assessment found 2 incompatible packages, a small enough set to resolve during the migration tasks.

| Value | Description |
|-------|-------------|
| **Resolve Inline** (selected) | Research and resolve incompatible package references within the same task, removing old references or replacing consuming code as needed. |
| Defer Resolution | Make the project compile with temporary stubs and create follow-up tasks for real replacements. |
| Compatibility Mode | Keep legacy references using compatibility mechanisms; use only for transitive dependencies or narrow Windows-only cases. |

### Unsupported API Handling
The assessment found binary and source incompatible APIs that must be handled during the framework migration.

| Value | Description |
|-------|-------------|
| **Fix Inline** (selected) | Resolve API changes in the same migration task, including complex replacements where required. |
| Defer Complex Changes | Apply simple replacements inline but use temporary stubs for complex changes and create follow-up resolution tasks. |

### System.Web Adapters
System.Web/ASP.NET Framework usage is extensive and side-by-side migration is selected.

| Value | Description |
|-------|-------------|
| **Use System.Web Adapters** (selected) | Add Microsoft.AspNetCore.SystemWebAdapters compatibility shims to enable incremental migration from System.Web APIs. |
| Direct Migration to ASP.NET Core APIs | Replace all System.Web usage immediately with native ASP.NET Core APIs without compatibility shims. |

## Modernization

### Configuration Migration
The assessment detected legacy configuration system usage that must be converted for ASP.NET Core.

| Value | Description |
|-------|-------------|
| **Auto-migrate to .NET Core Configuration** (selected) | Convert web.config/appSettings and connection string usage to appsettings.json and IConfiguration during the migration. |
| Manual Migration with Mapping Document | Generate a detailed settings mapping before migration for complex or business-sensitive configuration. |

### Assembly Binding Redirects
The assessment found multiple binding redirect issues, including missing redirects, conflicts, and version downgrades.

| Value | Description |
|-------|-------------|
| Remove Binding Redirects | Remove redirects inline because modern .NET does not use .NET Framework binding redirects. |
| **Document and Review Before Removing** (selected) | Review redirects first because the volume and conflicts may represent underlying dependency version issues. |
