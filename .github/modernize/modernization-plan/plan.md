# Modernization Plan: ContosoUniversity Azure Migration

**Project**: ContosoUniversity

---

## Technical Framework

- **Language**: C# / .NET Framework 4.8
- **Framework**: ASP.NET MVC 5.2.9
- **Build Tool**: MSBuild / NuGet (packages.config)
- **Database**: SQL Server (LocalDB) via Entity Framework Core 3.1.32
- **Key Dependencies**: Entity Framework Core 3.1.32, System.Messaging (MSMQ), ASP.NET MVC 5, Newtonsoft.Json 13.0.3

---

## Overview

This migration modernizes the ContosoUniversity ASP.NET MVC 5 web application from .NET Framework 4.8 to .NET 10 and migrates all on-premises infrastructure dependencies to Azure managed services. The application currently uses a local SQL Server database, Windows MSMQ for notifications, and local file system storage for teaching material uploads. The new architecture will:

- Upgrade the application to .NET 10 with ASP.NET Core MVC to enable modern cloud deployment and containerization
- Replace local SQL Server with Azure SQL Database using Managed Identity for secure, passwordless authentication
- Replace Windows-only MSMQ with Azure Service Bus for cloud-native, cross-platform messaging
- Replace local file system storage (`~/Uploads/TeachingMaterials/`) with Azure Blob Storage for durable, scalable file management
- Ensure all dependencies are free of known CVEs before deployment
- Deploy the modernized application to Azure App Service with Managed Identity

The migration follows a phased approach: first upgrading the runtime, then migrating each Azure service dependency, followed by security hardening and final deployment.

---

## Migration Impact Summary

| Application          | Original Service       | New Azure Service          | Authentication     | Comments                                      |
|----------------------|------------------------|----------------------------|--------------------|-----------------------------------------------|
| ContosoUniversity    | SQL Server (LocalDB)   | Azure SQL Database         | Managed Identity   | EF Core migration, passwordless auth          |
| ContosoUniversity    | MSMQ                   | Azure Service Bus          | Managed Identity   | Notification queue, replace System.Messaging  |
| ContosoUniversity    | Local File System      | Azure Blob Storage         | Managed Identity   | Teaching material image uploads               |

---

## Migration Tasks

### Task 1 - Upgrade .NET to net10.0

Upgrade the ContosoUniversity project from .NET Framework 4.8 / ASP.NET MVC 5 to .NET 10 with
ASP.NET Core MVC. This includes converting the legacy project format, migrating configuration from
Web.config to appsettings.json, and updating the application pipeline. This upgrade is required
to enable cloud deployment and compatibility with modern Azure SDKs.

**Type**: upgrade

---

### Task 2 - Migrate SQL Server to Azure SQL Database

Replace the local SQL Server (LocalDB) database connection with Azure SQL Database using Managed
Identity for passwordless authentication. Update Entity Framework Core configuration and connection
strings to target Azure SQL Database.

**Type**: transform | **Skill**: migration-azure-sql-database

---

### Task 3 - Migrate MSMQ to Azure Service Bus

Replace the Windows MSMQ-based notification system (System.Messaging) with Azure Service Bus.
Migrate the NotificationService to use Azure Service Bus queues with Managed Identity
authentication, ensuring notifications for create/update/delete operations continue to work.

**Type**: transform | **Skill**: migration-azure-servicebus

---

### Task 4 - Migrate Local File Storage to Azure Blob Storage

Replace the local file system storage (~/Uploads/TeachingMaterials/) used for course teaching
material image uploads with Azure Blob Storage. Migrate all file upload, retrieval, and deletion
operations in CoursesController to use Azure Blob Storage with Managed Identity.

**Type**: transform | **Skill**: migration-azure-storage-blob

---

### Task 5 - Security and CVE Remediation

Scan all project dependencies for known CVEs and remediate any identified vulnerabilities to
ensure the application is secure before deployment.

**Type**: security | **Skill**: validate-cves-and-fix

---

### Task 6 - Deploy to Azure App Service

Deploy the modernized application to Azure App Service using Managed Identity for secure access
to all Azure resources (Azure SQL Database, Service Bus, Blob Storage).

**Type**: deployment | **Target**: Azure App Service

---
