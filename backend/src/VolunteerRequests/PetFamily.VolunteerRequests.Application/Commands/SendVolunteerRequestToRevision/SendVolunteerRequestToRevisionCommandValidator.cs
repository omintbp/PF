using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.VolunteerRequests.Domain.ValueObjects;

namespace PetFamily.VolunteerRequests.Application.Commands.SendVolunteerRequestToRevision;

public class SendVolunteerRequestToRevisionCommandValidator : AbstractValidator<SendVolunteerRequestToRevisionCommand>
{
    public SendVolunteerRequestToRevisionCommandValidator()
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