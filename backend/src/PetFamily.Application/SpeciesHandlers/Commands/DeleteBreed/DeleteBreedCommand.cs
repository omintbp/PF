using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesHandlers.Commands.Delete;

public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;