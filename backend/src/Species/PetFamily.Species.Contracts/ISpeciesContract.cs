using CSharpFunctionalExtensions;
using PetFamily.Core.DTOs.Species;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Contracts;

public interface ISpeciesContract
{
    Task<Result<bool, ErrorList>> CheckIfBreedExists(
        Guid speciesId,
        Guid breedId,
        CancellationToken cancellationToken = default);

    Task<Result<bool, ErrorList>> CheckIfSpeciesExists(
        Guid speciesId,
        CancellationToken cancellationToken = default);
}