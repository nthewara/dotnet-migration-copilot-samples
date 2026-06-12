## Files Modified
- `.github/upgrades/scenarios/dotnet-version-upgrade/scenario-instructions.md`
- `.github/upgrades/scenarios/dotnet-version-upgrade/scenario.json`
- `.github/upgrades/scenarios/dotnet-version-upgrade/tasks.md`
- `.github/upgrades/scenarios/dotnet-version-upgrade/tasks/02-scaffold-contosouniversity-core/task.md`
- `.github/upgrades/scenarios/dotnet-version-upgrade/tasks/02-scaffold-contosouniversity-core/progress-details.md`
- `ContosoUniversity/ContosoUniversity.csproj`
- `ContosoUniversity/ContosoUniversity.sln`
- `ContosoUniversity.Core/ContosoUniversity.Core.csproj`
- `ContosoUniversity.Core/Program.cs`
- `ContosoUniversity.Core/appsettings.json`
- `ContosoUniversity.Core/appsettings.Development.json`
- `ContosoUniversity.Core/Properties/launchSettings.json`
- `ContosoUniversity.Core/wwwroot/.gitkeep`

## Build Result
- Errors: 0
- Warnings: 0
- Projects built:
  - `ContosoUniversity.Core/ContosoUniversity.Core.csproj` with `dotnet build`
  - Full solution `ContosoUniversity/ContosoUniversity.sln` with Visual Studio MSBuild
- Output verified: `ContosoUniversity.Core/bin/Debug/net10.0/ContosoUniversity.Core.dll`

## Test Result
- Tests run: 0
- Passed: 0
- Failed: 0
- Notes: No test project is present in the solution/workspace.

## Changes Summary
- Scaffolded a new side-by-side ASP.NET Core MVC project named `ContosoUniversity.Core` targeting `net10.0`.
- Added System.Web adapters and YARP reverse proxy packages.
- Configured the Core app to proxy unmatched requests to the old Framework app at `http://localhost:58801`.
- Added `/health`, controller route mapping, default MVC routing, and basic adapter services.
- Migrated initial global configuration values from `Web.config` into `appsettings.json`.
- Added the new Core project to the solution and linked the old project with `_MigrateToProjectGuid`.
- Captured baseline routes, controllers, filters, bundles, startup behavior, configuration, auth state, and pipeline findings in `task.md`.

## Smoke Validation
- Started the Core project with launch settings.
- Verified `https://localhost:7024/health` returns HTTP 200 with body `"ok"`.
- Proxy routing is configured through YARP catch-all forwarding to `http://localhost:58801`; live proxy response was not exercised because the old IIS Express app was not running during this smoke check.

## Issues Encountered
- The scaffold tool initially produced `Yarp.ReverseProxy` `2.0.1`; this was updated to the supported `2.3.0` version resolved for `net10.0`.
- The ASP.NET Core developer certificate is not trusted on this machine, which produced a runtime warning when starting the app over HTTPS. This does not block build or health endpoint validation with `Invoke-WebRequest -SkipCertificateCheck`.

## Done-When Verification
- New ASP.NET Core project added to solution: yes.
- Targets `net10.0`: yes.
- Builds successfully: yes, project and full solution builds succeeded with 0 errors and 0 warnings.
- Proxy routing configured to old Framework app: yes, `ProxyTo` is `http://localhost:58801` and catch-all YARP forwarding is registered.
- Minimal/stub response works: yes, `/health` returned HTTP 200.
- Existing Framework project remains present and buildable: yes, it remains in the solution and built successfully.
