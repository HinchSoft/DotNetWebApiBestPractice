using FluentValidation;

namespace DemoApplication.Handlers.AddUser;

internal sealed class AddUserValidator : AbstractValidator<AddUserCommand>
{
    public AddUserValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Please specify a last name");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Please specify a first name");
        RuleFor(x => x.Email).EmailAddress()
            .WithMessage("Please specify a valid email address ");
        RuleFor(x => x.DateOfBirth).LessThan(DateOnly.FromDateTime(timeProvider.GetLocalNow().Date))
            .When(x => x.DateOfBirth != null)
            .WithMessage("Please specify a valid date of birth");
    }
}
