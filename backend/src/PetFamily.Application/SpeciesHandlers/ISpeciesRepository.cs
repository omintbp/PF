using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.AggregateRoot;

namespace PetFamily.Application.SpeciesHandlers;

public interface ISpeciesRepository
{
    Task<SpeciesId> Add(Species species, CancellationToken cancellationToken = default!);

    Task<Result<Species, Error>> GetById(SpeciesId speciesId, CancellationToken cancellationToken = default!);

    Guid Delete(Species species);
}