using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Application.Commands.CreateDiscussion;
using PetFamily.Discussions.Contracts;
using PetFamily.SharedKernel;

namespace PetFamily.Discussions.Presentation;

public class DiscussionsContract : IDiscussionsContract
{
    private readonly ICommandHandler<Guid, CreateDiscussionCommand> _createDiscussionCommandHandler;

    public DiscussionsContract(ICommandHandler<Guid, CreateDiscussionCommand> createDiscussionCommandHandler)
    {
        _createDiscussionCommandHandler = createDiscussionCommandHandler;
    }

    public async Task<Result<Guid, ErrorList>> CreateDiscussionHandler(
        IEnumerable<Guid> users,
        Guid relationId,
        CancellationToken cancellationToken)
    {
        var command = new CreateDiscussionCommand(relationId, users);
        
        var result = await _createDiscussionCommandHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error;
        
        return result.Value;
    }
}