using Microsoft.EntityFrameworkCore;

namespace CorporateCrm.Infrastructure.Persistence;

public sealed class CrmDbContext : DbContext
{
    public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options) {}

    public DbSet<EventStoreEventEntity> EventStoreEvents => Set<EventStoreEventEntity>();
    public DbSet<CustomerReadModelEntity> CustomersReadModel => Set<CustomerReadModelEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventStoreEventEntity>(builder =>
        {
            builder.ToTable("event_store_events");
            builder.HasKey(customer => customer.Id);
            builder.Property(customer => customer.AggregateId).IsRequired();
            builder.Property(customer => customer.AggregateType).HasMaxLength(100).IsRequired();
            builder.Property(customer => customer.EventType).HasMaxLength(150).IsRequired();
            builder.Property(customer => customer.EventVersion).IsRequired();
            builder.Property(customer => customer.EventData).HasColumnType("jsonb").IsRequired();
            builder.Property(customer => customer.Metadata).HasColumnType("jsonb").IsRequired();
            builder.Property(customer => customer.OccurredAt).IsRequired();
            builder.HasIndex(customer => customer.AggregateId).HasDatabaseName("idx_event_store_aggregate_id");
        });

        modelBuilder.Entity<CustomerReadModelEntity>(builder =>
        {
            builder.ToTable("customers_read_model");
            builder.HasKey(customer => customer.Id);

            builder.Property(customer => customer.CustomerType).HasMaxLength(20).IsRequired();
            builder.Property(customer => customer.Name).HasMaxLength(200).IsRequired();
            builder.Property(customer => customer.CpfCnpj).HasMaxLength(20).IsRequired();
            builder.Property(customer => customer.Email).HasMaxLength(200).IsRequired();
            builder.Property(customer => customer.Phone).HasMaxLength(50);

            builder.Property(customer => customer.BirthOrFoundationDate).IsRequired();

            builder.Property(customer => customer.StateRegistration).HasMaxLength(50);
            builder.Property(customer => customer.IsStateRegistrationExempt).IsRequired();

            builder.Property(customer => customer.PostalCode).HasMaxLength(20);
            builder.Property(customer => customer.Street).HasMaxLength(200);
            builder.Property(customer => customer.Number).HasMaxLength(20);
            builder.Property(customer => customer.Neighborhood).HasMaxLength(200);
            builder.Property(customer => customer.City).HasMaxLength(200);
            builder.Property(customer => customer.State).HasMaxLength(50);

            builder.Property(customer => customer.CreatedAt).IsRequired();
            builder.Property(customer => customer.UpdatedAt).IsRequired();

            builder.HasIndex(customer => customer.CpfCnpj).IsUnique().HasDatabaseName("ux_customers_cpf_cnpj");
            builder.HasIndex(customer => customer.Email).IsUnique().HasDatabaseName("ux_customers_email");
        });
    }
}