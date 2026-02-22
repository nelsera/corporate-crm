using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CorporateCrm.Infrastructure.Persistence;
using CorporateCrm.Infrastructure.EventStore;

namespace CorporateCrm.Infrastructure.Tests.Persistence;

public class EfCoreEventStoreTests
{
    [Fact]
    public async Task Should_append_event_to_database()
    {
        // Arrange
        await using var conn = new SqliteConnection("DataSource=:memory:");
        await conn.OpenAsync();

        var options = new DbContextOptionsBuilder<CrmDbContext>()
            .UseSqlite(conn)
            .Options;

        await using var db = new CrmDbContext(options);
        await db.Database.EnsureCreatedAsync();

        var store = new EfCoreEventStore(db);

        var aggregateId = Guid.NewGuid();
        var evt = new { Type = "CustomerCreated", Name = "Nelson" };

        // Act
        await store.AppendAsync(
            streamId: $"Customer-{aggregateId}",
            @event: evt,
            ct: CancellationToken.None
        );

        // Assert
        var saved = await db.Set<EventStoreEvent>().ToListAsync();
        saved.Should().HaveCount(1);
        saved[0].AggregateId.Should().Be(aggregateId);
        saved[0].EventType.Should().NotBeNullOrWhiteSpace();
        saved[0].EventData.Should().NotBeNull();
        saved[0].OccurredAt.Should().NotBe(default);
    }
}