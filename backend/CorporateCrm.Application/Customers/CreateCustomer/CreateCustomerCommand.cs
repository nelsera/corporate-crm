using MediatR;

namespace CorporateCrm.Application.Customers.CreateCustomer;

public record CreateCustomerCommand(
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
    string PerformedBy
) : IRequest<Guid>;