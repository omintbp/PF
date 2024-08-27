using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Application.Volunteers;

public interface IVolunteerRepository
{
    Task<VolunteerId> Add(Volunteer request, CancellationToken cancellationToken = default!);
}