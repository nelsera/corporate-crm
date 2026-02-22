namespace CorporateCrm.Application.Common.Abstractions;

public interface ICustomerReadModelRepository
{
    Task<bool> ExistsByCpfCnpjOrEmailAsync(string cpfCnpj, string email, CancellationToken ct);

    Task UpsertFromCreatedAsync(Guid id, CreateCustomerReadModelDto dto, CancellationToken ct);

    Task<CustomerReadModelDto?> GetByIdAsync(Guid id, CancellationToken ct);
}

public sealed record CreateCustomerReadModelDto(
    string CustomerType,
    string Name,
    string CpfCnpj,
    string Email,
    string? Phone,
    DateTime BirthOrFoundationDate,
    string? StateRegistration,
    bool IsStateRegistrationExempt,
    string? PostalCode,
    string? Street,
    string? Number,
    string? Neighborhood,
    string? City,
    string? State
);

public sealed record CustomerReadModelDto(
    Guid Id,
    string CustomerType,
    string Name,
    string CpfCnpj,
    string Email,
    string? Phone,
    DateTime BirthOrFoundationDate,
    string? StateRegistration,
    bool IsStateRegistrationExempt,
    string? PostalCode,
    string? Street,
    string? Number,
    string? Neighborhood,
    string? City,
    string? State,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);