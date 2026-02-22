using FluentValidation;

namespace CorporateCrm.Application.Customers.CreateCustomer;

public class CreateCustomerCommandValidator 
    : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(customer => customer.CustomerType)
            .Must(customerType => customerType == "Individual" || customerType == "Company")
            .WithMessage("O tipo de cliente deve ser Pessoa Física ou Empresarial.");

        RuleFor(customer => customer.Name).NotEmpty();
        RuleFor(customer => customer.CpfCnpj).NotEmpty();
        RuleFor(customer => customer.Email).NotEmpty().EmailAddress();
        RuleFor(customer => customer.BirthOrFoundationDate).NotEmpty();

        When(customerType => customerType.CustomerType == "Individual", () =>
        {
            RuleFor(customerType => customerType.BirthOrFoundationDate)
                .Must(BeAtLeast18YearsOld)
                .WithMessage("O indivíduo deve ter pelo menos 18 anos de idade.");
        });

        When(customer => customer.CustomerType == "Company", () =>
        {
            RuleFor(customer => customer)
                .Must(customer => customer.IsStateRegistrationExempt || 
                           !string.IsNullOrWhiteSpace(customer.StateRegistration))
                .WithMessage("A empresa deve informar o Registro Estadual ou marcar como isenta.");
        });
    }

    private bool BeAtLeast18YearsOld(DateTime date)
    {
        return date <= DateTime.UtcNow.AddYears(-18);
    }
}