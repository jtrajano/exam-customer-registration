using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using CustomerRegistration.Application.Interfaces;
using CustomerRegistration.Application.Services;
using System.Reflection;

namespace CustomerRegistration.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register the Customer Service
        services.AddScoped<ICustomerService, CustomerService>();

        // Register all validators from this assembly
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
