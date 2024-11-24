using CSharpFunctionalExtensions;
using PetFamily.Discussions.Contracts;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Presentation;

public class DiscussionsContract : IDiscussionsContract
{
    public async Task<Result<Guid, Error>> CreateDiscussionHandler(Guid firstUser, Guid secondUser, Guid relationId, CancellationToken cancellationToken)
    {
        return Guid.NewGuid();
    }
}