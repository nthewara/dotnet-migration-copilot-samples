# ContosoUniversity .NET 10.0 Upgrade Tasks

## Overview

This document tracks the execution of the ContosoUniversity project upgrade from .NET Framework 4.8 to .NET 10.0. The single project will be converted to SDK-style and upgraded in one atomic operation, migrating from ASP.NET MVC to ASP.NET Core.

**Progress**: 0/3 tasks complete (0%) ![0%](https://progress-bar.xyz/0)

---

## Tasks

### [ ] TASK-001: Verify prerequisites
**References**: Plan §4 Phase 0

- [ ] (1) Verify .NET 10.0 SDK is installed and available
- [ ] (2) .NET 10.0 SDK meets minimum requirements (**Verify**)

---

### [ ] TASK-002: Atomic project conversion, framework upgrade, and package remediation
**References**: Plan §5 Steps 1-9, Plan §6 Project-by-Project Plans, Plan §7 Package Update Reference, Plan §8 Breaking Changes Catalog

- [ ] (1) Convert ContosoUniversity/ContosoUniversity.csproj from classic WAP format to SDK-style per Plan §5 Step 1
- [ ] (2) Project file converted to SDK-style format (**Verify**)
- [ ] (3) Update TargetFramework from net48 to net10.0 in ContosoUniversity.csproj
- [ ] (4) Target framework set to net10.0 (**Verify**)
- [ ] (5) Apply all package actions per Plan §7 Package Update Reference (upgrade 24 packages including Microsoft.Data.SqlClient security fix, replace Antlr with Antlr4, remove ASP.NET Framework packages, update Entity Framework Core packages to 10.0.8)
- [ ] (6) All package updates applied (**Verify**)
- [ ] (7) Restore all dependencies
- [ ] (8) All dependencies restored successfully (**Verify**)
- [ ] (9) Migrate application startup from Global.asax.cs to Program.cs and ASP.NET Core hosting per Plan §5 Step 3
- [ ] (10) Migrate routing from RouteCollection to endpoint routing per Plan §5 Step 3
- [ ] (11) Migrate global filters to ASP.NET Core filters/middleware per Plan §5 Step 3
- [ ] (12) Migrate MVC controllers, actions, and views from System.Web.Mvc to ASP.NET Core MVC per Plan §5 Step 4 and Plan §8 Breaking Changes Catalog
- [ ] (13) Migrate static assets and remove System.Web.Optimization bundling per Plan §5 Step 5
- [ ] (14) Migrate configuration from web.config to appsettings.json per Plan §5 Step 6
- [ ] (15) Update Entity Framework Core data access and DbContext registration per Plan §5 Step 7
- [ ] (16) Replace System.Messaging usage per Plan §5 Step 8 (implement selected MSMQ replacement strategy)
- [ ] (17) Build solution and fix all compilation errors using Plan §8 Breaking Changes Catalog as reference for API replacements
- [ ] (18) Solution builds with 0 errors (**Verify**)

---

### [ ] TASK-003: Final commit
**References**: Plan §12 Source Control Strategy

- [ ] (1) Commit all changes with message: "TASK-003: Complete ContosoUniversity upgrade to .NET 10.0"

---