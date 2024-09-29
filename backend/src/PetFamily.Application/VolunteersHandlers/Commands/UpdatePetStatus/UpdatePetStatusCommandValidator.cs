using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.PetManagement;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersHandlers.Commands.UpdatePetStatus;

public class UpdatePetStatusCommandValidator : AbstractValidator<UpdatePetStatusCommand>
{
    public UpdatePetStatusCommandValidator()
    {
        RuleFor(c => c.NewStatus).Must(status =>
                status is HelpStatus.NeedsHelp or HelpStatus.LookingForHome)
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsInvalid());
        
        RuleFor(c => c.PetId).NotEmpty().WithError(Errors.General.ValueIsInvalid());
    }
}