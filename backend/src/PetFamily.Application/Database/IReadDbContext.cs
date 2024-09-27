using PetFamily.Application.DTOs.Species;
using PetFamily.Application.DTOs.Volunteers;

namespace PetFamily.Application.Database;

public interface IReadDbContext
{
    public IQueryable<VolunteerDto> Volunteers { get; }

    public IQueryable<SpeciesDto> Species { get; }

    public IQueryable<PetDto> Pets { get; }

    public IQueryable<BreedDto> Breeds { get; }
}