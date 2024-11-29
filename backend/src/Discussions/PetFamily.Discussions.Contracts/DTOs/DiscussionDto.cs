namespace PetFamily.Discussions.Contracts.DTOs;

public record DiscussionDto
{
    public Guid DiscussionId { get; init; }

    public Guid RelationId { get; init; }

    public List<Guid> DiscussionUsers { get; init; } = [];

    public long TotalMessagesCount { get; set; }

    public List<MessageDto> Messages { get; init; } = [];
}