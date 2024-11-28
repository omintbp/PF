using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.VolunteerRequests.Application;
using PetFamily.VolunteerRequests.Domain.AggregateRoot;
using PetFamily.VolunteerRequests.Infrastructure.DbContexts;

namespace PetFamily.VolunteerRequests.Infrastructure.Repositories;

public class VolunteerRequestsRepository(WriteDbContext context)
    : IVolunteerRequestsRepository
{
    public async Task<VolunteerRequestId> Add(
        VolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        await context.AddAsync(request, cancellationToken);

        return request.Id;
    }

    public async Task<Result<VolunteerRequest, Error>> GetById(
        VolunteerRequestId volunteerRequestId,
        CancellationToken cancellationToken = default)
    {
        var request = await context.VolunteerRequests
            .FirstOrDefaultAsync(v => v.Id == volunteerRequestId, cancellationToken);

        if (request is null)
            return Errors.General.NotFound(volunteerRequestId.Value);

        return request;
    }

    public async Task<Guid> Delete(
        VolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        context.VolunteerRequests.Remove(request);

        await context.SaveChangesAsync(cancellationToken);

        return request.Id.Value;
    }
}