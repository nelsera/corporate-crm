namespace CorporateCrm.Infrastructure.Persistence;

public sealed class CustomerReadModelEntity
{
    public Guid Id { get; set; }

    public string CustomerType { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string CpfCnpj { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string? Phone { get; set; }

    public DateOnly BirthOrFoundationDate { get; set; }

    public string? StateRegistration { get; set; }

    public bool IsStateRegistrationExempt { get; set; }

    public string? PostalCode { get; set; }

    public string? Street { get; set; }

    public string? Number { get; set; }

    public string? Neighborhood { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}