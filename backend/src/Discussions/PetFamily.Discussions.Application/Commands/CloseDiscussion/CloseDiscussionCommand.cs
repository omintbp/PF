using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Commands.CloseDiscussion;

public record CloseDiscussionCommand(Guid UserId, Guid DiscussionId) : ICommand;