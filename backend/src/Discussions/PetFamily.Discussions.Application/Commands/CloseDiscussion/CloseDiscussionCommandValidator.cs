using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Commands.CloseDiscussion;

public class CloseDiscussionCommandValidator : AbstractValidator<CloseDiscussionCommand>
{
    public CloseDiscussionCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid("UserId"));

        RuleFor(c => c.DiscussionId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid("DiscussionId"));
    }
}