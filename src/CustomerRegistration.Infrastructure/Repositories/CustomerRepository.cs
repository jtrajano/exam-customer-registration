using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CustomerRegistration.Domain.Entities;
using CustomerRegistration.Domain.Interfaces;
using CustomerRegistration.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace CustomerRegistration.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CustomerRepository> _logger;

    public CustomerRepository(ApplicationDbContext context, ILogger<CustomerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Customer> AddAsync(Customer customer)
    {
        try
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully added customer to database with ID: {Id}", customer.Id);
            return customer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding customer to database");
            throw;
        }
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        _logger.LogDebug("Fetching customer {Id} from database", id);
        return await _context.Customers.FindAsync(id);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        _logger.LogDebug("Fetching all customers from database");
        return await _context.Customers.ToListAsync();
    }
}
