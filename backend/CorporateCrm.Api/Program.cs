using CorporateCrm.Application.Common.Abstractions;
using CorporateCrm.Application.Common.Exceptions;
using CorporateCrm.Application.Customers.CreateCustomer;
using CorporateCrm.Infrastructure.EventStore;
using CorporateCrm.Infrastructure.Persistence;
using CorporateCrm.Infrastructure.ReadModel;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<CrmDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// MediatR & Validators
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly));
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerCommandValidator>();

// Infrastructure implementations
builder.Services.AddScoped<IEventStore, EfCoreEventStore>();
builder.Services.AddScoped<ICustomerReadModelRepository, EfCoreCustomerReadModelRepository>();

var app = builder.Build();

// Global error handling
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;

        if (exception is ValidationException ve)
        {
            context.Response.StatusCode = 400;

            await context.Response.WriteAsJsonAsync(new
            {
                message = "Validation failed",
                errors = ve.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            });

            return;
        }

        if (exception is ConflictException ce)
        {
            context.Response.StatusCode = 409;

            await context.Response.WriteAsJsonAsync(new { message = ce.Message });

            return;
        }

        context.Response.StatusCode = 500;

        await context.Response.WriteAsJsonAsync(new { message = "Unexpected error" });
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();