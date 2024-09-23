using PetFamily.Application.SpeciesHandlers.Commands.Create;

namespace PetFamily.API.Controllers.Species.Requests;

public record CreateSpeciesRequest(string Name)
{
    public CreateSpeciesCommand ToCommand() =>
        new(Name);
}