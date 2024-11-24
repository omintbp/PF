using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.VolunteerRequests.Domain.ValueObjects;

namespace PetFamily.VolunteerRequests.Application.Commands.RejectVolunteerRequest;

public class RejectVolunteerRequestCommandValidator : AbstractValidator<RejectVolunteerRequestCommand>
{
    public RejectVolunteerRequestCommandValidator()
    {
        RuleFor(r => r.VolunteerRequestId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(r => r.AdminId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(r => r.RejectionComment)
            .MustBeValueObject(RejectionComment.Create);
    }
}