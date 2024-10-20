using PetFamily.Species.Application.Commands.Create;

namespace PetFamily.Species.Presentation.Requests;

public record CreateSpeciesRequest(string Name)
{
    public CreateSpeciesCommand ToCommand() =>
        new(Name);
}