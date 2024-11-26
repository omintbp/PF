using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.VolunteerRequests.Application.Commands.UpdateVolunteerRequest;

public class UpdateVolunteerRequestCommandValidator : AbstractValidator<UpdateVolunteerRequestCommand>
{
    public UpdateVolunteerRequestCommandValidator()
    {
        RuleFor(r => r.UserId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(r => r.Experience)
            .MustBeValueObject(Experience.Create);

        RuleForEach(r => r.Requisites)
            .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
    }
}