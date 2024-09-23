using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.SpeciesManagement.ValueObjects;

namespace PetFamily.Application.SpeciesHandlers.Commands.CreateBreed;

public class CreateBreedCommandValidator : AbstractValidator<CreateBreedCommand>
{
    public CreateBreedCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.Name).MustBeValueObject(BreedName.Create);
    }
}