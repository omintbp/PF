using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;

namespace PetFamily.Species.Application;

public interface ISpeciesRepository
{
    Task<SpeciesId> Add(Domain.AggregateRoot.Species species, CancellationToken cancellationToken = default!);

    Task<Result<Domain.AggregateRoot.Species, Error>> GetById(SpeciesId speciesId, CancellationToken cancellationToken = default!);

    Guid Delete(Domain.AggregateRoot.Species species);
}