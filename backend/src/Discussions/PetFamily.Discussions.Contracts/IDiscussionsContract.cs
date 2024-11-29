using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Contracts;

public interface IDiscussionsContract
{
    Task<Result<Guid, ErrorList>> CreateDiscussionHandler(
        IEnumerable<Guid> users,
        Guid relationId,
        CancellationToken cancellationToken);
}