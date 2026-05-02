using FluentValidation.TestHelper;
using CustomerRegistration.Application.Validators;
using CustomerRegistration.Application.DTOs;

namespace CustomerRegistration.Test.UnitTests;

public class CreateCustomerRequestValidatorTests
{
    private readonly CreateCustomerRequestValidator _validator;

    public CreateCustomerRequestValidatorTests()
    {
        _validator = new CreateCustomerRequestValidator();
    }

    [Fact]
    public void Should_Have_Error_When_FirstName_Is_Empty()
    {
        var model = new CreateCustomerRequest { FirstName = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new CreateCustomerRequest { Email = "invalid-email" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_PhoneNumber_Is_Invalid()
    {
        var model = new CreateCustomerRequest { PhoneNumber = "abc" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Request_Is_Valid()
    {
        var model = new CreateCustomerRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "+1234567890",
            SignatureBase64 = "data:image/png;base64,iVBOR..."
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
