using Microsoft.EntityFrameworkCore;

namespace CorporateCrm.Infrastructure.Persistence;

public sealed class CrmDbContext : DbContext
{
    public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options) {}

    public DbSet<EventStoreEventEntity> EventStoreEvents => Set<EventStoreEventEntity>();
    public DbSet<CustomerReadModelEntity> CustomersReadModel => Set<CustomerReadModelEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventStoreEventEntity>(b =>
        {
            b.ToTable("event_store_events");
            b.HasKey(x => x.Id);
            b.Property(x => x.AggregateId).IsRequired();
            b.Property(x => x.AggregateType).HasMaxLength(100).IsRequired();
            b.Property(x => x.EventType).HasMaxLength(150).IsRequired();
            b.Property(x => x.EventVersion).IsRequired();
            b.Property(x => x.EventData).HasColumnType("jsonb").IsRequired();
            b.Property(x => x.Metadata).HasColumnType("jsonb").IsRequired();
            b.Property(x => x.OccurredAt).IsRequired();
            b.HasIndex(x => x.AggregateId).HasDatabaseName("idx_event_store_aggregate_id");
        });

        modelBuilder.Entity<CustomerReadModelEntity>(b =>
        {
            b.ToTable("customers_read_model");
            b.HasKey(x => x.Id);

            b.Property(x => x.CustomerType).HasMaxLength(20).IsRequired();
            b.Property(x => x.Name).HasMaxLength(200).IsRequired();
            b.Property(x => x.CpfCnpj).HasMaxLength(20).IsRequired();
            b.Property(x => x.Email).HasMaxLength(200).IsRequired();
            b.Property(x => x.Phone).HasMaxLength(50);

            b.Property(x => x.BirthOrFoundationDate).IsRequired();

            b.Property(x => x.StateRegistration).HasMaxLength(50);
            b.Property(x => x.IsStateRegistrationExempt).IsRequired();

            b.Property(x => x.PostalCode).HasMaxLength(20);
            b.Property(x => x.Street).HasMaxLength(200);
            b.Property(x => x.Number).HasMaxLength(20);
            b.Property(x => x.Neighborhood).HasMaxLength(200);
            b.Property(x => x.City).HasMaxLength(200);
            b.Property(x => x.State).HasMaxLength(50);

            b.Property(x => x.CreatedAt).IsRequired();
            b.Property(x => x.UpdatedAt).IsRequired();

            b.HasIndex(x => x.CpfCnpj).IsUnique().HasDatabaseName("ux_customers_cpf_cnpj");
            b.HasIndex(x => x.Email).IsUnique().HasDatabaseName("ux_customers_email");
        });
    }
}