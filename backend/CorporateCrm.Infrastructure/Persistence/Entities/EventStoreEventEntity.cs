namespace CorporateCrm.Infrastructure.Persistence;

public sealed class EventStoreEventEntity
{
    public Guid Id { get; set; }

    public Guid AggregateId { get; set; }

    public string AggregateType { get; set; } = default!;

    public string EventType { get; set; } = default!;

    public int EventVersion { get; set; }

    public string EventData { get; set; } = default!;

    public string Metadata { get; set; } = default!;

    public DateTimeOffset OccurredAt { get; set; }
}