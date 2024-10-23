using PetFamily.Core.DTOs.Species;

namespace PetFamily.Species.Application;

public interface IReadDbContext
{
    public IQueryable<BreedDto> Breeds { get; }

    public IQueryable<SpeciesDto> Species { get; }
}