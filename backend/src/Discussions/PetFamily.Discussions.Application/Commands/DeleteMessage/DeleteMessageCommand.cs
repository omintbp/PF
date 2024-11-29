using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Commands.DeleteMessage;

public record DeleteMessageCommand(
    Guid DiscussionId,
    Guid MessageId,
    Guid UserId) : ICommand;