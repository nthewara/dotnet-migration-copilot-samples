namespace ContosoUniversity.Services;

public sealed class MessageQueueOptions
{
    public string QueuePath { get; set; } = @".\Private$\ContosoUniversityNotifications";
}
