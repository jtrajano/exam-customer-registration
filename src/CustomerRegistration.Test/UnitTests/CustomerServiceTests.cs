using Moq;
using FluentAssertions;
using CustomerRegistration.Application.Services;
using CustomerRegistration.Application.DTOs;
using CustomerRegistration.Domain.Entities;
using CustomerRegistration.Domain.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;

namespace CustomerRegistration.Test.UnitTests;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _repositoryMock;
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        _repositoryMock = new Mock<ICustomerRepository>();
        _service = new CustomerService(_repositoryMock.Object, NullLogger<CustomerService>.Instance);
    }

    [Fact]
    public async Task CreateCustomerAsync_WithValidRequest_ShouldReturnCustomerDto()
    {
        // Arrange
        var request = new CreateCustomerRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "+1234567890",
            SignatureBase64 = "data:image/png;base64,iVBOR..."
        };

        var expectedCustomer = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            SignatureBase64 = request.SignatureBase64,
            DateCreated = DateTime.UtcNow
        };

        _repositoryMock.Setup(x => x.AddAsync(It.IsAny<Customer>()))
            .ReturnsAsync(expectedCustomer);

        // Act
        var result = await _service.CreateCustomerAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be(request.FirstName);
        result.LastName.Should().Be(request.LastName);
        result.Email.Should().Be(request.Email);
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_WhenExists_ShouldReturnDto()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer
        {
            Id = customerId,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com"
        };

        _repositoryMock.Setup(x => x.GetByIdAsync(customerId))
            .ReturnsAsync(customer);

        // Act
        var result = await _service.GetCustomerByIdAsync(customerId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(customerId);
        result.Email.Should().Be(customer.Email);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _repositoryMock.Setup(x => x.GetByIdAsync(customerId))
            .ReturnsAsync((Customer?)null);

        // Act
        var result = await _service.GetCustomerByIdAsync(customerId);

        // Assert
        result.Should().BeNull();
    }
}
