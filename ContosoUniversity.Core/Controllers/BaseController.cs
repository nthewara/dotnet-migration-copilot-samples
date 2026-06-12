using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Controllers;

public abstract class BaseController : Controller
{
    protected readonly SchoolContext db;
    protected readonly INotificationService notificationService;

    protected BaseController(SchoolContext context, INotificationService notificationService)
    {
        db = context;
        this.notificationService = notificationService;
    }

    protected void SendEntityNotification(string entityType, string entityId, EntityOperation operation)
    {
        SendEntityNotification(entityType, entityId, null, operation);
    }

    protected void SendEntityNotification(string entityType, string entityId, string entityDisplayName, EntityOperation operation)
    {
        try
        {
            var userName = User?.Identity?.Name ?? "System";
            notificationService.SendNotification(entityType, entityId, entityDisplayName, operation, userName);
        }
        catch (Exception ex)
        {
            // Log the error but don't break the main operation.
            System.Diagnostics.Debug.WriteLine($"Failed to send notification: {ex.Message}");
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            notificationService.Dispose();
        }

        base.Dispose(disposing);
    }
}
