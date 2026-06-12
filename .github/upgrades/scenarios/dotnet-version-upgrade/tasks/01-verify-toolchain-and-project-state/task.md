# 01-verify-toolchain-and-project-state: Verify upgrade prerequisites

Verify the repository, solution, installed .NET SDK support for `net10.0`, and the current state of `ContosoUniversity.csproj` before making migration changes. This task should confirm the source branch and working branch are correct, check `global.json` compatibility if present, confirm the legacy web project remains buildable before migration, and inventory the project shape needed by the side-by-side scaffold.

The assessment found an old-style .NET Framework web application with legacy `System.Web`/ASP.NET Framework features, packages.config-style migration risks, and binding redirect conflicts. This prerequisite task captures the starting point so later scaffold and migration work can be validated against a known baseline.

**Done when**: The `net10.0` SDK is validated, repository branch and workflow files are consistent, the existing solution state is documented, and any blocking prerequisite issue is either fixed or recorded before scaffold work begins.
