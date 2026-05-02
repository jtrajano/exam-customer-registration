using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerRegistration.Domain.Entities;

namespace CustomerRegistration.Domain.Interfaces;

public interface ICustomerRepository
{
    Task<Customer> AddAsync(Customer customer);
    Task<Customer?> GetByIdAsync(Guid id);
    Task<IEnumerable<Customer>> GetAllAsync();
}
