using PetFamily.Core.DTOs.Volunteers;

namespace PetFamily.Volunteers.Application;

public interface IReadDbContext
{
    IQueryable<PetDto> Pets { get; }

    IQueryable<VolunteerDto> Volunteers { get; }
}