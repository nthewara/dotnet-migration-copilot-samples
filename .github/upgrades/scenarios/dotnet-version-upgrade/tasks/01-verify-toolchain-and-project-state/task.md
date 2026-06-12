# 01-verify-toolchain-and-project-state: Verify upgrade prerequisites

Verify the repository, solution, installed .NET SDK support for `net10.0`, and the current state of `ContosoUniversity.csproj` before making migration changes. This task should confirm the source branch and working branch are correct, check `global.json` compatibility if present, confirm the legacy web project remains buildable before migration, and inventory the project shape needed by the side-by-side scaffold.

The assessment found an old-style .NET Framework web application with legacy `System.Web`/ASP.NET Framework features, packages.config-style migration risks, and binding redirect conflicts. This prerequisite task captures the starting point so later scaffold and migration work can be validated against a known baseline.

**Done when**: The `net10.0` SDK is validated, repository branch and workflow files are consistent, the existing solution state is documented, and any blocking prerequisite issue is either fixed or recorded before scaffold work begins.

## Research Findings

### Projects Affected
- `ContosoUniversity/ContosoUniversity.csproj` — existing .NET Framework 4.8 ASP.NET Framework web project used as the baseline and proxy target for the side-by-side migration.

### Repository and Workflow State
- Git branch is `upgrade-dotnet-10`, matching the configured working branch.
- Source branch remains `main`; commit strategy is `Single Commit at End`.
- Workflow artifacts exist under `.github/upgrades/scenarios/dotnet-version-upgrade/` and are consistent after plan approval.

### Toolchain Validation
- `validate_dotnet_sdk_installation(net10.0)` succeeded.
- Installed SDK list includes `10.0.301`.
- No `global.json` was found, so no SDK pin needed adjustment.
- Visual Studio MSBuild was located at `C:\Program Files\Microsoft Visual Studio\18\Enterprise\MSBuild\Current\Bin\MSBuild.exe`.

### Current Project Shape
- `ContosoUniversity.csproj` is old-style MSBuild XML with `ToolsVersion="15.0"` and `TargetFrameworkVersion` `v4.8`.
- Project type GUIDs include ASP.NET Web Application and C# project GUIDs.
- The project uses `packages.config` with 45 package entries.
- System.Web/MVC startup files in scope for later migration include `Global.asax.cs`, `App_Start/BundleConfig.cs`, `App_Start/FilterConfig.cs`, and `App_Start/RouteConfig.cs`.
- The old web project is intentionally not converted to SDK-style because the selected approach is side-by-side migration.

### Package and Dependency Baseline
- Packages are currently defined in `ContosoUniversity/packages.config` and referenced from the legacy project file.
- Assessment/package dependency data identifies 2 incompatible packages, 24 recommended package upgrades, 1 vulnerable package, and binding redirect conflicts to handle in later tasks.

### Build Baseline
- Existing solution build command used Visual Studio MSBuild with `/restore /t:Build /p:Configuration=Debug /p:Platform="Any CPU" /v:minimal`.
- Baseline build succeeded and produced `ContosoUniversity/bin/ContosoUniversity.dll`.

### Decomposition Decision
- This prerequisite task is atomic: it validates environment, branch, workflow state, project shape, and baseline build without code migration changes.
