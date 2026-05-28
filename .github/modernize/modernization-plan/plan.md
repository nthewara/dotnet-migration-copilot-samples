# Modernization Plan: ContosoUniversity Azure Migration

**Project**: ContosoUniversity

---

## Technical Framework

- **Language**: C# / .NET Framework 4.8
- **Framework**: ASP.NET MVC 5.2.9
- **Build Tool**: MSBuild (packages.config / non-SDK style)
- **Database**: SQL Server (LocalDB) via Entity Framework Core 3.1
- **Key Dependencies**: Microsoft.EntityFrameworkCore 3.1.32, Microsoft.AspNet.Mvc 5.2.9, System.Messaging (MSMQ), Newtonsoft.Json 13.0.3

---

## Overview

This migration modernizes the ContosoUniversity ASP.NET MVC 5 application by replacing on-premises and local infrastructure with Azure managed services. The application currently uses SQL Server LocalDB for relational data, MSMQ for internal notification messaging, and the local file system for storing teaching material uploads. The new architecture will:

- Replace SQL Server LocalDB with Azure SQL Database using Managed Identity for secure, passwordless authentication
- Replace MSMQ (System.Messaging) notification queue with Azure Service Bus for reliable cloud-native messaging
- Replace local file system storage for teaching materials with Azure Blob Storage for scalable, durable file management

The migration preserves existing application functionality while establishing a cloud-ready foundation using Azure managed services and Managed Identity throughout.

---

## Migration Impact Summary

| Application          | Original Service          | New Azure Service        | Authentication   | Comments                                      |
|----------------------|---------------------------|--------------------------|------------------|-----------------------------------------------|
| ContosoUniversity    | SQL Server (LocalDB)      | Azure SQL Database       | Managed Identity | EF Core 3.1 with SqlServer provider           |
| ContosoUniversity    | MSMQ (System.Messaging)   | Azure Service Bus        | Managed Identity | NotificationService send/receive operations   |
| ContosoUniversity    | Local File System         | Azure Blob Storage       | Managed Identity | Teaching materials upload in CoursesController|

---

## Migration Tasks

### Task 1: Migrate SQL Server to Azure SQL Database

Migrate the SQL Server LocalDB database connection to Azure SQL Database using Managed Identity (passwordless) authentication. Update connection configuration and EF Core provider settings accordingly.

**Depends on**: None

### Task 2: Migrate MSMQ to Azure Service Bus

Migrate the `NotificationService` class from MSMQ (`System.Messaging.MessageQueue`) to Azure Service Bus. Replace queue creation, send, and receive operations with Azure Service Bus equivalents using Managed Identity authentication.

**Depends on**: None

### Task 3: Migrate Local File Storage to Azure Blob Storage

Migrate teaching material file upload and retrieval operations in `CoursesController` from local file system paths (`~/Uploads/TeachingMaterials/`) to Azure Blob Storage using Managed Identity authentication.

**Depends on**: None

---

## Security Compliance

Scan all project dependencies for known CVEs and remediate any identified vulnerabilities to ensure the application is secure.

**Requirements**: Upgrade vulnerable dependencies to the minimum patched version. If a CVE fix requires a major version upgrade, document the affected dependency, current version, upgraded version, and breaking change risk. Verify the project builds and tests pass after remediation.

**Depends on**: Task 1, Task 2, Task 3
