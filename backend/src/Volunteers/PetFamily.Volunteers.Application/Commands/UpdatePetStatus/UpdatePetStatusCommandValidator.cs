using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Commands.UpdatePetStatus;

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