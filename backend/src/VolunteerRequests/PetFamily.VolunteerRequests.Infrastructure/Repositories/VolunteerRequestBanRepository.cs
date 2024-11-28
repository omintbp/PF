using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.VolunteerRequests.Application;
using PetFamily.VolunteerRequests.Domain.AggregateRoot;
using PetFamily.VolunteerRequests.Infrastructure.DbContexts;

namespace PetFamily.VolunteerRequests.Infrastructure.Repositories;

public class VolunteerRequestBanRepository(WriteDbContext context)
    : IVolunteerRequestBanRepository
{
    public async Task<VolunteerRequestBanId> Add(
        VolunteerRequestBan ban,
        CancellationToken cancellationToken = default)
    {
        await context.AddAsync(ban, cancellationToken);

        return ban.Id;
    }

    public async Task<Result<List<VolunteerRequestBan>, Error>> GetByUserId(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var bans = await context.VolunteerRequestBans
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

        return bans;
    }
}