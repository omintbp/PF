using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Contracts;

public interface IVolunteerContract
{
    Task<Result<bool, ErrorList>> CheckIfPetExistsByBreedId(Guid breedId, CancellationToken cancellationToken);
    
    Task<Result<bool, ErrorList>> CheckIfPetExistsBySpeciesId(Guid speciesId, CancellationToken cancellationToken);
}