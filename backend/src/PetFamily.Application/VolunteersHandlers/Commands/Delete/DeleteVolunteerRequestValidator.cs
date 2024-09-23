using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersHandlers.Commands.Delete;

public class DeleteVolunteerRequestValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerRequestValidator()
    {
        RuleFor(v => v.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());
    }
}