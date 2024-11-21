using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.VolunteerRequests.Domain.AggregateRoot;

namespace PetFamily.VolunteerRequests.Application;

public interface IVolunteerRequestsRepository
{
    Task<VolunteerRequestId> Add(
        VolunteerRequest request,
        CancellationToken cancellationToken = default!);

    Task<Result<VolunteerRequest, Error>> GetById(
        VolunteerRequestId volunteerRequestId,
        CancellationToken cancellationToken = default!);

    Task<Guid> Delete(
        VolunteerRequest request,
        CancellationToken cancellationToken = default!);
}