using FluentValidation;

namespace PetFamily.Application.SpeciesHandlers.Commands.Delete;

public class DeleteBreedCommandValidator : AbstractValidator<DeleteBreedCommand>
{
    public DeleteBreedCommandValidator()
    {
        RuleFor(x => x.SpeciesId).NotEmpty();
        RuleFor(x => x.BreedId).NotEmpty();
    }
}