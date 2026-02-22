using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Xunit;
using CorporateCrm.Application.Customers.CreateCustomer;
using CorporateCrm.Application.Common.Abstractions;
using FluentValidation;

namespace CorporateCrm.Application.Tests.Customers;

public class CreateCustomerHandlerTests
{
    [Fact]
    public async Task Should_append_event_when_valid()
    {
        // Arrange
        var eventStore = Substitute.For<IEventStore>();
        var validator = new CreateCustomerCommandValidator();

        var handler = new CreateCustomerHandler(eventStore, validator);

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

        // Act
        var result = await handler.Handle(cmd, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();

        await eventStore.Received(1)
            .AppendAsync(
                Arg.Any<string>(),
                Arg.Any<object>(),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Should_throw_validation_exception_when_invalid()
    {
        // Arrange
        var eventStore = Substitute.For<IEventStore>();
        var validator = new CreateCustomerCommandValidator();

        var handler = new CreateCustomerHandler(eventStore, validator);

        var cmd = new CreateCustomerCommand
        {
            CustomerType = "Individual",
            Name = "Nelson",
            CpfCnpj = "39940838859",
            BirthOrFoundationDate = null, // inválido
            PerformedBy = "nelson"
        };

        // Act
        Func<Task> act = async () =>
            await handler.Handle(cmd, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}