using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Application.Commands.CreateVolunteerAccount;

public class CreateVolunteerAccountCommandValidator : AbstractValidator<CreateVolunteerAccountCommand>
{
    public CreateVolunteerAccountCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(c => c.Experience)
            .MustBeValueObject(Experience.Create);

        RuleForEach(c => c.Requisites)
            .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
    }
}