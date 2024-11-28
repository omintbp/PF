using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerRequests.Application.Commands.TakeVolunteerRequestToReview;

public class TakeVolunteerRequestToReviewCommandValidator : AbstractValidator<TakeVolunteerRequestToReviewCommand>
{
    public TakeVolunteerRequestToReviewCommandValidator()
    {
        RuleFor(r => r.AdminId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(r => r.VolunteerRequestId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());
    }
}