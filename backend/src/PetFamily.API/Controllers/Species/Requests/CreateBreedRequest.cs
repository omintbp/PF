using PetFamily.Application.SpeciesHandlers.Commands.CreateBreed;

namespace PetFamily.API.Controllers.Species.Requests;

public record CreateBreedRequest(string Name)
{
    public CreateBreedCommand ToCommand(Guid speciesId) =>
        new(speciesId, Name);
}