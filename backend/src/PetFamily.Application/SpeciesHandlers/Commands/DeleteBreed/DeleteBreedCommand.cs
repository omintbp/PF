using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesHandlers.Commands.DeleteBreed;

public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;