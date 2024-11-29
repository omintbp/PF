using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Discussions.Domain.ValueObjects;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Application.Commands.AddMessage;

public class AddMessageCommandValidator : AbstractValidator<AddMessageCommand>
{
    public AddMessageCommandValidator()
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
    }
}