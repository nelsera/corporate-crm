using CorporateCrm.Application.Common.Abstractions;
using CorporateCrm.Application.Common.Exceptions;
using CorporateCrm.Application.Customers.CreateCustomer;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CorporateCrm.Api.Controllers;

[ApiController]
[Route("customers")]
public sealed class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICustomerReadModelRepository _readModel;

    public CustomersController(IMediator mediator, ICustomerReadModelRepository readModel)
    {
        _mediator = mediator;
        _readModel = readModel;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command, CancellationToken ct)
    {
        var id = await _mediator.Send(command, ct);
 
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var customer = await _readModel.GetByIdAsync(id, ct);
 
        return customer is null ? NotFound() : Ok(customer);
    }
}