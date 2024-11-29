using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Discussions.Domain.ValueObjects;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Commands.UpdateMessage;

public class UpdateMessageCommandValidator : AbstractValidator<UpdateMessageCommand>
{
    public UpdateMessageCommandValidator()
    {
        RuleFor(c => c.Message)
            .MustBeValueObject(Text.Create);

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