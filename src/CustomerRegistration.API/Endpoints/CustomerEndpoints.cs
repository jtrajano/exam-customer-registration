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
        [FromServices] IValidator<CreateCustomerRequest> validator,
        [FromServices] ILogger<Program> logger)
    {
        logger.LogInformation("Creating new customer with email: {Email}", request.Email);
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation failed for customer creation: {Errors}", validationResult.Errors);
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var customer = await customerService.CreateCustomerAsync(request);
        logger.LogInformation("Successfully created customer with ID: {Id}", customer.Id);
        return Results.Created($"/api/customers/{customer.Id}", customer);
    }

    private static async Task<IResult> HandleGetCustomerById(
        [FromRoute] Guid id,
        [FromServices] ICustomerService customerService,
        [FromServices] ILogger<Program> logger)
    {
        logger.LogInformation("Fetching customer with ID: {Id}", id);
        var customer = await customerService.GetCustomerByIdAsync(id);
        
        if (customer is null)
        {
            logger.LogWarning("Customer with ID: {Id} not found", id);
            return Results.NotFound();
        }

        return Results.Ok(customer);
    }

    private static async Task<IResult> HandleGetAllCustomers(
        [FromServices] ICustomerService customerService,
        [FromServices] ILogger<Program> logger)
    {
        logger.LogInformation("Fetching all customers");
        var customers = await customerService.GetAllCustomersAsync();
        return Results.Ok(customers);
    }
}