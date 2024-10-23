using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.Delete;

public record DeleteSpeciesCommand(Guid SpeciesId) : ICommand;