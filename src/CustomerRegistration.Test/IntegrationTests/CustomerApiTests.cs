using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using CustomerRegistration.Application.DTOs;
using CustomerRegistration.Domain.Interfaces;
using CustomerRegistration.Domain.Entities;
using FluentAssertions;

namespace CustomerRegistration.Test.IntegrationTests;

public class CustomerApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<ICustomerRepository> _repositoryMock = new();

    public CustomerApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace the real repository with a mock
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICustomerRepository));
                if (descriptor != null) services.Remove(descriptor);
                services.AddScoped(_ => _repositoryMock.Object);
            });
        });
    }

    [Fact]
    public async Task CreateCustomer_WithValidData_ReturnsCreated()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new CreateCustomerRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "+1234567890",
            SignatureBase64 = "data:image/png;base64,iVBOR..."
        };

        _repositoryMock.Setup(x => x.AddAsync(It.IsAny<Customer>()))
            .ReturnsAsync(new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                SignatureBase64 = request.SignatureBase64,
                DateCreated = DateTime.UtcNow
            });

        // Act
        var response = await client.PostAsJsonAsync("/api/customers", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var customer = await response.Content.ReadFromJsonAsync<CustomerDto>();
        customer.Should().NotBeNull();
        customer!.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task GetCustomer_WhenNotExists_ReturnsNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();
        var customerId = Guid.NewGuid();
        _repositoryMock.Setup(x => x.GetByIdAsync(customerId))
            .ReturnsAsync((Customer?)null);

        // Act
        var response = await client.GetAsync($"/api/customers/{customerId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
