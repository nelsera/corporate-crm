using FluentAssertions;
using Xunit;
using CorporateCrm.Application.Customers.CreateCustomer;

namespace CorporateCrm.Application.Tests.Customers;

public class CreateCustomerCommandValidatorTests
{
    private readonly CreateCustomerCommandValidator _validator = new();

    [Fact]
    public void Should_require_birth_date_for_individual()
    {
        var cmd = new CreateCustomerCommand
        {
            CustomerType = "Individual",
            Name = "Nelson",
            CpfCnpj = "39940838859",
            BirthOrFoundationDate = null,
            PerformedBy = "nelson"
        };

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "BirthOrFoundationDate");
    }

    [Fact]
    public void Should_reject_individual_under_18()
    {
        var cmd = new CreateCustomerCommand
        {
            CustomerType = "Individual",
            Name = "Nelson",
            CpfCnpj = "39940838859",
            BirthOrFoundationDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-10)),
            PerformedBy = "nelson"
        };

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("18"));
    }

    [Fact]
    public void Should_pass_for_valid_individual()
    {
        var cmd = new CreateCustomerCommand
        {
            CustomerType = "Individual",
            Name = "Nelson",
            CpfCnpj = "39940838859",
            BirthOrFoundationDate = DateOnly.Parse("1990-06-13"),
            Email = "nelson@email.com",
            Phone = "11999999999",
            PostalCode = "11065651",
            Street = "Rua X",
            City = "Santos",
            State = "SP",
            PerformedBy = "nelson"
        };

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeTrue();
    }
}