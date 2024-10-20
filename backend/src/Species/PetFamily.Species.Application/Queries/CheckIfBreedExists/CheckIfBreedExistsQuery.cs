using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Queries.CheckIfBreedExists;

public record CheckIfBreedExistsQuery(Guid SpeciesId, Guid BreedId) : IQuery;