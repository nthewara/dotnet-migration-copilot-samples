# 03.09-views-and-static-assets: Migrate Razor views, layout, and static assets

# 03.09-views-and-static-assets: Migrate Razor views, layout, and static assets

## Objective
Migrate MVC views, layouts, partials, static assets, and bundling references from the Framework project into `ContosoUniversity.Core`. Scope includes moving `Content/` and `Scripts/` assets to `wwwroot`, replacing `System.Web.Optimization` usage such as `@Scripts.Render` and `@Styles.Render`, and adding Core MVC view infrastructure such as `_ViewImports.cshtml` where needed.

## Research Context
`BundleConfig.cs` defines bundles for jQuery, jQuery validation, Modernizr, Bootstrap/respond, and CSS. The approved options require replacing System.Web.Optimization with Core-compatible static references or equivalent. The Core project already uses `UseStaticFiles()`.

## Execution Notes
Load the Razor/views migration satellite before editing. Preserve layout and navigation behavior where possible. Do not delete the old Framework project or its assets; copy/adapt into the Core project.

**Done when**: Core views compile/render for migrated controllers, static assets are available from `wwwroot`, no Core views depend on `@Scripts.Render` or `@Styles.Render`, and the Core project builds with zero errors and warnings.

## Research Findings

### Projects Affected
- `ContosoUniversity.Core/ContosoUniversity.Core.csproj` — receives migrated view infrastructure, layout updates, and `wwwroot` static assets.
- `ContosoUniversity/ContosoUniversity.csproj` — remains present and unchanged; legacy Content/Scripts folders are copied from, not deleted.

### Asset and Bundle Inventory
- `BundleConfig.cs` configured jQuery, jQuery validation, Modernizr, Bootstrap/respond, and CSS bundles.
- Core views no longer had `@Scripts.Render` or `@Styles.Render` after earlier controller subtasks, but several validation sections referenced non-existent `~/lib/...` assets.
- Legacy public assets available for migration: `Content/Site.css`, `Content/notifications.css`, and JavaScript files under `Scripts/` including jQuery, Bootstrap, validation, Modernizr, respond, and notifications scripts.
- The legacy project did not include a Bootstrap CSS file in `Content/`, so the Core layout was adjusted to avoid referencing a missing `bootstrap.min.css`.

### Migration Decisions
- Copied legacy `Content/` CSS into `ContosoUniversity.Core/wwwroot/css/`.
- Copied legacy `Scripts/` JavaScript into `ContosoUniversity.Core/wwwroot/js/`.
- Kept teaching material uploads under `wwwroot/Uploads/TeachingMaterials` from the Courses migration.
- Added `Views/_ViewStart.cshtml` so migrated views consistently use `Views/Shared/_Layout.cshtml`.
- Updated `_Layout.cshtml` to reference `~/css/Site.css`, `~/js/jquery-3.4.1.min.js`, and `~/js/bootstrap.min.js`.
- Updated validation script sections to reference `~/js/jquery.validate.min.js` and `~/js/jquery.validate.unobtrusive.min.js`.

### Validation
- Searches found no remaining `@Scripts.Render`, `@Styles.Render`, `~/lib`, `~/Content`, `~/Scripts`, or `System.Web` references in Core `.cshtml` views.
- Searches found no remaining `Server.MapPath`, `HostingEnvironment.MapPath`, `VirtualPathProvider`, `System.Web.Optimization`, or `BundleTable` references in the Core project.
- Runtime checks verified `/` renders with layout asset references, `/css/Site.css` is served, and `/js/jquery.validate.min.js` is served.
- `dotnet build ContosoUniversity.Core.csproj` succeeded with 0 errors and 0 warnings.
- Full solution MSBuild succeeded with 0 errors and 0 warnings.

### Decomposition Decision
- This subtask was executed atomically because it only normalizes view infrastructure and static asset references after controller-specific views were already copied.
