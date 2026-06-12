# .NET Version Upgrade

## Preferences
- **Flow Mode**: Guided
- **Target Framework**: net10.0

## Source Control
- **Source Branch**: main
- **Working Branch**: upgrade-dotnet-10
- **Commit Strategy**: Single Commit at End
- **Branch Sync**: Auto (Merge)

## Upgrade Options
**Source**: .github/upgrades/scenarios/dotnet-version-upgrade/upgrade-options.md

### Strategy
- Upgrade Strategy: All-at-Once

### Project Structure
- Project Approach: Side-by-side

### Compatibility
- Unsupported Packages: Resolve Inline (2 incompatible packages)
- Unsupported API Handling: Fix Inline
- System.Web Adapters: Use System.Web Adapters
  Skill: aspnet-system-web-adapters

### Modernization
- Configuration Migration: Auto-migrate to .NET Core Configuration
- Assembly Binding Redirects: Document and Review Before Removing

## Strategy
**Selected**: All-At-Once
**Rationale**: Single .NET Framework 4.8 ASP.NET Framework web project; no dependency graph phasing is needed, and the selected side-by-side approach isolates the new ASP.NET Core net10.0 project while the old Framework app remains live.

### Execution Constraints
- Single coordinated migration for the assessed application; validate the full solution after the Core project scaffold and after web migration.
- Side-by-side web migration modifies strategy execution: the old Framework project stays in place while a new ASP.NET Core project is scaffolded and migrated.
- Resolve incompatible packages and API changes inline during migration tasks; do not create deferred stub cleanup work unless execution becomes blocked.
- Review binding redirects before removing them so dependency version conflicts are understood during package cleanup.

### Side-by-Side Web Migration Constraints
- Scaffold task must complete and validate (builds, stub 200 response) before migrate starts
- Old Framework project remains live and deployable throughout entire migrate phase
- Migrate task will be broken into subtasks at execution time — load migrating-aspnet-framework-to-core skill
- Libraries in migrate task scope are handled in dependency order before web layer assets
- Reference cleanup (test projects, multi-targeting) is part of migrate, not a separate task
- Old project is NOT deleted by the agent — documented as post-upgrade step for user

## Build Tool Decisions
- **ContosoUniversity.csproj**: Visual Studio MSBuild (`MSBuild.exe`) for the existing legacy non-SDK-style ASP.NET Framework web project.
- **ContosoUniversity.Core.csproj**: `dotnet build` for the new SDK-style ASP.NET Core `net10.0` project.

## Key Decisions Log
- User selected Guided mode during initialization.
- User approved the generated assessment and confirmed proceeding to planning.
- User approved the generated upgrade options for planning.
- Commit strategy updated to Single Commit at End because the selected All-At-Once strategy is best represented as one atomic upgrade commit.
- User approved the generated plan and confirmed proceeding to execution.
