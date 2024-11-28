using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerRequests.Application.Commands.ApproveVolunteerRequest;

public class ApproveVolunteerRequestCommandValidator : AbstractValidator<ApproveVolunteerRequestCommand>
{
    public ApproveVolunteerRequestCommandValidator()
    {
        RuleFor(r => r.VolunteerRequestId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(r => r.AdminId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());
    }
}