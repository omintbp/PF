using FluentValidation;

namespace PetFamily.Application.SpeciesHandlers.Commands.Delete;

public class DeleteSpeciesCommandValidator : AbstractValidator<DeleteSpeciesCommand>
{
    public DeleteSpeciesCommandValidator()
    {
        RuleFor(x => x.SpeciesId).NotEmpty();
    }
}