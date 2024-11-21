using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Discussions.Application;
using PetFamily.Discussions.Domain.AggregateRoot;
using PetFamily.Discussions.Infrastructure.DbContexts;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;

namespace PetFamily.Discussions.Infrastructure.Repositories;

public class DiscussionsRepository(WriteDbContext context)
    : IDiscussionsRepository
{
    public async Task<DiscussionId> Add(
        Discussion discussion,
        CancellationToken cancellationToken = default)
    {
        context.Add(discussion);

        await context.SaveChangesAsync(cancellationToken);

        return discussion.Id;
    }

    public async Task<Result<Discussion, Error>> GetById(
        DiscussionId discussionId,
        CancellationToken cancellationToken = default)
    {
        var discussion = await context.Discussions
            .FirstOrDefaultAsync(v => v.Id == discussionId, cancellationToken);

        if (discussion is null)
            return Errors.General.NotFound(discussionId.Value);

        return discussion;
    }

    public async Task<Guid> Delete(
        Discussion discussion,
        CancellationToken cancellationToken = default)
    {
        context.Discussions.Remove(discussion);

        await context.SaveChangesAsync(cancellationToken);

        return discussion.Id.Value;
    }
}