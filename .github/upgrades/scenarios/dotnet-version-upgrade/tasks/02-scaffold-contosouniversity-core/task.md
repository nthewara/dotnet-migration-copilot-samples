# 02-scaffold-contosouniversity-core: Scaffold ASP.NET Core side-by-side project

Create a new ASP.NET Core `net10.0` project alongside the existing `ContosoUniversity` Framework web project and configure it for side-by-side migration. The new project should use modern SDK-style project format, reference the appropriate ASP.NET Core framework packages, and include a reverse-proxy/YARP setup that lets the old Framework app remain live while routes are migrated incrementally.

This task is necessary because the selected Project Approach is Side-by-side and the assessment identified extensive ASP.NET Framework/System.Web usage. The old web project is excluded from SDK-style conversion and direct TFM replacement; instead, the new Core project becomes the migration target.

**Done when**: The new ASP.NET Core project is added to the solution, targets `net10.0`, builds successfully, has proxy routing configured to the old Framework application, and can serve a minimal/stub response without deleting or breaking the existing Framework project.
