using CSharpFunctionalExtensions;
using PetFamily.Discussions.Domain.AggregateRoot;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;

namespace PetFamily.Discussions.Application;

public interface IDiscussionsRepository
{
    Task<DiscussionId> Add(
        Discussion discussion,
        CancellationToken cancellationToken = default!);

    Task<Result<Discussion, Error>> GetById(
        DiscussionId discussionId,
        CancellationToken cancellationToken = default!);

    Task<Guid> Delete(
        Discussion discussion,
        CancellationToken cancellationToken = default!);
}