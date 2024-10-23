using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Queries.CheckIfSpeciesExists;

public record CheckIfSpeciesExistsQuery(Guid SpeciesId) : IQuery;