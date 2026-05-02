using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerRegistration.Application.DTOs;

namespace CustomerRegistration.Application.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerRequest request);
    Task<CustomerDto?> GetCustomerByIdAsync(Guid id);
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
}
