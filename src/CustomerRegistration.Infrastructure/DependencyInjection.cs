using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CustomerRegistration.Domain.Interfaces;
using CustomerRegistration.Infrastructure.Persistence;
using CustomerRegistration.Infrastructure.Repositories;

namespace CustomerRegistration.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=CustomerRegistration.db";

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<ICustomerRepository, CustomerRepository>();

        // Automatically create database on startup
        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
        }

        return services;
    }
}
