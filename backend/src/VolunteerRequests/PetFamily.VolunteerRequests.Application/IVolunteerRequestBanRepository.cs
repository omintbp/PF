using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.VolunteerRequests.Domain.AggregateRoot;

namespace PetFamily.VolunteerRequests.Application;

public interface IVolunteerRequestBanRepository
{
    Task<VolunteerRequestBanId> Add(
        VolunteerRequestBan ban,
        CancellationToken cancellationToken = default!);

    Task<Result<List<VolunteerRequestBan>, Error>> GetByUserId(
        Guid userId,
        CancellationToken cancellationToken = default!);
}