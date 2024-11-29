using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Commands.CreateDiscussion;

public class CreateDiscussionCommandValidator : AbstractValidator<CreateDiscussionCommand>
{
    public CreateDiscussionCommandValidator()
    {
        RuleFor(c => c.RelationId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid("RelationId"));

        RuleFor(c => c.Users)
            .Must(u => u.Count() == 2)
            .WithError(Errors.General.ValueIsInvalid("Users"));
    }
}