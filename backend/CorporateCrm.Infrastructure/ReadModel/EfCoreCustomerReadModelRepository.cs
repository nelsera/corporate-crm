using CorporateCrm.Application.Common.Abstractions;
using CorporateCrm.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CorporateCrm.Infrastructure.ReadModel;

public sealed class EfCoreCustomerReadModelRepository : ICustomerReadModelRepository
{
    private readonly CrmDbContext _db;

    public EfCoreCustomerReadModelRepository(CrmDbContext db) => _db = db;

    public async Task<bool> ExistsByCpfCnpjOrEmailAsync(string cpfCnpj, string email, CancellationToken ct)
    {
        return await _db.CustomersReadModel.AnyAsync(
            customer => customer.CpfCnpj == cpfCnpj || customer.Email == email,
            ct
        );
    }

    public async Task UpsertFromCreatedAsync(Guid id, CreateCustomerReadModelDto dto, CancellationToken ct)
    {
        var now = DateTimeOffset.UtcNow;

        var entity = await _db.CustomersReadModel.FirstOrDefaultAsync(customer => customer.Id == id, ct);

        if (entity is null)
        {
            entity = new CustomerReadModelEntity
            {
                Id = id,
                CreatedAt = now
            };
            _db.CustomersReadModel.Add(entity);
        }

        entity.CustomerType = dto.CustomerType;
        entity.Name = dto.Name;
        entity.CpfCnpj = dto.CpfCnpj;
        entity.Email = dto.Email;
        entity.Phone = dto.Phone;
        entity.BirthOrFoundationDate = DateOnly.FromDateTime(dto.BirthOrFoundationDate);

        entity.StateRegistration = dto.StateRegistration;
        entity.IsStateRegistrationExempt = dto.IsStateRegistrationExempt;

        entity.PostalCode = dto.PostalCode;
        entity.Street = dto.Street;
        entity.Number = dto.Number;
        entity.Neighborhood = dto.Neighborhood;
        entity.City = dto.City;
        entity.State = dto.State;

        entity.UpdatedAt = now;

        await _db.SaveChangesAsync(ct);
    }

    public async Task<CustomerReadModelDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var entity = await _db.CustomersReadModel.AsNoTracking().FirstOrDefaultAsync(customer => customer.Id == id, ct);

        if (entity is null) {
            return null;
        }

        return new CustomerReadModelDto(
            Id: entity.Id,
            CustomerType: entity.CustomerType,
            Name: entity.Name,
            CpfCnpj: entity.CpfCnpj,
            Email: entity.Email,
            Phone: entity.Phone,
            BirthOrFoundationDate: entity.BirthOrFoundationDate.ToDateTime(TimeOnly.MinValue),
            StateRegistration: entity.StateRegistration,
            IsStateRegistrationExempt: entity.IsStateRegistrationExempt,
            PostalCode: entity.PostalCode,
            Street: entity.Street,
            Number: entity.Number,
            Neighborhood: entity.Neighborhood,
            City: entity.City,
            State: entity.State,
            CreatedAt: entity.CreatedAt,
            UpdatedAt: entity.UpdatedAt
        );
    }
}