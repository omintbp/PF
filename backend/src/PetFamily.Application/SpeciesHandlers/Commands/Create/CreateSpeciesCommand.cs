using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesHandlers.Commands.Create;

public record CreateSpeciesCommand(string Name) : ICommand;