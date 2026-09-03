using DemoDomain.Models;
using FluentValidation;

namespace DemoDomain.Validators;

public sealed class UserValidator : AbstractValidator<User>
{
    public UserValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Please specify a last name");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Please specify a first name");
        RuleFor(x => x.Email).EmailAddress()
            .WithMessage("Please specify a valid email address ");
        RuleFor(x => x.DateOfBirth).LessThan(DateOnly.FromDateTime(timeProvider.GetLocalNow().Date))
            .When(x => x.DateOfBirth != null)
            .WithMessage("Please specify a valid date of birth");
    }
}