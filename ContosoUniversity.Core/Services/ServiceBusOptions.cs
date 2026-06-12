namespace ContosoUniversity.Services;

public sealed class ServiceBusOptions
{
    public string FullyQualifiedNamespace { get; set; }
    public string QueueName { get; set; } = "contoso-university-notifications";
}
