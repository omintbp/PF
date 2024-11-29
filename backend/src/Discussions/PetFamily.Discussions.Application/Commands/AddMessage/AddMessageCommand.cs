using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Commands.AddMessage;

public record AddMessageCommand(
    Guid DiscussionId,
    Guid UserId,
    string Message) : ICommand;