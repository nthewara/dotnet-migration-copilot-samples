## Detected Hints

### hint: web-controller-migration-units
- **Status**: active
- **Priority**: MUST
- **Evidence**: `ContosoUniversity/Controllers` contains 6 concrete MVC controllers plus shared `BaseController`; side-by-side migration guidance requires one controller per subtask when >5 controllers exist.
- **Detected**: During task `03-migrate-contosouniversity-web` research.

### hint: system-messaging-replacement
- **Status**: active
- **Priority**: MUST
- **Evidence**: `ContosoUniversity/Services/NotificationService.cs` uses `MessageQueue`; assessment includes MSMQ/message queuing issues.
- **Detected**: During task `03-migrate-contosouniversity-web` research.

### hint: web-bundling-and-static-assets
- **Status**: active
- **Priority**: SHOULD
- **Evidence**: `App_Start/BundleConfig.cs` configures System.Web.Optimization bundles; views use legacy Content/Scripts structure.
- **Detected**: During task `03-migrate-contosouniversity-web` research.

## Breakdown Decisions

### task: 03-migrate-contosouniversity-web
- Broken into 10 subtasks based on hints: `web-controller-migration-units`, `system-messaging-replacement`, and `web-bundling-and-static-assets`.
