using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs.Species;
using PetFamily.SharedKernel;
using PetFamily.Species.Application.Queries.CheckIfBreedExists;
using PetFamily.Species.Application.Queries.CheckIfSpeciesExists;
using PetFamily.Species.Contracts;

namespace PetFamily.Species.Presentation;

public class SpeciesContract : ISpeciesContract
{
    private readonly IQueryHandler<bool, CheckIfSpeciesExistsQuery> _checkIfSpeciesExistsHandler;
    private readonly IQueryHandler<bool, CheckIfBreedExistsQuery> _checkIfBreedExistsHandler;

    public SpeciesContract(
        IQueryHandler<bool, CheckIfSpeciesExistsQuery> checkIfSpeciesExistsHandler,
        IQueryHandler<bool, CheckIfBreedExistsQuery> checkIfBreedExistsHandler)
    {
        _checkIfSpeciesExistsHandler = checkIfSpeciesExistsHandler;
        _checkIfBreedExistsHandler = checkIfBreedExistsHandler;
    }

    public async Task<Result<bool, ErrorList>> CheckIfBreedExists(
        Guid speciesId,
        Guid breedId,
        CancellationToken cancellationToken = default)
    {
        var query = new CheckIfBreedExistsQuery(speciesId, breedId);
        
        var result = await _checkIfBreedExistsHandler.Handle(query, cancellationToken);

        if (result.IsFailure)
            return result.Error;

        return result.Value;
    }

    public async Task<Result<bool, ErrorList>> CheckIfSpeciesExists(
        Guid speciesId,
        CancellationToken cancellationToken = default)
    {
        var query = new CheckIfSpeciesExistsQuery(speciesId);
        
        var result = await _checkIfSpeciesExistsHandler.Handle(query, cancellationToken);

        if (result.IsFailure)
            return result.Error;

        return result.Value;
    }
}