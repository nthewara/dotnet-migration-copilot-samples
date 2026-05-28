# Modernization Assessment Summary

**Target Azure Services**: Any

## Overall Statistics

**Total Applications**: 1

**Name: ContosoUniversity**
- Mandatory: 2 issues
- Potential: 2 issues
- Optional: 2 issues

> **Severity Levels Explained:**
> - **Mandatory**: The issue has to be resolved for the migration to be successful.
> - **Potential**: This issue may be blocking in some situations but not in others. These issues should be reviewed to determine whether a change is required or not.
> - **Optional**: The issue discovered is real issue fixing which could improve the app after migration, however it is not blocking.

## Applications Profile

### Name: ContosoUniversity
- **Frameworks**: .NETFramework,Version=v4.8
- **Languages**: C#
- **Build Tools**: MSBuild

**Key Findings**:
- **Mandatory Issues (13 locations)**:
  - <!--ruleid=Identity.0002-->Windows authentication detected (1 location found)
  - <!--ruleid=Queue.0003-->MSMQ usage is detected (12 locations found)
- **Potential Issues (9 locations)**:
  - <!--ruleid=Database.0002-->SQL database connection detected (1 location found)
  - <!--ruleid=Local.0003-->Local or network IO operations detected (8 locations found)
- **Optional Issues (3 locations)**:
  - <!--ruleid=Scale.0001-->Static content detected (1 location found)
  - <!--ruleid=Security.0002-->Connection strings without configuration builders detected (2 locations found)

## Next Steps

For comprehensive migration guidance and best practices, visit:
- [GitHub Copilot modernization](https://aka.ms/ghcp-appmod)

Have questions or suggestions? [Share your feedback](https://aka.ms/ghcp-appmod/feedback)
