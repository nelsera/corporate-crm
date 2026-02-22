using System.Text.Json;
using FluentValidation;
using MediatR;
using CorporateCrm.Application.Common.Abstractions;
using CorporateCrm.Application.Common.Exceptions;
using CorporateCrm.Domain.Customers.Events;

namespace CorporateCrm.Application.Customers.CreateCustomer;

public sealed class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly IValidator<CreateCustomerCommand> _validator;
    private readonly ICustomerReadModelRepository _readModel;
    private readonly IEventStore _eventStore;

    public CreateCustomerHandler(
        IValidator<CreateCustomerCommand> validator,
        ICustomerReadModelRepository readModel,
        IEventStore eventStore)
    {
        _validator = validator;

        _readModel = readModel;

        _eventStore = eventStore;
    }

    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken ct)
    {
        // Validation
        var validation = await _validator.ValidateAsync(request, ct);

        if (!validation.IsValid)
        {
            throw new ValidationException(validation.Errors);
        }

        // Uniqueness (CPF/CNPJ OR Email)
        var exists = await _readModel.ExistsByCpfCnpjOrEmailAsync(request.CpfCnpj, request.Email, ct);

        if (exists)
        {
            throw new ConflictException("O cliente já existe para o CPF/CNPJ ou e-mail fornecido.");
        }

        // Create event
        var customerId = Guid.NewGuid();

        var evt = new CustomerCreated(
            CustomerId: customerId,
            CustomerType: request.CustomerType,
            Name: request.Name,
            CpfCnpj: request.CpfCnpj,
            Email: request.Email,
            Phone: request.Phone,
            BirthOrFoundationDate: request.BirthOrFoundationDate,
            StateRegistration: request.StateRegistration,
            IsStateRegistrationExempt: request.IsStateRegistrationExempt,
            PostalCode: request.PostalCode,
            Street: request.Street,
            Number: request.Number,
            Neighborhood: request.Neighborhood,
            City: request.City,
            State: request.State
        );

        var occurredAt = DateTimeOffset.UtcNow;

        var metadata = new
        {
            performedBy = request.PerformedBy,
            occurredAt = occurredAt,
            correlationId = Guid.NewGuid().ToString()
        };

        // Append to Event Store (immutable log)
        await _eventStore.AppendAsync(
            aggregateId: customerId,
            aggregateType: "Customer",
            eventType: "CustomerCreated",
            eventVersion: 1,
            eventData: evt,
            metadata: metadata,
            occurredAt: occurredAt,
            ct: ct
        );

        // Project to Read Model
        await _readModel.UpsertFromCreatedAsync(
            customerId,
            new CreateCustomerReadModelDto(
                request.CustomerType,
                request.Name,
                request.CpfCnpj,
                request.Email,
                request.Phone,
                request.BirthOrFoundationDate,
                request.StateRegistration,
                request.IsStateRegistrationExempt,
                request.PostalCode,
                request.Street,
                request.Number,
                request.Neighborhood,
                request.City,
                request.State
            ),
            ct
        );

        return customerId;
    }
}
