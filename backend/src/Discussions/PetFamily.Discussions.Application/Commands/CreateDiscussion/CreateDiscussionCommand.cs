using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Commands.CreateDiscussion;

public record CreateDiscussionCommand(Guid RelationId, IEnumerable<Guid> Users) : ICommand;