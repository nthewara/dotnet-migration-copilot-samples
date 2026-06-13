# Migration Plan: Windows Active Directory to Microsoft Entra ID

Primary project: ContosoUniversity ASP.NET MVC 5 on .NET Framework 4.8.
Current AD usage is documented/planned, not implemented in code. Migrate authentication assumptions to Microsoft Entra ID using OWIN/OpenID Connect and Microsoft Graph group checks.

Packages: Microsoft.Identity.Web.OWIN 3.14.1, Microsoft.Identity.Client 4.77.0, Microsoft.Graph 5.93.0.

Tasks: update packages, Web.config, OWIN startup, Graph helper, BaseController user identity, FilterConfig authorization, CVE/completeness/consistency/build validation.
