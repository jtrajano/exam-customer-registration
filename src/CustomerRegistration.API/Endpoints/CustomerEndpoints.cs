using Microsoft.AspNetCore.Mvc;
using CustomerRegistration.Application.DTOs;
using CustomerRegistration.Application.Interfaces;
using CustomerRegistration.Application.Validators;
using FluentValidation;

public static class CustomerEndpoints
{
    public static void AddCustomerEndpoints(this WebApplication app)
    {
        var customersApi = app.MapGroup("/api/customers");

        customersApi.MapPost("/", HandleCreateCustomer)
        .WithName("CreateCustomer");

        customersApi.MapGet("/{id:guid}", HandleGetCustomerById)
        .WithName("GetCustomerById");

        customersApi.MapGet("/", HandleGetAllCustomers)
        .WithName("GetAllCustomers");

    }

    private static async Task<IResult> HandleCreateCustomer(
        [FromBody] CreateCustomerRequest request,
        [FromServices] ICustomerService customerService, 
        [FromServices] IValidator<CreateCustomerRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var customer = await customerService.CreateCustomerAsync(request);
        return Results.Created($"/api/customers/{customer.Id}", customer);
    }

    private static async Task<IResult> HandleGetCustomerById(
        [FromRoute] Guid id,
        [FromServices] ICustomerService customerService)
    {
        var customer = await customerService.GetCustomerByIdAsync(id);
        return customer is not null ? Results.Ok(customer) : Results.NotFound();
    }

    private static async Task<IResult> HandleGetAllCustomers([FromServices] ICustomerService customerService)
    {
        var customers = await customerService.GetAllCustomersAsync();
        return Results.Ok(customers);
    }
}