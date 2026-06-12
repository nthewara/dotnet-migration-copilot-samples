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

    protected Task SendEntityNotificationAsync(string entityType, string entityId, EntityOperation operation)
    {
        return SendEntityNotificationAsync(entityType, entityId, null, operation);
    }

    protected async Task SendEntityNotificationAsync(string entityType, string entityId, string entityDisplayName, EntityOperation operation)
    {
        try
        {
            var userName = User?.Identity?.Name ?? "System";
            await notificationService.SendNotificationAsync(entityType, entityId, entityDisplayName, operation, userName);
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
