using PetFamily.Domain.Entities.Volunteers;

namespace PetFamily.Application.Volunteers;

public interface IVolunteerRepository
{
    Task<VolunteerId> Add(Volunteer request, CancellationToken cancellationToken = default!);
}