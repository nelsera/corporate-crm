using FluentValidation;

namespace CorporateCrm.Application.Customers.CreateCustomer;

public class CreateCustomerCommandValidator 
    : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(customer => customer.CustomerType)
            .Must(customerType => customerType == "Individual" || customerType == "Company")
            .WithMessage("CustomerType must be Individual or Company.");

        RuleFor(customer => customer.Name).NotEmpty();
        RuleFor(customer => customer.CpfCnpj).NotEmpty();
        RuleFor(customer => customer.Email).NotEmpty().EmailAddress();
        RuleFor(customer => customer.BirthOrFoundationDate).NotEmpty();

        When(customerType => customerType.CustomerType == "Individual", () =>
        {
            RuleFor(customerType => customerType.BirthOrFoundationDate)
                .Must(BeAtLeast18YearsOld)
                .WithMessage("Individual must be at least 18 years old.");
        });

        When(customer => customer.CustomerType == "Company", () =>
        {
            RuleFor(customer => customer)
                .Must(customer => customer.IsStateRegistrationExempt || 
                           !string.IsNullOrWhiteSpace(customer.StateRegistration))
                .WithMessage("Company must inform State Registration or mark as exempt.");
        });
    }

    private bool BeAtLeast18YearsOld(DateTime date)
    {
        return date <= DateTime.UtcNow.AddYears(-18);
    }
}