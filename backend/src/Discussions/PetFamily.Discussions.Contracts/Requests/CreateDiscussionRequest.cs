namespace PetFamily.Discussions.Contracts.Requests;

public record CreateDiscussionRequest(Guid RelationId, IEnumerable<Guid> Users);