using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesHandlers.Commands.CreateBreed;

public record CreateBreedCommand(Guid SpeciesId, string Name) : ICommand;