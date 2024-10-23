using PetFamily.Species.Application.Commands.CreateBreed;

namespace PetFamily.Species.Presentation.Requests;

public record CreateBreedRequest(string Name)
{
    public CreateBreedCommand ToCommand(Guid speciesId) =>
        new(speciesId, Name);
}