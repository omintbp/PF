using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Commands.UpdateMessage;

public record UpdateMessageCommand(
    Guid DiscussionId,
    Guid MessageId,
    Guid UserId,
    string Message) : ICommand;