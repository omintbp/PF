using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesHandlers.Commands.Delete;

public record DeleteSpeciesCommand(Guid SpeciesId) : ICommand;