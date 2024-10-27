using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());
        
        RuleFor(c => c.Password)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());
        
        RuleFor(c => c.UserName)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());
    }
}