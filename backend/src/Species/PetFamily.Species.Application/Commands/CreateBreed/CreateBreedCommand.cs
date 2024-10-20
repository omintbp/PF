using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.CreateBreed;

public record CreateBreedCommand(Guid SpeciesId, string Name) : ICommand;