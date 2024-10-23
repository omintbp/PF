using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Species.Domain.ValueObjects;

namespace PetFamily.Species.Application.Commands.Create;

public class CreateSpeciesCommandValidator : AbstractValidator<CreateSpeciesCommand>
{
    public CreateSpeciesCommandValidator()
    {
        RuleFor(x => x.Name).MustBeValueObject(SpeciesName.Create);
    }
}