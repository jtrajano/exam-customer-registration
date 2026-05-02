using FluentValidation;
using CustomerRegistration.Application.DTOs;

namespace CustomerRegistration.Application.Validators;

public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format");
        RuleFor(x => x.SignatureBase64).NotEmpty().WithMessage("Signature is required");
    }
}
