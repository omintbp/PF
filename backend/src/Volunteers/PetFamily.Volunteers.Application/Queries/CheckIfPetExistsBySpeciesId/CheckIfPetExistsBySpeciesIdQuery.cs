using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Queries.CheckIfPetExistsBySpeciesId;

public record CheckIfPetExistsBySpeciesIdQuery(Guid SpeciesId) : IQuery;