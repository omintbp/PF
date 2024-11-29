using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Commands.DeleteMessage;

public class DeleteMessageCommandValidator : AbstractValidator<DeleteMessageCommand>
{
    public DeleteMessageCommandValidator()
    {
        RuleFor(c => c.DiscussionId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid("DiscussionId"));

        RuleFor(c => c.UserId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid("UserId"));

        RuleFor(c => c.MessageId)
            .NotNull()
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid("MessageId"));
    }
}