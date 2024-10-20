using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Species.Domain.ValueObjects;

namespace PetFamily.Species.Application.Commands.CreateBreed;

public class CreateBreedCommandValidator : AbstractValidator<CreateBreedCommand>
{
    public CreateBreedCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.Name).MustBeValueObject(BreedName.Create);
    }
}