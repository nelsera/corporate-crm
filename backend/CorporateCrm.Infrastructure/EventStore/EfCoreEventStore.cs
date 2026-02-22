using System.Text.Json;
using CorporateCrm.Application.Common.Abstractions;
using CorporateCrm.Infrastructure.Persistence;

namespace CorporateCrm.Infrastructure.EventStore;

public sealed class EfCoreEventStore : IEventStore
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly CrmDbContext _db;

    public EfCoreEventStore(CrmDbContext db) => _db = db;

    public async Task AppendAsync(
        Guid aggregateId,
        string aggregateType,
        string eventType,
        int eventVersion,
        object eventData,
        object metadata,
        DateTimeOffset occurredAt,
        CancellationToken ct)
    {
        var entity = new EventStoreEventEntity
        {
            Id = Guid.NewGuid(),
            AggregateId = aggregateId,
            AggregateType = aggregateType,
            EventType = eventType,
            EventVersion = eventVersion,
            EventData = JsonSerializer.Serialize(eventData, JsonOptions),
            Metadata = JsonSerializer.Serialize(metadata, JsonOptions),
            OccurredAt = occurredAt
        };

        _db.EventStoreEvents.Add(entity);

        await _db.SaveChangesAsync(ct);
    }
}