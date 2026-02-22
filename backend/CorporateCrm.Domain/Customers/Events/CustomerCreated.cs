namespace CorporateCrm.Domain.Customers.Events;

public sealed record CustomerCreated(
    Guid CustomerId,
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