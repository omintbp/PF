using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());
        
        RuleFor(c => c.Password)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());
    }
}