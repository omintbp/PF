using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(command => command.AccessToken)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());
        
        RuleFor(command => command.RefreshToken)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());
    }
}