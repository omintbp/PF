using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.Queries.CheckIfPetExistsByBreedId;
using PetFamily.Volunteers.Application.Queries.CheckIfPetExistsBySpeciesId;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.Volunteers.Presentation;

public class VolunteerContract : IVolunteerContract
{
    private readonly IQueryHandler<bool, CheckIfPetExistsBySpeciesIdQuery> _checkBySpeciesIdHandler;
    private readonly IQueryHandler<bool, CheckIfPetExistsByBreedIdQuery> _checkByBreedIdHandler;

    public VolunteerContract(
        IQueryHandler<bool, CheckIfPetExistsBySpeciesIdQuery> checkBySpeciesIdHandler,
        IQueryHandler<bool, CheckIfPetExistsByBreedIdQuery> checkByBreedIdHandler)
    {
        _checkBySpeciesIdHandler = checkBySpeciesIdHandler;
        _checkByBreedIdHandler = checkByBreedIdHandler;
    }

    public async Task<Result<bool, ErrorList>> CheckIfPetExistsByBreedId(
        Guid breedId,
        CancellationToken cancellationToken)
    {
        var query = new CheckIfPetExistsByBreedIdQuery(breedId);

        var result = await _checkByBreedIdHandler.Handle(query, cancellationToken);

        if (result.IsFailure)
            return result.Error;

        return result.Value;
    }

    public async Task<Result<bool, ErrorList>> CheckIfPetExistsBySpeciesId(
        Guid speciesId,
        CancellationToken cancellationToken)
    {
        var query = new CheckIfPetExistsBySpeciesIdQuery(speciesId);

        var result = await _checkBySpeciesIdHandler.Handle(query, cancellationToken);

        if (result.IsFailure)
            return result.Error;

        return result.Value;
    }
}