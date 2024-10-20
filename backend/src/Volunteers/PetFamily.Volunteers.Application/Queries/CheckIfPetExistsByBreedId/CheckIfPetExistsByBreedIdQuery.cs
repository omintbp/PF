using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Queries.CheckIfPetExistsByBreedId;

public record CheckIfPetExistsByBreedIdQuery(Guid BreedId) : IQuery;