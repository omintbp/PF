using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Contracts;

public interface IDiscussionsContract
{
    Task<Result<Guid, Error>> CreateDiscussionHandler(
        Guid firstUser,
        Guid secondUser,
        Guid relationId,
        CancellationToken cancellationToken);
}