namespace CorporateCrm.Application.Common.Abstractions;

public interface IEventStore
{
    Task AppendAsync(
        Guid aggregateId,
        string aggregateType,
        string eventType,
        int eventVersion,
        object eventData,
        object metadata,
        DateTimeOffset occurredAt,
        CancellationToken ct);
}