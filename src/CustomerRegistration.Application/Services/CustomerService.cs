using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerRegistration.Application.DTOs;
using CustomerRegistration.Application.Interfaces;
using CustomerRegistration.Domain.Entities;
using CustomerRegistration.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CustomerRegistration.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerRequest request)
    {
        _logger.LogInformation("Creating customer with email: {Email}", request.Email);
        
        // Basic validation could be here or via FluentValidation
        if (string.IsNullOrWhiteSpace(request.Email)) throw new ArgumentException("Email is required");

        var customer = new Customer
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            SignatureBase64 = request.SignatureBase64,
            DateCreated = DateTime.UtcNow
        };

        var created = await _customerRepository.AddAsync(customer);
        _logger.LogInformation("Customer created with ID: {Id}", created.Id);

        return MapToDto(created);
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting customer by ID: {Id}", id);
        var customer = await _customerRepository.GetByIdAsync(id);
        return customer != null ? MapToDto(customer) : null;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        _logger.LogInformation("Getting all customers");
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(MapToDto);
    }

    private static CustomerDto MapToDto(Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            SignatureBase64 = customer.SignatureBase64,
            DateCreated = customer.DateCreated
        };
    }
}
